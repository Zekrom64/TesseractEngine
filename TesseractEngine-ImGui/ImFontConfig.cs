using System.Numerics;

namespace Tesseract.ImGui {

	public record class ImFontConfig {

		public byte[] FontData { get; init; } = null!;
		public int FontNo { get; init; } = 0;
		public float SizePixels { get; init; }
		public int OversampleH { get; init; } = 3;
		public int OversampleV { get; init; } = 1;
		public bool PixelSnapH { get; init; } = false;
		public Vector2 GlyphExtraSpacing { get; init; } = Vector2.Zero;
		public Vector2 GlyphOffset { get; init; } = Vector2.Zero;
		public IReadOnlyCollection<(char, char)>? GlyphRanges { get; init; } = null;
		public float GlyphMinAdvanceX { get; init; } = 0;
		public float GlyphMaxAdvanceX { get; init; } = float.MaxValue;
		public bool MergeMode { get; init; } = false;
		public uint FontBuilderFlags { get; init; } = 0;
		public float RasterizerMultiply { get; init; } = 1;
		public char EllipsisChar { get; init; } = unchecked((char)-1);

	}

}
