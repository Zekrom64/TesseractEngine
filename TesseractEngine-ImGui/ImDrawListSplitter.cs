namespace Tesseract.ImGui {
	public class ImDrawListSplitter {

		private int current;
		private int count;
		private readonly List<ImDrawChannel> channels = new();

		public void Clear() {
			current = 0;
			count = 1;
		}

		public void ClearFreeMemory() {

		}

		public void Split(ImDrawList drawList, int count) {

		}

		public void Merge(ImDrawList drawList) {

		}

		public void SetCurrentChannel(ImDrawList drawList, int channel) {

		}

	}

}
