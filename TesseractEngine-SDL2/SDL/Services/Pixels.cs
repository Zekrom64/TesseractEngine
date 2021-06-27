using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;

namespace Tesseract.SDL.Services {
	public static class SDLPixelService {

		private static readonly Dictionary<SDLPixelFormatEnum, PixelFormat> fromSDL = new() {
			{ SDLPixelFormatEnum.Unknown , null },
			{ SDLPixelFormatEnum.RGB24, PixelFormat.R8G8B8UNorm },
			{ SDLPixelFormatEnum.BGR24, PixelFormat.B8G8R8UNorm },
			{ SDLPixelFormatEnum.RGBA32, PixelFormat.R8G8B8A8UNorm },
			{ SDLPixelFormatEnum.BGRA32, PixelFormat.B8G8R8A8UNorm },
			{ SDLPixelFormatEnum.ABGR32, PixelFormat.A8B8G8R8UNorm }
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
		/// <returns>Converted pixel format, or null</returns>
		public static SDLPixelFormatEnum ConvertPixelFormat(PixelFormat pixelFormat) => toSDL.GetValueOrDefault(pixelFormat, SDLPixelFormatEnum.Unknown);

	}
}
