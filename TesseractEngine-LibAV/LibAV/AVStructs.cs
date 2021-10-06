using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	// --==[ libavutil ]==--
	// rational.h

	[StructLayout(LayoutKind.Sequential)]	
	public struct AVRational : IComparable<AVRational> {

		public int Num;

		public int Den;

		public AVRational(int num, int den) {
			Num = num;
			Den = den;
		}

		public static AVRational Reduce(long num, long den, long max) {
			AVRational dst = new();
			LibAVUtil.Functions.av_reduce(out dst.Num, out dst.Den, num, den, max);
			return dst;
		}

		public int CompareTo(AVRational other) {
			long tmp = Num * (long)other.Den - other.Num * (long)Den;
			if (tmp != 0) return (int)((tmp ^ Den ^ other.Den) >> 63) | 1;
			else if (other.Den != 0 && Den != 0) return 0;
			else if (Num != 0 && other.Num != 0) return (Num >> 31) - (other.Num >> 31);
			else return int.MinValue;
		}

		public static implicit operator double(AVRational a) => a.Num / (double)a.Den;

		public static AVRational operator *(AVRational b, AVRational c) => LibAVUtil.Functions.av_mul_q(b, c);

		public static AVRational operator /(AVRational b, AVRational c) => LibAVUtil.Functions.av_div_q(b, c);
		
		public static AVRational operator +(AVRational b, AVRational c) => LibAVUtil.Functions.av_add_q(b, c);

		public static AVRational operator -(AVRational b, AVRational c) => LibAVUtil.Functions.av_sub_q(b, c);

		public AVRational Inverse() => new() { Num = Den, Den = Num };

		public static AVRational FromDouble(double d, int max) => LibAVUtil.Functions.av_d2q(d, max);

		public int Nearer(AVRational q1, AVRational q2) => LibAVUtil.Functions.av_nearer_q(this, q1, q2);

		public int FindNearest(in ReadOnlySpan<AVRational> list) {
			unsafe {
				fixed(AVRational* pList = list) {
					return LibAVUtil.Functions.av_find_nearest_q_idx(this, (IntPtr)pList);
				}
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVBufferRef {

		[NativeType("AVBuffer*")]
		public IntPtr Buffer;

		[NativeType("uint8_t*")]
		public IntPtr Data;

		public int Size;

	};

	// dict.h

	[StructLayout(LayoutKind.Sequential)]
	public struct AVDictionaryEntry {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Key;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Value;

	}

	// frame.h
	
	[StructLayout(LayoutKind.Sequential)]
	public struct AVFrameSideData {

		public AVFrameSideDataType Type;

		[NativeType("uint8_t*")]
		public IntPtr Data;

		public int Size;

		[NativeType("AVDictionary*")]
		public IntPtr Metadata;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVFrame {

		public const int NumDataPointers = 8;

		[NativeType("uint8_t*")]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumDataPointers)]
		public IntPtr[] Data;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumDataPointers)]
		public int[] LineSize;

		[NativeType("uint8_t**")]
		public IntPtr ExtendedData;

		public int Width;

		public int Height;

		public int NbSamples;

		public int Format;

		public bool KeyFrame;

		public AVPictureType PictType;

		public AVRational SampleAspectRatio;

		public long Pts;

#if LIBAV_VERSION_56
		[Obsolete("Deprecated in libav >57")]
		public long PktPts;
#endif

		public long PktDts;

		public int CodedPictureNumber;

		public int DisplayPictureNumber;

		public int Quality;

		public IntPtr Opaque;

#if LIBAV_VERSION_56
		[Obsolete("Deprecated in libav >57")]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumDataPointers)]
		public ulong[] Error;
