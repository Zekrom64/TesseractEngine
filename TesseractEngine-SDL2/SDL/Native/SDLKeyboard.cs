using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL.Native {
	
	public struct SDL_Keysym {
		public SDLScancode scancode;
		public SDLKeycode sym;
		public ushort mod;
		public uint unused;
	}

}
