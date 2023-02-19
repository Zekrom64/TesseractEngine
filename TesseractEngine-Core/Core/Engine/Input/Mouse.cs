using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;

namespace Tesseract.Core.Engine.Input {

	// Input source for mouse buttons
	internal class MouseButtonInputSource : IInputSource<bool> {

		internal const string BaseID = "mouse_button";

		// Dictionary of names for certain buttons
		private static readonly Dictionary<int, string> nameOverrides = new() {
				{ IMouse.LeftButton, "Mouse Left Button" },
				{ IMouse.MiddleButton, "Mouse Middle Button" },
				{ IMouse.RightButton, "Mouse Right Button" }
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

		public MouseButtonInputSource(BoolInputHandler handler, int button) {
			this.handler = handler;
			Name = nameOverrides.GetValueOrDefault(button, $"Mouse Button {button + 1}");
			ID = $"mouse_button:{button}";
		}

	}

	// Input source for the mouse wheel (handled as a button for clicks in every axis)
	internal class MouseWheelInputSource : IInputSource<bool> {

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

		public void FireClicks(int count) {
			for (int i = 0; i < count; i++) {
				CurrentValue = true;
				CurrentValue = false;
			}
		}

		public MouseWheelInputSource(BoolInputHandler handler, string name, string id) {
			this.handler = handler;
			Name = name;
			ID = id;
		}

	}

	// Input source for mouse motion
	internal class MouseMotionInputSource : IInputSource<Vector2> {

		private readonly Vector2InputHandler handler;

		public string Name => "Mouse";

		public string ID => "mouse";

		private Vector2 value;
		public Vector2 CurrentValue {
			get => value;
			internal set {
				if (value != this.value) {
					this.value = value;
					OnChange?.Invoke(value);
					handler.Fire(this);
				}
			}
		}

		public event Action<Vector2>? OnChange;

		public MouseMotionInputSource(Vector2InputHandler handler) {
			this.handler = handler;
		}

	}


}
