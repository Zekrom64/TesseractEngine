namespace Tesseract.ImGui {

	public static class ImGui {

		public const string Version = "1.88 WIP";
		public const int VersionNum = 18712;

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


		public static void StyleColorsDark(ImGuiStyle? style = null) {

		}

		public static void StyleColorsLight(ImGuiStyle? style = null) {

		}

		public static void StyleColorsClassic(ImGuiStyle? style = null) {

		}


		public static void Begin(string name, ref bool open)

	}

}