using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	/// <summary>
	/// LINQ-related helper methods.
	/// </summary>
	public static class LINQ {

		/// <summary>
		/// Generates a sequence of integer values.
		/// </summary>
		/// <param name="count">Number of values in sequence</param>
		/// <param name="start">First value in sequence</param>
		/// <param name="advance">Amount to advance for every iteration of sequence</param>
		/// <returns>Sequence of integer values</returns>
		public static IEnumerable<int> Seq(int count, int start = 0, int advance = 1) {
			for (int i = 0, j = start; i < count; i++, j += advance) yield return j;
		}

		/// <summary>
		/// Generates a sequence of duplicate values.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="value">The value to duplicate</param>
		/// <param name="times">The number of duplicate values</param>
		/// <returns>Sequence of duplicates</returns>
		public static IEnumerable<T> Dup<T>(T value, int times) {
			while (times-- > 0) yield return value;
		}

	}
}
