using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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

		private static string DefaultFormatter(IReadOnlyList<T> list) {
			StringBuilder sb = new();
			sb.Append("{ ");
			foreach (T val in list) sb.Append(val?.ToString()).Append(' ');
			sb.Append('}');
			return sb.ToString();
		}

		private class StringFormattedReadOnlyList : IReadOnlyList<T> {

			public required IReadOnlyList<T> List { get; init; }

			public required Func<IReadOnlyList<T>, string> Formatter { get; init; }

			public T this[int index] => List[index];

			public int Count => List.Count;

			public IEnumerator<T> GetEnumerator() => List.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();

			public override bool Equals(object? obj) => List.Equals(obj);

			public override int GetHashCode() => List.GetHashCode();

			public override string ToString() => Formatter(List);
		}

		/// <summary>
		/// Adds formatting to the <see cref="object.ToString"/> method of a list.
		/// </summary>
		/// <param name="list">List to add formatting to</param>
		/// <param name="formatter">Formatting function</param>
		/// <returns>List with formatting</returns>
		public static IReadOnlyList<T> AddStringFormatting(IReadOnlyList<T> list, Func<IReadOnlyList<T>, string>? formatter = null) {
			formatter ??= DefaultFormatter;
			return new StringFormattedReadOnlyList() { List = list, Formatter = formatter };
		}

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
