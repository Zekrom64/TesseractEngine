﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	/// <summary>
	/// Array extension methods.
	/// </summary>
	public static class ArrayExtensions {

		/// <summary>
		/// Invokes <see cref="Array.ConvertAll{TInput, TOutput}(TInput[], Converter{TInput, TOutput})"/> with this
		/// array as the input array.
		/// </summary>
		/// <typeparam name="T1">Type to convert from</typeparam>
		/// <typeparam name="T2">Type to convert to</typeparam>
		/// <param name="array">This array</param>
		/// <param name="convert">Converter function</param>
		/// <returns>Converted array</returns>
		public static T2[] ConvertAll<T1,T2>(this T1[] array, Converter<T1,T2> convert) => Array.ConvertAll(array, convert);

		/// <summary>
		/// Creates a shallow clone of this array.
		/// </summary>
		/// <typeparam name="T">Array element type</typeparam>
		/// <param name="array">Array to clone</param>
		/// <returns>Shallow clone of array</returns>
		public static T[] ShallowClone<T>(this T[] array) {
			T[] newarr = new T[array.Length];
			Array.Copy(array, newarr, array.Length);
			return newarr;
		}

	}

	/// <summary>
	/// Span extension methods.
	/// </summary>
	public static class SpanExtensions {

		/// <summary>
		/// Converts all the elements in this span using a converter function.
		/// The returned span is created from a new array.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="span">Span of elements to convert</param>
		/// <param name="convert">Converter function</param>
		/// <returns>Span of converted elements</returns>
		public static Span<T2> ConvertAll<T1,T2>(this ReadOnlySpan<T1> span, Converter<T1,T2> convert) {
			Span<T2> dst = new T2[span.Length];
			for (int i = 0; i < span.Length; i++) dst[i] = convert(span[i]);
			return dst;
		}

		/// <summary>
		/// Converts all the elements in this span using a converter function.
		/// The returned span is created from a new array.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="span">Span of elements to convert</param>
		/// <param name="convert">Converter function</param>
		/// <returns>Span of converted elements</returns>
		public static Span<T2> ConvertAll<T1, T2>(this Span<T1> span, Converter<T1, T2> convert) {
			Span<T2> dst = new T2[span.Length];
			for (int i = 0; i < span.Length; i++) dst[i] = convert(span[i]);
			return dst;
		}

		/// <summary>
		/// Converts all the elements in this span using a converter function.
		/// The destination span is provided to this method and returned.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="span">Span of elements to convert</param>
		/// <param name="convert">Converter function</param>
		/// <param name="dst">The destination span</param>
		/// <returns>Span of converted elements</returns>
		public static Span<T2> ConvertAll<T1, T2>(this ReadOnlySpan<T1> span, Converter<T1, T2> convert, Span<T2> dst) {
			int n = System.Math.Min(span.Length, dst.Length);
			for (int i = 0; i < n; i++) dst[i] = convert(span[i]);
			return dst;
		}

		/// <summary>
		/// Converts all the elements in this span using a converter function.
		/// The destination span is provided to this method and returned.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="span">Span of elements to convert</param>
		/// <param name="convert">Converter function</param>
		/// <param name="dst">The destination span</param>
		/// <returns>Span of converted elements</returns>
		public static Span<T2> ConvertAll<T1, T2>(this Span<T1> span, Converter<T1, T2> convert, Span<T2> dst) {
			int n = System.Math.Min(span.Length, dst.Length);
			for (int i = 0; i < n; i++) dst[i] = convert(span[i]);
			return dst;
		}

	}

	/// <summary>
	/// IEnumerable extensions.
	/// </summary>
	public static class EnumerableExtensions {

		/// <summary>
		/// Gets the first element in the enumerable or returns a provided default value.
		/// </summary>
		/// <typeparam name="T">Enumerable element type</typeparam>
		/// <param name="e">Enumerable</param>
		/// <param name="value">Default value</param>
		/// <returns>First value or default</returns>
		public static T FirstOrDefault<T>(this IEnumerable<T> e, T value) => e.Any() ? e.First() : value;

		/// <summary>
		/// Gets the last element in the enumerable or returns a provided default value.
		/// </summary>
		/// <typeparam name="T">Enumerable element type</typeparam>
		/// <param name="e">Enumerable</param>
		/// <param name="value">Default value</param>
		/// <returns>Last value or default</returns>
		public static T LastOrDefault<T>(this IEnumerable<T> e, T value) => e.Any() ? e.Last() : value;

		/// <summary>
		/// Attempts to get the first value in the enumerable, indicating if successful.
		/// </summary>
		/// <typeparam name="T">Enumerable element type</typeparam>
		/// <param name="e">Enumerable</param>
		/// <param name="value">First value</param>
		/// <returns>If the first value was retrieved</returns>
		public static bool TryGetFirst<T>(this IEnumerable<T> e, out T value) {
			if (e.Any()) {
				value = e.First();
				return true;
			} else {
				value = default;
				return false;
			}
		}

		/// <summary>
		/// Attempts to get the last value in the enumerable, indicating if successful.
		/// </summary>
		/// <typeparam name="T">Enumerable element type</typeparam>
		/// <param name="e">Enumerable</param>
		/// <param name="value">Last value</param>
		/// <returns>If the last value was retrieved</returns>
		public static bool TryGetLast<T>(this IEnumerable<T> e, out T value) {
			if (e.Any()) {
				value = e.Last();
				return true;
			} else {
				value = default;
				return false;
			}
		}

	}

	public static class VectorExtensions {

		public static void CopyTo(this Vector2 v, Span<float> span, int offset = 0) {
			span[offset++] = v.X;
			span[offset] = v.Y;
		}

		public static void CopyTo(this Vector3 v, Span<float> span, int offset = 1) {
			span[offset++] = v.X;
			span[offset++] = v.Y;
			span[offset] = v.Z;
		}

		public static void CopyTo(this Vector4 v, Span<float> span, int offset = 1) {
			span[offset++] = v.X;
			span[offset++] = v.Y;
			span[offset++] = v.Z;
			span[offset] = v.W;
		}

		public static Vector2 ReadFrom(this Vector2 v, in ReadOnlySpan<float> span, int offset = 0) {
			int n = span.Length;
			if (n > 0) {
				v.X = span[offset++];
				n--;
			}
			if (n > 0) {
				v.Y = span[offset];
			}
			return v;
		}

		public static Vector3 ReadFrom(this Vector3 v, in ReadOnlySpan<float> span, int offset = 0) {
			int n = span.Length;
			if (n > 0) {
				v.X = span[offset++];
				n--;
			}
			if (n > 0) {
				v.Y = span[offset++];
				n--;
			}
			if (n > 0) {
				v.Z = span[offset];
			}
			return v;
		}

		public static Vector4 ReadFrom(this Vector4 v, in ReadOnlySpan<float> span, int offset = 0) {
			int n = span.Length;
			if (n > 0) {
				v.X = span[offset++];
				n--;
			}
			if (n > 0) {
				v.Y = span[offset++];
				n--;
			}
			if (n > 0) {
				v.Z = span[offset++];
				n--;
			}
			if (n > 0) {
				v.W = span[offset];
			}
			return v;
		}

	}
}
