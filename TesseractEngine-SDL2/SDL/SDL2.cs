using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Util;
using Tesseract.SDL.Native;

namespace Tesseract.SDL {

	/// <summary>
	/// Bitmask of SDL subsystems.
	/// </summary>
	public enum SDLSubsystems : uint {
		/// <summary>
		/// Timer subsystem.
		/// </summary>
		Timer =          0x00000001,
		/// <summary>
		/// Audio subsystem.
		/// </summary>
		Audio =          0x00000010,
		/// <summary>
		/// Video subsystem. Automatically initializes the events subsystem.
		/// </summary>
		Video =          0x00000020,
		/// <summary>
		/// Joystick subsystem. Automatically initializes the events subsystem.
		/// </summary>
		Joystick =       0x00000200,
		/// <summary>
		/// Haptic subsystem.
		/// </summary>
		Haptic =         0x00001000,
		/// <summary>
		/// Controller subsystem. Automatically initializes the events subsystem.
		/// </summary>
		GameController = 0x00002000,
		/// <summary>
		/// Events subsystem.
		/// </summary>
		Events =         0x00004000,
		/// <summary>
		/// Sensor subsystem.
		/// </summary>
		Sensor =         0x00008000,
		/// <summary>
		/// All of the subsystems.
		/// </summary>
		Everything = Timer | Audio | Video | Events | Joystick | Haptic | GameController | Sensor
	}

	public static class SDL2 {

		public static readonly LibrarySpec LibrarySpec = new() { Name = "SDL2" };

		private static Library library = null;
		public static Library Library {
			get {
				if (library == null) library = LibraryManager.Load(LibrarySpec);
				return library;
			}
		}

		private static SDLFunctions functions = null;
		public static SDLFunctions Functions {
			get {
				if (functions == null) {
					functions = new SDLFunctions();
					Library.LoadFunctions(functions);
				}
				return functions;
			}
		}

		static SDL2() {
			AppDomain.CurrentDomain.ProcessExit += (s, e) => Quit();
		}

		internal static string GetError() {
			string error = MemoryUtil.GetStringASCII(Functions.SDL_GetError());
			Functions.SDL_ClearError();
			return error;
		}

		internal static void CheckError(int ret) {
			if (ret != 0) throw new SDLException(GetError());
		}

		// SDL.h

		/// <summary>
		/// Initializes the SDL subsystems as specified by the flags.
		/// </summary>
		/// <seealso cref="InitSubSystem(SDLSubsystems)"/>
		/// <seealso cref="Quit"/>
		/// <param name="flags">Subsystems to initialize</param>
		public static void Init(SDLSubsystems flags) => CheckError(Functions.SDL_Init((uint)flags));

		/// <summary>
		/// <para>Initializes specific SDL subsystems.</para>
		/// <para>
		/// Subsystem initialization is ref-counted, you must call <see cref="QuitSubSystem(SDLSubsystems)"/> to correctly
		/// shutdown a subsystem manually (or call <see cref="Quit"/> to force shutdown). If a subsystem is already loaded then this call
		/// will increase the ref-count and return.
		/// </para>
		/// </summary>
		/// <seealso cref="Init(SDLSubsystems)"/>
		/// <seealso cref="QuitSubSystem(SDLSubsystems)"/>
		/// <seealso cref="Quit"/>
		/// <param name="flags">Subsystems to explicitly initialize</param>
		public static void InitSubSystem(SDLSubsystems flags) => CheckError(Functions.SDL_InitSubSystem((uint)flags));

		/// <summary>
		/// Cleans up specific SDL subsystems.
		/// </summary>
		/// <param name="flags"></param>
		public static void QuitSubSystem(SDLSubsystems flags) => Functions.SDL_QuitSubSystem((uint)flags);

		/// <summary>
		/// Cleans up all initialized subsystems. This is automatically called when the program exits.
		/// </summary>
		public static void Quit() => Functions.SDL_Quit();

		// SDL_pixels.h

