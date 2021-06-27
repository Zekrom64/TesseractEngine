using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {

	public interface IReadOnlyIndexer<K, V> {

		public V this[K key] { get; }

	}

	public interface IIndexer<K,V> : IReadOnlyIndexer<K,V> {

		public new V this[K key] { get; set; }

	}

	public class FuncReadOnlyIndexer<K,V> : IReadOnlyIndexer<K,V> {

		public readonly Func<K, V> Getter;

		public FuncReadOnlyIndexer(Func<K,V> getter) {
			Getter = getter;
		}

		public V this[K key] => Getter(key);

	}

	public class FuncReadOnlyIndexer<K,V,T1> : IReadOnlyIndexer<K,V> {

		public readonly Func<T1, K, V> Getter;
		public readonly T1 First;

		public FuncReadOnlyIndexer(Func<T1,K,V> getter, T1 t1) {
			Getter = getter;
			First = t1;
		}

		public V this[K key] => Getter(First, key);

	}

}
