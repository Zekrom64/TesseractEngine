using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4b : IVector4Int<Vector4b, byte>, IEquatable<IReadOnlyTuple4<byte>> {

		public byte X;

		public byte Y;

		public byte Z;

		public byte W;

		public byte this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
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
					case 3:
						W = value;
						break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		public Span<byte> AsSpan {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MemoryMarshal.CreateSpan(ref X, 4);
		}

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == 0 && Y == 0 && Z == 0 && W == 0;
		}

		public byte LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		byte ITuple<byte, byte>.X { get => X; set => X = value; }
		byte ITuple<byte, byte>.Y { get => Y; set => Y = value; }
		byte ITuple<byte, byte, byte>.Z { get => Z; set => Z = value; }
		byte ITuple<byte, byte, byte, byte>.W { get => W; set => W = value; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b(byte s) {
			X = Y = Z = W = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b(byte x, byte y, byte z, byte w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b(int s) {
			X = Y = Z = W = (byte)s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b(int x, int y, int z, int w) {
			X = (byte)x;
			Y = (byte)y;
			Z = (byte)z;
			W = (byte)w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b(IReadOnlyTuple3<byte> tuple, byte w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = w;
		}

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector4b v && Equals(v);

		public override int GetHashCode() => X + Y * 5 + Z * 7 + W * 11;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b Create(byte x, byte y, byte z, byte w) => new(x, y, z, w);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector4b min, ref Vector4b max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
			ExMath.MinMax(ref min.W, ref max.W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Abs() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte DistanceSquared(Vector4b v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Dot(Vector4b v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(IReadOnlyTuple4<byte>? other) => other != null && other.X == X && other.Y == Y && other.Z == Z && other.W == W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4b other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Max(Vector4b v2) => new(Math.Max(X, v2.X), Math.Max(Y, v2.Y), Math.Max(Z, v2.Z), Math.Max(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Min(Vector4b v2) => new(Math.Min(X, v2.X), Math.Min(Y, v2.Y), Math.Min(Z, v2.Z), Math.Min(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte Sum() => (byte)(X + Y + Z + W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator +(Vector4b value) => value;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator +(Vector4b left, byte right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator +(Vector4b left, Vector4b right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator -(Vector4b value) => new(-value.X, -value.Y, -value.Z, -value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator -(Vector4b left, byte right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator -(Vector4b left, Vector4b right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator ~(Vector4b value) => new(~value.X, ~value.Y, ~value.Z, ~value.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator ++(Vector4b value) => value + 1;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator --(Vector4b value) => value - 1;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator *(Vector4b left, byte right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator *(Vector4b left, Vector4b right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator /(Vector4b left, byte right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator /(Vector4b left, Vector4b right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator %(Vector4b left, byte right) => new(left.X % right, left.Y % right, left.Z % right, left.W % right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator %(Vector4b left, Vector4b right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z, left.W % right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator &(Vector4b left, byte right) => new(left.X & right, left.Y & right, left.Z & right, left.W & right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator &(Vector4b left, Vector4b right) => new(left.X & right.X, left.Y & right.Y, left.Z & right.Z, left.W & right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator |(Vector4b left, byte right) => new(left.X | right, left.Y | right, left.Z | right, left.W | right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator |(Vector4b left, Vector4b right) => new(left.X | right.X, left.Y | right.Y, left.Z | right.Z, left.W | right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator ^(Vector4b left, byte right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right, left.W ^ right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator ^(Vector4b left, Vector4b right) => new(left.X ^ right.X, left.Y ^ right.Y, left.Z ^ right.Z, left.W ^ right.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator <<(Vector4b value, int shiftAmount) => new(value.X << shiftAmount, value.Y << shiftAmount, value.Z << shiftAmount, value.W << shiftAmount);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator >>(Vector4b value, int shiftAmount) => new(value.X >> shiftAmount, value.Y >> shiftAmount, value.Z >> shiftAmount, value.W >> shiftAmount);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4b left, Vector4b right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4b left, Vector4b right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z || left.W != right.W;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4b operator >>>(Vector4b value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount, value.W >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4b(Vector4ui v) => new((byte)v.X, (byte)v.Y, (byte)v.Z, (byte)v.W);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector4b(Vector4i v) => new((byte)v.X, (byte)v.Y, (byte)v.Z, (byte)v.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector4(Vector4b v) => new(v.X, v.Y, v.Z, v.W);

	}

}
