using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {

	/// <summary>
	/// Returns the size of the file in this RWops, or -1 if unknown.
	/// </summary>
	/// <param name="context">RWops context structure</param>
	/// <returns>Size of file, or -1</returns>
	public delegate long PFN_SDL_RWops_size([NativeType("SDL_RWops*")] IntPtr context);

	/// <summary>
	/// Seek to the offset relative to a position in the stream.
	/// </summary>
	/// <param name="context">RWops context structure</param>
	/// <param name="offset">Offset to seek</param>
	/// <param name="whence">Position to seek relative to</param>
	/// <returns>The final offset in the data stream, or -1 on error</returns>
	public delegate long PFN_SDL_RWops_seek([NativeType("SDL_RWops*")] IntPtr context, long offset, SDLRWWhence whence);

	/// <summary>
	/// Reads up to a maximum number of objects of a given size from the data stream.
	/// </summary>
	/// <param name="context">RWops context structure</param>
	/// <param name="ptr">Pointer to write objects to</param>
	/// <param name="size">Size of objects</param>
	/// <param name="maxnum">Number of objects to read</param>
	/// <returns>Number of objects read, or 0 if at end of file</returns>
	public delegate UIntPtr PFN_SDL_RWops_read([NativeType("SDL_RWops*")] IntPtr context, IntPtr ptr, UIntPtr size, UIntPtr maxnum);

	/// <summary>
	/// Writes up to a number of objects of a given size to the data stream.
	/// </summary>
	/// <param name="context">RWops context structure</param>
	/// <param name="ptr">Pointer to read objects from</param>
	/// <param name="size">Size of objects</param>
	/// <param name="num">Number of objects to write</param>
	/// <returns>Number of obejcts written, or 0 on error or at end of file</returns>
	public delegate UIntPtr PFN_SDL_RWops_write([NativeType("SDL_RWops*")] IntPtr context, IntPtr ptr, UIntPtr size, UIntPtr num);

	/// <summary>
	/// Closes and frees the RWops structure.
	/// </summary>
	/// <param name="context">RWops context structure</param>
	/// <returns>Zero if successful, or -1 on error</returns>
	public delegate int PFN_SDL_RWops_close([NativeType("SDL_RWops*")] IntPtr context);

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

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public PFN_SDL_RWops_size Size;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public PFN_SDL_RWops_seek Seek;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public PFN_SDL_RWops_read Read;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public PFN_SDL_RWops_write Write;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public PFN_SDL_RWops_close Close;

		public SDLRWOpsType Type;
		public SDL_RWops_hidden Hidden;

	}

}
