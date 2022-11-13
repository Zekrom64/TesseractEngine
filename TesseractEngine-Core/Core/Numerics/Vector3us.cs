using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A three-component of 16-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3us : IVector3Int<Vector3us, ushort>, IEquatable<IReadOnlyTuple3<ushort>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public ushort X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public ushort Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public ushort Z;

		ushort ITuple<ushort, ushort>.X { get => X; set => X = value; }

		ushort ITuple<ushort, ushort>.Y { get => Y; set => X = value; }

		ushort ITuple<ushort, ushort, ushort>.Z { get => Z; set => Z = value; }

		public Span<ushort> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 3);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0;
		}

		public ushort LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us(ushort s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us(int s) {
			X = Y = Z = (ushort)s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us(ushort x, ushort y, ushort z) {
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
		public Vector3us(int x, int y, int z) {
			X = (ushort)x;
			Y = (ushort)y;
			Z = (ushort)z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us(IReadOnlyTuple3<ushort> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<ushort>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<ushort> other && Equals(other);

		public override int GetHashCode() => X + Y * 5 + Z * 7;

		public override string ToString() => $"({X},{Y},{Z})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us Create(ushort x, ushort y, ushort z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3us min, ref Vector3us max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us Min(Vector3us v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3us Max(Vector3us v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort Sum() => (ushort)(X + Y + Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort Dot(Vector3us v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ushort DistanceSquared(Vector3us v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3us other) => X == other.X && Y == other.Y && Z == other.Z;

		public ushort this[int index] {
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
		public static bool operator ==(Vector3us left, Vector3us right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3us left, Vector3us right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us left, Vector3us right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us left, int right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us left, Vector3us right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us left, int right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator *(Vector3us left, Vector3us right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator *(Vector3us left, int right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator /(Vector3us left, Vector3us right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator /(Vector3us left, int right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator %(Vector3us left, Vector3us right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator %(Vector3us left, int right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ++(Vector3us value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator --(Vector3us value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator >>(Vector3us left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator <<(Vector3us left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ~(Vector3us value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator &(Vector3us left, Vector3us right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator &(Vector3us left, int right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator |(Vector3us left, Vector3us right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator |(Vector3us left, int right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ^(Vector3us left, Vector3us right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ^(Vector3us left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator %(Vector3us left, ushort right) => new(left.X % right, left.Y % right, left.Z % right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator &(Vector3us left, ushort right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator |(Vector3us left, ushort right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator ^(Vector3us left, ushort right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us left, ushort right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us left, ushort right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator *(Vector3us left, ushort right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator /(Vector3us left, ushort right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator -(Vector3us value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us value) => value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator >>>(Vector3us value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3us(Vector3 v) => new((ushort)v.X, (ushort)v.Y, (ushort)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3us(Vector3b v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3us(Vector3i v) => new((ushort)v.X, (ushort)v.Y, (ushort)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3us(Vector3s v) => new((ushort)v.X, (ushort)v.Y, (ushort)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3us(Vector3ui v) => new((ushort)v.X, (ushort)v.Y, (ushort)v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3us v) => new(v.X, v.Y, v.Z);
	}

}
