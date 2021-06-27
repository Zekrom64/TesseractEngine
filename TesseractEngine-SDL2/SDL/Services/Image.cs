using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;

namespace Tesseract.SDL.Services {
	public class SDLServiceImage : IImage {

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
			if (Surface.MustLock) Surface.Lock();
			return new UnmanagedPointer<byte>(Surface.Pixels);
		}

		public void UnmapPixels() {
			if (Surface.MustLock) Surface.Unlock();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Surface.Dispose();
		}
	}

	public class SDLImageIOService : IImageIO {
		public IImage Load(ResourceLocation location) {
			string mime = location.Metadata.MIMEType;
			if (mime != MIME.BMP) throw new ArgumentException("MIME type not supported by image IO", nameof(location));
			return Load(location.ReadFully(), mime);
		}

		public IImage Load(Span<byte> binary, string mimeType) {
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
