namespace Tesseract.ImGui {
	public record ImGuiTableColumnSortSpects {

		public int ColumnUserID { get; init; }
		public short ColumnIndex { get; init; }
		public short ColumnOrder { get; init; }
		public ImGuiSortDirection SortDirection { get; init; }

	}

}
