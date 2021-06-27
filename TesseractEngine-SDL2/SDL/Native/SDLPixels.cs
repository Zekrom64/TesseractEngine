using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Util;

namespace Tesseract.SDL.Native {

	/// <summary>
	/// Stores a SDL color palette.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Palette {

		private readonly int ncolors;
		private readonly IntPtr colors;
		/// <summary>
		/// The palette version.
		/// </summary>
		public uint Version;
		/// <summary>
		/// Reference count of palette.
		/// </summary>
		public int RefCount;

		/// <summary>
		/// The array of colors in the palette.
		/// </summary>
		public SDLColor[] Colors {
			get {
				SDLColor[] colors = new SDLColor[ncolors];
				unsafe {
					fixed(SDLColor* pColors = colors) {
						long length = ncolors * sizeof(SDLColor);
						Buffer.MemoryCopy(this.colors.ToPointer(), pColors, length, length);
					}
				}
				return colors;
			}
			/*
			set {
				unsafe {
					fixed(SDLColor* pColors = value) {
						long length = Math.Min(ncolors, value.Length) * sizeof(SDLColor);
						Buffer.MemoryCopy(pColors, colors.ToPointer(), ncolors * sizeof(SDLColor), length);
					}
				}
			}
			*/
		}

		public SDLColor this[int index] {
			get {
				if (index < 0 || index >= ncolors) throw new IndexOutOfRangeException();
				unsafe {
					return ((SDLColor*)colors.ToPointer())[index];
				}
			}
			/*
			set {
				if (index < 0 || index >= ncolors) throw new IndexOutOfRangeException();
				unsafe {
					((SDLColor*)colors.ToPointer())[index] = value;
				}
			}
			*/
		}

	}

	/// <summary>
	/// Stores pixel format information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_PixelFormat {

		/// <summary>
		/// The enumeration value of the format.
		/// </summary>
		public uint Format;
		private readonly IntPtr palette;
		/// <summary>
		/// The number of bits per pixel in the format.
		/// </summary>
		public byte BitsPerPixel;
		/// <summary>
		/// The number of bytes per pixel in the format.
		/// </summary>
		public byte BytesPerPixel;
		private readonly byte padding0;
		private readonly byte padding1;
		/// <summary>
		/// Bitmask of red component in a pixel value.
		/// </summary>
		public uint RMask;
		/// <summary>
		/// Bitmask of green component in a pixel vlaue.
		/// </summary>
		public uint GMask;
		/// <summary>
		/// Bitmask of blue component in a pixel value.
		/// </summary>
		public uint BMask;
		/// <summary>
		/// Bitmask of alpha component in a pixel value.
		/// </summary>
		public uint AMask;
		public byte RLoss;
		public byte GLoss;
		public byte BLoss;
		public byte ALoss;
		public byte RShift;
		public byte GShift;
		public byte BShift;
		public byte AShift;
		public int RefCount;
		private readonly IntPtr next;

		/// <summary>
		/// Gets the color palette of the format, if it exists.
		/// </summary>
		public SDL_Palette? Palette => palette == IntPtr.Zero ? null : Marshal.PtrToStructure<SDL_Palette>(palette);
		/// <summary>
		/// Gets the next pixel format in the chain, if it exists.
		/// </summary>
		public SDL_PixelFormat? Next => next == IntPtr.Zero ? null : Marshal.PtrToStructure<SDL_PixelFormat>(next);

	}

}
