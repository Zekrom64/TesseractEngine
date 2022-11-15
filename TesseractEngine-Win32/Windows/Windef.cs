using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;

namespace Tesseract.Windows {

	using LONG = Int32;

	/// <summary>
	/// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT {

		/// <summary>
		/// Specifies the x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public LONG Left;
		/// <summary>
		/// Specifies the y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		public LONG Top;
		/// <summary>
		/// Specifies the y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public LONG Right;
		/// <summary>
		/// Specifies the x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		public LONG Bottom;

		public static implicit operator Recti(RECT rect) =>
			new(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

		public static implicit operator RECT(Recti rect) {
			var min = rect.Minimum;
			var max = rect.Maximum;
			return new() {
			   Left = min.X,
			   Top = min.Y,
			   Right = max.X,
			   Bottom = max.Y
		   };
		}

	}

	/// <summary>
	/// The POINT structure defines the x- and y-coordinates of a point.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {

		/// <summary>
		/// Specifies the x-coordinate of the point.
		/// </summary>
		public LONG X;

		/// <summary>
		/// Specifies the y-coordinate of the point.
		/// </summary>
		public LONG Y;

		public static implicit operator Vector2i(POINT pt) => new(pt.X, pt.Y);

		public static implicit operator POINT(Vector2i v) => new() { X = v.X, Y = v.Y };

	}

}
