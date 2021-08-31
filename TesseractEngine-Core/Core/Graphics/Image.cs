using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;
using Tesseract.Core.Services;

namespace Tesseract.Core.Graphics {
	
	public interface IImage : IDisposable, Services.IServiceProvider {

		public IReadOnlyTuple2<int> Size { get; }

		public PixelFormat Format { get; }

		public IImage Duplicate();

		public IPointer<byte> MapPixels(MapMode mode);

		public void UnmapPixels();

	}

	public class SoftwareImage : IImage {

		public PixelFormat Format { get; }
		public IReadOnlyTuple2<int> Size { get; }

		private readonly Memory<byte> pixels;
		private MemoryHandle? pixelHandle;

		public SoftwareImage(int w, int h, PixelFormat format) {
			Format = format;
			Size = new Vector2i(w, h);
			pixels = new byte[w * h * format.SizeOf];
		}

		public SoftwareImage(IReadOnlyTuple2<int> size, PixelFormat format) {
			Format = format;
			Size = new Vector2i(size);
			pixels = new byte[Size.X * Size.Y * format.SizeOf];
		}

		public IImage Duplicate() {
			SoftwareImage newImage = new(Size, Format);
			pixels.CopyTo(newImage.pixels);
			return newImage;
		}

		public IPointer<byte> MapPixels(MapMode mode) {
			if (!pixelHandle.HasValue) pixelHandle = pixels.Pin();
			unsafe { return new UnmanagedPointer<byte>((IntPtr)pixelHandle.Value.Pointer); }
		}

		public void UnmapPixels() {
			if (pixelHandle.HasValue) {
				pixelHandle.Value.Dispose();
				pixelHandle = null;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			UnmapPixels();
		}
	}

	public interface IImageIO {

		public IImage Load(ResourceLocation location);

		public IImage Load(ReadOnlySpan<byte> binary, string mimeType);

		public Span<byte> Save(IImage image, string mimeType);

	}

	public interface IProcessableImage {

		public IImage Convert(PixelFormat format);

		public void Blit(IReadOnlyRect<int> dstArea, IImage src, IReadOnlyTuple2<int> srcPos);

		public void Fill(IReadOnlyRect<int> dstArea, IReadOnlyColor color);

	}

}
