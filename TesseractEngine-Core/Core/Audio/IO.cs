using System;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// An audio IO provides a way of recording or playing raw audio from an audio system.
	/// </summary>
	public interface IAudioIO : IDisposable {

		/// <summary>
		/// The state of the audio IO (if it is playing or not).
		/// </summary>
		public bool State { get; set; }

		/// <summary>
		/// The underlying audio stream provided by this IO.
		/// </summary>
		public IAudioStream Stream { get; }

		/// <summary>
		/// Sets the audio IO to play.
		/// </summary>
		public void Play() { State = true; }

		/// <summary>
		/// Sets the audio IO to pause.
		/// </summary>
		public void Pause() { State = false; }

		/// <summary>
		/// The gain to apply to audio processed by this audio IO.
		/// </summary>
		public float Gain { get; set; }

	}

	/// <summary>
	/// Audio output creation information.
	/// </summary>
	public record AudioOutputCreateInfo {

		/// <summary>
		/// The audio format this output will use.
		/// </summary>
		public AudioFormat Format { get; init; } = null!;

	}

	/// <summary>
	/// Audio input creation information.
	/// </summary>
	public record AudioInputCreateInfo {

		/// <summary>
		/// The audio format this input will use.
		/// </summary>
		public AudioFormat Format { get; init; } = null!;

	}

}
