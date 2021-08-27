using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Graphics;
using Tesseract.Core.Native;
using Tesseract.Core.Util;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	/// <summary>
	/// Enumeration of SDL pixel types.
	/// </summary>
	public enum SDLPixelType {
		/// <summary>
		/// Unknown pixel type.
		/// </summary>
		Unknown,
		/// <summary>
		/// 1-bpp indexed pixel.
		/// </summary>
		Index1,
		/// <summary>
		/// 4-bpp indexed pixel.
		/// </summary>
		Index4,
		/// <summary>
		/// 8-bpp indexed pixel.
		/// </summary>
		Index8,
		/// <summary>
		/// Packed 8-bit pixel.
		/// </summary>
		Packed8,
		/// <summary>
		/// Packed 16-bit pixel.
		/// </summary>
		Packed16,
		/// <summary>
		/// Packed 32-bit pixel.
		/// </summary>
		Packed32,
		/// <summary>
		/// Array of 8-bit component pixels.
		/// </summary>
		ArrayU8,
		/// <summary>
		/// Array of 16-bit component pixels.
		/// </summary>
		ArrayU16,
		/// <summary>
		/// Array of 32-bit component pixels.
		/// </summary>
		ArrayU32,
		/// <summary>
		/// Array of 16-bit floating component pixels.
		/// </summary>
		ArrayF16,
		/// <summary>
		/// Array of 32-bit floating component pixels.
		/// </summary>
		ArrayF32
	}

	/// <summary>
	/// Enumeration of SDL bitmap orders.
	/// </summary>
	public enum SDLBitmapOrder {
		/// <summary>
		/// Bitmap order not specified.
		/// </summary>
		None,
		/// <summary>
		/// Bitmap is ordered with pixels from LSB to MSB.
		/// </summary>
		_4321,
		/// <summary>
		/// Bitmap is ordered with pixels from MSB to LSB.
		/// </summary>
		_1234
	}

	/// <summary>
	/// Enumeration of SDL packed format orders.
	/// </summary>
	public enum SDLPackedOrder {
		/// <summary>
		/// Packed order not specified.
		/// </summary>
		None,
		/// <summary>
		/// Packed order is (ignored), red, green, blue from MSB to LSB.
		/// </summary>
		XRGB,
		/// <summary>
		/// Packed order is red, green, blue, (ignored) from MSB to LSB.
		/// </summary>
		RGBX,
		/// <summary>
		/// Packed order is alpha, red, green, blue from MSB to LSB.
		/// </summary>
		ARGB,
		/// <summary>
		/// Packed order is red, green, blue, alpha from MSB to LSB.
		/// </summary>
		RGBA,
		/// <summary>
		/// Packed order is (ignored), blue, green, red from MSB to LSB.
		/// </summary>
		XBGR,
		/// <summary>
		/// Packed order is blue, green, red, (ignored) from MSB to LSB.
		/// </summary>
		BGRX,
		/// <summary>
		/// Packed order is alpha, blue, green, red from MSB to LSB.
		/// </summary>
		ABGR,
		/// <summary>
		/// Packed order is blue, green, red, alpha from MSB to LSB.
		/// </summary>
		BGRA
	}

	/// <summary>
	/// Enumeration of SDL array format orders.
	/// </summary>
	public enum SDLArrayOrder {
		/// <summary>
		/// Array order not specified.
		/// </summary>
		None,
		/// <summary>
		/// Order of components is red, green, blue.
		/// </summary>
		RGB,
		/// <summary>
		/// Order of components is red, green, blue, alpha.
		/// </summary>
		RGBA,
		/// <summary>
		/// Order of components is alpha, red, green, blue.
		/// </summary>
		ARGB,
		/// <summary>
		/// Order of components is blue, green, red.
		/// </summary>
		BGR,
		/// <summary>
		/// Order of components is blue, green, red, alpha.
		/// </summary>
		BGRA,
		/// <summary>
		/// Order of components is alpha, blue, green, red.
		/// </summary>
		ABGR
	}

	/// <summary>
	/// Enumeration of SDL packed layouts.
	/// </summary>
	public enum SDLPackedLayout {
		/// <summary>
		/// Packed layout not specified.
		/// </summary>
		None,
		/// <summary>
		/// Components are 2x3-bit and 2-bit from MSB to LSB.
		/// </summary>
		_332,
		/// <summary>
		/// Components are 4x4-bit from MSB to LSB.
		/// </summary>
		_4444,
		/// <summary>
		/// Components are 1-bit and 3x5-bit from MSB to LSB.
		/// </summary>
		_1555,
		/// <summary>
		/// Components are 3x5-bit and 1-bit from MSB to LSB.
		/// </summary>
		_5551,
		/// <summary>
		/// Components are 5-bit, 6-bit, and 5-bit from MSB to LSB.
		/// </summary>
		_565,
		/// <summary>
		/// Components are 4x8-bit from MSB to LSB.
		/// </summary>
		_8888,
		/// <summary>
		/// Components are 2-bit and 3x10-bit from MSB to LSB.
		/// </summary>
		_2101010,
		/// <summary>
		/// Components are 3x10-bit and 2-bit from MSB to LSB.
		/// </summary>
		_1010102
	}

	public struct SDLPixelFormatEnum : IValuedEnum<uint> {

		public static uint DefinePixelFourCC(char a, char b, char c, char d) =>
			(uint)((a & 0xFF) | ((b & 0xFF) << 8) | ((c & 0xFF) << 16) | ((d & 0xFF) << 23));

		public static uint DefinePixelFormat(SDLPixelType type, SDLBitmapOrder order, uint bits, uint bytes) =>
			(1 << 28) | ((uint)type << 25) | ((uint)order << 20) | (bits << 8) | bytes;

		public static uint DefinePixelFormat(SDLPixelType type, SDLPackedOrder order, SDLPackedLayout layout, uint bits, uint bytes) =>
			(1 << 28) | ((uint)type << 25) | ((uint)order << 20) | ((uint)layout << 16) | (bits << 8) | bytes;

		public static uint DefinePixelFormat(SDLPixelType type, SDLArrayOrder order, uint bits, uint bytes) =>
			(1 << 28) | ((uint)type << 25) | ((uint)order << 20) | (bits << 8) | bytes;

		/// <summary>
		/// An unknown pixel format.
		/// </summary>
		public static readonly SDLPixelFormatEnum Unknown = 0;
		public static readonly SDLPixelFormatEnum Index1LSB = DefinePixelFormat(SDLPixelType.Index1, SDLBitmapOrder._4321, 1, 0);
		public static readonly SDLPixelFormatEnum Index1MSB = DefinePixelFormat(SDLPixelType.Index1, SDLBitmapOrder._1234, 1, 0);
		public static readonly SDLPixelFormatEnum Index4LSB = DefinePixelFormat(SDLPixelType.Index4, SDLBitmapOrder._4321, 4, 0);
		public static readonly SDLPixelFormatEnum Index4MSB = DefinePixelFormat(SDLPixelType.Index4, SDLBitmapOrder._1234, 4, 0);
		public static readonly SDLPixelFormatEnum Index8 = DefinePixelFormat(SDLPixelType.Index8, SDLBitmapOrder.None, 8, 1);

		public static readonly SDLPixelFormatEnum RGB332 = DefinePixelFormat(SDLPixelType.Packed8, SDLPackedOrder.XRGB, SDLPackedLayout._332, 8, 1);
		public static readonly SDLPixelFormatEnum XRGB4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XRGB, SDLPackedLayout._4444, 12, 2);
		public static readonly SDLPixelFormatEnum RGB444 = XRGB4444;
		public static readonly SDLPixelFormatEnum XBGR4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XBGR, SDLPackedLayout._4444, 12, 2);
		public static readonly SDLPixelFormatEnum BGR444 = XBGR4444;
		public static readonly SDLPixelFormatEnum XRGB1555 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XRGB, SDLPackedLayout._1555, 15, 2);
		public static readonly SDLPixelFormatEnum RGB555 = XRGB1555;
		public static readonly SDLPixelFormatEnum XBGR1555 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XBGR, SDLPackedLayout._1555, 15, 2);
		public static readonly SDLPixelFormatEnum BGR555 = XBGR1555;
		public static readonly SDLPixelFormatEnum ARGB4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.ARGB, SDLPackedLayout._4444, 16, 2);
		public static readonly SDLPixelFormatEnum RGBA4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.RGBA, SDLPackedLayout._4444, 16, 2);
		public static readonly SDLPixelFormatEnum ABGR4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.ABGR, SDLPackedLayout._4444, 16, 2);
		public static readonly SDLPixelFormatEnum BGRA4444 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.BGRA, SDLPackedLayout._4444, 16, 2);
		public static readonly SDLPixelFormatEnum ARGB1555 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.ARGB, SDLPackedLayout._1555, 16, 2);
		public static readonly SDLPixelFormatEnum RGBA5551 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.RGBA, SDLPackedLayout._5551, 16, 2);
		public static readonly SDLPixelFormatEnum ABGR1555 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.ABGR, SDLPackedLayout._1555, 16, 2);
		public static readonly SDLPixelFormatEnum BGRA5551 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.BGRA, SDLPackedLayout._5551, 16, 2);
		public static readonly SDLPixelFormatEnum RGB565 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XRGB, SDLPackedLayout._565, 16, 2);
		public static readonly SDLPixelFormatEnum BGR565 = DefinePixelFormat(SDLPixelType.Packed16, SDLPackedOrder.XBGR, SDLPackedLayout._565, 16, 2);
		public static readonly SDLPixelFormatEnum RGB24 = DefinePixelFormat(SDLPixelType.ArrayU8, SDLArrayOrder.RGB, 24, 3);
		public static readonly SDLPixelFormatEnum BGR24 = DefinePixelFormat(SDLPixelType.ArrayU8, SDLArrayOrder.BGR, 24, 3);
		public static readonly SDLPixelFormatEnum XRGB8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.XRGB, SDLPackedLayout._8888, 24, 4);
		public static readonly SDLPixelFormatEnum RGB888 = XRGB8888;
		public static readonly SDLPixelFormatEnum RGBX8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.RGBX, SDLPackedLayout._8888, 24, 4);
		public static readonly SDLPixelFormatEnum XBGR8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.XBGR, SDLPackedLayout._8888, 24, 4);
		public static readonly SDLPixelFormatEnum BGR888 = XBGR8888;
		public static readonly SDLPixelFormatEnum BGRX8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.BGRX, SDLPackedLayout._8888, 24, 4);
		public static readonly SDLPixelFormatEnum ARGB8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.ARGB, SDLPackedLayout._8888, 32, 4);
		public static readonly SDLPixelFormatEnum RGBA8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.RGBA, SDLPackedLayout._8888, 32, 4);
		public static readonly SDLPixelFormatEnum ABGR8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.ABGR, SDLPackedLayout._8888, 32, 3);
		public static readonly SDLPixelFormatEnum BGRA8888 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.BGRA, SDLPackedLayout._8888, 32, 4);
		public static readonly SDLPixelFormatEnum ARGB2101010 = DefinePixelFormat(SDLPixelType.Packed32, SDLPackedOrder.ARGB, SDLPackedLayout._2101010, 32, 4);

		public static readonly SDLPixelFormatEnum RGBA32 = BitConverter.IsLittleEndian ? ABGR8888 : RGBA8888;
		public static readonly SDLPixelFormatEnum ARGB32 = BitConverter.IsLittleEndian ? BGRA8888 : ARGB8888;
		public static readonly SDLPixelFormatEnum BGRA32 = BitConverter.IsLittleEndian ? ARGB8888 : BGRA8888;
		public static readonly SDLPixelFormatEnum ABGR32 = BitConverter.IsLittleEndian ? RGBA8888 : ABGR8888;

		/// <summary>
		/// Planar mode: Y + V + U (3 planes)
		/// </summary>
		public static readonly SDLPixelFormatEnum YV12 = DefinePixelFourCC('Y', 'V', '1', '2');
		/// <summary>
		/// Planar mode: Y + U + V (3 planes)
		/// </summary>
		public static readonly SDLPixelFormatEnum IYUV = DefinePixelFourCC('I', 'Y', 'U', 'V');
		/// <summary>
		/// Packed mode: Y0 + U0 + Y1 + V0 (1 plane)
		/// </summary>
		public static readonly SDLPixelFormatEnum YUY2 = DefinePixelFourCC('Y', 'U', 'Y', '2');
		/// <summary>
		/// Packed mode: U0 + Y0 + V0 + Y1 (1 plane)
		/// </summary>
		public static readonly SDLPixelFormatEnum UYVY = DefinePixelFourCC('U', 'Y', 'V', 'Y');
		/// <summary>
		/// Packed mode: Y0 + V0 + Y1 + U0 (1 plane)
		/// </summary>
		public static readonly SDLPixelFormatEnum YVYU = DefinePixelFourCC('Y', 'V', 'Y', 'U');
		/// <summary>
		/// Planar mode: Y + U/V interleaved (2 planes)
		/// </summary>
		public static readonly SDLPixelFormatEnum NV12 = DefinePixelFourCC('N', 'V', '1', '2');
		/// <summary>
		/// Planar mode: Y + V/U interleaved
		/// </summary>
		public static readonly SDLPixelFormatEnum NV21 = DefinePixelFourCC('N', 'V', '2', '1');
		/// <summary>
		/// Android video texture format.
		/// </summary>
		public static readonly SDLPixelFormatEnum ExternalOES = DefinePixelFourCC('O', 'E', 'S', ' ');

		public static SDLPixelFormatEnum FromMasks(int bpp, uint rmask, uint gmask, uint bmask, uint amask) => SDL2.Functions.SDL_MasksToPixelFormatEnum(bpp, rmask, gmask, bmask, amask);

		public uint Value { get; }

		/// <summary>
		/// If the pixel format is a special format defined by a four-character code.
		/// </summary>
		public bool IsFourCC => Value != 0 && ((Value >> 28) & 0xF) != 1;
		/// <summary>
		/// If the pixel format uses indexed color.
		/// </summary>
		public bool IsIndexed => PixelType switch {
			SDLPixelType.Index1 or SDLPixelType.Index4 or SDLPixelType.Index8 => true,
			_ => false
		};
		/// <summary>
		/// If the pixel format's components are stored in packed bitfields.
		/// </summary>
		public bool IsPacked => PixelType switch {
			SDLPixelType.Packed8 or SDLPixelType.Packed16 or SDLPixelType.Packed32 => true,
			_ => false
		};
		/// <summary>
		/// If the pixel format's components are stored in an array.
		/// </summary>
		public bool IsArray => PixelType switch {
			SDLPixelType.ArrayU8 or SDLPixelType.ArrayU16 or SDLPixelType.ArrayU32 or SDLPixelType.ArrayF16 or SDLPixelType.ArrayF32 => true,
			_ => false
		};
		/// <summary>
		/// If the pixel format supports alpha values.
		/// </summary>
		public bool IsAlpha => (IsPacked && PackedPixelOrder switch {
			SDLPackedOrder.ARGB or SDLPackedOrder.RGBA or SDLPackedOrder.ABGR or SDLPackedOrder.BGRA => true,
			_ => false
		}) || (IsArray && ArrayPixelOrder switch {
			SDLArrayOrder.ARGB or SDLArrayOrder.RGBA or SDLArrayOrder.ABGR or SDLArrayOrder.BGRA => true,
			_ => false
		});

		/// <summary>
		/// The type of pixels in the pixel format.
		/// </summary>
		public SDLPixelType PixelType => (SDLPixelType)((Value >> 24) & 0xF);
		/// <summary>
		/// The packed order of the format.
		/// </summary>
		public SDLPackedOrder PackedPixelOrder => (SDLPackedOrder)((Value >> 20) & 0xF);
		/// <summary>
		/// The bitmap order of the format.
		/// </summary>
		public SDLBitmapOrder BitmapPixelOrder => (SDLBitmapOrder)((Value >> 20) & 0xF);
		/// <summary>
		/// The array order of the format.
		/// </summary>
		public SDLArrayOrder ArrayPixelOrder => (SDLArrayOrder)((Value >> 20) & 0xF);
		/// <summary>
		/// The packed layout of the format.
		/// </summary>
		public SDLPackedLayout PixelLayout => (SDLPackedLayout)((Value >> 16) & 0xF);
		/// <summary>
		/// The number of bits per pixel.
		/// </summary>
		public uint BitsPerPixel => (Value >> 8) & 0xFF;
		/// <summary>
		/// The number of bytes per pixel.
		/// </summary>
		public int BytesPerPixel => IsFourCC ? (this == YUY2 || this == UYVY || this == YVYU ? 2 : 1) : (int)(Value & 0xFF);

		/// <summary>
		/// The name of the pixel format.
		/// </summary>
		public string Name => MemoryUtil.GetASCII(SDL2.Functions.SDL_GetPixelFormatName(Value));

		public SDLPixelFormatEnum(uint value) {
			Value = value;
		}

		public bool ToMasks(out int bpp, out uint rmask, out uint gmask, out uint bmask, out uint amask) => SDL2.Functions.SDL_PixelFormatEnumToMasks(Value, out bpp, out rmask, out gmask, out bmask, out amask);

		public bool Equals(IValuedEnum<uint> e) => Value == e.Value;

		public override bool Equals(object obj) {
			if (obj is SDLPixelFormatEnum format) return Equals(format);
			else return false;
		}

		public override int GetHashCode() => (int)Value;

		public static implicit operator SDLPixelFormatEnum(uint value) => new(value);

		public static implicit operator uint(SDLPixelFormatEnum format) => format.Value;

		public static bool operator ==(SDLPixelFormatEnum left, SDLPixelFormatEnum right) => left.Equals(right);

		public static bool operator !=(SDLPixelFormatEnum left, SDLPixelFormatEnum right) => !left.Equals(right);

	}

	/// <summary>
	/// Stores a single SDL color value.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLColor {

		/// <summary>
		/// Red component.
		/// </summary>
		public byte R;
		/// <summary>
		/// Green component.
		/// </summary>
		public byte G;
		/// <summary>
		/// Blue component.
		/// </summary>
		public byte B;
		/// <summary>
		/// Alpha component.
		/// </summary>
		public byte A;

	}

	public class SDLPalette : IDisposable {

		public IPointer<SDL_Palette> Palette { get; private set; }

		public SDLPalette(IPointer<SDL_Palette> pointer) {
			Palette = pointer;
		}

		public SDLPalette(int ncolors) {
			Palette = new UnmanagedPointer<SDL_Palette>(SDL2.Functions.SDL_AllocPalette(ncolors));
		}

		public int NColors {
			get {
				unsafe {
					return ((SDL_Palette*)Palette.Ptr)->NColors;
				}
			}
		}

		public ReadOnlySpan<SDLColor> Colors {
			get {
				unsafe {
					return ((SDL_Palette*)Palette.Ptr)->Colors;
				}
			}
		}

		public SDLColor this[int index] => Colors[index];

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Palette != null && !Palette.IsNull) {
				SDL2.Functions.SDL_FreePalette(Palette.Ptr);
				Palette = null;
			}
		}

		public void SetColors(in ReadOnlySpan<SDLColor> colors, int first = 0) {
			unsafe {
				fixed(SDLColor* pColors = colors) {
					SDL2.Functions.SDL_SetPaletteColors(Palette.Ptr, (IntPtr)pColors, first, colors.Length);
				}
			}
		}

		public void SetColors(int first, params SDLColor[] colors) {
			unsafe {
				fixed (SDLColor* pColors = colors) {
					SDL2.Functions.SDL_SetPaletteColors(Palette.Ptr, (IntPtr)pColors, first, colors.Length);
				}
			}
		}

	}

	public class SDLPixelFormat : IDisposable {

		public IPointer<SDL_PixelFormat> PixelFormat { get; private set; }

		public SDLPixelFormat(IPointer<SDL_PixelFormat> ptr) {
			PixelFormat = ptr;
		}

		public SDLPixelFormat(SDLPixelFormatEnum format) {
			PixelFormat = new UnmanagedPointer<SDL_PixelFormat>(SDL2.Functions.SDL_AllocFormat(format));
		}

		public uint Format {
			get {
				unsafe {
					return ((SDL_PixelFormat*)PixelFormat.Ptr)->Format;
				}
			}
		}

		public SDLPalette Palette {
			get {
				unsafe {
					return new(((SDL_PixelFormat*)PixelFormat.Ptr)->Palette);
				}
			}
			set => SDL2.CheckError(SDL2.Functions.SDL_SetPixelFormatPalette(PixelFormat.Ptr, value.Palette.Ptr));
		}

		public uint MapRGB(byte r, byte g, byte b) => SDL2.Functions.SDL_MapRGB(PixelFormat.Ptr, r, g, b);

		public uint MapRGBA(byte r, byte g, byte b, byte a) => SDL2.Functions.SDL_MapRGBA(PixelFormat.Ptr, r, g, b, a);

		public uint MapColor(IReadOnlyColor color) {
			if (color is Color4b c4b) return MapRGBA(c4b.R, c4b.G, c4b.B, c4b.A);
			else if (color is Color3b c3b) return MapRGB(c3b.R, c3b.G, c3b.B);
			else {
				Vector4 norm = color.Normalized;
				return MapRGBA((byte)(255 * norm.X), (byte)(255 * norm.Y), (byte)(255 * norm.Z), (byte)(255 * norm.W));
			}
		}

		public Color3b GetRGB(uint color) {
			Color3b c = new();
			SDL2.Functions.SDL_GetRGB(color, PixelFormat.Ptr, out c.R, out c.G, out c.B);
			return c;
		}

		public Color4b GetRGBA(uint color) {
			Color4b c = new();
			SDL2.Functions.SDL_GetRGBA(color, PixelFormat.Ptr, out c.R, out c.G, out c.B, out c.A);
			return c;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (PixelFormat != null && !PixelFormat.IsNull) {
				SDL2.Functions.SDL_FreeFormat(PixelFormat.Ptr);
				PixelFormat = null;
			}
		}

	}

}
