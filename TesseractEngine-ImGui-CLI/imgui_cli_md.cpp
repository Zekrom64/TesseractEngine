#include "imgui_md.h"
#include "imgui_cli.h"
#include <gcroot.h>
#include "imgui_cli_font.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;
using namespace System::Runtime::InteropServices;

inline Vector4 cvtvec(ImVec4 v) { return Vector4(v.x, v.y, v.z, v.w); }
inline Vector2 cvtvec(ImVec2 v) { return Vector2(v.x, v.y); }
inline ImVec4 cvtvec(Vector4 v) { return { v.X, v.Y, v.Z, v.W }; }
inline ImVec2 cvtvec(Vector2 v) { return { v.X, v.Y }; }

namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	public value struct ImGuiMDImageInfo {
		UIntPtr TextureId;
		Vector2 Size;
		Vector2 UV0;
		Vector2 UV1;
		Vector4 ColorTint;
		Vector4 ColorBorder;
	};

	class imgui_md_impl;

	public ref class ImGuiMD abstract {
	private:
		imgui_md_impl* m_md;
	protected:

		virtual bool GetImage(String^ href, [OutAttribute] ImGuiMDImageInfo% info);
		virtual ImFontCLI^ GetFont();
		virtual Vector4 GetColor();

		/// <summary>
		/// Called to open the given URL in the current Markdown document when clicked.
		/// </summary>
		/// <param name="url">The URL to open</param>
		virtual void OpenURL(String^ url) { }

		/// <summary>
		/// Called when a newline is encountered in the Markdown source where it is not semantically meaningful.
		/// </summary>
		virtual void SoftBreak() { }

		/// <summary>
		/// Called when an HTML div tag is encountered, passing the 'class' attribute given to it.
		/// </summary>
		/// <param name="htmlClass">The class of the div</param>
		/// <param name="enter">If the tag is a begin or end tag</param>
		virtual void HTMLDiv(String^ htmlClass, bool enter) { }

		property bool IsUnderline { bool get(); void set(bool v); }
		property bool IsStrikethrough { bool get(); void set(bool v); }
		property bool IsEm { bool get(); void set(bool v); }
		property bool IsStrong { bool get(); void set(bool v); }
		property bool IsTableHeader { bool get(); void set(bool v); }
		property bool IsTableBody { bool get(); void set(bool v); }
		property bool IsImage { bool get(); void set(bool v); }
		property bool IsCode { bool get(); void set(bool v); }
		property int HLevel { int get(); void set(int v); }

	internal:
		bool CallGetImage(const char* href, ImGuiMDImageInfo% info) {
			return GetImage(gcnew String(href), info);
		}

		ImFont* CallGetFont() {
			return GetFont()->m_font;
		}

		ImVec4 CallGetColor() { return cvtvec(GetColor()); }

		void CallOpenURL(const char* url) { OpenURL(gcnew String(url)); }
		void CallSoftBreak() { SoftBreak(); }
		void CallHTMLDiv(const char* htmlClass, bool enter) { HTMLDiv(gcnew String(htmlClass), enter); }

	public:
		ImGuiMD();
		~ImGuiMD();

		/// <summary>
		/// Renders the given UTF-8 encoded Markdown text.
		/// </summary>
		/// <param name="text">Text to render</param>
		/// <returns>If rendering succeeded</returns>
		bool Print(ReadOnlySpan<uint8_t> text);

		/// <summary>
		/// Renders the given Markdown text.
		/// </summary>
		/// <param name="text">Tex tto render</param>
		/// <returns>If rendering succeeded</returns>
		bool Print(String^ text);
	};

	class imgui_md_impl : public imgui_md {
	private:
		gcroot<ImGuiMD^> m_md;
	protected:
		virtual bool get_image(image_info& info) const override {
			ImGuiMDImageInfo nfo;
			bool ret = m_md->CallGetImage(m_href.c_str(), nfo);
			info.texture_id = (ImTextureID)nfo.TextureId;
			info.size = cvtvec(nfo.Size);
			info.uv0 = cvtvec(nfo.UV0);
			info.uv1 = cvtvec(nfo.UV1);
			info.col_tint = cvtvec(nfo.ColorTint);
			info.col_border = cvtvec(nfo.ColorBorder);
			return ret;
		}

		virtual ImFont* get_font() const override {
			return m_md->CallGetFont();
		}

		virtual ImVec4 get_color() const override {
			return m_md->CallGetColor();
		}

		virtual void open_url() const {
			m_md->CallOpenURL(m_href.c_str());
		}

		virtual void soft_break() {
			m_md->CallSoftBreak();
		}

		virtual void html_div(const std::string& dclass, bool e) {
			m_md->CallHTMLDiv(dclass.c_str(), e);
		}

	public:
#define DECLACCESSOR(NAME) \
	inline bool get_##NAME() const { return m_##NAME; } \
	inline void set_##NAME(bool v) { m_##NAME = v; }

		DECLACCESSOR(is_underline)
		DECLACCESSOR(is_strikethrough)
		DECLACCESSOR(is_em)
		DECLACCESSOR(is_strong)
		DECLACCESSOR(is_table_header)
		DECLACCESSOR(is_table_body)
		DECLACCESSOR(is_image)
		DECLACCESSOR(is_code)

		inline unsigned int get_hlevel() const { return m_hlevel; }
		inline void set_hlevel(unsigned int v) { m_hlevel = v; }

		inline ImFont* base_get_font() { return imgui_md::get_font(); }
		inline ImVec4 base_get_color() { return imgui_md::get_color(); }

		imgui_md_impl(ImGuiMD^ md) {
			m_md = md;
		}

		~imgui_md_impl() {
			m_md = nullptr;
		}
	};

	ImGuiMD::ImGuiMD() {
		m_md = new imgui_md_impl(this);
	}

	ImGuiMD::~ImGuiMD() {
		delete m_md;
	}
	
	bool ImGuiMD::GetImage(String^ href, [OutAttribute] ImGuiMDImageInfo% info) {
		return false;
	}

	ImFontCLI^ ImGuiMD::GetFont() {
		return gcnew ImFontCLI(m_md->base_get_font());
	}

	Vector4 ImGuiMD::GetColor() {
		return cvtvec(m_md->base_get_color());
	}
	
	bool ImGuiMD::Print(ReadOnlySpan<uint8_t> text) {
		IM_SPAN_TO_STR(pText, text);
		return m_md->print(pText, pText + text.Length) == 0;
	}

	bool ImGuiMD::Print(String^ text) {
		StringParam pText(text);
		return m_md->print(pText.c_str(), pText.c_str() + pText.length()) == 0;
	}

#define DECLACCESSOR2(NAME, UNAME) \
	bool ImGuiMD::NAME::get() { return m_md->get_##UNAME(); } \
	void ImGuiMD::NAME::set(bool v) { m_md->set_##UNAME(v); }

	DECLACCESSOR2(IsUnderline, is_underline)
	DECLACCESSOR2(IsStrikethrough, is_strikethrough)
	DECLACCESSOR2(IsEm, is_em)
	DECLACCESSOR2(IsStrong, is_strong)
	DECLACCESSOR2(IsTableHeader, is_table_header)
	DECLACCESSOR2(IsTableBody, is_table_body)
	DECLACCESSOR2(IsImage, is_image)
	DECLACCESSOR2(IsCode, is_code)

	int ImGuiMD::HLevel::get() { return (int)m_md->get_hlevel(); }
	void ImGuiMD::HLevel::set(int v) { m_md->set_hlevel((unsigned)v); }

}}}}
