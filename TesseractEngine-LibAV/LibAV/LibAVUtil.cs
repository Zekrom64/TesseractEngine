﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.LibAV.Native;

namespace Tesseract.LibAV {

	public static class LibAVUtil {

		public static readonly LibrarySpec LibrarySpec = new() { Name = "avutil" };
		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);
		public static LibAVUtilFunctions Functions { get; } = new();

		public const int FFLambdaShift = 7;
		public const int FFLambdaScale = 1 << FFLambdaShift;
		public const int FFQP2Lambda = 118;
		public const int FFLambdaMax = 256 * 128 - 1;
		public const int FFQualityScale = FFLambdaScale;

		public const long AVNoPTSValue = long.MinValue;
		public const int AVTimeBase = 1000000;
		public static readonly AVRational AVTimeBaseQ = new(1, AVTimeBase);

		static LibAVUtil() {
			Library.LoadFunctions(Functions);

#if LIBAVUTIL_VERSION_56
			if (VersionMajor(Version) > 56) throw new AVException($"avutil library has incompatible version >56");
#endif
		}

		// version.h

		public static int AVVersionInt(int a, int b, int c) => (a << 16) | (b << 8) | c;

		public static string AVVersionDot(int a, int b, int c) => a + "." + b + "." + c;

		public static string AVVersion(int a, int b, int c) => AVVersionDot(a, b, c);

		public static int VersionMajor(int v) => v >> 16;

		public static int VersionMinor(int v) => (v >> 8) & 0xFF;

		public static int VersionMicro(int v) => v & 0xFF;

		// avutil.h

		public static int Version => (int)Functions.avutil_version();

		public static string VersionInfo => MemoryUtil.GetASCII(Functions.av_version_info());

		public static string Configuration => MemoryUtil.GetASCII(Functions.avutil_configuration());

		public static string License => MemoryUtil.GetASCII(Functions.avutil_license());

		public static char GetPictureTypeChar(AVPictureType pictureType) => (char)Functions.av_get_picture_type_char(pictureType);

		public static AVRational TimeBaseQ => Functions.av_get_time_base_q();

		// cpu.h

		public static AVCPUFlags CPUFlags => Functions.av_get_cpu_flags();

		public static AVCPUFlags CPUFlagsMask {
			set => Functions.av_set_cpu_flags_mask(value);
		}

		public static AVCPUFlags ParseCPUFlags(string s) => Functions.av_parse_cpu_flags(s);

		public static int CPUCount => Functions.av_cpu_count();

		public static nuint MaxAlignment => Functions.av_cpu_max_align();

		// log.h

		public static AVLogLevel LogLevel {
			get => Functions.av_log_get_level();
			set => Functions.av_log_set_level(value);
		}

		public static AVLogFlags LogFlags {
			set => Functions.av_log_set_flags(value);
		}

		// hwcontext.h

		public static AVHWDeviceType HWDeviceFindTypeByName(string name) => Functions.av_hwdevice_find_type_by_name(name);

		public static string HWDeviceGetTypeName(AVHWDeviceType type) => MemoryUtil.GetASCII(Functions.av_hwdevice_get_type_name(type));

		public static AVHWDeviceType HWDeviceIterateTypes(AVHWDeviceType prev) => Functions.av_hwdevice_iterate_types(prev);

		public static AVBuffer HWDeviceCtxAlloc(AVHWDeviceType type) => new(Functions.av_hwdevice_ctx_alloc(type));

		public static AVError HWDeviceCtxInit(AVBuffer buffer) => Functions.av_hwdevice_ctx_init(buffer.Buffer.Ptr);

		public static AVError HWDeviceCtxCreate(out AVBuffer buffer, AVHWDeviceType type, string device, AVDictionary opts, int flags = 0) {
			IntPtr buf = IntPtr.Zero;
			AVError err;
			unsafe {
				err = Functions.av_hwdevice_ctx_create((IntPtr)(&buf), type, device, opts != null ? opts.Dictionary : IntPtr.Zero, flags);
			}
			buffer = buf != IntPtr.Zero ? new AVBuffer(buf) : null;
			return err;
		}

		public static AVError HWDeviceCtxCreateDerived(out AVBuffer dstCtx, AVHWDeviceType type, AVBuffer srcCtx, int flags = 0) {
			IntPtr buf = IntPtr.Zero;
			AVError err;
			unsafe {
				err = Functions.av_hwdevice_ctx_create_derived((IntPtr)(&buf), type, srcCtx.Buffer.Ptr, flags);
			}
			dstCtx = buf != IntPtr.Zero ? new AVBuffer(buf) : null;
			return err;
		}

