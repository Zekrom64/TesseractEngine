using System.Numerics;

namespace Tesseract.ImGui {
	public class ImFont {

		public readonly List<float> IndexAdvanceX = new();
		public float FallbackAdvanceX { get; }
		public float FontSize;

		public readonly List<char> IndexLookup = new();
		public readonly List<ImFontGlyph> Glyphs = new();
		public ImFontGlyph FallbackGlyph { get; }

		public ImFontAtlas ContainerAtlas { get; }
		public readonly List<ImFontConfig> ConfigData = new();
		public char FallbackChar { get; } = (char)0xFFFD;
		public char EllipsisChar { get; } = '…';
		public char DotChar { get; } = '.';
		public bool DirtyLookupTables { get; }
		public float Scale = 1;
		public float Ascent { get; }
		public float Descent { get; }
		public int MetricsTotalSurface { get; }
		internal byte[] Used4kPagesMap = new byte[0x10000 / 4096 / 8];

		public ImFont() {

		}

		public ImFontGlyph FintGlyph(char c) {

		}

		public ImFontGlyph FindGlyphNoFallback(char c) {

		}

		public float GetCharAdvance(char c) => c < IndexAdvanceX.Count ? IndexAdvanceX[c] : FallbackAdvanceX;

		public bool IsLoaded => ContainerAtlas != null;

		public string DebugName => ConfigData.Count > 0 ? ConfigData[0].Name : "<unknown>";

		public Vector2 CalcTextSizeA(float size, float maxWidth, float wrapWidth, string text, int textBegin = 0) {

		}

		public int CalcWordWrapPositionA(float scale, string text, float wrapWidth, int textBegin = 0) {

		}

		public void RenderChar(ImDrawList drawList, float size, Vector2 pos, uint col, char c) {

		}

		public void RenderText(ImDrawList drawList, float size, Vector2 pos, uint col, Vector4 clipRect, string text, int textBegin = 0, int textEnd = -1, float wrapWidth = 0, bool cpuFineClip = false) {

		}


		internal void BuildLookupTable() {

		}

		internal void ClearOutputData() {

		}

		internal void GrowIndex(int newSize) {

		}

		internal void AddGlyph(ImFontConfig srcConfig, char c, Vector2 xy0, Vector2 xy1, Vector2 uv0, Vector2 uv1, float advanceX) {

		}

		internal void AddRemapChar(char dst, char src, bool overwriteDst = true) {

		}

		internal void SetGlyphVisible(char c, bool visible) {

		}

		internal bool IsGlyphRangeUnused(char cBegin, char cEnd) {

		}

	}

}
