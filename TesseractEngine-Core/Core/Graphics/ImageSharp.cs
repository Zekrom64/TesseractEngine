using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Resource;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Tesseract.Core.Graphics.QOI;
using System.Numerics;

namespace Tesseract.Core.Graphics {

	/// <summary>
	/// Interface for ImageSharp-based images.
	/// </summary>
	public interface IImageSharpImage : IProcessableImage {

		/// <summary>
		/// The abstract ImageSharp image that this object wraps.
		/// </summary>
		public Image AbstractImage { get; }

		/// <summary>
		/// Wraps an ImageSharp image as a standard Tesseract image.
		/// </summary>
		/// <param name="img">Image to wrap</param>
		/// <returns>Wrapped image</returns>
		public static IImageSharpImage Create(Image img) => ImageSharpService.pixelTypeToFormat[img.GetType().GetGenericArguments()[0]].OutputConverter(img);

		/// <summary>
		/// Creates an ImageSharp image from the given generic image object.
		/// </summary>
		/// <param name="img">Image to convert</param>
		/// <returns>Converted image</returns>
		/// <exception cref="ArgumentException">If the image's pixel format is not supported</exception>
		public static IImageSharpImage Create(IImage img) {
			if (img is IImageSharpImage isi) return isi;
			if (!ImageSharpService.pixelFormatToType.TryGetValue(img.Format, out ImageSharpService.FormatInfo? pt))
				throw new ArgumentException($"Image pixel format {img.Format} is not supported by ImageSharp", nameof(img));
			else return Create(pt.InputConverter(img));
		}

	}

	/// <summary>
	/// Implementation of an ImageSharp image with a known pixel type.
	/// </summary>
	/// <typeparam name="TPixel">Pixel type</typeparam>
	public class ImageSharpImage<TPixel> : IImageSharpImage where TPixel : unmanaged, IPixel<TPixel> {

		/// <summary>
		/// The underlying image object.
		/// </summary>
		public Image<TPixel> Image { get; private set; }

		public Image AbstractImage => Image;

		/// <summary>
		/// Creates a wrapper around the given ImageSharp image.
		/// </summary>
		/// <param name="img">Image to wrap</param>
		/// <exception cref="ArgumentException">If the image's pixel format is not supported</exception>
		public ImageSharpImage(Image<TPixel> img) {
			if (!ImageSharpService.pixelTypeToFormat.TryGetValue(typeof(TPixel), out ImageSharpService.FormatInfo? format))
				throw new ArgumentException($"Unsupported ImageSharp pixel type '{typeof(TPixel).Name}'", nameof(img));
			Format = format.Format;
			Image = img;
		}

		public IReadOnlyTuple2<int> Size => new Vector2i(Image.Width, Image.Height);

		public PixelFormat Format { get; }

		public void Dispose() {
			GC.SuppressFinalize(this);
			Image.Dispose();
		}

		// If the mapped memory is directly from the image
		private bool mapDirect;
		// The memory mapping mode
		private MapMode mapMode;
		// The mapped pixel memory
		private Memory<TPixel> mappedPixelMemory;
		// The managed pointer to the mapped pixel memory
		private ManagedPointer<TPixel> mappedMemoryPtr;

		public IPointer<byte> MapPixels(MapMode mode) {
			if (mappedMemoryPtr) return MemoryUtil.RecastAs<TPixel, byte>(mappedMemoryPtr);

			mapMode = mode;
			// Get direct access to the image's pixel memory if possible
			if (Image.DangerousTryGetSinglePixelMemory(out Memory<TPixel> directMemory)) {
				mappedPixelMemory = directMemory;
				mapDirect = true;
			} else {
				// Else allocate a new memory buffer and copy the pixel data to it
				mappedPixelMemory = new Memory<TPixel>(new TPixel[Image.Width * Image.Height]);
				mapDirect = false;
				if (mode != MapMode.WriteOnly)
					Image.CopyPixelDataTo(mappedPixelMemory.Span);
			}

			// Map the memory as a managed pointer
			mappedMemoryPtr = new ManagedPointer<TPixel>(mappedPixelMemory);
			return MemoryUtil.RecastAs<TPixel, byte>(mappedMemoryPtr);
		}

		public void UnmapPixels() {
			if (!mappedMemoryPtr) return;
			// If not directly mapped and data has been written to it
			if (!mapDirect && mapMode != MapMode.ReadOnly) {
				// Process the image's pixel data
				Image.ProcessPixelRows(accessor => {
					UnmanagedPointer<TPixel> ptr = new(mappedMemoryPtr.Ptr, mappedMemoryPtr.ArraySize);
					int pxsize = Format.SizeOf;
					int rowLength = accessor.Width * pxsize;

					// For each pixel row, copy a row back to the image and shfit the pointer
					for(int y = 0; y < accessor.Height; y++, ptr += accessor.Width) {
						Span<TPixel> row = accessor.GetRowSpan(y);
						MemoryUtil.Copy(row, ptr, (ulong)rowLength);
					}
				});
			}
			// Unmap the pointer
			mappedMemoryPtr.Dispose();
			mappedMemoryPtr = default;
			mappedPixelMemory = default;
		}

