using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;
using Tesseract.Core.Utilities.Data;

namespace Tesseract.Core.Engine.Registry {

	/// <summary>
	/// <para>A mapping for IDs of registry objects from save to runtime.</para>
	/// <para>
	/// Because the runtime IDs of registry objects may change they cannot be relied
	/// upon to identify objects in any save data. However, it is still advantageous
	/// to identify objects in save data with integer IDs to save space. This ID mapping
	/// provides translation between the ID of an object in save data and the extant object
	/// in the runtime.
	/// </para>
	/// <para>
	/// This mapping system also accounts for the possibility of content changing between uses
	/// of save data; ID mappings for objects that do not exist at runtime are preserved
	/// and will return a null entry, and new mappings are dynamically added for objects
	/// that do not yet have one when the mappings are loaded.
	/// </para>
	/// <para>
	/// Although ID mappings can be used dynamically, each registry has a 'global' ID map
	/// which is corellated to the current save data.
	/// </para>
	/// </summary>
	/// <typeparam name="T">The type of registry object being mapped</typeparam>
	public class RegistryIDMap<T> where T : RegistryObject {

		/// <summary>
		/// The registry this mapping belongs to.
		/// </summary>
		public IRegistry<T> Registry { get; }

		// Persistent dictionary of name -> save ID
		private readonly Dictionary<string, int> saveRegistry = new();

		// Mapping lists for converting IDs
		private readonly List<int> regToSave = new();
		private readonly List<int> saveToReg = new();

		public RegistryIDMap(IRegistry<T> baseRegistry) {
			Registry = baseRegistry;
		}

		/// <summary>
		/// Resets this ID map, clearing all ID mappings.
		/// </summary>
		public void Reset() {
			regToSave.Clear();
			saveToReg.Clear();
		}

		private static void AddMapping(List<int> map, int src, int dst) {
			if (src < 0) return;
			if (src >= map.Count) map.AddRange(LINQ.Dup(-1, (src + 1) - map.Count));
			map[src] = dst;
		}

		/// <summary>
		/// Imports ID mappings from the given data, additionally initialzing
		/// any new mappings from the linked registry.
		/// </summary>
		/// <param name="mapData">ID mapping data</param>
		public void Import(DataObject mapData) {
			// Copy name-ID mapping to a dictionary
			saveRegistry.Clear();
			foreach (var entry in mapData) saveRegistry[entry.Key] = (int)entry.Value;

			// Initialize mapping lists with entries that exist in the registry
			foreach(var entry in saveRegistry) {
				RegistryKey name = entry.Key;
				int saveID = entry.Value;
				if (Registry.TryGetByKey(name, out T? val)) {
					AddMapping(regToSave, val.ID, saveID);
					AddMapping(saveToReg, saveID, val.ID);
				} else {
					AddMapping(saveToReg, saveID, -1);
				}
			}

			// Initialize new save ID mappings for objects in the registry that are not in the current mapping
			foreach(T entry in Registry.Entries) {
				if (!saveRegistry.ContainsKey(entry.UnlocalizedName.ToString())) {
					int saveID = saveToReg.Count;
					if (saveID == 0) saveID = 1;

					AddMapping(regToSave, entry.ID, saveID);
					AddMapping(saveToReg, saveID, entry.ID);
					saveRegistry[entry.UnlocalizedName.ToString()] = saveID;
				}
			}
		}

		/// <summary>
		/// Exports ID mappings to the given data stream.
		/// </summary>
		/// <param name="mapData">Data to export ID mapping to</param>
		public void Export(IStreamingDataObject mapData) {
			foreach (var entry in saveRegistry) mapData[entry.Key] = entry.Value;
		}

		/// <summary>
		/// Loads a registry object from the given save ID.
		/// </summary>
		/// <param name="id">Save data ID value</param>
		/// <returns>Corresponding object, or null if none exists</returns>
		public T? Load(int id) {
			if (id < 0 || id >= saveToReg.Count) return default;
			id = saveToReg[id];
			return Registry.TryGetByID(id, out T? item) ? item : default;
		}

		/// <summary>
		/// Stores a registry object as a save ID.
		/// </summary>
		/// <param name="item">The object to save</param>
		/// <returns>Corresponding save ID</returns>
		/// <exception cref="ArgumentException">If the registry object cannot be saved</exception>
		public int Store(T item) {
			int id = item.ID;
			if (id < 0 || id >= regToSave.Count) throw new ArgumentException("Registry object has invalid ID", nameof(item));
			id = regToSave[id];
			if (id == -1) throw new ArgumentException("Registry object does not have save ID mapping", nameof(item));
			return id;
		}

	}

}
