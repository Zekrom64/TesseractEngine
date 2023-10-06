using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Tesseract.Core.Engine.Registry {

	/// <summary>
	/// Base implementation of a <see cref="IRegistry{T}"/>.
	/// </summary>
	/// <typeparam name="T">Registry object type</typeparam>
	public class BaseRegistry<T> : IRegistry<T> where T : RegistryObject {

		public string UnlocalizedName { get; }

		public RegistryIDMap<T> CurrentIDMap { get; }

		public IReadOnlyCollection<T> Entries => byID;

		// Registry entries by name
		private readonly Dictionary<RegistryKey, T> byName = new();
		// Registry entries by ID
		private readonly List<T> byID = new();

		private class Registrator : IRegistrator<T> {

			public IRegistry<T> Registry => registry;

			public string NameSpace { get; }

			private readonly BaseRegistry<T> registry;
			internal readonly Dictionary<RegistryKey,T> itemsToRegister = new();

			public Registrator(BaseRegistry<T> registry, string nameSpace) {
				this.registry = registry;
				NameSpace = nameSpace;
			}

			public void Register(T entry) {
				var uname = entry.UnlocalizedName;
				if (uname.RegistryName != null && uname.RegistryName != registry.UnlocalizedName)
					throw new ArgumentException("Cannot register object to different registry");
				if (uname.NameSpace != NameSpace)
					throw new ArgumentException("Cannot register object from different namespace", nameof(entry));

				registry.rwlock.EnterReadLock();
				try {
					if (registry.isFrozen) throw new InvalidOperationException("Cannot register an object after the registry was frozen");
					lock (itemsToRegister) {
						if (itemsToRegister.ContainsKey(uname)) throw new ArgumentException($"Entry with name {uname} already exists");
						itemsToRegister.Add(uname.WithoutRegistryName(), entry);
					}
				} finally {
					registry.rwlock.ExitReadLock();
				}
			}

		}

		// The set of registrators by their namespaces
		private readonly Dictionary<string, Registrator> registrators = new();
		// If the registry is frozen
		private volatile bool isFrozen = false;
		// Reader/writer lock to manage concurrent registration
		private readonly ReaderWriterLockSlim rwlock = new();

		public BaseRegistry(string unlocalizedName) {
			UnlocalizedName = unlocalizedName;
			CurrentIDMap = new RegistryIDMap<T>(this);
		}

		public IRegistrator<T> Begin(string nameSpace) {
			lock(registrators) {
				if (!registrators.TryGetValue(nameSpace, out Registrator? registrator))
					registrator = new Registrator(this, nameSpace);
				return registrator;
			}
		}

		public void Freeze() {
			rwlock.EnterWriteLock();
			try {
				int nextID = 1;
				foreach(Registrator registrator in registrators.Values) {
					var items = registrator.itemsToRegister;
					foreach(var item in items) {
						item.Value.ID = nextID++;
						byID.Add(item.Value);
						byName[item.Key] = item.Value;
					}
					items.Clear();
				}
				isFrozen = true;
			} finally {
				rwlock.ExitWriteLock();
				rwlock.Dispose();
			}
		}

		public T GetByID(int id) {
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
			if (TryGetByID(id, out T? retn)) return retn;
			else throw new KeyNotFoundException();
		}

		public bool TryGetByID(int id, [NotNullWhen(true)] out T? result) {
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
			if (id < 1 || id > byID.Count) {
				result = default;
				return false;
			}
			result = byID[id - 1];
			return true;
		}

		public T GetByKey(RegistryKey name) {
			if (name.RegistryName != null && name.RegistryName != UnlocalizedName)
				throw new ArgumentException("Cannot lookup key in different registry", nameof(name));
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
			return byName[name.WithoutRegistryName()];
		}

		public bool TryGetByKey(RegistryKey name, [NotNullWhen(true)] out T? result) {
			if (name.RegistryName != null && name.RegistryName != UnlocalizedName)
				throw new ArgumentException("Cannot lookup key in different registry", nameof(name));
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
			return byName.TryGetValue(name.WithoutRegistryName(), out result);
		}

	}

}
