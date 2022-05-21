using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	/// <summary>
	/// Class providing utilities for working with binary data.
	/// </summary>
	public static class BinaryUtils {

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static ushort ByteSwap(ushort value) =>  (ushort)((value >> 8) | (value << 8));

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static short ByteSwap(short value) => (short)ByteSwap((ushort)value);

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static uint ByteSwap(uint value) {
			value = (value << 16) | (value >> 16);
			value = ((value & 0xFF00FF00) >> 8) | ((value & 0x00FF00FF) << 8);
			return value;
		}

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static int ByteSwap(int value) => (int)ByteSwap((uint)value);

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static ulong ByteSwap(ulong value) {
			value = (value << 32) | (value >> 32);
			value = ((value & 0xFFFF0000FFFF0000) >> 16) | ((value & 0x0000FFFF0000FFFF) << 16);
			value = ((value & 0xFF00FF00FF00FF00) >> 8) | ((value & 0x00FF00FF00FF00FF) << 8);
			return value;
		}

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static long ByteSwap(long value) => (long)ByteSwap((ulong)value);

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static float ByteSwap(float value) => BitConverter.Int32BitsToSingle(ByteSwap(BitConverter.SingleToInt32Bits(value)));

		/// <summary>
		/// Swaps the byte order of the supplied value.
		/// </summary>
		/// <param name="value">Value to swap byte order</param>
		/// <returns>Byte-order swapped value</returns>
		public static double ByteSwap(double value) => BitConverter.Int64BitsToDouble(ByteSwap(BitConverter.DoubleToInt64Bits(value)));


		/// <summary>
		/// Converts the supplied byte sequence to an unsigned 16-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static ushort ToUInt16(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			ushort value = BitConverter.ToUInt16(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		/// <summary>
		/// Converts the supplied byte sequence to a signed 16-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static short ToInt16(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (short)ToUInt16(binary, isLittleEndian);

		/// <summary>
		/// Converts the supplied byte sequence to an unsigned 32-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static uint ToUInt32(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			uint value = BitConverter.ToUInt32(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		/// <summary>
		/// Converts the supplied byte sequence to a signed 32-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static int ToInt32(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (int)ToUInt32(binary, isLittleEndian);

		/// <summary>
		/// Converts the supplied byte sequence to an unsigned 64-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static ulong ToUInt64(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			ulong value = BitConverter.ToUInt64(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		/// <summary>
		/// Converts the supplied byte sequence to a signed 64-bit integer it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static long ToInt64(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (long)ToUInt32(binary, isLittleEndian);

		/// <summary>
		/// Converts the supplied byte sequence to a single-precision IEEE float it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static float ToSingle(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => BitConverter.Int32BitsToSingle(ToInt32(binary, isLittleEndian));

		/// <summary>
		/// Converts the supplied byte sequence to a double-precision IEEE float it little or big endian format.
		/// </summary>
		/// <param name="binary">Input byte sequence</param>
		/// <param name="isLittleEndian">If the value is little endian</param>
		/// <returns>The converted value</returns>
		public static double ToDouble(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => BitConverter.Int64BitsToDouble(ToInt64(binary, isLittleEndian));


		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, ushort value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(ushort)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, short value, bool isLittleEndian = true) => WriteBytes(dst, (ushort)value, isLittleEndian);

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, uint value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(uint)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, int value, bool isLittleEndian = true) => WriteBytes(dst, (uint)value, isLittleEndian);

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, ulong value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(ulong)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, long value, bool isLittleEndian = true) => WriteBytes(dst, (ulong)value, isLittleEndian);

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, float value, bool isLittleEndian = true) => WriteBytes(dst, BitConverter.SingleToInt32Bits(value), isLittleEndian);

		/// <summary>
		/// Writes the bytes of the supplied value in little or big endian format.
		/// </summary>
		/// <param name="dst">Desintation byte sequence</param>
		/// <param name="value">The value whose bytes to write</param>
		/// <param name="isLittleEndian">If the bytes should be written in little-endian order</param>
		/// <returns>The destination byte sequence</returns>
		/// <exception cref="ArgumentException">If the span is too small to hold the byte representation of the value</exception>
		public static Span<byte> WriteBytes(Span<byte> dst, double value, bool isLittleEndian = true) => WriteBytes(dst, BitConverter.DoubleToInt64Bits(value), isLittleEndian);

	}

}