		/// <summary>
		/// Fills the ramp array with a gamma ramp computed from a gamma value.
		/// </summary>
		/// <param name="gamma">The gamma value</param>
		/// <param name="ramp">Array to store computed gamma ramp in</param>
		public static void CalculateGammaRamp(float gamma, ushort[] ramp) => Functions.SDL_CalculateGammaRamp(gamma, ramp);

		// SDL_rwops.h

		// SDL_rect.h

		/// <summary>
		/// Computes the rectangle that contains an entire set of points.
		/// </summary>
		/// <param name="points">Points to test for enclosure</param>
		/// <param name="clip">Optional clipping rectangle to apply to points</param>
		/// <returns>Clipping rectagle, or null if no points are enclosed</returns>
		public static SDLRect? EnclosePoints(SDLPoint[] points, SDLRect? clip = null) => EnclosePoints(new Span<SDLPoint>(points), clip);

		/// <summary>
		/// Computes the rectangle that contains an entire set of points.
		/// </summary>
		/// <param name="points">Points to test for enclosure</param>
		/// <param name="clip">Optional clipping rectangle to apply to points</param>
		/// <returns>Clipping rectagle, or null if no points are enclosed</returns>
		public static SDLRect? EnclosePoints(Span<SDLPoint> points, SDLRect? clip = null) {
			unsafe {
				SDLRect cclip = clip.GetValueOrDefault();
				fixed (SDLPoint* pPoints = points) {
					if (Functions.SDL_EnclosePoints((IntPtr)pPoints, points.Length, clip.HasValue ? ((IntPtr)(&cclip)) : IntPtr.Zero, out SDLRect ret)) return ret;
					else return null;
				}
			}
		}

		/// <summary>
		/// Tests if a rectangle and line intersect, clamping the points defining the line to within the
		/// rectangle border if so.
		/// </summary>
		/// <param name="rect">Rectangle to test</param>
		/// <param name="x1">First line X coordinate</param>
		/// <param name="y1">First line Y coordinate</param>
		/// <param name="x2">Second line X coordinate</param>
		/// <param name="y2">Second line Y coordinate</param>
		/// <returns>If the rectangle and line intersect</returns>
		public static bool IntersectRectAndLine(in SDLRect rect, ref int x1, ref int y1, ref int x2, ref int y2) =>
			Functions.SDL_IntersectRectAndLine(rect, ref x1, ref y1, ref x2, ref y2);

		// SDL_blendmode.h

		/// <summary>
		/// Creates a custom blend mode to use with blitting operations based on a set of blend factors and operations.
		/// </summary>
		/// <param name="srcColorFactor">Source color factor</param>
		/// <param name="dstColorFactor">Destination color factor</param>
		/// <param name="colorOperation">Color blend operation</param>
		/// <param name="srcAlphaFactor">Source alpha factor</param>
		/// <param name="dstAlphaFactor">Destination alpha factor</param>
		/// <param name="alphaOperation">Alpha blend operation</param>
		/// <returns>Custom blend mode</returns>
		/// <seealso cref="SDLBlendMode"/>
		/// <seealso cref="SDLBlendFactor"/>
		/// <seealso cref="SDLBlendOperation"/>
		public static SDLBlendMode ComposeCustomBlendMode(SDLBlendFactor srcColorFactor, SDLBlendFactor dstColorFactor, SDLBlendOperation colorOperation, SDLBlendFactor srcAlphaFactor, SDLBlendFactor dstAlphaFactor, SDLBlendOperation alphaOperation) =>
			Functions.SDL_ComposeCustomBlendMode(srcColorFactor, dstColorFactor, colorOperation, srcAlphaFactor, dstAlphaFactor, alphaOperation);

		// SDL_surface.h

		/// <summary>
		/// Loads a BMP file from an SDL RWOps stream.
		/// </summary>
		/// <param name="rwops">RWOps stream to load from</param>
		/// <returns>Loaded surface</returns>
		/// <seealso cref="LoadBMP(SDLSpanRWOps)"/>
		public static SDLSurface LoadBMP(SDLRWOps rwops) {
			IntPtr pSurface = Functions.SDL_LoadBMP_RW(rwops.RWOps.Ptr, 0);
			if (pSurface != IntPtr.Zero) throw new SDLException(GetError());
			return new SDLSurface(new UnmanagedPointer<SDL_Surface>(pSurface));
		}

