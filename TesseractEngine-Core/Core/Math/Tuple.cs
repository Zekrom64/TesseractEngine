using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Util;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Math {

	// IReadOnlyTuple

	public interface IReadOnlyTuple<T1, T2> {

		public T1 X { get; }

		public T2 Y { get; }
		
	}

	public interface IReadOnlyTuple<T1, T2, T3> : IReadOnlyTuple<T1,T2> { 
	
		public T3 Z { get; }

	}

	public interface IReadOnlyTuple<T1, T2, T3, T4> : IReadOnlyTuple<T1, T2, T3> {
		
		public T4 W { get; }

	}

	// ITuple

	public interface ITuple<T1, T2> : IReadOnlyTuple<T1, T2> {

		public new T1 X { get; set; }

		public new T2 Y { get; set; }

		T1 IReadOnlyTuple<T1, T2>.X => X;

		T2 IReadOnlyTuple<T1, T2>.Y => Y;

	}

	public interface ITuple<T1, T2, T3> : ITuple<T1, T2>, IReadOnlyTuple<T1, T2, T3> {

		public new T3 Z { get; set; }

		T3 IReadOnlyTuple<T1, T2, T3>.Z => Z;

	}

	public interface ITuple<T1, T2, T3, T4> : ITuple<T1, T2, T3>, IReadOnlyTuple<T1, T2, T3, T4> {

		public new T4 W { get; set; }

		T4 IReadOnlyTuple<T1, T2, T3, T4>.W => W;

	}

	// Tuple

	public struct Tuple<T1, T2> : ITuple<T1, T2> {

		public T1 X { get; set; }
		public T2 Y { get; set; }

		public Tuple(T1 x, T2 y) {
			X = x;
			Y = y;
		}

		public Tuple(IReadOnlyTuple<T1, T2> t) {
			X = t.X;
			Y = t.Y;
		}

		public static implicit operator Tuple<T1, T2>(ValueTuple<T1, T2> vt) => new(vt.Item1, vt.Item2);

		public static implicit operator ValueTuple<T1, T2>(Tuple<T1, T2> t) => (t.X, t.Y);

	}

	public struct Tuple<T1, T2, T3> : ITuple<T1, T2, T3> {

		public T1 X { get; set; }
		public T2 Y { get; set; }
		public T3 Z { get; set; }

		public Tuple(T1 x, T2 y, T3 z) {
			X = x;
			Y = y;
			Z = z;
		}

		public Tuple(IReadOnlyTuple<T1, T2, T3> t) {
			X = t.X;
			Y = t.Y;
			Z = t.Z;
		}

		public static implicit operator Tuple<T1, T2, T3>(ValueTuple<T1, T2, T3> vt) => new(vt.Item1, vt.Item2, vt.Item3);

		public static implicit operator ValueTuple<T1, T2, T3>(Tuple<T1, T2, T3> t) => (t.X, t.Y, t.Z);

	}

	public struct Tuple<T1, T2, T3, T4> : ITuple<T1, T2, T3, T4> {

		public T1 X { get; set; }
		public T2 Y { get; set; }
		public T3 Z { get; set; }
		public T4 W { get; set; }

		public Tuple(T1 x, T2 y, T3 z, T4 w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public Tuple(IReadOnlyTuple<T1, T2, T3, T4> t) {
			X = t.X;
			Y = t.Y;
			Z = t.Z;
			W = t.W;
		}

		public static implicit operator Tuple<T1, T2, T3, T4>(ValueTuple<T1, T2, T3, T4> vt) => new(vt.Item1, vt.Item2, vt.Item3, vt.Item4);

		public static implicit operator ValueTuple<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> t) => (t.X, t.Y, t.Z, t.W);

	}

	// Tuple2

	public interface IReadOnlyTuple2<T> : IReadOnlyTuple<T, T>,  IReadOnlyIndexer<int,T> { }

	public interface ITuple2<T> : IReadOnlyTuple2<T>, ITuple<T, T>, IIndexer<int,T> { }

	[StructLayout(LayoutKind.Sequential)]
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

		public T this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				_ => default
			};
			set {
				switch (key) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

		T IReadOnlyIndexer<int, T>.this[int key] => this[key];

		public static implicit operator Tuple2<T>(ValueTuple<T, T> t) => new(t.Item1, t.Item2);

		public static implicit operator ValueTuple<T, T>(Tuple2<T> t) => (t.X, t.Y);

		public static implicit operator Tuple<T, T>(Tuple2<T> t) => new(t.X, t.Y);

	}

	// Tuple3

	public interface IReadOnlyTuple3<T> : IReadOnlyTuple2<T>, IReadOnlyTuple<T, T, T> { }

	public interface ITuple3<T> : ITuple2<T>, ITuple<T, T, T>, IReadOnlyTuple3<T> { }

	[StructLayout(LayoutKind.Sequential)]
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

		public static implicit operator Tuple3<T>(ValueTuple<T, T, T> t) => new(t.Item1, t.Item2, t.Item3);

		public static implicit operator ValueTuple<T, T, T>(Tuple3<T> t) => (t.X, t.Y, t.Z);

		public static implicit operator Tuple<T, T, T>(Tuple3<T> t) => new(t.X, t.Y, t.Z);

	}

	// Tuple4

	public interface IReadOnlyTuple4<T> : IReadOnlyTuple3<T>, IReadOnlyTuple<T, T, T, T> { }

	public interface ITuple4<T> : ITuple3<T>, ITuple<T, T, T, T>, IReadOnlyTuple4<T> { }

	[StructLayout(LayoutKind.Sequential)]
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

		public static implicit operator Tuple4<T>(ValueTuple<T, T, T, T> t) => new(t.Item1, t.Item2, t.Item3, t.Item4);

		public static implicit operator ValueTuple<T, T, T, T>(Tuple4<T> t) => (t.X, t.Y, t.Z, t.W);

		public static implicit operator Tuple<T, T, T, T>(Tuple4<T> t) => new(t.X, t.Y, t.Z, t.W);

	}

}
