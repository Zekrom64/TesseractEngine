using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A two-component vector of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2i : IVector2<Vector2i, int>, IEquatable<IReadOnlyTuple2<int>> {

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i Create(int x, int y) => new(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector2i min, ref Vector2i max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
		}

		/// <summary>
		/// Vector X value.
		/// </summary>
		public int X;

		/// <summary>
		/// Vector Y value.
		/// </summary>
		public int Y;

		int ITuple<int, int>.X { get => X; set => X = value; }

		int ITuple<int, int>.Y { get => Y; set => Y = value; }

		public Span<int> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 2);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0;
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
		public Vector2i(int s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i(int x, int y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i(IReadOnlyTuple2<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2i other) => X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<int> other && Equals(other);

		public override int GetHashCode() => X + Y * 5;

		public override string ToString() => $"({X},{Y})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Abs() => new(Math.Abs(X), Math.Abs(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Min(Vector2i v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Max(Vector2i v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Sum() => X + Y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dot(Vector2i v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int DistanceSquared(Vector2i v2) => (this - v2).LengthSquared;

		public bool Equals(IReadOnlyTuple2<int>? other) => other != null && X == other.X && Y == other.Y;

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index into vector</param>
		/// <returns>Value at index in vector</returns>
		public int this[int index] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => index switch {
				0 => X,
				1 => Y,
				_ => default
			};
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2i left, Vector2i right) => left.X == right.X && left.Y == right.Y;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2i left, Vector2i right) => left.X != right.X || left.Y != right.Y;

		// Arithmetic operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i left, Vector2i right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i left, int right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i left, Vector2i right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i left, int right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator *(Vector2i left, Vector2i right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator *(Vector2i left, int right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator /(Vector2i left, Vector2i right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator /(Vector2i left, int right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator %(Vector2i left, Vector2i right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator %(Vector2i left, int right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator -(Vector2i value) => new(-value.X, -value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator +(Vector2i value) => new(Math.Abs(value.X), Math.Abs(value.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ++(Vector2i value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator --(Vector2i value) => new(value.X - 1, value.Y - 1);

		// Bitwise operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator >>(Vector2i left, int right) => new(left.X >> right, left.Y >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator >>(Vector2i left, Vector2i right) => new(left.X >> right.X, left.Y >> right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator <<(Vector2i left, int right) => new(left.X << right, left.Y << right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator <<(Vector2i left, Vector2i right) => new(left.X << right.X, left.Y << right.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ~(Vector2i value) => new(~value.X, ~value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator &(Vector2i left, Vector2i right) => new(left.X & right.X, left.Y & right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator &(Vector2i left, int right) => new(left.X & right, left.Y & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator |(Vector2i left, Vector2i right) => new(left.X | right.X, left.Y | right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator |(Vector2i left, int right) => new(left.X | right, left.Y | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ^(Vector2i left, Vector2i right) => new(left.X ^ right.X, left.Y ^ right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i operator ^(Vector2i left, int right) => new(left.X ^ right, left.Y ^ right);

		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="int"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2 v) => new((int)v.X, (int)v.Y);
		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="int"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2d v) => new((int)v.X, (int)v.Y);
		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="int"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2ui v) => new((int)v.X, (int)v.Y);

		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="int"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2i(Vector3i v) => new(v.X, v.Y);
		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="int"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2i(Vector4i v) => new(v.X, v.Y);

		/// <summary>
		/// Componentwise casts to a two-component vector of <see cref="float"/>s.
		/// </summary>
		/// <param name="v">Vector to cast</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);

	}

}
