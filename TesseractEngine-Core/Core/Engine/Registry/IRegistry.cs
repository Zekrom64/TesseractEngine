using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.Core.Engine.Registry {

	/// <summary>
	/// A registry object is something that can be registered and referenced by an ID or unlocalized name.
	/// </summary>
	public abstract class RegistryObject {

		/// <summary>
		/// The unique unlocalized name of the object.
		/// </summary>
		public RegistryKey UnlocalizedName { get; }

		/// <summary>
		/// The runtime ID of the object. If the object has not been initialized with one, it will be -1.
		/// </summary>
		public int ID { get; internal set; } = -1;

		protected RegistryObject(RegistryKey unlocalizedName) {
			UnlocalizedName = unlocalizedName;
		}

	}

	/// <summary>
	/// A registry stores a mapping of objects by their runtime ID and name.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRegistry<T> where T : RegistryObject {

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
		/// Gets an object in the regitry by its runtime ID, throwing an exception if it does not exist.
		/// </summary>
		/// <param name="id">The runtime ID of the object</param>
		/// <returns>The retrieved object</returns>
		public T GetByID(int id);

		/// <summary>
		/// Tries to get an object by its runtime ID, returning if it was successful.
		/// </summary>
		/// <param name="id">The runtime ID of the object</param>
		/// <param name="result">The retrieved object</param>
		/// <returns>If the objet was successfully retrieved</returns>
		public bool TryGetByID(int id, [NotNullWhen(true)] out T? result);

		/// <summary>
		/// Gets an object in the registry by its unlocalized name, throwing an exception if it does not exist.
		/// </summary>
		/// <param name="key">The named key of the object</param>
		/// <returns>THe retrieved object</returns>
		public T GetByKey(RegistryKey key);

		/// <summary>
		/// Tries to get an object by its unlocalized name, returning if it was successful.
		/// </summary>
		/// <param name="key">The named key of the object</param>
		/// <param name="result">The retrieved object</param>
		/// <returns>If the objet was successfully retrieved</returns>
		public bool TryGetByKey(RegistryKey key, [NotNullWhen(true)] out T? result);

		/// <summary>
		/// Begins registering objects within a specific namespace by returning a registrator for said namespace.
		/// </summary>
		/// <param name="nameSpace">The namespace of the registrator</param>
		/// <returns>Registrator to begin registering objects with</returns>
		public IRegistrator<T> Begin(string nameSpace);

		/// <summary>
		/// Freezes the entries in the registry, disallowing any further registration, assigning
		/// IDs for all the entries, and making them available via the <tt>*GetBy*</tt> methods.
		/// </summary>
		public void Freeze();

	}

	/// <summary>
	/// A registrator is an interface that allows objects to be added to a registry within a specific namespace.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRegistrator<T> where T : RegistryObject {

		/// <summary>
		/// The name space this registrator is assigned to.
		/// </summary>
		public string NameSpace { get; }

		/// <summary>
		/// The registry this registrator belongs to.
		/// </summary>
		public IRegistry<T> Registry { get; }

		/// <summary>
		/// Registers a single entry.
		/// </summary>
		/// <param name="entry">Entry to register</param>
		public void Register(T entry);
		
		/// <summary>
		/// Registers a collection of entries.
		/// </summary>
		/// <param name="entries">Entries to register</param>
		public void Register(params T[] entries) {
			foreach(T entry in entries) Register(entry);
		}

		/// <summary>
		/// Registers a collection of entries.
		/// </summary>
		/// <param name="entries">Entries to register</param>
		public void Register(IEnumerable<T> entries) {
			foreach (T entry in entries) Register(entry);
		}

	}

}
