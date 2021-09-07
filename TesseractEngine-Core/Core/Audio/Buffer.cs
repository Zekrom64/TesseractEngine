using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Core.Audio {

	/// <summary>
	/// An audio buffer provides storage for audio used in a 3D audio system.
	/// </summary>
	public interface IAudioBuffer : IDisposable {

		/// <summary>
		/// The format of audio data stored in the buffer.
		/// </summary>
		public AudioFormat Format { get; }

		/// <summary>
		/// The capacity of the audio buffer in samples.
		/// </summary>
		public int Capacity { get; }

		/// <summary>
		/// Maps the underlying memory of the audio buffer.
		/// </summary>
		/// <param name="mode">Memory mapping mode</param>
		/// <returns>Pointer to mapped memory</returns>
		public IPointer<byte> Map(MapMode mode);

		/// <summary>
		/// Unmaps the audio buffer memory.
		/// </summary>
		public void Unmap();

		/// <summary>
		/// Updates the contents of the audio buffer with the given sample data
		/// </summary>
		/// <typeparam name="T">Sample type</typeparam>
		/// <param name="data">Sample data</param>
		public void Update<T>(in ReadOnlySpan<T> data) where T : unmanaged;

		/// <summary>
		/// Updates the contents of the audio buffer with the given sample data
		/// </summary>
		/// <typeparam name="T">Sample type</typeparam>
		/// <param name="data">Pointer to sample data</param>
		/// <param name="length">Number of samples to update</param>
		public void Update<T>(IConstPointer<T> data, int length) where T : unmanaged;
	
	}

	/// <summary>
	/// Audio buffer creation information.
	/// </summary>
	public record AudioBufferCreateInfo {

		/// <summary>
		/// The audio format the buffer will use.
		/// </summary>
		public AudioFormat Format { get; init; }

		/// <summary>
		/// The number of samples the buffer will store.
		/// </summary>
		public int NumSamples { get; init; }

	}

}
