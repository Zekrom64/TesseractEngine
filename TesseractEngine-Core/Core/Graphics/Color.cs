using System.Numerics;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics {

	public interface IReadOnlyColor {

		public Vector4 Normalized { get; }

	}

	public interface IColor : IReadOnlyColor {

		public new Vector4 Normalized { get; set; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color3b : ITuple3<byte>, IColor {

		public byte R;
		public byte G;
		public byte B;

		public Color3b(byte r, byte g, byte b) {
			R = r;
			G = g;
			B = b;
		}

		public byte this[int key] {
			get => key switch {
				0 => R,
				1 => G,
				2 => B,
				_ => default
			};
			set {
				switch (key) {
					case 0: R = value; break;
					case 1: G = value; break;
					case 2: B = value; break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		public byte X { get => R; set => R = value; }
		public byte Y { get => G; set => G = value; }
		public byte Z { get => B; set => B = value; }

		byte IReadOnlyTuple<byte, byte>.X => R;
		byte IReadOnlyTuple<byte, byte>.Y => G;
		byte IReadOnlyTuple<byte, byte, byte>.Z => B;

		public Vector4 Normalized {
			get => new(R / 255.0f, G / 255.0f, B / 255.0f, 1.0f);
			set {
				R = (byte)(value.X * 255.0f);
				G = (byte)(value.Y * 255.0f);
				B = (byte)(value.Z * 255.0f);
			}
		}

		Vector4 IReadOnlyColor.Normalized => Normalized;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color4b : ITuple4<byte>, IColor {

		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public byte this[int key] {
			get => key switch {
				0 => R,
				1 => G,
				2 => B,
				3 => A,
				_ => default
			};
			set {
				switch (key) {
					case 0: R = value; break;
					case 1: G = value; break;
					case 2: B = value; break;
					case 3: A = value; break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		public byte Z { get => R; set => R = value; }
		public byte X { get => G; set => G = value; }
		public byte Y { get => B; set => B = value; }
		public byte W { get => A; set => A = value; }

		byte IReadOnlyTuple<byte, byte>.X => R;
		byte IReadOnlyTuple<byte, byte>.Y => G;
		byte IReadOnlyTuple<byte, byte, byte>.Z => B;
		byte IReadOnlyTuple<byte, byte, byte, byte>.W => A;

		public Vector4 Normalized {
			get => new(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);
			set {
				R = (byte)(value.X * 255.0f);
				G = (byte)(value.Y * 255.0f);
				B = (byte)(value.Z * 255.0f);
				A = (byte)(value.W * 255.0f);
			}
		}

		Vector4 IReadOnlyColor.Normalized => Normalized;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color4f : ITuple4<float>, IColor {

		public Vector4 Vector;

		public Color4f(ITuple4<float> tuple) {
			Vector = new(tuple.X, tuple.Y, tuple.Z, tuple.W);
		}

		public Color4f(Vector4 vector) {
			Vector = vector;
		}

		public Color4f(float r, float g, float b) {
			Vector = new Vector4(r, g, b, 1.0f);
		}

		public Color4f(float r, float g, float b, float a) {
			Vector = new Vector4(r, g, b, a);
		}

		public float R { get => Vector.X; set => Vector.X = value; }
		public float G { get => Vector.Y; set => Vector.Y = value; }
		public float B { get => Vector.Z; set => Vector.Z = value; }
		public float A { get => Vector.W; set => Vector.W = value; }

		public float Z { get => R; set => R = value; }
		public float X { get => G; set => G = value; }
		public float Y { get => B; set => B = value; }
		public float W { get => A; set => A = value; }

		float IReadOnlyTuple<float, float>.X => R;
		float IReadOnlyTuple<float, float>.Y => G;
		float IReadOnlyTuple<float, float, float>.Z => B;
		float IReadOnlyTuple<float, float, float, float>.W => A;

		public float this[int key] {
			get => key switch {
				0 => R,
				1 => G,
				2 => B,
				3 => A,
				_ => default
			};
			set {
				switch (key) {
					case 0: R = value; break;
					case 1: G = value; break;
					case 2: B = value; break;
					case 3: A = value; break;
				}
			}
		}

		float IReadOnlyIndexer<int, float>.this[int key] => this[key];

		public Vector4 Normalized { get => Vector; set => Vector = value; }

		Vector4 IReadOnlyColor.Normalized => Vector;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color3f : ITuple3<float>, IColor {

		public Vector3 Vector;

		public Color3f(ITuple4<float> tuple) {
			Vector = new(tuple.X, tuple.Y, tuple.Z);
		}

		public Color3f(Vector3 vector) {
			Vector = vector;
		}

		public Color3f(float r, float g, float b) {
			Vector = new Vector3(r, g, b);
		}

		public float R { get => Vector.X; set => Vector.X = value; }
		public float G { get => Vector.Y; set => Vector.Y = value; }
		public float B { get => Vector.Z; set => Vector.Z = value; }

		public float Z { get => R; set => R = value; }
		public float X { get => G; set => G = value; }
		public float Y { get => B; set => B = value; }

		float IReadOnlyTuple<float, float>.X => R;
		float IReadOnlyTuple<float, float>.Y => G;
		float IReadOnlyTuple<float, float, float>.Z => B;

		public float this[int key] {
			get => key switch {
				0 => R,
				1 => G,
				2 => B,
				_ => default
			};
			set {
				switch (key) {
					case 0: R = value; break;
					case 1: G = value; break;
					case 2: B = value; break;
				}
			}
		}

		float IReadOnlyIndexer<int, float>.this[int key] => this[key];

		public Vector4 Normalized { get => new(Vector, 1.0f); set => Vector = new Vector3() { X = value.X, Y = value.Y, Z = value.Z }; }

		Vector4 IReadOnlyColor.Normalized => new(Vector, 1.0f);

	}

	public static class Colors {

		public static Color4f Average(this IReadOnlyColor c1, IReadOnlyColor c2) => new((c1.Normalized + c2.Normalized) * 0.5f);

		public static Color4f Mix(this IReadOnlyColor c1, IReadOnlyColor c2, float ratio) => new((c1.Normalized * ratio) + (c2.Normalized * (1.0f - ratio)));

	}

}
