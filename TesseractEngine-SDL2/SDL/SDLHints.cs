using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	public enum SDLHintPriority {
		Default,
		Normal,
		Override
	}

	public delegate void SDLHintCallback(IntPtr userdata, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string oldval, [MarshalAs(UnmanagedType.LPStr)] string newval);

}
