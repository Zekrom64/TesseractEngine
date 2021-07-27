using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Input;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Services;
using Tesseract.Core.Util;
using Tesseract.GL;

namespace Tesseract.SDL.Services {

	public class SDLServiceWindowSystem : IWindowSystem, IGLWindowSystem {

		public bool CustomCursorSupport => true;

		public IWindow CreateWindow(string title, int w, int h, WindowAttributeList attributes = null) => new SDLServiceWindow(title, w, h, attributes);

		public IDisplay[] GetDisplays() => SDL2.Displays.ConvertAll(display => new SDLServiceDisplay(display));

		public void RunEvents() {
			SDL2.PumpEvents();
			SDLEvent? evt;
			while ((evt = SDL2.PollEvent()).HasValue) PushEvent(evt.Value);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Kept as instanced in case instance fields are required in future")]
		public bool PushEvent(in SDLEvent evt) {
			switch(evt.Type) {
				case SDLEventType.WindowEvent: {
					IntPtr pWindow = SDL2.Functions.SDL_GetWindowData(SDL2.Functions.SDL_GetWindowFromID(evt.Window.WindowID), SDLServiceWindow.WindowDataID);
					SDLServiceWindow window = new ObjectPointer<SDLServiceWindow>(pWindow).Value;
					switch (evt.Window.Event) {
						case SDLWindowEventID.Resized: window.DoOnResize(new Vector2i(evt.Window.Data1, evt.Window.Data2)); break;
						case SDLWindowEventID.Moved: window.DoOnMove(new Vector2i(evt.Window.Data1, evt.Window.Data2)); break;
						case SDLWindowEventID.Minimized: window.DoOnMinimized(); break;
						case SDLWindowEventID.Maximized: window.DoOnMaximized(); break;
						case SDLWindowEventID.Restored: window.DoOnRestored(); break;
						case SDLWindowEventID.FocusGained: window.DoOnFocused(); break;
						case SDLWindowEventID.FocusLost: window.DoOnUnfocused(); break;
						case SDLWindowEventID.Close: window.DoOnClosing(); break;
						default: break;
					}
				} break;
				default: return false;
			}
			return true;
		}

		public T GetService<T>(IService<T> service) {
			if (service == GLServices.GLWindowSystem) return (T)(object)this;
			return default;
		}

		public void SetGLHint(GLWindowHint hint, int value) {
			SDLGLAttr attr = hint switch {
				GLWindowHint.RedBits => SDLGLAttr.RedSize,
				GLWindowHint.GreenBits => SDLGLAttr.GreenSize,
				GLWindowHint.BlueBits => SDLGLAttr.BlueSize,
				GLWindowHint.AlphaBits => SDLGLAttr.AlphaSize,
				GLWindowHint.DepthBits => SDLGLAttr.DepthSize,
				GLWindowHint.StencilBits => SDLGLAttr.StencilSize,
				GLWindowHint.AccumRedBits => SDLGLAttr.AccumRedSize,
				GLWindowHint.AccumGreenBits => SDLGLAttr.AccumGreenSize,
				GLWindowHint.AccumBlueBits => SDLGLAttr.AccumBlueSize,
				GLWindowHint.AccumAlphaBits => SDLGLAttr.AccumAlphaSize,
				GLWindowHint.Doublebuffer => SDLGLAttr.DoubleBuffer,
				GLWindowHint.ContextVersionMajor => SDLGLAttr.ContextMajorVersion,
				GLWindowHint.ContextVersionMinor => SDLGLAttr.ContextMinorVersion,
				GLWindowHint.ContextProfile => SDLGLAttr.ContextProfileMask,
				GLWindowHint.DebugContext => SDLGLAttr.ContextFlags,
				_ => throw new ArgumentException("Unsupported OpenGL window hint", nameof(hint))
			};
			// Correct hint value for certain hint types
			switch (hint) {
				case GLWindowHint.ContextProfile:
					value = (GLProfile)value switch {
						GLProfile.Compatibility => (int)SDLGLProfile.Compatibility,
						GLProfile.Core => (int)SDLGLProfile.Core,
						_ => throw new ArgumentException("Unsupported OpenGL profile", nameof(value)),
					};
					break;
				case GLWindowHint.DebugContext:
					value = (value != 0) ? (int)SDLGLContextFlag.DebugFlag : 0;
					break;
				default: break;
			}
			SDL2.CheckError(SDL2.Functions.SDL_GL_SetAttribute(attr, value));
		}

		// TODO
		public ICursor CreateCursor(IImage image, Vector2i hotspot) => throw new NotImplementedException();
		public ICursor CreateStandardCursor(StandardCursor std) => throw new NotImplementedException();
	}

