using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {
	/// <summary>
	/// A three-component of 16-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3us : ITuple3<ushort>, IEquatable<IReadOnlyTuple3<ushort>> {

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

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3us(ushort s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3us(int s) {
			X = Y = Z = (ushort)s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
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
		public Vector3us(int x, int y, int z) {
			X = (ushort)x;
			Y = (ushort)y;
			Z = (ushort)z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3us(IReadOnlyTuple3<ushort> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<ushort> other) => X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object obj) => obj is IReadOnlyTuple3<ushort> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 10) ^ (Z << 20);

		public override string ToString() => $"({X},{Y},{Z})";

		public ushort this[int index] {
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
		public static Vector3us operator -(Vector3us value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3us operator +(Vector3us value) => new(+value.X, +value.Y, +value.Z);

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

	}

}