		/// <summary>
		/// Loads a BMP file from an SDL RWOps stream.
		/// </summary>
		/// <param name="rwops">RWOps stream to load from</param>
		/// <returns>Loaded surface</returns>
		/// <seealso cref="LoadBMP(SDLRWOps)"/>
		public static SDLSurface LoadBMP(SDLSpanRWOps rwops) {
			IntPtr pSurface = Functions.SDL_LoadBMP_RW(rwops.RWOps.Ptr, 0);
			if (pSurface != IntPtr.Zero) throw new SDLException(GetError());
			return new SDLSurface(new UnmanagedPointer<SDL_Surface>(pSurface));
		}

		/// <summary>
		/// Saves a surface as a BMP file to an SDL RWOps stream.
		/// </summary>
		/// <param name="surface">Surface to save</param>
		/// <param name="rwops">RWOps stream to save to</param>
		public static void SaveBMP(SDLSurface surface, SDLRWOps rwops) =>
			CheckError(Functions.SDL_SaveBMP_RW(surface.Surface.Ptr, rwops.RWOps.Ptr, 0));

		/// <summary>
		/// Saves a surface as a BMP file to an SDL RWOps stream.
		/// </summary>
		/// <param name="surface">Surface to save</param>
		/// <param name="rwops">RWOps stream to save to</param>
		public static void SaveBMP(SDLSurface surface, SDLSpanRWOps rwops) =>
			CheckError(Functions.SDL_SaveBMP_RW(surface.Surface.Ptr, rwops.RWOps.Ptr, 0));

		/// <summary>
		/// Converts pixel data from one format to another.
		/// </summary>
		/// <param name="width">Width of pixel surface</param>
		/// <param name="height">Height of pixel surface</param>
		/// <param name="srcFormat">Source pixel format</param>
		/// <param name="src">Source pixels</param>
		/// <param name="srcPitch">Source pixel pitch in bytes per row</param>
		/// <param name="dstFormat">Destination pixel format</param>
		/// <param name="dst">Destination pixels</param>
		/// <param name="dstPitch">Destination pixel pitch in bytes per row</param>
		/// <seealso cref="ConvertPixels(int, int, SDLPixelFormatEnum, Span{byte}, int, SDLPixelFormatEnum, Span{byte}, int)"/>
		public static void ConvertPixels(int width, int height, SDLPixelFormatEnum srcFormat, IntPtr src, int srcPitch, SDLPixelFormatEnum dstFormat, IntPtr dst, int dstPitch) =>
			CheckError(Functions.SDL_ConvertPixels(width, height, srcFormat, src, srcPitch, dstFormat, dst, dstPitch));

		/// <summary>
		/// Converts pixel data from one format to another.
		/// </summary>
		/// <param name="width">Width of pixel surface</param>
		/// <param name="height">Height of pixel surface</param>
		/// <param name="srcFormat">Source pixel format</param>
		/// <param name="src">Source pixels</param>
		/// <param name="srcPitch">Source pixel pitch in bytes per row</param>
		/// <param name="dstFormat">Destination pixel format</param>
		/// <param name="dst">Destination pixels</param>
		/// <param name="dstPitch">Destination pixel pitch in bytes per row</param>
		/// <seealso cref="ConvertPixels(int, int, SDLPixelFormatEnum, IntPtr, int, SDLPixelFormatEnum, IntPtr, int)"/>
		public static void ConvertPixels(int width, int height, SDLPixelFormatEnum srcFormat, Span<byte> src, int srcPitch, SDLPixelFormatEnum dstFormat, Span<byte> dst, int dstPitch) {
			unsafe {
				fixed(byte* pSrc = src) {
					fixed(byte* pDst = dst) {
						ConvertPixels(width, height, srcFormat, (IntPtr)pSrc, srcPitch, dstFormat, (IntPtr)pDst, dstPitch);
					}
				}
			}
		}

