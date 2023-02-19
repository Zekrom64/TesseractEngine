using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.DirectX.Direct2D {

	[StructLayout(LayoutKind.Sequential)]
	public struct D2DRectF {

		public float Left;
		public float Top;
		public float Right;
		public float Bottom;

		public static implicit operator D2DRectF(Rectf r) => new() {
			Left = r.Minimum.X,
			Top = r.Minimum.Y,
			Right = r.Maximum.X,
			Bottom = r.Maximum.Y
		};
		public static implicit operator Rectf(D2DRectF r) => new(r.Left, r.Right, r.Right - r.Left, r.Bottom - r.Top);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2DRectU {

		public uint Left;
		public uint Top;
		public uint Right;
		public uint Bottom;

		public static implicit operator D2DRectU(Recti r) => new() {
			Left = checked((uint)r.Minimum.X),
			Top = checked((uint)r.Minimum.Y),
			Right = checked((uint)r.Maximum.X),
			Bottom = checked((uint)r.Maximum.Y)
		};

	}

}
