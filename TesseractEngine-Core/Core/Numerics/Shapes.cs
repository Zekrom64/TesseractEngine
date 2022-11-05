using System;
using System.Numerics;

namespace Tesseract.Core.Numerics {

	/// <summary>
	/// A read-only rectangle defined by a 2D position and size.
	/// </summary>
	/// <typeparam name="T">Coordinate numeric type</typeparam>
	public interface IReadOnlyRect<T> where T : unmanaged {

		/// <summary>
		/// The position of the top-left corner of the rectangle.
		/// </summary>
		public IReadOnlyTuple2<T> Position { get; }
		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public IReadOnlyTuple2<T> Size { get; }

		/// <summary>
		/// If the rectangle is empty (ie. has a size of 0 in either axis).
		/// </summary>
		public bool Empty { get; }

		/// <summary>
		/// The total area of the rectangle (width * height).
		/// </summary>
		public T Area { get; }

	}

	/// <summary>
	/// A modifiable version of an <see cref="IReadOnlyRect{T}"/>.
	/// </summary>
	/// <typeparam name="T">Coordinate numeric type</typeparam>
	public interface IRect<T> : IReadOnlyRect<T> where T : unmanaged {

		/// <summary>
		/// The position of the top-left corner of the rectangle.
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
	public struct Recti : IRect<int>, IEquatable<IReadOnlyRect<int>>, IEquatable<Recti> {

		/// <summary>
		/// The position of the top-left corner of the rectangle.
		/// </summary>
		public Vector2i Position;
		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public Vector2i Size;

		/// <summary>
		/// The top-left X coordinate.
		/// </summary>
		public int X0 => Position.X;
		/// <summary>
		/// The top-left Y coordinate.
		/// </summary>
		public int Y0 => Position.Y;
		/// <summary>
		/// The bottom-right X coordinate.
		/// </summary>
		public int X1 => Position.X + Size.X;
		/// <summary>
		/// The bottom-right Y coordinate.
		/// </summary>
		public int Y1 => Position.Y + Size.Y;

		public Recti(IReadOnlyTuple2<int> size) : this(size.X, size.Y) { }

		public Recti(int width, int height) {
			Position = new Vector2i(0, 0);
			Size = new Vector2i(width, height);
		}

		public Recti(int x, int y, int w, int h) {
			Position = new Vector2i(x, y);
			Size = new Vector2i(w, h);
		}

		/// <summary>
		/// Creates a rectangle defined by a position and size.
		/// </summary>
		/// <param name="position">Position tuple</param>
		/// <param name="size">Size tuple</param>
		public Recti(IReadOnlyTuple2<int> position, IReadOnlyTuple2<int> size) {
			Position = new Vector2i(position);
			Size = new Vector2i(size);
		}

		public bool Empty => Size.X == 0 || Size.Y == 0;

		public int Area => Size.X * Size.Y;

		IReadOnlyTuple2<int> IReadOnlyRect<int>.Position => Position;

		ITuple2<int> IRect<int>.Position { get => Position; set => Position = new Vector2i(value); }

		IReadOnlyTuple2<int> IReadOnlyRect<int>.Size => Size;

		ITuple2<int> IRect<int>.Size { get => Size; set => Size = new Vector2i(value); }


		public bool Equals(IReadOnlyRect<int>? r) => r != null && Position.Equals(r.Position) && Size.Equals(r.Size);

		public bool Equals(Recti r) => Position == r.Position && Size == r.Size;

		public override bool Equals(object? obj) => obj is IReadOnlyRect<int> r && Equals(r);

		public override int GetHashCode() => Position.GetHashCode() ^ Size.GetHashCode();

		public override string ToString() => $"Recti({Size}@{Position})";

		public static bool operator ==(Recti r1, Recti r2) => r1.Equals(r2);

		public static bool operator !=(Recti r1, Recti r2) => !r1.Equals(r2);

	}

	/// <summary>
	/// A rectangle implementation using <see cref="float"/> as the numeric type.
	/// </summary>
	public struct Rectf : IRect<float>, IEquatable<IReadOnlyRect<float>>, IEquatable<Rectf> {

		/// <summary>
		/// The position of the top-left corner of the rectangle.
		/// </summary>
		public Vector2 Position;
		/// <summary>
		/// The size of the rectangle.
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// The top-left X coordinate.
		/// </summary>
		public float X0 => Position.X;
		/// <summary>
		/// The top-left Y coordinate.
		/// </summary>
		public float Y0 => Position.Y;
		/// <summary>
		/// The bottom-right X coordinate.
		/// </summary>
		public float X1 => Position.X + Size.X;
		/// <summary>
		/// The bottom-right Y coordinate.
		/// </summary>
		public float Y1 => Position.Y + Size.Y;

		/// <summary>
		/// Creates a rectangle defined by an X and Y position and a width and height.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		public Rectf(float x, float y, float w, float h) {
			Position = new Vector2(x, y);
			Size = new Vector2(w, h);
		}

		public bool Empty => Size.X == 0 && Size.Y == 0;

		public float Area => Size.X * Size.Y;

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Position => new Tuple2<float>(Position.X, Position.Y);

		ITuple2<float> IRect<float>.Position { get => new Tuple2<float>(Position.X, Position.Y); set => Position = new Vector2(value.X, value.Y); }

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Size => new Tuple2<float>(Size.X, Size.Y);

		ITuple2<float> IRect<float>.Size { get => new Tuple2<float>(Size.X, Size.Y); set => Size = new Vector2(value.X, value.Y); }


		public bool Equals(IReadOnlyRect<float>? r) => r != null && Position.Equals(r.Position) && Size.Equals(r.Size);

		public bool Equals(Rectf r) => Position == r.Position && Size == r.Size;

		public override bool Equals(object? obj) => obj is IReadOnlyRect<float> r && Equals(r);

		public override int GetHashCode() => Position.GetHashCode() ^ Size.GetHashCode();

		public override string ToString() => $"Rectf({Size}@{Position})";

		public static bool operator ==(Rectf r1, Rectf r2) => r1.Equals(r2);

		public static bool operator !=(Rectf r1, Rectf r2) => !r1.Equals(r2);

	}

}
