using System;
using System.Collections;
using System.Collections.Generic;

namespace Tesseract.Core.Collections {

	/// <summary>
	/// An equatable list is a value type that can check for equaltity between lists. List
	/// equality means that the lists are the same length and every element at corresponding
	/// indicies is equal. Element equality is checked using the standard
	/// <see cref="object.Equals(object?, object?)"/> function.
	/// </summary>
	/// <typeparam name="T">List element type</typeparam>
	public readonly struct EquatableList<T> : IEquatable<EquatableList<T>>, IReadOnlyList<T> {

		/// <summary>
		/// The underlying list.
		/// </summary>
		public readonly IReadOnlyList<T> List;

		public EquatableList(IReadOnlyList<T> list) { List = list; }

		public EquatableList(IEnumerable<T> e) { List = new List<T>(e); }

		public T this[int index] => List != null ? List[index] : default!;

		public int Count => List != null ? List.Count : 0;

		public bool Equals(EquatableList<T> other) {
			if (List.Count != other.List.Count) return false;
			for (int i = 0; i < List.Count; i++) if (!Equals(List[i], other.List[i])) return false;
			return true;
		}

		public override bool Equals(object? obj) => obj is EquatableList<T> list && Equals(list);

		public override int GetHashCode() => List.GetHashCode();

		public override string? ToString() => List.ToString();


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
	public readonly struct EquatableSet<T> : IEquatable<EquatableSet<T>>, IReadOnlySet<T> {

		/// <summary>
		/// The underlying set.
		/// </summary>
		public readonly IReadOnlySet<T> Set;

		public EquatableSet(IReadOnlySet<T> list) { Set = list; }

		public EquatableSet(IEnumerable<T> e) { Set = new HashSet<T>(e); }

		public int Count => Set.Count;

		public bool Equals(EquatableSet<T> other) {
			if (Set.Count != other.Set.Count) return false;
			foreach (T t in Set) if (!other.Set.Contains(t)) return false;
			return true;
		}

		public override bool Equals(object? obj) => obj is EquatableSet<T> list && Equals(list);

		public override int GetHashCode() => Set.GetHashCode();

		public override string? ToString() => Set.ToString();


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

}
