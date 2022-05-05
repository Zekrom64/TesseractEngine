using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Util;

namespace Tesseract.Core.Net {
	
	public class Client : INetConnection {

		public INetInterface Interface { get; }

		public uint ConnectionID { get; protected set; } = 0;

		public object ConnectionInfo => socket.ConnectionInfo;

		public IPAddress? RemoteAddress { get; }

		private volatile bool isAlive = true;
		public bool IsAlive { get => isAlive; protected set => isAlive = value; }

		public NetCloseInfo? CloseInfo { get; protected set; } = null;

		private ulong txBytes = 0;
		public ulong TxBytes => Interlocked.Read(ref txBytes);

		private ulong rxBytes = 0;
		public ulong RxBytes => Interlocked.Read(ref rxBytes);

		private readonly INetSocket socket;

		private readonly Thread connectionThread;

		/// <summary>
		/// Creates a new client with the given interface, and an optional existing socket.
		/// </summary>
		/// <param name="iface">The network interface</param>
		/// <param name="socket">The already opened socket, or null</param>
		protected Client(INetInterface iface, INetSocket? customSocket) {
			Interface = iface;

			if (customSocket == null) {
				// Initialize the socket based on the network interface
				Socket sock = new(
					iface.UseIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork,
					SocketType.Stream,
					ProtocolType.Tcp
				);
				sock.Blocking = false;

				// Get the host name from the interface, for clients this must not be null
				string? hostname = iface.HostName;
				if (hostname == null) throw new ArgumentNullException(nameof(iface), "Host name must be non-null");
				// Get the list of IP addresses associated with this host name (or the address itself if the host name is an address
				IPAddress[] addresses = Dns.GetHostAddresses(hostname);
				if (addresses.Length == 0) throw new ArgumentException($"Host name \"{hostname}\" could not be resolved to an IP address");

				// Use the first address in the list as the remote address and connect the socket to it
				RemoteAddress = addresses[0];
				sock.Connect(new IPEndPoint(RemoteAddress, iface.Port));

				// Start the connection's networking thread
				socket = new NetSocket(sock);
			} else {
				socket = customSocket;
				if (socket.ConnectionInfo is IPEndPoint ep) RemoteAddress = ep.Address;
			}
			
			connectionThread = new Thread(RunNetworking);
			connectionThread.Start();
		}

		public virtual void Dispose() {
			GC.SuppressFinalize(this);
			// Make sure the connection is closed before disposing
			if (IsAlive) {
				Close(NetCloseCause.Disconnect);
				while (IsAlive) Thread.Sleep(10);
			}
			// Dispose of the network socket
			socket.Dispose();
		}

		// Networking should be updated every 10 milliseconds, even if nothing is received
		private static readonly TimeSpan NetworkInterval = TimeSpan.FromMilliseconds(10);
		// A "keep-alive" packet should be sent every 100 milliseconds
		private static readonly TimeSpan KeepaliveInterval = TimeSpan.FromMilliseconds(100);

		/// <summary>
		/// Enumeration of results of trying to perform a completion.
		/// </summary>
		protected enum CompletionResult {
			/// <summary>
			/// The completion is still waiting for a packet.
			/// </summary>
			Await,
			/// <summary>
			/// The completion was done successfully.
			/// </summary>
			Completed,
			/// <summary>
			/// The completion failed and must be discarded.
			/// </summary>
			Discarded
		}

		/// <summary>
		/// Object managing the state for a completion.
		/// </summary>
		protected class CompletionState {

			// The completion source for the completion packet
			private readonly TaskCompletionSource<Packet> taskCompletion = new();

			/// <summary>
			/// The task that will complete when the correct completion packet is received.
			/// </summary>
			public Task<Packet> CompletionTask { get; }

			/// <summary>
			/// The expected completion ID.
			/// </summary>
			public uint CompletionID { get; }

			public CompletionState(uint completionID, CancellationToken ct) {
				CompletionTask = AwaitPacket(ct);
				CompletionID = completionID;
			}

			/// <summary>
			/// Attempts to perform the completion with the given packet.
			/// </summary>
			/// <param name="completion">Packet to try as a completion</param>
			/// <returns>If the completion state is finished</returns>
			public CompletionResult TryComplete(Packet completion) {
				// If the IDs match, set the result to perform the completion
				if (completion.CompletionNumber == CompletionID) {
					taskCompletion.SetResult(completion);
					return CompletionResult.Completed;
				}
				// If the completion is already done (ie. cancelled), don't do it and discard the state
				if (CompletionTask.IsCompleted) return CompletionResult.Discarded;
				// Else not complete
				return CompletionResult.Await;
			}

