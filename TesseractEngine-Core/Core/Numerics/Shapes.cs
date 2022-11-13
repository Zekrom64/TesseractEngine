using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// A read-only rectangle defined by a 2D position and size.
	/// </summary>
	/// <typeparam name="T">Coordinate numeric type</typeparam>
	public interface IReadOnlyRect<T> where T : unmanaged, INumber<T> {

		/// <summary>
		/// The position of the rectangle. These must be the coordinates of the minimum point on the rectangle.
		/// Normally this will be the top-left of the rectangle.
		/// </summary>
		public IReadOnlyTuple2<T> Position { get; }

		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public IReadOnlyTuple2<T> Size { get; }

		/// <summary>
		/// The minimum point of the rectangle, equivalent to <see cref="Position"/>.
		/// </summary>
		public IReadOnlyTuple2<T> Minimum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Position;
		}

		/// <summary>
		/// The maximum point of the rectangle, equivalent to <see cref="Position"/> + <see cref="Size"/>.
		/// </summary>
		public IReadOnlyTuple2<T> Maximum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				var pos = Position;
				var size = Size;
				return new Tuple2<T>(pos.X + size.X, pos.Y + size.Y);
			}
		}

		/// <summary>
		/// If the rectangle is empty (ie. has a size of 0 in either axis).
		/// </summary>
		public bool Empty {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				var size = Size;
				return size.X == T.Zero && size.Y == T.Zero;
			}
		}

		/// <summary>
		/// The total area of the rectangle (width * height).
		/// </summary>
		public T Area {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				var size = Size;
				return size.X * size.Y;
			}
		}

	}

	/// <summary>
	/// Extended version of <see cref="IReadOnlyRect{T}"/> which defines additional operators.
	/// </summary>
	/// <typeparam name="TSelf">This rectangle type</typeparam>
	/// <typeparam name="T">Coordinate numeric type</typeparam>
	public interface IReadOnlyRect<TSelf,T> :
		IReadOnlyRect<T>, IEquatable<TSelf>
		where TSelf : IReadOnlyRect<TSelf, T>
		where T : unmanaged, INumber<T> {

		/// <summary>
		/// Creates a new instance of this rectangle type.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		/// <returns></returns>
		public static abstract TSelf Create(T x, T y, T w, T h);

		/// <summary>
		/// Tests if two rectangles intersect each other.
		/// </summary>
		/// <typeparam name="TRect">The type of the other rectangle</typeparam>
		/// <param name="r2">The rectangle to test for intersection</param>
		/// <returns>If the two rectangles intersect</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Intersects<TRect>(TRect r2)
			where TRect : IReadOnlyRect<TRect, T>
			=> !Intersect(r2).Empty;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TSelf Intersect<TRect>(TRect r2)
			where TRect : IReadOnlyRect<TRect, T> {
			var r1max = Maximum;
			var r1min = Minimum;
			var r2max = r2.Maximum;
			var r2min = r2.Minimum;
			T left = T.Max(r1min.X, r2min.X);
			T right = T.Min(r1max.X, r2max.X);
			T top = T.Max(r1min.Y, r2min.Y);
			T bottom = T.Min(r1max.Y, r2max.Y);
			if (left < right && top < bottom) return TSelf.Create(left, top, right - left, top - bottom);
			else {
				T zero = T.AdditiveIdentity;
				return TSelf.Create(zero, zero, zero, zero);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TRect"></typeparam>
		/// <param name="r2"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TSelf Union<TRect>(TRect r2)
			where TRect : IReadOnlyRect<TRect, T> {
			var r1max = Maximum;
			var r1min = Minimum;
			var r2max = r2.Maximum;
			var r2min = r2.Minimum;
			T minx = T.Min(r1min.X, r2min.X);
			T miny = T.Min(r1min.Y, r2min.Y);
			T maxx = T.Max(r1max.X, r2max.X);
			T maxy = T.Max(r1max.Y, r2max.Y);
			return TSelf.Create(minx, miny, maxx - minx, maxy - miny);
		}

		/// <summary>
		/// Tests if two rectangles of potentially different types are equal.
		/// </summary>
		/// <typeparam name="TRect">Other rectangle type</typeparam>
		/// <param name="r2"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals<TRect>(TRect r2)
			where TRect : IReadOnlyRect<TRect, T> {
			var p1 = Position;
			var p2 = r2.Position;
			var s1 = Size;
			var s2 = r2.Size;
			return p1.X == p2.X && p1.Y == p2.Y && s1.X == s2.X && s1.Y == s2.Y;
		}

	}

	/// <summary>
	/// A modifiable version of an <see cref="IReadOnlyRect{T}"/>.
	/// </summary>
	/// <typeparam name="T">Coordinate numeric type</typeparam>
	public interface IRect<TSelf,T> :
		IReadOnlyRect<TSelf,T> 
		where TSelf : IRect<TSelf, T>
		where T : unmanaged, INumber<T> {

		/// <summary>
		/// The position of the rectangle. These must be the coordinates of the minimum point on the rectangle.
		/// Normally this will be the top-left of the rectangle.
		/// </summary>
		public new ITuple2<T> Position { get; set; }

		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public new ITuple2<T> Size { get; set; }

	}

	/// <summary>
	/// A rectangle implementation using <see cref="int"/> as the numeric type.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Recti : IReadOnlyRect<int>, IRect<Recti, int>, IEquatable<Recti> {

		/// <summary>
		/// Creates a copy of a rectangle of another type.
		/// </summary>
		/// <typeparam name="TRect">Rectangle type to convert from</typeparam>
		/// <param name="rect">Rectangle to copy</param>
		/// <returns>Copy of rectangle</returns>
		public static Recti Create<TRect>(TRect rect)
			where TRect : IReadOnlyRect<TRect, int> => new(rect.Position, rect.Size);

		public static Recti Create(int x, int y, int w, int h) => new(x, y, w, h);

		/// <summary>
		/// The position of the rectangle. These must be the coordinates of the minimum point on the rectangle.
		/// Normally this will be the top-left of the rectangle.
		/// </summary>
		public Vector2i Position = default;

		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public Vector2i Size = default;

		/// <summary>
		/// The minimum point of the rectangle, equivalent to <see cref="Position"/>.
		/// </summary>
		public Vector2i Minimum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Position;
		}

		/// <summary>
		/// The maximum point of the rectangle, equivalent to <see cref="Position"/> + <see cref="Size"/>.
		/// </summary>
		public Vector2i Maximum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Position + Size;
		}

		IReadOnlyTuple2<int> IReadOnlyRect<int>.Position => Position;

		IReadOnlyTuple2<int> IReadOnlyRect<int>.Size => Size;

		ITuple2<int> IRect<Recti, int>.Position {
			get => Position;
			set => Position = new(value);
		}
		
		ITuple2<int> IRect<Recti, int>.Size {
			get => Size;
			set => Size = new(value);
		}

		/// <summary>
		/// Creates a new rectangle at (0,0) with the given size.
		/// </summary>
		/// <param name="size">Size tuple</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Recti(IReadOnlyTuple2<int> size) : this(size.X, size.Y) { }

		/// <summary>
		/// Creates a new rectangle at (0,0) with the given size.
		/// </summary>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Recti(int width, int height) {
			Position = new Vector2i(0, 0);
			Size = new Vector2i(width, height);
		}

		/// <summary>
		/// Creates a rectangle defined by a position and size.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Recti(int x, int y, int w, int h) {
			Position = new Vector2i(x, y);
			Size = new Vector2i(w, h);
		}

		/// <summary>
		/// Creates a rectangle defined by a position and size.
		/// </summary>
		/// <param name="position">Position tuple</param>
		/// <param name="size">Size tuple</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Recti(IReadOnlyTuple2<int> position, IReadOnlyTuple2<int> size) {
			Position = new Vector2i(position);
			Size = new Vector2i(size);
		}

		public bool Equals(Recti r) => Position == r.Position && Size == r.Size;

		public override bool Equals(object? obj) => obj is Recti r && Equals(r);

		public override int GetHashCode() => Position.GetHashCode() ^ Size.GetHashCode();

		public override string ToString() => $"Recti({Size}@{Position})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Recti r1, Recti r2) => r1.Equals(r2);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Recti r1, Recti r2) => !r1.Equals(r2);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Recti(Rectf r) => new((int)r.Position.X, (int)r.Position.Y, (int)r.Size.X, (int)r.Size.Y);

	}

	/// <summary>
	/// A rectangle implementation using <see cref="float"/> as the numeric type.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Rectf : IReadOnlyRect<float>, IRect<Rectf, float>, IEquatable<Rectf> {

		/// <summary>
		/// Creates a copy of a rectangle of another type.
		/// </summary>
		/// <typeparam name="TRect">Rectangle type to convert from</typeparam>
		/// <param name="rect">Rectangle to copy</param>
		/// <returns>Copy of rectangle</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Recti Create<TRect>(TRect rect)
			where TRect : IReadOnlyRect<TRect, int> => new(rect.Position, rect.Size);

		public static Rectf Create(float x, float y, float w, float h) => new(x, y, w, h);

		/// <summary>
		/// The position of the rectangle. These must be the coordinates of the minimum point on the rectangle.
		/// Normally this will be the top-left of the rectangle.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// The minimum point of the rectangle, equivalent to <see cref="Position"/>.
		/// </summary>
		public Vector2 Minimum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Position;
		}

		/// <summary>
		/// The maximum point of the rectangle, equivalent to <see cref="Position"/> + <see cref="Size"/>.
		/// </summary>
		public Vector2 Maximum {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Position + Size;
		}

		ITuple2<float> IRect<Rectf, float>.Position {
			get => new Tuple2<float>(Position.X, Position.Y);
			set => Position = new(value.X, value.Y);
		}

		ITuple2<float> IRect<Rectf, float>.Size {
			get => new Tuple2<float>(Size.X, Size.Y);
			set => Size = new(value.X, value.Y);
		}

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Position => new Tuple2<float>(Position.X, Position.Y);

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Size => new Tuple2<float>(Size.X, Size.Y);

		/// <summary>
		/// Creates a new rectangle at (0,0) with the given size.
		/// </summary>
		/// <param name="size">Size tuple</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rectf(IReadOnlyTuple2<float> size) : this(size.X, size.Y) { }

		/// <summary>
		/// Creates a new rectangle at (0,0) with the given size.
		/// </summary>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rectf(float width, float height) {
			Position = new Vector2(0, 0);
			Size = new Vector2(width, height);
		}

		/// <summary>
		/// Creates a rectangle defined by a position and size.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rectf(float x, float y, float w, float h) {
			Position = new Vector2(x, y);
			Size = new Vector2(w, h);
		}

		/// <summary>
		/// Creates a rectangle defined by a position and size.
		/// </summary>
		/// <param name="position">Position tuple</param>
		/// <param name="size">Size tuple</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Rectf(IReadOnlyTuple2<float> position, IReadOnlyTuple2<float> size) {
			Position = new Vector2(position.X, position.Y);
			Size = new Vector2(size.X, size.Y);
		}

		public bool Equals(Rectf r) => Position == r.Position && Size == r.Size;

		public override bool Equals(object? obj) => obj is Rectf r && Equals(r);

		public override int GetHashCode() => Position.GetHashCode() ^ Size.GetHashCode();

		public override string ToString() => $"Rectf({Size}@{Position})";

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Rectf r1, Rectf r2) => r1.Equals(r2);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Rectf r1, Rectf r2) => !r1.Equals(r2);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Rectf(Recti r) => new(r.Position.X, r.Position.Y, r.Size.X, r.Size.Y);

	}

}
