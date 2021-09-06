using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Core.Audio {

	public interface IAudioBuffer : IDisposable {

		public IPointer<byte> Map(MapMode mode);

		public void Unmap();

		public void Update<T>(in ReadOnlySpan<T> data) where T : unmanaged;

		public void Update<T>(IConstPointer<T> data, int length) where T : unmanaged;
	
	}

	public record AudioBufferCreateInfo {

		public AudioFormat Format { get; init; }

		public int NumSamples { get; init; }

	}

}
