using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;

namespace Tesseract.OpenAL.Audio {

	internal class ALAudioEmitter : IAudioEmitter {

		public ALAudioSystem3D AudioSystem { get; }

		public AL11 AL11 => AudioSystem.AL11;

		private uint Source { get; }

		public AudioEmitterFlags Flags { get; }

		public Vector3 Position {
			get => AL11.GetSource3f(Source, ALSourceAttrib.Position);
			set => AL11.Source3f(Source, ALSourceAttrib.Position, value);
		}

		public Vector3 Velocity {
			get => AL11.GetSource3f(Source, ALSourceAttrib.Velocity);
			set => AL11.Source3f(Source, ALSourceAttrib.Velocity, value);
		}

		public Vector3 Direction {
			get => AL11.GetSource3f(Source, ALSourceAttrib.Direction);
			set => AL11.Source3f(Source, ALSourceAttrib.Direction, value);
		}

		public (float Min, float Max) DistanceClamp {
			get => (
				AL11.GetSourcef(Source, ALSourceAttrib.ReferenceDistance),
				AL11.GetSourcef(Source, ALSourceAttrib.MaxDistance)
			);
			set {
				AL11.Sourcef(Source, ALSourceAttrib.ReferenceDistance, value.Min);
				AL11.Sourcef(Source, ALSourceAttrib.MaxDistance, value.Max);
			}
		}

		public float Gain {
			get => AL11.GetSourcef(Source, ALSourceAttrib.Gain);
			set => AL11.Sourcef(Source, ALSourceAttrib.Gain, value);
		}

		public float InnerConeAngle {
			get => AL11.GetSourcef(Source, ALSourceAttrib.ConeInnerAngle);
			set => AL11.Sourcef(Source, ALSourceAttrib.ConeInnerAngle, value);
		}

		public float OuterConeAngle {
			get => AL11.GetSourcef(Source, ALSourceAttrib.ConeOuterAngle);
			set => AL11.Sourcef(Source, ALSourceAttrib.ConeOuterAngle, value);
		}

		public float AttenuationFactor {
			get => AL11.GetSourcef(Source, ALSourceAttrib.AirAbsorptionFactor);
			set => AL11.Sourcef(Source, ALSourceAttrib.AirAbsorptionFactor, value);
		}

		public AudioEmitterState State {
			get => ALEnums.Convert((ALSourceState)AL11.GetSourcei(Source, ALSourceAttrib.State));
			set {
				switch (value) {
					case AudioEmitterState.Playing:
						AL11.SourcePlay(Source);
						break;
					case AudioEmitterState.Paused:
						AL11.SourcePause(Source);
						break;
					case AudioEmitterState.Stopped:
						AL11.SourceStop(Source);
						break;
					default:
						break;
				}
			}
		}

		public bool Looping {
			get => AL11.GetSourcei(Source, ALSourceAttrib.Looping) != 0;
			set => AL11.Sourcei(Source, ALSourceAttrib.Looping, value ? 1 : 0);
		}


		internal ALAudioEmitter(ALAudioSystem3D system, AudioEmitterCreateInfo createInfo) {
			AudioSystem = system;
			Source = AL11.GenSources();
			Flags = createInfo.Flags;
			if (Flags.HasFlag(AudioEmitterFlags.Relative)) AL11.Sourcei(Source, ALSourceAttrib.Relative, 1);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			AL11.DeleteSources(Source);
		}


		private readonly List<ALAudioBuffer> queuedBuffers = new();

		public void Enqueue(IEnumerable<IAudioBuffer> buffers) {
			Span<uint> bufferIDs = stackalloc uint[buffers.Count()];
			int i = 0;
			foreach(IAudioBuffer buffer in buffers) {
				ALAudioBuffer alBuffer = (ALAudioBuffer)buffer;
				bufferIDs[i++] = alBuffer.Buffer;
				queuedBuffers.Add(alBuffer);
			}
			AL11.SourceQueueBuffers(Source, bufferIDs);
		}

		public void Enqueue(IAudioBuffer buffer) {
			ALAudioBuffer alBuffer = (ALAudioBuffer)buffer;
			queuedBuffers.Add(alBuffer);
			AL11.SourceQueueBuffers(Source, alBuffer.Buffer);
		}

		public IAudioBuffer[] Dequeue() {
			if (Flags.HasFlag(AudioEmitterFlags.Static)) throw new InvalidOperationException("Cannot dequeue from static emitter");

			int maxUnqueue = AL11.GetSourcei(Source, ALSourceAttrib.BuffersProcessed);
			Span<uint> bufferIDs = maxUnqueue > 1024 ? new uint[maxUnqueue] : stackalloc uint[maxUnqueue];
			bufferIDs.Clear();

			AL11.SourceUnqueueBuffers(Source, bufferIDs);

			List<IAudioBuffer> buffers = new(maxUnqueue);
			foreach(uint id in bufferIDs) {
				if (id == 0) break;
				for(int i = 0; i < queuedBuffers.Count; i++) {
					ALAudioBuffer buffer = queuedBuffers[i];
					if (buffer.Buffer == id) {
						buffers.Add(buffer);
						queuedBuffers.RemoveAt(i);
						break;
					}
				}
			}
			return buffers.ToArray();
		}

	}

}
