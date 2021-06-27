using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;

namespace Tesseract.Core.Graphics {
	
	public interface IImage : IDisposable {

		public IReadOnlyTuple2<int> Size { get; }

		public PixelFormat Format { get; }

		public IImage Duplicate();

		public IPointer<byte> MapPixels(MapMode mode);

		public void UnmapPixels();

	}

	public class SoftwareImage : IImage {

		public PixelFormat Format { get; }
		public IReadOnlyTuple2<int> Size { get; }

		private readonly byte[] pixels;
		private GCHandle? pixelHandle;

		public SoftwareImage(int w, int h, PixelFormat format) {
			Format = format;
			Size = new Vector2i(w, h);
		}

		public SoftwareImage(IReadOnlyTuple2<int> size, PixelFormat format) {
			Format = format;
			Size = new Vector2i(size);
		}

		public IImage Duplicate() {
			SoftwareImage newImage = new(Size, Format);
			Array.Copy(pixels, newImage.pixels, pixels.Length);
			return newImage;
		}

		public IPointer<byte> MapPixels(MapMode mode) {
			if (!pixelHandle.HasValue) pixelHandle = GCHandle.Alloc(pixels);
			return new UnmanagedPointer<byte>(pixelHandle.Value.AddrOfPinnedObject());
		}

		public void UnmapPixels() {
			if (pixelHandle.HasValue) {
				pixelHandle.Value.Free();
				pixelHandle = null;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (pixelHandle.HasValue) pixelHandle.Value.Free();
		}
	}

	public interface IImageIO {

		public IImage Load(ResourceLocation location);

		public IImage Load(Span<byte> binary, string mimeType);

		public Span<byte> Save(IImage image, string mimeType);

	}

	public interface IImageProcessing {

		public IImage Convert(IImage image, PixelFormat format);

		public void Blit(IImage dst, IReadOnlyRect<int> dstArea, IImage src, IReadOnlyTuple2<int> srcPos);

		public void Fill(IImage dst, IReadOnlyRect<int> dstArea, IReadOnlyColor color);

	}

}
