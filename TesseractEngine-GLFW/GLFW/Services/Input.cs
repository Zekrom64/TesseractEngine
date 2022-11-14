using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;
using Tesseract.Core.Numerics;
using Tesseract.Core.Services;

namespace Tesseract.GLFW.Services {

	public class GLFWServiceKeyboard : IKeyboard {

		public static readonly IReadOnlyDictionary<GLFWKey, Key> GLFWToStdKey = new Dictionary<GLFWKey, Key>() {
			{ GLFWKey.A, Key.A },
			{ GLFWKey.B, Key.B },
			{ GLFWKey.C, Key.C },
			{ GLFWKey.D, Key.D },
			{ GLFWKey.E, Key.E },
			{ GLFWKey.F, Key.F },
			{ GLFWKey.G, Key.G },
			{ GLFWKey.H, Key.H },
			{ GLFWKey.I, Key.I },
			{ GLFWKey.J, Key.J },
			{ GLFWKey.K, Key.K },
			{ GLFWKey.L, Key.L },
			{ GLFWKey.M, Key.M },
			{ GLFWKey.N, Key.N },
			{ GLFWKey.O, Key.O },
			{ GLFWKey.P, Key.P },
			{ GLFWKey.Q, Key.Q },
			{ GLFWKey.R, Key.R },
			{ GLFWKey.S, Key.S },
			{ GLFWKey.T, Key.T },
			{ GLFWKey.U, Key.U },
			{ GLFWKey.V, Key.V },
			{ GLFWKey.W, Key.W },
			{ GLFWKey.X, Key.X },
			{ GLFWKey.Y, Key.Y },
			{ GLFWKey.Z, Key.Z },

			{ GLFWKey._0, Key._0 },
			{ GLFWKey._1, Key._1 },
			{ GLFWKey._2, Key._2 },
			{ GLFWKey._3, Key._3 },
			{ GLFWKey._4, Key._4 },
			{ GLFWKey._5, Key._5 },
			{ GLFWKey._6, Key._6 },
			{ GLFWKey._7, Key._7 },
			{ GLFWKey._8, Key._8 },
			{ GLFWKey._9, Key._9 },

			{ GLFWKey.Space, Key.Space },
			{ GLFWKey.Apostrophe, Key.Quote },
			{ GLFWKey.Comma, Key.Comma },
			{ GLFWKey.Minus, Key.Minus },
			{ GLFWKey.Period, Key.Period },
			{ GLFWKey.Slash, Key.Slash },
			{ GLFWKey.Semicolon, Key.Semicolon },
			{ GLFWKey.Equal, Key.Equals },
			{ GLFWKey.LeftBracket, Key.LBracket },
			{ GLFWKey.Backslash, Key.Backslash },
			{ GLFWKey.RightBracket, Key.RBracket },
			{ GLFWKey.GraveAccent, Key.Grave },
			{ GLFWKey.Escape, Key.Escape },
			{ GLFWKey.Enter, Key.Enter },
			{ GLFWKey.Tab, Key.Tab },
			{ GLFWKey.Backspace, Key.Backspace },
			{ GLFWKey.Insert, Key.Insert },
			{ GLFWKey.Delete, Key.Delete },
			{ GLFWKey.Right, Key.Right },
			{ GLFWKey.Left, Key.Left },
			{ GLFWKey.Down, Key.Down },
			{ GLFWKey.Up, Key.Up },
			{ GLFWKey.PageUp, Key.PageUp },
			{ GLFWKey.PageDown, Key.PageDown },
			{ GLFWKey.Home, Key.Home },
			{ GLFWKey.End, Key.End },
			{ GLFWKey.PrintScreen, Key.PrintScreen },
			{ GLFWKey.Pause, Key.Pause },

			{ GLFWKey.F1, Key.F1 },
			{ GLFWKey.F2, Key.F2 },
			{ GLFWKey.F3, Key.F3 },
			{ GLFWKey.F4, Key.F4 },
			{ GLFWKey.F5, Key.F5 },
			{ GLFWKey.F6, Key.F6 },
			{ GLFWKey.F7, Key.F7 },
			{ GLFWKey.F8, Key.F8 },
			{ GLFWKey.F9, Key.F9 },
			{ GLFWKey.F10, Key.F10 },
			{ GLFWKey.F11, Key.F11 },
			{ GLFWKey.F12, Key.F12 },

			{ GLFWKey.Kp0, Key.Numpad0 },
			{ GLFWKey.Kp1, Key.Numpad1 },
			{ GLFWKey.Kp2, Key.Numpad2 },
			{ GLFWKey.Kp3, Key.Numpad3 },
			{ GLFWKey.Kp4, Key.Numpad4 },
			{ GLFWKey.Kp5, Key.Numpad5 },
			{ GLFWKey.Kp6, Key.Numpad6 },
			{ GLFWKey.Kp7, Key.Numpad7 },
			{ GLFWKey.Kp8, Key.Numpad8 },
			{ GLFWKey.Kp9, Key.Numpad9 },

			{ GLFWKey.KpDecimal, Key.NumpadDecimal },
			{ GLFWKey.KpDivide, Key.NumpadDivide },
			{ GLFWKey.KpMultiply, Key.NumpadMultiply },
			{ GLFWKey.KpSubtract, Key.NumpadMinus },
			{ GLFWKey.KpAdd, Key.NumpadAdd },
			{ GLFWKey.KpEnter, Key.NumpadEnter },

			{ GLFWKey.LeftShift, Key.LShift },
			{ GLFWKey.LeftControl, Key.LCtrl },
			{ GLFWKey.LeftAlt, Key.LAlt },
			{ GLFWKey.RightShift, Key.RShift },
			{ GLFWKey.RightControl, Key.RCtrl },
			{ GLFWKey.RightAlt, Key.RAlt }
		};

