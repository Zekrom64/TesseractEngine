using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;
using Tesseract.SDL.Native;

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
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter0;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter1;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter2;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter3;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter4;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter5;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter6;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter7;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter8;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public SDLAudioFilter Filter9;
		public int FilterIndex;

		public SDLAudioFilter this[int index] {
			get => index switch {
				0 => Filter0,
				1 => Filter1,
				2 => Filter2,
				3 => Filter3,
				4 => Filter4,
				5 => Filter5,
				6 => Filter6,
				7 => Filter7,
				8 => Filter8,
				9 => Filter9,
				_ => throw new IndexOutOfRangeException()
			};
			set {
				switch(index) {
					case 0: Filter0 = value; break;
					case 1: Filter1 = value; break;
					case 2: Filter2 = value; break;
					case 3: Filter3 = value; break;
					case 4: Filter4 = value; break;
					case 5: Filter5 = value; break;
					case 6: Filter6 = value; break;
					case 7: Filter7 = value; break;
					case 8: Filter8 = value; break;
					case 9: Filter9 = value; break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		public void Build(SDLAudioFormat srcFormat, byte srcChannels, int srcRate, SDLAudioFormat dstFormat, byte dstChannels, int dstRate) =>
			SDL2.CheckError(SDL2.Functions.SDL_BuildAudioCVT(out this, srcFormat, srcChannels, srcRate, dstFormat, dstChannels, dstRate));

		public void Convert() => SDL2.CheckError(SDL2.Functions.SDL_ConvertAudio(this));
	}

	public enum SDLAudioStatus {
		Stopped = 0,
		Playing,
		Paused
	}

	public class AudioDevice : IDisposable {

		public uint AudioDeviceID { get; private set; }

		public SDLAudioSpec AudioSpec { get; }

		public SDLAudioStatus Status => SDL2.Functions.SDL_GetAudioDeviceStatus(AudioDeviceID);

		public uint QueuedAudioSize => SDL2.Functions.SDL_GetQueuedAudioSize(AudioDeviceID);

		public AudioDevice(string device, bool capture, SDLAudioSpec desired, SDLAudioAllowChange allowedChanges) {
			AudioDeviceID = SDL2.Functions.SDL_OpenAudioDevice(device, capture ? 1 : 0, desired, out SDLAudioSpec obtained, allowedChanges);
			if (AudioDeviceID == 0) throw new SDLException(SDL2.GetError());
			AudioSpec = obtained;
		}

		public void Pause() => SDL2.Functions.SDL_PauseAudioDevice(AudioDeviceID, 1);

		public void Unpause() => SDL2.Functions.SDL_PauseAudioDevice(AudioDeviceID, 1);

		public void Queue(in ReadOnlySpan<byte> data) {
			unsafe {
				fixed(byte* pData = data) {
					SDL2.CheckError(SDL2.Functions.SDL_QueueAudio(AudioDeviceID, (IntPtr)pData, (uint)data.Length));
				}
			}
		}

		public void Queue(IConstPointer<byte> data, uint length) => SDL2.CheckError(SDL2.Functions.SDL_QueueAudio(AudioDeviceID, data.Ptr, length));

		public uint Deueue(Span<byte> data) {
			unsafe {
				fixed (byte* pData = data) {
					return SDL2.Functions.SDL_DequeueAudio(AudioDeviceID, (IntPtr)pData, (uint)data.Length);
				}
			}
		}

		public uint Dequeue(IConstPointer<byte> data, uint length) => SDL2.Functions.SDL_DequeueAudio(AudioDeviceID, data.Ptr, length);

		public void ClearQueuedAudio() => SDL2.Functions.SDL_ClearQueuedAudio(AudioDeviceID);

		public void Lock() => SDL2.Functions.SDL_LockAudioDevice(AudioDeviceID);

		public void Unlock() => SDL2.Functions.SDL_UnlockAudioDevice(AudioDeviceID);

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (AudioDeviceID != 0) {
				SDL2.Functions.SDL_CloseAudioDevice(AudioDeviceID);
				AudioDeviceID = 0;
			}
		}

	}

	public class SDLAudioStream : IDisposable {

		public IPointer<SDL_AudioStream> AudioStream { get; private set; }

		public int Available => SDL2.Functions.SDL_AudioStreamAvailable(AudioStream.Ptr);

		public SDLAudioStream(SDLAudioFormat srcFormat, byte srcChannels, int srcRate, SDLAudioFormat dstFormat, byte dstChannels, byte dstRate) {
			IntPtr pStream = SDL2.Functions.SDL_NewAudioStream(srcFormat, srcChannels, srcRate, dstFormat, dstChannels, dstRate);
			if (pStream == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			AudioStream = new UnmanagedPointer<SDL_AudioStream>(pStream);
		}

		public void Put(in ReadOnlySpan<byte> buf) {
			unsafe {
				fixed(byte* pBuf = buf) {
					SDL2.CheckError(SDL2.Functions.SDL_AudioStreamPut(AudioStream.Ptr, (IntPtr)pBuf, buf.Length));
				}
			}
		}

		public void Put(IConstPointer<byte> buf, int len) => SDL2.CheckError(SDL2.Functions.SDL_AudioStreamPut(AudioStream.Ptr, buf.Ptr, len));

		public Span<byte> Get(Span<byte> buf) {
			int len;
			unsafe {
				fixed(byte* pBuf = buf) {
					len = SDL2.Functions.SDL_AudioStreamGet(AudioStream.Ptr, (IntPtr)pBuf, buf.Length);
				}
			}
			SDL2.CheckError(len);
			return buf[..len];
		}

		public int Get(IPointer<byte> buf, int len) {
			len = SDL2.Functions.SDL_AudioStreamGet(AudioStream.Ptr, buf.Ptr, len);
			SDL2.CheckError(len);
			return len;
		}

		public void Flush() => SDL2.CheckError(SDL2.Functions.SDL_AudioStreamFlush(AudioStream.Ptr));

		public void Clear() => SDL2.Functions.SDL_AudioStreamClear(AudioStream.Ptr);

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (AudioStream != null) {
				SDL2.Functions.SDL_FreeAudioStream(AudioStream.Ptr);
				AudioStream = new NullPointer<SDL_AudioStream>();
			}
		}

	}

}
