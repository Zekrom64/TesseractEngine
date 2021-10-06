using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class SWSContext : IDisposable {

		[NativeType("SwsContext*")]
		public IntPtr Context { get; }

		public SWSContext() {
			Context = SWScale.Functions.sws_alloc_context();
		}

		public SWSContext([NativeType("SwsContext*")] IntPtr context) {
			Context = context;
		}

		public static SWSContext GetContext(Vector2i srcSize, AVPixelFormat srcFormat, Vector2i dstSize, AVPixelFormat dstFormat, SWSFlags flags, SWSFilter srcFilter, SWSFilter dstFilter, in ReadOnlySpan<double> param) {
			unsafe {
				fixed(double* pParam = param) {
					IntPtr pCtx = SWScale.Functions.sws_getContext(srcSize.X, srcSize.Y, srcFormat, dstSize.X, dstSize.Y, dstFormat, flags, srcFilter, dstFilter, (IntPtr)pParam);
					if (pCtx == IntPtr.Zero) throw new AVException("Failed to get SwsContext");
					return new SWSContext(pCtx);
				}
			}
		}

		public static SWSContext GetContext(Vector2i srcSize, AVPixelFormat srcFormat, Vector2i dstSize, AVPixelFormat dstFormat, SWSFlags flags, SWSFilter srcFilter, SWSFilter dstFilter, params double[] param) {
			unsafe {
				fixed (double* pParam = param) {
					IntPtr pCtx = SWScale.Functions.sws_getContext(srcSize.X, srcSize.Y, srcFormat, dstSize.X, dstSize.Y, dstFormat, flags, srcFilter, dstFilter, (IntPtr)pParam);
					if (pCtx == IntPtr.Zero) throw new AVException("Failed to get SwsContext");
					return new SWSContext(pCtx);
				}
			}
		}

		public void Init(SWSFilter srcFilter, SWSFilter dstFilter) {
			int err = SWScale.Functions.sws_init_context(Context, srcFilter, dstFilter);
			if (err < 0) throw new AVException("Failed to initialize SwsContext", (AVError)err);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			SWScale.Functions.sws_freeContext(Context);
		}

		public int Scale([NativeType("const uint8_t* const[]")] in ReadOnlySpan<IntPtr> srcSlice, in ReadOnlySpan<int> srcStride, int srcSliceY, int srcSliceH, [NativeType("uint8_t* const[]")] in ReadOnlySpan<IntPtr> dst, in ReadOnlySpan<int> dstStride) {
			unsafe {
				fixed(IntPtr* pSrcSlice = srcSlice, pDst = dst) {
					fixed(int* pSrcStride = srcStride, pDstStride = dstStride) {
						return SWScale.Functions.sws_scale(Context, (IntPtr)pSrcSlice, (IntPtr)pSrcStride, srcSliceY, srcSliceH, (IntPtr)pDst, (IntPtr)pDstStride);
					}
				}
			}
		}

		public void SetColorspaceDetails(Vector4i invTable, int srcRange, Vector4i table, int dstRange, int brightness, int contrast, int saturation) {
			if (SWScale.Functions.sws_setColorspaceDetails(Context, invTable, srcRange, table, dstRange, brightness, contrast, saturation) < 0)
				throw new AVException("Setting colorspace details is not supported on this SwsContext");
		}

		public void SetColorspaceDetails(out Vector4i invTable, out int srcRange, out Vector4i table, out int dstRange, out int brightness, out int contrast, out int saturation) {
			if (SWScale.Functions.sws_getColorspaceDetails(Context, out IntPtr pInvTable, out srcRange, out IntPtr pTable, out dstRange, out brightness, out contrast, out saturation) >= 0) {
				invTable = MemoryUtil.ReadUnmanaged<Vector4i>(pInvTable);
				table = MemoryUtil.ReadUnmanaged<Vector4i>(pTable);
			} else throw new AVException("Setting colorspace details is not supported on this SwsContext");
		}

	}
}
