using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
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

	[Flags]
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

	[Flags]
	public enum SDLGLProfile {
		Core = 0x0001,
		Compatibility = 0x0002,
		ES = 0x0004
	}

	[Flags]
	public enum SDLGLContextFlag {
		DebugFlag = 0x0001,
		ForwardCompatibleFlag = 0x0002,
		RobustAccessFlag = 0x0004,
		ResetIsolationFlag = 0x0008
	}

	[Flags]
	public enum SDLGLContextReleaseFlag {
		None = 0x0000,
		Flush = 0x0001
	}

	[Flags]
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

	public delegate SDLHitTestResult SDLHitTest([NativeType("SDL_Window*")] IntPtr window, in Vector2i area, IntPtr data);

	public class SDLDisplay {

		public readonly int DisplayIndex;

		public string Name {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(SDL2.Functions.SDL_GetDisplayName(DisplayIndex))!;
				}
			}
		}

		public Recti Bounds {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDisplayBounds(DisplayIndex, out Recti rect));
					return rect;
				}
			}
		}

		public Recti UsableBounds {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDisplayUsableBounds(DisplayIndex, out Recti rect));
					return rect;
				}
			}
		}

		public float DiagonalDPI {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out float ddpi, out _, out _));
					return ddpi;
				}
			}
		}

		public float HorizontalDPI {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out _, out float hdpi, out _));
					return hdpi;
				}
			}
		}

		public float VerticalDPI {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDisplayDPI(DisplayIndex, out _, out _, out float vdpi));
					return vdpi;
				}
			}
		}

		public SDLDisplayOrientation Orientation {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetDisplayOrientation(DisplayIndex);
				}
			}
		}

		public SDLDisplayMode[] Modes {
			get {
				unsafe {
					SDLDisplayMode[] modes = new SDLDisplayMode[SDL2.Functions.SDL_GetNumDisplayModes(DisplayIndex)];
					for (int i = 0; i < modes.Length; i++) SDL2.CheckError(SDL2.Functions.SDL_GetDisplayMode(DisplayIndex, i, out modes[i]));
					return modes;
				}
			}
		}

		public SDLDisplayMode DesktopDisplayMode {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetDesktopDisplayMode(DisplayIndex, out SDLDisplayMode mode));
					return mode;
				}
			}
		}

		public SDLDisplayMode CurrentDisplayMode {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetCurrentDisplayMode(DisplayIndex, out SDLDisplayMode mode));
					return mode;
				}
			}
		}

		public int Direct3D9AdapterIndex {
			get {
				unsafe {
					return SDL2.Functions.SDL_Direct3D9GetAdapterIndex(DisplayIndex);
				}
			}
		}

		public SDLDisplay(int displayIndex) {
			DisplayIndex = displayIndex;
		}

		public SDLDisplayMode? GetClosestDisplayMode(in SDLDisplayMode mode) {
			unsafe {
				SDLDisplayMode* ptr = SDL2.Functions.SDL_GetClosestDisplayMode(DisplayIndex, mode, out _);
				return ptr == (SDLDisplayMode*)0 ? null : *ptr;
			}
		}

		public (int, int) DXGIGetOutputInfo() {
			unsafe {
				SDL2.Functions.SDL_DXGIGetOutputInfo(DisplayIndex, out int adapterIndex, out int outputIndex);
				return (adapterIndex, outputIndex);
			}
		}

	}

	public class SDLWindow : IDisposable {

		[NativeType("SDL_Window*")]
		public IntPtr Window { get; private set; }

		public SDLDisplay Display {
			get {
				unsafe {
					int index = SDL2.Functions.SDL_GetWindowDisplayIndex(Window);
					if (index < 0) SDL2.CheckError(index);
					return new SDLDisplay(index);
				}
			}
		}

		public SDLDisplayMode DisplayMode {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetWindowDisplayMode(Window, out SDLDisplayMode mode));
					return mode;
				}
			}
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetWindowDisplayMode(Window, value));
				}
			}
		}

		public SDLPixelFormatEnum PixelFormat {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetWindowPixelFormat(Window);
				}
			}
		}

		public uint ID {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetWindowID(Window);
				}
			}
		}

		public SDLWindowFlags Flags {
			get {
				unsafe {
					return (SDLWindowFlags)SDL2.Functions.SDL_GetWindowFlags(Window);
				}
			}
		}

		public string Title {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(SDL2.Functions.SDL_GetWindowTitle(Window))!;
				}
			}
			set {
				unsafe {
					fixed(byte* pTitle = MemoryUtil.StackallocUTF8(value, stackalloc byte[1024])) {
						SDL2.Functions.SDL_SetWindowTitle(Window, pTitle);
					}
				}
			}
		}

		public SDLSurface Icon {
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowIcon(Window, (SDL_Surface*)(value?.Surface?.Ptr ?? IntPtr.Zero));
				}
			}
		}

		public Vector2i Position {
			get {
				unsafe {
					SDL2.Functions.SDL_GetWindowPosition(Window, out int x, out int y);
					return new Vector2i(x, y);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowPosition(Window, value.X, value.Y);
				}
			}
		}

		public Vector2i Size {
			get {
				unsafe {
					SDL2.Functions.SDL_GetWindowSize(Window, out int w, out int h);
					return new Vector2i(w, h);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowSize(Window, value.X, value.Y);
				}
			}
		}

		public Vector2i MinimumSize {
			get {
				unsafe {
					SDL2.Functions.SDL_GetWindowMinimumSize(Window, out int w, out int h);
					return new Vector2i(w, h);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowMinimumSize(Window, value.X, value.Y);
				}
			}
		}

		public Vector2i MaximumSize {
			get {
				unsafe {
					SDL2.Functions.SDL_GetWindowMaximumSize(Window, out int w, out int h);
					return new Vector2i(w, h);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowMaximumSize(Window, value.X, value.Y);
				}
			}
		}

		public bool Bordered {
			get => (Flags & SDLWindowFlags.Borderless) == 0;
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowBordered(Window, value);
				}
			}
		}

		public bool Resizable {
			get => (Flags & SDLWindowFlags.Resizable) != 0;
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowResizable(Window, value);
				}
			}
		}

		public SDLWindowFlags Fullscreen {
			get => Flags;
			set {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_SetWindowFullscreen(Window, (uint)value));
				}
			}
		}

		public SDLSurface Surface {
			get {
				unsafe {
					return new(new UnmanagedPointer<SDL_Surface>((IntPtr)SDL2.Functions.SDL_GetWindowSurface(Window)));
				}
			}
		}

		public bool Grab {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetWindowGrab(Window);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowGrab(Window, value);
				}
			}
		}

		public float Brightness {
			get {
				unsafe {
					return SDL2.Functions.SDL_GetWindowBrightness(Window);
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowBrightness(Window, value);
				}
			}
		}

		public float Opacity {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetWindowOpacity(Window, out float opacity));
					return opacity;
				}
			}
			set {
				unsafe {
					SDL2.Functions.SDL_SetWindowOpacity(Window, value);
				}
			}
		}

		public bool IsShapedWindow {
			get {
				unsafe {
					return SDL2.Functions.SDL_IsShapedWindow(Window);
				}
			}
		}

		public SDLWindowShapeMode ShapedWindowMode {
			get {
				unsafe {
					SDL2.CheckError(SDL2.Functions.SDL_GetShapedWindowMode(Window, out SDLWindowShapeMode shapeMode));
					return shapeMode;
				}
			}
		}

		public SDLWindow([NativeType("SDL_Window*")] IntPtr window) {
			Window = window;
		}

		public SDLWindow(string title, int x, int y, int w, int h, SDLWindowFlags flags) {
			unsafe {
				fixed(byte* pTitle = MemoryUtil.StackallocUTF8(title, stackalloc byte[1024])) {
					IntPtr ptr = SDL2.Functions.SDL_CreateWindow(pTitle, x, y, w, h, (uint)flags);
					if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
					Window = ptr;
				}
			}
		}

		public static SDLWindow Create(IntPtr data) {
			unsafe {
				IntPtr ptr = SDL2.Functions.SDL_CreateWindowFrom(data);
				if (ptr == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLWindow(ptr);
			}
		}

		public void GetWindowBordersSize(out int top, out int left, out int bottom, out int right) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_GetWindowBordersSize(Window, out top, out left, out bottom, out right));
			}
		}

		public void Show() {
			unsafe {
				SDL2.Functions.SDL_ShowWindow(Window);
			}
		}

		public void Hide() {
			unsafe {
				SDL2.Functions.SDL_HideWindow(Window);
			}
		}

		public void Raise() {
			unsafe {
				SDL2.Functions.SDL_RaiseWindow(Window);
			}
		}

		public void Maximize() {
			unsafe {
				SDL2.Functions.SDL_MaximizeWindow(Window);
			}
		}

		public void Minimize() {
			unsafe {
				SDL2.Functions.SDL_MinimizeWindow(Window);
			}
		}

		public void Restore() {
			unsafe {
				SDL2.Functions.SDL_RestoreWindow(Window);
			}
		}

		public void UpdateSurface() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_UpdateWindowSurface(Window));
			}
		}

		public void UpdateSurfaceRects(Span<Recti> rects) {
			unsafe {
				fixed (Recti* pRects = rects) {
					SDL2.CheckError(SDL2.Functions.SDL_UpdateWindowSurfaceRects(Window, pRects, rects.Length));
				}
			}
		}

		public void UpdateSurfaceRects(params Recti[] rects) => UpdateSurfaceRects(new Span<Recti>(rects));

		public void SetModalFor(SDLWindow parent) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_SetWindowModalFor(Window, parent.Window));
			}
		}

		public void SetInputFocus() {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_SetWindowInputFocus(Window));
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Window != IntPtr.Zero) {
				unsafe {
					SDL2.Functions.SDL_DestroyWindow(Window);
				}
				Window = IntPtr.Zero;
			}
		}

		public IntPtr this[string name] {
			get {
				unsafe {
					fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
						return SDL2.Functions.SDL_GetWindowData(Window, pName);
					}
				}
			}
			set {
				unsafe {
					fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
						SDL2.Functions.SDL_SetWindowData(Window, pName, value);
					}
				}
			}
		}

		public bool IsScreenKeyboardShown {
			get {
				unsafe {
					return SDL2.Functions.SDL_IsScreenKeyboardShown(Window);
				}
			}
		}

		public void WarpMouseInWindow(int x, int y) {
			unsafe {
				SDL2.Functions.SDL_WarpMouseInWindow(Window, x, y);
			}
		}

		public IntPtr MetalCreateView() {
			unsafe {
				return SDL2.Functions.SDL_Metal_CreateView(Window);
			}
		}

		public Vector2i MetalGetDrawableSize() {
			unsafe {
				SDL2.Functions.SDL_Metal_GetDrawableSize(Window, out int w, out int h);
				return new Vector2i(w, h);
			}
		}

		public SDLRenderer CreateRenderer(int index, SDLRendererFlags flags) {
			unsafe {
				IntPtr pRender = SDL2.Functions.SDL_CreateRenderer(Window, index, flags);
				if (pRender == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				return new SDLRenderer(pRender);
			}
		}

		public SDLRenderer? Renderer {
			get {
				unsafe {
					IntPtr pRender = SDL2.Functions.SDL_GetRenderer(Window);
					if (pRender == IntPtr.Zero) return null;
					return new SDLRenderer(pRender);
				}
			}
		}

		public void SetWindowShape(SDLSurface shape, SDLWindowShapeMode shapeMode) {
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_SetWindowShape(Window, (SDL_Surface*)shape.Surface.Ptr, shapeMode));
			}
		}

		public SDLSysWMInfo? GetWindowWMInfo(SDLVersion ver) {
			unsafe {
				SDL_SysWMinfo info = new() {
					Version = ver
				};
				if (!SDL2.Functions.SDL_GetWindowWMInfo(Window, ref info)) return null;
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

		private unsafe delegate* unmanaged<IntPtr, out int, out int, void> fnGetDrawableSize;

		/// <summary>
		/// Gets the drawable size by invoking the appropriate underlying API function (eg. <c>SDL_GL_GetDrawableSize</c>).
		/// </summary>
		public Vector2i DrawableSize {
			get {
				unsafe {
					if (fnGetDrawableSize == null) {
						var flags = Flags;
						if ((flags & SDLWindowFlags.OpenGL) != 0) fnGetDrawableSize = SDL2.Functions.SDL_GL_GetDrawableSize;
						else if ((flags & SDLWindowFlags.Vulkan) != 0) fnGetDrawableSize = SDL2.Functions.SDL_Vulkan_GetDrawableSize;
						else fnGetDrawableSize = SDL2.Functions.SDL_GetWindowSize;
					}

					Vector2i sz = default;
					fnGetDrawableSize(Window, out sz.X, out sz.Y);
					return sz;
				}
			}
		}

	}

}
