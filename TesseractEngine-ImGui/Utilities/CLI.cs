using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.Utilities {

	namespace CLI {

		/// <summary>
		/// Enumerator implementation for lists. Can be used in cases where yield returns are not available (such as C++/CLI).
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		public class ListEnumerator<T> : IEnumerator<T> {

			private readonly IReadOnlyList<T> list;
			private int index = -1;

			public ListEnumerator(IReadOnlyList<T> list) {
				this.list = list;
			}

			T IEnumerator<T>.Current => list[index];

			object IEnumerator.Current => list[index]!;

			void IDisposable.Dispose() {
				GC.SuppressFinalize(this);
			}

			bool IEnumerator.MoveNext() => ++index < list.Count;

			void IEnumerator.Reset() => index = -1;

		}

		public abstract class ReadOnlyListBase<T> : IReadOnlyList<T> {

			protected abstract T Get(int index);

			public T this[int index] => Get(index);

			public abstract int Count { get; }

			public IEnumerator<T> GetEnumerator() {
				return new ListEnumerator<T>(this);
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}
		}

		/// <summary>
		/// Base implementation for CLI lists, solves some overriding problems and simplifies implementation.
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		public abstract class ListBase<T> : ReadOnlyListBase<T>, IList<T> {

			protected abstract void Set(int index, T value);

			T IList<T>.this[int index] { get => Get(index); set => Set(index, value); }

			public bool IsReadOnly => false;

			public abstract void Add(T item);

			public abstract void Clear();

			public bool Contains(T item) {
				return IndexOf(item) >= 0;
			}

			public void CopyTo(T[] array, int arrayIndex) {
				int length = Math.Min(Count, array.Length - arrayIndex);
				for (int i = 0; i < length; i++) array[arrayIndex + i] = this[i];
			}

			public abstract int IndexOf(T item);

			public abstract void Insert(int index, T item);

			public bool Remove(T item) {
				int index = IndexOf(item);
				if (index < 0) return false;
				RemoveAt(index);
				return true;
			}

			public abstract void RemoveAt(int index);

		}

	}

}
