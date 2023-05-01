using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
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

		public SDLSurface(int width, int height, SDLPixelFormatEnum format, IntPtr pixels = default, int pitch = 0) {
			unsafe {
				if (pixels != IntPtr.Zero) {
					if (pitch == 0) pitch = width * format.BytesPerPixel;
					Surface = new UnmanagedPointer<SDL_Surface>((IntPtr)SDL2.Functions.SDL_CreateRGBSurfaceWithFormatFrom(pixels, width, height, (int)format.BitsPerPixel, pitch, format));
				} else {
					Surface = new UnmanagedPointer<SDL_Surface>((IntPtr)SDL2.Functions.SDL_CreateRGBSurfaceWithFormat(0, width, height, (int)format.BitsPerPixel, format));
				}
			}
		}

		public SDLSurfaceFlags Flags {
			get {
				unsafe {
					return ((SDL_Surface*)Surface.Ptr)->Flags;
				}
			}
		}

		/// <summary>
		/// The width of the surface.
		/// </summary>
		public int W {
			get {
				unsafe {
					return ((SDL_Surface*)Surface.Ptr)->W;
				}
			}
		}

		/// <summary>
		/// The height of the surface.
		/// </summary>
		public int H {
			get {
				unsafe {
					return ((SDL_Surface*)Surface.Ptr)->H;
				}
			}
		}

		/// <summary>
		/// The pixel format of the surface.
		/// </summary>
		public SDLPixelFormat PixelFormat {
			get {
				unsafe {
					return new(((SDL_Surface*)Surface.Ptr)->Format);
				}
			}
		}

		/// <summary>
		/// The pixel format enumeration value of the surface.
		/// </summary>
		public SDLPixelFormatEnum PixelFormatEnum => PixelFormat.Format;

		/// <summary>
		/// The palette of the surface.
		/// </summary>
		public SDLPalette Palette {
			set {
				unsafe {
					SDL2.Functions.SDL_SetSurfacePalette((SDL_Surface*)Surface.Ptr, (SDL_Palette*)value.Palette.Ptr);
				}
			}

			get => PixelFormat.Palette;
		}

		/// <summary>
		/// The run length encoding hint of the surface.
		/// </summary>
		public bool RLEHint {
			get => (Flags & SDLSurfaceFlags.RLEAccel) != 0;
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceRLE((SDL_Surface*)Surface.Ptr, value));
				}
			}
		}

		/// <summary>
		/// The color key of the surface.
		/// </summary>
		public uint? ColorKey {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetColorKey((SDL_Surface*)Surface.Ptr, value.HasValue, value ?? 0));
				}
			}

			get {
				unsafe {
					if (!SDL2.Functions.SDL_HasColorKey((SDL_Surface*)Surface.Ptr)) return null;
					SDL2.CheckError(SDL2.Functions.SDL_GetColorKey((SDL_Surface*)Surface.Ptr, out uint key));
					return key;
				}
			}
		}

		/// <summary>
		/// The modulating color of the surface.
		/// </summary>
		public Vector3b ColorMod {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceColorMod((SDL_Surface*)Surface.Ptr, value.X, value.Y, value.Z));
				}
			}

			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceColorMod((SDL_Surface*)Surface.Ptr, out byte R, out byte G, out byte B));
					return new(R, G, B);
				}
			}
		}

		/// <summary>
		/// The modulating alpha of the surface.
		/// </summary>
		public byte AlphaMod {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceAlphaMod((SDL_Surface*)Surface.Ptr, value));
				}
			}

			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceAlphaMod((SDL_Surface*)Surface.Ptr, out byte alpha));
					return alpha;
				}
			}
		}

		/// <summary>
		/// The blend mode to use for blitting operations.
		/// </summary>
		public SDLBlendMode BlendMode {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetSurfaceBlendMode((SDL_Surface*)Surface.Ptr, value));
				}
			}

			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetSurfaceBlendMode((SDL_Surface*)Surface.Ptr, out SDLBlendMode blendMode));
					return blendMode;
				}
			}
		}

		/// <summary>
		/// The clipping rectangle for blitting operations.
		/// </summary>
		public Recti? ClipRect {
			set {
				unsafe {
					if (!value.HasValue) SDL2.CheckError(SDL2.Functions.SDL_SetClipRect((SDL_Surface*)Surface.Ptr, (Recti*)0));
					else {
						Recti rect = value.Value;
						SDL2.CheckError(SDL2.Functions.SDL_SetClipRect((SDL_Surface*)Surface.Ptr, &rect));
					}
				}
			}
			get {
				unsafe {
					SDL2.Functions.SDL_GetClipRect((SDL_Surface*)Surface.Ptr, out Recti rect);
					return rect;
				}
			}
		}

		/// <summary>
		/// If the surface must be locked to access its pixel data.
		/// </summary>
		/// <seealso cref="Lock"/>
		/// <seealso cref="Unlock"/>
		public bool MustLock {
			get {
				unsafe {
					return ((SDL_Surface*)Surface.Ptr)->MustLock;
				}
			}
		}

		/// <summary>
		/// The raw pixel data managed by the surface. This may only be usable
		/// once <see cref="Lock"/> has been called if <see cref="MustLock"/> is true.
		/// </summary>
		/// <seealso cref="Lock"/>
		public IntPtr Pixels {
			get {
				unsafe {
					return ((SDL_Surface*)Surface.Ptr)->Pixels;
				}
			}
		}

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
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_LockSurface((SDL_Surface*)Surface.Ptr));
				return Pixels;
			}
		}

		/// <summary>
		/// Unlocks the pixels of this surface after calling <see cref="Lock"/>.
		/// </summary>
		/// <seealso cref="Lock"/>
		public void Unlock() {
			unsafe {
				SDL2.Functions.SDL_UnlockSurface((SDL_Surface*)Surface.Ptr);
			}
		}

		/// <summary>
		/// Creates a new duplicate of this surface.
		/// </summary>
		/// <returns>Duplicate of surface</returns>
		public SDLSurface Duplicate() {
			unsafe {
				IntPtr ptr = (IntPtr)SDL2.Functions.SDL_DuplicateSurface((SDL_Surface*)Surface.Ptr);
				if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new(new UnmanagedPointer<SDL_Surface>(ptr));
			}
		}

		/// <summary>
		/// Converts this surface to a new surface of a different pixel format.
		/// </summary>
		/// <param name="pixelFormat">New pixel format</param>
		/// <param name="flags">Flags to create surface with</param>
		/// <returns>Copy of surface with new format</returns>
		public SDLSurface Convert(SDLPixelFormatEnum pixelFormat, SDLSurfaceFlags flags = 0) {
			unsafe {
				IntPtr ptr = (IntPtr)SDL2.Functions.SDL_ConvertSurfaceFormat((SDL_Surface*)Surface.Ptr, pixelFormat, (uint)flags);
				if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new(new UnmanagedPointer<SDL_Surface>(ptr));
			}
		}

		/// <summary>
		/// Fills this surface with a single color.
		/// </summary>
		/// <param name="color">Color value to fill surface with</param>
		/// <seealso cref="FillRect(SDLRect, uint)"/>
		/// <seealso cref="FillRects(Span{SDLRect}, uint)"/>
		/// <seealso cref="FillRects(uint, SDLRect[])"/>
		public void Fill(uint color) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_FillRect((SDL_Surface*)Surface.Ptr, (Recti*)0, color));
			}
		}

		/// <summary>
		/// Fills an area of this surface with a single color.
		/// </summary>
		/// <param name="rect">Area to fill</param>
		/// <param name="color">Color value to fill area with</param>
		/// <seealso cref="Fill(uint)"/>
		/// <seealso cref="FillRects(Span{SDLRect}, uint)"/>
		/// <seealso cref="FillRects(uint, SDLRect[])"/>
		public void FillRect(Recti rect, uint color) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_FillRect((SDL_Surface*)Surface.Ptr, &rect, color));
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
		public void FillRects(uint color, params Recti[] rects) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_FillRects((SDL_Surface*)Surface.Ptr, pRects, rects.Length, color));
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
		public void FillRects(Span<Recti> rects, uint color) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_FillRects((SDL_Surface*)Surface.Ptr, pRects, rects.Length, color));
				}
			}
		}

		/// <summary>
		/// Copies a region of another image to this image.
		/// </summary>
		/// <param name="dstRect">Destination blit area, or null to use whole area</param>
		/// <param name="src">Source surface</param>
		/// <param name="srcRect">Source blit area, or null to use whole area</param>
		public void Blit(Recti? dstRect, SDLSurface src, Recti? srcRect) {
			unsafe {
				Recti srcr = srcRect ?? new Recti(src.W, src.H);
				SDL2.CheckError(SDL2.Functions.SDL_UpperBlit((SDL_Surface*)src.Surface.Ptr, &srcr, (SDL_Surface*)Surface.Ptr, dstRect ?? new Recti(W, H)));
			}
		}

		/// <summary>
		/// Copies a region of another image to this image, performing a simple stretch if the areas are different sizes.
		/// </summary>
		/// <param name="dstRect">Destination blit area, or null to use whole area</param>
		/// <param name="src">Source surface</param>
		/// <param name="srcRect">Source blit area, or null to use whole area</param>
		public void BlitScaled(Recti? dstRect, SDLSurface src, Recti? srcRect) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_UpperBlitScaled((SDL_Surface*)src.Surface.Ptr, srcRect ?? new Recti(src.W, src.H), (SDL_Surface*)Surface.Ptr, dstRect ?? new Recti(W, H)));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Surface != null && !Surface.IsNull) {
				unsafe {
					SDL2.Functions.SDL_FreeSurface((SDL_Surface*)Surface.Ptr);
				}
				Surface = new NullPointer<SDL_Surface>();
			}
		}

		public SDLRenderer CreateSoftwareRenderer() {
			unsafe {
				IntPtr pRender = SDL2.Functions.SDL_CreateSoftwareRenderer((SDL_Surface*)Surface.Ptr);
				if (pRender == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLRenderer(pRender);
			}
		}
	}

}
