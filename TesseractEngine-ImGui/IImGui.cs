using System.Numerics;

namespace Tesseract.ImGui {
	public interface IImGui {

		// Type constructors

		public IImGuiStyle NewStyle();

		public IImFontAtlas NewFontAtlas();

		public IImGuiStorage NewStorage();

		// Context creation and access
		// - Each context create its own ImFontAtlas by default. You may instance one yourself and pass it to CreateContext() to share a font atlas between contexts.
		// - DLL users: heaps and globals are not shared across DLL boundaries! You will need to call SetCurrentContext() + SetAllocatorFunctions()
		//   for each static/DLL boundary you are calling from. Read "Context and Memory Allocators" section of imgui.cpp for details.

		public IImGuiContext CreateContext(IImFontAtlas? sharedFontAtlas = null);

		public void DestroyContext(IImGuiContext? ctx = null);

		public IImGuiContext CurrentContext { get; set; }
		

		public IImGuiIO IO { get; }

		public IImGuiStyle Style { get; set; }

		public void NewFrame();

		public void EndFrame();

		public void Render();

		public IImDrawData GetDrawData();


		public void ShowDemoWindow(ref bool open);

		public void ShowDemoWindow(bool? open = null) {
			bool pOpen = open ?? true;
			ShowDemoWindow(ref pOpen);
		}

		public void ShowMetricsWindow(ref bool open);

		public void ShowMetricsWindow(bool? open = null) {
			bool pOpen = open ?? true;
			ShowMetricsWindow(ref pOpen);
		}

		public void ShowStackToolWindow(ref bool open);

		public void ShowStackToolWindow(bool? open = null) {
			bool pOpen = open ?? true;
			ShowStackToolWindow(ref pOpen);
		}

		public void ShowAboutWindow(ref bool open);

		public void ShowAboutWindow(bool? open = null) {
			bool pOpen = open ?? true;
			ShowAboutWindow(ref pOpen);
		}

		public void ShowStyleEditor(IImGuiStyle? style = null);

		public void ShowStyleSelector(string label);

		public void ShowFontSelector(string label);

		public void ShowUserGuide();

		public string Version { get; }


		public void StyleColorsDark(IImGuiStyle? dst = null);

		public void StyleColorsLight(IImGuiStyle? dst = null);

		public void StyleColorsClassic(IImGuiStyle? dst = null);

		// Windows
		// - Begin() = push window to the stack and start appending to it. End() = pop window from the stack.
		// - Passing 'bool* p_open != NULL' shows a window-closing widget in the upper-right corner of the window,
		//   which clicking will set the boolean to false when clicked.
		// - You may append multiple times to the same window during the same frame by calling Begin()/End() pairs multiple times.
		//   Some information such as 'flags' or 'p_open' will only be considered by the first call to Begin().
		// - Begin() return false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting
		//   anything to the window. Always call a matching End() for each Begin() call, regardless of its return value!
		//   [Important: due to legacy reason, this is inconsistent with most other functions such as BeginMenu/EndMenu,
		//    BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding BeginXXX function
		//    returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]
		// - Note that the bottom of window stack always contains a window called "Debug".

		public void Begin(string name, ref bool open, ImGuiWindowFlags flags = default);

		public void Begin(string name, bool? open = null, ImGuiWindowFlags flags = default) {
			bool pOpen = open ?? true;
			Begin(name, ref pOpen, flags);
		}

		public void End();

		// Child Windows
		// - Use child windows to begin into a self-contained independent scrolling/clipping regions within a host window. Child windows can embed their own child.
		// - For each independent axis of 'size': ==0.0f: use remaining host window size / >0.0f: fixed size / <0.0f: use remaining window size minus abs(size) / Each axis can use a different mode, e.g. ImVec2(0,400).
		// - BeginChild() returns false to indicate the window is collapsed or fully clipped, so you may early out and omit submitting anything to the window.
		//   Always call a matching EndChild() for each BeginChild() call, regardless of its return value.
		//   [Important: due to legacy reason, this is inconsistent with most other functions such as BeginMenu/EndMenu,
		//    BeginPopup/EndPopup, etc. where the EndXXX call should only be called if the corresponding BeginXXX function
		//    returned true. Begin and BeginChild are the only odd ones out. Will be fixed in a future update.]

		public void BeginChild(string strId, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = 0);

		public void BeginChild(uint id, Vector2 size = default, bool border = false, ImGuiWindowFlags flags = 0);

		public void EndChild();

		// Windows Utilities
		// - 'current window' = the window we are appending into while inside a Begin()/End() block. 'next window' = next window we will Begin() into.

		public bool IsWindowAppearing { get; }

		public bool IsWindowCollapsed { get; }

		public bool IsWindowFocused(ImGuiFocusedFlags flags = default);

		public bool IsWindowHovered(ImGuiHoveredFlags flags = default);

		public IImDrawList GetWindowDrawList();

		public Vector2 WindowPos { get; }

		public Vector2 WindowSize { get; }

		public float WindowWidth { get; }

		public float WindowHeight { get; }

		// Window manipulation
		// - Prefer using SetNextXXX functions (before Begin) rather that SetXXX functions (after Begin).

		public void SetNextWindowPos(Vector2 pos, ImGuiCond cond = default, Vector2 pivot = default);

		public void SetNextWindowSize(Vector2 size, ImGuiCond cond = default);

