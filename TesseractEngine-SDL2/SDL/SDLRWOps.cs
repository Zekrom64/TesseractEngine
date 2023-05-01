using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	/// <summary>
	/// Enumeration of RWOp types.
	/// </summary>
	public enum SDLRWOpsType : uint {
		/// <summary>
		/// Unknown stream type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Win32 file stream.
		/// </summary>
		WinFile = 1,
		/// <summary>
		/// C stdio file stream.
		/// </summary>
		StdFile = 2,
		/// <summary>
		/// Android asset file.
		/// </summary>
		JNIFile = 3,
		/// <summary>
		/// Memory stream.
		/// </summary>
		Memory = 4,
		/// <summary>
		/// Read-only memory stream.
		/// </summary>
		ReadOnlyMemory = 5
	}

	/// <summary>
	/// The "whence" value when seeking.
	/// </summary>
	public enum SDLRWWhence : int {
		/// <summary>
		/// Seek from the beginning of the file.
		/// </summary>
		Set = 0,
		/// <summary>
		/// Seek relative to the current position in the file.
		/// </summary>
		Cur = 1,
		/// <summary>
		/// Seek relative to the end of the file.
		/// </summary>
		End = 2
	}

	/// <summary>
	/// An SDLRWOps object wraps a managed SDL RWOps stream.
	/// </summary>
	public class SDLRWOps : IDisposable {

		/// <summary>
		/// The underlying RWOps structure.
		/// </summary>
		public ManagedPointer<SDL_RWops> RWOps { get; private set; }

		private MemoryHandle memHandle;

		/// <summary>
		/// Creates a new RWOps object from a file.
		/// </summary>
		/// <param name="path">The path to the file</param>
		/// <param name="mode">The mode to open the file with</param>
		public SDLRWOps(string path, string mode) {
			unsafe {
				fixed(byte* pPath = MemoryUtil.StackallocUTF8(path, stackalloc byte[1024]), pMode = MemoryUtil.StackallocUTF8(mode, stackalloc byte[16])) {
					RWOps = new ManagedPointer<SDL_RWops>((IntPtr)SDL2.Functions.SDL_RWFromFile(pPath, pMode), ptr => SDL2.Functions.SDL_FreeRW((SDL_RWops*)ptr));
				}
			}
		}

		/// <summary>
		/// Creates a new RWOps object from a region of memory.
		/// </summary>
		/// <param name="memory">Memory to </param>
		public SDLRWOps(Memory<byte> memory) {
			memHandle = memory.Pin();
			unsafe {
				RWOps = new ManagedPointer<SDL_RWops>((IntPtr)SDL2.Functions.SDL_RWFromMem((IntPtr)memHandle.Pointer, memory.Length), ptr => SDL2.Functions.SDL_FreeRW((SDL_RWops*)ptr));
			}
		}

		public SDLRWOps(ReadOnlyMemory<byte> memory) {
			memHandle = memory.Pin();
			unsafe {
				RWOps = new ManagedPointer<SDL_RWops>((IntPtr)SDL2.Functions.SDL_RWFromConstMem((IntPtr)memHandle.Pointer, memory.Length), ptr => SDL2.Functions.SDL_FreeRW((SDL_RWops*)ptr));
			}
		}

		public long Size {
			get {
				unsafe {
					long size = SDL2.Functions.SDL_RWsize((SDL_RWops*)RWOps.Ptr);
					if (size < 0) throw new SDLException(SDL2.GetError());
					return size;
				}
			}
		}

		public long Position {
			get {
				unsafe {
					long pos = SDL2.Functions.SDL_RWtell((SDL_RWops*)RWOps.Ptr);
					if (pos < 0) throw new SDLException(SDL2.GetError());
					return pos;
				}
			}
			set {
				unsafe {
					long newpos = SDL2.Functions.SDL_RWseek((SDL_RWops*)RWOps.Ptr, value, SDLRWWhence.Set);
					if (newpos < 0) throw new SDLException(SDL2.GetError());
				}
			}
		}

		public int Read(Span<byte> span) {
			unsafe {
				fixed (byte* pSpan = span) {
					int read = (int)SDL2.Functions.SDL_RWread((SDL_RWops*)RWOps.Ptr, (IntPtr)pSpan, 1, (nuint)span.Length);
					if (read == 0) {
						IntPtr error = SDL2.Functions.SDL_GetError();
						if (error != IntPtr.Zero) {
							string strerror = MemoryUtil.GetASCII(error)!;
							SDL2.Functions.SDL_ClearError();
							throw new SDLException(strerror);
						}
					}
					return read;
				}
			}
		}

		public int Read(byte[] data, int offset, int count) => Read(new Span<byte>(data).Slice(offset, count));

		public int Write(Span<byte> span) {
			unsafe {
				fixed (byte* pSpan = span) {
					int read = (int)SDL2.Functions.SDL_RWwrite((SDL_RWops*)RWOps.Ptr, (IntPtr)pSpan, 1, (nuint)span.Length);
					if (read == 0) {
						IntPtr error = SDL2.Functions.SDL_GetError();
						if (error != IntPtr.Zero) {
							string strerror = MemoryUtil.GetASCII(error)!;
							SDL2.Functions.SDL_ClearError();
							throw new SDLException(strerror);
						}
					}
					return read;
				}
			}
		}

		public int Write(byte[] data, int offset, int length) => Write(new Span<byte>(data).Slice(offset, length));

		public ManagedPointer<byte> LoadFile() {
			unsafe {
				IntPtr data = SDL2.Functions.SDL_LoadFile_RW((SDL_RWops*)RWOps.Ptr, out nuint size, false);
				if (data == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new ManagedPointer<byte>(data, ptr => SDL2.Functions.SDL_free(data));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			RWOps.Dispose();
		}

		public static implicit operator SDLSpanRWOps(SDLRWOps rwops) => new(rwops.RWOps);

	}

	public readonly ref struct SDLSpanRWOps {

		public ManagedPointer<SDL_RWops> RWOps { get; }

		public SDLSpanRWOps(ManagedPointer<SDL_RWops> rwops) {
			RWOps = rwops;
		}

		public SDLSpanRWOps(Span<byte> span) {
			unsafe {
				fixed (byte* pSpan = span) {
					RWOps = new ManagedPointer<SDL_RWops>((IntPtr)SDL2.Functions.SDL_RWFromConstMem((IntPtr)pSpan, span.Length), ptr => SDL2.Functions.SDL_FreeRW((SDL_RWops*)ptr));
				}
			}
		}

		public SDLSpanRWOps(ReadOnlySpan<byte> span) {
			unsafe {
				fixed (byte* pSpan = span) {
					RWOps = new ManagedPointer<SDL_RWops>((IntPtr)SDL2.Functions.SDL_RWFromConstMem((IntPtr)pSpan, span.Length), ptr => SDL2.Functions.SDL_FreeRW((SDL_RWops*)ptr));
				}
			}
		}

		public void Dispose() => RWOps.Dispose();

	}

	public class SDLStreamRWOps : IDisposable {

		// RWOp functions may silently catch exceptions, but this is to avoid exceptions propagating through native code.

		[UnmanagedCallersOnly]
		private static unsafe long RWSize(SDL_RWops* pOps) {
			try {
				SDLStreamRWOps stream = new ObjectPointer<SDLStreamRWOps>(pOps->Hidden.Unknown.Data1).Value!;
				return stream.Stream.Length;
			} catch(Exception) {
				return -1;
			}
		}

		[UnmanagedCallersOnly]
		private static unsafe long RWSeek(SDL_RWops* pOps, long offset, SDLRWWhence whence) {
			try {
				SDLStreamRWOps stream = new ObjectPointer<SDLStreamRWOps>(pOps->Hidden.Unknown.Data1).Value!;
				if (!stream.Stream.CanSeek) return -1;
				switch (whence) {
					case SDLRWWhence.Set:
						stream.Stream.Position = offset;
						break;
					case SDLRWWhence.Cur:
						stream.Stream.Position += offset;
						break;
					case SDLRWWhence.End:
						long length = stream.Stream.Length;
						if (length == -1) return -1;
						stream.Stream.Position = length + offset;
						break;
				}
				return stream.Stream.Position;
			} catch (Exception) {
				return -1;
			}
		}

		[UnmanagedCallersOnly]
		private static unsafe nuint RWRead(SDL_RWops* pOps, IntPtr pDst, nuint size, nuint maxnum) {
			try {
				SDLStreamRWOps stream = new ObjectPointer<SDLStreamRWOps>(pOps->Hidden.Unknown.Data1).Value!;
				long bytes = (long)(size * maxnum);
				long offset = 0;
				while (offset < bytes) {
					unsafe {
						int length = (int)Math.Min(bytes - offset, int.MaxValue);
						Span<byte> dst = new((void*)pDst, length);
						int nread = stream.Stream.Read(dst);
						if (nread == 0) return (nuint)offset;
						offset += nread;
					}
				}
				return (nuint)offset;
			} catch (Exception) {
				return 0;
			}
		}

		[UnmanagedCallersOnly]
		private static unsafe nuint RWWrite(SDL_RWops* pOps, IntPtr pDst, nuint size, nuint maxnum) {
			long offset = 0;
			try {
				SDLStreamRWOps stream = new ObjectPointer<SDLStreamRWOps>(pOps->Hidden.Unknown.Data1).Value!;
				long bytes = (long)(size * maxnum);
				while (offset < bytes) {
					unsafe {
						int length = (int)Math.Min(bytes - offset, int.MaxValue);
						Span<byte> dst = new((void*)pDst, length);
						stream.Stream.Write(dst);
						offset += length;
					}
				}
				return (nuint)offset;
			} catch (Exception) {
				return (nuint)offset;
			}
		}

		[UnmanagedCallersOnly]
		private static unsafe int RWClose(SDL_RWops* pOps) {
			try {
				SDLStreamRWOps stream = new ObjectPointer<SDLStreamRWOps>(pOps->Hidden.Unknown.Data1).Value!;
				stream.Stream.Dispose();
				return 0;
			} catch (Exception) {
				return -1;
			}
		}

		public Stream Stream { get; }

		public SDLSpanRWOps RWOps => new(rwops);

		private readonly ObjectPointer<SDLStreamRWOps> self;

		private readonly ManagedPointer<SDL_RWops> rwops;

		public SDLStreamRWOps(Stream stream) {
			Stream = stream;
			self = new ObjectPointer<SDLStreamRWOps>(this);
			unsafe {
				rwops = new ManagedPointer<SDL_RWops> {
					Value = new SDL_RWops() {
						Size = &RWSize,
						Seek = &RWSeek,
						Read = &RWRead,
						Write = &RWWrite,
						Close = &RWClose,
						Type = SDLRWOpsType.Unknown,
						// Hidden data 1 is a pointer to self
						Hidden = new SDL_RWops_hidden() {
							Unknown = new SDL_RWops_hidden_unknown() {
								Data1 = self.Ptr
							}
						}
					}
				};
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Stream.Dispose();
			self.Dispose();
		}

		public static implicit operator SDLSpanRWOps(SDLStreamRWOps stream) => stream.RWOps;

	}

}
