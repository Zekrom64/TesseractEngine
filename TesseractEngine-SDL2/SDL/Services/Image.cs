using System;
using System.IO;
using Tesseract.Core.Graphics;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;
using Tesseract.Core.Services;

namespace Tesseract.SDL.Services {

	public class SDLServiceImage : IImage, IProcessableImage {

		public SDLSurface Surface { get; }

		public IReadOnlyTuple2<int> Size { get; }

		public PixelFormat Format { get; }

		public SDLServiceImage(SDLSurface surface) {
			Surface = surface;
			Size = new Vector2i(surface.W, surface.H);
			Format = SDLPixelService.ConvertPixelFormat(surface.PixelFormatEnum);
		}

		public SDLServiceImage(int w, int h, PixelFormat format) {
			SDLPixelFormatEnum sdlformat = SDLPixelService.ConvertPixelFormat(format);
			if (sdlformat == SDLPixelFormatEnum.Unknown) throw new ArgumentException("Unsupported pixel format", nameof(format));
			Surface = new SDLSurface(w, h, sdlformat);
			Size = new Vector2i(w, h);
			Format = format;
		}

		public SDLServiceImage(IImage image) {
			if (image is SDLServiceImage sdlimage) {
				Surface = sdlimage.Surface.Duplicate();
				Size = sdlimage.Size;
				Format = sdlimage.Format;
			} else {
				SDLPixelFormatEnum sdlformat = SDLPixelService.ConvertPixelFormat(image.Format);
				if (sdlformat == SDLPixelFormatEnum.Unknown) throw new ArgumentException("Source image has unsupported pixel format", nameof(image));
				Format = image.Format;
				Size = image.Size;
				Surface = new SDLSurface(Size.X, Size.Y, sdlformat);
				IPointer<byte> pSrc = image.MapPixels(MapMode.ReadOnly);
				IPointer<byte> pDst = MapPixels(MapMode.WriteOnly);
				MemoryUtil.Copy(pDst, pSrc, Size.X * Size.Y * Format.SizeOf);
				image.UnmapPixels();
				UnmapPixels();
			}
		}

		public IImage Duplicate() => new SDLServiceImage(Surface.Duplicate());

		public IPointer<byte> MapPixels(MapMode mode) {
			if (Surface.MustLock) return new UnmanagedPointer<byte>(Surface.Lock());
			return new UnmanagedPointer<byte>(Surface.Pixels);
		}

		public void UnmapPixels() {
			if (Surface.MustLock) Surface.Unlock();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Surface.Dispose();
		}

		public T GetService<T>(IService<T> service) {
			if (service == GraphicsServices.ProcessableImage) return (T)(object)this;
			return default;
		}

		public IImage Convert(PixelFormat format) {
			SDLPixelFormatEnum pxfmt = SDLPixelService.ConvertPixelFormat(format);
			if (pxfmt == SDLPixelFormatEnum.Unknown) throw new SDLException("Cannot convert to unsupported pixel format");
			SDLServiceImage newimg = new(Surface.Convert(pxfmt));
			return newimg;
		}

		public void Blit(IReadOnlyRect<int> dstArea, IImage src, IReadOnlyTuple2<int> srcPos) {
			SDLServiceImage sdlsrc;
			bool dispose = false;
			if (src is SDLServiceImage) sdlsrc = src as SDLServiceImage;
			else {
				sdlsrc = new SDLServiceImage(src);
				dispose = true;
			}
			Surface.Blit(new SDLRect() {
				X = dstArea.Position.X,
				Y = dstArea.Position.Y,
				W = dstArea.Size.X,
				H = dstArea.Size.Y
			},
			sdlsrc.Surface, new SDLRect() {
				X = srcPos.X,
				Y = srcPos.Y,
				W = dstArea.Size.X,
				H = dstArea.Size.Y
			});
			if (dispose) sdlsrc.Dispose();
		}

		public void Fill(IReadOnlyRect<int> dstArea, IReadOnlyColor color) {
			Surface.FillRect(new SDLRect() {
				X = dstArea.Position.X,
				Y = dstArea.Position.Y,
				W = dstArea.Size.X,
				H = dstArea.Size.Y
			}, Surface.PixelFormat.MapColor(color));
		}

	}

	public class SDLImageIOService : IImageIO {

		public IImage Load(ResourceLocation location) {
			string mime = location.Metadata.MIMEType;
			if (mime != MIME.BMP) throw new ArgumentException("MIME type not supported by image IO", nameof(location));
			using MemoryStream ms = new();
			using Stream stream = location.OpenStream();
			stream.CopyTo(ms);
			return Load(ms.ToArray(), mime);

		}

		public IImage Load(ReadOnlySpan<byte> binary, string mimeType) {
			if (mimeType != MIME.BMP) throw new ArgumentException("MIME type not supported by image IO", nameof(mimeType));
			using SDLSpanRWOps rwops = new(binary);
			return new SDLServiceImage(SDL2.LoadBMP(rwops));
		}

		public Span<byte> Save(IImage image, string mimeType) {
			if (mimeType != MIME.BMP) throw new ArgumentException("MIME type not supported by image IO", nameof(mimeType));

			bool dispose = false;
			SDLServiceImage sdlimage;
			if (image is SDLServiceImage) sdlimage = image as SDLServiceImage;
			else {
				sdlimage = new SDLServiceImage(image);
				dispose = true;
			}

			try {
				using MemoryStream mstream = new();
				using SDLStreamRWOps rwstream = new(mstream);
				SDL2.SaveBMP(sdlimage.Surface, rwstream);
				return mstream.ToArray();
			} finally {
				if (dispose) sdlimage.Dispose();
			}
		}

	}

}
