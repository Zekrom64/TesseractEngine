using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Numerics {

    /// <summary>
    /// Generic 4-component vector implementation.
    /// </summary>
    /// <typeparam name="T">Vector element type</typeparam>
    public struct Vector4<T> :
		IVector4<Vector4<T>, T>
		where T : unmanaged, INumber<T>, IEquatable<T> {

		/// <summary>
		/// A 'zero' vector.
		/// </summary>
		public static readonly Vector4<T> Zero = new(T.Zero);

		/// <summary>
		/// A 'one' vector, constructed using <see cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity"/> from the element type.
		/// </summary>
		public static readonly Vector4<T> One = new(T.MultiplicativeIdentity);

		/// <summary>
		/// The X component.
		/// </summary>
		public T X;
		/// <summary>
		/// The Y component.
		/// </summary>
		public T Y;
		/// <summary>
		/// The Z component.
		/// </summary>
		public T Z;
		/// <summary>
		/// The W component.
		/// </summary>
		public T W;

		public T this[int key] {
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
				switch (key) {
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

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

		public Span<T> AsSpan => MemoryMarshal.CreateSpan(ref X, 4);

		T ITuple<T, T>.X { get => X; set => X = value; }
		T ITuple<T, T>.Y { get => Y; set => Y = value; }
		T ITuple<T, T, T>.Z { get => Z; set => Z = value; }
		T ITuple<T, T, T, T>.W { get => W; set => W = value; }

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == T.Zero && Y == T.Zero && Z == T.Zero && W == T.Zero;
		}

		public T LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(T s) {
			X = Y = Z = W = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(T x, T y, T z, T w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(IReadOnlyTuple4<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(IReadOnlyTuple3<T> tuple, T w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = w;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4(IReadOnlyTuple2<T> tuple, T z, T w) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
			W = w;
		}

		public static Vector4<T> Create(T x, T y, T z, T w) => new(x, y, z, w);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector4<T> min, ref Vector4<T> max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
			ExMath.MinMax(ref min.W, ref max.W);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector4<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4<T> Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4<T> Abs() => new(T.Abs(X), T.Abs(Y), T.Abs(Z), T.Abs(W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4<T> Min(Vector4<T> v2) => new(T.Min(X, v2.X), T.Min(Y, v2.Y), T.Min(Z, v2.Z), T.Min(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4<T> Max(Vector4<T> v2) => new(T.Max(X, v2.X), T.Max(Y, v2.Y), T.Max(Z, v2.Z), T.Max(W, v2.W));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Sum() => X + Y + Z + W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Dot(Vector4<T> v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T DistanceSquared(Vector4<T> v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator +(Vector4<T> value) => value.Abs();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator +(Vector4<T> left, T right) => new(left.X + right, left.Y + right, left.Z + right, left.W + right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator +(Vector4<T> left, Vector4<T> right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator -(Vector4<T> value) => new(-value.X, -value.Y, -value.Z, -value.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator -(Vector4<T> left, T right) => new(left.X - right, left.Y - right, left.Z - right, left.W - right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator -(Vector4<T> left, Vector4<T> right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator ++(Vector4<T> value) => value + One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator --(Vector4<T> value) => value - One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator *(Vector4<T> left, T right) => new(left.X * right, left.Y * right, left.Z * right, left.W * right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator *(Vector4<T> left, Vector4<T> right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator /(Vector4<T> left, T right) => new(left.X / right, left.Y / right, left.Z / right, left.W / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4<T> operator /(Vector4<T> left, Vector4<T> right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector4<T> left, Vector4<T> right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector4<T> left, Vector4<T> right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z || left.W != right.W;

		public override bool Equals(object? obj) => obj is Vector4<T> v && this == v;

		public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() * 5 + Z.GetHashCode() * 7 + W.GetHashCode() * 11;

		public override string ToString() => $"({X},{Y},{Z},{W})";

	}

}
