using System.Numerics;

namespace Tesseract.ImGui {

	// Load and rasterize multiple TTF/OTF fonts into a same texture. The font atlas will build a single texture holding:
	//  - One or more fonts.
	//  - Custom graphics data needed to render the shapes needed by Dear ImGui.
	//  - Mouse cursor shapes for software cursor rendering (unless setting 'Flags |= ImFontAtlasFlags_NoMouseCursors' in the font atlas).
	// It is the user-code responsibility to setup/build the atlas, then upload the pixel data into a texture accessible by your graphics api.
	//  - Optionally, call any of the AddFont*** functions. If you don't call any, the default font embedded in the code will be loaded for you.
	//  - Call GetTexDataAsAlpha8() or GetTexDataAsRGBA32() to build and retrieve pixels data.
	//  - Upload the pixels data into a texture within your graphics system (see imgui_impl_xxxx.cpp examples)
	//  - Call SetTexID(my_tex_id); and pass the pointer/identifier to your texture in a format natural to your graphics API.
	//    This value will be passed back to you during rendering to identify the texture. Read FAQ entry about ImTextureID for more details.
	// Common pitfalls:
	// - If you pass a 'glyph_ranges' array to AddFont*** functions, you need to make sure that your array persist up until the
	//   atlas is build (when calling GetTexData*** or Build()). We only copy the pointer, not the data.
	// - Important: By default, AddFontFromMemoryTTF() takes ownership of the data. Even though we are not writing to it, we will free the pointer on destruction.
	//   You can set font_cfg->FontDataOwnedByAtlas=false to keep ownership of your data and it won't be freed,
	// - Even though many functions are suffixed with "TTF", OTF data is supported just as well.
	// - This is an old API and it is currently awkward for those and and various other reasons! We will address them in the future!
	public interface IImFontAtlas {

		public IImFont AddFont(ImFontConfig config);

		public IImFont AddFontDefault(ImFontConfig? config = null);

		public IImFont AddFontFromFileTTF(string filename, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null);

		public IImFont AddFontFromMemoryTTF(byte[] fontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null);

		public IImFont AddFontFromMemoryCompressedTTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null);

		public IImFont AddFontFromMemoryCompressedBase85TTF(byte[] compressedFontData, float sizePixels, ImFontConfig? config = null, IReadOnlyCollection<(char, char)>? glyphRanges = null);


		public void ClearInputData();

		public void ClearTexData();

		public void ClearFonts();

		public void Clear();


		public bool Build();

		public void GetTexDataAsAlpha8(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel);

		public void GetTexDataAsRGBA32(Span<byte> outPixels, out int outWidth, out int outHeight, out int outBytesPerPixel);

		public bool IsBuilt { get; }

		public void SetTexID(IntPtr id);


		public ReadOnlySpan<char> GlyphRangesDefault { get; }

		public ReadOnlySpan<char> GlyphRangesKorean { get; }

		public ReadOnlySpan<char> GlyphRangesJapanese { get; }

		public ReadOnlySpan<char> GlyphRangesChineseFull { get; }

		public ReadOnlySpan<char> GlyphRangesChineseSimplifiedCommon { get; }

		public ReadOnlySpan<char> GlyphRangesCyrillic { get; }

		public ReadOnlySpan<char> GlyphRangesThai { get; }

		public ReadOnlySpan<char> GlyphRangesVietnamese { get; }


		public int AddCustomRectRegular(int width, int height);

		public int AddCustomRectFontGlyph(IImFont font, char id, int width, int height, int advanceX, Vector2 offset = default);

		public IImFontAtlasCustomRect GetCustomRectByIndex(int index);


		public void CalcCustomRectUV(IImFontAtlasCustomRect rect, out Vector2 outUVMin, out Vector2 outUVMax);

		public bool GetMouseCursorTexData(ImGuiMouseCursor cursor, out Vector2 outOffset, out Vector2 outSize, Span<Vector2> outUVBorder, Span<Vector2> outUVFill);


		public ImFontAtlasFlags Flags { get; set; }

		public IntPtr TexID { get; set; }

		public int TexDesiredWidth { get; set; }

		public int TexGlyphPadding { get; set; }

		public bool Locked { get; set; }

	}

}
