using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

namespace Tesseract.SDL.Native {

	/// <summary>
	/// A collection of pixels used in software blitting.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Surface {

		/// <summary>
		/// The surface flags.
		/// </summary>
		public SDLSurfaceFlags Flags;
		[NativeType("SDL_PixelFormat*")]
		private readonly IntPtr format;
		/// <summary>
		/// The width of the surface.
		/// </summary>
		public readonly int W;
		/// <summary>
		/// The height of the surface.
		/// </summary>
		public readonly int H;
		/// <summary>
		/// The length of a row of pixels in bytes.
		/// </summary>
		public readonly int Pitch;
		/// <summary>
		/// Pointer to pixel data.
		/// </summary>
		public IntPtr Pixels;
		/// <summary>
		/// User-defined data assocated with surface.
		/// </summary>
		public IntPtr Userdata;
		private readonly int locked;
		private readonly IntPtr lockData;
		/// <summary>
		/// Clipping area to clip blits from.
		/// </summary>
		public readonly Recti ClipRect;
		private readonly IntPtr map;
		public int RefCount;

		public bool MustLock => (Flags & SDLSurfaceFlags.RLEAccel) != 0;

		/// <summary>
		/// The pixel format of the surface.
		/// </summary>
		public IPointer<SDL_PixelFormat> Format => new UnmanagedPointer<SDL_PixelFormat>(format);

	}

	public delegate int SDL_blit([NativeType("SDL_Surface*")] IntPtr src, [In] ref Recti srcrect, [NativeType("SDL_Surface*")] IntPtr dst, [In] ref Recti dstrect);

}
