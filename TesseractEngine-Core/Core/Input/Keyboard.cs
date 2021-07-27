using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Input {

	/// <summary>
	/// Enumeration of recognized keys. Note that these only correspond to keys on a keyboard, and the
	/// actual characters produced should be determined by the text input system.
	/// </summary>
	public enum Key {
		A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
		_1, _2, _3, _4, _5, _6, _7, _8, _9, _0,

		Grave,
		Minus,
		Equals,
		LBracket,
		RBracket,
		Backslash,
		Semicolon,
		Quote,
		Comma,
		Period,
		Slash,

		Escape,
		Backspace,
		Tab,
		Enter,
		Space,
		Left,
		Up,
		Right,
		Down,

		LShift,
		RShift,
		LCtrl,
		RCtrl,
		LAlt,
		RAlt,

		Insert,
		Delete,
		PrintScreen,
		Pause,
		Home,
		End,
		PageUp,
		PageDown,

		F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

		Numpad0,
		Numpad1,
		Numpad2,
		Numpad3,
		Numpad4,
		Numpad5,
		Numpad6,
		Numpad7,
		Numpad8,
		Numpad9,

		NumpadDivide,
		NumpadMultiply,
		NumpadMinus,
		NumpadAdd,
		NumpadDecimal,
		NumpadEnter
	}

	/// <summary>
	/// Bitmask enumeration of modifier keys. A combination of modifier keys can be made by bitwise ORing multiple keys.
	/// </summary>
	public enum KeyMod {
		LCtrl = 0x0001,
		RCtrl = 0x0002,
		Ctrl = LCtrl | RCtrl,
		LShift = 0x0004,
		RShift = 0x0008,
		Shift = LShift | RShift,
		LAlt = 0x0010,
		RAlt = 0x0020,
		Alt = LAlt | RAlt
	}

	/// <summary>
	/// A key event records the information for a key input.
	/// </summary>
	public struct KeyEvent {

		/// <summary>
		/// The key that is being pressed or released.
		/// </summary>
		public Key Key;

		/// <summary>
		/// The state of modifier keys when the event ocurred.
		/// </summary>
		public KeyMod Mod;

		/// <summary>
		/// The new state of the key.
		/// </summary>
		public bool State;

		/// <summary>
		/// If the key event is fired according to the system's key repetition settings.
		/// </summary>
		public bool Repeat;

	}

	/// <summary>
	/// A key input manages keyboard input events and state.
	/// </summary>
	public interface IKeyInput {

		/// <summary>
		/// Event fired when a key is pressed or released.
		/// </summary>
		public event Action<KeyEvent> OnKey;

		/// <summary>
		/// Gets the state of an individual key. If the key does not
		/// exist its state is false (released).
		/// </summary>
		/// <param name="key">Key to get state for</param>
		/// <returns>State of the key (pressed or released)</returns>
		public bool GetKeyState(Key key);

	}

	/// <summary>
	/// A keyboard provides input through a number of keys. Input directly from the keyboard is only
	/// supported if <see cref="HasGlobalKeyState"/> is set.
	/// </summary>
	public interface IKeyboard : IInputDevice, IKeyInput, ITextInput {

		/// <summary>
		/// If access to a global keyboard state is supported.
		/// </summary>
		public bool HasGlobalKeyState { get; }

	}

}
