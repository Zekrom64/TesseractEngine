#include "imgui.h"
#include "imgui_cli.h"
#include "imgui_cli_draw.h"
#include "imgui_cli_font.h"
#include "imgui_cli_style.h"
#include "imgui_cli_io.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

typedef unsigned char byte;

/* Note: Counterintuitively we use Tesseract::CLI::ImGui instead of Tesseract::ImGui::CLI because
 * the C++ / CLI compiler is retarded and will override ImGui's C++ types with the managed versions.
 */
namespace Tesseract { namespace CLI { namespace ImGui {

	// Globals that CLI doesn't like being in managed types
	StringParam g_comboGetterText;
	StringParam g_listBoxText;

	public ref class ImGuiContextCLI : Tesseract::ImGui::IImGuiContext {
	internal:
		ImGuiContext* m_context;

		ImGuiContextCLI(ImGuiContext* ctx) : m_context(ctx) {}
	};

	public ref class ImGuiInputTextCallbackDataCLI : Tesseract::ImGui::IImGuiInputTextCallbackData {
	internal:
		ImGuiInputTextCallbackData* m_data;

		ImGuiInputTextCallbackDataCLI(ImGuiInputTextCallbackData* data) : m_data(data) { }

	public:
		virtual property Tesseract::ImGui::ImGuiInputTextFlags EventFlag {
			virtual Tesseract::ImGui::ImGuiInputTextFlags get() { return (Tesseract::ImGui::ImGuiInputTextFlags)m_data->EventFlag; }
		}

		virtual property Tesseract::ImGui::ImGuiInputTextFlags Flags {
			virtual Tesseract::ImGui::ImGuiInputTextFlags get() { return (Tesseract::ImGui::ImGuiInputTextFlags)m_data->Flags; }
		}

		virtual property wchar_t EventChar {
			virtual wchar_t get() { return m_data->EventChar; }
		}

		virtual property Tesseract::ImGui::ImGuiKey EventKey {
			virtual Tesseract::ImGui::ImGuiKey get() { return (Tesseract::ImGui::ImGuiKey)m_data->EventKey; }
		}

		virtual property System::Span<byte> Buf {
			virtual Span<byte> get() {
				return Span<byte>(m_data->Buf, m_data->BufSize);
			}
		}

		virtual property int BufSize {
			virtual int get() {
				return m_data->BufSize;
			}
		}

		virtual property int BufTextLen {
			virtual int get() {
				return m_data->BufTextLen;
			}
		}

		virtual property bool BufDirty {
			virtual bool get() { return m_data->BufDirty; }
			virtual void set(bool value) { m_data->BufDirty = true; }
		}

		virtual property int CursorPos {
			virtual int get() { return m_data->CursorPos; }
			virtual void set(int value) { m_data->CursorPos = value; }
		}

		virtual property int SelectionStart {
			virtual int get() { return m_data->SelectionStart; }
			virtual void set(int value) { m_data->SelectionStart = value; }
		}

		virtual property int SelectionEnd {
			virtual int get() { return m_data->SelectionEnd; }
			virtual void set(int value) { m_data->SelectionEnd = value; }
		}

		virtual property bool HasSelection {
			virtual bool get() { return m_data->HasSelection(); }
		}

		virtual void DeleteChars(int pos, int bytesCount) {
			m_data->DeleteChars(pos, bytesCount);
		}

		virtual void InsertChars(int pos, System::String^ text) {
			StringParam pText(text);
			m_data->InsertChars(pos, pText.begin(), pText.end());
		}

		virtual void SelectAll() {
			m_data->SelectAll();
		}

		virtual void ClearSelection() {
			m_data->ClearSelection();
		}

	};

	public ref class ImGuiTableSortSpecsCLI : Tesseract::ImGui::IImGuiTableSortSpecs {
	internal:
		ImGuiTableSortSpecs* m_specs;

		ImGuiTableSortSpecsCLI(ImGuiTableSortSpecs* specs) : m_specs(specs) { }

	public:
		virtual property System::ReadOnlySpan<Tesseract::ImGui::ImGuiTableColumnSortSpecs> Specs {
			virtual ReadOnlySpan<Tesseract::ImGui::ImGuiTableColumnSortSpecs> get() {
				return ReadOnlySpan<Tesseract::ImGui::ImGuiTableColumnSortSpecs>((void*)m_specs->Specs, m_specs->SpecsCount);
			}
		}

		virtual property bool SpecsDirty {
			virtual bool get() { return m_specs->SpecsDirty; }
		}
	};

	public ref class ImGuiPayloadCLI : Tesseract::ImGui::IImGuiPayload {
	internal:
		const ImGuiPayload* m_payload;

		ImGuiPayloadCLI(const ImGuiPayload* payload) : m_payload(payload) {}

	public:
		virtual property System::ReadOnlySpan<byte> Data {
			virtual ReadOnlySpan<byte> get() {
				return ReadOnlySpan<byte>(m_payload->Data, m_payload->DataSize);
			}
		}

		virtual property bool IsPreview {
			virtual bool get() { return m_payload->IsPreview(); }
		}

		virtual property bool IsDelivery {
			virtual bool get() { return m_payload->IsDelivery(); }
		}

		virtual bool IsDataType(System::String^ type) {
			StringParam pType(type);
			return m_payload->IsDataType(pType.c_str());
		}
	};

	public ref class ImGuiStorageCLI : Tesseract::ImGui::IImGuiStorage {
	internal:
		ImGuiStorage* m_storage;
		bool m_allocd;

		ImGuiStorageCLI(ImGuiStorage* storage, bool allocd) : m_storage(storage), m_allocd(allocd) {}

		~ImGuiStorageCLI() {
			if (m_allocd) delete m_storage;
		}

	public:
		virtual void Clear() {
			m_storage->Clear();
		}

		virtual int GetInt(unsigned int key, int defaultVal) {
			return m_storage->GetInt(key, defaultVal);
		}

		virtual void SetInt(unsigned int key, int val) {
			m_storage->SetInt(key, val);
		}

		virtual bool GetBool(unsigned int key, bool defaultVal) {
			return m_storage->GetBool(key, defaultVal);
		}

		virtual void SetBool(unsigned int key, bool val) {
			m_storage->SetBool(key, val);
		}

		virtual float GetFloat(unsigned int key, float defaultVal) {
			return m_storage->GetFloat(key, defaultVal);
		}

		virtual void SetFloat(unsigned int key, float val) {
			m_storage->SetFloat(key, val);
		}

		virtual System::IntPtr GetPtr(unsigned int key, System::IntPtr defaultVal) {
			void* ptr = m_storage->GetVoidPtr(key);
			return ptr ? (IntPtr)ptr : defaultVal;
		}

		virtual void SetPtr(unsigned int key, System::IntPtr val) {
			m_storage->SetVoidPtr(key, (void*)val);
		}

		virtual void BuildSortByKey() {
			m_storage->BuildSortByKey();
		}
	};


	void ImDrawListCLI::AddText(Tesseract::ImGui::IImFont^ font, float fontSize, System::Numerics::Vector2 pos, unsigned int col, System::String^ text, float wrapWidth, System::Nullable<System::Numerics::Vector4> cpuFineClipRect) {
		ImFont* pFont = ((ImFontCLI^)font)->m_font;
		StringParam pText(text);
		ImVec4 clip;
		if (cpuFineClipRect.HasValue) {
			auto clipVal = cpuFineClipRect.Value;
			clip = { clipVal.X, clipVal.Y, clipVal.Z, clipVal.W };
		}
		m_drawlist->AddText(pFont, fontSize, { pos.X, pos.Y }, col, pText.begin(), pText.end(), wrapWidth, cpuFineClipRect.HasValue ? &clip : nullptr);
	}

	ImFontCLI::ImFontCLI(ImFont* font) : m_font(font) {
		m_atlas = gcnew ImFontAtlasCLI(font->ContainerAtlas, false);
		m_indexadvancex = gcnew IndexAdvanceXImpl(&font->IndexAdvanceX);
		m_indexlookup = gcnew IndexLookupImpl(&font->IndexLookup);
		m_glyphs = gcnew GlyphsImpl(&font->Glyphs);
	}


	public ref class ImGuiCLI : Tesseract::ImGui::IImGui {
	public:

		// Type constructors

		virtual Tesseract::ImGui::IImGuiStyle^ NewStyle() {
			return gcnew ImGuiStyleCLI();
		}

		virtual Tesseract::ImGui::IImFontAtlas^ NewFontAtlas() {
			return gcnew ImFontAtlasCLI(new ImFontAtlas(), true);
		}

		virtual Tesseract::ImGui::IImGuiStorage^ NewStorage() {
			return gcnew ImGuiStorageCLI(new ImGuiStorage(), true);
		}

		// Context creation and access
		// - Each context create its own ImFontAtlas by default. You may instance one yourself and pass it to CreateContext() to share a font atlas between contexts.
		// - DLL users: heaps and globals are not shared across DLL boundaries! You will need to call SetCurrentContext() + SetAllocatorFunctions()
		//   for each static/DLL boundary you are calling from. Read "Context and Memory Allocators" section of imgui.cpp for details.

		virtual Tesseract::ImGui::IImGuiContext^ CreateContext(Tesseract::ImGui::IImFontAtlas^ sharedFontAtlas) {
			ImFontAtlas* atlas = nullptr;
			if (sharedFontAtlas != nullptr) atlas = ((ImFontAtlasCLI^)sharedFontAtlas)->m_atlas;
			return gcnew ImGuiContextCLI(::ImGui::CreateContext(atlas));
		}

		virtual void DestroyContext(Tesseract::ImGui::IImGuiContext^ ctx) {
			ImGuiContext* p_ctx = nullptr;
			if (ctx) p_ctx = ((ImGuiContextCLI^)ctx)->m_context;
			::ImGui::DestroyContext(p_ctx);
		}

	private:
		ImGuiContextCLI^ m_currentContext = nullptr;

	public:
		virtual property Tesseract::ImGui::IImGuiContext^ CurrentContext {
			virtual Tesseract::ImGui::IImGuiContext^ get() {
				ImGuiContext* ctx = ::ImGui::GetCurrentContext();
				if (ctx == nullptr) {
					m_currentContext = nullptr;
					return nullptr;
				}
				if (m_currentContext == nullptr || m_currentContext->m_context != ctx)
					m_currentContext = gcnew ImGuiContextCLI(ctx);
				return m_currentContext;
			}
			virtual void set(Tesseract::ImGui::IImGuiContext^ value) {
				m_currentContext = (ImGuiContextCLI^)value;
				::ImGui::SetCurrentContext(m_currentContext->m_context);
			}
		}

		// Main

	private:
		ImGuiIOCLI^ m_io = nullptr;

	public:
		virtual property Tesseract::ImGui::IImGuiIO^ IO {
			virtual Tesseract::ImGui::IImGuiIO^ get() {
				if (!m_io) m_io = gcnew ImGuiIOCLI(&::ImGui::GetIO());
				return m_io;
			}
		}

	private:
		ImGuiStyleCLI^ m_style = nullptr;

	public:
		virtual property Tesseract::ImGui::IImGuiStyle^ Style {
			virtual Tesseract::ImGui::IImGuiStyle^ get() {
				if (!m_style) m_style = gcnew ImGuiStyleCLI(&::ImGui::GetStyle());
				return m_style;
			}
			virtual void set(Tesseract::ImGui::IImGuiStyle^ value) {
				::ImGui::GetStyle() = *((ImGuiStyleCLI^)value)->m_style;
			}
		}

		virtual void NewFrame() {
			::ImGui::NewFrame();
			DrawCallbackHolder::Instance->Clear();
		}

		virtual void EndFrame() { ::ImGui::EndFrame(); }
		virtual void Render() { ::ImGui::Render(); }

	private:
		ImDrawDataCLI^ m_drawData = nullptr;

	public:
		virtual Tesseract::ImGui::IImDrawData^ GetDrawData() {
			ImDrawData* ddata = ::ImGui::GetDrawData();
			if (ddata == nullptr) {
				m_drawData = nullptr;
				return nullptr;
			}
			if (m_drawData == nullptr || m_drawData->m_drawdata != ddata)
				m_drawData = gcnew ImDrawDataCLI(::ImGui::GetDrawData());
			return m_drawData;
		}

		// Demo, Debug, Information

		virtual void ShowDemoWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowDemoWindow(pOpen);
		}

		virtual void ShowMetricsWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowMetricsWindow(pOpen);
		}

		virtual void ShowStackToolWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowStackToolWindow(pOpen);
		}

