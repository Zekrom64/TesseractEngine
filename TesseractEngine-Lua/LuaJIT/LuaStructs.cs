using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	/// <summary>
	/// A structure used to carry different pieces of information about an active Lua function.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LuaDebug {

		/// <summary>
		/// The event which provoked this debug snapshot.
		/// </summary>
		public LuaHookEvent Event;

		private readonly IntPtr NamePtr;

		/// <summary>
		/// The name for the current function, or null if there is none defined.
		/// </summary>
		public readonly string? Name {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryUtil.GetUTF8(NamePtr);
		}

		private readonly IntPtr NameWhatPtr;
		
		/// <summary>
		/// Clarifies the <see cref="Name"/> field as being <b>global</b>, <b>local</b>,
		/// <b>method</b>, <b>field</b>, <b>upvalue</b> or an empty string if none of
		/// these apply.
		/// </summary>
		public readonly string NameWhat {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryUtil.GetUTF8(NameWhatPtr)!;
		}

		private readonly IntPtr WhatPtr;

		/// <summary>
		/// The type of function, one of <b>Lua</b>, <b>C</b>, <b>main</b> (if in the
		/// main part of a chunk), or <b>tail</b> (if the function did a tail call).
		/// </summary>
		public readonly string What {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryUtil.GetUTF8(WhatPtr)!;
		}

		private readonly IntPtr SourcePtr;

		/// <summary>
		/// If the function was defined by a string this is passed as the source. If
		/// defined in a file the source starts with '@' followed by the filename.
		/// </summary>
		public readonly string Source {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryUtil.GetUTF8(SourcePtr)!;
		}

		/// <summary>
		/// The current line number, or -1 if not known.
		/// </summary>
		public int CurrentLine;

		/// <summary>
		/// The number of upvalues passed to the function.
		/// </summary>
		public int NUps;

		/// <summary>
		/// The line number where the function definition starts.
		/// </summary>
		public int LineDefined;

		/// <summary>
		/// The line number where the function definition ends.
		/// </summary>
		public int LastLineDefined;

		private unsafe fixed byte short_src[Lua.IDSize];

		/// <summary>
		/// A more printable version of <see cref="Source"/> used in error messages.
		/// </summary>
		public readonly string ShortSrc {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					fixed(byte* pShortSrc = short_src) {
						return MemoryUtil.GetUTF8((IntPtr)pShortSrc, Lua.IDSize)!;
					}
				}
			}
		}

		// Extra padding to make sure the Lua implementation doesn't trash memory
		// for any other implementation-defined private fields
		private unsafe fixed byte padding[128];

	}

}
