using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVPacketRef : IDisposable {

		public static void Free(IntPtr pPacket) {
			unsafe {
				LibAVCodec.Functions.av_packet_free((IntPtr)(&pPacket));
			}
		}

		public ManagedPointer<AVPacket> Packet { get; private set; }

		public AVPacketRef(ManagedPointer<AVPacket> packet) {
			Packet = packet;
		}

		public AVPacketRef() {
			Packet = new ManagedPointer<AVPacket>(LibAVCodec.Functions.av_packet_alloc(), Free);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Packet) {
				Packet.Dispose();
				Packet = default;
			}
		}

		public AVPacketRef Clone() => new(new ManagedPointer<AVPacket>(LibAVCodec.Functions.av_packet_clone(Packet), Free));

		public void Init() => LibAVCodec.Functions.av_init_packet(Packet);

		public void New(int size) {
			AVError err = LibAVCodec.Functions.av_new_packet(Packet, size);
			if (err != AVError.None) throw new AVException("Failed to initialize packet payload", err);
		}

		public void Shrink(int size) => LibAVCodec.Functions.av_shrink_packet(Packet, size);

		public void Grow(int growBy) => LibAVCodec.Functions.av_grow_packet(Packet, growBy);

		public Span<byte> NewSideData(AVPacketSideDataType type, int size) {
			IntPtr pData = LibAVCodec.Functions.av_packet_new_side_data(Packet, type, size);
			unsafe {
				return new Span<byte>((void*)pData, size);
			}
		}

		public void AddSideData(AVPacketSideDataType type, in ReadOnlySpan<byte> data) {
			unsafe {
				fixed(byte* pData = data) {
					IntPtr pNewData = LibAVUtil.Functions.av_malloc((nuint)data.Length);
					MemoryUtil.Copy(new UnmanagedPointer<byte>(pNewData), data, data.Length);
					int err = LibAVCodec.Functions.av_packet_add_side_data(Packet, type, pNewData, (nuint)data.Length);
					if (err < 0) throw new AVException("Failed to add sided packet data", (AVError)err);
				}
			}
		}

		public void FreeSideData() => LibAVCodec.Functions.av_packet_free_side_data(Packet);

		// av_packet_ref
		// av_packet_unref
		// av_packet_move_ref

		public void CopyProps(AVPacketRef src) {
			AVError err = LibAVCodec.Functions.av_packet_copy_props(Packet, src.Packet);
			if (err != AVError.None) throw new AVException("Failed to copy packet properties", err);
		}

		public void RescaleTS(AVRational src, AVRational dst) => LibAVCodec.Functions.av_packet_rescale_ts(Packet, src, dst);

	}

}
