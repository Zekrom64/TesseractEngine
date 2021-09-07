using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// An audio system provides a simple method of playing and capturing audio.
	/// </summary>
	public interface IAudioSystem : IDisposable {

		/// <summary>
		/// Creates an audio output.
		/// </summary>
		/// <param name="createInfo">Audio output creation info</param>
		/// <returns>Audio output</returns>
		public IAudioIO CreateOutput(AudioOutputCreateInfo createInfo);

		/// <summary>
		/// Creates an audio input.
		/// </summary>
		/// <param name="createInfo">Audio input creation info</param>
		/// <returns>Audio input</returns>
		public IAudioIO CreateInput(AudioInputCreateInfo createInfo);

	}

	/// <summary>
	/// Enumeration of audio distance models for 3D audio systems.
	/// </summary>
	public enum AudioDistanceModel {
		/// <summary>
		/// Audio attenuation increases inversly with distance (~= d / dMin).
		/// </summary>
		Inverse,
		/// <summary>
		/// Similar to <see cref="Inverse"/>, but distances are first clamped using <see cref="IAudioSystem3D.DistanceClamp"/>.
		/// </summary>
		InverseClamped,
		/// <summary>
		/// Audio attenuation increases linearly with distance (~= d-dMin / dMax-dMin).
		/// </summary>
		Linear,
		/// <summary>
		/// Similar to <see cref="Linear"/>, but distances are first clamped using <see cref="IAudioSystem3D.DistanceClamp"/>.
		/// </summary>
		LinearClamped,
		/// <summary>
		/// Audio attenuation increases exponentially with distance (~= dMin / d).
		/// </summary>
		Exponential,
		/// <summary>
		/// Similar to <see cref="Exponential"/>, but distances are first clamped using <see cref="IAudioSystem3D.DistanceClamp"/>.
		/// </summary>
		ExponentialClamped
	}

	/// <summary>
	/// <para>
	/// A 3D audio system provides more advanced methods of audio playback, performing
	/// audio processing to simulate audio effects in a 3D environment such as attenuation over
	/// distance, directional audio, and the doppler effect.
	/// </para>
	/// <para>
	/// 3D audio processing is done using listeners (<see cref="IAudioListener"/>) and emitters (<see cref="IAudioEmitter"/>).
	/// There is a builtin <see cref="DefaultListener"/> which will output to the audio output defined during the creation
	/// of a 3D audio system. Each emitter has audio data supplied to it by <see cref="IAudioBuffer"/>s. By default emitters
	/// operate in streaming mode, where each enqueued buffer will be used in order and then dequeued for reuse. However,
	/// emitters can also operate in static mode where only a single buffer will be used for the lifetime of the buffer and
	/// will store the entire audio clip. This mode may be more useful for short and often-used sound effects.
	/// </para>
	/// </summary>
	public interface IAudioSystem3D : IDisposable {

		/// <summary>
		/// The range of distances to use in a clamped distance model.
		/// </summary>
		public (float, float) DistanceClamp { get; set; }

		/// <summary>
		/// The model to use for attenuation based on distance.
		/// </summary>
		public AudioDistanceModel DistanceModel { get; set; }

		/// <summary>
		/// The speed of sound in the 3D audio model.
		/// </summary>
		public float SpeedOfSound { get; set; }

		/// <summary>
		/// Factor determining the influence of the doppler effect, larger values will increase the effect.
		/// </summary>
		public float DopplerFactor { get; set; }

		/// <summary>
		/// Gets the default listener for the audio system. This is managed by the audio system
		/// and should not be disposed.
		/// </summary>
		public IAudioListener DefaultListener { get; }

		/// <summary>
		/// Creates an audio buffer.
		/// </summary>
		/// <param name="createInfo">Audio buffer creation information</param>
		/// <returns>Created audio buffer</returns>
		public IAudioBuffer CreateBuffer(AudioBufferCreateInfo createInfo);

		/// <summary>
		/// Creates an audio emitter.
		/// </summary>
		/// <param name="createInfo">Audio emitter creation information</param>
		/// <returns>Created audio emitter</returns>
		public IAudioEmitter CreateEmitter(AudioEmitterCreateInfo createInfo);

	}

}
