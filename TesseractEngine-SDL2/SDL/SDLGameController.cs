using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL {
	
	public enum SDLGameControllerType {
		Unknown = 0,
		Xbox360,
		XboxOne,
		PS3,
		PS4,
		NintendoSwitchPro,
		Virtual,
		PS5
	}

	public enum SDLGameControllerBindType {
		None = 0,
		Button,
		Axis,
		Hat
	}

	public enum SDLGameControllerAxis : sbyte {
		Invalid = -1,
		LeftX,
		LeftY,
		RightX,
		RightY,
		TriggerLeft,
		TriggerRight
	}

	public enum SDLGameControllerButton : sbyte {
		Invalid = -1,
		A,
		B,
		X,
		Y,
		Back,
		Guide,
		Start,
		LeftStick,
		RightStick,
		LeftShoulder,
		RightShoulder,
		DPadUp,
		DPadDown,
		DPadLeft,
		DPadRight,
		Misc1,
		Paddle1,
		Paddle2,
		Paddle3,
		Paddle4,
		Touchpad
	}

}
