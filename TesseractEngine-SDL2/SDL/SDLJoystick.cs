using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {
	
	public enum SDLJoystickType {
		Unknown,
		GameController,
		Wheel,
		ArcadeStick,
		FlightStick,
		DancePad,
		Guitar,
		DrumKit,
		ArcadePad,
		Throttle
	}

	public enum SDLJoystickPowerLevel {
		Unknown = -1,
		Empty,
		Low,
		Medium,
		Full,
		Wired
	}

	public enum SDLHat : byte {
		Centered = 0,
		Up = 0x01,
		Right = 0x02,
		Down = 0x04,
		Left = 0x08,
		RightUp = Right | Up,
		RightDown = Right | Down,
		LeftUp = Left | Up,
		LeftDown = Left | Down
	}

	public struct SDLJoystickDevice {

		public int DeviceIndex { get; init; }

		public string Name => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_JoystickNameForIndex(DeviceIndex));

		public int PlayerIndex => SDL2.Functions.SDL_JoystickGetDevicePlayerIndex(DeviceIndex);

		public Guid GUID => SDL2.Functions.SDL_JoystickGetDeviceGUID(DeviceIndex);

		public ushort Vendor => SDL2.Functions.SDL_JoystickGetDeviceVendor(DeviceIndex);

		public ushort Product => SDL2.Functions.SDL_JoystickGetDeviceProduct(DeviceIndex);

		public SDLJoystickType Type => SDL2.Functions.SDL_JoystickGetDeviceType(DeviceIndex);

		public int InstanceID => SDL2.Functions.SDL_JoystickGetDeviceInstanceID(DeviceIndex);

		public SDLJoystick Open() {
			IntPtr pJoy = SDL2.Functions.SDL_JoystickOpen(DeviceIndex);
			if (pJoy == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLJoystick(pJoy);
		}

		public void DetachVirtual() => SDL2.CheckError(SDL2.Functions.SDL_JoystickDetachVirtual(DeviceIndex));

		public bool IsVirtual => SDL2.Functions.SDL_JoystickIsVirtual(DeviceIndex);

		public bool IsGameController => SDL2.Functions.SDL_IsGameController(DeviceIndex);

		public SDLGameControllerDevice GameController => new() { DeviceIndex = DeviceIndex };

	}

	public class SDLJoystick : IDisposable {

		public IPointer<SDL_Joystick> Joystick { get; }

		internal SDLJoystick(IntPtr pJoystick) {
			Joystick = new UnmanagedPointer<SDL_Joystick>(pJoystick);
		}

		public static SDLJoystick FromInstanceID(int instanceID) {
			IntPtr pJoy = SDL2.Functions.SDL_JoystickFromInstanceID(instanceID);
			if (pJoy == IntPtr.Zero) return null;
			return new SDLJoystick(pJoy);
		}

		public static SDLJoystick FromPlayerIndex(int playerIndex) {
			IntPtr pJoy = SDL2.Functions.SDL_JoystickFromPlayerIndex(playerIndex);
			if (pJoy == IntPtr.Zero) return null;
			return new SDLJoystick(pJoy);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			SDL2.Functions.SDL_JoystickClose(Joystick.Ptr);
		}

		public void SetVirtualAxis(int axis, short value) => SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualAxis(Joystick.Ptr, axis, value));

		public void SetVirtualButton(int button, SDLButtonState state) => SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualButton(Joystick.Ptr, button, state));

		public void SetVirtualHat(int hat, SDLButtonState state) => SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualHat(Joystick.Ptr, hat, state));

		public string Name => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_JoystickName(Joystick.Ptr));

		public int PlayerIndex {
			get => SDL2.Functions.SDL_JoystickGetPlayerIndex(Joystick.Ptr);
			set => SDL2.Functions.SDL_JoystickSetPlayerIndex(Joystick.Ptr, value);
		}

		public Guid GUID => SDL2.Functions.SDL_JoystickGetGUID(Joystick.Ptr);

		public ushort Vendor => SDL2.Functions.SDL_JoystickGetVendor(Joystick.Ptr);

		public ushort Product => SDL2.Functions.SDL_JoystickGetProduct(Joystick.Ptr);

		public ushort ProductVersion => SDL2.Functions.SDL_JoystickGetProductVersion(Joystick.Ptr);

		public string Serial => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_JoystickGetSerial(Joystick.Ptr));

		public SDLJoystickType Type => SDL2.Functions.SDL_JoystickGetType(Joystick.Ptr);

		public bool Attached => SDL2.Functions.SDL_JoystickGetAttached(Joystick.Ptr);

		public int InstanceID => SDL2.Functions.SDL_JoystickInstanceID(Joystick.Ptr);

		public int NumAxes => SDL2.Functions.SDL_JoystickNumAxes(Joystick.Ptr);

		public int NumBalls => SDL2.Functions.SDL_JoystickNumBalls(Joystick.Ptr);

		public int NumHats => SDL2.Functions.SDL_JoystickNumHats(Joystick.Ptr);

		public int NumButtons => SDL2.Functions.SDL_JoystickNumButtons(Joystick.Ptr);

		public short GetAxis(int axis) => SDL2.Functions.SDL_JoystickGetAxis(Joystick.Ptr, axis);

		public short? GetAxisInitialState(int axis) {
			if (!SDL2.Functions.SDL_JoystickGetAxisInitialState(Joystick.Ptr, axis, out short state)) return null;
			else return state;
		}

		public SDLHat GetHat(int hat) => SDL2.Functions.SDL_JoystickGetHat(Joystick.Ptr, hat);

		public void GetBall(int ball, out int dx, out int dy) => SDL2.CheckError(SDL2.Functions.SDL_JoystickGetBall(Joystick.Ptr, ball, out dx, out dy));

		public SDLButtonState GetButton(int button) => SDL2.Functions.SDL_JoystickGetButton(Joystick.Ptr, button);

		public void Rumble(ushort lowFreqRumble, ushort highFreqRumble, uint durationMillisec) => SDL2.Functions.SDL_JoystickRumble(Joystick.Ptr, lowFreqRumble, highFreqRumble, durationMillisec);

		public void RumbleTriggers(ushort leftRumble, ushort rightRumble, uint durationMillisec) => SDL2.Functions.SDL_JoystickRumbleTriggers(Joystick.Ptr, leftRumble, rightRumble, durationMillisec);

		public bool HasLED => SDL2.Functions.SDL_JoystickHasLED(Joystick.Ptr);

		public void SetLED(byte red, byte green, byte blue) => SDL2.Functions.SDL_JoystickSetLED(Joystick.Ptr, red, green, blue);

		public SDLJoystickPowerLevel CurrentPowerLevel => SDL2.Functions.SDL_JoystickCurrentPowerLevel(Joystick.Ptr);

	}

}
