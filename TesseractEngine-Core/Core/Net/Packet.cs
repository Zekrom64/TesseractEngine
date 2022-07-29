using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Net {

	/// <summary>
	/// <para>
	/// A packet ID identifies the type of a packet within the context of a <see cref="PacketManager"/>. A
	/// packet ID is comprised of a module ID and a sub-ID.
	/// </para>
	/// <para>
	/// A module ID identifies the networking module that manages the packet. While module IDs are generally
	/// managed by the packet manager, there are two fixed IDs, <see cref="ModuleInternal"/> used internally
	/// by the networking system and <see cref="ModuleApplication"/> which are reserved for the base application
	/// using the networking system. A packet manager may assign additional modules as necessary such as for
	/// mods that need networking functionality.
	/// </para>
	/// <para>The sub-ID determines the type of packet within a module and is generally hardcoded.</para>
	/// </summary>
	public record struct PacketID : IData {

		public const int SizeOf = sizeof(ushort) * 2;

		/// <summary>
		/// The module ID for the internal networking system.
		/// </summary>
		public const ushort ModuleInternal = 0;

		/// <summary>
		/// The module ID for the application.
		/// </summary>
		public const ushort ModuleApplication = 1;

		/// <summary>
		/// The module component of the ID.
		/// </summary>
		public ushort ModuleID { get; set; }

		/// <summary>
		/// The sub-ID component of the ID.
		/// </summary>
		public ushort SubID { get; set; }

		public PacketID(ushort moduleID, ushort subID) {
			ModuleID = moduleID;
			SubID = subID;
		}

		public void Read(BinaryReader br) {
			ModuleID = br.ReadUInt16();
			SubID = br.ReadUInt16();
		}

		public void Write(BinaryWriter bw) {
			bw.Write(ModuleID);
			bw.Write(SubID);
		}
	}

	/// <summary>
	/// A header that all packets use.
	/// </summary>
	public struct PacketHeader : IData {

		public const int SizeOf = PacketID.SizeOf + sizeof(uint) * 3;

		/// <summary>
		/// The ID of the packet.
		/// </summary>
		public PacketID ID;

		/// <summary>
		/// The sequence number of the packet.
		/// </summary>
		public uint SequenceNumber;

		/// <summary>
		/// The completion number of the packet.
		/// </summary>
		public uint CompletionNumber;

		/// <summary>
		/// The length of the packet's payload in bytes.
		/// </summary>
		public uint Length;

		public void Read(BinaryReader br) {
			ID.Read(br);
			SequenceNumber = br.ReadUInt32();
			CompletionNumber = br.ReadUInt32();
			Length = br.ReadUInt32();
		}

		public void Write(BinaryWriter bw) {
			ID.Write(bw);
			bw.Write(SequenceNumber);
			bw.Write(CompletionNumber);
			bw.Write(Length);
		}
	}

	/// <summary>
	/// A structure containing the undecoded data of a packet. A packet manager will decode this
	/// data into a complete <see cref="Packet"/> object.
	/// </summary>
	public readonly struct PacketData {

		/// <summary>
		/// The ID of the packet.
		/// </summary>
		public PacketID ID { get; init; }

		/// <summary>
		/// The sequence number of the packet.
		/// </summary>
		public uint SequenceNumber { get; init; }

		/// <summary>
		/// The completion number of the packet.
		/// </summary>
		public uint CompletionNumber { get; init; }

		/// <summary>
		/// The payload data of the packet.
		/// </summary>
		public byte[] Data { get; init; }

	}

	/// <summary>
	/// The base class for all packets.
	/// </summary>
	public abstract class Packet : IData {

		/// <summary>
		/// An ID number identifying the type of packet. It is the job of
		/// a <see cref="PacketManager"/> to determine how this number is
		/// assigned, but there are some required packets.
		/// </summary>
		public PacketID ID { get; internal set; }

		/// <summary>
		/// A sequence number identifying this packet over a connection. The sequence number
		/// starts at 1 and increases for every packet sent in one direction. This number is
		/// used for completions via <see cref="CompletionNumber"/>.
		/// </summary>
		public uint SequenceNumber { get; internal set; }

		/// <summary>
		/// A sequence number identifying which packet this packet is 'responding' to, if any.
		/// Completions may themselves 
		/// If this is not a completion, this value will be 0.
		/// </summary>
		public uint CompletionNumber { get; internal set; }

		/// <summary>
		/// Called to read the payload data for this packet from a binary reader.
		/// </summary>
		/// <param name="br">Reader to read data from</param>
		public abstract void Read(BinaryReader br);

		/// <summary>
		/// Called to write the payload data from this packet to a binary writer.
		/// </summary>
		/// <param name="bw">Writer to write data to</param>
		public abstract void Write(BinaryWriter bw);

	}
	
	/// <summary>
	/// A packet manager handles the mapping of packet IDs to packet class types.
	/// </summary>
	public abstract class PacketManager {

		private readonly Dictionary<PacketID, Func<Packet>> ctors = new();

		/// <summary>
		/// Registers a new packet with the packet manager.
		/// </summary>
		/// <typeparam name="T">The type of the packet</typeparam>
		/// <param name="id">The ID to map the packet to</param>
		protected void RegisterPacket<T>(PacketID id) where T : Packet, new() => ctors[id] = () => new T();

		protected PacketManager() {
			// All packet managers must register the basic internal packets
			RegisterPacket<InternalPacket00BKeepalive>(new PacketID(0, 0));
			RegisterPacket<InternalPacket01CClientInfo>(new PacketID(0, 1));
			RegisterPacket<InternalPacket02SConfirmInfo>(new PacketID(0, 2));
			RegisterPacket<InternalPacket03BDisconnect>(new PacketID(0, 3));
			RegisterPacket<InternalPacket04BHeartbeat>(new PacketID(0, 4));
		}

		// Note: Functions are made virtual here to provide additional flexibility for implementers.

		/// <summary>
		/// Attempts to find a constructor for the given packet ID.
		/// </summary>
		/// <param name="id">Packet ID</param>
		/// <returns>The constructor for the corresponding packet type</returns>
		public virtual Func<Packet>? FindConstructor(PacketID id) {
			if (ctors.TryGetValue(id, out var func)) return func;
			return null;
		}

		/// <summary>
		/// Constructs a packet of the type corresponding to the given ID.
		/// </summary>
		/// <param name="id">Packet ID</param>
		/// <returns>Constructed packet</returns>
		public virtual Packet Construct(PacketID id) => ctors[id]();

	}

}
