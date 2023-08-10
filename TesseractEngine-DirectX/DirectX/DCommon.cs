using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.DirectX.GI;

namespace Tesseract.DirectX {

	/// <summary>
	/// Indicates the measuring method used for text layout.
	/// </summary>
	public enum DWriteMeasuringMode {
		/// <summary>
		/// Specifies that text is measured using glyph ideal metrics whose values are independent to the current display resolution.
		/// </summary>
		Natural,
		/// <summary>
		/// Specifies that text is measured using glyph ideal metrics whose values are independent to the current display resolution.
		/// </summary>
		GDIClassic,
		/// <summary>
		/// Specifies that text is measured using the same glyph display metrics as text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY.
		/// </summary>
		GDINatural
	}

	/// <summary>
	/// Specifies which formats are supported in the font, either at a font-wide level or per glyph.
	/// </summary>
	[Flags]
	public enum DWriteGlyphImageFormats {
		/// <summary>
		/// Indicates no data is available for this glyph.
		/// </summary>
		None = 0,
		/// <summary>
		/// The glyph has TrueType outlines.
		/// </summary>
		TrueType = 0x01,
		/// <summary>
		/// The glyph has CFF outlines.
		/// </summary>
		CFF = 0x02,
		/// <summary>
		/// The glyph has multilayered COLR data.
		/// </summary>
		COLR = 0x04,
		/// <summary>
		/// The glyph has SVG outlines as standard XML. Fonts may store the content gzip'd rather than plain text,
		/// indicated by the first two bytes as gzip header {0x1F 0x8B}.
		/// </summary>
		SVG = 0x08,
		/// <summary>
		/// The glyph has PNG image data, with standard PNG IHDR.
		/// </summary>
		PNG = 0x10,
		/// <summary>
		/// The glyph has JPEG image data, with standard JIFF SOI header.
		/// </summary>
		JPEG = 0x20,
		/// <summary>
		/// The glyph has TIFF image data.
		/// </summary>
		TIFF = 0x40,
		/// <summary>
		/// The glyph has raw 32-bit premultiplied BGRA data.
		/// </summary>
		PremultipliedB8G8R8A8 = 0x80
	}

	/// <summary>
	/// Specifies how the alpha value of a bitmap or render target should be treated.
	/// </summary>
	public enum D2D1AlphaMode : uint {
		/// <summary>
		/// The alpha value might not be meaningful.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// The alpha value has been premultiplied. Each color is first scaled by the alpha value. The
		/// alpha value itself is the same in both straight and premultiplied alpha. Typically, no color
		/// channel value is greater than the alpha channel value. If a color channel value in a premultiplied
		/// format is greater than the alpha channel, the standard source-over blending math results in an additive blend.
		/// </summary>
		Premultiplied,
		/// <summary>
		/// The alpha value has not been premultiplied. The alpha channel indicates the transparency of the color.
		/// </summary>
		Straight,
		/// <summary>
		/// The alpha value is ignored.
		/// </summary>
		Ignore
	}

	/// <summary>
	/// Contains the data format and alpha mode for a bitmap or render target.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1PixelFormat {

		/// <summary>
		/// A value that specifies the size and arrangement of channels in each pixel.
		/// </summary>
		public DXGIFormat Format;
		/// <summary>
		/// A value that specifies whether the alpha channel is using pre-multiplied alpha, straight alpha, whether
		/// it should be ignored and considered opaque, or whether it is unknown.
		/// </summary>
		public D2D1AlphaMode AlphaMode;

	}

}
