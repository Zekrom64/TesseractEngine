using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract.Core.Net {
	
	/// <summary>
	/// A net socket provides an interface for sending and receiving data over a network. Net sockets
	/// provide a custom way of communicating between client and server outside of the standard
	/// socket system. An example use case would be connections made through a client such as Steam
	/// which uses its own internal networking system for secure communication.
	/// </summary>
	public interface INetSocket : IDisposable {

		/// <summary>
		/// If the socket is currently connected.
		/// </summary>
		public bool Connected { get; }

		/// <summary>
		/// An object describing the connection in detail. See <see cref="INetConnection.ConnectionInfo"/> for how this
		/// object should behave.
		/// </summary>
		public object ConnectionInfo { get; }

		/// <summary>
		/// Disconnects this socket from the network connection.
		/// </summary>
		public void Disconnect();

		/// <summary>
		/// Sends bytes to the remote end of the connection.
		/// </summary>
		/// <param name="data">Buffer containing the data to send</param>
		/// <returns>The number of bytes actually sent</returns>
		public int Send(in ReadOnlySpan<byte> data);

		/// <summary>
		/// Receives bytes from the remote end of the connection.
		/// </summary>
		/// <param name="data">Buffer to store received data into</param>
		/// <returns>The number of bytes actually received</returns>
		public int Receive(Span<byte> data);

	}

	/// <summary>
	/// A net server socket provides an interface for custom listening sockets to
	/// spawn <see cref="INetSocket"/> connections.
	/// </summary>
	public interface INetServerSocket : IDisposable {

		/// <summary>
		/// Listens for a connection on this server socket.
		/// </summary>
		/// <param name="ct">A cancellation token to cancel the listen operation with</param>
		/// <returns>A task completed when a new socket is available</returns>
		public Task<INetSocket> Listen(CancellationToken ct);

	}

	/// <summary>
	/// An <see cref="INetSocket"/> implementation wrapping a standard socket.
	/// </summary>
	internal class NetSocket : INetSocket {

		private readonly Socket socket;

		public NetSocket(Socket socket) {
			this.socket = socket;
			socket.Blocking = false;
			ConnectionInfo = (object?)socket.RemoteEndPoint ?? "";
		}

		public bool Connected => socket.Connected;

		public object ConnectionInfo { get; }

		public void Disconnect() => socket.Disconnect(false);

		public void Dispose() => socket.Dispose();

		public int Receive(Span<byte> data) => socket.Receive(data);

		public int Send(in ReadOnlySpan<byte> data) => socket.Send(data);

	}

	/// <summary>
	/// An <see cref="INetServerSocket"/> implementation wrapping a standard server socket.
	/// </summary>
	internal class NetServerSocket : INetServerSocket {

		private readonly Socket serverSocket;

		public NetServerSocket(Socket socket) {
			serverSocket = socket;
		}

		public void Dispose() => serverSocket.Dispose();

		public async Task<INetSocket> Listen(CancellationToken ct) =>
			new NetSocket(await serverSocket.AcceptAsync(ct));

	}

}
