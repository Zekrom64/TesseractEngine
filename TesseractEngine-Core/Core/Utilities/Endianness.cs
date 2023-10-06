using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Utilities {

	//======================//
	// Little-Endian Values //
	//======================//

	/// <summary>
	/// A 16-bit integer which will always be stored in little-endian byte order in memory.
	/// </summary>
	public struct LittleInt16 {

		private short memValue;

		public short Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleInt16(short value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator short(LittleInt16 le16) => le16.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleInt16(short s) => new(s);

	}

	public struct LittleUInt16 {

		private ushort memValue;

		public ushort Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleUInt16(ushort value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ushort(LittleUInt16 le16) => le16.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleUInt16(ushort s) => new(s);

	}

	public struct LittleInt32 {

		private int memValue;

		public int Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian)
					memValue = value;
				else
					memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleInt32(int value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(LittleInt32 le32) => le32.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleInt32(int s) => new(s);

	}

	public struct LittleUInt32 {

		private uint memValue;

		public uint Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian)
					memValue = value;
				else
					memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleUInt32(uint value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint(LittleUInt32 le32) => le32.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleUInt32(uint s) => new(s);

	}

	public struct LittleInt64 {

		private long memValue;

		public long Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian)
					memValue = value;
				else
					memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleInt64(long value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator long(LittleInt64 le64) => le64.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleInt64(long s) => new(s);

	}

	public struct LittleUInt64 {

		private ulong memValue;

		public ulong Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleUInt64(ulong value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(LittleUInt64 le64) => le64.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleUInt64(ulong s) => new(s);

	}

	public struct LittleSingle {

		private float memValue;

		public float Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(memValue)));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian) memValue = value;
				else memValue = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleSingle(float value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float(LittleSingle les) => les.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleSingle(float s) => new(s);

	}

	public struct LittleDouble {

		private double memValue;

		public double Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => BitConverter.IsLittleEndian ? memValue : BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(memValue)));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (BitConverter.IsLittleEndian) memValue = value;
				else memValue = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LittleDouble(double value) {
			if (BitConverter.IsLittleEndian) memValue = value;
			else memValue = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double(LittleDouble led) => led.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator LittleDouble(double s) => new(s);

	}

	//===================//
	// Big-Endian Values //
	//===================//

	public struct BigInt16 {

		private short memValue;

		public short Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigInt16(short value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator short(BigInt16 le16) => le16.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigInt16(short s) => new(s);

	}

	public struct BigUInt16 {

		private ushort memValue;

		public ushort Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigUInt16(ushort value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ushort(BigUInt16 le16) => le16.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigUInt16(ushort s) => new(s);

	}

	public struct BigInt32 {

		private int memValue;

		public int Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigInt32(int value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(BigInt32 le32) => le32.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigInt32(int s) => new(s);

	}

	public struct BigUInt32 {

		private uint memValue;

		public uint Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigUInt32(uint value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint(BigUInt32 le32) => le32.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigUInt32(uint s) => new(s);

	}

	public struct BigInt64 {

		private long memValue;

		public long Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigInt64(long value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator long(BigInt64 le64) => le64.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigInt64(long s) => new(s);

	}

	public struct BigUInt64 {

		private ulong memValue;

		public ulong Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BinaryPrimitives.ReverseEndianness(memValue);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BinaryPrimitives.ReverseEndianness(value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigUInt64(ulong value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BinaryPrimitives.ReverseEndianness(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(BigUInt64 le64) => le64.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigUInt64(ulong s) => new(s);

	}

	public struct BigSingle {

		private float memValue;

		public float Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(memValue)));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigSingle(float value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator float(BigSingle les) => les.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigSingle(float s) => new(s);

	}

	public struct BigDouble {

		private double memValue;

		public double Value {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly get => !BitConverter.IsLittleEndian ? memValue : BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(memValue)));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (!BitConverter.IsLittleEndian) memValue = value;
				else memValue = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BigDouble(double value) {
			if (!BitConverter.IsLittleEndian) memValue = value;
			else memValue = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator double(BigDouble led) => led.Value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator BigDouble(double s) => new(s);

	}

}
