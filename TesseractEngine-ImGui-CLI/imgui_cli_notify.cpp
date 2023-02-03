#include "imgui_notify.h"
#include "imgui_cli.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;


namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	public enum ImGuiToastType {
		None,
		Success,
		Warning,
		Error,
		Info
	};

	public ref class ImGuiNotify {
	private:
		ImGuiToast* m_toast;

	public:
		property ImGuiToastType Type {
			ImGuiToastType get() { return (ImGuiToastType)m_toast->get_type(); }
			void set(ImGuiToastType value) { m_toast->set_type((::ImGuiToastType)value); }
		}

		property String^ Title {
			String^ get() { return gcnew String(m_toast->get_title()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_toast->set_title("%s", pValue.c_str());
			}
		}

		property String^ Content {
			String^ get() { return gcnew String(m_toast->get_content()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_toast->set_content("%s", pValue.c_str());
			}
		}

		property TimeSpan DismissTime {
			TimeSpan get() { return TimeSpan::FromMilliseconds(m_toast->get_dismiss_time()); }
			void set(TimeSpan value) { m_toast->set_dismiss_time((int)value.TotalMilliseconds); }
		}

		ImGuiNotify() {
			m_toast = new ImGuiToast(ImGuiToastType_None);
		}

		~ImGuiNotify() {
			delete m_toast;
		}

		void Insert() {
			::ImGui::InsertNotification(*m_toast);
		}

		static property int NumNotifications {
			int get() { return (int)::ImGui::notifications.size(); }
		}

		static void RemoveNotification(int index) {
			if (index < 0 || index >= ::ImGui::notifications.size()) throw gcnew System::IndexOutOfRangeException();
			::ImGui::RemoveNotification(index);
		}

		static void RenderNotifications() {
			::ImGui::RenderNotifications();
		}

		static void MergeIconsWithLatestFont(float fontSize, bool fontDataOwnedByAtlas) {
			::ImGui::MergeIconsWithLatestFont(fontSize, fontDataOwnedByAtlas);
		}

		static void MergeIconsWithLatestFont(float fontSize) { MergeIconsWithLatestFont(fontSize, false); }

	};

}}}}