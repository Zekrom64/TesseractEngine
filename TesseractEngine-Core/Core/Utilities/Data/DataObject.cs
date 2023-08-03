using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Tesseract.Core.Collections;

namespace Tesseract.Core.Utilities.Data {

	/// <summary>
	/// <para>A mapping of generic data compatible with <see cref="DataBox"/> by string names.</para>
	/// </summary>
	public class DataObject :
		IEquatable<DataObject>,
		ICollection<KeyValuePair<string, DataBox>>,
		IReadOnlyCollection<KeyValuePair<string, DataBox>>,
		IDictionary<string, DataBox>,
		IReadOnlyDictionary<string, DataBox>,
		IEnumerable<KeyValuePair<string, DataBox>>,
		IConstructibleData<DataObject>,
		IStreamingDataObject {

		// The current hash code of the object
		internal int? hashCode = null;
		// The current string representation of the object
		private string? asString = null;

		private void Invalidate() {
			hashCode = null;
			asString = null;
		}

		private Dictionary<string, DataBox>? data = null;
		private Dictionary<string, DataBox> Data {
			get {
				data ??= new();
				return data;
			}
		}

		/// <summary>
		/// The number of entries in the object.
		/// </summary>
		public int Count => data?.Count ?? 0;

		public bool IsReadOnly => false;

		public ICollection<string> Keys => data?.Keys as ICollection<string> ?? Array.Empty<string>();

		public ICollection<DataBox> Values => data?.Values as ICollection<DataBox> ?? Array.Empty<DataBox>();

		IEnumerable<string> IReadOnlyDictionary<string, DataBox>.Keys => Keys;

		IEnumerable<DataBox> IReadOnlyDictionary<string, DataBox>.Values => Values;

		public DataBox this[string key] {
			get => data?[key] ?? throw new KeyNotFoundException("Data object is empty");
			set => Add(key, value);
		}

		//===================//
		// Read-Only Methods //
		//===================//

		/// <summary>
		/// Gets a value from the object.
		/// </summary>
		/// <typeparam name="T">The type of the value</typeparam>
		/// <param name="name">The name of the value</param>
		/// <returns>The value from the object</returns>
		/// <exception cref="KeyNotFoundException">If the name does not exist in this object</exception>
		/// <exception cref="InvalidCastException">If the value exists but is of a different type</exception>
		public T Get<T>(string name) {
			if (data == null) throw new KeyNotFoundException($"Data object is empty");
			return data[name].As<T>();
		}

		public bool TryGetValue(string key, [MaybeNullWhen(false)] out DataBox value) {
			if (data == null) {
				value = default;
				return false;
			}
			return data.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<string, DataBox>> GetEnumerator() =>
			data?.GetEnumerator() ?? Collection<KeyValuePair<string, DataBox>>.EmptyEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool Contains(KeyValuePair<string, DataBox> item) {
			if (data == null) return false;
			if (data.TryGetValue(item.Key, out DataBox value)) return item.Value == value;
			else return false;
		}

		public bool ContainsKey(string key) => data != null && data.ContainsKey(key);

		public void CopyTo(KeyValuePair<string, DataBox>[] array, int arrayIndex) {
			if (Count == 0) return;
			foreach(string key in data!.Keys) {
				var value = data![key];
				array[arrayIndex++] = new KeyValuePair<string, DataBox>(key, value);
			}
		}

		//======================//
		// Modification Methods //
		//======================//

		public void Add(KeyValuePair<string, DataBox> pair) => Add(pair.Key, pair.Value);

		public void Add(string name, DataBox value) {
			Data[name] = value;
			Invalidate();
		}

		public void Clear() {
			data = null;
			Invalidate();
		}

		public bool Remove(string name) {
			bool existed = data?.Remove(name) ?? false;
			Invalidate();
			return existed;
		}

		public bool Remove(KeyValuePair<string, DataBox> item) {
			if (data == null) return false;
			if (!data.TryGetValue(item.Key, out DataBox value)) return false;
			if (value == item.Value) {
				data.Remove(item.Key);
				Invalidate();
				return true;
			} else return false;
		}


		public override int GetHashCode() {
			// If hash code is invalid recalculate
			if (hashCode == null) {
				// Shortcut if empty
				if (Count == 0) hashCode = 0;
				else {
					// Iterate the object in order and hash based on the keys and values
					uint hash = 0;
					var data = this.data!;
					foreach(string key in data.Keys.Order()) {
						hash ^= (uint)key.GetHashCode();
						hash = BitOperations.RotateLeft(hash, 5);
						hash ^= (uint)data[key].GetHashCode();
						hash = BitOperations.RotateRight(hash, 3);
					}
					hashCode = (int)hash;
				}
			}
			return hashCode.Value;
		}

		public bool Equals(DataObject? other) {
			// Not equal to null
			if (other == null) return false;
			// Equal if same reference
			if (ReferenceEquals(this, other)) return true;
			// Not equal if counts differ
			if (Count != other.Count) return false;
			// Not equal if hashes are available and differ
			if (hashCode != null && other.hashCode != null && hashCode != other.hashCode) return false;

			// Counts are equal so if data is null both are empty
			if (data == null) return true;
			else {
				// Do a deep compare of both
				foreach(string key in Keys) {
					if (!other.TryGetValue(key, out DataBox value)) return false;
					if (data[key] != value) return false;
				}
				return false;
			}
		}

		public override bool Equals(object? obj) => obj is DataObject data && Equals(data);


		//=======================//
		// Miscellaneous Methods //
		//=======================//


		public override string ToString() {
			if (asString == null) {
				StringBuilder sb = new();
				// Format is '{<key>:<value>, ... }'
				sb.Append('{');
				if (data != null && data.Count > 0) {
					foreach(string key in data.Keys) {
						sb.Append(key);
						sb.Append(':');
						sb.Append(data[key]);
						sb.Append(',');
					}
					sb.Remove(sb.Length - 1, 1);
				}
				sb.Append('}');
				asString = sb.ToString();
			}
			return asString;
		}

		public static DataObject Construct(BinaryReader br) {
			DataObject value = new();

			int count = br.Read7BitEncodedInt();
			if (count == 0) return value;

			for(int i = 0; i < count; i++) {
				string name = StructuredData.ReadString(br);
				var entry = DataBox.Construct(br);
				value[name] = entry;
			}

			return value;
		}

		public void Write(BinaryWriter bw) {
			bw.Write7BitEncodedInt(Count);

			if (data != null && data.Count > 0) {
				foreach(string key in data.Keys) {
					StructuredData.WriteString(key, bw);
					data[key].Write(bw);
				}
			}
		}

		public void Write(string key, Action<IStreamingDataObject> writer) {
			DataObject data2 = new();
			writer(data2);
			this[key] = data2;
		}

	}

}
