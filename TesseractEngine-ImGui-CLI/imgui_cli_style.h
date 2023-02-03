#pragma once

#include "imgui_cli.h"
#include "imgui.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui {

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

}}}