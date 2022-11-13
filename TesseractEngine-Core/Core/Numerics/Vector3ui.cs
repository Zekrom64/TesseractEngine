using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A three-component of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3ui : IVector3Int<Vector3ui, uint>, IEquatable<IReadOnlyTuple3<uint>> {

		public static readonly Vector3ui Zero = new(0, 0, 0);

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

		public Span<uint> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 3);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0;
		}

		public uint LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		/// <summary>
		/// Creates a new vector from a scalar value.
		/// </summary>
		/// <param name="s">Scalar value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui(uint s) {
			X = Y = Z = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui(uint x, uint y, uint z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui(IReadOnlyTuple3<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="z">Z component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui(IReadOnlyTuple2<uint> tuple, uint z) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
		}

		public bool Equals(IReadOnlyTuple3<uint>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple3<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X + Y * 5 + Z * 7);

		public override string ToString() => $"({X},{Y},{Z})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui Create(uint x, uint y, uint z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3ui min, ref Vector3ui max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Min(Vector3ui v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Max(Vector3ui v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Sum() => X + Y + Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Dot(Vector3ui v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint DistanceSquared(Vector3ui v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3ui other) => X == other.X && Y == other.Y;

		public uint this[int index] {
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator -(Vector3ui value) => value - 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator +(Vector3ui value) => value + 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3ui operator >>>(Vector3ui value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3ui(Vector3 v) => new((uint)v.X, (uint)v.Y, (uint)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3ui(Vector3b v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3ui(Vector3i v) => new((uint)v.X, (uint)v.Y, (uint)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3ui(Vector3s v) => new((uint)v.X, (uint)v.Y, (uint)v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3ui(Vector3us v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(Vector3ui v) => new(v.X, v.Y, v.Z);
	}

}
