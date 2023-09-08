using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Collections {
	
	/// <summary>
	/// Similar to a <see cref="IDictionary{TKey, TValue}"/> but maintains an inverted copy
	/// of the entries whose contents are updated when the dictionary is modified.
	/// Note that this does not prevent the creation of a disjoint relationship between copies
	/// (eg. mapping A to C and B to C will be reflected as a mapping from C to B in the converse).
	/// Removing an element from one dictionary will remove its pair based on the existing value
	/// in the first dictionary, even if the relationship is disjoint.
	/// </summary>
	/// <typeparam name="K">Key type</typeparam>
	/// <typeparam name="V">Value type</typeparam>
	public interface IConverseDictionary<K, V> : IDictionary<K, V>, IReadOnlyDictionary<K, V> where K : notnull where V : notnull {

		/// <summary>
		/// The reversed copy of this dictionary.
		/// </summary>
		public IConverseDictionary<V, K> Reverse { get; }

	}

	/// <summary>
	/// Implementation of an <see cref="IConverseDictionary{K, V}"/> using two mirrored <see cref="Dictionary{TKey, TValue}"/>.
	/// </summary>
	/// <typeparam name="K">Key type</typeparam>
	/// <typeparam name="V">Value type</typeparam>
	public class ConverseDictionary<K, V> : IConverseDictionary<K, V> where K : notnull where V : notnull {

		private readonly Dictionary<K, V> dictionary = new();

		private readonly ConverseDictionary<V, K> reverse;

		public ConverseDictionary() {
			reverse = new ConverseDictionary<V, K>(this);
		}

		private ConverseDictionary(ConverseDictionary<V, K> reverse) {
			this.reverse = reverse;
		}

		public V this[K key] {
			get => dictionary[key];
			set {
				dictionary[key] = value;
				reverse.dictionary[value] = key;
			}
		}

		public IConverseDictionary<V, K> Reverse => reverse;

		public ICollection<K> Keys => dictionary.Keys;

		public ICollection<V> Values => dictionary.Values;

		public int Count => dictionary.Count;

		public bool IsReadOnly => false;

		IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;

		IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;

		public void Add(K key, V value) {
			dictionary[key] = value;
			reverse.dictionary[value] = key;
		}

		public void Add(KeyValuePair<K, V> item) => Add(item.Key, item.Value);

		public void Clear() {
			dictionary.Clear();
			reverse.dictionary.Clear();
		}

		public bool Contains(KeyValuePair<K, V> item) => dictionary.Contains(item);

		public bool ContainsKey(K key) => dictionary.ContainsKey(key);

		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) {
			foreach(var entry in dictionary) array[arrayIndex++] = entry;
		}

		public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => dictionary.GetEnumerator();

		public bool Remove(K key) {
			if (dictionary.TryGetValue(key, out V? value)) {
				reverse.dictionary.Remove(value);
				dictionary.Remove(key);
				return true;
			} else return false;
		}

		public bool Remove(KeyValuePair<K, V> item) {
			bool removed = false;
			if (dictionary.TryGetValue(item.Key, out V? value) && Equals(item.Value, value)) {
				dictionary.Remove(item.Key);
				removed = true;
			}
			if (reverse.dictionary.TryGetValue(item.Value, out K? key) && Equals(item.Key, key)) {
				reverse.dictionary.Remove(item.Value);
				removed = true;
			}
			return removed;
		}

		public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value) => dictionary.TryGetValue(key, out value);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

}
