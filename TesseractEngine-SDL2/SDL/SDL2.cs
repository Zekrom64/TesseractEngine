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

		internal static int CheckError(int ret) {
			if (ret < 0) throw new SDLException(GetError());
			return ret;
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
				fixed (byte* pSrc = src) {
					fixed (byte* pDst = dst) {
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

		public const int ButtonLeft = 1;
		public const int ButtonMiddle = 2;
		public const int ButtonRight = 3;
		public const int ButtonX1 = 4;
		public const int ButtonX2 = 5;

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

		// SDL_gamecontroller.h

		public static int GameControllerAddMappingsFromRW(SDLRWOps rwops) {
			int n = Functions.SDL_GameControllerAddMappingsFromRW(rwops.RWOps.Ptr, 0);
			if (n < 0) throw new SDLException(GetError());
			return n;
		}

		public static int GameControllerAddMappingsFromRW(SDLSpanRWOps rwops) {
			int n = Functions.SDL_GameControllerAddMappingsFromRW(rwops.RWOps.Ptr, 0);
			if (n < 0) throw new SDLException(GetError());
			return n;
		}

		public static bool GameControllerAddMapping(string mapping) {
			int n = Functions.SDL_GameControllerAddMapping(mapping);
			if (n < 0) throw new SDLException(GetError());
			return n == 1;
		}

		public static SDLGameControllerMapping[] Mappings {
			get {
				int n = Functions.SDL_GameControllerNumMappings();
				SDLGameControllerMapping[] mappings = new SDLGameControllerMapping[n];
				for (int i = 0; i < n; i++) mappings[i] = new SDLGameControllerMapping() { MappingIndex = i };
				return mappings;
			}
		}

		public static string GameControllerMappingForGUID(Guid guid) {
			IntPtr pMapping = Functions.SDL_GameControllerMappingForGUID(guid);
			string mapping = MemoryUtil.GetStringASCII(pMapping);
			Functions.SDL_free(pMapping);
			return mapping;
		}

		public static void GameControllerUpdate() => Functions.SDL_GameControllerUpdate();

		public static SDLGameControllerAxis GameControllerGetAxisFromString(string str) => Functions.SDL_GameControllerGetAxisFromString(str);

		public static string GameControllerGetStringForAxis(SDLGameControllerAxis axis) => MemoryUtil.GetStringASCII(Functions.SDL_GameControllerGetStringForAxis(axis));

		public static SDLGameControllerButton GameControllerGetButtonFromString(string str) => Functions.SDL_GameControllerGetButtonFromString(str);

		public static string GameControllerGetStringForButton(SDLGameControllerButton button) => MemoryUtil.GetStringASCII(Functions.SDL_GameControllerGetStringForButton(button));

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
				fixed (SDLEvent* pEvents = events) {
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

		// SDL_cpuinfo.h

		public static int CPUCount => Functions.SDL_GetCPUCount();

		public static int CPUCacheLineSize => Functions.SDL_GetCPUCacheLineSize();

		public static bool HasRDTSC => Functions.SDL_HasRDTSC();
		public static bool HasAltiVec => Functions.SDL_HasAltiVec();
		public static bool HasMMX => Functions.SDL_HasMMX();
		public static bool Has3DNow => Functions.SDL_Has3DNow();
		public static bool HasSSE => Functions.SDL_HasSSE();
		public static bool HasSSE2 => Functions.SDL_HasSSE2();
		public static bool HasSSE3 => Functions.SDL_HasSSE3();
		public static bool HasSSE41 => Functions.SDL_HasSSE41();
		public static bool HasSSE42 => Functions.SDL_HasSSE42();
		public static bool HasAVX => Functions.SDL_HasAVX();
		public static bool HasAVX2 => Functions.SDL_HasAVX2();
		public static bool HasAVX512F => Functions.SDL_HasAVX512F();
		public static bool HasARMSIMD => Functions.SDL_HasARMSIMD();
		public static bool HasNEON => Functions.SDL_HasNEON();

		public static int SystemRAM => Functions.SDL_GetSystemRAM();

		public static nuint SIMDAlignment => Functions.SDL_SIMDGetAlignment();

		public static IntPtr SIMDAlloc(nuint len) => Functions.SDL_SIMDAlloc(len);

		public static IntPtr SIMDRealloc(IntPtr ptr, nuint len) => Functions.SDL_SIMDRealloc(ptr, len);

		public static void SIMDFree(IntPtr ptr) => Functions.SDL_SIMDFree(ptr);

		// SDL_audio.h

		public const int MixMaxVolume = 128;

		public static string[] AudioDrivers {
			get {
				int numDrivers = Functions.SDL_GetNumAudioDrivers();
				string[] drivers = new string[numDrivers];
				for (int i = 0; i < numDrivers; i++) drivers[i] = MemoryUtil.GetStringASCII(Functions.SDL_GetAudioDriver(i));
				return drivers;
			}
		}

		public static void AudioInit(string driverName) => CheckError(Functions.SDL_AudioInit(driverName));

		public static void AudioQuit() => Functions.SDL_AudioQuit();

		public static string CurrentAudioDriver => MemoryUtil.GetStringASCII(Functions.SDL_GetCurrentAudioDriver());

		public static void OpenAudio(in SDLAudioSpec desired, out SDLAudioSpec obtained) => CheckError(Functions.SDL_OpenAudio(desired, out obtained));

		public static string[] AudioDevices {
			get {
				int numDevices = Functions.SDL_GetNumAudioDevices(0);
				string[] devices = new string[numDevices];
				for (int i = 0; i < numDevices; i++) devices[i] = MemoryUtil.GetStringASCII(Functions.SDL_GetAudioDeviceName(i, 0));
				return devices;
			}
		}

		public static string[] AudioCaptureDevices {
			get {
				int numDevices = Functions.SDL_GetNumAudioDevices(1);
				string[] devices = new string[numDevices];
				for (int i = 0; i < numDevices; i++) devices[i] = MemoryUtil.GetStringASCII(Functions.SDL_GetAudioDeviceName(i, 1));
				return devices;
			}
		}

		public static void PauseAudio(bool pauseOn) => Functions.SDL_PauseAudio(pauseOn ? 1 : 0);

		public static (SDLAudioSpec, ManagedPointer<byte>, uint) LoadWAV(SDLRWOps rwops) {
			IntPtr pSpec = Functions.SDL_LoadWAV_RW(rwops.RWOps.Ptr, 0, out SDLAudioSpec spec, out IntPtr buf, out uint len);
			if (pSpec == IntPtr.Zero) throw new SDLException(GetError());
			return (spec, new ManagedPointer<byte>(buf, ptr => Functions.SDL_FreeWAV(ptr)), len);
		}

		public static Span<byte> MixAudio(Span<byte> dst, in ReadOnlySpan<byte> src, int volume) {
			unsafe {
				fixed(byte* pDst = dst) {
					fixed(byte* pSrc = src) {
						Functions.SDL_MixAudio((IntPtr)pDst, (IntPtr)pSrc, (uint)Math.Min(dst.Length, src.Length), volume);
					}
				}
			}
			return dst;
		}

		public static IPointer<byte> MixAudio(IPointer<byte> dst, IConstPointer<byte> src, uint length, int volume) {
			Functions.SDL_MixAudio(dst.Ptr, src.Ptr, length, volume);
			return dst;
		}

		public static Span<byte> MixAudio(Span<byte> dst, in ReadOnlySpan<byte> src, SDLAudioFormat format, int volume) {
			unsafe {
				fixed (byte* pDst = dst) {
					fixed (byte* pSrc = src) {
						Functions.SDL_MixAudioFormat((IntPtr)pDst, (IntPtr)pSrc, format, (uint)Math.Min(dst.Length, src.Length), volume);
					}
				}
			}
			return dst;
		}

		public static IPointer<byte> MixAudio(IPointer<byte> dst, IConstPointer<byte> src, SDLAudioFormat format, uint length, int volume) {
			Functions.SDL_MixAudioFormat(dst.Ptr, src.Ptr, format, length, volume);
			return dst;
		}

		public static void LockAudio() => Functions.SDL_LockAudio();

		public static void UnlockAudio() => Functions.SDL_UnlockAudio();

		public static void CloseAudio() => Functions.SDL_CloseAudio();

		// SDL_clipboard.h

		public static string ClipboardText {
			get {
				if (!Functions.SDL_HasClipboardText()) return null;
				return MemoryUtil.GetStringASCII(Functions.SDL_GetClipboardText());
			}
			set => Functions.SDL_SetClipboardText(value);
		}

		// SDL_filesystem.h

		public static string BasePath => MemoryUtil.GetStringASCII(Functions.SDL_GetBasePath());

		public static string GetPrefPath(string org, string app) => MemoryUtil.GetStringASCII(Functions.SDL_GetPrefPath(org, app));

		// SDL_gesture.h

		public static void RecordGesture() => Functions.SDL_RecordGesture(-1);

		public static void SaveAllDollarTemplates(SDLRWOps rwops) => CheckError(Functions.SDL_SaveAllDollarTemplates(rwops.RWOps.Ptr));

		public static void SaveAllDollarTemplates(SDLSpanRWOps rwops) => CheckError(Functions.SDL_SaveAllDollarTemplates(rwops.RWOps.Ptr));

		public static void SaveDollarTemplate(long gestureID, SDLRWOps rwops) => CheckError(Functions.SDL_SaveDollarTemplate(gestureID, rwops.RWOps.Ptr));

		public static void SaveDollarTemplate(long gestureID, SDLSpanRWOps rwops) => CheckError(Functions.SDL_SaveDollarTemplate(gestureID, rwops.RWOps.Ptr));

		// SDL_haptic.h

		public const uint HapticInfinity = 4294967295U;

		public static bool MouseIsHaptic => Functions.SDL_MouseIsHaptic() != 0;

		public static SDLHaptic HapticOpenFromMouse() {
			IntPtr haptic = Functions.SDL_HapticOpenFromMouse();
			if (haptic == IntPtr.Zero) throw new SDLException(GetError());
			return new SDLHaptic(haptic);
		}

		// SDL_hints.h

		public const string HintFramebufferAcceleration = "SDL_FRAMEBUFFER_ACCELERATION";
		public const string HintRenderDriver = "SDL_RENDER_DRIVER";
		public const string HintRenderOpenGLShaders = "SDL_RENDER_OPENGL_SHADERS";
		public const string HintRenderDirect3DThreadsafe = "SDL_RENDER_DIRECT3D_THREADSAFE";
		public const string HintRenderDirect3D11Debug = "SDL_RENDER_DIRECT3D11_DEBUG";
		public const string HintRenderLogicalSizeMode = "SDL_RENDER_LOGICAL_SIZE_MODE";
		public const string HintRenderScaleQuality = "SDL_RENDER_SCALE_QUALITY";
		public const string HintRenderVSync = "SDL_RENDER_VSYNC";
		public const string HintVideoAllowScreensaver = "SDL_VIDEO_ALLOW_SCREENSAVER";
		public const string HintVideoExternalContext = "SDL_VIDEO_EXTERNAL_CONTEXT";
		public const string HintVideoX11XVidmode = "SDL_VIDEO_X11_XVIDMODE";
		public const string HintVideoX11XInerama = "SDL_VIDEO_X11_XINERAMA";
		public const string HintVideoX11XRandr = "SDL_VIDEO_X11_XRANDR";
		public const string HintVideoX11WindowVisualID = "SDL_VIDEO_X11_WINDOW_VISUAL_ID";
		public const string HintVideoX11NetWMPing = "SDL_VIDEO_X11_NET_WM_PING";
		public const string HintVideoX11NetWMBypassCompositor = "SDL_VIDEO_X11_NET_WM_BYPASS_COMPOSITOR";
		public const string HintVideoX11ForceEGL = "SDL_VIDEO_X11_FORCE_EGL";
		public const string HintWindowFrameUsableWhileCursorHidden = "SDL_WINDOW_FRAME_USABLE_WHILE_CURSOR_HIDDEN";
		public const string HintWindowsIntResourceIcon = "SDL_WINDOWS_INTRESOURCE_ICON";
		public const string HintWindowsIntResourceIconSmall = "SDL_WINDOWS_INTRESOURCE_ICON_SMALL";
		public const string HintWindowsEnableMessageLoop = "SDL_WINDOWS_ENABLE_MESSAGELOOP";
		public const string HintGrabKeyboard = "SDL_GRAB_KEYBOARD";
		public const string HintMouseDoubleClickTime = "SDL_MOUSE_DOUBLE_CLICK_TIME";
		public const string HintMouseDoubleClickRadius = "SDL_MOUSE_DOUBLE_CLICK_RADIUS";
		public const string HintMouseNormalSpeedScale = "SDL_MOUSE_NORMAL_SPEED_SCALE";
		public const string HintMouseRelativeSpeedScale = "SDL_MOUSE_RELATIVE_SPEED_SCALE";
		public const string HintMouseRelativeScaling = "SDL_MOUSE_RELATIVE_SCALING";
		public const string HintMouseRelativeModeWarp = "SDL_MOUSE_RELATIVE_MODE_WARP";
		public const string HintMouseFocusClickthrough = "SDL_MOUSE_FOCUS_CLICKTHROUGH";
		public const string HintTouchMouseEvents = "SDL_TOUCH_MOUSE_EVENTS";
		public const string HintMouseTouchEvents = "SDL_MOUSE_TOUCH_EVENTS";
		public const string HintVideoMinimizeOnFocusLoss = "SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS";
		public const string HintIdleTimerDisabled = "SDL_IOS_IDLE_TIMER_DISABLED";
		public const string HintOrientations = "SDL_IOS_ORIENTATIONS";
		public const string HintAppleTVControllerUIEvents = "SDL_APPLE_TV_CONTROLLER_UI_EVENTS";
		public const string HintAppleTVRemoteAllowOrientation = "SDL_APPLE_TV_REMOTE_ALLOW_ORIENTATION";
		public const string HintIOSHideHomeIndicator = "SDL_IOS_HIDE_HOME_INDICATOR";
		public const string HintAccelerometerAsJoystick = "SDL_ACCELEROMETER_AS_JOYSTICK";
		public const string HintTVRemoteAsJoystick = "SDL_TV_REMOTE_AS_JOYSTICK";
		public const string HintXInputEnabled = "SDL_XINPUT_ENABLED";
		public const string HintXInputUseOldJoystickMapping = "SDL_XINPUT_USE_OLD_JOYSTICK_MAPPING";
		public const string HintGameControllerType = "SDL_GAMECONTROLLERTYPE";
		public const string HintGameControllerConfig = "SDL_GAMECONTROLLERCONFIG";
		public const string HintGameControllerConfigFile = "SDL_GAMECONTROLLERCONFIG_FILE";
		public const string HintGameControllerIgnoreDevices = "SDL_GAMECONTROLLER_IGNORE_DEVICES";
		public const string HintGameControllerIngoreDevicesExcept = "SDL_GAMECONTROLLER_IGNORE_DEVICES_EXCEPT";
		public const string HintGameControllerUseButtonLabels = "SDL_GAMECONTROLLER_USE_BUTTON_LABELS";
		public const string HintJoystickAllowBackgroundEvents = "SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS";
		public const string HintJoystickHIDAPI = "SDL_JOYSTICK_HIDAPI";
		public const string HintJoystickHIDAPIPS4 = "SDL_JOYSTICK_HIDAPI_PS4";
		public const string HintJoystickHIDAPIPS5 = "SDL_JOYSTICK_HIDAPI_PS5";
		public const string HintJoystickHIDAPIPS4Rumble = "SDL_JOYSTICK_HIDAPI_PS4_RUMBLE";
		public const string HintJoystickHIDAPISteam = "SDL_JOYSTICK_HIDAPI_STEAM";
		public const string HintJoystickHIDAPISwitch = "SDL_JOYSTICK_HIDAPI_SWITCH";
		public const string HintJoystickHIDAPIXBox = "SDL_JOYSTICK_HIDAPI_XBOX";
		public const string HintJoystickHIDAPICorrelateXInput = "SDL_JOYSTICK_HIDAPI_CORRELATE_XINPUT";
		public const string HintJoystickHIDAPIGamecube = "SDL_JOYSTICK_HIDAPI_GAMECUBE";
		public const string HintEnableSteamControllers = "SDL_ENABLE_STEAM_CONTROLLERS";
		public const string HintJoystickRawInput = "SDL_JOYSTICK_RAWINPUT";
		public const string HintJoystickThread = "SDL_JOYSTICK_THREAD";
		public const string HintLinuxJoystickDeadZones = "SDL_LINUX_JOYSTICK_DEADZONES";
		public const string HintAllowTopmost = "SDL_ALLOW_TOPMOST";
		public const string HintTimerResolution = "SDL_TIMER_RESOLUTION";
		public const string HintQTWaylandContentOrientation = "SDL_QTWAYLAND_CONTENT_ORIENTATION";
		public const string HintQTWaylandWindowFlags = "SDL_QTWAYLAND_WINDOW_FLAGS";
		public const string HintThreadStackSize = "SDL_THREAD_STACK_SIZE";
		public const string HintThreadPriorityPolicy = "SDL_THREAD_PRIORITY_POLICY";
		public const string HintThreadForceRealtimeTimeCritical = "SDL_THREAD_FORCE_REALTIME_TIME_CRITICAL";
		public const string HintVideoHighDPIDisabled = "SDL_VIDEO_HIGHDPI_DISABLED";
		public const string HintMacCtrlClickEmulateRightClick = "SDL_MAC_CTRL_CLICK_EMULATE_RIGHT_CLICK";
		public const string HintVideoWinD3DCompiler = "SDL_VIDEO_WIN_D3DCOMPILER";
		public const string HintVideoWindowSharePixelFormat = "SDL_VIDEO_WINDOW_SHARE_PIXEL_FORMAT";
		public const string HintWinRTPrivacyPolicyURL = "SDL_WINRT_PRIVACY_POLICY_URL";
		public const string HintWinRTPrivacyPolicyLabel = "SDL_WINRT_PRIVACY_POLICY_LABEL";
		public const string HintWinRTHandleBackButton = "SDL_WINRT_HANDLE_BACK_BUTTON";
		public const string HintVideoMacHandleSpaces = "SDL_VIDEO_MAC_HANDLE_SPACES";
		public const string HintMacBackgroundApp = "SDL_MAC_BACKGROUND_APP";
		public const string HintAndroidAPKExpansionMainFileVersion = "SDL_ANDROID_APK_EXPANSION_MAIN_FILE_VERSION";
		public const string HintAndroidAPKExpansionPatchFileVersion = "SDL_ANDROID_APK_EXPANSION_PATCH_FILE_VERSION";
		public const string HintIMEInternalEditing = "SDL_IME_INTERNAL_EDITING";
		public const string HintAndroidTrapBackButton = "SDL_ANDROID_TRAP_BACK_BUTTON";
		public const string HintAndroidBlockOnPause = "SDL_ANDROID_BLOCK_ON_PAUSE";
		public const string HintAndroidBlockOnPauseAudio = "SDL_ANDROID_BLOCK_ON_PAUSEAUDIO";
		public const string HintReturnKeyHidesIME = "SDL_RETURN_KEY_HIDES_IME";
		public const string HintEmscriptenKeyboardElement = "SDL_EMSCRIPTEN_KEYBOARD_ELEMENT";
		public const string HintEmscriptenAsyncify = "SDL_EMSCRIPTEN_ASYNCIFY";
		public const string HintNoSignalHandlers = "SDL_NO_SIGNAL_HANDLERS";
		public const string HindWindowsNoCloseOnAltF4 = "SDL_WINDOWS_NO_CLOSE_ON_ALT_F4";
		public const string HintBMPSaveLegacyFormat = "SDL_BMP_SAVE_LEGACY_FORMAT";
		public const string HintWindowsDisableThreadNaming = "SDL_WINDOWS_DISABLE_THREAD_NAMING";
		public const string HintRPIVideoLayer = "SDL_RPI_VIDEO_LAYER";
		public const string HintVideoDoubleBuffer = "SDL_VIDEO_DOUBLE_BUFFER";
		public const string HintOpenGLESDriver = "SDL_OPENGL_ES_DRIVER";
		public const string HintAudioResamplingMode = "SDL_AUDIO_RESAMPLING_MODE";
		public const string HintAudioCategory = "SDL_AUDIO_CATEGORY";
		public const string HintRenderBatching = "SDL_RENDER_BATCHING";
		public const string HintAutoUpdateJoysticks = "SDL_AUTO_UPDATE_JOYSTICKS";
		public const string HintAutoUpdateSensors = "SDL_AUTO_UPDATE_SENSORS";
		public const string HintEventLogging = "SDL_EVENT_LOGGING";
		public const string HintWaveRIFFChunkSize = "SDL_WAVE_RIFF_CHUNK_SIZE";
		public const string HintWaveTruncation = "SDL_WAVE_TRUNCATION";
		public const string HintWaveFactChunk = "SDL_WAVE_FACT_CHUNK";
		public const string HintDisplayUsableBounds = "SDL_DISPLAY_USABLE_BOUNDS";
		public const string HintAudioDeviceAppName = "SDL_AUDIO_DEVICE_APP_NAME";
		public const string HintAudioDeviceStreamName = "SDL_AUDIO_DEVICE_STREAM_NAME";
		public const string HintPreferredLocales = "SDL_PREFERRED_LOCALES";

		public static void SetHint(string hint, string value, SDLHintPriority priority) => Functions.SDL_SetHintWithPriority(hint, value, priority);

		public static void SetHint(string hint, string value) => Functions.SDL_SetHint(hint, value);

		public static string GetHint(string hint) => MemoryUtil.GetStringASCII(Functions.SDL_GetHint(hint));

		public static bool GetHintBoolean(string hint, bool defaultValue = false) => Functions.SDL_GetHintBoolean(hint, defaultValue);

		public static void AddHintCallback(string hint, SDLHintCallback callback, IntPtr userdata) => Functions.SDL_AddHintCallback(hint, callback, userdata);

		public static void DelHintCallback(string hint, SDLHintCallback callback, IntPtr userdata) => Functions.SDL_DelHintCallback(hint, callback, userdata);

		// SDL_locale.h

		public static SDLLocale[] PreferredLocales {
			get {
				IntPtr ptrLocales = Functions.SDL_GetPreferredLocales();
				if (ptrLocales == IntPtr.Zero) throw new SDLException(GetError());
				List<SDLLocale> locales = new();
				UnmanagedPointer<SDLLocale> pLocales = new(ptrLocales);
				SDLLocale locale;
				do {
					locale = pLocales.Value;
					if (locale.Language != null) locales.Add(locale);
				} while (locale.Language != null);
				return locales.ToArray();
			}
		}

		// SDL_log.h

		public static void LogSetAllPriority(SDLLogPriority priority) => Functions.SDL_LogSetAllPriority(priority);

		public static void LogSetPriority(int category, SDLLogPriority priority) => Functions.SDL_LogSetPriority(category, priority);

		public static SDLLogPriority LogGetPriority(int category) => Functions.SDL_LogGetPriority(category);

		public static void LogResetPriorities() => Functions.SDL_LogResetPriorities();

		public static void Log(string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_Log("%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogVerbose(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogVerbose(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogDebug(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogDebug(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogInfo(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogInfo(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogWarn(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogWarn(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogError(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogError(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogCritical(int category, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogCritical(category, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static void LogMessage(int category, SDLLogPriority priority, string msg) {
			IntPtr pMsg = Marshal.StringToHGlobalAnsi(msg);
			Functions.SDL_LogMessage(category, priority, "%s", pMsg);
			Marshal.FreeHGlobal(pMsg);
		}

		public static (SDLLogOutputFunction, IntPtr) LogOutputFunction {
			get {
				Functions.SDL_LogGetOutputFunction(out IntPtr pCallback, out IntPtr userdata);
				SDLLogOutputFunction callback = null;
				if (pCallback != IntPtr.Zero) callback = Marshal.GetDelegateForFunctionPointer<SDLLogOutputFunction>(pCallback);
				return (callback, userdata);
			}
			set => Functions.SDL_LogSetOutputFunction(value.Item1, value.Item2);
		}

		// SDL_messagebox.h

		public static int ShowMessageBox(SDLMessageBoxData data) {
			ManagedPointer<SDLMessageBoxButtonData> buttons = default;
			if (data.Buttons != null) buttons = new ManagedPointer<SDLMessageBoxButtonData>(data.Buttons);
			ManagedPointer<SDLMessageBoxColorScheme> colorScheme = default;
			if (data.ColorScheme != null) colorScheme = new ManagedPointer<SDLMessageBoxColorScheme>(data.ColorScheme.Value);
			SDL_MessageBoxData mbdata = new() {
				Flags = data.Flags,
				Window = data != null ? data.Window.Window.Ptr : IntPtr.Zero,
				Title = data.Title,
				Message = data.Message,
				NumButtons = buttons.Count,
				Buttons = buttons.Ptr,
				ColorScheme = colorScheme.Ptr
			};
			int ret = Functions.SDL_ShowMessageBox(mbdata, out int buttonID);
			if (mbdata.Buttons != IntPtr.Zero) buttons.Dispose();
			if (mbdata.ColorScheme != IntPtr.Zero) colorScheme.Dispose();
			CheckError(ret);
			return buttonID;
		}

		public static void ShowSimpleMessageBox(SDLMessageBoxFlags flags, string title, string message, SDLWindow window = null) =>
			CheckError(Functions.SDL_ShowSimpleMessageBox(flags, title, message, window != null ? window.Window.Ptr : IntPtr.Zero));

		// SDL_metal.h

		public static void MetalDestroyView(IntPtr view) => Functions.SDL_Metal_DestroyView(view);

		public static IntPtr MetalGetLayer(IntPtr view) => Functions.SDL_Metal_GetLayer(view);

		// SDL_misc.h

		public static void OpenURL(string url) => CheckError(Functions.SDL_OpenURL(url));

		// SDL_platform.h

		public static string Platform => MemoryUtil.GetStringASCII(Functions.SDL_GetPlatform());

		// SDL_power.h

		public static (SDLPowerState, int, int) PowerState {
			get {
				SDLPowerState state = Functions.SDL_GetPowerInfo(out int secs, out int pct);
				return (state, secs, pct);
			}
		}

		// SDL_render.h

		public static SDLRendererInfo[] RenderDrivers {
			get {
				int n = Functions.SDL_GetNumRenderDrivers();
				SDLRendererInfo[] infos = new SDLRendererInfo[n];
				for (int i = 0; i < n; i++) Functions.SDL_GetRenderDriverInfo(i, out infos[i]);
				return infos;
			}
		}

		public static (SDLWindow, SDLRenderer) CreateWindowAndRenderer(int width, int height, SDLWindowFlags windowFlags) {
			CheckError(Functions.SDL_CreateWindowAndRenderer(width, height, windowFlags, out IntPtr window, out IntPtr renderer));
			return (new SDLWindow((IPointer<SDL_Window>)new UnmanagedPointer<SDL_Window>(window)), new SDLRenderer(renderer));
		}

		// SDL_sensor.h

		public const float StandardGravity = 9.80665f;

	}
}
