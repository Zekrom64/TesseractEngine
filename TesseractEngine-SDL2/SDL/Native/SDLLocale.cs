using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.SDL.Native {

	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Locale {

		[NativeType("const char*")]
		public IntPtr Language;
		[NativeType("const char*")]
		public IntPtr Contry;

	}

}