		public static AVBuffer HWFrameCtxAlloc(AVBuffer deviceCtx) {
			IntPtr buf = Functions.av_hwframe_ctx_alloc(deviceCtx.Buffer.Ptr);
			return buf != IntPtr.Zero ? new AVBuffer(buf) : null;
		}

		public static AVError HWFrameCtxInit(AVBuffer _ref) => Functions.av_hwframe_ctx_init(_ref.Buffer.Ptr);

		public static AVError HWFrameGetBuffer(AVBuffer hwframeCtx, IPointer<AVFrame> frame, int flags = 0) => Functions.av_hwframe_get_buffer(hwframeCtx.Buffer.Ptr, frame.Ptr, flags);

		public static AVError HWFrameTransferData(IPointer<AVFrame> dst, IConstPointer<AVFrame> src, int flags = 0) => Functions.av_hwframe_transfer_data(dst.Ptr, src.Ptr, flags);

		public static AVError HWFrameTransferGetFormats(AVBuffer hwframeCtx, AVHWFrameTransferDirection dir, out ReadOnlySpan<AVPixelFormat> formats, int flags = 0) {
			unsafe {
				AVPixelFormat* pFormats;
				AVError ret = Functions.av_hwframe_transfer_get_formats(hwframeCtx.Buffer.Ptr, dir, (IntPtr)(&pFormats), flags);
				if (ret != AVError.None) {
					formats = default;
					return ret;
				}
				int n = 0;
				while (pFormats[n] != AVPixelFormat.None) n++;
				formats = new ReadOnlySpan<AVPixelFormat>(pFormats, n);
				return AVError.None;
			}
		}

		public static IntPtr HWDeviceHWConfigAlloc(AVBuffer deviceCtx) => Functions.av_hwdevice_hwconfig_alloc(deviceCtx.Buffer.Ptr);

		public static IPointer<AVHWFramesConstraints> HWDeviceGetHWFrameConstraints(AVBuffer _ref, IntPtr hwconfig) {
			IntPtr pConstraints = Functions.av_hwdevice_get_hwframe_constraints(_ref.Buffer.Ptr, hwconfig);
			return pConstraints != IntPtr.Zero ? new UnmanagedPointer<AVHWFramesConstraints>(pConstraints) : null;
		}

		public static void HWFrameConstraintsFree(ref IPointer<AVHWFramesConstraints> constraints) {
			IntPtr pConstraints = constraints.Ptr;
			unsafe {
				Functions.av_hwframe_constraints_free((IntPtr)(&pConstraints));
			}
			if (pConstraints == IntPtr.Zero) constraints = null;
		}

		public static AVError HWFrameMap(IPointer<AVFrame> dst, IConstPointer<AVFrame> src, AVHWFrameMapFlags flags) => Functions.av_hwframe_map(dst.Ptr, src.Ptr, flags);

		public static AVError HWFrameCtxCreateDerived(out AVBuffer derivedFrameCtx, AVPixelFormat format, AVBuffer derivedDeviceCtx, AVBuffer sourceFrameCtx, AVHWFrameMapFlags flags) {
			IntPtr pCtx;
			AVError err;
			unsafe {
				err = Functions.av_hwframe_ctx_create_derived((IntPtr)(&pCtx), format, derivedDeviceCtx.Buffer.Ptr, sourceFrameCtx.Buffer.Ptr, flags);
			}
			if (err != AVError.None) {
				derivedFrameCtx = null;
				return err;
			}
			derivedFrameCtx = new AVBuffer(pCtx);
			return AVError.None;
		}

		// intfloat.h

		/* Pretty much just the standard BitConverter functionality but more contrived in the headers because C
		public static float Int2Float(uint i) => BitConverter.Int32BitsToSingle((int)i);

		public static uint Float2Int(float f) => (uint)BitConverter.SingleToInt32Bits(f);

		public static double Int2Double(ulong i) => BitConverter.Int64BitsToDouble((long)i);

		public static ulong Double2Int(double f) => (ulong)BitConverter.DoubleToInt64Bits(f);
		*/

		// mathematics.h

		public static long GCD(long a, long b) => Functions.av_gcd(a, b);

		public static long Rescale(long a, long b, long c) => Functions.av_rescale(a, b, c);

		public static long Rescale(long a, long b, long c, AVRounding round) => Functions.av_rescale_rnd(a, b, c, round);

		public static long Rescale(long a, AVRational bq, AVRational cq) => Functions.av_rescale_q(a, bq, cq);

		public static long Rescale(long a, AVRational bq, AVRational cq, AVRounding round) => Functions.av_rescale_q_rnd(a, bq, cq, round);

		public static int CompareTS(long tsA, AVRational tbA, long tsB, AVRational tbB) => Functions.av_compare_ts(tsA, tbA, tsB, tbB);

