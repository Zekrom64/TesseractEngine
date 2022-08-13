using System.Numerics;
using System.Text;
using Tesseract.Core.Numerics;

namespace Tesseract.ImGui {

	public static class ImGui {

		// Needed because C# doesn't have a standard field for FLT_MIN
		internal const float FltMin = 1.175494351e-38f;

		/// <summary>
		/// The global ImGui implementation, set this before using any functions in this class.
		/// </summary>
		public static IImGui GImGui { get; set; } = default!;


		// Context creation and access
		// - Each context create its own ImFontAtlas by default. You may instance one yourself and pass it to CreateContext() to share a font atlas between contexts.
		// - DLL users: heaps and globals are not shared across DLL boundaries! You will need to call SetCurrentContext() + SetAllocatorFunctions()
		//   for each static/DLL boundary you are calling from. Read "Context and Memory Allocators" section of imgui.cpp for details.

		public static IImGuiContext CreateContext(IImFontAtlas? sharedFontAtlas = null) => GImGui.CreateContext(sharedFontAtlas);

		public static void DestroyContext(IImGuiContext? ctx = null) => GImGui.DestroyContext(ctx);

		public static IImGuiContext CurrentContext { get => GImGui.CurrentContext; set => GImGui.CurrentContext = value; }


		/// <summary>
		/// Access the IO structure (mouse/keyboard/gamepad inputs, time, various configuration options/flags)
		/// </summary>
		public static IImGuiIO IO => GImGui.IO;

		/// <summary>
		/// Access the Style structure (colors, sizes). Always use <see cref="PushStyleColor(ImGuiCol, uint)"/>, <see cref="PushStyleVar(ImGuiStyleVar, float)"/> to modify style mid-frame!
		/// </summary>
		public static IImGuiStyle Style { get => GImGui.Style; set => GImGui.Style = value; }

		/// <summary>
		/// Start a new Dear ImGui frame, you can submit any command from this point until <see cref="Render"/>/<see cref="EndFrame"/>.
		/// </summary>
		public static void NewFrame() => GImGui.NewFrame();

		/// <summary>
		/// Ends the Dear ImGui frame. Automatically called by <see cref="Render"/>. If you don't need to render data (skipping rendering) you may call EndFrame() without Render()... but you'll have wasted CPU already! If you don't need to render, better to not create any windows and not call NewFrame() at all!
		/// </summary>
		public static void EndFrame() => GImGui.EndFrame();

		/// <summary>
		/// Ends the Dear ImGui frame, finalize the draw data. You can then get call <see cref="GetDrawData"/>.
		/// </summary>
		public static void Render() => GImGui.Render();

		/// <summary>
		/// Valid after <see cref="Render"/> and until the next call to <see cref="NewFrame"/>. This is what you have to render.
		/// </summary>
		/// <returns>Draw data for the current frame</returns>
		public static IImDrawData GetDrawData() => GImGui.GetDrawData();


		public static void ShowDemoWindow(ref bool open) => GImGui.ShowDemoWindow(ref open);

		public static void ShowDemoWindow(bool? open = null) => GImGui.ShowDemoWindow(open);

		public static void ShowMetricsWindow(ref bool open) => GImGui.ShowMetricsWindow(ref open);

		public static void ShowMetricsWindow(bool? open = null) => GImGui.ShowMetricsWindow(open);

		public static void ShowStackToolWindow(ref bool open) => GImGui.ShowStackToolWindow(ref open);

		public static void ShowStackToolWindow(bool? open = null) => GImGui.ShowStackToolWindow(open);

		public static void ShowAboutWindow(ref bool open) => GImGui.ShowAboutWindow(ref open);

		public static void ShowAboutWindow(bool? open = null) => GImGui.ShowAboutWindow(open);

		public static void ShowStyleEditor(IImGuiStyle? style = null) => GImGui.ShowStyleEditor(style);

		public static void ShowStyleSelector(string label) => GImGui.ShowStyleSelector(label);

		public static void ShowFontSelector(string label) => GImGui.ShowFontSelector(label);

		public static void ShowUserGuide() => GImGui.ShowUserGuide();

		public static string Version => GImGui.Version;


		public static void StyleColorsDark(IImGuiStyle? dst = null) => GImGui.StyleColorsDark(dst);

		public static void StyleColorsLight(IImGuiStyle? dst = null) => GImGui.StyleColorsLight(dst);

		public static void StyleColorsClassic(IImGuiStyle? dst = null) => GImGui.StyleColorsClassic(dst);


		public static void Begin(string name, ref bool open, ImGuiWindowFlags flags = default) => GImGui.Begin(name, ref open, flags);

		public static void Begin(string name, bool? open = null, ImGuiWindowFlags flags = default) => GImGui.Begin(name, open, flags);

		public static void End() => GImGui.End();


		public static void BeginChild(string strID, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = default) => GImGui.BeginChild(strID, size, border, flags);

		public static void BeginChild(uint id, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = default) => GImGui.BeginChild(id, size, border, flags);

		public static void EndChild() => GImGui.EndChild();


		public static bool IsWindowAppearing => GImGui.IsWindowAppearing;

		public static bool IsWindowCollapsed => GImGui.IsWindowCollapsed;

		public static bool IsWindowFocused(ImGuiFocusedFlags flags = default) => GImGui.IsWindowFocused(flags);

		public static bool IsWindowHovered(ImGuiHoveredFlags flags = default) => GImGui.IsWindowHovered(flags);

		public static IImDrawList GetWindowDrawList() => GImGui.GetWindowDrawList();

		public static Vector2 WindowPos => GImGui.WindowPos;

		public static Vector2 WindowSize => GImGui.WindowSize;

		public static float WindowWidth => GImGui.WindowWidth;

		public static float WindowHeight => GImGui.WindowHeight;


		public static void SetNextWindowPos(Vector2 pos, ImGuiCond cond = default, Vector2 pivot = default) => GImGui.SetNextWindowPos(pos, cond, pivot);

		public static void SetNextWindowSize(Vector2 size, ImGuiCond cond = default) => GImGui.SetNextWindowSize(size, cond);

