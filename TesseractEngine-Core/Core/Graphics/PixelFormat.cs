using System;
using System.Collections.Generic;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics {

	/// <summary>
	/// Enumeration of pixel format channel types.
	/// </summary>
	public enum ChannelType {
		/// <summary>
		/// Red color component channel.
		/// </summary>
		Red,
		/// <summary>
		/// Green color component channel.
		/// </summary>
		Green,
		/// <summary>
		/// Blue color component channel.
		/// </summary>
		Blue,
		/// <summary>
		/// Alpha color component channel.
		/// </summary>
		Alpha,
		/// <summary>
		/// Depth value channel.
		/// </summary>
		Depth,
		/// <summary>
		/// Stencil value channel.
		/// </summary>
		Stencil,
		/// <summary>
		/// Luminance channel.
		/// </summary>
		Luminance,
		/// <summary>
		/// Exponent channel (for floating-point formats with a shared exponent).
		/// </summary>
		Exponent
	}

	/// <summary>
	/// Enumeration of pixel format channel numeric types.
	/// </summary>
	public enum ChannelNumberFormat {
		/// <summary>
		/// Number format is not defined.
		/// </summary>
		Undefined,
		/// <summary>
		/// Unsigned integers of channel size normalized to between 0.0 and 1.0.
		/// </summary>
		UnsignedNorm,
		/// <summary>
		/// Signed integers of channel size normalized to between -1.0 and 1.0.
		/// </summary>
		SignedNorm,
		/// <summary>
		/// Unsigned integers of channel size converted to floating point equivalent values.
		/// </summary>
		UnsignedScaled,
		/// <summary>
		/// Signed integers of channel size converted to floating point equivalent values.
		/// </summary>
		SignedScaled,
		/// <summary>
		/// Unsigned integers of channel size.
		/// </summary>
		UnsignedInt,
		/// <summary>
		/// Signed integers of channel size.
		/// </summary>
		SignedInt,
		/// <summary>
		/// Unsigned floating point numbers of channel size.
		/// </summary>
		UnsignedFloat,
		/// <summary>
		/// Signed floating point numbers of channel size.
		/// </summary>
		SignedFloat,
		/// <summary>
		/// Similar to <see cref="UnsignedNorm"/> but red, green, and blue components are scaled using sRGB nonlinear encoding while alpha is unchanged.
		/// </summary>
		SRGB
	}

	/// <summary>
	/// A pixel format channel 
	/// </summary>
	public struct PixelChannel : IEquatable<PixelChannel> {

		/// <summary>
		/// The type of this channel.
		/// </summary>
		public ChannelType Type { get; init; }

		/// <summary>
		/// <para>The offset of this channel in the overall pixel format.</para>
		/// <para>
		/// This value depends on if the overall format is packed; if so it is in bits, else it is in bytes. If
		/// this value is -1 the format is opaque and the exact offset of the component cannot be determined.
		/// </para>
		/// </summary>
		public int Offset { get; init; }

		/// <summary>
		/// The size of this channel.
		/// <para>
		/// This value depends on if the overall format is packed; if so it is in bits, else it is in bytes.
		/// </para>
		/// </summary>
		public int Size { get; init; }

		/// <summary>
		/// The number format of values in this channel.
		/// </summary>
		public ChannelNumberFormat NumberFormat { get; init; }

		public bool Equals(PixelChannel other) => Type == other.Type && Offset == other.Offset && Size == other.Size && NumberFormat == other.NumberFormat;

		public override bool Equals(object? obj) => obj is PixelChannel channel && Equals(channel);

		public static bool operator ==(PixelChannel left, PixelChannel right) => left.Equals(right);

		public static bool operator !=(PixelChannel left, PixelChannel right) => !(left == right);

		public override int GetHashCode() => Offset ^ Size ^ ((int)Type << 8) ^ ((int)NumberFormat << 12);
	}

	/// <summary>
	/// Enumeration of pixel format types.
	/// </summary>
	public enum PixelFormatType {
		/// <summary>
		/// Color pixel format.
		/// </summary>
		Color,
		/// <summary>
		/// Depth pixel format.
		/// </summary>
		Depth,
		/// <summary>
		/// Stencil pixel format.
		/// </summary>
		Stencil,
		/// <summary>
		/// Combined depth-stencil format.
		/// </summary>
		DepthStencil
	}

	/// <summary>
	/// A pixel format describes a mapping between binary data and numeric values stored by the format.
	/// <para>
	/// The pixel format class has many predefined formats with a common naming convention. First, the components are
	/// listed with their size in bits, followed by a number format that the channels use. If the name ends with "Pack",
	/// this means the pixel format is packed into bit fields in an integer of the bit size of the following number. The order of
	/// the components in the name corresponds to their order in the pixel memory; for unpacked formats they are ordered ascending
	/// in memory, and for unpacked formats they are ordered descending from the most significant bit of the packed integer.
	/// </para>
	/// </summary>
	public sealed record PixelFormat : IEquatable<PixelFormat> {

		private int HashCode { get; init; }

		/// <summary>
		/// If the channel values are "packed" (in bitfields or encoded values) or raw unpacked values.
		/// </summary>
		public bool Packed { get; init; }

		/// <summary>
		/// The channels defined in the format.
		/// </summary>
		public IReadOnlyList<PixelChannel> Channels { get; init; } = Collections<PixelChannel>.EmptyList;

		/// <summary>
		/// The size of the pixel format in bytes.
		/// </summary>
		public int SizeOf { get; init; }

		/// <summary>
		/// The type of pixel format this is.
		/// </summary>
		public PixelFormatType Type { get; init; }

		/// <summary>
		/// The common number format of the pixel format's channels, or <see cref="ChannelNumberFormat.Undefined"/> if channel number formats differ.
		/// </summary>
		public ChannelNumberFormat NumberFormat { get; init; }

		/// <summary>
		/// If the number format is floating-point.
		/// </summary>
		public bool IsNumberFormatFloating => NumberFormat == ChannelNumberFormat.SignedFloat || NumberFormat == ChannelNumberFormat.UnsignedFloat;

		/// <summary>
		/// If the number format is normalized.
		/// </summary>
		public bool IsNumberFormatNormalized => NumberFormat == ChannelNumberFormat.UnsignedNorm || NumberFormat == ChannelNumberFormat.SignedNorm;

		private PixelFormat() { }

		/// <summary>
		/// Tests if this pixel format has a channel of the given type.
		/// </summary>
		/// <param name="type">Channel type to test for</param>
		/// <returns>If the format contains the channel</returns>
		public bool HasChannel(ChannelType type) {
			foreach (PixelChannel channel in Channels) if (channel.Type == type) return true;
			return false;
		}

		public bool Equals(PixelFormat? other) {
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			if (HashCode != other.HashCode) return false;

			if (Packed != other.Packed) return false;

			foreach (PixelChannel channel in Channels) {
				bool hasChannel = false;
				foreach (PixelChannel otherChannel in other.Channels) {
					if (otherChannel == channel) {
						hasChannel = true;
						break;
					}
				}
				if (!hasChannel) return false;
			}

			return true;
		}

		public override int GetHashCode() => HashCode;

		/// <summary>
		/// Tests if a pixel format is bitwise compatible with this format. This
		/// only tests if each channel in the given format has a corresponding mapping in
		/// this format, not whether the numeric formats or channels are the same.
		/// </summary>
		/// <param name="other">Format to test with</param>
		/// <returns></returns>
		public bool IsCompatible(PixelFormat other) {
			if (Packed ^ other.Packed) return false; // Could *technically* test but bit-ordering makes things complicated.
			if (SizeOf != other.SizeOf) return false;
			foreach (PixelChannel channel in other.Channels) {
				bool hasChannel = false;
				foreach (PixelChannel ch2 in Channels) {
					if (ch2.Offset == channel.Offset && ch2.Size == channel.Size) {
						hasChannel = true;
						break;
					}
				}
				if (!hasChannel) return false;
			}
			return true;
		}

		// Deduces other pixel format properties from the given set of pixel channels
		private static void DeducePropertiesFromChannels(PixelChannel[] channels, bool packed, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat, out int channelHash) {
			channelHash = 0;
			// If packed format channel size values are in bits else in bytes
			int channelWidth = packed ? 1 : 8;
			int bitSize = 0;
			numberFormat = channels[0].NumberFormat;
			formatType = PixelFormatType.Color;
			foreach (PixelChannel channel in channels) {
				channelHash = (channelHash << 6) ^ channel.GetHashCode();
				// Deduce common number format or default to undefined
				if (numberFormat != ChannelNumberFormat.Undefined && channel.NumberFormat != numberFormat) numberFormat = ChannelNumberFormat.Undefined;
				// Increment bit size of format
				bitSize += channel.Size * channelWidth;
				// Deduce format type given channel types
				switch (channel.Type) {
					case ChannelType.Red or ChannelType.Green or ChannelType.Blue or ChannelType.Alpha:
						formatType = PixelFormatType.Color;
						break;
					case ChannelType.Depth:
						if (formatType == PixelFormatType.Stencil) formatType = PixelFormatType.DepthStencil;
						else formatType = PixelFormatType.Depth;
						break;
					case ChannelType.Stencil:
						if (formatType == PixelFormatType.Depth) formatType = PixelFormatType.DepthStencil;
						else formatType = PixelFormatType.Stencil;
						break;
				}
			}
			// Byte size of format is bit size / 8 rounded up
			byteSize = bitSize / 8;
			if ((bitSize % 8) != 0) byteSize++;
		}

		/// <summary>
		/// Defines an unpacked format using the given channels.
		/// </summary>
		/// <param name="channels">Pixel format channels</param>
		/// <returns>Unpacked pixel format</returns>
		public static PixelFormat DefineUnpackedFormat(params PixelChannel[] channels) {
			DeducePropertiesFromChannels(channels, false, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat, out int channelHash);
			return new PixelFormat() {
				Packed = false,
				Channels = new List<PixelChannel>(channels),
				SizeOf = byteSize,
				Type = formatType,
				NumberFormat = numberFormat,

				HashCode = channelHash ^ (byteSize << 8) ^ (((int)formatType) << 4) ^ ((int)numberFormat)
			};
		}

		/// <summary>
		/// Defines a packed format using the given channels.
		/// </summary>
		/// <param name="channels">Pixel format channels</param>
		/// <returns>Packed pixel format</returns>
		public static PixelFormat DefinePackedFormat(params PixelChannel[] channels) {
			DeducePropertiesFromChannels(channels, true, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat, out int channelHash);
			return new PixelFormat() {
				Packed = true,
				Channels = new List<PixelChannel>(channels),
				SizeOf = byteSize,
				Type = formatType,
				NumberFormat = numberFormat,

				HashCode = ~(channelHash ^ (byteSize << 8) ^ (((int)formatType) << 4) ^ ((int)numberFormat))
			};
		}

		/// <summary>
		/// Defines a packed format using the given channels.
		/// </summary>
		/// <param name="byteSize">Explicit size of the pixel format in bytes</param>
		/// <param name="channels">Pixel format channels</param>
		/// <returns>Packed pixel format</returns>
		public static PixelFormat DefinePackedFormat(int byteSize, params PixelChannel[] channels) {
			DeducePropertiesFromChannels(channels, true, out int _, out PixelFormatType formatType, out ChannelNumberFormat numberFormat, out int channelHash);
			return new PixelFormat() {
				Packed = true,
				Channels = new List<PixelChannel>(channels),
				SizeOf = byteSize,
				Type = formatType,
				NumberFormat = numberFormat,

				HashCode = ~(channelHash ^ (byteSize << 8) ^ (((int)formatType) << 4) ^ ((int)numberFormat))
			};
		}

		//=======================//
		// Unpacked RGB formats //
		//=======================//

		/// <summary>
		/// A pixel format with an 8-bit unsigned normalized red channel.
		/// </summary>
		public static readonly PixelFormat R8UNorm = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm });

		/// <summary>
		/// A pixel format with an 8-bit signed normalized red channel.
		/// </summary>
		public static readonly PixelFormat R8SNorm = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm });

		/// <summary>
		/// A pixel format with an 8-bit unsigned scaled red channel.
		/// </summary>
		public static readonly PixelFormat R8UScaled = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled });

		/// <summary>
		/// A pixel format with an 8-bit signed scaled red channel.
		/// </summary>
		public static readonly PixelFormat R8SScaled = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled });

		/// <summary>
		/// A pixel format with an 8-bit unsigned integer red channel.
		/// </summary>
		public static readonly PixelFormat R8UInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt });

		/// <summary>
		/// A pixel format with an 8-bit signed integer red channel.
		/// </summary>
		public static readonly PixelFormat R8SInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt });

		/// <summary>
		/// A pixel format with an 8-bit sRGB red channel.
		/// </summary>
		public static readonly PixelFormat R8SRGB = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB });

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit signed normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned scaled red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit signed scaled red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit signed integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit sRGB red and green channels.
		/// </summary>
		public static readonly PixelFormat R8G8SRGB = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit signed normalized red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned scaled red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit signed scaled red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit signed integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit sRGB red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8SRGB = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit signed normalized blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned scaled blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit signed scaled blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned integer blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit signed integer blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit sRGB blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8SRGB = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit signed normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned scaled red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit signed scaled red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit signed integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit sRGB red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8SRGB = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit signed normalized blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned scaled blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit signed scaled blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned integer blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit signed integer blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 8-bit sRGB blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B8G8R8A8SRGB = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Red, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized alpha, red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A8R8G8B8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with a 16-bit unsigned normalized red channel.
		/// </summary>
		public static readonly PixelFormat R16UNorm = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm });

		/// <summary>
		/// A pixel format with a 16-bit signed normalized red channel.
		/// </summary>
		public static readonly PixelFormat R16SNorm = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm });

		/// <summary>
		/// A pixel format with a 16-bit unsigned scaled red channel.
		/// </summary>
		public static readonly PixelFormat R16UScaled = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled });

		/// <summary>
		/// A pixel format with a 16-bit signed scaled red channel.
		/// </summary>
		public static readonly PixelFormat R16SScaled = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled });

		/// <summary>
		/// A pixel format with a 16-bit unsigned integer red channel.
		/// </summary>
		public static readonly PixelFormat R16UInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt });

		/// <summary>
		/// A pixel format with a 16-bit signed integer red channel.
		/// </summary>
		public static readonly PixelFormat R16SInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt });

		/// <summary>
		/// A pixel format with a 16-bit signed floating-point red channel.
		/// </summary>
		public static readonly PixelFormat R16SFloat = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat });

		/// <summary>
		/// A pixel format with 16-bit unsigned normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit signed normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned scaled red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit signed scaled red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed floating-point red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned normalized red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit signed normalized red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned scaled red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit signed scaled red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed floating-point red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit signed normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16SNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned scaled red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16UScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit signed scaled red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16SScaled = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A pixel format with 16-bit unsigned integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 16-bit signed floating-point red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R16G16B16A16SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 6, Size = 2, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with a 32-bit unsigned integer red channel.
		/// </summary>
		public static readonly PixelFormat R32UInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt });

		/// <summary>
		/// A pixel format with a 32-bit signed integer red channel.
		/// </summary>
		public static readonly PixelFormat R32SInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt });

		/// <summary>
		/// A pixel format with a 32-bit signed floating-point red channel.
		/// </summary>
		public static readonly PixelFormat R32SFloat = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat });

		/// <summary>
		/// A pixel format with 32-bit unsigned integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R32G32UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R32G32SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed floating-point red and green channels.
		/// </summary>
		public static readonly PixelFormat R32G32SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 32-bit unsigned integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed floating-point red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 32-bit unsigned integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32A32UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32A32SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 32-bit signed floating-point red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R32G32B32A32SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with a 64-bit unsigned integer red channel.
		/// </summary>
		public static readonly PixelFormat R64UInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt });

		/// <summary>
		/// A pixel format with a 64-bit signed integer red channel.
		/// </summary>
		public static readonly PixelFormat R64SInt = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt });

		/// <summary>
		/// A pixel format with a 64-bit signed floating-point red channel.
		/// </summary>
		public static readonly PixelFormat R64SFloat = DefineUnpackedFormat(new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat });

		/// <summary>
		/// A pixel format with 64-bit unsigned integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R64G64UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed integer red and green channels.
		/// </summary>
		public static readonly PixelFormat R64G64SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed floating-point red and green channels.
		/// </summary>
		public static readonly PixelFormat R64G64SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 64-bit unsigned integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed integer red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed floating-point red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with 64-bit unsigned integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64A64UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed integer red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64A64SInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A pixel format with 64-bit signed floating-point red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R64G64B64A64SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		//====================//
		// Packed RGB formats //
		//====================//

		/// <summary>
		/// A packed 8-bit pixel format with 4-bit unsigned normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R4G4UNormPack8 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 4, Size = 3, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 0, Size = 3, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with 4-bit unsigned normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R4G4B4A4UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with 4-bit unsigned normalized blue, green, red, and alpha channels.
		/// </summary>
		public static readonly PixelFormat B4G4R4A4UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with 4-bit unsigned normalized alpha, red, blue, and green channels.
		/// </summary>
		public static readonly PixelFormat A4R4G4B4UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 12, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 8, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 4, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 4, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with unsigned normalized components, using 5-bit red and blue and 6-bit green channels.
		/// </summary>
		public static readonly PixelFormat R5G6B5UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 6, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with unsigned normalized components, using 5-bit blue and red and 6-bit green channels.
		/// </summary>
		public static readonly PixelFormat B5G6R5UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 6, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with unsigned normalized components, using 5-bit red, green, and blue and 1-bit alpha components.
		/// </summary>
		public static readonly PixelFormat R5G5B5A1UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 6, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 1, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with unsigned normalized components, using 5-bit blue, green, and red and 1-bit alpha components.
		/// </summary>
		public static readonly PixelFormat B5G5R5A1UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 11, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 6, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 1, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 16-bit pixel format with unsigned normalized components, using 1-bit alpha and 5-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A1R5G5B5UNormPack16 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 15, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 10, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 5, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 5, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit unsigned normalized alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8UNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit signed normalized alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8SNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit unsigned scaled alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8UScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit signed scaled alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8SScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit unsigned integer alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8UIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit signed integer alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8SIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 8-bit sRGB alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8SRGBPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 24, Size = 8, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 16, Size = 8, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Green, Offset = 8, Size = 8, NumberFormat = ChannelNumberFormat.SRGB },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 8, NumberFormat = ChannelNumberFormat.SRGB }
		);

		/// <summary>
		/// A packed 32-bit pixel format with 16-bit unsigned normalized red and green channels.
		/// </summary>
		public static readonly PixelFormat R16G16UNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 16, Size = 16, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 0, Size = 16, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned normalized components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10UNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed normalized components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10SNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned scaled components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10UScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed scaled components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10SScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned integer components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10UIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed integer components, using 2-bit alpha and 10-bit red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat A2R10G10B10SIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned normalized components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10UNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed normalized components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10SNormPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedNorm }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned scaled components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10UScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed scaled components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10SScaledPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedScaled }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned integer components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10UIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with signed integer components, using 2-bit alpha and 10-bit blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A2B10G10R10SIntPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 30, Size = 2, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 20, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Green, Offset = 10, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 10, NumberFormat = ChannelNumberFormat.SignedInt }
		);

		/// <summary>
		/// A packed 32-bit pixel format with unsigned floating-point components, using 10-bit blue and 11-bit green and red channels.
		/// </summary>
		public static readonly PixelFormat B10G11R11UFloatPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Blue, Offset = 22, Size = 10, NumberFormat = ChannelNumberFormat.UnsignedFloat },
			new PixelChannel() { Type = ChannelType.Green, Offset = 11, Size = 11, NumberFormat = ChannelNumberFormat.UnsignedFloat },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 11, NumberFormat = ChannelNumberFormat.UnsignedFloat }
		);

		//=======================//
		// Depth/Stencil Formats //
		//=======================//

		/// <summary>
		/// A pixel format with a 16-bit unsigned normalized depth channel.
		/// </summary>
		public static readonly PixelFormat D16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed pixel format with a 24-bit unsigned normalized depth channel.
		/// </summary>
		public static readonly PixelFormat X8D24UNorm = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 24, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A pixel format with a 32-bit signed floating point depth channel.
		/// </summary>
		public static readonly PixelFormat D32SFloat = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 4, NumberFormat = ChannelNumberFormat.SignedFloat }
		);

		/// <summary>
		/// A pixel format with an 8-bit unsigned integer stencil channel.
		/// </summary>
		public static readonly PixelFormat S8UInt = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Stencil, Offset = -1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed pixel format with a 16-bit unsigned normalized depth channel and an 8-bit unsigned integer stencil channel.
		/// </summary>
		public static readonly PixelFormat D16UNormS8UInt = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 16, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Stencil, Offset = -1, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed pixel format with a 24-bit unsigned normalized depth channel and an 8-bit unsigned integer stencil channel.
		/// </summary>
		public static readonly PixelFormat D24UNormS8UInt = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 24, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Stencil, Offset = -1, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		/// <summary>
		/// A packed pixel format with a 32-bit signed floating point depth channel and an 8-bit unsigned integer stencil channel.
		/// </summary>
		public static readonly PixelFormat D32SFloatS8UInt = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Depth, Offset = -1, Size = 32, NumberFormat = ChannelNumberFormat.SignedFloat },
			new PixelChannel() { Type = ChannelType.Stencil, Offset = -1, Size = 8, NumberFormat = ChannelNumberFormat.UnsignedInt }
		);

		//=============================//
		// Luminance/Grayscale formats //
		//=============================//

		/// <summary>
		/// An unpacked pixel format with an 8-bit unsigned normalized luminance channel.
		/// </summary>
		public static readonly PixelFormat L8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Luminance, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// An unpacked pixel format with an 16-bit unsigned normalized luminance channel.
		/// </summary>
		public static readonly PixelFormat L16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Luminance, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// An unpacked pixel format with 8-bit unsigned normalized luminance and alpha channels.
		/// </summary>
		public static readonly PixelFormat L8A8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Luminance, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// An unpacked pixel format with 16-bit unsigned normalized luminance and alpha channels.
		/// </summary>
		public static readonly PixelFormat L16A16UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Luminance, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 2, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		//=======================//
		// Miscellaneous formats //
		//=======================//

		/// <summary>
		/// An unpacked pixel format with an 8-bit unsigned normalized alpha channel.
		/// </summary>
		public static readonly PixelFormat A8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

		/// <summary>
		/// A packed pixel format with unsigned floating-point components using a shared 5-bit exponent and 9-bit red, green, and blue mantissas.
		/// </summary>
		public static readonly PixelFormat E5B9G9R9UFloatPack32 = DefinePackedFormat(
			new PixelChannel() { Type = ChannelType.Exponent, Offset = 27, Size = 5, NumberFormat = ChannelNumberFormat.Undefined },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 18, Size = 9, NumberFormat = ChannelNumberFormat.Undefined },
			new PixelChannel() { Type = ChannelType.Green, Offset = 9, Size = 9, NumberFormat = ChannelNumberFormat.Undefined },
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 9, NumberFormat = ChannelNumberFormat.Undefined }
		);

	}
}
