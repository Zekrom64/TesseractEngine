using System;
using System.Collections;
using System.Collections.Generic;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Collections {
	/// <summary>
	/// A threadsafe list implementation which provides atomicity by creating a copy of the list every time it is modified.
	/// Enumeration will always be consistent with the list at the time the enumerator was created.
	/// </summary>
	/// <typeparam name="T">The element type</typeparam>
	public class CopyOnWriteList<T> : IList<T>, IReadOnlyList<T> {

		// The lock for atomically modifying the array
		private readonly object arrayLock = new();
		// The current array value
		private volatile T[] array = Array.Empty<T>();

		public T this[int index] {
			get => array[index];
			set {
				lock (arrayLock) {
					T[] newarray = new T[array.Length];
					array.CopyTo(newarray.AsSpan());
					newarray[index] = value;
					array = newarray;
				}
			}
		}

		public int Count => array.Length;

		public bool IsReadOnly => false;

		public void Add(T item) {
			lock (arrayLock) {
				Insert(array.Length, item);
			}
		}

		public void Clear() {
			lock (arrayLock) {
				array = Array.Empty<T>();
			}
		}

		public bool Contains(T item) => IndexOf(item) >= 0;

		public void CopyTo(T[] array, int arrayIndex) {
			this.array.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator() {
			T[] array = this.array;
			for (int i = 0; i < array.Length; i++) yield return array[i];
		}

		public int IndexOf(T item) {
			T[] array = this.array;
			for (int i = 0; i < array.Length; i++) if (Equals(item, array[i])) return i;
			return -1;
		}

		public void Insert(int index, T item) {
			lock (arrayLock) {
				if (index < 0 || index > array.Length) throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be in the range [0, Count]");
				T[] newarray = new T[array.Length + 1];
				if (index != 0) Array.Copy(array, 0, newarray, 0, index);
				if (index != array.Length) Array.Copy(array, index, newarray, index + 1, array.Length - index);
				newarray[array.Length] = item;
				array = newarray;
			}
		}

		public bool Remove(T item) {
			lock (arrayLock) {
				int index = IndexOf(item);
				if (index >= 0) {
					RemoveAt(index);
					return true;
				}
				else return false;
			}
		}

		public void RemoveAt(int index) {
			lock (arrayLock) {
				if (index < 0 || index >= array.Length) throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be in the range [0, Count)");
				T[] newarray = new T[array.Length - 1];
				if (index != 0) Array.Copy(array, 0, newarray, 0, index);
				if (index != array.Length - 1) Array.Copy(array, index + 1, newarray, index, array.Length - index - 1);
				array = newarray;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	}

}
