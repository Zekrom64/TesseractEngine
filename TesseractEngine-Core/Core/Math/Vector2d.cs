using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	/// <summary>
	/// A two-component vector of 64-bit floating point values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector2d : ITuple2<double>, IEquatable<IReadOnlyTuple2<double>> {

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

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector2d(double s) {
			X = Y = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		public Vector2d(double x, double y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector2d(IReadOnlyTuple2<double> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool Equals(IReadOnlyTuple2<double>? other) => other != null && X == other.X && Y == other.Y;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple2<double> other && Equals(other);

		public override int GetHashCode() => (int)(BitConverter.DoubleToInt64Bits(X) ^ BitConverter.DoubleToInt64Bits(Y));

		public override string ToString() => $"({X},{Y})";

		/// <summary>
		/// Indexes the values in this vector.
		/// </summary>
		/// <param name="index">Index doubleo vector</param>
		/// <returns>Value at index in vector</returns>
		public double this[int index] {
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
		public static Vector2d operator +(Vector2d value) => new(+value.X, +value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator ++(Vector2d value) => new(value.X + 1, value.Y + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2d operator --(Vector2d value) => new(value.X - 1, value.Y - 1);

	}

}
