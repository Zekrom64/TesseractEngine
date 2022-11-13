using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A three-component of 16-bit signed integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3s : IVector3Int<Vector3s, short>, IEquatable<IReadOnlyTuple3<short>> {

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

		public Span<short> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 3);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0;
		}

		public short LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s(short s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s(int s) {
			X = Y = Z = (short)s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s(int x, int y, int z) {
			X = (short)x;
			Y = (short)y;
			Z = (short)z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s(IReadOnlyTuple3<short> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<short>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<short> other && Equals(other);

		public override int GetHashCode() => X + Y * 5 + Z * 7;

		public override string ToString() => $"({X},{Y},{Z})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s Create(short x, short y, short z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3s min, ref Vector3s max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s Min(Vector3s v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3s Max(Vector3s v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short Sum() => (short)(X + Y + Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short Dot(Vector3s v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public short DistanceSquared(Vector3s v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3s other) => X == other.X && Y == other.Y && Z == other.Z;

		public short this[int index] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		public static Vector3s operator +(Vector3s value) => new(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator %(Vector3s left, short right) => new(left.X % right, left.Y % right, left.Z % right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator &(Vector3s left, short right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator |(Vector3s left, short right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator ^(Vector3s left, short right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator +(Vector3s left, short right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator -(Vector3s left, short right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator *(Vector3s left, short right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator /(Vector3s left, short right) => new(left.X / right, left.Y / right, left.Z / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3s operator >>>(Vector3s value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3s(Vector3 v) => new((short)v.X, (short)v.Y, (short)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3s(Vector3b v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3s(Vector3i v) => new((short)v.X, (short)v.Y, (short)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3s(Vector3ui v) => new((short)v.X, (short)v.Y, (short)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3s(Vector3us v) => new((short)v.X, (short)v.Y, (short)v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3s v) => new(v.X, v.Y, v.Z);
	}
}
