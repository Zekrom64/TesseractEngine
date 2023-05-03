using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETStyle : IImGuiStyle {

		internal readonly ImGuiStylePtr style;
		internal readonly bool allocd;

		internal ImGuiNETStyle(ImGuiStylePtr style) {
			this.style = style;
			allocd = false;
		}

		public ImGuiNETStyle() {
			unsafe {
				style = new ImGuiStylePtr(ImGuiNative.ImGuiStyle_ImGuiStyle());
			}
			allocd = true;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				if (allocd) ImGuiNative.ImGuiStyle_destroy(style.NativePtr);
			}
		}

		public float Alpha { get => style.Alpha; set => style.Alpha = value; }

		public float DisabledAlpha { get => style.DisabledAlpha; set => style.DisabledAlpha = value; }

		public Vector2 WindowPadding { get => style.WindowPadding; set => style.WindowPadding = value; }

		public float WindowRounding { get => style.WindowRounding; set => style.WindowRounding = value; }

		public float WindowBorderSize { get => style.WindowBorderSize; set => style.WindowBorderSize = value; }

		public Vector2 WindowMinSize { get => style.WindowMinSize; set => style.WindowMinSize = value; }

		public Vector2 WindowTitleAlign { get => style.WindowTitleAlign; set => style.WindowTitleAlign = value; }

		public ImGuiDir WindowMenuButtonPosition { get => (ImGuiDir)style.WindowMenuButtonPosition; set => style.WindowMenuButtonPosition = (ImGuiNET.ImGuiDir)value; }

		public float ChildRounding { get => style.ChildRounding; set => style.ChildRounding = value; }

		public float ChildBorderSize { get => style.ChildBorderSize; set => style.ChildBorderSize = value; }

		public float PopupRounding { get => style.PopupRounding; set => style.PopupRounding = value; }

		public float PopupBorderSize { get => style.PopupBorderSize; set => style.PopupBorderSize = value; }

		public Vector2 FramePadding { get => style.FramePadding; set => style.FramePadding = value; }

		public float FrameRounding { get => style.FrameRounding; set => style.FrameRounding = value; }

		public float FrameBorderSize { get => style.FrameBorderSize; set => style.FrameBorderSize = value; }

		public Vector2 ItemSpacing { get => style.ItemSpacing; set => style.ItemSpacing = value; }

		public Vector2 ItemInnerSpacing { get => style.ItemInnerSpacing; set => style.ItemInnerSpacing = value; }

		public Vector2 CellPadding { get => style.CellPadding; set => style.CellPadding = value; }

		public Vector2 TouchExtraPadding { get => style.TouchExtraPadding; set => style.TouchExtraPadding = value; }

		public float IndentSpacing { get => style.IndentSpacing; set => style.IndentSpacing = value; }

		public float ColumnsMinSpacing { get => style.ColumnsMinSpacing; set => style.ColumnsMinSpacing = value; }

		public float ScrollbarSize { get => style.ScrollbarSize; set => style.ScrollbarSize = value; }

		public float ScrollbarRounding { get => style.ScrollbarRounding; set => style.ScrollbarRounding = value; }

		public float GrabMinSize { get => style.GrabMinSize; set => style.GrabMinSize = value; }

		public float GrabRounding { get => style.GrabRounding; set => style.GrabRounding = value; }

		public float LogSliderDeadzone { get => style.LogSliderDeadzone; set => style.LogSliderDeadzone = value; }

		public float TabRounding { get => style.TabRounding; set => style.TabRounding = value; }

		public float TabBorderSize { get => style.TabBorderSize; set => style.TabBorderSize = value; }

		public float TabMinWidthForCloseButton { get => style.TabMinWidthForCloseButton; set => style.TabMinWidthForCloseButton = value; }

		public ImGuiDir ColorButtonPosition { get => (ImGuiDir)style.ColorButtonPosition; set => style.ColorButtonPosition = (ImGuiNET.ImGuiDir)value; }

		public Vector2 ButtonTextAlign { get => style.ButtonTextAlign; set => style.ButtonTextAlign = value; }

		public Vector2 SelectableTextAlign { get => style.SelectableTextAlign; set => style.SelectableTextAlign = value; }

		public Vector2 DisplayWindowPadding { get => style.DisplayWindowPadding; set => style.DisplayWindowPadding = value; }

		public Vector2 DisplaySafeAreaPadding { get => style.DisplaySafeAreaPadding; set => style.DisplaySafeAreaPadding = value; }

		public float MouseCursorScale { get => style.MouseCursorScale; set => style.MouseCursorScale = value; }

		public bool AntiAliasedLines { get => style.AntiAliasedLines; set => style.AntiAliasedLines = value; }

		public bool AntiAliasedLinesUseTex { get => style.AntiAliasedLinesUseTex; set => style.AntiAliasedLinesUseTex = value; }

		public bool AntiAliasedFill { get => style.AntiAliasedFill; set => style.AntiAliasedFill = value; }

		public float CurveTessellationTol { get => style.CurveTessellationTol; set => style.CurveTessellationTol = value; }

		public float CircleTessellationMaxError { get => style.CircleTessellationMaxError; set => style.CircleTessellationMaxError = value; }

		public Span<Vector4> Colors {
			get {
				unsafe {
					return new Span<Vector4>(style.Colors.Data, style.Colors.Count);
				}
			}
		}

		public void ScaleAllSizes(float scaleFactor) {
			style.ScaleAllSizes(scaleFactor);
		}

	}

}
