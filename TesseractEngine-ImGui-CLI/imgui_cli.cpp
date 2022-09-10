#include "imgui.h"
#include "imgui_cli.h"

using namespace Tesseract::Core::Numerics;
using namespace Tesseract::Core::Native;
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui {

	ref class ImFontAtlasCLI;

	// Globals that CLI doesn't like being in managed types
	StringParam g_comboGetterText;
	StringParam g_listBoxText;

	ref class DrawCallbackHolder {
	public:
		static initonly DrawCallbackHolder^ Instance = gcnew DrawCallbackHolder();

		List<Tesseract::ImGui::ImDrawCallback^>^ m_callbacks = gcnew List<Tesseract::ImGui::ImDrawCallback^>();
		Dictionary<Tesseract::ImGui::ImDrawCallback^, UIntPtr>^ m_lookups = gcnew Dictionary<Tesseract::ImGui::ImDrawCallback^, UIntPtr>();

		void* Register(Tesseract::ImGui::ImDrawCallback^ cbk) {
			UIntPtr value;
			if (m_lookups->TryGetValue(cbk, value)) return (void*)value;
			m_callbacks->Add(cbk);
			uintptr_t idx = m_callbacks->Count;
			m_lookups->Add(cbk, (UIntPtr)idx);
			return (void*)idx;
		}

		Tesseract::ImGui::ImDrawCallback^ Get(void* ptr) {
			if (ptr == nullptr) return nullptr;
			uintptr_t iptr = (uintptr_t)ptr;
			if (iptr <= m_callbacks->Count) return m_callbacks[(int)(iptr - 1)];
			else return nullptr;
		}

		void Clear() {
			m_callbacks->Clear();
			m_lookups->Clear();
		}
	};

	void g_customDrawCallback(const ImDrawList* parentList, const ImDrawCmd* cmd);

	public ref class ImGuiContextCLI : Tesseract::ImGui::IImGuiContext {
	internal:
		ImGuiContext* m_context;

		ImGuiContextCLI(ImGuiContext* ctx) : m_context(ctx) {}
	};

	public ref class ImGuiStyleCLI : Tesseract::ImGui::IImGuiStyle {
	internal:
		ImGuiStyle* m_style;
		bool m_allocd;

		ImGuiStyleCLI() {
			m_style = new ImGuiStyle();
			m_allocd = true;
		}

		ImGuiStyleCLI(ImGuiStyle* style) : m_style(style), m_allocd(false) {}

		~ImGuiStyleCLI() {
			if (m_allocd) delete m_style;
		}

	public:
		virtual property float Alpha {
			virtual float get() { return m_style->Alpha; }
			virtual void set(float value) { m_style->Alpha = value; }
		}

		virtual property float DisabledAlpha {
			virtual float get() { return m_style->DisabledAlpha; }
			virtual void set(float value) { m_style->DisabledAlpha = value; }
		}

		virtual property System::Numerics::Vector2 WindowPadding {
			virtual Vector2 get() { return Vector2(m_style->WindowPadding.x, m_style->WindowPadding.y); }
			virtual void set(Vector2 value) { m_style->WindowPadding = { value.X, value.Y }; }
		}

		virtual property float WindowRounding {
			virtual float get() { return m_style->WindowRounding; }
			virtual void set(float value) { m_style->WindowRounding = value; }
		}

		virtual property float WindowBorderSize {
			virtual float get() { return m_style->WindowBorderSize; }
			virtual void set(float value) { m_style->WindowBorderSize = value; }
		}

		virtual property System::Numerics::Vector2 WindowMinSize {
			virtual Vector2 get() { return Vector2(m_style->WindowMinSize.x, m_style->WindowMinSize.y); }
			virtual void set(Vector2 value) { m_style->WindowMinSize = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 WindowTitleAlign {
			virtual Vector2 get() { return Vector2(m_style->WindowTitleAlign.x, m_style->WindowTitleAlign.y); }
			virtual void set(Vector2 value) { m_style->WindowTitleAlign = { value.X, value.Y }; }
		}

		virtual property Tesseract::ImGui::ImGuiDir WindowMenuButtonPosition {
			virtual Tesseract::ImGui::ImGuiDir get() { return (Tesseract::ImGui::ImGuiDir)m_style->WindowMenuButtonPosition; }
			virtual void set(Tesseract::ImGui::ImGuiDir value) { m_style->WindowMenuButtonPosition = (ImGuiDir)value; }
		}

		virtual property float ChildRounding {
			virtual float get() { return m_style->ChildRounding; }
			virtual void set(float value) { m_style->ChildRounding = value; }
		}

		virtual property float ChildBorderSize {
			virtual float get() { return m_style->ChildBorderSize; }
			virtual void set(float value) { m_style->ChildBorderSize = value; }
		}

		virtual property float PopupRounding {
			virtual float get() { return m_style->PopupRounding; }
			virtual void set(float value) { m_style->PopupRounding = value; }
		}

		virtual property float PopupBorderSize {
			virtual float get() { return m_style->PopupBorderSize; }
			virtual void set(float value) { m_style->PopupBorderSize = value; }
		}

		virtual property System::Numerics::Vector2 FramePadding {
			virtual Vector2 get() { return Vector2(m_style->FramePadding.x, m_style->FramePadding.y); }
			virtual void set(Vector2 value) { m_style->FramePadding = { value.X, value.Y }; }
		}

		virtual property float FrameRounding {
			virtual float get() { return m_style->FrameRounding; }
			virtual void set(float value) { m_style->FrameRounding = value; }
		}

		virtual property float FrameBorderSize {
			virtual float get() { return m_style->FrameBorderSize; }
			virtual void set(float value) { m_style->FrameBorderSize = value; }
		}

		virtual property System::Numerics::Vector2 ItemSpacing {
			virtual Vector2 get() { return Vector2(m_style->ItemSpacing.x, m_style->ItemSpacing.y); }
			virtual void set(Vector2 value) { m_style->ItemSpacing = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 ItemInnerSpacing {
			virtual Vector2 get() { return Vector2(m_style->ItemInnerSpacing.x, m_style->ItemInnerSpacing.y); }
			virtual void set(Vector2 value) { m_style->ItemInnerSpacing = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 CellPadding {
			virtual Vector2 get() { return Vector2(m_style->CellPadding.x, m_style->CellPadding.y); }
			virtual void set(Vector2 value) { m_style->CellPadding = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 TouchExtraPadding {
			virtual Vector2 get() { return Vector2(m_style->TouchExtraPadding.x, m_style->TouchExtraPadding.y); }
			virtual void set(Vector2 value) { m_style->TouchExtraPadding = { value.X, value.Y }; }
		}

		virtual property float IndentSpacing {
			virtual float get() { return m_style->IndentSpacing; }
			virtual void set(float value) { m_style->IndentSpacing = value; }
		}

		virtual property float ColumnsMinSpacing {
			virtual float get() { return m_style->ColumnsMinSpacing; }
			virtual void set(float value) { m_style->ColumnsMinSpacing = value; }
		}

		virtual property float ScrollbarSize {
			virtual float get() { return m_style->ScrollbarSize; }
			virtual void set(float value) { m_style->ScrollbarSize = value; }
		}

		virtual property float ScrollbarRounding {
			virtual float get() { return m_style->ScrollbarRounding; }
			virtual void set(float value) { m_style->ScrollbarRounding = value; }
		}

		virtual property float GrabMinSize {
			virtual float get() { return m_style->GrabMinSize; }
			virtual void set(float value) { m_style->GrabMinSize = value; }
		}

		virtual property float GrabRounding {
			virtual float get() { return m_style->GrabRounding; }
			virtual void set(float value) { m_style->GrabRounding = value; }
		}

		virtual property float LogSliderDeadzone {
			virtual float get() { return m_style->LogSliderDeadzone; }
			virtual void set(float value) { m_style->LogSliderDeadzone = value; }
		}

		virtual property float TabRounding {
			virtual float get() { return m_style->TabRounding; }
			virtual void set(float value) { m_style->TabRounding = value; }
		}

		virtual property float TabBorderSize {
			virtual float get() { return m_style->TabBorderSize; }
			virtual void set(float value) { m_style->TabBorderSize = value; }
		}

		virtual property float TabMinWidthForCloseButton {
			virtual float get() { return m_style->TabMinWidthForCloseButton; }
			virtual void set(float value) { m_style->TabMinWidthForCloseButton = value; }
		}

		virtual property Tesseract::ImGui::ImGuiDir ColorButtonPosition {
			virtual Tesseract::ImGui::ImGuiDir get() { return (Tesseract::ImGui::ImGuiDir)m_style->ColorButtonPosition; }
			virtual void set(Tesseract::ImGui::ImGuiDir value) { m_style->ColorButtonPosition = (ImGuiDir)value; }
		}

		virtual property System::Numerics::Vector2 ButtonTextAlign {
			virtual Vector2 get() { return Vector2(m_style->ButtonTextAlign.x, m_style->ButtonTextAlign.y); }
			virtual void set(Vector2 value) { m_style->ButtonTextAlign = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 SelectableTextAlign {
			virtual Vector2 get() { return Vector2(m_style->SelectableTextAlign.x, m_style->SelectableTextAlign.y); }
			virtual void set(Vector2 value) { m_style->SelectableTextAlign = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 DisplayWindowPadding {
			virtual Vector2 get() { return Vector2(m_style->DisplayWindowPadding.x, m_style->DisplayWindowPadding.y); }
			virtual void set(Vector2 value) { m_style->DisplayWindowPadding = { value.X, value.Y }; }
		}

		virtual property System::Numerics::Vector2 DisplaySafeAreaPadding {
			virtual Vector2 get() { return Vector2(m_style->DisplaySafeAreaPadding.x, m_style->DisplaySafeAreaPadding.y); }
			virtual void set(Vector2 value) { m_style->DisplaySafeAreaPadding = { value.X, value.Y }; }
		}

		virtual property float MouseCursorScale {
			virtual float get() { return m_style->MouseCursorScale; }
			virtual void set(float value) { m_style->MouseCursorScale = value; }
		}

		virtual property bool AntiAliasedLines {
			virtual bool get() { return m_style->AntiAliasedLines; }
			virtual void set(bool value) { m_style->AntiAliasedLines = value; }
		}

		virtual property bool AntiAliasedLinesUseTex {
			virtual bool get() { return m_style->AntiAliasedLinesUseTex; }
			virtual void set(bool value) { m_style->AntiAliasedLinesUseTex = value; }
		}

		virtual property bool AntiAliasedFill {
			virtual bool get() { return m_style->AntiAliasedFill; }
			virtual void set(bool value) { m_style->AntiAliasedFill = value; }
		}

		virtual property float CurveTessellationTol {
			virtual float get() { return m_style->CurveTessellationTol; }
			virtual void set(float value) { m_style->CurveTessellationTol = value; }
		}

		virtual property float CircleTessellationMaxError {
			virtual float get() { return m_style->CircleTessellationMaxError; }
			virtual void set(float value) { m_style->CircleTessellationMaxError = value; }
		}

		virtual property System::Span<System::Numerics::Vector4> Colors {
			virtual Span<Vector4> get() {
				return Span<Vector4>((void*)m_style->Colors, sizeof(m_style->Colors) / sizeof(ImVec4));
			}
		}

		virtual void ScaleAllSizes(float scaleFactor) {
			m_style->ScaleAllSizes(scaleFactor);
		}

	};

	public ref class ImDrawListCLI : public Tesseract::ImGui::IImDrawList {
	internal:
		ref class CmdBufferImpl : Tesseract::Core::Utilities::CLI::ListBase<Tesseract::ImGui::ImDrawCmd> {
		internal:
			ImDrawListCLI^ m_drawlist;
			ImVector<ImDrawCmd>* m_vec;

			CmdBufferImpl(ImDrawListCLI^ drawlist) : m_drawlist(drawlist), m_vec(&drawlist->m_drawlist->CmdBuffer) {}

			ImDrawCmd* Find(Tesseract::ImGui::ImDrawCmd item) {
				ImDrawCmd nitem = ConvertCmd(item);
				ImDrawCmd* itr = m_vec->begin();
				while (itr != m_vec->end()) {
					if (!memcmp(itr, &nitem, sizeof(ImDrawCmd)))
						break;
				}
				return itr;
			}

		public:
			virtual property int Count {
				virtual int get() override {
					return m_vec->Size;
				}
			}

			virtual Tesseract::ImGui::ImDrawCmd Get(int index) override {
				return ConvertCmd(m_vec->operator[](index));
			}

			virtual void Set(int index, Tesseract::ImGui::ImDrawCmd value) override {
				m_vec->operator[](index) = ConvertCmd(value);
			}

			virtual void Add(Tesseract::ImGui::ImDrawCmd item) override {
				m_vec->push_back(ConvertCmd(item));
			}

			virtual void Clear() override {
				m_vec->clear();
			}

			virtual int IndexOf(Tesseract::ImGui::ImDrawCmd item) override {
				auto itr = Find(item);
				if (itr == m_vec->end()) return -1;
				else return (int)(itr - m_vec->begin());
			}

			virtual void Insert(int index, Tesseract::ImGui::ImDrawCmd item) override {
				m_vec->insert(m_vec->begin() + index, ConvertCmd(item));
			}

			virtual void RemoveAt(int index) override {
				m_vec->erase(m_vec->begin() + index);
			}

		};

		ref class IdxBufferImpl : Tesseract::ImGui::IImVector<unsigned short> {
		internal:
			ImDrawListCLI^ m_drawlist;
			ImVector<unsigned short>* m_vec;

			IdxBufferImpl(ImDrawListCLI^ drawlist) : m_drawlist(drawlist) {
				m_vec = &drawlist->m_drawlist->IdxBuffer;
			}

		public:
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return (System::Collections::IEnumerator^)GetEnumerator();
			}

			virtual System::Collections::Generic::IEnumerator<unsigned short>^ GetEnumerator() {
				return gcnew Tesseract::Core::Utilities::CLI::ListEnumerator<unsigned short>((IReadOnlyList<unsigned short>^)this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}
			
			virtual property bool IsReadOnly {
				virtual bool get() { return false; }
			}

			virtual void Add(unsigned short item) {
				m_vec->push_back(item);
			}

			virtual void Clear() {
				m_vec->clear();
			}

			virtual bool Contains(unsigned short item) {
				return m_vec->find(item) != m_vec->end();
			}

			virtual void CopyTo(array<unsigned short, 1>^ arr, int arrayIndex) {
				int len = Math::Min(arr->Length - arrayIndex, Count);
				if (len > 1) {
					pin_ptr<unsigned short> pArr = &arr[arrayIndex];
					memcpy(pArr, m_vec->Data, len * sizeof(unsigned short));
				}
			}

			virtual bool Remove(unsigned short item) {
				auto itr = m_vec->find(item);
				if (itr != m_vec->end()) {
					m_vec->erase(itr);
					return true;
				} else return false;
			}

			virtual property unsigned short default[int] {
				virtual unsigned short get(int index) {
					return m_vec->Data[index];
				}
				virtual void set(int index, unsigned short value) {
					m_vec->Data[index] = value;
				}
			}

			virtual int IndexOf(unsigned short item) {
				auto itr = m_vec->find(item);
				return itr != m_vec->end() ? (int)(itr - m_vec->begin()) : -1;
			}
			
			virtual void Insert(int index, unsigned short item) {
				m_vec->insert(m_vec->begin() + index, item);
			}

			virtual void RemoveAt(int index) {
				m_vec->erase(m_vec->begin() + index);
			}

			virtual System::Span<unsigned short> AsSpan() {
				return System::Span<unsigned short>(m_vec->Data, Count);
			}

			virtual void Resize(int newSize) {
				m_vec->resize(newSize);
			}
		};

		ref class VtxBufferImpl : Tesseract::ImGui::IImVector<Tesseract::ImGui::ImDrawVert> {
		internal:
			ImDrawListCLI^ m_drawlist;
			ImVector<ImDrawVert>* m_vec;

			VtxBufferImpl(ImDrawListCLI^ drawlist) : m_drawlist(drawlist) {
				m_vec = &drawlist->m_drawlist->VtxBuffer;
			}

			static ImDrawVert ConvertVtx(Tesseract::ImGui::ImDrawVert vtx) {
				ImDrawVert nvtx = {};
				nvtx.pos = { vtx.Pos.X, vtx.Pos.Y };
				nvtx.uv = { vtx.UV.X, vtx.UV.Y };
				nvtx.col = vtx.Col;
				return nvtx;
			}

			static Tesseract::ImGui::ImDrawVert ConvertVtx(ImDrawVert vtx) {
				Tesseract::ImGui::ImDrawVert mvtx = {};
				mvtx.Pos = Vector2(vtx.pos.x, vtx.pos.y);
				mvtx.UV = Vector2(vtx.uv.x, vtx.uv.y);
				mvtx.Col = vtx.col;
				return mvtx;
			}

			ImDrawVert* Find(Tesseract::ImGui::ImDrawVert vtx) {
				ImDrawVert nvtx = ConvertVtx(vtx);
				ImDrawVert* itr = m_vec->begin();
				while (itr != m_vec->end()) {
					if (!memcmp(itr, &nvtx, sizeof(ImDrawVert)))
						break;
				}
				return itr;
			}

		public:
			virtual System::Collections::IEnumerator^ GetEnumeratorBase() = System::Collections::IEnumerable::GetEnumerator{
				return GetEnumerator();
			}

			virtual System::Collections::Generic::IEnumerator<Tesseract::ImGui::ImDrawVert>^ GetEnumerator() {
				return gcnew Tesseract::Core::Utilities::CLI::ListEnumerator<Tesseract::ImGui::ImDrawVert>((IReadOnlyList<Tesseract::ImGui::ImDrawVert>^)this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}
			
			virtual property bool IsReadOnly {
				virtual bool get() { return false; }
			}

			virtual void Add(Tesseract::ImGui::ImDrawVert item) {
				m_vec->push_back(ConvertVtx(item));
			}
			
			virtual void Clear() {
				m_vec->clear();
			}
			
			virtual bool Contains(Tesseract::ImGui::ImDrawVert item) {
				return Find(item) != m_vec->end();
			}
			
			virtual void CopyTo(array<Tesseract::ImGui::ImDrawVert, 1>^ arr, int arrayIndex) {
				int len = Math::Min(m_vec->Size, arr->Length - arrayIndex);
				pin_ptr<Tesseract::ImGui::ImDrawVert> pArr = &arr[arrayIndex];
				memcpy(pArr, m_vec->Data, len * sizeof(ImDrawVert));
			}
			
			virtual bool Remove(Tesseract::ImGui::ImDrawVert item) {
				auto itr = Find(item);
				if (itr) {
					m_vec->erase(itr);
					return true;
				} else return false;
			}

			virtual property Tesseract::ImGui::ImDrawVert default[int]{
				virtual Tesseract::ImGui::ImDrawVert get(int index) {
					return ConvertVtx(m_vec->operator[](index));
				}
				virtual void set(int index, Tesseract::ImGui::ImDrawVert value) {
					m_vec->operator[](index) = ConvertVtx(value);
				}
			}

			virtual int IndexOf(Tesseract::ImGui::ImDrawVert item) {
				auto itr = Find(item);
				return itr != m_vec->end() ? (int)(itr - m_vec->begin()) : -1;
			}
			
			virtual void Insert(int index, Tesseract::ImGui::ImDrawVert item) {
				m_vec->insert(m_vec->begin() + index, ConvertVtx(item));
			}
			
			virtual void RemoveAt(int index) {
				m_vec->erase(m_vec->begin() + index);
			}
			
			virtual System::Span<Tesseract::ImGui::ImDrawVert> AsSpan() {
				return System::Span<Tesseract::ImGui::ImDrawVert>(m_vec->Data, m_vec->Size);
			}

			virtual void Resize(int newSize) {
				m_vec->resize(newSize);
			}
		};

		ImDrawList* m_drawlist;
		bool m_allocd;
		CmdBufferImpl^ m_cmdbuffer;
		IdxBufferImpl^ m_idxbuffer;
		VtxBufferImpl^ m_vtxbuffer;

		ImDrawListCLI(ImDrawList* drawlist, bool allocd) : m_drawlist(drawlist), m_allocd(allocd) {
			m_cmdbuffer = gcnew CmdBufferImpl(this);
			m_idxbuffer = gcnew IdxBufferImpl(this);
			m_vtxbuffer = gcnew VtxBufferImpl(this);
		}

		~ImDrawListCLI() {
			if (m_allocd) delete m_drawlist;
		}

		static ImDrawCmd ConvertCmd(Tesseract::ImGui::ImDrawCmd cmd) {
			ImDrawCmd ncmd = {};
			ncmd.ClipRect = { cmd.ClipRect.X, cmd.ClipRect.Y, cmd.ClipRect.Z, cmd.ClipRect.W };
			ncmd.TextureId = (void*)cmd.TextureID;
			ncmd.VtxOffset = cmd.VtxOffset;
			ncmd.IdxOffset = cmd.IdxOffset;
			ncmd.ElemCount = cmd.ElemCount;
			if (cmd.UserCallback) {
				if (cmd.UserCallback == Tesseract::ImGui::GImGui::ResetRenderState) {
					ncmd.UserCallback = ImDrawCallback_ResetRenderState;
				} else {
					ncmd.UserCallback = g_customDrawCallback;
					ncmd.UserCallbackData = DrawCallbackHolder::Instance->Register(cmd.UserCallback);
				}
			}
			return ncmd;
		}

		static Tesseract::ImGui::ImDrawCmd ConvertCmd(ImDrawCmd cmd) {
			Tesseract::ImGui::ImDrawCmd mcmd = {};
			mcmd.ClipRect = Vector4(cmd.ClipRect.x, cmd.ClipRect.y, cmd.ClipRect.z, cmd.ClipRect.w);
			mcmd.TextureID = (UIntPtr)cmd.TextureId;
			mcmd.VtxOffset = cmd.VtxOffset;
			mcmd.IdxOffset = cmd.IdxOffset;
			mcmd.ElemCount = cmd.ElemCount;
			if (cmd.UserCallback) {
				if (cmd.UserCallback == g_customDrawCallback) {
					mcmd.UserCallback = DrawCallbackHolder::Instance->Get(cmd.UserCallbackData);
				} else if (cmd.UserCallback == ImDrawCallback_ResetRenderState) {
					mcmd.UserCallback = Tesseract::ImGui::GImGui::ResetRenderState;
				}
			}
			return mcmd;
		}

		
	public:
		virtual property System::Collections::Generic::IList<Tesseract::ImGui::ImDrawCmd>^ CmdBuffer {
			virtual IList<Tesseract::ImGui::ImDrawCmd>^ get() {
				return m_cmdbuffer;
			}
		}

		virtual property Tesseract::ImGui::IImVector<unsigned short>^ IdxBuffer {
			virtual Tesseract::ImGui::IImVector<unsigned short>^ get() {
				return m_idxbuffer;
			}
		}

		virtual property Tesseract::ImGui::IImVector<Tesseract::ImGui::ImDrawVert>^ VtxBuffer {
			virtual Tesseract::ImGui::IImVector<Tesseract::ImGui::ImDrawVert>^ get() {
				return m_vtxbuffer;
			}
		}

		virtual property Tesseract::ImGui::ImDrawListFlags Flags {
			virtual Tesseract::ImGui::ImDrawListFlags get() {
				return (Tesseract::ImGui::ImDrawListFlags)m_drawlist->Flags;
			}
			virtual void set(Tesseract::ImGui::ImDrawListFlags value) {
				m_drawlist->Flags = (ImDrawListFlags)value;
			}
		}

		virtual property System::Numerics::Vector2 ClipRectMin {
			virtual Vector2 get() {
				auto value = m_drawlist->GetClipRectMin();
				return Vector2(value.x, value.y);
			}
		}

		virtual property System::Numerics::Vector2 ClipRectMax {
			virtual Vector2 get() {
				auto value = m_drawlist->GetClipRectMax();
				return Vector2(value.x, value.y);
			}
		}

		virtual void PushClipRect(System::Numerics::Vector2 clipRectMin, System::Numerics::Vector2 clipRectMax, bool intersectWithCurrentClipRect) {
			m_drawlist->PushClipRect({ clipRectMin.X, clipRectMin.Y }, { clipRectMax.X, clipRectMax.Y }, intersectWithCurrentClipRect);
		}

		virtual void PushClipRectFullScreen() {
			m_drawlist->PushClipRectFullScreen();
		}

		virtual void PopClipRect() {
			m_drawlist->PopClipRect();
		}

		virtual void PushTextureID(System::UIntPtr textureID) {
			m_drawlist->PushTextureID((void*)textureID);
		}

		virtual void PopTextureID() {
			m_drawlist->PopTextureID();
		}

		virtual void AddLine(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, unsigned int col, float thickness) {
			m_drawlist->AddLine({ p1.X, p1.Y }, { p2.X, p2.Y }, col, thickness);
		}

		virtual void AddRect(System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, unsigned int col, float rounding, Tesseract::ImGui::ImDrawFlags flags, float thickness) {
			m_drawlist->AddRect({ pMin.X, pMin.Y }, { pMax.X, pMax.Y }, col, rounding, (ImDrawFlags)flags, thickness);
		}

		virtual void AddRectFilled(System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, unsigned int col, float rounding, Tesseract::ImGui::ImDrawFlags flags) {
			m_drawlist->AddRectFilled({ pMin.X, pMin.Y }, { pMax.X, pMax.Y }, col, rounding, (ImDrawFlags)flags);
		}

		virtual void AddRectFilledMultiColor(System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, unsigned int colUprLeft, unsigned int colUprRight, unsigned int colBotRight, unsigned int colBotLeft) {
			m_drawlist->AddRectFilledMultiColor({ pMin.X, pMin.Y }, { pMax.X, pMax.Y }, colUprLeft, colUprRight, colBotRight, colBotLeft);
		}

		virtual void AddQuad(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, unsigned int col, float thickness) {
			m_drawlist->AddQuad({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, { p4.X, p4.Y }, col, thickness);
		}

		virtual void AddQuadFilled(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, unsigned int col) {
			m_drawlist->AddQuadFilled({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, { p4.X, p4.Y }, col);
		}

		virtual void AddTriangle(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, unsigned int col, float thickness) {
			m_drawlist->AddTriangle({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, col, thickness);
		}

		virtual void AddTriangleFilled(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, unsigned int col) {
			m_drawlist->AddTriangleFilled({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, col);
		}

		virtual void AddCircle(System::Numerics::Vector2 center, float radius, unsigned int col, int numSegments, float thickness) {
			m_drawlist->AddCircle({ center.X, center.Y }, radius, col, numSegments, thickness);
		}

		virtual void AddCircleFilled(System::Numerics::Vector2 center, float radius, unsigned int col, int numSegments) {
			m_drawlist->AddCircle({ center.X, center.Y }, radius, col, numSegments);
		}

		virtual void AddNgon(System::Numerics::Vector2 center, float radius, unsigned int col, int numSegments, float thickness) {
			m_drawlist->AddNgon({ center.X, center.Y }, radius, col, numSegments, thickness);
		}

		virtual void AddNgonFilled(System::Numerics::Vector2 center, float radius, unsigned int col, int numSegments) {
			m_drawlist->AddNgonFilled({ center.X, center.Y }, radius, col, numSegments);
		}

		virtual void AddText(System::Numerics::Vector2 pos, unsigned int col, System::String^ text) {
			StringParam pText(text);
			m_drawlist->AddText({ pos.X, pos.Y }, col, pText.begin(), pText.end());
		}

		virtual void AddText(Tesseract::ImGui::IImFont^ font, float fontSize, System::Numerics::Vector2 pos, unsigned int col, System::String^ text, float wrapWidth, System::Nullable<System::Numerics::Vector4> cpuFineClipRect);

		virtual void AddPolyline(System::ReadOnlySpan<System::Numerics::Vector2> points, unsigned int col, Tesseract::ImGui::ImDrawFlags flags, float thickness) {
			pin_ptr<Vector2> pPoints = &MemoryMarshal::GetReference(points);
			m_drawlist->AddPolyline((ImVec2*)pPoints, points.Length, col, (ImDrawFlags)flags, thickness);
		}

		virtual void AddConvexPolyFilled(System::ReadOnlySpan<System::Numerics::Vector2> points, unsigned int col) {
			pin_ptr<Vector2> pPoints = &MemoryMarshal::GetReference(points);
			m_drawlist->AddConvexPolyFilled((ImVec2*)pPoints, points.Length, col);
		}

		virtual void AddBezierCubic(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, unsigned int col, float thickness, int numSegments) {
			m_drawlist->AddBezierCubic({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, { p4.X, p4.Y }, col, thickness, numSegments);
		}

		virtual void AddBezierQuadratic(System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, unsigned int col, float thickness, int numSegments) {
			m_drawlist->AddBezierQuadratic({ p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, col, thickness, numSegments);
		}

		virtual void AddImage(System::UIntPtr userTextureID, System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, System::Numerics::Vector2 uvMin, System::Numerics::Vector2 uvMax, unsigned int col) {
			m_drawlist->AddImage((void*)userTextureID, { pMin.X, pMin.Y }, { pMax.X, pMax.Y }, { uvMin.X, uvMin.Y }, { uvMax.X, uvMax.Y }, col);
		}

		virtual void AddImage(System::UIntPtr userTextureID, System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, System::Numerics::Vector2 uvMin) {
			AddImage(userTextureID, pMin, pMax, uvMin, Vector2::One, 0xFFFFFFFF);
		}

		virtual void AddImageQuad(System::UIntPtr userTextureID, System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, System::Numerics::Vector2 uv1, System::Numerics::Vector2 uv2, System::Numerics::Vector2 uv3, System::Numerics::Vector2 uv4, unsigned int col) {
			m_drawlist->AddImageQuad((void*)userTextureID, { p1.X, p1.Y }, { p2.X, p2.Y }, { p3.X, p3.Y }, { p4.X, p4.Y }, { uv1.X, uv1.Y }, { uv2.X, uv2.Y }, { uv3.X, uv3.Y }, { uv4.X, uv4.Y }, col);
		}

		virtual void AddImageQuad(System::UIntPtr userTextureID, System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, System::Numerics::Vector2 uv1, System::Numerics::Vector2 uv2, System::Numerics::Vector2 uv3) {
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, uv3, Vector2(0, 1), 0xFFFFFFFF);
		}

		virtual void AddImageQuad(System::UIntPtr userTextureID, System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, System::Numerics::Vector2 uv1, System::Numerics::Vector2 uv2) {
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, uv2, Vector2::One);
		}

		virtual void AddImageQuad(System::UIntPtr userTextureID, System::Numerics::Vector2 p1, System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, System::Numerics::Vector2 uv1) {
			AddImageQuad(userTextureID, p1, p2, p3, p4, uv1, Vector2(1, 0));
		}

		virtual void AddImageRounded(System::UIntPtr userTextureID, System::Numerics::Vector2 pMin, System::Numerics::Vector2 pMax, System::Numerics::Vector2 uvMin, System::Numerics::Vector2 uvMax, unsigned int col, float rounding, Tesseract::ImGui::ImDrawFlags flags) {
			m_drawlist->AddImageRounded((void*)userTextureID, { pMin.X, pMin.Y }, { pMax.X, pMax.Y }, { uvMin.X, uvMin.Y }, { uvMax.X, uvMax.Y }, col, rounding, (ImDrawFlags)flags);
		}

		virtual void PathClear() {
			m_drawlist->PathClear();
		}

		virtual void PathLineTo(System::Numerics::Vector2 pos) {
			m_drawlist->PathLineTo({ pos.X, pos.Y });
		}

		virtual void PathLineToMergeDuplicate(System::Numerics::Vector2 pos) {
			m_drawlist->PathLineToMergeDuplicate({ pos.X, pos.Y });
		}

		virtual void PathFillConvex(unsigned int col) {
			m_drawlist->PathFillConvex(col);
		}

		virtual void PathStroke(unsigned int col, Tesseract::ImGui::ImDrawFlags flags, float thickness) {
			m_drawlist->PathStroke(col, (ImDrawFlags)flags, thickness);
		}

		virtual void PathArcTo(System::Numerics::Vector2 center, float radius, float aMin, float aMax, int numSegments) {
			m_drawlist->PathArcTo({ center.X, center.Y }, radius, aMin, aMax, numSegments);
		}

		virtual void PathArcToFast(System::Numerics::Vector2 center, float radius, int aMinOf12, int aMaxOf12) {
			m_drawlist->PathArcToFast({ center.X, center.Y }, radius, aMinOf12, aMaxOf12);
		}

		virtual void PathBezierCubicCurveTo(System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, System::Numerics::Vector2 p4, int numSegments) {
			m_drawlist->PathBezierCubicCurveTo({ p2.X, p2.Y }, { p3.X, p3.Y }, { p4.X, p4.Y }, numSegments);
		}

		virtual void PathBezierQuadraticCurveTo(System::Numerics::Vector2 p2, System::Numerics::Vector2 p3, int numSegments) {
			m_drawlist->PathBezierQuadraticCurveTo({ p2.X, p2.Y }, { p3.X, p3.Y }, numSegments);
		}

		virtual void PathRect(System::Numerics::Vector2 rectMin, System::Numerics::Vector2 rectMax, float rounding, Tesseract::ImGui::ImDrawFlags flags) {
			m_drawlist->PathRect({ rectMin.X, rectMin.Y }, { rectMax.X, rectMax.Y }, rounding, (ImDrawFlags)flags);
		}

		virtual void AddCallback(Tesseract::ImGui::ImDrawCallback^ callback) {
			m_drawlist->AddCallback(g_customDrawCallback, DrawCallbackHolder::Instance->Register(callback));
		}

		virtual void AddDrawCmd() {
			m_drawlist->AddDrawCmd();
		}

		virtual Tesseract::ImGui::IImDrawList^ CloneOutput() {
			return gcnew ImDrawListCLI(m_drawlist->CloneOutput(), true);
		}
		
		virtual void ChannelsSplit(int count) {
			m_drawlist->ChannelsSplit(count);
		}

		virtual void ChannelsMerge() {
			m_drawlist->ChannelsMerge();
		}

		virtual void ChannelsSetCurrent(int n) {
			m_drawlist->ChannelsSetCurrent(n);
		}

		virtual void PrimReserve(int idxCount, int vtxCount) {
			m_drawlist->PrimReserve(idxCount, vtxCount);
		}

		virtual void PrimUnreserve(int idxCount, int vtxCount) {
			m_drawlist->PrimUnreserve(idxCount, vtxCount);
		}

		virtual void PrimRect(System::Numerics::Vector2 a, System::Numerics::Vector2 b, unsigned int col) {
			m_drawlist->PrimRect({ a.X, a.Y }, { b.X, b.Y }, col);
		}

		virtual void PrimRectUV(System::Numerics::Vector2 a, System::Numerics::Vector2 b, System::Numerics::Vector2 uvA, System::Numerics::Vector2 uvB, unsigned int col) {
			m_drawlist->PrimRectUV({ a.X, a.Y }, { b.X, b.Y }, { uvA.X, uvA.Y }, { uvB.X, uvB.Y }, col);
		}

		virtual void PrimQuadUV(System::Numerics::Vector2 a, System::Numerics::Vector2 b, System::Numerics::Vector2 c, System::Numerics::Vector2 d, System::Numerics::Vector2 uvA, System::Numerics::Vector2 uvB, System::Numerics::Vector2 uvC, System::Numerics::Vector2 uvD, unsigned int col) {
			m_drawlist->PrimQuadUV({ a.X, a.Y }, { b.X, b.Y }, { c.X, c.Y }, { d.X, d.Y }, { uvA.X, uvA.Y }, { uvB.X, uvB.Y }, { uvC.X, uvC.Y }, { uvD.X, uvD.Y }, col);
		}

		virtual void PrimWriteVtx(System::Numerics::Vector2 pos, System::Numerics::Vector2 uv, unsigned int col) {
			m_drawlist->PrimWriteVtx({ pos.X, pos.Y }, { uv.X, uv.Y }, col);
		}

		virtual void PrimWriteIdx(unsigned short idx) {
			m_drawlist->PrimWriteIdx(idx);
		}

		virtual void PrimVtx(System::Numerics::Vector2 pos, System::Numerics::Vector2 uv, unsigned int col) {
			m_drawlist->PrimVtx({ pos.X, pos.Y }, { uv.X, uv.Y }, col);
		}
	};

	public ref class ImDrawDataCLI : public Tesseract::ImGui::IImDrawData {
	internal:
		ImDrawData* m_drawdata;
		List<ImDrawListCLI^>^ m_drawlists;

		ImDrawDataCLI(ImDrawData* drawdata) : m_drawdata(drawdata) {
			m_drawlists = gcnew List<ImDrawListCLI^>();
		}

		void UpdateDrawLists() {
			bool valid = m_drawlists->Count == m_drawdata->CmdListsCount;
			if (valid) {
				for (int i = 0; i < m_drawdata->CmdListsCount; i++) {
					if (m_drawlists->default[i]->m_drawlist != m_drawdata->CmdLists[i]) {
						valid = false;
						break;
					}
				}
			}
			if (valid) return;
			m_drawlists->Clear();
			for (int i = 0; i < m_drawdata->CmdListsCount; i++) {
				m_drawlists->Add(gcnew ImDrawListCLI(m_drawdata->CmdLists[i], false));
			}
		}

	public:
		virtual property bool Valid {
			virtual bool get() { return m_drawdata->Valid; }
		}

		virtual property int TotalIdxCount {
			virtual int get() { return m_drawdata->TotalIdxCount; }
		}

		virtual property int TotalVtxCount {
			virtual int get() { return m_drawdata->TotalVtxCount; }
		}
		
		virtual property System::Collections::Generic::IReadOnlyList<Tesseract::ImGui::IImDrawList^>^ CmdLists {
			virtual IReadOnlyList<Tesseract::ImGui::IImDrawList^>^ get() {
				UpdateDrawLists();
				return (IReadOnlyList<Tesseract::ImGui::IImDrawList^>^)m_drawlists;
			}
		}

		virtual property System::Numerics::Vector2 DisplayPos {
			virtual Vector2 get() { return Vector2(m_drawdata->DisplayPos.x, m_drawdata->DisplayPos.y); }
		}

		virtual property System::Numerics::Vector2 DisplaySize {
			virtual Vector2 get() { return Vector2(m_drawdata->DisplaySize.x, m_drawdata->DisplaySize.y); }
		}

		virtual property System::Numerics::Vector2 FramebufferScale {
			virtual Vector2 get() { return Vector2(m_drawdata->FramebufferScale.x, m_drawdata->FramebufferScale.y); }
		}

		virtual void Clear() {
			m_drawdata->Clear();
		}

		virtual void DeIndexAllBuffers() {
			m_drawdata->DeIndexAllBuffers();
		}

		virtual void ScaleClipRects(System::Numerics::Vector2 fbScale) {
			m_drawdata->ScaleClipRects({ fbScale.X, fbScale.Y });
		}
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

		virtual property System::Span<unsigned char> Buf {
			virtual Span<unsigned char> get() {
				return Span<unsigned char>(m_data->Buf, m_data->BufSize);
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

	public ref class ImDrawListSharedDataCLI : Tesseract::ImGui::IImDrawListSharedData {
	internal:
		ImDrawListSharedData* m_data;

		ImDrawListSharedDataCLI(ImDrawListSharedData* data) : m_data(data) {}
	};

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
				return gcnew Tesseract::Core::Utilities::CLI::ListEnumerator<float>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property float default[int]{
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
				return gcnew Tesseract::Core::Utilities::CLI::ListEnumerator<wchar_t>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property wchar_t default[int]{
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
				return gcnew Tesseract::Core::Utilities::CLI::ListEnumerator<Tesseract::ImGui::ImFontGlyph>(this);
			}

			virtual property int Count {
				virtual int get() { return m_vec->Size; }
			}

			virtual property Tesseract::ImGui::ImFontGlyph default[int]{
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

		virtual property wchar_t DotChar {
			virtual wchar_t get() { return m_font->DotChar; }
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
		pin_ptr<unsigned char> pData; \
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

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryTTF(array<unsigned char, 1>^ fontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<unsigned char> pFontData = &fontData[0];
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromMemoryTTF(pFontData, fontData->Length, sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryCompressedTTF(array<unsigned char, 1>^ compressedFontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<unsigned char> pFontData = &compressedFontData[0];
			ImFontConfig ncfg = ConvertConfig(config);
			INIT_FONT_GLYPHS(config, ncfg);
			const ImWchar* pGlyphRanges = glyphRanges ? CreateGlyphRange(glyphRanges) : nullptr;
			ImFontCLI^ font = gcnew ImFontCLI(m_atlas->AddFontFromMemoryCompressedTTF(pFontData, compressedFontData->Length, sizePixels, config ? &ncfg : nullptr, pGlyphRanges));
			return font;
		}

		virtual Tesseract::ImGui::IImFont^ AddFontFromMemoryCompressedBase85TTF(array<unsigned char, 1>^ compressedFontData, float sizePixels, Tesseract::ImGui::ImFontConfig^ config, System::Collections::Generic::IReadOnlyCollection<System::ValueTuple<wchar_t, wchar_t>>^ glyphRanges) {
			pin_ptr<unsigned char> pFontData = &compressedFontData[0];
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

		virtual System::ReadOnlySpan<unsigned char> GetTexDataAsAlpha8(int% outWidth, int% outHeight, int% outBytesPerPixel) {
			pin_ptr<int> pOutWidth = &outWidth, pOutHeight = &outHeight, pOutBytesPerPixel = &outBytesPerPixel;
			unsigned char* pData;
			m_atlas->GetTexDataAsAlpha8(&pData, pOutWidth, pOutHeight, pOutBytesPerPixel);
			return ReadOnlySpan<unsigned char>(pData, outWidth * outHeight * outBytesPerPixel);
		}

		virtual System::ReadOnlySpan<unsigned char> GetTexDataAsRGBA32(int% outWidth, int% outHeight, int% outBytesPerPixel) {
			pin_ptr<int> pOutWidth = &outWidth, pOutHeight = &outHeight, pOutBytesPerPixel = &outBytesPerPixel;
			unsigned char* pData;
			m_atlas->GetTexDataAsRGBA32(&pData, pOutWidth, pOutHeight, pOutBytesPerPixel);
			return ReadOnlySpan<unsigned char>(pData, outWidth * outHeight * outBytesPerPixel);
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

	public ref class ImGuiIOCLI : Tesseract::ImGui::IImGuiIO {
	internal:
		ImGuiIO* m_io;
		ImFontAtlasCLI^ m_fonts;
		ImFontCLI^ m_fontdefault;

		ImGuiIOCLI(ImGuiIO* io) : m_io(io) {
			m_fonts = gcnew ImFontAtlasCLI(io->Fonts, false);
		}

	public:
		virtual property Tesseract::ImGui::ImGuiConfigFlags ConfigFlags {
			virtual Tesseract::ImGui::ImGuiConfigFlags get() { return (Tesseract::ImGui::ImGuiConfigFlags)m_io->ConfigFlags; }
			virtual void set(Tesseract::ImGui::ImGuiConfigFlags value) { m_io->ConfigFlags = (ImGuiConfigFlags)value; }
		}

		virtual property Tesseract::ImGui::ImGuiBackendFlags BackendFlags {
			virtual Tesseract::ImGui::ImGuiBackendFlags get() { return (Tesseract::ImGui::ImGuiBackendFlags)m_io->BackendFlags; }
			virtual void set(Tesseract::ImGui::ImGuiBackendFlags value) { m_io->BackendFlags = (ImGuiBackendFlags)value; }
		}

		virtual property System::Numerics::Vector2 DisplaySize {
			virtual Vector2 get() { return Vector2(m_io->DisplaySize.x, m_io->DisplaySize.y); }
			virtual void set(Vector2 value) { m_io->DisplaySize = { value.X, value.Y }; }
		}

		virtual property float DeltaTime {
			virtual float get() { return m_io->DeltaTime; }
			virtual void set(float value) { m_io->DeltaTime = value; }
		}

		virtual property float IniSavingRate {
			virtual float get() { return m_io->IniSavingRate; }
			virtual void set(float value) { m_io->IniSavingRate = value; }
		}

	private:
		StringHolder^ strIniFilename = gcnew StringHolder();
		StringHolder^ strLogFilename = gcnew StringHolder();

	public:
		virtual property System::String^ IniFilename {
			virtual String^ get() {
				strIniFilename->Set(m_io->IniFilename);
				return strIniFilename->Get();
			}
			virtual void set(String^ value) {
				strIniFilename->Set(value);
				m_io->IniFilename = strIniFilename->c_str();
			}
		}

		virtual property System::String^ LogFilename {
			virtual String^ get() {
				strLogFilename->Set(m_io->LogFilename);
				return strLogFilename->Get();
			}
			virtual void set(String^ value) {
				strLogFilename->Set(value);
				m_io->LogFilename = strLogFilename->c_str();
			}
		}

		virtual property float MouseDoubleClickTime {
			virtual float get() { return m_io->MouseDoubleClickTime; }
			virtual void set(float value) { m_io->MouseDoubleClickTime = value; }
		}

		virtual property float MouseDoubleClickMaxDist {
			virtual float get() { return m_io->MouseDoubleClickMaxDist; }
			virtual void set(float value) { m_io->MouseDoubleClickMaxDist = value; }
		}

		virtual property float MouseDragThreshold {
			virtual float get() { return m_io->MouseDragThreshold; }
			virtual void set(float value) { m_io->MouseDragThreshold = value; }
		}

		virtual property float KeyRepeatDelay {
			virtual float get() { return m_io->KeyRepeatDelay; }
			virtual void set(float value) { m_io->KeyRepeatDelay = value; }
		}

		virtual property float KeyRepeatRate {
			virtual float get() { return m_io->KeyRepeatRate; }
			virtual void set(float value) { m_io->KeyRepeatRate = value; }
		}

		virtual property Tesseract::ImGui::IImFontAtlas^ Fonts {
			virtual Tesseract::ImGui::IImFontAtlas^ get() {
				return m_fonts;
			}
		}

		virtual property float FontGlobalScale {
			virtual float get() { return m_io->FontGlobalScale; }
			virtual void set(float value) { m_io->FontGlobalScale = value; }
		}

		virtual property bool FontAllowUserScaling {
			virtual bool get() { return m_io->FontAllowUserScaling; }
			virtual void set(bool value) { m_io->FontAllowUserScaling = value; }
		}

		virtual property Tesseract::ImGui::IImFont^ FontDefault {
			virtual Tesseract::ImGui::IImFont^ get() {
				ImFont* font = m_io->FontDefault;
				if (font) return gcnew ImFontCLI(font);
				else return nullptr;
			}
			virtual void set(Tesseract::ImGui::IImFont^ value) {
				m_io->FontDefault = m_fontdefault->m_font;
			}
		}

		virtual property System::Numerics::Vector2 DisplayFramebufferScale {
			virtual Vector2 get() { return Vector2(m_io->DisplayFramebufferScale.x, m_io->DisplayFramebufferScale.y); }
			virtual void set(Vector2 value) { m_io->DisplayFramebufferScale = { value.X, value.Y }; }
		}

		virtual property bool MouseDrawCursor {
			virtual bool get() { return m_io->MouseDrawCursor; }
			virtual void set(bool value) { m_io->MouseDrawCursor = value; }
		}

		virtual property bool ConfigMacOSXBehaviors {
			virtual bool get() { return m_io->ConfigMacOSXBehaviors; }
			virtual void set(bool value) { m_io->ConfigMacOSXBehaviors = value; }
		}

		virtual property bool ConfigInputTrickleEventQueue {
			virtual bool get() { return m_io->ConfigInputTrickleEventQueue; }
			virtual void set(bool value) { m_io->ConfigInputTrickleEventQueue = value; }
		}

		virtual property bool ConfigInputTextCursorBlink {
			virtual bool get() { return m_io->ConfigInputTextCursorBlink; }
			virtual void set(bool value) { m_io->ConfigInputTextCursorBlink = value; }
		}

		virtual property bool ConfigDragClickToInputText {
			virtual bool get() { return m_io->ConfigDragClickToInputText; }
			virtual void set(bool value) { m_io->ConfigDragClickToInputText = value; }
		}

		virtual property bool ConfigWindowsResizeFromEdges {
			virtual bool get() { return m_io->ConfigWindowsResizeFromEdges; }
			virtual void set(bool value) { m_io->ConfigWindowsResizeFromEdges = value; }
		}

		virtual property bool ConfigWindowsMoveFromTitleBarOnly {
			virtual bool get() { return m_io->ConfigWindowsMoveFromTitleBarOnly; }
			virtual void set(bool value) { m_io->ConfigWindowsMoveFromTitleBarOnly = value; }
		}

		virtual property float ConfigMemoryCompactTimer {
			virtual float get() { return m_io->ConfigMemoryCompactTimer; }
			virtual void set(float value) { m_io->ConfigMemoryCompactTimer = value; }
		}

		// Platform Functions

	private:
		StringHolder^ strBackendPlatformName = gcnew StringHolder();
		StringHolder^ strBackendRendererName = gcnew StringHolder();

	public:
		virtual property System::String^ BackendPlatformName {
			virtual String^ get() {
				strBackendPlatformName->Set(m_io->BackendPlatformName);
				return strBackendPlatformName->Get();
			}
			virtual void set(String^ value) {
				strBackendPlatformName->Set(value);
				m_io->BackendPlatformName = strBackendPlatformName->c_str();
			}
		}

		virtual property System::String^ BackendRendererName {
			virtual String^ get() {
				strBackendRendererName->Set(m_io->BackendRendererName);
				return strBackendRendererName->Get();
			}
			virtual void set(String^ value) {
				strBackendRendererName->Set(value);
				m_io->BackendRendererName = strBackendRendererName->c_str();
			}
		}

	private:
		static Func<String^>^ getClipboardTextFn = nullptr;
		static StringHolder^ clipboardTextSet = gcnew StringHolder();
		static const char* getClipboardTextCbk(void*) {
			clipboardTextSet->Set(getClipboardTextFn->Invoke());
			return clipboardTextSet->c_str();
		}

		static Action<String^>^ setClipboardTextFn = nullptr;
		static void setClipboardTextCbk(void*, const char* str) {
			setClipboardTextFn->Invoke(gcnew String(str));
		}

		static Action<Tesseract::ImGui::ImGuiViewport, Tesseract::ImGui::ImGuiPlatformImeData>^ setPlatformImeDataFn = nullptr;
		static void setPlatformImeDataCbk(ImGuiViewport* viewport, ImGuiPlatformImeData* data) {
			Tesseract::ImGui::ImGuiViewport m_viewport = {};
			m_viewport.Flags = (Tesseract::ImGui::ImGuiViewportFlags)viewport->Flags;
			m_viewport.Pos = Vector2(viewport->Pos.x, viewport->Pos.y);
			m_viewport.Size = Vector2(viewport->Size.x, viewport->Size.y);
			m_viewport.WorkPos = Vector2(viewport->WorkPos.x, viewport->WorkPos.y);
			m_viewport.WorkSize = Vector2(viewport->WorkSize.x, viewport->WorkSize.y);
			m_viewport.PlatformHandleRaw = (IntPtr)viewport->PlatformHandleRaw;

			Tesseract::ImGui::ImGuiPlatformImeData m_data = {};
			m_data.WantVisible = data->WantVisible;
			m_data.InputPos = Vector2(data->InputPos.x, data->InputPos.y);
			m_data.InputLineHeight = data->InputLineHeight;

			setPlatformImeDataFn->Invoke(m_viewport, m_data);
		}

	public:
		virtual property System::Func<System::String^>^ GetClipboardTextFn {
			virtual void set(Func<String^>^ value) {
				getClipboardTextFn = value;
				m_io->GetClipboardTextFn = (const char* (*)(void*))getClipboardTextCbk;
			}
		}

		virtual property System::Action<System::String^>^ SetClipboardTextFn {
			virtual void set(Action<String^>^ value) {
				setClipboardTextFn = value;
				m_io->SetClipboardTextFn = (void(*)(void*, const char*))setClipboardTextCbk;
			}
		}

		virtual property System::Action<Tesseract::ImGui::ImGuiViewport, Tesseract::ImGui::ImGuiPlatformImeData>^ SetPlatformImeDataFn {
			virtual void set(Action<Tesseract::ImGui::ImGuiViewport, Tesseract::ImGui::ImGuiPlatformImeData>^ value) {
				setPlatformImeDataFn = value;
				m_io->SetPlatformImeDataFn = (void(*)(ImGuiViewport*, ImGuiPlatformImeData*))setPlatformImeDataCbk;
			}
		}

		// Output

		virtual property bool WantCaptureMouse {
			virtual bool get() { return m_io->WantCaptureMouse; }
		}

		virtual property bool WantCaptureKeyboard {
			virtual bool get() { return m_io->WantCaptureKeyboard; }
		}

		virtual property bool WantTextInput {
			virtual bool get() { return m_io->WantTextInput; }
		}

		virtual property bool WantSetMousePos {
			virtual bool get() { return m_io->WantSetMousePos; }
		}

		virtual property bool WantSaveIniSettings {
			virtual bool get() { return m_io->WantSaveIniSettings; }
			virtual void set(bool value) { m_io->WantSaveIniSettings = value; }
		}

		virtual property bool NavActive {
			virtual bool get() { return m_io->NavActive; }
		}

		virtual property bool NavVisible {
			virtual bool get() { return m_io->NavVisible; }
		}

		virtual property float Framerate {
			virtual float get() { return m_io->Framerate; }
		}

		virtual property int MetricsRenderVertices {
			virtual int get() { return m_io->MetricsRenderVertices; }
		}

		virtual property int MetricsRenderIndices {
			virtual int get() { return m_io->MetricsRenderIndices; }
		}

		virtual property int MetricsRenderWindows {
			virtual int get() { return m_io->MetricsRenderWindows; }
		}

		virtual property int MetricsActiveWindows {
			virtual int get() { return m_io->MetricsActiveWindows; }
		}

		virtual property int MetricsActiveAllocations {
			virtual int get() { return m_io->MetricsActiveAllocations; }
		}

		virtual property System::Numerics::Vector2 MouseDelta {
			virtual Vector2 get() { return Vector2(m_io->MouseDelta.x, m_io->MouseDelta.y); }
		}

		// Input

		virtual void AddKeyEvent(Tesseract::ImGui::ImGuiKey key, bool down) {
			m_io->AddKeyEvent((ImGuiKey)key, down);
		}

		virtual void AddKeyAnalogEvent(Tesseract::ImGui::ImGuiKey key, bool down, float v) {
			m_io->AddKeyAnalogEvent((ImGuiKey)key, down, v);
		}

		virtual void AddMousePosEvent(float x, float y) {
			m_io->AddMousePosEvent(x, y);
		}

		virtual void AddMouseButtonEvent(int button, bool down) {
			m_io->AddMouseButtonEvent(button, down);
		}

		virtual void AddMouseWheelEvent(float x, float y) {
			m_io->AddMouseWheelEvent(x, y);
		}

		virtual void AddFocusEvent(bool focused) {
			m_io->AddFocusEvent(focused);
		}

		virtual void AddInputCharacter(int c) {
			m_io->AddInputCharacter(c);
		}

		virtual void AddInputCharacterUTF16(wchar_t c) {
			m_io->AddInputCharacterUTF16(c);
		}

		virtual void AddInputCharactersUTF8(System::ReadOnlySpan<unsigned char> str) {
			pin_ptr<unsigned char> pStr = &MemoryMarshal::GetReference(str);
			m_io->AddInputCharactersUTF8((const char*)pStr);
		}

		virtual void AddInputCharacters(System::String^ str) {
			StringParam pStr(str);
			m_io->AddInputCharactersUTF8(pStr.c_str());
		}

		// [Internal]

		virtual property System::Numerics::Vector2 MousePos {
			virtual Vector2 get() {
				return Vector2(m_io->MousePos.x, m_io->MousePos.y);
			}
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
		virtual property System::ReadOnlySpan<unsigned char> Data {
			virtual ReadOnlySpan<unsigned char> get() {
				return ReadOnlySpan<unsigned char>(m_payload->Data, m_payload->DataSize);
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

		virtual void ShowDemoWindow(System::Nullable<bool> open) {
			bool pOpen = open.GetValueOrDefault();
			::ImGui::ShowDemoWindow(open.HasValue ? &pOpen : nullptr);
		}

		virtual void ShowMetricsWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowMetricsWindow(pOpen);
		}

		virtual void ShowMetricsWindow(System::Nullable<bool> open) {
			bool pOpen = open.GetValueOrDefault();
			::ImGui::ShowMetricsWindow(open.HasValue ? &pOpen : nullptr);
		}

		virtual void ShowStackToolWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowStackToolWindow(pOpen);
		}

		virtual void ShowStackToolWindow(System::Nullable<bool> open) {
			bool pOpen = open.GetValueOrDefault();
			::ImGui::ShowStackToolWindow(open.HasValue ? &pOpen : nullptr);
		}

		virtual void ShowAboutWindow(bool% open) {
			pin_ptr<bool> pOpen = &open;
			::ImGui::ShowAboutWindow(pOpen);
		}

		virtual void ShowAboutWindow(System::Nullable<bool> open) {
			bool pOpen = open.GetValueOrDefault();
			::ImGui::ShowAboutWindow(open.HasValue ? &pOpen : nullptr);
		}

		virtual void ShowStyleEditor(Tesseract::ImGui::IImGuiStyle^ style) {
			ImGuiStyle* pStyle = nullptr;
			if (style) ((ImGuiStyleCLI^)style)->m_style;
			::ImGui::ShowStyleEditor(pStyle);
		}

		virtual void ShowStyleSelector(System::String^ label) {
			StringParam pLabel(label);
			::ImGui::ShowStyleSelector(pLabel.c_str());
		}

		virtual void ShowFontSelector(System::String^ label) {
			StringParam pLabel(label);
			::ImGui::ShowFontSelector(pLabel.c_str());
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

		virtual void Begin(System::String^ name, bool% open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pName(name);
			pin_ptr<bool> pOpen = &open;
			::ImGui::Begin(pName.c_str(), pOpen, (ImGuiWindowFlags)flags);
		}

		virtual void Begin(System::String^ name, System::Nullable<bool> open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pName(name);
			bool vopen, *pvopen = nullptr;
			if (open.HasValue) {
				vopen = open.Value;
				pvopen = &vopen;
			}
			::ImGui::Begin(pName.c_str(), pvopen, (ImGuiWindowFlags)flags);
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

		virtual void BeginChild(System::String^ strId, System::Numerics::Vector2 size, bool border, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pStrID(strId);
			::ImGui::BeginChild(pStrID.c_str(), { size.X, size.Y }, border, (ImGuiWindowFlags)flags);
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

		virtual void SetWindowPos(System::String^ name, System::Numerics::Vector2 pos, Tesseract::ImGui::ImGuiCond cond) {
			StringParam pName(name);
			::ImGui::SetWindowPos(pName.c_str(), { pos.X, pos.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowSize(System::String^ name, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiCond cond) {
			StringParam pName(name);
			::ImGui::SetWindowSize(pName.c_str(), { size.X, size.Y }, (ImGuiCond)cond);
		}

		virtual void SetWindowCollapsed(System::String^ name, bool collapsed, Tesseract::ImGui::ImGuiCond cond) {
			StringParam pName(name);
			::ImGui::SetWindowCollapsed(pName.c_str(), collapsed, (ImGuiCond)cond);
		}

		virtual void SetWindowFocus(System::String^ name) {
			StringParam pName(name);
			::ImGui::SetWindowFocus(pName.c_str());
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

		virtual void PushAllowKeyboardFocus(bool allowKeyboardFocus) {
			::ImGui::PushAllowKeyboardFocus(allowKeyboardFocus);
		}

		virtual void PopAllowKeyboardFocus() {
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

		virtual void PushID(System::String^ strID) {
			StringParam pStrID(strID);
			::ImGui::PushID(pStrID.c_str());
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

		virtual unsigned int GetID(System::String^ strID) {
			StringParam pStrID(strID);
			return ::ImGui::GetID(pStrID.c_str());
		}

		virtual unsigned int GetID(System::IntPtr ptrID) {
			return ::ImGui::GetID((void*)ptrID);
		}

		// Widgets: Text

		virtual void Text(System::String^ text) {
			StringParam pText(text);
			::ImGui::TextUnformatted(pText.begin(), pText.end());
		}

		virtual void TextColored(System::Numerics::Vector4 col, System::String^ fmt) {
			StringParam pFmt(fmt, true);
			::ImGui::TextColored({ col.X, col.Y, col.Z, col.W }, pFmt.c_str());
		}

		virtual void TextDisabled(System::String^ fmt) {
			StringParam pFmt(fmt, true);
			::ImGui::TextDisabled(pFmt.c_str());
		}

		virtual void TextWrapped(System::String^ fmt) {
			StringParam pFmt(fmt, true);
			::ImGui::TextWrapped(pFmt.c_str());
		}

		virtual void LabelText(System::String^ label, System::String^ fmt) {
			StringParam pLabel(label), pFmt(fmt, true);
			::ImGui::LabelText(pLabel.c_str(), pFmt.c_str());
		}

		virtual void BulletText(System::String^ fmt) {
			StringParam pFmt(fmt, true);
			::ImGui::BulletText(pFmt.c_str());
		}

		// Widgets: Main
		// - Most widgets return true when the value has been changed or when pressed/selected
		// - You may also use one of the many IsItemXXX functions (e.g. IsItemActive, IsItemHovered, etc.) to query widget state.

		virtual bool Button(System::String^ label, System::Numerics::Vector2 size) {
			StringParam pLabel(label);
			return ::ImGui::Button(pLabel.c_str(), { size.X, size.Y });
		}

		virtual bool SmallButton(System::String^ label) {
			StringParam pLabel(label);
			return ::ImGui::SmallButton(pLabel.c_str());
		}

		virtual bool InvisibleButton(System::String^ strID, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiButtonFlags flags) {
			StringParam pStrID(strID);
			return ::ImGui::InvisibleButton(pStrID.c_str(), { size.X, size.Y }, (ImGuiButtonFlags)flags);
		}

		virtual bool ArrowButton(System::String^ strID, Tesseract::ImGui::ImGuiDir dir) {
			StringParam pStrID(strID);
			return ::ImGui::ArrowButton(pStrID.c_str(), (ImGuiDir)dir);
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

		virtual bool ImageButton(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1, int framePadding, System::Numerics::Vector4 bgCol, System::Numerics::Vector4 tintCol) {
			return ::ImGui::ImageButton((void*)userTextureID, { size.X, size.Y }, { uv0.X, uv0.Y }, { uv1.X, uv1.Y }, framePadding, { bgCol.X, bgCol.Y, bgCol.Z, bgCol.W }, { tintCol.X, tintCol.Y, tintCol.Z, tintCol.W });
		}

		virtual bool ImageButton(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0, System::Numerics::Vector2 uv1, int framePadding, System::Numerics::Vector4 bgCol) {
			return ImageButton(userTextureID, size, uv0, uv1, framePadding, bgCol, Vector4::One);
		}

		virtual bool ImageButton(System::UIntPtr userTextureID, System::Numerics::Vector2 size, System::Numerics::Vector2 uv0) {
			return ImageButton(userTextureID, size, uv0, Vector2::One, -1, Vector4::Zero);
		}

		virtual bool Checkbox(System::String^ label, bool% v) {
			StringParam pLabel(label);
			pin_ptr<bool> pv = &v;
			return ::ImGui::Checkbox(pLabel.c_str(), pv);
		}

		virtual bool CheckboxFlags(System::String^ label, int% flags, int flagsValue) {
			StringParam pLabel(label);
			pin_ptr<int> pFlags = &flags;
			return ::ImGui::CheckboxFlags(pLabel.c_str(), pFlags, flagsValue);
		}

		virtual bool CheckboxFlags(System::String^ label, unsigned int% flags, unsigned int flagsValue) {
			StringParam pLabel(label);
			pin_ptr<unsigned int> pFlags = &flags;
			return ::ImGui::CheckboxFlags(pLabel.c_str(), pFlags, flagsValue);
		}

		virtual bool RadioButton(System::String^ label, bool active) {
			StringParam pLabel(label);
			return ::ImGui::RadioButton(pLabel.c_str(), active);
		}

		virtual bool RadioButton(System::String^ label, int% v, int vButton) {
			StringParam pLabel(label);
			pin_ptr<int> pv = &v;
			return ::ImGui::RadioButton(pLabel.c_str(), pv, vButton);
		}

		virtual void ProgressBar(float fraction, System::Numerics::Vector2 sizeArg, System::String^ overlay) {
			StringParam pOverlay(overlay);
			::ImGui::ProgressBar(fraction, { sizeArg.X, sizeArg.Y }, pOverlay.c_str());
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

		virtual bool BeginCombo(System::String^ label, System::String^ previewValue, Tesseract::ImGui::ImGuiComboFlags flags) {
			StringParam pLabel(label), pPreviewValue(previewValue);
			return ::ImGui::BeginCombo(pLabel.c_str(), pPreviewValue.c_str(), (ImGuiComboFlags)flags);
		}

		virtual void EndCombo() {
			::ImGui::EndCombo();
		}

		virtual bool Combo(System::String^ label, int% currentItem, System::Collections::Generic::IEnumerable<System::String^>^ items, int popupMaxHeightInItems) {
			StringParam pLabel(label);
			pin_ptr<int> pCurrentItem = &currentItem;
			StringArrayParam pItems(items);
			return ::ImGui::Combo(pLabel.c_str(), pCurrentItem, pItems.data(), (int)pItems.length(), popupMaxHeightInItems);
		}

		virtual bool Combo(System::String^ label, int% currentItem, System::String^ itemsSeparatedByZeros, int popupMaxHeightInItems) {
			StringParam pLabel(label), pItemsSeparatedByZeros(itemsSeparatedByZeros);
			pin_ptr<int> pCurrentItem = &currentItem;
			return ::ImGui::Combo(pLabel.c_str(), pCurrentItem, pItemsSeparatedByZeros.c_str(), popupMaxHeightInItems);
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
		virtual bool Combo(System::String^ label, int% currentItem, Tesseract::ImGui::IImGui::ComboItemsGetter^ itemsGetter, int itemscount, int popupMaxHeightInItems) {
			StringParam pLabel(label);
			pin_ptr<int> pCurrentItem = &currentItem;
			comboGetterFn = itemsGetter;
			return ::ImGui::Combo(pLabel.c_str(), pCurrentItem, (bool(*)(void*,int,const char**))comboGetterCbk, nullptr, itemscount, popupMaxHeightInItems);
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

		virtual bool DragFloat(System::String^ label, float% v, float vSpeed, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pv = &v;
			return ::ImGui::DragFloat(pLabel.c_str(), pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat2(System::String^ label, System::Numerics::Vector2% v, float vSpeed, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::DragFloat2(pLabel.c_str(), (float*)pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat3(System::String^ label, System::Numerics::Vector3% v, float vSpeed, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::DragFloat3(pLabel.c_str(), (float*)pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloat4(System::String^ label, System::Numerics::Vector4% v, float vSpeed, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::DragFloat4(pLabel.c_str(), (float*)pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragFloatRange2(System::String^ label, float% vCurrentMin, float% vCurrentMax, float vSpeed, float vMin, float vMax, System::String^ format, System::String^ formatMax, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format), pFormatMax(formatMax);
			pin_ptr<float> pvCurrentMin = &vCurrentMin, pvCurrentMax = &vCurrentMax;
			return ::ImGui::DragFloatRange2(pLabel.c_str(), pvCurrentMin, pvCurrentMax, vSpeed, vMin, vMax, pFormat.c_str(), pFormatMax.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragInt(System::String^ label, int% v, float vSpeed, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &v;
			return ::ImGui::DragInt(pLabel.c_str(), pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool DragInt2(System::String^ label, System::Span<int> v, float vSpeed, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt2(pLabel.c_str(), pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragInt3(System::String^ label, System::Span<int> v, float vSpeed, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt3(pLabel.c_str(), pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragInt4(System::String^ label, System::Span<int> v, float vSpeed, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			bool retn = ::ImGui::DragInt4(pLabel.c_str(), pv, vSpeed, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
			return retn;
		}

		virtual bool DragIntRange2(System::String^ label, int% vCurrentMin, int% vCurrentMax, float vSpeed, int vMin, int vMax, System::String^ format, System::String^ formatMax, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format), pFormatMax(formatMax);
			pin_ptr<int> pvCurrentMin = &vCurrentMin, pvCurrentMax = &vCurrentMax;
			return ::ImGui::DragIntRange2(pLabel.c_str(), pvCurrentMin, pvCurrentMax, vSpeed, vMin, vMax, pFormat.c_str(), pFormatMax.c_str(), (ImGuiSliderFlags)flags);
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
		virtual bool DragScalar(System::String^ label, T% data, float vSpeed, Tesseract::ImGui::ImNullable<T> min, Tesseract::ImGui::ImNullable<T> max, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
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
			return ::ImGui::DragScalar(pLabel.c_str(), GetDataType<T>(), pData, vSpeed, pMin, pMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool DragScalarN(System::String^ label, System::Span<T> data, float vSpeed, Tesseract::ImGui::ImNullable<T> min, Tesseract::ImGui::ImNullable<T> max, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			pin_ptr<T> pData = &MemoryMarshal::GetReference(data); StringParam pLabel(label), pFormat(format);
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
			return ::ImGui::DragScalarN(pLabel.c_str(), GetDataType<T>(), pData, data.Length, vSpeed, pMin, pMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		// Widgets: Regular Sliders
		// - CTRL+Click on any slider to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.
		// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: %.0f" -> Biscuit: 1; etc.
		// - Format string may also be set to NULL or use the default format ("%f" or "%d").
		// - Legacy: Pre-1.78 there are SliderXXX() function signatures that takes a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.
		//   If you get a warning converting a float to ImGuiSliderFlags, read https://github.com/ocornut/imgui/issues/3361

		virtual bool SliderFloat(System::String^ label, float% v, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pv = &v;
			return ::ImGui::SliderFloat(pLabel.c_str(), pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat2(System::String^ label, System::Numerics::Vector2% v, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::SliderFloat2(pLabel.c_str(), (float*)pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat3(System::String^ label, System::Numerics::Vector3% v, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::SliderFloat3(pLabel.c_str(), (float*)pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderFloat4(System::String^ label, System::Numerics::Vector4% v, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::SliderFloat4(pLabel.c_str(), (float*)pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderAngle(System::String^ label, float% vRad, float vDegreesMin, float vDegreesMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pv = &vRad;
			return ::ImGui::SliderAngle(pLabel.c_str(), pv, vDegreesMin, vDegreesMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt(System::String^ label, int% v, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &v;
			return ::ImGui::SliderInt(pLabel.c_str(), pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt2(System::String^ label, System::Span<int> v, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt2(pLabel.c_str(), pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt3(System::String^ label, System::Span<int> v, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 3) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt3(pLabel.c_str(), pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool SliderInt4(System::String^ label, System::Span<int> v, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			if (v.Length < 4) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::SliderInt4(pLabel.c_str(), pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool SliderScalar(System::String^ label, T% data, T min, T max, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<T> pData = &data, pMin = &min, pMax = &max;
			return ::ImGui::SliderScalar(pLabel.c_str(), GetDataType<T>(), pData, pMin, pMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool SliderScalarN(System::String^ label, System::Span<T> data, T min, T max, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<T> pData = &MemoryMarshal::GetReference(data), pMin = &min, pMax = &max;
			return ::ImGui::SliderScalarN(pLabel.c_str(), GetDataType<T>(), pData, data.Length, pMin, pMax, pFormat.c_str(), (ImGuiSliderFlags)flags);;
		}

		virtual bool VSliderFloat(System::String^ label, System::Numerics::Vector2 size, float% v, float vMin, float vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pv = &v;
			return ::ImGui::VSliderFloat(pLabel.c_str(), {size.X, size.Y}, pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		virtual bool VSliderInt(System::String^ label, System::Numerics::Vector2 size, int% v, int vMin, int vMax, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pv = &v;
			return ::ImGui::VSliderInt(pLabel.c_str(), {size.X, size.Y}, pv, vMin, vMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool VSliderScalar(System::String^ label, System::Numerics::Vector2 size, T% data, T min, T max, System::String^ format, Tesseract::ImGui::ImGuiSliderFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<T> pData = &data, pMin = &min, pMax = &max;
			return ::ImGui::VSliderScalar(pLabel.c_str(), { size.X, size.Y }, GetDataType<T>(), pData, pMin, pMax, pFormat.c_str(), (ImGuiSliderFlags)flags);
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
		virtual bool InputText(System::String^ label, Tesseract::ImGui::ImGuiTextBuffer^ buf, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			StringParam pLabel(label);
			pin_ptr<unsigned char> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputText(pLabel.c_str(), (char*)pBuf, buf->Buf.Length, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}

		virtual bool InputTextMultiline(System::String^ label, Tesseract::ImGui::ImGuiTextBuffer^ buf, System::Numerics::Vector2 size, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			StringParam pLabel(label);
			pin_ptr<unsigned char> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputTextMultiline(pLabel.c_str(), (char*)pBuf, buf->Buf.Length, { size.X, size.Y }, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}
		
		virtual bool InputTextWithHint(System::String^ label, System::String^ hint, Tesseract::ImGui::ImGuiTextBuffer^ buf, Tesseract::ImGui::ImGuiInputTextFlags flags, Tesseract::ImGui::ImGuiInputTextCallback^ callback) {
			StringParam pLabel(label), pHint(hint);
			pin_ptr<unsigned char> pBuf = &MemoryMarshal::GetReference(buf->Buf);
			inputTextFn = callback;
			return ::ImGui::InputTextWithHint(pLabel.c_str(), pHint.c_str(), (char*)pBuf, buf->Buf.Length, (ImGuiInputTextFlags)flags, (ImGuiInputTextCallback)inputTextCbk);
		}
		
		virtual bool InputFloat(System::String^ label, float% v, float step, float stepFast, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pv = &v;
			return ::ImGui::InputFloat(pLabel.c_str(), pv, step, stepFast, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat2(System::String^ label, System::Numerics::Vector2% v, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector2> pv = &v;
			return ::ImGui::InputFloat2(pLabel.c_str(), (float*)pv, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat3(System::String^ label, System::Numerics::Vector3% v, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector3> pv = &v;
			return ::ImGui::InputFloat3(pLabel.c_str(), (float*)pv, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputFloat4(System::String^ label, System::Numerics::Vector4% v, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<Vector4> pv = &v;
			return ::ImGui::InputFloat4(pLabel.c_str(), (float*)pv, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt(System::String^ label, int% v, int step, int stepFast, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label);
			pin_ptr<int> pv = &v;
			return ::ImGui::InputInt(pLabel.c_str(), pv, step, stepFast, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt2(System::String^ label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 2) throw gcnew System::ArgumentException("Value span must have length >= 2", "v");
			StringParam pLabel(label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt2(pLabel.c_str(), pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt3(System::String^ label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 3) throw gcnew System::ArgumentException("Value span must have length >= 3", "v");
			StringParam pLabel(label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt3(pLabel.c_str(), pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputInt4(System::String^ label, System::Span<int> v, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			if (v.Length < 4) throw gcnew System::ArgumentException("Value span must have length >= 4", "v");
			StringParam pLabel(label);
			pin_ptr<int> pv = &MemoryMarshal::GetReference(v);
			return ::ImGui::InputInt4(pLabel.c_str(), pv, (ImGuiInputTextFlags)flags);
		}
		
		virtual bool InputDouble(System::String^ label, double% v, double step, double stepFast, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<double> pv = &v;
			return ::ImGui::InputDouble(pLabel.c_str(), pv, step, stepFast, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool InputScalar(System::String^ label, T% data, Tesseract::ImGui::ImNullable<T> step, Tesseract::ImGui::ImNullable<T> stepFast, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
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
			return ::ImGui::InputScalar(pLabel.c_str(), GetDataType<T>(), pData, pStep, pStepFast, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}

		generic<typename T>
		where T : value class, gcnew()
		virtual bool InputScalarN(System::String^ label, System::Span<T> data, Tesseract::ImGui::ImNullable<T> step, Tesseract::ImGui::ImNullable<T> stepFast, System::String^ format, Tesseract::ImGui::ImGuiInputTextFlags flags) {
			StringParam pLabel(label), pFormat(format);
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
			return ::ImGui::InputScalarN(pLabel.c_str(), GetDataType<T>(), pData, data.Length, pStep, pStepFast, pFormat.c_str(), (ImGuiInputTextFlags)flags);
		}

		// Widgets: Color Editor/Picker (tip: the ColorEdit* functions have a little color square that can be left-clicked to open a picker, and right-clicked to open an option menu.)
		// - Note that in C++ a 'float v[X]' function argument is the _same_ as 'float* v', the array syntax is just a way to document the number of elements that are expected to be accessible.
		// - You can pass the address of a first float element out of a contiguous structure, e.g. &myvector.x

		virtual bool ColorEdit3(System::String^ label, System::Numerics::Vector3% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			StringParam pLabel(label);
			pin_ptr<Vector3> pCol = &col;
			return ::ImGui::ColorEdit3(pLabel.c_str(), (float*)pCol, (ImGuiColorEditFlags)flags);
		}

		virtual bool ColorEdit4(System::String^ label, System::Numerics::Vector4% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			StringParam pLabel(label);
			pin_ptr<Vector4> pCol = &col;
			return ::ImGui::ColorEdit4(pLabel.c_str(), (float*)pCol, (ImGuiColorEditFlags)flags);
		}
		
		virtual bool ColorPicker3(System::String^ label, System::Numerics::Vector3% col, Tesseract::ImGui::ImGuiColorEditFlags flags) {
			StringParam pLabel(label);
			pin_ptr<Vector3> pCol = &col;
			return ::ImGui::ColorPicker3(pLabel.c_str(), (float*)pCol, (ImGuiColorEditFlags)flags);
		}
		
		virtual bool ColorPicker4(System::String^ label, System::Numerics::Vector4% col, Tesseract::ImGui::ImGuiColorEditFlags flags, System::Nullable<System::Numerics::Vector4> refCol) {
			StringParam pLabel(label);
			pin_ptr<Vector4> pCol = &col;
			Vector4 vRef = {};
			pin_ptr<Vector4> pRef = nullptr;
			if (refCol.HasValue) {
				vRef = refCol.Value;
				pRef = &vRef;
			}
			return ::ImGui::ColorPicker4(pLabel.c_str(), (float*)pCol, (ImGuiColorEditFlags)flags, (float*)pRef);
		}
		
		virtual bool ColorButton(System::String^ descId, System::Numerics::Vector4 col, Tesseract::ImGui::ImGuiColorEditFlags flags, System::Numerics::Vector2 size) {
			StringParam pDescID(descId);
			return ::ImGui::ColorButton(pDescID.c_str(), { col.X, col.Y, col.Z, col.W }, (ImGuiColorEditFlags)flags, { size.X, size.Y });
		}
		
		virtual void SetColorEditOptions(Tesseract::ImGui::ImGuiColorEditFlags flags) {
			::ImGui::SetColorEditOptions((ImGuiColorEditFlags)flags);
		}

		// Widgets: Trees
		// - TreeNode functions return true when the node is open, in which case you need to also call TreePop() when you are finished displaying the tree node contents.

		virtual bool TreeNode(System::String^ label) {
			StringParam pLabel(label);
			return ::ImGui::TreeNode(pLabel.c_str());
		}

		virtual bool TreeNode(System::String^ strID, System::String^ fmt) {
			StringParam pStrID(strID), pFmt(fmt, true);
			return ::ImGui::TreeNode(pStrID.c_str(), pFmt.c_str());
		}

		virtual bool TreeNode(System::IntPtr ptrID, System::String^ fmt) {
			StringParam pFmt(fmt, true);
			return ::ImGui::TreeNode((void*)ptrID, pFmt.c_str());
		}

		virtual bool TreeNodeEx(System::String^ label, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			StringParam pLabel(label);
			return ::ImGui::TreeNodeEx(pLabel.c_str(), (ImGuiTreeNodeFlags)flags);
		}

		virtual bool TreeNodeEx(System::String^ strID, Tesseract::ImGui::ImGuiTreeNodeFlags flags, System::String^ fmt) {
			StringParam pStrID(strID), pFmt(fmt, true);
			return ::ImGui::TreeNodeEx(pStrID.c_str(), (ImGuiTreeNodeFlags)flags, pFmt.c_str());
		}

		virtual bool TreeNodeEx(System::IntPtr ptrID, Tesseract::ImGui::ImGuiTreeNodeFlags flags, System::String^ fmt) {
			StringParam pFmt(fmt, true);
			return ::ImGui::TreeNodeEx((void*)ptrID, (ImGuiTreeNodeFlags)flags, pFmt.c_str());
		}

		virtual void TreePush(System::String^ strID) {
			StringParam pStrID(strID);
			::ImGui::TreePush(pStrID.c_str());
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

		virtual bool CollapsingHeader(System::String^ label, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			StringParam pLabel(label);
			return ::ImGui::CollapsingHeader(pLabel.c_str(), (ImGuiTreeNodeFlags)flags);
		}

		virtual bool CollapsingHeader(System::String^ label, bool% visible, Tesseract::ImGui::ImGuiTreeNodeFlags flags) {
			StringParam pLabel(label);
			pin_ptr<bool> pVisible = &visible;
			return ::ImGui::CollapsingHeader(pLabel.c_str(), pVisible, (ImGuiTreeNodeFlags)flags);
		}

		virtual void SetNextItemOpen(bool isOpen, Tesseract::ImGui::ImGuiCond cond) {
			::ImGui::SetNextItemOpen(isOpen, (ImGuiCond)cond);
		}

		// Widgets: Selectables
		// - A selectable highlights when hovered, and can display another color when selected.
		// - Neighbors selectable extend their highlight bounds in order to leave no gap between them. This is so a series of selected Selectable appear contiguous.

		virtual bool Selectable(System::String^ label, bool selected, Tesseract::ImGui::ImGuiSelectableFlags flags, System::Numerics::Vector2 size) {
			StringParam pLabel(label);
			return ::ImGui::Selectable(pLabel.c_str(), selected, (ImGuiSelectableFlags)flags, { size.X, size.Y });
		}

		virtual bool Selectable(System::String^ label, bool% selected, Tesseract::ImGui::ImGuiSelectableFlags flags, System::Numerics::Vector2 size) {
			StringParam pLabel(label);
			pin_ptr<bool> pSelected = &selected;
			return ::ImGui::Selectable(pLabel.c_str(), pSelected, (ImGuiSelectableFlags)flags, { size.X, size.Y });
		}

		// Widgets: List Boxes
		// - This is essentially a thin wrapper to using BeginChild/EndChild with some stylistic changes.
		// - The BeginListBox()/EndListBox() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() or any items.
		// - The simplified/old ListBox() api are helpers over BeginListBox()/EndListBox() which are kept available for convenience purpose. This is analoguous to how Combos are created.
		// - Choose frame width:   size.x > 0.0f: custom  /  size.x < 0.0f or -FLT_MIN: right-align   /  size.x = 0.0f (default): use current ItemWidth
		// - Choose frame height:  size.y > 0.0f: custom  /  size.y < 0.0f or -FLT_MIN: bottom-align  /  size.y = 0.0f (default): arbitrary default height which can fit ~7 items

		virtual bool BeginListBox(System::String^ label, System::Numerics::Vector2 size) {
			StringParam pLabel(label);
			return ::ImGui::BeginListBox(pLabel.c_str(), { size.X, size.Y });
		}

		virtual void EndListBox() {
			::ImGui::EndListBox();
		}

		virtual bool ListBox(System::String^ label, int% currentItem, System::Collections::Generic::IEnumerable<System::String^>^ items, int heightInItems) {
			StringParam pLabel(label);
			StringArrayParam pItems(items);
			pin_ptr<int> pCurrentItem = &currentItem;
			return ::ImGui::ListBox(pLabel.c_str(), pCurrentItem, pItems.data(), (int)pItems.length(), heightInItems);
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
		virtual bool ListBox(System::String^ label, int% currentItem, Tesseract::ImGui::IImGui::ListBoxItemsGetter^ itemsGetter, int itemsCount, int heightInItems) {
			StringParam pLabel(label);
			pin_ptr<int> pCurrentItem = &currentItem;
			listBoxGetterFn = itemsGetter;
			return ::ImGui::ListBox(pLabel.c_str(), pCurrentItem, (bool(*)(void*, int, const char**))listBoxGetterCbk, nullptr, itemsCount, heightInItems);
		}

		// Widgets: Data Plotting
		// - Consider using ImPlot (https://github.com/epezent/implot) which is much better!

	private:
		static System::Func<int, float>^ plotValuesGetterFn = nullptr;
		static float plotValuesGetterCbk(void*, int idx) {
			return plotValuesGetterFn->Invoke(idx);
		}

	public:
		virtual void PlotLines(System::String^ label, System::ReadOnlySpan<float> values, int valuesCount, System::String^ overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize, int stride) {
			StringParam pLabel(label);
			pin_ptr<const float> pValues = &MemoryMarshal::GetReference(values);
			StringParam pOverlayText(overlayText);
			::ImGui::PlotLines(pLabel.c_str(), (const float*)pValues, valuesCount < 0 ? values.Length : valuesCount, 0, pOverlayText.c_str(), scaleMin, scaleMax, { graphSize.X, graphSize.Y }, stride * sizeof(float));
		}

		virtual void PlotLines(System::String^ label, System::Func<int, float>^ valuesGetter, int valuesCount, int valuesOffset, System::String^ overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize) {
			StringParam pLabel(label);
			StringParam pOverlayText(overlayText);
			plotValuesGetterFn = valuesGetter;
			::ImGui::PlotLines(pLabel.c_str(), (float(*)(void*,int))plotValuesGetterCbk, nullptr, valuesCount, valuesOffset, pOverlayText.c_str(), scaleMin, scaleMax, { graphSize.X, graphSize.Y });
		}

		virtual void PlotHistogram(System::String^ label, System::ReadOnlySpan<float> values, int valuesCount, System::String^ overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize, int stride) {
			StringParam pLabel(label);
			pin_ptr<const float> pValues = &MemoryMarshal::GetReference(values);
			StringParam pOverlayText(overlayText);
			::ImGui::PlotHistogram(pLabel.c_str(), (const float*)pValues, valuesCount < 0 ? values.Length : valuesCount, 0, pOverlayText.c_str(), scaleMin, scaleMax, { graphSize.X, graphSize.Y }, stride * sizeof(float));
		}

		virtual void PlotHistogram(System::String^ label, System::Func<int, float>^ valuesGetter, int valuesCount, int valuesOffset, System::String^ overlayText, float scaleMin, float scaleMax, System::Numerics::Vector2 graphSize) {
			StringParam pLabel(label);
			StringParam pOverlayText(overlayText);
			plotValuesGetterFn = valuesGetter;
			::ImGui::PlotHistogram(pLabel.c_str(), (float(*)(void*, int))plotValuesGetterCbk, nullptr, valuesCount, valuesOffset, pOverlayText.c_str(), scaleMin, scaleMax, { graphSize.X, graphSize.Y });
		}

		// Widgets: Value() Helpers.
		// - Those are merely shortcut to calling Text() with a format string. Output single value in "name: value" format (tip: freely declare more in your code to handle your types. you can add functions to the ImGui namespace)

		virtual void Value(System::String^ prefix, bool b) {
			StringParam pPrefix(prefix);
			::ImGui::Value(pPrefix.c_str(), b);
		}

		virtual void Value(System::String^ prefix, int v) {
			StringParam pPrefix(prefix);
			::ImGui::Value(pPrefix.c_str(), v);
		}

		virtual void Value(System::String^ prefix, unsigned int v) {
			StringParam pPrefix(prefix);
			::ImGui::Value(pPrefix.c_str(), v);
		}

		virtual void Value(System::String^ prefix, float v, System::String^ floatFormat) {
			StringParam pPrefix(prefix), pFloatFormat(floatFormat);
			::ImGui::Value(pPrefix.c_str(), v, pFloatFormat.c_str());
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

		virtual bool BeginMenu(System::String^ label, bool enabled) {
			StringParam pLabel(label);
			return ::ImGui::BeginMenu(pLabel.c_str(), enabled);
		}

		virtual void EndMenu() {
			::ImGui::EndMenu();
		}

		virtual bool MenuItem(System::String^ label, System::String^ shortcut, bool selected, bool enabled) {
			StringParam pLabel(label), pShortcut(shortcut);
			return ::ImGui::MenuItem(pLabel.c_str(), pShortcut.c_str(), selected, enabled);
		}

		virtual bool MenuItem(System::String^ label, System::String^ shortcut, bool% selected, bool enabled) {
			StringParam pLabel(label), pShortcut(shortcut);
			pin_ptr<bool> pSelected = &selected;
			return ::ImGui::MenuItem(pLabel.c_str(), pShortcut.c_str(), pSelected, enabled);
		}

		// Tooltips
		// - Tooltip are windows following the mouse. They do not take focus away.

		virtual void BeginTooltip() {
			::ImGui::BeginTooltip();
		}

		virtual void EndTooltip() {
			::ImGui::EndTooltip();
		}

		virtual void SetTooltip(System::String^ fmt) {
			StringParam pFmt(fmt, true);
			::ImGui::SetTooltip(pFmt.c_str());
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

		virtual bool BeginPopup(System::String^ strID, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pStrID(strID);
			return ::ImGui::BeginPopup(pStrID.c_str(), (ImGuiWindowFlags)flags);
		}

		virtual bool BeginPopupModal(System::String^ name, bool% open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pName(name);
			pin_ptr<bool> pOpen = &open;
			return ::ImGui::BeginPopupModal(pName.c_str(), pOpen, (ImGuiWindowFlags)flags);
		}
		
		virtual bool BeginPopupModal(System::String^ name, System::Nullable<bool> open, Tesseract::ImGui::ImGuiWindowFlags flags) {
			StringParam pName(name);
			bool vOpen, *pOpen = nullptr;
			if (open.HasValue) {
				vOpen = open.Value;
				pOpen = &vOpen;
			}
			return ::ImGui::BeginPopupModal(pName.c_str(), pOpen, (ImGuiWindowFlags)flags);
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

		virtual void OpenPopup(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			StringParam pStrID(strID);
			::ImGui::OpenPopup(pStrID.c_str(), (ImGuiPopupFlags)popupFlags);
		}

		virtual void OpenPopup(unsigned int id, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			::ImGui::OpenPopup(id, (ImGuiPopupFlags)popupFlags);
		}
		
		virtual void OpenPopupOnItemClick(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			StringParam pStrID(strID);
			::ImGui::OpenPopupOnItemClick(pStrID.c_str(), (ImGuiPopupFlags)popupFlags);
		}
		
		virtual void CloseCurrentPopup() {
			::ImGui::CloseCurrentPopup();
		}

		// Popups: open+begin combined functions helpers
		//  - Helpers to do OpenPopup+BeginPopup where the Open action is triggered by e.g. hovering an item and right-clicking.
		//  - They are convenient to easily create context menus, hence the name.
		//  - IMPORTANT: Notice that BeginPopupContextXXX takes ImGuiPopupFlags just like OpenPopup() and unlike BeginPopup(). For full consistency, we may add ImGuiWindowFlags to the BeginPopupContextXXX functions in the future.
		//  - IMPORTANT: Notice that we exceptionally default their flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter, so if you add other flags remember to re-add the ImGuiPopupFlags_MouseButtonRight.

		virtual bool BeginPopupContextItem(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			StringParam pStrID(strID);
			return ::ImGui::BeginPopupContextItem(pStrID.c_str(), (ImGuiPopupFlags)popupFlags);
		}

		virtual bool BeginPopupContextWindow(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			StringParam pStrID(strID);
			return ::ImGui::BeginPopupContextWindow(pStrID.c_str(), (ImGuiPopupFlags)popupFlags);
		}

		virtual bool BeginPopupContextVoid(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags popupFlags) {
			StringParam pStrID(strID);
			return ::ImGui::BeginPopupContextVoid(pStrID.c_str(), (ImGuiPopupFlags)popupFlags);
		}

		// Popups: query functions
		//  - IsPopupOpen(): return true if the popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId: return true if any popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId + ImGuiPopupFlags_AnyPopupLevel: return true if any popup is open.

		virtual bool IsPopupOpen(System::String^ strID, Tesseract::ImGui::ImGuiPopupFlags flags) {
			StringParam pStrID(strID);
			return ::ImGui::IsPopupOpen(pStrID.c_str(), (ImGuiPopupFlags)flags);
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

		virtual bool BeginTable(System::String^ strID, int column, Tesseract::ImGui::ImGuiTableFlags flags, System::Numerics::Vector2 outerSize, float innerWidth) {
			StringParam pStrID(strID);
			return ::ImGui::BeginTable(pStrID.c_str(), column, (ImGuiTableFlags)flags, { outerSize.X, outerSize.Y }, innerWidth);
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

		virtual void TableSetupColumn(System::String^ label, Tesseract::ImGui::ImGuiTableColumnFlags flags, float initWidthOrWeight, unsigned int userID) {
			StringParam pLabel(label);
			::ImGui::TableSetupColumn(pLabel.c_str(), (ImGuiTableColumnFlags)flags, initWidthOrWeight, userID);
		}
		
		virtual void TableSetupScrollFreeze(int cols, int rows) {
			::ImGui::TableSetupScrollFreeze(cols, rows);
		}
		
		virtual void TableHeadersRow() {
			::ImGui::TableHeadersRow();
		}
		
		virtual void TableHeader(System::String^ label) {
			StringParam pLabel(label);
			::ImGui::TableHeader(pLabel.c_str());
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

		virtual void Columns(int count, System::String^ id, bool border) {
			StringParam pID(id);
			::ImGui::Columns(count, pID.c_str(), border);
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

		virtual bool BeginTabBar(System::String^ strID, Tesseract::ImGui::ImGuiTabBarFlags flags) {
			StringParam pStrID(strID);
			return ::ImGui::BeginTabBar(pStrID.c_str(), (ImGuiTabBarFlags)flags);
		}
		
		virtual void EndTabBar() {
			::ImGui::EndTabBar();
		}
		
		virtual bool BeginTabItem(System::String^ label, bool% open, Tesseract::ImGui::ImGuiTabItemFlags flags) {
			StringParam pLabel(label);
			pin_ptr<bool> pOpen = &open;
			return ::ImGui::BeginTabItem(pLabel.c_str(), pOpen, (ImGuiTabItemFlags)flags);
		}
		
		virtual bool BeginTabItem(System::String^ label, System::Nullable<bool> open, Tesseract::ImGui::ImGuiTabItemFlags flags) {
			StringParam pLabel(label);
			bool vOpen, *pOpen = nullptr;
			if (open.HasValue) {
				vOpen = open.Value;
				pOpen = &vOpen;
			}
			return ::ImGui::BeginTabItem(pLabel.c_str(), pOpen, (ImGuiTabItemFlags)flags);
		}
		
		virtual void EndTabItem() {
			::ImGui::EndTabItem();
		}
		
		virtual bool TabItemButton(System::String^ label, Tesseract::ImGui::ImGuiTabItemFlags flags) {
			StringParam pLabel(label);
			return ::ImGui::TabItemButton(pLabel.c_str(), (ImGuiTabItemFlags)flags);
		}

		virtual void SetTabItemClosed(System::String^ tabOrDockedWindowLabel) {
			StringParam pTabOrDockedWindowLabel(tabOrDockedWindowLabel);
			::ImGui::SetTabItemClosed(pTabOrDockedWindowLabel.c_str());
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
			StringParam pFmt(fmt, true);
			::ImGui::LogText(pFmt.c_str());
		}

		// Drag and Drop
		// - On source items, call BeginDragDropSource(), if it returns true also call SetDragDropPayload() + EndDragDropSource().
		// - On target candidates, call BeginDragDropTarget(), if it returns true also call AcceptDragDropPayload() + EndDragDropTarget().
		// - If you stop calling BeginDragDropSource() the payload is preserved however it won't have a preview tooltip (we currently display a fallback "..." tooltip, see #1725)
		// - An item can be both drag source and drop target.

		virtual bool BeginDragDropSource(Tesseract::ImGui::ImGuiDragDropFlags flags) {
			return ::ImGui::BeginDragDropSource((ImGuiDragDropFlags)flags);
		}
		
		virtual bool SetDragDropPayload(System::String^ type, System::ReadOnlySpan<unsigned char> data, Tesseract::ImGui::ImGuiCond cond) {
			StringParam pType(type);
			pin_ptr<unsigned char> pData = &MemoryMarshal::GetReference(data);
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

		virtual System::Numerics::Vector2 CalcTextSize(System::String^ text, bool hideTextAfterDoubleHash, float wrapWidth) {
			StringParam pText(text);
			auto retn = ::ImGui::CalcTextSize(pText.begin(), pText.end(), hideTextAfterDoubleHash, wrapWidth);
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

		virtual int GetMouseClickedAmount(Tesseract::ImGui::ImGuiMouseButton button) {
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

		virtual void LoadIniSettingsFromMemory(System::ReadOnlySpan<unsigned char> iniData) {
			pin_ptr<const unsigned char> pIniData = &MemoryMarshal::GetReference(iniData);
			::ImGui::LoadIniSettingsFromMemory((const char*)pIniData, iniData.Length);
		}

		virtual void SaveIniSettingsToDisk(System::String^ iniFilename) {
			StringParam pIniFilename(iniFilename);
			::ImGui::SaveIniSettingsToDisk(pIniFilename.c_str());
		}

		virtual System::ReadOnlySpan<unsigned char> SaveIniSettingsToMemory() {
			size_t size = 0;
			const char* pData = ::ImGui::SaveIniSettingsToMemory(&size);
			return ReadOnlySpan<unsigned char>((void*)pData, (int)size);
		}

	};

	void g_customDrawCallback(const ImDrawList* parentList, const ImDrawCmd* cmd) {
		Tesseract::ImGui::ImDrawCallback^ cbk = DrawCallbackHolder::Instance->Get(cmd->UserCallbackData);
		if (cbk) {
			cbk->Invoke(gcnew ImDrawListCLI(const_cast<ImDrawList*>(parentList), false), ImDrawListCLI::ConvertCmd(*cmd));
		}
	}

}}}
