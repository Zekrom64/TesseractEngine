using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Audio {

	public interface IAudioSystem : IDisposable {

		public IAudioIO CreateOutput(AudioOutputCreateInfo createInfo);

		public IAudioIO CreateInput(AudioInputCreateInfo createInfo);

	}

	public interface IAudioSystem3D : IDisposable {

		public IAudioListener DefaultListener { get; }

		public IAudioBuffer CreateBuffer(AudioBufferCreateInfo createInfo);

		public IAudioEmitter CreateEmitter(AudioEmitterCreateInfo createInfo);

	}

}
