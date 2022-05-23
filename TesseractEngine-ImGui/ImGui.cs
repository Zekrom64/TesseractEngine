using System.Numerics;
using System.Text;
using Tesseract.Core.Numerics;

namespace Tesseract.ImGui {

	public class ImGuiInputTextCallbackData {

		public ImGuiInputTextFlags EventFlags;
		public ImGuiInputTextFlags Flags;

		public char EventChar;
		public ImGuiKey EventKey;
		public readonly StringBuilder Buf = new();
		public bool BufDirty;
		public int CursorPos;
		public int SelectionStart;
		public int SelectionEnd;

		public ImGuiInputTextCallbackData() {

		}

		public void DeleteChars(int pos, int count) => Buf.Remove(pos, count);
		public void InsertChars(int pos, string text, int end = -1) {
			if (end < 0) end = text.Length;
			Buf.Append(text, 0, end);
		}

		public void SelectAll() {
			SelectionStart = 0;
			SelectionEnd = Buf.Length;
		}

		public void ClearSelection() {
			SelectionStart = SelectionEnd = Buf.Length;
		}

		public bool HasSelection => SelectionStart != SelectionEnd;

	}

	public delegate int ImGuiInputTextCallback(ImGuiInputTextCallbackData data);

	public class ImGuiSizeCallbackData {

		public Vector2 Pos;
		public Vector2 CurrentSize;
		public Vector2 DesiredSize;

	}

	public delegate void ImGuiSizeCallback(ImGuiSizeCallbackData data);

	public static partial class ImGui {

		public const string Version = "1.88 WIP";
		public const int VersionNum = 18712;

		// Context creation and access

		private static ImGuiContext? currentContext = null;
		public static ImGuiContext CurrentContext {
			get {
				if (currentContext == null) throw new InvalidOperationException("Attempted to perform ImGui operation without context set");
				return currentContext;
			}
			set => currentContext = value;
		}

		public static ImGuiContext CreateContext(ImFontAtlas? sharedAtlas = null) {

		}

		public static void DestroyContext(ImGuiContext context) {

		}

		// Main

		public static ImGuiIO IO;

		public static ImGuiStyle Style;

		public static void NewFrame() {

		}

		public static void EndFrame() {

		}

		public static void Render() {

		}

		public static ImDrawData GetDrawData() {

		}

		// Demo, Debug, Information

		public static void ShowDemoWindow(ref bool open) {

		}

		public static void ShowDemoWindow(bool? open = null) { bool p_open = open ?? true; ShowDemoWindow(ref p_open); }

		public static void ShowMetricsWindow(ref bool open) {

		}

		public static void ShowMetricsWindow(bool? open = null) { bool p_open = open ?? true; ShowMetricsWindow(ref p_open); }

		public static void ShowStackToolWindow(ref bool open) {

		}

		public static void ShowStackToolWindow(bool? open = null) { bool p_open = open ?? true; ShowStackToolWindow(ref p_open); }

		public static void ShowAboutWindow(ref bool open) {

		}

		public static void ShowAboutWindow(bool? open = null) { bool p_open = open ?? true; ShowAboutWindow(ref p_open); }

		public static void ShowStyleEditor(ImGuiStyle? style = null) {

		}

		public static bool ShowStyleSelector(string label) {

		}

		public static void ShowFontSelector(string label) {

		}

		public static void ShowUserGuide() {

		}

		// Styles

		public static void StyleColorsDark(ImGuiStyle? style = null) {

		}

		public static void StyleColorsLight(ImGuiStyle? style = null) {

		}

		public static void StyleColorsClassic(ImGuiStyle? style = null) {

		}

		// Windows

		public static bool Begin(string name, ref bool open, ImGuiWindowFlags flags = 0) {

		}

		public static bool Begin(string name, bool? open = null, ImGuiWindowFlags flags = 0) {
			bool p_open = open ?? true;
			return Begin(name, ref p_open, flags);
		}

		public static void End() {

		}

		// Child Windows

		public static bool BeginChild(string name, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = 0) {

		}

		public static bool BeginChild(uint id, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = 0) {

		}

		public static void EndChild() {

		}

		// Windows Utilities

		public static bool IsWindowAppearing;

		public static bool IsWindowCollapsed;

