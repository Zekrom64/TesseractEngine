namespace Tesseract.ImGui {
	public class ImGuiStorage {

		public struct ImGuiStoragePair {

			public int Key;

			public int AsInt {
				get => (int)AsNInt;
				set => AsNInt = value;
			}
			public float AsFloat {
				get => BitConverter.Int32BitsToSingle((int)AsNInt);
				set => AsNInt = BitConverter.SingleToInt32Bits(value);
			}
			public nint AsNInt;

		}

		public readonly List<ImGuiStoragePair> Data = new();

		public void Clear() => Data.Clear();

		public int GetInt(int key, int defaultVal = 0) { }
		public void SetInt(int key, int val) { }
		public bool GetBool(int key, bool defaultVal = false) { }
		public void SetBool(int key, bool val) { }
		public float GetFloat(int key, float defaultVal = 0) { }
		public void SetFloat(int key, float val) { }
		public nint GetPtr(int key, nint defaultVal = 0) { }
		public void SetPtr(int key, nint val) { }

		public void BuildSortByKey() {

		}

	}

}
