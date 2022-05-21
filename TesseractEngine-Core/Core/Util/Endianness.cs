using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Util {
	// Virtually every system is little-endian, but this is not required by the .NET runtime
	// There is also some utility in having known-endianness types for storage

	public static class Endianness {

		public static bool IsLittleEndian {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => BitConverter.IsLittleEndian;
		}

		public static bool IsBigEndian {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => !BitConverter.IsLittleEndian;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Swap(short s) => (short)((s << 8) | (s >> 8));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort Swap(ushort s) => (ushort)((s << 8) | (s >> 8));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Swap(int i) => (i << 24) | ((i << 8) & 0xFF0000) | ((i >> 8) & 0xFF00) | (i >> 24);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Swap(uint i) => (i << 24) | ((i << 8) & 0xFF0000) | ((i >> 8) & 0xFF00) | (i >> 24);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Swap(long l) => ((long)Swap((int)l) << 32) | (Swap((int)(l >> 32)) & 0xFFFFFFFFL);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Swap(ulong l) => ((ulong)Swap((uint)l) << 32) | Swap((uint)(l >> 32));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Swap(float f) => BitConverter.Int32BitsToSingle(Swap(BitConverter.SingleToInt32Bits(f)));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Swap(double d) => BitConverter.Int64BitsToDouble(Swap(BitConverter.DoubleToInt64Bits(d)));

	}

	//======================//
	// Little-Endian Values //
	//======================//

	public struct LittleInt16 {

		private short memValue;

		public short Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleInt16(short value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator short(LittleInt16 le16) => le16.Value;

		public static implicit operator LittleInt16(short s) => new(s);

	}

	public struct LittleUInt16 {

		private ushort memValue;

		public ushort Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleUInt16(ushort value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator ushort(LittleUInt16 le16) => le16.Value;

		public static implicit operator LittleUInt16(ushort s) => new(s);

	}

	public struct LittleInt32 {

		private int memValue;

		public int Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleInt32(int value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator int(LittleInt32 le32) => le32.Value;

		public static implicit operator LittleInt32(int s) => new(s);

	}

	public struct LittleUInt32 {

		private uint memValue;

		public uint Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleUInt32(uint value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator uint(LittleUInt32 le32) => le32.Value;

		public static implicit operator LittleUInt32(uint s) => new(s);

	}

	public struct LittleInt64 {

		private long memValue;

		public long Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleInt64(long value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator long(LittleInt64 le64) => le64.Value;

		public static implicit operator LittleInt64(long s) => new(s);

	}

	public struct LittleUInt64 {

		private ulong memValue;

		public ulong Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleUInt64(ulong value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator ulong(LittleUInt64 le64) => le64.Value;

		public static implicit operator LittleUInt64(ulong s) => new(s);

	}

	public struct LittleSingle {

		private float memValue;

		public float Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleSingle(float value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator float(LittleSingle les) => les.Value;

		public static implicit operator LittleSingle(float s) => new(s);

	}

	public struct LittleDouble {

		private double memValue;

		public double Value {
			get => Endianness.IsLittleEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsLittleEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public LittleDouble(double value) {
			if (Endianness.IsLittleEndian) memValue = value;
			else memValue = Endianness.Swap(value);
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
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigInt16(short value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator short(BigInt16 le16) => le16.Value;

		public static implicit operator BigInt16(short s) => new(s);

	}

	public struct BigUInt16 {

		private ushort memValue;

		public ushort Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigUInt16(ushort value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator ushort(BigUInt16 le16) => le16.Value;

		public static implicit operator BigUInt16(ushort s) => new(s);

	}

	public struct BigInt32 {

		private int memValue;

		public int Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigInt32(int value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator int(BigInt32 le32) => le32.Value;

		public static implicit operator BigInt32(int s) => new(s);

	}

	public struct BigUInt32 {

		private uint memValue;

		public uint Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigUInt32(uint value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator uint(BigUInt32 le32) => le32.Value;

		public static implicit operator BigUInt32(uint s) => new(s);

	}

	public struct BigInt64 {

		private long memValue;

		public long Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigInt64(long value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator long(BigInt64 le64) => le64.Value;

		public static implicit operator BigInt64(long s) => new(s);

	}

	public struct BigUInt64 {

		private ulong memValue;

		public ulong Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigUInt64(ulong value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator ulong(BigUInt64 le64) => le64.Value;

		public static implicit operator BigUInt64(ulong s) => new(s);

	}

	public struct BigSingle {

		private float memValue;

		public float Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigSingle(float value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator float(BigSingle les) => les.Value;

		public static implicit operator BigSingle(float s) => new(s);

	}

	public struct BigDouble {

		private double memValue;

		public double Value {
			get => Endianness.IsBigEndian ? memValue : Endianness.Swap(memValue);
			set {
				if (Endianness.IsBigEndian) memValue = value;
				else memValue = Endianness.Swap(value);
			}
		}

		public BigDouble(double value) {
			if (Endianness.IsBigEndian) memValue = value;
			else memValue = Endianness.Swap(value);
		}

		public static implicit operator double(BigDouble led) => led.Value;

		public static implicit operator BigDouble(double s) => new(s);

	}

}