	public class SDLServiceDisplayMode : IDisplayMode {

		public readonly SDLDisplayMode DisplayMode;

		public SDLServiceDisplayMode(SDLDisplayMode mode) {
			DisplayMode = mode;
		}

		public PixelFormat PixelFormat => SDLPixelService.ConvertPixelFormat(DisplayMode.Format);

		public IReadOnlyTuple2<int> Size => new Vector2i(DisplayMode.W, DisplayMode.H);

		public int RefreshRate => DisplayMode.RefreshRate;
	}

	public class SDLServiceDisplay : IDisplay {

		public readonly SDLDisplay Display;

		public IReadOnlyTuple2<int> Position {
			get {
				SDLRect bounds = Display.Bounds;
				return new Vector2i(bounds.X, bounds.Y);
			}
		}

		public string Name => Display.Name;

		public IDisplayMode CurrentMode => new SDLServiceDisplayMode(Display.CurrentDisplayMode);

		public IDisplayMode[] GetDisplayModes() => Display.Modes.ConvertAll(mode => new SDLServiceDisplayMode(mode));

		public T GetService<T>(IService<T> service) => default;

		public SDLServiceDisplay(SDLDisplay display) {
			Display = display;
		}

	}

	public class SDLServiceWindow : IDisposable, IWindow, IGammaRampObject, IGLContextProvider {

		internal const string WindowDataID = "__GCHandle";

		public readonly SDLWindow Window;

		private readonly object lockGlcontext = new();
		private SDLGLContext glcontext = null;

		public string Title { get => Window.Name; set => Window.Name = value; }
		public Vector2i Size { get => Window.Size; set => Window.Size = value; }
		public Vector2i Position { get => Window.Position; set => Window.Position = value; }
		public bool Minimized {
			get => (Window.Flags & SDLWindowFlags.Minimized) != 0;
			set {
				if (value) Window.Minimize();
				else Window.Restore();
			}
		}
		public bool Maximized {
			get => (Window.Flags & SDLWindowFlags.Maximized) != 0;
			set {
				if (value) Window.Maximize();
				else Window.Restore();
			}
		}
		public bool Visible {
			get => (Window.Flags & SDLWindowFlags.Shown) != 0;
			set {
				if (value) Window.Show();
				else Window.Hide();
			}
		}
		public bool Focused {
			get => (Window.Flags & SDLWindowFlags.InputFocus) != 0;
			set {
				if (value) Window.SetInputFocus();
			}
		}
		public bool Closing { get; set; }
		public float Opacity { get => Window.Opacity; set => Window.Opacity = value; }
		public bool Resizable { get => Window.Resizable; set => Window.Resizable = value; }
		public bool Fullscreen { get => (Window.Flags & SDLWindowFlags.Fullscreen) != 0; }
		public IDisplay FullscreenDisplay {
			get {
				if (!Fullscreen) return null;
				int index = SDL2.Functions.SDL_GetWindowDisplayIndex(Window.Window.Ptr);
				if (index < 0) throw new SDLException(SDL2.GetError());
				else return new SDLServiceDisplay(new SDLDisplay(index));
			}
		}

