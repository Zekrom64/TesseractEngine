﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Input;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Services;
using System.Runtime.InteropServices;

namespace Tesseract.GLFW.Services {

	public class GLFWServiceCursor : ICursor {

		public readonly GLFWCursor Cursor;

		public GLFWServiceCursor(GLFWCursor cursor) {
			Cursor = cursor;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Cursor.Dispose();
		}
	}

	public class GLFWServiceDisplayMode : IDisplayMode {

		public readonly GLFWVidMode VidMode;

		public GLFWServiceDisplayMode(GLFWVidMode vidmode) {
			VidMode = vidmode;
		}

		// Not really a "pixel format" provided by GLFW, but instead guess from the provided bit depths
		public PixelFormat PixelFormat {
			get {
				// Most common format is 8-bit RGB(A), don't bother converting if we know this
				if (VidMode.RedBits == 8 && VidMode.GreenBits == 8 && VidMode.BlueBits == 8) return PixelFormat.R8G8B8UNorm;
				// Else create a packed format that holds all of the bits
				return PixelFormat.DefinePackedFormat(
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = 0, Size = VidMode.RedBits, Type = ChannelType.Red },
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = VidMode.RedBits, Size = VidMode.GreenBits, Type = ChannelType.Green },
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = VidMode.RedBits + VidMode.GreenBits, Size = VidMode.BlueBits, Type = ChannelType.Blue }
				);
			}
		}

		public IReadOnlyTuple2<int> Size => new Vector2i(VidMode.Width, VidMode.Height);

		public int RefreshRate => VidMode.RefreshRate;

	}

	public class GLFWServiceGammaRamp : IGammaRamp {

		private readonly ushort[] red, green, blue;

		public int Length { get; }

		public ushort[] Red {
			get => red; 
			set => value.CopyTo(red.AsSpan());
		}

		public ushort[] Green {
			get => green;
			set => value.CopyTo(green.AsSpan());
		}

		public ushort[] Blue {
			get => blue;
			set => value.CopyTo(blue.AsSpan());
		}

		public Vector3us this[int index] {
			get => new(red[index], green[index], blue[index]);
			set {
				red[index] = value.X;
				green[index] = value.Y;
				blue[index] = value.Z;
			}
		}

		public GLFWServiceGammaRamp(int count) {
			Length = count;
			red = new ushort[count];
			green = new ushort[count];
			blue = new ushort[count];
		}

	}

	public class GLFWServiceDisplay : IDisplay, IGammaRampObject {

		public GLFWMonitor Monitor;

		public GLFWServiceDisplay(GLFWMonitor monitor) {
			Monitor = monitor;
		}

		public IReadOnlyTuple2<int> Position => Monitor.Pos;

		public string Name => Monitor.Name!;

		public IDisplayMode CurrentMode => new GLFWServiceDisplayMode(Monitor.VideoMode);

		public IGammaRamp GammaRamp {
			get {
				var ramp = Monitor.GammaRamp;
				GLFWServiceGammaRamp rampout = new((int)ramp.Size);
				unsafe {
					new Span<ushort>((void*)ramp.Red, rampout.Length).CopyTo(rampout.Red);
					new Span<ushort>((void*)ramp.Green, rampout.Length).CopyTo(rampout.Green);
					new Span<ushort>((void*)ramp.Blue, rampout.Length).CopyTo(rampout.Blue);
				}
				return rampout;
			}
			set {
				GLFWGammaRamp ramp = new() {
					Size = (uint)value.Length
				};
				unsafe {
					fixed(ushort* pRed = value.Red) {
						fixed(ushort* pGreen = value.Green) {
							fixed(ushort* pBlue = value.Blue) {
								ramp.Red = (IntPtr)pRed;
								ramp.Green = (IntPtr)pGreen;
								ramp.Blue = (IntPtr)pBlue;
								Monitor.GammaRamp = ramp;
							}
						}
					}
				}
			}
		}

		public IGammaRamp CreateCompatibleGammaRamp() => new GLFWServiceGammaRamp((int)Monitor.GammaRamp.Size);

		public IDisplayMode[] GetDisplayModes() {
			ReadOnlySpan<GLFWVidMode> glfwmodes = Monitor.VideoModes;
			IDisplayMode[] modes = new IDisplayMode[glfwmodes.Length];
			for (int i = 0; i < modes.Length; i++) modes[i] = new GLFWServiceDisplayMode(glfwmodes[i]);
			return modes;
		}

		public T? GetService<T>(IService<T> service) where T : notnull {
			if (service == GraphicsServices.GammaRampObject)
				return (T)(object)this;
			else return ServiceInjector.Lookup(this, service);
		}

	}

	public class GLFWServiceWindow : IWindow, IClipboard {

		public readonly GLFWWindow Window;

		public static event Action<WindowAttributeList>? OnParseAttributes;

		public GLFWServiceWindow(string title, int w, int h, WindowAttributeList? attribs) {
			this.title = title;
			bool ignoreScaling = false;
			if (attribs != null) {
				if (attribs.TryGet(WindowAttributes.Focused, out bool focused)) GLFW3.WindowHint(GLFWWindowAttrib.Focused, focused ? 1 : 0);
				if (attribs.TryGet(WindowAttributes.Maximized, out bool maximized)) GLFW3.WindowHint(GLFWWindowAttrib.Maximized, maximized ? 1 : 0);
				if (attribs.TryGet(WindowAttributes.Minimized, out bool minimized)) GLFW3.WindowHint(GLFWWindowAttrib.Iconified, minimized ? 1 : 0);
				if (attribs.TryGet(WindowAttributes.Resizable, out bool resizable)) GLFW3.WindowHint(GLFWWindowAttrib.Resizable, resizable ? 1 : 0);
				if (attribs.TryGet(WindowAttributes.Visible, out bool visible)) GLFW3.WindowHint(GLFWWindowAttrib.Visible, visible ? 1 : 0);
				if (attribs.TryGet(WindowAttributes.NoScaling, out bool noScaling)) ignoreScaling = noScaling;
				OnParseAttributes?.Invoke(attribs);
			}
			if (!ignoreScaling) GLFW3.WindowHint(GLFWWindowAttrib.ScaleToMonitor, 1);
			Window = new(new Vector2i(w, h), title);
			if (attribs != null) {
				if (attribs.TryGet(WindowAttributes.Closing, out bool closing)) Window.ShouldClose = closing;
				if (attribs.TryGet(WindowAttributes.Opacity, out float opacity)) Window.Opacity = opacity;
				if (attribs.TryGet(WindowAttributes.Position, out Vector2i pos)) Window.Pos = pos;
			}
			Window.UserPointer = new ObjectPointer<GLFWServiceWindow>(this).Ptr;
			Window.SizeCallback = (IntPtr pWindow, int w, int h) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (!window.Minimized) window.OnResize?.Invoke(new Vector2i(w, h));
				}
			};
			Window.PosCallback = (IntPtr pWindow, int x, int y) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (!window.Minimized) window.OnMove?.Invoke(new Vector2i(x, y));
				}
			};
			Window.IconifyCallback = (IntPtr pWindow, bool iconified) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (iconified) window.OnMinimized?.Invoke();
					else window.OnRestored?.Invoke();
				}
			};
			Window.MaximizeCallback = (IntPtr pWindow, bool maximized) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (maximized) window.OnMaximized?.Invoke();
				}
			};
			Window.FocusCallback = (IntPtr pWindow, bool focused) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (focused) window.OnFocused?.Invoke();
					else window.OnUnfocused?.Invoke();
				}
			};
			Window.CloseCallback = (IntPtr pWindow) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					window.OnClosing?.Invoke();
				}
			};
			Window.KeyCallback = (IntPtr pWindow, GLFWKey key, int scancode, GLFWButtonState state, GLFWKeyMod mods) => {
				if (GLFWServiceKeyboard.GLFWToStdKey.TryGetValue(key, out Key stdkey)) {
					KeyEvent evt = new() {
						Key = stdkey,
						State = state != GLFWButtonState.Release,
						Repeat = state == GLFWButtonState.Repeat,
						Mod = GLFWServiceKeyboard.GLFWToStdKeyMod(mods)
					};
					unsafe {
						GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
						window.OnKey?.Invoke(evt);
					}
					GLFWServiceInputSystem.FireKeyEvent(evt);
				}
			};
			Window.CharCallback = (IntPtr pWindow, uint codepoint) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (!window.textInput) return;
					string text = char.ConvertFromUtf32((int)codepoint);
					TextInputEvent evt = new() {
						Text = text
					};
					window.OnTextInput?.Invoke(evt);
					GLFWServiceInputSystem.FireTextInputEvent(evt);
				}
			};
			Window.CursorEnterCallback = (IntPtr pWindow, bool entered) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					if (entered) {
						Vector2d pos = window.Window.CursorPos;
						window.lastCursorPos.X = (int)pos.X;
						window.lastCursorPos.Y = (int)pos.Y;
					}
				}
			};
			Window.CursorPosCallback = (IntPtr pWindow, double x, double y) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					Vector2i pos = new((int)x, (int)y);
					MouseMoveEvent evt = new() {
						Position = pos,
						Delta = pos - window.lastCursorPos
					};
					window.lastCursorPos = pos;
					window.OnMouseMove?.Invoke(evt);
					GLFWServiceInputSystem.FireMouseMoveEvent(evt with { Position = evt.Position + window.Position });
				}
			};
			Window.MouseButtonCallback = (IntPtr pWindow, GLFWMouseButton button, GLFWButtonState state, GLFWKeyMod mods) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					Vector2d pos = window.Window.CursorPos;
					MouseButtonEvent evt = new() {
						Position = new Vector2i((int)pos.X, (int)pos.Y),
						State = state != GLFWButtonState.Release,
						Button = GLFWServiceMouse.GLFWToStdButton(button),
						Mod = GLFWServiceKeyboard.GLFWToStdKeyMod(mods)
					};
					window.OnMouseButton?.Invoke(evt);
					GLFWServiceInputSystem.FireMouseButtonEvent(evt with { Position = evt.Position + window.Position });
				}
			};
			Window.ScrollCallback = (IntPtr pWindow, double x, double y) => {
				unsafe {
					GLFWServiceWindow window = new ObjectPointer<GLFWServiceWindow>(GLFW3.Functions.glfwGetWindowUserPointer(pWindow)).Value!;
					Vector2d pos = window.Window.CursorPos;
					MouseWheelEvent evt = new() {
						Position = new Vector2i((int)pos.X, (int)pos.Y),
						Delta = new Vector2i((int)x, (int)y)
					};
					window.OnMouseWheel?.Invoke(evt);
					GLFWServiceInputSystem.FireMouseWheelEvent(evt with { Position = evt.Position + window.Position });
				}
			};
		}

		private string title;
		public string Title {
			get => title;
			set => Window.Title = title = value;
		}

		public Vector2i Size { get => Window.Size; set => Window.Size = value; }

		public Vector2i Position { get => Window.Pos; set => Window.Pos = value; }

		public bool Minimized {
			get => Window.GetAttrib(GLFWWindowAttrib.Iconified) != 0;
			set {
				if (value) Window.Iconify();
				else Window.Restore();
			}
		}

		public bool Maximized {
			get => Window.GetAttrib(GLFWWindowAttrib.Maximized) != 0;
			set {
				if (value) Window.Maximize();
				else Window.Restore();
			}
		}

		public bool Visible {
			get => Window.GetAttrib(GLFWWindowAttrib.Visible) != 0;
			set {
				if (value) Window.Show();
				else Window.Hide();
			}
		}

		public bool Focused {
			get => Window.GetAttrib(GLFWWindowAttrib.Focused) != 0;
			set {
				if (value) Window.Focus();
			}
		}

		public bool Closing { get => Window.ShouldClose; set => Window.ShouldClose = value; }

		public float Opacity { get => Window.Opacity; set => Window.Opacity = value; }

		public bool Resizable { get => Window.GetAttrib(GLFWWindowAttrib.Resizable) != 0; set => Window.SetAttrib(GLFWWindowAttrib.Resizable, value ? 1 : 0); }

		public bool Fullscreen => Window.Monitor;

		public IDisplay? FullscreenDisplay {
			get {
				GLFWMonitor monitor = Window.Monitor;
				if (!monitor) return null;
				else return new GLFWServiceDisplay(monitor);
			}
		}

		public IWindowSurface? Surface => null;

		public Vector2i MousePosition {
			get {
				Vector2d pos = Window.CursorPos;
				return new((int)pos.X, (int)pos.Y);
			}
		}

		public string? ClipboardText {
			get => Window.Clipboard;
			set => Window.Clipboard = value;
		}

		public Vector2i DrawableSize => Window.FramebufferSize;

		public CursorMode CursorMode {
			get {
				return Window.GetInputMode<GLFWCursorMode>(GLFWInputMode.Cursor) switch {
					GLFWCursorMode.Hidden => CursorMode.Hidden,
					GLFWCursorMode.Disabled => CursorMode.Locked,
					_ => CursorMode.Default
				};
			}
			set => Window.SetInputMode(GLFWInputMode.Cursor, value switch {
				CursorMode.Hidden => GLFWCursorMode.Hidden,
				CursorMode.Locked => GLFWCursorMode.Disabled,
				_ => GLFWCursorMode.Normal,
			});
		}

		public event Action<Vector2i>? OnResize;
		public event Action<Vector2i>? OnMove;
		public event Action? OnMinimized;
		public event Action? OnMaximized;
		public event Action? OnRestored;
		public event Action? OnFocused;
		public event Action? OnUnfocused;
		public event Action? OnClosing;
		public event Action<KeyEvent>? OnKey;
		public event Action<TextInputEvent>? OnTextInput;
