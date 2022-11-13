using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A two-component vector of 64-bit floating point values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2d : IVector2Float<Vector2d, double>, IEquatable<IReadOnlyTuple2<double>> {

		/// <summary>
		/// Vector X value.
		/// </summary>
		public double X;

		/// <summary>
		/// Vector Y value.
		/// </summary>
		public double Y;

		double ITuple<double, double>.X { get => X; set => X = value; }

		double ITuple<double, double>.Y { get => Y; set => X = value; }

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0;
		}

		public double Length {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Math.Sqrt(LengthSquared);
		}

		public double LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X * X + Y * Y;
		}

		public Span<double> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 2);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d(double s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d(double x, double y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d(IReadOnlyTuple2<double> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<double>? other) => other != null && X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<double> other && Equals(other);

		public override int GetHashCode() => (int)X + (int)Y * 5;

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index doubleo vector</param>
		/// <returns>Value at index in vector</returns>
		public double this[int index] {
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

		// Miscellaneous operations

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Abs() => new(Math.Abs(X), Math.Abs(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Max(Vector2d v) => new(Math.Max(X, v.X), Math.Max(Y, v.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Min(Vector2d v) => new(Math.Min(X, v.X), Math.Min(Y, v.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Round() => new(Math.Round(X), Math.Round(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Floor() => new(Math.Floor(X), Math.Floor(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Ceiling() => new(Math.Ceiling(X), Math.Ceiling(Y));

		public double Distance(Vector2d v2) => Math.Sqrt(DistanceSquared(v2));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Normalize() => this / Length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2d Sqrt() => new(Math.Sqrt(X), Math.Sqrt(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d Create(double x, double y) => new(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector2d min, ref Vector2d max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Sum() => X + Y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double Dot(Vector2d v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double DistanceSquared(Vector2d v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2d other) => X == other.X && Y == other.Y;

		// Equality operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2d left, Vector2d right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2d left, Vector2d right) => !(left == right);

		// Arithmetic operators

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d left, Vector2d right) => new(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d left, double right) => new(left.X + right, left.Y + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d left, Vector2d right) => new(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d left, double right) => new(left.X - right, left.Y - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(Vector2d left, Vector2d right) => new(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator *(Vector2d left, double right) => new(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator /(Vector2d left, Vector2d right) => new(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator /(Vector2d left, double right) => new(left.X / right, left.Y / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator %(Vector2d left, Vector2d right) => new(left.X % right.X, left.Y % right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator %(Vector2d left, double right) => new(left.X % right, left.Y % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator -(Vector2d value) => new(-value.X, -value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator +(Vector2d value) => new(Math.Abs(value.X), Math.Abs(value.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator ++(Vector2d value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator --(Vector2d value) => new(value.X - 1, value.Y - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2d(Vector2 v) => new(v.X, v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2d(Vector2i v) => new(v.X, v.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2d(Vector2ui v) => new(v.X, v.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector2(Vector2d v) => new((float)v.X, (float)v.Y);

	}

}
