#include "imgui_notify.h"
#include "imgui_cli.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;


namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	/// <summary>
	/// Enumeration of types of toasts.
	/// </summary>
	public enum ImGuiToastType {
		/// <summary>
		/// The toast is of no specific type.
		/// </summary>
		None = ImGuiToastType_None,
		/// <summary>
		/// The toast indicates success.
		/// </summary>
		Success = ImGuiToastType_Success,
		/// <summary>
		/// The toast indicates a warning.
		/// </summary>
		Warning = ImGuiToastType_Warning,
		/// <summary>
		/// The toast indicates an error.
		/// </summary>
		Error = ImGuiToastType_Error,
		/// <summary>
		/// The toast presents some information.
		/// </summary>
		Info = ImGuiToastType_Info
	};

	/// <summary>
	/// Provides methods for creating 'toast'-style notifications.
	/// </summary>
	public ref class ImGuiToast {
	private:
		::ImGuiToast* m_toast;

	public:
		/// <summary>
		/// The type of the notification.
		/// </summary>
		property ImGuiToastType Type {
			ImGuiToastType get() { return (ImGuiToastType)m_toast->get_type(); }
			void set(ImGuiToastType value) { m_toast->set_type((::ImGuiToastType)value); }
		}

		/// <summary>
		/// The title of the notification.
		/// </summary>
		property String^ Title {
			String^ get() { return gcnew String(m_toast->get_title()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_toast->set_title("%s", pValue.c_str());
			}
		}

		/// <summary>
		/// The content of the notification.
		/// </summary>
		property String^ Content {
			String^ get() { return gcnew String(m_toast->get_content()); }
			void set(String^ value) {
				StringParam pValue(value);
				m_toast->set_content("%s", pValue.c_str());
			}
		}

		/// <summary>
		/// The time until the notification will be dismissed.
		/// </summary>
		property TimeSpan DismissTime {
			TimeSpan get() { return TimeSpan::FromMilliseconds(m_toast->get_dismiss_time()); }
			void set(TimeSpan value) { m_toast->set_dismiss_time((int)value.TotalMilliseconds); }
		}

		ImGuiToast() {
			m_toast = new ::ImGuiToast(ImGuiToastType_None);
		}

		~ImGuiToast() {
			delete m_toast;
		}

		/// <summary>
		/// Inserts this notification at the front of the queue.
		/// </summary>
		void Insert() {
			::ImGui::InsertNotification(*m_toast);
		}

		/// <summary>
		/// The total number of active notifications.
		/// </summary>
		static property int NumNotifications {
			int get() { return (int)::ImGui::notifications.size(); }
		}

		/// <summary>
		/// Removes the notification at the given index.
		/// </summary>
		/// <param name="index">Index of notification to remove</param>
		static void RemoveNotification(int index) {
			if (index < 0 || index >= ::ImGui::notifications.size()) throw gcnew System::IndexOutOfRangeException();
			::ImGui::RemoveNotification(index);
		}

		/// <summary>
		/// Renders the queue of notifications.
		/// </summary>
		static void RenderNotifications() {
			::ImGui::RenderNotifications();
		}

		/// <summary>
		/// Merges the notification icons into the font atlas. This should be called once at initialization.
		/// </summary>
		/// <param name="fontSize">The size of the standard font</param>
		static void MergeIconsWithLatestFont(float fontSize) { ::ImGui::MergeIconsWithLatestFont(fontSize, false); }

	};

}}}}