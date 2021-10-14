using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class AVCodecContextRef : IDisposable {

		public ManagedPointer<AVCodecContext> Context { get; private set; }

		public bool IsOpen => LibAVCodec.Functions.avcodec_is_open(Context);

		public AVCodecContextRef([NativeType("AVCodecContext*")] IntPtr pContext) {
			Context = new ManagedPointer<AVCodecContext>(pContext);
		}

		public AVCodecContextRef(AVCodecRef codec) {
			Context = new ManagedPointer<AVCodecContext>(LibAVCodec.Functions.avcodec_alloc_context3(codec.Codec));
		}

		public AVCodecContextRef() {
			Context = new ManagedPointer<AVCodecContext>(LibAVCodec.Functions.avcodec_alloc_context3(IntPtr.Zero));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			IntPtr ctx = Context;
			unsafe {
				LibAVCodec.Functions.avcodec_free_context((IntPtr)(&ctx));
			}
			Context = default;
		}

		public void Open(AVCodecRef codec, ref AVDictionary options) {
			IntPtr pOptions = options.Dictionary;
			AVError err;
			unsafe {
				err = LibAVCodec.Functions.avcodec_open2(Context, codec.Codec, (IntPtr)(&pOptions));
			}
			if (err != AVError.None) throw new AVException("Failed to open codec", err);
			options = new AVDictionary(pOptions);
		}

		public void AlignDimensions(ref int width, ref int height) => LibAVCodec.Functions.avcodec_align_dimensions(Context, ref width, ref height);

		public void AlignDimensions(ref int width, ref int height, Span<int> linesizeAlign) {
			if (linesizeAlign.Length < AVFrame.NumDataPointers) throw new ArgumentException("Span must have at least AV_NUM_DATA_POINTERS elements", nameof(linesizeAlign));
			unsafe {
				fixed(int* pLinesizeAlign = linesizeAlign) {
					LibAVCodec.Functions.avcodec_align_dimensions2(Context, ref width, ref height, (IntPtr)pLinesizeAlign);
				}
			}
		}

		// avcodec_decode_subtitle2

		public void SendPacket(AVPacketRef pkt) {
			AVError err = LibAVCodec.Functions.avcodec_send_packet(Context, pkt.Packet);
			if (err != AVError.None) throw new AVException("Failed to send packet", err);
		}

		public void ReceiveFrame(AVFrameRef frame) {
			AVError err = LibAVCodec.Functions.avcodec_receive_frame(Context, frame.Frame);
			if (err != AVError.None) throw new AVException("Failed to receive frame", err);
		}

		public void SendFrame(AVFrameRef frame) {
			AVError err = LibAVCodec.Functions.avcodec_send_frame(Context, frame.Frame);
			if (err != AVError.None) throw new AVException("Failed to send frame", err);
		}

		public void ReceivePacket(AVPacketRef pkt) {
			AVError err = LibAVCodec.Functions.avcodec_receive_packet(Context, pkt.Packet);
			if (err != AVError.None) throw new AVException("Failed to receive packet", err);
		}

		// avcodec_get_hw_frames_parameters

		// avcodec_encode_subtitle

		public void FlushBuffers() => LibAVCodec.Functions.avcodec_flush_buffers(Context);

		public int GetAudioFrameDuration(int frameBytes = 0) => LibAVCodec.Functions.av_get_audio_frame_duration(Context, frameBytes);

	}

}
