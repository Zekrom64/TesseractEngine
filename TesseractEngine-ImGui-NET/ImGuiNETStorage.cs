using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETStorage : IImGuiStorage {

		private static readonly uint sizeofStorage = (uint)Marshal.SizeOf<ImGuiStorage>();

		internal readonly ImGuiStoragePtr storage;

		private readonly bool allocd;

		internal ImGuiNETStorage(ImGuiStoragePtr storage) {
			this.storage = storage;
			allocd = false;
		}

		internal ImGuiNETStorage() {
			storage = ImGuiNET.ImGui.MemAlloc(sizeofStorage);
			unsafe {
				storage.NativePtr->Data = default;
			}
			allocd = true;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				if (allocd) ImGuiNET.ImGui.MemFree((nint)storage.NativePtr);
			}
		}


		public void BuildSortByKey() => storage.BuildSortByKey();

		public void Clear() => storage.Clear();

		public bool GetBool(uint key, bool defaultVal = false) => storage.GetBool(key, defaultVal);

		public float GetFloat(uint key, float defaultVal = 0) => storage.GetFloat(key, defaultVal);

		public int GetInt(uint key, int defaultVal = 0) => storage.GetInt(key, defaultVal);

		public nint GetPtr(uint key, nint defaultVal = 0) {
			nint val = storage.GetVoidPtr(key);
			if (val == 0) val = defaultVal;
			return val;
		}

		public void SetBool(uint key, bool val) => storage.SetBool(key, val);

		public void SetFloat(uint key, float val) => storage.SetFloat(key, val);

		public void SetInt(uint key, int val) => storage.SetInt(key, val);

		public void SetPtr(uint key, nint val) => storage.SetVoidPtr(key, val);

	}

}
