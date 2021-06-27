using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.SDL {
	
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLPoint {

		public int X;
		public int Y;

		public bool InRect(SDLRect r) => X >= r.X && X < (r.X + r.W) && Y >= r.Y && Y < (r.Y + r.H);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLFPoint {

		public float X;
		public float Y;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLRect : IEquatable<SDLRect> {

		public int X;
		public int Y;
		public int W;
		public int H;

		public bool Empty => W <= 0 || H <= 0;

		public bool HasIntersection(SDLRect b) => SDL2.Functions.SDL_HasIntersection(ref this, ref b);

		public SDLRect? Intersect(SDLRect b) {
			if (SDL2.Functions.SDL_IntersectRect(ref this, ref b, out SDLRect result)) return result;
			else return null;
		}

		public SDLRect Union(SDLRect b) {
			SDL2.Functions.SDL_UnionRect(ref this, ref b, out SDLRect result);
			return result;
		}

		public bool Equals(SDLRect b) => X == b.X && Y == b.Y && W == b.W && H == b.H;

		public override bool Equals(object obj) => obj is SDLRect rect && Equals(rect);

		public static bool operator ==(SDLRect left, SDLRect right) => left.Equals(right);

		public static bool operator !=(SDLRect left, SDLRect right) => !(left == right);

		public override int GetHashCode() => X ^ (Y << 8) ^ (W << 8) ^ (H << 8);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLFRect {

		public float X;
		public float Y;
		public float W;
		public float H;

	}

}
