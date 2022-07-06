using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Services;

namespace Tesseract.SDL.Services {

	public class SDLServiceKeyboard : IKeyboard {

		public static readonly IReadOnlyDictionary<SDLScancode, Key> SDLToStdKey = new Dictionary<SDLScancode, Key>() {
			{ SDLScancode.Grave, Key.Grave },
			{ SDLScancode.Minus, Key.Minus },
			{ SDLScancode.Equals, Key.Equals },
			{ SDLScancode.LeftBracket, Key.LBracket },
			{ SDLScancode.RightBracket, Key.RBracket },
			{ SDLScancode.Backslash, Key.Backslash },
			{ SDLScancode.Semicolon, Key.Semicolon },
			{ SDLScancode.Apostrophe, Key.Quote },
			{ SDLScancode.Comma, Key.Comma },
			{ SDLScancode.Period, Key.Period },
			{ SDLScancode.Slash, Key.Slash },

			{ SDLScancode.Escape, Key.Escape },
			{ SDLScancode.Backspace, Key.Backspace },
			{ SDLScancode.Tab, Key.Tab },
			{ SDLScancode.Return, Key.Enter },
			{ SDLScancode.Space, Key.Space },
			{ SDLScancode.Left, Key.Left },
			{ SDLScancode.Right, Key.Right },
			{ SDLScancode.Up, Key.Up },
			{ SDLScancode.Down, Key.Down },

			{ SDLScancode.LShift, Key.LShift },
			{ SDLScancode.RShift, Key.RShift },
			{ SDLScancode.LCtrl, Key.LCtrl },
			{ SDLScancode.RCtrl, Key.RCtrl },
			{ SDLScancode.LAlt, Key.LAlt },
			{ SDLScancode.RAlt, Key.RAlt },

			{ SDLScancode.Insert, Key.Insert },
			{ SDLScancode.Delete, Key.Delete },
			{ SDLScancode.PrintScreen, Key.PrintScreen },
			{ SDLScancode.Pause, Key.Pause },
			{ SDLScancode.Home, Key.Home },
			{ SDLScancode.End, Key.End },
			{ SDLScancode.PageUp, Key.PageUp },
			{ SDLScancode.PageDown, Key.PageDown },

			{ SDLScancode.KpDivide, Key.NumpadDivide },
			{ SDLScancode.KpMultiply, Key.NumpadMultiply },
			{ SDLScancode.KpMinus, Key.NumpadMinus },
			{ SDLScancode.KpPlus, Key.NumpadAdd },
			// For some reason SDL has "KpDecimal" and "KpPeriod", but KpPeriod seems to be the actually used one
			//{ SDLScancode.KpDecimal, Key.NumpadDecimal },
			{ SDLScancode.KpPeriod, Key.NumpadDecimal },
			{ SDLScancode.KpEnter, Key.NumpadEnter },

			{ SDLScancode.A, Key.A },
			{ SDLScancode.B, Key.B },
			{ SDLScancode.C, Key.C },
			{ SDLScancode.D, Key.D },
			{ SDLScancode.E, Key.E },
			{ SDLScancode.F, Key.F },
			{ SDLScancode.G, Key.G },
			{ SDLScancode.H, Key.H },
			{ SDLScancode.I, Key.I },
			{ SDLScancode.J, Key.J },
			{ SDLScancode.K, Key.K },
			{ SDLScancode.L, Key.L },
			{ SDLScancode.M, Key.M },
			{ SDLScancode.N, Key.N },
			{ SDLScancode.O, Key.O },
			{ SDLScancode.P, Key.P },
			{ SDLScancode.Q, Key.Q },
			{ SDLScancode.R, Key.R },
			{ SDLScancode.S, Key.S },
			{ SDLScancode.T, Key.T },
			{ SDLScancode.U, Key.U },
			{ SDLScancode.V, Key.V },
			{ SDLScancode.W, Key.W },
			{ SDLScancode.X, Key.X },
			{ SDLScancode.Y, Key.Y },
			{ SDLScancode.Z, Key.Z },

			{ SDLScancode._0, Key._0 },
			{ SDLScancode._1, Key._1 },
			{ SDLScancode._2, Key._2 },
			{ SDLScancode._3, Key._3 },
			{ SDLScancode._4, Key._4 },
			{ SDLScancode._5, Key._5 },
			{ SDLScancode._6, Key._6 },
			{ SDLScancode._7, Key._7 },
			{ SDLScancode._8, Key._8 },
			{ SDLScancode._9, Key._9 },

			{ SDLScancode.Kp0, Key.Numpad0 },
			{ SDLScancode.Kp1, Key.Numpad1 },
			{ SDLScancode.Kp2, Key.Numpad2 },
			{ SDLScancode.Kp3, Key.Numpad3 },
			{ SDLScancode.Kp4, Key.Numpad4 },
			{ SDLScancode.Kp5, Key.Numpad5 },
			{ SDLScancode.Kp6, Key.Numpad6 },
			{ SDLScancode.Kp7, Key.Numpad7 },
			{ SDLScancode.Kp8, Key.Numpad8 },
			{ SDLScancode.Kp9, Key.Numpad9 },

			{ SDLScancode.F1, Key.F1 },
			{ SDLScancode.F2, Key.F2 },
			{ SDLScancode.F3, Key.F3 },
			{ SDLScancode.F4, Key.F4 },
			{ SDLScancode.F5, Key.F5 },
			{ SDLScancode.F6, Key.F6 },
			{ SDLScancode.F7, Key.F7 },
			{ SDLScancode.F8, Key.F8 },
			{ SDLScancode.F9, Key.F9 },
			{ SDLScancode.F10, Key.F10 },
			{ SDLScancode.F11, Key.F11 },
			{ SDLScancode.F12, Key.F12 },
		};