			// Method to generate the actual packet task with cancellation
			private async Task<Packet> AwaitPacket(CancellationToken ct) {
				Task<Packet> packetTask = taskCompletion.Task;
				return await packetTask.WaitAsync(ct);
			}

		}

		/// <summary>
		/// Object managing the state for network reciving and transmission.
		/// </summary>
		protected class NetState {

			/// <summary>
			/// The sequence number for the next transmitted packet.
			/// </summary>
			public uint TxSequence = 1;

			/// <summary>
			/// The expected sequence number for the next received packet.
			/// </summary>
			public uint RxSequence = 1;

			/// <summary>
			/// The list of packets to transmit.
			/// </summary>
			public IList<Packet> TxBuffer { get; } = new List<Packet>();

			/// <summary>
			/// The list of scheduled completions.
			/// </summary>
			public IList<CompletionState> Completions { get; } = new List<CompletionState>();

			/// <summary>
			/// Writes all of the packets in the transmit buffer to a binary writer, then clears the buffer.
			/// </summary>
			/// <param name="bw"></param>
			public void WritePackets(BinaryWriter bw) {
				foreach (Packet packet in TxBuffer) packet.Write(bw);
				TxBuffer.Clear();
			}

			/// <summary>
			/// If the connection should close once the transmit buffer is empty. Also indicates
			/// that no new packets should be added.
			/// </summary>
			public bool ShouldClose = false;

			/// <summary>
			/// The closing info to set when closed.
			/// </summary>
			public NetCloseInfo? ClosingInfo = null;

		}

		/// <summary>
		/// The current network state. This must be locked to access since it is shared between threads.
		/// </summary>
		protected NetState State { get; } = new();

		/// <summary>
		/// Invoked by the network thread when a "bad" packet is received, passing the information
		/// known about the packet.
		/// </summary>
		/// <param name="header">The bad packet's header</param>
		/// <param name="payload">The bad packet's payload</param>
		protected void HandleBadPacket(PacketHeader header, byte[] payload) {
			Interface.OnBadPacket(new PacketData() {
				ID = header.ID,
				SequenceNumber = header.SequenceNumber,
				CompletionNumber = header.CompletionNumber,
				Data = payload
			}, this);
		}

		/// <summary>
		/// <para>
		/// Invoked by the network thread to perform the task of receiving a packet, passing the
		/// information known about the packet.
		/// </para>
		/// <para>
		/// This method performs the task of validating the packet at the network layer, and
		/// constructing and reading an instance of a <see cref="Packet"/> object from the packet
		/// data. If there is an error decoding the packet, <see cref="HandleBadPacket(PacketHeader, byte[])"/>
		/// is invoked and null is returned.
		/// </para>
		/// </summary>
		/// <param name="ns">The current network state</param>
		/// <param name="header">The packet's header</param>
		/// <param name="payload">The packet's payload</param>
		/// <returns>The decoded packet, or null</returns>
		protected virtual Packet? ReceivePacket(NetState ns, PacketHeader header, byte[] payload) {
			// Copy and increment sequence number
			uint expectedID = ns.RxSequence++;
			// Find packet constructor
			var ctor = Interface.PacketManager.FindConstructor(header.ID);
			// If not found, bad packet
			if (ctor == null) {
				HandleBadPacket(header, payload);
				return null;
			}
			// Build the packet and decode it from the payload
			Packet pkt = ctor();
			using MemoryStream ms = new(payload);
			try {
				pkt.Read(new BinaryReader(ms));
			} catch (Exception) {
				HandleBadPacket(header, payload);
				return null;
			}
			// Check that sequence numbers match
			if (pkt.SequenceNumber != expectedID) {
				HandleBadPacket(header, payload);
				return null;
			}
			// Finally, return good packet
			return pkt;
		}

		/// <summary>
		/// <para>
		/// Internal method called when packets are first received. Return value
		/// indicates if the packet should be passed to the network interface.
		/// </para>
		/// <para>
		/// This method also performs completion handling.
		/// </para>
		/// </summary>
		/// <param name="state">The current network state</param>
		/// <param name="pkt">Received packet</param>
		/// <returns>If the packet should continue to the network interface</returns>
		protected virtual bool CheckReceivedPacket(NetState state, Packet pkt) {
			// Check packet against completions
			if (pkt.CompletionNumber != 0) {
				var completions = state.Completions;
				bool completed = false;
				for (int i = 0; i < completions.Count; i++) {
					var completion = completions[i];
					switch (completion.TryComplete(pkt)) {
						case CompletionResult.Completed:
							completed = true;
							completions.RemoveAt(i);
							i--;
							break;
						case CompletionResult.Discarded:
							completions.RemoveAt(i);
							i--;
							break;
						case CompletionResult.Await:
							break;
					}
				}
				if (!completed) Interface.OnOrphanedCompletion(pkt, this);
			}

			if (pkt.ID.ModuleID == 0) {
				// If received a heartbeat packet and waiting for a response, respond
				if (pkt is InternalPacket04BHeartbeat pkHeartbeat && pkHeartbeat.Respond) {
					Send(new InternalPacket04BHeartbeat() { Respond = false }, pkt);
				}
				return false;
			} else return true;
		}

