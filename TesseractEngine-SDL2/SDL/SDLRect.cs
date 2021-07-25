using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;

namespace Tesseract.SDL {
	
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct SDLPoint : IReadOnlyTuple2<int> {

		private readonly int x;
		public int X { get => x; init => x = value; }
		private readonly int y;
		public int Y { get => y; init => y = value; }

		public int this[int key] => key switch {
			0 => x,
			1 => y,
			_ => 0
		};

		public bool InRect(SDLRect r) => X >= r.X && X < (r.X + r.W) && Y >= r.Y && Y < (r.Y + r.H);

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct SDLFPoint : IReadOnlyTuple2<float> {

		private readonly float x;
		public float X { get => x; init => x = value; }
		private readonly float y;
		public float Y { get => y; init => y = value; }

		public float this[int key] => key switch {
			0 => x,
			1 => y,
			_ => 0
		};

		public bool InRect(SDLFRect r) => X >= r.X && X < (r.X + r.W) && Y >= r.Y && Y < (r.Y + r.H);
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct SDLRect : IEquatable<SDLRect>, IReadOnlyRect<int> {

		private readonly int x;
		public int X { get => x; init => x = value; }
		private readonly int y;
		public int Y { get => y; init => y = value; }
		private readonly int w;
		public int W { get => w; init => w = value; }
		private readonly int h;
		public int H { get => h; init => h = value; }

		public bool Empty => W <= 0 || H <= 0;

		public IReadOnlyTuple2<int> Position => new Vector2i(x, y);

		public IReadOnlyTuple2<int> Size => new Vector2i(w, h);

		public int Area => Empty ? 0 : w * h;

		public bool HasIntersection(in SDLRect b) => SDL2.Functions.SDL_HasIntersection(this, b);

		public SDLRect? Intersect(in SDLRect b) {
			if (SDL2.Functions.SDL_IntersectRect(this, b, out SDLRect result)) return result;
			else return null;
		}

		public SDLRect Union(in SDLRect b) {
			SDL2.Functions.SDL_UnionRect(this, b, out SDLRect result);
			return result;
		}

		public bool Equals(SDLRect b) => X == b.X && Y == b.Y && W == b.W && H == b.H;

		public override bool Equals(object obj) => obj is SDLRect rect && Equals(rect);

		public static bool operator ==(in SDLRect left, in SDLRect right) => left.Equals(right);

		public static bool operator !=(in SDLRect left, in SDLRect right) => !(left == right);

		public override int GetHashCode() => X ^ (Y << 8) ^ (W << 8) ^ (H << 8);

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct SDLFRect : IReadOnlyRect<float> {

		private readonly float x;
		public float X { get => x; init => x = value; }
		private readonly float y;
		public float Y { get => y; init => y = value; }
		private readonly float w;
		public float W { get => w; init => w = value; }
		private readonly float h;
		public float H { get => h; init => h = value; }

		public IReadOnlyTuple2<float> Position => new Tuple2<float>(x, y);

		public IReadOnlyTuple2<float> Size => new Tuple2<float>(w, h);

		public bool Empty => w <= 0 || h <= 0;

		public float Area => Empty ? 0 : w * h;

	}

}
