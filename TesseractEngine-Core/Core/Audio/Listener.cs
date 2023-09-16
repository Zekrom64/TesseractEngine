using System;
using System.Numerics;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// A listener will receive audio from emitters and determine how it is affected using
	/// properties of both the emitter and listener.
	/// </summary>
	public interface IAudioListener : IDisposable {

		/// <summary>
		/// The position of the listener.
		/// </summary>
		public Vector3 Position { get; set; }

		/// <summary>
		/// The velocity of the listener.
		/// </summary>
		public Vector3 Velocity { get; set; }

		/// <summary>
		/// The amount of gain to apply to audio received by the listener.
		/// </summary>
		public float Gain { get; set; }

		/// <summary>
		/// A quaternion describing the orientation of the listener as a rotation from the "forward" vector of (0, 0, -1),
		/// looking into the Z-axis.
		/// </summary>
		public Quaternion Orientation { get; set; }

	}

}