		/// <summary>
		/// The global conversion mode for YUV formats.
		/// </summary>
		public static SDLYUVConversionMode YUVConversionMode {
			get => Functions.SDL_GetYUVConversionMode();
			set => Functions.SDL_SetYUVConversionMode(value);
		}

		/// <summary>
		/// Gets the YUV conversion mode to use for a specific resolution.
		/// </summary>
		/// <param name="width">Resolution width</param>
		/// <param name="height">Resolution height</param>
		/// <returns>YUV conversion mode to use at resolution</returns>
		public static SDLYUVConversionMode GetConversionModeForResolution(int width, int height) => Functions.SDL_GetYUVConversionModeForResolution(width, height);

		// SDL_video.h

		/// <summary>
		/// Getter for all active displays.
		/// </summary>
		public static SDLDisplay[] Displays {
			get {
				int num = Functions.SDL_GetNumVideoDisplays();
				if (num < 0) throw new SDLException(GetError());
				return LINQ.Seq(num).Select(index => new SDLDisplay(index)).ToArray();
			}
		}

		/// <summary>
		/// Window position incdicating that the window should be positioned wherever the windowing system wants.
		/// </summary>
		public const int WindowPosUndefined = 0x1FFF0000;

		/// <summary>
		/// Window position indicating that the window should be centered wherever the windowing system decides.
		/// </summary>
		public const int WindowPosCentered = 0x2FFF0000;

		/// <summary>
		/// Creates a window position indicating the window should be positioned wherever the windowing system wants on the given display.
		/// </summary>
		/// <param name="display">Display to position on</param>
		/// <returns>Window position</returns>
		public static int WindowPosUndefinedOnDisplay(SDLDisplay display) => 0x1FFF0000 | display.DisplayIndex;

		/// <summary>
		/// Creates a window position indicating the window should be centered on the given display.
		/// </summary>
		/// <param name="display">Display to center on</param>
		/// <returns>Window position</returns>
		public static int WindowPosCenteredOnDisplay(SDLDisplay display) => 0x2FFF0000 | display.DisplayIndex;

		/// <summary>
		/// The list of video drivers compiled into the SDL library.
		/// </summary>
		public static string[] VideoDrivers {
			get {
				string[] drivers = new string[Functions.SDL_GetNumVideoDrivers()];
				for (int i = 0; i < drivers.Length; i++) drivers[i] = MemoryUtil.GetStringASCII(Functions.SDL_GetVideoDriver(i));
				return drivers;
			}
		}

		/// <summary>
		/// Initializes the video subsystem, optionally specifying a video driver.
		/// </summary>
		/// <param name="driverName">Video driver name, or null for default</param>
		public static void VideoInit(string driverName) => CheckError(Functions.SDL_VideoInit(driverName));

		/// <summary>
		/// Shuts down the video subsystem.
		/// </summary>
		public static void VideoQuit() => Functions.SDL_VideoQuit();

		/// <summary>
		/// The current video driver.
		/// </summary>
		public static string CurrentVideoDriver => MemoryUtil.GetStringASCII(Functions.SDL_GetCurrentVideoDriver());

		/// <summary>
		/// Gets a window from a stored ID, or null if it doesn't exist.
		/// </summary>
		/// <param name="id">Window ID</param>
		/// <returns>Window with ID, or null</returns>
		public static SDLWindow GetWindowFromID(uint id) {
			IntPtr ptr = Functions.SDL_GetWindowFromID(id);
			return ptr == IntPtr.Zero ? null : new SDLWindow((IPointer<SDL_Window>)new UnmanagedPointer<SDL_Window>(ptr));
		}

		/// <summary>
		/// Gets the window that currently has grabbed input.
		/// </summary>
		/// <returns>Window with grabbed input or null</returns>
		public static SDLWindow GetGrabbedWindow() {
			IntPtr ptr = Functions.SDL_GetGrabbedWindow();
			return ptr == IntPtr.Zero ? null : new SDLWindow((IPointer<SDL_Window>)new UnmanagedPointer<SDL_Window>(ptr));
		}

		// SDL_keyboard.h

