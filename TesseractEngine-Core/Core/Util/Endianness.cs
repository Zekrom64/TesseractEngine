using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Util {

	//======================//
	// Little-Endian Values //
	//======================//

	public struct LittleInt16 {

		private short memValue;

		public short Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleInt16(short value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator short(LittleInt16 le16) => le16.Value;

		public static implicit operator LittleInt16(short s) => new(s);

	}

	public struct LittleUInt16 {

		private ushort memValue;

		public ushort Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleUInt16(ushort value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator ushort(LittleUInt16 le16) => le16.Value;

		public static implicit operator LittleUInt16(ushort s) => new(s);

	}

	public struct LittleInt32 {

		private int memValue;

		public int Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleInt32(int value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator int(LittleInt32 le32) => le32.Value;

		public static implicit operator LittleInt32(int s) => new(s);

	}

	public struct LittleUInt32 {

		private uint memValue;

		public uint Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleUInt32(uint value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator uint(LittleUInt32 le32) => le32.Value;

		public static implicit operator LittleUInt32(uint s) => new(s);

	}

	public struct LittleInt64 {

		private long memValue;

		public long Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleInt64(long value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator long(LittleInt64 le64) => le64.Value;

		public static implicit operator LittleInt64(long s) => new(s);

	}

	public struct LittleUInt64 {

		private ulong memValue;

		public ulong Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleUInt64(ulong value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator ulong(LittleUInt64 le64) => le64.Value;

		public static implicit operator LittleUInt64(ulong s) => new(s);

	}

	public struct LittleSingle {

		private float memValue;

		public float Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleSingle(float value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator float(LittleSingle les) => les.Value;

		public static implicit operator LittleSingle(float s) => new(s);

	}

	public struct LittleDouble {

		private double memValue;

		public double Value {
			get => BinaryUtils.IsLittleEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsLittleEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public LittleDouble(double value) {
			if (BinaryUtils.IsLittleEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator double(LittleDouble led) => led.Value;

		public static implicit operator LittleDouble(double s) => new(s);

	}

	//===================//
	// Big-Endian Values //
	//===================//

	public struct BigInt16 {

		private short memValue;

		public short Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigInt16(short value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator short(BigInt16 le16) => le16.Value;

		public static implicit operator BigInt16(short s) => new(s);

	}

	public struct BigUInt16 {

		private ushort memValue;

		public ushort Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigUInt16(ushort value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator ushort(BigUInt16 le16) => le16.Value;

		public static implicit operator BigUInt16(ushort s) => new(s);

	}

	public struct BigInt32 {

		private int memValue;

		public int Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigInt32(int value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator int(BigInt32 le32) => le32.Value;

		public static implicit operator BigInt32(int s) => new(s);

	}

	public struct BigUInt32 {

		private uint memValue;

		public uint Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigUInt32(uint value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator uint(BigUInt32 le32) => le32.Value;

		public static implicit operator BigUInt32(uint s) => new(s);

	}

	public struct BigInt64 {

		private long memValue;

		public long Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigInt64(long value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator long(BigInt64 le64) => le64.Value;

		public static implicit operator BigInt64(long s) => new(s);

	}

	public struct BigUInt64 {

		private ulong memValue;

		public ulong Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigUInt64(ulong value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator ulong(BigUInt64 le64) => le64.Value;

		public static implicit operator BigUInt64(ulong s) => new(s);

	}

	public struct BigSingle {

		private float memValue;

		public float Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigSingle(float value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator float(BigSingle les) => les.Value;

		public static implicit operator BigSingle(float s) => new(s);

	}

	public struct BigDouble {

		private double memValue;

		public double Value {
			get => BinaryUtils.IsBigEndian ? memValue : BinaryUtils.ByteSwap(memValue);
			set {
				if (BinaryUtils.IsBigEndian) memValue = value;
				else memValue = BinaryUtils.ByteSwap(value);
			}
		}

		public BigDouble(double value) {
			if (BinaryUtils.IsBigEndian) memValue = value;
			else memValue = BinaryUtils.ByteSwap(value);
		}

		public static implicit operator double(BigDouble led) => led.Value;

		public static implicit operator BigDouble(double s) => new(s);

	}

}
