using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;

namespace Tesseract.Core.Engine.Input {

	// Holds semantic information for controller input sources
	internal record class ControllerSourceSemantics<T> where T : struct, IEquatable<T> {

		public string? Name { get; init; } = null;

		public string TypeName { get; init; } = "";

		public required string IDName { get; init; }

		public Action<ControllerSource<T>, T> OnModify { get; init; } = (src, val) => src.Handler.Fire(src);

	}

	// Input source for controller components
	internal class ControllerSource<T> : IInputSource<T> where T : struct, IEquatable<T> {

		internal readonly InputHandler<T> Handler;

		private readonly ControllerSourceSemantics<T> semantics;

		public string Name { get; }

		public string ID { get; }

		private T value;
		public T CurrentValue {
			get => value;
			internal set {
				if (!value.Equals(this.value)) {
					this.value = value;
					OnChange?.Invoke(value);
					semantics.OnModify(this, value);
				}
			}
		}

		public event Action<T>? OnChange;

		public ControllerSource(ControllerHandler handler, ControllerSourceSemantics<T> semantics, int index, GamepadControl? control) {
			Handler = handler.Manager.GetHandler<T>();
			this.semantics = semantics;
			int player = handler.PlayerIndex;

			string name = $"Controller {player + 1}, ";
			if (control != null) {
				if (ControllerHandler.NameOverrides.TryGetValue(control.Value, out string? ctrlName)) name += ctrlName;
				else name += control.ToString();
				ID = $"{semantics.IDName}:{player}:{control}";
			} else {
				name += semantics.Name ?? $"{semantics.TypeName} {index + 1}";
				ID = $"{semantics.IDName}:{player}:${index}";
			}
			Name = name;
		}

	}

	public class ControllerHandler {

		// Lists of controls of certain types
		private static readonly GamepadControl[] buttonControls = new GamepadControl[] {
			GamepadControl.A, GamepadControl.B, GamepadControl.X, GamepadControl.Y,
			GamepadControl.LeftBumper, GamepadControl.RightBumper,
			GamepadControl.Back, GamepadControl.Start, GamepadControl.Guide,
			GamepadControl.LeftThumbStick, GamepadControl.RightThumbStick,
			GamepadControl.DPadUp, GamepadControl.DPadDown, GamepadControl.DPadLeft, GamepadControl.DPadRight,
			GamepadControl.Miscellaneous,
			GamepadControl.Paddle1, GamepadControl.Paddle2, GamepadControl.Paddle3, GamepadControl.Paddle4,
			GamepadControl.Touchpad
		};

		private static readonly GamepadControl[] axisControls = new GamepadControl[] {
			GamepadControl.LeftX, GamepadControl.LeftY,
			GamepadControl.RightX, GamepadControl.RightY,
			GamepadControl.LeftTrigger, GamepadControl.RightTrigger
		};

		// Special names for certain controls
		internal static readonly Dictionary<GamepadControl, string> NameOverrides = new() {
			{ GamepadControl.LeftBumper, "Left Bumper" },
			{ GamepadControl.RightBumper, "Right Bumper" },
			{ GamepadControl.LeftThumbStick, "Left Thumbstick" },
			{ GamepadControl.RightThumbStick, "Right Thumbstick" },
			{ GamepadControl.DPadUp, "D-Pad Up" },
			{ GamepadControl.DPadDown, "D-Pad Down" },
			{ GamepadControl.DPadLeft, "D-Pad Left" },
			{ GamepadControl.DPadRight, "D-Pad Right" },
			{ GamepadControl.Paddle1, "Paddle 1" },
			{ GamepadControl.Paddle2, "Paddle 2" },
			{ GamepadControl.Paddle3, "Paddle 3" },
			{ GamepadControl.Paddle4, "Paddle 4" },
			{ GamepadControl.LeftX, "Left Thumbstick X" },
			{ GamepadControl.LeftY, "Left Thumbstick Y" },
			{ GamepadControl.RightX, "Right Thumbstick X" },
			{ GamepadControl.RightY, "Right Thumbstick Y" },
			{ GamepadControl.LeftTrigger, "Left Trigger" },
			{ GamepadControl.RightTrigger, "Right Trigger" }
		};

		// Semantics for axes and buttons
		private static readonly ControllerSourceSemantics<bool> buttonSemantics = new() {
			IDName = "controller_button",
			TypeName = "Button",
			OnModify = (ControllerSource<bool> src, bool val) => {
				if (val) src.Handler.Fire(src);
			}
		};

		private static readonly ControllerSourceSemantics<float> axisSemantics = new() { IDName = "controller_axis", TypeName = "Axis" };

		/// <summary>
		/// The input manager this controller belongs to.
		/// </summary>
		public InputManager Manager { get; }

		/// <summary>
		/// The player index of the controller.
		/// </summary>
		public int PlayerIndex { get; }