		public static readonly IReadOnlyDictionary<Key, GLFWKey> StdToGLFWKey = GLFWToStdKey.ToDictionary(item => item.Value, item => item.Key);

		public static KeyMod GLFWToStdKeyMod(GLFWKeyMod mods) {
			KeyMod km = 0;
			if ((mods & GLFWKeyMod.Alt) != 0) km |= KeyMod.Alt;
			if ((mods & GLFWKeyMod.Shift) != 0) km |= KeyMod.Shift;
			if ((mods & GLFWKeyMod.Control) != 0) km |= KeyMod.Ctrl;
			return km;
		}
		
		public bool HasGlobalKeyState => false;

		public string Name => "GLFW Keyboard";

#pragma warning disable 0067
		public event Action? OnDisconnected;
		
		public event Action<KeyEvent>? OnKey;
		
		public event Action<TextInputEvent>? OnTextInput;

		public event Action<TextEditEvent>? OnTextEdit;
#pragma warning restore 0067

		public void EndTextInput() => throw new NotImplementedException();

		public bool GetKeyState(Key key) => throw new NotImplementedException();

		public void StartTextInput() => throw new NotImplementedException();

	}

	public class GLFWServiceMouse : IMouse {

		public static int GLFWToStdButton(GLFWMouseButton button) => button switch {
			GLFWMouseButton.Left => IMouse.LeftButton,
			GLFWMouseButton.Right => IMouse.RightButton,
			GLFWMouseButton.Middle => IMouse.MiddleButton,
			_ => IMouse.GetAuxMouseButton(button - GLFWMouseButton.Middle)
		};

		public static GLFWMouseButton StdToGLFWButton(int button) => button switch {
			IMouse.LeftButton => GLFWMouseButton.Left,
			IMouse.RightButton => GLFWMouseButton.Right,
			IMouse.MiddleButton => GLFWMouseButton.Middle,
			_ => GLFWMouseButton.Middle + (button - IMouse.GetAuxMouseButton(0))
		};

		public bool HasGlobalMouseState => false;

		public string Name => "GLFW Mouse";

