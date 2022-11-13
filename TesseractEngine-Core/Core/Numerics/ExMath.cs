using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tesseract.Core.Numerics {

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
		public static T Min<T>(T a, T b) where T : INumber<T> => T.Min(a, b);

		/// <summary>
		/// Computes the minimum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static T Min<T>(T a, T b, T c) where T : INumber<T> => Min(Min(a, b), c);

		/// <summary>
		/// Computes the minimum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static T Min<T>(T a, T b, T c, T d) where T : INumber<T> => Min(Min(a, b), Min(c, d));

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static T Min<T>(params T[] values) where T : INumber<T> {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			T min = values[0];
			for (int i = 1; i < values.Length; i++) min = T.Min(min, values[i]);
			return min;
		}

		/// <summary>
		/// Computes the minimum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static T Min<T>(in ReadOnlySpan<T> values) where T : INumber<T> {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			T min = values[0];
			for (int i = 1; i < values.Length; i++) min = T.Min(min, values[i]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of two values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <returns>Minimum value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Max<T>(T a, T b) where T : INumber<T> => T.Max(a, b);

		/// <summary>
		/// Computes the maximum of three values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <returns>Minimum value</returns>
		public static T Max<T>(T a, T b, T c) where T : INumber<T> => Max(Max(a, b), c);

		/// <summary>
		/// Computes the maximum of four values.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="c">Third value</param>
		/// <param name="d">Fourth value</param>
		/// <returns>Minimum value</returns>
		public static T Max<T>(T a, T b, T c, T d) where T : INumber<T> => Max(Max(a, b), Max(c, d));

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static T Max<T>(params T[] values) where T : INumber<T> {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			T min = values[0];
			for (int i = 1; i < values.Length; i++) min = T.Max(min, values[i]);
			return min;
		}

		/// <summary>
		/// Computes the maximum of an array of values.
		/// </summary>
		/// <param name="values">Array of values</param>
		/// <returns>Minimum value</returns>
		public static T Max<T>(in ReadOnlySpan<T> values) where T : INumber<T> {
			if (values.Length < 1) throw new ArgumentException("Cannot find minimum of zero-length array", nameof(values));
			T min = values[0];
			for (int i = 1; i < values.Length; i++) min = T.Max(min, values[i]);
			return min;
		}

		/// <summary>
		/// Computes the minimum and maximum of two values, placing the determined
		/// minimum and maximum in the respective referenced arguments.
		/// </summary>
		/// <param name="min">Value to store the minimum in</param>
		/// <param name="max">Value to store the maximum in</param>
		public static void MinMax<T>(ref T min, ref T max) where T : INumber<T> {
			if (min > max) (max, min) = (min, max);
		}

		/// <summary>
		/// Tests if a and b are about equal, within the range of the given epsilon value.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="epsilon">Epsilon value</param>
		/// <returns>If the values are about equal</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAbout(float a, float b, float epsilon) => MathF.Abs(a - b) <= epsilon;

		/// <summary>
		/// Tests if a and b are about equal, within the range of the given epsilon value.
		/// </summary>
		/// <param name="a">First value</param>
		/// <param name="b">Second value</param>
		/// <param name="epsilon">Epsilon value</param>
		/// <returns>If the values are about equal</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAbout(double a, double b, double epsilon) => Math.Abs(a - b) <= epsilon;

	}

}
