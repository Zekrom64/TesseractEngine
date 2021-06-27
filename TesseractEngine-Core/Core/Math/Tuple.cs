using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Util;

namespace Tesseract.Core.Math {

	public interface IReadOnlyTuple2<T> : IReadOnlyIndexer<int,T> {

		public T X { get; }

		public T Y { get; }

	}

	public interface ITuple2<T> : IReadOnlyTuple2<T>, IIndexer<int,T> {

		public new T X { get; set; }

		public new T Y { get; set; }

	}

	public interface IReadOnlyTuple3<T> : IReadOnlyTuple2<T> {

		public T Z { get; }

	}

	public interface ITuple3<T> : ITuple2<T>, IReadOnlyTuple3<T> {

		public new T Z { get; set; }

	}

	public interface IReadOnlyTuple4<T> : IReadOnlyTuple3<T> {

		public T W { get; }

	}

	public interface ITuple4<T> : ITuple3<T>, IReadOnlyTuple4<T> {

		public new T W { get; set; }

	}

	public struct Tuple2<T> : ITuple2<T> {

		public T X { get; set; }
		public T Y { get; set; }

		public Tuple2(T s) {
			X = Y = s;
		}

		public Tuple2(T x, T y) {
			X = x;
			Y = y;
		}

		public Tuple2(IReadOnlyTuple2<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		T IReadOnlyTuple2<T>.X => X;

		T IReadOnlyTuple2<T>.Y => Y;

		public T this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch(key) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

	}

	public struct Tuple3<T> : ITuple3<T> {

		public T X { get; set; }
		public T Y { get; set; }
		public T Z { get; set; }

		public Tuple3(T s) {
			X = Y = Z = s;
		}

		public Tuple3(T x, T y, T z) {
			X = x;
			Y = y;
			Z = z;
		}

		public Tuple3(IReadOnlyTuple3<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		T IReadOnlyTuple2<T>.X => X;

		T IReadOnlyTuple2<T>.Y => Y;

		T IReadOnlyTuple3<T>.Z => Z;

		public T this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => default
			};
			set {
				switch (key) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

	}

	public struct Tuple4<T> : ITuple4<T> {

		public T X { get; set; }
		public T Y { get; set; }
		public T Z { get; set; }
		public T W { get; set; }

		public Tuple4(T s) {
			X = Y = Z = W = s;
		}

		public Tuple4(T x, T y, T z, T w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Tuple4(IReadOnlyTuple4<T> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
			W = tuple.W;
		}

		T IReadOnlyTuple2<T>.X => X;

		T IReadOnlyTuple2<T>.Y => Y;

		T IReadOnlyTuple3<T>.Z => Z;

		T IReadOnlyTuple4<T>.W => W;

		public T this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
			set {
				switch (key) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
					case 3: W = value; break;
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

	}

}