		// The underlying control sources
		private readonly List<ControllerSource<bool>> buttons = new();
		private readonly List<ControllerSource<float>> axes = new();
		private readonly object[] gamepadControls = new object[2];

		internal readonly ControllerSource<Vector2> leftStick, rightStick;

		// The current joystick/gamepad
		private IJoystick? joystick = null;
		private IGamepad? gamepad = null;

		internal ControllerHandler(InputManager manager, int playerIndex) {
			Manager = manager;
			PlayerIndex = playerIndex;

			foreach (GamepadControl button in buttonControls) gamepadControls[(int)button] = new ControllerSource<bool>(this, buttonSemantics, 0, button);
			foreach (GamepadControl axis in axisControls) gamepadControls[(int)axis] = new ControllerSource<float>(this, axisSemantics, 0, axis);

			leftStick = new ControllerSource<Vector2>(this, new ControllerSourceSemantics<Vector2>() { IDName = "controller_axis", Name = "Left Stick" }, 0, null);
			rightStick = new ControllerSource<Vector2>(this, new ControllerSourceSemantics<Vector2>() { IDName = "controller_axis", Name = "Right Stick" }, 1, null);
		}

		/// <summary>
		/// The biaxial input for the left thumbstick.
		/// </summary>
		public IInputSource<Vector2> LeftStick { get; private set; } = NullInputSource<Vector2>.Instance;

		/// <summary>
		/// The biaxial input for the right thumbstick.
		/// </summary>
		public IInputSource<Vector2> RightStick { get; private set; } = NullInputSource<Vector2>.Instance;

		/// <summary>
		/// Event fired when the underlying joystick for the controller changes.
		/// </summary>
		public event Action? OnControlSchemeChanged;

		// Sets the underlying joystick for this controller
		internal void SetJoystick(IJoystick? joystick) {
			if (joystick != this.joystick) {
				this.joystick = joystick;
				gamepad = joystick as IGamepad;
				OnControlSchemeChanged?.Invoke();
			}
		}

		private ControllerSource<bool> GetButtonRaw(int index) {
			while (buttons.Count <= index) buttons.Add(new ControllerSource<bool>(this, buttonSemantics, index, null));
			return buttons[index];
		}

		private ControllerSource<float> GetAxisRaw(int index) {
			while (axes.Count <= index) axes.Add(new ControllerSource<float>(this, axisSemantics, index, null));
			return axes[index];
		}

		private ControllerSource<T>? GetGamepadControlRaw<T>(GamepadControl control) where T : struct, IEquatable<T> => gamepadControls[(int)control] as ControllerSource<T>;

		internal void Process() {
			if (joystick != null) {
				ReadOnlySpan<bool> buttons = joystick.Buttons;
				for(int i = 0; i < buttons.Length; i++) GetButtonRaw(i).CurrentValue = buttons[i];
				ReadOnlySpan<float> axes = joystick.Axes;
				for(int i = 0; i < axes.Length; i++) GetAxisRaw(i).CurrentValue = axes[i];

				if (gamepad != null) {
					int src;
					buttons = gamepad.GamepadButtons;
					foreach(GamepadControl button in buttonControls) {
						src = gamepad.GetGamepadControlIndex(button);
						if (src >= 0) {
							var control = GetGamepadControlRaw<bool>(button);
							if (control != null) control.CurrentValue = buttons[src];
						}
					}

					axes = gamepad.GamepadAxes;
					foreach(GamepadControl axis in axisControls) {
						src = gamepad.GetGamepadControlIndex(axis);
						if (src >= 0) {
							var control = GetGamepadControlRaw<float>(axis);
							if (control != null) control.CurrentValue = axes[src];
						}
					}

					leftStick.CurrentValue = new(GetGamepadControl<float>(GamepadControl.LeftX).CurrentValue, GetGamepadControl<float>(GamepadControl.LeftY).CurrentValue);
					rightStick.CurrentValue = new(GetGamepadControl<float>(GamepadControl.RightX).CurrentValue, GetGamepadControl<float>(GamepadControl.RightY).CurrentValue);
				}
			}
		}

		/// <summary>
		/// Gets the input source for the button at the given index.
		/// </summary>
		/// <param name="index">Index of the button</param>
		/// <returns>The corresponding button</returns>
		public IInputSource<bool> GetButton(int index) => GetButtonRaw(index);

		/// <summary>
		/// Gets the input source for the axis at the given index.
		/// </summary>
		/// <param name="index">Index of the axis</param>
		/// <returns>The corresponding axis</returns>
		public IInputSource<float> GetAxis(int index) => GetAxisRaw(index);

		/// <summary>
		/// Gets the input source for the given control.
		/// </summary>
		/// <typeparam name="T">Type of input value</typeparam>
		/// <param name="control">The control to get the source for</param>
		/// <returns>The corresponding input source for the control</returns>
		public IInputSource<T> GetGamepadControl<T>(GamepadControl control) where T : struct => gamepadControls[(int)control] as IInputSource<T> ?? NullInputSource<T>.Instance;

	}

}
