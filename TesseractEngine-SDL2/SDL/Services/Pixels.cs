using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;

namespace Tesseract.SDL.Services {

	/// <summary>
	/// The SDL pixel service is a support class for conversion between <see cref="PixelFormat"/> and
	/// <see cref="SDLPixelFormatEnum"/> types.
	/// </summary>
	public static class SDLPixelService {

		private static readonly Dictionary<SDLPixelFormatEnum, PixelFormat> fromSDL = new() {
			{ SDLPixelFormatEnum.Unknown, null },
			// Standard pixel formats
			{ SDLPixelFormatEnum.RGB24, PixelFormat.R8G8B8UNorm },
			{ SDLPixelFormatEnum.BGR24, PixelFormat.B8G8R8UNorm },
			{ SDLPixelFormatEnum.RGBA32, PixelFormat.R8G8B8A8UNorm },
			{ SDLPixelFormatEnum.BGRA32, PixelFormat.B8G8R8A8UNorm },
			{ SDLPixelFormatEnum.ABGR32, PixelFormat.A8B8G8R8UNorm },
			{ SDLPixelFormatEnum.ARGB32, PixelFormat.A8R8G8B8UNorm },
			// Uncommon pixel formats supported by SDL
			{ SDLPixelFormatEnum.RGB332, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 5, Size = 3, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 3, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.XRGB4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.XBGR4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.XRGB1555, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 10, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new	PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new	PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm}
			) },
			{ SDLPixelFormatEnum.XBGR1555, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 10, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.ARGB4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new	PixelChannel() { Type = ChannelType.Red, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.RGBA4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.ABGR4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.BGRA4444, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Blue, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.ARGB1555, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 15, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 10, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.RGBA5551, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 6, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 1, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.ABGR1555, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 15, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 10, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.BGRA5551, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Blue, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 6, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 1, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.RGB565, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Red, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 6, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.BGR565, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Blue, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 6, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.XRGB8888, PixelFormat.DefinePackedFormat(4,
				new PixelChannel() { Type = ChannelType.Red, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.RGBX8888, PixelFormat.DefinePackedFormat(4,
				new PixelChannel() { Type = ChannelType.Red, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.BGRX8888, PixelFormat.DefinePackedFormat(4,
				new PixelChannel() { Type = ChannelType.Blue, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) },
			{ SDLPixelFormatEnum.ARGB2101010, PixelFormat.DefinePackedFormat(
				new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
				new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm }
			) }
		};

		private static readonly Dictionary<PixelFormat, SDLPixelFormatEnum> toSDL = fromSDL.ToDictionary(item => item.Value, item => item.Key);

		/// <summary>
		/// Converts an SDL pixel format to a standard pixel format.
		/// </summary>
		/// <param name="pixelFormat">Pixel format to convert</param>
		/// <returns>Converted pixel format, or null</returns>
		public static PixelFormat ConvertPixelFormat(SDLPixelFormatEnum pixelFormat) => fromSDL.GetValueOrDefault(pixelFormat);

		/// <summary>
		/// Converts a standard pixel format to an SDL pixel format.
		/// </summary>
		/// <param name="pixelFormat">Pixel format to convert</param>
		/// <returns>Converted pixel format, or <see cref="SDLPixelFormatEnum.Unknown"/></returns>
		public static SDLPixelFormatEnum ConvertPixelFormat(PixelFormat pixelFormat) => toSDL.GetValueOrDefault(pixelFormat, SDLPixelFormatEnum.Unknown);

	}
}
