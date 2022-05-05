using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract.Core.Net {

	/// <summary>
	/// Enumeration of discrete reasons a network connection will close.
	/// </summary>
	public enum NetCloseCause : byte {
		/// <summary>
		/// The closing cause is unknown.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The connection was manually closed by one of the endpoints.
		/// </summary>
		Disconnect = 1,
		/// <summary>
		/// The connection closed because of a network error.
		/// </summary>
		NetworkError,
		/// <summary>
		/// There was a mismatch between the client and server networking system versions.
		/// </summary>
		NetworkVerMismatch,
		/// <summary>
		/// There was a mismatch between the client and server subsystem IDs.
		/// </summary>
		SubsysIDMismatch,

		/// <summary>
		/// The connection was closed with a custom reason.
		/// </summary>
		Custom = 255
	}

	/// <summary>
	/// Structure for network close information
	/// </summary>
	public readonly record struct NetCloseInfo {
		
		/// <summary>
		/// The cause of the connection closing.
		/// </summary>
		public NetCloseCause Cause { get; init; }

		/// <summary>
		/// A message describing why the connection closed.
		/// </summary>
		public string Message { get; init; }

		/// <summary>
		/// If the connection was closed by the remote part of the connection.
		/// </summary>
		public bool Remote { get; init; }

	}

	/// <summary>
	/// A network connection abstracts the network details of a client-server connection.
	/// </summary>
	public interface INetConnection : IDisposable {

		/// <summary>
		/// A unique number identifying this connection in the context it is created from. For servers, this
		/// ID is assigned when a connection is accepted. For clients, this is normally fixed to 0.
		/// </summary>
		public uint ConnectionID { get; }

		/// <summary>
		/// An object describing the connection in detail. For simple IP connections this will be the <see cref="EndPoint"/> of the connection,
		/// but connections with custom sockets may return different types of objects. Any connection information objects should
		/// override the <see cref="object.ToString"/> method to provide a user-readable string describing the connection.
		/// </summary>
		public object ConnectionInfo { get; }

		/// <summary>
		/// The remote address used by the connection. If the remote address is unavailable
		/// or obfuscated, this will be null.
		/// </summary>
		public IPAddress? RemoteAddress { get; }

		/// <summary>
		/// If the connection is alive.
		/// </summary>
		public bool IsAlive { get; }

		/// <summary>
		/// Information describing why the connection closed. This will be null until
		/// <see cref="IsAlive"/> is false.
		/// </summary>
		public NetCloseInfo? CloseInfo { get; }

		/// <summary>
		/// The number of bytes the connection has transmitted.
		/// </summary>
		public ulong TxBytes { get; }

		/// <summary>
		/// The number of bytes the connection has received.
		/// </summary>
		public ulong RxBytes { get; }

		/// <summary>
		/// A user-defined value assocateed with this connection. This can be used as a shortcut to
		/// access associated data instead of using another lookup method such as a dictionary.
		/// </summary>
		public object? UserData { get; set; }

		/// <summary>
		/// Sends a packet to the remote host, potentially in response to a received packet.
		/// </summary>
		/// <param name="packet">Packet to send</param>
		/// <param name="responseTo">Received packet to respond to, or null if standalone</param>
		public void Send(Packet packet, Packet? responseTo = null);

		/// <summary>
		/// Sends a packet to the remote host and await a response, returning a task that will be
		/// complete when a response is received. This may be done in response to a received packet.
		/// Note that cancellation is the responsibility of the caller, and tasks waiting on
		/// responding packets should complete in reasonable time in case of network problems.
		/// </summary>
		/// <param name="packet">Packet to send</param>
		/// <param name="ct">Cancellation token to use for the completion task</param>
		/// <param name="responseTo">Received packet to respond to</param>
		/// <returns>Task that will complete when a response is received</returns>
		public Task<Packet> SendAndAwait(Packet packet, CancellationToken ct, Packet? responseTo = null);

		/// <summary>
		/// Closes the network connection, using the given information as the cause.
		/// </summary>
		/// <param name="cause">The reason the connection is closing</param>
		/// <param name="message">A message to identify why the connection is closing</param>
		public void Close(NetCloseCause cause, string message = "");

		/// <summary>
		/// Measures the round-trip delay between sending a packet to the remote connection
		/// and receiving a response.
		/// </summary>
		/// <param name="timeout">The amount of time to wait before timing out</param>
		/// <returns>A task which will complete when the delay has been measured</returns>
		public Task<TimeSpan> MeasureDelay(TimeSpan timeout);

	}
	
	/// <summary>
	/// A network interface defines how the networking system will behave for a specific application.
	/// </summary>
	public interface INetInterface {

		/// <summary>
		/// The version number of the network interface used by a subsytem. This is defined
		/// by the engine library instead of the user, as it is used for internal networking
		/// versioning. This version number is used to determine if there is fundamental
		/// incompatibility between the networking versions the client and server use.
		/// Applications should perform their own version checking on top of the network interface.
		/// </summary>
		public const uint SubsystemNetVersion = 1;

		/// <summary>
		/// The unique ID of the network subsystem, assigned for each application using the network system.
		/// This is used to separate different applications that may use the same networking system
		/// from erroneously connecting to each other.
		/// </summary>
		public Guid SubsystemID { get; }

		/// <summary>
		/// The network port that the networking system will use.
		/// </summary>
		public int Port { get; }

		/// <summary>
		/// If the networking system should prefer using IPv6 addresses.
		/// </summary>
		public bool UseIPv6 { get; }

		/// <summary>
		/// For servers, an explicit host name to use or null if it should use the default. For clients,
		/// the host name to connect to.
		/// </summary>
		public virtual string? HostName => null;

		/// <summary>
		/// The timeout value to use. If no packets are transferred within this time period, the connection should be terminated.
		/// </summary>
		public virtual TimeSpan Timeout => TimeSpan.FromSeconds(10);

		/// <summary>
		/// The packet manager for this application.
		/// </summary>
		public PacketManager PacketManager { get; }

		/// <summary>
		/// Event fired when a connection is established.
		/// </summary>
		/// <param name="connection">The connection that was opened</param>
		public virtual void OnConnect(INetConnection connection) { }

		/// <summary>
		/// Event fired when a connection is closed, optionally with an exception that caused the close.
		/// </summary>
		/// <param name="connection">The connection that closed</param>
		/// <param name="error">The error that caused the disconnect, or null</param>
		public virtual void OnDisconnect(INetConnection connection, Exception? error) { }

		/// <summary>
		/// Event fired when a packet is received.
		/// </summary>
		/// <param name="packet">The packet that was received</param>
		/// <param name="connection">The connection that received the packet</param>
		public virtual void OnPacketReceived(Packet packet, INetConnection connection) { }

		/// <summary>
		/// Event fired when an invalid packet is received (either the ID is invalid or
		/// there was an error when decoding the packet).
		/// </summary>
		/// <param name="packetData">The data of the bad packet</param>
		/// <param name="connection">The connection that received the packet</param>
		public virtual void OnBadPacket(PacketData packetData, INetConnection connection) { }

		/// <summary>
		/// Event fired when a packet is received for a completion which does not exist.
		/// Note that this packet is also passed to <see cref="OnPacketReceived(Packet, INetConnection)"/>.
		/// </summary>
		/// <param name="packet">The completion packet that was received</param>
		/// <param name="connection">The connection that received the packet</param>
		public virtual void OnOrphanedCompletion(Packet packet, INetConnection connection) { }

	}

}
