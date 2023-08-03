using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;
using Tesseract.Core.Utilities.Data;

namespace Tesseract.Core.Engine.Registry {

	public class RegistryIDMap<T> where T : IRegistryObject {

		public IRegistry<T> Registry { get; }

		private readonly List<int> regToSave = new();
		private readonly List<int> saveToReg = new();

		public RegistryIDMap(IRegistry<T> baseRegistry) {
			Registry = baseRegistry;
		}

		public void Reset() {
			regToSave.Clear();
			saveToReg.Clear();
		}

		public void Initialize(DataObject mapData) {
			static void AddMapping(List<int> map, int src, int dst) {
				if (src < 0) return;
				if (src >= map.Count) map.AddRange(LINQ.Dup(-1, (src + 1) - map.Count));
				map[src] = dst;
			}

			foreach(var entry in mapData) {
				RegistryKey name = entry.Key;
				int saveID = (int)entry.Value;
				if (Registry.TryGetByKey(name, out T? val)) {
					AddMapping(regToSave, val.ID, saveID);
					AddMapping(saveToReg, saveID, val.ID);
				} else {
					AddMapping(saveToReg, saveID, -1);
				}
			}

			foreach(T entry in Registry.Entries) {
				if (!mapData.ContainsKey(entry.UnlocalizedName))
			}
		}

		public T? Load(int id) {

		}

		public int Store(T item) {

		}

	}

}