		public static SDLWindow GetKeyboardFocus() {
			IntPtr ptr = Functions.SDL_GetKeyboardFocus();
			return ptr == IntPtr.Zero ? null : new SDLWindow((IPointer<SDL_Window>)new UnmanagedPointer<SDL_Window>(ptr));
		}

		public static ReadOnlySpan<SDLButtonState> GetKeyboardState() {
			IntPtr ptr = Functions.SDL_GetKeyboardState(out int numkeys);
			unsafe {
				return new ReadOnlySpan<SDLButtonState>((void*)ptr, numkeys);
			}
		}

		public static SDLKeymod ModState {
			get => Functions.SDL_GetModState();
			set => Functions.SDL_SetModState(value);
		}

		public static SDLKeycode GetKeyFromScancode(SDLScancode scancode) => Functions.SDL_GetKeyFromScancode(scancode);

		public static SDLScancode GetScancodeFromKey(SDLKeycode key) => Functions.SDL_GetScancodeFromKey(key);

		public static string GetScancodeName(SDLScancode scancode) => MemoryUtil.GetStringASCII(Functions.SDL_GetScancodeName(scancode));

		public static SDLScancode GetScancodeFromName(string name) => Functions.SDL_GetScancodeFromName(name);

		public static string GetKeyName(SDLKeycode key) => MemoryUtil.GetStringASCII(Functions.SDL_GetKeyName(key));

		public static SDLKeycode GetKeyFromName(string name) => Functions.SDL_GetKeyFromName(name);

		public static void StartTextInput() => Functions.SDL_StartTextInput();

		public static bool IsTextInputActive => Functions.SDL_IsTextInputActive();

		public static void StopTextInput() => Functions.SDL_StopTextInput();

		public static void SetTextInputRect(in SDLRect rect) => Functions.SDL_SetTextInputRect(rect);

		public static bool HasScreenKeyboardSupport => Functions.SDL_HasScreenKeyboardSupport();

		// SDL_mouse.h

		public static SDLMouseButtonState MakeMouseButton(int index) => (SDLMouseButtonState)(1 << (index - 1));

		public static SDLWindow GetMouseFocus() {
			IntPtr ptr = Functions.SDL_GetMouseFocus();
			return ptr == IntPtr.Zero ? null : new SDLWindow((IPointer<SDL_Window>)new UnmanagedPointer<SDL_Window>(ptr));
		}

		public static SDLMouseButtonState GetMouseState(out int x, out int y) => (SDLMouseButtonState)Functions.SDL_GetMouseState(out x, out y);

		public static SDLMouseButtonState GetGlobalMouseState(out int x, out int y) => (SDLMouseButtonState)Functions.SDL_GetGlobalMouseState(out x, out y);

		public static SDLMouseButtonState GetRelativeMouseState(out int x, out int y) => (SDLMouseButtonState)Functions.SDL_GetRelativeMouseState(out x, out y);

		public static void WarpMouseGlobal(int x, int y) => CheckError(Functions.SDL_WarpMouseGlobal(x, y));

		public static bool RelativeMouseMode {
			get => Functions.SDL_GetRelativeMouseMode();
			set => CheckError(Functions.SDL_SetRelativeMouseMode(value));
		}

		public static bool CaptureMouse {
			set => CheckError(Functions.SDL_CaptureMouse(value));
		}

		public static SDLCursor Cursor {
			get => new(new UnmanagedPointer<SDL_Cursor>(Functions.SDL_GetCursor()));
			set => Functions.SDL_SetCursor(value.Cursor.Ptr);
		}

		public static bool ShowCursor {
			get => Functions.SDL_ShowCursor(-1) == 1;
			set => Functions.SDL_ShowCursor(value ? 1 : 0);
		}

		// SDL_joystick.h

		public const short JoystickAxisMax = short.MaxValue;
		public const short JoystickAxisMin = short.MinValue;

		public const uint TouchMouseID = 0xFFFFFFFF;
		public const long MouseTouchID = -1;

		public static void LockJoysticks() => Functions.SDL_LockJoysticks();

		public static void UnlockJoysticks() => Functions.SDL_UnlockJoysticks();

