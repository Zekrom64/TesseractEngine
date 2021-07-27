using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Math;

namespace Tesseract.Core.Input {

	/// <summary>
	/// A mouse button event ocurrs when a button on the mouse changes state.
	/// </summary>
	public struct MouseButtonEvent {

		/// <summary>
		/// The position of the mouse when the button is activated.
		/// </summary>
		public Vector2i Position;

		/// <summary>
		/// The button causing the event.
		/// </summary>
		public int Button;

		/// <summary>
		/// The modifier key state at the time of the event.
		/// </summary>
		public KeyMod Mod;

		/// <summary>
		/// The new state of the mouse button.
		/// </summary>
		public bool State;

	}

	/// <summary>
	/// A mouse movement event is fired when a mouse moves.
	/// </summary>
	public struct MouseMoveEvent {

		/// <summary>
		/// The new position of the mouse.
		/// </summary>
		public Vector2i Position;

		/// <summary>
		/// The difference between the new and previous mouse positions.
		/// </summary>
		public Vector2i Delta;

	}

	/// <summary>
	/// A mouse wheel event is fired when the wheel(s) on a mouse move. Note that most mice only
	/// report wheel movement in the Y axis for scrolling.
	/// </summary>
	public struct MouseWheelEvent {

		/// <summary>
		/// The position of the mouse when the wheels are scrolled.
		/// </summary>
		public Vector2i Position;

		/// <summary>
		/// The number of 'clicks' the wheels moved in the X and Y directions.
		/// </summary>
		public Vector2i Delta;

	}

	/// <summary>
	/// A mouse input provides mouse input events and state.
	/// </summary>
	public interface IMouseInput {

		/// <summary>
		/// Event fired when the mouse is moved.
		/// </summary>
		public event Action<MouseMoveEvent> OnMouseMove;

		/// <summary>
		/// Event fired when a mouse button is activated.
		/// </summary>
		public event Action<MouseButtonEvent> OnMouseButton;

		/// <summary>
		/// Event fired when the scroll wheel(s) of a mouse move.
		/// </summary>
		public event Action<MouseWheelEvent> OnMouseWheel;

		/// <summary>
		/// Gets the current state of a mouse button. Buttons that do not exist will
		/// report as false (released).
		/// </summary>
		/// <param name="button">The button to get the state of</param>
		/// <returns>The state of the button (pressed or released)</returns>
		public bool GetMouseButtonState(int button);

		/// <summary>
		/// Gets the position of the mouse relative to this input.
		/// </summary>
		public Vector2i MousePosition { get; }

	}

	/// <summary>
	/// A mouse provides positional input. Note that there may technically be multiple mice
	/// connected to a system, but this input system only reports as if there is one mouse (ie.
	/// the input is based on the system cursor).
	/// </summary>
	public interface IMouse : IInputDevice, IMouseInput {

		/// <summary>
		/// ID of the left mouse button.
		/// </summary>
		public const int LeftButton = 0;

		/// <summary>
		/// ID of the middle mouse button. For most mice this is integrated with the scroll wheel,
		/// but some mice may have it activated differently (such as touchpads) or in some cases
		/// have no middle button.
		/// </summary>
		public const int MiddleButton = 1;

		/// <summary>
		/// ID of the right mouse button.
		/// </summary>
		public const int RightButton = 2;

		/// <summary>
		/// <para>
		/// Creates an ID for an auxiliary mouse button.
		/// </para>
		/// <para>
		/// Mice may have more than the standard 2-3 buttons, and these are considered "auxiliary buttons".
		/// They are reported via their own IDs which can be made with this method.
		/// </para>
		/// </summary>
		/// <param name="index">The zero-based index of the auxiliary mouse button</param>
		/// <returns>The ID of the auxiliary mouse button</returns>
		public static int GetAuxMouseButton(int index) => index + 3;

		/// <summary>
		/// If access to the global mouse state is supported.
		/// </summary>
		public bool HasGlobalMouseState { get; }

	}

}
