using System;

namespace Tesseract.Core.Util {

	public interface IReadOnlyIndexer<K, V> {

		public V this[K key] { get; }

	}

	public interface IIndexer<K, V> : IReadOnlyIndexer<K, V> {

		public new V this[K key] { get; set; }

	}

	public class FuncReadOnlyIndexer<K, V> : IReadOnlyIndexer<K, V> {

		public readonly Func<K, V> Getter;

		public FuncReadOnlyIndexer(Func<K, V> getter) {
			Getter = getter;
		}

		public V this[K key] => Getter(key);

	}

	public class FuncReadOnlyIndexer<K, V, T1> : IReadOnlyIndexer<K, V> {

		public readonly Func<T1, K, V> Getter;
		public readonly T1 First;

		public FuncReadOnlyIndexer(T1 first, Func<T1, K, V> getter) {
			First = first;
			Getter = getter;
		}

		public V this[K key] => Getter(First, key);

	}

	public class FuncIndexer<K, V> : IIndexer<K, V> {

		public readonly Func<K, V> Getter;
		public readonly Action<K, V> Setter;

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

	public class FuncIndexer<K, V, T1> : IIndexer<K, V> {

		public readonly Func<T1, K, V> Getter;
		public readonly Action<T1, K, V> Setter;
		public readonly T1 First;

		public FuncIndexer(T1 first, Func<T1, K, V> getter, Action<T1, K, V> setter) {
			First = first;
			Getter = getter;
			Setter = setter;
		}

		public V this[K key] {
			get => Getter(First, key);
			set => Setter(First, key, value);
		}

		V IReadOnlyIndexer<K, V>.this[K key] => Getter(First, key);

	}

}
