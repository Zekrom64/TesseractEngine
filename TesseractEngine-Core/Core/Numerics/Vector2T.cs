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
	/// Generic 2-component vector implementation.
	/// </summary>
	/// <typeparam name="T">Vector element type</typeparam>
	public struct Vector2<T> :
		IVector2<Vector2<T>, T>
		where T : unmanaged, INumber<T>, IEquatable<T> {

		/// <summary>
		/// A 'zero' vector.
		/// </summary>
		public static readonly Vector2<T> Zero = new(T.Zero);

		/// <summary>
		/// A 'one' vector, constructed using <see cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity"/> from the element type.
		/// </summary>
		public static readonly Vector2<T> One = new(T.MultiplicativeIdentity);

		/// <summary>
		/// The X component.
		/// </summary>
		public T X;
		/// <summary>
		/// The Y component.
		/// </summary>
		public T Y;

		public T this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
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
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

		public Span<T> AsSpan => MemoryMarshal.CreateSpan(ref X, 2);

		T ITuple<T, T>.X { get => X; set => X = value; }
		T ITuple<T, T>.Y { get => Y; set => Y = value; }

		public bool IsZeroLength {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => X == T.Zero && Y == T.Zero;
		}

		public T LengthSquared {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Dot(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2(T s) {
			X = Y = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2(T x, T y) {
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2(IReadOnlyTuple2<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public static Vector2<T> Create(T x, T y) => new(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref Vector2<T> min, ref Vector2<T> max) {
			ExMath.MinMax(ref min.X, ref max.X);
			ExMath.MinMax(ref min.Y, ref max.Y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector2<T> other) => X.Equals(other.X) && Y.Equals(other.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2<T> Swizzle(int x, int y) => new(this[x], this[y]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2<T> Abs() => new(T.Abs(X), T.Abs(Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2<T> Min(Vector2<T> v2) => new(T.Min(X, v2.X), T.Min(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2<T> Max(Vector2<T> v2) => new(T.Max(X, v2.X), T.Max(Y, v2.Y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Sum() => X + Y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Dot(Vector2<T> v2) => (this * v2).Sum();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T DistanceSquared(Vector2<T> v2) => (this - v2).LengthSquared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator +(Vector2<T> value) => value.Abs();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator +(Vector2<T> left, T right) => new(left.X + right, left.Y + right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator +(Vector2<T> left, Vector2<T> right) => new(left.X + right.X, left.Y + right.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator -(Vector2<T> value) => new(-value.X, -value.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator -(Vector2<T> left, T right) => new(left.X - right, left.Y - right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator -(Vector2<T> left, Vector2<T> right) => new(left.X - right.X, left.Y - right.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator ++(Vector2<T> value) => value + One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator --(Vector2<T> value) => value - One;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator *(Vector2<T> left, T right) => new(left.X * right, left.Y * right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator *(Vector2<T> left, Vector2<T> right) => new(left.X * right.X, left.Y * right.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator /(Vector2<T> left, T right) => new(left.X / right, left.Y / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2<T> operator /(Vector2<T> left, Vector2<T> right) => new(left.X / right.X, left.Y / right.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2<T> left, Vector2<T> right) => left.X == right.X && left.Y == right.Y;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2<T> left, Vector2<T> right) => left.X != right.X || left.Y != right.Y;

		public override bool Equals(object? obj) => obj is Vector2<T> v && this == v;

		public override int GetHashCode() => X.GetHashCode() + Y.GetHashCode() * 5;

		public override string ToString() => $"({X},{Y})";
	}

}
