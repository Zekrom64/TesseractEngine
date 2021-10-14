using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVBSFContextRef : IDisposable {

		private static void Free(IntPtr ctx) {
			unsafe {
				LibAVCodec.Functions.av_bsf_free((IntPtr)(&ctx));
			}
		}

		public ManagedPointer<AVBSFContext> BSFContext { get; private set; }

		public AVBSFContextRef(ManagedPointer<AVBSFContext> bsfctx) {
			BSFContext = bsfctx;
		}

		public AVBSFContextRef(AVBitStreamFilterRef bsf) {
			int err = LibAVCodec.Functions.av_bsf_alloc(bsf.BitStreamFilter, out IntPtr ctx);
			if (err < 0) throw new AVException("Failed to allocate bitstream filter context", (AVError)err);
			BSFContext = new(ctx, Free);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (BSFContext) {
				BSFContext.Dispose();
				BSFContext = default;
			}
		}

		public void Init() {
			int err = LibAVCodec.Functions.av_bsf_init(BSFContext);
			if (err < 0) throw new AVException("Failed to initialize bitstream filter context", (AVError)err);
		}

		public void SendPacket(AVPacketRef pkt) {
			AVError err = LibAVCodec.Functions.av_bsf_send_packet(BSFContext, pkt.Packet);
			if (err != AVError.None) throw new AVException("Failed to send packet to bitstream filter context", err);
		}

		public void ReceivePacket(AVPacketRef pkt) {
			AVError err = LibAVCodec.Functions.av_bsf_receive_packet(BSFContext, pkt.Packet);
			if (err != AVError.None) throw new AVException("Failed to receive packet from bitstream filter context", err);
		}

		public void Flush() => LibAVCodec.Functions.av_bsf_flush(BSFContext);

	}

}
