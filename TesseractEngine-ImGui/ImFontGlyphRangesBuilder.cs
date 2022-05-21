namespace Tesseract.ImGui {
	public class ImFontGlyphRangesBuilder {

		public readonly List<uint> UsedChars = new();

		public void Clear() {
			UsedChars.EnsureCapacity(0x4000);
			for (int i = 0; i < UsedChars.Count; i++) UsedChars[i] = 0;
		}

		public bool GetBit(int n) {
			uint i = UsedChars[n >> 5];
			return (i & (1 << (n & 0x1F))) != 0;
		}

		public void SetBit(int n) {
			int i = n >> 5;
			uint u = UsedChars[i];
			u |= 1u << (n & 0x1F);
			UsedChars[i] = u;
		}

		public void AddChar(char c) => SetBit((int)c);
		
		public void AddText(string text) {

		}

		public void AddRanges(IEnumerable<(char,char)> ranges) {

		}

		public void BuildRanges(ICollection<(char,char)> outRanges) {

		}

	}

}
