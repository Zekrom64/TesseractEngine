using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	/// <summary>
	/// A three-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4i : ITuple4<int>, IEquatable<IReadOnlyTuple4<int>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public int Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public int Z;
		/// <summary>
		/// Vector W component.
		/// </summary>
		public int W;

		int ITuple<int, int>.X { get => X; set => X = value; }

		int ITuple<int, int>.Y { get => Y; set => X = value; }

		int ITuple<int, int, int>.Z { get => Z; set => Z = value; }

		int ITuple<int, int, int, int>.W { get => W; set => W = value; }

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector4i(int s) {
			X = Y = Z = W = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		public Vector4i(int x, int y, int z, int w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector4i(IReadOnlyTuple4<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		public bool Equals(IReadOnlyTuple4<int>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple4<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 8) ^ (Z << 16) ^ (W << 24);

		public override string ToString() => $"({X},{Y},{Z},{W})";

		public int this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4i left, Vector4i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4i left, Vector4i right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i left, Vector4i right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i left, int right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i left, Vector4i right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i left, int right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator *(Vector4i left, Vector4i right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator *(Vector4i left, int right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator /(Vector4i left, Vector4i right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator /(Vector4i left, int right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator %(Vector4i left, Vector4i right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator %(Vector4i left, int right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator -(Vector4i value) => new(-value.X, -value.Y, -value.Z, -value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator +(Vector4i value) => new(+value.X, +value.Y, +value.Z, +value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ++(Vector4i value) => new(value.X + 1, value.Y + 1, value.Z + 1, value.W + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator --(Vector4i value) => new(value.X - 1, value.Y - 1, value.Z - 1, value.W - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator >>(Vector4i left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right, left.W >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator <<(Vector4i left, int right) => new(left.X << right, left.Y << right, left.Z << right, left.W << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ~(Vector4i value) => new(~value.X, ~value.Y, ~value.Z, ~value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator &(Vector4i left, Vector4i right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z, left.W & right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator &(Vector4i left, int right) => new(left.X & right, left.Y & right, left.Z & right, left.W & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator |(Vector4i left, Vector4i right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z, left.W | right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator |(Vector4i left, int right) => new(left.X | right, left.Y | right, left.Z | right, left.W | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ^(Vector4i left, Vector4i right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z, left.W ^ right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ^(Vector4i left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right, left.W ^ right);

		public static implicit operator Vector<int>(Vector4i v) => new(stackalloc[] { v.X, v.Y, v.Z, v.W });
		public static implicit operator Vector4i(Vector<int> v) => new(v[0], v[1], v[2], v[3]);

	}

}
