using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops_hidden_androidio {

		public IntPtr FileNameRef;
		public IntPtr InputStreamRef;
		public IntPtr ReadableByteChannelRef;
		public IntPtr ReadMethod;
		public IntPtr AssetFileDescriptorRef;
		public long Position;
		public long Size;
		public long Offset;
		public int FD;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops_hidden_windowsio {

		public SDLBool Append;
		public IntPtr H;
		public IntPtr Data;
		public UIntPtr Size;
		public UIntPtr Left;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops_hidden_stdio {

		public SDLBool Autoclose;
		[NativeType("FILE*")]
		public IntPtr FP;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops_hidden_mem {

		[NativeType("Uint8*")]
		public IntPtr Base;
		[NativeType("Uint8*")]
		public IntPtr Here;
		[NativeType("Uint8*")]
		public IntPtr Stop;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops_hidden_unknown {

		public IntPtr Data1;
		public IntPtr Data2;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct SDL_RWops_hidden {

		[FieldOffset(0)]
		public SDL_RWops_hidden_androidio AndroidIO;
		[FieldOffset(0)]
		public SDL_RWops_hidden_windowsio WindowsIO;
		[FieldOffset(0)]
		public SDL_RWops_hidden_stdio StdIO;
		[FieldOffset(0)]
		public SDL_RWops_hidden_mem Mem;
		[FieldOffset(0)]
		public SDL_RWops_hidden_unknown Unknown;

	}

	/// <summary>
	/// A read/write operation structure.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_RWops {

		[NativeType("SInt64 (*size)(SDL_RWops* context)")]
		public unsafe delegate* unmanaged<SDL_RWops*, long> Size;
		[NativeType("SInt64 (*seek)(SDL_RWops* context, SInt64 offset, int whence)")]
		public unsafe delegate* unmanaged<SDL_RWops*, long, SDLRWWhence, long> Seek;
		[NativeType("size_t (*read)(SDL_RWops* context, void* ptr, size_t size, size_t maxnum)")]
		public unsafe delegate* unmanaged<SDL_RWops*, IntPtr, nuint, nuint, nuint> Read;
		[NativeType("size_t (*write)(SDL_RWops* context, void* ptr, size_t size, size_t num)")]
		public unsafe delegate* unmanaged<SDL_RWops*, IntPtr, nuint, nuint, nuint> Write;
		[NativeType("int (*close)(SDL_RWops* context)")]
		public unsafe delegate* unmanaged<SDL_RWops*, int> Close;

		public SDLRWOpsType Type;
		public SDL_RWops_hidden Hidden;

	}

}
