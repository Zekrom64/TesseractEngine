using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL {
	
	public enum SDLSystemCursor {
		Arrow,
		IBeam,
		Wait,
		Crosshair,
		WaitArrow,
		SizeNWSE,
		SizeNESW,
		SizeWE,
		SizeNS,
		SizeAll,
		No,
		Hand
	}

	public enum SDLMouseWheelDirection : uint {
		Normal,
		Flipped
	}

	public enum SDLMouseButtonState : uint {
		Left = 0x0001,
		Middle = 0x0002,
		Right = 0x0004,
		X1 = 0x0008,
		X2 = 0x0010
	}

}
