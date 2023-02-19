using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Numerics {

    [StructLayout(LayoutKind.Sequential)]
	public struct Vector3b : IVector3Int<Vector3b, byte>, IEquatable<IReadOnlyTuple3<byte>> {

		public byte X;

		public byte Y;

		public byte Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(byte s) {
			X = Y = Z = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(byte x, byte y, byte z) {
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(int s) {
			X = Y = Z = (byte)s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(int x, int y, int z) {
			X = (byte)x;
			Y = (byte)y;
			Z = (byte)z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(IReadOnlyTuple3<byte> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public byte this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				switch(key) {
					case 0:
						X = value;
						break;
					case 1:
						Y = value;
						break;
					case 2:
						Z = value;
						break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		byte ITuple<byte, byte>.X { get => X; set => X = value; }
		byte ITuple<byte, byte>.Y { get => Y; set => Y = value; }
		byte ITuple<byte, byte, byte>.Z { get => Z; set => Z = value; }

		public Span<byte> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 3);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0;
		}

		public byte LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector3b v && Equals(v);

		public override int GetHashCode() => X + Y * 5 + Z * 7;

		public override string ToString() => $"({X},{Y})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(IReadOnlyTuple3<byte>? other) => other != null && other.X == X && other.Y == Y && other.Z == Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		public static Vector3b Create(byte x, byte y, byte z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3b min, ref Vector3b max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3b other) => X == other.X && Y == other.Y && Z == other.Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Min(Vector3b v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Max(Vector3b v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Sum() => (byte)(X + Y + Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Dot(Vector3b v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte DistanceSquared(Vector3b v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator %(Vector3b left, byte right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator &(Vector3b left, byte right) => new(left.X & right, left.Y & right, left.Z & right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator |(Vector3b left, byte right) => new(left.X | right, left.Y | right, left.Z | right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator ^(Vector3b left, byte right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator +(Vector3b left, byte right) => new(left.X + right, left.Y + right, left.Z + right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator -(Vector3b left, byte right) => new(left.X - right, left.Y - right, left.Z - right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator *(Vector3b left, byte right) => new(left.X * right, left.Y * right, left.Z * right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator /(Vector3b left, byte right) => new(left.X / right, left.Y / right, left.Z / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3b left, Vector3b right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3b left, Vector3b right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator +(Vector3b left, Vector3b right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator -(Vector3b left, Vector3b right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator *(Vector3b left, Vector3b right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator /(Vector3b left, Vector3b right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator -(Vector3b value) => new(-value.X, -value.Y, -value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator +(Vector3b value) => value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator ++(Vector3b value) => value + 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator --(Vector3b value) => value - 1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator %(Vector3b left, Vector3b right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator <<(Vector3b value, int shiftAmount) => new(value.X << shiftAmount, value.Y << shiftAmount, value.Z << shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator >>(Vector3b value, int shiftAmount) => new(value.X >> shiftAmount, value.Y >> shiftAmount, value.Z >> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator >>>(Vector3b value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator &(Vector3b left, Vector3b right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator |(Vector3b left, Vector3b right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator ^(Vector3b left, Vector3b right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3b operator ~(Vector3b value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3b(Vector3s v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3b(Vector3us v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3b(Vector3i v) => new(v.X, v.Y, v.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3b(Vector3ui v) => new((byte)v.X, (byte)v.Y, (byte)v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3b(Vector4b v) => new(v.X, v.Y, v.Z);

	}

}