		public Vector2i MousePosition => throw new NotImplementedException();

#pragma warning disable 0067
		public event Action? OnDisconnected;

		public event Action<MouseMoveEvent>? OnMouseMove;

		public event Action<MouseButtonEvent>? OnMouseButton;

		public event Action<MouseWheelEvent>? OnMouseWheel;
#pragma warning restore 0067

		public bool GetMouseButtonState(int button) => throw new NotImplementedException();

	}

	public class GLFWServiceJoystick : IJoystick {

		public GLFWJoystick Joystick { get; init; }

		public bool IsGamepad => Joystick.IsGamepad;

		public bool[] Buttons {
			get {
				ReadOnlySpan<GLFWButtonState> state = Joystick.Buttons;
				bool[] buttons = new bool[state.Length];
				for (int i = 0; i < buttons.Length; i++) buttons[i] = state[i] != GLFWButtonState.Release;
				return buttons;
			}
		}

		public float[] Axes => Joystick.Axes.ToArray();

		public HatState[] Hats {
			get {
				ReadOnlySpan<GLFWHat> state = Joystick.Hats;
				HatState[] hats = new HatState[state.Length];
				for (int i = 0; i < hats.Length; i++) {
					GLFWHat hat = state[i];
					HatState hs = HatState.Centered;
					if ((hat & GLFWHat.Up) != 0) {
						if ((hat & GLFWHat.Left) != 0) hs = HatState.UpLeft;
						else if ((hat & GLFWHat.Right) != 0) hs = HatState.UpRight;
						else hs = HatState.Up;
					} else if ((hat & GLFWHat.Down) != 0) {
						if ((hat & GLFWHat.Left) != 0) hs = HatState.DownLeft;
						else if ((hat & GLFWHat.Right) != 0) hs = HatState.DownRight;
						else hs = HatState.Down;
					} else if ((hat & GLFWHat.Left) != 0) hs = HatState.Left;
					else if ((hat & GLFWHat.Right) != 0) hs = HatState.Right;
					hats[i] = hs;
				}
				return hats;
			}
		}

		public string Name => Joystick.Name!;

		public event Action? OnDisconnected;
		internal void DoOnDisconnect() => OnDisconnected?.Invoke();

	}

	public class GLFWServiceGamepad : GLFWServiceJoystick, IGamepad {

		public bool[] GamepadButtons {
			get {
				GLFWGamepadState? state = Joystick.GamepadState;
				if (!state.HasValue) return Array.Empty<bool>();
				else {
					ReadOnlySpan<GLFWButtonState> glfwbuttons = state.Value.Buttons;
					bool[] buttons = new bool[glfwbuttons.Length];
					for (int i = 0; i < buttons.Length; i++) buttons[i] = glfwbuttons[i] != GLFWButtonState.Release;
					return buttons;
				}
			}
		}

		public float[] GamepadAxes {
			get {
				GLFWGamepadState? state = Joystick.GamepadState;
				if (!state.HasValue) return Array.Empty<float>();
				else return state.Value.Axes.ToArray();
			}
		}

