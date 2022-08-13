using System.Numerics;
using Tesseract.Core;
using Tesseract.Core.Native;

namespace Tesseract.ImGui {

	/// <summary>
	/// Communicate most settings and inputs/outputs to Dear ImGui using this structure.
	/// </summary>
	public interface IImGuiIO {

		// Configuration

		/// <summary>
		/// See <see cref="ImGuiConfigFlags"/> enum. Set by user/application. Gamepad/keyboard navigation options, etc.
		/// </summary>
		public ImGuiConfigFlags ConfigFlags { get; set; }
		/// <summary>
		/// See <see cref="ImGuiBackendFlags"/> enum. Set by backend (imgui_impl_xxx files or custom backend) to communicate features supported by the backend.
		/// </summary>
		public ImGuiBackendFlags BackendFlags { get; set; }
		/// <summary>
		/// Main display size, in pixels (generally == <see cref="GImGui.MainViewport"/>.Size). May change every frame.
		/// </summary>
		public Vector2 DisplaySize { get; set; }
		/// <summary>
		/// Time elapsed since last frame, in seconds. May change every frame.
		/// </summary>
		public float DeltaTime { get; set; }
		/// <summary>
		/// Minimum time between saving positions/sizes to .ini file, in seconds.
		/// </summary>
		public float IniSavingRate { get; set; }
		/// <summary>
		/// Path to .ini file (important: default "imgui.ini" is relative to current working dir!). Set null to disable automatic .ini loading/saving or if you want to manually call LoadIniSettingsXXX() / SaveIniSettingsXXX() functions.
		/// </summary>
		public string? IniFilename { get; set; }
		/// <summary>
		/// Path to .log file (default parameter to ImGui::LogToFile when no file is specified).
		/// </summary>
		public string LogFilename { get; set; }
		/// <summary>
		/// Time for a double-click, in seconds.
		/// </summary>
		public float MouseDoubleClickTime { get; set; }
		/// <summary>
		/// Distance threshold to stay in to validate a double-click, in pixels.
		/// </summary>
		public float MouseDoubleClickMaxDist { get; set; }
		/// <summary>
		/// Distance threshold before considering we are dragging.
		/// </summary>
		public float MouseDragThreshold { get; set; }
		/// <summary>
		/// When holding a key/button, time before it starts repeating, in seconds (for buttons in Repeat mode, etc.).
		/// </summary>
		public float KeyRepeatDelay { get; set; }
		/// <summary>
		/// When holding a key/button, rate at which it repeats, in seconds.
		/// </summary>
		public float KeyRepeatRate { get; set; }

		/// <summary>
		/// Font atlas: load, rasterize and pack one or more fonts into a single texture.
		/// </summary>
		public IImFontAtlas Fonts { get; }
		/// <summary>
		/// Global scale all fonts.
		/// </summary>
		public float FontGlobalScale { get; set; }
		/// <summary>
		/// Allow user scaling text of individual window with CTRL+Wheel.
		/// </summary>
		public bool FontAllowUserScaling { get; set; }
		/// <summary>
		/// Font to use on <see cref="GImGui.NewFrame"/>. Use null to uses <see cref="Fonts"/>.Fonts[0].
		/// </summary>
		public IImFont? FontDefault { get; set; }
		/// <summary>
		/// For retina display or other situations where window coordinates are different from framebuffer coordinates. This generally ends up in <see cref="ImDrawData.FramebufferScale"/>.
		/// </summary>
		public Vector2 DisplayFramebufferScale { get; set; }

