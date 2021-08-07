using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.GLFW {
	
	public enum GLFWButtonState : byte {
		Release = 0,
		Press = 1,
		Repeat = 2
	}

	public enum GLFWHat : byte {
		Centered = 0,
		Up = 0x1,
		Down = 0x2,
		Left = 0x4,
		Right = 0x8,
		RightUp = Right | Up,
		RightDown = Right | Down,
		LeftUp = Left | Up,
		LeftDown = Left | Down
	}

	public enum GLFWKey {
		Unknown = -1,
		Space = 32,
		Apostrophe = 39,
		Comma = 44,
		Minus,
		Period,
		Slash,
		_0,
		_1,
		_2,
		_3,
		_4,
		_5,
		_6,
		_7,
		_8,
		_9,
		Semicolon = 59,
		Equal = 61,
		A = 65,
		B,
		C,
		D,
		E,
		F,
		G,
		H,
		I,
		J,
		K,
		L,
		M,
		N,
		O,
		P,
		Q,
		R,
		S,
		T,
		U,
		V,
		W,
		X,
		Y,
		Z,
		LeftBracket,
		Backslash,
		RightBracket,
		GraveAccent,
		World1 = 161,
		World2,

		Escape = 256,
		Enter,
		Tab,
		Backspace,
		Insert,
		Delete,
		Right,
		Left,
		Down,
		Up,
		PageUp,
		PageDown,
		Home,
		End,
		CapsLock = 280,
		ScrollLock,
		NumLock,
		PrintScreen,
		Pause,
		F1 = 290,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		F13,
		F14,
		F15,
		F16,
		F17,
		F18,
		F19,
		F20,
		F21,
		F22,
		F23,
		F24,
		F25,
		Kp0 = 320,
		Kp1,
		Kp2,
		Kp3,
		Kp4,
		Kp5,
		Kp6,
		Kp7,
		Kp8,
		Kp9,
		KpDecimal,
		KpDivide,
		KpMultiply,
		KpSubtract,
		KpAdd,
		KpEnter,
		KpEqual,
		LeftShift = 340,
		LeftControl,
		LeftAlt,
		LeftSuper,
		RightShift,
		RightControl,
		RightAlt,
		RightSuper,
		Menu
	}

	public enum GLFWKeyMod {
		Shift = 0x1,
		Control = 0x2,
		Alt = 0x4,
		Super = 0x8,
		CapsLock = 0x10,
		NumLock = 0x20
	}

	public enum GLFWMouseButton : int {
		Left = 0,
		Right = 1,
		Middle = 2
	}

	public enum GLFWGamepadButton : int {
		A = 0,
		B,
		X,
		Y,
		LeftBumper,
		RightBumper,
		Back,
		Start,
		Guide,
		LeftThumb,
		RightThumb,
		DpadUp,
		DpadRight,
		DpadDown,
		DpadLeft,

		Cross = A,
		Circle = B,
		Square = X,
		Triangle = Y
	}

	public enum GLFWGamepadAxis : int {
		LeftX = 0,
		LeftY,
		RightX,
		RightY,
		LeftTrigger,
		RightTrigger
	}

	public enum GLFWError {
		NoError = 0,
		NotInitialized = 0x00010001,
		NoCurrentContext = 0x00010002,
		InvaidEnum = 0x00010003,
		InvalidValue = 0x00010004,
		OutOfMemory = 0x00010005,
		APIUnavailable = 0x00010006,
		VersionUnavailable = 0x00010007,
		PlatformError = 0x00010008,
		FormatUnavailable = 0x00010009,
		NoWindowContext = 0x0001000A
	}

	public enum GLFWWindowAttrib : int {
		Focused = 0x00020001,
		Iconified = 0x00020002,
		Resizable = 0x00020003,
		Visible = 0x00020004,
		Decorated = 0x00020005,
		AutoIconify = 0x00020006,
		Floating = 0x00020007,
		Maximized = 0x00020008,
		CenterCursor = 0x00020009,
		TransparentFramebuffer = 0x0002000A,
		Hovered = 0x0002000B,
		FocusOnShow = 0x0002000C,
		RedBits = 0x00021001,
		GreenBits = 0x00021002,
		BlueBits = 0x00021003,
		AlphaBits = 0x00021004,
		DepthBits = 0x00021005,
		StencilBits = 0x00021006,
		AccumRedBits = 0x00021007,
		AccumGreenBits = 0x00021008,
		AccumBlueBits = 0x00021009,
		AccumAlphaBits = 0x0002100A,
		AuxBuffers = 0x0002100B,
		Stereo = 0x0002100C,
		Samples = 0x0002100D,
		SRGBCapable = 0x0002100E,
		RefreshRate = 0x0002100F,
		DoubleBuffer = 0x00021010,
		ClientAPI = 0x00022001,
		ContextVersionMajor = 0x00022002,
		ContextVersionMinor = 0x00022003,
		ContextRevision = 0x00022004,
		ContextRobustness = 0x00022005,
		OpenGLForwardCompat = 0x00022006,
		OpenGLDebugContext = 0x00022007,
		OpenGLProfile = 0x00022008,
		ContextReleaseBehavior = 0x00022009,
		ContextNoError = 0x0002200A,
		ContextCreationAPI = 0x0002200B,
		ScaleToMonitor = 0x0002200C,
		CocoaRetinaFramebuffer = 0x00023001,
		CocoaFrameName = 0x00023002,
		CocoaGraphicsSwitching = 0x00023003,
		X11ClassName = 0x00024001,
		X11InstanceName = 0x00024002
	}

	public enum GLFWClientAPI {
		NoAPI = 0,
		OpenGLAPI = 0x00030001,
		OpenGLESAPI = 0x00030002
	}

	public enum GLFWContextRobustness {
		NoRobustness = 0,
		NoResetNotification = 0x00031001,
		LoseContextOnReset = 0x00031002
	}

	public enum GLFWOpenGLProfile {
		AnyProfile = 0,
		CoreProfile = 0x00032001,
		CompatProfile = 0x00032002
	}

	public enum GLFWInputMode : int {
		Cursor = 0x00033001,
		StickyKeys = 0x00033002,
		StickyMouseButtons = 0x00033003,
		LockKeyMods = 0x00033004,
		RawMouseMotion = 0x00033005
	}

	public enum GLFWCursorMode {
		Normal = 0x00034001,
		Hidden = 0x00034002,
		Disabled = 0x00034003
	}

	public enum GLFWContextReleaseBehavior {
		Any = 0,
		Flush = 0x00035001,
		None = 0x00035002
	}

	public enum GLFWContextCreationAPI {
		Native = 0x00036001,
		EGL = 0x00036002,
		OSMesa = 0x00036003
	}

	public enum GLFWCursorShape : int {
		Arrow = 0x00036001,
		IBeam = 0x00036002,
		Crosshair = 0x00036003,
		Hand = 0x00036004,
		HResize = 0x00036005,
		VResize = 0x00036006
	}

	public enum GLFWConnectState {
		Connected = 0x00040001,
		Disconnected = 0x00040002
	}

	public enum GLFWInitHint : int {
		JoystickHatButtons = 0x00050001,
		CocoaChdirResources = 0x00051001,
		CocoaMenubar = 0x00051002
	}

}
