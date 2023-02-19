using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core;
using Tesseract.SDL;
using Tesseract.ImGui;
using System.Diagnostics;
using Tesseract.Core.Numerics;
using System.Numerics;

namespace Tesseract.ImGui.SDL {

	public static class ImGuiSDL2 {

		private static SDLWindow? window = null;
		private static SDLRenderer? renderer = null;
		private static ulong time;
		private static int mouseButtonsDown;
		private static readonly SDLCursor[] mouseCursors = new SDLCursor[(int)ImGuiMouseCursor.Count];
		private static bool mouseCanUseGlobalState;

		private static string GetClipboardText() => SDL2.ClipboardText ?? string.Empty;

		private static void SetClipboardText(string text) => SDL2.ClipboardText = text;

		private static ImGuiKey KeycodeToImGuiKey(SDLKeycode keycode) => keycode switch {
			SDLKeycode.Tab => ImGuiKey.Tab,
			SDLKeycode.Left => ImGuiKey.LeftArrow,
			SDLKeycode.Right => ImGuiKey.RightArrow,
			SDLKeycode.Up => ImGuiKey.UpArrow,
			SDLKeycode.Down => ImGuiKey.DownArrow,
			SDLKeycode.PageUp => ImGuiKey.PageUp,
			SDLKeycode.PageDown => ImGuiKey.PageDown,
			SDLKeycode.Home => ImGuiKey.Home,
			SDLKeycode.End => ImGuiKey.End,
			SDLKeycode.Insert => ImGuiKey.Insert,
			SDLKeycode.Delete => ImGuiKey.Delete,
			SDLKeycode.Backspace => ImGuiKey.Backspace,
			SDLKeycode.Space => ImGuiKey.Space,
			SDLKeycode.Return => ImGuiKey.Enter,
			SDLKeycode.Escape => ImGuiKey.Escape,
			SDLKeycode.Quote => ImGuiKey.Apostrophe,
			SDLKeycode.Comma => ImGuiKey.Comma,
			SDLKeycode.Minus => ImGuiKey.Minus,
			SDLKeycode.Period => ImGuiKey.Period,
			SDLKeycode.Slash => ImGuiKey.Slash,
			SDLKeycode.Semicolon => ImGuiKey.Semicolon,
			SDLKeycode.Equals => ImGuiKey.Equal,
			SDLKeycode.LeftBracket => ImGuiKey.LeftBracket,
			SDLKeycode.Backslash => ImGuiKey.Backslash,
			SDLKeycode.RightBracket => ImGuiKey.RightBracket,
			SDLKeycode.Backquote => ImGuiKey.GraveAccent,
			SDLKeycode.CapsLock => ImGuiKey.CapsLock,
			SDLKeycode.ScrollLock => ImGuiKey.ScrollLock,
			SDLKeycode.NumLockClear => ImGuiKey.NumLock,
			SDLKeycode.PrintScreen => ImGuiKey.PrintScreen,
			SDLKeycode.Pause => ImGuiKey.Pause,
			SDLKeycode.Kp0 => ImGuiKey.Keypad0,
			SDLKeycode.Kp1 => ImGuiKey.Keypad1,
			SDLKeycode.Kp2 => ImGuiKey.Keypad2,
			SDLKeycode.Kp3 => ImGuiKey.Keypad3,
			SDLKeycode.Kp4 => ImGuiKey.Keypad4,
			SDLKeycode.Kp5 => ImGuiKey.Keypad5,
			SDLKeycode.Kp6 => ImGuiKey.Keypad6,
			SDLKeycode.Kp7 => ImGuiKey.Keypad7,
			SDLKeycode.Kp8 => ImGuiKey.Keypad8,
			SDLKeycode.Kp9 => ImGuiKey.Keypad9,
			SDLKeycode.KpPeriod => ImGuiKey.KeypadDecimal,
			SDLKeycode.KpDivide => ImGuiKey.KeypadDivide,
			SDLKeycode.KpMultiply => ImGuiKey.KeypadMultiply,
			SDLKeycode.KpMinus => ImGuiKey.KeypadSubtract,
			SDLKeycode.KpPlus => ImGuiKey.KeypadAdd,
			SDLKeycode.KpEnter => ImGuiKey.KeypadEnter,
			SDLKeycode.KpEquals => ImGuiKey.KeypadEqual,
			SDLKeycode.LCtrl => ImGuiKey.LeftCtrl,
			SDLKeycode.LShift => ImGuiKey.LeftShift,
			SDLKeycode.LAlt => ImGuiKey.LeftAlt,
			SDLKeycode.LGUI => ImGuiKey.LeftSuper,
			SDLKeycode.RCtrl => ImGuiKey.RightCtrl,
			SDLKeycode.RShift => ImGuiKey.RightShift,
			SDLKeycode.RAlt => ImGuiKey.RightAlt,
			SDLKeycode.RGUI => ImGuiKey.RightSuper,
			SDLKeycode.Application => ImGuiKey.Menu,
			SDLKeycode._0 => ImGuiKey._0,
			SDLKeycode._1 => ImGuiKey._1,
			SDLKeycode._2 => ImGuiKey._2,
			SDLKeycode._3 => ImGuiKey._3,
			SDLKeycode._4 => ImGuiKey._4,
			SDLKeycode._5 => ImGuiKey._5,
			SDLKeycode._6 => ImGuiKey._6,
			SDLKeycode._7 => ImGuiKey._7,
			SDLKeycode._8 => ImGuiKey._8,
			SDLKeycode._9 => ImGuiKey._9,
			SDLKeycode.A => ImGuiKey.A,
			SDLKeycode.B => ImGuiKey.B,
			SDLKeycode.C => ImGuiKey.C,
			SDLKeycode.D => ImGuiKey.D,
			SDLKeycode.E => ImGuiKey.E,
			SDLKeycode.F => ImGuiKey.F,
			SDLKeycode.G => ImGuiKey.G,
			SDLKeycode.H => ImGuiKey.H,
			SDLKeycode.I => ImGuiKey.I,
			SDLKeycode.J => ImGuiKey.J,
			SDLKeycode.K => ImGuiKey.K,
			SDLKeycode.L => ImGuiKey.L,
			SDLKeycode.M => ImGuiKey.M,
			SDLKeycode.N => ImGuiKey.N,
			SDLKeycode.O => ImGuiKey.O,
			SDLKeycode.P => ImGuiKey.P,
			SDLKeycode.Q => ImGuiKey.Q,
			SDLKeycode.R => ImGuiKey.R,
			SDLKeycode.S => ImGuiKey.S,
			SDLKeycode.T => ImGuiKey.T,
			SDLKeycode.U => ImGuiKey.U,
			SDLKeycode.V => ImGuiKey.V,
			SDLKeycode.W => ImGuiKey.W,
			SDLKeycode.X => ImGuiKey.X,
			SDLKeycode.Y => ImGuiKey.Y,
			SDLKeycode.Z => ImGuiKey.Z,
			SDLKeycode.F1 => ImGuiKey.F1,
			SDLKeycode.F2 => ImGuiKey.F2,
			SDLKeycode.F3 => ImGuiKey.F3,
			SDLKeycode.F4 => ImGuiKey.F4,
			SDLKeycode.F5 => ImGuiKey.F5,
			SDLKeycode.F6 => ImGuiKey.F6,
			SDLKeycode.F7 => ImGuiKey.F7,
			SDLKeycode.F8 => ImGuiKey.F8,
			SDLKeycode.F9 => ImGuiKey.F9,
			SDLKeycode.F10 => ImGuiKey.F10,
			SDLKeycode.F11 => ImGuiKey.F11,
			SDLKeycode.F12 => ImGuiKey.F12,
			_ => ImGuiKey.None
		};

