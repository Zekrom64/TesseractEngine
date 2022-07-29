using System.Numerics;
using Tesseract.Core.Graphics;

namespace Tesseract.Core.Input {

	/// <summary>
	/// A light pattern determines how the lights of a light system are lit.
	/// </summary>
	public interface ILightPattern { }

	/// <summary>
	/// A simple light pattern setting all lights to a single color.
	/// </summary>
	public record SingleLightPattern : ILightPattern {

		/// <summary>
		/// The color of the light.
		/// </summary>
		public Vector3 Color { get; init; } = Vector3.Zero;

	}

	/// <summary>
	/// A light system controls a system of lights associated with an input device. While
	/// not strictly an input device, this can provide visual feedback to a user.
	/// </summary>
	public interface ILightSystem : IInputDevice {

		/// <summary>
		/// Tests if a light pattern is supported.
		/// </summary>
		/// <param name="pattern">Pattern to test</param>
		/// <returns>If the light pattern is supported</returns>
		public bool IsLightPatternSupported(ILightPattern pattern);

		/// <summary>
		/// Sets the pattern of lights on the device.
		/// </summary>
		/// <param name="pattern"></param>
		public void SetLightPattern(ILightPattern pattern);

	}

}