		public static bool IsWindowFocused(ImGuiFocusedFlags flags = 0) {

		}

		public static bool IsWindowHovered(ImGuiHoveredFlags flags = 0) {

		}

		public static ImDrawList WindowDrawList;

		public static Vector2 WindowPos;

		public static Vector2 WindowSize;

		public static float WindowWidth;

		public static float WindowHeight;

		// Window manipulation

		public static void SetNextWindowPos(Vector2 pos, ImGuiCond cond = 0, Vector2 pivot = default) {

		}

		public static void SetNextWindowSize(Vector2 size, ImGuiCond cond = 0) {

		}

		public static void SetNextWindowSizeConstraints(Vector2 sizeMin, Vector2 sizeMax, ImGuiSizeCallback? customCallback = null) {

		}

		public static void SetNextWindowContentSize(Vector2 size) {

		}

		public static void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = 0) {

		}

		public static void SetNextWindowFocus() {

		}

		public static void SetNextWindowBGAlpha(float alpha) {

		}

		public static void SetWindowPos(Vector2 pos, ImGuiCond cond = 0) {

		}

		public static void SetWindowSize(Vector2 size, ImGuiCond cond = 0) {

		}

		public static void SetWindowCollapsed(bool collapsed, ImGuiCond cond = 0) {

		}

		public static void SetWindowFocus() {

		}

		public static void SetWindowFontScale(float scale) {

		}

		public static void SetWindowPos(string name, Vector2 pos, ImGuiCond cond = 0) {

		}

		public static void SetWindowSize(string name, Vector2 pos, ImGuiCond cond = 0) {

		}

		public static void SetWindowCollapsed(string name, bool collapsed, ImGuiCond cond = 0) {

		}

		public static void SetWindowFocus(string name) {

		}

		// Content region

		public static Vector2 ContentRegionAvail;

		public static Vector2 ContentRegionMax;

		public static Vector2 WindowContentRegionMin;

		public static Vector2 WindowContentRegionMax;

		// Window scrolling

		public static float ScrollX;

		public static float ScrollY;

		public static float ScrollMaxX;

		public static float ScrollMaxY;

		public static void SetScrollHereX(float centerXRatio = 0.5f) {

		}

		public static void SetScrollHereY(float centerYRatio = 0.5f) {

		}

		public static void SetScrollFromPosX(float localX, float centerXRatio = 0.5f) {

		}

		public static void SetScrollFromPosY(float localY, float centerYRatio = 0.5f) {

		}

		// Parameters stack (shared)

		public static void PushFont(ImFont font) {

		}

		public static void PopFont() {

		}

		public static void PushStyleColor(ImGuiCol idx, uint color) {

		}

		public static void PushStyleColor(ImGuiCol idx, Vector4 col) {

		}

		public static void PopStyleColor(int count = 1) {

		}

		public static void PushStyleVar(ImGuiStyleVar idx, float val) {

		}

		public static void PushStyleVar(ImGuiStyleVar idx, Vector2 val) {

		}

		public static void PopStyleVar(int count = 1) {

		}

		public static void PushAllowKeyboardFocus(bool allowKeyboardFocus) {

		}

		public static void PopAllowKeyboardFocus() {

		}

		public static void PushButtonRepeat(bool repeat) {

		}

		public static void PopButtonRepeat() {

		}

		// Parameters stack  (current window)

		public static void PushItemWidth(float width) {

		}

		public static void PopItemWidth() {

		}

		public static void SetNextItemWidth(float width) {

		}

		public static float CalcItemWidth() {

		}

		public static void PushTextWrapPos(float wrapLocalPosX = 0.5f) {

		}

		public static void PopTextWrapPos() {

		}

		// Style read access

		public static ImFont Font;

		public static float FontSize;

		public static Vector2 FontTexUvWhitePixel;

		public static uint GetColorU32(ImGuiCol idx, float alphaMul = 1.0f) {

		}

		public static uint GetColorU32(Vector4 col) {

		}

		public static uint GetColorU32(uint col) => col;

		public static Vector4 GetStyleColorVec4(ImGuiCol idx) {

		}

		// Cursor / Layout

		public static void Separator() {

		}

		public static void SameLine(float offsetFromStartX = 0.0f, float spacing = 1.0f) {

		}

