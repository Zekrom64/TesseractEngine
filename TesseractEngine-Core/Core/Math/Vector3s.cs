using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	/// <summary>
	/// A three-component of 16-bit signed integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3s : ITuple3<short>, IEquatable<IReadOnlyTuple3<short>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public short X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public short Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public short Z;

		short ITuple<short, short>.X { get => X; set => X = value; }

		short ITuple<short, short>.Y { get => Y; set => X = value; }

		short ITuple<short, short, short>.Z { get => Z; set => Z = value; }

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3s(short s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3s(int s) {
			X = Y = Z = (short)s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3s(short x, short y, short z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3s(int x, int y, int z) {
			X = (short)x;
			Y = (short)y;
			Z = (short)z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3s(IReadOnlyTuple3<short> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<short>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<short> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 10) ^ (Z << 20);

		public override string ToString() => $"({X},{Y},{Z})";

		public short this[int index] {
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3s left, Vector3s right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3s left, Vector3s right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator +(Vector3s left, Vector3s right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator +(Vector3s left, int right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator -(Vector3s left, Vector3s right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator -(Vector3s left, int right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator *(Vector3s left, Vector3s right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator *(Vector3s left, int right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator /(Vector3s left, Vector3s right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator /(Vector3s left, int right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator %(Vector3s left, Vector3s right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator %(Vector3s left, int right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator -(Vector3s value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator +(Vector3s value) => new(+value.X, +value.Y, +value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator ++(Vector3s value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator --(Vector3s value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator >>(Vector3s left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator <<(Vector3s left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator ~(Vector3s value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator &(Vector3s left, Vector3s right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator &(Vector3s left, int right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator |(Vector3s left, Vector3s right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator |(Vector3s left, int right) => new(left.X | (short)right, left.Y | (short)right, left.Z | (short)right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator ^(Vector3s left, Vector3s right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator ^(Vector3s left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

	}
}