		/// <summary>
		/// Request ImGui to draw a mouse cursor for you (if you are on a platform without a mouse cursor). Cannot be easily renamed to 'io.ConfigXXX' because this is frequently used by backend implementations.
		/// </summary>
		public bool MouseDrawCursor { get; set; }
		/// <summary>
		/// OS X style: Text editing cursor movement using Alt instead of Ctrl, Shortcuts using Cmd/Super instead of Ctrl, Line/Text Start and End using Cmd+Arrows instead of Home/End, Double click selects by word instead of selecting whole text, Multi-selection in lists uses Cmd/Super instead of Ctrl.
		/// </summary>
		public bool ConfigMacOSXBehaviors { get; set; }
		/// <summary>
		/// Enable input queue trickling: some types of events submitted during the same frame (e.g. button down + up) will be spread over multiple frames, improving interactions with low framerates.
		/// </summary>
		public bool ConfigInputTrickleEventQueue { get; set; }
		/// <summary>
		/// Enable blinking cursor (optional as some users consider it to be distracting).
		/// </summary>
		public bool ConfigInputTextCursorBlink { get; set; }
		/// <summary>
		/// Enable turning DragXXX widgets into text input with a simple mouse click-release (without moving). Not desirable on devices without a keyboard.
		/// </summary>
		public bool ConfigDragClickToInputText { get; set; }
		/// <summary>
		/// Enable resizing of windows from their edges and from the lower-left corner. This requires (<see cref="BackendFlags"/> & <see cref="ImGuiBackendFlags.HasMouseCursors"/>) because it needs mouse cursor feedback. (This used to be a per-window ImGuiWindowFlags_ResizeFromAnySide flag)
		/// </summary>
		public bool ConfigWindowsResizeFromEdges { get; set; }
		/// <summary>
		/// Enable allowing to move windows only when clicking on their title bar. Does not apply to windows without a title bar.
		/// </summary>
		public bool ConfigWindowsMoveFromTitleBarOnly { get; set; }
		/// <summary>
		/// Timer (in seconds) to free transient windows/tables memory buffers when unused. Set to -1.0f to disable.
		/// </summary>
		public float ConfigMemoryCompactTimer { get; set; }

		// Platform Functions

		/// <summary>
		/// Optional: Platform backend name (informational only! will be displayed in About Window).
		/// </summary>
		public string? BackendPlatformName { get; set; }
		/// <summary>
		/// Optional: Renderer backend name (informational only! will be displayed in About Window).
		/// </summary>
		public string? BackendRendererName { get; set; }

		/// <summary>
		/// Optional: Access OS clipboard.
		/// </summary>
		public Func<string>? GetClipboardTextFn { set; }
		/// <summary>
		/// Optional: Access OS clipboard.
		/// </summary>
		public Action<string>? SetClipboardTextFn { set; }

		/// <summary>
		/// Optional: Notify OS Input Method Editor of the screen position of your cursor for text input position (e.g. when using Japanese/Chinese IME on Windows).
		/// </summary>
		public Action<ImGuiViewport, ImGuiPlatformImeData>? SetPlatformImeDataFn { set; }

		// Input

		/// <summary>
		/// Queue a new key down/up event. Key should be "translated" (as in, generally <see cref="ImGuiKey.A"/> matches the key end-user would use to emit an 'A' character).
		/// </summary>
		/// <param name="key">The key for the event</param>
		/// <param name="down">If the key was pressed or released</param>
		public void AddKeyEvent(ImGuiKey key, bool down);

		/// <summary>
		/// Queue a new key down/up event for analog values (e.g. <see cref="ImGuiKey"/>.Gamepad* values). Dead-zones should be handled by the backend.
		/// </summary>
		/// <param name="key">The key for the event</param>
		/// <param name="down">If the key was pressed or released</param>
		/// <param name="v">The analog value of the key in the event</param>
		public void AddKeyAnalogEvent(ImGuiKey key, bool down, float v);

		/// <summary>
		/// Queue a mouse position update. Use -<see cref="float.MaxValue"/>,-<see cref="float.MaxValue"/> to signify no mouse (e.g. app not focused and not hovered)
		/// </summary>
		/// <param name="x">The X mouse position</param>
		/// <param name="y">The Y mouse position</param>
		public void AddMousePosEvent(float x, float y);

		/// <summary>
		/// Queue a mouse button change.
		/// </summary>
		/// <param name="button">Which mouse button changed</param>
		/// <param name="down">If the button was pressed or released</param>
		public void AddMouseButtonEvent(int button, bool down);

		/// <summary>
		/// Queue a mouse wheel update
		/// </summary>
		/// <param name="x">Mouse wheel X change</param>
		/// <param name="y">Mouse wheel Y change</param>
		public void AddMouseWheelEvent(float x, float y);

		/// <summary>
		/// Queue a gain/loss of focus for the application (generally based on OS/platform focus of your window)
		/// </summary>
		/// <param name="focused">If the application was focused or unfocused</param>
		public void AddFocusEvent(bool focused);

		/// <summary>
		/// Queue a new character input.
		/// </summary>
		/// <param name="c">The Unicode codepoint to queue</param>
		public void AddInputCharacter(int c);