		public static readonly IReadOnlyDictionary<Key, SDLScancode> StdToSDLKey = SDLToStdKey.ToDictionary(item => item.Value, item => item.Key);

		public static KeyMod SDLToStdKeyMod(SDLKeymod mod) {
			KeyMod mod2 = 0;
			if ((mod & SDLKeymod.LCtrl) != 0) mod2 |= KeyMod.LCtrl;
			if ((mod & SDLKeymod.RCtrl) != 0) mod2 |= KeyMod.RCtrl;
			if ((mod & SDLKeymod.LAlt) != 0) mod2 |= KeyMod.LAlt;
			if ((mod & SDLKeymod.RAlt) != 0) mod2 |= KeyMod.RAlt;
			if ((mod & SDLKeymod.LShift) != 0) mod2 |= KeyMod.LShift;
			if ((mod & SDLKeymod.RShift) != 0) mod2 |= KeyMod.RShift;
			return mod2;
		}

		public static SDLKeymod StdToSDLKeyMod(KeyMod mod) {
			SDLKeymod mod2 = 0;
			if ((mod & KeyMod.LCtrl) != 0) mod2 |= SDLKeymod.LCtrl;
			if ((mod & KeyMod.RCtrl) != 0) mod2 |= SDLKeymod.RCtrl;
			if ((mod & KeyMod.LAlt) != 0) mod2 |= SDLKeymod.LAlt;
			if ((mod & KeyMod.RAlt) != 0) mod2 |= SDLKeymod.RAlt;
			if ((mod & KeyMod.LShift) != 0) mod2 |= SDLKeymod.LShift;
			if ((mod & KeyMod.RShift) != 0) mod2 |= SDLKeymod.RShift;
			return mod2;
		}

		public bool HasGlobalKeyState => true;

		public string Name => "SDL Keyboard";

		public event Action OnDisconnected { add { } remove { } }

		public event Action<KeyEvent>? OnKey;
		internal void DoOnKey(KeyEvent evt) => OnKey?.Invoke(evt);
		public event Action<TextInputEvent>? OnTextInput;
		internal void DoOnTextInput(TextInputEvent evt) => OnTextInput?.Invoke(evt);
		public event Action<TextEditEvent>? OnTextEdit;
		internal void DoOnTextEdit(TextEditEvent evt) => OnTextEdit?.Invoke(evt);

		public void EndTextInput() => SDL2.StopTextInput();

		public static bool GetCurrentKeyState(Key key) {
			SDLScancode scancode = StdToSDLKey[key];
			return SDL2.GetKeyboardState()[(int)scancode] != SDLButtonState.Released;
		}

		public bool GetKeyState(Key key) => GetCurrentKeyState(key);

		public T? GetService<T>(IService<T> service) => default;

		public void StartTextInput() => SDL2.StartTextInput();

	}

	public class SDLServiceMouse : IMouse {

		public static int SDLToStdButton(int button) => button switch {
			SDL2.ButtonLeft => IMouse.LeftButton,
			SDL2.ButtonMiddle => IMouse.MiddleButton,
			SDL2.ButtonRight => IMouse.RightButton,
			_ => IMouse.GetAuxMouseButton(button - SDL2.ButtonX1)
		};