		public static long CompareMod(ulong a, ulong b, ulong mod) => Functions.av_compare_mod(a, b, mod);

		// channel_layout.h

		public static AVChannelMask GetChannelLayout(string name) => Functions.av_get_channel_layout(name);

		public static string GetChannelLayoutString(AVChannelMask channelLayout, int nbChannels = -1) {
			Span<byte> buf = stackalloc byte[4096];
			unsafe {
				fixed(byte* pBuf = buf) {
					Functions.av_get_channel_layout_string((IntPtr)pBuf, buf.Length, nbChannels, channelLayout);
				}
			}
			return MemoryUtil.GetASCII(buf);
		}

		public static int GetChannelLayoutNbChannels(AVChannelMask channelLayout) => Functions.av_get_channel_layout_nb_channels(channelLayout);

		public static AVChannelMask GetDefaultChannelLayout(int nbChannels) => Functions.av_get_default_channel_layout(nbChannels);

		public static int GetChannelLayoutChannelIndex(AVChannelMask channelLayout, AVChannelMask channel) => Functions.av_get_channel_layout_channel_index(channelLayout, channel);

		public static AVChannelMask ChannelLayoutExtractChannel(AVChannelMask channelLayout, int index) => Functions.av_channel_layout_extract_channel(channelLayout, index);

		public static string GetChannelName(AVChannelMask channel) => MemoryUtil.GetASCII(Functions.av_get_channel_name(channel));

		// mem.h

		public static ManagedPointer<byte> Malloc(nuint size) => new(Functions.av_malloc(size), new(Functions.av_free), (int)size);

		public static ManagedPointer<byte> MallocArray(nuint nmemb, nuint size) => new(Functions.av_malloc_array(nmemb, size), new(Functions.av_free), (int)(nmemb * size));

		public static ManagedPointer<byte> Realloc(IntPtr ptr, nuint size) => new(Functions.av_realloc(ptr, size), new(Functions.av_free), (int)size);

		public static ManagedPointer<byte> Reallocp(IntPtr ptr, nuint size) {
			AVError err;
			unsafe {
				err = Functions.av_reallocp((IntPtr)(&ptr), size);
			}
			if (err != AVError.None) throw new AVException("Call to av_reallocp() failed", err);
			return new ManagedPointer<byte>(ptr, new(Functions.av_free), (int)size);
		}

		public static ManagedPointer<byte> ReallocArray(IntPtr ptr, nuint nmemb, nuint size) => new(Functions.av_realloc_array(ptr, nmemb, size), new(Functions.av_free), (int)(nmemb * size));

		public static ManagedPointer<byte> ReallocpArray(IntPtr ptr, nuint nmemb, nuint size) {
			AVError err;
			unsafe {
				err = Functions.av_reallocp_array((IntPtr)(&ptr), nmemb, size);
			}
			if (err != AVError.None) throw new AVException("Call to av_reallocp_array() failed", err);
			return new ManagedPointer<byte>(ptr, new(Functions.av_free), (int)size);
		}

		public static void Free(IntPtr ptr) => Functions.av_free(ptr);

		public static ManagedPointer<byte> Mallocz(nuint size) => new(Functions.av_mallocz(size), new(Functions.av_free), (int)size);

		public static ManagedPointer<byte> MalloczArray(nuint nmemb, nuint size) => new(Functions.av_mallocz_array(nmemb, size), new(Functions.av_free), (int)(nmemb * size));

		public static ManagedPointer<byte> Strdup([NativeType("const char*")] IntPtr ptr) => new(Functions.av_strdup(ptr), new(Functions.av_free), MemoryUtil.FindFirst(ptr, 0));

		public static ManagedPointer<byte> Strndup([NativeType("const char*")] IntPtr ptr, nuint len) {
			ptr = Functions.av_strndup(ptr, len);
			return new ManagedPointer<byte>(ptr, new(Functions.av_free), MemoryUtil.FindFirst(ptr, 0, (int)len));
		}

		public static void MemcpyBackptr(IntPtr dst, int back, int count) => Functions.av_memcpy_backptr(dst, back, count);

		public static ManagedPointer<byte> FastRealloc(IntPtr ptr, out uint size, nuint minSize) {
			ptr = Functions.av_fast_realloc(ptr, out size, minSize);
			return new ManagedPointer<byte>(ptr, new(Functions.av_free), (int)size);
		}

		public static ManagedPointer<byte> FastMalloc(IntPtr ptr, out uint size, nuint minSize) {
			unsafe {
				Functions.av_fast_malloc((IntPtr)(&ptr), out size, minSize);
			}
			return new ManagedPointer<byte>(ptr, new(Functions.av_free), (int)size);
		}

	}

}
