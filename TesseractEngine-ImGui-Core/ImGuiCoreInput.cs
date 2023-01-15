using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Input;
using Tesseract.Core.Numerics;
using Tesseract.Core.Services;

namespace Tesseract.ImGui.Core {

	public static class ImGuiCoreInput {

		private static IInputSystem? inputSystem;
		private static IWindow? window;
		private static readonly Stopwatch stopwatch = new();
		private static long lastTicks;

		private static readonly ICursor?[] mouseCursors = new ICursor[(int)ImGuiMouseCursor.Count];

		private static ImGuiKey Convert(Key key) => key switch {
			Key.Tab => ImGuiKey.Tab,
			Key.Left => ImGuiKey.LeftArrow,
			Key.Right => ImGuiKey.RightArrow,
			Key.Up => ImGuiKey.UpArrow,
			Key.Down => ImGuiKey.DownArrow,
			Key.PageUp => ImGuiKey.PageUp,
			Key.PageDown => ImGuiKey.PageDown,
			Key.Home => ImGuiKey.Home,
			Key.End => ImGuiKey.End,
			Key.Insert => ImGuiKey.Insert,
			Key.Delete => ImGuiKey.Delete,
			Key.Backspace => ImGuiKey.Backspace,
			Key.Space => ImGuiKey.Space,
			Key.Enter => ImGuiKey.Enter,
			Key.Escape => ImGuiKey.Escape,
			Key.Quote => ImGuiKey.Apostrophe,
			Key.Comma => ImGuiKey.Comma,
			Key.Minus => ImGuiKey.Minus,
			Key.Period => ImGuiKey.Period,
			Key.Slash => ImGuiKey.Slash,
			Key.Semicolon => ImGuiKey.Semicolon,
			Key.Equals => ImGuiKey.Equal,
			Key.LBracket => ImGuiKey.LeftBracket,
			Key.Backslash => ImGuiKey.Backslash,
			Key.RBracket => ImGuiKey.RightBracket,
			Key.Grave => ImGuiKey.GraveAccent,
			Key.PrintScreen => ImGuiKey.PrintScreen,
			Key.Pause => ImGuiKey.Pause,
			Key.Numpad0 => ImGuiKey.Keypad0,
			Key.Numpad1 => ImGuiKey.Keypad1,
			Key.Numpad2 => ImGuiKey.Keypad2,
			Key.Numpad3 => ImGuiKey.Keypad3,
			Key.Numpad4 => ImGuiKey.Keypad4,
			Key.Numpad5 => ImGuiKey.Keypad5,
			Key.Numpad6 => ImGuiKey.Keypad6,
			Key.Numpad7 => ImGuiKey.Keypad7,
			Key.Numpad8 => ImGuiKey.Keypad8,
			Key.Numpad9 => ImGuiKey.Keypad9,
			Key.NumpadDecimal => ImGuiKey.KeypadDecimal,
			Key.NumpadDivide => ImGuiKey.KeypadDivide,
			Key.NumpadMultiply => ImGuiKey.KeypadMultiply,
			Key.NumpadMinus => ImGuiKey.KeypadSubtract,
			Key.NumpadAdd => ImGuiKey.KeypadAdd,
			Key.NumpadEnter => ImGuiKey.KeypadEnter,
			Key.LCtrl => ImGuiKey.LeftCtrl,
			Key.LShift => ImGuiKey.LeftShift,
			Key.LAlt => ImGuiKey.LeftAlt,
			Key.RCtrl => ImGuiKey.RightCtrl,
			Key.RShift => ImGuiKey.RightShift,
			Key.RAlt => ImGuiKey.RightAlt,
			Key._0 => ImGuiKey._0,
			Key._1 => ImGuiKey._1,
			Key._2 => ImGuiKey._2,
			Key._3 => ImGuiKey._3,
			Key._4 => ImGuiKey._4,
			Key._5 => ImGuiKey._5,
			Key._6 => ImGuiKey._6,
			Key._7 => ImGuiKey._7,
			Key._8 => ImGuiKey._8,
			Key._9 => ImGuiKey._9,
			Key.A => ImGuiKey.A,
			Key.B => ImGuiKey.B,
			Key.C => ImGuiKey.C,
			Key.D => ImGuiKey.D,
			Key.E => ImGuiKey.E,
			Key.F => ImGuiKey.F,
			Key.G => ImGuiKey.G,
			Key.H => ImGuiKey.H,
			Key.I => ImGuiKey.I,
			Key.J => ImGuiKey.J,
			Key.K => ImGuiKey.K,
			Key.L => ImGuiKey.L,
			Key.M => ImGuiKey.M,
			Key.N => ImGuiKey.N,
			Key.O => ImGuiKey.O,
			Key.P => ImGuiKey.P,
			Key.Q => ImGuiKey.Q,
			Key.R => ImGuiKey.R,
			Key.S => ImGuiKey.S,
			Key.T => ImGuiKey.T,
			Key.U => ImGuiKey.U,
			Key.V => ImGuiKey.V,
			Key.W => ImGuiKey.W,
			Key.X => ImGuiKey.X,
			Key.Y => ImGuiKey.Y,
			Key.Z => ImGuiKey.Z,
			Key.F1 => ImGuiKey.F1,
			Key.F2 => ImGuiKey.F2,
			Key.F3 => ImGuiKey.F3,
			Key.F4 => ImGuiKey.F4,
			Key.F5 => ImGuiKey.F5,
			Key.F6 => ImGuiKey.F6,
			Key.F7 => ImGuiKey.F7,
			Key.F8 => ImGuiKey.F8,
			Key.F9 => ImGuiKey.F9,
			Key.F10 => ImGuiKey.F10,
			Key.F11 => ImGuiKey.F11,
			Key.F12 => ImGuiKey.F12,
			_ => ImGuiKey.None
		};

