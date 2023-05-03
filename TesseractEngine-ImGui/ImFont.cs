using System.Numerics;

namespace Tesseract.ImGui {

	public interface IImFont {

		public IReadOnlyList<float> IndexAdvanceX { get; }
		public float FallbackAdvanceX { get; }
		public float FontSize { get; set; }

		public IReadOnlyList<char> IndexLookup { get; }
		public IReadOnlyList<ImFontGlyph> Glyphs { get; }
		public ImFontGlyph FallbackGlyph { get; }

		public IImFontAtlas ContainerAtlas { get; }
		public char FallbackChar { get; }
		public char EllipsisChar { get; }
		public bool DirtyLookupTables { get; }
		public float Scale { get; set; }
		public float Ascent { get; }
		public float Descent { get; }
		public int MetricsTotalSurface { get; }

		public ImFontGlyph FintGlyph(char c);

		public ImFontGlyph? FindGlyphNoFallback(char c);

		public float GetCharAdvance(char c);

		public bool IsLoaded { get; }

		public string DebugName { get; }

		public Vector2 CalcTextSizeA(float size, float maxWidth, float wrapWidth, string text, int textBegin = 0);

		public int CalcWordWrapPositionA(float scale, string text, float wrapWidth, int textBegin = 0);

		public void RenderChar(IImDrawList drawList, float size, Vector2 pos, uint col, char c);

		public void RenderText(IImDrawList drawList, float size, Vector2 pos, uint col, Vector4 clipRect, string text, int textBegin = 0, int textEnd = -1, float wrapWidth = 0, bool cpuFineClip = false);

	}

}