		public static SDLJoystickDevice[] Joysticks {
			get {
				SDLJoystickDevice[] joysticks = new SDLJoystickDevice[Functions.SDL_NumJoysticks()];
				for (int i = 0; i < joysticks.Length; i++) joysticks[i] = new SDLJoystickDevice() { DeviceIndex = i };
				return joysticks;
			}
		}

		public static SDLJoystickDevice JoystickAttachVirtual(SDLJoystickType type, int naxes, int nbuttons, int nhats) {
			int dev = Functions.SDL_JoystickAttachVirtual(type, naxes, nbuttons, nhats);
			if (dev == -1) throw new SDLException(GetError());
			return new SDLJoystickDevice() { DeviceIndex = dev };
		}

		public static void JoystickUpdate() => Functions.SDL_JoystickUpdate();

		public static bool JoystickEventState {
			get => Functions.SDL_JoystickEventState(-1) == 1;
			set => Functions.SDL_JoystickEventState(value ? 1 : 0);
		}

		// SDL_touch.h

		public static SDLTouchDevice[] TouchDevices {
			get {
				SDLTouchDevice[] devices = new SDLTouchDevice[Functions.SDL_GetNumTouchDevices()];
				for (int i = 0; i < devices.Length; i++) devices[i] = new SDLTouchDevice() { DeviceIndex = i };
				return devices;
			}
		}

		// SDL_events.h

		public static void PumpEvents() => Functions.SDL_PumpEvents();

		public static Span<SDLEvent> PeepEvents(Span<SDLEvent> events, int numevents, SDLEventAction action, uint minType = 0, uint maxType = uint.MaxValue) {
			unsafe {
				fixed(SDLEvent* pEvents = events) {
					Functions.SDL_PeepEvents((IntPtr)pEvents, numevents, action, minType, maxType);
				}
			}
			return events;
		}

		public static bool HasEvent(SDLEventType type) => Functions.SDL_HasEvent((uint)type);

		public static bool HasEvents(uint minType = 0, uint maxType = uint.MaxValue) => Functions.SDL_HasEvents(minType, maxType);

		public static void FlushEvent(SDLEventType type) => Functions.SDL_FlushEvent((uint)type);

		public static void FlushEvents(uint minType = 0, uint maxType = uint.MaxValue) => Functions.SDL_FlushEvents(minType, maxType);

		public static SDLEvent? PollEvent() {
			if (Functions.SDL_PollEvent(out SDLEvent evt) == 1) return evt;
			else return null;
		}

		public static SDLEvent WaitEvent() {
			if (Functions.SDL_WaitEvent(out SDLEvent evt) != 0) throw new SDLException(GetError());
			return evt;
		}

		public static SDLEvent? WaitEventTimeout(int timeout) {
			// SDL doesn't actually tell us if WaitEventTimeout succeeds or timed out, so a sentry event type is used to detect this
			SDLEvent evt = new() { Type = SDLEventType.FirstEvent };
			if (Functions.SDL_WaitEventTimeout(ref evt, timeout) != 0) throw new SDLException(GetError());
			return evt.Type == SDLEventType.FirstEvent ? null : evt;
		}

		public static void PushEvent(SDLEvent evt) => Functions.SDL_PushEvent(evt);

		public static void SetEventFilter(SDLEventFilter filter, IntPtr userdata = default) => Functions.SDL_SetEventFilter(filter, userdata);

		public static bool GetEventFilter(out SDLEventFilter filter, out IntPtr userdata) {
			if (!Functions.SDL_GetEventFilter(out IntPtr pFilter, out userdata)) {
				filter = null;
				return false;
			} else {
				filter = Marshal.GetDelegateForFunctionPointer<SDLEventFilter>(pFilter);
				return true;
			}
		}

		public static void AddEventWatch(SDLEventFilter filter, IntPtr userdata = default) => Functions.SDL_AddEventWatch(filter, userdata);

		public static void DelEventWatch(SDLEventFilter filter, IntPtr userdata = default) => Functions.SDL_DelEventWatch(filter, userdata);

		public static void FilterEvents(SDLEventFilter filter, IntPtr userdata = default) => Functions.SDL_FilterEvents(filter, userdata);

	}
}