		private static void UpdateKeyModifiers(SDLKeymod keyMods) {
			IImGuiIO io = GImGui.IO;
			io.AddKeyEvent(ImGuiKey.ModCtrl, (keyMods & SDLKeymod.Ctrl) != 0);
			io.AddKeyEvent(ImGuiKey.ModShift, (keyMods & SDLKeymod.Shift) != 0);
			io.AddKeyEvent(ImGuiKey.ModAlt, (keyMods & SDLKeymod.Alt) != 0);
			io.AddKeyEvent(ImGuiKey.ModSuper, (keyMods & SDLKeymod.GUI) != 0);
		}

		public static bool ProcessEvent(SDLEvent evt) {
			IImGuiIO io = GImGui.IO;

			switch(evt.Type) {
				case SDLEventType.MouseMotion:
					io.AddMousePosEvent(evt.Motion.X, evt.Motion.Y);
					return true;
				case SDLEventType.MouseWheel:
					float wheelX = evt.Wheel.X > 0 ? 1 : evt.Wheel.X < 0 ? -1 : 0;
					float wheelY = evt.Wheel.Y > 0 ? 1 : evt.Wheel.Y < 0 ? -1 : 0;
					io.AddMouseWheelEvent(wheelX, wheelY);
					return true;
				case SDLEventType.MouseButtonDown:
				case SDLEventType.MouseButtonUp:
					ImGuiMouseButton mouseButton = evt.Button.Button switch {
						SDL2.ButtonLeft => ImGuiMouseButton.Left,
						SDL2.ButtonRight => ImGuiMouseButton.Right,
						SDL2.ButtonMiddle => ImGuiMouseButton.Middle,
						_ => (ImGuiMouseButton)(-1)
					};
					if ((int)mouseButton == -1) break;
					io.AddMouseButtonEvent(mouseButton, evt.Type == SDLEventType.MouseButtonDown);
					mouseButtonsDown = (evt.Type == SDLEventType.MouseButtonDown) ? (mouseButtonsDown | (1 << (int)mouseButton)) : (mouseButtonsDown & ~(1 << (int)mouseButton));
					return true;
				case SDLEventType.TextInput:
					io.AddInputCharacters(evt.Text.Text);
					return true;
				case SDLEventType.KeyDown:
				case SDLEventType.KeyUp:
					UpdateKeyModifiers(evt.Key.Keysym.Mod);
					ImGuiKey key = KeycodeToImGuiKey(evt.Key.Keysym.Sym);
					io.AddKeyEvent(key, evt.Type == SDLEventType.KeyDown);
					return true;
				case SDLEventType.WindowEvent:
					switch(evt.Window.Event) {
						case SDLWindowEventID.Leave:
							io.AddMousePosEvent(-float.MaxValue, -float.MaxValue);
							break;
						case SDLWindowEventID.FocusGained:
							io.AddFocusEvent(true);
							break;
						case SDLWindowEventID.FocusLost:
							io.AddFocusEvent(false);
							break;
					}
					return true;
			}

			return false;
		}

