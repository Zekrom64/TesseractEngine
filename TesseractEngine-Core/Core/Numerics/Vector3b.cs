using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Numerics {

	public struct Vector3b : ITuple3<byte>, IEquatable<IReadOnlyTuple3<byte>> {

		public byte X;

		public byte Y;

		public byte Z;

		public Vector3b(byte s) {
			X = Y = Z = s;
		}

		public Vector3b(byte x, byte y, byte z) {
			X = x;
			Y = y;
			Z = z;
		}

		public byte this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
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
				}
			}
		}

		byte IReadOnlyIndexer<int, byte>.this[int key] => this[key];

		byte ITuple<byte, byte>.X { get => X; set => X = value; }
		byte ITuple<byte, byte>.Y { get => Y; set => Y = value; }
		byte ITuple<byte, byte, byte>.Z { get => Z; set => Z = value; }

		public bool Equals(IReadOnlyTuple3<byte>? other) => other != null && other.X == X && other.Y == Y && other.Z == Z;
	}

}
