using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Math;

namespace Tesseract.LibAV.Native {
	
	public class SWScaleFunctions {

		public delegate uint PFN_swscale_version();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_swscale_configuration();
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_swscale_license();

		public PFN_swscale_version swscale_version;
		public PFN_swscale_configuration swscale_configuration;
		public PFN_swscale_license swscale_license;

		[return: NativeType("const int*")]
		public delegate IntPtr PFN_sws_getCoefficients(SWSColorspace colorspace);

		public PFN_sws_getCoefficients sws_getCoefficients;

		public delegate bool PFN_sws_isSupportedInput(AVPixelFormat format);
		public delegate bool PFN_sws_isSupportedOutput(AVPixelFormat format);
		public delegate bool PFN_sws_isSupportedEndiannessConversion(AVPixelFormat format);

		public PFN_sws_isSupportedInput sws_isSupportedInput;
		public PFN_sws_isSupportedOutput sws_isSupportedOutput;
		public PFN_sws_isSupportedEndiannessConversion sws_isSupportedEndiannessConversion;

		[return: NativeType("SwsContext*")]
		public delegate IntPtr PFN_sws_alloc_context();
		public delegate int PFN_sws_init_context([NativeType("SwsContext*")] IntPtr context, [NativeType("SwsFilter*")] IntPtr srcFilter, [NativeType("SwsFilter*")] IntPtr dstFilter);
		public delegate void PFN_sws_freeContext([NativeType("SwsContext*")] IntPtr context);
		[return: NativeType("SwsContext*")]
		public delegate IntPtr PFN_sws_getContext(int srcW, int srcH, AVPixelFormat srcPixelFormat, int dstW, int dstH, AVPixelFormat dstPixelFormat, SWSFlags flags, [NativeType("SwsFilter*")] IntPtr srcFilter, [NativeType("SwsFilter*")] IntPtr dstFilter, [NativeType("const double*")] IntPtr param);
		public delegate int PFN_sws_scale([NativeType("SwsContext*")] IntPtr context, [NativeType("const uint8_t* const[]")] IntPtr srcSlice, [NativeType("const int[]")] IntPtr srcStride, int srcSliceY, int srcSliceH, [NativeType("uint8_t* const[]")] IntPtr dst, [NativeType("const int[]")] IntPtr dstStride);
		public delegate int PFN_sws_setColorspaceDetails([NativeType("SwsContext*")] IntPtr context, in Vector4i invTable, int srcRange, in Vector4i table, int dstRange, int brightness, int contrast, int saturation);
		public delegate int PFN_sws_getColorspaceDetails([NativeType("SwsContext*")] IntPtr context, [NativeType("int**")] out IntPtr invTable, out int srcRange, [NativeType("int**")] out IntPtr table, out int dstRange, out int brightness, out int contrast, out int saturation);

		public PFN_sws_alloc_context sws_alloc_context;
		public PFN_sws_init_context sws_init_context;
		public PFN_sws_freeContext sws_freeContext;
		public PFN_sws_getContext sws_getContext;
		public PFN_sws_scale sws_scale;
		public PFN_sws_setColorspaceDetails sws_setColorspaceDetails;
		public PFN_sws_getColorspaceDetails sws_getColorspaceDetails;

		[return: NativeType("SwsVector*")]
		public delegate IntPtr PFN_sws_allocVec(int length);
		[return: NativeType("SwsVector*")]
		public delegate IntPtr PFN_sws_getGaussianVec(double variance, double quality);
		[return: NativeType("SwsVector*")]
		public delegate IntPtr PFN_sws_getConstVec(double c, int length);
		[return: NativeType("SwsVector*")]
		public delegate IntPtr PFN_sws_getIdentityVec();
		public delegate void PFN_sws_scaleVec([NativeType("SwsVector*")] IntPtr a, double scalar);
		public delegate void PFN_sws_normalizeVec([NativeType("SwsVector*")] IntPtr a, double height);
		public delegate void PFN_sws_convVec([NativeType("SwsVector*")] IntPtr a, [NativeType("SwsVector*")] IntPtr b);
		public delegate void PFN_sws_addVec([NativeType("SwsVector*")] IntPtr a, [NativeType("SwsVector*")] IntPtr b);
		public delegate void PFN_sws_subVec([NativeType("SwsVector*")] IntPtr a, [NativeType("SwsVector*")] IntPtr b);
		public delegate void PFN_sws_shiftVec([NativeType("SwsVector*")] IntPtr a, int shift);
		[return: NativeType("SwsVector*")]
		public delegate IntPtr PFN_sws_cloneVec([NativeType("SwsVector*")] IntPtr a);
		public delegate void PFN_sws_printVec2([NativeType("SwsVector*")] IntPtr a, [NativeType("AVClass*")] IntPtr log_ctx, AVLogLevel logLevel);
		public delegate void PFN_sws_freeVec([NativeType("SwsVector*")] IntPtr a);

		public PFN_sws_allocVec sws_allocVec;
		public PFN_sws_getGaussianVec sws_getGaussianVec;
		public PFN_sws_getConstVec sws_getConstVec;
		public PFN_sws_getIdentityVec sws_getIdentityVec;
		public PFN_sws_scaleVec sws_scaleVec;
		public PFN_sws_normalizeVec sws_normalizeVec;
		public PFN_sws_convVec sws_convVec;
		public PFN_sws_addVec sws_addVec;
		public PFN_sws_subVec sws_subVec;
		public PFN_sws_shiftVec sws_shiftVec;
		public PFN_sws_cloneVec sws_cloneVec;
		public PFN_sws_printVec2 sws_printVec2;
		public PFN_sws_freeVec sws_freeVec;

		[return: NativeType("SwsFilter*")]
		public delegate IntPtr PFN_sws_getDefaultFilter(float lumaGBlur, float chromaGBlur, float lumaSharpen, float chromaSharpen, float chromaHShift, float chromaVShift, bool verbose);
		public delegate void PFN_sws_freeFilter([NativeType("SwsFilter*")] IntPtr filter);

		public PFN_sws_getDefaultFilter sws_getDefaultFilter;
		public PFN_sws_freeFilter sws_freeFilter;

		[return: NativeType("SwsContext*")]
		public delegate IntPtr PFN_sws_getCachedContext([NativeType("SwsContext*")] IntPtr context, int srcW, int srcH, AVPixelFormat srcFormat, int dstW, int dstH, AVPixelFormat dstFormat, SWSFlags flags, [NativeType("SwsFilter*")] IntPtr srcFilter, [NativeType("SwsFilter*")] IntPtr dstFilter, [NativeType("const double*")] IntPtr param);

		public PFN_sws_getCachedContext sws_getCachedContext;

		public delegate void PFN_sws_convertPalette8ToPacked32([NativeType("const uint8_t*")] IntPtr src, [NativeType("uint8_t*")] IntPtr dst, int numPixels, [NativeType("const uint8_t*")] IntPtr palette);
		public delegate void PFN_sws_convertPalette8ToPacked24([NativeType("const uint8_t*")] IntPtr src, [NativeType("uint8_t*")] IntPtr dst, int numPixels, [NativeType("const uint8_t*")] IntPtr palette);

		public PFN_sws_convertPalette8ToPacked32 sws_convertPalette8ToPacked32;
		public PFN_sws_convertPalette8ToPacked24 sws_convertPalette8ToPacked24;

		[return: NativeType("AVClass*")]
		public delegate IntPtr PFN_sws_get_class();

		public PFN_sws_get_class sws_get_class;

	}

}
