using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.ImGui {

	public delegate int ImGuiInputTextCallback(ref ImGuiInputTextCallbackData data);

	// Shared state of InputText(), passed as an argument to your callback when a ImGuiInputTextFlags_Callback* flag is used.
	// The callback function should return 0 by default.
	// Callbacks (follow a flag name and see comments in ImGuiInputTextFlags_ declarations for more details)
	// - ImGuiInputTextFlags_CallbackEdit:        Callback on buffer edit (note that InputText() already returns true on edit, the callback is useful mainly to manipulate the underlying buffer while focus is active)
	// - ImGuiInputTextFlags_CallbackAlways:      Callback on each iteration
	// - ImGuiInputTextFlags_CallbackCompletion:  Callback on pressing TAB
	// - ImGuiInputTextFlags_CallbackHistory:     Callback on pressing Up/Down arrows
	// - ImGuiInputTextFlags_CallbackCharFilter:  Callback on character inputs to replace or discard them. Modify 'EventChar' to replace or discard, or return 1 in callback to discard.
	// - ImGuiInputTextFlags_CallbackResize:      Callback on buffer capacity changes request (beyond 'buf_size' parameter value), allowing the string to grow.
	[StructLayout(LayoutKind.Sequential)]
	public struct ImGuiInputTextCallbackData {

		public ImGuiInputTextFlags EventFlag;

		public ImGuiInputTextFlags Flags;

		public IntPtr UserData;

		public char EventChar;

		public ImGuiKey EventKey;

		[NativeType("char*")]
		public IntPtr Buf;

		public int BufTextLen;

		public int BufSize;

		[MarshalAs(UnmanagedType.U1)]
		public bool BufDirty;

		public int CursorPos;

		public int SelectionStart;

		public int SelectionEnd;

		public void DeleteChars(int pos, int bytesCount);

		public void InsertChars(int pos, string text);

		public void SelectAll() {
			SelectionStart = 0;
			SelectionEnd = BufTextLen;
		}

		public void ClearSelection() {
			SelectionStart = SelectionEnd = BufTextLen;
		}

		public bool HasSelection => SelectionStart != SelectionEnd;

	}

}
