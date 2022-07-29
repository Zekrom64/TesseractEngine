using System;
using System.Collections.Generic;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Input {

	/// <summary>
	/// An input system provides access to a number of different input devices. Input devices are global; their
	/// reported states are presented irrespective of any windowing system. Input devices (particularly joysticks,
	/// gamepads, and haptic devices) may be hot swapped, and attempting to access devices after they have been
	/// disconnected results in undefined behavior. When an input system is disposed it releases any resources
	/// used back to the system, and resources are managed automatically while it is active.
	/// </summary>
	[ThreadSafety(ThreadSafetyLevel.MainThread)]
	public interface IInputSystem : Services.IServiceProvider, IDisposable {

		/// <summary>
		/// Event fired when an input device is connected to the system.
		/// </summary>
		public event Action<IInputDevice> OnConnected;

		/// <summary>
		/// The keyboard connected to the input system, or null if no keyboard is present.
		/// </summary>
		public IKeyboard Keyboard { get; }

		/// <summary>
		/// The mouse connected to the input system, or null if no mouse is present.
		/// </summary>
		public IMouse Mouse { get; }

		/// <summary>
		/// The list of joysticks connected to the input system.
		/// </summary>
		public IReadOnlyList<IJoystick> Joysticks { get; }

		/// <summary>
		/// The list of gamepads connected to the input system.
		/// </summary>
		public IReadOnlyList<IGamepad> Gamepads { get; }

		/// <summary>
		/// The list of haptic devices connected to the input system.
		/// </summary>
		public IReadOnlyList<IHapticDevice> HapticDevices { get; }

		/// <summary>
		/// The list of light systems connected to the input system.
		/// </summary>
		public IReadOnlyList<ILightSystem> LightSystems { get; }

		/// <summary>
		/// Polls the input system for events.
		/// </summary>
		public void RunEvents();

	}

	/// <summary>
	/// An input device is a device managed by an input system.
	/// </summary>
	public interface IInputDevice : Services.IServiceProvider {

		/// <summary>
		/// Gets a human-readable name for the input device. This may be determined from the
		/// device or be a generic name if no specific name is known.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Event fired if the device is disconnected. Once a device is disconnected it is
		/// no longer valid to access it.
		/// </summary>
		public event Action OnDisconnected;

	}

}