		public static void NewLine() {

		}

		public static void Spacing() {

		}

		public static void Dummy(Vector2 size) {

		}

		public static void Indent(float indentW = 0.0f) {

		}

		public static void Unindent(float indentW = 0.0f) {

		}

		public static void BeginGroup() {

		}

		public static void EndGroup() {

		}

		public static Vector2 CursorPos;

		public static Vector2 CursorStartPos;

		public static Vector2 CursorScreenPos;

		public static void AlignTextToFramePadding() {

		}

		public static float TextLineHeight;

		public static float TextLineHeightWithSpacing;

		public static float FrameHeight;

		public static float FrameHeightWithSpacing;

		// ID stack/scopes

		public static void PushID(string id) {

		}

		public static void PushID(nuint id) {

		}

		public static void PushID(int id) {

		}

		public static void PopID() {

		}

		public static int GetID(string strID) {

		}

		public static int GetID(nuint id) {

		}

		// Widgets: Text

		public static partial void TextUnformatted(string text);
		public static partial void Text(string fmt, params object[] args);
		public static partial void TextColored(Vector4 col, string fmt, params object[] args);
		public static partial void TextDisabled(string fmt, params object[] args);
		public static partial void TextWrapped(string fmt, params object[] args);
		public static partial void LabelText(string label, string fmt, params object[] args);
		public static partial void BulletText(string fmt, params object[] args);

		// Widgets: Main

		public static partial bool Button(string label, Vector2 size = default);
		public static partial bool SmallButton(string label);
		public static partial bool InvisibleButton(string id, Vector2 size, ImGuiButtonFlags flags = 0);
		public static partial bool ArrowButton(string id, ImGuiDir dir);

		public static partial void Image(nint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tintCol, Vector4 borderCol = default);
		public static void Image(nint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1) =>
			Image(userTextureID, size, uv0, uv1, Vector4.One);
		public static void Image(nint userTextureID, Vector2 size, Vector2 uv0 = default) =>
			Image(userTextureID, size, uv0, Vector2.One);

		public static partial bool ImageButton(nint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding, Vector4 bgCol, Vector4 tintCol);
		public static bool ImageButton(nint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding = -1, Vector4 bgCol = default) =>
			ImageButton(userTextureID, size, uv0, uv1, framePadding, bgCol, Vector4.One);
		public static bool ImageButton(nint userTextureID, Vector2 size, Vector2 uv0 = default) =>
			ImageButton(userTextureID, size, uv0, Vector2.One);

		public static partial bool Checkbox(string label, ref bool v);
		public static partial bool CheckboxFlags(string label, ref int flags, int flagsValue);
		public static partial bool RadioButton(string label, bool active);
		public static partial bool RadioButton(string label, ref int v, int vButton);

		public static partial void ProgressBar(float fraction, Vector2 sizeArg, string? overlay = null);
		public static void ProgressPar(float fraction) =>
			ProgressBar(fraction, new Vector2(-float.MinValue, 0));

		public static void Bullet();

		// Widgets: Combo Box

		public static partial bool BeginCombo(string label, string previewValue, ImGuiComboFlags flags = 0);
		public static partial void EndCombo();
		public static partial bool Combo<T>(string label, ref int currentItem, IReadOnlyCollection<T> items, Func<T,string>? getter = null, int popupMaxHeightInItems = -1);

		// Widgets: Drag Sliders

