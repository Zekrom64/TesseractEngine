#pragma once

#include "imgui_cli.h"
#include "imgui.h"
#include "imgui_cli_font.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;

namespace Tesseract { namespace CLI { namespace ImGui {

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

		virtual void AddMouseButtonEvent(Tesseract::ImGui::ImGuiMouseButton button, bool down) {
			m_io->AddMouseButtonEvent((ImGuiMouseButton)button, down);
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

		virtual void AddInputCharactersUTF8(System::ReadOnlySpan<uint8_t> str) {
			pin_ptr<uint8_t> pStr = &MemoryMarshal::GetReference(str);
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

}}}
