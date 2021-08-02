using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL {

	public delegate void SDLWindowsMessageHook(IntPtr userdata, IntPtr hwnd, uint message, ulong wparam, long lparam);
	public delegate void SDLiOSAnimationCallback(IntPtr userdata);

	public enum SDLAndroidStorageState : int {
		Read = 0x1,
		Write = 0x2
	}

	public enum SDLWinRTPath {
		InstalledLocation,
		LocalFolder,
		RoamingFolder,
		TempFolder
	}

	public enum SDLWinRTDeviceFamily {
		Unknown,
		Desktop,
		Mobile,
		Xbox
	}

}