		public static partial bool DragFloat(string label, ref float v, float vSpeed = 1.0f, float vMin = 0.0f, float vMax = 0.0f, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool DragFloat2(string label, ref Vector2 v, float vSpeed = 1.0f, float vMin = 0.0f, float vMax = 0.0f, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool DragFloat3(string label, ref Vector3 v, float vSpeed = 1.0f, float vMin = 0.0f, float vMax = 0.0f, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool DragFloat4(string label, ref Vector4 v, float vSpeed = 1.0f, float vMin = 0.0f, float vMax = 0.0f, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool DragFloatRange2(string label, ref float vCurrentMin, ref float vCurrentMax, float vSpeed = 1.0f, float vMin = 0.0f, float vMax = 0.0f, string format = "0.000", string? formatMax = null, ImGuiSliderFlags flags = 0);
		public static partial bool DragInt(string label, ref int v, float vSpeed = 1.0f, int vMin = 0, int vMax = 0, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool DragInt2(string label, ref Vector2i v, float vSpeed = 1.0f, int vMin = 0, int vMax = 0, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool DragInt3(string label, ref Vector3i v, float vSpeed = 1.0f, int vMin = 0, int vMax = 0, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool DragInt4(string label, ref Vector4i v, float vSpeed = 1.0f, int vMin = 0, int vMax = 0, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool DragIntRange2(string label, ref int vCurrentMin, ref int vCurrentMax, float vSpeed = 1.0f, int vMin = 0, int vMax = 0, string format = "0", string? formatMax = null, ImGuiSliderFlags flags = 0);
		public static partial bool DragScalar<T>(string label, ref T data, float vSpeed = 1.0f, T? min = null, T? max = null, string? format = null, ImGuiSliderFlags flags = 0) where T : unmanaged ;
		public static partial bool DragScalarN<T>(string label, Span<T> data, float vSpeed = 1.0f, T? min = null, T? max = null, string? format = null, ImGuiSliderFlags flags = 0) where T : unmanaged;

		// Widgets: Regular Sliders

		public static partial bool SliderFloat(string label, ref float v, float vMin, float vMax, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool SliderFloat2(string label, ref Vector2 v, float vMin, float vMax, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool SliderFloat3(string label, ref Vector3 v, float vMin, float vMax, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool SliderFloat4(string label, ref Vector4 v, float vMin, float vMax, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool SliderAngle(string label, ref float vRad, float vDegreesMin = -360.0f, float vDegreesMax = 360.0f, string format = "0.000 deg", ImGuiSliderFlags flags = 0);
		public static partial bool SliderInt(string label, ref int v, float vMin, float vMax, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool SliderInt2(string label, ref Vector2i v, float vMin, float vMax, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool SliderInt3(string label, ref Vector3i v, float vMin, float vMax, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool SliderInt4(string label, ref Vector4i v, float vMin, float vMax, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool SliderScalar<T>(string label, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0);
		public static partial bool SliderScalarN<T>(string label, Span<T> data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0);
		public static partial bool VSliderFloat(string label, ref float v, float vMin, float vMax, string format = "0.000", ImGuiSliderFlags flags = 0);
		public static partial bool VSliderInt(string label, ref int v, float vMin, float vMax, string format = "0", ImGuiSliderFlags flags = 0);
		public static partial bool VSliderScalar<T>(string label, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0);

		// Widgets: Input with Keyboard

		public static partial bool InputText(string label, StringBuilder buf, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback? callback = null);
		public static partial bool InputTextMultiline(string label, StringBuilder buf, Vector2 size = default, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback? callback = null);
		public static partial bool InputTextWithHint(string label, string hint, StringBuilder buf, ImGuiInputTextFlags flags = 0, ImGuiInputTextCallback? callback = null);
		public static partial bool InputFloat(string label, ref float v, float step = 0.0f, float stepFast = 0.0f, string format = "0.000", ImGuiInputTextFlags flags = 0);
		public static partial bool InputFloat2(string label, ref Vector2 v, string format = "0.000", ImGuiInputTextFlags flags = 0);
		public static partial bool InputFloat3(string label, ref Vector3 v, string format = "0.000", ImGuiInputTextFlags flags = 0);
		public static partial bool InputFloat4(string label, ref Vector4 v, string format = "0.000", ImGuiInputTextFlags flags = 0);
		public static partial bool InputInt(string label, ref int v, int step = 1, int stepFast = 100, ImGuiInputTextFlags flags = 0);
		public static partial bool InputInt2(string label, ref Vector2i v, ImGuiInputTextFlags flags = 0);
		public static partial bool InputInt3(string label, ref Vector3i v, ImGuiInputTextFlags flags = 0);
		public static partial bool InputInt4(string label, ref Vector4i v, ImGuiInputTextFlags flags = 0);
		public static partial bool InputDouble(string label, ref double v, double step = 0.0, double stepFast = 0.0, string format = "0.000000", ImGuiInputTextFlags flags = 0);
		public static partial bool InputScalar<T>(string label, ref T data, T? step = null, T? stepFast = null, string? format = null, ImGuiInputTextFlags flags = 0) where T : unmanaged;
		public static partial bool InputScalarN<T>(string label, Span<T> data, T? step = null, T? stepFast = null, string? format = null, ImGuiInputTextFlags flags = 0) where T : unmanaged;

		// Widgets: Color Editor/Picker

		public static partial bool ColorEdit3(string label, ref Vector3 col, ImGuiColorEditFlags flags = 0);
		public static partial bool ColorEdit4(string label, ref Vector4 col, ImGuiColorEditFlags flags = 0);
		public static partial bool ColorPicker3(string label, ref Vector3 col, ImGuiColorEditFlags flags = 0);
		public static partial bool ColorPicker4(string label, ref Vector4 col, ImGuiColorEditFlags flags = 0);
		public static partial bool ColorButton(string descID, Vector4 col, ImGuiColorEditFlags flags = 0, Vector2 size = default);

		public static void SetColorEditOptions(ImGuiColorEditFlags flags = 0) {

		}

		// Widgets: Trees

		public static partial bool TreeNode(string label);
		public static partial bool TreeNode(string strID, string fmt, params object[] args);
		public static partial bool TreeNode(nint id, string fmt, params object[] args);
		public static partial bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags = 0);
		public static partial bool TreeNodeEx(string strID, ImGuiTreeNodeFlags flags, string fmt, params object[] args);
		public static partial bool TreeNodeEx(nint id, ImGuiTreeNodeFlags flags, string fmt, params object[] args);
		public static partial bool TreePush(string id);
		public static partial bool TreePush(nint id = 0);
		public static partial void TreePop();

		public static float TreeNodeToLabelSpacing;

		public static partial bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags = 0);
		public static partial bool CollapsingHeader(string label, ref bool visible, ImGuiTreeNodeFlags flags = 0);

		public static void SetNextItemOpen(bool isOpen, ImGuiCond cond = 0) {

		}

		// Widgets: Selectable

		public static partial bool Selectable(string label, bool selected = false, ImGuiSelectableFlags flags = 0, Vector2 size = default);
		public static partial bool Selectable(string label, ref bool selected, ImGuiSelectableFlags flags = 0, Vector2 size = default);

		// Widgets: List Boxes

		public static partial bool BeginListBox(string label, Vector2 size = default);
		public static partial void EndListBox();
		public static partial bool ListBox<T>(string label, ref int currentItem, IReadOnlyCollection<T> items, Func<T,string>? getter = null, int heightInItems = -1);

		// Widgets: Data Plotting

		public static partial void PlotLines(string label, in ReadOnlySpan<float> values, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);
		public static partial void PlotLines(string label, Func<int, float> getter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);
		public static partial void PlotHistogram(string label, in ReadOnlySpan<float> values, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);
		public static partial void PlotHistogram(string label, Func<int, float> getter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);

		// Widgets: Values

		public static partial void Value(string prefix, bool b);
		public static partial void Value(string prefix, int v);
		public static partial void Value(string prefix, uint v);
		public static partial void Value(string prefix, float v, string? floatFormat = null);

		// Widgets: Menus

		public static partial bool BeginMenuBar();
		public static partial void EndMenuBar();
		public static partial bool BeginMainMenuBar();
		public static partial void EndMainMenuBar();
		public static partial bool BeginMenu(string label, bool enabled);
		public static partial boid EndMenu();
		public static partial bool MenuItem(string label, string? shortcut = null, bool selected = false, bool enabled = true);
		public static partial bool MenuItem(string label, string? shortcut, ref bool selected, bool enabled = true);

		// Tooltips

		public static partial void BeginTooltip();
		public static partial void EndTooltip();
		public static partial void SetTooltip(string fmt, params object[] args);

		// Popups, Modals

		public static partial bool BeginPopup(string strID, ImGuiWindowFlags flags = 0);
		public static partial bool BeginPopupModal(string name, ref bool open, ImGuiWindowFlags flags = 0);
		public static bool BeginPopupModal(string name, bool? open = null, ImGuiWindowFlags flags = 0) {
			bool popen = open ?? true;
			return BeginPopupModal(name, ref popen, flags);
		}

		// Popups: open/close functions

		public static void OpenPopup(string strID, ImGuiPopupFlags flags = 0) {

		}

		public static void OpenPopup(int id, ImGuiPopupFlags flags = 0) {

		}

		public static void OpenPopupOnItemClick(string? strID = null, ImGuiPopupFlags flags = 0) {

		}

		public static void CloseCurrentPopup() {

		}

		// Popups: open+begin combined function helpers

		public static bool BeginPopupContextItem(string? strID = null, ImGuiPopupFlags flags = 0) {

		}

		public static bool BeginPopupContextWindow(string? strID = null, ImGuiPopupFlags flags = 0) {

		}

		public static bool BeginPopupContextVoid(string? strID = null, ImGuiPopupFlags flags = 0) {

		}

		// Popups: Query functions

		public static bool IsPopupOpen(string id, ImGuiPopupFlags flags = 0) {

		}

		// Tables

		public static partial bool BeginTable(string id, int column, ImGuiTableFlags flags = 0, Vector2 outerSize = default, float innerWidth = 0.0f);
		public static partial void EndTable();
		public static partial void TableNextRow(ImGuiTableRowFlags flags = 0, float minRowHeight = 0.0f);
		public static partial bool TableNextColumn();
		public static partial bool TableSetColumnIndex(int column);

		// Tables: Headers & Columns declaration

		public static partial void TableSetupColumn(string label, ImGuiTableColumnFlags flags = 0, float initWidthOrHeight = 0.0f, int userID = 0);
		public static partial void TableSetupScrollFreeze(int cols, int rows);
		public static partial void TableHeadersRow();
		public static partial void TableHeader(string label);

		// Tables: Sorting

		public static ImGuiTableSortSpecs TableSortSpects;

		// Tables: Miscellaneous Functions

		public static int TableColumnCount;
		public static int TableColumnIndex;
		public static int TableRowIndex;
		public static partial string TableGetColumnName(int column = -1);
		public static partial ImGuiTableColumnFlags TableGetColumnFlags(int column = -1);
		public static partial void TableSetColumnEnabled(int column, bool v);
		public static partial void TableSetBgColor(ImGuiTableBgTarget target, uint color, int column = -1);

		// Tab Bars, Tabs

		public static partial bool BeginTabBar(string strID, ImgGuiTabBarFlags flags = 0);
		public static partial void EndTabBar();
		public static partial bool BeginTabItem(string label, ref bool open, ImGuiTabItemFlags flags = 0);
		public static bool BeginTabItem(string label, bool? open = null, ImGuiTabItemFlags flags = 0) {
			bool popen = open ?? true;
			return BeginTabItem(label, ref popen, flags);
		}
		public static partial bool TabItemButton(string label, ImGuiTabItemFlags flags = 0);

		public static void SetTabItemClosed(string tabOrDockedWindowLabel) {

		}

		// Logging/Capture

		public static void LogToTTY(int autoOpenDepth = -1) {

		}

		public static void LogToFile(int autoOpenDepth = -1, string? filename = null) {

		}

		public static void LogToClipboard(int autoOpenDepth = -1) {

		}

		public static void LogFinish() {

		}

		public static void LogButtons() {

		}

		public static void LogText(string fmt, params string[] args) {

		}

		// Drag and Drop

		public static bool BeginDragDropSource(ImGuiDragDropFlags flags = 0) {

		}

		public static bool SetDragDropPayload(string type, in ReadOnlySpan<byte> data, ImGuiCond cond = 0) {

		}

		public static void EndDragDropSource() {

		}

		public static void BeginDragDropTarget() {

		}

		public static ImGuiPayload AcceptDragDropPayload(string type, ImGuiDragDropFlags flags = 0) {

		}

		public static void EndDragDropTarget() {

		}

		public static ImGuiPayload GetDragDropPayload() {

		}

		// Disabling

		public static void BeginDisabled(bool disabled = true) {

		}

		public static void EndDisabled() {

		}

		// Clipping

		public static void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWothCurrentClipRect) {

		}

		public static void PopClipRect() {

		}

		// Focus, Activation

		public static void SetItemDefaultFocus() {

		}

		public static void SetKeyboardFocusHere(int offset = 0) {

		}

		// Item/Widgets Utilities and Query Functions

		public static bool IsItemHovered(ImGuiHoveredFlags flags = 0) {

		}

		public static bool IsItemActive;
		public static bool IsItemFocused;

		public static bool IsItemClicked(ImGuiMouseButton button = 0) {

		}

		public static bool IsItemVisible;
		public static bool IsItemEdited;
		public static bool IsItemActivated;
		public static bool IsItemDeactivated;
		public static bool IsItemDeactivatedAfterEdit;
		public static bool IsItemToggledOpen;
		public static bool IsAnyItemHovered;
		public static bool IsAnyItemActive;
		public static bool IsAnyItemFocused;
		public static Vector2 ItemRectMin;
		public static Vector2 ItemRectMax;
		public static Vector2 ItemRectSize;

		public static void SetItemAllowOverlap() {

		}

		// Viewports

		public static ImGuiViewport MainViewport;

		// Miscellaneous Utilities

		public static bool IsRectVisible(Vector2 size) {

		}

		public static bool IsRectVisible(Vector2 rectMin, Vector2 rectMax) {

		}

		public static double Time;

		public static int FrameCount;

		public static ImDrawList BackgroundDrawList;

		public static ImDrawList ForegroundDrawList;

		public static ImDrawListSharedData DrawListSharedData;

		public static string GetStyleColorName(ImGuiCol idx) {

		}

		public static ImGuiStorage StateStorage;

		public static bool BeginChildFrame(int id, Vector2 size, ImGuiWindowFlags flags = 0) {

		}

		public static void EndChildFrame() {

		}

		// Text Utilities

		public static Vector2 CalcTextSize(string text, bool hideTextAfterDoubleHash = false, float wrapWidth = -1.0f) {

		}

		// Color Utilities

		public static Vector4 ColorConvertU32ToFloat4(uint _in) {

		}

		public static uint ColorConvertFloat4ToU32(Vector4 _in) {

		}

		public static Vector3 ColorConvertRGBToHSV(Vector3 rgb) {

		}

		public static Vector3 ColorConvertHSVToRGB(Vector3 hsv) {

		}

		// Inputs Utilities: Keyboard

		public static bool IsKeyDown(ImGuiKey key) {

		}

		public static bool IsKeyPressed(ImGuiKey key, bool repeat = true) {

		}

		public static bool IsKeyReleased(ImGuiKey key) {

		}

		public static int GetKeyPressedAmount(ImGuiKey key, float repeatDelay, float repeatRate) {

		}

		public static string GetKeyName(ImGuiKey key) {

		}

		public static void CaptureKeyboardFromApp(bool wantCaptureKeyboardValue = true) {

		}

		// Input Utilities: Mouse

		public static bool IsMouseDown(ImGuiMouseButton button) {

		}

		public static bool IsMouseClicked(ImGuiMouseButton button, bool repeat = false) {

		}

		public static bool IsMouseReleased(ImGuiMouseButton button) { }

		public static bool IsMouseDoubleClicked(ImGuiMouseButton button) {

		}

		public static int GetMouseClickedCount(ImGuiMouseButton button) {

		}

		public static bool IsMouseHoveringRect(Vector2 rmin, Vector2 rmax, bool clip = true) {

		}

		public static bool IsMousePosValid(Vector2 pos) {

		}

		public static bool IsAnyMouseDown;
		public static Vector2 MousePos;
		public static Vector2 MousePosOnOpeningCurrentPopup;

		public static bool IsMouseDragging(ImGuiMouseButton button, float lockThreshold = -1.0f) {

		} 

		public static Vector2 GetMouseDragDelta(ImGuiMouseButton button, float lockThreshold = -1.0f) {

		}

		public static void ResetMouseDragDelta() {

		}

		public static ImGuiMouseCursor MouseCursor;

		public static void CaptureMouseFromApp(bool wantCaptureMouseValue = true) {

		}

		// Clipboard Utilities

		public static string ClipboardText;

		// Settings/.Ini Utilities

		public static void LoadIniSettingsFromDisk(string iniFilename) {

		}

		public static void LoadIniSettingsFromMemory(string iniData) {

		}

		public static void SaveIniSettingsToDisk(string iniFilename) {

		}

		public static string SaveIniSettingsToMemory() {

		}

		// Debug Utilities

		// Memory Allocators

	}

}