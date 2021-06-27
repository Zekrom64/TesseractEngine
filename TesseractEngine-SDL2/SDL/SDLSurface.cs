using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	/// <summary>
	/// Enumeration of SDL surface flags.
	/// </summary>
	public enum SDLSurfaceFlags : uint {
		/// <summary>
		/// Surface uses preallocated memory.
		/// </summary>
		Prealloc = 1,
		/// <summary>
		/// Surface is RLE encoded.
		/// </summary>
		RLEAccel = 2,
		/// <summary>
		/// Surface is referenced internally.
		/// </summary>
		DontFree = 4,
		/// <summary>
		/// Surface uses aligned memory.
		/// </summary>
		SIMDAligned = 8
	}

	/// <summary>
	/// Conversion mode between YUV and RGB color.
	/// </summary>
	public enum SDLYUVConversionMode {
		/// <summary>
		/// Full range JPEG.
		/// </summary>
		JPEG,
		/// <summary>
		/// BT.601
		/// </summary>
		BT601,
		/// <summary>
		/// BT.706
		/// </summary>
		BT709,
		/// <summary>
		/// BT.601 for SD content, BT.709 for HD content.
		/// </summary>
		Automatic
	}

	public class SDLSurface : IDisposable {

		public IPointer<SDL_Surface> Surface { get; private set; }

		public SDLSurface(IPointer<SDL_Surface> pointer) {
			Surface = pointer;
		}

		public SDLSurface(int width, int height, SDLPixelFormatEnum format, IntPtr? pixels = null, int pitch = 0) {
			if (pixels.HasValue) {
				if (pitch == 0) pitch = width * format.BytesPerPixel;
				Surface = new UnmanagedPointer<SDL_Surface>(SDL2.Functions.SDL_CreateRGBSurfaceWithFormatFrom(pixels.Value, width, height, (int)format.BitsPerPixel, pitch, format));
			} else {
				Surface = new UnmanagedPointer<SDL_Surface>(SDL2.Functions.SDL_CreateRGBSurfaceWithFormat(0, width, height, (int)format.BitsPerPixel, format));
			}
		}

		/// <summary>
		/// The width of the surface.
		/// </summary>
		public int W => Surface.Value.W;

		/// <summary>
		/// The height of the surface.
		/// </summary>
		public int H => Surface.Value.H;

		/// <summary>
		/// The pixel format of the surface.
		/// </summary>
		public SDL_PixelFormat PixelFormat => Surface.Value.Format;

		/// <summary>
		/// The pixel format enumeration value of the surface.
		/// </summary>
		public SDLPixelFormatEnum PixelFormatEnum => PixelFormat.Format;

		/// <summary>
		/// The palette of the surface.
		/// </summary>
		public SDLPalette Palette {
			set => SDL2.Functions.SDL_SetSurfacePalette(Surface.Ptr, value.Palette.Ptr);
		}

		/// <summary>
		/// The run length encoding hint of the surface.
		/// </summary>
		public bool RLEHint {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceRLE(Surface.Ptr, value ? 1 : 0));
		}

		/// <summary>
		/// The color key of the surface.
		/// </summary>
		public uint? ColorKey {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetColorKey(Surface.Ptr, value.HasValue ? 1 : 0, value ?? 0));
			get {
				if (!SDL2.Functions.SDL_HasColorKey(Surface.Ptr)) return null;
				SDL2.CheckError(SDL2.Functions.SDL_GetColorKey(Surface.Ptr, out uint key));
				return key;
			}
		}

		/// <summary>
		/// The modulating color of the surface.
		/// </summary>
		public Color3b ColorMod {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceColorMod(Surface.Ptr, value.R, value.G, value.B));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceColorMod(Surface.Ptr, out byte R, out byte G, out byte B));
				return new(R, G, B);
			}
		}

		/// <summary>
		/// The modulating alpha of the surface.
		/// </summary>
		public byte AlphaMod {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceAlphaMod(Surface.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceAlphaMod(Surface.Ptr, out byte alpha));
				return alpha;
			}
		}

		/// <summary>
		/// The blend mode to use for blitting operations.
		/// </summary>
		public SDLBlendMode BlendMode {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceBlendMode(Surface.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceBlendMode(Surface.Ptr, out SDLBlendMode blendMode));
				return blendMode;
			}
		}

		/// <summary>
		/// The clipping rectangle for blitting operations.
		/// </summary>
		public SDLRect? ClipRect {
			set {
				if (!value.HasValue) SDL2.CheckError(SDL2.Functions.SDL_SetClipRect(Surface.Ptr, IntPtr.Zero));
				else {
					unsafe {
						SDLRect rect = value.Value;
						SDL2.CheckError(SDL2.Functions.SDL_SetClipRect(Surface.Ptr, (IntPtr)(&rect)));
					}
				}
			}
			get {
				SDL2.Functions.SDL_GetClipRect(Surface.Ptr, out SDLRect rect);
				return rect;
			}
		}

		/// <summary>
		/// If the surface must be locked to access its pixel data.
		/// </summary>
		/// <seealso cref="Lock"/>
		/// <seealso cref="Unlock"/>
		public bool MustLock => Surface.Value.MustLock;

		/// <summary>
		/// The raw pixel data managed by the surface. This may only be usable
		/// once <see cref="Lock"/> has been called if <see cref="MustLock"/> is true.
		/// </summary>
		/// <seealso cref="Lock"/>
		public IntPtr Pixels => Surface.Value.Pixels;

		/// <summary>
		/// <para>Locks the pixels of this surface for direct access.</para>
		/// <para>
		/// Surfaces may use run length encoding to compress image data, but for
		/// direct pixel access this must be unpacked before access and repacked
		/// after access. Locking may only be necessary to unpack this data
		/// if <see cref="MustLock"/> is true.
		/// </para>
		/// </summary>
		/// <returns>Locked pixel data</returns>
		/// <seealso cref="MustLock"/>
		/// <seealso cref="Pixels"/>
		/// <seealso cref="Unlock"/>
		public IntPtr Lock() {
			SDL2.CheckError(SDL2.Functions.SDL_LockSurface(Surface.Ptr));
			return Surface.Value.Pixels;
		}

		/// <summary>
		/// Unlocks the pixels of this surface after calling <see cref="Lock"/>.
		/// </summary>
		/// <seealso cref="Lock"/>
		public void Unlock() => SDL2.Functions.SDL_UnlockSurface(Surface.Ptr);

		/// <summary>
		/// Creates a new duplicate of this surface.
		/// </summary>
		/// <returns>Duplicate of surface</returns>
		public SDLSurface Duplicate() {
			IntPtr ptr = SDL2.Functions.SDL_DuplicateSurface(Surface.Ptr);
			if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new(new UnmanagedPointer<SDL_Surface>(ptr));
		}

		/// <summary>
		/// Converts this surface to a new surface of a different pixel format.
		/// </summary>
		/// <param name="pixelFormat">New pixel format</param>
		/// <param name="flags">Flags to create surface with</param>
		/// <returns>Copy of surface with new format</returns>
		public SDLSurface Convert(SDLPixelFormatEnum pixelFormat, SDLSurfaceFlags flags = 0) {
			IntPtr ptr = SDL2.Functions.SDL_ConvertSurfaceFormat(Surface.Ptr, pixelFormat, (uint)flags);
			if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new(new UnmanagedPointer<SDL_Surface>(ptr));
		}

		/// <summary>
		/// Fills this surface with a single color.
		/// </summary>
		/// <param name="color">Color value to fill surface with</param>
		/// <seealso cref="FillRect(SDLRect, uint)"/>
		/// <seealso cref="FillRects(Span{SDLRect}, uint)"/>
		/// <seealso cref="FillRects(uint, SDLRect[])"/>
		public void Fill(uint color) {
			SDL2.CheckError(SDL2.Functions.SDL_FillRect(Surface.Ptr, IntPtr.Zero, color));
		}

		/// <summary>
		/// Fills an area of this surface with a single color.
		/// </summary>
		/// <param name="rect">Area to fill</param>
		/// <param name="color">Color value to fill area with</param>
		/// <seealso cref="Fill(uint)"/>
		/// <seealso cref="FillRects(Span{SDLRect}, uint)"/>
		/// <seealso cref="FillRects(uint, SDLRect[])"/>
		public void FillRect(SDLRect rect, uint color) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_FillRect(Surface.Ptr, (IntPtr)(&rect), color));
			}
		}

		/// <summary>
		/// Fills areas of this surface with a single color.
		/// </summary>
		/// <param name="color">Color value to fill areas with</param>
		/// <param name="rects">Areas to fill</param>
		/// <seealso cref="Fill(uint)"/>
		/// <seealso cref="FillRect(SDLRect, uint)"/>
		/// <seealso cref="FillRects(Span{SDLRect}, uint)"/>
		public void FillRects(uint color, params SDLRect[] rects) {
			unsafe {
				fixed (SDLRect* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_FillRects(Surface.Ptr, (IntPtr)pRects, rects.Length, color));
				}
			}
		}

		/// <summary>
		/// Fills areas of this surface with a single color.
		/// </summary>
		/// <param name="rects">Areas to fill</param>
		/// <param name="color">Color value to fill areas with</param>
		/// <seealso cref="Fill(uint)"/>
		/// <seealso cref="FillRect(SDLRect, uint)"/>
		/// <seealso cref="FillRects(uint, SDLRect[])"/>
		public void FillRects(Span<SDLRect> rects, uint color) {
			unsafe {
				fixed (SDLRect* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_FillRects(Surface.Ptr, (IntPtr)pRects, rects.Length, color));
				}
			}
		}

		/// <summary>
		/// Copies a region of another image to this image.
		/// </summary>
		/// <param name="dstRect">Destination blit area, or null to use whole area</param>
		/// <param name="src">Source surface</param>
		/// <param name="srcRect">Source blit area, or null to use whole area</param>
		public void Blit(SDLRect? dstRect, SDLSurface src, SDLRect? srcRect) {
			unsafe {
				SDLRect dstr, srcr;
				if (dstRect.HasValue) dstr = dstRect.Value;
				if (srcRect.HasValue) srcr = srcRect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_UpperBlit(src.Surface.Ptr, srcRect.HasValue ? (IntPtr)(&srcr) : IntPtr.Zero, Surface.Ptr, dstRect.HasValue ? (IntPtr)(&dstr) : IntPtr.Zero));
			}
		}

		/// <summary>
		/// Copies a region of another image to this image, performing a simple stretch if the areas are different sizes.
		/// </summary>
		/// <param name="dstRect">Destination blit area, or null to use whole area</param>
		/// <param name="src">Source surface</param>
		/// <param name="srcRect">Source blit area, or null to use whole area</param>
		public void SoftStretch(SDLRect? dstRect, SDLSurface src, SDLRect? srcRect) {
			unsafe {
				SDLRect dstr, srcr;
				if (dstRect.HasValue) dstr = dstRect.Value;
				if (srcRect.HasValue) srcr = srcRect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_UpperBlitScaled(src.Surface.Ptr, srcRect.HasValue ? (IntPtr)(&srcr) : IntPtr.Zero, Surface.Ptr, dstRect.HasValue ? (IntPtr)(&dstr) : IntPtr.Zero));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Surface != null && !Surface.IsNull) {
				SDL2.Functions.SDL_FreeSurface(Surface.Ptr);
				Surface = null;
			}
		}
	}

	public class SDLPalette : IDisposable {

		public IPointer<SDL_Palette> Palette { get; private set; }

		public SDLPalette(IPointer<SDL_Palette> pointer) {
			Palette = pointer;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Palette != null && !Palette.IsNull) {
				SDL2.Functions.SDL_FreePalette(Palette.Ptr);
				Palette = null;
			}
		}

	}

}
