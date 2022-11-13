using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.SDL {
	
	/* // SDL point/structure types made redundant by the core library
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLPoint : ITuple2<int> {

		public int X = 0;
		public int Y = 0;

		int ITuple<int, int>.X { get => X; set => X = value; }
		int ITuple<int, int>.Y { get => Y; set => Y = value; }

		int IReadOnlyIndexer<int, int>.this[int key] => this[key];

		public int this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				_ => 0
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SDLPoint(int x, int y) {
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SDLPoint(IReadOnlyTuple2<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool InRect(SDLRect r) => X >= r.X && X < (r.X + r.W) && Y >= r.Y && Y < (r.Y + r.H);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator SDLPoint(Vector2i v) => new(v);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2i(SDLPoint p) => new(p);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLFPoint : ITuple2<float> {

		public float X = 0;
		public float Y = 0;

		float ITuple<float, float>.X { get => X; set => X = value; }
		float ITuple<float, float>.Y { get => Y; set => Y = value; }

		float IReadOnlyIndexer<int, float>.this[int key] => this[key];

		public float this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				_ => 0
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
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SDLFPoint(float x, float y) {
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SDLFPoint(Vector2 v) {
			X = v.X;
			Y = v.Y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SDLFPoint(IReadOnlyTuple2<float> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		public bool InRect(SDLFRect r) => X >= r.X && X < (r.X + r.W) && Y >= r.Y && Y < (r.Y + r.H);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator SDLFPoint(Vector2 v) => new(v);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(SDLFPoint p) => new(p.X, p.Y);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLRect : IReadOnlyRect<SDLRect, int>, IEquatable<SDLRect> {

		public int X;
		public int Y;
		public int W;
		public int H;

		public ITuple2<int> Position {
			get => new Vector2i(X, Y);
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		public ITuple2<int> Size {
			get => new Vector2i(W, H);
			set {
				W = value.X;
				H = value.Y;
			}
		}

		public SDLRect(int x, int y, int w, int h) {
			X = x;
			Y = y;
			W = w;
			H = h;
		}

		IReadOnlyTuple2<int> IReadOnlyRect<SDLRect, int>.Position => throw new NotImplementedException();

		IReadOnlyTuple2<int> IReadOnlyRect<SDLRect, int>.Size => throw new NotImplementedException();

		public static SDLRect Create(int x, int y, int w, int h) {
			throw new NotImplementedException();
		}

		public bool Equals(SDLRect other) => X == other.X && Y == other.Y && W == other.W && H == other.H;

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is SDLRect rect && Equals(rect);

		public bool HasIntersection(in SDLRect b) => SDL2.Functions.SDL_HasIntersection(this, b);

		public SDLRect? Intersect(in SDLRect b) {
			if (SDL2.Functions.SDL_IntersectRect(this, b, out SDLRect result)) return result;
			else return null;
		}

		public SDLRect Union(in SDLRect b) {
			SDL2.Functions.SDL_UnionRect(this, b, out SDLRect result);
			return result;
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLFRect : IReadOnlyRect<float> {

		private readonly float X;
		private readonly float Y;
		private readonly float W;
		private readonly float H;

		public IReadOnlyTuple2<float> Position => new Tuple2<float>(x, y);

		public IReadOnlyTuple2<float> Size => new Tuple2<float>(w, h);

		public bool Empty => w <= 0 || h <= 0;

		public float Area => Empty ? 0 : w * h;

	}
	*/

}
