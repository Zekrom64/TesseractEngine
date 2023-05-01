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

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_JoystickNameForIndex(DeviceIndex))!;
				}
			}
		}

		public int PlayerIndex {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDevicePlayerIndex(DeviceIndex);
				}
			}
		}

		public Guid GUID {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDeviceGUID(DeviceIndex);
				}
			}
		}

		public ushort Vendor {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDeviceVendor(DeviceIndex);
				}
			}
		}

		public ushort Product {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDeviceProduct(DeviceIndex);
				}
			}
		}

		public SDLJoystickType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDeviceType(DeviceIndex);
				}
			}
		}

		public int InstanceID {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetDeviceInstanceID(DeviceIndex);
				}
			}
		}

		public SDLJoystick Open() {
			unsafe {
				IntPtr pJoy = SDL2.Functions.SDL_JoystickOpen(DeviceIndex);
				if (pJoy == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLJoystick(pJoy);
			}
		}

		public void DetachVirtual() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_JoystickDetachVirtual(DeviceIndex));
			}
		}

		public bool IsVirtual {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickIsVirtual(DeviceIndex);
				}
			}
		}

		public bool IsGameController {
			get {
				unsafe {
					return SDL2.Functions.SDL_IsGameController(DeviceIndex);
				}
			}
		}

		public SDLGameControllerDevice GameController {
			get {
				return new() {
					DeviceIndex = DeviceIndex
				};
			}
		}

	}

	public class SDLJoystick : IDisposable {

		[NativeType("SDL_Joystick*")]
		public IntPtr Joystick { get; }

		internal SDLJoystick(IntPtr pJoystick) {
			Joystick = pJoystick;
		}

		public static SDLJoystick? FromInstanceID(int instanceID) {
			unsafe {
				IntPtr pJoy = SDL2.Functions.SDL_JoystickFromInstanceID(instanceID);
				if (pJoy == IntPtr.Zero) return null;
				return new SDLJoystick(pJoy);
			}
		}

		public static SDLJoystick? FromPlayerIndex(int playerIndex) {
			unsafe {
				IntPtr pJoy = SDL2.Functions.SDL_JoystickFromPlayerIndex(playerIndex);
				if (pJoy == IntPtr.Zero) return null;
				return new SDLJoystick(pJoy);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				SDL2.Functions.SDL_JoystickClose(Joystick);
			}
		}

		public void SetVirtualAxis(int axis, short value) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualAxis(Joystick, axis, value));
			}
		}

		public void SetVirtualButton(int button, SDLButtonState state) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualButton(Joystick, button, state));
			}
		}

		public void SetVirtualHat(int hat, SDLHat state) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_JoystickSetVirtualHat(Joystick, hat, state));
			}
		}

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_JoystickName(Joystick))!;
				}
			}
		}

		public int PlayerIndex {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetPlayerIndex(Joystick);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_JoystickSetPlayerIndex(Joystick, value);
				}
			}
		}

		public Guid GUID {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetGUID(Joystick);
				}
			}
		}

		public ushort Vendor {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetVendor(Joystick);
				}
			}
		}

		public ushort Product {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetProduct(Joystick);
				}
			}
		}

		public ushort ProductVersion {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetProductVersion(Joystick);
				}
			}
		}

		public string? Serial {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_JoystickGetSerial(Joystick));
				}
			}
		}

		public SDLJoystickType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetType(Joystick);
				}
			}
		}

		public bool Attached {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickGetAttached(Joystick);
				}
			}
		}

		public int InstanceID {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickInstanceID(Joystick);
				}
			}
		}

		public int NumAxes {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickNumAxes(Joystick);
				}
			}
		}

		public int NumBalls {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickNumBalls(Joystick);
				}
			}
		}

		public int NumHats {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickNumHats(Joystick);
				}
			}
		}

		public int NumButtons {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickNumButtons(Joystick);
				}
			}
		}

		public short GetAxis(int axis) {
			unsafe {
				return SDL2.Functions.SDL_JoystickGetAxis(Joystick, axis);
			}
		}

		public short? GetAxisInitialState(int axis) {
			unsafe {
				if (!SDL2.Functions.SDL_JoystickGetAxisInitialState(Joystick, axis, out short state)) return null;
				else return state;
			}
		}

		public SDLHat GetHat(int hat) {
			unsafe {
				return SDL2.Functions.SDL_JoystickGetHat(Joystick, hat);
			}
		}

		public void GetBall(int ball, out int dx, out int dy) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_JoystickGetBall(Joystick, ball, out dx, out dy));
			}
		}

		public SDLButtonState GetButton(int button) {
			unsafe {
				return SDL2.Functions.SDL_JoystickGetButton(Joystick, button);
			}
		}

		public void Rumble(ushort lowFreqRumble, ushort highFreqRumble, uint durationMillisec) {
			unsafe {
				SDL2.Functions.SDL_JoystickRumble(Joystick, lowFreqRumble, highFreqRumble, durationMillisec);
			}
		}

		public void RumbleTriggers(ushort leftRumble, ushort rightRumble, uint durationMillisec) {
			unsafe {
				SDL2.Functions.SDL_JoystickRumbleTriggers(Joystick, leftRumble, rightRumble, durationMillisec);
			}
		}

		public bool HasLED {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickHasLED(Joystick);
				}
			}
		}

		public void SetLED(byte red, byte green, byte blue) {
			unsafe {
				SDL2.Functions.SDL_JoystickSetLED(Joystick, red, green, blue);
			}
		}

		public SDLJoystickPowerLevel CurrentPowerLevel {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickCurrentPowerLevel(Joystick);
				}
			}
		}

		public bool IsHaptic {
			get {
				unsafe {
					return SDL2.Functions.SDL_JoystickIsHaptic(Joystick) > 0;
				}
			}
		}
	}

}