		private static readonly string[] globalMouseWhitelist = new string[] { "windows", "cocoa", "x11", "DIVE", "WMAN" };

		public static bool Init(SDLWindow window, SDLRenderer? renderer) {
			IImGuiIO io = GImGui.IO;
			io.BackendPlatformName = "imgui_impl_sdl (Managed)";
			io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos;

			ImGuiSDL2.window = window;
			ImGuiSDL2.renderer = renderer;
			mouseCanUseGlobalState = globalMouseWhitelist.Contains(SDL2.CurrentVideoDriver);

			io.SetClipboardTextFn = SetClipboardText;
			io.GetClipboardTextFn = GetClipboardText;

			mouseCursors[(int)ImGuiMouseCursor.Arrow] = new SDLCursor(SDLSystemCursor.Arrow);
			mouseCursors[(int)ImGuiMouseCursor.TextInput] = new SDLCursor(SDLSystemCursor.IBeam);
			mouseCursors[(int)ImGuiMouseCursor.ResizeAll] = new SDLCursor(SDLSystemCursor.SizeAll);
			mouseCursors[(int)ImGuiMouseCursor.ResizeNS] = new SDLCursor(SDLSystemCursor.SizeNS);
			mouseCursors[(int)ImGuiMouseCursor.ResizeEW] = new SDLCursor(SDLSystemCursor.SizeWE);
			mouseCursors[(int)ImGuiMouseCursor.ResizeNESW] = new SDLCursor(SDLSystemCursor.SizeNESW);
			mouseCursors[(int)ImGuiMouseCursor.ResizeNWSE] = new SDLCursor(SDLSystemCursor.SizeNWSE);
			mouseCursors[(int)ImGuiMouseCursor.Hand] = new SDLCursor(SDLSystemCursor.Hand);
			mouseCursors[(int)ImGuiMouseCursor.NotAllowed] = new SDLCursor(SDLSystemCursor.No);
			
			if (Platform.CurrentPlatformType == PlatformType.Windows) {
				SDLSysWMInfo? info;
				if ((info = window.GetWindowWMInfo(SDL2.Version)) != null) {
					var viewport = GImGui.MainViewport;
					Debug.Assert(info.Info != null);
					viewport.PlatformHandleRaw = ((SDLSysWMInfoWin)info.Info).HWnd;
				}
			}


			return true;
		}

