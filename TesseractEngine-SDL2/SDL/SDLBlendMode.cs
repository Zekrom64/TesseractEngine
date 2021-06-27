using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.SDL {
	
	/// <summary>
	/// Predefined blend modes
	/// </summary>
	public enum SDLBlendMode : int {
		/// <summary>
		/// No blending, <c>DST = SRC</c>
		/// </summary>
		None = 0x00000000,
		/// <summary>
		/// Alpha blending, <c>DST(RGB) = SRC(RGB) * SRC(A) + (DST(RGB) * (1 - SRC(A))), DST(A) = SRC(A) + (DST(A) * (1 - SRC(A)))</c>
		/// </summary>
		Blend = 0x00000001,
		/// <summary>
		/// Additive blending, <c>DST(RGB) = (SRC(RGB) * SRC(A)) + DST(RGB), DST(A) = DST(A)</c>
		/// </summary>
		Add = 0x00000002,
		/// <summary>
		/// Color modulate, <c>DST(RGB) = SRC(RGB) * DST(RGB), DST(A) = DST(A)</c>
		/// </summary>
		Modulate = 0x00000004,
		/// <summary>
		/// Color multiply, <c>DST(RGB) = (SRC(RGB) * DST(RGB)) + (DST(RGB) * (1 - SRC(A))), DST(A) = (SRC(A) * DST(A)) + (DST(A) * (1 - SRC(A)))</c>
		/// </summary>
		Multiply = 0x00000008,
		/// <summary>
		/// Invalid blend mode.
		/// </summary>
		Invalid = 0x7FFFFFFF
	}

	/// <summary>
	/// The operation to apply for blending.
	/// </summary>
	public enum SDLBlendOperation : int {
		/// <summary>
		/// Addition, <c>DST + SRC</c>
		/// </summary>
		Add = 1,
		/// <summary>
		/// Subtraction, <c>DST - SRC</c>
		/// </summary>
		Subtract = 2,
		/// <summary>
		/// Reverse subtraction, <c>SRC - DST</c>
		/// </summary>
		ReverseSubtract = 3,
		/// <summary>
		/// Minimum, <c>min(DST, SRC)</c>
		/// </summary>
		Minimum = 4,
		/// <summary>
		/// Maximum, <c>max(DST, SRC)</c>
		/// </summary>
		Maximum = 5
	}

	/// <summary>
	/// The normalized factor to use in blending.
	/// </summary>
	public enum SDLBlendFactor : int {
		/// <summary>
		/// <c>(0, 0, 0, 0)</c>
		/// </summary>
		Zero = 1,
		/// <summary>
		/// <c>(1, 1, 1, 1)</c>
		/// </summary>
		One = 2,
		/// <summary>
		/// <c>(SRC(R), SRC(G), SRC(B), SRC(A))</c>
		/// </summary>
		SrcColor = 3,
		/// <summary>
		/// <c>(1 - SRC(R), 1 - SRC(G), 1 - SRC(B), 1 - SRC(A))</c>
		/// </summary>
		OneMinusSrcColor = 4,
		/// <summary>
		/// <c>(SRC(A), SRC(A), SRC(A), SRC(A))</c>
		/// </summary>
		SrcAlpha = 5,
		/// <summary>
		/// <c>(1 - SRC(A), 1 - SRC(A), 1 - SRC(A), 1 - SRC(A))</c>
		/// </summary>
		OneMinusSrcAlpha = 6,
		/// <summary>
		/// <c>(DST(R), DST(G), DST(B), DST(A))</c>
		/// </summary>
		DstColor = 7,
		/// <summary>
		/// <c>(1 - DST(R), 1 - DST(G), 1 - DST(B), 1 - DST(A))</c>
		/// </summary>
		OneMinusDstColor = 8,
		/// <summary>
		/// <c>(DST(A), DST(A), DST(A), DST(A))</c>
		/// </summary>
		DstAlpha = 9,
		/// <summary>
		/// <c>(1 - DST(A), 1 - DST(A), 1 - DST(A), 1 - DST(A))</c>
		/// </summary>
		OneMinusDstAlpha = 10
	}

}