		public static void Init(IInputSystem inputSystem, IWindow window, IWindowSystem windowSystem) {
			IImGuiIO io = GImGui.IO;
			io.BackendPlatformName = "Tesseract Core Input";

			ImGuiCoreInput.inputSystem = inputSystem;
			ImGuiCoreInput.window = window;

			var clipboard = window.GetService(InputServices.Clipboard) ?? inputSystem.GetService(InputServices.Clipboard);
			if (clipboard != null) {
				io.SetClipboardTextFn = str => clipboard.ClipboardText = str;
				io.GetClipboardTextFn = () => clipboard.ClipboardText ?? string.Empty;
			}

			if (windowSystem.CustomCursorSupport) {
				io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;

				mouseCursors[(int)ImGuiMouseCursor.Arrow] = windowSystem.CreateStandardCursor(StandardCursor.Arrow);
				mouseCursors[(int)ImGuiMouseCursor.TextInput] = windowSystem.CreateStandardCursor(StandardCursor.IBeam);
				mouseCursors[(int)ImGuiMouseCursor.ResizeEW] = windowSystem.CreateStandardCursor(StandardCursor.HResize);
				mouseCursors[(int)ImGuiMouseCursor.ResizeNS] = windowSystem.CreateStandardCursor(StandardCursor.VResize);
				mouseCursors[(int)ImGuiMouseCursor.Hand] = windowSystem.CreateStandardCursor(StandardCursor.Hand);
			}

			window.OnKey += OnKey;
			window.OnMouseMove += OnMouseMoved;
			window.OnMouseButton += OnMouseButton;
			window.OnMouseWheel += OnMouseWheel;
			window.OnFocused += OnFocused;
			window.OnUnfocused += OnUnfocused;

			stopwatch.Start();
		}

		private static void UpdateModifiers(KeyMod mod) {
			var io = GImGui.IO;
			io.AddKeyEvent(ImGuiKey.ModShift, (mod & KeyMod.Shift) != 0);
			io.AddKeyEvent(ImGuiKey.ModCtrl, (mod & KeyMod.Ctrl) != 0);
			io.AddKeyEvent(ImGuiKey.ModAlt, (mod & KeyMod.Alt) != 0);
		}

		private static void OnKey(KeyEvent key) {
			var imkey = Convert(key.Key);
			UpdateModifiers(key.Mod);
			if (imkey != ImGuiKey.None) GImGui.IO.AddKeyEvent(imkey, key.State);
		}



		private static void OnMouseMoved(MouseMoveEvent evt) => GImGui.IO.AddMousePosEvent(evt.Position.X, evt.Position.Y);

		private static void OnMouseButton(MouseButtonEvent evt) {
			Debug.Assert(window != null);
			UpdateModifiers(evt.Mod);

			int button = evt.Button switch {
				IMouse.LeftButton => 0,
				IMouse.RightButton => 1,
				IMouse.MiddleButton => 2,
				_ => -1
			};
			if (button != -1) GImGui.IO.AddMouseButtonEvent(button, evt.State);
		}

		private static void OnMouseWheel(MouseWheelEvent evt) => GImGui.IO.AddMouseWheelEvent(evt.Delta.X, evt.Delta.Y);

		private static void OnFocused() => GImGui.IO.AddFocusEvent(true);

		private static void OnUnfocused() => GImGui.IO.AddFocusEvent(false);

		public static void Shutdown() {
			if (window != null) {
				window.OnKey -= OnKey;
				window.OnMouseMove -= OnMouseMoved;
				window.OnMouseButton -= OnMouseButton;
				window.OnMouseWheel -= OnMouseWheel;
				window.OnFocused -= OnFocused;
				window.OnUnfocused -= OnUnfocused;
			}
		}

