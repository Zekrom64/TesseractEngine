using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A two-component vector of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2i : ITuple2<int>, IEquatable<IReadOnlyTuple2<int>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y value.
		/// </summary>
		public int Y;

		int ITuple<int, int>.X { get => X; set => X = value; }

		int ITuple<int, int>.Y { get => Y; set => X = value; }

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2i(int s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2i(int x, int y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2i(IReadOnlyTuple2<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		/// <summary>
		/// Casts a tuple of another type to a vector type.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="tuple">Tuple to cast</param>
		/// <returns>Vector from tuple value</returns>
		public static Vector2i Cast<T>(IReadOnlyTuple2<T> tuple) where T : unmanaged =>
			new(Convert.ToInt32(tuple.X), Convert.ToInt32(tuple.Y));

		public bool Equals(IReadOnlyTuple2<int>? other) => other != null && X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 16);

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index into vector</param>
		/// <returns>Value at index in vector</returns>
		public int this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2i left, Vector2i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2i left, Vector2i right) => !(left == right);

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
		public static Vector2i operator <<(Vector2i left, int right) => new(left.X << right, left.Y << right);

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2 v) => new((int)v.X, (int)v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2d v) => new((int)v.X, (int)v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2i(Vector2ui v) => new((int)v.X, (int)v.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2i v) => new(v.X, v.Y);

	}

}
