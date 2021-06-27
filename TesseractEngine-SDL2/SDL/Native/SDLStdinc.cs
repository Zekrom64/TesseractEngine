using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL.Native {
	
	public struct SDLBool {

		public int Value;

		public SDLBool(int value) {
			Value = value;
		}

		public SDLBool(bool value) {
			Value = value ? 1 : 0;
		}

		public static implicit operator bool(SDLBool b) => b.Value != 0;

		public static implicit operator int(SDLBool b) => b.Value;

		public static implicit operator SDLBool(bool b) => new(b);

		public static implicit operator SDLBool(int b) => new(b);

	}

}
