#pragma once

#include "imgui_cli.h"
#include "imgui.h"
#include "imgui_cli_draw.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui {

	ref class ImFontAtlasCLI;
	
	public ref class ImFontCLI : public Tesseract::ImGui::IImFont {
	internal:
		ref class IndexAdvanceXImpl : IReadOnlyList<float> {
		internal:
			ImVector<float>* m_vec;

			IndexAdvanceXImpl(ImVector<float>* vec) : m_vec(vec) {}

		public:
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			virtual System::Collections::Generic::IEnumerator<float>^ GetEnumerator() {
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<float>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property float default[int] {
				virtual float get(int index) { return m_vec->operator[](index); }
			}
		};

		ref class IndexLookupImpl : IReadOnlyList<wchar_t> {
		internal:
			ImVector<ImWchar>* m_vec;

			IndexLookupImpl(ImVector<ImWchar>* vec) : m_vec(vec) {}

		public:
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			virtual System::Collections::Generic::IEnumerator<wchar_t>^ GetEnumerator() {
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<wchar_t>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property wchar_t default[int] {
				virtual wchar_t get(int index) { return m_vec->operator[](index); }
			}
		};

		ref class GlyphsImpl : IReadOnlyList<Tesseract::ImGui::ImFontGlyph> {
		internal:
			ImVector<ImFontGlyph>* m_vec;

			GlyphsImpl(ImVector<ImFontGlyph>* vec) : m_vec(vec) {}

		public:
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			virtual System::Collections::Generic::IEnumerator<Tesseract::ImGui::ImFontGlyph>^ GetEnumerator() {
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<Tesseract::ImGui::ImFontGlyph>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property Tesseract::ImGui::ImFontGlyph default[int] {
				virtual Tesseract::ImGui::ImFontGlyph get(int index) { return ConvertGlyph(m_vec->operator[](index)); }
			}
		};

		ImFont* m_font;
		ImFontAtlasCLI^ m_atlas;
		IndexAdvanceXImpl^ m_indexadvancex;
		IndexLookupImpl^ m_indexlookup;
		GlyphsImpl^ m_glyphs;

		ImFontCLI(ImFont* font);

		static Tesseract::ImGui::ImFontGlyph ConvertGlyph(ImFontGlyph glyph) {
			Tesseract::ImGui::ImFontGlyph mglyph = {};
			mglyph.Colored = glyph.Colored;
			mglyph.Visible = glyph.Visible;
			mglyph.Codepoint = glyph.Codepoint;
			mglyph.AdvanceX = glyph.AdvanceX;
			mglyph.XY0 = Vector2(glyph.X0, glyph.Y0);
			mglyph.XY1 = Vector2(glyph.X1, glyph.Y1);
			mglyph.UV0 = Vector2(glyph.U0, glyph.V0);
			mglyph.UV1 = Vector2(glyph.U1, glyph.V1);
			return mglyph;
		}

	public:
		virtual property System::Collections::Generic::IReadOnlyList<float>^ IndexAdvanceX {
			virtual IReadOnlyList<float>^ get() { return m_indexadvancex; }
		}

		virtual property float FallbackAdvanceX {
			virtual float get() { return m_font->FallbackAdvanceX; }
		}

		virtual property float FontSize {
			virtual float get() { return m_font->FontSize; }
			virtual void set(float value) { m_font->FontSize = value; }
		}

		virtual property System::Collections::Generic::IReadOnlyList<wchar_t>^ IndexLookup {
			virtual IReadOnlyList<wchar_t>^ get() { return m_indexlookup; }
		}

		virtual property System::Collections::Generic::IReadOnlyList<Tesseract::ImGui::ImFontGlyph>^ Glyphs {
			virtual IReadOnlyList<Tesseract::ImGui::ImFontGlyph>^ get() { return m_glyphs; }
		}

		virtual property Tesseract::ImGui::ImFontGlyph FallbackGlyph {
			virtual Tesseract::ImGui::ImFontGlyph get() { return ConvertGlyph(*m_font->FallbackGlyph); }
		}

		virtual property Tesseract::ImGui::IImFontAtlas^ ContainerAtlas {
			virtual Tesseract::ImGui::IImFontAtlas^ get() { return (Tesseract::ImGui::IImFontAtlas^)m_atlas; }
		}

		virtual property wchar_t FallbackChar {
			virtual wchar_t get() { return m_font->FallbackChar; }
		}

		virtual property wchar_t EllipsisChar {
			virtual wchar_t get() { return m_font->EllipsisChar; }
		}

		virtual property bool DirtyLookupTables {
			virtual bool get() { return m_font->DirtyLookupTables; }
		}

		virtual property float Scale {
			virtual float get() { return m_font->Scale; }
			virtual void set(float value) { m_font->Scale = value; }
		}

		virtual property float Ascent {
			virtual float get() { return m_font->Ascent; }
		}

		virtual property float Descent {
			virtual float get() { return m_font->Descent; }
		}

		virtual property int MetricsTotalSurface {
			virtual int get() { return m_font->MetricsTotalSurface; }
		}

		virtual property bool IsLoaded {
			virtual bool get() { return m_font->IsLoaded(); }
		}

		virtual property System::String^ DebugName {
			virtual String^ get() { return gcnew String(m_font->GetDebugName()); }
		}

		virtual Tesseract::ImGui::ImFontGlyph FintGlyph(wchar_t c) {
			return ConvertGlyph(*m_font->FindGlyph(c));
		}

		virtual System::Nullable<Tesseract::ImGui::ImFontGlyph> FindGlyphNoFallback(wchar_t c) {
			auto glyph = m_font->FindGlyphNoFallback(c);
			if (glyph == nullptr) return {};
			else return ConvertGlyph(*glyph);
		}

		virtual float GetCharAdvance(wchar_t c) {
			return m_font->GetCharAdvance(c);
		}

		virtual System::Numerics::Vector2 CalcTextSizeA(float size, float maxWidth, float wrapWidth, System::String^ text, int textBegin) {
			StringParam pText(text->Substring(textBegin));
			auto retn = m_font->CalcTextSizeA(size, maxWidth, wrapWidth, pText.begin(), pText.end());
			return System::Numerics::Vector2(retn.x, retn.y);
		}

		virtual int CalcWordWrapPositionA(float scale, System::String^ text, float wrapWidth, int textBegin) {
			StringParam pText(text->Substring(textBegin));
			return pText.to_index(m_font->CalcWordWrapPositionA(scale, pText.begin(), pText.end(), wrapWidth));
		}

		virtual void RenderChar(Tesseract::ImGui::IImDrawList^ drawList, float size, System::Numerics::Vector2 pos, unsigned int col, wchar_t c) {
			ImDrawList* pDrawList = ((ImDrawListCLI^)drawList)->m_drawlist;
			m_font->RenderChar(pDrawList, size, { pos.X, pos.Y }, col, c);
		}

		virtual void RenderText(Tesseract::ImGui::IImDrawList^ drawList, float size, System::Numerics::Vector2 pos, unsigned int col, System::Numerics::Vector4 clipRect, System::String^ text, int textBegin, int textEnd, float wrapWidth, bool cpuFineClip) {
			ImDrawList* pDrawList = ((ImDrawListCLI^)drawList)->m_drawlist;
			if (textEnd < 0) textEnd = text->Length;
			StringParam pText(text->Substring(textBegin, textBegin - textEnd));
			m_font->RenderText(pDrawList, size, { pos.X, pos.Y }, col, { clipRect.X, clipRect.Y, clipRect.Z, clipRect.W }, pText.begin(), pText.end(), wrapWidth, cpuFineClip);
		}
	};

	public ref class ImFontAtlasCustomRectCLI : Tesseract::ImGui::IImFontAtlasCustomRect {
	internal:
		ImFontAtlasCustomRect* m_rect;

		ImFontAtlasCustomRectCLI(ImFontAtlasCustomRect* rect) : m_rect(rect) {}

	public:
		virtual property unsigned short Width {
			virtual unsigned short get() { return m_rect->Width; }
			virtual void set(unsigned short value) { m_rect->Width = value; }
		}

		virtual property unsigned short Height {
			virtual unsigned short get() { return m_rect->Height; }
			virtual void set(unsigned short value) { m_rect->Height = value; }
		}

		virtual property unsigned short X {
			virtual unsigned short get() { return m_rect->X; }
		}

		virtual property unsigned short Y {
			virtual unsigned short get() { return m_rect->Y; }
		}

		virtual property unsigned int GlyphID {
			virtual unsigned int get() { return m_rect->GlyphID; }
			virtual void set(unsigned int value) { m_rect->GlyphID = value; }
		}

		virtual property float GlyphAdvanceX {
			virtual float get() { return m_rect->GlyphAdvanceX; }
			virtual void set(float value) { m_rect->GlyphAdvanceX = value; }
		}

		virtual property System::Numerics::Vector2 GlyphOffset {
			virtual Vector2 get() { return Vector2(m_rect->GlyphOffset.x, m_rect->GlyphOffset.y); }
			virtual void set(Vector2 value) { m_rect->GlyphOffset = { value.X, value.Y }; }
		}

		virtual property Tesseract::ImGui::IImFont^ Font {
			virtual Tesseract::ImGui::IImFont^ get() { return gcnew ImFontCLI(m_rect->Font); }
			virtual void set(Tesseract::ImGui::IImFont^ value) { m_rect->Font = value ? ((ImFontCLI^)value)->m_font : nullptr; }
		}

		virtual property bool IsPacked {
			virtual bool get() { return m_rect->IsPacked(); }
		}

	};

	public ref class ImFontAtlasCLI : Tesseract::ImGui::IImFontAtlas {
	internal:
		ImFontAtlas* m_atlas;
		bool m_allocd;
		List<IntPtr>^ m_glyphranges = gcnew List<IntPtr>();

		ImFontAtlasCLI(ImFontAtlas* atlas, bool allocd) : m_atlas(atlas), m_allocd(allocd) {}
		~ImFontAtlasCLI() {
			if (m_allocd) delete m_atlas;
			for (int i = 0; i < m_glyphranges->Count; i++) delete (wchar_t*)(void*)m_glyphranges->default[i];
		}

		static int GetRangeLength(const wchar_t* ptr) {
			int length = 0;
			while (*ptr++) length++;
			return length;
		}

		static ImFontConfig ConvertConfig(Tesseract::ImGui::ImFontConfig^ cfg) {
			ImFontConfig ncfg = {};
			if (cfg) {
				ncfg.FontNo = cfg->FontNo;
				ncfg.SizePixels = cfg->SizePixels;
				ncfg.OversampleH = cfg->OversampleH;
				ncfg.OversampleV = cfg->OversampleV;
				ncfg.PixelSnapH = cfg->PixelSnapH;
				ncfg.GlyphExtraSpacing = { cfg->GlyphExtraSpacing.X, cfg->GlyphExtraSpacing.Y };
				ncfg.GlyphOffset = { cfg->GlyphOffset.X, cfg->GlyphOffset.Y };
				ncfg.GlyphMinAdvanceX = cfg->GlyphMinAdvanceX;
				ncfg.GlyphMaxAdvanceX = cfg->GlyphMaxAdvanceX;
				ncfg.MergeMode = cfg->MergeMode;
				ncfg.FontBuilderFlags = cfg->FontBuilderFlags;
				ncfg.RasterizerMultiply = cfg->RasterizerMultiply;
				ncfg.EllipsisChar = cfg->EllipsisChar;
			}
			return ncfg;
		}

		const ImWchar* CreateGlyphRange(IReadOnlyCollection<ValueTuple<wchar_t, wchar_t>>^ collection) {
			ImWchar* pGlyphRanges = new ImWchar[(size_t)collection->Count * 2 + 1];
			auto en = collection->GetEnumerator();
			int i = 0;
			while (en->MoveNext()) {
				auto pair = en->Current;
				pGlyphRanges[i++] = pair.Item1;
				pGlyphRanges[i++] = pair.Item2;
			}
			pGlyphRanges[collection->Count * 2] = 0;
			return pGlyphRanges;
		}

	public:
		virtual property bool IsBuilt {
			virtual bool get() { return m_atlas->IsBuilt(); }
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesDefault {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesDefault());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesKorean {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesKorean());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesJapanese {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesJapanese());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesChineseFull {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesChineseFull());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesChineseSimplifiedCommon {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesChineseSimplifiedCommon());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesCyrillic {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesCyrillic());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesThai {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesThai());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property System::ReadOnlySpan<wchar_t> GlyphRangesVietnamese {
			virtual ReadOnlySpan<wchar_t> get() {
				wchar_t* ptr = (wchar_t*)const_cast<ImWchar*>(m_atlas->GetGlyphRangesVietnamese());
				return ReadOnlySpan<wchar_t>(ptr, GetRangeLength(ptr));
			}
		}

		virtual property Tesseract::ImGui::ImFontAtlasFlags Flags {
			virtual Tesseract::ImGui::ImFontAtlasFlags get() {
				return (Tesseract::ImGui::ImFontAtlasFlags)m_atlas->Flags;
			}
			virtual void set(Tesseract::ImGui::ImFontAtlasFlags value) {
				m_atlas->Flags = (ImFontAtlasFlags)value;
			}
		}

		virtual property System::UIntPtr TexID {
			virtual UIntPtr get() { return (UIntPtr)m_atlas->TexID; }
			virtual void set(UIntPtr value) { m_atlas->TexID = (void*)value; }
		}

		virtual property int TexDesiredWidth {
			virtual int get() { return m_atlas->TexDesiredWidth; }
			virtual void set(int value) { m_atlas->TexDesiredWidth = value; }
		}

		virtual property int TexGlyphPadding {
			virtual int get() { return m_atlas->TexGlyphPadding; }
			virtual void set(int value) { m_atlas->TexGlyphPadding = value; }
		}

		virtual property bool Locked {
			virtual bool get() { return m_atlas->Locked; }
		}

#define INIT_FONT_DATA(INCFG,OUTCFG) \
		pin_ptr<uint8_t> pData; \
		if (INCFG && INCFG->FontData) { \
			pData = &INCFG->FontData[0]; \
			OUTCFG.FontData = pData; \
			OUTCFG.FontDataSize = INCFG->FontData->Length; \
		}
#define INIT_FONT_GLYPHS(INCFG,OUTCFG) \
		if (INCFG && INCFG->GlyphRanges) { \
			const ImWchar* pGlyphRanges = CreateGlyphRange(INCFG->GlyphRanges); \
			m_glyphranges->Add((IntPtr)(void*)pGlyphRanges); \
			OUTCFG.GlyphRanges = pGlyphRanges; \
		}

		virtual Tesseract::ImGui::IImFont^ AddFont(Tesseract::ImGui::ImFontConfig^ config) {
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_DATA(config, ncfg);
			INIT_FONT_GLYPHS(config, ncfg);
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFont(&ncfg));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontDefault(Tesseract::ImGui::ImFontConfig^ config) {
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_DATA(config, ncfg);
			INIT_FONT_GLYPHS(config, ncfg);
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontDefault(config ? &ncfg : nullptr));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromFileTTF(System::String^ filename, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			StringParam pFilename(filename);
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromFileTTF(pFilename.c_str(), sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryTTF(array<uint8_t, 1>^ fontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<uint8_t> pFontData = &fontData[0];
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromMemoryTTF(pFontData, fontData->Length, sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryCompressedTTF(array<uint8_t, 1>^ compressedFontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<uint8_t> pFontData = &compressedFontData[0];
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromMemoryCompressedTTF(pFontData, compressedFontData->Length, sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryCompressedBase85TTF(array<uint8_t, 1>^ compressedFontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<uint8_t> pFontData = &compressedFontData[0];
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromMemoryCompressedBase85TTF((const char*)pFontData, sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual void ClearInputData() {
			m_atlas->ClearInputData();
		}

		virtual void ClearTexData() {
			m_atlas->ClearTexData();
		}

		virtual void ClearFonts() {
			m_atlas->ClearFonts();
		}

		virtual void Clear() {
			m_atlas->Clear();
		}

		virtual bool Build() {
			return m_atlas->Build();
		}

		virtual System::ReadOnlySpan<uint8_t> GetTexDataAsAlpha8(int% outWidth, int% outHeight, int% outBytesPerPixel) {
			pin_ptr<int> pOutWidth = &outWidth, pOutHeight = &outHeight, pOutBytesPerPixel = &outBytesPerPixel;
			uint8_t* pData;
			m_atlas->GetTexDataAsAlpha8(&pData, pOutWidth, pOutHeight, pOutBytesPerPixel);
			return ReadOnlySpan<uint8_t>(pData, outWidth * outHeight * outBytesPerPixel);
		}

		virtual System::ReadOnlySpan<uint8_t> GetTexDataAsRGBA32(int% outWidth, int% outHeight, int% outBytesPerPixel) {
			pin_ptr<int> pOutWidth = &outWidth, pOutHeight = &outHeight, pOutBytesPerPixel = &outBytesPerPixel;
			uint8_t* pData;
			m_atlas->GetTexDataAsRGBA32(&pData, pOutWidth, pOutHeight, pOutBytesPerPixel);
			return ReadOnlySpan<uint8_t>(pData, outWidth * outHeight * outBytesPerPixel);
		}

		virtual void SetTexID(System::UIntPtr id) {
			m_atlas->SetTexID((void*)id);
		}

		virtual int AddCustomRectRegular(int width, int height) {
			return m_atlas->AddCustomRectRegular(width, height);
		}

		virtual int AddCustomRectFontGlyph(Tesseract::ImGui::IImFont^ font, wchar_t id, int width, int height, float advanceX, System::Numerics::Vector2 offset) {
			return m_atlas->AddCustomRectFontGlyph(((ImFontCLI^)font)->m_font, id, width, height, advanceX, { offset.X, offset.Y });
		}

		virtual Tesseract::ImGui::IImFontAtlasCustomRect^ GetCustomRectByIndex(int index) {
			return gcnew ImFontAtlasCustomRectCLI(m_atlas->GetCustomRectByIndex(index));
		}

		virtual void CalcCustomRectUV(Tesseract::ImGui::IImFontAtlasCustomRect^ rect, System::Numerics::Vector2% outUVMin, System::Numerics::Vector2% outUVMax) {
			pin_ptr<Vector2> pOutUVMin = &outUVMin, pOutUVMax = &outUVMax;
			m_atlas->CalcCustomRectUV(((ImFontAtlasCustomRectCLI^)rect)->m_rect, (ImVec2*)pOutUVMin, (ImVec2*)pOutUVMax);
		}

		virtual bool GetMouseCursorTexData(Tesseract::ImGui::ImGuiMouseCursor cursor, System::Numerics::Vector2% outOffset, System::Numerics::Vector2% outSize, System::Span<System::Numerics::Vector2> outUVBorder, System::Span<System::Numerics::Vector2> outUVFill) {
			if (outUVBorder.Length < 2) throw gcnew ArgumentException("Output span must have length >=2", "outUVBorder");
			if (outUVFill.Length < 2) throw gcnew ArgumentException("Output span must have length >=2", "outUVBorder");
			pin_ptr<Vector2> pOutOffset = &outOffset, pOutSize = &outSize, pOutUVBorder = &MemoryMarshal::GetReference(outUVBorder), pOutUVFill = &MemoryMarshal::GetReference(outUVFill);
			return m_atlas->GetMouseCursorTexData((ImGuiMouseCursor)cursor, (ImVec2*)pOutOffset, (ImVec2*)pOutSize, (ImVec2*)pOutUVBorder, (ImVec2*)pOutUVFill);
		}
	};

}}}
