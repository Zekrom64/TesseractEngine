using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_MessageBoxData {

		public SDLMessageBoxFlags Flags;

		[NativeType("SDL_Window*")]
		public IntPtr Window;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Title;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Message;

		public int NumButtons;

		[NativeType("const SDL_MessageBoxButtonData*")]
		public IntPtr Buttons;

		[NativeType("const SDL_MessageBoxColorScheme*")]
		public IntPtr ColorScheme;

	}

}
