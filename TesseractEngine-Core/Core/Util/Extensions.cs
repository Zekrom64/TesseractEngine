using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {
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

	}

	public static class SpanExtensions {

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
