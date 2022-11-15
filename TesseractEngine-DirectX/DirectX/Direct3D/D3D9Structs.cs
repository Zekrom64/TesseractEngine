using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Numerics;

namespace Tesseract.DirectX.Direct3D {

	// d3d9type.h

	[StructLayout(LayoutKind.Sequential)]
	public struct D3DColorValue {

		public float R;
		public float G;
		public float B;
		public float A;

		public static implicit operator D3DColorValue(Vector4 v) => new() { R = v.X, G = v.Y, B = v.Z, A = v.W };
		public static implicit operator Vector4(D3DColorValue c) => new() { X = c.R, Y = c.G, Z = c.B, W = c.A };

	}

}
