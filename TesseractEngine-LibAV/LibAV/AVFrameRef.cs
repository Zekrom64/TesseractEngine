using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	public class AVFrameRef : IDisposable, ICloneable {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Variable is modified indirectly by pointer in the libav API")]
		private IntPtr frame;
		public IPointer<AVFrame> Frame => new ManagedPointer<AVFrame>(frame);

		public AVFrameRef(IntPtr pFrame) {
			frame = pFrame;
		}

		public AVFrameRef() {
			frame = LibAVUtil.Functions.av_frame_alloc();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (frame != IntPtr.Zero) {
				unsafe {
					fixed(IntPtr* pFrame = &frame) {
						LibAVUtil.Functions.av_frame_free((IntPtr)pFrame);
					}
				}
			}
		}

		public AVFrameRef Clone() => new(LibAVUtil.Functions.av_frame_clone(frame));

		object ICloneable.Clone() => Clone();

		public AVError GetBuffer(int align = 0) => LibAVUtil.Functions.av_frame_get_buffer(frame, align);

		public bool IsWritable => LibAVUtil.Functions.av_frame_is_writable(frame);

		public AVError MakeWritable() => LibAVUtil.Functions.av_frame_make_writable(frame);

		public AVError Copy(AVFrameRef src) {
			int ret = LibAVUtil.Functions.av_frame_copy(frame, src.frame);
			if (ret < 0) return (AVError)ret;
			else return AVError.None;
		}

		public AVError CopyProps(AVFrameRef src) {
			int ret = LibAVUtil.Functions.av_frame_copy_props(frame, src.frame);
			if (ret < 0) return (AVError)ret;
			else return AVError.None;
		}

		public AVBuffer GetPlaneBuffer(int plane) {
			IntPtr ptr = LibAVUtil.Functions.av_frame_get_plane_buffer(frame, plane);
			return ptr != IntPtr.Zero ? new AVBuffer(ptr) : null;
		}

		public IPointer<AVFrameSideData> NewSideData(AVFrameSideDataType type, int size) => new UnmanagedPointer<AVFrameSideData>(LibAVUtil.Functions.av_frame_new_side_data(frame, type, size));

		public IPointer<AVFrameSideData> GetSideData(AVFrameSideDataType type) => new UnmanagedPointer<AVFrameSideData>(LibAVUtil.Functions.av_frame_get_side_data(frame, type));

		public void RemoveSideData(AVFrameSideDataType type) => LibAVUtil.Functions.av_frame_remove_side_data(frame, type);

		public AVError ApplyCropping(AVFrameCropFlags flags) {
			int ret = LibAVUtil.Functions.av_frame_apply_cropping(frame, flags);
			if (ret >= 0) return AVError.None;
			else return (AVError)ret;
		}

	}
}
