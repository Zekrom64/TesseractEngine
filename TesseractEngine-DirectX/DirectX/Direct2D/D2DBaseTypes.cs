using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.DirectX.Direct2D {

	/// <summary>
	/// Represents a rectangle defined by the coordinates of the upper-left corner (left, top) and the coordinates
	/// of the lower-right corner (right, bottom).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DRectF : IRect<D2DRectF, float>, IEquatable<D2DRectF> {

		/// <summary>
		/// The x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public float Left;

		/// <summary>
		/// The y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public float Top;

		/// <summary>
		/// The x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public float Right;

		/// <summary>
		/// The y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public float Bottom;

		public ITuple2<float> Position {
			get => new Vector2<float>(Left, Top);
			set {
				Left = value.X;
				Top = value.Y;
			}
		}

		public ITuple2<float> Size {
			get => new Vector2<float>(Right - Left, Bottom - Top);
			set {
				Right = Left + value.X;
				Bottom = Top + value.Y;
			}
		}

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Position => Position;

		IReadOnlyTuple2<float> IReadOnlyRect<float>.Size => Size;

		public static D2DRectF Create(float x, float y, float w, float h) => new() {
			Left = x,
			Top = y,
			Right = x + w,
			Bottom = y + h
		};

		public bool Equals(D2DRectF other) => Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;

		public override bool Equals(object? obj) => obj is D2DRectF rect && Equals(rect);

		public static implicit operator D2DRectF(Rectf r) => new() {
			Left = r.Minimum.X,
			Top = r.Minimum.Y,
			Right = r.Maximum.X,
			Bottom = r.Maximum.Y
		};

		public static implicit operator Rectf(D2DRectF r) => new(r.Left, r.Right, r.Right - r.Left, r.Bottom - r.Top);

		public static bool operator ==(D2DRectF left, D2DRectF right) => left.Equals(right);

		public static bool operator !=(D2DRectF left, D2DRectF right) => !(left == right);

		public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);
	}

	/// <summary>
	/// Represents a rectangle defined by the coordinates of the upper-left corner (left, top) and the coordinates
	/// of the lower-right corner (right, bottom).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct D2DRectU : IRect<D2DRectU, uint> {

		/// <summary>
		/// The x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public uint Left;

		/// <summary>
		/// The y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public uint Top;

		/// <summary>
		/// The x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public uint Right;

		/// <summary>
		/// The y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public uint Bottom;

		public ITuple2<uint> Position {
			get => new Vector2ui(Left, Top);
			set {
				Left = value.X;
				Top = value.Y;
			}
		}

		public ITuple2<uint> Size {
			get => new Vector2ui(Right - Left, Bottom - Top);
			set {
				Right = Left + value.X;
				Bottom = Top + value.Y;
			}
		}

		IReadOnlyTuple2<uint> IReadOnlyRect<uint>.Position => Position;

		IReadOnlyTuple2<uint> IReadOnlyRect<uint>.Size => Size;

		public static D2DRectU Create(uint x, uint y, uint w, uint h) => new() {
			Left = x,
			Top = y,
			Right = x + w,
			Bottom = y + h
		};

		public bool Equals(D2DRectU other) => Left == other.Left && Right == other.Right && Top == other.Top && Bottom == other.Bottom;

		public static implicit operator D2DRectU(Recti r) => new() {
			Left = checked((uint)r.Minimum.X),
			Top = checked((uint)r.Minimum.Y),
			Right = checked((uint)r.Maximum.X),
			Bottom = checked((uint)r.Maximum.Y)
		};

		public override bool Equals(object? obj) => obj is D2DRectU rect && Equals(rect);

		public static bool operator ==(D2DRectU left, D2DRectU right) => left.Equals(right);

		public static bool operator !=(D2DRectU left, D2DRectU right) => !(left == right);

		public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

	}

}