#endif

		public int RepeatPict;

		public bool InterlacedFrame;

		public bool TopFieldFirst;

		public bool PaletteHasChanged;

		public long ReorderedOpaque;

		public int SampleRate;

		public ulong ChannelLayout;

		[NativeType("AVBufferRef*")]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumDataPointers)]
		public IntPtr[] Buf;

		[NativeType("AVBufferRef**")]
		public IntPtr ExtendedBuf;

		public int NbExtendedBuf;

		[NativeType("AVFrameSideData**")]
		public IntPtr SideData;

		public int NbSideData;

		public AVFrameFlags Flags;

		public AVColorRange ColorRange;

		public AVColorPrimaries ColorPrimaries;

		public AVColorTransferCharacteristic ColorTrc;

		public AVColorSpace ColorSpace;

		public AVChromaLocation ChromaLocation;

		[NativeType("AVBufferRef*")]
		public IntPtr HwFramesCtx;

		public nuint CropTop;

		public nuint CropBottom;

		public nuint CropLeft;

		public nuint CropRight;

		[NativeType("AVBufferRef*")]
		public IntPtr OpaqueRef;

	}

	// log.h

	[StructLayout(LayoutKind.Sequential)]
	public struct AVClass {

		[MarshalAs(UnmanagedType.LPStr)]
		public string ClassName;

		[return: NativeType("const char*")]
		public delegate IntPtr ItemNameFn(IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ItemNameFn ItemName;

		[NativeType("const AVOption*")]
		public IntPtr Option;

		public int Version;

		public int LogLevelOffsetOffset;

		public int ParentLogContextOffset;

		public delegate IntPtr ChildNextFn(IntPtr obj, IntPtr prev);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ChildNextFn ChildNext;

		[return: NativeType("const AVClass*")]
		public delegate IntPtr ChildClassNextFn([NativeType("const AVClass*")] IntPtr prev);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ChildClassNextFn ChildClassNext;

	}

	// hwcontext.h

	[StructLayout(LayoutKind.Sequential)]
	public struct AVHWDeviceContext {

		[NativeType("const AVClass*")]
		public IntPtr AVClass;

		[NativeType("AVHWDeviceInternal*")]
		public IntPtr Internal;

		public AVHWDeviceType Type;

		public IntPtr HWCtx;

		public delegate void FreeFn([NativeType("AVHWDeviceContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FreeFn Free;

		public IntPtr UserOpaque;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVHWFramesContext {

		[NativeType("const AVClass*")]
		public IntPtr AVClass;

		[NativeType("AVHWFramesInternal*")]
		public IntPtr Internal;

		[NativeType("AVBufferRef*")]
		public IntPtr DeviceRef;

		[NativeType("AVHWDeviceContext*")]
		public IntPtr DeviceCtx;

		public IntPtr HWCtx;

		public delegate void FreeFn([NativeType("AVHWFramesContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FreeFn Free;

		public IntPtr UserOpaque;

		[NativeType("AVBufferPool*")]
		public IntPtr Pool;

		public int InitialPoolSize;

		public AVPixelFormat Format;

		public AVPixelFormat SWFormat;

		public int Width;

		public int Height;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVHWFramesConstraints {

		[NativeType("AVPixelFormat*")]
		public IntPtr ValidHWFormats;

		[NativeType("AVPixelFormat*")]
		public IntPtr ValidSWFormats;

		public int MinWidth;

		public int MinHeight;

		public int MaxWidth;

		public int MaxHeight;

	}

	// --==[ libavcodec ]==--
	// avcodec.h

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecDescriptor {

		public AVCodecID ID;

		public AVMediaType Type;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;
		
		[MarshalAs(UnmanagedType.LPStr)]
		public string LongName;

		public AVCodecPropFlags Props;
		
		[NativeType("const AVProfile*")]
		public IntPtr Profiles;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RCOverride {

		public int StartFrame;

		public int EndFrame;

		public int QScale;

		public float QualityFactor;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVPanScan {

		public int ID;

		public int Width;

		public int Height;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3 * 2)]
		public short[] Position;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCPBProperties {

		public int MaxBitrate;

		public int MinBitrate;

		public int AvgBitrate;

		public int BufferSize;

		public ulong VBVDelay;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVPacketSideData {

		[NativeType("uint8_t*")]
		public IntPtr Data;

		public int Size;

		public AVPacketSideDataType Type;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVPacket {

		[NativeType("AVBufferRef*")]
		public IntPtr Buf;

		public long PTS;

		public long DTS;

		[NativeType("uint8_t*")]
		public IntPtr Data;

		public int Size;

		public int StreamIndex;

		public AVPacketFlags Flags;

		[NativeType("AVPacketSideData*")]
		public IntPtr SideData;

		public int SideDataElems;

		public long Duration;

		public long Pos;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public long ConvergenceDuration;
#endif
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecContext {

		[NativeType("const AVClass*")]
		public IntPtr Class;

		public int LogLevelOffset;

		public AVMediaType CodecType;

		[NativeType("const AVCodec*")]
		public IntPtr Codec;

		public AVCodecID CodecID;

		public uint CodecTag;

		public IntPtr PrivData;

		[NativeType("AVCodecInternal*")]
		public IntPtr Internal;

		public IntPtr Opaque;

		public int BitRate;

		public int BitRateTolerance;

		public int GlobalQuality;

		public int CompressionLevel;

		public AVCodecFlags Flags;

		public AVCodecFlags2 Flags2;

		[NativeType("uint8_t*")]
		public IntPtr ExtraData;

		public int ExtraDataSize;

		public AVRational TimeBase;

		public int TicksPerFrame;

		public int Delay;

		public int Width, Height;

		public int CodedWidth, CodedHeight;

		public int GOPSize;

		public AVPixelFormat PixFmt;

		public delegate void DrawHorizBandFn([NativeType("AVCodecContext*")] IntPtr s, [NativeType("const AVFrame*")] IntPtr src, [MarshalAs(UnmanagedType.LPArray, SizeConst = AVFrame.NumDataPointers)] int[] offset, int y, int type, int height);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public DrawHorizBandFn DrawHorizBand;

		public delegate AVPixelFormat GetFormatFn([NativeType("AVCodecContext*")] IntPtr s, [NativeType("const AVPixelFormat*")] IntPtr fmt);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public GetFormatFn GetFormat;

		public int MaxBFrames;

		public float BQuantFactor;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int BFrameStrategy;
#endif

		public float BQuantOffset;

		public int HasBFrames;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MPEGQuant;
#endif

		public float IQuantFactor;

		public float IQuantOffset;

		public float LuminanceMasking;

		public float TemporalComplexMasking;

		public float SpatialComplexMapping;

		public float PMasking;

		public float DarkMasking;

		public int SliceCount;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public AVPredictionMethod predictionMethod;
#endif

		[NativeType("int*")]
		public IntPtr SliceOffset;

		public AVRational SampleAspectRatio;

		public AVCompFunction MotionEstComp;

		public AVCompFunction MotionEstSubpixelComp;

		public AVCompFunction MacroblockComp;

		public AVCompFunction InterlacedDCTComp;

		public int DiamondSize;

		public int LastPredictorCount;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int PreMotionEst;
#endif

		public AVCompFunction MotionEstPreComp;

		public int MotionEstPreDiamondSize;

		public int MotionEstSubpelQuality;

		public int MotionEstRange;

		public AVSliceFlags SliceFlags;

		public AVMacroblockDecisionMode MacroblockDecisionMode;

		[NativeType("uint16_t*")]
		public IntPtr IntraMatrix;

		[NativeType("uint16_t*")]
		public IntPtr InterMatrix;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int ScenechangeThreshold;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int NoiseReduction;
#endif

		public int IntraDCPrecision;

		public int SkipTop;

		public int SkipBottom;

		public int MBLangrangeMinMult;

		public int MBLangrangeMaxMult;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MotionEstPenaltyCompensation;
#endif

		public int BidirRefine;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int BRDScale;
#endif

		public int KeyIntMin;

		public int Refs;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int ChromaOffset;
#endif

		public int MV0Threshold;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int BSensitivity;
#endif

		public AVColorPrimaries ColorPrimaries;

		public AVColorTransferCharacteristic ColorTrc;

		public AVColorSpace ColorSpace;

		public AVColorRange ColorRange;

		public AVChromaLocation ChromaSampleLocation;

		public int Slices;

		public AVFieldOrder FieldOrder;

		public int SampleRate;

		public int Channels;

		public AVSampleFormat SampleFmt;

		public int FrameSize;

		public int FrameNumber;

		public int BlockAlign;

		public int Cutoff;

		public ulong ChannelLayout;

		public ulong RequestChannelLayout;

		public AVAudioServiceType AudioServiceType;

		public AVSampleFormat FrequestSampleFmt;

		public delegate int GetBuffer2Fn([NativeType("AVCodecContext*")] IntPtr s, [NativeType("AVFrame*")] IntPtr frame, int flags);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public GetBuffer2Fn GetBuffer2;

		[Obsolete("Marked as deprecated in libav headers")]
		public int RefcountedFrames;

		public float QCompress;

		public float QBlur;

		public int QMin;

		public int QMax;

		public int MaxQDiff;

		public int RCBufferSize;

		public int RCOverrideCount;

		[NativeType("RcOverride*")]
		public IntPtr RCOverride;

		public int RCMaxRate;

		public int RCMinRate;

		public float RCMaxAvailableVBVUse;

		public float RCMinVBVOverflowUse;

		public int RCInitialBufferCapacity;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public AVCoderType CoderType;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int ContextModel;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int FrameSkipThreshold;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int FrameSkipFactor;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int FrameSkipExp;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int FrameSkipCmp;
#endif

		public int Trellis;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MinPredictionOrder;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MaxPredictionOrder;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public long TimecodeFrameStart;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public delegate void RTPCallbackFn([NativeType("AVCodecContext*")] IntPtr avctx, IntPtr data, int size, int mbNb);

		[Obsolete("Deprecated in libavcodec >= 59")]
		public RTPCallbackFn RTPCallback;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int RTPPayloadSize;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MVBits;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int HeaderBits;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int ITexBits;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int PTexBits;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int ICount;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int PCount;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int SkipCount;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int MiscBits;
#endif

		[MarshalAs(UnmanagedType.LPStr)]
		public string StatsOut;

		[MarshalAs(UnmanagedType.LPStr)]
		public string StatsIn;

		public AVWorkaroundBugs WorkaroundBugs;

		public AVStandardCompliance StrictStdCompliance;

		public AVErrorConcealment ErrorConcealment;

		public AVDebugFlags Debug;

		public AVErrorRecognitionFlags ErrRecognition;

		public long ReorderedOpaque;

		[NativeType("const AVHWAccel*")]
		public IntPtr HWAccel;

		public IntPtr HWAccelContext;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = AVFrame.NumDataPointers)]
		public ulong[] Error;

		public AVDCTAlgorithm DCTAlgo;

		public AVIDCTAlgorithm IDCTAlgorithm;

		public int BitsPerCodedSample;

		public int BitsPerRawSample;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		[NativeType("AVFrame*")]
		public IntPtr CodedFrame;
#endif

		public int ThreadCount;

		public AVThreadType ThreadType;

		public int ThreadSafeCallbacks;

		public delegate int ExecuteFnFuncFn([NativeType("AVCodecContext*")] IntPtr c2, IntPtr arg);

		public delegate int ExecuteFn([NativeType("AVCodecContext*")] IntPtr c, [MarshalAs(UnmanagedType.FunctionPtr)] ExecuteFnFuncFn func, IntPtr arg2, ref int ret, int count, int size);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ExecuteFn Execute;

		public delegate int Execute2FnFuncFn([NativeType("AVCodecContext*")] IntPtr c2, IntPtr arg, int jobnr, int threadnr);

		public delegate int Execute2Fn([NativeType("AVCodecContext*")] IntPtr c, [MarshalAs(UnmanagedType.FunctionPtr)] Execute2FnFuncFn func, IntPtr arg2, ref int ret, int count);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public Execute2Fn Execute2;

		public int NSSEWeight;

		public AVCodecProfile Profile;

		public int Level;

		public AVDiscard SkipLoopFilter;

		public AVDiscard SkipIDCT;

		public AVDiscard SkipFrame;

		[NativeType("uint8_t*")]
		public IntPtr SubtitleHeader;

		public int SubtitleHeaderSize;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public ulong VBVDelay;

		[Obsolete("Deprecated in libavcodec >= 59")]
		public int SideDataOnlyPackets;
#endif

		public int InitialPadding;

		public AVRational Framerate;

		public AVPixelFormat SWPixFmt;

		[NativeType("AVPacketSideData*")]
		public IntPtr CodedSideData;

		public int NbCodedSideData;

		[NativeType("AVBufferRef*")]
		public IntPtr HWFramesCtx;

		public int ApplyCropping;

		[NativeType("AVBufferRef*")]
		public IntPtr HWDeviceCtx;

		public AVHWAccelFlags HWAccelFlags;

		public int ExtraHWFrames;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVProfile {

		public AVCodecProfile Profile;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecHWConfig {

		public AVPixelFormat PixFmt;

		public AVCodecHWConfigMethod Methods;

		public AVHWDeviceType DeviceType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodec {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;

		[MarshalAs(UnmanagedType.LPStr)]
		public string LongName;

		public AVMediaType Type;

		public AVCodecID ID;

		public AVCodecCapabilities Capabilities;

		[NativeType("const AVRational*")]
		public IntPtr SupportedFramerates;

		[NativeType("const AVPixelFormat*")]
		public IntPtr PixFmts;

		[NativeType("const int*")]
		public IntPtr SupportedSampleRates;

		[NativeType("const AVSampleFormat*")]
		public IntPtr SampleFmts;

		[NativeType("const uint64_t*")]
		public IntPtr ChannelLayouts;

		[NativeType("const AVClass*")]
		public IntPtr PrivClass;

		[NativeType("const AVProfile*")]
		public IntPtr Profiles;

		[MarshalAs(UnmanagedType.LPStr)]
		public string WrapperName;

		public int PrivDataSize;

		[NativeType("AVCodec*")]
		public IntPtr Next;

		public delegate int InitThreadCopyFn([NativeType("AVCodecContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public InitThreadCopyFn InitThreadCopy;

		public delegate int UpdateThreadContextFn([NativeType("AVCodecContext*")] IntPtr dst, [NativeType("const AVCodecContext*")] IntPtr src);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public UpdateThreadContextFn UpdateThreadContext;

		[NativeType("const AVCodecDefault*")]
		public IntPtr Defaults;

		public delegate void InitStaticDataFn([NativeType("AVCodec*")] IntPtr codec);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public InitStaticDataFn InitStaticData;

		public delegate int InitFn([NativeType("AVCodecContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public InitFn Init;

		public delegate int EncodeSubFn([NativeType("AVCodecContext*")] IntPtr ctx, [NativeType("uint8_t*")] IntPtr buf, int bufSize, [NativeType("const AVSubtitle*")] IntPtr sub);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public EncodeSubFn EncodeSub;

		public delegate int Encode2Fn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVPacket*")] IntPtr avpkt, [NativeType("const AVFrame*")] IntPtr frame, ref int gotPacketPtr);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public Encode2Fn Encode2;

		public delegate int DecodeFn([NativeType("AVCodecContext*")] IntPtr avctx, IntPtr outdata, out int outdataSize, [NativeType("AVPacket*")] IntPtr avpkt);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public DecodeFn Decode;

		public delegate int CloseFn([NativeType("AVCodecContext*")] IntPtr avctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CloseFn Close;

		public delegate int SendFrameFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const AVFrame*")] IntPtr frame);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SendFrameFn SendFrame;

		public delegate int ReceivePacketFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVPacket*")] IntPtr avpkt);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ReceivePacketFn ReceivePacket;

		public delegate int ReceiveFrameFn([NativeType("AVCodecContext*")] IntPtr ctx, [NativeType("AVFrame*")] IntPtr frame);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ReceiveFrameFn ReceiveFrame;

		public delegate void FlushFn([NativeType("AVCodecContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FlushFn Flush;

		public AVCodecCap CapsInternal;

		[MarshalAs(UnmanagedType.LPStr)]
		public string BitstreamFilters;

		[NativeType("const AVCodecHWConfigInternal**")]
		public IntPtr HWConfigs;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVHWAccel {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;

		public AVMediaType Type;

		public AVCodecID ID;

		public AVPixelFormat PixFmt;

		public int Capabilities;

		[NativeType("AVHWAccel*")]
		public IntPtr Next;

		public delegate int AllocFrameFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVFrame*")] IntPtr frame);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public AllocFrameFn AllocFrame;

		public delegate int StartFrameFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const uint8_t*")] IntPtr buf, uint bufSize);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public StartFrameFn StartFrame;

		public delegate int DecodeSliceFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const uint8_t*")] IntPtr buf, uint bufSize);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public DecodeSliceFn DecodeSlice;

		public delegate int EndFrameFn([NativeType("AVCodecContext*")] IntPtr avctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public EndFrameFn EndFrame;

		public int FramePrivDataSize;

		public delegate int InitFn([NativeType("AVCodecContext*")] IntPtr avctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public InitFn Init;

		public delegate int UninitFn([NativeType("AVCodecContext*")] IntPtr avctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public UninitFn Uninit;

		public int PrivDataSize;

		public int CapsInternal;

		public delegate int FrameParamsFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("AVBufferRef*")] IntPtr hwframesCtx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FrameParamsFn FrameParams;

	}

#if LIBAVCODEC_VERSION_58

	[Obsolete("Deprecated in libavcodec >= 59")]
	[StructLayout(LayoutKind.Sequential)]
	public struct AVPicture {

		[Obsolete("Deprecated in libavcodec >= 59")]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = AVFrame.NumDataPointers)]
		[NativeType("uint8_t*[AV_NUM_DATA_POINTERS]")]
		public IntPtr[] Data;

		[Obsolete("Deprecated in libavcodec >= 59")]
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = AVFrame.NumDataPointers)]
		public int[] LineSize;

	}

#endif

	[StructLayout(LayoutKind.Sequential)]
	public struct AVSubtitleRect {

		public int X;

		public int Y;

		public int W;

		public int H;

		public int NbColors;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public AVPicture Pict;
#endif

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		[NativeType("uint8_t*[4]")]
		public IntPtr[] Data;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] Linesize;

		public AVSubtitleType Type;

		[MarshalAs(UnmanagedType.LPUTF8Str)]
		public string Text;

		[MarshalAs(UnmanagedType.LPStr)]
		public string ASS;

		public AVSubtitleFlags Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVSubtitle {

		public ushort Format;

		public uint StartDisplayTime;

		public uint EndDisplayTime;

		public uint NumRects;

		[NativeType("AVSubtitleRect**")]
		public IntPtr Rects;

		public long PTS;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecParameters {

		public AVMediaType CodecType;

		public AVCodecID CodecID;

		public uint CodecTag;

		[NativeType("uint8_t*")]
		public IntPtr ExtraData;

		public int ExtraDataSize;

		public int Format;

		public int BitRate;

		public int BitsPerCodedSample;

		public int Profile;

		public int Level;

		public int Width;

		public int Height;

		public AVRational SampleAspectRatio;

		public AVFieldOrder FieldOrder;

		public AVColorRange ColorRange;

		public AVColorPrimaries ColorPrimaries;

		public AVColorTransferCharacteristic ColorTRC;

		public AVColorSpace ColorSpace;

		public AVChromaLocation ChromaLocation;

		public ulong ChannelLayout;

		public int Channels;

		public int SampleRate;

		public int BlockAlign;

		public int InitialPadding;

		public int TrailingPadding;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecParserContext {

		public const int NumParserPTS = 4;

		public IntPtr PrivData;

		[NativeType("AVCodecParser*")]
		public IntPtr Parser;

		public long FrameOffset;

		public long CurOffset;

		public long NextFrameOffset;

		public int PictType;

		public int RepeatPict;

		public long PTS;

		public long DTS;

		public long LastPTS;

		public long LastDTS;

		public int FetchTimestamp;

		public int CurFrameStartIndex;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumParserPTS)]
		public long[] CurFrameOffset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumParserPTS)]
		public long[] CurFramePTS;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumParserPTS)]
		public long[] CurFrameDTS;

		public AVCodecParserFlags Flags;

		public long Offset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumParserPTS)]
		public long[] CurFrameEnd;

		public int KeyFrame;

#if LIBAVCODEC_VERSION_58
		[Obsolete("Deprecated in libavcodec >= 59")]
		public long ConvergenceDuration;
#endif

		public int DTSSyncPoint;

		public int DTSRefDTSDelta;

		public int PTS_DTSDelta;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = NumParserPTS)]
		public long[] CurFramePos;

		public long Pos;

		public long LastPos;

		public int Duration;

		public AVFieldOrder FieldOrder;

		public AVPictureStructure PictureStructure;

		public int OutputPictureNumber;

		public int Width;

		public int Height;

		public int CodedWidth;

		public int CodedHeight;

		public int Format;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AVCodecParser {

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public int[] CodecIDs;

		public int PrivDataSize;

		public delegate int ParserInitFn([NativeType("AVCodecParserContext*")] IntPtr s);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ParserInitFn ParserInit;

		public delegate int ParserParseFn([NativeType("AVCodecParserContext*")] IntPtr s, [NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const uint8_t*")] out IntPtr outBuf, out int outBufSize, [NativeType("const uint8_t*")] IntPtr buf, int bufSize);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ParserParseFn ParserParse;

		public delegate void ParserCloseFn([NativeType("AVCodecParserContext*")] IntPtr s);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public ParserCloseFn ParserClose;

		public delegate int SplitFn([NativeType("AVCodecContext*")] IntPtr avctx, [NativeType("const uint8_t*")] IntPtr buf, int bufSize);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SplitFn Split;

		[NativeType("AVCodecParser*")]
		public IntPtr Next;

	}

#if LIBAVCODEC_VERSION_58
	[Obsolete("Deprecated in libavcodec >= 59")]
	public struct AVBitStreamFilterContext {

		public IntPtr PrivData;

		[NativeType("AVBitStreamFilter*")]
		public IntPtr Filter;

		[NativeType("AVCodecParserContext*")]
		public IntPtr Parser;

		[NativeType("AVBitStreamFilterContext*")]
		public IntPtr Next;

	}
#endif

	public struct AVBSFContext {

		[NativeType("AVClass*")]
		public IntPtr Class;

		[NativeType("AVBitStreamFilter*")]
		public IntPtr Filter;

		[NativeType("AVBSFInternal*")]
		public IntPtr Internal;

		public IntPtr PrivData;

		[NativeType("AVCodecParameters*")]
		public IntPtr ParIn;

		[NativeType("AVCodecParameters*")]
		public IntPtr ParOut;

		public AVRational TimeBaseIn;

		public AVRational TimeBaseOut;

	}

	public struct AVBitStreamFilter {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;

		[NativeType("const AVCodecID*")]
		public IntPtr CodecIDs;

		[NativeType("const AVClass*")]
		public IntPtr PrivClass;

		public int PrivDataSize;

		public delegate int InitFn([NativeType("AVBSFContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public InitFn Init;

		public delegate int FilterFn([NativeType("AVBSFContext*")] IntPtr ctx, [NativeType("AVPacket*")] IntPtr pkt);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FilterFn Filter;

		public delegate void CloseFn([NativeType("AVBSFContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public CloseFn Close;

		public delegate void FlushFn([NativeType("AVBSFContext*")] IntPtr ctx);

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public FlushFn Flush;

	}

	// internal.h

	// --==[ libswscale ]==--
	// swscale.h

	[StructLayout(LayoutKind.Sequential)]
	public struct SwsVector {

		[NativeType("double*")]
		public IntPtr Coeff;

		public int Length;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SwsFilter {

		[NativeType("SwsVector*")]
		public IntPtr LumH;

		[NativeType("SwsVector*")]
		public IntPtr LumV;

		[NativeType("SwsVector*")]
		public IntPtr ChrH;

		[NativeType("SwsVector*")]
		public IntPtr ChrV;

	}

}
