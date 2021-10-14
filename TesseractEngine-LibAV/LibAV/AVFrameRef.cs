using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	public class AVFrameRef : IDisposable, ICloneable {

		public static void Free(IntPtr pFrame) {
			unsafe {
				LibAVUtil.Functions.av_frame_free((IntPtr)(&pFrame));
			}
		}

		public ManagedPointer<AVFrame> Frame { get; private set; }

		public AVFrameRef(ManagedPointer<AVFrame> pFrame) {
			Frame = pFrame;
		}

		public AVFrameRef() {
			Frame = new(LibAVUtil.Functions.av_frame_alloc(), Free);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Frame) {
				Frame.Dispose();
				Frame = default;
			}
		}

		public AVFrameRef Clone() => new(new ManagedPointer<AVFrame>(LibAVUtil.Functions.av_frame_clone(Frame), Free));

		object ICloneable.Clone() => Clone();

		public AVError GetBuffer(int align = 0) => LibAVUtil.Functions.av_frame_get_buffer(Frame, align);

		public bool IsWritable => LibAVUtil.Functions.av_frame_is_writable(Frame);

		public AVError MakeWritable() => LibAVUtil.Functions.av_frame_make_writable(Frame);

		public AVError Copy(AVFrameRef src) {
			int ret = LibAVUtil.Functions.av_frame_copy(Frame, src.Frame);
			if (ret < 0) return (AVError)ret;
			else return AVError.None;
		}

		public AVError CopyProps(AVFrameRef src) {
			int ret = LibAVUtil.Functions.av_frame_copy_props(Frame, src.Frame);
			if (ret < 0) return (AVError)ret;
			else return AVError.None;
		}

		public AVBuffer GetPlaneBuffer(int plane) {
			IntPtr ptr = LibAVUtil.Functions.av_frame_get_plane_buffer(Frame, plane);
			return ptr != IntPtr.Zero ? new AVBuffer(ptr) : null;
		}

		public IPointer<AVFrameSideData> NewSideData(AVFrameSideDataType type, int size) => new UnmanagedPointer<AVFrameSideData>(LibAVUtil.Functions.av_frame_new_side_data(Frame, type, size));

		public IPointer<AVFrameSideData> GetSideData(AVFrameSideDataType type) => new UnmanagedPointer<AVFrameSideData>(LibAVUtil.Functions.av_frame_get_side_data(Frame, type));

		public void RemoveSideData(AVFrameSideDataType type) => LibAVUtil.Functions.av_frame_remove_side_data(Frame, type);

		public AVError ApplyCropping(AVFrameCropFlags flags) {
			int ret = LibAVUtil.Functions.av_frame_apply_cropping(Frame, flags);
			if (ret >= 0) return AVError.None;
			else return (AVError)ret;
		}

		public void FillAudioFrame(int nbChannels, AVSampleFormat sampleFmt, in ReadOnlySpan<byte> buf, int align = 0) {
			unsafe {
				fixed(byte* pBuf = buf) {
					int err = LibAVCodec.Functions.avcodec_fill_audio_frame(Frame, nbChannels, sampleFmt, (IntPtr)pBuf, buf.Length, align);
					if (err < 0) throw new AVException("Failed to fill audio frame", (AVError)err);
				}
			}
		}

	}
}
