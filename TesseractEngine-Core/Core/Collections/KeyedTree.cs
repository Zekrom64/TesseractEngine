using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesseract.Core.Collections {
	/// <summary>
	/// A keyed tree implements a tree data structure where each branch and sub-branch is identified by a key.
	/// Because this is internally implemented using dictionaries, the tree should be considered unordered,
	/// although it may be iterated recursively.
	/// </summary>
	/// <typeparam name="K"></typeparam>
	/// <typeparam name="L"></typeparam>
	public class KeyedTree<K, L> where K : notnull {

		/// <summary>
		/// A branch is an entry in a tree with an associated leaf and collection of sub-branches.
		/// </summary>
		public class Branch {

			/// <summary>
			/// The key identifying this branch.
			/// </summary>
			public K Key { get; }

			/// <summary>
			/// The leaf associated with this branch.
			/// </summary>
			public L? Leaf { get; set; }

			/// <summary>
			/// The collection of sub-branches attached to this branch.
			/// </summary>
			public IReadOnlyDictionary<K, Branch> Branches => branches;
			private readonly Dictionary<K, Branch> branches = new();

			internal Branch(K key) {
				Key = key;
			}

			/// <summary>
			/// If this branch has a sub-branch with the given key.
			/// </summary>
			/// <param name="key">Branch key</param>
			/// <returns>If the sub-branch exists</returns>
			public bool HasBranch(K key) => branches.ContainsKey(key);

			/// <summary>
			/// Gets the sub-branch with the given key.
			/// </summary>
			/// <param name="key">Branch key</param>
			/// <returns>The sub-branch with the given key</returns>
			public Branch GetBranch(K key) => branches[key];

			/// <summary>
			/// Attempts to get the sub-branch with the given key.
			/// </summary>
			/// <param name="key">Branch key</param>
			/// <param name="branch">Sub-branch or null</param>
			/// <returns>If the sub-branch exists</returns>
			public bool TryGetBranch(K key, out Branch? branch) => branches.TryGetValue(key, out branch);

			/// <summary>
			/// Gets the sub-branch with the given key or creates it if it doesn't exist.
			/// </summary>
			/// <param name="key">Branch key</param>
			/// <returns>Sub-branch</returns>
			public Branch GetOrCreateBranch(K key) {
				if (!branches.TryGetValue(key, out Branch? branch)) {
					branch = new(key);
					branches[key] = branch;
				}
				return branch;
			}

			public Branch this[K key] {
				get => branches[key];
			}


			/// <summary>
			/// If this branch contains the given sub-path.
			/// </summary>
			/// <param name="keys">Keys composing the sub-path</param>
			/// <returns>If the sub-path exists</returns>
			public bool HasPath(IEnumerable<K> keys) {
				if (!keys.Any()) return true;
				K key = keys.First();
				if (branches.TryGetValue(key, out Branch? branch)) return branch.HasPath(keys.Skip(1));
				return false;
			}

			/// <summary>
			/// Gets the sub-branch at the given sub-path.
			/// </summary>
			/// <param name="keys">Keys composing the sub-path</param>
			/// <returns>Branch at sub-path</returns>
			public Branch GetPath(IEnumerable<K> keys) {
				if (!keys.Any()) return this;
				return branches[keys.First()].GetPath(keys.Skip(1));
			}

			/// <summary>
			/// Attempts to get the sub-branch at the given sub-path.
			/// </summary>
			/// <param name="keys"></param>
			/// <param name="branch"></param>
			/// <returns></returns>
			public bool TryGetPath(IEnumerable<K> keys, out Branch? branch) {
				branch = null;
				if (!keys.Any()) {
					branch = this;
					return true;
				}
				if (branches.TryGetValue(keys.First(), out Branch? subbranch)) return subbranch.TryGetPath(keys.Skip(1), out branch);
				return false;
			}

			/// <summary>
			/// Gets the sub-path with the given keys or creates it if it doesn't exist.
			/// </summary>
			/// <param name="keys">Keys composing the sub-path</param>
			/// <returns>Branch at sub-path</returns>
			public Branch GetOrCreatePath(IEnumerable<K> keys) {
				if (!keys.Any()) return this;
				K key = keys.First();
				if (!branches.TryGetValue(key, out Branch? branch)) {
					branch = new(key);
					branches[key] = branch;
				}
				return branch.GetOrCreatePath(keys.Skip(1));
			}

			private void Iterate(Action<IReadOnlyList<K>, Branch> iterator, List<K> path) {
				int pathindex = path.Count;
				path.Add(Key);

				iterator(path, this);
				foreach (Branch branch in branches.Values) branch.Iterate(iterator, path);

				path.RemoveAt(pathindex);
			}

			/// <summary>
			/// Recursively iterates over this branch, invoking the given iterator for every branch entry.
			/// </summary>
			/// <param name="iterator">Iterator function</param>
			public void Iterate(Action<IReadOnlyList<K>, Branch> iterator) => Iterate(iterator, new());

		}

		/// <summary>
		/// The collection of branches from the root of this tree.
		/// </summary>
		public IReadOnlyDictionary<K, Branch> Branches => branches;
		private readonly Dictionary<K, Branch> branches = new();

		/// <summary>
		/// If this tree has the given branch.
		/// </summary>
		/// <param name="key">Branch key</param>
		/// <returns>If the branch exists</returns>
		public bool HasBranch(K key) => branches.ContainsKey(key);

		/// <summary>
		/// Gets the given branch.
		/// </summary>
		/// <param name="key">Branch key</param>
		/// <returns>The branch with the given key</returns>
		public Branch GetBranch(K key) => branches[key];

		/// <summary>
		/// Attempts to get a branch with the given key.
		/// </summary>
		/// <param name="key">Branch key</param>
		/// <param name="branch">The branch with the given key, or null</param>
		/// <returns>If the branch exists</returns>
		public bool TryGetBranch(K key, out Branch? branch) => branches.TryGetValue(key, out branch);

		/// <summary>
		/// Gets or creates the given branch.
		/// </summary>
		/// <param name="key">Branch key</param>
		/// <returns>Branch with the given key</returns>
		public Branch GetOrCreateBranch(K key) {
			if (!branches.TryGetValue(key, out Branch? branch)) {
				branch = new(key);
				branches[key] = branch;
			}
			return branch;
		}

		public Branch this[K key] {
			get => branches[key];
		}

		/// <summary>
		/// If this tree has a branch at the given path.
		/// </summary>
		/// <param name="keys">Keys composing the path</param>
		/// <returns>If a branch exists at the path</returns>
		public bool HasPath(IEnumerable<K> keys) {
			if (branches.TryGetValue(keys.First(), out Branch? branch)) return branch.HasPath(keys.Skip(1));
			return false;
		}

		/// <summary>
		/// Gets the branch at the given path.
		/// </summary>
		/// <param name="keys">Keys composing the path</param>
		/// <returns>The branch at the given path</returns>
		public Branch GetPath(IEnumerable<K> keys) => branches[keys.First()].GetPath(keys.Skip(1));

		/// <summary>
		/// Attempts to get the branch at the given path.
		/// </summary>
		/// <param name="keys">Keys composing the path</param>
		/// <param name="branch">The branch at the given path, or null</param>
		/// <returns>If the branch exists at the path</returns>
		public bool TryGetPath(IEnumerable<K> keys, out Branch? branch) {
			branch = null;
			if (branches.TryGetValue(keys.First(), out Branch? subbranch)) return subbranch.TryGetPath(keys.Skip(1), out branch);
			return false;
		}

		/// <summary>
		/// Gets or creates the branch at the given path.
		/// </summary>
		/// <param name="keys">Keys composing the path</param>
		/// <returns>The branch at the given path</returns>
		public Branch GetOrCreatePath(IEnumerable<K> keys) {
			K key = keys.First();
			if (!branches.TryGetValue(key, out Branch? branch)) {
				branch = new(key);
				branches[key] = branch;
			}
			return branch.GetOrCreatePath(keys.Skip(1));
		}

		/// <summary>
		/// Recursively iterates over each branch of this tree.
		/// </summary>
		/// <param name="iterator">Iterator function</param>
		public void Iterate(Action<IReadOnlyList<K>, Branch> iterator) {
			foreach (Branch branch in branches.Values) branch.Iterate(iterator);
		}

	}

}