		public void SetNextWindowSizeConstraints(Vector2 sizeMin, Vector2 sizeMax, ImGuiSizeCallback? customCallback = null);

		public void SetNextWindowContentSize(Vector2 size);

		public void SetNextWindowCollapsed(bool collapsed, ImGuiCond cond = default);

		public void SetNextWindowFocus();

		public void SetNextWindowBgAlpha(float alpha);

		public void SetWindowPos(Vector2 pos, ImGuiCond cond = default);

		public void SetWindowSize(Vector2 size, ImGuiCond cond = default);

		public void SetWindowCollapsed(bool collapsed, ImGuiCond cond = default);

		public void SetWindowFocus();

		public void SetWindowFontScale(float scale);

		public void SetWindowPos(string name, Vector2 pos, ImGuiCond cond = default);

		public void SetWindowSize(string name, Vector2 size, ImGuiCond cond = default);

		public void SetWindowCollapsed(string name, bool collapsed, ImGuiCond cond = default);

		public void SetWindowFocus(string name);

		// Content region
		// - Retrieve available space from a given point. GetContentRegionAvail() is frequently useful.
		// - Those functions are bound to be redesigned (they are confusing, incomplete and the Min/Max return values are in local window coordinates which increases confusion)

		public Vector2 ContentRegionAvail { get; }

		public Vector2 ContentRegionMax { get; }

		public Vector2 WindowContentRegionMax { get; }

		public Vector2 WindowContentRegionMin { get; }

		// Windows Scrolling

		public float ScrollX { get; set; }

		public float ScrollY { get; set; }

		public float ScrollMaxX { get; }

		public float ScrollMaxY { get; }

		public void SetScrollHereX(float centerXRatio = 0.5f);

		public void SetScrollHereY(float centerYRatio = 0.5f);

		public void SetScrollFromPosX(float localX, float centerXRatio = 0.5f);

		public void SetScrollFromPosY(float localY, float centerYRatio = 0.5f);

		// Parameters stacks (shared)

		public void PushFont(IImFont font);

		public void PopFont();

		public void PushStyleColor(ImGuiCol idx, uint col);

		public void PushStyleColor(ImGuiCol idx, Vector4 col);

		public void PopStyleColor(int count = 1);

		public void PushStyleVar(ImGuiStyleVar idx, float val);

		public void PushStyleVar(ImGuiStyleVar idx, Vector2 val);

		public void PopStyleVar(int count = 1);

		public void PushAllowKeyboardFocus(bool allowKeyboardFocus);

		public void PopAllowKeyboardFocus();

		public void PushButtonRepeat(bool repeat);

		public void PopButtonRepeat();

		// Parameters stacks (current window)

		public void PushItemWidth(float itemWidth);

		public void PopItemWidth();

		public void SetNextItemWidth(float itemWidth);

		public float CalcItemWidth();

		public void PushTextWrapPos(float wrapLocalPosX = 0);

		public void PopTextWrapPos();

		// Style read access
		// - Use the style editor (ShowStyleEditor() function) to interactively see what the colors are)

		public IImFont Font { get; }

		public float FontSize { get; }

		public Vector2 FontTexUvWhitePixel { get; }

		public uint GetColorU32(ImGuiCol idx, float alphaMul = 1);

		public uint GetColorU32(Vector4 col);

		public uint GetColorU32(uint col);

		public Vector4 GetStyleColorVec4(ImGuiCol idx);

		// Cursor / Layout
		// - By "cursor" we mean the current output position.
		// - The typical widget behavior is to output themselves at the current cursor position, then move the cursor one line down.
		// - You can call SameLine() between widgets to undo the last carriage return and output at the right of the preceding widget.
		// - Attention! We currently have inconsistencies between window-local and absolute positions we will aim to fix with future API:
		//    Window-local coordinates:   SameLine(), GetCursorPos(), SetCursorPos(), GetCursorStartPos(), GetContentRegionMax(), GetWindowContentRegion*(), PushTextWrapPos()
		//    Absolute coordinate:        GetCursorScreenPos(), SetCursorScreenPos(), all ImDrawList:: functions.

		public void Separator();

		public void SameLine(float offsetFromStartX = 0, float spacing = -1);

		public void NewLine();

		public void Spacing();

		public void Dummy(Vector2 size);

		public void Indent(float indentW = 0);

		public void Unindent(float indentW = 0);

		public void BeginGroup();

		public void EndGroup();

		public Vector2 CursorPos { get; set; }

		public float CursorPosX { get; set; }
		
		public float CursorPosY { get; set; }

		public Vector2 CursorStartPos { get; }

		public Vector2 CursorScreenPos { get; set; }

		public void AlignTextToFramePadding();

		public float TextLineHeight { get; }

		public float TextLineHeightWithSpacing { get; }

		public float FrameHeight { get; }

		public float FrameHeightWithSpacing { get; }

		// ID stack/scopes
		// Read the FAQ (docs/FAQ.md or http://dearimgui.org/faq) for more details about how ID are handled in dear imgui.
		// - Those questions are answered and impacted by understanding of the ID stack system:
		//   - "Q: Why is my widget not reacting when I click on it?"
		//   - "Q: How can I have widgets with an empty label?"
		//   - "Q: How can I have multiple widgets with the same label?"
		// - Short version: ID are hashes of the entire ID stack. If you are creating widgets in a loop you most likely
		//   want to push a unique identifier (e.g. object pointer, loop index) to uniquely differentiate them.
		// - You can also use the "Label##foobar" syntax within widget label to distinguish them from each others.
		// - In this header file we use the "label"/"name" terminology to denote a string that will be displayed + used as an ID,
		//   whereas "str_id" denote a string that is only used as an ID and not normally displayed.

