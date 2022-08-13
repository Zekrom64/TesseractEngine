using System.Runtime.InteropServices;

namespace Tesseract.ImGui {

	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiTableColumnSortSpecs {

		public int ColumnUserID;

		public short ColumnIndex;

		public short ColumnOrder;

		private byte sortDirection;
		public ImGuiSortDirection SortDirection { get => (ImGuiSortDirection)sortDirection; set => sortDirection = (byte)value; }

	}

	public interface IImGuiTableSortSpecs {

		public ReadOnlySpan<ImGuiTableColumnSortSpecs> Specs { get; }

		public bool SpecsDirty { get; }

	}

}