		virtual void ShowAboutWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowAboutWindow(pOpen);
		}

		virtual void ShowStyleEditor(Tesseract::ImGui::IImGuiStyle^ style) {
			ImGuiStyle* pStyle = nullptr;
			if (style) ((ImGuiStyleCLI^)style)->m_style;
			::ImGui::ShowStyleEditor(pStyle);
		}

		virtual void ShowStyleSelector(ReadOnlySpan<byte> label) {
			IM_SPAN_TO_STR(pLabel, label);
			::ImGui::ShowStyleSelector(pLabel);
		}

		virtual void ShowFontSelector(ReadOnlySpan<byte> label) {
			IM_SPAN_TO_STR(pLabel, label);
			::ImGui::ShowFontSelector(pLabel);
		}
		
		virtual void ShowUserGuide() {
			::ImGui::ShowUserGuide();
		}

		virtual property System::String^ Version {
			virtual String^ get() {
				return gcnew String(::ImGui::GetVersion());
			}
		}

		// Styles

		virtual void StyleColorsDark(Tesseract::ImGui::IImGuiStyle^ dst) {
			ImGuiStyle* pDst = nullptr;
			if (dst) ((ImGuiStyleCLI^)dst)->m_style;
			::ImGui::StyleColorsDark(pDst);
		}

		virtual void StyleColorsLight(Tesseract::ImGui::IImGuiStyle^ dst) {
			ImGuiStyle* pDst = nullptr;
			if (dst) ((ImGuiStyleCLI^)dst)->m_style;
			::ImGui::StyleColorsLight(pDst);
		}

		virtual void StyleColorsClassic(Tesseract::ImGui::IImGuiStyle^ dst) {
			ImGuiStyle* pDst = nullptr;
			if (dst) ((ImGuiStyleCLI^)dst)->m_style;
			::ImGui::StyleColorsClassic(pDst);
		}

		// Windows
		// - Begin() = push window to the stack and start appending to it. End() = pop window from the stack.
		// - Passing 'bool* p_open != NULL' shows a window-closing widget in the upper-right corner of the window,
		//   which clicking will set the boolean to false when clicked.
		// - You may append multiple times to the same window during the same frame by calling Begin()/End() pairs multiple times.
		//   Some information such as 'flags' or 'p_open' will only be considered by the first call to Begin().
		// - Begin() return false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting
		//   anything to the window. Always call a matching End() for each Begin() call, regardless of its return value!
		//   [Important: due to legacy reason, this is inconsistent with most other functions such as BeginMenu/EndMenu,
		//    BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding BeginXXX function
		//    returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]
		// - Note that the bottom of window stack always contains a window called "Debug".

		virtual bool Begin(ReadOnlySpan<byte> name, bool% open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			IM_SPAN_TO_STR(pName, name);
			pin_ptr<bool> pOpen = &open;
			return ::ImGui::Begin(pName, pOpen, (ImGuiWindowFlags)flags);
		}

		virtual void End() {
			::ImGui::End();
		}

		// Child Windows
		// - Use child windows to begin into a self-contained independent scrolling/clipping regions within a host window. Child windows can embed their own child.
		// - For each independent axis of 'size': ==0.0f: use remaining host window size / >0.0f: fixed size / <0.0f: use remaining window size minus abs(size) / Each axis can use a different mode, e.g. ImVec2(0,400).
		// - BeginChild() returns false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting anything to the window.
		//   Always call a matching EndChild() for each BeginChild() call, regardless of its return value.
		//   [Important: due to legacy reason, this is inconsistent with most other functions such as BeginMenu/EndMenu,
		//    BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding BeginXXX function
		//    returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]

		virtual void BeginChild(ReadOnlySpan<byte> strId, System::Numerics::Vector2 size, bool border, Tesseract::ImGui::ImGuiWindowFlags flags) {
			IM_SPAN_TO_STR(pStrId, strId);
			::ImGui::BeginChild(pStrId, { size.X, size.Y }, border, (ImGuiWindowFlags)flags);
		}

		virtual void BeginChild(unsigned int id, System::Numerics::Vector2 size, bool border, Tesseract::ImGui::ImGuiWindowFlags flags) {
			::ImGui::BeginChild(id, { size.X, size.Y }, border, (ImGuiWindowFlags)flags);
		}

		virtual void EndChild() {
			::ImGui::EndChild();
		}

		// Windows Utilities
		// - 'current window' = the window we are appending into while inside a Begin()/End() block. 'next window' = next window we will Begin() into.

		virtual bool IsWindowFocused(Tesseract::ImGui::ImGuiFocusedFlags flags) {
			return ::ImGui::IsWindowFocused((ImGuiFocusedFlags)flags);
		}

		virtual bool IsWindowHovered(Tesseract::ImGui::ImGuiHoveredFlags flags) {
			return ::ImGui::IsWindowHovered((ImGuiHoveredFlags)flags);
		}

		virtual Tesseract::ImGui::IImDrawList^ GetWindowDrawList() {
			return gcnew ImDrawListCLI(::ImGui::GetWindowDrawList(), false);
		}

		virtual property bool IsWindowAppearing {
			virtual bool get() { return ::ImGui::IsWindowAppearing(); }
		}

		virtual property bool IsWindowCollapsed {
			virtual bool get() { return ::ImGui::IsWindowCollapsed(); }
		}

		virtual property System::Numerics::Vector2 WindowPos {
			virtual Vector2 get() {
				auto value = ::ImGui::GetWindowPos();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 WindowSize {
			virtual Vector2 get() {
				auto value = ::ImGui::GetWindowSize();
				return Vector2(value.x, value.y);
			}
		}

		virtual property float WindowWidth {
			virtual float get() { return ::ImGui::GetWindowWidth(); }
		}

		virtual property float WindowHeight {
			virtual float get() { return ::ImGui::GetWindowHeight(); }
		}

		// Window manipulation
		// - Prefer using SetNextXXX functions (before Begin) rather that SetXXX functions (after Begin).

		virtual void SetNextWindowPos(System::Numerics::Vector2 pos, Tesseract::ImGui::ImGuiCond cond, System::Numerics::Vector2 pivot) {
			::ImGui::SetNextWindowPos({ pos.X, pos.Y }, (ImGuiCond)cond, { pivot.X, pivot.Y });
		}

		virtual void SetNextWindowSize(System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetNextWindowSize({ size.X, size.Y }, (ImGuiCond)cond);
		}

	private:
		Tesseract::ImGui::ImGuiSizeCallback^ setNextWindowSizeConstraintsCallback;

	public:
		virtual void SetNextWindowSizeConstraints(System::Numerics::Vector2 sizeMin, System::Numerics::Vector2 sizeMax, Tesseract::ImGui::ImGuiSizeCallback^ customCallback) {
			setNextWindowSizeConstraintsCallback = customCallback;
			IntPtr pCustomCallback = IntPtr::Zero;
			if (customCallback) pCustomCallback = Marshal::GetFunctionPointerForDelegate(customCallback);
			::ImGui::SetNextWindowSizeConstraints({ sizeMin.X, sizeMin.Y }, { sizeMax.X, sizeMax.Y }, (ImGuiSizeCallback)(void*)pCustomCallback, nullptr);
		}

		virtual void SetNextWindowContentSize(System::Numerics::Vector2 size) {
			::ImGui::SetNextWindowContentSize({ size.X, size.Y });
		}

		virtual void SetNextWindowCollapsed(bool collapsed, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetNextWindowCollapsed(collapsed, (ImGuiCond)cond);
		}

		virtual void SetNextWindowFocus() {
			::ImGui::SetNextWindowFocus();
		}

		virtual void SetNextWindowBgAlpha(float alpha) {
			::ImGui::SetNextWindowBgAlpha(alpha);
		}

		virtual void SetWindowPos(System::Numerics::Vector2 pos, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetWindowPos({ pos.X, pos.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowSize(System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetWindowSize({ size.X, size.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowCollapsed(bool collapsed, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetWindowCollapsed(collapsed, (ImGuiCond)cond);
		}

		virtual void SetWindowFocus() {
			::ImGui::SetWindowFocus();
		}

		virtual void SetWindowFontScale(float scale) {
			::ImGui::SetWindowFontScale(scale);
		}

		virtual void SetWindowPos(ReadOnlySpan<byte> name, System::Numerics::Vector2 pos, Tesseract::ImGui::ImGuiCond cond) {
			IM_SPAN_TO_STR(pName, name);
			::ImGui::SetWindowPos(pName, { pos.X, pos.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowSize(ReadOnlySpan<byte> name, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiCond cond) {
			IM_SPAN_TO_STR(pName, name);
			::ImGui::SetWindowSize(pName, { size.X, size.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowCollapsed(ReadOnlySpan<byte> name, bool collapsed, Tesseract::ImGui::ImGuiCond cond) {
			IM_SPAN_TO_STR(pName, name);
			::ImGui::SetWindowCollapsed(pName, collapsed, (ImGuiCond)cond);
		}

		virtual void SetWindowFocus(ReadOnlySpan<byte> name) {
			IM_SPAN_TO_STR(pName, name);
			::ImGui::SetWindowFocus(pName);
		}

		// Content region
		// - Retrieve available space from a given point. GetContentRegionAvail() is frequently useful.
		// - Those functions are bound to be redesigned (they are confusing, incomplete and the Min/Max return values are in local window coordinates which increases confusion)

		virtual property System::Numerics::Vector2 ContentRegionAvail {
			virtual Vector2 get() {
				auto value = ::ImGui::GetContentRegionAvail();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 ContentRegionMax {
			virtual Vector2 get() {
				auto value = ::ImGui::GetContentRegionMax();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 WindowContentRegionMax {
			virtual Vector2 get() {
				auto value = ::ImGui::GetWindowContentRegionMax();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 WindowContentRegionMin {
			virtual Vector2 get() {
				auto value = ::ImGui::GetWindowContentRegionMin();
				return Vector2(value.x, value.y);
			}
		}

		// Windows Scrolling

		virtual property float ScrollX {
			virtual float get() { return ::ImGui::GetScrollX(); }
			virtual void set(float value) { ::ImGui::SetScrollX(value); }
		}

		virtual property float ScrollY {
			virtual float get() { return ::ImGui::GetScrollY(); }
			virtual void set(float value) { ::ImGui::SetScrollY(value); }
		}

		virtual property float ScrollMaxX {
			virtual float get() { return ::ImGui::GetScrollMaxX(); }
		}

		virtual property float ScrollMaxY {
			virtual float get() { return ::ImGui::GetScrollMaxY(); }
		}

		virtual void SetScrollHereX(float centerXRatio) {
			::ImGui::SetScrollHereX(centerXRatio);
		}

		virtual void SetScrollHereY(float centerYRatio) {
			::ImGui::SetScrollHereY(centerYRatio);
		}

		virtual void SetScrollFromPosX(float localX, float centerXRatio) {
			::ImGui::SetScrollFromPosX(localX, centerXRatio);
		}

		virtual void SetScrollFromPosY(float localY, float centerYRatio) {
			::ImGui::SetScrollFromPosY(localY, centerYRatio);
		}

		// Parameters stacks (shared)

		virtual void PushFont(Tesseract::ImGui::IImFont^ font) {
			::ImGui::PushFont(((ImFontCLI^)font)->m_font);
		}

		virtual void PopFont() {
			::ImGui::PopFont();
		}

		virtual void PushStyleColor(Tesseract::ImGui::ImGuiCol idx, unsigned int col) {
			::ImGui::PushStyleColor((ImGuiCol)idx, col);
		}

		virtual void PushStyleColor(Tesseract::ImGui::ImGuiCol idx, System::Numerics::Vector4 col) {
			::ImGui::PushStyleColor((ImGuiCol)idx, { col.X, col.Y, col.Z, col.W });
		}

		virtual void PopStyleColor(int count) {
			::ImGui::PopStyleColor(count);
		}

		virtual void PushStyleVar(Tesseract::ImGui::ImGuiStyleVar idx, float val) {
			::ImGui::PushStyleVar((ImGuiStyleVar)idx, val);
		}

		virtual void PushStyleVar(Tesseract::ImGui::ImGuiStyleVar idx, System::Numerics::Vector2 val) {
			::ImGui::PushStyleVar((ImGuiStyleVar)idx, { val.X, val.Y });
		}

		virtual void PopStyleVar(int count) {
			::ImGui::PopStyleVar(count);
		}

		virtual void PushTabStop(bool allowTabStop) {
			::ImGui::PushAllowKeyboardFocus(allowTabStop);
		}

		virtual void PopTabStop() {
			::ImGui::PopAllowKeyboardFocus();
		}

		virtual void PushButtonRepeat(bool repeat) {
			::ImGui::PushButtonRepeat(repeat);
		}

		virtual void PopButtonRepeat() {
			::ImGui::PopButtonRepeat();
		}

		// Parameters stacks (current window)

		virtual void PushItemWidth(float itemWidth) {
			::ImGui::PushItemWidth(itemWidth);
		}

		virtual void PopItemWidth() {
			::ImGui::PopItemWidth();
		}

		virtual void SetNextItemWidth(float itemWidth) {
			::ImGui::SetNextItemWidth(itemWidth);
		}

		virtual float CalcItemWidth() {
			return ::ImGui::CalcItemWidth();
		}

		virtual void PushTextWrapPos(float wrapLocalPosX) {
			::ImGui::PushTextWrapPos(wrapLocalPosX);
		}

		virtual void PopTextWrapPos() {
			::ImGui::PopTextWrapPos();
		}

		// Style read access
		// - Use the style editor (ShowStyleEditor() function) to interactively see what the colors are)

		virtual property Tesseract::ImGui::IImFont^ Font {
			virtual Tesseract::ImGui::IImFont^ get() {
				return gcnew ImFontCLI(::ImGui::GetFont());
			}
		}

		virtual property float FontSize {
			virtual float get() { return ::ImGui::GetFontSize(); }
		}

		virtual property System::Numerics::Vector2 FontTexUvWhitePixel {
			virtual Vector2 get() {
				auto value = ::ImGui::GetFontTexUvWhitePixel();
				return Vector2(value.x, value.y);
			}
		}

		virtual unsigned int GetColorU32(Tesseract::ImGui::ImGuiCol idx, float alphaMul) {
			return ::ImGui::GetColorU32((ImGuiCol)idx, alphaMul);
		}

		virtual unsigned int GetColorU32(System::Numerics::Vector4 col) {
			return ::ImGui::GetColorU32({col.X, col.Y, col.Z, col.W});
		}

		virtual unsigned int GetColorU32(unsigned int col) {
			return ::ImGui::GetColorU32(col);
		}

		virtual System::Numerics::Vector4 GetStyleColorVec4(Tesseract::ImGui::ImGuiCol idx) {
			auto& retn = ::ImGui::GetStyleColorVec4((ImGuiCol)idx);
			return Vector4(retn.x, retn.y, retn.z, retn.w);
		}

		// Cursor / Layout
		// - By "cursor" we mean the current output position.
		// - The typical widget behavior is to output themselves at the current cursor position, then move the cursor one line down.
		// - You can call SameLine() between widgets to undo the last carriage return and output at the right of the preceding widget.
		// - Attention! We currently have inconsistencies between window-local and absolute positions we will aim to fix with future API:
		//    Window-local coordinates:   SameLine(), GetCursorPos(), SetCursorPos(), GetCursorStartPos(), GetContentRegionMax(), GetWindowContentRegion*(), PushTextWrapPos()
		//    Absolute coordinate:        GetCursorScreenPos(), SetCursorScreenPos(), all ImDrawList:: functions.

		virtual void Separator() {
			::ImGui::Separator();
		}

		virtual void SameLine(float offsetFromStartX, float spacing) {
			::ImGui::SameLine(offsetFromStartX, spacing);
		}

		virtual void NewLine() {
			::ImGui::NewLine();
		}

		virtual void Spacing() {
			::ImGui::Spacing();
		}

		virtual void Dummy(System::Numerics::Vector2 size) {
			::ImGui::Dummy({ size.X, size.Y });
		}

		virtual void Indent(float indentW) {
			::ImGui::Indent(indentW);
		}

		virtual void Unindent(float indentW) {
			::ImGui::Unindent(indentW);
		}

		virtual void BeginGroup() {
			::ImGui::BeginGroup();
		}

		virtual void EndGroup() {
			::ImGui::EndGroup();
		}

		virtual property System::Numerics::Vector2 CursorPos {
			virtual Vector2 get() {
				auto value = ::ImGui::GetCursorPos();
				return Vector2(value.x, value.y);
			}
			virtual void set(Vector2 value) { ::ImGui::SetCursorPos({ value.X, value.Y }); }
		}

		virtual property float CursorPosX {
			virtual float get() { return ::ImGui::GetCursorPosX(); }
			virtual void set(float value) { ::ImGui::SetCursorPosX(value); }
		}

		virtual property float CursorPosY {
			virtual float get() { return ::ImGui::GetCursorPosY(); }
			virtual void set(float value) { ::ImGui::SetCursorPosY(value); }
		}

		virtual property System::Numerics::Vector2 CursorStartPos {
			virtual Vector2 get() {
				auto value = ::ImGui::GetCursorStartPos();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 CursorScreenPos {
			virtual Vector2 get() {
				auto value = ::ImGui::GetCursorScreenPos();
				return Vector2(value.x, value.y);
			}
			virtual void set(Vector2 value) { ::ImGui::SetCursorScreenPos({ value.X, value.Y }); }
		}

		virtual void AlignTextToFramePadding() {
			::ImGui::AlignTextToFramePadding();
		}

		virtual property float TextLineHeight {
			virtual float get() { return ::ImGui::GetTextLineHeight(); }
		}

		virtual property float TextLineHeightWithSpacing {
			virtual float get() { return ::ImGui::GetTextLineHeightWithSpacing(); }
		}

		virtual property float FrameHeight {
			virtual float get() { return ::ImGui::GetFrameHeight(); }
		}

		virtual property float FrameHeightWithSpacing {
			virtual float get() { return ::ImGui::GetFrameHeightWithSpacing(); }
		}

		// ID stack/scopes
		// Read the FAQ (docs/FAQ.md or http://dearimgui.org/faq) for more details about how ID are handled in dear imgui.
		// - Those questions are answered and impacted by understanding of the ID stack system:
		//   - "Q: Why is my widget not reacting when I click on it?"
		//   - "Q: How can I have widgets with an empty label?"
		//   - "Q: How can I have multiple widgets with the same label?"
		// - Short version: ID are hashes of the entire ID stack. If you are creating widgets in a loop you most likely
		//   want to push a unique identifier (e.g. object pointer, loop index) to uniquely differentiate them.
		// - You can also use the "Label##foobar" syntax within widget label to distinguish them from each others.
		// - In this header file we use the "label"/"name" terminology to denote a string that will be displayed + used as an ID,
		//   whereas "str_id" denote a string that is only used as an ID and not normally displayed.

		virtual void PushID(ReadOnlySpan<byte> strID) {
			IM_SPAN_TO_STR(pStrID, strID);
			::ImGui::PushID((const char*)pStrID);
		}

		virtual void PushID(System::IntPtr ptrID) {
			::ImGui::PushID((void*)ptrID);
		}

		virtual void PushID(int id) {
			::ImGui::PushID(id);
		}

		virtual void PopID() {
			::ImGui::PopID();
		}

		virtual unsigned int GetID(ReadOnlySpan<byte> strID) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::GetID((const char*)pStrID);
		}

		virtual unsigned int GetID(System::IntPtr ptrID) {
			return ::ImGui::GetID((void*)ptrID);
		}

		// Widgets: Text

		virtual void Text(ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			const char* pcText = (const char*)pText;
			::ImGui::TextUnformatted(pcText, pcText + text.Length);
		}

		virtual void TextColored(System::Numerics::Vector4 col, ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			::ImGui::TextColored({ col.X, col.Y, col.Z, col.W }, "%s", (const char*)pText);
		}

		virtual void TextDisabled(ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			::ImGui::TextDisabled("%s", (const char*)pText);
		}

		virtual void TextWrapped(ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			::ImGui::TextWrapped("%s", (const char*)pText);
		}

		virtual void LabelText(ReadOnlySpan<byte> label, ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pText, text);
			::ImGui::LabelText((const char*)pLabel, "%s", (const char*)pText);
		}

		virtual void BulletText(ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			::ImGui::BulletText("%s", (const char*)pText);
		}

		// Widgets: Main
		// - Most widgets return true when the value has been changed or when pressed/selected
		// - You may also use one of the many IsItemXXX functions (e.g. IsItemActive, IsItemHovered, etc.) to query widget state.

		virtual bool Button(ReadOnlySpan<byte> label, System::Numerics::Vector2 size) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::Button((const char*)pLabel, { size.X, size.Y });
		}

		virtual bool SmallButton(ReadOnlySpan<byte> label) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::SmallButton((const char*)pLabel);
		}

		virtual bool InvisibleButton(ReadOnlySpan<byte> strID, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiButtonFlags flags) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::InvisibleButton((const char*)pStrID, { size.X, size.Y }, (ImGuiButtonFlags)flags);
		}

		virtual bool ArrowButton(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiDir dir) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::ArrowButton((const char*)pStrID, (ImGuiDir)dir);
		}

		virtual void Image(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1, System::Numerics::Vector4 tintCol, System::Numerics::Vector4 borderCol) {
			::ImGui::Image((void*)userTextureID, { size.X, size.Y }, { uv0.X, uv0.Y }, { uv1.X, uv1.Y }, { tintCol.X, tintCol.Y, tintCol.Z, tintCol.W }, { borderCol.X, borderCol.Y, borderCol.Z, borderCol.W });
		}

		virtual void Image(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1) {
			Image(userTextureID, size, uv0, uv1, Vector4::One, Vector4::Zero);
		}

		virtual void Image(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0) {
			Image(userTextureID, size, uv0, Vector2::One);
		}

		virtual bool ImageButton(System::ReadOnlySpan<byte> strID, System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1, System::Numerics::Vector4 bgCol, System::Numerics::Vector4 tintCol) {
			return ::ImGui::ImageButton((void*)userTextureID, { size.X, size.Y }, { uv0.X, uv0.Y }, { uv1.X, uv1.Y }, 0, { bgCol.X, bgCol.Y, bgCol.Z, bgCol.W }, { tintCol.X, tintCol.Y, tintCol.Z, tintCol.W });
		}

		virtual bool ImageButton(System::ReadOnlySpan<byte> strID, System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1, System::Numerics::Vector4 bgCol) {
			return ImageButton(strID, userTextureID, size, uv0, uv1, bgCol, Vector4::One);
		}

		virtual bool ImageButton(System::ReadOnlySpan<byte> strID, System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0) {
			return ImageButton(strID, userTextureID, size, uv0, Vector2::One, Vector4::Zero);
		}

		virtual bool Checkbox(ReadOnlySpan<byte> label, bool% v) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<bool> pv = &v;
			return ::ImGui::Checkbox((const char*)pLabel, pv);
		}

		virtual bool CheckboxFlags(ReadOnlySpan<byte> label, int% flags, int flagsValue) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pFlags = &flags;
			return ::ImGui::CheckboxFlags((const char*)pLabel, pFlags, flagsValue);
		}

		virtual bool CheckboxFlags(ReadOnlySpan<byte> label, unsigned int% flags, unsigned int flagsValue) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<unsigned int> pFlags = &flags;
			return ::ImGui::CheckboxFlags((const char*)pLabel, pFlags, flagsValue);
		}

		virtual bool RadioButton(ReadOnlySpan<byte> label, bool active) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::RadioButton((const char*)pLabel, active);
		}

		virtual bool RadioButton(ReadOnlySpan<byte> label, int% v, int vButton) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pv = &v;
			return ::ImGui::RadioButton((const char*)pLabel, pv, vButton);
		}

		virtual void ProgressBar(float fraction, System::Numerics::Vector2 sizeArg, ReadOnlySpan<byte> overlay) {
			IM_SPAN_TO_STR(pOverlay, overlay);
			::ImGui::ProgressBar(fraction, { sizeArg.X, sizeArg.Y }, overlay.IsEmpty ? nullptr : (const char*)pOverlay);
		}

		virtual void ProgressBar(float fraction) {
			ProgressBar(fraction, Vector2(-FLT_MIN, 0), nullptr);
		}

		virtual void Bullet() {
			::ImGui::Bullet();
		}

		// Widgets: Combo Box
		// - The BeginCombo()/EndCombo() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() items.
		// - The old Combo() api are helpers over BeginCombo()/EndCombo() which are kept available for convenience purpose. This is analogous to how ListBox are created.

		virtual bool BeginCombo(ReadOnlySpan<byte> label, ReadOnlySpan<byte> previewValue, Tesseract::ImGui::ImGuiComboFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pPreviewValue, previewValue);
			return ::ImGui::BeginCombo((const char*)pLabel, (const char*)pPreviewValue, (ImGuiComboFlags)flags);
		}

		virtual void EndCombo() {
			::ImGui::EndCombo();
		}

		virtual bool Combo(ReadOnlySpan<byte> label, int% currentItem, System::Collections::Generic::IEnumerable<System::String^>^ items, int popupMaxHeightInItems) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pCurrentItem = &currentItem;
			StringArrayParam pItems(items);
			return ::ImGui::Combo((const char*)pLabel, pCurrentItem, pItems.data(), (int)pItems.length(), popupMaxHeightInItems);
		}

		virtual bool Combo(ReadOnlySpan<byte> label, int% currentItem, ReadOnlySpan<byte> itemsSeparatedByZeros, int popupMaxHeightInItems) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pItemsSeparatedByZeros, itemsSeparatedByZeros);
			pin_ptr<int> pCurrentItem = &currentItem;
			return ::ImGui::Combo((const char*)pLabel, pCurrentItem, (const char*)pItemsSeparatedByZeros, popupMaxHeightInItems);
		}

	private:
		static Tesseract::ImGui::IImGui::ComboItemsGetter^ comboGetterFn = nullptr;
		static bool comboGetterCbk(void*, int idx, const char** outText) {
			String^ text = nullptr;
			bool retn = comboGetterFn->Invoke(idx, text);
			*outText = (g_comboGetterText = text).c_str();
			return retn;
		}

	public:
		virtual bool Combo(ReadOnlySpan<byte> label, int% currentItem, Tesseract::ImGui::IImGui::ComboItemsGetter^ itemsGetter, int itemscount, int popupMaxHeightInItems) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pCurrentItem = &currentItem;
			comboGetterFn = itemsGetter;
			return ::ImGui::Combo((const char*)pLabel, pCurrentItem, (bool(*)(void*,int,const char**))comboGetterCbk, nullptr, itemscount, popupMaxHeightInItems);
		}

		// Widgets: Drag Sliders
		// - CTRL+Click on any drag box to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.
		// - For all the Float2/Float3/Float4/Int2/Int3/Int4 versions of every functions, note that a 'float v[X]' function argument is the same as 'float* v',
		//   the array syntax is just a way to document the number of elements that are expected to be accessible. You can pass address of your first element out of a contiguous set, e.g. &myvector.x
		// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: %.0f" -> Biscuit: 1; etc.
		// - Format string may also be set to NULL or use the default format ("%f" or "%d").
		// - Speed are per-pixel of mouse movement (v_speed=0.2f: mouse needs to move by 5 pixels to increase value by 1). For gamepad/keyboard navigation, minimum speed is Max(v_speed, minimum_step_at_given_precision).
		// - Use v_min < v_max to clamp edits to given limits. Note that CTRL+Click manual input can override those limits if ImGuiSliderFlags_AlwaysClamp is not used.
		// - Use v_max = FLT_MAX / INT_MAX etc to avoid clamping to a maximum, same with v_min = -FLT_MAX / INT_MIN to avoid clamping to a minimum.
		// - We use the same sets of flags for DragXXX() and SliderXXX() functions as the features are the same and it makes it easier to swap them.
		// - Legacy: Pre-1.78 there are DragXXX() function signatures that takes a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.
		//   If you get a warning converting a float to ImGuiSliderFlags, read https://github.com/ocornut/imgui/issues/3361

		virtual bool DragFloat(ReadOnlySpan<byte> label, float% v, float vSpeed, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<float> pv = &v;
			return ::ImGui::DragFloat(pLabel, pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat2(ReadOnlySpan<byte> label, System::Numerics::Vector2% v, float vSpeed, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::DragFloat2(pLabel, (float*)pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat3(ReadOnlySpan<byte> label, System::Numerics::Vector3% v, float vSpeed, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::DragFloat3(pLabel, (float*)pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat4(ReadOnlySpan<byte> label, System::Numerics::Vector4% v, float vSpeed, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::DragFloat4(pLabel, (float*)pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloatRange2(ReadOnlySpan<byte> label, float% vCurrentMin, float% vCurrentMax, float vSpeed, float vMin, float vMax, ReadOnlySpan<byte> format, ReadOnlySpan<byte> formatMax, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			IM_SPAN_TO_STR_DEFAULT(pFormatMax, formatMax, nullptr);
			pin_ptr<float> pvCurrentMin = &vCurrentMin, pvCurrentMax = &vCurrentMax;
			return ::ImGui::DragFloatRange2(pLabel, pvCurrentMin, pvCurrentMax, vSpeed, vMin, vMax, pFormat, pFormatMax, (ImGuiSliderFlags)flags);
		}

		virtual bool DragInt(ReadOnlySpan<byte> label, int% v, float vSpeed, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &v;
			return ::ImGui::DragInt(pLabel, pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool DragInt2(ReadOnlySpan<byte> label, System::Span<int> v, float vSpeed, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt2(pLabel, pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragInt3(ReadOnlySpan<byte> label, System::Span<int> v, float vSpeed, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt3(pLabel, pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragInt4(ReadOnlySpan<byte> label, System::Span<int> v, float vSpeed, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt4(pLabel, pv, vSpeed, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragIntRange2(ReadOnlySpan<byte> label, int% vCurrentMin, int% vCurrentMax, float vSpeed, int vMin, int vMax, ReadOnlySpan<byte> format, ReadOnlySpan<byte> formatMax, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			IM_SPAN_TO_STR_DEFAULT(pFormatMax, formatMax, nullptr);
			pin_ptr<int> pvCurrentMin = &vCurrentMin, pvCurrentMax = &vCurrentMax;
			return ::ImGui::DragIntRange2(pLabel, pvCurrentMin, pvCurrentMax, vSpeed, vMin, vMax, pFormat, pFormatMax, (ImGuiSliderFlags)flags);
		}

	private:
		generic<typename T>
		static ImGuiDataType GetDataType() {
			if (T::typeid == uint8_t::typeid) return ImGuiDataType_U8;
			if (T::typeid == uint16_t::typeid) return ImGuiDataType_U16;
			if (T::typeid == uint32_t::typeid) return ImGuiDataType_U32;
			if (T::typeid == uint64_t::typeid) return ImGuiDataType_U64;
			if (T::typeid == int8_t::typeid) return ImGuiDataType_S8;
			if (T::typeid == int16_t::typeid) return ImGuiDataType_S16;
			if (T::typeid == int32_t::typeid) return ImGuiDataType_S32;
			if (T::typeid == int64_t::typeid) return ImGuiDataType_S64;
			if (T::typeid == float::typeid) return ImGuiDataType_Float;
			if (T::typeid == double::typeid) return ImGuiDataType_Double;
			throw gcnew ArgumentException("Type does not map to an ImGuiDataType", "T");
		}

	public:
		generic<typename T>
		where T : value class, gcnew()
		virtual bool DragScalar(ReadOnlySpan<byte> label, T% data, float vSpeed, Tesseract::ImGui::ImNullable<T> min, Tesseract::ImGui::ImNullable<T> max, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &data;
			T vMin = {}, vMax = {};
			pin_ptr<T> pMin = nullptr, pMax = nullptr;
			if (min.HasValue) {
				vMin = min.Value;
				pMin = &vMin;
			}
			if (max.HasValue) {
				vMax = max.Value;
				pMax = &vMax;
			}
			return ::ImGui::DragScalar(pLabel, GetDataType<T>(), pData, vSpeed, pMin, pMax, pFormat, (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool DragScalarN(ReadOnlySpan<byte> label, System::Span<T> data, float vSpeed, Tesseract::ImGui::ImNullable<T> min, Tesseract::ImGui::ImNullable<T> max, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &MemoryMarshal::GetReference(data);
			T vMin = {}, vMax = {};
			pin_ptr<T> pMin = nullptr, pMax = nullptr;
			if (min.HasValue) {
				vMin = min.Value;
				pMin = &vMin;
			}
			if (max.HasValue) {
				vMax = max.Value;
				pMax = &vMax;
			}
			return ::ImGui::DragScalarN(pLabel, GetDataType<T>(), pData, data.Length, vSpeed, pMin, pMax, pFormat, (ImGuiSliderFlags)flags);
		}

		// Widgets: Regular Sliders
		// - CTRL+Click on any slider to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.
		// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: %.0f" -> Biscuit: 1; etc.
		// - Format string may also be set to NULL or use the default format ("%f" or "%d").
		// - Legacy: Pre-1.78 there are SliderXXX() function signatures that takes a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.
		//   If you get a warning converting a float to ImGuiSliderFlags, read https://github.com/ocornut/imgui/issues/3361

		virtual bool SliderFloat(ReadOnlySpan<byte> label, float% v, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<float> pv = &v;
			return ::ImGui::SliderFloat(pLabel, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat2(ReadOnlySpan<byte> label, System::Numerics::Vector2% v, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::SliderFloat2(pLabel, (float*)pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat3(ReadOnlySpan<byte> label, System::Numerics::Vector3% v, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::SliderFloat3(pLabel, (float*)pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat4(ReadOnlySpan<byte> label, System::Numerics::Vector4% v, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::SliderFloat4(pLabel, (float*)pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderAngle(ReadOnlySpan<byte> label, float% vRad, float vDegreesMin, float vDegreesMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.0f deg");
			pin_ptr<float> pv = &vRad;
			return ::ImGui::SliderAngle(pLabel, pv, vDegreesMin, vDegreesMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt(ReadOnlySpan<byte> label, int% v, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &v;
			return ::ImGui::SliderInt(pLabel, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt2(ReadOnlySpan<byte> label, System::Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt2(pLabel, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt3(ReadOnlySpan<byte> label, System::Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 3) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt3(pLabel, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt4(ReadOnlySpan<byte> label, System::Span<int> v, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 4) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt4(pLabel, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool SliderScalar(ReadOnlySpan<byte> label, T% data, T min, T max, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &data, pMin = &min, pMax = &max;
			return ::ImGui::SliderScalar(pLabel, GetDataType<T>(), pData, pMin, pMax, pFormat, (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool SliderScalarN(ReadOnlySpan<byte> label, System::Span<T> data, T min, T max, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &MemoryMarshal::GetReference(data), pMin = &min, pMax = &max;
			return ::ImGui::SliderScalarN(pLabel, GetDataType<T>(), pData, data.Length, pMin, pMax, pFormat, (ImGuiSliderFlags)flags);;
		}

		virtual bool VSliderFloat(ReadOnlySpan<byte> label, System::Numerics::Vector2 size, float% v, float vMin, float vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<float> pv = &v;
			return ::ImGui::VSliderFloat(pLabel, {size.X, size.Y}, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		virtual bool VSliderInt(ReadOnlySpan<byte> label, System::Numerics::Vector2 size, int% v, int vMin, int vMax, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%d");
			pin_ptr<int> pv = &v;
			return ::ImGui::VSliderInt(pLabel, {size.X, size.Y}, pv, vMin, vMax, pFormat, (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool VSliderScalar(ReadOnlySpan<byte> label, System::Numerics::Vector2 size, T% data, T min, T max, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &data, pMin = &min, pMax = &max;
			return ::ImGui::VSliderScalar(pLabel, { size.X, size.Y }, GetDataType<T>(), pData, pMin, pMax, pFormat, (ImGuiSliderFlags)flags);
		}

		// Widgets: Input with Keyboard
		// - If you want to use InputText() with std::string or any custom dynamic string type, see misc/cpp/imgui_stdlib.h and comments in imgui_demo.cpp.
		// - Most of the ImGuiInputTextFlags flags are only useful for InputText() and not for InputFloatX, InputIntX, InputDouble etc.

	private:
		static Tesseract::ImGui::ImGuiInputTextCallback^ inputTextFn = nullptr;
		static void inputTextCbk(ImGuiInputTextCallbackData* data) {
			inputTextFn->Invoke(gcnew ImGuiInputTextCallbackDataCLI(data));
		}

	public:
		virtual bool InputText(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTextBuffer^ buf, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<byte> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputText(pLabel, (char*)pBuf, buf->Buf.Length, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}

		virtual bool InputTextMultiline(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTextBuffer^ buf, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<byte> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputTextMultiline(pLabel, (char*)pBuf, buf->Buf.Length, { size.X, size.Y }, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}
		
		virtual bool InputTextWithHint(ReadOnlySpan<byte> label, ReadOnlySpan<byte> hint, Tesseract::ImGui::ImGuiTextBuffer^ buf, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pHint, hint);
			pin_ptr<byte> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputTextWithHint(pLabel, pHint, (char*)pBuf, buf->Buf.Length, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}
		
		virtual bool InputFloat(ReadOnlySpan<byte> label, float% v, float step, float stepFast, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<float> pv = &v;
			return ::ImGui::InputFloat(pLabel, pv, step, stepFast, pFormat, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat2(ReadOnlySpan<byte> label, System::Numerics::Vector2% v, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::InputFloat2(pLabel, (float*)pv, pFormat, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat3(ReadOnlySpan<byte> label, System::Numerics::Vector3% v, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::InputFloat3(pLabel, (float*)pv, pFormat, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat4(ReadOnlySpan<byte> label, System::Numerics::Vector4% v, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.3f");
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::InputFloat4(pLabel, (float*)pv, pFormat, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt(ReadOnlySpan<byte> label, int% v, int step, int stepFast, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pv = &v;
			return ::ImGui::InputInt(pLabel, pv, step, stepFast, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt2(ReadOnlySpan<byte> label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt2(pLabel, pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt3(ReadOnlySpan<byte> label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 3) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt3(pLabel, pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt4(ReadOnlySpan<byte> label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 4) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt4(pLabel, pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputDouble(ReadOnlySpan<byte> label, double% v, double step, double stepFast, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, "%.6f");
			pin_ptr<double> pv = &v;
			return ::ImGui::InputDouble(pLabel, pv, step, stepFast, pFormat, (ImGuiInputTextFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool InputScalar(ReadOnlySpan<byte> label, T% data, Tesseract::ImGui::ImNullable<T> step, Tesseract::ImGui::ImNullable<T> stepFast, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &data;
			T vStep = {}, vStepFast = {};
			pin_ptr<T> pStep = nullptr, pStepFast = nullptr;
			if (step.HasValue) {
				vStep = step.Value;
				pStep = &vStep;
			}
			if (stepFast.HasValue) {
				vStepFast = stepFast.Value;
				pStepFast = &vStepFast;
			}
			return ::ImGui::InputScalar(pLabel, GetDataType<T>(), pData, pStep, pStepFast, pFormat, (ImGuiInputTextFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool InputScalarN(ReadOnlySpan<byte> label, System::Span<T> data, Tesseract::ImGui::ImNullable<T> step, Tesseract::ImGui::ImNullable<T> stepFast, ReadOnlySpan<byte> format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pFormat, format, nullptr);
			pin_ptr<T> pData = &MemoryMarshal::GetReference(data);
			T vStep = {}, vStepFast = {};
			pin_ptr<T> pStep = nullptr, pStepFast = nullptr;
			if (step.HasValue) {
				vStep = step.Value;
				pStep = &vStep;
			}
			if (stepFast.HasValue) {
				vStepFast = stepFast.Value;
				pStepFast = &vStepFast;
			}
			return ::ImGui::InputScalarN(pLabel, GetDataType<T>(), pData, data.Length, pStep, pStepFast, pFormat, (ImGuiInputTextFlags)flags);
		}

		// Widgets: Color Editor/Picker (tip: the ColorEdit* functions have a little color square that can be left-clicked to open a picker, and right-clicked to open an option menu.)
		// - Note that in C++ a 'float v[X]' function argument is the _same_ as 'float* v', the array syntax is just a way to document the number of elements that are expected to be accessible.
		// - You can pass the address of a first float element out of a contiguous structure, e.g. &myvector.x

		virtual bool ColorEdit3(ReadOnlySpan<byte> label, System::Numerics::Vector3% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			pin_ptr<Vector3> pCol = &col;
			return ::ImGui::ColorEdit3(pLabel, (float*)pCol, (ImGuiColorEditFlags)flags);
		}

		virtual bool ColorEdit4(ReadOnlySpan<byte> label, System::Numerics::Vector4% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			pin_ptr<Vector4> pCol = &col;
			return ::ImGui::ColorEdit4(pLabel, (float*)pCol, (ImGuiColorEditFlags)flags);
		}
		
		virtual bool ColorPicker3(ReadOnlySpan<byte> label, System::Numerics::Vector3% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			pin_ptr<Vector3> pCol = &col;
			return ::ImGui::ColorPicker3(pLabel, (float*)pCol, (ImGuiColorEditFlags)flags);
		}
		
		virtual bool ColorPicker4(ReadOnlySpan<byte> label, System::Numerics::Vector4% col, Tesseract::ImGui::ImGuiColorEditFlags flags, System::Nullable<System::Numerics::Vector4> refCol) {
			IM_SPAN_TO_STR(pLabel, label)
			pin_ptr<Vector4> pCol = &col;
			Vector4 vRef = {};
			pin_ptr<Vector4> pRef = nullptr;
			if (refCol.HasValue) {
				vRef = refCol.Value;
				pRef = &vRef;
			}
			return ::ImGui::ColorPicker4(pLabel, (float*)pCol, (ImGuiColorEditFlags)flags, (float*)pRef);
		}
		
		virtual bool ColorButton(ReadOnlySpan<byte> descId, System::Numerics::Vector4 col, Tesseract::ImGui::ImGuiColorEditFlags flags, System::Numerics::Vector2 size) {
			IM_SPAN_TO_STR(pDescId, descId)
			return ::ImGui::ColorButton(pDescId, { col.X, col.Y, col.Z, col.W }, (ImGuiColorEditFlags)flags, { size.X, size.Y });
		}
		
		virtual void SetColorEditOptions(Tesseract::ImGui::ImGuiColorEditFlags flags) {
			::ImGui::SetColorEditOptions((ImGuiColorEditFlags)flags);
		}

		// Widgets: Trees
		// - TreeNode functions return true when the node is open, in which case you need to also call TreePop() when you are finished displaying the tree node contents.

		virtual bool TreeNode(ReadOnlySpan<byte> label) {
			IM_SPAN_TO_STR(pLabel, label)
			return ::ImGui::TreeNode(pLabel);
		}

		virtual bool TreeNode(ReadOnlySpan<byte> strID, ReadOnlySpan<byte> fmt) {
			IM_SPAN_TO_STR(pStrID, strID)
			IM_SPAN_TO_STR(pFmt, fmt)
			return ::ImGui::TreeNode(pStrID, "%s", pFmt);
		}

		virtual bool TreeNode(System::IntPtr ptrID, ReadOnlySpan<byte> fmt) {
			IM_SPAN_TO_STR(pFmt, fmt)
			return ::ImGui::TreeNode((void*)ptrID, "%s", pFmt);
		}

		virtual bool TreeNodeEx(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			return ::ImGui::TreeNodeEx(pLabel, (ImGuiTreeNodeFlags)flags);
		}

		virtual bool TreeNodeEx(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiTreeNodeFlags flags, ReadOnlySpan<byte> fmt) {
			IM_SPAN_TO_STR(pStrID, strID)
			IM_SPAN_TO_STR(pFmt, fmt)
			return ::ImGui::TreeNodeEx(pStrID, (ImGuiTreeNodeFlags)flags, pFmt);
		}

		virtual bool TreeNodeEx(System::IntPtr ptrID, Tesseract::ImGui::ImGuiTreeNodeFlags flags, ReadOnlySpan<byte> fmt) {
			IM_SPAN_TO_STR(pFmt, fmt)
			return ::ImGui::TreeNodeEx((void*)ptrID, (ImGuiTreeNodeFlags)flags, pFmt);
		}

		virtual void TreePush(ReadOnlySpan<byte> strID) {
			IM_SPAN_TO_STR(pStrID, strID)
			::ImGui::TreePush(pStrID);
		}

		virtual void TreePush(System::IntPtr ptrID) {
			::ImGui::TreePush((void*)ptrID);
		}

		virtual void TreePop() {
			::ImGui::TreePop();
		}

		virtual property float TreeNodeToLabelSpacing {
			virtual float get() { return ::ImGui::GetTreeNodeToLabelSpacing(); }
		}

		virtual bool CollapsingHeader(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			return ::ImGui::CollapsingHeader(pLabel, (ImGuiTreeNodeFlags)flags);
		}

		virtual bool CollapsingHeader(ReadOnlySpan<byte> label, bool% visible, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			IM_SPAN_TO_STR(pLabel, label)
			pin_ptr<bool> pVisible = &visible;
			return ::ImGui::CollapsingHeader(pLabel, pVisible, (ImGuiTreeNodeFlags)flags);
		}

		virtual void SetNextItemOpen(bool isOpen, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetNextItemOpen(isOpen, (ImGuiCond)cond);
		}

		// Widgets: Selectables
		// - A selectable highlights when hovered, and can display another color when selected.
		// - Neighbors selectable extend their highlight bounds in order to leave no gap between them. This is so a series of selected Selectable appear contiguous.

		virtual bool Selectable(ReadOnlySpan<byte> label, bool selected, Tesseract::ImGui::ImGuiSelectableFlags flags, System::Numerics::Vector2 size) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::Selectable(pLabel, selected, (ImGuiSelectableFlags)flags, { size.X, size.Y });
		}

		virtual bool Selectable(ReadOnlySpan<byte> label, bool% selected, Tesseract::ImGui::ImGuiSelectableFlags flags, System::Numerics::Vector2 size) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<bool> pSelected = &selected;
			return ::ImGui::Selectable(pLabel, pSelected, (ImGuiSelectableFlags)flags, { size.X, size.Y });
		}

		// Widgets: List Boxes
		// - This is essentially a thin wrapper to using BeginChild/EndChild with some stylistic changes.
		// - The BeginListBox()/EndListBox() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() or any items.
		// - The simplified/old ListBox() api are helpers over BeginListBox()/EndListBox() which are kept available for convenience purpose. This is analoguous to how Combos are created.
		// - Choose frame width:   size.x > 0.0f: custom  /  size.x < 0.0f or -FLT_MIN: right-align   /  size.x = 0.0f (default): use current ItemWidth
		// - Choose frame height:  size.y > 0.0f: custom  /  size.y < 0.0f or -FLT_MIN: bottom-align  /  size.y = 0.0f (default): arbitrary default height which can fit ~7 items

		virtual bool BeginListBox(ReadOnlySpan<byte> label, System::Numerics::Vector2 size) {
			IM_SPAN_TO_STR(pLabel, label)
			return ::ImGui::BeginListBox(pLabel, { size.X, size.Y });
		}

		virtual void EndListBox() {
			::ImGui::EndListBox();
		}

		virtual bool ListBox(ReadOnlySpan<byte> label, int% currentItem, System::Collections::Generic::IEnumerable<System::String^>^ items, int heightInItems) {
			IM_SPAN_TO_STR(pLabel, label);
			StringArrayParam pItems(items);
			pin_ptr<int> pCurrentItem = &currentItem;
			return ::ImGui::ListBox(pLabel, pCurrentItem, pItems.data(), (int)pItems.length(), heightInItems);
		}

	private:
		static Tesseract::ImGui::IImGui::ListBoxItemsGetter^ listBoxGetterFn = nullptr;
		static bool listBoxGetterCbk(void*, int idx, const char** pText) {
			String^ text;
			bool retn = listBoxGetterFn->Invoke(idx, text);
			*pText = (g_listBoxText = text).c_str();
			return retn;
		}

	public:
		virtual bool ListBox(ReadOnlySpan<byte> label, int% currentItem, Tesseract::ImGui::IImGui::ListBoxItemsGetter^ itemsGetter, int itemsCount, int heightInItems) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<int> pCurrentItem = &currentItem;
			listBoxGetterFn = itemsGetter;
			return ::ImGui::ListBox(pLabel, pCurrentItem, (bool(*)(void*, int, const char**))listBoxGetterCbk, nullptr, itemsCount, heightInItems);
		}

		// Widgets: Data Plotting
		// - Consider using ImPlot (https://github.com/epezent/implot) which is much better!

	private:
		static System::Func<int, float>^ plotValuesGetterFn = nullptr;
		static float plotValuesGetterCbk(void*, int idx) {
			return plotValuesGetterFn->Invoke(idx);
		}

	public:
		virtual void PlotLines(ReadOnlySpan<byte> label, System::ReadOnlySpan<float> values, int valuesCount, ReadOnlySpan<byte> overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize, int stride) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pOverlayText, overlayText, nullptr)
			pin_ptr<const float> pValues = &MemoryMarshal::GetReference(values);
			::ImGui::PlotLines(pLabel, (const float*)pValues, valuesCount < 0 ? values.Length : valuesCount, 0, pOverlayText, scaleMin, scaleMax, { graphSize.X, graphSize.Y }, stride * sizeof(float));
		}

		virtual void PlotLines(ReadOnlySpan<byte> label, System::Func<int, float>^ valuesGetter, int valuesCount, int valuesOffset, ReadOnlySpan<byte> overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pOverlayText, overlayText, nullptr)
			plotValuesGetterFn = valuesGetter;
			::ImGui::PlotLines(pLabel, (float(*)(void*,int))plotValuesGetterCbk, nullptr, valuesCount, valuesOffset, pOverlayText, scaleMin, scaleMax, { graphSize.X, graphSize.Y });
		}

		virtual void PlotHistogram(ReadOnlySpan<byte> label, System::ReadOnlySpan<float> values, int valuesCount, ReadOnlySpan<byte> overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize, int stride) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pOverlayText, overlayText, nullptr)
			pin_ptr<const float> pValues = &MemoryMarshal::GetReference(values);
			::ImGui::PlotHistogram(pLabel, (const float*)pValues, valuesCount < 0 ? values.Length : valuesCount, 0, pOverlayText, scaleMin, scaleMax, { graphSize.X, graphSize.Y }, stride * sizeof(float));
		}

		virtual void PlotHistogram(ReadOnlySpan<byte> label, System::Func<int, float>^ valuesGetter, int valuesCount, int valuesOffset, ReadOnlySpan<byte> overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pOverlayText, overlayText, nullptr)
			plotValuesGetterFn = valuesGetter;
			::ImGui::PlotHistogram(pLabel, (float(*)(void*, int))plotValuesGetterCbk, nullptr, valuesCount, valuesOffset, pOverlayText, scaleMin, scaleMax, { graphSize.X, graphSize.Y });
		}

		// Widgets: Value() Helpers.
		// - Those are merely shortcut to calling Text() with a format string. Output single value in "name: value" format (tip: freely declare more in your code to handle your types. you can add functions to the ImGui namespace)

		virtual void Value(ReadOnlySpan<byte> prefix, bool b) {
			IM_SPAN_TO_STR(pPrefix, prefix)
			::ImGui::Value(pPrefix, b);
		}

		virtual void Value(ReadOnlySpan<byte> prefix, int v) {
			IM_SPAN_TO_STR(pPrefix, prefix)
			::ImGui::Value(pPrefix, v);
		}

		virtual void Value(ReadOnlySpan<byte> prefix, unsigned int v) {
			IM_SPAN_TO_STR(pPrefix, prefix)
			::ImGui::Value(pPrefix, v);
		}

		virtual void Value(ReadOnlySpan<byte> prefix, float v, ReadOnlySpan<byte> floatFormat) {
			IM_SPAN_TO_STR(pPrefix, prefix);
			IM_SPAN_TO_STR_DEFAULT(pFloatFormat, floatFormat, nullptr);
			::ImGui::Value(pPrefix, v, pFloatFormat);
		}

		// Widgets: Menus
		// - Use BeginMenuBar() on a window ImGuiWindowFlags_MenuBar to append to its menu bar.
		// - Use BeginMainMenuBar() to create a menu bar at the top of the screen and append to it.
		// - Use BeginMenu() to create a menu. You can call BeginMenu() multiple time with the same identifier to append more items to it.
		// - Not that MenuItem() keyboardshortcuts are displayed as a convenience but _not processed_ by Dear ImGui at the moment.

		virtual bool BeginMenuBar() {
			return ::ImGui::BeginMenuBar();
		}

		virtual void EndMenuBar() {
			::ImGui::EndMenuBar();
		}

		virtual bool BeginMainMenuBar() {
			return ::ImGui::BeginMainMenuBar();
		}

		virtual void EndMainMenuBar() {
			::ImGui::EndMainMenuBar();
		}

		virtual bool BeginMenu(ReadOnlySpan<byte> label, bool enabled) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::BeginMenu(pLabel, enabled);
		}

		virtual void EndMenu() {
			::ImGui::EndMenu();
		}

		virtual bool MenuItem(ReadOnlySpan<byte> label, ReadOnlySpan<byte> shortcut, bool selected, bool enabled) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pShortcut, shortcut, nullptr);
			return ::ImGui::MenuItem(pLabel, pShortcut, selected, enabled);
		}

		virtual bool MenuItem(ReadOnlySpan<byte> label, ReadOnlySpan<byte> shortcut, bool% selected, bool enabled) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR_DEFAULT(pShortcut, shortcut, nullptr);
			pin_ptr<bool> pSelected = &selected;
			return ::ImGui::MenuItem(pLabel, pShortcut, pSelected, enabled);
		}

		// Tooltips
		// - Tooltip are windows following the mouse. They do not take focus away.

		virtual void BeginTooltip() {
			::ImGui::BeginTooltip();
		}

		virtual void EndTooltip() {
			::ImGui::EndTooltip();
		}

		virtual void SetTooltip(ReadOnlySpan<byte> text) {
			IM_SPAN_TO_STR(pText, text);
			::ImGui::SetTooltip("%s", pText);
		}

		// Popups, Modals
		//  - They block normal mouse hovering detection (and therefore most mouse interactions) behind them.
		//  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.
		//  - Their visibility state (~bool) is held internally instead of being held by the programmer as we are used to with regular Begin*() calls.
		//  - The 3 properties above are related: we need to retain popup visibility state in the library because popups may be closed as any time.
		//  - You can bypass the hovering restriction by using ImGuiHoveredFlags_AllowWhenBlockedByPopup when calling IsItemHovered() or IsWindowHovered().
		//  - IMPORTANT: Popup identifiers are relative to the current ID stack, so OpenPopup and BeginPopup generally needs to be at the same level of the stack.
		//    This is sometimes leading to confusing mistakes. May rework this in the future.

		// Popups: begin/end functions
		//  - BeginPopup(): query popup state, if open start appending into the window. Call EndPopup() afterwards. ImGuiWindowFlags are forwarded to the window.
		//  - BeginPopupModal(): block every interactions behind the window, cannot be closed by user, add a dimming background, has a title bar.

		virtual bool BeginPopup(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiWindowFlags flags) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::BeginPopup(pStrID, (ImGuiWindowFlags)flags);
		}

		virtual bool BeginPopupModal(ReadOnlySpan<byte> name, bool% open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			IM_SPAN_TO_STR(pName, name);
			pin_ptr<bool> pOpen = &open;
			return ::ImGui::BeginPopupModal(pName, pOpen, (ImGuiWindowFlags)flags);
		}
		
		virtual void EndPopup() {
			::ImGui::EndPopup();
		}

		// Popups: open/close functions
		//  - OpenPopup(): set popup state to open. ImGuiPopupFlags are available for opening options.
		//  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.
		//  - CloseCurrentPopup(): use inside the BeginPopup()/EndPopup() scope to close manually.
		//  - CloseCurrentPopup() is called by default by Selectable()/MenuItem() when activated (FIXME: need some options).
		//  - Use ImGuiPopupFlags_NoOpenOverExistingPopup to avoid opening a popup if there's already one at the same level. This is equivalent to e.g. testing for !IsAnyPopupOpen() prior to OpenPopup().
		//  - Use IsWindowAppearing() after BeginPopup() to tell if a window just opened.
		//  - IMPORTANT: Notice that for OpenPopupOnItemClick() we exceptionally default flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter

		virtual void OpenPopup(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			IM_SPAN_TO_STR(pStrID, strID);
			::ImGui::OpenPopup(pStrID, (ImGuiPopupFlags)popupFlags);
		}

		virtual void OpenPopup(unsigned int id, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			::ImGui::OpenPopup(id, (ImGuiPopupFlags)popupFlags);
		}
		
		virtual void OpenPopupOnItemClick(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			IM_SPAN_TO_STR_DEFAULT(pStrID, strID, nullptr);
			::ImGui::OpenPopupOnItemClick(pStrID, (ImGuiPopupFlags)popupFlags);
		}
		
		virtual void CloseCurrentPopup() {
			::ImGui::CloseCurrentPopup();
		}

		// Popups: open+begin combined functions helpers
		//  - Helpers to do OpenPopup+BeginPopup where the Open action is triggered by e.g. hovering an item and right-clicking.
		//  - They are convenient to easily create context menus, hence the name.
		//  - IMPORTANT: Notice that BeginPopupContextXXX takes ImGuiPopupFlags just like OpenPopup() and unlike BeginPopup(). For full consistency, we may add ImGuiWindowFlags to the BeginPopupContextXXX functions in the future.
		//  - IMPORTANT: Notice that we exceptionally default their flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter, so if you add other flags remember to re-add the ImGuiPopupFlags_MouseButtonRight.

		virtual bool BeginPopupContextItem(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			IM_SPAN_TO_STR_DEFAULT(pStrID, strID, nullptr);
			return ::ImGui::BeginPopupContextItem(pStrID, (ImGuiPopupFlags)popupFlags);
		}

		virtual bool BeginPopupContextWindow(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			IM_SPAN_TO_STR_DEFAULT(pStrID, strID, nullptr);
			return ::ImGui::BeginPopupContextWindow(pStrID, (ImGuiPopupFlags)popupFlags);
		}

		virtual bool BeginPopupContextVoid(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			IM_SPAN_TO_STR_DEFAULT(pStrID, strID, nullptr);
			return ::ImGui::BeginPopupContextVoid(pStrID, (ImGuiPopupFlags)popupFlags);
		}

		// Popups: query functions
		//  - IsPopupOpen(): return true if the popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId: return true if any popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId + ImGuiPopupFlags_AnyPopupLevel: return true if any popup is open.

		virtual bool IsPopupOpen(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiPopupFlags flags) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::IsPopupOpen(pStrID, (ImGuiPopupFlags)flags);
		}

		// Tables
		// - Full-featured replacement for old Columns API.
		// - See Demo->Tables for demo code.
		// - See top of imgui_tables.cpp for general commentary.
		// - See ImGuiTableFlags_ and ImGuiTableColumnFlags_ enums for a description of available flags.
		// The typical call flow is:
		// - 1. Call BeginTable().
		// - 2. Optionally call TableSetupColumn() to submit column name/flags/defaults.
		// - 3. Optionally call TableSetupScrollFreeze() to request scroll freezing of columns/rows.
		// - 4. Optionally call TableHeadersRow() to submit a header row. Names are pulled from TableSetupColumn() data.
		// - 5. Populate contents:
		//    - In most situations you can use TableNextRow() + TableSetColumnIndex(N) to start appending into a column.
		//    - If you are using tables as a sort of grid, where every columns is holding the same type of contents,
		//      you may prefer using TableNextColumn() instead of TableNextRow() + TableSetColumnIndex().
		//      TableNextColumn() will automatically wrap-around into the next row if needed.
		//    - IMPORTANT: Comparatively to the old Columns() API, we need to call TableNextColumn() for the first column!
		//    - Summary of possible call flow:
		//        --------------------------------------------------------------------------------------------------------
		//        TableNextRow() -> TableSetColumnIndex(0) -> Text("Hello 0") -> TableSetColumnIndex(1) -> Text("Hello 1")  // OK
		//        TableNextRow() -> TableNextColumn()      -> Text("Hello 0") -> TableNextColumn()      -> Text("Hello 1")  // OK
		//                          TableNextColumn()      -> Text("Hello 0") -> TableNextColumn()      -> Text("Hello 1")  // OK: TableNextColumn() automatically gets to next row!
		//        TableNextRow()                           -> Text("Hello 0")                                               // Not OK! Missing TableSetColumnIndex() or TableNextColumn()! Text will not appear!
		//        --------------------------------------------------------------------------------------------------------
		// - 5. Call EndTable()

		virtual bool BeginTable(ReadOnlySpan<byte> strID, int column, Tesseract::ImGui::ImGuiTableFlags flags, System::Numerics::Vector2 outerSize, float innerWidth) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::BeginTable(pStrID, column, (ImGuiTableFlags)flags, { outerSize.X, outerSize.Y }, innerWidth);
		}

		virtual void EndTable() {
			::ImGui::EndTable();
		}
		
		virtual void TableNextRow(Tesseract::ImGui::ImGuiTableRowFlags rowFlags, float minRowHeight) {
			::ImGui::TableNextRow((ImGuiTableRowFlags)rowFlags, minRowHeight);
		}
		
		virtual bool TableNextColumn() {
			return ::ImGui::TableNextColumn();
		}
		
		virtual bool TableSetColumnIndex(int columnN) {
			return ::ImGui::TableSetColumnIndex(columnN);
		}

		// Tables: Headers & Columns declaration
		// - Use TableSetupColumn() to specify label, resizing policy, default width/weight, id, various other flags etc.
		// - Use TableHeadersRow() to create a header row and automatically submit a TableHeader() for each column.
		//   Headers are required to perform: reordering, sorting, and opening the context menu.
		//   The context menu can also be made available in columns body using ImGuiTableFlags_ContextMenuInBody.
		// - You may manually submit headers using TableNextRow() + TableHeader() calls, but this is only useful in
		//   some advanced use cases (e.g. adding custom widgets in header row).
		// - Use TableSetupScrollFreeze() to lock columns/rows so they stay visible when scrolled.

		virtual void TableSetupColumn(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTableColumnFlags flags, float initWidthOrWeight, unsigned int userID) {
			IM_SPAN_TO_STR(pLabel, label);
			::ImGui::TableSetupColumn(pLabel, (ImGuiTableColumnFlags)flags, initWidthOrWeight, userID);
		}
		
		virtual void TableSetupScrollFreeze(int cols, int rows) {
			::ImGui::TableSetupScrollFreeze(cols, rows);
		}
		
		virtual void TableHeadersRow() {
			::ImGui::TableHeadersRow();
		}
		
		virtual void TableHeader(ReadOnlySpan<byte> label) {
			IM_SPAN_TO_STR(pLabel, label);
			::ImGui::TableHeader(pLabel);
		}

		// Tables: Sorting
		// - Call TableGetSortSpecs() to retrieve latest sort specs for the table. NULL when not sorting.
		// - When 'SpecsDirty == true' you should sort your data. It will be true when sorting specs have changed
		//   since last call, or the first time. Make sure to set 'SpecsDirty = false' after sorting, else you may
		//   wastefully sort your data every frame!
		// - Lifetime: don't hold on this pointer over multiple frames or past any subsequent call to BeginTable().

		virtual property Tesseract::ImGui::IImGuiTableSortSpecs^ TableSortSpecs {
			virtual Tesseract::ImGui::IImGuiTableSortSpecs^ get() {
				ImGuiTableSortSpecs* specs = ::ImGui::TableGetSortSpecs();
				if (specs) return gcnew ImGuiTableSortSpecsCLI(specs);
				else return nullptr;
			}
		}

		// Tables: Miscellaneous functions
		// - Functions args 'int column_n' treat the default value of -1 as the same as passing the current column index.

		virtual property int TableColumnCount {
			virtual int get() { return ::ImGui::TableGetColumnCount(); }
		}

		virtual property int TableColumnIndex {
			virtual int get() { return ::ImGui::TableGetColumnIndex(); }
		}

		virtual property int TableRowIndex {
			virtual int get() { return ::ImGui::TableGetRowIndex(); }
		}

		virtual System::String^ TableGetColumnName(int columnN) {
			return gcnew String(::ImGui::TableGetColumnName(columnN));
		}
		
		virtual Tesseract::ImGui::ImGuiTableColumnFlags TableGetColumnFlags(int columnN) {
			return (Tesseract::ImGui::ImGuiTableColumnFlags)::ImGui::TableGetColumnFlags(columnN);
		}
		
		virtual void TableSetColumnEnabled(int columnN, bool v) {
			::ImGui::TableSetColumnEnabled(columnN, v);
		}
		
		virtual void TableSetBgColor(Tesseract::ImGui::ImGuiTableBgTarget target, unsigned int color, int columnN) {
			::ImGui::TableSetBgColor((ImGuiTableBgTarget)target, color, columnN);
		}

		// Legacy Columns API (prefer using Tables!)
		// - You can also use SameLine(pos_x) to mimic simplified columns.

		virtual void Columns(int count, ReadOnlySpan<byte> id, bool border) {
			IM_SPAN_TO_STR_DEFAULT(pID, id, nullptr);
			::ImGui::Columns(count, pID, border);
		}

		virtual void NextColumn() {
			::ImGui::NextColumn();
		}

		virtual property int ColumnIndex {
			virtual int get() { return ::ImGui::GetColumnIndex(); }
		}

		virtual float GetColumnWidth(int columnIndex) {
			return ::ImGui::GetColumnWidth(columnIndex);
		}
		
		virtual void SetColumnWidth(int columnIndex, float width) {
			::ImGui::SetColumnWidth(columnIndex, width);
		}
		
		virtual float GetColumnOffset(int columnIndex) {
			return ::ImGui::GetColumnOffset(columnIndex);
		}
		
		virtual void SetColumnOffset(int columnIndex, float width) {
			::ImGui::SetColumnOffset(columnIndex, width);
		}

		virtual property int ColumnsCount {
			virtual int get() { return ::ImGui::GetColumnsCount(); }
		}

		// Tab Bars, Tabs

		virtual bool BeginTabBar(ReadOnlySpan<byte> strID, Tesseract::ImGui::ImGuiTabBarFlags flags) {
			IM_SPAN_TO_STR(pStrID, strID);
			return ::ImGui::BeginTabBar(pStrID, (ImGuiTabBarFlags)flags);
		}
		
		virtual void EndTabBar() {
			::ImGui::EndTabBar();
		}
		
		virtual bool BeginTabItem(ReadOnlySpan<byte> label, bool% open, Tesseract::ImGui::ImGuiTabItemFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<bool> pOpen = &open;
			return ::ImGui::BeginTabItem(pLabel, pOpen, (ImGuiTabItemFlags)flags);
		}
		
		virtual void EndTabItem() {
			::ImGui::EndTabItem();
		}
		
		virtual bool TabItemButton(ReadOnlySpan<byte> label, Tesseract::ImGui::ImGuiTabItemFlags flags) {
			IM_SPAN_TO_STR(pLabel, label);
			return ::ImGui::TabItemButton(pLabel, (ImGuiTabItemFlags)flags);
		}

		virtual void SetTabItemClosed(ReadOnlySpan<byte> tabOrDockedWindowLabel) {
			IM_SPAN_TO_STR(pTabOrDockedWindowLabel, tabOrDockedWindowLabel);
			::ImGui::SetTabItemClosed(pTabOrDockedWindowLabel);
		}

		// Logging/Capture
		// - All text output from the interface can be captured into tty/file/clipboard. By default, tree nodes are automatically opened during logging.

		virtual void LogToTTY(int autoOpenDepth) {
			::ImGui::LogToTTY(autoOpenDepth);
		}
		
		virtual void LogToFile(int autoOpenDepth, System::String^ filename) {
			StringParam pFilename(filename);
			::ImGui::LogToFile(autoOpenDepth, pFilename.c_str());
		}
		
		virtual void LogToClipboard(int autoOpenDepth) {
			::ImGui::LogToClipboard(autoOpenDepth);
		}
		
		virtual void LogFinish() {
			::ImGui::LogFinish();
		}
		
		virtual void LogButtons() {
			::ImGui::LogButtons();
		}
		
		virtual void LogText(System::String^ fmt) {
			StringParam pFmt(fmt);
			::ImGui::LogText("%s", pFmt.c_str());
		}

		// Drag and Drop
		// - On source items, call BeginDragDropSource(), if it returns true also call SetDragDropPayload() + EndDragDropSource().
		// - On target candidates, call BeginDragDropTarget(), if it returns true also call AcceptDragDropPayload() + EndDragDropTarget().
		// - If you stop calling BeginDragDropSource() the payload is preserved however it won't have a preview tooltip (we currently display a fallback "..." tooltip, see #1725)
		// - An item can be both drag source and drop target.

		virtual bool BeginDragDropSource(Tesseract::ImGui::ImGuiDragDropFlags flags) {
			return ::ImGui::BeginDragDropSource((ImGuiDragDropFlags)flags);
		}
		
		virtual bool SetDragDropPayload(System::String^ type, System::ReadOnlySpan<byte> data, Tesseract::ImGui::ImGuiCond cond) {
			StringParam pType(type);
			pin_ptr<byte> pData = &MemoryMarshal::GetReference(data);
			return ::ImGui::SetDragDropPayload(pType.c_str(), pData, data.Length, (ImGuiCond)cond);
		}
		
		virtual void EndDragDropSource() {
			::ImGui::EndDragDropSource();
		}
		
		virtual bool BeginDragDropTarget() {
			return ::ImGui::BeginDragDropTarget();
		}
		
		virtual Tesseract::ImGui::IImGuiPayload^ AcceptDragDropPayload(System::String^ type, Tesseract::ImGui::ImGuiDragDropFlags flags) {
			StringParam pType(type);
			const ImGuiPayload* payload = ::ImGui::AcceptDragDropPayload(pType.c_str(), (ImGuiDragDropFlags)flags);
			if (payload) return gcnew ImGuiPayloadCLI(payload);
			else return nullptr;
		}

		virtual void EndDragDropTarget() {
			::ImGui::EndDragDropTarget();
		}

		virtual property Tesseract::ImGui::IImGuiPayload^ DragDropPayload {
			virtual Tesseract::ImGui::IImGuiPayload^ get() {
				const ImGuiPayload* payload = ::ImGui::GetDragDropPayload();
				if (payload) return gcnew ImGuiPayloadCLI(payload);
				else return nullptr;
			}
		}

		// Disabling [BETA API]
		// - Disable all user interactions and dim items visuals (applying style.DisabledAlpha over current colors)
		// - Those can be nested but it cannot be used to enable an already disabled section (a single BeginDisabled(true) in the stack is enough to keep everything disabled)
		// - BeginDisabled(false) essentially does nothing useful but is provided to facilitate use of boolean expressions. If you can avoid calling BeginDisabled(False)/EndDisabled() best to avoid it.

		virtual void BeginDisabled(bool disabled) {
			::ImGui::BeginDisabled(disabled);
		}

		virtual void EndDisabled() {
			::ImGui::EndDisabled();
		}

		// Clipping
		// - Mouse hovering is affected by ImGui::PushClipRect() calls, unlike direct calls to ImDrawList::PushClipRect() which are render only.

		virtual void PushClipRect(System::Numerics::Vector2 clipRectMin, System::Numerics::Vector2 clipRectMax, bool intersectWithCurrentClipRect) {
			::ImGui::PushClipRect({ clipRectMin.X, clipRectMin.Y }, { clipRectMax.X, clipRectMax.Y }, intersectWithCurrentClipRect);
		}

		virtual void PopClipRect() {
			::ImGui::PopClipRect();
		}

		// Focus, Activation
		// - Prefer using "SetItemDefaultFocus()" over "if (IsWindowAppearing()) SetScrollHereY()" when applicable to signify "this is the default item"

		virtual void SetItemDefaultFocus() {
			::ImGui::SetItemDefaultFocus();
		}

		virtual void SetKeyboardFocusHere(int offset) {
			::ImGui::SetKeyboardFocusHere(offset);
		}

		// Item/Widgets Utilities and Query Functions
		// - Most of the functions are referring to the previous Item that has been submitted.
		// - See Demo Window under "Widgets->Querying Status" for an interactive visualization of most of those functions.

		virtual bool IsItemHovered(Tesseract::ImGui::ImGuiHoveredFlags flags) {
			return ::ImGui::IsItemHovered((ImGuiHoveredFlags)flags);
		}

		virtual property bool IsItemActive {
			virtual bool get() { return ::ImGui::IsItemActive(); }
		}

		virtual property bool IsItemFocused {
			virtual bool get() { return ::ImGui::IsItemFocused(); }
		}

		virtual bool IsItemClicked(Tesseract::ImGui::ImGuiMouseButton mouseButton) {
			return ::ImGui::IsItemClicked((ImGuiMouseButton)mouseButton);
		}

		virtual property bool IsItemVisible {
			virtual bool get() { return ::ImGui::IsItemVisible(); }
		}

		virtual property bool IsItemEdited {
			virtual bool get() { return ::ImGui::IsItemEdited(); }
		}

		virtual property bool IsItemActivated {
			virtual bool get() { return ::ImGui::IsItemActivated(); }
		}

		virtual property bool IsItemDeactivated {
			virtual bool get() { return ::ImGui::IsItemDeactivated(); }
		}

		virtual property bool IsItemDeactivatedAfterEdit {
			virtual bool get() { return ::ImGui::IsItemDeactivatedAfterEdit(); }
		}

		virtual property bool IsItemToggledOpen {
			virtual bool get() { return ::ImGui::IsItemToggledOpen(); }
		}

		virtual property bool IsAnyItemHovered {
			virtual bool get() { return ::ImGui::IsAnyItemHovered(); }
		}

		virtual property bool IsAnyItemActive {
			virtual bool get() { return ::ImGui::IsAnyItemActive(); }
		}

		virtual property bool IsAnyItemFocused {
			virtual bool get() { return ::ImGui::IsAnyItemFocused(); }
		}

		virtual property System::Numerics::Vector2 ItemRectMin {
			virtual Vector2 get() {
				auto value = ::ImGui::GetItemRectMin();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 ItemRectMax {
			virtual Vector2 get() {
				auto value = ::ImGui::GetItemRectMax();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 ItemRectSize {
			virtual Vector2 get() {
				auto value = ::ImGui::GetItemRectSize();
				return Vector2(value.x, value.y);
			}
		}

		virtual void SetItemAllowOverlap() {
			::ImGui::SetItemAllowOverlap();
		}

		// Viewports
		// - Currently represents the Platform Window created by the application which is hosting our Dear ImGui windows.
		// - In 'docking' branch with multi-viewport enabled, we extend this concept to have multiple active viewports.
		// - In the future we will extend this concept further to also represent Platform Monitor and support a "no main platform window" operation mode.

		virtual property Tesseract::ImGui::ImGuiViewport MainViewport {
			virtual Tesseract::ImGui::ImGuiViewport get() {
				ImGuiViewport* viewport = ::ImGui::GetMainViewport();
				Tesseract::ImGui::ImGuiViewport m_viewport = {};
				m_viewport.Flags = (Tesseract::ImGui::ImGuiViewportFlags)viewport->Flags;
				m_viewport.Pos = Vector2(viewport->Pos.x, viewport->Pos.y);
				m_viewport.Size = Vector2(viewport->Size.x, viewport->Size.y);
				m_viewport.WorkPos = Vector2(viewport->WorkPos.x, viewport->WorkPos.y);
				m_viewport.WorkSize = Vector2(viewport->WorkSize.x, viewport->WorkSize.y);
				m_viewport.PlatformHandleRaw = (IntPtr)viewport->PlatformHandleRaw;
				return m_viewport;
			}
			virtual void set(Tesseract::ImGui::ImGuiViewport value) {
				ImGuiViewport* viewport = ::ImGui::GetMainViewport();
				viewport->Flags = (ImGuiViewportFlags)value.Flags;
				viewport->Pos = { value.Pos.X, value.Pos.Y };
				viewport->Size = { value.Size.X, value.Size.Y };
				viewport->WorkPos = { value.WorkPos.X, value.WorkPos.Y };
				viewport->WorkSize = { value.WorkSize.X, value.WorkSize.Y };
				viewport->PlatformHandleRaw = (void*)value.PlatformHandleRaw;
			}
		}

		// Miscellaneous Utilities

		virtual bool IsRectVisible(System::Numerics::Vector2 size) {
			return ::ImGui::IsRectVisible({ size.X, size.Y });
		}

		virtual bool IsRectVisible(System::Numerics::Vector2 rectMin, System::Numerics::Vector2 rectMax) {
			return ::ImGui::IsRectVisible({ rectMin.X, rectMin.Y }, { rectMax.X, rectMax.Y });
		}

		virtual property double Time {
			virtual double get() { return ::ImGui::GetTime(); }
		}

		virtual property int FrameCount {
			virtual int get() { return ::ImGui::GetFrameCount(); }
		}

		virtual property Tesseract::ImGui::IImDrawList^ BackgroundDrawList {
			virtual Tesseract::ImGui::IImDrawList^ get() {
				return gcnew ImDrawListCLI(::ImGui::GetBackgroundDrawList(), false);
			}
		}

		virtual property Tesseract::ImGui::IImDrawList^ ForegroundDrawList {
			virtual Tesseract::ImGui::IImDrawList^ get() {
				return gcnew ImDrawListCLI(::ImGui::GetForegroundDrawList(), false);
			}
		}

		virtual property Tesseract::ImGui::IImDrawListSharedData^ DrawListSharedData {
			virtual Tesseract::ImGui::IImDrawListSharedData^ get() {
				return gcnew ImDrawListSharedDataCLI(::ImGui::GetDrawListSharedData());
			}
		}

		virtual System::String^ GetStyleColorName(Tesseract::ImGui::ImGuiCol idx) {
			return gcnew String(::ImGui::GetStyleColorName((ImGuiCol)idx));
		}

	private:
		ImGuiStorageCLI^ m_statestorage = nullptr;

	public:
		virtual property Tesseract::ImGui::IImGuiStorage^ StateStorage {
			virtual Tesseract::ImGui::IImGuiStorage^ get() {
				if (!m_statestorage) m_statestorage = gcnew ImGuiStorageCLI(::ImGui::GetStateStorage(), false);
				return m_statestorage;
			}
			virtual void set(Tesseract::ImGui::IImGuiStorage^ value) {
				m_statestorage = ((ImGuiStorageCLI^)value);
				::ImGui::SetStateStorage(m_statestorage->m_storage);
			}
		}

		virtual bool BeginChildFrame(unsigned int id, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiWindowFlags flags) {
			return ::ImGui::BeginChildFrame(id, { size.X, size.Y }, (ImGuiWindowFlags)flags);
		}

		// Text Utilities

		virtual System::Numerics::Vector2 CalcTextSize(ReadOnlySpan<byte> text, bool hideTextAfterDoubleHash, float wrapWidth) {
			IM_SPAN_TO_STR(pText, text);
			auto retn = ::ImGui::CalcTextSize(pText, pText + text.Length, hideTextAfterDoubleHash, wrapWidth);
			return Vector2(retn.x, retn.y);
		}

		// Inputs Utilities: Keyboard
		// Without IMGUI_DISABLE_OBSOLETE_KEYIO: (legacy support)
		//   - For 'ImGuiKey key' you can still use your legacy native/user indices according to how your backend/engine stored them in io.KeysDown[].
		// With IMGUI_DISABLE_OBSOLETE_KEYIO: (this is the way forward)
		//   - Any use of 'ImGuiKey' will assert when key < 512 will be passed, previously reserved as native/user keys indices
		//   - GetKeyIndex() is pass-through and therefore deprecated (gone if IMGUI_DISABLE_OBSOLETE_KEYIO is defined)

		virtual bool IsKeyDown(Tesseract::ImGui::ImGuiKey key) {
			return ::ImGui::IsKeyDown((ImGuiKey)key);
		}

		virtual bool IsKeyPressed(Tesseract::ImGui::ImGuiKey key, bool repeat) {
			return ::ImGui::IsKeyPressed((ImGuiKey)repeat);
		}

		virtual bool IsKeyReleased(Tesseract::ImGui::ImGuiKey key) {
			return ::ImGui::IsKeyReleased((ImGuiKey)key);
		}

		virtual int GetKeyPressedAmount(Tesseract::ImGui::ImGuiKey key, float repeatDelay, float rate) {
			return ::ImGui::GetKeyPressedAmount((ImGuiKey)key, repeatDelay, rate);
		}

		virtual System::String^ GetKeyName(Tesseract::ImGui::ImGuiKey key) {
			return gcnew String(::ImGui::GetKeyName((ImGuiKey)key));
		}

		virtual void CaptureKeyboardFromApp(bool wantCaptureKeyboardValue) {
			::ImGui::CaptureKeyboardFromApp(wantCaptureKeyboardValue);
		}

		// Inputs Utilities: Mouse
		// - To refer to a mouse button, you may use named enums in your code e.g. ImGuiMouseButton_Left, ImGuiMouseButton_Right.
		// - You can also use regular integer: it is forever guaranteed that 0=Left, 1=Right, 2=Middle.
		// - Dragging operations are only reported after mouse has moved a certain distance away from the initial clicking position (see 'lock_threshold' and 'io.MouseDraggingThreshold')

		virtual bool IsMouseDown(Tesseract::ImGui::ImGuiMouseButton button) {
			return ::ImGui::IsMouseDown((ImGuiMouseButton)button);
		}

		virtual bool IsMouseClicked(Tesseract::ImGui::ImGuiMouseButton button, bool repeat) {
			return ::ImGui::IsMouseClicked((ImGuiMouseButton)button, repeat);
		}

		virtual bool IsMouseReleased(Tesseract::ImGui::ImGuiMouseButton button) {
			return ::ImGui::IsMouseReleased((ImGuiMouseButton)button);
		}

		virtual bool IsMouseDoubleClicked(Tesseract::ImGui::ImGuiMouseButton button) {
			return ::ImGui::IsMouseDoubleClicked((ImGuiMouseButton)button);
		}

		virtual int GetMouseClickedCount(Tesseract::ImGui::ImGuiMouseButton button) {
			return ::ImGui::GetMouseClickedCount((ImGuiMouseButton)button);
		}

		virtual bool IsMouseHoveringRect(System::Numerics::Vector2 rMin, System::Numerics::Vector2 rMax, bool clip) {
			return ::ImGui::IsMouseHoveringRect({ rMin.X, rMin.Y }, { rMax.X, rMax.Y }, clip);
		}

		virtual bool IsMousePosValid(System::Nullable<System::Numerics::Vector2> mousePos) {
			ImVec2 v, *pv = nullptr;
			if (mousePos.HasValue) {
				auto vMousePos = mousePos.Value;
				v = { vMousePos.X, vMousePos.Y };
				pv = &v;
			}
			return ::ImGui::IsMousePosValid(pv);
		}

		virtual property bool IsAnyMouseDown {
			virtual bool get() { return ::ImGui::IsAnyMouseDown(); }
		}

		virtual property System::Numerics::Vector2 MousePos {
			virtual Vector2 get() {
				auto value = ::ImGui::GetMousePos();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 MousePosOnOpeningCurrentPopup {
			virtual Vector2 get() {
				auto value = ::ImGui::GetMousePosOnOpeningCurrentPopup();
				return Vector2(value.x, value.y);
			}
		}

		virtual bool IsMouseDragging(Tesseract::ImGui::ImGuiMouseButton button, float lockThreshold) {
			return ::ImGui::IsMouseDragging((ImGuiMouseButton)button, lockThreshold);
		}

		virtual System::Numerics::Vector2 GetMouseDragDelta(Tesseract::ImGui::ImGuiMouseButton button, float lockThreshold) {
			auto retn = ::ImGui::GetMouseDragDelta((ImGuiMouseButton)button, lockThreshold);
			return Vector2(retn.x, retn.y);
		}

		virtual void ResetMouseDragDelta(Tesseract::ImGui::ImGuiMouseButton button) {
			::ImGui::ResetMouseDragDelta((ImGuiMouseButton)button);
		}

		virtual property Tesseract::ImGui::ImGuiMouseCursor MouseCursor {
			virtual Tesseract::ImGui::ImGuiMouseCursor get() { return (Tesseract::ImGui::ImGuiMouseCursor)::ImGui::GetMouseCursor(); }
			virtual void set(Tesseract::ImGui::ImGuiMouseCursor value) { ::ImGui::SetMouseCursor((ImGuiMouseCursor)value); }
		}

		virtual void CaptureMouseFromApp(bool wantCaptureMouseValue) {
			::ImGui::CaptureMouseFromApp(wantCaptureMouseValue);
		}

		// Clipboard Utilities
		// - Also see the LogToClipboard() function to capture GUI into clipboard, or easily output text data to the clipboard.

		virtual property System::String^ ClipboardText {
			virtual String^ get() {
				return gcnew String(::ImGui::GetClipboardText());
			}
			virtual void set(System::String^ value) {
				StringParam p_value(value);
				::ImGui::SetClipboardText(p_value.c_str());
			}
		}

		// Settings/.Ini Utilities
		// - The disk functions are automatically called if io.IniFilename != NULL (default is "imgui.ini").
		// - Set io.IniFilename to NULL to load/save manually. Read io.WantSaveIniSettings description about handling .ini saving manually.
		// - Important: default value "imgui.ini" is relative to current working dir! Most apps will want to lock this to an absolute path (e.g. same path as executables).

		virtual void LoadIniSettingsFromDisk(System::String^ iniFilename) {
			StringParam pIniFilename(iniFilename);
			::ImGui::LoadIniSettingsFromDisk(pIniFilename.c_str());
		}

		virtual void LoadIniSettingsFromMemory(System::ReadOnlySpan<byte> iniData) {
			pin_ptr<const byte> pIniData = &MemoryMarshal::GetReference(iniData);
			::ImGui::LoadIniSettingsFromMemory((const char*)pIniData, iniData.Length);
		}

		virtual void SaveIniSettingsToDisk(System::String^ iniFilename) {
			StringParam pIniFilename(iniFilename);
			::ImGui::SaveIniSettingsToDisk(pIniFilename.c_str());
		}

		virtual System::ReadOnlySpan<byte> SaveIniSettingsToMemory() {
			size_t size = 0;
			const char* pData = ::ImGui::SaveIniSettingsToMemory(&size);
			return ReadOnlySpan<byte>((void*)pData, (int)size);
		}

	};

	void g_customDrawCallback(const ImDrawList* parentList, const ImDrawCmd* cmd) {
		Tesseract::ImGui::ImDrawCallback^ cbk = DrawCallbackHolder::Instance->Get(cmd->UserCallbackData);
		if (cbk) {
			cbk->Invoke(gcnew ImDrawListCLI(const_cast<ImDrawList*>(parentList), false), ImDrawListCLI::ConvertCmd(*cmd));
		}
	}

}}}
