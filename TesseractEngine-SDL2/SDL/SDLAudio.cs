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
		[NativeType("void callback(void* userdata, UInt8* stream, int len)")]
		public unsafe delegate* unmanaged<IntPtr, IntPtr, int, void> Callback;
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
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter0;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter1;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter2;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter3;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter4;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter5;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter6;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter7;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter8;
		public unsafe delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void> Filter9;
		public int FilterIndex;

		public SDLAudioFilter this[int index] {
			get {
				if (index < 0 || index > 9) throw new IndexOutOfRangeException();
				unsafe {
					fixed(delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void>* pFilters = &Filter0) {
						return Marshal.GetDelegateForFunctionPointer<SDLAudioFilter>((IntPtr)pFilters[index]);
					}
				}
			}
			set {
				if (index < 0 || index > 9) throw new IndexOutOfRangeException();
				unsafe {
					fixed (delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void>* pFilters = &Filter0) {
						pFilters[index] = (delegate* unmanaged<ref SDLAudioCVT, SDLAudioFormat, void>)Marshal.GetFunctionPointerForDelegate(value);
					}
				}
			}
		}

		public void Build(SDLAudioFormat srcFormat, byte srcChannels, int srcRate, SDLAudioFormat dstFormat, byte dstChannels, int dstRate) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_BuildAudioCVT(out this, srcFormat, srcChannels, srcRate, dstFormat, dstChannels, dstRate));
			}
		}

		public void Convert() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_ConvertAudio(this));
			}
		}
	}

	public enum SDLAudioStatus {
		Stopped = 0,
		Playing,
		Paused
	}

	public class SDLAudioDevice : IDisposable {

		public uint AudioDeviceID { get; private set; }

		public SDLAudioSpec AudioSpec { get; }

		public SDLAudioStatus Status {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetAudioDeviceStatus(AudioDeviceID);
				}
			}
		}

		public uint QueuedAudioSize {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetQueuedAudioSize(AudioDeviceID);
				}
			}
		}

		public SDLAudioDevice(string device, bool capture, SDLAudioSpec desired, SDLAudioAllowChange allowedChanges) {
			unsafe {
				fixed(byte* pDevice = MemoryUtil.StackallocUTF8(device, stackalloc byte[256])) {
					AudioDeviceID = SDL2.Functions.SDL_OpenAudioDevice(pDevice, capture, desired, out SDLAudioSpec obtained, allowedChanges);
					if (AudioDeviceID == 0) throw new SDLException(SDL2.GetError());
					AudioSpec = obtained;
				}
			}
		}

		public void Pause() {
			unsafe {
				SDL2.Functions.SDL_PauseAudioDevice(AudioDeviceID, true);
			}
		}

		public void Unpause() {
			unsafe {
				SDL2.Functions.SDL_PauseAudioDevice(AudioDeviceID, false);
			}
		}

		public void Queue(in ReadOnlySpan<byte> data) {
			unsafe {
				fixed(byte* pData = data) {
					SDL2.CheckError(SDL2.Functions.SDL_QueueAudio(AudioDeviceID, (IntPtr)pData, (uint)data.Length));
				}
			}
		}

		public void Queue(IConstPointer<byte> data, uint length) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_QueueAudio(AudioDeviceID, data.Ptr, length));
			}
		}

		public uint Deueue(Span<byte> data) {
			unsafe {
				fixed (byte* pData = data) {
					return SDL2.Functions.SDL_DequeueAudio(AudioDeviceID, (IntPtr)pData, (uint)data.Length);
				}
			}
		}

		public uint Dequeue(IConstPointer<byte> data, uint length) {
			unsafe {
				return SDL2.Functions.SDL_DequeueAudio(AudioDeviceID, data.Ptr, length);
			}
		}

		public void ClearQueuedAudio() {
			unsafe {
				SDL2.Functions.SDL_ClearQueuedAudio(AudioDeviceID);
			}
		}

		public void Lock() {
			unsafe {
				SDL2.Functions.SDL_LockAudioDevice(AudioDeviceID);
			}
		}

		public void Unlock() {
			unsafe {
				SDL2.Functions.SDL_UnlockAudioDevice(AudioDeviceID);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (AudioDeviceID != 0) {
				unsafe {
					SDL2.Functions.SDL_CloseAudioDevice(AudioDeviceID);
				}
				AudioDeviceID = 0;
			}
		}

	}

	public class SDLAudioStream : IDisposable {

		[NativeType("SDL_AudioStream*")]
		public IntPtr AudioStream { get; private set; }

		public int Available {
			get {
				unsafe {
					return SDL2.Functions.SDL_AudioStreamAvailable(AudioStream);
				}
			}
		}

		public SDLAudioStream(SDLAudioFormat srcFormat, byte srcChannels, int srcRate, SDLAudioFormat dstFormat, byte dstChannels, byte dstRate) {
			unsafe {
				IntPtr pStream = SDL2.Functions.SDL_NewAudioStream(srcFormat, srcChannels, srcRate, dstFormat, dstChannels, dstRate);
				if (pStream == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				AudioStream = pStream;
			}
		}

		public void Put(in ReadOnlySpan<byte> buf) {
			unsafe {
				fixed(byte* pBuf = buf) {
					SDL2.CheckError(SDL2.Functions.SDL_AudioStreamPut(AudioStream, (IntPtr)pBuf, buf.Length));
				}
			}
		}

		public void Put(IConstPointer<byte> buf, int len) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_AudioStreamPut(AudioStream, buf.Ptr, len));
			}
		}

		public Span<byte> Get(Span<byte> buf) {
			int len;
			unsafe {
				fixed(byte* pBuf = buf) {
					len = SDL2.Functions.SDL_AudioStreamGet(AudioStream, (IntPtr)pBuf, buf.Length);
				}
			}
			SDL2.CheckError(len);
			return buf[..len];
		}

		public int Get(IPointer<byte> buf, int len) {
			unsafe {
				len = SDL2.Functions.SDL_AudioStreamGet(AudioStream, buf.Ptr, len);
				SDL2.CheckError(len);
				return len;
			}
		}

		public void Flush() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_AudioStreamFlush(AudioStream));
			}
		}

		public void Clear() {
			unsafe {
				SDL2.Functions.SDL_AudioStreamClear(AudioStream);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (AudioStream != IntPtr.Zero) {
				unsafe {
					SDL2.Functions.SDL_FreeAudioStream(AudioStream);
				}
				AudioStream = IntPtr.Zero;
			}
		}

	}

}
