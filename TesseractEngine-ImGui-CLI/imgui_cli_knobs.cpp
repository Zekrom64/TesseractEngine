#include "imgui-knobs.h"
#include "imgui_cli.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;


namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	[FlagsAttribute]
	public enum class ImGuiKnobFlags {
		NoTitle = ImGuiKnobFlags_NoTitle,
		NoInput = ImGuiKnobFlags_NoInput,
		ValueTooltip = ImGuiKnobFlags_ValueTooltip,
		DragHorizontal = ImGuiKnobFlags_DragHorizontal
	};

	public enum class ImGuiKnobVariant {
		Tick = ImGuiKnobVariant_Tick,
		Dot = ImGuiKnobVariant_Dot,
		Wiper = ImGuiKnobVariant_Wiper,
		WiperOnly = ImGuiKnobVariant_WiperOnly,
		WiperDot = ImGuiKnobVariant_WiperDot,
		Stepped = ImGuiKnobVariant_Stepped,
		Space = ImGuiKnobVariant_Space,
	};

	public ref class ImGuiKnobs abstract sealed {
	public:
		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags, int steps) {
			pin_ptr<unsigned char> pLabel = &MemoryMarshal::GetReference(label), pFormat = &MemoryMarshal::GetReference(format);
			pin_ptr<float> pValue = &value;
			::ImGuiKnobs::Knob((const char*)pLabel, pValue, vMin, vMax, speed, format.IsEmpty ? (const char*)pFormat : nullptr, (::ImGuiKnobVariant)variant, size, (::ImGuiKnobFlags)flags, steps);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags) {
			Knob(label, value, vMin, vMax, speed, format, variant, size, flags, 10);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size) {
			Knob(label, value, vMin, vMax, speed, format, variant, size, (ImGuiKnobFlags)0);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant) {
			Knob(label, value, vMin, vMax, speed, format, variant, 0);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed, ReadOnlySpan<Byte> format) {
			Knob(label, value, vMin, vMax, speed, format, ImGuiKnobVariant::Tick);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax, float speed) {
			Knob(label, value, vMin, vMax, speed, ReadOnlySpan<Byte>::Empty);
		}

		static void Knob(ReadOnlySpan<Byte> label, float% value, float vMin, float vMax) {
			Knob(label, value, vMin, vMax, 0);
		}


		static void Knob(String^ label, float% value, float vMin, float vMax, float speed, String^ format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags, int steps) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<float> pValue = &value;
			::ImGuiKnobs::Knob(pLabel.c_str(), pValue, vMin, vMax, speed, pFormat.c_str(), (::ImGuiKnobVariant)variant, size, (::ImGuiKnobFlags)flags, steps);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax, float speed, String^ format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags) {
			Knob(label, value, vMin, vMax, speed, format, variant, size, flags, 10);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax, float speed, String^ format, ImGuiKnobVariant variant, float size) {
			Knob(label, value, vMin, vMax, speed, format, variant, size, (ImGuiKnobFlags)0);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax, float speed, String^ format, ImGuiKnobVariant variant) {
			Knob(label, value, vMin, vMax, speed, format, variant, 0);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax, float speed, String^ format) {
			Knob(label, value, vMin, vMax, speed, format, ImGuiKnobVariant::Tick);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax, float speed) {
			Knob(label, value, vMin, vMax, speed, nullptr);
		}

		static void Knob(String^ label, float% value, float vMin, float vMax) {
			Knob(label, value, vMin, vMax, 0);
		}


		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags, int steps) {
			pin_ptr<unsigned char> pLabel = &MemoryMarshal::GetReference(label), pFormat = &MemoryMarshal::GetReference(format);
			pin_ptr<int> pValue = &value;
			::ImGuiKnobs::KnobInt((const char*)pLabel, pValue, vMin, vMax, speed, format.IsEmpty ? (const char*)pFormat : nullptr, (::ImGuiKnobVariant)variant, size, (::ImGuiKnobFlags)flags, steps);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, size, flags, 10);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant, float size) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, size, (ImGuiKnobFlags)0);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed, ReadOnlySpan<Byte> format, ImGuiKnobVariant variant) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, 0);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed, ReadOnlySpan<Byte> format) {
			KnobInt(label, value, vMin, vMax, speed, format, ImGuiKnobVariant::Tick);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax, float speed) {
			KnobInt(label, value, vMin, vMax, speed, ReadOnlySpan<Byte>::Empty);
		}

		static void KnobInt(ReadOnlySpan<Byte> label, int% value, int vMin, int vMax) {
			KnobInt(label, value, vMin, vMax, 0);
		}


		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed, String^ format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags, int steps) {
			StringParam pLabel(label), pFormat(format);
			pin_ptr<int> pValue = &value;
			::ImGuiKnobs::KnobInt(pLabel.c_str(), pValue, vMin, vMax, speed, pFormat.c_str(), (::ImGuiKnobVariant)variant, size, (::ImGuiKnobFlags)flags, steps);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed, String^ format, ImGuiKnobVariant variant, float size, ImGuiKnobFlags flags) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, size, flags, 10);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed, String^ format, ImGuiKnobVariant variant, float size) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, size, (ImGuiKnobFlags)0);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed, String^ format, ImGuiKnobVariant variant) {
			KnobInt(label, value, vMin, vMax, speed, format, variant, 0);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed, String^ format) {
			KnobInt(label, value, vMin, vMax, speed, format, ImGuiKnobVariant::Tick);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax, float speed) {
			KnobInt(label, value, vMin, vMax, speed, nullptr);
		}

		static void KnobInt(String^ label, int% value, int vMin, int vMax) {
			KnobInt(label, value, vMin, vMax, 0);
		}
	};

}}}}