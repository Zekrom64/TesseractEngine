using System.Text;

namespace Tesseract.ImGui.Internal {
	internal class ImGuiInputTextState {

		public int ID;
		public StringBuilder Text = new();
		public int CurLen => Text.Length;
		public float ScrollX;
		public ImSTB.STBTextEditState Stb = new();
		public float CursorAnim;
		public bool CursorFollow;
		public bool SelectedAllMouseLock;
		public bool Edited;
		public ImGuiTextFlags Flags;

		public void ClearText() => Text.Clear();

		public void ClearFreeMemory() { }

		public int UndoAvailCount => Stb.UndoState.UndoPoint;

		public int RedoAvailCount => ImSTB.STBTextEdit.UndoStateCount - Stb.UndoState.RedoPoint;

		public void OnKeyPressed(int key) {
			// TODO
			// stb_textedit_key(this, &Stb, key)
			CursorFollow = true;
			CursorAnimReset();
		}

		public void CursorAnimReset() {
			CursorAnim = -0.30f;
		}

		public void CursorClamp() {
			Stb.Cursor = Math.Min(Stb.Cursor, CurLen);
			Stb.SelectStart = Math.Min(Stb.SelectStart, CurLen);
			Stb.SelectEnd = Math.Min(Stb.SelectEnd, CurLen);
		}

		public bool HasSelection => Stb.SelectStart != Stb.SelectEnd;

		public int CursorPos => Stb.Cursor;

		public int SelectionStart => Stb.SelectStart;

		public int SelectionEnd => Stb.SelectEnd;

		public void SelectAll() {
			Stb.SelectStart = 0;
			Stb.Cursor = Stb.SelectEnd = CurLen;
			Stb.HasPreferredX = false;
		}

	}

}
