namespace Tesseract.ImGui {
	public class ImGuiTextFilter {

		public readonly char[] InputBuf = new char[256];
		public readonly List<string> Filters = new();
		public int CountGrep;

		public ImGuiTextFilter(string defaultFilter = "") {

		}

		public bool Draw(string label = "Filter (inc,-exc)", float width = 0.0f) {

		}

		public bool PassFilter(string text) {

		}

		public void Build() {

		}

		public void Clear() {
			InputBuf[0] = '\0';
			Build();
		}

		public bool IsActive => Filters.Count != 0;

	}

}
