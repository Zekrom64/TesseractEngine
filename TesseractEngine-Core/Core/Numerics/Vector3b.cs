using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {

	public struct Vector3b : ITuple3<byte>, IEquatable<IReadOnlyTuple3<byte>> {

		public byte X;

		public byte Y;

		public byte Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(byte s) {
			X = Y = Z = s;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b(byte x, byte y, byte z) {
			X = x;
			Y = y;
			Z = z;
		}

		public byte this[int key] {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
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
					case 2:
						Z = value;
						break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		byte ITuple<byte, byte>.X { get => X; set => X = value; }
		byte ITuple<byte, byte>.Y { get => Y; set => Y = value; }
		byte ITuple<byte, byte, byte>.Z { get => Z; set => Z = value; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(IReadOnlyTuple3<byte>? other) => other != null && other.X == X && other.Y == Y && other.Z == Z;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3b Swizzle(int x, int y, int z) => new(this[x], this[y], this[z]);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector4b Swizzle(int x, int y, int z, int w) => new(this[x], this[y], this[z], this[w]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Unswizzle(Vector3b v, int x, int y, int z) {
			this[x] = v.X;
			this[y] = v.Y;
			this[z] = v.Z;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Unswizzle(Vector4b v, int x, int y, int z, int w) {
			this[x] = v.X;
			this[y] = v.Y;
			this[z] = v.Z;
			this[w] = v.W;
		}

	}

}
