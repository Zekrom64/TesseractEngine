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

	// channel_layout.h

	[Flags]
	public enum AVChannelMask : ulong {
		FrontLeft =      0x00000001,
		FrontRight =     0x00000002,
		FrontCenter =    0x00000004,
		LowFrequency =   0x00000008,
		BackLeft =       0x00000010,
		BackRight =      0x00000020,
		FrontLeftOfCenter = 0x00000040,
		FrontRightOfCenter = 0x00000080,
		BackCenter =     0x00000100,
		SideLeft =       0x00000200,
		SideRight =      0x00000400,
		TopLeft =        0x00000800,
		TopFrontLeft =   0x00001000,
		TopFrontCenter = 0x00002000,
		TopFrontRight =  0x00004000,
		TopBackLeft =    0x00008000,
		TopBackCenter =  0x00010000,
		TopBackRight =   0x00020000,
		StereoLeft =     0x20000000,
		StereoRight =    0x40000000,
		WideLeft =       0x0000000080000000,
		WideRight =      0x0000000100000000,
		SurroundDirectLeft = 0x0000000200000000,
		SurroundDirectRight = 0x0000000400000000,
		LowFrequency2 =  0x0000000800000000,
		Native =         0x8000000000000000,

		Mono = FrontCenter,
		Stereo = FrontLeft | FrontRight,
		_2Point1 = Stereo | LowFrequency,
		_2_1 = Stereo | BackCenter,
		Surround = Stereo | FrontCenter,
		_3Point1 = Surround | LowFrequency,
		_4Point0 = Surround | BackCenter,
		_4Point1 = _4Point0 | LowFrequency,
		_2_2 = Stereo | SideLeft | SideRight,
		Quad = Stereo | BackLeft | BackRight,
		_5Point0 = Surround | SideLeft | SideRight,
		_5Point1 = _5Point0 | LowFrequency,
		_5Point0Back = Surround | BackLeft | BackRight,
		_5Point1Back = _5Point0Back | LowFrequency,
		_6Point0 = _5Point0 | BackCenter,
		_6Point0Front = _2_2 | FrontLeftOfCenter | FrontRightOfCenter,
		Hexagonal = _5Point0Back | BackCenter,
		_6Point1 = _5Point1 | BackCenter,
		_6Point1Back = _5Point1Back | BackCenter,
		_6Point1Front = _6Point0Front | LowFrequency,
		_7Point0 = _5Point0 | BackLeft | BackRight,
		_7Point0Front = _5Point0 | FrontLeftOfCenter | FrontRightOfCenter,
		_7Point1 = _5Point1 | BackLeft | BackRight,
		_7Point1Wide = _5Point1 | FrontLeftOfCenter | FrontRightOfCenter,
		_7Point1WideBack = _5Point1Back | FrontLeftOfCenter | FrontRightOfCenter,
		Octagonal = _5Point0 | BackLeft | BackCenter | BackRight,
		Hexadecagonal = Octagonal | WideLeft | WideRight | TopBackLeft | TopBackRight | TopBackCenter | TopFrontLeft | TopFrontRight | TopFrontCenter,
		StereoDownmix = StereoLeft | StereoRight
	}

	public enum AVMatrixEncoding : int {
		None,
		Dolby,
		DLPII,
		DLPIIX,
		DLPIIZ,
		DolbyEx,
		DolbyHeadphone
	}

	// --==[ libavcodec ]==--
	// avcodec.h

	public enum AVCodecID : int {
		None,
		MPEG1Video,
		MPEG2Video,
		H261,
		H263,
		RV10,
		RV20,
		MJPEG,
		MJPEGB,
		LJPEG,
		SP5X,
		JPEGLS,
		MPEG4,
		RawVideo,
		MSMPEG4V1,
		MSMPEG4V2,
		MSMPEG4V3,
		WMV1,
		WMV2,
		H263P,
		H263I,
		FLV1,
		SVQ1,
		SVQ3,
		DVVideo,
		HuffYUV,
		CYUV,
		H264,
		Indeo3,
		VP3,
		Theora,
		ASV1,
		ASV2,
		FFV1,
		_4XM,
		VCR1,
		CLJR,
		MDEC,
		ROQ,
		InterplayVideo,
		XAN_WC3,
		XAN_WC4,
		RPZA,
		Cinepak,
		WS_VQA,
		MSRLE,
		MSVideo1,
		IDCIN,
		_8BPS,
		SMC,
		FLIC,
		TrueMotion1,
		VMDVideo,
		MSZH,
		ZLib,
		QTRLE,
		TSCC,
		ULTI,
		QDRAW,
		VIXL,
		QPEG,
		PNG,
		PPM,
		PBM,
		PGM,
		PGMYUV,
		PAM,
		FFVHUFF,
		RV30,
		RV40,
		VC1,
		WMV3,
		LOCO,
		WNV1,
		AASC,
		Indeo2,
		Fraps,
		TrueMotion2,
		BMP,
		CSCD,
		MMVideo,
		ZMBV,
		AVS,
		SmackVideo,
		NUV,
		KMVC,
		FlashSV,
		CAVS,
		JPEG2000,
		VMNC,
		VP5,
		VP6,
		VP6F,
		TARGA,
		DSICINVideo,
		TIERTEXSEQVidoe,
		TIFF,
		GIF,
		DXA,
		DNXHD,
		THP,
		SGI,
		C93,
		BethSoftVid,
		PTX,
		TXD,
		VP6A,
		AMV,
		VB,
		PCX,
		SunRast,
		Indeo4,
		Indeo5,
		Mimic,
		RL2,
		Escape124,
		DIRAC,
		BFI,
		CMV,
		MotionPixels,
		TGV,
		TGQ,
		TQI,
		Aura,
		Aura2,
		V210X,
		TMV,
		V210,
		DPX,
		MAD,
		FRWU,
		FlashSV2,
		CDGraphics,
		R210,
		ANM,
		BinkVideo,
		IFF_ILBM,
		IFF_ByteRun1,
		KGV1,
		YOP,
		VP8,
		Pictor,
		ANSI,
		A64_Multi,
		A64_Multi5,
		R10K,
		MXPEG,
		Lagarith,
		ProRes,
		JV,
		DFA,
		WMV3Image,
		VC1Image,
		UTVideo,
		BMVVideo,
		VBLE,
		DXTORY,
		V410,
		XWD,
		CDXL,
		XBM,
		ZeroCodec,
		MSS1,
		MSA1,
		TSCC2,
		MTS2,
		CLLC,
		MSS2,
		VP9,
		AIC,
		Escape130,
		G2M,
		WebP,
		HNM4Video,
		HEVC,
		FIC,
		AliasPix,
		BrenderPix,
		PAFVideo,
		EXR,
		VP7,
		SANM,
		SGIRLE,
		MVC1,
		MVC2,
		HQX,
		TDSC,
		HQ_HQA,
		HAP,
		DDS,
		DXV,
		ScreenPresso,
		RSCC,
		MagicYUV,
		TrueMotion2RT,
		AV1,
		Pixlet,
		CFHD,
		FMVC,
		ClearVideo,

		PCM_S16LE = 0x10000,
		PCM_S16BE,
		PCM_U16LE,
		PCM_U16BE,
		PCM_S8,
		PCM_U8,
		PCM_MuLaw,
		PCM_ALaw,
		PCM_S32LE,
		PCM_S32BE,
		PCM_U32LE,
		PCM_U32BE,
		PCM_S24LE,
		PCM_S24BE,
		PCM_U24LE,
		PCM_U24BE,
		PCM_S24DAUD,
		PCM_Zork,
		PCM_S16LE_Planar,
		PCM_DVD,
		PCM_F32BE,
		PCM_F32LE,
		PCM_F64BE,
		PCM_F64LE,
		PCM_BluRay,
		PCM_LXF,
		S302M,
		PCM_S8_Planar,
		PCM_S24LE_Planar,
		PCM_S32LE_Planar,
		PCM_S16BE_Planar,

		ADPCM_IMA_QT = 0x11000,
		ADPCM_IMA_WAV,
		ADPCM_IMA_DK3,
		ADPCM_IMA_DK4,
		ADPCM_IMA_WS,
		ADPCM_IMA_SMJPEG,
		ADPCM_MS,
		ADPCM_4XM,
		ADPCM_XA,
		ADPCM_ADX,
		ADPCM_EA,
		ADPCM_G726,
		ADPCM_CT,
		ADPCM_SWF,
		ADPCM_Yamaha,
		ADPCM_SBPro4,
		ADPCM_SBPro3,
		ADPCM_SBPro2,
		ADPCM_THP,
		ADPCM_IMA_AMV,
		ADPCM_EA_R1,
		ADPCM_EA_R3,
		ADPCM_EA_R2,
		ADPCM_IMA_EA_SEAD,
		ADPCM_IMA_EA_EACS,
		ADPCM_EA_XS,
		ADPCM_EA_MaxisXA,
		ADPCM_IMA_ISS,
		ADPCM_G722,
		ADPCM_IMA_APC,
		ADPCM_VIMA,

		AMR_NB = 0x12000,
		AMR_WB,

		RA144 = 0x13000,
		RA288,

		ROQ_DPCM = 0x14000,
		InterplayDPCM,
		XAN_DPCM,
		SOL_DPCM,

		MP2 = 0x15000,
		MP3,
		AAC,
		AC3,
		DTS,
		Vorbis,
		DVAudio,
		WMAV1,
		WMAV2,
		MACE3,
		MACE6,
		VMDAudio,
		FLAC,
		MP3ADU,
		MP3ON4,
		Shorten,
		ALAC,
		WestwoodSND1,
		GSM,
		QDM2,
		COOK,
		TrueSpeech,
		TTA,
		SmackAudio,
		QCELP,
		WAVPack,
		DSICINAudio,
		IMC,
		MusePack7,
		MLP,
		GSM_MS,
		ATRAC3,
		APE,
		NellyMoser,
		MusePack8,
		Speex,
		WMAVoice,
		WMAPro,
		WMALossless,
		ATRAC3P,
		EAC3,
		SIPR,
		MP1,
		TWINVQ,
		TrueHD,
		MP4ALS,
		ATRAC1,
		BinkAudioRDFT,
		BinkAudioDCT,
		AAC_LATM,
		QDMC,
		CELT,
		G723_1,
		G729,
		_8SVX_EXP,
		_8SVX_FIB,
		BMVAudio,
		RALF,
		IAC,
		ILBC,
		Opus,
		ComfortNoise,
		TAK,
		MetaSound,
		PAFAudio,
		ON2AVC,
		DSS_SP,

		DVDSubtitle = 0x17000,
		DVBSubtitle,
		Text,
		XSub,
		SSA,
		MOVText,
		HDMV_PGSSubtitle,
		DVBTeletext,
		SRT,

		TTF = 0x18000,

		Probe = 0x19000,

		MPEG2TS = 0x20000,
		MPEG4Systems = 0x20001,

		FFMetadata = 0x21000,
		WrappedAVFrame = 0x21001
	}

	[Flags]
	public enum AVCodecPropFlags : uint {
		IntraOnly = 1 << 0,
		Lossy = 1 << 1,
		Lossless = 1 << 2,
		Reorder = 1 << 3
	}

	public enum AVDiscard : int {
		None = -16,
		Default = 0,
		NonRef = 8,
		BiDir = 16,
		NonKey = 32,
		All = 48
	}

	public enum AVAudioServiceType {
		Main = 0,
		Effects,
		VisuallyImpaired,
		HearingImpaired,
		Dialogue,
		Commentary,
		Emergency,
		VoiceOver,
		Karaoke
	}

	[Flags]
	public enum AVCodecFlags : uint {
		Unaligned = 1 << 0,
		QScale = 1 << 1,
		_4MV = 1 << 2,
		OutputCorrupt = 1 << 3,
		QPEL = 1 << 4,
		Pass1 = 1 << 9,
		Pass2 = 1 << 10,
		LoopFilter = 1 << 11,
		Gray = 1 << 13,
		PSNR = 1 << 15,
		Truncated = 1 << 16,
		InterlacedDCT = 1 << 18,
		LowDelay = 1 << 19,
		GlobalHeader = 1 << 22,
		BitExact = 1 << 23,
		ACPred = 1 << 24,
		InterlacedME = 1 << 29,
		ClosedGOP = unchecked((uint)(1 << 31))
	}

	[Flags]
	public enum AVCodecFlags2 : uint {
		Fast = 1 << 0,
		NoOutput = 1 << 2,
		LocalHeader = 1 << 3,
		Chunks = 1 << 15,
		IgnoreCrop = 1 << 16
	}

	[Flags]
	public enum AVCodecCapabilities : uint {
		DrawHorizBand = 1 << 0,
		DR1 = 1 << 1,
		Truncated = 1 << 3,
		Delay = 1 << 5,
		SmallLastFrame = 1 << 6,
		Subframes = 1 << 8,
		Experimental = 1 << 9,
		ChannelConf = 1 << 10,
		FrameThreads = 1 << 12,
		SliceThreads = 1 << 13,
		ParamChange = 1 << 14,
		AutoThreads = 1 << 15,
		VariableFrameSize = 1 << 16,
		Hardware = 1 << 17,
		Hybrid = 1 << 18,
		EncoderReorderedOpaque = 1 << 19
	}

	public enum AVPacketSideDataType : int {
		Palette,
		NewExtraData,
		ParamChange,
		H263MBInfo,
		ReplayGain,
		DisplayMatrix,
		Stereo3D,
		AudioServiceType,
		QualityFactor,
		FallbackTrack,
		CPBProperties,
		Spherical
	}

	[Flags]
	public enum AVPacketFlags : uint {
		Key = 0x0001,
		Corrupt = 0x0002
	}

	[Flags]
	public enum AVSideDataParamChangeFlags : uint {
		ChannelCount = 0x0001,
		ChannelLayout = 0x0002,
		SampleRate = 0x0004,
		Dimensions = 0x0008,
	}

	public enum AVFieldOrder : int {
		Unknown,
		Progressive,
		TT,
		BB,
		TB,
		BT
	}

	[Obsolete("Deprecated in libavcodec >= 59")]
	public enum AVPredictionMethod : int {
		Left,
		Plane,
		Median
	}

	public enum AVCompFunction : int {
		SAD = 0,
		SSE,
		SATD,
		DCT,
		PSNR,
		BIT,
		RD,
		Zero,
		VSAD,
		VSSE,
		NSSE,
		DCTMax,
		DCT264,
		Chroma
	}

	[Flags]
	public enum AVSliceFlags : uint {
		CodedOrder = 0x0001,
		AllowField = 0x0002,
		AllowPlane = 0x0004
	}

	public enum AVMacroblockDecisionMode : int {
		Simple = 0,
		Bits = 1,
		RD = 2
	}

	public enum AVCoderType : int {
		VLC = 0,
		AC,
		Raw,
		RLE
	}

	[Flags]
	public enum AVWorkaroundBugs : uint {
		AutoDetect = 1,
		XVIDInterlace = 4,
		UMP4 = 8,
		NoPadding = 16,
		AMV = 32,
		QPelChroma = 64,
		StdQPel = 128,
		QPelChroma2 = 256,
		DirectBlocksize = 512,
		Edge = 1024,
		HPelChroma = 2048,
		DCClip = 4096,
		Microsoft = 8192,
		Truncated = 16384
	}

	public enum AVStandardCompliance : int {
		VeryStrict = 2,
		Strict = 1,
		Normal = 0,
		Unofficial = -1,
		Experimental = -2
	}

	public enum AVErrorConcealment : int {
		GuessMVS = 1,
		DEBlock = 2
	}

	[Flags]
	public enum AVDebugFlags : uint {
		PictInfo = 1,
		RC = 2,
		Bitstream = 4,
		MBType = 8,
		QP = 16,
		DCTCoeff = 0x40,
		Skip = 0x80,
		StartCode = 0x100,
		ER = 0x400,
		MMCO = 0x800,
		Bugs = 0x1000,
		Buffers = 0x8000,
		Threads = 0x10000
	}

	[Flags]
	public enum AVErrorRecognitionFlags : uint {
		CRCCheck = 1 << 0,
		Bitstream = 1 << 1,
		Buffer = 1 << 2,
		Explode = 1 << 3
	}

	public enum AVDCTAlgorithm : int {
		Auto = 0,
		FastInt,
		Int,
		MMX,
		AltiVec = 5,
		FAAN
	}

	public enum AVIDCTAlgorithm : int {
		Auto = 0,
		Int,
		Simple,
		SimpleMMX,
		ARM = 7,
		AltiVec,
		SimpleARM = 10,
		XVid = 14,
		SimpleARMV5TE = 16,
		SimpleARMV6,
		FAAN = 20,
		SimpleNeon = 22
	}

	public enum AVThreadType : int {
		Frame = 1,
		Slice = 2
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Matches enum values in header")]
	public enum AVCodecProfile : int {
		Unknown = -99,
		Reserved = -100,

		AACMain = 0,
		AACLow = 1,
		AAC_SSR = 2,
		AAC_LTP = 3,
		AAC_HE = 4,
		AAC_HE_V2 = 28,
		AAC_LD = 22,
		AAC_ELD = 38,
		MPEG2_AACLow = 128,
		MPEG2_AAC_HE = 131,

		DTS = 20,
		DTS_ES = 30,
		DTS_96_24 = 40,
		DTS_HD_HRA = 50,
		DTS_HD_MA = 60,
		DTSExpress = 70,

		MPEG2_422 = 0,
		MPEG2High = 1,
		MPEG2SS = 2,
		MPEG2SNRScalable = 3,
		MPEG2Main = 4,
		MPEG2Simple = 5,

		H264Constrained = 1 << 9,
		H264Intra = 1 << 11,

		H264Baseline = 66,
		H264ConstrainedBaseline = H264Baseline | H264Constrained,
		H264Main = 77,
		H264Extended = 88,
		H264High = 100,
		H264High10 = 110,
		H264High10Intra = H264High10 | H264Intra,
		H264MultiviewHigh = 118,
		H264High422 = 122,
		H264High422Intra = H264High422 | H264Intra,
		H264StereoHigh = 128,
		H264High444 = 144,
		H264High444Predictive = 244,
		H264High444Intra = H264High444Predictive | H264Intra,
		H264CAVLC444 = 44,

		VC1Simple = 0,
		VC1Main = 1,
		VC1Complex = 2,
		VC1Advanced = 3,

		MPEG4Simple = 0,
		MPEG4SimpleScalable = 1,
		MPEG4Core = 2,
		MPEG4Main = 3,
		MPEG4NBit = 4,
		MPEG4ScalableTexture = 5,
		MPEG4SimpleFaceAnimation = 6,
		MPEG4BasicAnimatedTexture = 7,
		MPEG4Hybrid = 8,
		MPEG4AdvancedRealTime = 9,
		MPEG4CoreScalable = 10,
		MPEG4AdvancedCoding = 11,
		MPEG4AdvancedCore = 12,
		MPEG4AdvancedScalableTexture = 13,
		MPEG4SimpleStudio = 14,
		MPEG4AdvancedSimple = 15,

		JPEG2000CStreamRestriction0 = 1,
		JPEG2000CStreamRestriction1 = 2,
		JPEG2000CStreamNoRestriction = 32768,
		JPEG2000DCinema2K = 3,
		JPEG2000DCinema4K = 4,

		VP9_0 = 0,
		VP9_1 = 1,
		VP9_2 = 2,
		VP9_3 = 3,

		HEVCMain = 1,
		HEVCMain10 = 2,
		HEVCMainStillPicture = 3,
		HEVCRExt = 4,

		AV1Main = 0,
		AV1High = 1,
		AV1Professional = 2
	}

	[Flags]
	public enum AVHWAccelFlags : uint {
		IgnoreLevel = 1 << 0,
		AllowHighDepth = 1 << 1,
		AllowProfileMismatch = 1 << 2
	}

	[Flags]
	public enum AVCodecHWConfigMethod : uint {
		HWDeviceCtx = 0x01,
		HWFramesCtx = 0x02,
		Internal = 0x04,
		AdHoc = 0x08
	}

	public enum AVSubtitleType : int {
		None,
		Bitmap,
		Text,
		ASS
	}
	
	[Flags]
	public enum AVSubtitleFlags : uint {
		Forced = 0x1
	}

	public enum AVPictureStructure : int {
		Unknown,
		TopField,
		BottomField,
		Frame
	}

	[Flags]
	public enum AVCodecParserFlags : uint {
		CompleteFrames = 0x0001,
		Once = 0x0002,
		FetchedOffset = 0x0004
	}
	
	[Flags]
	public enum AVLossFlags : int {
		Resolution = 0x0001,
		Depth = 0x0002,
		Colorspace = 0x0004,
		Alpha = 0x0008,
		ColorQuant = 0x0010,
		Chroma = 0x0020
	}

	public enum AVLockOp : int {
		Create,
		Obtain,
		Release,
		Destroy
	}

	// internal.h

	[Flags]
	public enum AVCodecCap : uint {
		InitThreadsafe = 1 << 0,
		InitCleanup = 1 << 1,
		SetsPktDTS = 1 << 2,
		ExportsCropping = 1 << 3
	}

	// --==[ libswscale ]==--
	// swscale.h

	[Flags]
	public enum SWSFlags : uint {
		FastBilinear = 1,
		Bilinear = 2,
		Bicubic = 4,
		X = 8,
		Point = 0x10,
		Area = 0x20,
		Bicublin = 0x40,
		Gauss = 0x80,
		Sinc = 0x100,
		Lanczos = 0x200,
		Spline = 0x400,

		PrintInfo = 0x1000,
		FullChrHInt = 0x2000,
		FullChrHInp = 0x4000,
		DirectBGR = 0x8000,
		AccurateRound = 0x40000,
		BitExact = 0x80000
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Values overlap in defining header")]
	public enum SWSColorspace : int {
		ITU709 = 1,
		FCC = 4,
		ITU601 = 5,
		IUF624 = 5,
		SMPTE170M = 5,
		SMPTE240M = 7,
		Default = 5
	}

	// --==[ libavformat ]==--
	// avio.h

	public enum AVIOSeekableFlags : int {
		Normal = 1,
		Time = 2
	}

	public enum AVIODataMarkerType : int {
		Header,
		SyncPoint,
		BoundaryPoint,
		Unknown,
		Trailer
	}

	public enum AVIOOpenFlags : int {
		Read = 1,
		Write = 2,
		ReadWrite = Read | Write,

		NonBlock = 8
	}

}