		private void RunNetworking() {
			// The receive and transmit FIFOs
			FIFOStream rxstream = new(), txstream = new();
			// The receive and transmit buffers
			byte[] rxbuffer = new byte[4096], txbuffer = new byte[4096];
			// Transmit buffer state variables
			int txoffset = 0, txlength = 0;
			// Binary readers and writers for the receive and transmit FIFOs
			BinaryReader rxrd = new(rxstream);
			BinaryWriter txwr = new(txstream);

			// Timestamps for the last received packet and last keepalive send
			DateTime lastRxPacket = DateTime.Now, lastKeepAlive = lastRxPacket;

			// Packet decoding state
			PacketHeader header = new();
			bool hasHeader = false;

			// Connection close state
			bool closeFlag = false;
			NetCloseInfo? closeInfo = null;
			Exception? closeException = null;

			do {
				// Entire operation is done in a try-block to catch any exceptions while processing
				try {
					// If the socket is not connected it cannot be alive
					if (!socket.Connected) {
						IsAlive = false;
						break;
					}

					DateTime now = DateTime.Now;

					// Read bytes from socket and transfer to FIFO
					int numrx;
					do {
						numrx = socket.Receive(rxbuffer);
						rxstream.Write(rxbuffer, 0, numrx);
						Interlocked.Add(ref rxBytes, (ulong)numrx);
					} while (numrx > 0);

					lock (State) {
						// Decode packets until we run out of data
						while (true) {
							// Read header
							if (!hasHeader) {
								if (rxstream.Length < PacketHeader.SizeOf) break;
								header.Read(rxrd);
								hasHeader = true;
							}
							// Wait until we have the complete payload
							if (header.Length > rxstream.Length) break;
							// Read payload
							byte[] payload = new byte[header.Length];
							rxstream.Read(payload);
							// Decode packet
							Packet? pkt = ReceivePacket(State, header, payload);
							// Fire packet received event
							if (pkt != null) {
								if (CheckReceivedPacket(State, pkt))
									Interface.OnPacketReceived(pkt, this);
								lastRxPacket = now;
							}
							// Reset state
							hasHeader = false;
						}

						// Encode packets to transmit
						State.WritePackets(txwr);
						// Update close flag and info
						closeFlag = State.ShouldClose;
						closeInfo = State.ClosingInfo;
					}

					// Check that we have not timed out
					if ((now - lastRxPacket) > Interface.Timeout) IsAlive = false;
					// Enqueue keepalive packet if needed
					if ((now - lastKeepAlive) > KeepaliveInterval) {
						Send(new InternalPacket00BKeepalive());
						lastKeepAlive = now;
					}

					// If we are closing, do special synchronous transmit operations (all remaining packets MUST be sent)
					if (closeFlag) {
						void SendSync(byte[] buffer, int offset, int length) {
							do {
								int n = socket.Send(buffer.AsSpan().Slice(offset, length));
								offset += n;
								length -= n;
								Interlocked.Add(ref txBytes, (ulong)n);
							} while (length > 0);
						}
						// Write any remaining bytes
						if (txlength > 0) SendSync(txbuffer, txoffset, txlength);
						// Write all buffered packets
						while(txstream.Length > 0) {
							txlength = txstream.Read(txbuffer);
							SendSync(txbuffer, 0, txlength);
						}
						// Close the socket voluntarily
						socket.Disconnect();
						// The connection is no longer alive since we are closing it
						CloseInfo = closeInfo;
						IsAlive = false;
					} else {
						// Else do normal non-blocking transmit
						int n;
						do {
							// If the buffer is empty, refill it
							if (txlength <= 0) {
								txlength = txstream.Read(txbuffer);
								txoffset = 0;
							}
							do {
								// Attempt to send data
								n = socket.Send(txbuffer.AsSpan().Slice(txoffset, txlength));
								txoffset += n;
								txlength -= n;
								Interlocked.Add(ref txBytes, (ulong)n);
								// Repeat while data is actually being sent and we still have data in the buffer
							} while (n > 0 && txlength > 0);
							// Keep transmitting until either the socket stops transmitting or we run out of bytes to transmit
						} while (n > 0 && txstream.Length > 0);
					}

					// Sleep for the networking interval
					Thread.Sleep(NetworkInterval);
				} catch (Exception e) {
					// If an exception occurs, forcibly disconnect us since we are in an unknown state
					closeException = e;
					CloseInfo = new NetCloseInfo() {
						Cause = NetCloseCause.NetworkError,
						Message = "Unhandled exception: " + e.Message,
						Remote = false
					};
					IsAlive = false;
				}
			} while (IsAlive);
			// Really make sure the socket is disconnected
			if (socket.Connected) socket.Disconnect();
			// Fire events
			Interface.OnDisconnect(this, closeException);
			OnClosed();
		}

