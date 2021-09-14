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

}
