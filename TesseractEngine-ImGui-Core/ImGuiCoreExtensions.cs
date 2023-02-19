using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.ImGui.Core {

	/// <summary>
	/// Extensions to the <see cref="IImGui"/> interface 
	/// </summary>
	public static class ImGuiCoreExtensions {

		public static void DragInt2(this IImGui imgui, string label, ref Vector2i v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = ImGuiSliderFlags.None) =>
			imgui.DragInt2(label, v.AsSpan, vSpeed, vMin, vMax, format, flags);

		public static void DragInt3(this IImGui imgui, string label, ref Vector3i v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = ImGuiSliderFlags.None) =>
			imgui.DragInt3(label, v.AsSpan, vSpeed, vMin, vMax, format, flags);

		public static void DragInt4(this IImGui imgui, string label, ref Vector4i v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = ImGuiSliderFlags.None) =>
			imgui.DragInt4(label, v.AsSpan, vSpeed, vMin, vMax, format, flags);

		public static bool SliderInt2(this IImGui imgui, string label, ref Vector2i v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			imgui.SliderInt2(label, v.AsSpan, vMin, vMax, format, flags);

		public static bool SliderInt3(this IImGui imgui, string label, ref Vector3i v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			imgui.SliderInt3(label, v.AsSpan, vMin, vMax, format, flags);

		public static bool SliderInt4(this IImGui imgui, string label, ref Vector4i v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			imgui.SliderInt4(label, v.AsSpan, vMin, vMax, format, flags);

		public static bool InputInt2(this IImGui imgui, string label, Vector2i v, ImGuiInputTextFlags flags = default) =>
			imgui.InputInt2(label, v.AsSpan, flags);

		public static bool InputInt3(this IImGui imgui, string label, Vector3i v, ImGuiInputTextFlags flags = default) =>
			imgui.InputInt3(label, v.AsSpan, flags);

		public static bool InputInt4(this IImGui imgui, string label, Vector4i v, ImGuiInputTextFlags flags = default) =>
			imgui.InputInt4(label, v.AsSpan, flags);

	}

}
