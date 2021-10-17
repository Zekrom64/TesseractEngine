using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	/// <summary>
	/// A three-component of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3ui : ITuple3<uint>, IEquatable<IReadOnlyTuple3<uint>> {

		/// <summary>
		/// Vector X component.
		/// </summary>
		public uint X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public uint Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public uint Z;

		uint ITuple<uint, uint>.X { get => X; set => X = value; }

		uint ITuple<uint, uint>.Y { get => Y; set => X = value; }

		uint ITuple<uint, uint, uint>.Z { get => Z; set => Z = value; }

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3ui(uint s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3ui(uint x, uint y, uint z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3ui(IReadOnlyTuple3<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<uint> other) => X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object obj) => obj is IReadOnlyTuple3<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X ^ (Y << 10) ^ (Z << 20));

		public override string ToString() => $"({X},{Y},{Z})";

		public uint this[int index] {
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
		public static bool operator ==(Vector3ui left, Vector3ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3ui left, Vector3ui right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui left, Vector3ui right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui left, uint right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui left, Vector3ui right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui left, uint right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator *(Vector3ui left, Vector3ui right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator *(Vector3ui left, uint right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator /(Vector3ui left, Vector3ui right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator /(Vector3ui left, uint right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator %(Vector3ui left, Vector3ui right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator %(Vector3ui left, uint right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui value) => new((uint)-value.X, (uint)-value.Y, (uint)-value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui value) => new(+value.X, +value.Y, +value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ++(Vector3ui value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator --(Vector3ui value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator >>(Vector3ui left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator <<(Vector3ui left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ~(Vector3ui value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator &(Vector3ui left, Vector3ui right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator &(Vector3ui left, uint right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator |(Vector3ui left, Vector3ui right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator |(Vector3ui left, uint right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ^(Vector3ui left, Vector3ui right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator ^(Vector3ui left, uint right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		public static implicit operator Vector<uint>(Vector3ui v) => new(stackalloc[] { v.X, v.Y, v.Z });
		public static implicit operator Vector3ui(Vector<uint> v) => new(v[0], v[1], v[2]);

		public static explicit operator Vector3i(Vector3ui v) => new((int)v.X, (int)v.Y, (int)v.Z);

	}

}
