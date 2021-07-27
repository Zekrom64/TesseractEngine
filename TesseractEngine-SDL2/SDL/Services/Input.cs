using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Services;

namespace Tesseract.SDL.Services {

	public class SDLServiceKeyboard : IKeyboard {

		public static readonly IReadOnlyDictionary<SDLScancode, Key> SDLToStdKey;

		static SDLServiceKeyboard() {
			Dictionary<SDLScancode, Key> dict = new() {
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
				{ SDLScancode.KpDecimal, Key.NumpadDecimal },
				{ SDLScancode.KpEnter, Key.NumpadEnter }
			};

			for (int i = 0; i < 26; i++) dict[(SDLScancode)(((int)SDLScancode.A) + i)] = (Key)(((int)Key.A) + i);
			for (int i = 0; i < 10; i++) {
				dict[(SDLScancode)(((int)SDLScancode._1) + i)] = (Key)(((int)Key._1) + i);
				dict[(SDLScancode)(((int)SDLScancode.Kp1) + i)] = (Key)(((int)Key.Numpad1) + i);
			}
			for (int i = 0; i < 12; i++) dict[(SDLScancode)(((int)SDLScancode.F1) + i)] = (Key)(((int)Key.F1) + i);
			SDLToStdKey = dict;
		}

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

		public event Action OnDisconnected;
		public event Action<KeyEvent> OnKey;
		internal void DoOnKey(KeyEvent evt) => OnKey?.Invoke(evt);
		public event Action<TextInputEvent> OnTextInput;
		internal void DoOnTextInput(TextInputEvent evt) => OnTextInput?.Invoke(evt);
		public event Action<TextEditEvent> OnTextEdit;
		internal void DoOnTextEdit(TextEditEvent evt) => OnTextEdit?.Invoke(evt);

		public void EndTextInput() => SDL2.StopTextInput();

		public static bool GetCurrentKeyState(Key key) {
			SDLScancode scancode = StdToSDLKey[key];
			return SDL2.GetKeyboardState()[(int)scancode] != SDLButtonState.Released;
		}

		public bool GetKeyState(Key key) => GetCurrentKeyState(key);

		public T GetService<T>(IService<T> service) => default;

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

		public event Action OnDisconnected;
		public event Action<MouseMoveEvent> OnMouseMove;
		internal void DoOnMouseMove(MouseMoveEvent evt) => OnMouseMove?.Invoke(evt);
		public event Action<MouseButtonEvent> OnMouseButton;
		internal void DoOnMouseButton(MouseButtonEvent evt) => OnMouseButton?.Invoke(evt);
		public event Action<MouseWheelEvent> OnMouseWheel;
		internal void DoOnMouseWheel(MouseWheelEvent evt) => OnMouseWheel?.Invoke(evt);

		public static bool GetCurrentMouseButtonState(int button) {
			SDLMouseButtonState mb = SDL2.GetGlobalMouseState(out _, out _);
			return (mb & SDL2.MakeMouseButton(StdToSDLButton(button))) != 0;
		}

		public bool GetMouseButtonState(int button) => GetCurrentMouseButtonState(button);

		public T GetService<T>(IService<T> service) => default;

	}

	public class SDLServiceInputSystem : IInputSystem {

		internal readonly SDLServiceKeyboard keyboard = new();
		public IKeyboard Keyboard => keyboard;

		internal readonly SDLServiceMouse mouse = new();
		public IMouse Mouse => mouse;

		public IReadOnlyList<IJoystick> Joysticks => throw new NotImplementedException();

		public IReadOnlyList<IGamepad> Gamepads => throw new NotImplementedException();

		public IReadOnlyList<IHapticDevice> HapticDevices => throw new NotImplementedException();

		public IReadOnlyList<ILightSystem> LightSystems => throw new NotImplementedException();

		public event Action<IInputDevice> OnConnected;

		private SDLKeymod lastModState = default;

		public SDLServiceInputSystem() {
			lastModState = SDL2.ModState;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public T GetService<T>(IService<T> service) => default;

		public void RunEvents() {
			SDL2.PumpEvents();
			SDLEvent? evt;
			while ((evt = SDL2.PollEvent()).HasValue) PushEvent(evt.Value);
		}

		private static SDLServiceWindow GetWindowFromID(uint id) {
			IntPtr pSDLWindow = SDL2.Functions.SDL_GetWindowFromID(id);
			if (pSDLWindow == IntPtr.Zero) return null;
			IntPtr pWindow = SDL2.Functions.SDL_GetWindowData(pSDLWindow, SDLServiceWindow.WindowDataID);
			if (pWindow == IntPtr.Zero) return null;
			return new ObjectPointer<SDLServiceWindow>(pWindow).Value;
		}

		private void PushEvent(in SDLEvent evt) {
			switch (evt.Type) {
				case SDLEventType.WindowEvent: {
					SDLServiceWindow window = GetWindowFromID(evt.Window.WindowID);
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
				} break;
				case SDLEventType.KeyDown:
				case SDLEventType.KeyUp: {
					lastModState = evt.Key.Keysym.Mod;
					KeyEvent key = new() {
						Key = SDLServiceKeyboard.SDLToStdKey[evt.Key.Keysym.Scancode],
						State = evt.Key.State != SDLButtonState.Released,
						Mod = SDLServiceKeyboard.SDLToStdKeyMod(evt.Key.Keysym.Mod),
						Repeat = evt.Key.Repeat != 0
					};
					GetWindowFromID(evt.Key.WindowID)?.DoOnKey(key);
					keyboard.DoOnKey(key);
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
					SDLServiceWindow window = GetWindowFromID(evt.Button.WindowID);
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
					SDLServiceWindow window = GetWindowFromID(evt.Button.WindowID);
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
					SDLServiceWindow window = GetWindowFromID(evt.Button.WindowID);
					if (window != null) {
						whl.Position = window.LastMousePos;
						window.DoOnMouseWheel(whl);
						whl.Position += window.Position;
					} else SDL2.GetGlobalMouseState(out whl.Position.X, out whl.Position.Y);
					mouse.DoOnMouseWheel(whl);
				} break;
			}
		}

	}

}
