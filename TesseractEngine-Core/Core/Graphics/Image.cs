using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Tesseract.Core.Graphics.Accelerated;

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

		/// <summary>
		/// <para>
		/// Gets or sets an individual pixel in the image's memory.
		/// </para>
		/// <para>
		/// This will read values based on the image's pixel format and convert to floating-point as
		/// defined by the number format of each component. The order of elements returned in this
		/// vector is independent of the ordering in memory and is based on the type of format:
		/// <list type="bullet">
		/// <item>
		/// Color - Red, Green, Blue, Alpha, with unused components initialized to 0 except alpha which instead is 1. If
		/// a luminance format is used red, green, and blue will be initialized to the same value when reading and an
		/// implementation-defined RGB to luminance formula is used when writing.
		/// </item>
		/// <item>Depth/Stencil - Depth, Stencil, with the same placement regardless of which components are present, default to 0</item>
		/// </list>
		/// </para>
		/// </summary>
		/// <param name="x">Pixel X coordinate</param>
		/// <param name="y">Pixel Y coordinate</param>
		/// <returns>Value of pixel at the given coordinates</returns>
		public Vector4 this[int x, int y] { get; set; }

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
		/// A span containing the raw bytes of the pixel data.
		/// </summary>
		public Span<byte> Pixels => pixels.Span;

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
			unsafe { return new UnmanagedPointer<byte>((IntPtr)pixelHandle.Value.Pointer, pixels.Length); }
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

		public Vector4 this[int x, int y] {
			get {
				int offset = (y * Size.X + x) * Format.SizeOf;
				Vector4 val = default;
				Span<byte> pixel = Pixels[offset..(offset + Format.SizeOf)];
				switch(Format.Type) {
					case PixelFormatType.Color:
						val.W = 1;
						if (Format.HasChannel(ChannelType.Luminance)) {
							val.X = val.Y = val.Z = (float)Format.ReadChannel(ChannelType.Luminance, pixel);
						} else {
							val.X = (float)Format.ReadChannel(ChannelType.Red, pixel);
							if (Format.HasChannel(ChannelType.Green))
								val.Y = (float)Format.ReadChannel(ChannelType.Green, pixel);
							if (Format.HasChannel(ChannelType.Blue))
								val.Y = (float)Format.ReadChannel(ChannelType.Blue, pixel);
						}
						if (Format.HasChannel(ChannelType.Alpha))
							val.W = (float)Format.ReadChannel(ChannelType.Alpha, pixel);
						break;
					case PixelFormatType.Depth:
					case PixelFormatType.Stencil:
					case PixelFormatType.DepthStencil:
						if (Format.HasChannel(ChannelType.Depth)) val.X = (float)Format.ReadChannel(ChannelType.Depth, pixel);
						if (Format.HasChannel(ChannelType.Stencil)) val.Y = (float)Format.ReadChannel(ChannelType.Stencil, pixel);
						break;
				}
				return val;
			}
			set {
				int offset = (y * Size.X + x) * Format.SizeOf;
				Span<byte> pixel = Pixels[offset..(offset + Format.SizeOf)];
				switch (Format.Type) {
					case PixelFormatType.Color:
						if (Format.HasChannel(ChannelType.Luminance)) {
							byte luma = (byte)(0.299f * value.X + 0.587f * value.Y + 0.114f * value.Z);
							Format.WriteChannel(ChannelType.Luminance, pixel, (decimal)luma);
						} else {
							Format.WriteChannel(ChannelType.Red, pixel, (decimal)value.X);
							if (Format.HasChannel(ChannelType.Green))
								Format.WriteChannel(ChannelType.Green, pixel, (decimal)value.Y);
							if (Format.HasChannel(ChannelType.Blue))
								Format.WriteChannel(ChannelType.Blue, pixel, (decimal)value.Z);
						}
						if (Format.HasChannel(ChannelType.Alpha))
							Format.WriteChannel(ChannelType.Alpha, pixel, (decimal)value.W);
						break;
					case PixelFormatType.Depth:
					case PixelFormatType.Stencil:
					case PixelFormatType.DepthStencil:
						if (Format.HasChannel(ChannelType.Depth)) Format.WriteChannel(ChannelType.Depth, pixel, (decimal)value.X);
						if (Format.HasChannel(ChannelType.Stencil)) Format.WriteChannel(ChannelType.Stencil, pixel, (decimal)value.Y);
						break;
				}
			}
		}
	}

	/// <summary>
	/// An Image IO object provides methods for loading and saving images.
	/// </summary>
	public interface IImageIO {

		/// <summary>
		/// Tests if this IO instance can load images of the given MIME type.
		/// </summary>
		/// <param name="mimeType">MIME type to test</param>
		/// <returns>If images of this type can be loaded</returns>
		public bool CanLoad(string mimeType);

		/// <summary>
		/// Tests if this IO instance can save images of the given MIME type.
		/// </summary>
		/// <param name="mimeType">MIME type to test</param>
		/// <returns>If images of this type can be saved</returns>
		public bool CanSave(string mimeType);

		/// <summary>
		/// Loads an image from the given resource location.
		/// </summary>
		/// <param name="location">Resource location to load from</param>
		/// <returns>Loaded image</returns>
		public IImage Load(ResourceLocation location);

		/// <summary>
		/// Loads an image from memory, optionally specifying a MIME type to describe
		/// the format of the image. If the supplied MIME type is null, the IO will
		/// attempt to determine the image format from the supplied binary.
		/// </summary>
		/// <param name="binary">The raw binary image data</param>
		/// <param name="mimeType">The image MIME type, or null</param>
		/// <returns>Loaded image</returns>
		public IImage Load(in ReadOnlySpan<byte> binary, string? mimeType = null);

		/// <summary>
		/// Saves an image to aa stream in the given format.
		/// </summary>
		/// <param name="image">Image to save</param>
		/// <param name="mimeType">Image format MIME type</param>
		/// <param name="stream">Stream to save the image to</param>
		public void Save(IImage image, string mimeType, Stream stream);

		/// <summary>
		/// Saves an image to memory in the given format.
		/// </summary>
		/// <param name="image">Image to save</param>
		/// <param name="mimeType">Image format MIME type</param>
		/// <returns>Saved image</returns>
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
		public IProcessableImage Convert(PixelFormat format);

		/// <summary>
		/// Performs a copy of a region of pixels ("blit") from a source image to this image.
		/// </summary>
		/// <param name="dstArea">Destination area to copy to</param>
		/// <param name="src">Source image to copy from</param>
		/// <param name="srcPos">Position within source image to copy from</param>
		public void Blit(Recti dstArea, IImage src, IReadOnlyTuple2<int> srcPos);

		/// <summary>
		/// Creates a resized version of this image using nearest-neighbor sampling.
		/// </summary>
		/// <param name="newSize">The size of the resized image</param>
		/// <returns>The resized image</returns>
		public IProcessableImage Resize(IReadOnlyTuple2<int> newSize);

		/// <summary>
		/// Fills a region in this image with a constant color.
		/// </summary>
		/// <param name="dstArea">Destination area to fill</param>
		/// <param name="color">Color to fill with</param>
		public void Fill(Recti dstArea, Vector4 color);

	}

}
