using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Tesseract.Core.Net {

	/// <summary>
	/// A server accepts incoming connections and manages <see cref="RemoteClient"/> instances
	/// for each connection.
	/// </summary>
	public abstract class Server : IDisposable {

		public INetInterface Interface { get; }

		// The server listening socket
		private readonly INetServerSocket serverSocket;

		// Cancellation source to stop listening for clients
		private readonly CancellationTokenSource shutdownToken = new();
		// List of clients the server is tracking
		internal readonly List<RemoteClient> clients = new();

		/// <summary>
		/// The list of clients currently connected to the server. Note that because
		/// connections are handled concurrently, this only returns a copy of the
		/// client list at the current instant, and clients in that list may spontaneously
		/// become disconnected at any time.
		/// </summary>
		public IReadOnlyList<RemoteClient> Clients {
			get {
				lock(clients) {
					return new List<RemoteClient>(clients);
				}
			}
		}

		/// <summary>
		/// Creates a new network server with the given interface, and begins listening for connections.
		/// </summary>
		/// <param name="iface">The server's network interface</param>
		/// <param name="socket">A custom server socket to use, or null to use a standard IP socket</param>
		/// <exception cref="IOException">If the server's host name is non-null and a DNS lookup on it fails</exception>
		public Server(INetInterface iface, INetServerSocket? socket = null) {
			Interface = iface;
			
			if (socket == null) {
				// Create the socket to use as a server
				Socket sock = new(
					iface.UseIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork,
					SocketType.Stream,
					ProtocolType.Tcp
				);
				// Determine the address to bind the server to
				string? hostname = Interface.HostName;
				if (hostname == null) hostname = Dns.GetHostName();
				IPAddress[] addresses = Dns.GetHostAddresses(hostname);
				if (addresses.Length == 0) throw new IOException($"No available IP addresses to bind for hostname \"{hostname}\"");
				// Bind the server to the address and port
				sock.Bind(new IPEndPoint(addresses[0], iface.Port));
				sock.Listen();
				// Start listening
				serverSocket = new NetServerSocket(sock);
			} else {
				serverSocket = socket;
			}

			Listen(shutdownToken.Token);
		}

		// Async method called to accept a socket as a new connection
		private async void Accept(INetSocket socket, uint connection) {
			// Create a cancellation source and cancel after the timeout
			using CancellationTokenSource cts = new();
			cts.CancelAfter(Interface.Timeout);
			// Accept a new remote client
			RemoteClient rc = await RemoteClient.Accept(this, socket, connection, cts.Token);
			// If not cancelled, add it to the list
			if (!cts.Token.IsCancellationRequested) {
				Interface.OnConnect(rc);
				lock (clients) {
					clients.Add(rc);
				}
			}
		}

		// Async method called to listen for sockets to accept
		private async void Listen(CancellationToken ct) {
			uint connection = 0;
			while(!ct.IsCancellationRequested) {
				INetSocket remote = await serverSocket.Listen(ct);
				if (ct.IsCancellationRequested) return;
				Accept(remote, connection++);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			shutdownToken.Cancel();
			lock (clients) {
				Parallel.ForEach(clients, c => {
					c.Close(NetCloseCause.Disconnect, "Server is forcibly closing");
					c.Dispose();
				});
			}
			serverSocket.Dispose();
		}

	}

	/// <summary>
	/// A remote client is the server-side mirror for a client connection.
	/// </summary>
	public class RemoteClient : Client {

		/// <summary>
		/// The server the remote client belongs to.
		/// </summary>
		public Server Server { get; }

		// Task completion for the server connection
		protected readonly TaskCompletionSource tscConnect = new();

		private RemoteClient(Server server, INetSocket sock, uint connection) : base(server.Interface, sock) {
			Server = server;
			ConnectionID = connection;
		}

		// Accept a connection from the server.
		internal static Task<RemoteClient> Accept(Server server, INetSocket sock, uint connection, CancellationToken ct) {
			RemoteClient client = new(server, sock, connection);
			return client.CompleteConnection(ct).ContinueWith((Task t) => client);
		}

		protected override Task CompleteConnection(CancellationToken ct) {
			return tscConnect.Task.WaitAsync(ct);
		}

		protected override bool CheckReceivedPacket(NetState state, Packet pkt) {
			base.CheckReceivedPacket(state, pkt);

			if (pkt.ID.ModuleID == PacketID.ModuleInternal) {
				// If received client info
				if (pkt is InternalPacket01CClientInfo pkCInfo) {
					// Check network subsystem version
					if (pkCInfo.SubsystemNetVersion != INetInterface.SubsystemNetVersion) {
						Close(NetCloseCause.NetworkVerMismatch, "Network version mismatch");
						tscConnect.SetException(new IOException("Network version mismatch"));
					// Check network subsystem ID
					} else if (pkCInfo.SubsystemID != Interface.SubsystemID) {
						Close(NetCloseCause.SubsysIDMismatch, "Subsystem ID mismatch");
						tscConnect.SetException(new IOException("Subsystem ID mismatch"));
					} else {
						// Complete the connection
						Send(new InternalPacket02SConfirmInfo(), pkt);
						tscConnect.SetResult();
					}
				}
				return false;
			} else return true;
		}

		protected override void OnClosed() {
			// Remove the connection from the client list when closed
			var clist = Server.clients;
			lock(clist) {
				clist.Remove(this);
			}
		}

	}

}
