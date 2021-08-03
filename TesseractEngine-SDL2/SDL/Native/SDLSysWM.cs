using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_SysWMmsg {

		public SDLVersion Version;
		public SDLSysWMType Subsystem;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_SysWMinfo {

		public SDLVersion Version;
		public SDLSysWMType Subsystem;

		[StructLayout(LayoutKind.Explicit, Size = 64)]
		public struct SDL_SysWMinfo_info {

			[FieldOffset(0)]
			public SDLSysWMInfoWin Win;

		}

		public SDL_SysWMinfo_info Info;

	}

}