		public static int StdToSDLButton(int button) => button switch {
			IMouse.LeftButton => SDL2.ButtonLeft,
			IMouse.MiddleButton => SDL2.ButtonMiddle,
			IMouse.RightButton => SDL2.ButtonRight,
			_ => SDL2.ButtonX1 + (button - IMouse.GetAuxMouseButton(0))
		};

		public bool HasGlobalMouseState => true;

		public string Name => "SDL Mouse";

		public Vector2i MousePosition {
			get {
				Vector2i pos = new();
				SDL2.GetGlobalMouseState(out pos.X, out pos.Y);
				return pos;
			}
		}

		public event Action OnDisconnected { add { } remove { } }

		public event Action<MouseMoveEvent>? OnMouseMove;
		internal void DoOnMouseMove(MouseMoveEvent evt) => OnMouseMove?.Invoke(evt);
		public event Action<MouseButtonEvent>? OnMouseButton;
		internal void DoOnMouseButton(MouseButtonEvent evt) => OnMouseButton?.Invoke(evt);
		public event Action<MouseWheelEvent>? OnMouseWheel;
		internal void DoOnMouseWheel(MouseWheelEvent evt) => OnMouseWheel?.Invoke(evt);

		public static bool GetCurrentMouseButtonState(int button) {
			SDLMouseButtonState mb = SDL2.GetGlobalMouseState(out _, out _);
			return (mb & SDL2.MakeMouseButton(StdToSDLButton(button))) != 0;
		}

		public bool GetMouseButtonState(int button) => GetCurrentMouseButtonState(button);

		public T? GetService<T>(IService<T> service) => default;

	}

	public class SDLServiceJoystick : IJoystick {

		private class SDLServiceJoystickLightSystem : ILightSystem {

			public SDLServiceJoystick Joystick { get; init; } = null!;

			public string Name => Joystick.Name;

			public event Action? OnDisconnected;
			internal void DoOnDisconnected() => OnDisconnected?.Invoke();

			public T? GetService<T>(IService<T> service) => default;

			public bool IsLightPatternSupported(ILightPattern pattern) => pattern is SingleLightPattern;

			public void SetLightPattern(ILightPattern pattern) {
				if (pattern is SingleLightPattern light) {
					Vector4 color = light.Color.Normalized;
					Joystick.Joystick.SetLED((byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
				}
			}

		}

		public readonly SDLJoystick Joystick;
		private readonly SDLServiceJoystickLightSystem? lightSystem;

		public event Action? OnDisconnected;
		internal void DoOnDisconnected() {
			OnDisconnected?.Invoke();
			if (lightSystem != null) lightSystem.DoOnDisconnected();
		}

		public SDLServiceJoystick(SDLJoystick joystick) {
			Joystick = joystick;
			if (joystick.HasLED) lightSystem = new SDLServiceJoystickLightSystem() { Joystick = this };
			else lightSystem = null;
		}

		public bool IsGamepad { get; protected set; } = false;

		public bool[] Buttons {
			get {
				bool[] buttons = new bool[Joystick.NumButtons];
				for (int i = 0; i < buttons.Length; i++) buttons[i] = Joystick.GetButton(i) != SDLButtonState.Released;
				return buttons;
			}
		}

		public float[] Axes {
			get {
				float[] axes = new float[Joystick.NumAxes];
				for (int i = 0; i < axes.Length; i++) {
					float unorm = (Joystick.GetAxis(i) - SDL2.JoystickAxisMin) / (float)(SDL2.JoystickAxisMax - SDL2.JoystickAxisMin);
					axes[i] = (unorm * 2.0f) - 1.0f;
				}
				return axes;
			}
		}

		public virtual T? GetService<T>(IService<T> service) {
			if (service == InputServices.LightSystem) return (T?)(object?)lightSystem;
			// TODO: Advanced haptic services
			return default;
		}

		public static HatState SDLToStdHat(SDLHat hat) => hat switch {
			SDLHat.Centered => HatState.Centered,
			SDLHat.Up => HatState.Up,
			SDLHat.Down => HatState.Down,
			SDLHat.Right => HatState.Right,
			SDLHat.Left => HatState.Left,
			SDLHat.RightUp => HatState.UpRight,
			SDLHat.RightDown => HatState.DownRight,
			SDLHat.LeftUp => HatState.UpLeft,
			SDLHat.LeftDown => HatState.DownLeft,
			_ => default
		};

		public HatState[] Hats {
			get {
				HatState[] hats = new HatState[Joystick.NumHats];
				for (int i = 0; i < hats.Length; i++) hats[i] = SDLToStdHat(Joystick.GetHat(i));
				return hats;
			}
		}

		public string Name => Joystick.Name;

	}

