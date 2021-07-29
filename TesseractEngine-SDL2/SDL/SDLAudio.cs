using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

namespace Tesseract.SDL {
	
	public struct SDLAudioFormat {

		public static readonly SDLAudioFormat U8 = new() { Value = 0x0008 };
		public static readonly SDLAudioFormat S8 = new() { Value = 0x8008 };
		public static readonly SDLAudioFormat U16LSB = new() { Value = 0x0010 };
		public static readonly SDLAudioFormat S16LSB = new() { Value = 0x8010 };
		public static readonly SDLAudioFormat U16MSB = new() { Value = 0x1010 };
		public static readonly SDLAudioFormat S16MSB = new() { Value = 0x9010 };
		public static readonly SDLAudioFormat U16 = U16LSB;
		public static readonly SDLAudioFormat S16 = S16LSB;
		public static readonly SDLAudioFormat S32LSB = new() { Value = 0x8020 };
		public static readonly SDLAudioFormat S32MSB = new() { Value = 0x9020 };
		public static readonly SDLAudioFormat S32 = S32LSB;
		public static readonly SDLAudioFormat F32LSB = new() { Value = 0x8120 };
		public static readonly SDLAudioFormat F32MSB = new() { Value = 0x9120 };
		public static readonly SDLAudioFormat F32 = F32LSB;
		public static readonly SDLAudioFormat U16Sys = BitConverter.IsLittleEndian ? U16LSB : U16MSB;
		public static readonly SDLAudioFormat S16Sys = BitConverter.IsLittleEndian ? S16LSB : S16MSB;
		public static readonly SDLAudioFormat S32Sys = BitConverter.IsLittleEndian ? S32LSB : S32MSB;
		public static readonly SDLAudioFormat F32Sys = BitConverter.IsLittleEndian ? F32LSB : F32MSB;

		public ushort Value { get; set; }

		public int BitSize => Value & 0xFF;

		public bool IsFloat {
			get => (Value & 0x100) != 0;
			set {
				if (value) Value |= 0x100;
				else Value &= 0x100;
			}
		}

		public bool IsBigEndian {
			get => (Value & 0x1000) != 0;
			set {
				if (value) Value |= 0x1000;
				else Value &= 0x1000;
			}
		}

		public bool IsSigned {
			get => (Value & 0x8000) != 0;
			set {
				if (value) Value |= 0x8000;
				else Value &= 0x8000;
			}
		}

		public bool IsInt {
			get => !IsFloat;
			set => IsFloat = !value;
		}

		public bool IsLittleEndian {
			get => !IsBigEndian;
			set => IsBigEndian = !value;
		}

		public bool IsUnsigned {
			get => !IsSigned;
			set => IsSigned = !value;
		}

	}

	public enum SDLAudioAllowChange : int {
		Frequency = 0x00000001,
		Format = 0x00000002,
		Channels = 0x00000004,
		Samples = 0x00000008,
		Any = Frequency | Format | Channels | Samples
	}

	public delegate void SDLAudioCallback(IntPtr userdata, [NativeType("Uint8*")] IntPtr stream, int len);

	[StructLayout(LayoutKind.Sequential)]	
	public struct SDLAudioSpec {
		public int Freq;
		public SDLAudioFormat Format;
		public byte Channels;
		public byte Silence;
		public ushort Samples;
		private readonly ushort padding;
		public uint Size;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioCallback Callback;
		public IntPtr Userdata;
	}

	public delegate void SDLAudioFilter(ref SDLAudioCVT cvt, SDLAudioFormat format);

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLAudioCVT {
		public int Needed;
		public SDLAudioFormat SrcFormat;
		public SDLAudioFormat DstFormat;
		public double RateIncrement;
		[NativeType("Uint8*")]
		public IntPtr Buf;
		public int Len;
		public int LenCvt;
		public int LenMult;
		public double LenRatio;
		private IntPtr filter0;
		private IntPtr filter1;
		private IntPtr filter2;
		private IntPtr filter3;
		private IntPtr filter4;
		private IntPtr filter5;
		private IntPtr filter6;
		private IntPtr filter7;
		private IntPtr filter8;
		private IntPtr filter9;
		/*
		public IIndexer<int, SDLAudioFilter> Filters => new FuncIndexer<int, SDLAudioFilter>(
			i => {
				if (i < 0 || i > 9) throw new IndexOutOfRangeException();
				unsafe {
					ref IntPtr filter0 = ref this.filter0;

				}
			},
			(i, filter) => {
				if (i < 0 || i > 9) throw new IndexOutOfRangeException();

			}
		);
		*/
		public int FilterIndex;
	}

}
