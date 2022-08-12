using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {

	public struct Vector4b : ITuple4<byte>, IEquatable<IReadOnlyTuple4<byte>> {

		public byte X;

		public byte Y;

		public byte Z;

		public byte W;

		public Vector4b(byte s) {
			X = Y = Z = W = s;
		}

		public Vector4b(byte x, byte y, byte z, byte w) {
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public byte this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				3 => W,
				_ => default
			};
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
					case 3:
						W = value;
						break;
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		byte ITuple<byte, byte>.X { get => X; set => X = value; }
		byte ITuple<byte, byte>.Y { get => Y; set => Y = value; }
		byte ITuple<byte, byte, byte>.Z { get => Z; set => Z = value; }
		byte ITuple<byte, byte, byte, byte>.W { get => W; set => W = value; }

		public bool Equals(IReadOnlyTuple4<byte>? other) => other != null && other.X == X && other.Y == Y && other.Z == Z && other.W == W;
	}

}
