using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using Tesseract.Core.Graphics;
using Tesseract.Core.Math;
using Tesseract.Core.Resource;
using Tesseract.Core.Native;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IImage = Tesseract.Core.Graphics.IImage;

namespace Tesseract.Utilities.Graphics {

	public interface IImageSharpImage : IImage, IProcessableImage {

		public Image AbstractImage { get; }

		private static readonly Dictionary<Type, Func<Image, IImageSharpImage>> imageBuilders = new() {
			{ typeof(A8), img => new ImageSharpImage<A8>((Image<A8>)img) },
			{ typeof(Argb32), img => new ImageSharpImage<Argb32>((Image<Argb32>)img) },
			{ typeof(Bgr24), img => new ImageSharpImage<Bgr24>((Image<Bgr24>)img) },
			{ typeof(Bgr565), img => new ImageSharpImage<Bgr565>((Image<Bgr565>)img) },
			{ typeof(Bgra32), img => new ImageSharpImage<Bgra32>((Image<Bgra32>)img) },
			{ typeof(Bgra4444), img => new ImageSharpImage<Bgra4444>((Image<Bgra4444>)img) },
			{ typeof(Bgra5551), img => new ImageSharpImage<Bgra5551>((Image<Bgra5551>)img) },
			{ typeof(L16), img => new ImageSharpImage<L16>((Image<L16>)img) },
			{ typeof(L8), img => new ImageSharpImage<L8>((Image<L8>)img) },
			{ typeof(La16), img => new ImageSharpImage<La16>((Image<La16>)img) },
			{ typeof(La32), img => new ImageSharpImage<La32>((Image<La32>)img) },
			{ typeof(Rg32), img => new ImageSharpImage<Rg32>((Image<Rg32>)img) },
			{ typeof(Rgb24), img => new ImageSharpImage<Rgb24>((Image<Rgb24>)img) },
			{ typeof(Rgb48), img => new ImageSharpImage<Rgb48>((Image<Rgb48>)img) },
			{ typeof(Rgba1010102), img => new ImageSharpImage<Rgba1010102>((Image<Rgba1010102>)img) },
			{ typeof(Rgba32), img => new ImageSharpImage<Rgba32>((Image<Rgba32>)img) },
			{ typeof(Rgba64), img => new ImageSharpImage<Rgba64>((Image<Rgba64>)img) }
		};

		public static IImageSharpImage Create(Image img) => imageBuilders[img.GetType().GetGenericArguments()[0]](img);

		private static readonly Dictionary<Type, Func<IImage, Image>> imageConverters = new() {
			{ typeof(A8), img => {
				var ptr = MemoryUtil.RecastAs<byte,A8>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<A8>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Argb32), img => {
				var ptr = MemoryUtil.RecastAs<byte, Argb32>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Argb32>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Bgr24), img => {
				var ptr = MemoryUtil.RecastAs<byte, Bgr24>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Bgr24>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Bgr565), img => {
				var ptr = MemoryUtil.RecastAs<byte, Bgr565>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Bgr565>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Bgra32), img => {
				var ptr = MemoryUtil.RecastAs<byte, Bgra32>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Bgra32>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Bgra4444), img => {
				var ptr = MemoryUtil.RecastAs<byte, Bgra4444>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Bgra4444>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Bgra5551), img => {
				var ptr = MemoryUtil.RecastAs<byte, Bgra5551>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Bgra5551>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(L16), img => {
				var ptr = MemoryUtil.RecastAs<byte, L16>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<L16>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(L8), img => {
				var ptr = MemoryUtil.RecastAs<byte, L8>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<L8>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(La16), img => {
				var ptr = MemoryUtil.RecastAs<byte, La16>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<La16>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(La32), img => {
				var ptr = MemoryUtil.RecastAs<byte, La32>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<La32>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rg32), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rg32>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rg32>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rgb24), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rgb24>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rgb24>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rgb48), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rgb48>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rgb48>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rgba1010102), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rgba1010102>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rgba1010102>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rgba32), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rgba32>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rgba32>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} },
			{ typeof(Rgba64), img => {
				var ptr = MemoryUtil.RecastAs<byte, Rgba64>(img.MapPixels(MapMode.ReadOnly));
				var img2 = Image.LoadPixelData<Rgba64>(ptr.Span, img.Size.X, img.Size.Y);
				img.UnmapPixels();
				return img2;
			} }
		};

