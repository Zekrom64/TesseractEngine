using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Math {

	/// <summary>
	/// Extended math functionality.
	/// </summary>
	public static class ExMath {

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Min(byte a, byte b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static byte Min(byte a, byte b, byte c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static byte Min(byte a, byte b, byte c, byte d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static byte Min(params byte[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			byte min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte Max(byte a, byte b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static byte Max(byte a, byte b, byte c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static byte Max(byte a, byte b, byte c, byte d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static byte Max(params byte[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			byte min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Min(sbyte a, sbyte b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static sbyte Min(sbyte a, sbyte b, sbyte c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static sbyte Min(sbyte a, sbyte b, sbyte c, sbyte d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static sbyte Min(params sbyte[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			sbyte min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Max(sbyte a, sbyte b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static sbyte Max(sbyte a, sbyte b, sbyte c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static sbyte Max(sbyte a, sbyte b, sbyte c, sbyte d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static sbyte Max(params sbyte[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			sbyte min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Min(short a, short b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static short Min(short a, short b, short c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static short Min(short a, short b, short c, short d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static short Min(params short[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			short min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Max(short a, short b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static short Max(short a, short b, short c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static short Max(short a, short b, short c, short d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static short Max(params short[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			short min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort Min(ushort a, ushort b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static ushort Min(ushort a, ushort b, ushort c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static ushort Min(ushort a, ushort b, ushort c, ushort d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static ushort Min(params ushort[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			ushort min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort Max(ushort a, ushort b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static ushort Max(ushort a, ushort b, ushort c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static ushort Max(ushort a, ushort b, ushort c, ushort d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static ushort Max(params ushort[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			ushort min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Min(int a, int b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static int Min(int a, int b, int c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static int Min(int a, int b, int c, int d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static int Min(params int[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			int min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Max(int a, int b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static int Max(int a, int b, int c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static int Max(int a, int b, int c, int d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static int Max(params int[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			int min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Min(uint a, uint b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static uint Min(uint a, uint b, uint c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static uint Min(uint a, uint b, uint c, uint d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static uint Min(params uint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			uint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Max(uint a, uint b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static uint Max(uint a, uint b, uint c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static uint Max(uint a, uint b, uint c, uint d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static uint Max(params uint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			uint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Min(long a, long b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static long Min(long a, long b, long c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static long Min(long a, long b, long c, long d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static long Min(params long[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			long min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Max(long a, long b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static long Max(long a, long b, long c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static long Max(long a, long b, long c, long d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static long Max(params long[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			long min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Min(ulong a, ulong b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static ulong Min(ulong a, ulong b, ulong c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static ulong Min(ulong a, ulong b, ulong c, ulong d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static ulong Min(params ulong[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			ulong min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Max(ulong a, ulong b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static ulong Max(ulong a, ulong b, ulong c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static ulong Max(ulong a, ulong b, ulong c, ulong d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static ulong Max(params ulong[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			ulong min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static nint Min(nint a, nint b) => a < b ? a : b;

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static nint Min(nint a, nint b, nint c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static nint Min(nint a, nint b, nint c, nint d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static nint Min(params nint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			nint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static nint Max(nint a, nint b) => a > b ? a : b;

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static nint Max(nint a, nint b, nint c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static nint Max(nint a, nint b, nint c, nint d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static nint Max(params nint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			nint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static nuint Min(nuint a, nuint b) => a < b ? a : b;

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static nuint Min(nuint a, nuint b, nuint c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static nuint Min(nuint a, nuint b, nuint c, nuint d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static nuint Min(params nuint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			nuint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static nuint Max(nuint a, nuint b) => a > b ? a : b;

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static nuint Max(nuint a, nuint b, nuint c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static nuint Max(nuint a, nuint b, nuint c, nuint d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static nuint Max(params nuint[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			nuint min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float a, float b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static float Min(float a, float b, float c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static float Min(float a, float b, float c, float d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static float Min(params float[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			float min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float a, float b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static float Max(float a, float b, float c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static float Max(float a, float b, float c, float d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static float Max(params float[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			float min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Min(double a, double b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static double Min(double a, double b, double c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static double Min(double a, double b, double c, double d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static double Min(params double[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			double min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Max(double a, double b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static double Max(double a, double b, double c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static double Max(double a, double b, double c, double d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static double Max(params double[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			double min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Min(decimal a, decimal b) => System.Math.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static decimal Min(decimal a, decimal b, decimal c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static decimal Min(decimal a, decimal b, decimal c, decimal d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static decimal Min(params decimal[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			decimal min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal Max(decimal a, decimal b) => System.Math.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static decimal Max(decimal a, decimal b, decimal c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static decimal Max(decimal a, decimal b, decimal c, decimal d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static decimal Max(params decimal[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			decimal min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Half Min(Half a, Half b) => a < b ? a : b;

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static Half Min(Half a, Half b, Half c) => Min(a, Min(b, c));

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static Half Min(Half a, Half b, Half c, Half d) => Min(a, Min(b, c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static Half Min(params Half[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			Half min = values[0];
			for (int i = 1; i < values.Length; i++) min = Min(min, values[1]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Maximum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Half Max(Half a, Half b) => a > b ? a : b;

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Maximum value</returns>
		public static Half Max(Half a, Half b, Half c) => Max(a, Max(b, c));

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Maximum value</returns>
		public static Half Max(Half a, Half b, Half c, Half d) => Max(a, Max(b, c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Maximum value</returns>
		public static Half Max(params Half[] values) {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			Half min = values[0];
			for (int i = 1; i < values.Length; i++) min = Max(min, values[1]);
			return min;
		}


		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref byte min, ref byte max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref sbyte min, ref sbyte max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref short min, ref short max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref ushort min, ref ushort max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref int min, ref int max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref uint min, ref uint max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref long min, ref long max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref ulong min, ref ulong max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref float min, ref float max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref double min, ref double max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref decimal min, ref decimal max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax(ref Half min, ref Half max) {
			if (min > max) {
				var tmp = min;
				min = max;
				max = tmp;
			}
		}

	}

}
