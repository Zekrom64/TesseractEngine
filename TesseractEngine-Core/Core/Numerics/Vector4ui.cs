using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {
	/// <summary>
	/// A four-component of 32-bit unsigned integers.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4ui : IVector4Int<Vector4ui, uint>, IEquatable<IReadOnlyTuple4<uint>> {

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
		/// <summary>
		/// Vector W component.
		/// </summary>
		public uint W;

		uint ITuple<uint, uint>.X { get => X; set => X = value; }

		uint ITuple<uint, uint>.Y { get => Y; set => X = value; }

		uint ITuple<uint, uint, uint>.Z { get => Z; set => Z = value; }

		uint ITuple<uint, uint, uint, uint>.W { get => W; set => W = value; }

		public Span<uint> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 4);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0 && W == 0;
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
		public Vector4ui(uint s) {
			X = Y = Z = W = s;
		}

		/// <summary>
		/// Creates a new vector from component values.
		/// </summary>
		/// <param name="x">X component value</param>
		/// <param name="y">Y component value</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui(uint x, uint y, uint z, uint w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui(IReadOnlyTuple4<uint> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="w">W component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui(IReadOnlyTuple3<uint> tuple, uint w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = w;
		}

		/// <summary>
		/// Creates a new vector from an existing tuple.
		/// </summary>
		/// <param name="tuple">Tuple to copy</param>
		/// <param name="z">Z component value</param>
		/// <param name="w">W component value</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui(IReadOnlyTuple2<uint> tuple, uint z, uint w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
			W = w;
		}

		public bool Equals(IReadOnlyTuple4<uint>? other) => other != null && X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public override bool Equals(object? obj) => obj is IReadOnlyTuple4<uint> other && Equals(other);

		public override int GetHashCode() => (int)(X + Y * 5 + Z * 7 + W * 11);

		public override string ToString() => $"({X},{Y},{Z},{W})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui Create(uint x, uint y, uint z, uint w) => new(x, y, z, w);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2ui Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3ui Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector4ui min, ref Vector4ui max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
			ExMath.MinMax(ref min.W, ref max.W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Min(Vector4ui v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z), Math.Min(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4ui Max(Vector4ui v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z), Math.Max(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Sum() => X + Y + Z + W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Dot(Vector4ui v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint DistanceSquared(Vector4ui v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4ui other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		public uint this[int index] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => index switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				switch (index) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4ui left, Vector4ui right) => left.Equals(right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4ui left, Vector4ui right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui left, Vector4ui right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui left, uint right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui left, Vector4ui right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui left, uint right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator *(Vector4ui left, Vector4ui right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator *(Vector4ui left, uint right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator /(Vector4ui left, Vector4ui right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator /(Vector4ui left, uint right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator %(Vector4ui left, Vector4ui right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator %(Vector4ui left, uint right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator -(Vector4ui value) => new((uint)-value.X, (uint)-value.Y, (uint)-value.Z, (uint)-value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator +(Vector4ui value) => value.Abs();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ++(Vector4ui value) => value + 1;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator --(Vector4ui value) => value - 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator >>(Vector4ui left, int right) => new(left.X >> right, left.Y >> right, left.Z >> right, left.W >> right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator <<(Vector4ui left, int right) => new(left.X << right, left.Y << right, left.Z << right, left.W << right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ~(Vector4ui value) => new(~value.X, ~value.Y, ~value.Z, ~value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator &(Vector4ui left, Vector4ui right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z, left.W & right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator &(Vector4ui left, uint right) => new(left.X & right, left.Y & right, left.Z & right, left.W & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator |(Vector4ui left, Vector4ui right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z, left.W | right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator |(Vector4ui left, uint right) => new(left.X | right, left.Y | right, left.Z | right, left.W | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ^(Vector4ui left, Vector4ui right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z, left.W ^ right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator ^(Vector4ui left, uint right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right, left.W ^ right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4ui(Vector4b v) => new(v.X, v.Y, v.Z, v.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4ui(Vector4 v) => new((uint)v.X, (uint)v.Y, (uint)v.Z, (uint)v.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4ui(Vector4i v) => new((uint)v.X, (uint)v.Y, (uint)v.Z, (uint)v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector4ui v) => new(v.X, v.Y, v.Z, v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4ui operator >>>(Vector4ui value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount, value.W >>> shiftAmount);
	}

}
