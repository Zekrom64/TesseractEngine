using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;

namespace Tesseract.Core.Engine.Input {

	// Input source for keyboard keys
	internal class KeyInputSource : IInputSource<bool> {

		// Dictionary of explicit key names
		private static readonly Dictionary<Key, string> nameOverrides = new() {
			{ Key._0, "0" },
			{ Key._1, "1" },
			{ Key._2, "2" },
			{ Key._3, "3" },
			{ Key._4, "4" },
			{ Key._5, "5" },
			{ Key._6, "6" },
			{ Key._7, "7" },
			{ Key._8, "8" },
			{ Key._9, "9" },
			{ Key.Grave, "`" },
			{ Key.Minus, "-" },
			{ Key.Equals, "=" },
			{ Key.LBracket, "[" },
			{ Key.RBracket, "]" },
			{ Key.Backslash, "\\" },
			{ Key.Semicolon, ";" },
			{ Key.Quote, "\'" },
			{ Key.Comma, "," },
			{ Key.Period, "." },
			{ Key.Slash, "/" },
			{ Key.LShift, "Left Shift" },
			{ Key.RShift, "Right Shift" },
			{ Key.LCtrl, "Left Control" },
			{ Key.RCtrl, "Right Control" },
			{ Key.LAlt, "Left Alt" },
			{ Key.RAlt, "Right Alt" },
			{ Key.PrintScreen, "Print Screen" },
			{ Key.PageUp, "Page Up" },
			{ Key.PageDown, "Page Down" },
			{ Key.Numpad0, "Numpad 0" },
			{ Key.Numpad1, "Numpad 1" },
			{ Key.Numpad2, "Numpad 2" },
			{ Key.Numpad3, "Numpad 3" },
			{ Key.Numpad4, "Numpad 4" },
			{ Key.Numpad5, "Numpad 5" },
			{ Key.Numpad6, "Numpad 6" },
			{ Key.Numpad7, "Numpad 7" },
			{ Key.Numpad8, "Numpad 8" },
			{ Key.Numpad9, "Numpad 9" },
			{ Key.NumpadDivide, "Numpad /" },
			{ Key.NumpadMultiply, "Numpad *" },
			{ Key.NumpadMinus, "Numpad -" },
			{ Key.NumpadAdd, "Numpad +" },
			{ Key.NumpadDecimal, "Numpad ." },
			{ Key.NumpadEnter, "Numpad Enter" }
		};

		private readonly BoolInputHandler handler;

		public string Name { get; }

		public string ID { get; }

		private bool value;
		public bool CurrentValue {
			get => value;
			internal set {
				if (value ^ this.value) {
					OnChange?.Invoke(value);
					this.value = value;
					if (value) handler.Fire(this);
				}
			}
		}

		public event Action<bool>? OnChange;

		public KeyInputSource(BoolInputHandler handler, Key key) {
			this.handler = handler;
			Name = nameOverrides.GetValueOrDefault(key, key.ToString());
			ID = $"key:{key}";
		}

	}

}
