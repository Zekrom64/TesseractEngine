using System.Numerics;

namespace Tesseract.ImGui {
	public class ImFontAtlasCustomRect {

		public ushort Width = 0, Height = 0;
		public ushort X = 0xFFFF, Y = 0xFFFF;
		public uint GlyphID = 0;
		public float GlyphAdvanceX = 0;
		public Vector2 GlyphOffset = Vector2.Zero;
		public ImFont? Font = null;

		public ImFontAtlasCustomRect() { }

		public bool IsPacked => X != 0xFFFF;

	}

}
