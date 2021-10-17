using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {
	
	/// <summary>
	/// An equatable list is a value type that can check for equaltity between lists. List
	/// equality means that the lists are the same length and every element at corresponding
	/// indicies is equal. Element equality is checked using the standard
	/// <see cref="object.Equals(object?, object?)"/> function.
	/// </summary>
	/// <typeparam name="T">List element type</typeparam>
	public struct EquatableList<T> : IEquatable<EquatableList<T>>, IReadOnlyList<T> {

		/// <summary>
		/// The underlying list.
		/// </summary>
		public readonly IReadOnlyList<T> List;

		public EquatableList(IReadOnlyList<T> list) { List = list; }

		public EquatableList(IEnumerable<T> e) { List = new List<T>(e); }

		public T this[int index] => List[index];

		public int Count => List.Count;

		public bool Equals(EquatableList<T> other) {
			if (List == null && other.List == null) return true;
			else if (List == null ^ other.List == null) return false;

			if (List.Count != other.List.Count) return false;
			for (int i = 0; i < List.Count; i++) if (!Equals(List[i], other.List[i])) return false;
			return true;
		}

		public override bool Equals(object obj) => obj is EquatableList<T> list && Equals(list);

		public override int GetHashCode() => List != null ? List.GetHashCode() : 0;

		public override string ToString() => List != null ? List.ToString() : "null";


		public IEnumerator<T> GetEnumerator() => List.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();


		public static bool operator ==(EquatableList<T> left, EquatableList<T> right) => left.Equals(right);

		public static bool operator !=(EquatableList<T> left, EquatableList<T> right) => !left.Equals(right);

	}

	/// <summary>
	/// An equatable set is a value type that can check for equaltity between sets. Set equality
	/// means that each set contains exactly all of the other set's elements. This is checked
	/// by testing that both sets are the same size and that at least one set contains all the
	/// elements of the opposite set.
	/// </summary>
	/// <typeparam name="T">Set element type</typeparam>
	public struct EquatableSet<T> : IEquatable<EquatableSet<T>>, IReadOnlySet<T> {

		/// <summary>
		/// The underlying set.
		/// </summary>
		public readonly IReadOnlySet<T> Set;

		public EquatableSet(IReadOnlySet<T> list) { Set = list; }

		public EquatableSet(IEnumerable<T> e) { Set = new HashSet<T>(e); }

		public int Count => Set.Count;

		public bool Equals(EquatableSet<T> other) {
			if (Set == null && other.Set == null) return true;
			else if (Set == null ^ other.Set == null) return false;

			if (Set.Count != other.Set.Count) return false;
			foreach (T t in Set) if (!other.Set.Contains(t)) return false;
			return true;
		}

		public override bool Equals(object obj) => obj is EquatableSet<T> list && Equals(list);

		public override int GetHashCode() => Set != null ? Set.GetHashCode() : 0;

		public override string ToString() => Set != null ? Set.ToString() : "null";


		public bool Contains(T item) => Set.Contains(item);

		public IEnumerator<T> GetEnumerator() => Set.GetEnumerator();

		public bool IsProperSubsetOf(IEnumerable<T> other) => Set.IsProperSubsetOf(other);

		public bool IsProperSupersetOf(IEnumerable<T> other) => Set.IsProperSupersetOf(other);

		public bool IsSubsetOf(IEnumerable<T> other) => Set.IsSubsetOf(other);

		public bool IsSupersetOf(IEnumerable<T> other) => Set.IsSupersetOf(other);

		public bool Overlaps(IEnumerable<T> other) => Set.Overlaps(other);

		public bool SetEquals(IEnumerable<T> other) => Set.SetEquals(other);

		IEnumerator IEnumerable.GetEnumerator() => Set.GetEnumerator();


		public static bool operator ==(EquatableSet<T> left, EquatableSet<T> right) => left.Equals(right);

		public static bool operator !=(EquatableSet<T> left, EquatableSet<T> right) => !left.Equals(right);

	}

	/// <summary>
	/// <para>
	/// A hashed value is a value type that encapsulates a value and a cached hash code for
	/// that value. The hashed value overrides hash code and equality functions such that
	/// the hash code of the hashed value is the cached hash code and hash equality is
	/// checked before regular object equality. Note that because the hash code is cached
	/// the type of the underlying value should be immutable/unmodifiable to prevent
	/// the hash code from changing once initialized.
	/// </para>
	/// <para>The primary use for hashed values is to improve performance of hash-based
	/// containers (dictionaries, sets) when using complex keys that may have significant
	/// overhead in generating hash codes.</para>
	/// </summary>
	/// <typeparam name="T">Underlying value type</typeparam>
	public struct HashedValue<T> : IEquatable<HashedValue<T>>, IEquatable<T> {

		/// <summary>
		/// Computes the hash code for any type. This will return 0 if the value is null for
		/// nullable types, otherwise it returns the regular <see cref="GetHashCode"/> result.
		/// </summary>
		/// <param name="val">Type value</param>
		/// <returns>Value hash code</returns>
		public static int Hash(T val) {
			if (Equals(null, val)) return 0;
			else return val.GetHashCode();
		}

		private bool flag;
		private T value;
		/// <summary>
		/// The underlying value being hashed.
		/// </summary>
		public T Value {
			get => value;
			set {
				flag = true;
				this.value = value;
			}
		}

		private int hash;
		/// <summary>
		/// The cached hash code for the corresponding value.
		/// </summary>
		public int HashValue {
			get {
				if (flag) hash = Hash(value);
				return hash;
			}
		}

		/// <summary>
		/// Creates a new hashed value.
		/// </summary>
		/// <param name="val">Initial value</param>
		public HashedValue(T val) {
			value = val;
			hash = Hash(val);
			flag = false;
		}

		public bool Equals(HashedValue<T> other) => HashValue == other.HashValue && Equals(Value, other.Value);

		public bool Equals(T val) => HashValue == Hash(val) && Equals(Value, val);

		public override bool Equals(object obj) =>
			(obj is HashedValue<T> hv && Equals(hv)) ||
			(obj is T val && Equals(val));

		public override int GetHashCode() => HashValue;

		public static bool operator ==(HashedValue<T> hv1, HashedValue<T> hv2) => hv1.Equals(hv2);

		public static bool operator !=(HashedValue<T> hv1, HashedValue<T> hv2) => !hv1.Equals(hv2);

		public static implicit operator HashedValue<T>(T val) => new(val);

		public static implicit operator T(HashedValue<T> hv) => hv.Value;

	}

}
