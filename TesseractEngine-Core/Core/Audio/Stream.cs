using System;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// <para>
	/// An audio stream provides a source of audio data.
	/// </para>
	/// <para>
	/// Audio streams may have a number of different sub-streams depending on the
	/// audio format the stream uses. In interleaved mode there is only one
	/// stream which all samples are stored in. In planar mode each channel has
	/// its own substream ordered by its appearance in the format's list of channels.
	/// </para>
	/// <para>
	/// Note: In interleaved mode the number of samples read should be an integer
	/// multiple of the number of channels in the format, such that only whole
	/// blocks of samples are read. Some implementations may cause undefined behavior
	/// if this is not done.
	/// </para>
	/// <para>
	/// Note: In planar mode new data may not become available on a stream until all
	/// substreams have advanced enough. If a substream's data is not needed it
	/// can be discarded.
	/// </para>
	/// </summary>
	public interface IAudioStream {

		/// <summary>
		/// The format the stream uses.
		/// </summary>
		public AudioFormat Format { get; }

		/// <summary>
		/// <para>Reads raw sample data from one of the substreams within this audio stream.</para>
		/// <para>
		/// While this method reads raw bytes instead of samples, attempting to read a number of bytes not
		/// an integer multiple of the size of the sample type may cause undefined behavior.
		/// </para>
		/// </summary>
		/// <param name="stream">The index of the substream to read from</param>
		/// <param name="buffer">Buffer to read sample data into</param>
		/// <returns></returns>
		public int Read(int stream, Span<byte> buffer);

	}

	/// <summary>
	/// <para>A form of <see cref="IAudioStream"/> that specifies the type of sample used.</para>
	/// <para>
	/// Note: While implementations are encouraged to derive from this interface, there is no explicit
	/// requirement that they do so and some may only derive from <see cref="IAudioStream"/>. 
	/// </para>
	/// </summary>
	/// <typeparam name="TSample"></typeparam>
	public interface IAudioStream<TSample> : IAudioStream {

		/// <summary>
		/// The format the stream uses.
		/// </summary>
		public new AudioFormat<TSample> Format { get; }

		AudioFormat IAudioStream.Format => Format;

		/// <summary>
		/// Reads samples from one of the substreams within this audio stream.
		/// </summary>
		/// <param name="stream">The index of the substream to read from</param>
		/// <param name="sampleBuffer">Buffer to read samples into</param>
		/// <returns></returns>
		public int ReadSamples(int stream, Span<TSample> sampleBuffer);

	}

}
