using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {

	public static class SWScale {

		public static readonly LibrarySpec Spec = new() { Name = "swscale" };
		public static readonly Library Library = LibraryManager.Load(Spec);
		public static SWScaleFunctions Functions { get; } = new();

		public const int SrcVChrDropMask = 0x30000;
		public const int SrcVChrDropShift = 16;

		public const int ParamDefault = 123456;

		public const double MaxReduceCutoff = 0.002;

		static SWScale() {
			Library.LoadFunctions(Functions);
		}

		public static int Version => (int)Functions.swscale_version();

		public static string Configuration => MemoryUtil.GetASCII(Functions.swscale_configuration());

		public static string License => MemoryUtil.GetASCII(Functions.swscale_license());

		public static bool IsSupportedInput(AVPixelFormat format) => Functions.sws_isSupportedInput(format);

		public static bool IsSupportedOutput(AVPixelFormat format) => Functions.sws_isSupportedOutput(format);

		public static bool IsSupportedEndiannessConversion(AVPixelFormat format) => Functions.sws_isSupportedEndiannessConversion(format);


		public static void ConvertPalette8ToPacked32(IConstPointer<byte> src, IPointer<byte> dst, int numPixels, IConstPointer<byte> palette) =>
			Functions.sws_convertPalette8ToPacked32(src.Ptr, dst.Ptr, numPixels, palette.Ptr);

		public static void ConvertPalette8ToPacked32(in ReadOnlySpan<byte> src, Span<byte> dst, in ReadOnlySpan<byte> palette) {
			int numPixels = Math.Min(src.Length, dst.Length / 4);
			unsafe {
				fixed(byte* pSrc = src, pDst = dst, pPalette = palette) {
					Functions.sws_convertPalette8ToPacked32((IntPtr)pSrc, (IntPtr)pDst, numPixels, (IntPtr)pPalette);
				}
			}
		}

		public static void ConvertPalette8ToPacked24(IConstPointer<byte> src, IPointer<byte> dst, int numPixels, IConstPointer<byte> palette) =>
			Functions.sws_convertPalette8ToPacked24(src.Ptr, dst.Ptr, numPixels, palette.Ptr);

		public static void ConvertPalette8ToPacked24(in ReadOnlySpan<byte> src, Span<byte> dst, in ReadOnlySpan<byte> palette) {
			int numPixels = Math.Min(src.Length, dst.Length / 3);
			unsafe {
				fixed (byte* pSrc = src, pDst = dst, pPalette = palette) {
					Functions.sws_convertPalette8ToPacked24((IntPtr)pSrc, (IntPtr)pDst, numPixels, (IntPtr)pPalette);
				}
			}
		}

		// TODO: sws_get_class

	}
}
