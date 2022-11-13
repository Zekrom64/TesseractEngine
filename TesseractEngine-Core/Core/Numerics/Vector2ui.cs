using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A two-component vector of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2ui : IVector2Int<Vector2ui, uint>, IEquatable<IReadOnlyTuple2<uint>> {

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

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0;
		}

		public uint LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		public Span<uint> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 2);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui(uint s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui(uint x, uint y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui(IReadOnlyTuple2<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		/// <summary>
		/// Casts a tuple of another type to a vector type.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="tuple">Tuple to cast</param>
		/// <returns>Vector from tuple value</returns>
		public static Vector2ui Cast<T>(IReadOnlyTuple2<T> tuple) where T : unmanaged =>
			new(Convert.ToUInt32(tuple.X), Convert.ToUInt32(tuple.Y));

		public bool Equals(IReadOnlyTuple2<uint>? other) => other != null && X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X + Y * 5);

		public override string ToString() => $"({X},{Y})";

		public static Vector2ui Create(uint x, uint y) => new(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector2ui min, ref Vector2ui max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Min(Vector2ui v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Max(Vector2ui v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Sum() => X + Y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Dot(Vector2ui v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint DistanceSquared(Vector2ui v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2ui other) => X == other.X && Y == other.Y;

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index uinto vector</param>
		/// <returns>Value at index in vector</returns>
		public uint this[int index] {
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator -(Vector2ui value) => new((uint)-value.X, (uint)-value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator +(Vector2ui value) => value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2ui operator >>>(Vector2ui value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2ui(Vector2 v) => new((uint)v.X, (uint)v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2ui(Vector2d v) => new((uint)v.X, (uint)v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2ui(Vector2i v) => new((uint)v.X, (uint)v.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2ui(Vector3ui v) => new(v.X, v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2ui(Vector4ui v) => new(v.X, v.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(Vector2ui v) => new(v.X, v.Y);
	}

}
