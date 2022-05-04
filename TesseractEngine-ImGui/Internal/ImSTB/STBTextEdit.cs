using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.ImSTB {
	
	internal static class STBTextEdit {

		public const int UndoStateCount = 99;
		public const int UndoCharCount = 999;

	}

	internal class STBTextEdit<TStr, TChar> where TChar : IEquatable<TChar> {

		public delegate int StringLenFn(TStr str);
		public delegate TChar GetCharFn(TStr str, int idx);
		public delegate float GetWidthFn(TStr str, int lineStartIdx, int charIdx);
		public delegate int KeyToTextFn(int key);
		public delegate void LayoutRowFn(ref STBTextEditRow r, TStr str, int lineStartIdx);
		public delegate int MoveWordLeftFn(TStr str, int idx);
		public delegate int MoveWordRightFn(TStr str, int idx);
		public delegate void DeleteCharsFn(TStr str, int pos, int n);
		public delegate bool InsertCharsFn(TStr str, int pos, IReadOnlyCollection<TChar> newText);

		public struct Parameters {

			public StringLenFn StringLen;
			public GetCharFn GetChar;
			public GetWidthFn GetWidth;
			public KeyToTextFn KeyToText;
			public TChar Newline;
			public LayoutRowFn LayoutRow;
			public MoveWordLeftFn MoveWordLeft;
			public MoveWordRightFn MoveWordRight;
			public DeleteCharsFn DeleteChars;
			public InsertCharsFn InsertChars;

		}
		
		public Parameters Params { get; }

		public STBTextEdit(Parameters p) {
			Params = p;
		}

		public int LocateCoord(TStr str, float x, float y) {
			STBTextEditRow r = new();
			int n = Params.StringLen(str);
			float base_y = 0, prev_x;
			int i = 0, k;

			while(i < n) {
				Params.LayoutRow(ref r, str, i);
				if (r.NumChars <= 0) return n;
				if (i == 0 && y < base_y + r.YMin)
					return 0;
				if (y < base_y + r.YMax) break;
				i += r.NumChars;
				base_y += r.BaselineYDelta;
			}

			if (i >= n) return n;

			if (x < r.X0) return i;

			if (x < r.X1) {
				prev_x = r.X0;
				for (k = 0; k < r.NumChars; k++) {
					float w = Params.GetWidth(str, i, k);
					if (x < prev_x + w) {
						if (x < prev_x + w / 2)
							return k + i;
						else
							return k + i + 1;
					}
					prev_x += w;
				}
			}

			if (Params.GetChar(str, i + r.NumChars - 1).Equals(Params.Newline))
				return i + r.NumChars - 1;
			else
				return i + r.NumChars;
		}

		public void Click(TStr str, STBTextEditState state, float x, float y) {
			if (state.SingleLine) {
				STBTextEditRow r = new();
				Params.LayoutRow(ref r, str, 0);
				y = r.YMin;
			}

			state.Cursor = LocateCoord(str, x, y);
			state.SelectStart = state.Cursor;
			state.SelectEnd = state.Cursor;
			state.HasPreferredX = false;
		}

		public void Drag(TStr str, STBTextEditState state, float x, float y) {
			int p = 0;

			if (state.SingleLine) {
				STBTextEditRow r = new();
				Params.LayoutRow(ref r, str, 0);
				y = r.YMin;
			}

			if (state.SelectStart == state.SelectEnd)
				state.SelectStart = state.Cursor;

			p = LocateCoord((TStr)str, x, y);
			state.Cursor = state.SelectEnd = p;
		}

		public void FindCharpos(ref STBFindState find, TStr str, int n, bool singleLine) {
			STBTextEditRow r = new();
			int prevStart = 0;
			int z = Params.StringLen(str);
			int i = 0, first;

			if (n == z) {
				if (singleLine) {
					Params.LayoutRow(ref r, str, 0);
					find.Y = 0;
					find.FirstChar = 0;
					find.Length = z;
					find.Height = r.YMax - r.YMin;
					find.X = r.X1;
				} else {
					find.Y = 0;
					find.X = 0;
					find.Height = 1;
					while(i < z) {
						Params.LayoutRow(ref r, str, i);
						prevStart = i;
						i += r.NumChars;
					}
					find.FirstChar = i;
					find.Length = 0;
					find.PrevFirst = prevStart;
				}
				return;
			}

			find.Y = 0;
			while(true) {
				Params.LayoutRow(ref r, str, i);
				if (n < i + r.NumChars)
					break;
				prevStart = i;
				i += r.NumChars;
				find.Y += r.BaselineYDelta;
			}

			find.FirstChar = first = i;
			find.Length = r.NumChars;
			find.Height = r.YMax - r.YMin;
			find.PrevFirst = prevStart;

			find.X = r.X0;
			for (i = 0; first + i < n; i++)
				find.X += Params.GetWidth(str, first, i);
		}

		public void Clamp(TStr str, ref STBTextEditState state) {
			int n = Params.StringLen(str);
			if (state.HasSelection) {
				if (state.SelectStart > n) state.SelectStart = n;
				if (state.SelectEnd > n) state.SelectEnd = n;
				if (state.SelectStart == state.SelectEnd)
					state.Cursor = state.SelectStart;
			}

		}

	}

	internal struct STBUndoRecord {

		public int Where;
		public int InsertLength;
		public int DeleteLength;
		public int CharStorage;

	}

	internal class STBUndoState {

		public STBUndoRecord[] UndoRec = new STBUndoRecord[STBTextEdit.UndoStateCount];
		public int[] UndoChar = new int[STBTextEdit.UndoCharCount];
		public short UndoPoint, RedoPoint;
		public int UndoCharPoint, RedoCharPoint;

	}

	internal class STBTextEditState {

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
		public readonly STBUndoState UndoState = new();

		public bool HasSelection => SelectStart != SelectEnd;

	}

	internal struct STBTextEditRow {

		public float X0, X1;
		public float BaselineYDelta;
		public float YMin, YMax;
		public int NumChars;

	}

	internal struct STBFindState {

		public float X, Y;
		public float Height;
		public int FirstChar, Length;
		public int PrevFirst;

	}

}
