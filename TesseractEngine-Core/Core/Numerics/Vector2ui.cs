using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A two-component vector of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2ui : ITuple2<uint>, IEquatable<IReadOnlyTuple2<uint>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public uint X;
		/// <summary>
		/// Vector Y value.
		/// </summary>
		public uint Y;

		uint ITuple<uint, uint>.X { get => X; set => X = value; }

		uint ITuple<uint, uint>.Y { get => Y; set => X = value; }

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2ui(uint s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2ui(uint x, uint y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2ui(IReadOnlyTuple2<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<uint>? other) => other != null && X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X ^ (Y << 16));

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index uinto vector</param>
		/// <returns>Value at index in vector</returns>
		public uint this[int index] {
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
		public static bool operator ==(Vector2ui left, Vector2ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2ui left, Vector2ui right) => !(left == right);

		// Arithmetic operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui left, Vector2ui right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui left, uint right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui left, Vector2ui right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui left, uint right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator *(Vector2ui left, Vector2ui right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator *(Vector2ui left, uint right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator /(Vector2ui left, Vector2ui right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator /(Vector2ui left, uint right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator %(Vector2ui left, Vector2ui right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator %(Vector2ui left, uint right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui value) => new((uint)-value.X, (uint)-value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui value) => new(+value.X, +value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ++(Vector2ui value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator --(Vector2ui value) => new(value.X - 1, value.Y - 1);

		// Bitwise operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator >>(Vector2ui left, int right) => new(left.X >> right, left.Y >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator <<(Vector2ui left, int right) => new(left.X << right, left.Y << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ~(Vector2ui value) => new(~value.X, ~value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator &(Vector2ui left, Vector2ui right) => new(left.X & right.X, left.Y & right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator &(Vector2ui left, uint right) => new(left.X & right, left.Y & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator |(Vector2ui left, Vector2ui right) => new(left.X | right.X, left.Y | right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator |(Vector2ui left, uint right) => new(left.X | right, left.Y | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ^(Vector2ui left, Vector2ui right) => new(left.X ^ right.X, left.Y ^ right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator ^(Vector2ui left, uint right) => new(left.X ^ right, left.Y ^ right);

		public static implicit operator Vector<uint>(Vector2ui v) => new(stackalloc[] { v.X, v.Y });
		public static implicit operator Vector2ui(Vector<uint> v) => new(v[0], v[1]);

		public static explicit operator Vector2i(Vector2ui v) => new((int)v.X, (int)v.Y);

	}

}
