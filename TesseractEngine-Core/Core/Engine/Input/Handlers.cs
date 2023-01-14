using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Engine.Input {

	/// <summary>
	/// An input handler manages input sources for a particular type of input.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class InputHandler<T> where T : struct {

		/// <summary>
		/// The empty handler for a given input type, used when no handler implementation is present.
		/// </summary>
		public static readonly InputHandler<T> Empty = new();

		// The list of sources available for this handler
		internal readonly CopyOnWriteList<IInputSource<T>> Sources = new();

		// Event fired when an input from this handler 'activates' (eg. button press or joystick movement)
		internal event Action<IInputSource<T>>? SourceFired;

		internal InputHandler() { }

		// Fires the SourceFired event
		internal void Fire(IInputSource<T> source) => SourceFired?.Invoke(source);

	}

	/// <summary>
	/// Implementation of the input handler for <tt>bool</tt> sources.
	/// </summary>
	public class BoolInputHandler : InputHandler<bool> {

		private readonly Dictionary<Key, KeyInputSource> keySources = new();
		private readonly Dictionary<int, MouseButtonInputSource> mouseButtonSources = new();
		private readonly MouseWheelInputSource mouseWheelUp, mouseWheelDown, mouseWheelLeft, mouseWheelRight;

		public IInputSource<bool> MouseWheelUp => mouseWheelUp;
		public IInputSource<bool> MouseWheelDown => mouseWheelDown;
		public IInputSource<bool> MouseWheelLeft => mouseWheelLeft;
		public IInputSource<bool> MouseWheelRight => mouseWheelRight;

		public BoolInputHandler(TesseractEngine engine) {
			var window = engine.CreateInfo.Window;

			// Create key sources
			foreach (Key key in Enum.GetValues<Key>()) {
				var source = new KeyInputSource(this, key);
				keySources.Add(key, source);
				Sources.Add(source);
			}
			window.OnKey += (KeyEvent evt) => keySources[evt.Key].CurrentValue = evt.State;

			// Create mouse button sources
			for (int i = 0; i < 3; i++) {
				var source = new MouseButtonInputSource(this, i);
				mouseButtonSources.Add(i, source);
				Sources.Add(source);
			}
			window.OnMouseButton += (MouseButtonEvent evt) => GetMouseButtonImpl(evt.Button).CurrentValue = evt.State;

			// Create mouse wheel input sources
			Sources.Add(mouseWheelUp = new MouseWheelInputSource(this, "Scroll Up", "mouse_wheel_up"));
			Sources.Add(mouseWheelDown = new MouseWheelInputSource(this, "Scroll Down", "mouse_wheel_down"));
			Sources.Add(mouseWheelLeft = new MouseWheelInputSource(this, "Scroll Left", "mouse_wheel_left"));
			Sources.Add(mouseWheelRight = new MouseWheelInputSource(this, "Scroll Right", "mouse_wheel_right"));
			window.OnMouseWheel += (MouseWheelEvent evt) => {
				if (evt.Delta.X > 0) mouseWheelRight.FireClicks(evt.Delta.X);
				else if (evt.Delta.X < 0) mouseWheelLeft.FireClicks(-evt.Delta.X);
				if (evt.Delta.Y > 0) mouseWheelUp.FireClicks(evt.Delta.Y);
				else if (evt.Delta.Y < 0) mouseWheelDown.FireClicks(-evt.Delta.Y);
			};

			var inputSystem = engine.CreateInfo.InputSystem;
			// TODO: Joystick/gamepad input
		}

		public IInputSource<bool> GetKey(Key key) => keySources[key];

		public IInputSource<bool> GetMouseButton(int button) => GetMouseButtonImpl(button);

		private MouseButtonInputSource GetMouseButtonImpl(int button) {
			if (mouseButtonSources.TryGetValue(button, out MouseButtonInputSource? source)) return source;
			source = new MouseButtonInputSource(this, button);
			mouseButtonSources.Add(button, source);
			Sources.Add(source);
			return source;
		}

	}

	public class Vector2InputHandler : InputHandler<Vector2> {

		private readonly MouseMotionInputSource mouse;

		public IInputSource<Vector2> Mouse => mouse;

		public Vector2InputHandler(TesseractEngine engine) {
			var window = engine.CreateInfo.Window;

			// Add the mouse motion source
			mouse = new MouseMotionInputSource(this);

			window.OnMouseMove += (MouseMoveEvent evt) => mouse.CurrentValue = evt.Position;
		}

	}

}
