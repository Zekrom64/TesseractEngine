using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Graphics;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {
	
	public enum SDLRendererFlags : uint {
		Software = 0x1,
		Accelerated = 0x2,
		PresentVSync = 0x4,
		TargetTexture = 0x8
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLRendererInfo {

		public string Name;

		public SDLRendererFlags Flags;

		public uint NumTextureFormats;

		public SDLPixelFormatEnum TextureFormat0;
		public SDLPixelFormatEnum TextureFormat1;
		public SDLPixelFormatEnum TextureFormat2;
		public SDLPixelFormatEnum TextureFormat3;
		public SDLPixelFormatEnum TextureFormat4;
		public SDLPixelFormatEnum TextureFormat5;
		public SDLPixelFormatEnum TextureFormat6;
		public SDLPixelFormatEnum TextureFormat7;
		public SDLPixelFormatEnum TextureFormat8;
		public SDLPixelFormatEnum TextureFormat9;
		public SDLPixelFormatEnum TextureFormat10;
		public SDLPixelFormatEnum TextureFormat11;
		public SDLPixelFormatEnum TextureFormat12;
		public SDLPixelFormatEnum TextureFormat13;
		public SDLPixelFormatEnum TextureFormat14;
		public SDLPixelFormatEnum TextureFormat15;

		public SDLPixelFormatEnum this[int index] {
			get => index switch {
				0 => TextureFormat0,
				1 => TextureFormat1,
				2 => TextureFormat2,
				3 => TextureFormat3,
				4 => TextureFormat4,
				5 => TextureFormat5,
				6 => TextureFormat6,
				7 => TextureFormat7,
				8 => TextureFormat8,
				9 => TextureFormat9,
				10 => TextureFormat10,
				11 => TextureFormat11,
				12 => TextureFormat12,
				13 => TextureFormat13,
				14 => TextureFormat14,
				15 => TextureFormat15,
				_ => throw new IndexOutOfRangeException()
			};
			set {
				switch(index) {
					case 0: TextureFormat0 = value; break;
					case 1: TextureFormat1 = value; break;
					case 2: TextureFormat2 = value; break;
					case 3: TextureFormat3 = value; break;
					case 4: TextureFormat4 = value; break;
					case 5: TextureFormat5 = value; break;
					case 6: TextureFormat6 = value; break;
					case 7: TextureFormat7 = value; break;
					case 8: TextureFormat8 = value; break;
					case 9: TextureFormat9 = value; break;
					case 10: TextureFormat10 = value; break;
					case 11: TextureFormat11 = value; break;
					case 12: TextureFormat12 = value; break;
					case 13: TextureFormat13 = value; break;
					case 14: TextureFormat14 = value; break;
					case 15: TextureFormat15 = value; break;
					default: throw new IndexOutOfRangeException();
				}
			}
		}

		public int MaxTextureWidth;

		public int MaxTextureHeight;

	}

	public enum SDLScaleMode {
		Nearest,
		Linear,
		Best
	}

	public enum SDLTextureAccess {
		Static,
		Streaming,
		Target
	}

	public enum SDLTextureModulate : uint {
		None = 0,
		Color = 0x1,
		Alpha = 0x2
	}

	public enum SDLRendererFlip : uint {
		None = 0,
		Horizontal = 0x1,
		Vertical = 0x2
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLVertex {

		public Vector2 Position;
		public Vector4b Color;
		public Vector2 TexCoord;

	}

	public class SDLRenderer : IDisposable {

		public IPointer<SDL_Renderer> Renderer { get; private set; }

		public SDLRendererInfo Info {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetRendererInfo(Renderer.Ptr, out SDLRendererInfo info));
				return info;
			}
		}

		public Vector2i OutputSize {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetRendererOutputSize(Renderer.Ptr, out int w, out int h));
				return new Vector2i(w, h);
			}
		}

		public bool RenderTargetSupported => SDL2.Functions.SDL_RenderTargetSupported(Renderer.Ptr);

		public SDLTexture? RenderTarget {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetRenderTarget(Renderer.Ptr, value != null ? value.Texture.Ptr : IntPtr.Zero));
			get {
				IntPtr pTex = SDL2.Functions.SDL_GetRenderTarget(Renderer.Ptr);
				if (pTex == IntPtr.Zero) return null;
				return new SDLTexture(pTex);
			}
		}

		public Vector2i LogicalSize {
			set => SDL2.CheckError(SDL2.Functions.SDL_RenderSetLogicalSize(Renderer.Ptr, value.X, value.Y));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_RenderGetLogicalSize(Renderer.Ptr, out int w, out int h));
				return new Vector2i(w, h);
			}
		}

		public bool IntegerScale {
			set => SDL2.CheckError(SDL2.Functions.SDL_RenderSetIntegerScale(Renderer.Ptr, value));
			get => SDL2.Functions.SDL_RenderGetIntegerScale(Renderer.Ptr);
		}

		public Recti Viewport {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_RenderSetViewport(Renderer.Ptr, (IntPtr)(&value)));
				}
			}
			get {
				SDL2.Functions.SDL_RenderGetViewport(Renderer.Ptr, out Recti rect);
				return rect;
			}
		}

		public Recti ClipRect {
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_RenderSetClipRect(Renderer.Ptr, (IntPtr)(&value)));
				}
			}
			get {
				SDL2.Functions.SDL_RenderGetClipRect(Renderer.Ptr, out Recti rect);
				return rect;
			}
		}

		public bool IsClipEnabled => SDL2.Functions.SDL_RenderIsClipEnabled(Renderer.Ptr);

		public Vector2 Scale {
			set => SDL2.CheckError(SDL2.Functions.SDL_RenderSetScale(Renderer.Ptr, value.X, value.Y));
			get {
				SDL2.Functions.SDL_RenderGetScale(Renderer.Ptr, out float x, out float y);
				return new Vector2(x, y);
			}
		}

		public Vector4b DrawColor {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetRenderDrawColor(Renderer.Ptr, value.X, value.Y, value.Z, value.W));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetRenderDrawColor(Renderer.Ptr, out byte r, out byte g, out byte b, out byte a));
				return new Vector4b() { X = r, Y = g, Z = b, W = a };
			}
		}

		public SDLBlendMode BlendMode {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetRenderDrawBlendMode(Renderer.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetRenderDrawBlendMode(Renderer.Ptr, out SDLBlendMode blendMode));
				return blendMode;
			}
		}

		[NativeType("IDirect3DDevice9*")]
		public IntPtr D3D9Device => SDL2.Functions.SDL_RenderGetD3D9Device(Renderer.Ptr);

		public SDLRenderer(IntPtr pRenderer) {
			Renderer = new UnmanagedPointer<SDL_Renderer>(pRenderer);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Renderer != null) {
				SDL2.Functions.SDL_DestroyRenderer(Renderer.Ptr);
				Renderer = new NullPointer<SDL_Renderer>();
			}
		}

		public SDLTexture CreateTexture(SDLPixelFormatEnum format, SDLTextureAccess access, int w, int h) {
			IntPtr pTex = SDL2.Functions.SDL_CreateTexture(Renderer.Ptr, format, access, w, h);
			if (pTex == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLTexture(pTex);
		}

		public SDLTexture CreateTextureFromSurface(SDLSurface surface) {
			IntPtr pTex = SDL2.Functions.SDL_CreateTextureFromSurface(Renderer.Ptr, surface.Surface.Ptr);
			if (pTex == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLTexture(pTex);
		}

		public void ResetViewport() => SDL2.CheckError(SDL2.Functions.SDL_RenderSetViewport(Renderer.Ptr, IntPtr.Zero));

		public void ResetClipRect() => SDL2.CheckError(SDL2.Functions.SDL_RenderSetClipRect(Renderer.Ptr, IntPtr.Zero));

		public void Clear() => SDL2.CheckError(SDL2.Functions.SDL_RenderClear(Renderer.Ptr));

		public void DrawPoint(Vector2i point) => SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPoint(Renderer.Ptr, point.X, point.Y));

		public void DrawPoints(in ReadOnlySpan<Vector2i> points) {
			unsafe {
				fixed(Vector2i* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPoints(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawPoints(params Vector2i[] points) {
			unsafe {
				fixed (Vector2i* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPoints(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawLine(int x1, int y1, int x2, int y2) => SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLine(Renderer.Ptr, x1, y1, x2, y2));

		public void DrawLines(in ReadOnlySpan<Vector2i> points) {
			unsafe {
				fixed(Vector2i* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLines(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawLines(params Vector2i[] points) {
			unsafe {
				fixed (Vector2i* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLines(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawRect(Recti rect) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRect(Renderer.Ptr, (IntPtr)(&rect)));
			}
		}

		public void DrawRects(in ReadOnlySpan<Recti> rects) {
			unsafe {
				fixed(Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRects(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void DrawRects(params Recti[] rects) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRects(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void FillRect(Recti rect) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_RenderFillRect(Renderer.Ptr, (IntPtr)(&rect)));
			}
		}

		public void FillRects(in ReadOnlySpan<Recti> rects) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderFillRects(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void FillRects(params Recti[] rects) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderFillRects(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void Copy(SDLTexture texture, Recti? srcrect = null, Recti? dstrect = null) {
			unsafe {
				Recti sr;
				if (srcrect.HasValue) sr = srcrect.Value;
				Recti dr;
				if (dstrect.HasValue) dr = dstrect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_RenderCopy(Renderer.Ptr, texture.Texture.Ptr, srcrect.HasValue ? (IntPtr)(&sr) : IntPtr.Zero, dstrect.HasValue ? (IntPtr)(&dr) : IntPtr.Zero));
			}
		}

		public void Copy(SDLTexture texture, double angle, Vector2i center, SDLRendererFlip flip, Recti? srcrect = null, Recti? dstrect = null) {
			unsafe {
				Recti sr;
				if (srcrect.HasValue) sr = srcrect.Value;
				Recti dr;
				if (dstrect.HasValue) dr = dstrect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_RenderCopyEx(Renderer.Ptr, texture.Texture.Ptr, srcrect.HasValue ? (IntPtr)(&sr) : IntPtr.Zero, dstrect.HasValue ? (IntPtr)(&dr) : IntPtr.Zero, angle, center, flip));
			}
		}

		public void DrawPoint(Vector2 point) => SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPointF(Renderer.Ptr, point.X, point.Y));

		public void DrawPoints(in ReadOnlySpan<Vector2> points) {
			unsafe {
				fixed (Vector2* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPointsF(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawPoints(params Vector2[] points) {
			unsafe {
				fixed (Vector2* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawPointsF(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawLine(float x1, float y1, float x2, float y2) => SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLineF(Renderer.Ptr, x1, y1, x2, y2));

		public void DrawLines(in ReadOnlySpan<Vector2> points) {
			unsafe {
				fixed (Vector2* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLinesF(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawLines(params Vector2[] points) {
			unsafe {
				fixed (Vector2* pPoints = points) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawLinesF(Renderer.Ptr, (IntPtr)pPoints, points.Length));
				}
			}
		}

		public void DrawRect(Rectf rect) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRectF(Renderer.Ptr, (IntPtr)(&rect)));
			}
		}

		public void DrawRects(in ReadOnlySpan<Rectf> rects) {
			unsafe {
				fixed (Rectf* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRectsF(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void DrawRects(params Rectf[] rects) {
			unsafe {
				fixed (Rectf* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderDrawRectsF(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void FillRect(Rectf rect) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_RenderFillRectF(Renderer.Ptr, (IntPtr)(&rect)));
			}
		}

		public void FillRects(in ReadOnlySpan<Rectf> rects) {
			unsafe {
				fixed (Rectf* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderFillRectsF(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void FillRects(params Rectf[] rects) {
			unsafe {
				fixed (Rectf* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_RenderFillRectsF(Renderer.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void Copy(SDLTexture texture, Rectf? srcrect = null, Rectf? dstrect = null) {
			unsafe {
				Rectf sr;
				if (srcrect.HasValue) sr = srcrect.Value;
				Rectf dr;
				if (dstrect.HasValue) dr = dstrect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_RenderCopyF(Renderer.Ptr, texture.Texture.Ptr, srcrect.HasValue ? (IntPtr)(&sr) : IntPtr.Zero, dstrect.HasValue ? (IntPtr)(&dr) : IntPtr.Zero));
			}
		}

		public void Copy(SDLTexture texture, double angle, Vector2 center, SDLRendererFlip flip, Rectf? srcrect = null, Rectf? dstrect = null) {
			unsafe {
				Rectf sr;
				if (srcrect.HasValue) sr = srcrect.Value;
				Rectf dr;
				if (dstrect.HasValue) dr = dstrect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_RenderCopyExF(Renderer.Ptr, texture.Texture.Ptr, srcrect.HasValue ? (IntPtr)(&sr) : IntPtr.Zero, dstrect.HasValue ? (IntPtr)(&dr) : IntPtr.Zero, angle, center, flip));
			}
		}

		public void RenderGeometry(SDLTexture? texture, in ReadOnlySpan<SDLVertex> vertices, in ReadOnlySpan<int> indices) {
			unsafe {
				fixed(SDLVertex* pVertices = vertices) {
					fixed(int* pIndices = indices) {
						SDL2.CheckError(SDL2.Functions.SDL_RenderGeometry(Renderer.Ptr, texture?.Texture?.Ptr ?? IntPtr.Zero, (IntPtr)pVertices, vertices.Length, (IntPtr)pIndices, indices.Length));
					}
				}
			}
		}

		public void RenderGeometryRaw<T>(SDLTexture texture, in ReadOnlySpan<float> xy, int xyStride, in ReadOnlySpan<Vector4b> color, int colorStride, in ReadOnlySpan<float> uv, int uvStride, in ReadOnlySpan<T> indices, int numVertices = -1, int numIndices = -1) where T : unmanaged {
			unsafe {
				fixed (float* pXY = xy) {
					fixed (Vector4b* pColor = color) {
						fixed (float* pUV = uv) {
							int maxVertices = ExMath.Min(xy.Length / xyStride, color.Length / colorStride, uv.Length / uvStride);
							if (numVertices > maxVertices || numVertices < 0) numVertices = maxVertices;
							fixed(T* pIndices = indices) {
								int maxIndices = indices.Length;
								if (numIndices > maxIndices || numIndices < 0) numIndices = maxIndices;
								SDL2.CheckError(SDL2.Functions.SDL_RenderGeometryRaw(Renderer.Ptr, texture.Texture.Ptr, (IntPtr)pXY, xyStride, (IntPtr)pColor, colorStride, (IntPtr)pUV, uvStride, numVertices, (IntPtr)pIndices, numIndices, Marshal.SizeOf<T>()));
							}
						}
					}
				}
			}
		}

		public void ReadPixels(SDLPixelFormatEnum format, IPointer<byte> pixels, int pitch, Recti? rect = null) {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_RenderReadPixels(Renderer.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, format, pixels.Ptr, pitch));
			}
		}

		public void Present() => SDL2.Functions.SDL_RenderPresent(Renderer.Ptr);

		public void Flush() => SDL2.CheckError(SDL2.Functions.SDL_RenderFlush(Renderer.Ptr));

		public IntPtr GetMetalLayer() => SDL2.Functions.SDL_RenderGetMetalLayer(Renderer.Ptr);

		public IntPtr GetMetalCommandEncoder() => SDL2.Functions.SDL_RenderGetMetalCommandEncoder(Renderer.Ptr);

	}

	public class SDLTexture : IDisposable {

		public IPointer<SDL_Texture> Texture { get; private set; }

		public (SDLPixelFormatEnum, SDLTextureAccess, Vector2i) Info {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_QueryTexture(Texture.Ptr, out SDLPixelFormatEnum format, out SDLTextureAccess access, out int w, out int h));
				return (format, access, new Vector2i(w, h));
			}
		}

		public Vector3b ColorMod {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetTextureColorMod(Texture.Ptr, value.X, value.Y, value.Z));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetTextureColorMod(Texture.Ptr, out byte r, out byte g, out byte b));
				return new Vector3b(r, g, b);
			}
		}

		public byte AlphaMod {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetTextureAlphaMod(Texture.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetTextureAlphaMod(Texture.Ptr, out byte a));
				return a;
			}
		}

		public SDLBlendMode BlendMode {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetTextureBlendMode(Texture.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetTextureBlendMode(Texture.Ptr, out SDLBlendMode blendMode));
				return blendMode;
			}
		}

		public SDLScaleMode ScaleMode {
			set => SDL2.CheckError(SDL2.Functions.SDL_SetTextureScaleMode(Texture.Ptr, value));
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetTextureScaleMode(Texture.Ptr, out SDLScaleMode scaleMode));
				return scaleMode;
			}
		}

		public SDLTexture(IntPtr pTexture) {
			Texture = new UnmanagedPointer<SDL_Texture>(pTexture);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Texture != null) {
				SDL2.Functions.SDL_DestroyTexture(Texture.Ptr);
				Texture = new NullPointer<SDL_Texture>();
			}
		}

		public void UpdateTexture(Recti? rect, IConstPointer<byte> pixels, int pitch) {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_UpdateTexture(Texture.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, pixels.Ptr, pitch));
			}
		}

		public void UpdateTexture<T>(Recti? rect, in ReadOnlySpan<T> pixels, int pitch) where T : unmanaged {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				fixed(T* pPixels = pixels) {
					SDL2.CheckError(SDL2.Functions.SDL_UpdateTexture(Texture.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, (IntPtr)pPixels, pitch));
				}
			}
		}

		public void UpdateYUVTexture(Recti? rect, IConstPointer<byte> ypixels, int ypitch, IConstPointer<byte> upixels, int upitch, IConstPointer<byte> vpixels, int vpitch) {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_UpdateYUVTexture(Texture.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, ypixels.Ptr, ypitch, upixels.Ptr, upitch, vpixels.Ptr, vpitch));
			}
		}

		public (IPointer<byte>, int) LockTexture(Recti? rect = null) {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_LockTexture(Texture.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, out IntPtr pixels, out int pitch));
				return (new UnmanagedPointer<byte>(pixels), pitch);
			}
		}

		public SDLSurface LockTextureToSurface(Recti? rect = null) {
			unsafe {
				Recti r;
				if (rect.HasValue) r = rect.Value;
				SDL2.CheckError(SDL2.Functions.SDL_LockTextureToSurface(Texture.Ptr, rect.HasValue ? (IntPtr)(&r) : IntPtr.Zero, out IntPtr surface));
				return new SDLSurface(new UnmanagedPointer<SDL_Surface>(surface));
			}
		}

		public void UnlockTexture() => SDL2.Functions.SDL_UnlockTexture(Texture.Ptr);

		public Vector2 GLBindTexture() {
			SDL2.CheckError(SDL2.Functions.SDL_GL_BindTexture(Texture.Ptr, out float texw, out float texh));
			return new Vector2(texw, texh);
		}

		public void GLUnbindTexture() => SDL2.CheckError(SDL2.Functions.SDL_GL_UnbindTexture(Texture.Ptr));

	}

}
