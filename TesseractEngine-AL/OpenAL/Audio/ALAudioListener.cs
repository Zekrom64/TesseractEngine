using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Audio;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenAL.Audio {

	internal class ALAudioListener : IAudioListener {

		public ALAudioSystem3D AudioSystem { get; }

		public AL11 AL11 => AudioSystem.AL11;

		public Vector3 Position {
			get => AL11.GetListener3f(ALListenerAttrib.Position);
			set => AL11.Listener3f(ALListenerAttrib.Position, value);
		}

		public Vector3 Velocity {
			get => AL11.GetListener3f(ALListenerAttrib.Velocity);
			set => AL11.Listener3f(ALListenerAttrib.Velocity, value);
		}

		public float Gain {
			get => AL11.GetListenerf(ALListenerAttrib.Gain);
			set => AL11.Listenerf(ALListenerAttrib.Gain, value);
		}

		public Quaternion Orientation {
			get {
				(Vector3 forward, Vector3 up) = AudioSystem.AL11.ListenerOrientation;
				Vector3 x = Vector3.Cross(forward, up).Normalize();
				Vector3 y = Vector3.Cross(up, forward).Normalize();
				Matrix4x4 matrix = Matrix4x4.Identity;
				matrix.M11 = x.X;
				matrix.M21 = y.X;
				matrix.M31 = forward.X;
				matrix.M12 = x.Y;
				matrix.M22 = y.Y;
				matrix.M32 = forward.Y;
				matrix.M13 = x.Z;
				matrix.M23 = y.Z;
				matrix.M33 = forward.Z;
				return Quaternion.CreateFromRotationMatrix(matrix);
			}
			set {
				var atVector = Vector3.Transform(new Vector3(0, 0, -1), value);
				var upVector = Vector3.Transform(new Vector3(0, 1, 0), value);
				AudioSystem.AL11.ListenerOrientation = (atVector, upVector);
			}
		}

		internal ALAudioListener(ALAudioSystem3D system) {
			AudioSystem = system;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}

}