		public void PushID(string strID);

		public void PushID(nint ptrID);

		public void PushID(int id);

		public void PopID();

		public uint GetID(string strID);

		public uint GetID(nint ptrID);

		// Widgets: Text

		public void Text(string fmt);

		public void TextColored(Vector4 col, string fmt);

		public void TextDisabled(string fmt);

		public void TextWrapped(string fmt);

		public void LabelText(string label, string fmt);

		public void BulletText(string fmt);

		// Widgets: Main
		// - Most widgets return true when the value has been changed or when pressed/selected
		// - You may also use one of the many IsItemXXX functions (e.g. IsItemActive, IsItemHovered, etc.) to query widget state.

		public bool Button(string label, Vector2 size = default);

		public bool SmallButton(string label);

		public bool InvisibleButton(string strID, Vector2 size, ImGuiButtonFlags flags = default);

		public bool ArrowButton(string strID, ImGuiDir dir);

		public void Image(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tintCol, Vector4 borderCol = default);

		public void Image(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1) => Image(userTextureID, size, uv0, uv1, Vector4.One);

		public void Image(nuint userTextureID, Vector2 size, Vector2 uv0 = default) => Image(userTextureID, size, uv0, Vector2.One);

		public bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding, Vector4 bgCol, Vector4 tintCol);

