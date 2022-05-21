using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Resource;
using Tesseract.Core.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Tesseract.Core.Graphics.QOI {

	/// <summary>
	/// Enum of QOI channel values.
	/// </summary>
	public enum QOIChannels : byte {
		/// <summary>
		/// RGB channels.
		/// </summary>
		RGB = 3,
		/// <summary>
		/// RGBA channels.
		/// </summary>
		RGBA = 4
	}

	/// <summary>
	/// Enum of QOI colorspace values.
	/// </summary>
	public enum QOIColorspace {
		/// <summary>
		/// sRGB colorspace with linear alpha.
		/// </summary>
		SRGB = 0,
		/// <summary>
		/// Linear colorspace.
		/// </summary>
		Linear = 1
	}

	/// <summary>
	/// An <see cref="ImageIO"/> implementation for Quite Ok Image (QOI) encoded files.
	/// </summary>
	public class QOIImageFormat : IImageFormat {

		/// <summary>
		/// Instance of the QOI format object.
		/// </summary>
		public static readonly QOIImageFormat Instance = new();

		public string Name => "QOI";

		public string DefaultMimeType => MIME.QOI;

		public IEnumerable<string> MimeTypes {
			get {
				yield return MIME.QOI;
			}
		}

		public IEnumerable<string> FileExtensions {
			get {
				yield return "qoi";
			}
		}

		private QOIImageFormat() { }

		// Hash function for QOI colors
		internal static int Hash(Rgba32 color) => (color.R * 3 + color.G * 5 + color.B * 7 + color.A * 11) & 0x3F;

	}

	public class QOIEncoder : IImageEncoder {

		public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
			// Write the magic header
			Span<byte> tmp = stackalloc byte[4] {
				(byte)'q',
				(byte)'o',
				(byte)'i',
				(byte)'f'
			};
			stream.Write(tmp);

			// Write the width and height
			int width = image.Width, height = image.Height;
			stream.Write(BinaryUtils.WriteBytes(tmp, width));
			stream.Write(BinaryUtils.WriteBytes(tmp, height));
			int npixels = width * height;


			bool hasAlpha = false;

			// Check which format we are using and write it
			if (typeof(TPixel) == typeof(Rgba32)) {
				// 4-byte, linear
				stream.WriteByte(4);
				stream.WriteByte(1);
				hasAlpha = true;
			} else if (typeof(TPixel) == typeof(Rgb24)) {
				// 3-byte, linear
				stream.WriteByte(3);
				stream.WriteByte(1);
			} else throw new ArgumentException("Image has unsupported pixel format for QOI encoding", nameof(image));

			// For each pixel in the image (top -> bottom, left -> right)
			image.ProcessPixelRows(accesor => {
				// The number of pixels currently being RLE'd
				int runLen = 0;

				// The last color seen by the encoder
				Rgba32 c = new(0, 0, 0, 255);
				// The current pixel being encoded
				Rgba32 nc = new();

				// Hashtable of already seen colors (only updated by DIFF, LUMA, and RGB(A) packets)
				Span<Rgba32> table = stackalloc Rgba32[64];
				// Corresponding boolean list for if a hashtable slot is valid
				Span<bool> tableValid = stackalloc bool[64];

				// Initialize the table with the initial color
				int tmp2 = QOIImageFormat.Hash(c);
				table[tmp2] = c;
				tableValid[tmp2] = true;

				for (int y = 0; y < height; y++) {
					Span<TPixel> row = accesor.GetRowSpan(y);
					for(int x = 0; x < width; x++) {
						row[x].ToRgba32(ref nc);
						if (!hasAlpha) nc.A = 0xFF;

						// Check for RLE capability first
						if (c == nc) runLen++;
						else {
							// Else need to try a different technique
							// Write any remaining RLE packet
							if (runLen != 0) stream.WriteByte((byte)(0xC0 | (runLen - 1)));

							// Generate the hash of the pixel
							int hash = QOIImageFormat.Hash(nc);

							// Next, attempt to use the hash table
							if (table[hash] == nc && tableValid[hash]) stream.WriteByte((byte)hash);
							else {
								// Then, attempt to encode using pixel differences
								sbyte dr = (sbyte)(nc.R - c.R);
								sbyte dg = (sbyte)(nc.G - c.G);
								sbyte db = (sbyte)(nc.B - c.B);
								// Try DIFF packet first
								if (dr >= -2 && dr <= 1 && dg >= -2 && dg <= 1 && db >= -2 && db <= 1) {
									stream.WriteByte((byte)(0x40 | (((dr + 2) & 0x3) << 4) | (((dg + 2) & 0x3) << 2) | ((db + 2) & 0x3)));
								} else {
									// Then try LUMA packet
									sbyte drmdg = (sbyte)(dr - dg);
									sbyte dbmdg = (sbyte)(db - dg);
									if (dg >= -32 && dg <= 31 && drmdg >= -8 && drmdg <= 7 && drmdg >= -8 && drmdg <= 7) {
										stream.WriteByte((byte)(0x80 | ((dg + 32) & 0x3F)));
										stream.WriteByte((byte)(
											(((drmdg) + 8) << 4) |
											((dbmdg + 8) & 0xF)
										));
									} else {
										// Finally, resort to an RGB(A) packet
										if (nc.A == c.A) {
											stream.WriteByte(0xFE);
											stream.WriteByte(nc.R);
											stream.WriteByte(nc.G);
											stream.WriteByte(nc.B);
										} else {
											stream.WriteByte(0xFF);
											stream.WriteByte(nc.R);
											stream.WriteByte(nc.G);
											stream.WriteByte(nc.B);
											stream.WriteByte(nc.A);
										}
									}
								}
								// Update hashtable with newest color
								table[hash] = nc;
								tableValid[hash] = true;
							}
						}

						// Handle RLE length limit of 62
						if (runLen == 62) {
							stream.WriteByte(0xFD);
							runLen = 0;
						}
					}
				}

				// If any RLE encoding remaining, write it
				if (runLen != 0) stream.WriteByte((byte)(0xC0 | (runLen - 1)));
			});

			// Write the footer
			stream.Write(stackalloc byte[] {
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1
			});
		}

		public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> =>
			Task.Run(() => Encode(image, stream), cancellationToken);

	}

	public class QOIDecoder : IImageDecoder {

		// Decodes the contents of a QOI image into an ImageSharp image of the given pixel format
		private Image DecodePixels<TPixel>(Stream stream, Image<TPixel> img) where TPixel : unmanaged, IPixel<TPixel> {
			byte NextByte() {
				int val = stream.ReadByte();
				if (val == -1) throw new IOException("Unexpected end of stream");
				return (byte)val;
			}

			img.ProcessPixelRows(access => {
				Rgba32 c = new();
				TPixel px = new();

				PixelAlphaRepresentation alphaRep = img.PixelType.AlphaRepresentation ?? PixelAlphaRepresentation.None;
				bool hasAlpha = alphaRep != PixelAlphaRepresentation.None;

				Span<Rgba32> array = stackalloc Rgba32[64];
				array[QOIImageFormat.Hash(c)] = c;
				int run = 0;

				for (int y = 0; y < access.Height; y++) {
					Span<TPixel> row = access.GetRowSpan(y);
					for(int x = 0; x < access.Width; x++) {
						// If has remaining pixels in RLE, use that instead
						if (run > 0) {
							row[x] = px;
							run--;
							continue;
						}

						byte op = NextByte();

						switch (op) {
							case 0xFE: // QOI_OP_RGB
								c.R = NextByte();
								c.G = NextByte();
								c.B = NextByte();
								break;
							case 0xFF: // QOI_OP_RGBA
								c.R = NextByte();
								c.G = NextByte();
								c.B = NextByte();
								c.A = NextByte();
								break;
							default:
								switch (op & 0xC0) {
									case 0b00_000000: // QOI_OP_INDEX
										c = array[op & 0x3F];
										break;
									case 0b01_000000: // QOI_OP_DIFF
										{
											int dr = ((op >> 4) & 0x3) - 2;
											int dg = ((op >> 2) & 0x3) - 2;
											int db = (op & 0x3) - 2;
											c.R += (byte)dr;
											c.G += (byte)dg;
											c.B += (byte)db;
										}
										break;
									case 0b10_000000: // QOI_OP_LUMA
										{
											int dg = (op & 0x3F) - 32;
											byte op2 = NextByte();
											int drmdg = ((op2 >> 4) & 0xF) - 8;
											int dbmdg = (op2 & 0xF) - 8;
											int dr = dg + drmdg;
											int db = dg + dbmdg;
											c.R += (byte)dr;
											c.G += (byte)dg;
											c.B += (byte)db;
										}
										break;
									case 0b11_000000: // QOI_OP_RUN
										run = (op & 0x3F); // Current pixel will be stored, remaining run length is read
										break;
								}
								break;
						}

						// Store decoded pixel, and update array
						px.FromRgba32(c);
						row[x] = px;
						array[QOIImageFormat.Hash(c)] = c;
					}
				}
			});

			return img;
		}

		private struct Header {

			public uint Width;
			public uint Height;
			public QOIChannels Channels;
			public QOIColorspace Colorspace;

		}

		private Header ReadHeader(Stream stream) {
			Header header = new();
			Span<byte> buf = stackalloc byte[4];

			stream.ReadFully(buf);
			if (
				buf[0] != 'q' ||
				buf[1] != 'o' ||
				buf[2] != 'i' ||
				buf[3] != 'f'
			) throw new InvalidDataException("Magic value does not match QOI format");

			stream.ReadFully(buf);
			header.Width = BinaryUtils.ToUInt32(buf, false);
			stream.ReadFully(buf);
			header.Height = BinaryUtils.ToUInt32(buf, false);
			int nextb = stream.ReadByte();
			if (nextb == -1) throw new IOException("Unexpected end of stream");
			header.Channels = (QOIChannels)nextb;
			nextb = stream.ReadByte();
			if (nextb == -1) throw new IOException("Unexpected end of stream");
			header.Colorspace = (QOIColorspace)nextb;

			return header;
		}

		public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
			Header header = ReadHeader(stream);
			Image<TPixel> img = new((int)header.Width, (int)header.Height);
			DecodePixels(stream, img);
			return img;
		}

		public Image Decode(Configuration configuration, Stream stream, CancellationToken cancellationToken) {
			Header header = ReadHeader(stream);
			Image img = header.Channels switch {
				QOIChannels.RGB => DecodePixels(stream, new Image<Rgb24>((int)header.Width, (int)header.Height)),
				QOIChannels.RGBA => DecodePixels(stream, new Image<Rgba32>((int)header.Width, (int)header.Height)),
				_ => throw new InvalidDataException("Unknown QOI channel value"),
			};
			return img;
		}

	}

	public class QOIImageFormatDetector : IImageFormatDetector {

		public int HeaderSize => 4;

		public IImageFormat? DetectFormat(ReadOnlySpan<byte> header) =>
			(header.Length >= 4 &&
			header[0] == 'q' &&
			header[1] == 'o' &&
			header[2] == 'i' &&
			header[3] == 'f') ? QOIImageFormat.Instance : null;

	}

}
