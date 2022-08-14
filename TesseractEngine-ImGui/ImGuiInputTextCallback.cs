using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui {

	public delegate int ImGuiInputTextCallback(IImGuiInputTextCallbackData data);

	// Shared state of InputText(), passed as an argument to your callback when a ImGuiInputTextFlags_Callback* flag is used.
	// The callback function should return 0 by default.
	// Callbacks (follow a flag name and see comments in ImGuiInputTextFlags_ declarations for more details)
	// - ImGuiInputTextFlags_CallbackEdit:        Callback on buffer edit (note that InputText() already returns true on edit, the callback is useful mainly to manipulate the underlying buffer while focus is active)
	// - ImGuiInputTextFlags_CallbackAlways:      Callback on each iteration
	// - ImGuiInputTextFlags_CallbackCompletion:  Callback on pressing TAB
	// - ImGuiInputTextFlags_CallbackHistory:     Callback on pressing Up/Down arrows
	// - ImGuiInputTextFlags_CallbackCharFilter:  Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
	// - ImGuiInputTextFlags_CallbackResize:      Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow.
	public interface IImGuiInputTextCallbackData {

		public ImGuiInputTextFlags EventFlag { get; }

		public ImGuiInputTextFlags Flags { get; }

		public char EventChar { get; }

		public ImGuiKey EventKey { get; }

		public Span<byte> Buf { get; }

		public int BufSize { get; }

		public int BufTextLen { get; }

		public bool BufDirty { get; set; }

		public int CursorPos { get; set; }

		public int SelectionStart { get; set; }

		public int SelectionEnd { get; set; }

		public void DeleteChars(int pos, int bytesCount);

		public void InsertChars(int pos, string text);

		public void SelectAll();

		public void ClearSelection();

		public bool HasSelection => SelectionStart != SelectionEnd;

	}

}
