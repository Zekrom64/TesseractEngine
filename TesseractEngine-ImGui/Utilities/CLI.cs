using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
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

		public class DictionaryKeyCollection<K, V> : ICollection<K> {

			public IDictionary<K, V> Dictionary { get; }

			public DictionaryKeyCollection(IDictionary<K, V> dictionary) {
				Dictionary = dictionary;
			}

			public int Count => Dictionary.Count;

			public bool IsReadOnly => true;

			public void Add(K item) => throw new NotSupportedException();

			public void Clear() => throw new NotSupportedException();

			public bool Contains(K item) => Dictionary.ContainsKey(item);

			public void CopyTo(K[] array, int arrayIndex) {
				foreach (var entry in Dictionary) array[arrayIndex++] = entry.Key;
			}

			public IEnumerator<K> GetEnumerator() {
				foreach (var entry in Dictionary) yield return entry.Key;
			}

			public bool Remove(K item) => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		}

		public class DictionaryValueCollection<K,V> : ICollection<V> {
			
			public IDictionary<K,V> Dictionary { get; }

			public DictionaryValueCollection(IDictionary<K, V> dictionary) {
				Dictionary = dictionary;
			}

			public int Count => Dictionary.Count;

			public bool IsReadOnly => true;

			public void Add(V item) => throw new NotSupportedException();

			public void Clear() => throw new NotSupportedException();

			public bool Contains(V item) {
				foreach(var entry in Dictionary)
					if (Equals(entry.Value, item)) return true;
				return false;
			}

			public void CopyTo(V[] array, int arrayIndex) {
				foreach (var entry in Dictionary) array[arrayIndex++] = entry.Value;
			}

			public IEnumerator<V> GetEnumerator() {
				foreach (var entry in Dictionary) yield return entry.Value;
			}

			public bool Remove(V item) => throw new NotSupportedException();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		}

		/// <summary>
		/// Container class for string parameters passed to ImGui. ImGui expects ASCII/UTF-8 strings while
		/// .NET uses UTF-16 strings, so this will convert as needed. This is primarily used internally
		/// for marshalling to the CLI implementation.
		/// </summary>
		public ref struct StringParam {

			private const int StackBufferSize = 1024;

			private unsafe fixed byte stackBuffer[StackBufferSize];

			private Span<byte> StackBufferSpan {
				get {
					unsafe {
						fixed (byte* ptr = stackBuffer) {
							return new Span<byte>(ptr, StackBufferSize - 1);
						}
					}
				}
			}

			private readonly Memory<byte> heapBuffer;

			private readonly int byteLength;

			/// <summary>
			/// Gets the underlying bytes for this string.
			/// </summary>
			public ReadOnlySpan<byte> Bytes => (byteLength < StackBufferSize ? StackBufferSpan : heapBuffer.Span)[..byteLength];

			/// <summary>
			/// Creates a new temporary string parameter.
			/// </summary>
			/// <param name="str">String value to set</param>
			public StringParam(string? str) {
				heapBuffer = default;
				byteLength = 0;
				StackBufferSpan.Fill(0);

				if (str != null) {
					// Try to convert into the stack allocated buffer first
					var encode = Encoding.UTF8.GetEncoder();
					encode.Convert(str, StackBufferSpan, false, out _, out byteLength, out bool completed);

					// If unable to convert the whole string, convert via heap-allocated memory
					if (!completed) {
						heapBuffer = Encoding.UTF8.GetBytes(str + '\0');
						byteLength = heapBuffer.Length - 1;
					}
				}
			}

			public static implicit operator StringParam(string? str) => new(str);

			public static implicit operator ReadOnlySpan<byte>(StringParam param) => param.Bytes;

		}

	}

}
