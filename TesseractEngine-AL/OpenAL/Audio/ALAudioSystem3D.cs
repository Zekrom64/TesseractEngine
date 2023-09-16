using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;

namespace Tesseract.OpenAL.Audio {

	public class ALAudioSystem3D : IAudioSystem3D {

		public ALCDevice Device { get; }

		public ALCContext Context { get; }

		public AL AL { get; }

		public AL11 AL11 => AL.AL11;

		public AudioDistanceModel DistanceModel {
			get => ALEnums.Convert(AL11.DistanceModel);
			set => AL11.DistanceModel = ALEnums.Convert(value);
		}

		public float SpeedOfSound {
			get => AL11.SpeedOfSound;
			set => AL11.SpeedOfSound = value;
		}

		public float DopplerFactor {
			get => AL11.DopplerFactor;
			set => AL11.DopplerFactor = value;
		}

		public IAudioListener DefaultListener { get; }

		public IAudioBuffer CreateBuffer(AudioBufferCreateInfo createInfo) => new ALAudioBuffer(this, createInfo);

		public IAudioEmitter CreateEmitter(AudioEmitterCreateInfo createInfo) => new ALAudioEmitter(this, createInfo);

		public ALAudioSystem3D() {
			string deviceName = ALC.GetString(ALCGetString.DefaultDeviceSpecifier)!;
			Device = new ALCDevice(deviceName);
			Context = Device.CreateContext(Span<int>.Empty);
			AL = new AL(Context);

			DefaultListener = new ALAudioListener(this);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Context.Dispose();
			Device.Dispose();
		}

	}

}