		public static void SetNextWindowSizeConstraints(Vector2 sizeMin, Vector2 sizeMax, ImGuiSizeCallback? customCallback = null) => GImGui.SetNextWindowSizeConstraints(sizeMin, sizeMax, customCallback);

		public static void SetNextWindowContentSize(Vector2 size) => GImGui.SetNextWindowContentSize(size);

		public static void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = default) => GImGui.SetNextWindowCollapsed(collapsed, cond);

		public static void SetNextWindowFocus() => GImGui.SetNextWindowFocus();

		public static void SetNextWindowBgAlpha(float alpha) => GImGui.SetNextWindowBgAlpha(alpha);

		public static void SetWindowPos(Vector2 pos, ImGuiCond cond = default) => GImGui.SetWindowPos(pos, cond);

		public static void SetWindowSize(Vector2 size, ImGuiCond cond = default) => GImGui.SetWindowSize(size, cond);

		public static void SetWindowCollapsed(bool collapsed, ImGuiCond cond = default) => GImGui.SetWindowCollapsed(collapsed, cond);

		public static void SetWindowFocus() => GImGui.SetWindowFocus();

		public static void SetWindowFontScale(float scale) => GImGui.SetWindowFontScale(scale);

		public static void SetWindowPos(string name, Vector2 pos, ImGuiCond cond = default) => GImGui.SetWindowPos(name, pos, cond);

		public static void SetWindowSize(string name, Vector2 size, ImGuiCond cond = default) => GImGui.SetWindowSize(name, size, cond);

		public static void SetWindowCollapsed(string name, bool collapsed, ImGuiCond cond = default) => GImGui.SetWindowCollapsed(name, collapsed, cond);

		public static void SetWindowFocus(string name) => GImGui.SetWindowFocus(name);


		public static Vector2 ContentRegionAvail => GImGui.ContentRegionAvail;

		public static Vector2 ContentRegionMax => GImGui.ContentRegionMax;

		public static Vector2 WindowContentRegionMax => GImGui.WindowContentRegionMax;

		public static Vector2 WindowContentRegionMin => GImGui.WindowContentRegionMin;


		public static float ScrollX => GImGui.ScrollX;

		public static float ScrollY => GImGui.ScrollY;

		public static float ScrollMaxX => GImGui.ScrollMaxX;

		public static float ScrollMaxY => GImGui.ScrollMaxY;

		public static void SetScrollHereX(float centerXRatio = 0.5f) => GImGui.SetScrollHereX(centerXRatio);

		public static void SetScrollHereY(float centerYRatio = 0.5f) => GImGui.SetScrollHereY(centerYRatio);

		public static void SetScrollFromPosX(float localX, float centerXRatio = 0.5f) => GImGui.SetScrollFromPosX(localX, centerXRatio);

		public static void SetScrollFromPosY(float localY, float centerYRatio = 0.5f) => GImGui.SetScrollFromPosY(localY, centerYRatio);


		public static void PushFont(IImFont font) => GImGui.PushFont(font);

		public static void PopFont() => GImGui.PopFont();

		public static void PushStyleColor(ImGuiCol idx, uint col) => GImGui.PushStyleColor(idx, col);

		public static void PushStyleColor(ImGuiCol idx, Vector4 col) => GImGui.PushStyleColor(idx, col);

		public static void PopStyleColor(int count = 1) => GImGui.PopStyleColor(count);

		public static void PushStyleVar(ImGuiStyleVar idx, float val) => GImGui.PushStyleVar(idx, val);

		public static void PushStyleVar(ImGuiStyleVar idx, Vector2 val) => GImGui.PushStyleVar(idx, val);

		public static void PopStyleVar(int count = 1) => GImGui.PopStyleVar(count);

		public static void PushAllowKeyboardFocus(bool allowKeyboardFocus) => GImGui.PushAllowKeyboardFocus(allowKeyboardFocus);

		public static void PopAllowKeyboardFocus() => GImGui.PopAllowKeyboardFocus();

		public static void PushButtonRepeat(bool repeat) => GImGui.PushButtonRepeat(repeat);

		public static void PopButtonRepeat() => GImGui.PopButtonRepeat();


		public static void PushItemWidth(float itemWidth) => GImGui.PushItemWidth(itemWidth);

		public static void PopItemWidth() => GImGui.PopItemWidth();

		public static void SetNextItemWidth(float itemWidth) => GImGui.SetNextItemWidth(itemWidth);

		public static float CalcItemWidth() => GImGui.CalcItemWidth();

		public static void PushTextWrapPos(float wrapLocalPosX = 0) => GImGui.PushTextWrapPos(wrapLocalPosX);

		public static void PopTextWrapPos() => GImGui.PopTextWrapPos();


		public static IImFont Font => GImGui.Font;

		public static float FontSize => GImGui.FontSize;

		public static Vector2 FontTexUvWhitePixel => GImGui.FontTexUvWhitePixel;

		public static uint GetColorU32(ImGuiCol idx, float alphaMul = 1) => GImGui.GetColorU32(idx, alphaMul);

		public static uint GetColorU32(Vector4 col) => GImGui.GetColorU32(col);

		public static uint GetColorU32(uint col) => GImGui.GetColorU32(col);

		public static Vector4 GetStyleColorVec4(ImGuiCol idx) => GImGui.GetStyleColorVec4(idx);


		public static void Separator() => GImGui.Separator();

		public static void SameLine(float offsetFromStartX = 0, float spacing = -1) => GImGui.SameLine(offsetFromStartX, spacing);

		public static void NewLine() => GImGui.NewLine();

		public static void Spacing() => GImGui.Spacing();

		public static void Dummy(Vector2 size) => GImGui.Dummy(size);

		public static void Indent(float indentW = 0) => GImGui.Indent(indentW);

		public static void Unindent(float indentW = 0) => GImGui.Unindent(indentW);

		public static void BeginGroup() => GImGui.BeginGroup();

		public static void EndGroup() => GImGui.EndGroup();


		public static Vector2 CursorPos {
			get => GImGui.CursorPos;
			set => GImGui.CursorPos = value;
		}

		public static float CursorPosX {
			get => GImGui.CursorPosX;
			set => GImGui.CursorPosX = value;
		}

		public static float CursorPosY {
			get => GImGui.CursorPosY;
			set => GImGui.CursorPosY = value;
		}

