#pragma once

#include "imgui_cli.h"
#include "imgui.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui {

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

	public ref class ImDrawListCLI : public Tesseract::ImGui::IImDrawList {
	internal:
		ref class CmdBufferImpl : Tesseract::ImGui::Utilities::CLI::ListBase<Tesseract::ImGui::ImDrawCmd> {
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
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<unsigned short>((IReadOnlyList<unsigned short>^)this);
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
				return gcnew Tesseract::ImGui::Utilities::CLI::ListEnumerator<Tesseract::ImGui::ImDrawVert>((IReadOnlyList<Tesseract::ImGui::ImDrawVert>^)this);
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

			virtual property Tesseract::ImGui::ImDrawVert default[int] {
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

	public ref class ImDrawListSharedDataCLI : Tesseract::ImGui::IImDrawListSharedData {
	internal:
		ImDrawListSharedData* m_data;

		ImDrawListSharedDataCLI(ImDrawListSharedData* data) : m_data(data) {}
	};

}}}
