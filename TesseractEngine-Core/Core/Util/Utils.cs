using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	public static class BinaryUtils {

		public static ushort ByteSwap(ushort value) =>  (ushort)((value >> 8) | (value << 8));

		public static short ByteSwap(short value) => (short)ByteSwap((ushort)value);

		public static uint ByteSwap(uint value) {
			value = (value << 16) | (value >> 16);
			value = ((value & 0xFF00FF00) >> 8) | ((value & 0x00FF00FF) << 8);
			return value;
		}

		public static int ByteSwap(int value) => (int)ByteSwap((uint)value);

		public static ulong ByteSwap(ulong value) {
			value = (value << 32) | (value >> 32);
			value = ((value & 0xFFFF0000FFFF0000) >> 16) | ((value & 0x0000FFFF0000FFFF) << 16);
			value = ((value & 0xFF00FF00FF00FF00) >> 8) | ((value & 0x00FF00FF00FF00FF) << 8);
			return value;
		}

		public static long ByteSwap(long value) => (long)ByteSwap((ulong)value);

		public static float ByteSwap(float value) => BitConverter.Int32BitsToSingle(ByteSwap(BitConverter.SingleToInt32Bits(value)));

		public static double ByteSwap(double value) => BitConverter.Int64BitsToDouble(ByteSwap(BitConverter.DoubleToInt64Bits(value)));


		public static ushort ToUInt16(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			ushort value = BitConverter.ToUInt16(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		public static short ToInt16(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (short)ToUInt16(binary, isLittleEndian);

		public static uint ToUInt32(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			uint value = BitConverter.ToUInt32(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		public static int ToInt32(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (int)ToUInt32(binary, isLittleEndian);

		public static ulong ToUInt64(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) {
			ulong value = BitConverter.ToUInt64(binary);
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			return value;
		}

		public static long ToInt64(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => (long)ToUInt32(binary, isLittleEndian);

		public static float ToSingle(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => BitConverter.Int32BitsToSingle(ToInt32(binary, isLittleEndian));

		public static double ToDouble(in ReadOnlySpan<byte> binary, bool isLittleEndian = true) => BitConverter.Int64BitsToDouble(ToInt64(binary, isLittleEndian));


		public static Span<byte> WriteBytes(Span<byte> dst, ushort value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(ushort)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		public static Span<byte> WriteBytes(Span<byte> dst, short value, bool isLittleEndian = true) => WriteBytes(dst, (ushort)value, isLittleEndian);

		public static Span<byte> WriteBytes(Span<byte> dst, uint value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(uint)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		public static Span<byte> WriteBytes(Span<byte> dst, int value, bool isLittleEndian = true) => WriteBytes(dst, (uint)value, isLittleEndian);

		public static Span<byte> WriteBytes(Span<byte> dst, ulong value, bool isLittleEndian = true) {
			if (dst.Length < sizeof(ulong)) throw new ArgumentException("Span is not large enough to write value", nameof(dst));
			if (BitConverter.IsLittleEndian != isLittleEndian) value = ByteSwap(value);
			BitConverter.TryWriteBytes(dst, value);
			return dst;
		}

		public static Span<byte> WriteBytes(Span<byte> dst, long value, bool isLittleEndian = true) => WriteBytes(dst, (ulong)value, isLittleEndian);

		public static Span<byte> WriteBytes(Span<byte> dst, float value, bool isLittleEndian = true) => WriteBytes(dst, BitConverter.SingleToInt32Bits(value), isLittleEndian);

		public static Span<byte> WriteBytes(Span<byte> dst, double value, bool isLittleEndian = true) => WriteBytes(dst, BitConverter.DoubleToInt64Bits(value), isLittleEndian);

	}

}
