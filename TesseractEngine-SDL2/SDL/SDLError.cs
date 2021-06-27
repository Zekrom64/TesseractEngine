using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL {

	public enum SDLErrorCode {
		NoMem,
		FileRead,
		FileWrite,
		FileSeek,
		Unsupported,
		LastError
	}
	
	public class SDLException : Exception {

		public SDLException(string msg) : base(msg) { }

	}

}
