using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		Stencil
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

		public override bool Equals(object obj) => obj is PixelChannel channel && Equals(channel);

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
	/// </summary>
	public class PixelFormat : IEquatable<PixelFormat> {

		// Deduces other pixel format properties from the given set of pixel channels
		private static void DeducePropertiesFromChannels(PixelChannel[] channels, bool packed, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat) {
			// If packed format channel size values are in bits else in bytes
			int channelWidth = packed ? 1 : 8;
			int bitSize = 0;
			numberFormat = channels[0].NumberFormat;
			formatType = PixelFormatType.Color;
			foreach(PixelChannel channel in channels) {
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
			DeducePropertiesFromChannels(channels, false, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat);
			return new PixelFormat() {
				Packed = false,
				Channels = new List<PixelChannel>(channels),
				SizeOf = byteSize,
				Type = formatType,
				NumberFormat = numberFormat
			};
		}

		/// <summary>
		/// Defines a packed format using the given channels.
		/// </summary>
		/// <param name="channels">Pixel format channels</param>
		/// <returns>Packed pixel format</returns>
		public static PixelFormat DefinePackedFormat(params PixelChannel[] channels) {
			DeducePropertiesFromChannels(channels, true, out int byteSize, out PixelFormatType formatType, out ChannelNumberFormat numberFormat);
			return new PixelFormat() {
				Packed = true,
				Channels = new List<PixelChannel>(channels),
				SizeOf = byteSize,
				Type = formatType,
				NumberFormat = numberFormat
			};
		}

		/// <summary>
		/// A pixel format with 8-bit unsigned normalized red, green, and blue channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
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
		/// A pixel format with 8-bit unsigned normalized red, green, blue, and alpha channels.
		/// </summary>
		public static readonly PixelFormat R8G8B8A8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Red, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
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
		/// A pixel format with 8-bit unsigned normalized alpha, blue, green, and red channels.
		/// </summary>
		public static readonly PixelFormat A8B8G8R8UNorm = DefineUnpackedFormat(
			new PixelChannel() { Type = ChannelType.Alpha, Offset = 0, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Blue, Offset = 1, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Green, Offset = 2, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm },
			new PixelChannel() { Type = ChannelType.Red, Offset = 3, Size = 1, NumberFormat = ChannelNumberFormat.UnsignedNorm }
		);

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

		/// <summary>
		/// If the channel values are "packed" (in bitfields or encoded values) or raw unpacked values.
		/// </summary>
		public bool Packed { get; init; }

		/// <summary>
		/// The channels defined in the format.
		/// </summary>
		public IReadOnlyList<PixelChannel> Channels { get; init; }

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

		public bool Equals(PixelFormat other) {
			if (other == null) return false;
			if (this == other) return true;

			if (Packed != other.Packed) return false;

			foreach(PixelChannel channel in Channels) {
				bool hasChannel = false;
				foreach(PixelChannel otherChannel in other.Channels) {
					if (otherChannel == channel) {
						hasChannel = true;
						break;
					}
				}
				if (!hasChannel) return false;
			}

			return true;
		}

		public override bool Equals(object obj) => obj is PixelFormat format && Equals(format);

		public override int GetHashCode() => SizeOf ^ ((int)Type << 8) ^ ((int)NumberFormat << 12) ^ (Packed ? -1 : 0);

		public static bool operator ==(PixelFormat left, PixelFormat right) => ReferenceEquals(left, right) || (left?.Equals(right)).GetValueOrDefault(false);

		public static bool operator !=(PixelFormat left, PixelFormat right) => !(left == right);

	}
}
