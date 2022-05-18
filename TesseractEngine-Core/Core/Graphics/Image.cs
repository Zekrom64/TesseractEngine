using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Tesseract.Core.Graphics {

	/// <summary>
	/// An image is a 2D collection of pixels stored in accessible memory. Pixels are stored
	/// left to right, top to bottom, with each pixel value immediately after the previous in memory.
	/// Because implementations of this interface may hold pixel data in unmanaged memory, images
	/// should be disposed of after they are no longer needed to avoid memory leaks.
	/// </summary>
	public interface IImage : IDisposable, Services.IServiceProvider {

		/// <summary>
		/// The size of the image in pixels.
		/// </summary>
		public IReadOnlyTuple2<int> Size { get; }

		/// <summary>
		/// The format of each pixel in the image.
		/// </summary>
		public PixelFormat Format { get; }

		/// <summary>
		/// Creates a duplicate of this image.
		/// </summary>
		/// <returns>Duplicate image</returns>
		public IImage Duplicate();

		/// <summary>
		/// Maps the pixel data of this image as a byte pointer, using the given map
		/// mode to determine how the data will be accessed.
		/// </summary>
		/// <param name="mode">Mapping mode for the pixel data</param>
		/// <returns>Pointer to mapped pixel data</returns>
		public IPointer<byte> MapPixels(MapMode mode);

		/// <summary>
		/// Unmaps any mapped pixel data.
		/// </summary>
		public void UnmapPixels();

	}

	/// <summary>
	/// An array image is an image whose pixels are stored in a managed byte array.
	/// </summary>
	public class ArrayImage : IImage {

		public PixelFormat Format { get; }
		public IReadOnlyTuple2<int> Size { get; }

		// Pixel memory
		private readonly Memory<byte> pixels;
		// Pixel memory handle
		private MemoryHandle? pixelHandle;

		/// <summary>
		/// Creates a new array image.
		/// </summary>
		/// <param name="w">Width in pixels</param>
		/// <param name="h">Height in pixels</param>
		/// <param name="format">Format of pixels</param>
		/// <param name="pixels">Existing array to wrap as pixels, or null to create a new array</param>
		/// <exception cref="ArgumentException">If one of the arguments passed is invalid</exception>
		public ArrayImage(int w, int h, PixelFormat format, byte[]? pixels = null) {
			Format = format;
			Size = new Vector2i(w, h);
			int nBytes = w * h * format.SizeOf;
			if (pixels == null) pixels = new byte[nBytes];
			else if (pixels.Length < nBytes) throw new ArgumentException("Pixel data array is too small for requested image size and format");
			this.pixels = pixels;
		}

		/// <summary>
		/// Creates a new array image.
		/// </summary>
		/// <param name="size">Size in picels</param>
		/// <param name="format">Format of pixels</param>
		/// <param name="pixels">Existing array to wrap as pixels, or null to create a new array</param>
		public ArrayImage(IReadOnlyTuple2<int> size, PixelFormat format, byte[]? pixels = null) : this(size.X, size.Y, format, pixels) { }

		public IImage Duplicate() {
			ArrayImage newImage = new(Size, Format);
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

		public bool CanLoad(string mimeType);

		public bool CanSave(string mimeType);

		public IImage Load(ResourceLocation location);

		public IImage Load(in ReadOnlySpan<byte> binary, string? mimeType);

		public void Save(IImage image, string mimeType, Stream stream);

		public byte[] Save(IImage image, string mimeType) {
			using MemoryStream ms = new();
			Save(image, mimeType, ms);
			return ms.ToArray();
		}

	}

	/// <summary>
	/// A processable image is an image which supports additional operations for manipulating its contents.
	/// </summary>
	public interface IProcessableImage : IImage {

		/// <summary>
		/// Converts this image to a new image of a different pixel format.
		/// </summary>
		/// <param name="format">New pixel format</param>
		/// <returns>Converted image</returns>
		public IImage Convert(PixelFormat format);

		/// <summary>
		/// Performs a copy of a region of pixels ("blit") from a source image to this image.
		/// </summary>
		/// <param name="dstArea">Destination area to copy to</param>
		/// <param name="src">Source image to copy from</param>
		/// <param name="srcPos">Position within source image to copy from</param>
		public void Blit(IReadOnlyRect<int> dstArea, IImage src, IReadOnlyTuple2<int> srcPos);

		/// <summary>
		/// Fills a region in this image with a constant color.
		/// </summary>
		/// <param name="dstArea">Destination area to fill</param>
		/// <param name="color">Color to fill with</param>
		public void Fill(IReadOnlyRect<int> dstArea, IReadOnlyColor color);

	}

}
