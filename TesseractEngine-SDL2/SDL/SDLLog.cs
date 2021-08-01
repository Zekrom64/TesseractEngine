using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	public enum SDLLogCategory : int {
		Application,
		Error,
		Assert,
		System,
		Audio,
		Video,
		Render,
		Input,
		Test,

		Reserved1,
		Reserved2,
		Reserved3,
		Reserved4,
		Reserved5,
		Reserved6,
		Reserved7,
		Reserved8,
		Reserved9,
		Reserved10,

		Custom
	}

	public enum SDLLogPriority {
		Verbose = 1,
		Debug,
		Info,
		Warn,
		Error,
		Critical
	}

	public delegate void SDLLogOutputFunction(IntPtr userdata, int category, SDLLogPriority priority, [MarshalAs(UnmanagedType.LPStr)] string message);

}
