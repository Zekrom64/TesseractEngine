namespace Tesseract.ImGui.Internal {
	internal class ImBitVector {

		public readonly List<uint> Storage = new();

		public void Create(int sz) {
			sz = (sz + 31) >> 5;
			Storage.EnsureCapacity(sz);
			Storage.AddRange(new uint[Storage.Count - sz]);
		}

		public void Clear() => Storage.Clear();

		public bool TestBit(int n) => (Storage[n >> 5] & (1 << (n & 0x1F))) != 0;

		public void SetBit(int n) {
			int i = n >> 5;
			uint u = Storage[i];
			u |= 1u << (n & 0x1F);
			Storage[i] = u;
		}

		public void ClearBit(int n) {
			int i = n >> 5;
			uint u = Storage[i];
			u &= ~(1u << (n & 0x1F));
			Storage[i] = u;
		}

	}

}
