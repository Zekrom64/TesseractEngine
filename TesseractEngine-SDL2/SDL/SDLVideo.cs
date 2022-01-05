using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct SDLDisplayMode {
		private readonly uint format;
		public uint Format { get => format; init => format = value; }
		private readonly int w;
		public int W { get => w; init => w = value; }
		private readonly int h;
		public int H { get => h; init => h = value; }
		private readonly int refreshRate;
		public int RefreshRate { get => refreshRate; init => refreshRate = value; }
		private readonly IntPtr driverData;
		public IntPtr DriverData { get => driverData; init => driverData = value; }
	}

	public enum SDLWindowFlags : uint {
		Fullscreen = 0x00000001,
		OpenGL = 0x00000002,
		Shown = 0x00000004,
		Hidden = 0x00000008,
		Borderless = 0x00000010,
		Resizable = 0x00000020,
		Minimized = 0x00000040,
		Maximized = 0x00000080,
		InputGrabbed = 0x00000100,
		InputFocus = 0x00000200,
		MouseFocus = 0x00000400,
		FullscreenDesktop = Fullscreen | 0x00001000,
		Foreign = 0x00000800,
		AllowHighDPI = 0x00002000,
		MouseCapture = 0x00004000,
		AlwaysOnTop = 0x00008000,
		SkipTaskbar = 0x00010000,
		Utility = 0x0020000,
		Tooltip = 0x0040000,
		PopupMenu = 0x00080000,
		Vulkan = 0x10000000
	}

	public enum SDLWindowEventID : byte {
		None,
		Shown,
		Hidden,
		Exposed,
		Moved,
		Resized,
		SizeChanged,
		Minimized,
		Maximized,
		Restored,
		Enter,
		Leave,
		FocusGained,
		FocusLost,
		Close,
		TakeFocus,
		HitTest
	}

	public enum SDLDisplayEventID : byte {
		None,
		Orientation
	}

	public enum SDLDisplayOrientation {
		Unknown,
		Landscape,
		LandscapeFlipped,
		Portrait,
		PortraitFlipped
	}

	public enum SDLGLAttr {
		RedSize,
		GreenSize,
		BlueSize,
		AlphaSize,
		BufferSize,
		DoubleBuffer,
		DepthSize,
		StencilSize,
		AccumRedSize,
		AccumGreenSize,
		AccumBlueSize,
		AccumAlphaSize,
		Stereo,
		MultisampleBuffers,
		MultisampleSamples,
		AcceleratedVisual,
		RetainedBacking,
		ContextMajorVersion,
		ContextMinorVersion,
		ContextEGL,
		ContextFlags,
		ContextProfileMask,
		ShareWithCurrentContext,
		FramebufferSRGBCapable,
		ContextReleaseBehavior,
		ContextResetNotification,
		ContextNoError
	}

	public enum SDLGLProfile {
		Core = 0x0001,
		Compatibility = 0x0002,
		ES = 0x0004
	}

	public enum SDLGLContextFlag {
		DebugFlag = 0x0001,
		ForwardCompatibleFlag = 0x0002,
		RobustAccessFlag = 0x0004,
		ResetIsolationFlag = 0x0008
	}

	public enum SDLGLContextReleaseFlag {
		None = 0x0000,
		Flush = 0x0001
	}

	public enum SDLGLContextResetNotification {
		NoNotification = 0x0000,
		LoseContext = 0x0001
	}

	public enum SDLHitTestResult {
		Normal,
		Draggable,
		ResizeTopLeft,
		ResizeTop,
		ResizeTopRight,
		ResizeRight,
		ResizeBottomRight,
		ResizeBottom,
		ResizeBottomLeft,
		ResizeLeft,
	}

	public delegate SDLHitTestResult SDLHitTest([NativeType("SDL_Window*")] IntPtr window, in SDLPoint area, IntPtr data);

	public class SDLDisplay {

		public readonly int DisplayIndex;

		public string Name => Marshal.PtrToStringUTF8(SDL2.Functions.SDL_GetDisplayName(DisplayIndex))!;

		public SDLRect Bounds {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDisplayBounds(DisplayIndex, out SDLRect rect));
				return rect;
			}
		}

		public SDLRect UsableBounds {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDisplayUsableBounds(DisplayIndex, out SDLRect rect));
				return rect;
			}
		}

		public float DiagonalDPI {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out float ddpi, out _, out _));
				return ddpi;
			}
		}

		public float HorizontalDPI {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out _, out float hdpi, out _));
				return hdpi;
			}
		}

		public float VerticalDPI {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out _, out _, out float vdpi));
				return vdpi;
			}
		}

		public SDLDisplayOrientation Orientation => SDL2.Functions.SDL_GetDisplayOrientation(DisplayIndex);

		public SDLDisplayMode[] Modes {
			get {
				SDLDisplayMode[] modes = new SDLDisplayMode[SDL2.Functions.SDL_GetNumDisplayModes(DisplayIndex)];
				for (int i = 0; i < modes.Length; i++) SDL2.CheckError(SDL2.Functions.SDL_GetDisplayMode(DisplayIndex, i, out modes[i]));
				return modes;
			}
		}

		public SDLDisplayMode DesktopDisplayMode {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetDesktopDisplayMode(DisplayIndex, out SDLDisplayMode mode));
				return mode;
			}
		}

		public SDLDisplayMode CurrentDisplayMode {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetCurrentDisplayMode(DisplayIndex, out SDLDisplayMode mode));
				return mode;
			}
		}

		public int Direct3D9AdapterIndex => SDL2.Functions.SDL_Direct3D9GetAdapterIndex(DisplayIndex);

		public SDLDisplay(int displayIndex) {
			DisplayIndex = displayIndex;
		}

		public SDLDisplayMode? GetClosestDisplayMode(in SDLDisplayMode mode) {
			IntPtr ptr = SDL2.Functions.SDL_GetClosestDisplayMode(DisplayIndex, mode, out _);
			return ptr == IntPtr.Zero ? null : Marshal.PtrToStructure<SDLDisplayMode>(ptr);
		}

		public (int, int) DXGIGetOutputInfo() {
			SDL2.Functions.SDL_DXGIGetOutputInfo(DisplayIndex, out int adapterIndex, out int outputIndex);
			return (adapterIndex, outputIndex);
		}

	}

	public class SDLWindow : IDisposable {

		public IPointer<SDL_Window> Window { get; private set; }

		public SDLDisplay Display {
			get {
				int index = SDL2.Functions.SDL_GetWindowDisplayIndex(Window.Ptr);
				if (index < 0) SDL2.CheckError(index);
				return new SDLDisplay(index);
			}
		}

		public SDLDisplayMode DisplayMode {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetWindowDisplayMode(Window.Ptr, out SDLDisplayMode mode));
				return mode;
			}
			set => SDL2.CheckError(SDL2.Functions.SDL_SetWindowDisplayMode(Window.Ptr, value));
		}

		public SDLPixelFormatEnum PixelFormat => SDL2.Functions.SDL_GetWindowPixelFormat(Window.Ptr);

		public uint ID => SDL2.Functions.SDL_GetWindowID(Window.Ptr);

		public SDLWindowFlags Flags => (SDLWindowFlags)SDL2.Functions.SDL_GetWindowFlags(Window.Ptr);

		public string Name {
			get => Marshal.PtrToStringUTF8(SDL2.Functions.SDL_GetWindowTitle(Window.Ptr))!;
			set => SDL2.Functions.SDL_SetWindowTitle(Window.Ptr, value);
		}

		public SDLSurface Icon {
			set => SDL2.Functions.SDL_SetWindowIcon(Window.Ptr, value?.Surface.Ptr ?? IntPtr.Zero);
		}

		public Vector2i Position {
			get {
				SDL2.Functions.SDL_GetWindowPosition(Window.Ptr, out int x, out int y);
				return new Vector2i(x, y);
			}
			set => SDL2.Functions.SDL_SetWindowPosition(Window.Ptr, value.X, value.Y);
		}

		public Vector2i Size {
			get {
				SDL2.Functions.SDL_GetWindowSize(Window.Ptr, out int w, out int h);
				return new Vector2i(w, h);
			}
			set => SDL2.Functions.SDL_SetWindowSize(Window.Ptr, value.X, value.Y);
		}

		public Vector2i MinimumSize {
			get {
				SDL2.Functions.SDL_GetWindowMinimumSize(Window.Ptr, out int w, out int h);
				return new Vector2i(w, h);
			}
			set => SDL2.Functions.SDL_SetWindowMinimumSize(Window.Ptr, value.X, value.Y);
		}

		public Vector2i MaximumSize {
			get {
				SDL2.Functions.SDL_GetWindowMaximumSize(Window.Ptr, out int w, out int h);
				return new Vector2i(w, h);
			}
			set => SDL2.Functions.SDL_SetWindowMaximumSize(Window.Ptr, value.X, value.Y);
		}

		public bool Bordered {
			get => (Flags & SDLWindowFlags.Borderless) == 0;
			set => SDL2.Functions.SDL_SetWindowBordered(Window.Ptr, value);
		}

		public bool Resizable {
			get => (Flags & SDLWindowFlags.Resizable) != 0;
			set => SDL2.Functions.SDL_SetWindowResizable(Window.Ptr, value);
		}

		public SDLWindowFlags Fullscreen {
			get => Flags;
			set => SDL2.CheckError(SDL2.Functions.SDL_SetWindowFullscreen(Window.Ptr, (uint)value));
		}

		public SDLSurface Surface => new(new UnmanagedPointer<SDL_Surface>(SDL2.Functions.SDL_GetWindowSurface(Window.Ptr)));

		public bool Grab {
			get => SDL2.Functions.SDL_GetWindowGrab(Window.Ptr);
			set => SDL2.Functions.SDL_SetWindowGrab(Window.Ptr, value);
		}

		public float Brightness {
			get => SDL2.Functions.SDL_GetWindowBrightness(Window.Ptr);
			set => SDL2.Functions.SDL_SetWindowBrightness(Window.Ptr, value);
		}

		public float Opacity {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetWindowOpacity(Window.Ptr, out float opacity));
				return opacity;
			}
			set => SDL2.Functions.SDL_SetWindowOpacity(Window.Ptr, value);
		}

		public bool IsShapedWindow => SDL2.Functions.SDL_IsShapedWindow(Window.Ptr);

		public SDLWindowShapeMode ShapedWindowMode {
			get {
				SDL2.CheckError(SDL2.Functions.SDL_GetShapedWindowMode(Window.Ptr, out SDLWindowShapeMode shapeMode));
				return shapeMode;
			}
		}

		public SDLWindow(IPointer<SDL_Window> window) {
			Window = window;
		}

		public SDLWindow(string title, int x, int y, int w, int h, SDLWindowFlags flags) {
			IntPtr ptr = SDL2.Functions.SDL_CreateWindow(title, x, y, w, h, (uint)flags);
			if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			Window = new UnmanagedPointer<SDL_Window>(ptr);
		}

		public SDLWindow(IntPtr data) {
			IntPtr ptr = SDL2.Functions.SDL_CreateWindowFrom(data);
			if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			Window = new UnmanagedPointer<SDL_Window>(ptr);
		}

		public void GetWindowBordersSize(out int top, out int left, out int bottom, out int right) =>
			SDL2.CheckError(SDL2.Functions.SDL_GetWindowBordersSize(Window.Ptr, out top, out left, out bottom, out right));

		public void Show() => SDL2.Functions.SDL_ShowWindow(Window.Ptr);

		public void Hide() => SDL2.Functions.SDL_HideWindow(Window.Ptr);

		public void Raise() => SDL2.Functions.SDL_RaiseWindow(Window.Ptr);

		public void Maximize() => SDL2.Functions.SDL_MaximizeWindow(Window.Ptr);

		public void Minimize() => SDL2.Functions.SDL_MinimizeWindow(Window.Ptr);

		public void Restore() => SDL2.Functions.SDL_RestoreWindow(Window.Ptr);

		public void UpdateSurface() => SDL2.CheckError(SDL2.Functions.SDL_UpdateWindowSurface(Window.Ptr));

		public void UpdateSurfaceRects(Span<SDLRect> rects) {
			unsafe {
				fixed (SDLRect* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_UpdateWindowSurfaceRects(Window.Ptr, (IntPtr)pRects, rects.Length));
				}
			}
		}

		public void UpdateSurfaceRects(SDLRect[] rects) => UpdateSurfaceRects(new Span<SDLRect>(rects));

		public void SetModalFor(SDLWindow parent) => SDL2.CheckError(SDL2.Functions.SDL_SetWindowModalFor(Window.Ptr, parent.Window.Ptr));

		public void SetInputFocus() => SDL2.CheckError(SDL2.Functions.SDL_SetWindowInputFocus(Window.Ptr));

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Window != null && !Window.IsNull) {
				SDL2.Functions.SDL_DestroyWindow(Window.Ptr);
				Window = null!;
			}
		}

		public IntPtr this[string name] {
			get => SDL2.Functions.SDL_GetWindowData(Window.Ptr, name);
			set => SDL2.Functions.SDL_SetWindowData(Window.Ptr, name, value);
		}

		public bool IsScreenKeyboardShown => SDL2.Functions.SDL_IsScreenKeyboardShown(Window.Ptr);

		public void WarpMouseInWindow(int x, int y) => SDL2.Functions.SDL_WarpMouseInWindow(Window.Ptr, x, y);

		public IntPtr MetalCreateView => SDL2.Functions.SDL_Metal_CreateView(Window.Ptr);

		public Vector2i MetalGetDrawableSize() {
			SDL2.Functions.SDL_Metal_GetDrawableSize(Window.Ptr, out int w, out int h);
			return new Vector2i(w, h);
		}

		public SDLRenderer CreateRenderer(int index, SDLRendererFlags flags) {
			IntPtr pRender = SDL2.Functions.SDL_CreateRenderer(Window.Ptr, index, flags);
			if (pRender == IntPtr.Zero) throw new SDLException(SDL2.GetError());
			return new SDLRenderer(pRender);
		}

		public SDLRenderer? Renderer {
			get {
				IntPtr pRender = SDL2.Functions.SDL_GetRenderer(Window.Ptr);
				if (pRender == IntPtr.Zero) return null;
				return new SDLRenderer(pRender);
			}
		}

		public void SetWindowShape(SDLSurface shape, SDLWindowShapeMode shapeMode) =>
			SDL2.CheckError(SDL2.Functions.SDL_SetWindowShape(Window.Ptr, shape.Surface.Ptr, shapeMode));

		public SDLSysWMInfo GetWindowWMInfo(SDLVersion ver) {
			unsafe {
				SDL_SysWMinfo info = new() {
					Version = ver
				};
				if (!SDL2.Functions.SDL_GetWindowWMInfo(Window.Ptr, ref info)) return null;
				object? wminfo = null;
				switch(info.Subsystem) {
					case SDLSysWMType.Windows:
						wminfo = info.Info.Win;
						break;
					default: break;
				}
				return new SDLSysWMInfo() {
					Version = info.Version,
					Subsystem = info.Subsystem,
					Info = wminfo
				};
			}
		}

	}

}
