using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.Internal.ImSTB {
	
	internal static partial class STBTextEdit {

		// ImGui is cheeky and scatters these definitions around different files

		// imgui_internal.h
		public const float GetWidthNewline = -1;
		public const int UndoStateCount = 99;
		public const int UndoCharCount = 999;

		// imgui_widgets.cpp
		public static int StringLen(ImGuiInputTextState obj) => obj.CurLen;

		public static char GetChar(ImGuiInputTextState obj, int idx) => obj.Text[idx];

		public static float GetWidth(ImGuiInputTextState obj, int lineStartIdx, int charIdx) {
			char c = obj.Text[lineStartIdx + charIdx];
			if (c == '\n') return GetWidthNewline;
			ImGuiContext g = 
		}

	}

	internal struct STBUndoRecord {
		public int Where;
		public int InsertLength;
		public int DeleteLength;
		public int CharStorage;
	}

	internal class STBUndoState {
		public STBUndoRecord[] UndoRec { get; } = new STBUndoRecord[STBTextEdit.UndoStateCount];
		public int[] UndoChar { get; } = new int[STBTextEdit.UndoCharCount];
		public short UndoPoint, RedoPoint;
		public int UndoCharPoint, RedoCharPoint;
	}

	internal class STBTexteditState {
		public int Cursor;
		public int SelectStart;
		public int SelectEnd;
		public byte InsertMode;
		public int RowCountPerPage;
		public bool CursorAtEndOfLine;
		public bool Initialized;
		public bool HasPreferredX;
		public bool SingleLine;
		public float PreferredX;
		public STBUndoState UndoState = new();
	}

	internal struct STBTexteditRow {
		public float X0, X1;
		public float BaselineYDelta;
		public float YMin, YMax;
		public int NumChars;
	}

	internal static partial class STBTextEdit {

		public static int LocateCoord(ImGuiInputTextState str, float x, float y) {

		}

	}

}
