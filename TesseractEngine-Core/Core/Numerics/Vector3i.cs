using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A three-component of 32-bit integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3i : IVector3Int<Vector3i, int>, IEquatable<IReadOnlyTuple3<int>> {

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

		public Span<int> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 3);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0;
		}

		public int LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i(int s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i(int x, int y, int z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i(IReadOnlyTuple2<int> tuple, int z) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i(IReadOnlyTuple3<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public bool Equals(IReadOnlyTuple3<int>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<int> other && Equals(other);

		public override int GetHashCode() => X + Y * 5 + Z * 7;

		public override string ToString() => $"({X},{Y},{Z})";

		public static Vector3i Create(int x, int y, int z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4i Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3i min, ref Vector3i max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Min(Vector3i v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3i Max(Vector3i v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Sum() => X + Y + Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dot(Vector3i v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int DistanceSquared(Vector3i v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3i other) => X == other.X && Y == other.Y && Z == other.Z;

		public int this[int index] {
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
		public static Vector3i operator >>(Vector3i left, Vector3i right) => new(left.X >> right.X, left.Y >> right.Y, left.Z >> right.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator <<(Vector3i left, int right) => new(left.X << right, left.Y << right, left.Z << right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator <<(Vector3i left, Vector3i right) => new(left.X << right.X, left.Y << right.Y, left.Z << right.Z);

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
		public static implicit operator Vector3i(Vector3b v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3i(Vector3ui v) => new((int)v.X, (int)v.Y, (int)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3i(Vector3s v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3i(Vector3us v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3i(Vector4i v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3i v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3i operator >>>(Vector3i value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);
	}

}