		public static IImageSharpImage Create(IImage img) {
			if (img is IImageSharpImage isi) return isi;
			if (!ImageSharpService.pixelFormatToType.TryGetValue(img.Format, out Type? pt))
				throw new ArgumentException($"Image pixel format {img.Format} is not supported by ImageSharp", nameof(img));
			else return Create(imageConverters[pt](img));
		}

	}

	public class ImageSharpImage<TPixel> : IImageSharpImage where TPixel : unmanaged, IPixel<TPixel> {

		public Image<TPixel> Image { get; private set; }

		public Image AbstractImage => Image;

		public ImageSharpImage(Image<TPixel> img) {
			if (!ImageSharpService.pixelTypeToFormat.TryGetValue(typeof(TPixel), out PixelFormat? format))
				throw new ArgumentException($"Unsupported ImageSharp pixel type '{typeof(TPixel).Name}'", nameof(img));
			Format = format;
			Image = img;
		}

		public IReadOnlyTuple2<int> Size => new Vector2i(Image.Width, Image.Height);

		public PixelFormat Format { get; }

		public void Dispose() {
			GC.SuppressFinalize(this);
			Image.Dispose();
		}

		//   Could be done more efficiently, but ImageSharp only provides Span-based access without using ugly reflection hacks,
		// and returning pointers provided by span objects will cause undefined behavior once the memory pinning goes out of scope
		private MapMode mapMode;
		private Memory<byte> mappedPixelMemory;
		private ManagedPointer<byte> mappedMemoryPtr;

		public IPointer<byte> MapPixels(MapMode mode) {
			if (mappedMemoryPtr) return mappedMemoryPtr;
			mapMode = mode;
			mappedPixelMemory = new Memory<byte>(new byte[Image.Width * Image.Height * Format.SizeOf]);
			mappedMemoryPtr = new ManagedPointer<byte>(mappedPixelMemory);
			if (mode != MapMode.WriteOnly) {
				Span<TPixel> pixels = Image.GetPixelRowSpan(0);
				MemoryUtil.Copy(new UnmanagedPointer<TPixel>(mappedMemoryPtr.Ptr), pixels, mappedPixelMemory.Length);
			}
			return mappedMemoryPtr;
		}

		public void UnmapPixels() {
			if (!mappedMemoryPtr) return;
			if (mapMode != MapMode.ReadOnly) {
				var newimg = SixLabors.ImageSharp.Image.LoadPixelData<TPixel>(mappedPixelMemory.Span, Image.Width, Image.Height);
				Image.Dispose();
				Image = newimg;
			}
			mappedMemoryPtr.Dispose();
			mappedMemoryPtr = default;
			mappedPixelMemory = default;
		}

		public IImage Duplicate() => new ImageSharpImage<TPixel>(Image.Clone());

		public IImage Convert(PixelFormat format) => IImageSharpImage.Create(this);

		public void Blit(IReadOnlyRect<int> dstArea, IImage src, IReadOnlyTuple2<int> srcPos) {
			IImageSharpImage isi;
			bool dispose = false;
			if (src is IImageSharpImage image) isi = image;
			else {
				isi = IImageSharpImage.Create(src);
				dispose = true;
			}
			try {
				int w = dstArea.Size.X, h = dstArea.Size.Y;
				Image srcimg = isi.AbstractImage;
				// If blitting a subsection of the image, clone and crop
				if (srcimg.Width != w || srcimg.Height != h || srcPos.X != 0 || srcPos.Y != 0) srcimg = srcimg.Clone(
					x => x.Crop(
						new Rectangle() {
							X = srcPos.X,
							Y = srcPos.Y,
							Width = w,
							Height = h
						}
					)
				);
				// Mutate this image by drawing the source image at the specified position
				Image.Mutate(x => x.DrawImage(srcimg, new Point() { X = dstArea.Position.X, Y = dstArea.Position.Y }, 1f));
			} finally {
				if (dispose) isi.Dispose();
			}
		}

		public void Fill(IReadOnlyRect<int> dstArea, IReadOnlyColor color) => throw new NotImplementedException();
	}

	public class ImageSharpService : IImageIO {

		internal static readonly Dictionary<Type, PixelFormat> pixelTypeToFormat = new() {
			{ typeof(A8), PixelFormat.A8UNorm },
			{ typeof(Argb32), PixelFormat.A8R8G8B8UNorm },
			{ typeof(Bgr24), PixelFormat.B8G8R8UNorm },
			{ typeof(Bgr565), PixelFormat.B5G6R5UNormPack16 },
			{ typeof(Bgra32), PixelFormat.B8G8R8A8UNorm },
			{ typeof(Bgra4444), PixelFormat.A4R4G4B4UNormPack16 },
			{ typeof(Bgra5551), PixelFormat.A1R5G5B5UNormPack16 },
			{ typeof(L16), PixelFormat.L16UNorm },
			{ typeof(L8), PixelFormat.L8UNorm },
			{ typeof(La16), PixelFormat.L8A8UNorm },
			{ typeof(La32), PixelFormat.L16A16UNorm },
			{ typeof(Rg32), PixelFormat.R16G16UNormPack32 },
			{ typeof(Rgb24), PixelFormat.R8G8B8UNorm },
			{ typeof(Rgb48), PixelFormat.R16G16B16UNorm },
			{ typeof(Rgba1010102), PixelFormat.A2B10G10R10UNormPack32 },
			{ typeof(Rgba32), PixelFormat.R8G8B8A8UNorm },
			{ typeof(Rgba64), PixelFormat.R16G16B16A16UNorm }
		};

		internal static readonly Dictionary<PixelFormat, Type> pixelFormatToType = pixelTypeToFormat.ToDictionary(item => item.Value, item => item.Key);

		public IImage Load(Core.Resource.ResourceLocation location) {
			using var stream = location.OpenStream();
			return IImageSharpImage.Create(Image.Load(location.OpenStream()));
		}

		public IImage Load(ReadOnlySpan<byte> binary, string mimeType) => IImageSharpImage.Create(Image.Load(binary));

		public Span<byte> Save(IImage image, string mimeType) {
			Image img;
			bool dispose = false;
			if (image is IImageSharpImage isi) img = isi.AbstractImage;
			else {
				img = IImageSharpImage.Create(image).AbstractImage;
				dispose = true;
			}
			try {
				using var stream = new MemoryStream();
				switch (mimeType) {
					case MIME.BMP:
						img.SaveAsBmp(stream);
						break;
					case MIME.GIF:
						img.SaveAsGif(stream);
						break;
					case MIME.JPEG:
						img.SaveAsJpeg(stream);
						break;
					case MIME.PNG:
						img.SaveAsPng(stream);
						break;
					default:
						throw new ArgumentException("Unsupported image MIME type", mimeType);
				}
				return stream.ToArray();
			} finally {
				if (dispose) img.Dispose();
			}
		}

	}

}
