using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public abstract class ImGuiNETListBase<T> : IList<T> {

		public abstract T this[int index] { get; set; }

		public bool IsReadOnly => false;

		public abstract int Count { get; }

		public void Add(T item) => Insert(0, item);

		public abstract void Clear();

		public bool Contains(T item) => IndexOf(item) >= 0;

		public void CopyTo(T[] array, int arrayIndex) {
			for (int i = 0; i < Count; i++) array[arrayIndex++] = this[i];
		}

		public IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < Count; i++) yield return this[i];
		}

		public int IndexOf(T item) {
			for (int i = 0; i < Count; i++) {
				if (Equals(this[i], item)) return i;
			}
			return -1;
		}

		public abstract void Insert(int index, T item);

		public bool Remove(T item) {
			int index = IndexOf(item);
			if (index < 0) return false;
			RemoveAt(index);
			return true;
		}

		public abstract void RemoveAt(int index);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	}

}