	public class SDLServiceGamepad : SDLServiceJoystick, IGamepad {

		private class SDLServiceGamepadLightSystem : ILightSystem {

			public SDLServiceGamepad Gamepad { get; init; } = null!;

			public string Name => Gamepad.Name;

			public event Action? OnDisconnected;
			internal void DoOnDisconnected() => OnDisconnected?.Invoke();

			public T? GetService<T>(IService<T> service) => default;

			public bool IsLightPatternSupported(ILightPattern pattern) => pattern is SingleLightPattern;

			public void SetLightPattern(ILightPattern pattern) {
				if (pattern is SingleLightPattern light) {
					Vector4 color = light.Color.Normalized;
					Gamepad.Joystick.SetLED((byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255));
				}
			}

		}

		public readonly SDLGameController GameController;
		private readonly SDLServiceGamepadLightSystem? lightSystem;

		public SDLServiceGamepad(SDLGameController gameController) : base(gameController.Joystick) {
			GameController = gameController;
			if (GameController.HasLED) lightSystem = new SDLServiceGamepadLightSystem() { Gamepad = this };
			else lightSystem = null;
		}

		// Apparently the compiler has a hissy fit trying to override a virtual with a nullable return
#nullable disable
		public override T GetService<T>(IService<T> service) {
			if (service == InputServices.LightSystem) return (T)(object)lightSystem;
			// TODO: Advanced haptic services
			return base.GetService(service);
		}
#nullable restore

		public const int NumButtons = 21;

		public bool[] GamepadButtons {
			get {
				bool[] buttons = new bool[NumButtons];
				for (int i = 0; i < NumButtons; i++) {
					SDLGameControllerButton button = (SDLGameControllerButton)i;
					buttons[i] = GameController.HasButton(button) && GameController.GetButton(button);
				}
				return buttons;
			}
		}

		public const int NumAxes = 6;

		public float[] GamepadAxes {
			get {
				float[] axes = new float[NumAxes];
				for (int i = 0; i < NumAxes; i++) {
					SDLGameControllerAxis axis = (SDLGameControllerAxis)i;
					axes[i] = GameController.HasAxis(axis) ? GameController.GetAxis(axis) : 0.0f;
				}
				return axes;
			}
		}

		public int GetGamepadControlIndex(GamepadControl control) {
			SDLGameControllerButton button = control switch {
				GamepadControl.A => SDLGameControllerButton.A,
				GamepadControl.B => SDLGameControllerButton.B,
				GamepadControl.X => SDLGameControllerButton.X,
				GamepadControl.Y => SDLGameControllerButton.Y,
				GamepadControl.LeftBumper => SDLGameControllerButton.LeftShoulder,
				GamepadControl.RightBumper => SDLGameControllerButton.RightShoulder,
				GamepadControl.Back => SDLGameControllerButton.Back,
				GamepadControl.Start => SDLGameControllerButton.Start,
				GamepadControl.Guide => SDLGameControllerButton.Guide,
				GamepadControl.LeftThumbStick => SDLGameControllerButton.LeftStick,
				GamepadControl.RightThumbStick => SDLGameControllerButton.RightStick,
				GamepadControl.DPadUp => SDLGameControllerButton.DPadUp,
				GamepadControl.DPadDown => SDLGameControllerButton.DPadDown,
				GamepadControl.DPadLeft => SDLGameControllerButton.DPadLeft,
				GamepadControl.DPadRight => SDLGameControllerButton.DPadRight,
				GamepadControl.Miscellaneous => SDLGameControllerButton.Misc1,
				GamepadControl.Paddle1 => SDLGameControllerButton.Paddle1,
				GamepadControl.Paddle2 => SDLGameControllerButton.Paddle2,
				GamepadControl.Paddle3 => SDLGameControllerButton.Paddle3,
				GamepadControl.Paddle4 => SDLGameControllerButton.Paddle4,
				GamepadControl.Touchpad => SDLGameControllerButton.Touchpad,
				_ => SDLGameControllerButton.Invalid
			};
			if (button != SDLGameControllerButton.Invalid) return GameController.HasButton(button) ? (int)button : -1;
			SDLGameControllerAxis axis = control switch {
				GamepadControl.LeftX => SDLGameControllerAxis.LeftX,
				GamepadControl.LeftY => SDLGameControllerAxis.LeftY,
				GamepadControl.RightX => SDLGameControllerAxis.RightX,
				GamepadControl.RightY => SDLGameControllerAxis.RightY,
				GamepadControl.LeftTrigger => SDLGameControllerAxis.TriggerLeft,
				GamepadControl.RightTrigger => SDLGameControllerAxis.TriggerRight,
				_ => SDLGameControllerAxis.Invalid
			};
			if (axis != SDLGameControllerAxis.Invalid) return GameController.HasAxis(axis) ? (int)axis : -1;
			return -1;
		}

	}