		public IImage Duplicate() => new ImageSharpImage<TPixel>(Image.Clone());

		public IImage Convert(PixelFormat format) => IImageSharpImage.Create(this); // TODO: Pixel format selection

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

		public void Fill(IReadOnlyRect<int> dstArea, Vector4 color) => Image.Mutate(x => x.Fill(
			new Color(color), new RectangleF(dstArea.Position.X, dstArea.Position.Y, dstArea.Size.X, dstArea.Size.Y)));
	}

	/// <summary>
	/// An implementation of several image services using ImageSharp.
	/// </summary>
	public class ImageSharpService : IImageIO {

		internal class FormatInfo {

			public Type PixelType { get; }

			public PixelFormat Format { get; }

			public Func<Image, IImageSharpImage> OutputConverter { get; }

			public Func<IImage, Image> InputConverter { get; }

			public Func<int, int, Image> AbstractConstructor { get; }

			public FormatInfo(Type pixelType, PixelFormat format, Func<Image, IImageSharpImage> outConverter, Func<IImage, Image> inConverter, Func<int, int, Image> constructor) {
				PixelType = pixelType;
				Format = format;
				OutputConverter = outConverter;
				InputConverter = inConverter;
				AbstractConstructor = constructor;
			}

		}

		internal static readonly Dictionary<Type, FormatInfo> pixelTypeToFormat = new() {
			{ 
				typeof(A8),
				new FormatInfo(
				typeof(A8),
				PixelFormat.A8UNorm,
				img => new ImageSharpImage<A8>((Image<A8>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, A8>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<A8>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<A8>(w, h)
			)
			},
			{
				typeof(Argb32),
				new FormatInfo(
				typeof(Argb32),
				PixelFormat.A8B8G8R8UNorm,
				img => new ImageSharpImage<Abgr32>((Image<Abgr32>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Abgr32>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Abgr32>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Abgr32>(w, h)
			)
			},
			{
				typeof(Bgr24),
				new FormatInfo(
				typeof(Bgr24),
				PixelFormat.B8G8R8UNorm,
				img => new ImageSharpImage<Bgr24>((Image<Bgr24>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Bgr24>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Bgr24>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Bgr24>(w, h)
			)
			},
			{
				typeof(Bgr565),
				new FormatInfo(
				typeof(Bgr565),
				PixelFormat.B5G6R5UNormPack16,
				img => new ImageSharpImage<Bgr565>((Image<Bgr565>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Bgr565>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Bgr565>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Bgr565>(w, h)
			)
			},
			{
				typeof(Bgra32),
				new FormatInfo(
				typeof(Bgra32),
				PixelFormat.B8G8R8A8UNorm,
				img => new ImageSharpImage<Bgra32>((Image<Bgra32>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Bgra32>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Bgra32>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Bgra32>(w, h)
			)
			},
			{
				typeof(Bgra4444),
				new FormatInfo(
				typeof(Bgra4444),
				PixelFormat.A4R4G4B4UNormPack16,
				img => new ImageSharpImage<Bgra4444>((Image<Bgra4444>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Bgra4444>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Bgra4444>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Bgra4444>(w, h)
			)
			},
			{
				typeof(Bgra5551),
				new FormatInfo(
				typeof(Bgra5551),
				PixelFormat.A1R5G5B5UNormPack16,
				img => new ImageSharpImage<Bgra5551>((Image<Bgra5551>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Bgra5551>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Bgra5551>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Bgra5551>(w, h)
			)
			},
			{
				typeof(L16),
				new FormatInfo(
				typeof(L16),
				PixelFormat.L16UNorm,
				img => new ImageSharpImage<L16>((Image<L16>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, L16>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<L16>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<L16>(w, h)
			)
			},
			{
				typeof(L8),
				new FormatInfo(
				typeof(L8),
				PixelFormat.L8UNorm,
				img => new ImageSharpImage<L8>((Image<L8>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, L8>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<L8>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<L8>(w, h)
			)
			},
			{
				typeof(La16),
				new FormatInfo(
				typeof(La16),
				PixelFormat.L8A8UNorm,
				img => new ImageSharpImage<La16>((Image<La16>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, La16>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<La16>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<La16>(w, h)
			)
			},
			{
				typeof(La32),
				new FormatInfo(
				typeof(La32),
				PixelFormat.L16A16UNorm,
				img => new ImageSharpImage<La32>((Image<La32>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, La32>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<La32>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<La32>(w, h)
			)
			},
			{
				typeof(Rg32),
				new FormatInfo(
				typeof(Rg32),
				PixelFormat.R16G16UNorm,
				img => new ImageSharpImage<Rg32>((Image<Rg32>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rg32>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rg32>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rg32>(w, h)
			)
			},
			{
				typeof(Rgb24),
				new FormatInfo(
				typeof(Rgb24),
				PixelFormat.R8G8B8UNorm,
				img => new ImageSharpImage<Rgb24>((Image<Rgb24>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rgb24>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rgb24>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rgb24>(w, h)
			)
			},
			{
				typeof(Rgb48),
				new FormatInfo(
				typeof(Rgb48),
				PixelFormat.R16G16B16UNorm,
				img => new ImageSharpImage<Rgb48>((Image<Rgb48>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rgb48>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rgb48>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rgb48>(w, h)
			)
			},
			{
				typeof(Rgba1010102),
				new FormatInfo(
				typeof(Rgba1010102),
				PixelFormat.A2B10G10R10UNormPack32,
				img => new ImageSharpImage<Rgba1010102>((Image<Rgba1010102>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rgba1010102>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rgba1010102>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rgba1010102>(w, h)
			)
			},
			{
				typeof(Rgba32),
				new FormatInfo(
				typeof(Rgba32),
				PixelFormat.R8G8B8A8UNorm,
				img => new ImageSharpImage<Rgba32>((Image<Rgba32>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rgba32>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rgba32>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rgba32>(w, h)
			)
			},
			{
				typeof(Rgba64),
				new FormatInfo(
				typeof(Rgba64),
				PixelFormat.R16G16B16A16UNorm,
				img => new ImageSharpImage<Rgba64>((Image<Rgba64>)img),
				img => {
					var ptr = MemoryUtil.RecastAs<byte, Rgba64>(img.MapPixels(MapMode.ReadOnly));
					var img2 = Image.LoadPixelData<Rgba64>(ptr.Span, img.Size.X, img.Size.Y);
					img.UnmapPixels();
					return img2;
				},
				(int w, int h) => new Image<Rgba64>(w, h)
			)
			},
		};

		internal static readonly Dictionary<PixelFormat, FormatInfo> pixelFormatToType = pixelTypeToFormat.ToDictionary(item => item.Value.Format, item => item.Value);

		internal static readonly Configuration Config = Configuration.Default.Clone();

		/// <summary>
		/// Adds an image format for use by ImageSharp services.
		/// </summary>
		/// <param name="format">The image format</param>
		/// <param name="encode">An encoder for the format, or null</param>
		/// <param name="decode">A decoder for the format, or null</param>
		/// <param name="detector">A format detector for the format, or null</param>
		public static void AddFormat(IImageFormat format, IImageEncoder? encode, IImageDecoder? decode, IImageFormatDetector? detector) {
			var formatManager = Config.ImageFormatsManager;
			formatManager.AddImageFormat(format);
			if (encode != null) formatManager.SetEncoder(format, encode);
			if (decode != null) formatManager.SetDecoder(format, decode);
			if (detector != null) formatManager.AddImageFormatDetector(detector);
		}

		static ImageSharpService() {
			AddFormat(QOIImageFormat.Instance, new QOIEncoder(), new QOIDecoder(), new QOIImageFormatDetector());
		}

		public bool CanLoad(string mimeType) => Config.ImageFormatsManager.FindFormatByMimeType(mimeType) != null;

		public bool CanSave(string mimeType) => Config.ImageFormatsManager.FindFormatByMimeType(mimeType) != null;

		public IImage Load(ResourceLocation location) {
			using var stream = location.OpenStream();
			return IImageSharpImage.Create(Image.Load(Config, location.OpenStream()));
		}

		public IImage Load(in ReadOnlySpan<byte> binary, string? mimeType) => IImageSharpImage.Create(Image.Load(Config, binary));

		public void Save(IImage image, string mimeType, Stream stream) {
			Image img;
			bool dispose = false;
			if (image is IImageSharpImage isi) img = isi.AbstractImage;
			else {
				img = IImageSharpImage.Create(image).AbstractImage;
				dispose = true;
			}
			try {
				IImageFormat? format = Config.ImageFormatsManager.FindFormatByMimeType(mimeType);
				if (format == null) throw new ArgumentException($"Cannot encode image in unsupported mime type {mimeType}");
				img.Save(stream, format);
			} finally {
				if (dispose) img.Dispose();
			}
		}

	}

}