		public int GetGamepadControlIndex(GamepadControl control) => control switch {
			GamepadControl.A => (int)GLFWGamepadButton.A,
			GamepadControl.B => (int)GLFWGamepadButton.B,
			GamepadControl.X => (int)GLFWGamepadButton.X,
			GamepadControl.Y => (int)GLFWGamepadButton.Y,
			GamepadControl.LeftBumper => (int)GLFWGamepadButton.LeftBumper,
			GamepadControl.RightBumper => (int)GLFWGamepadButton.RightBumper,
			GamepadControl.Back => (int)GLFWGamepadButton.Back,
			GamepadControl.Start => (int)GLFWGamepadButton.Start,
			GamepadControl.Guide => (int)GLFWGamepadButton.Guide,
			GamepadControl.LeftThumbStick => (int)GLFWGamepadButton.LeftThumb,
			GamepadControl.RightThumbStick => (int)GLFWGamepadButton.RightThumb,
			GamepadControl.DPadUp => (int)GLFWGamepadButton.DpadUp,
			GamepadControl.DPadDown => (int)GLFWGamepadButton.DpadDown,
			GamepadControl.DPadLeft => (int)GLFWGamepadButton.DpadLeft,
			GamepadControl.DPadRight => (int)GLFWGamepadButton.DpadRight,

			GamepadControl.LeftX => (int)GLFWGamepadAxis.LeftX,
			GamepadControl.LeftY => (int)GLFWGamepadAxis.LeftY,
			GamepadControl.RightX => (int)GLFWGamepadAxis.RightX,
			GamepadControl.RightY => (int)GLFWGamepadAxis.RightY,
			GamepadControl.LeftTrigger => (int)GLFWGamepadAxis.LeftTrigger,
			GamepadControl.RightTrigger => (int)GLFWGamepadAxis.RightTrigger,
			_ => -1
		};

	}

	public class GLFWServiceInputSystem : IInputSystem {

		// Global GLFW events are forwarded to static events in the input system, then passed to invidivual instances
		private static event Action<int, GLFWConnectState>? OnJoystickConnect;

		private static bool callbacksInstalled = false;
		private static void InstallCallbacks() {
			if (!callbacksInstalled) {
				GLFW3.JoystickCallback = (int id, GLFWConnectState state) => OnJoystickConnect?.Invoke(id, state);
				callbacksInstalled = true;
			}
		}

		public GLFWServiceInputSystem() {
			OnJoystickConnect += (int id, GLFWConnectState state) => {
				switch(state) {
					case GLFWConnectState.Connected:
						GLFWJoystick joy = new() { ID = id };
						if (joy.IsGamepad) {
							GLFWServiceGamepad gamepad = new() { Joystick = joy };
							joysticks.Add(gamepad);
							gamepads.Add(gamepad);
							OnConnected?.Invoke(gamepad);
						} else {
							GLFWServiceJoystick joystick = new() { Joystick = joy };
							joysticks.Add(joystick);
							OnConnected?.Invoke(joystick);
						}
						break;
					case GLFWConnectState.Disconnected:
						for(int i = 0; i < gamepads.Count; i++) {
							GLFWServiceGamepad gamepad = gamepads[i];
							if (gamepad.Joystick.ID == id) {
								gamepad.DoOnDisconnect();
								gamepads.RemoveAt(i);
								joysticks.Remove(gamepad);
								return;
							}
						}
						for(int i = 0; i < joysticks.Count; i++) {
							GLFWServiceJoystick joystick = joysticks[i];
							if (joystick.Joystick.ID == id) {
								joystick.DoOnDisconnect();
								joysticks.RemoveAt(i);
								return;
							}
						}
						break;
				}
			};
			InstallCallbacks();

			foreach(GLFWJoystick joy in GLFW3.Joysticks) {
				if (joy.IsGamepad) {
					GLFWServiceGamepad gamepad = new() { Joystick = joy };
					joysticks.Add(gamepad);
					gamepads.Add(gamepad);
				} else joysticks.Add(new GLFWServiceJoystick() { Joystick = joy });
			}
		}

		public IKeyboard Keyboard { get; } = new GLFWServiceKeyboard();

		public IMouse Mouse => new GLFWServiceMouse();

		private readonly List<GLFWServiceJoystick> joysticks = new();
		public IReadOnlyList<IJoystick> Joysticks => joysticks;

		private readonly List<GLFWServiceGamepad> gamepads = new();
		public IReadOnlyList<IGamepad> Gamepads => gamepads;

		public IReadOnlyList<IHapticDevice> HapticDevices { get; } = new List<IHapticDevice>();

		public IReadOnlyList<ILightSystem> LightSystems { get; } = new List<ILightSystem>();

		public event Action<IInputDevice>? OnConnected;

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public void RunEvents() => GLFW3.PollEvents();
	}

}
