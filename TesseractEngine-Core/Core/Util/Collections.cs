using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tesseract.Core.Util {

	/// <summary>
	/// A read-only list that provides a view of an array. The underlying array
	/// may be separately modified and the changes will be reflected in the list.
	/// </summary>
	/// <typeparam name="T">Element type</typeparam>
	public class ReadOnlyArrayList<T> : IReadOnlyList<T> {

		private readonly T[] array;

		public ReadOnlyArrayList(T[] arr) {
			array = arr;
		}

		public T this[int index] => array[index];

		public int Count => array.Length;

		public IEnumerator<T> GetEnumerator() => array.AsEnumerable().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => array.GetEnumerator();

	}

	/// <summary>
	/// Collection utilities.
	/// </summary>
	public static class Collections {

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

	}

	/// <summary>
	/// Collection utilities.
	/// </summary>
	/// <typeparam name="T">Element type</typeparam>
	public static class Collections<T> {

		/// <summary>
		/// A read-only list that is always empty.
		/// </summary>
		public static readonly IReadOnlyList<T> EmptyList = new List<T>(0).AsReadOnly();

	}

}