		/// <summary>
		/// Internal method invoked when the connection has been fully closed. Note that is
		/// is fired after <see cref="INetInterface.OnDisconnect(INetConnection, Exception?)"/>
		/// is called.
		/// </summary>
		protected virtual void OnClosed() { }

		/// <summary>
		/// Completes the connection from this endpoint.
		/// </summary>
		/// <param name="ct">Cancellation token for cancelling the connection</param>
		/// <returns>Task completed when the connection is established</returns>
		/// <exception cref="InvalidDataException">If invalid data is received during connection</exception>
		protected virtual async Task CompleteConnection(CancellationToken ct) {
			var pkConfirm = await SendAndAwait(new InternalPacket01CClientInfo() {
				SubsystemID = Interface.SubsystemID,
				SubsystemNetVersion = INetInterface.SubsystemNetVersion
			}, ct);
			if (pkConfirm is not InternalPacket02SConfirmInfo) throw new InvalidDataException("Received response packet is not confirmation");
		}

		public void Send(Packet packet, Packet? responseTo = null) {
			// If responding to a packet, 
			if (responseTo != null) packet.CompletionNumber = responseTo.SequenceNumber;
			lock(State) {
				// Make sure we're not closing
				if (State.ShouldClose) return;
				// Assign the packet a sequence number and enqueue it
				packet.SequenceNumber = State.TxSequence++;
				State.TxBuffer.Add(packet);
			}
		}

		public Task<Packet> SendAndAwait(Packet packet, CancellationToken ct, Packet? responseTo = null) {
			if (responseTo != null) packet.CompletionNumber = responseTo.SequenceNumber;
			lock(State) {
				// Make sure we're not closing
				if (State.ShouldClose) return Task.FromException<Packet>(new IOException("The connection is closing"));
				// Assign the packet a sequence number and enqueue it
				packet.SequenceNumber = State.TxSequence++;
				State.TxBuffer.Add(packet);
				// Add a new completion for the packet's sequence number and return the completion task
				CompletionState cs = new(packet.SequenceNumber, ct);
				State.Completions.Add(cs);
				return cs.CompletionTask;
			}
		}

		public void Close(NetCloseCause cause, string message = "") {
			lock(State) {
				if (State.ShouldClose) return;
				// Enqueue new disconnect packet
				var pkt = new InternalPacket03BDisconnect() {
					Cause = cause,
					Message = message
				};
				pkt.SequenceNumber = State.TxSequence++;
				State.TxBuffer.Add(pkt);
				// Set state close information
				NetCloseInfo info = new() { Cause = cause, Message = message, Remote = false };
				State.ClosingInfo = info;
				// Signal the state to close
				State.ShouldClose = true;
			}
		}

		// Async method that will return the time after a round-trip heartbeat packet
		private async Task<DateTime> MeasureDelayInternal(CancellationToken ct) {
			// Send a heartbeat packet and wait for its response
			await SendAndAwait(new InternalPacket04BHeartbeat(), ct);
			// Return the current time
			return DateTime.Now;
		}

		// Intermediate async method for delay measurement that keeps track of the cancellation token resource
		private async Task<TimeSpan> MeasureDelayAsync(TimeSpan timeout) {
			// Get the current time
			DateTime start = DateTime.Now;
			// Create a cancellation source and use it for the timeout
			using CancellationTokenSource cts = new();
			cts.CancelAfter(timeout);
			// Wait for the end time
			DateTime end = await MeasureDelayInternal(cts.Token);
			// Return the difference in time
			return end - start;
		}

		public Task<TimeSpan> MeasureDelay(TimeSpan timeout) {
			return MeasureDelayAsync(timeout);
		}

		/// <summary>
		/// Begins the task of creaating a new client, connecting using the given network interface
		/// with cancellation provided by a <see cref="CancellationToken"/> for a timeout value.
		/// </summary>
		/// <param name="iface">The network interface</param>
		/// <param name="ct">The cancellation token for the task</param>
		/// <param name="socket">A custom socket to use, or null to use a standard IP socket based on the interface</param>
		/// <returns>Task for creating a new client</returns>
		public static Task<Client> Create(INetInterface iface, CancellationToken ct, INetSocket? socket = null) {
			Client client = new(iface, socket);
			return client.CompleteConnection(ct).ContinueWith((Task t) => {
				iface.OnConnect(client);
				return client;
			});
		}
	}
}
