using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Resource;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics {

	public class QOIImageIO : IImageIO {

		public IImage Load(ResourceLocation location) {
			string? mime = location.Metadata.MIMEType;
			if (mime != null && mime != MIME.QOI) throw new ArgumentException("Cannot load a non-QOI image");
			using Stream stream = location.OpenStream();
			return ReadQOI(stream.ReadFully());
		}

		public IImage Load(in ReadOnlySpan<byte> binary, string? mime) {
			if (mime != null && mime != MIME.QOI) throw new ArgumentException("Cannot load a non-QOI image");
			return ReadQOI(binary);
		}

		private static IImage ReadQOI(ReadOnlySpan<byte> binary) {
			int pos = 0;

			if (
				binary[pos++] != (char)'q' ||
				binary[pos++] != (char)'o' ||
				binary[pos++] != (char)'i' ||
				binary[pos++] != (char)'f'
			) throw new InvalidDataException("Magic value does not match QOI format");

			uint width = BinaryUtils.ToUInt32(binary[pos..]), height = BinaryUtils.ToUInt32(binary[(pos + 4)..]);
			pos += 8;
			byte channels = binary[pos++];
			byte colorspace = binary[pos++];

			bool alpha = channels switch {
				3 => false,
				4 => true,
				_ => throw new InvalidDataException("Invalid channels value")
			};
			PixelFormat pxformat = colorspace switch {
				0 => alpha ? PixelFormat.R8G8B8A8UNorm : PixelFormat.R8G8B8UNorm,
				1 => alpha ? PixelFormat.R8G8B8A8SRGB : PixelFormat.R8G8B8SRGB,
				_ => throw new InvalidDataException("Invalid colorspace value")
			};

			SoftwareImage img = new ((int)width, (int)height, pxformat);
			IPointer<byte> pixels = img.MapPixels(MapMode.WriteOnly);

			int npixels = (int)(width * height);

			try {

				Span<Color4b> array = stackalloc Color4b[64];
				int poff = 0;

				Color4b c = new(0, 0, 0, 0xFF);
				array[Hash(c)] = c;

				for (int i = 0; i < npixels; i++) {
					byte op = binary[pos++];

					switch (op) {
						case 0xFE: // QOI_OP_RGB
							c.R = binary[pos++];
							c.G = binary[pos++];
							c.B = binary[pos++];
							break;
						case 0xFF: // QOI_OP_RGBA
							c.R = binary[pos++];
							c.G = binary[pos++];
							c.B = binary[pos++];
							c.A = binary[pos++];
							break;
						default:
							switch(op & 0xC0) {
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
										byte op2 = binary[pos++];
										int drmdg = ((op2 >> 4) & 0xF) - 8;
										int dbmdg = (op2 & 0xF) - 8;
										int dr = dg + drmdg;
										int db = dg + dbmdg;
										c.R += (byte)dr;
										c.G += (byte)dg;
										c.B += (byte)db;
									}
									break;
								case 0b11_000000:
									{ // QOI_OP_RUN
										int run = (op & 0x3F) + 1;
										while(run-- > 0) {
											pixels[poff++] = c.R;
											pixels[poff++] = c.G;
											pixels[poff++] = c.B;
											if (alpha) pixels[poff++] = c.A;
											i++;
										}
									}
									break;
							}
							break;
					}

					pixels[poff++] = c.R;
					pixels[poff++] = c.G;
					pixels[poff++] = c.B;
					if (alpha) pixels[poff++] = c.A;
					array[Hash(c)] = c;
				}

			} finally {
				img.UnmapPixels();
			}

			return img;
		}

		private static int Hash(Color4b color) => (color.R * 3 + color.G * 5 + color.B * 7 + color.A * 11) & 0x3F;

		public void Save(IImage image, string mimeType, Stream stream) {
			if (mimeType != MIME.QOI) throw new ArgumentException("Cannot encode non-QOI format", nameof(mimeType));

			// Write the magic header
			Span<byte> tmp = stackalloc byte[4] {
				(byte)'q',
				(byte)'o',
				(byte)'i',
				(byte)'f'
			};
			stream.Write(tmp);

			// Write the width and height
			int width = image.Size.X, height = image.Size.Y;
			stream.Write(BinaryUtils.WriteBytes(tmp, width));
			stream.Write(BinaryUtils.WriteBytes(tmp, height));
			int npixels = width * height;

			PixelFormat format = image.Format;
			bool hasAlpha = format.HasChannel(ChannelType.Alpha);

			// Check which format we are using and write it
			if (format == PixelFormat.R8G8B8A8UNorm) {
				// 4-byte, linear
				stream.WriteByte(4);
				stream.WriteByte(1);
			} else if (format == PixelFormat.R8G8B8SRGB) {
				// 3-byte, linear
				stream.WriteByte(3);
				stream.WriteByte(1);
			} else if (format == PixelFormat.R8G8B8A8SRGB) {
				// 4-byte, sRGB
				stream.WriteByte(4);
				stream.WriteByte(0);
			} else if (format == PixelFormat.R8G8B8SRGB) {
				// 3-byte, sRGB
				stream.WriteByte(3);
				stream.WriteByte(0);
			} else throw new ArgumentException("Image has unsupported pixel format for QOI encoding", nameof(image));
			
			// Hashtable of already seen colors (only updated by DIFF, LUMA, and RGB(A) packets)
			Span<Color4b> table = stackalloc Color4b[64];
			// Corresponding boolean list for if a hashtable slot is valid
			Span<bool> tableValid = stackalloc bool[64];

			// The last color seen by the encoder
			Color4b c = new(0, 0, 0, 255);
			// The current pixel being encoded
			Color4b nc = new();
			// The number of pixels currently being RLE'd
			int runLen = 0;

			// Initialize the table with the initial color
			int tmp2 = Hash(c);
			table[tmp2] = c;
			tableValid[tmp2] = true;

			try {
				int poff = 0;
				IPointer<byte> pixels = image.MapPixels(MapMode.ReadOnly);

				// For each pixel in the image
				for (int i = 0; i < npixels; i++) {
					nc.R = pixels[poff++];
					nc.G = pixels[poff++];
					nc.B = pixels[poff++];
					if (hasAlpha) nc.A = pixels[poff++];
					else nc.A = 0xFF;

					// Check for RLE capability first
					if (c == nc) runLen++;
					else {
						// Else need to try a different technique
						// Write any remaining RLE packet
						if (runLen != 0) stream.WriteByte((byte)(0xC0 | (runLen - 1)));

						// Generate the hash of the pixel
						int hash = Hash(nc);

						// Next, attempt to use the hash table
						if (table[hash] == nc && tableValid[hash]) stream.WriteByte((byte)hash);
						else {
							// Then, attempt to encode using pixel differences
							sbyte dr = (sbyte)(nc.R - c.R);
							sbyte dg = (sbyte)(nc.G - c.G);
							sbyte db = (sbyte)(nc.B - c.B);
							// Try DIFF packet first
							if (dr >= -2 && dr <= 1 && dg >= -2 && dg <= 1 && db >= -2 && db <= 1) {
								stream.WriteByte((byte)(0x40 | (((dr+2) & 0x3) << 4) | (((dg+2) & 0x3) << 2) | ((db+2) & 0x3)));
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

				// If any RLE encoding remaining, write it
				if (runLen != 0) stream.WriteByte((byte)(0xC0 | (runLen - 1)));

				// Write the footer
				stream.Write(stackalloc byte[] {
					0, 0, 0, 0, 0, 0, 0, 1
				});
			} finally {
				image.UnmapPixels();
			}
		}

	}

}
