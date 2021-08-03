using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL {
	
	public enum SDLSysWMType {
		Unknown,
		Windows,
		X11,
		DirectFB,
		Cocoa,
		UIKit,
		Wayland,
		Mir,
		WinRT,
		Android,
		Vivante,
		OS2,
		Haiku
	}

	public record SDLSysWMMsg {

		public SDLVersion Version { get; init; }

		public SDLSysWMType Subsystem { get; init; }

		public object Msg { get; init; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLSysWMMsgWin {

		[NativeType("HWND")]
		public IntPtr HWnd;

		public uint Msg;

		public uint WParam;

		public ulong LParam;

	}

	public record SDLSysWMInfo {

		public SDLVersion Version { get; init; }

		public SDLSysWMType Subsystem { get; init; }

		public object Info { get; init; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLSysWMInfoWin {

		[NativeType("HWND")]
		public IntPtr HWnd;

		[NativeType("HDC")]
		public IntPtr HDC;

		[NativeType("HINSTANCE")]
		public IntPtr HInstance;

	}

}