		public event Action<Vector2i> OnResize;
		internal void DoOnResize(Vector2i size) => OnResize?.Invoke(size);
		public event Action<Vector2i> OnMove;
		internal void DoOnMove(Vector2i pos) => OnMove?.Invoke(pos);
		public event Action OnMinimized;
		internal void DoOnMinimized() => OnMinimized?.Invoke();
		public event Action OnMaximized;
		internal void DoOnMaximized() => OnMaximized?.Invoke();
		public event Action OnRestored;
		internal void DoOnRestored() => OnRestored?.Invoke();
		public event Action OnFocused;
		internal void DoOnFocused() => OnFocused?.Invoke();
		public event Action OnUnfocused;
		internal void DoOnUnfocused() => OnUnfocused?.Invoke();
		public event Action OnClosing;
		internal void DoOnClosing() => OnClosing?.Invoke();
		public event Action<KeyEvent> OnKey;
		internal void DoOnKey(KeyEvent evt) => OnKey?.Invoke(evt);
		public event Action<TextInputEvent> OnTextInput;
		internal void DoOnTextInput(TextInputEvent evt) => OnTextInput?.Invoke(evt);
		public event Action<TextEditEvent> OnTextEdit;
		internal void DoOnTextEdit(TextEditEvent evt) => OnTextEdit?.Invoke(evt);
		public event Action<MouseMoveEvent> OnMouseMove;
		internal void DoOnMouseMove(MouseMoveEvent evt) => OnMouseMove?.Invoke(evt);
		public event Action<MouseButtonEvent> OnMouseButton;
		internal void DoOnMouseButton(MouseButtonEvent evt) => OnMouseButton?.Invoke(evt);
		public event Action<MouseWheelEvent> OnMouseWheel;
		internal void DoOnMouseWheel(MouseWheelEvent evt) => OnMouseWheel?.Invoke(evt);

		private static SDLWindowFlags GetAttributeFlags(WindowAttributeList attributes) {
			if (attributes == null) return 0;
			SDLWindowFlags flags = 0;
			if (attributes.TryGet(WindowAttributes.Visible, out bool visible)) {
				if (visible) flags |= SDLWindowFlags.Shown;
				else flags |= SDLWindowFlags.Hidden;
			} else flags |= SDLWindowFlags.Shown;
			if (attributes.TryGet(WindowAttributes.Resizable, out bool resizable) && resizable) flags |= SDLWindowFlags.Resizable;
			if (attributes.TryGet(WindowAttributes.Minimized, out bool minimized) && minimized) flags |= SDLWindowFlags.Minimized;
			if (attributes.TryGet(WindowAttributes.Maximized, out bool maximized) && maximized) flags |= SDLWindowFlags.Maximized;
			if (attributes.TryGet(WindowAttributes.Focused, out bool focused) && focused) flags |= SDLWindowFlags.InputFocus;
			return flags;
		}

		public SDLServiceWindow(string title, int w, int h, WindowAttributeList attributes) {
			Vector2i position = new(SDL2.WindowPosUndefined);
			if (attributes != null) {
				if (attributes.TryGet(WindowAttributes.Title, out string newTitle)) title = newTitle;
				if (attributes.TryGet(WindowAttributes.Position, out Vector2i newPosition)) position = newPosition;
				if (attributes.TryGet(WindowAttributes.Size, out Vector2i newSize)) { w = newSize.X; h = newSize.Y; }
			}
			Window = new SDLWindow(title, position.X, position.Y, w, h, GetAttributeFlags(attributes));
			if (attributes != null) {
				if (attributes.TryGet(WindowAttributes.Closing, out bool closing)) Closing = closing;
				if (attributes.TryGet(WindowAttributes.Opacity, out float opacity)) Opacity = opacity;
			}
			SDL2.Functions.SDL_SetWindowData(Window.Window.Ptr, WindowDataID, new ObjectPointer<SDLServiceWindow>(this).Ptr);
		}

		public IGLContext CreateContext() {
			lock (lockGlcontext) {
				if (glcontext == null) {
					IntPtr pContext = SDL2.Functions.SDL_GL_CreateContext(Window.Window.Ptr);
					if (pContext == IntPtr.Zero) throw new SDLException(SDL2.GetError());
					glcontext = new SDLGLContext(Window.Window.Ptr, pContext);
				}
				return glcontext;
			}
		}

		public T GetService<T>(IService<T> service) {
			if (service == GLServices.GLContextProvider) return (T)(object)this;
			else if (service == GraphicsServices.GammaRampObject) return (T)(object)this;
			return default;
		}

		public void Restore() {
			if (Fullscreen) SDL2.CheckError(SDL2.Functions.SDL_SetWindowFullscreen(Window.Window.Ptr, 0));
			Window.Restore();
		}