		private static readonly float tickScale = 1.0f / Stopwatch.Frequency;

		public static void NewFrame() {
			Debug.Assert(window != null);
			Debug.Assert(inputSystem != null);
			var io = GImGui.IO;

			// Poll input system for events
			inputSystem.RunEvents();

			// Update display size and scale
			Vector2i size = window.Size, displaySize = window.DrawableSize;
			io.DisplaySize = size;
			io.DisplayFramebufferScale = (Vector2)displaySize / (Vector2)size;

			// Update time delta
			long current = stopwatch.ElapsedTicks;
			long delta = current - lastTicks;
			lastTicks = current;
			io.DeltaTime = delta * tickScale;

			// Update the mouse cursor
			if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0) {
				ImGuiMouseCursor imguiCursor = GImGui.MouseCursor;
				if (io.MouseDrawCursor || imguiCursor == ImGuiMouseCursor.None) {
					window.CursorMode = CursorMode.Hidden;
				} else {
					window.SetCursor(mouseCursors[(int)imguiCursor]);
					window.CursorMode = CursorMode.Default;
				}
			}

			if ((io.ConfigFlags & ImGuiConfigFlags.NavEnableGamepad) != 0) {
				io.BackendFlags &= ~ImGuiBackendFlags.HasGamepad;
				if (inputSystem.Gamepads.Count > 0) {
					var gamepad = inputSystem.Gamepads.Where(g => g.PlayerIndex == 0).First();
					io.BackendFlags |= ImGuiBackendFlags.HasGamepad;

					void MapButton(ImGuiKey key, GamepadControl control) {
						int controlIndex = gamepad.GetGamepadControlIndex(control);
						if (controlIndex != -1) io.AddKeyEvent(key, gamepad.Buttons[controlIndex]);
					}

					MapButton(ImGuiKey.GamepadStart, GamepadControl.Start);
					MapButton(ImGuiKey.GamepadBack, GamepadControl.Back);
					MapButton(ImGuiKey.GamepadFaceDown, GamepadControl.A);
					MapButton(ImGuiKey.GamepadFaceRight, GamepadControl.B);
					MapButton(ImGuiKey.GamepadFaceLeft, GamepadControl.X);
					MapButton(ImGuiKey.GamepadFaceUp, GamepadControl.Y);
					MapButton(ImGuiKey.GamepadDpadDown, GamepadControl.DPadDown);
					MapButton(ImGuiKey.GamepadDpadRight, GamepadControl.DPadRight);
					MapButton(ImGuiKey.GamepadDpadLeft, GamepadControl.DPadLeft);
					MapButton(ImGuiKey.GamepadDpadUp, GamepadControl.DPadUp);
					MapButton(ImGuiKey.GamepadL1, GamepadControl.LeftBumper);
					MapButton(ImGuiKey.GamepadR1, GamepadControl.RightBumper);
					MapButton(ImGuiKey.GamepadL2, GamepadControl.LeftTrigger);
					MapButton(ImGuiKey.GamepadR2, GamepadControl.RightTrigger);
					MapButton(ImGuiKey.GamepadL3, GamepadControl.LeftThumbStick);
					MapButton(ImGuiKey.GamepadR3, GamepadControl.RightThumbStick);

					void MapAnalog(ImGuiKey key, GamepadControl control, float v0, float v1) {
						int controlIndex = gamepad.GetGamepadControlIndex(control);
						if (controlIndex != -1) {
							float vn = (gamepad.Axes[controlIndex] - v0) / (v1 - v0);
							vn = Math.Clamp(vn, 0, 1);
							io.AddKeyAnalogEvent(key, vn > 0.1f, vn);
						}
					}

					const float DeadZone = 0.2f;
					MapAnalog(ImGuiKey.GamepadLStickLeft, GamepadControl.LeftX, -DeadZone, -1);
					MapAnalog(ImGuiKey.GamepadLStickRight, GamepadControl.LeftX, DeadZone, 1);
					MapAnalog(ImGuiKey.GamepadLStickUp, GamepadControl.LeftY, -DeadZone, -1);
					MapAnalog(ImGuiKey.GamepadLStickDown, GamepadControl.LeftY, DeadZone, 1);
					MapAnalog(ImGuiKey.GamepadRStickLeft, GamepadControl.RightX, -DeadZone, -1);
					MapAnalog(ImGuiKey.GamepadRStickRight, GamepadControl.RightX, DeadZone, 1);
					MapAnalog(ImGuiKey.GamepadRStickUp, GamepadControl.RightY, -DeadZone, -1);
					MapAnalog(ImGuiKey.GamepadRStickDown, GamepadControl.RightY, DeadZone, 1);
				}
			}
		}

	}

}
