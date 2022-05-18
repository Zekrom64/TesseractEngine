using System;
using System.Numerics;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// Bitmask of audio emitter flags.
	/// </summary>
	[Flags]
	public enum AudioEmitterFlags {
		/// <summary>
		/// If the emitter's positional attributes (position, velocity, direction) are relative to the listener.
		/// </summary>
		Relative = 0x0001,
		/// <summary>
		/// If the emitter will use static buffering, ie. the entire audio clip is loaded
		/// into a single buffer used for playback. If set, only one buffer may be enqueued
		/// and dequeuing is not allowed.
		/// </summary>
		Static = 0x0002
	}

	/// <summary>
	/// Enumeration of playing states for an audio emitter.
	/// </summary>
	public enum AudioEmitterState {
		/// <summary>
		/// The audio emitter is playing audio from its buffers.
		/// </summary>
		Playing,
		/// <summary>
		/// The audio emitter is paused, and once resumed will continue
		/// playing audio from its current position.
		/// </summary>
		Paused,
		/// <summary>
		/// The audio emitter is stopped, with no buffers available for
		/// 
		/// </summary>
		Stopped,
		/// <summary>
		/// The audio emitter is in 
		/// </summary>
		Initial
	}

	public interface IAudioEmitter : IDisposable {

		/// <summary>
		/// The flags of this audio emitter.
		/// </summary>
		public AudioEmitterFlags Flags { get; }

		/// <summary>
		/// The position of the emitter.
		/// </summary>
		public Vector3 Position { get; set; }

		/// <summary>
		/// The velocity of the emitter.
		/// </summary>
		public Vector3 Velocity { get; set; }

		/// <summary>
		/// The gain to apply to audio from this emitter.
		/// </summary>
		public float Gain { get; set; }

		/// <summary>
		/// A normalized vector specifying the direction of the emitter. If this is set to a
		/// zero vector the emitter is not directional and will emit in every direction.
		/// </summary>
		public Vector3 Direction { get; set; }

		/// <summary>
		/// The angle of the inner directional cone where there is no attenuation.
		/// </summary>
		public float InnerConeAngle { get; set; }

		/// <summary>
		/// The angle of the outer directional cone where attenuation is applied.
		/// </summary>
		public float OuterConeAngle { get; set; }

		/// <summary>
		/// Multiplier for distance-based attenuation applied to this emitter.
		/// </summary>
		public float AttenuationFactor { get; set; }

		/// <summary>
		/// The state of the audio emitter.
		/// </summary>
		public AudioEmitterState State { get; set; }

		/// <summary>
		/// Transitions the audio emitter into a playing state.
		/// </summary>
		public void Play() { State = AudioEmitterState.Playing; }

		/// <summary>
		/// Transitions the audio emitter into a paused state.
		/// </summary>
		public void Pause() { State = AudioEmitterState.Paused; }

		/// <summary>
		/// Transitions the audio emitter into a stopped state.
		/// </summary>
		public void Stop() { State = AudioEmitterState.Stopped; }

		/// <summary>
		/// Attempts to rewind this audio emitter to the beginning. This is
		/// only valid for emitters created with the <see cref="AudioEmitterFlags.Static"/> flag.
		/// </summary>
		public void Rewind() { State = AudioEmitterState.Initial; }

		/// <summary>
		/// If the audio emitter will loop after reaching the end of its buffer. This is only
		/// valid for emitters created with the <see cref="AudioEmitterFlags.Static"/> flag.
		/// </summary>
		public bool Looping { get; set; }

		/// <summary>
		/// Enqueuest a set of audio buffers for playback.
		/// </summary>
		/// <param name="buffers">Buffers to enqueue</param>
		public void Enqueue(in ReadOnlySpan<IAudioBuffer> buffers);

		/// <summary>
		/// Enqueuest a set of audio buffers for playback.
		/// </summary>
		/// <param name="buffers">Buffers to enqueue</param>
		public void Enqueue(params IAudioBuffer[] buffers);

		/// <summary>
		/// Enqueuest a set of audio buffers for playback.
		/// </summary>
		/// <param name="buffers">Buffers to enqueue</param>
		public void Enqueue(IAudioBuffer buffer);

		/// <summary>
		/// Dequeues audio buffers as they become available.
		/// </summary>
		/// <returns>Dequeued audio buffers</returns>
		public IAudioBuffer[] Dequeue();

	}

	/// <summary>
	/// Audio emitter creation information.
	/// </summary>
	public record AudioEmitterCreateInfo {

		/// <summary>
		/// The audio emitter's flags.
		/// </summary>
		public AudioEmitterFlags Flags { get; init; }

		/// <summary>
		/// The audio format this emitter will use.
		/// </summary>
		public AudioFormat Format { get; init; } = null!;

	}

}
