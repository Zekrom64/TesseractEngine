using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Net {

	/// <summary>
	/// A simple keepalive packet injected into the packet stream to keep
	/// the connection from timing out.
	/// </summary>
	public class InternalPacket00BKeepalive : Packet {

		public override void Read(BinaryReader br) { }

		public override void Write(BinaryWriter bw) { }

	}

	/// <summary>
	/// Internal client information packet, holding information about the
	/// client's networking system. This is sent from a client when first
	/// connecting to inform the server about itself.
	/// </summary>
	public class InternalPacket01CClientInfo : Packet {

		/// <summary>
		/// The client's network subsystem version.
		/// </summary>
		public uint SubsystemNetVersion;

		/// <summary>
		/// The client's network subsystem ID.
		/// </summary>
		public Guid SubsystemID;

		public InternalPacket01CClientInfo() { }

		public InternalPacket01CClientInfo(INetInterface iface) {
			SubsystemNetVersion = INetInterface.SubsystemNetVersion;
			SubsystemID = iface.SubsystemID;
		}

		public override void Read(BinaryReader br) {
			SubsystemNetVersion = (uint)br.Read7BitEncodedInt();
			SubsystemID = new Guid(br.ReadString());
		}

		public override void Write(BinaryWriter bw) {
			bw.Write7BitEncodedInt((int)SubsystemNetVersion);
			bw.Write(SubsystemID.ToString());
		}

	}

	/// <summary>
	/// Server connection confirmation packet, sent to a client to
	/// confirm that it's connection has been accepted. If not received
	/// either a timeout occurred, or a disconnect packet was sent to
	/// terminate the connection
	/// </summary>
	public class InternalPacket02SConfirmInfo : Packet {

		public override void Read(BinaryReader br) { }

		public override void Write(BinaryWriter bw) { }

	}

	/// <summary>
	/// A disconnect packet informs the remote host that the connection
	/// has been closed.
	/// </summary>
	public class InternalPacket03BDisconnect : Packet {

		/// <summary>
		/// A discrete reason why the connection was closed.
		/// </summary>
		public NetCloseCause Cause;

		/// <summary>
		/// A readable message to describe why the connection was closed.
		/// </summary>
		public string Message = "";

		public override void Read(BinaryReader br) {
			Cause = (NetCloseCause)br.ReadByte();
			Message = br.ReadString();
		}

		public override void Write(BinaryWriter bw) {
			bw.Write((byte)Cause);
			bw.Write(Message);
		}
	}

	/// <summary>
	/// A heartbeat packet signals the remote host to respond with
	/// its own heartbeat packet.
	/// </summary>
	public class InternalPacket04BHeartbeat : Packet {

		/// <summary>
		/// If the heartbeat should be responded to.
		/// </summary>
		public bool Respond;

		public InternalPacket04BHeartbeat() {
			Respond = true;
		}

		public override void Read(BinaryReader br) {
			Respond = br.ReadBoolean();
		}

		public override void Write(BinaryWriter bw) {
			bw.Write(Respond);
		}
	}

}
