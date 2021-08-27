using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.SDL {
	
	public enum SDLEventType : uint {
		FirstEvent = 0,

		Quit = 0x100,

		AppTerminating,
		AppLowMemory,
		AppWillEnterBackground,
		AppDidEnterBackground,
		AppWillEnterForeground,
		AppDidEnterForeground,

		DisplayEvent = 0x150,
		
		WindowEvent = 0x200,
		SysWMEvent,

		KeyDown = 0x300,
		KeyUp,
		TextEditing,
		TextInput,
		KeyMapChanged,

		MouseMotion = 0x400,
		MouseButtonDown,
		MouseButtonUp,
		MouseWheel,

		JoyAxisMotion = 0x600,
		JoyBallMotion,
		JoyHatMotion,
		JoyButtonDown,
		JoyButtonUp,
		JoyDeviceAdded,
		JoyDeviceRemoved,

		ControllerAxisMotion = 0x650,
		ControllerButtonDown,
		ControllerButtonUp,
		ControllerDeviceAdded,
		ControllerDeviceRemoved,
		ControllerDeviceRemapped,

		FingerDown = 0x700,
		FingerUp,
		FingerMotion,

		DollarGesture = 0x800,
		DollarRecord,
		MultiGesture,

		ClipboardUpdate = 0x900,

		DropFile = 0x1000,
		DropText,
		DropBegin,
		DropComplete,

		AudioDeviceAdded = 0x1100,
		AudioDeviceRemoved,

		SensorUpdate = 0x1200,

		RenderTargetsReset = 0x2000,
		RenderDeviceReset,

		UserEvent = 0x8000,

		LastEvent = 0xFFFF
	}

	public enum SDLButtonState : byte {
		Released = 0,
		Pressed = 1
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLCommonEvent {
		public SDLEventType Type;
		public uint Timestamp;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLDisplayEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint Display;
		public SDLDisplayEventID Event;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
		public int Data1;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLWindowEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public SDLWindowEventID Event;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
		public int Data1;
		public int Data2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLKeyboardEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public SDLButtonState State;
		[MarshalAs(UnmanagedType.U1)]
		public byte Repeat;
		private readonly byte padding2;
		private readonly byte padding3;
		public SDLKeysym Keysym;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLTextEditingEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		// char text[32]
		private uint text0;
		private readonly uint text1;
		private readonly uint text2;
		private readonly uint text3;
		private readonly uint text4;
		private readonly uint text5;
		private readonly uint text6;
		private readonly uint text7;
		public string Text {
			get {
				unsafe {
					ref uint u = ref text0;
					fixed (uint* p = &u) {
						return MemoryUtil.GetUTF8((IntPtr)p, 32);
					}
				}
			}
			set {
				unsafe {
					ref uint u = ref text0;
					fixed (uint* p = &u) {
						MemoryUtil.PutUTF8(value, (IntPtr)p, 32);
					}
				}
			}
		}
		public int Start;
		public int Length;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLTextInputEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		// char text[32]
		private uint text0;
		private readonly uint text1;
		private readonly uint text2;
		private readonly uint text3;
		private readonly uint text4;
		private readonly uint text5;
		private readonly uint text6;
		private readonly uint text7;
		public string Text {
			get {
				unsafe {
					ref uint u = ref text0;
					fixed (uint* p = &u) {
						return MemoryUtil.GetUTF8((IntPtr)p, 32);
					}
				}
			}
			set {
				unsafe {
					ref uint u = ref text0;
					fixed (uint* p = &u) {
						MemoryUtil.PutUTF8(value, (IntPtr)p, 32);
					}
				}
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMouseMotionEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public uint Which;
		public SDLMouseButtonState State;
		public int X;
		public int Y;
		public int XRel;
		public int YRel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMouseButtonEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public uint Which;
		public byte Button;
		public SDLButtonState State;
		public byte Clicks;
		private readonly byte padding1;
		public int X;
		public int Y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMouseWheelEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public uint Which;
		public int X;
		public int Y;
		public SDLMouseWheelDirection Direction;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLJoyAxisEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public byte Axis;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
		public short Value;
		private readonly ushort padding4;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLJoyBallEvent {
		public SDLEventType type;
		public uint Timestamp;
		public int Which;
		public byte Ball;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
		public short XRel;
		public short YRel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLJoyHatEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public byte Hat;
		public SDLHat Value;
		private readonly byte padding1;
		private readonly byte padding2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLJoyButtonEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public byte Button;
		public SDLButtonState State;
		private readonly byte padding1;
		private readonly byte padding2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLJoyDeviceEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLControllerAxisEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public SDLGameControllerAxis Axis;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
		public short Value;
		private readonly ushort padding4;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLControllerButtonEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public SDLGameControllerButton Button;
		public SDLButtonState State;
		private readonly byte padding1;
		private readonly byte padding2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLControllerDeviceEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLControllerTouchpadEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public int Touchpad;
		public int Finger;
		public float X;
		public float Y;
		public float Pressure;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLControllerSensorEvent {
		public SDLEventType type;
		public uint Timestamp;
		public int Which;
		public int Sensor;
		public Vector3 Data;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLAudioDeviceEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint Which;
		[MarshalAs(UnmanagedType.U1)]
		public bool IsCapture;
		private readonly byte padding1;
		private readonly byte padding2;
		private readonly byte padding3;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLTouchFingerEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public long TouchID;
		public long FingerID;
		public float X;
		public float Y;
		public float DX;
		public float DY;
		public float Pressure;
		public uint WindowID;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLMultiGestureEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public long TouchID;
		public float DTheta;
		public float DDist;
		public float X;
		public float Y;
		public ushort NumFingers;
		private readonly ushort padding;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLDollarGestureEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public long TouchID;
		public long GestureID;
		public uint NumFingers;
		public float Error;
		public float X;
		public float Y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLDropEvent {
		public SDLEventType Type;
		public uint Timestamp;
		[NativeType("char*")]
		public IntPtr File;
		public string FileStr => MemoryUtil.GetASCII(File);
		public uint WindowID;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLSensorEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public int Which;
		public float Data0;
		public float Data1;
		public float Data2;
		public float Data3;
		public float Data4;
		public float Data5;
		public float this[int index] {
			get => index switch {
				0 => Data0,
				1 => Data1,
				2 => Data2,
				3 => Data3,
				4 => Data4,
				5 => Data5,
				_ => 0
			};
			set {
				switch(index) {
					case 0: Data0 = value; break;
					case 1: Data1 = value; break;
					case 2: Data2 = value; break;
					case 3: Data3 = value; break;
					case 4: Data4 = value; break;
					case 5: Data5 = value; break;
				}
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLQuitEvent {
		public SDLEventType Type;
		public uint Timestamp;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLOSEvent {
		public SDLEventType Type;
		public uint Timestamp;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLUserEvent {
		public SDLEventType Type;
		public uint Timestamp;
		public uint WindowID;
		public int Code;
		public IntPtr Data1;
		public IntPtr Data2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLSysWMEvent {
		public SDLEventType Type;
		public uint Timestamp;
		[NativeType("SDL_SysWMmsg*")]
		public IntPtr Msg;
	}

	[StructLayout(LayoutKind.Explicit, Size = 56)]
	public struct SDLEvent {
		[FieldOffset(0)]
		public SDLEventType Type;
		[FieldOffset(0)]
		public SDLCommonEvent Common;
		[FieldOffset(0)]
		public SDLDisplayEvent Display;
		[FieldOffset(0)]
		public SDLWindowEvent Window;
		[FieldOffset(0)]
		public SDLKeyboardEvent Key;
		[FieldOffset(0)]
		public SDLTextEditingEvent Edit;
		[FieldOffset(0)]
		public SDLTextInputEvent Text;
		[FieldOffset(0)]
		public SDLMouseMotionEvent Motion;
		[FieldOffset(0)]
		public SDLMouseButtonEvent Button;
		[FieldOffset(0)]
		public SDLMouseWheelEvent Wheel;
		[FieldOffset(0)]
		public SDLJoyAxisEvent JAxis;
		[FieldOffset(0)]
		public SDLJoyBallEvent JBall;
		[FieldOffset(0)]
		public SDLJoyHatEvent JHat;
		[FieldOffset(0)]
		public SDLJoyButtonEvent JButton;
		[FieldOffset(0)]
		public SDLJoyDeviceEvent JDevice;
		[FieldOffset(0)]
		public SDLControllerAxisEvent CAxis;
		[FieldOffset(0)]
		public SDLControllerButtonEvent CButton;
		[FieldOffset(0)]
		public SDLControllerDeviceEvent CDevice;
		[FieldOffset(0)]
		public SDLControllerTouchpadEvent CTouchpad;
		[FieldOffset(0)]
		public SDLControllerSensorEvent CSensor;
		[FieldOffset(0)]
		public SDLAudioDeviceEvent ADevice;
		[FieldOffset(0)]
		public SDLSensorEvent Sensor;
		[FieldOffset(0)]
		public SDLQuitEvent Quit;
		[FieldOffset(0)]
		public SDLUserEvent User;
		[FieldOffset(0)]
		public SDLSysWMEvent SysWM;
		[FieldOffset(0)]
		public SDLTouchFingerEvent TFinger;
		[FieldOffset(0)]
		public SDLMultiGestureEvent MGesture;
		[FieldOffset(0)]
		public SDLDollarGestureEvent DGesture;
		[FieldOffset(0)]
		public SDLDropEvent Drop;
	}

	public enum SDLEventState : int {
		Query = -1,
		Disable = 0,
		Enable = 1
	}

	public enum SDLEventAction {
		AddEvent,
		PeekEvent,
		GetEvent
	}

	public delegate int SDLEventFilter(IntPtr userdata, in SDLEvent _event);

}
