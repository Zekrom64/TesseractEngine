using System;
using System.Collections.Generic;
using System.Linq;
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
}
