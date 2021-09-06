using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Audio {

	public enum AudioEmitterFlags {
		Relative = 0x0001,
		Static = 0x0002
	}

	public enum AudioEmitterState {
		Undefined,
		Playing,
		Paused,
		Stopped,
		Initial
	}

	public interface IAudioEmitter {

		public AudioEmitterFlags Flags { get; }

		public Vector3 Position { get; set; }
		
		public Vector3 Velocity { get; set; }

		public float Gain { get; set; }

		public Vector3 Direction { get; set; }

		public float InnerConeAngle { get; set; }

		public float OuterConeAngle { get; set; }

		public float AttenuationFactor { get; set; }

		public AudioEmitterState State { get; set; }

		public void Play() { State = AudioEmitterState.Playing; }

		public void Pause() { State = AudioEmitterState.Paused; }

		public void Stop() { State = AudioEmitterState.Stopped; }

		public void Rewind() { State = AudioEmitterState.Initial; }

		public bool Looping { get; set; }

		public void Enqueue(in ReadOnlySpan<IAudioBuffer> buffers);

		public void Enqueue(params IAudioBuffer[] buffers);

		public void Enqueue(IAudioBuffer buffer);

		public IAudioBuffer[] Dequeue();

	}

	public record AudioEmitterCreateInfo {

		public AudioEmitterFlags Flags { get; init; }

		public AudioFormat Format { get; init; }

	}

}