#pragma warning disable 0067
		public event Action<TextEditEvent>? OnTextEdit;
#pragma warning restore 0067
		private Vector2i lastCursorPos;
		public event Action<MouseMoveEvent>? OnMouseMove;
		public event Action<MouseButtonEvent>? OnMouseButton;
		public event Action<MouseWheelEvent>? OnMouseWheel;

		public void Dispose() {
			GC.SuppressFinalize(this);
			new ObjectPointer<GLFWServiceWindow>(Window.UserPointer).Dispose();
			Window.Dispose();
		}

		private bool textInput = false;

		public void EndTextInput() => textInput = false;

		public bool GetKeyState(Key key) => GLFWServiceKeyboard.StdToGLFWKey.TryGetValue(key, out GLFWKey glfwkey) && Window.GetKey(glfwkey) != GLFWButtonState.Release;

		public bool GetMouseButtonState(int button) => Window.GetMouseButton(GLFWServiceMouse.StdToGLFWButton(button)) != GLFWButtonState.Release;

		public void Restore() {
			if (Fullscreen) SetFullscreen(null, null);
			else Window.Restore();
		}

		public void SetCursor(ICursor? cursor) => Window.Cursor = cursor is GLFWServiceCursor gc ? gc.Cursor : null;

		private Vector2i? windowedSize = null;
		private Vector2i? windowPosition = null;
		public void SetFullscreen(IDisplay? display, IDisplayMode? mode) {
			GLFWServiceDisplay? glfwdisplay = display as GLFWServiceDisplay;
			GLFWServiceDisplayMode? glfwmode = (GLFWServiceDisplayMode?)mode;
			Vector2i size = Window.Size;
			Vector2i pos = Window.Pos;
			GLFWMonitor curmon = Window.Monitor;
			// If transitioning out of fullscreen to windowed mode
			if (glfwdisplay == null && curmon) {
				// If last window position is known use it
				if (windowPosition != null) pos = windowPosition.Value;
				// Else center on the current display
				else pos = curmon.Pos + (curmon.VideoMode.Size / 2) - (size / 2);
				// If last windowed size is known use it
				if (windowedSize != null) size = windowedSize.Value;
			// Else if transitioning from windowed to fullscreen mode
			} else if (glfwdisplay != null && !curmon) {
				if (glfwmode == null) throw new ArgumentNullException(nameof(mode));
				// Save current window size and position
				windowPosition = Position;
				windowedSize = Size;
				// New size comes from display mode (position is ignored)
				size = glfwmode.VidMode.Size;
			}
			// If new mode is fullscreen use the given refresh rate, else don't care
			int refreshRate = GLFW3.DontCare;
			if (glfwdisplay != null) {
				if (glfwmode == null) throw new ArgumentNullException(nameof(mode));
				refreshRate = glfwmode.VidMode.RefreshRate;
			}
			Window.SetMonitor(glfwdisplay != null ? glfwdisplay.Monitor : default, pos, size, refreshRate);
		}

		public void StartTextInput() => textInput = true;

		public T? GetService<T>(IService<T> service) where T : notnull {
			if (service == InputServices.Clipboard) return (T)(object)this;
			else return ServiceInjector.Lookup(this, service);
		}

	}

	public class GLFWServiceWindowSystem : IWindowSystem {

		public bool CustomCursorSupport => true;

		public ICursor CreateCursor(IImage image, Vector2i hotspot) {
			if (image.Format != PixelFormat.R8G8B8A8UNorm) throw new ArgumentException("Custom GLFW cursor requires image to use R8G8B8A8UNorm pixel format", nameof(image));
			IPointer<byte> pPixels = image.MapPixels(MapMode.ReadOnly);
			try {
				GLFWCursor cursor = new(new GLFWImage() { Width = image.Size.X, Height = image.Size.Y, Pixels = pPixels.Ptr }, hotspot);
				return new GLFWServiceCursor(cursor);
			} finally {
				image.UnmapPixels();
			}
		}

		public ICursor CreateStandardCursor(StandardCursor std) {
			GLFWCursorShape shape = std switch {
				StandardCursor.Arrow => GLFWCursorShape.Arrow,
				StandardCursor.Crosshair => GLFWCursorShape.Crosshair,
				StandardCursor.Hand => GLFWCursorShape.Hand,
				StandardCursor.HResize => GLFWCursorShape.HResize,
				StandardCursor.IBeam => GLFWCursorShape.IBeam,
				StandardCursor.VResize => GLFWCursorShape.VResize,
				_ => GLFWCursorShape.Arrow
			};
			return new GLFWServiceCursor(new GLFWCursor(shape));
		}

		public IWindow CreateWindow(string title, int w, int h, WindowAttributeList? attributes = null) {
			GLFW3.DefaultWindowHints();
			return new GLFWServiceWindow(title, w, h, attributes);
		}

		public IDisplay? PrimaryDisplay {
			get {
				var monitor = GLFW3.PrimaryMonitor;
				if (monitor.Monitor == 0) return null;
				return new GLFWServiceDisplay(monitor);
			}
		}

		public IDisplay[] GetDisplays() {
			GLFWMonitor[] monitors = GLFW3.Monitors;
			IDisplay[] displays = new IDisplay[monitors.Length];
			for (int i = 0; i < displays.Length; i++) displays[i] = new GLFWServiceDisplay(monitors[i]);
			return displays;
		}

	}
}
