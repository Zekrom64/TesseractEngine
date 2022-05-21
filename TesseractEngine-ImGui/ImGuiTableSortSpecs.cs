namespace Tesseract.ImGui {
	public class ImGuiTableSortSpecs {

		private IReadOnlyList<ImGuiTableColumnSortSpects> specs = Array.Empty<ImGuiTableColumnSortSpects>();
		public IReadOnlyList<ImGuiTableColumnSortSpects> Specs {
			get => specs;
			set {
				specs = value;
				SpecsDirty = true;
			}
		}
		public bool SpecsDirty;

	}

}
