using System;
using System.Collections.Generic;

namespace Tesseract.Core.Collections {

	/// <summary>
	/// Collection utilities.
	/// </summary>
	public static class Collection {

		/// <summary>
		/// Converts all the elements in a collection using a converter function.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="e">Enumerable collection</param>
		/// <param name="convert">Converter function</param>
		/// <returns>List of converted elements</returns>
		public static List<T2> ConvertAll<T1, T2>(IEnumerable<T1> e, Func<T1, T2> convert) {
			List<T2> list = new();
			foreach (T1 t in e) list.Add(convert(t));
			return list;
		}

		/// <summary>
		/// Converts all the elements in a collection using a converter function.
		/// </summary>
		/// <typeparam name="T1">Source type</typeparam>
		/// <typeparam name="T2">Destination type</typeparam>
		/// <param name="c">Read-only collection</param>
		/// <param name="convert">Converter function</param>
		/// <returns>List of converted elements</returns>
		public static List<T2> ConvertAll<T1, T2>(IReadOnlyCollection<T1> c, Func<T1, T2> convert) {
			List<T2> list = new(c.Count);
			foreach (T1 t in c) list.Add(convert(t));
			return list;
		}

		/// <summary>
		/// Creates a tuple with two duplicate elements.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="val">Value to duplicate</param>
		/// <returns>Duplicate tuple</returns>
		public static (T, T) TupleDup2<T>(T val) => (val, val);

		/// <summary>
		/// Creates a tuple with three duplicate elements.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="val">Value to duplicate</param>
		/// <returns>Duplicate tuple</returns>
		public static (T, T, T) TupleDup3<T>(T val) => (val, val, val);

		/// <summary>
		/// Creates a tuple with four duplicate elements.
		/// </summary>
		/// <typeparam name="T">Tuple element type</typeparam>
		/// <param name="val">Value to duplicate</param>
		/// <returns>Duplicate tuple</returns>
		public static (T, T, T, T) TupleDup4<T>(T val) => (val, val, val, val);

	}

	/// <summary>
	/// Collection utilities.
	/// </summary>
	/// <typeparam name="T">Element type</typeparam>
	public static class Collection<T> {

		/// <summary>
		/// A read-only list that is always empty.
		/// </summary>
		public static readonly IReadOnlyList<T> EmptyList = Array.Empty<T>();

	}

	/// <summary>
	/// Collection utilities.
	/// </summary>
	/// <typeparam name="T1">First element type</typeparam>
	/// <typeparam name="T2">Second element type</typeparam>
	public static class Collection<T1, T2> where T1 : notnull {

		/// <summary>
		/// A read-only dictionary that is always empty.
		/// </summary>
		public static readonly IReadOnlyDictionary<T1, T2> EmptyDictionary = new Dictionary<T1, T2>();

	}

}
