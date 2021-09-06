using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Audio {

	public interface IAudioListener {

		public Vector3 Position { get; set; }

		public Vector3 Velocity { get; set; }

		public float Gain { get; set; }

		public Quaternion Orientation { get; set; }

	}

}
