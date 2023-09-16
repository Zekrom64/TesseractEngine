using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL.Audio {

	internal class ALAudioBuffer : IAudioBuffer {

		public ALAudioSystem3D AudioSystem { get; }

		public AL11 AL11 => AudioSystem.AL11;

		public uint Buffer { get; }


		public AudioFormat Format { get; }

		public int Capacity { get; }


		private ALFormat ALFormat { get; }


		internal ALAudioBuffer(ALAudioSystem3D system, AudioBufferCreateInfo createInfo) {
			AudioSystem = system;
			Buffer = AL11.GenBuffers();

			Format = createInfo.Format;
			Capacity = createInfo.NumSamples;

			ALFormat = ALEnums.Convert(createInfo.Format);
		}


		public void Dispose() {
			GC.SuppressFinalize(this);
			AL11.DeleteBuffers(Buffer);
		}

		public IPointer<byte> Map(MapMode mode) {
			throw new NotSupportedException();
		}

		public void Unmap() {
			throw new NotSupportedException();
		}

		public void Update<T>(in ReadOnlySpan<T> data) where T : unmanaged {
			AL11.BufferData(Buffer, ALFormat, data, Format.SampleRate);
		}

		public void Update<T>(IConstPointer<T> data, int length) where T : unmanaged {
			AL11.BufferData(Buffer, ALFormat, data.Ptr, length * Marshal.SizeOf<T>(), Format.SampleRate);
		}

	}

}
