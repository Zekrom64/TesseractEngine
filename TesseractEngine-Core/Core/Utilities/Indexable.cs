using System;

namespace Tesseract.Core.Utilities {

	/// <summary>
	/// A read-only indexer provides a method for reading values using the array subscript.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	public interface IReadOnlyIndexer<K, V> {

		/// <summary>
		/// Reads a value from this object using the given key value.
		/// </summary>
		/// <param name="key">Key value</param>
		/// <returns>Indexed value</returns>
		public V this[K key] { get; }

	}

	/// <summary>
	/// An indexer is a superset of the read-only indexer, supporting reading and writing using the array subscript.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	public interface IIndexer<K, V> : IReadOnlyIndexer<K, V> {

		/// <summary>
		/// Reads or writes a value from this object using the given key value.
		/// </summary>
		/// <param name="key">Key value</param>
		/// <returns>Indexed value</returns>
		public new V this[K key] { get; set; }

	}

	/// <summary>
	/// A read-only indexer which invokes a function when read.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	public class FuncReadOnlyIndexer<K, V> : IReadOnlyIndexer<K, V> {

		private readonly Func<K, V> getter;

		public FuncReadOnlyIndexer(Func<K, V> getter) {
			this.getter = getter;
		}

		public V this[K key] => getter(key);

	}

	/// <summary>
	/// A read-only indexer which invokes a function using a constant object when read.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	/// <typeparam name="T1">Constant object type</typeparam>
	public class FuncReadOnlyIndexer<K, V, T1> : IReadOnlyIndexer<K, V> {

		private readonly Func<T1, K, V> Getter;
		private readonly T1 First;

		public FuncReadOnlyIndexer(T1 first, Func<T1, K, V> getter) {
			First = first;
			Getter = getter;
		}

		public V this[K key] => Getter(First, key);

	}

	/// <summary>
	/// An indexer which invokes a function when read or written.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	public class FuncIndexer<K, V> : IIndexer<K, V> {

		private readonly Func<K, V> Getter;
		private readonly Action<K, V> Setter;

		public FuncIndexer(Func<K, V> getter, Action<K, V> setter) {
			Getter = getter;
			Setter = setter;
		}

		public V this[K key] {
			get => Getter(key);
			set => Setter(key, value);
		}

		V IReadOnlyIndexer<K, V>.this[K key] => Getter(key);

	}

	/// <summary>
	/// An indexer which invokes a function using a constant object when read or written.
	/// </summary>
	/// <typeparam name="K">Indexer key type</typeparam>
	/// <typeparam name="V">Indexed value type</typeparam>
	/// <typeparam name="T1">Constant object type</typeparam>
	public class FuncIndexer<K, V, T1> : IIndexer<K, V> {

		private readonly Func<T1, K, V> getter;
		private readonly Action<T1, K, V> setter;
		private readonly T1 first;

		public FuncIndexer(T1 first, Func<T1, K, V> getter, Action<T1, K, V> setter) {
			this.first = first;
			this.getter = getter;
			this.setter = setter;
		}

		public V this[K key] {
			get => getter(first, key);
			set => setter(first, key, value);
		}

		V IReadOnlyIndexer<K, V>.this[K key] => getter(first, key);

	}

}
