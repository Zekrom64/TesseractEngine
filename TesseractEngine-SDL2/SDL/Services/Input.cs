using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;
using Tesseract.Core.Math;
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

		public bool HasGlobalKeyState => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public event Action OnDisconnected;
		public event Action<KeyEvent> OnKey;
		internal void DoOnKey(KeyEvent evt) => OnKey?.Invoke(evt);
		public event Action<TextInputEvent> OnTextInput;
		internal void DoOnTextInput(TextInputEvent evt) => OnTextInput?.Invoke(evt);
		public event Action<TextEditEvent> OnTextEdit;
		internal void DoOnTextEdit(TextEditEvent evt) => OnTextEdit?.Invoke(evt);

		public void EndTextInput() => SDL2.StopTextInput();

		public bool GetKeyState(Key key) {
			SDLScancode scancode = StdToSDLKey[key];
			return SDL2.GetKeyboardState()[(int)scancode] != SDLButtonState.Released;
		}

		public T GetService<T>(IService<T> service) => default;

		public void StartTextInput() => SDL2.StartTextInput();

	}

	public class SDLServiceMouse : IMouse {
		public bool HasGlobalPositioning => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public Vector2i MousePosition => throw new NotImplementedException();

		public event Action OnDisconnected;
		public event Action<MouseMoveEvent> OnMouseMove;
		public event Action<MouseButtonEvent> OnMouseButton;
		public event Action<MouseWheelEvent> OnMouseWheel;

		public bool GetMouseButtonState(int button) => throw new NotImplementedException();
		public T GetService<T>(IService<T> service) => throw new NotImplementedException();
	}

}