		public static Vector2 CursorStartPos => GImGui.CursorStartPos;

		public static Vector2 CursorScreenPos {
			get => GImGui.CursorScreenPos;
			set => GImGui.CursorScreenPos = value;
		}

		public static void AlignTextToFramePadding() => GImGui.AlignTextToFramePadding();

		public static float TextLineHeight => GImGui.TextLineHeight;

		public static float TextLineHeightWithSpacing => GImGui.TextLineHeightWithSpacing;

		public static float FrameHeight => GImGui.FrameHeight;

		public static float FrameHeightWithSpacing => GImGui.FrameHeightWithSpacing;


		public static void PushID(string strID) => GImGui.PushID(strID);

		public static void PushID(nint ptrID) => GImGui.PushID(ptrID);

		public static void PushID(int id) => GImGui.PushID(id);

		public static void PopID() => GImGui.PopID();

		public static uint GetID(string strID) => GImGui.GetID(strID);

		public static uint GetID(nint ptrID) => GImGui.GetID(ptrID);


		public static void Text(string fmt) => GImGui.Text(fmt);

		public static void TextColored(Vector4 col, string fmt) => GImGui.TextColored(col, fmt);

		public static void TextDisabled(string fmt) => GImGui.TextDisabled(fmt);

		public static void TextWrapped(string fmt) => GImGui.TextWrapped(fmt);

		public static void LabelText(string label, string fmt) => GImGui.LabelText(label, fmt);

		public static void BulletText(string fmt) => GImGui.BulletText(fmt);


		public static bool Button(string label, Vector2 size = default) => GImGui.Button(label, size);

		public static bool SmallButton(string label) => GImGui.SmallButton(label);

		public static bool InvisibleButton(string strID, Vector2 size, ImGuiButtonFlags flags = default) => GImGui.InvisibleButton(strID, size, flags);

		public static bool ArrowButton(string strID, ImGuiDir dir) => GImGui.ArrowButton(strID, dir);

		public static void Image(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tintCol, Vector4 borderCol = default) =>
			GImGui.Image(userTextureID, size, uv0, uv1, tintCol, borderCol);

		public static void Image(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1) => Image(userTextureID, size, uv0, uv1, Vector4.One);

		public static void Image(nuint userTextureID, Vector2 size, Vector2 uv0 = default) => Image(userTextureID, size, uv0, Vector2.One);

