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

		public IImage Load(ReadOnlySpan<byte> binary, string? mime) {
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

			uint width = BinaryUtils.ToUInt32(binary, pos), height = BinaryUtils.ToUInt32(binary, pos + 4);
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

		public Span<byte> Save(IImage image, string mimeType) {
			throw new NotImplementedException();
		}

	}

}
