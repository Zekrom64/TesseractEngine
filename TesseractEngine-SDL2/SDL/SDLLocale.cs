using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLLocale {

		[MarshalAs(UnmanagedType.LPStr)]
		public string Language;

		[MarshalAs(UnmanagedType.LPStr)]
		public string Country;

	}

}