	public class SDLServiceInputSystem : IInputSystem {

		internal readonly SDLServiceKeyboard keyboard = new();
		public IKeyboard Keyboard => keyboard;

		internal readonly SDLServiceMouse mouse = new();
		public IMouse Mouse => mouse;

		internal readonly List<SDLServiceJoystick> joysticks = new();
		public IReadOnlyList<IJoystick> Joysticks => joysticks;

		internal readonly List<SDLServiceGamepad> gamepads = new();
		public IReadOnlyList<IGamepad> Gamepads => gamepads;

		public IReadOnlyList<IHapticDevice> HapticDevices => throw new NotImplementedException();

		public IReadOnlyList<ILightSystem> LightSystems => throw new NotImplementedException();

		public event Action<IInputDevice>? OnConnected;

		private SDLKeymod lastModState = default;

		public SDLServiceInputSystem() {
			lastModState = SDL2.ModState;
			foreach(SDLJoystickDevice joydev in SDL2.Joysticks) {
				if (joydev.IsGameController) {
					SDLServiceGamepad gamepad = new(joydev.GameController.Open());
					joysticks.Add(gamepad);
					gamepads.Add(gamepad);
				} else joysticks.Add(new SDLServiceJoystick(joydev.Open()));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			gamepads.ForEach(gamepad => {
				gamepad.GameController.Dispose();
				joysticks.Remove(gamepad);
			});
			gamepads.Clear();
			foreach (SDLServiceJoystick joystick in joysticks) joystick.Joystick.Dispose();
			joysticks.Clear();
		}

		public T? GetService<T>(IService<T> service) => default;

		public void RunEvents() {
			SDL2.PumpEvents();
			SDLEvent? evt;
			while ((evt = SDL2.PollEvent()).HasValue) PushEvent(evt.Value);
		}

		private static SDLServiceWindow? GetWindowFromID(uint id) {
			IntPtr pSDLWindow = SDL2.Functions.SDL_GetWindowFromID(id);
			if (pSDLWindow == IntPtr.Zero) return null;
			IntPtr pWindow = SDL2.Functions.SDL_GetWindowData(pSDLWindow, SDLServiceWindow.WindowDataID);
			if (pWindow == IntPtr.Zero) return null;
			return new ObjectPointer<SDLServiceWindow>(pWindow).Value;
		}

		private void PushEvent(in SDLEvent evt) {
			switch (evt.Type) {
				case SDLEventType.WindowEvent: {
					SDLServiceWindow? window = GetWindowFromID(evt.Window.WindowID);
						if (window != null) {
							switch (evt.Window.Event) {
								case SDLWindowEventID.Resized: window.DoOnResize(new Vector2i(evt.Window.Data1, evt.Window.Data2)); break;
								case SDLWindowEventID.Moved: window.DoOnMove(new Vector2i(evt.Window.Data1, evt.Window.Data2)); break;
								case SDLWindowEventID.Minimized: window.DoOnMinimized(); break;
								case SDLWindowEventID.Maximized: window.DoOnMaximized(); break;
								case SDLWindowEventID.Restored: window.DoOnRestored(); break;
								case SDLWindowEventID.FocusGained: window.DoOnFocused(); break;
								case SDLWindowEventID.FocusLost: window.DoOnUnfocused(); break;
								case SDLWindowEventID.Close: window.DoOnClosing(); break;
								default: break;
							}
						}
				} break;
				case SDLEventType.KeyDown:
				case SDLEventType.KeyUp: {
					if (SDLServiceKeyboard.SDLToStdKey.TryGetValue(evt.Key.Keysym.Scancode, out Key k)) {
						lastModState = evt.Key.Keysym.Mod;
						KeyEvent key = new() {
							Key = k,
							State = evt.Key.State != SDLButtonState.Released,
							Mod = SDLServiceKeyboard.SDLToStdKeyMod(evt.Key.Keysym.Mod),
							Repeat = evt.Key.Repeat != 0
						};
						GetWindowFromID(evt.Key.WindowID)?.DoOnKey(key);
						keyboard.DoOnKey(key);
					}
				} break;
				case SDLEventType.TextInput: {
					TextInputEvent txt = new() {
						Text = evt.Text.Text
					};
					GetWindowFromID(evt.Text.WindowID)?.DoOnTextInput(txt);
					keyboard.DoOnTextInput(txt);
				} break;
				case SDLEventType.TextEditing: {
					TextEditEvent txt = new() {
						Start = evt.Edit.Start,
						Length = evt.Edit.Length,
						Text = evt.Edit.Text
					};
					GetWindowFromID(evt.Edit.WindowID)?.DoOnTextEdit(txt);
					keyboard.DoOnTextEdit(txt);
				} break;
				case SDLEventType.MouseButtonDown:
				case SDLEventType.MouseButtonUp: {
					MouseButtonEvent btn = new() {
						Button = SDLServiceMouse.SDLToStdButton(evt.Button.Button),
						Position = new Vector2i(evt.Button.X, evt.Button.Y),
						State = evt.Button.State != SDLButtonState.Released,
						// Keymod state not provided with mouse button, infer from last known state
						Mod = SDLServiceKeyboard.SDLToStdKeyMod(lastModState)
					};
					SDLServiceWindow? window = GetWindowFromID(evt.Button.WindowID);
					Vector2i pos = window != null ? window.Position : new();
					window?.DoOnMouseButton(btn);
					// Correct position to global and then fire event
					btn.Position += pos;
					mouse.DoOnMouseButton(btn);
				} break;
				case SDLEventType.MouseMotion: {
					MouseMoveEvent mv = new() {
						Position = new Vector2i(evt.Motion.X, evt.Motion.Y),
						Delta = new Vector2i(evt.Motion.XRel, evt.Motion.YRel)
					};
					SDLServiceWindow? window = GetWindowFromID(evt.Button.WindowID);
					Vector2i pos = window != null ? window.Position : new();
					window?.DoOnMouseMove(mv);
					// Correct position to global and then fire event
					mv.Position += pos;
					mouse.DoOnMouseMove(mv);
				} break;
				case SDLEventType.MouseWheel: {
					MouseWheelEvent whl = new() {
						Delta = new Vector2i(evt.Wheel.X, evt.Wheel.Y)
					};
					SDLServiceWindow? window = GetWindowFromID(evt.Button.WindowID);
					if (window != null) {
						whl.Position = window.LastMousePos;
						window.DoOnMouseWheel(whl);
						whl.Position += window.Position;
					} else SDL2.GetGlobalMouseState(out whl.Position.X, out whl.Position.Y);
					mouse.DoOnMouseWheel(whl);
				} break;
				case SDLEventType.JoyDeviceAdded: {
					int deviceIndex = evt.JDevice.Which;
					SDLJoystickDevice joydev = new() { DeviceIndex = deviceIndex };
					if (joydev.IsGameController) {
						SDLServiceGamepad gamepad = new(joydev.GameController.Open());
						joysticks.Add(gamepad);
						gamepads.Add(gamepad);
						OnConnected?.Invoke(gamepad);
					} else {
						SDLServiceJoystick joystick = new(joydev.Open());
						joysticks.Add(joystick);
						OnConnected?.Invoke(joystick);
					}
				} break;
				case SDLEventType.JoyDeviceRemoved: {
					int joystickID = evt.JDevice.Which;
					SDLJoystick? sdljoy = SDLJoystick.FromInstanceID(joystickID);
					if (sdljoy != null) {
						IntPtr pJoystick = sdljoy.Joystick.Ptr;
						for (int i = 0; i < joysticks.Count; i++) {
							SDLServiceJoystick svjoy = joysticks[i];
							SDLJoystick joy = svjoy.Joystick;
							if (joy.Joystick.Ptr == pJoystick) {
								svjoy.DoOnDisconnected();
								joysticks.RemoveAt(i);
								break;
							}
						}
					}
				} break;
			}
		}

	}

}
