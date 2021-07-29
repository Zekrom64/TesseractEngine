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

		public string Mapping {
			get {
				IntPtr pMapping = SDL2.Functions.SDL_GameControllerMappingForIndex(MappingIndex);
				string mapping = MemoryUtil.GetStringASCII(pMapping);
				SDL2.Functions.SDL_free(pMapping);
				return mapping;
			}
		}

	}

	public struct SDLGameControllerDevice {

		public int DeviceIndex { get; set; }

		public string Name => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_GameControllerNameForIndex(DeviceIndex));

		public SDLGameControllerType Type => SDL2.Functions.SDL_GameControllerTypeForIndex(DeviceIndex);

		public string Mapping {
			get {
				IntPtr pMapping = SDL2.Functions.SDL_GameControllerMappingForDeviceIndex(DeviceIndex);
				string mapping = MemoryUtil.GetStringASCII(pMapping);
				SDL2.Functions.SDL_free(pMapping);
				return mapping;
			}
		}

		public SDLGameController Open() {
			IntPtr pGameController = SDL2.Functions.SDL_GameControllerOpen(DeviceIndex);
			if (pGameController == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLGameController(pGameController);
		}

	}

	public class SDLGameController : IDisposable {

		public IPointer<SDL_GameController> GameController { get; }

		public string Name => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_GameControllerName(GameController.Ptr));

		public SDLGameControllerType Type => SDL2.Functions.SDL_GameControllerGetType(GameController.Ptr);

		public int PlayerIndex {
			get => SDL2.Functions.SDL_GameControllerGetPlayerIndex(GameController.Ptr);
			set => SDL2.Functions.SDL_GameControllerSetPlayerIndex(GameController.Ptr, value);
		}

		public ushort Vendor => SDL2.Functions.SDL_GameControllerGetVendor(GameController.Ptr);

		public ushort Product => SDL2.Functions.SDL_GameControllerGetProduct(GameController.Ptr);

		public ushort ProductVersion => SDL2.Functions.SDL_GameControllerGetProductVersion(GameController.Ptr);

		public string Serial => MemoryUtil.GetStringASCII(SDL2.Functions.SDL_GameControllerGetSerial(GameController.Ptr));

		public bool Attached => SDL2.Functions.SDL_GameControllerGetAttached(GameController.Ptr);

		public SDLJoystick Joystick => new(SDL2.Functions.SDL_GameControllerGetJoystick(GameController.Ptr));

		public string Mapping {
			get {
				IntPtr pMapping = SDL2.Functions.SDL_GameControllerMapping(GameController.Ptr);
				string mapping = MemoryUtil.GetStringASCII(pMapping);
				SDL2.Functions.SDL_free(pMapping);
				return mapping;
			}
		}

		public SDLGameController(IntPtr pGameController) {
			GameController = new UnmanagedPointer<SDL_GameController>(pGameController);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			SDL2.Functions.SDL_GameControllerClose(GameController.Ptr);
		}

		public static SDLGameController FromInstanceID(int instanceID) {
			IntPtr pGC = SDL2.Functions.SDL_GameControllerFromInstanceID(instanceID);
			if (pGC == IntPtr.Zero) return null;
			return new SDLGameController(pGC);
		}

		public static SDLGameController FromPlayerIndex(int playerIndex) {
			IntPtr pGC = SDL2.Functions.SDL_GameControllerFromPlayerIndex(playerIndex);
			if (pGC == IntPtr.Zero) return null;
			return new SDLGameController(pGC);
		}

		public SDLGameControllerButtonBind GetBind(SDLGameControllerAxis axis) => SDL2.Functions.SDL_GameControllerGetBindForAxis(GameController.Ptr, axis);

		public bool HasAxis(SDLGameControllerAxis axis) => SDL2.Functions.SDL_GameControllerHasAxis(GameController.Ptr, axis);

		public short GetAxis(SDLGameControllerAxis axis) => SDL2.Functions.SDL_GameControllerGetAxis(GameController.Ptr, axis);

		public SDLGameControllerButtonBind GetBind(SDLGameControllerButton button) => SDL2.Functions.SDL_GameControllerGetBindForButton(GameController.Ptr, button);

		public bool HasButton(SDLGameControllerButton button) => SDL2.Functions.SDL_GameControllerHasButton(GameController.Ptr, button);

		public bool GetButton(SDLGameControllerButton button) => SDL2.Functions.SDL_GameControllerGetButton(GameController.Ptr, button) != SDLButtonState.Released;

		public int NumTouchpads => SDL2.Functions.SDL_GameControllerGetNumTouchpads(GameController.Ptr);

		public int GetNumTouchpadFingers(int touchpad) => SDL2.Functions.SDL_GameControllerGetNumTouchpadFingers(GameController.Ptr, touchpad);

		public (bool, Vector2, float) GetTouchpadFinger(int touchpad, int finger) {
			SDL2.Functions.SDL_GameControllerGetTouchpadFinger(GameController.Ptr, touchpad, finger, out SDLButtonState state, out float x, out float y, out float pressure);
			return (state != SDLButtonState.Released, new Vector2(x, y), pressure);
		}

		public bool HasSensor(SDLSensorType type) => SDL2.Functions.SDL_GameControllerHasSensor(GameController.Ptr, type);

		public void SetSensorEnabled(SDLSensorType type, bool enable) => SDL2.Functions.SDL_GameControllerSetSensorEnabled(GameController.Ptr, type, enable);

		public bool GetSensorEnabled(SDLSensorType type) => SDL2.Functions.SDL_GameControllerIsSensorEnabled(GameController.Ptr, type);

		public Span<float> GetSensorData(SDLSensorType type, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					SDL2.Functions.SDL_GameControllerGetSensorData(GameController.Ptr, type, (IntPtr)pValues, values.Length);
				}
			}
			return values;
		}

		public void Rumble(ushort lowFreqRumble, ushort highFreqRumble, uint durationMillisec) => SDL2.Functions.SDL_GameControllerRumble(GameController.Ptr, lowFreqRumble, highFreqRumble, durationMillisec);

		public void RumbleTriggers(ushort leftRumble, ushort rightRumble, uint durationMillisec) => SDL2.Functions.SDL_GameControllerRumbleTriggers(GameController.Ptr, leftRumble, rightRumble, durationMillisec);

		public bool HasLED => SDL2.Functions.SDL_GameControllerHasLED(GameController.Ptr);

		public void SetLED(byte r, byte g, byte b) => SDL2.Functions.SDL_GameControllerSetLED(GameController.Ptr, r, g, b);

	}

}
