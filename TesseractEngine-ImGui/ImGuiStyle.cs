using System.Numerics;
using Tesseract.Core.Numerics;

namespace Tesseract.ImGui {

	/// <summary>
	/// You may modify the <see cref="GImGui.Style"/> main instance during initialization and before <see cref="GImGui.NewFrame"/>.
	/// During the frame, use <see cref="GImGui.PushStyleVar(ImGuiStyleVar, float)"/>/<see cref="GImGui.PopStyleVar(int)"/> to alter the main style values,
	/// and <see cref="GImGui.PushStyleColor(ImGuiCol, uint)"/>/<see cref="GImGui.PopStyleColor(int)"/> for colors.
	/// </summary>
	public interface IImGuiStyle : IDisposable {

		/// <summary>
		/// Global alpha applies to everything in Dear ImGui.
		/// </summary>
		public float Alpha { get; set; }
		/// <summary>
		/// Additional alpha multiplier applied by <see cref="GImGui.BeginDisabled(bool)"/>. Multiply over current value of Alpha.
		/// </summary>
		public float DisabledAlpha { get; set; }
		/// <summary>
		/// Padding within a window.
		/// </summary>
		public Vector2 WindowPadding { get; set; }
		/// <summary>
		/// Radius of window corners rounding. Set to 0.0f to have rectangular windows. Large values tend to lead to variety of artifacts and are not recommended.
		/// </summary>
		public float WindowRounding { get; set; }
		/// <summary>
		/// Thickness of border around windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
		/// </summary>
		public float WindowBorderSize { get; set; }
		/// <summary>
		/// Minimum window size. This is a global setting. If you want to constraint individual windows, use <see cref="GImGui.SetNextWindowSizeConstraints(Vector2, Vector2, ImGuiSizeCallback?)"/>.
		/// </summary>
		public Vector2 WindowMinSize { get; set; }
		/// <summary>
		/// Alignment for title bar text. Defaults to (0.0f,0.5f) for left-aligned,vertically centered.
		/// </summary>
		public Vector2 WindowTitleAlign { get; set; }
		/// <summary>
		/// Side of the collapsing/docking button in the title bar (None/Left/Right). Defaults to <see cref="ImGuiDir.Left"/>.
		/// </summary>
		public ImGuiDir WindowMenuButtonPosition { get; set; }
		/// <summary>
		/// Radius of child window corners rounding. Set to 0.0f to have rectangular windows.
		/// </summary>
		public float ChildRounding { get; set; }
		/// <summary>
		/// Thickness of border around child windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
		/// </summary>
		public float ChildBorderSize { get; set; }
		/// <summary>
		/// Radius of popup window corners rounding. (Note that tooltip windows use <see cref="WindowRounding"/>)
		/// </summary>
		public float PopupRounding { get; set; }
		/// <summary>
		/// Thickness of border around popup/tooltip windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
		/// </summary>
		public float PopupBorderSize { get; set; }
		/// <summary>
		/// Padding within a framed rectangle (used by most widgets).
		/// </summary>
		public Vector2 FramePadding { get; set; }
		/// <summary>
		/// Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets).
		/// </summary>
		public float FrameRounding { get; set; }
		/// <summary>
		/// Thickness of border around frames. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
		/// </summary>
		public float FrameBorderSize { get; set; }
		/// <summary>
		/// Horizontal and vertical spacing between widgets/lines.
		/// </summary>
		public Vector2 ItemSpacing { get; set; }
		/// <summary>
		/// Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).
		/// </summary>
		public Vector2 ItemInnerSpacing { get; set; }
		/// <summary>
		/// Padding within a table cell.
		/// </summary>
		public Vector2 CellPadding { get; set; }
		/// <summary>
		/// Expand reactive bounding box for touch-based system where touch position is not accurate enough. Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. So don't grow this too much!
		/// </summary>
		public Vector2 TouchExtraPadding { get; set; }
		/// <summary>
		/// Horizontal indentation when e.g. entering a tree node. Generally == (<see cref="FontSize"/> + <see cref="FramePadding"/>.X*2).
		/// </summary>
		public float IndentSpacing { get; set; }
		/// <summary>
		/// Minimum horizontal spacing between two columns. Preferably > (<see cref="FramePadding"/>.X + 1).
		/// </summary>
		public float ColumnsMinSpacing { get; set; }
		/// <summary>
		/// Width of the vertical scrollbar, Height of the horizontal scrollbar.
		/// </summary>
		public float ScrollbarSize { get; set; }
		/// <summary>
		/// Radius of grab corners for scrollbar.
		/// </summary>
		public float ScrollbarRounding { get; set; }
		/// <summary>
		/// Minimum width/height of a grab box for slider/scrollbar.
		/// </summary>
		public float GrabMinSize { get; set; }
		/// <summary>
		/// Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.
		/// </summary>
		public float GrabRounding { get; set; }
		/// <summary>
		/// The size in pixels of the dead-zone around zero on logarithmic sliders that cross zero.
		/// </summary>
		public float LogSliderDeadzone { get; set; }
		/// <summary>
		/// Radius of upper corners of a tab. Set to 0.0f to have rectangular tabs.
		/// </summary>
		public float TabRounding { get; set; }
		/// <summary>
		/// Thickness of border around tabs.
		/// </summary>
		public float TabBorderSize { get; set; }
		/// <summary>
		/// Minimum width for close button to appears on an unselected tab when hovered. Set to 0.0f to always show when hovering, set to FLT_MAX to never show close button unless selected.
		/// </summary>
		public float TabMinWidthForCloseButton { get; set; }
		/// <summary>
		/// Side of the color button in the ColorEdit4 widget (left/right). Defaults to <see cref="ImGuiDir.Right"/>.
		/// </summary>
		public ImGuiDir ColorButtonPosition { get; set; }
		/// <summary>
		/// Alignment of button text when button is larger than text. Defaults to (0.5f, 0.5f) (centered).
		/// </summary>
		public Vector2 ButtonTextAlign { get; set; }
		/// <summary>
		/// Alignment of selectable text. Defaults to (0.0f, 0.0f) (top-left aligned). It's generally important to keep this left-aligned if you want to lay multiple items on a same line.
		/// </summary>
		public Vector2 SelectableTextAlign { get; set; }
		/// <summary>
		/// Window position are clamped to be visible within the display area or monitors by at least this amount. Only applies to regular windows.
		/// </summary>
		public Vector2 DisplayWindowPadding { get; set; }
		/// <summary>
		/// If you cannot see the edges of your screen (e.g. on a TV) increase the safe area padding. Apply to popups/tooltips as well regular windows. NB: Prefer configuring your TV sets correctly!
		/// </summary>
		public Vector2 DisplaySafeAreaPadding { get; set; }
		/// <summary>
		/// Scale software rendered mouse cursor (when io.MouseDrawCursor is enabled). May be removed later.
		/// </summary>
		public float MouseCursorScale { get; set; }
		/// <summary>
		/// Enable anti-aliased lines/borders. Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to <see cref="ImDrawList"/>).
		/// </summary>
		public bool AntiAliasedLines { get; set; }
		/// <summary>
		/// Enable anti-aliased lines/borders using textures where possible. Require backend to render with bilinear filtering. Latched at the beginning of the frame (copied to <see cref="ImDrawList"/>).
		/// </summary>
		public bool AntiAliasedLinesUseTex { get; set; }
		/// <summary>
		/// Enable anti-aliased edges around filled shapes (rounded rectangles, circles, etc.). Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to <see cref="ImDrawList"/>).
		/// </summary>
		public bool AntiAliasedFill { get; set; }
		/// <summary>
		/// Tessellation tolerance when using PathBezierCurveTo() without a specific number of segments. Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality.
		/// </summary>
		public float CurveTessellationTol { get; set; }
		/// <summary>
		/// Maximum error (in pixels) allowed when using <see cref="ImDrawList.AddCircle(Vector2, float, uint, int, float)"/>/<see cref="ImDrawList.AddCircleFilled(Vector2, float, uint, int)"/> or drawing rounded corner rectangles with no explicit segment count specified. Decrease for higher quality but more geometry.
		/// </summary>
		public float CircleTessellationMaxError { get; set; }

		public Span<Vector4> Colors { get; }

		public void ScaleAllSizes(float scaleFactor);

	}

}
