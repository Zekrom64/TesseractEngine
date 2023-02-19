using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A four-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4i : IVector4Int<Vector4i, int>, IEquatable<IReadOnlyTuple4<int>> {

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

		public Span<int> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 4);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0 && W == 0;
		}

		public int LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i(IReadOnlyTuple4<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="w">W component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i(IReadOnlyTuple3<int> tuple, int w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i(IReadOnlyTuple2<int> tuple, int z, int w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
			W = w;
		}

		public bool Equals(IReadOnlyTuple4<int>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple4<int> other && Equals(other);

		public override int GetHashCode() => X + Y * 5 + Z * 7 + W * 11;

		public override string ToString() => $"({X},{Y},{Z},{W})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i Create(int x, int y, int z, int w) => new(x, y, z, w);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector4i min, ref Vector4i max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
			ExMath.MinMax(ref min.W, ref max.W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z), Math.Abs(W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Min(Vector4i v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z), Math.Min(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Max(Vector4i v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z), Math.Max(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Sum() => X + Y + Z + W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dot(Vector4i v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int DistanceSquared(Vector4i v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4i other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public int this[int index] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		public static Vector4i operator +(Vector4i value) => new(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z), Math.Abs(value.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator ++(Vector4i value) => value + 1;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator --(Vector4i value) => value - 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator >>(Vector4i left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right, left.W >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator >>(Vector4i left, Vector4i right) => new(left.X >> right.X, left.Y >> right.Y, left.Z >> right.Z, left.W >> right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator <<(Vector4i left, int right) => new(left.X << right, left.Y << right, left.Z << right, left.W << right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator <<(Vector4i left, Vector4i right) => new(left.X << right.X, left.Y << right.Y, left.Z << right.Z, left.W << right.W);

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4i(Vector4b v) => new(v.X, v.Y, v.Z, v.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4i(Vector4 v) => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4i(Vector4ui v) => new((int)v.X, (int)v.Y, (int)v.Z, (int)v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector4i v) => new(v.X, v.Y, v.Z, v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4i operator >>>(Vector4i value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount, value.W >>> shiftAmount);
	}

}
