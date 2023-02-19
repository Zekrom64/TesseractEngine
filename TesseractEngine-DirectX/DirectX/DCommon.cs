using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.DirectX.GI;

namespace Tesseract.DirectX {
	
	public enum DWriteMeasuringMode {
		Natural,
		GDIClassic,
		GDINatural
	}

	[Flags]
	public enum DWriteGlyphImageFormats {
		None = 0,
		TrueType = 0x01,
		CFF = 0x02,
		COLR = 0x04,
		SVG = 0x08,
		PNG = 0x10,
		JPEG = 0x20,
		TIFF = 0x40,
		PremultipliedB8G8R8A8 = 0x80
	}

	public enum D2D1AlphaMode : uint {
		Unknown = 0,
		Premultiplied,
		Straight,
		Ignore
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct D2D1PixelFormat {

		public DXGIFormat Format;
		public D2D1AlphaMode AlphaMode;

	}

}
