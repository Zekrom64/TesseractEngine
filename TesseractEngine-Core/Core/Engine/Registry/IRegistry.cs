using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesseract.Core.Engine.Registry {

	/// <summary>
	/// A registry object is something that can be registered and referenced by an ID or unlocalized name.
	/// </summary>
	public interface IRegistryObject {

		/// <summary>
		/// The unique unlocalized name of the object.
		/// </summary>
		public RegistryKey UnlocalizedName { get; }

		/// <summary>
		/// The runtime ID of the object.
		/// </summary>
		public int ID { get; set; }

	}

	/// <summary>
	/// A registry stores a mapping of objects by their runtime ID and name.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRegistry<T> where T : IRegistryObject {

		/// <summary>
		/// The unlocalized name of this registry.
		/// </summary>
		public string UnlocalizedName { get; }

		/// <summary>
		/// The current ID mapping for any loaded save data.
		/// </summary>
		public RegistryIDMap<T> CurrentIDMap { get; }

		/// <summary>
		/// The collection of all entries in the registry.
		/// </summary>
		public IReadOnlyCollection<T> Entries { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public T? GetByID(int id);

		public bool TryGetByID(int id, [NotNullWhen(true)] out T? result);

		public T? GetByKey(RegistryKey key);

		public bool TryGetByKey(RegistryKey key, [NotNullWhen(true)] out T? result);

		public IRegistrator<T> Begin(string nameSpace);

		public void Freeze();

	}

	public interface IRegistrator<T> where T : IRegistryObject {

		public IRegistry<T> Registry { get; }

		public void Register(T entry);
		
		public void Register(params T[] entries) {
			foreach(T entry in entries) Register(entry);
		}

		public void Register(IEnumerable<T> entries) {
			foreach (T entry in entries) Register(entry);
		}

	}

	public class BaseRegistry<T> : IRegistry<T> where T : IRegistryObject {

		public string UnlocalizedName { get; }

		public RegistryIDMap<T> CurrentIDMap { get; }

		public IReadOnlyCollection<T> Entries => byID;

		private readonly Dictionary<RegistryKey, T> byName = new();
		private readonly List<T> byID = new();

		private class Registrator : IRegistrator<T> {

			private readonly BaseRegistry<T> registry;
			private readonly string nameSpace;
			private readonly List<T> itemsToRegister = new();

			public IRegistry<T> Registry => registry;

			public Registrator(BaseRegistry<T> registry, string nameSpace) {
				this.registry = registry;
				this.nameSpace = nameSpace;
			}

			public void Register(T entry) {
				registry.rwlock.EnterReadLock();
				try {
					if (registry.isFrozen) throw new InvalidOperationException("Cannot register an object after the registry was frozen");
					lock (itemsToRegister) {
						itemsToRegister.Add(entry);
					}
				} finally {
					registry.rwlock.ExitReadLock();
				}
			}

		}

		private readonly Dictionary<string, Registrator> registrators = new();
		private volatile bool isFrozen = false;
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
				foreach(Registrator registrator in registrators.Values) {
					
				}
			} finally {
				rwlock.ExitWriteLock();
				rwlock.Dispose();
			}
			isFrozen = true;
		}

		public T? GetByID(int id) {
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
		}

		public bool TryGetByID(int id, [NotNullWhen(true)] out T? result) {
			if (!isFrozen) throw new InvalidOperationException("Cannot access the registry before it is frozen");
			throw new NotImplementedException();
		}

		public T GetByKey(RegistryKey name) {

		}

		public bool TryGetByKey(RegistryKey name, [NotNullWhen(true)] out T? result) {

		}

	}

}
