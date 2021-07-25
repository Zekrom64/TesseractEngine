using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLKeysym {

		public SDLScancode Scancode;
		public SDLKeycode Sym;
		public SDLKeymod Mod;
		private readonly uint unused;

	}

}