		public void SetFullscreen(IDisplay display, IDisplayMode mode) {
			SDLDisplay sdldisplay = (display as SDLServiceDisplay).Display;
			SDLDisplayMode sdlmode;
			if (mode is SDLServiceDisplayMode) sdlmode = (mode as SDLServiceDisplayMode).DisplayMode;
			else {
				sdlmode = new SDLDisplayMode() {
					W = mode.Size.X,
					H = mode.Size.Y,
					Format = SDLPixelService.ConvertPixelFormat(mode.PixelFormat),
					RefreshRate = mode.RefreshRate
				};
			}
			SDL2.Functions.SDL_GetClosestDisplayMode(sdldisplay.DisplayIndex, sdlmode, out sdlmode);
			// SDL's fullscreen system is a bit wonky
			// First set the window's display mode to the selected mode
			SDL2.CheckError(SDL2.Functions.SDL_SetWindowDisplayMode(Window.Window.Ptr, sdlmode));
			// Then move the window to the center of the display
			SDL2.Functions.SDL_SetWindowPosition(Window.Window.Ptr, SDL2.WindowPosCenteredOnDisplay(sdldisplay), SDL2.WindowPosCenteredOnDisplay(sdldisplay));
			// Then make fullscreen on the display
			SDL2.CheckError(SDL2.Functions.SDL_SetWindowFullscreen(Window.Window.Ptr, (uint)SDLWindowFlags.FullscreenDesktop));
		}

		public IGammaRamp GammaRamp {
			get {
				SDLServiceGammaRamp ramp = new();
				unsafe {
					fixed (ushort* pRed = ramp.Red) {
						fixed (ushort* pGreen = ramp.Green) {
							fixed (ushort* pBlue = ramp.Blue) {
								SDL2.CheckError(SDL2.Functions.SDL_GetWindowGammaRamp(Window.Window.Ptr, (IntPtr)pRed, (IntPtr)pGreen, (IntPtr)pBlue));
							}
						}
					}
				}
				return ramp;
			}
			set {
				unsafe {
					fixed(ushort* pRed = value.Red) {
						fixed(ushort* pGreen = value.Green) {
							fixed(ushort* pBlue = value.Blue) {
								SDL2.CheckError(SDL2.Functions.SDL_SetWindowGammaRamp(Window.Window.Ptr, (IntPtr)pRed, (IntPtr)pGreen, (IntPtr)pBlue));
							}
						}
					}
				}
			}
		}

		// TODO
		public bool CaptureMouse { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public IWindowSurface Surface => throw new NotImplementedException();

		public Vector2i MousePosition => throw new NotImplementedException();

		public IGammaRamp CreateCompatibleGammaRamp() => new SDLServiceGammaRamp();

		public void Dispose() {
			GC.SuppressFinalize(this);
			ObjectPointer<SDLServiceWindow> gchandle = new(SDL2.Functions.SDL_GetWindowData(Window.Window.Ptr, WindowDataID));
			gchandle.Dispose();
			Window.Dispose();
		}

		// TODO
		public void SetCursor(ICursor cursor) => throw new NotImplementedException();
		public bool GetKeyState(Key key) => throw new NotImplementedException();
		public void StartTextInput() => throw new NotImplementedException();
		public void EndTextInput() => throw new NotImplementedException();
		public bool GetMouseButtonState(int button) => throw new NotImplementedException();
	}

	public class SDLServiceGammaRamp : IGammaRamp {

		public const int RampSize = 256;

		private readonly ushort[] red = new ushort[RampSize], green = new ushort[RampSize], blue = new ushort[RampSize];

		public Vector3us this[int index] {
			get => new(red[index], green[index], blue[index]);
			set {
				red[index] = value.X;
				green[index] = value.Y;
				blue[index] = value.Z;
			}
		}

		public int Length => RampSize;

		public ushort[] Red { get => red; set => Array.Copy(value, red, Math.Min(red.Length, value.Length)); }
		public ushort[] Green { get => green; set => Array.Copy(value, green, Math.Min(green.Length, value.Length)); }
		public ushort[] Blue { get => blue; set => Array.Copy(value, blue, Math.Min(blue.Length, value.Length)); }

	}



}
