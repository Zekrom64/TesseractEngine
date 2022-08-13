using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.SDL.Native {

	/// <summary>
	/// Stores a SDL color palette.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Palette {

		public readonly int NColors;
		private readonly IntPtr colors;
		/// <summary>
		/// The palette version. Incrementally tracks modifications to the palette's colors.
		/// </summary>
		public readonly uint Version;
		/// <summary>
		/// Reference count of palette.
		/// </summary>
		public readonly int RefCount;

		/// <summary>
		/// The array of colors in the palette.
		/// </summary>
		public ReadOnlySpan<SDLColor> Colors {
			get {
				unsafe {
					return new ReadOnlySpan<SDLColor>((void*)colors, NColors);
				}
			}
		}

		public SDLColor this[int index] => Colors[index];

	}

	/// <summary>
	/// Stores pixel format information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_PixelFormat {

		/// <summary>
		/// The enumeration value of the format.
		/// </summary>
		public readonly uint Format;
		private readonly IntPtr palette;
		/// <summary>
		/// The number of bits per pixel in the format.
		/// </summary>
		public readonly byte BitsPerPixel;
		/// <summary>
		/// The number of bytes per pixel in the format.
		/// </summary>
		public readonly byte BytesPerPixel;
		private readonly byte padding0;
		private readonly byte padding1;
		/// <summary>
		/// Bitmask of red component in a pixel value.
		/// </summary>
		public readonly uint RMask;
		/// <summary>
		/// Bitmask of green component in a pixel vlaue.
		/// </summary>
		public readonly uint GMask;
		/// <summary>
		/// Bitmask of blue component in a pixel value.
		/// </summary>
		public readonly uint BMask;
		/// <summary>
		/// Bitmask of alpha component in a pixel value.
		/// </summary>
		public readonly uint AMask;
		public readonly byte RLoss;
		public readonly byte GLoss;
		public readonly byte BLoss;
		public readonly byte ALoss;
		public readonly byte RShift;
		public readonly byte GShift;
		public readonly byte BShift;
		public readonly byte AShift;
		public readonly int RefCount;
		private readonly IntPtr next;

		/// <summary>
		/// Gets the pointer to the color palette of the format.
		/// </summary>
		public IPointer<SDL_Palette> Palette => new UnmanagedPointer<SDL_Palette>(palette);
		/// <summary>
		/// Gets the pointer to the next pixel format in the chain.
		/// </summary>
		public IPointer<SDL_PixelFormat> Next => new UnmanagedPointer<SDL_PixelFormat>(next);

		/// <summary>
		/// Gets the color palette of the format, if it exists.
		/// </summary>
		public SDL_Palette? PaletteVal => palette == IntPtr.Zero ? null : Marshal.PtrToStructure<SDL_Palette>(palette);
		/// <summary>
		/// Gets the next pixel format in the chain, if it exists.
		/// </summary>
		public SDL_PixelFormat? NextFormat => next == IntPtr.Zero ? null : Marshal.PtrToStructure<SDL_PixelFormat>(next);

	}

}
