using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

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

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLGameControllerButtonBind {

		public SDLGameControllerBindType BindType;

		[StructLayout(LayoutKind.Explicit)]
		public struct SDLGameControllerButtonBindValue {

			[FieldOffset(0)]
			public int Button;

			[FieldOffset(0)]
			public int Axis;

			[StructLayout(LayoutKind.Sequential)]
			public struct SDLGameControllerButtonBindValueHat {

				public int Hat;

				public int HatMask;

			}

			[FieldOffset(0)]
			public SDLGameControllerButtonBindValueHat Hat;

		}

		public SDLGameControllerButtonBindValue Value;

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

	public struct SDLGameControllerMapping {

		public int MappingIndex { get; set; }

		public string? Mapping {
			get {
				unsafe {
					IntPtr pMapping = SDL2.Functions.SDL_GameControllerMappingForIndex(MappingIndex);
					string? mapping = MemoryUtil.GetASCII(pMapping);
					SDL2.Functions.SDL_free(pMapping);
					return mapping;
				}
			}
		}

	}

	public struct SDLGameControllerDevice {

		public int DeviceIndex { get; set; }

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_GameControllerNameForIndex(DeviceIndex))!;
				}
			}
		}

		public SDLGameControllerType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerTypeForIndex(DeviceIndex);
				}
			}
		}

		public string? Mapping {
			get {
				unsafe {
					IntPtr pMapping = SDL2.Functions.SDL_GameControllerMappingForDeviceIndex(DeviceIndex);
					string? mapping = MemoryUtil.GetASCII(pMapping);
					SDL2.Functions.SDL_free(pMapping);
					return mapping;
				}
			}
		}

		public SDLGameController Open() {
			unsafe {
				IntPtr pGameController = SDL2.Functions.SDL_GameControllerOpen(DeviceIndex);
				if (pGameController == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLGameController(pGameController);
			}
		}

	}

	public class SDLGameController : IDisposable {

		[NativeType("SDL_GameController*")]
		public IntPtr GameController { get; }

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_GameControllerName(GameController))!;
				}
			}
		}

		public SDLGameControllerType Type {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetType(GameController);
				}
			}
		}

		public int PlayerIndex {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetPlayerIndex(GameController);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_GameControllerSetPlayerIndex(GameController, value);
				}
			}
		}

		public ushort Vendor {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetVendor(GameController);
				}
			}
		}

		public ushort Product {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetProduct(GameController);
				}
			}
		}

		public ushort ProductVersion {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetProductVersion(GameController);
				}
			}
		}

		public string? Serial {
			get {
				unsafe {
					return MemoryUtil.GetASCII(SDL2.Functions.SDL_GameControllerGetSerial(GameController));
				}
			}
		}

		public bool Attached {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetAttached(GameController);
				}
			}
		}

		public SDLJoystick Joystick {
			get {
				unsafe {
					return new(SDL2.Functions.SDL_GameControllerGetJoystick(GameController));
				}
			}
		}

		public string? Mapping {
			get {
				unsafe {
					IntPtr pMapping = SDL2.Functions.SDL_GameControllerMapping(GameController);
					string? mapping = MemoryUtil.GetASCII(pMapping);
					SDL2.Functions.SDL_free(pMapping);
					return mapping;
				}
			}
		}

		public SDLGameController([NativeType("SDL_GameController*")] IntPtr pGameController) {
			GameController = pGameController;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				SDL2.Functions.SDL_GameControllerClose(GameController);
			}
		}

		public static SDLGameController? FromInstanceID(int instanceID) {
			unsafe {
				IntPtr pGC = SDL2.Functions.SDL_GameControllerFromInstanceID(instanceID);
				if (pGC == IntPtr.Zero) return null;
				return new SDLGameController(pGC);
			}
		}

		public static SDLGameController? FromPlayerIndex(int playerIndex) {
			unsafe {
				IntPtr pGC = SDL2.Functions.SDL_GameControllerFromPlayerIndex(playerIndex);
				if (pGC == IntPtr.Zero) return null;
				return new SDLGameController(pGC);
			}
		}

		public SDLGameControllerButtonBind GetBind(SDLGameControllerAxis axis) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerGetBindForAxis(GameController, axis);
			}
		}

		public bool HasAxis(SDLGameControllerAxis axis) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerHasAxis(GameController, axis);
			}
		}

		public short GetAxis(SDLGameControllerAxis axis) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerGetAxis(GameController, axis);
			}
		}

		public SDLGameControllerButtonBind GetBind(SDLGameControllerButton button) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerGetBindForButton(GameController, button);
			}
		}

		public bool HasButton(SDLGameControllerButton button) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerHasButton(GameController, button);
			}
		}

		public bool GetButton(SDLGameControllerButton button) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerGetButton(GameController, button) != SDLButtonState.Released;
			}
		}

		public int NumTouchpads {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerGetNumTouchpads(GameController);
				}
			}
		}

		public int GetNumTouchpadFingers(int touchpad) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerGetNumTouchpadFingers(GameController, touchpad);
			}
		}

		public (bool, Vector2, float) GetTouchpadFinger(int touchpad, int finger) {
			unsafe {
				SDL2.Functions.SDL_GameControllerGetTouchpadFinger(GameController, touchpad, finger, out SDLButtonState state, out float x, out float y, out float pressure);
				return (state != SDLButtonState.Released, new Vector2(x, y), pressure);
			}
		}

		public bool HasSensor(SDLSensorType type) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerHasSensor(GameController, type);
			}
		}

		public void SetSensorEnabled(SDLSensorType type, bool enable) {
			unsafe {
				SDL2.Functions.SDL_GameControllerSetSensorEnabled(GameController, type, enable);
			}
		}

		public bool GetSensorEnabled(SDLSensorType type) {
			unsafe {
				return SDL2.Functions.SDL_GameControllerIsSensorEnabled(GameController, type);
			}
		}

		public Span<float> GetSensorData(SDLSensorType type, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					SDL2.Functions.SDL_GameControllerGetSensorData(GameController, type, pValues, values.Length);
				}
			}
			return values;
		}

		public void Rumble(ushort lowFreqRumble, ushort highFreqRumble, uint durationMillisec) {
			unsafe {
				SDL2.Functions.SDL_GameControllerRumble(GameController, lowFreqRumble, highFreqRumble, durationMillisec);
			}
		}

		public void RumbleTriggers(ushort leftRumble, ushort rightRumble, uint durationMillisec) {
			unsafe {
				SDL2.Functions.SDL_GameControllerRumbleTriggers(GameController, leftRumble, rightRumble, durationMillisec);
			}
		}

		public bool HasLED {
			get {
				unsafe {
					return SDL2.Functions.SDL_GameControllerHasLED(GameController);
				}
			}
		}

		public void SetLED(byte r, byte g, byte b) {
			unsafe {
				SDL2.Functions.SDL_GameControllerSetLED(GameController, r, g, b);
			}
		}
	}

}
