using System.Numerics;

namespace Tesseract.ImGui {
	public class ImFontAtlas {

		public ImFontAtlasFlags Flags { get; set; }
		public nint TexID { get; set; }
		public int TexDesiredWidth { get; set; }
		public int TexGlyphPadding { get; set; }
		public bool Locked { get; internal set; }

		internal bool TexReady;
		internal bool TexPixelsUseColors;
		internal byte[]? TexPixelsAlpha8;
		internal uint[]? TexPixelsRGBA32;
		internal int TexWidth;
		internal int TexHeight;
		internal Vector2 TexUVScale;
		internal Vector2 TexUVWhitePixel;
		internal readonly List<ImFont> Fonts = new();
		internal readonly List<ImFontAtlasCustomRect> CustomRects = new();
		internal readonly List<ImFontConfig> ConfigData = new();
		internal readonly Vector4[] TexUVLines = new Vector4[ImDrawList.TexLinesMaxWidth + 1];
		
		internal ImFontBuilderIO FontBuilderIO;
		internal uint FontBuilderFlags;

		internal int PackIdMouseCursors;
		internal int PackIdLines;


		public ImFontAtlas() {

		}

		public ImFont AddFont(ImFontConfig config) {

		}

		public ImFont AddFontDefault(ImFontConfig? config = null) {

		}

		public ImFont AddFontFromFileTTF(string filename, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char,char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryTTF(byte[] fontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryCompressedTTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}

		public ImFont AddFontFromMemoryCompressedBase85TTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null) {

		}


		public void ClearInputData() {

		}

		public void ClearTexData() {

		}

		public void ClearFonts() {

		}

		public void Clear() {

		}


		public bool Build() {

		}

		public void GetTexDataAsAlpha8(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel) {

		}

		public void GetTexDataAsRGBA32(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel) {

		}

		public bool IsBuilt => Fonts.Count > 0 && TexReady;


		public static IEnumerable<(char, char)> GlyphRangesDefault;
		public static IEnumerable<(char, char)> GlyphRangesKorean;
		public static IEnumerable<(char, char)> GlyphRangesJapanese;
		public static IEnumerable<(char, char)> GlyphRangesChineseFull;
		public static IEnumerable<(char, char)> GlyphRangesChineseSimplifiedCommon;
		public static IEnumerable<(char, char)> GlyphRangesCyrillic;
		public static IEnumerable<(char, char)> GlyphRangesThai;
		public static IEnumerable<(char, char)> GlyphRangesVietnamese;


		public int AddCustomRectRegular(int width, int height) {

		} 

		public int AddCustomRectFontGlyph(ImFont font, char id, int width, int height, int advanceX, Vector2 offset = default) {

		}

		public ImFontAtlasCustomRect GetCustomRectByIndex(int index) {

		}


		public void CalcCustomRectUV(ImFontAtlasCustomRect rect, out Vector2 outUVMin, out Vector2 outUVMax) {

		}

		public bool GetMouseCursorTexData(ImGuiMouseCursor cursor, out Vector2 outOffset, out Vector2 outSize, Span<Vector2> outUVBorder, Span<Vector2> outUVFill) {

		}

	}

}
