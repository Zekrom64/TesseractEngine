using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Audio {
	
	public interface IAudioIO : IDisposable {

		public bool State { get; set; }

		public IAudioStream Stream { get; }

		public void Play() { State = true; }

		public void Pause() { State = false; }

		public float Gain { get; set; }

	}

	public record AudioOutputCreateInfo {

		public AudioFormat Format { get; init; }

	}

	public record AudioInputCreateInfo {

		public AudioFormat Format { get; init; }

	}

}
