using System;
using System.Collections.Generic;
using Tesseract.Core.Util;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// Enumeration of audio sample formats.
	/// </summary>
	public enum AudioSampleFormat {
		/// <summary>
		/// Each sample is an unsigned 8-bit integer.
		/// </summary>
		UnsignedByte,
		/// <summary>
		/// Each sample is a signed 16-bit integer.
		/// </summary>
		SignedShort,
		/// <summary>
		/// Each sample is a signed 32-bit integer.
		/// </summary>
		SignedInt,
		/// <summary>
		/// Each sample is a normalized 32-bit floating-point number.
		/// </summary>
		Float,
		/// <summary>
		/// Each sample is a normalized 64-bit floating-point number.
		/// </summary>
		Double
	}

	/// <summary>
	/// Enumeration of audio channels.
	/// </summary>
	public enum AudioChannel {
		/// <summary>
		/// An undefined audio channel. Used as a placeholder for channels whose type is not known.
		/// </summary>
		Undefined,
		/// <summary>
		/// Left channel.
		/// </summary>
		Left,
		/// <summary>
		/// Right channel.
		/// </summary>
		Right,
		/// <summary>
		/// Center channel.
		/// </summary>
		Center
	}

	/// <summary>
	/// An audio format describes how audio samples are stored and processed.
	/// </summary>
	public record AudioFormat {

		/// <summary>
		/// Gets the number of bytes per sample for a given audio sample format.
		/// </summary>
		/// <param name="format">Audio sample format</param>
		/// <returns>The number of bytes per sample</returns>
		public static int GetBytesPerSample(AudioSampleFormat format) => format switch {
			AudioSampleFormat.UnsignedByte => 1,
			AudioSampleFormat.SignedShort => 2,
			AudioSampleFormat.SignedInt => 4,
			AudioSampleFormat.Float => 4,
			AudioSampleFormat.Double => 8,
			_ => 0
		};

		/// <summary>
		/// The list of channels in this format, in the order they are stored either in individual streams for planar formats or
		/// by their ordering in a block of samples for interleaved formats.
		/// </summary>
		public IReadOnlyList<AudioChannel> Channels { get; init; } = Collections<AudioChannel>.EmptyList;

		/// <summary>
		/// If the audio is accessed in a planar manner, with each channel supplied in an independent stream of samples. Otherwise,
		/// the audio is accessed in an interleaved manner with a single stream of groups of samples for each channel.
		/// </summary>
		public bool IsPlanar { get; init; }

		/// <summary>
		/// The number of samples per second in each channel.
		/// </summary>
		public int SampleRate { get; init; }

		/// <summary>
		/// The format of each sample in the format.
		/// </summary>
		public AudioSampleFormat SampleFormat { get; init; }

		/// <summary>
		/// The number of bytes per sample based on the sample format of this format.
		/// </summary>
		public int BytesPerSample => GetBytesPerSample(SampleFormat);

		/// <summary>
		/// The number of bytes per block of samples. If planar this is simply <see cref="BytesPerSample"/>, else it
		/// is the number of bytes per sample multiplied by the number of channels.
		/// </summary>
		public int BytesPerBlock => BytesPerSample * (IsPlanar ? 1 : Channels.Count);

	}

	/// <summary>
	/// An audio format with a type-constrained sample type. <see cref="AudioFormat.SampleFormat"/> is determined from this type.
	/// </summary>
	/// <typeparam name="TSample">The type of sample to use</typeparam>
	public record AudioFormat<TSample> : AudioFormat {

		private static readonly Dictionary<Type, AudioSampleFormat> typeToSampleFormat = new() {
			{ typeof(byte), AudioSampleFormat.UnsignedByte },
			{ typeof(short), AudioSampleFormat.SignedShort },
			{ typeof(int), AudioSampleFormat.SignedInt },
			{ typeof(float), AudioSampleFormat.Float },
			{ typeof(double), AudioSampleFormat.Double }
		};

		public AudioFormat() {
			SampleFormat = typeToSampleFormat[typeof(TSample)];
		}

	}

}
