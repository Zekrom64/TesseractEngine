using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// Generic 3-component vector implementation.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	public struct Vector3<T> :
		IVector3<Vector3<T>, T>
		where T : unmanaged, INumber<T>, IEquatable<T> {

		/// <summary>
		/// A 'zero' vector.
		/// </summary>
		public static readonly Vector3<T> Zero = new(T.Zero);

		/// <summary>
		/// A 'one' vector, constructed using <see cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity"/> from the element type.
		/// </summary>
		public static readonly Vector3<T> One = new(T.MultiplicativeIdentity);

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

		public T this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
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
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

		public Span<T> AsSpan => MemoryMarshal.CreateSpan(ref X, 3);

		T ITuple<T, T>.X { get => X; set => X = value; }
		T ITuple<T, T>.Y { get => Y; set => Y = value; }
		T ITuple<T, T, T>.Z { get => Z; set => Z = value; }

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == T.Zero && Y == T.Zero && Z == T.Zero;
		}

		public T LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3(T s) {
			X = Y = Z = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3(T x, T y, T z) {
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3(IReadOnlyTuple3<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3(IReadOnlyTuple2<T> tuple, T z) {
			X = tuple.X;
			Y = tuple.Y;
			Z = z;
		}

		public static Vector3<T> Create(T x, T y, T z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector3<T> min, ref Vector3<T> max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
			ExMath.MinMax(ref min.Z, ref max.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3<T> other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3<T> Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3<T> Abs() => new(T.Abs(X), T.Abs(Y), T.Abs(Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3<T> Min(Vector3<T> v2) => new(T.Min(X, v2.X), T.Min(Y, v2.Y), T.Min(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3<T> Max(Vector3<T> v2) => new(T.Max(X, v2.X), T.Max(Y, v2.Y), T.Max(Z, v2.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Sum() => X + Y + Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Dot(Vector3<T> v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T DistanceSquared(Vector3<T> v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator +(Vector3<T> value) => value.Abs();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator +(Vector3<T> left, T right) => new(left.X + right, left.Y + right, left.Z + right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator +(Vector3<T> left, Vector3<T> right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator -(Vector3<T> value) => new(-value.X, -value.Y, -value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator -(Vector3<T> left, T right) => new(left.X - right, left.Y - right, left.Z - right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator -(Vector3<T> left, Vector3<T> right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator ++(Vector3<T> value) => value + One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator --(Vector3<T> value) => value - One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator *(Vector3<T> left, T right) => new(left.X * right, left.Y * right, left.Z * right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator *(Vector3<T> left, Vector3<T> right) => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator /(Vector3<T> left, T right) => new(left.X / right, left.Y / right, left.Z / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3<T> operator /(Vector3<T> left, Vector3<T> right) => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3<T> left, Vector3<T> right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3<T> left, Vector3<T> right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z;

		public override bool Equals(object? obj) => obj is Vector3<T> v && this == v;

		public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() * 5 + Z.GetHashCode() * 7;

		public override string ToString() => $"({X},{Y},{Z})";

	}

}