		/// <summary>
		/// Queue a new character input from an UTF-16 character, it can be a surrogate.
		/// </summary>
		/// <param name="c">The UTF-16 character to queue</param>
		public void AddInputCharacterUTF16(char c);

		/// <summary>
		/// Queue a new characters input from an UTF-8 string.
		/// </summary>
		/// <param name="str">The UTF-8 string to queue</param>
		public void AddInputCharactersUTF8(ReadOnlySpan<byte> str);

		/// <summary>
		/// Queue a new characters input from a managed string.
		/// </summary>
		/// <param name="str">The string to queue</param>
		public void AddInputCharacters(string str);


		// Output

		/// <summary>
		/// Set when Dear ImGui will use mouse inputs, in this case do not dispatch them to your main game/application (either way, always pass on mouse inputs to imgui). (e.g. unclicked mouse is hovering over an imgui window, widget is active, mouse was clicked over an imgui window, etc.).
		/// </summary>
		public bool WantCaptureMouse { get; }
		/// <summary>
		/// Set when Dear ImGui will use keyboard inputs, in this case do not dispatch them to your main game/application (either way, always pass keyboard inputs to imgui). (e.g. InputText active, or an imgui window is focused and navigation is enabled, etc.).
		/// </summary>
		public bool WantCaptureKeyboard { get; }
		/// <summary>
		/// Mobile/console: when set, you may display an on-screen keyboard. This is set by Dear ImGui when it wants textual keyboard input to happen (e.g. when a InputText widget is active).
		/// </summary>
		public bool WantTextInput { get; }
		/// <summary>
		/// MousePos has been altered, backend should reposition mouse on next frame. Rarely used! Set only when <see cref="ImGuiConfigFlags.NavEnableSetMousePos"/> flag is enabled.
		/// </summary>
		public bool WantSetMousePos { get; }
		/// <summary>
		/// When manual .ini load/save is active (<see cref="IniFilename"/> == null), this will be set to notify your application that you can call <see cref="GImGui.SaveIniSettingsToMemory"/> and save yourself. Important: clear <see cref="WantSaveIniSettings"/> yourself after saving!
		/// </summary>
		public bool WantSaveIniSettings { get; set; }
		/// <summary>
		/// Keyboard/Gamepad navigation is currently allowed (will handle <see cref="ImGuiKey"/>.NavXXX events) = a window is focused and it doesn't use the ImGuiWindowFlags_NoNavInputs flag.
		/// </summary>
		public bool NavActive { get; }
		/// <summary>
		/// Keyboard/Gamepad navigation is visible and allowed (will handle <see cref="ImGuiKey"/>.NavXXX events).
		/// </summary>
		public bool NavVisible { get; }
		/// <summary>
		/// Rough estimate of application framerate, in frame per second. Solely for convenience. Rolling average estimation based on <see cref="DeltaTime"/> over 120 frames.
		/// </summary>
		public float Framerate { get; }
		/// <summary>
		/// Vertices output during last call to <see cref="GImGui.Render"/>.
		/// </summary>
		public int MetricsRenderVertices { get; }
		/// <summary>
		/// Indices output during last call to <see cref="GImGui.Render"/> = number of triangles * 3.
		/// </summary>
		public int MetricsRenderIndices { get; }
		/// <summary>
		/// Number of visible windows.
		/// </summary>
		public int MetricsRenderWindows { get; }
		/// <summary>
		/// Number of active windows.
		/// </summary>
		public int MetricsActiveWindows { get; }
		/// <summary>
		/// Number of active allocations, updated by MemAlloc/MemFree based on current context. May be off if you have multiple imgui contexts.
		/// </summary>
		public int MetricsActiveAllocations { get; }
		/// <summary>
		/// Mouse delta. Note that this is zero if either current or previous position are invalid (-<see cref="float.MaxValue"/>,-<see cref="float.MaxValue"/>), so a disappearing/reappearing mouse won't have a huge delta.
		/// </summary>
		public Vector2 MouseDelta { get; }

		// [Internal]

		/// <summary>
		/// Mouse position, in pixels. Set to ImVec2(-FLT_MAX, -FLT_MAX) if mouse is unavailable (on another screen, etc.)
		/// </summary>
		public Vector2 MousePos { get; }

	}

}