		public bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0, Vector2 uv1, int framePadding = -1, Vector4 bgCol = default) => ImageButton(userTextureID, size, uv0, uv1, framePadding, bgCol, Vector4.One);

		public bool ImageButton(nuint userTextureID, Vector2 size, Vector2 uv0 = default) => ImageButton(userTextureID, size, uv0, Vector2.One);

		public bool Checkbox(string label, ref bool v);

		public bool CheckboxFlags(string label, ref int flags, int flagsValue);

		public bool CheckboxFlags(string label, ref uint flags, uint flagsValue);

		public bool RadioButton(string label, bool active);

		public bool RadioButton(string label, ref int v, int vButton);

		public void ProgressBar(float fraction, Vector2 sizeArg, string? overlay = null);

		public void ProgressBar(float fraction) => ProgressBar(fraction, new Vector2(GImGui.FltMin, 0));

		public void Bullet();

		// Widgets: Combo Box
		// - The BeginCombo()/EndCombo() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() items.
		// - The old Combo() api are helpers over BeginCombo()/EndCombo() which are kept available for convenience purpose. This is analogous to how ListBox are created.

		public bool BeginCombo(string label, string previewValue, ImGuiComboFlags flags = 0);

		public void EndCombo();

		public bool Combo(string label, ref int currentItem, IEnumerable<string> items, int popupMaxHeightInItems = -1);

		public bool Combo(string label, ref int currentItem, string itemsSeparatedByZeros, int popupMaxHeightInItems = -1);

		public delegate bool ComboItemsGetter(int idx, out string text);

		public bool Combo(string label, ref int currentItem, ComboItemsGetter itemsGetter, int itemscount, int popupMaxHeightInItems = -1);

		// Widgets: Drag Sliders
		// - CTRL+Click on any drag box to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.
		// - For all the Float2/Float3/Float4/Int2/Int3/Int4 versions of every functions, note that a 'float v[X]' function argument is the same as 'float* v',
		//   the array syntax is just a way to document the number of elements that are expected to be accessible. You can pass address of your first element out of a contiguous set, e.g. &myvector.x
		// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: %.0f" -> Biscuit: 1; etc.
		// - Format string may also be set to NULL or use the default format ("%f" or "%d").
		// - Speed are per-pixel of mouse movement (v_speed=0.2f: mouse needs to move by 5 pixels to increase value by 1). For gamepad/keyboard navigation, minimum speed is Max(v_speed, minimum_step_at_given_precision).
		// - Use v_min < v_max to clamp edits to given limits. Note that CTRL+Click manual input can override those limits if ImGuiSliderFlags_AlwaysClamp is not used.
		// - Use v_max = FLT_MAX / INT_MAX etc to avoid clamping to a maximum, same with v_min = -FLT_MAX / INT_MIN to avoid clamping to a minimum.
		// - We use the same sets of flags for DragXXX() and SliderXXX() functions as the features are the same and it makes it easier to swap them.
		// - Legacy: Pre-1.78 there are DragXXX() function signatures that takes a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.
		//   If you get a warning converting a float to ImGuiSliderFlags, read https://github.com/ocornut/imgui/issues/3361

		public bool DragFloat(string label, ref float v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool DragFloat2(string label, ref Vector2 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool DragFloat3(string label, ref Vector3 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool DragFloat4(string label, ref Vector4 v, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool DragFloatRange2(string label, ref float vCurrentMin, ref float vCurrentMax, float vSpeed = 1, float vMin = 0, float vMax = 0, string format = "%.3f", string? formatMax = null, ImGuiSliderFlags flags = default);

		public bool DragInt(string label, ref int v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default);

		// You may ask "why not just pass VectorXi by ref?" and the answer is because the C++/CLI compiler is retarded and has an aneurysim trying to import the type
		// To keep the C++/CLI compiler from exploding a workaround is to use spans and the .AsSpan property for passing to the function

		public bool DragInt2(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default);

		public bool DragInt3(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default);

		public bool DragInt4(string label, Span<int> v, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", ImGuiSliderFlags flags = default);

		public bool DragIntRange2(string label, ref int vCurrentMin, ref int vCurrentMax, float vSpeed = 1, int vMin = 0, int vMax = 0, string format = "%d", string? formatMax = null, ImGuiSliderFlags flags = default);

		// See ImNullable for why there are shennanigans here
		// Note: In practice T will be unmanaged, but again the C++/CLI compiler is retarded and only understands structs. We give the unmanaged restriction in GImGui.

		public bool DragScalar<T>(string label, ref T data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, string? format = default, ImGuiSliderFlags flags = default) where T : struct;

		public bool DragScalarN<T>(string label, Span<T> data, float vSpeed = 1, ImNullable<T> min = default, ImNullable<T> max = default, string? format = default, ImGuiSliderFlags flags = default) where T : struct;

		// Widgets: Regular Sliders
		// - CTRL+Click on any slider to turn them into an input box. Manually input values aren't clamped by default and can go off-bounds. Use ImGuiSliderFlags_AlwaysClamp to always clamp.
		// - Adjust format string to decorate the value with a prefix, a suffix, or adapt the editing and display precision e.g. "%.3f" -> 1.234; "%5.2f secs" -> 01.23 secs; "Biscuit: %.0f" -> Biscuit: 1; etc.
		// - Format string may also be set to NULL or use the default format ("%f" or "%d").
		// - Legacy: Pre-1.78 there are SliderXXX() function signatures that takes a final `float power=1.0f' argument instead of the `ImGuiSliderFlags flags=0' argument.
		//   If you get a warning converting a float to ImGuiSliderFlags, read https://github.com/ocornut/imgui/issues/3361

		public bool SliderFloat(string label, ref float v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool SliderFloat2(string label, ref Vector2 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool SliderFloat3(string label, ref Vector3 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool SliderFloat4(string label, ref Vector4 v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool SliderAngle(string label, ref float vRad, float vDegreesMin = -360, float vDegreesMax = 360, string format = "%.0f deg", ImGuiSliderFlags flags = default);

		public bool SliderInt(string label, ref int v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default);

		public bool SliderInt2(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default);

		public bool SliderInt3(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default);

		public bool SliderInt4(string label, Span<int> v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default);

		public bool SliderScalar<T>(string label, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0) where T : struct;

		public bool SliderScalarN<T>(string label, Span<T> data, T min, T max, string? format = null, ImGuiSliderFlags flags = 0) where T : struct;

		public bool VSliderFloat(string label, Vector2 size, ref float v, float vMin, float vMax, string format = "%.3f", ImGuiSliderFlags flags = default);

		public bool VSliderInt(string label, Vector2 size, ref int v, int vMin, int vMax, string format = "%d", ImGuiSliderFlags flags = default);

		public bool VSliderScalar<T>(string label, Vector2 size, ref T data, T min, T max, string? format = null, ImGuiSliderFlags flags = default) where T : struct;

		// Widgets: Input with Keyboard
		// - If you want to use InputText() with std::string or any custom dynamic string type, see misc/cpp/imgui_stdlib.h and comments in imgui_demo.cpp.
		// - Most of the ImGuiInputTextFlags flags are only useful for InputText() and not for InputFloatX, InputIntX, InputDouble etc.

		public bool InputText(string label, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null);

		public bool InputTextMultiline(string label, ImGuiTextBuffer buf, Vector2 size = default, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null);

		public bool InputTextWithHint(string label, string hint, ImGuiTextBuffer buf, ImGuiInputTextFlags flags = default, ImGuiInputTextCallback? callback = null);

		public bool InputFloat(string label, ref float v, float step = 0, float stepFast = 0, string format = "%.3f", ImGuiInputTextFlags flags = default);

		public bool InputFloat2(string label, ref Vector2 v, string format = "%.3f", ImGuiInputTextFlags flags = default);

		public bool InputFloat3(string label, ref Vector3 v, string format = "%.3f", ImGuiInputTextFlags flags = default);

		public bool InputFloat4(string label, ref Vector4 v, string format = "%.3f", ImGuiInputTextFlags flags = default);

		public bool InputInt(string label, ref int v, int step = 0, int stepFast = 0, ImGuiInputTextFlags flags = default);

		public bool InputInt2(string label, Span<int> v, ImGuiInputTextFlags flags = default);

		public bool InputInt3(string label, Span<int> v, ImGuiInputTextFlags flags = default);

		public bool InputInt4(string label, Span<int> v, ImGuiInputTextFlags flags = default);

		public bool InputDouble(string label, ref double v, double step = 0, double stepFast = 0, string format = "%.6f", ImGuiInputTextFlags flags = default);

		public bool InputScalar<T>(string label, ref T data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, string? format = null, ImGuiInputTextFlags flags = default) where T : struct;

		public bool InputScalarN<T>(string label, Span<T> data, ImNullable<T> pStep = default, ImNullable<T> pStepFast = default, string? format = null, ImGuiInputTextFlags flags = default) where T : struct;

		// Widgets: Color Editor/Picker (tip: the ColorEdit* functions have a little color square that can be left-clicked to open a picker, and right-clicked to open an option menu.)
		// - Note that in C++ a 'float v[X]' function argument is the _same_ as 'float* v', the array syntax is just a way to document the number of elements that are expected to be accessible.
		// - You can pass the address of a first float element out of a contiguous structure, e.g. &myvector.x

		public bool ColorEdit3(string label, ref Vector3 col, ImGuiColorEditFlags flags = default);

		public bool ColorEdit4(string label, ref Vector4 col, ImGuiColorEditFlags flags = default);

		public bool ColorPicker3(string label, ref Vector3 col, ImGuiColorEditFlags flags = default);

		public bool ColorPicker4(string label, ref Vector4 col, ImGuiColorEditFlags flags = default, Vector4? refCol = null);

		public bool ColorButton(string descId, Vector4 col, ImGuiColorEditFlags flags = default, Vector2 size = default);

		public void SetColorEditOptions(ImGuiColorEditFlags flags);

		// Widgets: Trees
		// - TreeNode functions return true when the node is open, in which case you need to also call TreePop() when you are finished displaying the tree node contents.

		public bool TreeNode(string label);

		public bool TreeNode(string strID, string fmt);

		public bool TreeNode(nint ptrID, string fmt);

		public bool TreeNodeEx(string label, ImGuiTreeNodeFlags flags = default);

		public bool TreeNodeEx(string strID, ImGuiTreeNodeFlags flags, string fmt);

		public bool TreeNodeEx(nint ptrID, ImGuiTreeNodeFlags flags, string fmt);

		public void TreePush(string strID);

		public void TreePush(nint ptrID = 0);

		public void TreePop();

		public float TreeNodeToLabelSpacing { get; }

		public bool CollapsingHeader(string label, ImGuiTreeNodeFlags flags = default);

		public bool CollapsingHeader(string label, ref bool pVisible, ImGuiTreeNodeFlags flags = default);

		public void SetNextItemOpen(bool isOpen, ImGuiCond cond = default);

		// Widgets: Selectables
		// - A selectable highlights when hovered, and can display another color when selected.
		// - Neighbors selectable extend their highlight bounds in order to leave no gap between them. This is so a series of selected Selectable appear contiguous.

		public bool Selectable(string label, bool selected = false, ImGuiSelectableFlags flags = default, Vector2 size = default);

		public bool Selectable(string label, ref bool selected, ImGuiSelectableFlags flags = default, Vector2 size = default);

		// Widgets: List Boxes
		// - This is essentially a thin wrapper to using BeginChild/EndChild with some stylistic changes.
		// - The BeginListBox()/EndListBox() api allows you to manage your contents and selection state however you want it, by creating e.g. Selectable() or any items.
		// - The simplified/old ListBox() api are helpers over BeginListBox()/EndListBox() which are kept available for convenience purpose. This is analoguous to how Combos are created.
		// - Choose frame width:   size.x > 0.0f: custom  /  size.x < 0.0f or -FLT_MIN: right-align   /  size.x = 0.0f (default): use current ItemWidth
		// - Choose frame height:  size.y > 0.0f: custom  /  size.y < 0.0f or -FLT_MIN: bottom-align  /  size.y = 0.0f (default): arbitrary default height which can fit ~7 items

		public bool BeginListBox(string label, Vector2 size = default);

		public void EndListBox();

		public bool ListBox(string label, ref int currentItem, IEnumerable<string> items, int heightInItems = -1);

		public delegate bool ListBoxItemsGetter(int idx, out string text);

		public bool ListBox(string label, ref int currentItem, ListBoxItemsGetter itemsGetter, int itemsCount, int heightInItems = -1);

		// Widgets: Data Plotting
		// - Consider using ImPlot (https://github.com/epezent/implot) which is much better!

		public void PlotLines(string label, ReadOnlySpan<float> values, int valuesCount = -1, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1);

		public void PlotLines(string label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);

		public void PlotHistogram(string label, ReadOnlySpan<float> values, int valuesCount = -1, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default, int stride = 1);

		public void PlotHistogram(string label, Func<int, float> valuesGetter, int valuesCount, int valuesOffset = 0, string? overlayText = null, float scaleMin = float.MaxValue, float scaleMax = float.MaxValue, Vector2 graphSize = default);

		// Widgets: Value() Helpers.
		// - Those are merely shortcut to calling Text() with a format string. Output single value in "name: value" format (tip: freely declare more in your code to handle your types. you can add functions to the ImGui namespace)

		public void Value(string prefix, bool b);

		public void Value(string prefix, int v);

		public void Value(string prefix, uint v);

		public void Value(string prefix, float v, string? floatFormat = null);

		// Widgets: Menus
		// - Use BeginMenuBar() on a window ImGuiWindowFlags_MenuBar to append to its menu bar.
		// - Use BeginMainMenuBar() to create a menu bar at the top of the screen and append to it.
		// - Use BeginMenu() to create a menu. You can call BeginMenu() multiple time with the same identifier to append more items to it.
		// - Not that MenuItem() keyboardshortcuts are displayed as a convenience but _not processed_ by Dear ImGui at the moment.

		public bool BeginMenuBar();

		public void EndMenuBar();

		public bool BeginMainMenuBar();

		public void EndMainMenuBar();

		public bool BeginMenu(string label, bool enabled = true);

		public void EndMenu();

		public bool MenuItem(string label, string? shortcut = null, bool selected = false, bool enabled = true);

		public bool MenuItem(string label, string? shortcut, ref bool selected, bool enabled = true);

		// Tooltips
		// - Tooltip are windows following the mouse. They do not take focus away.

		public void BeginTooltip();

		public void EndTooltip();

		public void SetTooltip(string fmt);

		// Popups, Modals
		//  - They block normal mouse hovering detection (and therefore most mouse interactions) behind them.
		//  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.
		//  - Their visibility state (~bool) is held internally instead of being held by the programmer as we are used to with regular Begin*() calls.
		//  - The 3 properties above are related: we need to retain popup visibility state in the library because popups may be closed as any time.
		//  - You can bypass the hovering restriction by using ImGuiHoveredFlags_AllowWhenBlockedByPopup when calling IsItemHovered() or IsWindowHovered().
		//  - IMPORTANT: Popup identifiers are relative to the current ID stack, so OpenPopup and BeginPopup generally needs to be at the same level of the stack.
		//    This is sometimes leading to confusing mistakes. May rework this in the future.

		// Popups: begin/end functions
		//  - BeginPopup(): query popup state, if open start appending into the window. Call EndPopup() afterwards. ImGuiWindowFlags are forwarded to the window.
		//  - BeginPopupModal(): block every interactions behind the window, cannot be closed by user, add a dimming background, has a title bar.

		public bool BeginPopup(string strID, ImGuiWindowFlags flags = default);

		public bool BeginPopupModal(string name, ref bool pOpen, ImGuiWindowFlags flags = default);

		public bool BeginPopupModal(string name, bool? open = null, ImGuiWindowFlags flags = default) {
			bool pOpen = open ?? true;
			return BeginPopupModal(name, ref pOpen, flags);
		}

		public void EndPopup();

		// Popups: open/close functions
		//  - OpenPopup(): set popup state to open. ImGuiPopupFlags are available for opening options.
		//  - If not modal: they can be closed by clicking anywhere outside them, or by pressing ESCAPE.
		//  - CloseCurrentPopup(): use inside the BeginPopup()/EndPopup() scope to close manually.
		//  - CloseCurrentPopup() is called by default by Selectable()/MenuItem() when activated (FIXME: need some options).
		//  - Use ImGuiPopupFlags_NoOpenOverExistingPopup to avoid opening a popup if there's already one at the same level. This is equivalent to e.g. testing for !IsAnyPopupOpen() prior to OpenPopup().
		//  - Use IsWindowAppearing() after BeginPopup() to tell if a window just opened.
		//  - IMPORTANT: Notice that for OpenPopupOnItemClick() we exceptionally default flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter

		public void OpenPopup(string strID, ImGuiPopupFlags popupFlags = default);

		public void OpenPopup(uint id, ImGuiPopupFlags popupFlags = default);

		public void OpenPopupOnItemClick(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight);

		public void CloseCurrentPopup();

		// Popups: open+begin combined functions helpers
		//  - Helpers to do OpenPopup+BeginPopup where the Open action is triggered by e.g. hovering an item and right-clicking.
		//  - They are convenient to easily create context menus, hence the name.
		//  - IMPORTANT: Notice that BeginPopupContextXXX takes ImGuiPopupFlags just like OpenPopup() and unlike BeginPopup(). For full consistency, we may add ImGuiWindowFlags to the BeginPopupContextXXX functions in the future.
		//  - IMPORTANT: Notice that we exceptionally default their flags to 1 (== ImGuiPopupFlags_MouseButtonRight) for backward compatibility with older API taking 'int mouse_button = 1' parameter, so if you add other flags remember to re-add the ImGuiPopupFlags_MouseButtonRight.

		public bool BeginPopupContextItem(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight);

		public bool BeginPopupContextWindow(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight);

		public bool BeginPopupContextVoid(string? strID = null, ImGuiPopupFlags popupFlags = ImGuiPopupFlags.MouseButtonRight);

		// Popups: query functions
		//  - IsPopupOpen(): return true if the popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId: return true if any popup is open at the current BeginPopup() level of the popup stack.
		//  - IsPopupOpen() with ImGuiPopupFlags_AnyPopupId + ImGuiPopupFlags_AnyPopupLevel: return true if any popup is open.

		public bool IsPopupOpen(string strID, ImGuiPopupFlags flags = default);

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

		public bool BeginTable(string strID, int column, ImGuiTableFlags flags = default, Vector2 outerSize = default, float innerWidth = 0);

		public void EndTable();

		public void TableNextRow(ImGuiTableRowFlags rowFlags = default, float minRowHeight = 0);

		public bool TableNextColumn();

		public bool TableSetColumnIndex(int columnN);

		// Tables: Headers & Columns declaration
		// - Use TableSetupColumn() to specify label, resizing policy, default width/weight, id, various other flags etc.
		// - Use TableHeadersRow() to create a header row and automatically submit a TableHeader() for each column.
		//   Headers are required to perform: reordering, sorting, and opening the context menu.
		//   The context menu can also be made available in columns body using ImGuiTableFlags_ContextMenuInBody.
		// - You may manually submit headers using TableNextRow() + TableHeader() calls, but this is only useful in
		//   some advanced use cases (e.g. adding custom widgets in header row).
		// - Use TableSetupScrollFreeze() to lock columns/rows so they stay visible when scrolled.

		public void TableSetupColumn(string label, ImGuiTableColumnFlags flags = 0, float initWidthOrWeight = 0, uint userID = 0);

		public void TableSetupScrollFreeze(int cols, int rows);

		public void TableHeadersRow();

		public void TableHeader(string label);

		// Tables: Sorting
		// - Call TableGetSortSpecs() to retrieve latest sort specs for the table. NULL when not sorting.
		// - When 'SpecsDirty == true' you should sort your data. It will be true when sorting specs have changed
		//   since last call, or the first time. Make sure to set 'SpecsDirty = false' after sorting, else you may
		//   wastefully sort your data every frame!
		// - Lifetime: don't hold on this pointer over multiple frames or past any subsequent call to BeginTable().

		public IImGuiTableSortSpecs TableSortSpecs { get; }

		// Tables: Miscellaneous functions
		// - Functions args 'int column_n' treat the default value of -1 as the same as passing the current column index.

		public int TableColumnCount { get; }

		public int TableColumnIndex { get; }

		public int TableRowIndex { get; }

		public string TableGetColumnName(int columnN = -1);

		public ImGuiTableColumnFlags TableGetColumnFlags(int columnN = -1);

		public void TableSetColumnEnabled(int columnN, bool v);

		public void TableSetBgColor(ImGuiTableBgTarget target, uint color, int columnN = -1);

		// Legacy Columns API (prefer using Tables!)
		// - You can also use SameLine(pos_x) to mimic simplified columns.

		public void Columns(int count = 1, string? id = null, bool border = true);

		public void NextColumn();

		public int ColumnIndex { get; }

		public float GetColumnWidth(int columnIndex = -1);

		public void SetColumnWidth(int columnIndex, float width);

		public float GetColumnOffset(int columnIndex = -1);

		public void SetColumnOffset(int columnIndex, float width);

		public int ColumnsCount { get; }

		// Tab Bars, Tabs

		public bool BeginTabBar(string strID, ImGuiTabBarFlags flags = default);

		public void EndTabBar();

		public bool BeginTabItem(string label, ref bool open, ImGuiTabItemFlags flags = default);

		public bool BeginTabItem(string label, bool? open, ImGuiTabItemFlags flags = default) {
			bool pOpen = open ?? true;
			return BeginTabItem(label, ref pOpen, flags);
		}

		public void EndTabItem();

		public bool TabItemButton(string label, ImGuiTabItemFlags flags = default);

		public void SetTabItemClosed(string tabOrDockedWindowLabel);

		// Logging/Capture
		// - All text output from the interface can be captured into tty/file/clipboard. By default, tree nodes are automatically opened during logging.

		public void LogToTTY(int autoOpenDepth = -1);

		public void LogToFile(int autoOpenDepth = -1, string? filename = null);

		public void LogToClipboard(int autoOpenDepth = -1);

		public void LogFinish();

		public void LogButtons();

		public void LogText(string fmt);

		// Drag and Drop
		// - On source items, call BeginDragDropSource(), if it returns true also call SetDragDropPayload() + EndDragDropSource().
		// - On target candidates, call BeginDragDropTarget(), if it returns true also call AcceptDragDropPayload() + EndDragDropTarget().
		// - If you stop calling BeginDragDropSource() the payload is preserved however it won't have a preview tooltip (we currently display a fallback "..." tooltip, see #1725)
		// - An item can be both drag source and drop target.

		public bool BeginDragDropSource(ImGuiDragDropFlags flags = 0);

		public bool SetDragDropPayload(string type, ReadOnlySpan<byte> data, ImGuiCond cond = default);

		public void EndDragDropSource();

		public bool BeginDragDropTarget();

		public IImGuiPayload? AcceptDragDropPayload(string type, ImGuiDragDropFlags flags = default);

		public void EndDragDropTarget();

		public IImGuiPayload? DragDropPayload { get; }

		// Disabling [BETA API]
		// - Disable all user interactions and dim items visuals (applying style.DisabledAlpha over current colors)
		// - Those can be nested but it cannot be used to enable an already disabled section (a single BeginDisabled(true) in the stack is enough to keep everything disabled)
		// - BeginDisabled(false) essentially does nothing useful but is provided to facilitate use of boolean expressions. If you can avoid calling BeginDisabled(False)/EndDisabled() best to avoid it.

		public void BeginDisabled(bool disabled = true);

		public void EndDisabled();

		// Clipping
		// - Mouse hovering is affected by ImGui::PushClipRect() calls, unlike direct calls to ImDrawList::PushClipRect() which are render only.

		public void PushClipRect(Vector2 clipRectMin, Vector2 clipRectMax, bool intersectWithCurrentClipRect);

		public void PopClipRect();

		// Focus, Activation
		// - Prefer using "SetItemDefaultFocus()" over "if (IsWindowAppearing()) SetScrollHereY()" when applicable to signify "this is the default item"

		public void SetItemDefaultFocus();

		public void SetKeyboardFocusHere(int offset = default);

		// Item/Widgets Utilities and Query Functions
		// - Most of the functions are referring to the previous Item that has been submitted.
		// - See Demo Window under "Widgets->Querying Status" for an interactive visualization of most of those functions.

		public bool IsItemHovered(ImGuiHoveredFlags flags = default);

		public bool IsItemActive { get; }

		public bool IsItemFocused { get; }

		public bool IsItemClicked(ImGuiMouseButton mouseButton = default);

		public bool IsItemVisible { get; }

		public bool IsItemEdited { get; }

		public bool IsItemActivated { get; }

		public bool IsItemDeactivated { get; }

		public bool IsItemDeactivatedAfterEdit { get; }

		public bool IsItemToggledOpen { get; }

		public bool IsAnyItemHovered { get; }

		public bool IsAnyItemActive { get; }

		public bool IsAnyItemFocused { get; }

		public Vector2 ItemRectMin { get; }

		public Vector2 ItemRectMax { get; }

		public Vector2 ItemRectSize { get; }

		public void SetItemAllowOverlap();

		// Viewports
		// - Currently represents the Platform Window created by the application which is hosting our Dear ImGui windows.
		// - In 'docking' branch with multi-viewport enabled, we extend this concept to have multiple active viewports.
		// - In the future we will extend this concept further to also represent Platform Monitor and support a "no main platform window" operation mode.

		public ImGuiViewport MainViewport { get; set; }

		// Miscellaneous Utilities

		public bool IsRectVisible(Vector2 size);

		public bool IsRectVisible(Vector2 rectMin, Vector2 rectMax);

		public double Time { get; }

		public int FrameCount { get; }

		public IImDrawList BackgroundDrawList { get; }

		public IImDrawList ForegroundDrawList { get; }

		public IImDrawListSharedData DrawListSharedData { get; }

		public string GetStyleColorName(ImGuiCol idx);

		public IImGuiStorage StateStorage { get; set; }

		public bool BeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags = default);

		// Text Utilities

		public Vector2 CalcTextSize(string text, bool hideTextAfterDoubleHash = false, float wrapWidth = -1);

		// Inputs Utilities: Keyboard
		// Without IMGUI_DISABLE_OBSOLETE_KEYIO: (legacy support)
		//   - For 'ImGuiKey key' you can still use your legacy native/user indices according to how your backend/engine stored them in io.KeysDown[].
		// With IMGUI_DISABLE_OBSOLETE_KEYIO: (this is the way forward)
		//   - Any use of 'ImGuiKey' will assert when key < 512 will be passed, previously reserved as native/user keys indices
		//   - GetKeyIndex() is pass-through and therefore deprecated (gone if IMGUI_DISABLE_OBSOLETE_KEYIO is defined)

		public bool IsKeyDown(ImGuiKey key);

		public bool IsKeyPressed(ImGuiKey key, bool repeat = true);

		public bool IsKeyReleased(ImGuiKey key);

		public int GetKeyPressedAmount(ImGuiKey key, float repeatDelay, float rate);

		public string GetKeyName(ImGuiKey key);

		public void CaptureKeyboardFromApp(bool wantCaptureKeyboardValue = true);

		// Inputs Utilities: Mouse
		// - To refer to a mouse button, you may use named enums in your code e.g. ImGuiMouseButton_Left, ImGuiMouseButton_Right.
		// - You can also use regular integer: it is forever guaranteed that 0=Left, 1=Right, 2=Middle.
		// - Dragging operations are only reported after mouse has moved a certain distance away from the initial clicking position (see 'lock_threshold' and 'io.MouseDraggingThreshold')

		public bool IsMouseDown(ImGuiMouseButton button);

		public bool IsMouseClicked(ImGuiMouseButton button, bool repeat = false);

		public bool IsMouseReleased(ImGuiMouseButton button);

		public bool IsMouseDoubleClicked(ImGuiMouseButton button);

		public int GetMouseClickedAmount(ImGuiMouseButton button);

		public bool IsMouseHoveringRect(Vector2 rMin, Vector2 rMax, bool clip = true);

		public bool IsMousePosValid(Vector2? mousePos = null);

		public bool IsAnyMouseDown { get; }

		public Vector2 MousePos { get; }

		public Vector2 MousePosOnOpeningCurrentPopup { get; }

		public bool IsMouseDragging(ImGuiMouseButton button, float lockThreshold = -1);

		public Vector2 GetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left, float lockThreshold = -1);

		public void ResetMouseDragDelta(ImGuiMouseButton button = ImGuiMouseButton.Left);

		public ImGuiMouseCursor MouseCursor { get; set; }

		public void CaptureMouseFromApp(bool wantCaptureMouseValue = true);

		// Clipboard Utilities
		// - Also see the LogToClipboard() function to capture GUI into clipboard, or easily output text data to the clipboard.

		public string ClipboardText { get; set; }

		// Settings/.Ini Utilities
		// - The disk functions are automatically called if io.IniFilename != NULL (default is "imgui.ini").
		// - Set io.IniFilename to NULL to load/save manually. Read io.WantSaveIniSettings description about handling .ini saving manually.
		// - Important: default value "imgui.ini" is relative to current working dir! Most apps will want to lock this to an absolute path (e.g. same path as executables).

		public void LoadIniSettingsFromDisk(string iniFilename);

		public void LoadIniSettingsFromMemory(ReadOnlySpan<byte> iniData);

		public void SaveIniSettingsToDisk(string iniFilename);

		public ReadOnlySpan<byte> SaveIniSettingsToMemory();

	}

}