using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Numerics {

	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix2x2 : IEquatable<Matrix2x2> {

		public static readonly Matrix2x2 Zero = new(0, 0, 0, 0);
		public static readonly Matrix2x2 Identity = new(1, 0, 0, 1);

		public float M11;
		public float M12;
		public float M21;
		public float M22;

		public Vector2 R1 {
			get => new(M11, M12);
			set {
				M11 = value.X;
				M12 = value.Y;
			}
		}
		public Vector2 R2 {
			get => new(M21, M22);
			set {
				M21 = value.X;
				M22 = value.Y;
			}
		}

		public Vector2 C1 {
			get => new(M11, M21);
			set {
				M11 = value.X;
				M21 = value.Y;
			}
		}
		public Vector2 C2 {
			get => new(M21, M22);
			set {
				M21 = value.X;
				M22 = value.Y;
			}
		}

		public bool IsIdentity => M11 == 1 && M12 == 0 && M21 == 0 && M22 == 1;

		public float Determinant => M11 * M22 - M12 * M21;

		public Matrix2x2 Inverse {
			get {
				float det = Determinant;
				if (det != 0) det = 1.0f / det;
				return new Matrix2x2(
					M22 * det, M12* -det,
					M21 * -det, M11 * det
				);
			}
		}

		public Matrix2x2(float m11, float m12, float m21, float m22) {
			M11 = m11;
			M12 = m12;
			M21 = m21;
			M22 = m22;
		}

		public bool Equals(Matrix2x2 other) => this == other;

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is Matrix2x2 m && Equals(m);

		public override int GetHashCode() =>
			M11.GetHashCode() + (M12.GetHashCode() * 5) + (M21.GetHashCode() * 10) + (M22.GetHashCode() * 15);

		public override string ToString() => $"{{ {{M11:{M11} M12:{M12}}} {{M21:{M21} M22:{M22}}} }}";

		public static bool operator ==(Matrix2x2 lhs, Matrix2x2 rhs) => lhs.M11 == rhs.M11 && lhs.M12 == rhs.M12 && lhs.M21 == rhs.M21 && lhs.M22 == rhs.M22;

		public static bool operator !=(Matrix2x2 lhs, Matrix2x2 rhs) => !(lhs == rhs);

		public static Matrix2x2 operator +(Matrix2x2 lhs, Matrix2x2 rhs) => new(lhs.M11 + rhs.M11, lhs.M12 + rhs.M12, lhs.M21 + rhs.M21, lhs.M22 + rhs.M22);

		public static Matrix2x2 operator -(Matrix2x2 lhs, Matrix2x2 rhs) => new(lhs.M11 - rhs.M11, lhs.M12 - rhs.M12, lhs.M21 - rhs.M21, lhs.M22 - rhs.M22);

		public static Matrix2x2 operator -(Matrix2x2 m) => new(-m.M11, -m.M12, -m.M21, -m.M22);

		public static Matrix2x2 operator *(Matrix2x2 lhs, Matrix2x2 rhs) => new(
			lhs.R1.Dot(rhs.C1), lhs.R1.Dot(rhs.C2),
			lhs.R2.Dot(rhs.C1), lhs.R2.Dot(rhs.C2)
		);

		public static Matrix2x2 operator *(Matrix2x2 m, float s) => new(
			m.M11 * s, m.M12 * s,
			m.M21 * s, m.M22 * s
		);

		public static Vector2 operator *(Matrix2x2 m, Vector2 v) => new(m.R1.Dot(v), m.R2.Dot(v));

	}
}
