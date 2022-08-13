using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A three-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3i : ITuple3<int>, IEquatable<IReadOnlyTuple3<int>> {

		public static readonly Vector3i Zero = new(0, 0, 0);

		/// <summary>
		/// Vector X component.
		/// </summary>
		public int X;
		/// <summary>
		/// Vector Y component.
		/// </summary>
		public int Y;
		/// <summary>
		/// Vector Z component.
		/// </summary>
		public int Z;

		int ITuple<int, int>.X { get => X; set => X = value; }

		int ITuple<int, int>.Y { get => Y; set => X = value; }

		int ITuple<int, int, int>.Z { get => Z; set => Z = value; }

		public Span<int> AsSpan => MemoryMarshal.CreateSpan(ref X, 3);

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		public Vector3i(int s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		public Vector3i(int x, int y, int z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		public Vector3i(IReadOnlyTuple3<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		/// <summary>
		/// Casts a tuple of another type to a vector type.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="tuple">Tuple to cast</param>
		/// <returns>Vector from tuple value</returns>
		public static Vector3i Cast<T>(IReadOnlyTuple3<T> tuple) where T : unmanaged =>
			new(Convert.ToInt32(tuple.X), Convert.ToInt32(tuple.Y), Convert.ToInt32(tuple.Z));

		public bool Equals(IReadOnlyTuple3<int>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<int> other && Equals(other);

		public override int GetHashCode() => X ^ (Y << 10) ^ (Z << 20);

		public override string ToString() => $"({X},{Y},{Z})";

		public int this[int index] {
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
		public static bool operator ==(Vector3i left, Vector3i right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3i left, Vector3i right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i left, Vector3i right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i left, int right) => new(left.X + right, left.Y + right, left.Z + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i left, Vector3i right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i left, int right) => new(left.X - right, left.Y - right, left.Z - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator *(Vector3i left, Vector3i right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator *(Vector3i left, int right) => new(left.X * right, left.Y * right, left.Z * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator /(Vector3i left, Vector3i right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator /(Vector3i left, int right) => new(left.X / right, left.Y / right, left.Z / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator %(Vector3i left, Vector3i right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator %(Vector3i left, int right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator -(Vector3i value) => new(-value.X, -value.Y, -value.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator +(Vector3i value) => new(Math.Abs(value.X), Math.Abs(value.Y), Math.Abs(value.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ++(Vector3i value) => new(value.X + 1, value.Y + 1, value.Z + 1);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator --(Vector3i value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator >>(Vector3i left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator <<(Vector3i left, int right) => new(left.X << right, left.Y << right, left.Z << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ~(Vector3i value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator &(Vector3i left, Vector3i right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator &(Vector3i left, int right) => new(left.X & right, left.Y & right, left.Z & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator |(Vector3i left, Vector3i right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator |(Vector3i left, int right) => new(left.X | right, left.Y | right, left.Z | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ^(Vector3i left, Vector3i right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator ^(Vector3i left, int right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3i(Vector3 v) => new((int)v.X, (int)v.Y, (int)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3i(Vector3ui v) => new((int)v.X, (int)v.Y, (int)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3i(Vector3s v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3i(Vector3us v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3i v) => new(v.X, v.Y, v.Z);

	}

}