		public static void Shutdown() {
			foreach (SDLCursor cursor in mouseCursors) cursor?.Dispose();
			IImGuiIO io = GImGui.IO;
			io.BackendPlatformName = null;
		}

		private static void UpdateMouseData() {
			Debug.Assert(window != null);
			IImGuiIO io = GImGui.IO;
			SDL2.CaptureMouse = mouseButtonsDown != 0;
			SDLWindow? focusedWindow = SDL2.GetKeyboardFocus();
			bool isAppFocused = window == focusedWindow || (focusedWindow != null && window.Window.Ptr == focusedWindow.Window.Ptr);
			if (isAppFocused) {
				if (io.WantSetMousePos) window.WarpMouseInWindow((int)io.MousePos.X, (int)io.MousePos.Y);
				if (mouseCanUseGlobalState && mouseButtonsDown == 0) {
					SDL2.GetGlobalMouseState(out int mouseXGlobal, out int mouseYGlobal);
					Vector2 pos = (Vector2)(new Vector2i(mouseXGlobal, mouseYGlobal) - window.Position);
					io.AddMousePosEvent(pos.X, pos.Y);
				}
			}
		}

		private static void UpdateMouseCursor() {
			IImGuiIO io = GImGui.IO;
			if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;
			ImGuiMouseCursor imguiCursor = GImGui.MouseCursor;
			if (io.MouseDrawCursor || imguiCursor == ImGuiMouseCursor.None) {
				SDL2.ShowCursor = false;
			} else {
				SDL2.Cursor = mouseCursors[(int)imguiCursor];
				SDL2.ShowCursor = true;
			}
		}

