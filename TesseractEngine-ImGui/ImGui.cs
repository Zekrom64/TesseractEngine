using System.Numerics;
using System.Text;
using Tesseract.Core.Numerics;

namespace Tesseract.ImGui {

	public interface IImGui {

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

		public IImDrawData GetWindowDrawList();

		public Vector2 WindowPos { get; }

		public Vector2 WindowSize { get; }

		public float WindowWidth { get; }

		public float WindowHeight { get; }

		// Window manipulation
		// - Prefer using SetNextXXX functions (before Begin) rather that SetXXX functions (after Begin).

		public void SetNextWindowPos(Vector2 pos, ImGuiCond cond = default, Vector2 pivot = default);

		public void SetNextWindowSize(Vector2 size, ImGuiCond cond = default);

		public void SetNextWindowSizeConstraints(Vector2 sizeMin, Vector2 sizeMax, ImGuiSizeCallback? customCallback = null, IntPtr customCallbackData = default);

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

	}

}