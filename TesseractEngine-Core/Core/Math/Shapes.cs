using System;
using System.Numerics;

namespace Tesseract.Core.Math {

	public interface IReadOnlyRect<T> where T : unmanaged {

		public IReadOnlyTuple2<T> Position { get; }
		public IReadOnlyTuple2<T> Size { get; }

		public bool Empty { get; }

		public T Area { get; }

	}

	public interface IRect<T> : IReadOnlyRect<T> where T : unmanaged {

		public new ITuple2<T> Position { get; set; }
		public new ITuple2<T> Size { get; set; }

	}

	public struct Recti : IRect<int>, IEquatable<IReadOnlyRect<int>>, IEquatable<Recti> {

		public Vector2i Position;
		public Vector2i Size;

		public int X0 => Position.X;
		public int Y0 => Position.Y;
		public int X1 => Position.X + Size.X;
		public int Y1 => Position.Y + Size.Y;

		public Recti(int x, int y, int w, int h) {
			Position = new Vector2i(x, y);
			Size = new Vector2i(w, h);
		}

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

	public struct Rectf : IRect<float>, IEquatable<IReadOnlyRect<float>>, IEquatable<Rectf> {

		public Vector2 Position;
		public Vector2 Size;

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
