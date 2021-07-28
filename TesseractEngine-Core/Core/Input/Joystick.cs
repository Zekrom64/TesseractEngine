using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Input {

	public enum GamepadControl {
		// Buttons
		/// <summary>
		/// The "A" or "cross" button.
		/// </summary>
		A,
		/// <summary>
		/// The "B" or "circle" button.
		/// </summary>
		B,
		/// <summary>
		/// The "X" or "square" button.
		/// </summary>
		X,
		/// <summary>
		/// The "Y" or "triangle" button.
		/// </summary>
		Y,
		/// <summary>
		/// The left bumper button.
		/// </summary>
		LeftBumper,
		/// <summary>
		/// The right bumper button.
		/// </summary>
		RightBumper,
		/// <summary>
		/// The back button.
		/// </summary>
		Back,
		/// <summary>
		/// The start button.
		/// </summary>
		Start,
		/// <summary>
		/// The guide button.
		/// </summary>
		Guide,
		/// <summary>
		/// The left thumb stick button.
		/// </summary>
		LeftThumbStick,
		/// <summary>
		/// The right thumb stick button.
		/// </summary>
		RightThumbStick,
		/// <summary>
		/// The D-Pad up button.
		/// </summary>
		DPadUp,
		/// <summary>
		/// The D-Pad down button.
		/// </summary>
		DPadDown,
		/// <summary>
		/// The D-Pad left button.
		/// </summary>
		DPadLeft,
		/// <summary>
		/// The D-Pad right button.
		/// </summary>
		DPadRight,
		/// <summary>
		/// Miscellaneous button (generally mapped to social/share/capture functionality).
		/// </summary>
		Miscellaneous,
		/// <summary>
		/// The first paddle button.
		/// </summary>
		Paddle1,
		/// <summary>
		/// The second paddle button.
		/// </summary>
		Paddle2,
		/// <summary>
		/// The third paddle button.
		/// </summary>
		Paddle3,
		/// <summary>
		/// The fourth paddle button.
		/// </summary>
		Paddle4,
		/// <summary>
		/// The touchpad button (such as on Playstation systems).
		/// </summary>
		Touchpad,
		// Axes
		/// <summary>
		/// The left thumb stick X axis.
		/// </summary>
		LeftX,
		/// <summary>
		/// The left thumb stick Y axis.
		/// </summary>
		LeftY,
		/// <summary>
		/// The right thumb stick X axis.
		/// </summary>
		RightX,
		/// <summary>
		/// The right thumb stick Y axis.
		/// </summary>
		RightY,
		/// <summary>
		/// The left trigger axis.
		/// </summary>
		LeftTrigger,
		/// <summary>
		/// The right trigger axis.
		/// </summary>
		RightTrigger
	}

	/// <summary>
	/// Enumeration of joystick hat states.
	/// </summary>
	public enum HatState {
		/// <summary>
		/// The hat is centered.
		/// </summary>
		Centered,
		/// <summary>
		/// The hat is up.
		/// </summary>
		Up,
		/// <summary>
		/// The hat is down.
		/// </summary>
		Down,
		/// <summary>
		/// The hat is left.
		/// </summary>
		Left,
		/// <summary>
		/// The hat is right.
		/// </summary>
		Right,
		/// <summary>
		/// The hat is to the top left.
		/// </summary>
		UpLeft,
		/// <summary>
		/// The hat is to the top right.
		/// </summary>
		UpRight,
		/// <summary>
		/// The hat is to the bottom left.
		/// </summary>
		DownLeft,
		/// <summary>
		/// The hat is to the bottom right.
		/// </summary>
		DownRight
	}
	
	/// <summary>
	/// A joystick is an input device that can have many different input mechanisms including buttons,
	/// axes (analog linear/directional inputs), and hats (specialized directional switches). Joysticks
	/// may also have additional features such as software-controlled rumbling, LED lights, or battery
	/// level detection. Joysticks are somewhat generic and do not specify a layout for these inputs, a
	/// more common specific type of joystick is a <see cref="IGamepad"/>.
	/// </summary>
	public interface IJoystick : IInputDevice {

		/// <summary>
		/// If the joystick is a gamepad. This can also be done by checking if the object
		/// is an instance of an <see cref="IGamepad"/> but this getter is more convenient.
		/// </summary>
		public bool IsGamepad { get; }

		/// <summary>
		/// The state of the buttons of the joystick.
		/// </summary>
		public bool[] Buttons { get; }

		/// <summary>
		/// The state of the axes of the joystick, normalized between ±1.
		/// </summary>
		public float[] Axes { get; }

		/// <summary>
		/// The state of the hats of the joystick.
		/// </summary>
		public HatState[] Hats { get; }

	}

	/// <summary>
	/// A gamepad is a type of joystick that follows a semi-standard layout. These are primarily
	/// based on the Playstation and Xbox controllers, but other manufacturers share a similar
	/// layout. While all gamepads are instances of joystick controls, gamepads have a more
	/// standardized layout and are easier to configure that joysticks (as pure joysticks
	/// provide no indication of how their buttons and axes are laid out).
	/// </summary>
	public interface IGamepad : IJoystick {

		/// <summary>
		/// The state of the buttons of the gamepad.
		/// </summary>
		public bool[] GamepadButtons { get; }

		/// <summary>
		/// The state of the axes of the gamepad, normalized between ±1.
		/// </summary>
		public float[] GamepadAxes { get; }

		/// <summary>
		/// Gets the index of a standard gamepad control. If the control does not
		/// exist its index will be -1. This index can be used to look up the corresponding
		/// state of the control in <see cref="GamepadButtons"/> or <see cref="GamepadAxes"/>.
		/// Note controls are either buttons or axes exclusively, check the control to see which
		/// it is.
		/// </summary>
		/// <param name="control">The control to get the index of</param>
		/// <returns>The index of the control in the gamepad state</returns>
		public int GetGamepadControlIndex(GamepadControl control);

	}

}