		public static bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding, Vector4 bgCol, Vector4 tintCol) =>
			GImGui.ImageButton(userTextureID, size, uv0, uv1, framePadding, bgCol, tintCol);

		public static bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding = -1, Vector4 bgCol = default) => ImageButton(userTextureID, size, uv0, uv1, framePadding, bgCol, Vector4.One);

		public static bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0 = default) => ImageButton(userTextureID, size, uv0, Vector2.One);

		public static bool Checkbox(string label, ref bool v) => GImGui.Checkbox(label, ref v);

		public static bool CheckboxFlags(string label, ref int flags, int flagsValue) => GImGui.CheckboxFlags(label, ref flags, flagsValue);

		public static bool CheckboxFlags(string label, ref uint flags, uint flagsValue) => GImGui.CheckboxFlags(label, ref flags, flagsValue);

		public static bool RadioButton(string label, bool active) => GImGui.RadioButton(label, active);

		public static bool RadioButton(string label, ref int v, int vButton) => GImGui.RadioButton(label, ref v, vButton);

		public static void ProgressBar(float fraction, Vector2 sizeArg, string? overlay = null) => GImGui.ProgressBar(fraction, sizeArg, overlay);

		public static void ProgressBar(float fraction) => ProgressBar(fraction, new Vector2(ImGui.FltMin, 0));

		public static void Bullet() => GImGui.Bullet();


		public static bool BeginCombo(string label, string previewValue, ImGuiComboFlags flags = default) => GImGui.BeginCombo(label, previewValue, flags);

		public static void EndCombo() => GImGui.EndCombo();

		public static bool Combo(string label, ref int currentItem, IEnumerable<string> items, int popupMaxHeightInItems = -1) => GImGui.Combo(label, ref currentItem, items, popupMaxHeightInItems);

		public static bool Combo(string label, ref int currentItem, string itemsSeparatedByZeros, int popupMaxHeightInItems = -1) => GImGui.Combo(label, ref currentItem, itemsSeparatedByZeros, popupMaxHeightInItems);

		public static bool Combo(string label, ref int currentItem, IImGui.ComboItemsGetter itemsGetter, int itemsCount, int popupMaxHeightInItems = -1) => GImGui.Combo(label, ref currentItem, itemsGetter, itemsCount, popupMaxHeightInItems);


		public static bool DragFloat(string label, ref float v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.DragFloat(label, ref v, vSpeed, vMin, vMax, format, flags);

		public static bool DragFloat2(string label, ref Vector2 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.DragFloat2(label, ref v, vSpeed, vMin, vMax, format, flags);

		public static bool DragFloat3(string label, ref Vector3 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.DragFloat3(label, ref v, vSpeed, vMin, vMax, format, flags);

		public static bool DragFloat4(string label, ref Vector4 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.DragFloat4(label, ref v, vSpeed, vMin, vMax, format, flags);

		public static bool DragFloatRange2(string label, ref float vCurrentMin, ref float vCurrentMax, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", string? formatMax = null, ImGuiSliderFlags flags = default) =>
			GImGui.DragFloatRange2(label, ref vCurrentMin, ref vCurrentMax, vSpeed, vMin, vMax, format, formatMax, flags);

		public static bool DragInt(string label, ref int v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.DragInt(label, ref v, vSpeed, vMin, vMax, format, flags);

		public static bool DragInt2(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.DragInt2(label, v, vSpeed, vMin, vMax, format, flags);

		public static bool DragInt3(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.DragInt3(label, v, vSpeed, vMin, vMax, format, flags);

		public static bool DragInt4(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.DragInt4(label, v, vSpeed, vMin, vMax, format, flags);

		public static bool DragIntRange2(string label, ref int vCurrentMin, ref int vCurrentMax, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", string? formatMax = null, ImGuiSliderFlags flags = default) =>
			GImGui.DragIntRange2(label, ref vCurrentMin, ref vCurrentMax, vSpeed, vMin, vMax, format, formatMax, flags);

		public static bool DragScalar<T>(string label, ref T data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, string? format = default, ImGuiSliderFlags flags = default) where T : unmanaged =>
			GImGui.DragScalar<T>(label, ref data, vSpeed, min, max, format, flags);

		public static bool DragScalarN<T>(string label, Span<T> data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, string? format = default, ImGuiSliderFlags flags = default) where T : unmanaged =>
			GImGui.DragScalarN<T>(label, data, vSpeed, min, max, format, flags);


		public static bool SliderFloat(string label, ref float v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.SliderFloat(label, ref v, vMin, vMax, format, flags);

		public static bool SliderFloat2(string label, ref Vector2 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.SliderFloat2(label, ref v, vMin, vMax, format, flags);

		public static bool SliderFloat3(string label, ref Vector3 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.SliderFloat3(label, ref v, vMin, vMax, format, flags);

		public static bool SliderFloat4(string label, ref Vector4 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.SliderFloat4(label, ref v, vMin, vMax, format, flags);

		public static bool SliderAngle(string label, ref float vRad, float vDegreesMin = -360, float vDegreesMax = 360, string format = "%.0f deg", ImGuiSliderFlags flags = default) =>
			GImGui.SliderAngle(label, ref vRad, vDegreesMin, vDegreesMax, format, flags);

		public static bool SliderInt(string label, ref int v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.SliderInt(label, ref v, vMin, vMax, format, flags);

		public static bool SliderInt2(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.SliderInt2(label, v, vMin, vMax, format, flags);

		public static bool SliderInt3(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.SliderInt3(label, v, vMin, vMax, format, flags);

		public static bool SliderInt4(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.SliderInt4(label, v, vMin, vMax, format, flags);

		public static bool SliderScalar<T>(string label, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0) where T : unmanaged =>
			GImGui.SliderScalar<T>(label, ref data, min, max, format, flags);

		public static bool SliderScalarN<T>(string label, Span<T> data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0) where T : unmanaged =>
			GImGui.SliderScalarN<T>(label, data, min, max, format, flags);

		public static bool VSliderFloat(string label, Vector2 size, ref float v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default) =>
			GImGui.VSliderFloat(label, size, ref v, vMin, vMax, format, flags);

		public static bool VSliderInt(string label, Vector2 size, ref int v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default) =>
			GImGui.VSliderInt(label, size, ref v, vMin, vMax, format, flags);

		public static bool VSliderScalar<T>(string label, Vector2 size, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = default) where T : unmanaged =>
			GImGui.VSliderScalar<T>(label, size, ref data, min, max, format, flags);


		public static bool InputText(string label, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null) =>
			GImGui.InputText(label, buf, flags, callback);

		public static bool InputTextMultiline(string label, ImGuiTextBuffer buf, Vector2 size = default, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null) =>
			GImGui.InputTextMultiline(label, buf, size, flags, callback);

		public static bool InputTextWithHint(string label, string hint, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null) =>
			GImGui.InputTextWithHint(label, hint, buf, flags, callback);

		public static bool InputFloat(string label, ref float v, float step = 0, float stepFast = 0, string format = "%.3f", ImGuiInputTextFlags flags = default) =>
			GImGui.InputFloat(label, ref v, step, stepFast, format, flags);

		public static bool InputFloat2(string label, ref Vector2 v, string format = "%.3f", ImGuiInputTextFlags flags = default) =>
			GImGui.InputFloat2(label, ref v, format, flags);

		public static bool InputFloat3(string label, ref Vector3 v, string format = "%.3f", ImGuiInputTextFlags flags = default) =>
			GImGui.InputFloat3(label, ref v, format, flags);

		public static bool InputFloat4(string label, ref Vector4 v, string format = "%.3f", ImGuiInputTextFlags flags = default) =>
			GImGui.InputFloat4(label, ref v, format, flags);

		public static bool InputInt(string label, ref int v, int step = 0, int stepFast = 0, ImGuiInputTextFlags flags = default) =>
			GImGui.InputInt(label, ref v, step, stepFast, flags);

		public static bool InputInt2(string label, Span<int> v, ImGuiInputTextFlags flags = default) =>
			GImGui.InputInt2(label, v, flags);

		public static bool InputInt3(string label, Span<int> v, ImGuiInputTextFlags flags = default) =>
			GImGui.InputInt3(label, v, flags);

		public static bool InputInt4(string label, Span<int> v, ImGuiInputTextFlags flags = default) =>
			GImGui.InputInt4(label, v, flags);

		public static bool InputDouble(string label, ref double v, double step = 0, double stepFast = 0, string format = "%.6f", ImGuiInputTextFlags flags = default) =>
			GImGui.InputDouble(label, ref v, step, stepFast, format, flags);

		public static bool InputScalar<T>(string label, ref T data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, string? format = null, ImGuiInputTextFlags flags = default) =>
			GImGui.InputScalar<T>(label, ref data, pStep, pStepFast, format, flags);

		public static bool InputScalarN<T>(string label, Span<T> data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, string? format = null, ImGuiInputTextFlags flags = default) =>
			GImGui.InputScalarN<T>(label, data, pStep, pStepFast, format, flags);


		public static bool ColorEdit3(string label, ref Vector3 col, ImGuiColorEditFlags flags = default) => GImGui.ColorEdit3(label, ref col, flags);

		public static bool ColorEdit4(string label, ref Vector4 col, ImGuiColorEditFlags flags = default) => GImGui.ColorEdit4(label, ref col, flags);

		public static bool ColorPicker3(string label, ref Vector3 col, ImGuiColorEditFlags flags = default) => GImGui.ColorPicker3(label, ref col, flags);

		public static bool ColorPicker4(string label, ref Vector4 col, ImGuiColorEditFlags flags = default, Vector4? refCol = null)
			 => GImGui.ColorPicker4(label, ref col, flags, refCol);

		public static bool ColorButton(string descId, Vector4 col, ImGuiColorEditFlags flags = default, Vector2 size = default) => GImGui.ColorButton(descId, col, flags, size);

		public static void SetColorEditOptions(ImGuiColorEditFlags flags) => GImGui.SetColorEditOptions(flags);


		public static bool TreeNode(string label) => GImGui.TreeNode(label);

		public static bool TreeNode(string strID, string fmt) => GImGui.TreeNode(strID, fmt);

		public static bool TreeNode(nint ptrID, string fmt) => GImGui.TreeNode(ptrID, fmt);

		public static bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags = default) => GImGui.TreeNodeEx(label, flags);

		public static bool TreeNodeEx(string strID, ImGuiTreeNodeFlags flags, string fmt) => GImGui.TreeNodeEx(strID, flags, fmt);

		public static bool TreeNodeEx(nint ptrID, ImGuiTreeNodeFlags flags, string fmt) => GImGui.TreeNodeEx(ptrID, flags, fmt);

		public static void TreePush(string strID) => GImGui.TreePush(strID);

		public static void TreePush(nint ptrID = 0) => GImGui.TreePush(ptrID);

		public static void TreePop() => GImGui.TreePop();

		public static float TreeNodeToLabelSpacing => GImGui.TreeNodeToLabelSpacing;

		public static bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags = default) => GImGui.CollapsingHeader(label, flags);

		public static bool CollapsingHeader(string label, ref bool pVisible, ImGuiTreeNodeFlags flags = default) => GImGui.CollapsingHeader(label, ref pVisible, flags);

		public static void SetNextItemOpen(bool isOpen, ImGuiCond cond = default) => GImGui.SetNextItemOpen(isOpen, cond);


		public static bool Selectable(string label, bool selected = false, ImGuiSelectableFlags flags = default, Vector2 size = default) => GImGui.Selectable(label, selected, flags, size);

		public static bool Selectable(string label, ref bool selected, ImGuiSelectableFlags flags = default, Vector2 size = default) => GImGui.Selectable(label, ref selected, flags, size);


		public static bool BeginListBox(string label, Vector2 size = default) => GImGui.BeginListBox(label, size);

		public static void EndListBox() => GImGui.EndListBox();

		public static bool ListBox(string label, ref int currentItem, IEnumerable<string> items, int heightInItems = -1) => GImGui.ListBox(label, ref currentItem, items, heightInItems);

		public static bool ListBox(string label, ref int currentItem, IImGui.ListBoxItemsGetter itemsGetter, int itemsCount, int heightInItems = -1) => GImGui.ListBox(label, ref currentItem, itemsGetter, itemsCount, heightInItems);


		public static void PlotLines(string label, ReadOnlySpan<float> values, int valuesCount = -1, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1) =>
			GImGui.PlotLines(label, values, valuesCount, overlayText, scaleMin, scaleMax, graphSize, stride);

		public static void PlotLines(string label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default) =>
			GImGui.PlotLines(label, valuesGetter, valuesCount, valuesOffset, overlayText, scaleMin, scaleMax, graphSize);

		public static void PlotHistogram(string label, ReadOnlySpan<float> values, int valuesCount = -1, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1) =>
			GImGui.PlotHistogram(label, values, valuesCount, overlayText, scaleMin, scaleMax, graphSize, stride);

		public static void PlotHistogram(string label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default) =>
			GImGui.PlotHistogram(label, valuesGetter, valuesCount, valuesOffset, overlayText, scaleMin, scaleMax, graphSize);


		public static void Value(string prefix, bool b) => GImGui.Value(prefix, b);

		public static void Value(string prefix, int v) => GImGui.Value(prefix, v);

		public static void Value(string prefix, uint v) => GImGui.Value(prefix, v);

		public static void Value(string prefix, float v, string? floatFormat = null) => GImGui.Value(prefix, v, floatFormat);


		public static bool BeginMenuBar() => GImGui.BeginMenuBar();

		public static void EndMenuBar() => GImGui.EndMenuBar();

		public static bool BeginMainMenuBar() => GImGui.BeginMainMenuBar();

		public static void EndMainMenuBar() => GImGui.EndMainMenuBar();

		public static bool BeginMenu(string label, bool enabled = true) => GImGui.BeginMenu(label, enabled);

		public static void EndMenu() => GImGui.EndMenu();

		public static bool MenuItem(string label, string? shortcut = null, bool selected = false, bool enabled = true) => GImGui.MenuItem(label, shortcut, selected, enabled);

		public static bool MenuItem(string label, string? shortcut, ref bool selected, bool enabled = true) => GImGui.MenuItem(label, shortcut, ref selected, enabled);


		public static void BeginTooltip() => GImGui.BeginTooltip();

		public static void EndTooltip() => GImGui.EndTooltip();

		public static void SetTooltip(string fmt) => GImGui.SetTooltip(fmt);


		public static bool BeginPopup(string strID, ImGuiWindowFlags flags = default) => GImGui.BeginPopup(strID, flags);

		public static bool BeginPopupModal(string name, ref bool pOpen, ImGuiWindowFlags flags = default) => GImGui.BeginPopupModal(name, ref pOpen, flags);

		public static bool BeginPopupModal(string name, bool? open = null, ImGuiWindowFlags flags = default) => GImGui.BeginPopupModal(name, open, flags);

		public static void EndPopup() => GImGui.EndPopup();


		public static void OpenPopup(string strID, ImGuiPopupFlags popupFlags = default) => GImGui.OpenPopup(strID, popupFlags);

		public static void OpenPopup(uint id, ImGuiPopupFlags popupFlags = default) => GImGui.OpenPopup(id, popupFlags);

		public static void OpenPopupOnItemClick(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight) => GImGui.OpenPopupOnItemClick(strID, popupFlags);

		public static void CloseCurrentPopup() => GImGui.CloseCurrentPopup();


		public static bool BeginPopupContextItem(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight) => GImGui.BeginPopupContextItem(strID, popupFlags);

		public static bool BeginPopupContextWindow(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight) => GImGui.BeginPopupContextWindow(strID, popupFlags);

		public static bool BeginPopupContextVoid(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight) => GImGui.BeginPopupContextVoid(strID, popupFlags);

		// Popups: query functions
		//  - IsPopupOpen(): return true if the popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId: return true if any popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId + ImGuiPopupFlags_AnyPopupLevel: return true if any popup is open.

		public static bool IsPopupOpen(string strID, ImGuiPopupFlags flags = default) => GImGui.IsPopupOpen(strID, flags);

		// Tables
		// - Full-featured replacement for old Columns API.
		// - See Demo->Tables for demo code.
		// - See top of imgui_tables.cpp for general commentary.
		// - See ImGuiTableFlags_ and ImGuiTableColumnFlags_ enums for a description of available flags.
		// The typical call flow is:
		// - 1. Call BeginTable().
		// - 2. Optionally call TableSetupColumn() to submit column name/flags/defaults.
		// - 3. Optionally call TableSetupScrollFreeze() to request scroll freezing of columns/rows.
		// - 4. Optionally call TableHeadersRow() to submit a header row. Names are pulled from TableSetupColumn() data.
		// - 5. Populate contents:
		//    - In most situations you can use TableNextRow() + TableSetColumnIndex(N) to start appending into a column.
		//    - If you are using tables as a sort of grid, where every columns is holding the same type of contents,
		//      you may prefer using TableNextColumn() instead of TableNextRow() + TableSetColumnIndex().
		//      TableNextColumn() will automatically wrap-around into the next row if needed.
		//    - IMPORTANT: Comparatively to the old Columns() API, we need to call TableNextColumn() for the first column!
		//    - Summary of possible call flow:
		//        --------------------------------------------------------------------------------------------------------
		//        TableNextRow() -> TableSetColumnIndex(0) -> Text("Hello 0") -> TableSetColumnIndex(1) -> Text("Hello 1")  // OK
		//        TableNextRow() -> TableNextColumn()      -> Text("Hello 0") -> TableNextColumn()      -> Text("Hello 1")  // OK
		//                          TableNextColumn()      -> Text("Hello 0") -> TableNextColumn()      -> Text("Hello 1")  // OK: TableNextColumn() automatically gets to next row!
		//        TableNextRow()                           -> Text("Hello 0")                                               // Not OK! Missing TableSetColumnIndex() or TableNextColumn()! Text will not appear!
		//        --------------------------------------------------------------------------------------------------------
		// - 5. Call EndTable()

		public static bool BeginTable(string strID, int column, ImGuiTableFlags flags = default, Vector2 outerSize = default, float innerWidth = 0) => GImGui.BeginTable(strID, column, flags, outerSize, innerWidth);

		public static void EndTable() => GImGui.EndTable();

		public static void TableNextRow(ImGuiTableRowFlags rowFlags = default, float minRowHeight = 0) => GImGui.TableNextRow(rowFlags, minRowHeight);

		public static bool TableNextColumn() => GImGui.TableNextColumn();

		public static bool TableSetColumnIndex(int columnN) => GImGui.TableSetColumnIndex(columnN);

		// Tables: Headers & Columns declaration
		// - Use TableSetupColumn() to specify label, resizing policy, default width/weight, id, various other flags etc.
		// - Use TableHeadersRow() to create a header row and automatically submit a TableHeader() for each column.
		//   Headers are required to perform: reordering, sorting, and opening the context menu.
		//   The context menu can also be made available in columns body using ImGuiTableFlags_ContextMenuInBody.
		// - You may manually submit headers using TableNextRow() + TableHeader() calls, but this is only useful in
		//   some advanced use cases (e.g. adding custom widgets in header row).
		// - Use TableSetupScrollFreeze() to lock columns/rows so they stay visible when scrolled.

		public static void TableSetupColumn(string label, ImGuiTableColumnFlags flags = 0, float initWidthOrWeight = 0, uint userID = 0) => GImGui.TableSetupColumn(label, flags, initWidthOrWeight, userID);

		public static void TableSetupScrollFreeze(int cols, int rows) => GImGui.TableSetupScrollFreeze(cols, rows);

		public static void TableHeadersRow() => GImGui.TableHeadersRow();

		public static void TableHeader(string label) => GImGui.TableHeader(label);

		// Tables: Sorting
		// - Call TableGetSortSpecs() to retrieve latest sort specs for the table. NULL when not sorting.
		// - When 'SpecsDirty == true' you should sort your data. It will be true when sorting specs have changed
		//   since last call, or the first time. Make sure to set 'SpecsDirty = false' after sorting, else you may
		//   wastefully sort your data every frame!
		// - Lifetime: don't hold on this pointer over multiple frames or past any subsequent call to BeginTable().

		public static IImGuiTableSortSpecs TableSortSpecs => GImGui.TableSortSpecs;

		// Tables: Miscellaneous functions
		// - Functions args 'int column_n' treat the default value of -1 as the same as passing the current column index.

		public static int TableColumnCount => GImGui.TableColumnCount;

		public static int TableColumnIndex => GImGui.TableColumnIndex;

		public static int TableRowIndex => GImGui.TableRowIndex;

		public static string TableGetColumnName(int columnN = -1) => GImGui.TableGetColumnName(columnN);

		public static ImGuiTableColumnFlags TableGetColumnFlags(int columnN = -1) => GImGui.TableGetColumnFlags(columnN);

		public static void TableSetColumnEnabled(int columnN, bool v) => GImGui.TableSetColumnEnabled(columnN, v);

		public static void TableSetBgColor(ImGuiTableBgTarget target, uint color, int columnN = -1) => GImGui.TableSetBgColor(target, color, columnN);

		// Legacy Columns API (prefer using Tables!)
		// - You can also use SameLine(pos_x) to mimic simplified columns.

		public static void Columns(int count = 1, string? id = null, bool border = true) => GImGui.Columns(count, id, border);

		public static void NextColumn() => GImGui.NextColumn();

		public static int ColumnIndex => GImGui.ColumnIndex;

		public static float GetColumnWidth(int columnIndex = -1) => GImGui.GetColumnWidth(columnIndex);

		public static void SetColumnWidth(int columnIndex, float width) => GImGui.SetColumnWidth(columnIndex, width);

		public static float GetColumnOffset(int columnIndex = -1) => GImGui.GetColumnWidth(columnIndex);

		public static void SetColumnOffset(int columnIndex, float width) => GImGui.SetColumnOffset(columnIndex, width);

		public static int ColumnsCount => GImGui.ColumnsCount;

		// Tab Bars, Tabs

		public static bool BeginTabBar(string strID, ImGuiTabBarFlags flags = default) => GImGui.BeginTabBar(strID, flags);

		public static void EndTabBar() => GImGui.EndTabBar();

		public static bool BeginTabItem(string label, ref bool open, ImGuiTabItemFlags flags = default) => GImGui.BeginTabItem(label, ref open, flags);

		public static bool BeginTabItem(string label, bool? open, ImGuiTabItemFlags flags = default) => GImGui.BeginTabItem(label, open, flags);

		public static void EndTabItem() => GImGui.EndTabItem();

		public static bool TabItemButton(string label, ImGuiTabItemFlags flags = default) => GImGui.TabItemButton(label, flags);

		public static void SetTabItemClosed(string tabOrDockedWindowLabel) => GImGui.SetTabItemClosed(tabOrDockedWindowLabel);

		// Logging/Capture
		// - All text output from the interface can be captured into tty/file/clipboard. By default, tree nodes are automatically opened during logging.

		public static void LogToTTY(int autoOpenDepth = -1) => GImGui.LogToTTY(autoOpenDepth);

		public static void LogToFile(int autoOpenDepth = -1, string? filename = null) => GImGui.LogToFile(autoOpenDepth, filename);

		public static void LogToClipboard(int autoOpenDepth = -1) => GImGui.LogToClipboard(autoOpenDepth);

		public static void LogFinish() => GImGui.LogFinish();

		public static void LogButtons() => GImGui.LogButtons();

		public static void LogText(string fmt) => GImGui.LogText(fmt);

		// Drag and Drop
		// - On source items, call BeginDragDropSource(), if it returns true also call SetDragDropPayload() + EndDragDropSource().
		// - On target candidates, call BeginDragDropTarget(), if it returns true also call AcceptDragDropPayload() + EndDragDropTarget().
		// - If you stop calling BeginDragDropSource() the payload is preserved however it won't have a preview tooltip (we currently display a fallback "..." tooltip, see #1725)
		// - An item can be both drag source and drop target.

		public static bool BeginDragDropSource(ImGuiDragDropFlags flags = 0) => GImGui.BeginDragDropSource(flags);

		public static bool SetDragDropPayload(string type, ReadOnlySpan<byte> data, ImGuiCond cond = default) => GImGui.SetDragDropPayload(type, data, cond);

		public static void EndDragDropSource() => GImGui.EndDragDropSource();

		public static bool BeginDragDropTarget() => GImGui.BeginDragDropTarget();

		public static IImGuiPayload? AcceptDragDropPayload(string type, ImGuiDragDropFlags flags = default) => GImGui.AcceptDragDropPayload(type, flags);

		public static void EndDragDropTarget() => GImGui.EndDragDropTarget();

		public static IImGuiPayload? DragDropPayload => GImGui.DragDropPayload;


		public static void BeginDisabled(bool disabled = true) => GImGui.BeginDisabled(disabled);

		public static void EndDisabled() => GImGui.EndDisabled();


		public static void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect) => GImGui.PushClipRect(clipRectMin, clipRectMax, intersectWithCurrentClipRect);

		public static void PopClipRect() => GImGui.PopClipRect();


		public static void SetItemDefaultFocus() => GImGui.SetItemDefaultFocus();

		public static void SetKeyboardFocusHere(int offset = default) => GImGui.SetKeyboardFocusHere(offset);


		public static bool IsItemHovered(ImGuiHoveredFlags flags = default) => GImGui.IsItemHovered(flags);

		public static bool IsItemActive => GImGui.IsItemActive;

		public static bool IsItemFocused => GImGui.IsItemFocused;

		public static bool IsItemClicked(ImGuiMouseButton mouseButton = default) => GImGui.IsItemClicked(mouseButton);

		public static bool IsItemVisible => GImGui.IsItemVisible;

		public static bool IsItemEdited => GImGui.IsItemEdited;

		public static bool IsItemActivated => GImGui.IsItemActivated;

		public static bool IsItemDeactivated => GImGui.IsItemDeactivated;

		public static bool IsItemDeactivatedAfterEdit => GImGui.IsItemDeactivatedAfterEdit;

		public static bool IsItemToggledOpen => GImGui.IsItemToggledOpen;

		public static bool IsAnyItemHovered => GImGui.IsAnyItemHovered;

		public static bool IsAnyItemActive => GImGui.IsAnyItemActive;

		public static bool IsAnyItemFocused => GImGui.IsAnyItemFocused;

		public static Vector2 ItemRectMin => GImGui.ItemRectMin;

		public static Vector2 ItemRectMax => GImGui.ItemRectMax;

		public static Vector2 ItemRectSize => GImGui.ItemRectSize;

		public static void SetItemAllowOverlap() => GImGui.SetItemAllowOverlap();


		public static ImGuiViewport MainViewport {
			get => GImGui.MainViewport;
			set => GImGui.MainViewport = value;
		}


		public static bool IsRectVisible(Vector2 size) => GImGui.IsRectVisible(size);

		public static bool IsRectVisible(Vector2 rectMin, Vector2 rectMax) => GImGui.IsRectVisible(rectMin, rectMax);

		public static double Time => GImGui.Time;

		public static int FrameCount => GImGui.FrameCount;

		public static IImDrawList BackgroundDrawList => GImGui.BackgroundDrawList;

		public static IImDrawList ForegroundDrawList => GImGui.ForegroundDrawList;

		public static IImDrawListSharedData DrawListSharedData => GImGui.DrawListSharedData;

		public static string GetStyleColorName(ImGuiCol idx) => GImGui.GetStyleColorName(idx);

		public static IImGuiStorage StateStorage {
			get => GImGui.StateStorage;
			set => GImGui.StateStorage = value;
		}

		public static bool BeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags = default) => GImGui.BeginChildFrame(id, size, flags);


		public static Vector2 CalcTextSize(string text, bool hideTextAfterDoubleHash = false, float wrapWidth = -1) => GImGui.CalcTextSize(text, hideTextAfterDoubleHash, wrapWidth);


		public static bool IsKeyDown(ImGuiKey key) => GImGui.IsKeyDown(key);

		public static bool IsKeyPressed(ImGuiKey key, bool repeat = true) => GImGui.IsKeyPressed(key, repeat);

		public static bool IsKeyReleased(ImGuiKey key) => GImGui.IsKeyReleased(key);

		public static int GetKeyPressedAmount(ImGuiKey key, float repeatDelay, float rate) => GImGui.GetKeyPressedAmount(key, repeatDelay, rate);

		public static string GetKeyName(ImGuiKey key) => GImGui.GetKeyName(key);

		public static void CaptureKeyboardFromApp(bool wantCaptureKeyboardValue = true) => GImGui.CaptureKeyboardFromApp(wantCaptureKeyboardValue);


		public static bool IsMouseDown(ImGuiMouseButton button) => GImGui.IsMouseDown(button);

		public static bool IsMouseClicked(ImGuiMouseButton button) => GImGui.IsMouseClicked(button);

		public static bool IsMouseReleased(ImGuiMouseButton button) => GImGui.IsMouseReleased(button);

		public static bool IsMouseDoubleClicked(ImGuiMouseButton button) => GImGui.IsMouseDoubleClicked(button);

		public static int GetMouseClickedAmount(ImGuiMouseButton button) => GImGui.GetMouseClickedAmount(button);

		public static bool IsMouseHoveringRect(Vector2 rMin, Vector2 rMax, bool clip = true) => GImGui.IsMouseHoveringRect(rMin, rMax, clip);

		public static bool IsMousePosValid(Vector2? mousePos = null) => GImGui.IsMousePosValid(mousePos);

		public static bool IsAnyMouseDown => GImGui.IsAnyMouseDown;

		public static Vector2 MousePos => GImGui.MousePos;

		public static Vector2 MousePosOnOpeningCurrentPopup => GImGui.MousePosOnOpeningCurrentPopup;

		public static bool IsMouseDragging(ImGuiMouseButton button, float lockThreshold = -1) => GImGui.IsMouseDragging(button, lockThreshold);

		public static Vector2 GetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left, float lockThreshold = -1) => GImGui.GetMouseDragDelta(button, lockThreshold);

		public static void ResetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left) => GImGui.ResetMouseDragDelta(button);

		public static ImGuiMouseCursor MouseCursor => GImGui.MouseCursor;

		public static void CaptureMouseFromApp(bool wantCaptureMouseValue = true) => GImGui.CaptureMouseFromApp(wantCaptureMouseValue);


		public static string ClipboardText {
			get => GImGui.ClipboardText;
			set => GImGui.ClipboardText = value;
		}


		public static void LoadIniSettingsFromDisk(string iniFilename) => GImGui.LoadIniSettingsFromDisk(iniFilename);

		public static void LoadIniSettingsFromMemory(in ReadOnlySpan<byte> iniData) => GImGui.LoadIniSettingsFromMemory(iniData);

		public static void SaveIniSettingsToDisk(string iniFilename) => GImGui.SaveIniSettingsToDisk(iniFilename);

		public static ReadOnlySpan<byte> SaveIniSettingsToMemory() => GImGui.SaveIniSettingsToMemory();


		/// <summary>
		/// Special Draw callback value to request renderer backend to reset the graphics/render state.
		/// The renderer backend needs to handle this special value, otherwise it will crash trying to call a function at this address.
		/// This is useful for example if you submitted callbacks which you know have altered the render state and you want it to be restored.
		/// It is not done by default because they are many perfectly useful way of altering render state for imgui contents (e.g. changing shader/blending settings before an Image call).
		/// </summary>
		public static readonly ImDrawCallback ResetRenderState = (IImDrawList parentList, in ImDrawCmd cmd) => { };

		// Color Utilities

		public const int Col32RShift = 0;
		public const int Col32GShift = 8;
		public const int Col32BShift = 16;
		public const int Col32AShift = 24;

		public static Vector4 ColorConvertU32ToFloat4(uint _in) {
			const float s = 1.0f / 255.0f;
			return new(
				((_in >> Col32RShift) & 0xFF) * s,
				((_in >> Col32GShift) & 0xFF) * s,
				((_in >> Col32BShift) & 0xFF) * s,
				((_in >> Col32AShift) & 0xFF) * s
			);
		}

		public static uint ColorConvertFloat4ToU32(Vector4 _in) {
			uint _out;
			_out = ((uint)F32ToInt8Sat(_in.X)) << Col32RShift;
			_out |= ((uint)F32ToInt8Sat(_in.Y)) << Col32GShift;
			_out |= ((uint)F32ToInt8Sat(_in.Z)) << Col32BShift;
			_out |= ((uint)F32ToInt8Sat(_in.W)) << Col32AShift;
			return _out;
		}

		public static void ColorConvertRGBToHSV(float r, float g, float b, out float h, out float s, out float v) {
			float K = 0;
			if (g < b) {
				(g, b) = (b, g);
				K = -1;
			}
			if (r < g) {
				(r, g) = (g, r);
				K = -2.0f / 6.0f - K;
			}

			float chroma = r - (g < b ? g : b);
			h = Math.Abs(K + (g - b) / (6 * chroma + 1e-20f));
			s = chroma / (r + 1e-20f);
			v = r;
		}

		public static void ColorConvertHSVToRGB(float h, float s, float v, out float r, out float g, out float b) {
			if (s == 0) {
				r = g = b = v;
				return;
			}

			h = (h % 1) / (60.0f / 360.0f);
			int i = (int)h;
			float f = h - i;
			float p = v * (1.0f - s);
			float q = v * (1.0f - s * f);
			float t = v * (1.0f - s * (1.0f - f));

			switch(i) {
				case 0:
					r = v;
					g = t;
					b = p;
					break;
				case 1:
					r = q;
					g = v;
					b = p;
					break;
				case 2:
					r = p;
					g = v;
					b = t;
					break;
				case 3:
					r = p;
					g = q;
					b = v;
					break;
				case 4:
					r = t;
					g = p;
					b = v;
					break;
				case 5:
				default:
					r = v;
					g = p;
					b = q;
					break;
			}
		}

		internal static float Saturate(float f) => f < 0 ? 0 : (f > 1) ? 1 : f;

		internal static int F32ToInt8Sat(float f) => (int)(Saturate(f) * 255.0f + 0.5f);

	}

}