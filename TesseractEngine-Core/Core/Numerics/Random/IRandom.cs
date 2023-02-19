using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Numerics.Random {

	/// <summary>
	/// A random number source provides access to a stream of random bits.
	/// </summary>
	public interface IRandom {

		/// <summary>
		/// Returns the next set of random bits from this random number source.
		/// </summary>
		/// <param name="bits">The number of random bits to get</param>
		/// <returns>The random bits from this source</returns>
		public int NextBits(int bits);

		/// <summary>
		/// Returns the next random integer within the given range.
		/// </summary>
		/// <param name="start">The start of the range (inclusive)</param>
		/// <param name="end">The end of the range (exclusive)</param>
		/// <returns>The next random integer within the range</returns>
		public int NextInt(int start, int end);

		/// <summary>
		/// Returns the next random integer between 0 and the maximum.
		/// </summary>
		/// <param name="max">The maximum of the range (exclusive)</param>
		/// <returns>The next random integer within the range</returns>
		public int NextInt(int max) => NextInt(0, max);

		/// <summary>
		/// Returns the next random floating-point number normalized between 0 and 1.
		/// </summary>
		/// <returns>The next random normalized float</returns>
		public float NextFloat();

		/// <summary>
		/// Randomly selects an item from the given list.
		/// </summary>
		/// <typeparam name="T">List element type</typeparam>
		/// <param name="list">The list to select items from</param>
		/// <returns>The randomly selected item</returns>
		public T Select<T>(IReadOnlyList<T> list) => list[NextInt(list.Count)];

	}

}
