using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LibAV {
	
	// --==[ libavutil ]==--
	// samplefmt.h

	public enum AVSampleFormat : int {
		None = -1,
		U8,
		S16,
		S32,
		Float,
		Double,
		U8Planar,
		S16Planar,
		S32Planar,
		FloatPlanar,
		DoublePlanar
	}

	// avutil.h

	public enum AVMediaType : int {
		Unknown = -1,
		Video,
		Audio,
		Data,
		Subtitle,
		Attachment
	}

	public enum AVPictureType : int {
		I = 1,
		P,
		B,
		S,
		SI,
		SP,
		BI
	}

	// error.h

	public enum AVError : int {
		None = 0,
		BitstreamFilterNotFOund = -0x39ACBD08,
		DecoderNotFound = -0x3CBABB08,
		DemuxerNotFound = -0x32BABB08,
		EncoderNotFound = -0x3CB1BA08,
		EndOfFile = -0x5FB9B0BB,
		Exit = -0x2BB5A7BB,
		FilterNotFound = -0x33B6B908,
		InvalidData = -0x3EBBB1B7,
		MuxerNotFOund = -0x27AAB208,
		OptionNotFound = -0x2BAFB008,
		PatchWelcome = -0x3AA8BEB0,
		ProtocolNotFound = -0x30ADAF08,
		StreamNotFound = -0x2DABAC08,
		Bug = -0x5FB8AABE,
		Unknown = -0x31B4B1AB,
		Experimental = -0x2BB2AFA8,
		InputChanged = -0x636E6701,
		OutputChanged = -0x636E6702
	}

	// buffer.h

	[Flags]
	public enum AVBufferFlags : uint {
		Readonly = 1
	}


	// cpu.h

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Flag bits are overlapped for different processor architectures")]
	[Flags]
	public enum AVCPUFlags : uint {
		Force = 0x80000000,

		MMX = 0x0001,
		MMXExt = 0x0002,
		_3DNow = 0x0004,
		SSE = 0x0008,
		SSE2 = 0x0010,
		SSE2Slow = 0x40000000,
		_3DNowExt = 0x0020,
		SSE3 = 0x0040,
		SSE3Slow = 0x20000000,
		Atom = 0x10000000,
		SSE4 = 0x0100,
		SSE42 = 0x0200,
		AVX = 0x4000,
		AVXSlow = 0x8000000,
		XOp = 0x0400,
		FMA4 = 0x0800,
		CMov = 0x1000,
		AVX2 = 0x8000,
		FMA3 = 0x10000,
		BMI1 = 0x20000,
		BMI2 = 0x40000,

		AltiVec = 0x0001,
		VSX = 0x0002,
		Power8 = 0x0004,

		ARMV5TE = 1 << 0,
		ARMV6 = 1 << 1,
		ARMV6T2 = 1 << 2,
		VFP = 1 << 3,
		VFPV3 = 1 << 4,
		Neon = 1 << 5,
		ARMV8 = 1 << 6,
		VFPVM = 1 << 7
	}

	// dict.h

	[Flags]
	public enum AVDictionaryFlags : uint {
		MatchCase = 1,
		IgnoreSuffix = 2,
		DontStrdupKey = 4,
		DontStrdupVal = 8,
		DontOverwrite = 16,
		Append = 32
	}

	// frame.h

	public enum AVFrameSideDataType : int {
		PanScan,
		A53CC,
		Stereo3D,
		MatrixEncoding,
		DownmixInfo,
		ReplayGain,
		DisplayMatrix,
		AFD,
		AudioServiceType,
		Spherical
	}

	public enum AVActiveFormatDescription : int {
		Same = 8,
		_4_3 = 9,
		_16_9 = 10,
		_13_9 = 11,
		_4_3_SP_14_9 = 13,
		_16_9_SP_14_9 = 14,
		SP_4_3 = 15
	}

	[Flags]
	public enum AVFrameFlags : uint {
		Corrupt = 1 << 0
	}

	// pixfmt.h

	public enum AVPixelFormat : int {
		None = -1,
		YUV420P,
		YUV422,
		RGB24,
		BGR24,
		YUV422P,
		YUV444P,
		YUV410P,
		YUV411P,
		Gray8,
		MonoWhite,
		MonoBlack,
		PAL8,
		YUVJ420P,
		YUVJ422P,
		YUVJ444P,
		UYVY422,
		UYYVYY411,
		BGR8,
		BGR4,
		BGR4Byte,
		RGB8,
		RGB4,
		RGB4Byte,
		NV12,
		NV21,

		ARGB,
		RGBA,
		ABGR,
		BGRA,

		Gray16BE,
		Gray16LE,
		YUV440P,
		YUVJ440P,
		YUVA420P,
		RGB48BE,
		RGB48LE,

		RGB565BE,
		RGB565LE,
		RGB555BE,
		RGB555LE,

		BGR565BE,
		BGR565LE,
		BGR555BE,
		BGR555LE,

#if LIBAV_VERSION_56
		VAAPI_Moco,
		VAAPI_IDCT,
		VAAPI_VLD,
		VAAPI = VAAPI_VLD,
#else
		VAAPI,
#endif

		YUV420P16LE,
		YUV420P16BE,
		YUV422P16LE,
		YUV422P16BE,
		YUV444P16LE,
		YUV444P16BE,
		DXVA2_VLD,

		RGB444LE,
		RGB444BE,
		BGR444LE,
		BGR444BE,
		YA8,

		Y400A = YA8,

		BGR48BE,
		BGR48LE,
		YUV420P9BE,
		YUV420P9LE,
		YUV420P10BE,
		YUV420P10LE,
		YUV422P10BE,
		YUV422P10LE,
		YUV444P9BE,
		YUV444P9LE,
		YUV444P10BE,
		YUV444P10LE,
		YUV422P9BE,
		YUV422P9LE,
		VDA_VLD,
		GBRP,
		GBRP9BE,
		GBRP9LE,
		GBRP10BE,
		GBRP10LE,
		GBRP16BE,
		GBRP16LE,
		YUVA422P,
		YUVA444P,
		YUVA420P9BE,
		YUVA420P9LE,
		YUVA422P9BE,
		YUVA422P9LE,
		YUVA444P9BE,
		YUVA444P9LE,
		YUVA420P10BE,
		YUVA420P10LE,
		YUVA422P10BE,
		YUVA422P10LE,
		YUVA444P10BE,
		YUVA444P10LE,
		YUVA420P16BE,
		YUVA420P16LE,
		YUVA422P16BE,
		YUVA422P16LE,
		YUVA444P16BE,
		YUVA444P16LE,
		VDPAU,
		XYZ12LE,
		XYZ12BE,
		NV16,
		NV20LE,
		NV20BE,

		RGBA64BE,
		RGBA64LE,
		BGRA64BE,
		BGRA64LE,

		YVYU422,

		VDA,

		YA16BE,
		YA16LE,

		GBRAP,
		GBRAP16BE,
		GBRAP16LE,

		QSV,

		MMAL,

		D3D11VA_VLD,

		Cuda,

		P010LE,
		P010BE,

		YUV420P12BE,
		YUV420P12LE,

		YUV422P12BE,
		YUV422P12LE,

		YUV444P12BE,
		YUV444P12LE,

		GBRP12BE,
		GBRP12LE,

		GBRAP12BE,
		GBRAP12LE,

		Gray12BE,
		Gray12LE,

		GBRAP10BE,
		GBRAP10LE,

		D3D11,

		Gray10BE,
		Gray10LE
	}

	public static class AVPixFmt {

		public static readonly AVPixelFormat RGB32;
		public static readonly AVPixelFormat BGR32;

		public static readonly AVPixelFormat Gray10;
		public static readonly AVPixelFormat Gray12;
		public static readonly AVPixelFormat Gray16;
		public static readonly AVPixelFormat YA16;
		public static readonly AVPixelFormat RGB48;
		public static readonly AVPixelFormat RGB565;
		public static readonly AVPixelFormat RGB555;
		public static readonly AVPixelFormat RGB444;
		public static readonly AVPixelFormat RGBA64;
		public static readonly AVPixelFormat BGR48;
		public static readonly AVPixelFormat BGR565;
		public static readonly AVPixelFormat BGR555;
		public static readonly AVPixelFormat BGR444;
		public static readonly AVPixelFormat BGRA64;

		public static readonly AVPixelFormat YUV420P9;
		public static readonly AVPixelFormat YUV422P9;
		public static readonly AVPixelFormat YUV444P9;
		public static readonly AVPixelFormat YUV420P10;
		public static readonly AVPixelFormat YUV422P10;
		public static readonly AVPixelFormat YUV444P10;
		public static readonly AVPixelFormat YUV420P12;
		public static readonly AVPixelFormat YUV422P12;
		public static readonly AVPixelFormat YUV444P12;
		public static readonly AVPixelFormat YUV420P16;
		public static readonly AVPixelFormat YUV422P16;
		public static readonly AVPixelFormat YUV444P16;

		public static readonly AVPixelFormat GBRP9;
		public static readonly AVPixelFormat GBRP10;
		public static readonly AVPixelFormat GBRP12;
		public static readonly AVPixelFormat GBRP16;

		public static readonly AVPixelFormat GBRAP10;
		public static readonly AVPixelFormat GBRAP12;
		public static readonly AVPixelFormat GBRAP16;

		public static readonly AVPixelFormat YUVA420P9;
		public static readonly AVPixelFormat YUVA422P9;
		public static readonly AVPixelFormat YUVA444P9;
		public static readonly AVPixelFormat YUVA420P10;
		public static readonly AVPixelFormat YUVA422P10;
		public static readonly AVPixelFormat YUVA444P10;
		public static readonly AVPixelFormat YUVA420P16;
		public static readonly AVPixelFormat YUVA422P16;
		public static readonly AVPixelFormat YUVA444P16;

		public static readonly AVPixelFormat XYZ12;
		public static readonly AVPixelFormat NV20;
		public static readonly AVPixelFormat P010;

		static AVPixFmt() {
			bool be = !BitConverter.IsLittleEndian;
			RGB32 = be ? AVPixelFormat.ARGB : AVPixelFormat.BGRA;
			BGR32 = be ? AVPixelFormat.ABGR : AVPixelFormat.RGBA;

			Gray10 = be ? AVPixelFormat.Gray10BE : AVPixelFormat.Gray10LE;
			Gray12 = be ? AVPixelFormat.Gray12BE : AVPixelFormat.Gray12LE;
			Gray16 = be ? AVPixelFormat.Gray16BE : AVPixelFormat.Gray16LE;
			YA16 = be ? AVPixelFormat.YA16BE : AVPixelFormat.YA16LE;
			RGB48 = be ? AVPixelFormat.RGB48BE : AVPixelFormat.RGB48LE;
			RGB565 = be ? AVPixelFormat.RGB565BE : AVPixelFormat.RGB565LE;
			RGB555 = be ? AVPixelFormat.RGB555BE : AVPixelFormat.RGB555LE;
			RGB444 = be ? AVPixelFormat.RGB444BE : AVPixelFormat.RGB444LE;
			RGBA64 = be ? AVPixelFormat.RGBA64BE : AVPixelFormat.RGBA64LE;
			BGR48 = be ? AVPixelFormat.BGR48BE : AVPixelFormat.BGR48LE;
			BGR565 = be ? AVPixelFormat.BGR565BE : AVPixelFormat.BGR565LE;
			BGR555 = be ? AVPixelFormat.BGR555BE : AVPixelFormat.BGR555LE;
			BGR444 = be ? AVPixelFormat.BGR444BE : AVPixelFormat.BGR444LE;
			BGRA64 = be ? AVPixelFormat.BGRA64BE : AVPixelFormat.BGRA64LE;

			YUV420P9 = be ? AVPixelFormat.YUV420P9BE : AVPixelFormat.YUV420P9LE;
			YUV422P9 = be ? AVPixelFormat.YUV422P9BE : AVPixelFormat.YUV422P9LE;
			YUV444P9 = be ? AVPixelFormat.YUV444P9BE : AVPixelFormat.YUV444P9LE;
			YUV420P10 = be ? AVPixelFormat.YUV420P10BE : AVPixelFormat.YUV420P10LE;
			YUV422P10 = be ? AVPixelFormat.YUV422P10BE : AVPixelFormat.YUV422P10LE;
			YUV444P10 = be ? AVPixelFormat.YUV444P10BE : AVPixelFormat.YUV444P10LE;
			YUV420P12 = be ? AVPixelFormat.YUV420P12BE : AVPixelFormat.YUV420P12LE;
			YUV422P12 = be ? AVPixelFormat.YUV422P12BE : AVPixelFormat.YUV422P12LE;
			YUV444P12 = be ? AVPixelFormat.YUV444P12BE : AVPixelFormat.YUV444P12LE;
			YUV420P16 = be ? AVPixelFormat.YUV420P16BE : AVPixelFormat.YUV420P16LE;
			YUV422P16 = be ? AVPixelFormat.YUV422P16BE : AVPixelFormat.YUV422P16LE;
			YUV444P16 = be ? AVPixelFormat.YUV444P16BE : AVPixelFormat.YUV444P16LE;

			GBRP9 = be ? AVPixelFormat.GBRP9BE : AVPixelFormat.GBRP9LE;
			GBRP10 = be ? AVPixelFormat.GBRP10BE : AVPixelFormat.GBRP10LE;
			GBRP12 = be ? AVPixelFormat.GBRP12BE : AVPixelFormat.GBRP12LE;
			GBRP16 = be ? AVPixelFormat.GBRP16BE : AVPixelFormat.GBRP16LE;

			GBRAP10 = be ? AVPixelFormat.GBRAP10BE : AVPixelFormat.GBRAP10LE;
			GBRAP12 = be ? AVPixelFormat.GBRAP12BE : AVPixelFormat.GBRAP12LE;
			GBRAP16 = be ? AVPixelFormat.GBRAP16BE : AVPixelFormat.GBRAP16LE;

			YUVA420P9 = be ? AVPixelFormat.YUVA420P9BE : AVPixelFormat.YUVA420P9LE;
			YUVA422P9 = be ? AVPixelFormat.YUVA422P9BE : AVPixelFormat.YUVA422P9LE;
			YUVA444P9 = be ? AVPixelFormat.YUVA444P9BE : AVPixelFormat.YUVA444P9LE;
			YUVA420P10 = be ? AVPixelFormat.YUVA420P10BE : AVPixelFormat.YUVA420P10LE;
			YUVA422P10 = be ? AVPixelFormat.YUVA422P10BE : AVPixelFormat.YUVA422P10LE;
			YUVA444P10 = be ? AVPixelFormat.YUVA444P10BE : AVPixelFormat.YUVA444P10LE;
			YUVA420P16 = be ? AVPixelFormat.YUVA420P16BE : AVPixelFormat.YUVA420P16LE;
			YUVA422P16 = be ? AVPixelFormat.YUVA422P16BE : AVPixelFormat.YUVA422P16LE;
			YUVA444P16 = be ? AVPixelFormat.YUVA444P16BE : AVPixelFormat.YUVA444P16LE;

			XYZ12 = be ? AVPixelFormat.XYZ12BE : AVPixelFormat.XYZ12LE;
			NV20 = be ? AVPixelFormat.NV20BE : AVPixelFormat.NV20LE;
			P010 = be ? AVPixelFormat.P010BE : AVPixelFormat.P010LE;
		}

	}

	public enum AVColorPrimaries : int {
		Reserved0 = 0,
		BT709,
		Unspecified,
		Reserved,
		BT470M,
		BT470BG,
		SMPTE170M,
		SMPTE240M,
		Film,
		BT2020,
		SMPTE428,
		SMPTEST428_1 = SMPTE428,
		SMPTE431,
		SMPTE432,
		JEDEC_P22
	}

	public enum AVColorTransferCharacteristic : int {
		Reserved0 = 0,
		BT709,
		Unspecified,
		Reserved,
		Gamma22,
		Gamma28,
		SMPTE170M,
		SMPTE240M,
		Linear,
		Log,
		LogSqrt,
		IEC61966_2_4,
		BT1361_ECG,
		IEC61966_2_1,
		BT2020_10,
		BT2020_12,
		SMPTE2084,
		SMPTEST2084 = SMPTE2084,
		SMPTE428,
		SMPTEST428 = SMPTE428,
		ARIB_STD_B67
	}

	public enum AVColorSpace : int {
		RGB = 0,
		BT709,
		Unspecified,
		Reserved,
		FCC,
		BT470BG,
		SMPTE170M,
		SMPTE240M,
		YCGCO,
		YCOCG = YCGCO,
		BT2020NCL,
		BT2020CL,
		SMPTE2085,
		ChromaDerivedNCL,
		ChromaDerivedCL,
		ICTCP
	}

	public enum AVColorRange : int {
		Unspecified = 0,
		MPEG,
		JPEG
	}

	public enum AVChromaLocation : int {
		Unspecified = 0,
		Left,
		Center,
		TopLeft,
		Top,
		BottomLeft,
		Bottom
	}

	// frame.h

	[Flags]
	public enum AVFrameCropFlags : uint {
		Unaligned = 1 << 0
	}

	// log.h

	public enum AVLogLevel : int {
		Quiet = -8,
		Panic = 0,
		Fatal = 8,
		Error = 16,
		Warning = 24,
		Info = 32,
		Verbose = 40,
		Debug = 48,
		Trace = 56
	}

	[Flags]
	public enum AVLogFlags : uint {
		SkipRepeated = 1
	}

	// hwcontext.h

	public enum AVHWDeviceType : int {
		None,
		VDPAU,
		CUDA,
		VAAPI,
		DXVA2,
		QSV,
		D3D11VA
	}

	public enum AVHWFrameTransferDirection : int {
		From,
		To
	}

	[Flags]
	public enum AVHWFrameMapFlags : uint {
		Read = 1 << 0,
		Write = 1 << 1,
		Overwrite = 1 << 2,
		Direct = 1 << 3
	}

	// mathematics.h

	public enum AVRounding : int {
		Zero = 0,
		Inf,
		Down,
		Up,
		NearInf
	}

}
