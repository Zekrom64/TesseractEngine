using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;
using Tesseract.Core.Collections;

namespace Tesseract.OpenAL.Audio {

	public static class ALEnums {

		private static readonly EquatableList<AudioChannel> Mono = new AudioChannel[] { AudioChannel.Center };
		private static readonly EquatableList<AudioChannel> Stereo = new AudioChannel[] { AudioChannel.Left, AudioChannel.Left };

		private static readonly Dictionary<ALFormat, AudioFormat> alToFormat = new() {
			{ ALFormat.Mono8, new AudioFormat() {
				Channels = Mono,
				SampleFormat = AudioSampleFormat.UnsignedByte,
				SampleRate = 0
			} },
			{ ALFormat.Stereo8, new AudioFormat() {
				Channels = Stereo,
				SampleFormat = AudioSampleFormat.UnsignedByte,
				SampleRate = 0
			} },
			{ ALFormat.Mono16, new AudioFormat() {
				Channels = Mono,
				SampleFormat = AudioSampleFormat.SignedShort,
				SampleRate = 0
			} },
			{ ALFormat.Stereo16, new AudioFormat() {
				Channels = Stereo,
				SampleFormat = AudioSampleFormat.SignedShort,
				SampleRate = 0
			} },
			{ ALFormat.MonoFloat32, new AudioFormat() {
				Channels = Mono,
				SampleFormat = AudioSampleFormat.Float,
				SampleRate = 0
			} },
			{ ALFormat.StereoFloat32, new AudioFormat() {
				Channels = Stereo,
				SampleFormat = AudioSampleFormat.Float,
				SampleRate = 0
			} }

		};

		private static readonly Dictionary<AudioFormat, ALFormat> formatToAL = alToFormat.ToDictionary(x => x.Value, x => x.Key);

		public static AudioFormat Convert(ALFormat format, int sampleRate) => alToFormat[format] with { SampleRate = sampleRate };

		public static ALFormat Convert(AudioFormat format) => formatToAL[format];


		public static ALDistanceModel Convert(AudioDistanceModel model) => model switch {
			AudioDistanceModel.Inverse => ALDistanceModel.InverseDistance,
			AudioDistanceModel.InverseClamped => ALDistanceModel.InverseDistanceClamped,
			AudioDistanceModel.Linear => ALDistanceModel.LinearDistance,
			AudioDistanceModel.LinearClamped => ALDistanceModel.LinearDistanceClamped,
			AudioDistanceModel.Exponential => ALDistanceModel.ExponentDistance,
			AudioDistanceModel.ExponentialClamped => ALDistanceModel.ExponentDistanceClamped,
			_ => default,
		};

		public static AudioDistanceModel Convert(ALDistanceModel model) => model switch {
			ALDistanceModel.InverseDistance => AudioDistanceModel.Inverse,
			ALDistanceModel.InverseDistanceClamped => AudioDistanceModel.InverseClamped,
			ALDistanceModel.LinearDistance => AudioDistanceModel.Linear,
			ALDistanceModel.LinearDistanceClamped => AudioDistanceModel.LinearClamped,
			ALDistanceModel.ExponentDistance => AudioDistanceModel.Exponential,
			ALDistanceModel.ExponentDistanceClamped => AudioDistanceModel.ExponentialClamped,
			_ => default
		};


		public static AudioEmitterState Convert(ALSourceState state) => state switch {
			ALSourceState.Initial => AudioEmitterState.Initial,
			ALSourceState.Playing => AudioEmitterState.Playing,
			ALSourceState.Paused => AudioEmitterState.Paused,
			ALSourceState.Stopped => AudioEmitterState.Stopped,
			_ => default
		};

	}

}
