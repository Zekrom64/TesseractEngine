using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Input {

	/// <summary>
	/// Interface implemented by types of haptic feedback information.
	/// </summary>
	public interface IHapticInfo { }

	/// <summary>
	/// A haptic effect is provided by a haptic device to manage
	/// a running effect once started. An effect may be stopped manually
	/// by disposing of it, and the effect will become invalid once stopped.
	/// Effects may be automatically stopped if they are timed or another
	/// effect is started.
	/// </summary>
	public interface IHapticEffect : IDisposable { }

	/// <summary>
	/// A simple rumble haptic effect.
	/// </summary>
	public record HapticRumbleInfo : IHapticInfo {

		/// <summary>
		/// The strength of the rumble, normalized between 0 and 1.
		/// </summary>
		public float Strength { get; init; }

		/// <summary>
		/// The duration of the rumble effect. If zero, the rumble is continuous.
		/// </summary>
		public TimeSpan Duration { get; init; }

	}

	/// <summary>
	/// A haptic device provides haptic feedback to a user. Note that haptic feedback devices
	/// are usually integrated into other devices such as joysticks and gamepads.
	/// </summary>
	public interface IHapticDevice : IInputDevice {

		/// <summary>
		/// If the haptic device supports multiple concurrent effects.
		/// </summary>
		public bool ConcurrentEffectSupport { get; }

		/// <summary>
		/// Tests if a haptic effect is supported.
		/// </summary>
		/// <param name="info">Haptic effect info</param>
		/// <returns>If the haptic effect is supported</returns>
		public bool IsEffectSupported(IHapticInfo info);

		/// <summary>
		/// Plays a haptic effect on this device. If concurrent effects are not
		/// supported the currently running effect will be stopped.
		/// </summary>
		/// <param name="info">Info for the new haptic event</param>
		/// <returns>The new haptic event</returns>
		public IHapticEffect PlayEffect(IHapticInfo info);

	}

}