		private static void UpdateGamepads() {
			IImGuiIO io = GImGui.IO;
			if ((io.ConfigFlags & ImGuiConfigFlags.NavEnableGamepad) == 0) return;

			io.BackendFlags &= ~ImGuiBackendFlags.HasGamepad;
			using SDLGameController? gameController = SDLGameController.FromPlayerIndex(0);
			if (gameController == null) return;
			io.BackendFlags |= ImGuiBackendFlags.HasGamepad;

			void MapButton(ImGuiKey key, SDLGameControllerButton button) =>
				io.AddKeyEvent(key, gameController.GetButton(button));

			void MapAnalog(ImGuiKey key, SDLGameControllerAxis axis, float v0, float v1) {
				float vn = (gameController.GetAxis(axis) - v0) / (float)(v1 - v0);
				if (vn > 1) vn = 1;
				else if (vn < 0) vn = 0;
				io.AddKeyAnalogEvent(key, vn > 0.1f, vn);
			}

			const int thumbDeadZone = 8000;

			MapButton(ImGuiKey.GamepadStart, SDLGameControllerButton.Start);
			MapButton(ImGuiKey.GamepadBack, SDLGameControllerButton.Back);
			MapButton(ImGuiKey.GamepadFaceDown, SDLGameControllerButton.A);
			MapButton(ImGuiKey.GamepadFaceRight, SDLGameControllerButton.B);
			MapButton(ImGuiKey.GamepadFaceLeft, SDLGameControllerButton.X);
			MapButton(ImGuiKey.GamepadFaceUp, SDLGameControllerButton.Y);
			MapButton(ImGuiKey.GamepadDpadDown, SDLGameControllerButton.DPadDown);
			MapButton(ImGuiKey.GamepadDpadRight, SDLGameControllerButton.DPadRight);
			MapButton(ImGuiKey.GamepadDpadLeft, SDLGameControllerButton.DPadLeft);
			MapButton(ImGuiKey.GamepadDpadUp, SDLGameControllerButton.DPadUp);
			MapButton(ImGuiKey.GamepadL1, SDLGameControllerButton.LeftShoulder);
			MapButton(ImGuiKey.GamepadR1, SDLGameControllerButton.RightShoulder);
			MapAnalog(ImGuiKey.GamepadL2, SDLGameControllerAxis.TriggerLeft, 0, short.MaxValue);
			MapAnalog(ImGuiKey.GamepadR2, SDLGameControllerAxis.TriggerRight, 0, short.MaxValue);
			MapButton(ImGuiKey.GamepadL3, SDLGameControllerButton.LeftStick);
			MapButton(ImGuiKey.GamepadR3, SDLGameControllerButton.RightStick);
			MapAnalog(ImGuiKey.GamepadLStickLeft, SDLGameControllerAxis.LeftX, -thumbDeadZone, short.MinValue);
			MapAnalog(ImGuiKey.GamepadLStickRight, SDLGameControllerAxis.LeftX, +thumbDeadZone, short.MaxValue);
			MapAnalog(ImGuiKey.GamepadLStickUp, SDLGameControllerAxis.LeftY, -thumbDeadZone, short.MinValue);
			MapAnalog(ImGuiKey.GamepadLStickDown, SDLGameControllerAxis.LeftY, +thumbDeadZone, short.MaxValue);
			MapAnalog(ImGuiKey.GamepadRStickLeft, SDLGameControllerAxis.RightX, -thumbDeadZone, short.MinValue);
			MapAnalog(ImGuiKey.GamepadRStickRight, SDLGameControllerAxis.RightX, +thumbDeadZone, short.MaxValue);
			MapAnalog(ImGuiKey.GamepadRStickUp, SDLGameControllerAxis.RightY, -thumbDeadZone, short.MinValue);
			MapAnalog(ImGuiKey.GamepadRStickDown, SDLGameControllerAxis.RightY, +thumbDeadZone, short.MaxValue);
		}

		private static readonly ulong Frequency = SDL2.PerformanceFrequency;

		public static void NewFrame() {
			Debug.Assert(window != null);
			IImGuiIO io = GImGui.IO;
			Vector2i size = window.Size, displaySize;
			if ((window.Flags & SDLWindowFlags.Minimized) != 0) size = default;
			displaySize = window.DrawableSize;
			io.DisplaySize = (Vector2)size;
			if (size.X > 0 && size.Y > 0)
				io.DisplayFramebufferScale = (Vector2)displaySize / (Vector2)size;

			ulong currentTime = SDL2.PerformanceCounter;
			io.DeltaTime = (float)(time > 0 ? ((double)(currentTime - time) / Frequency) : 1.0 / 60.0);
			time = currentTime;

			UpdateMouseData();
			UpdateMouseCursor();

			UpdateGamepads();
		}

	}

}
