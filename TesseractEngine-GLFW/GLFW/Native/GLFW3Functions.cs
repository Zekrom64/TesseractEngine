using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core;
using Tesseract.Core.Native;

namespace Tesseract.GLFW.Native {

	using XID = UIntPtr;

	public unsafe class GLFW3Functions {

		// glfw3.h

		[NativeType("int glfwInit()")]
		public delegate* unmanaged<bool> glfwInit;
		[NativeType("void glfwTerminate()")]
		public delegate* unmanaged<void> glfwTerminate;
		[NativeType("void glfwInitHint(int hint, int value)")]
		public delegate* unmanaged<GLFWInitHint, int, void> glfwInitHint;
		[NativeType("void glfwGetVersion(int* major, int* minor, int* patch)")]
		public delegate* unmanaged<out int, out int, out int, void> glfwGetVersion;
		[NativeType("const char* glfwGetVersionString()")]
		public delegate* unmanaged<IntPtr> glfwGetVersionString;
		[NativeType("int glfwGetError(const char** description)")]
		public delegate* unmanaged<out IntPtr, GLFWError> glfwGetError;
		[NativeType("GLFWerrorfun glfwSetErrorCallback(GLFWerrorfun cbk)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwSetErrorCallback;

		[NativeType("GLFWmonitor** glfwGetMonitors(int* count)")]
		public delegate* unmanaged<out int, IntPtr> glfwGetMonitors;
		[NativeType("GLFWmonitor* glfwGetPrimaryMonitor()")]
		public delegate* unmanaged<IntPtr> glfwGetPrimaryMonitor;
		[NativeType("void glfwGetMonitorPos(GLFWmonitor* monitor, int* xpos, int* ypos)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> glfwGetMonitorPos;
		[NativeType("void glfwGetMonitorWorkarea(GLFWmonitor* monitor, int* xpos, int* ypos, int* width, int* height)")]
		public delegate* unmanaged<IntPtr, out int, out int, out int, out int, void> glfwGetMonitorWorkarea;
		[NativeType("void glfwGetMonitorPhysicalSize(GLFWmonitor* monitor, int* widthMM, int* heightMM)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> glfwGetMonitorPhysicalSize;
		[NativeType("void glfwGetMonitorContentScale(GLFWmonitor* monitor, float* scaleX, float* scaleY)")]
		public delegate* unmanaged<IntPtr, out float, out float, void> glfwGetMonitorContentScale;
		[NativeType("const char* glfwGetMonitorName(GLFWmonitor* monitor)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetMonitorName;
		[NativeType("void glfwSetMonitorUserPointer(GLFWmonitor* monitor, void* pointer)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glfwSetMonitorUserPointer;
		[NativeType("void* glfwGetMonitorUserPointer(GLFWmonitor* monitor)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetMonitorUserPointer;
		[NativeType("GLFWmonitorfun glfwSetMonitorCallback(GLFWmonitorfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwSetMonitorCallback;
		[NativeType("const GLFWvidmode* glfwGetVideoModes(GLFWmonitor* monitor, int* count)")]
		public delegate* unmanaged<IntPtr, out int, GLFWVidMode*> glfwGetVideoModes;
		[NativeType("const GLFWvidmode* glfwGetVideoMode(GLFWmonitor* monitor)")]
		public delegate* unmanaged<IntPtr, GLFWVidMode*> glfwGetVideoMode;
		[NativeType("void glfwSetGamma(GLFWmonitor* monitor, float gamma)")]
		public delegate* unmanaged<IntPtr, float, void> glfwSetGamma;
		[NativeType("const GLFWgammaramp* glfwGetGammaRamp(GLFWmonitor* monitor)")]
		public delegate* unmanaged<IntPtr, GLFWGammaRamp*> glfwGetGammaRamp;
		[NativeType("void glfwSetGammaRamp(GLFWmonitor* monitor, const GLFWgammaramp* gammaRamp)")]
		public delegate* unmanaged<IntPtr, in GLFWGammaRamp, void> glfwSetGammaRamp;

		[NativeType("void glfwDefaultWindowHints()")]
		public delegate* unmanaged<void> glfwDefaultWindowHints;
		[NativeType("void glfwWindowHint(int hint, int value)")]
		public delegate* unmanaged<GLFWWindowAttrib, int, void> glfwWindowHint;
		[NativeType("void glfwWindowHintString(int hint, const char* value)")]
		public delegate* unmanaged<GLFWWindowAttrib, IntPtr, void> glfwWindowHintString;
		[NativeType("GLFWwindow* glfwCreateWindow(int width, int height, const char* title, GLFWmonitor* monitor, GLFWwindow* share)")]
		public delegate* unmanaged<int, int, IntPtr, IntPtr, IntPtr, IntPtr> glfwCreateWindow;
		[NativeType("void glfwDestroyWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwDestroyWindow;
		[NativeType("int glfwWindowShouldClose(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, bool> glfwWindowShouldClose;
		[NativeType("void glfwSetWindowShouldClose(GLFWwindow* window, int value)")]
		public delegate* unmanaged<IntPtr, bool, void> glfwSetWindowShouldClose;
		[NativeType("void glfwSetWindowTitle(GLFWwindow* window, const char* title)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glfwSetWindowTitle;
		[NativeType("void glfwSetWindowIcon(GLFWwindow* window, int count, const GLFWimage* images)")]
		public delegate* unmanaged<IntPtr, int, GLFWImage*, void> glfwSetWindowIcon;
		[NativeType("void glfwGetWindowPos(GLFWwindow* window, int* xpos, int* ypos")]
		public delegate* unmanaged<IntPtr, out int, out int, void> glfwGetWindowPos;
		[NativeType("void glfwSetWindowPos(GLFWwindow* window, int xpos, int ypos)")]
		public delegate* unmanaged<IntPtr, int, int, void> glfwSetWindowPos;
		[NativeType("void glfwGetWindowSize(GLFWwindow* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> glfwGetWindowSize;
		[NativeType("void glfwSetWindowSizeLimits(GLFWwindow* window, int minw, int minh, int maxw, int maxh)")]
		public delegate* unmanaged<IntPtr, int, int, int, int, void> glfwSetWindowSizeLimits;
		[NativeType("void glfwSetWindowAspectRatio(GLFWwindow* window, int numer, int denom)")]
		public delegate* unmanaged<IntPtr, int, int, void> glfwSetWindowAspectRatio;
		[NativeType("void glfwSetWindowSize(GLFWwindow* window, int w, int h)")]
		public delegate* unmanaged<IntPtr, int, int, void> glfwSetWindowSize;
		[NativeType("void glfwGetFramebufferSize(GLFWwindow* window, int* w, int* h)")]
		public delegate* unmanaged<IntPtr, out int, out int, void> glfwGetFramebufferSize;
		[NativeType("void glfwGetWindowFrameSize(GLFWwindow* window, int* left, int* top, int* right, int* bottom)")]
		public delegate* unmanaged<IntPtr, out int, out int, out int, out int, void> glfwGetWindowFrameSize;
		[NativeType("void glfwGetWindowContentScale(GLFWwindow* window, float* scaleX, float* scaleY)")]
		public delegate* unmanaged<IntPtr, out float, out float, void> glfwGetWindowContentScale;
		[NativeType("float glfwGetWindowOpacity(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, float> glfwGetWindowOpacity;
		[NativeType("void glfwSetWindowOpacity(GLFWwindow* window, float opacity)")]
		public delegate* unmanaged<IntPtr, float, void> glfwSetWindowOpacity;
		[NativeType("void glfwIconifyWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwIconifyWindow;
		[NativeType("void glfwRestoreWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwRestoreWindow;
		[NativeType("void glfwMaximizeWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwMaximizeWindow;
		[NativeType("void glfwShowWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwShowWindow;
		[NativeType("void glfwHideWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwHideWindow;
		[NativeType("void glfwFocusWindow(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwFocusWindow;
		[NativeType("void glfwRequestWindowAttention(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwRequestWindowAttention;
		[NativeType("GLFWmonitor* glfwGetWindowMonitor(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWindowMonitor;
		[NativeType("void glfwSetWindowMonitor(GLFWwindow* window, GLFWmonitor* monitor, int xpos, int ypos, int width, int height, int refreshRate)")]
		public delegate* unmanaged<IntPtr, IntPtr, int, int, int, int, int, void> glfwSetWindowMonitor;
		[NativeType("int glfwGetWindowAttrib(GLFWwindow* window, int attrib)")]
		public delegate* unmanaged<IntPtr, GLFWWindowAttrib, int> glfwGetWindowAttrib;
		[NativeType("void glfwSetWindowAttrib(GLFWwindow* window, int attrib, int value)")]
		public delegate* unmanaged<IntPtr, GLFWWindowAttrib, int, void> glfwSetWindowAttrib;
		[NativeType("void* glfwGetWindowUserPointer(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWindowUserPointer;
		[NativeType("void glfwSetWindowUserPointer(GLFWwindow* window, void* pointer)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glfwSetWindowUserPointer;

		[NativeType("GLFWwindowposfun glfwSetWindowPosCallback(GLFWwindow* window, GLFWwindowposfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowPosCallback;
		[NativeType("GLFWwindowsizefun glfwSetWindowSizeCallback(GLFWwindow* window, GLFWwindowsizefun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowSizeCallback;
		[NativeType("GLFWwindowclosefun glfwSetWindowCloseCallback(GLFWwindow* window, GLFWwindowclosefun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowCloseCallback;
		[NativeType("GLFWwindowrefreshfun glfwSetWindowRefreshCallback(GLFWwindow* window, GLFWwindowrefreshfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowRefreshCallback;
		[NativeType("GLFWwindowfocusfun glfwSetWindowFocusCallback(GLFWwindow* window, GLFWwindowfocusfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowFocusCallback;
		[NativeType("GLFWwindowiconifyfun glfwSetWindowIconifyCallback(GLFWwindow* window, GLFWwindowiconifyfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowIconifyCallback;
		[NativeType("GLFWwindowmaximizefun glfwSetWindowMaximizeCallback(GLFWwindow* window, GLFWwindowmaximizefun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowMaximizeCallback;
		[NativeType("GLFWframebuffersizefun glfwSetFramebufferSizeCallback(GLFWwindow* window, GLFWframebuffersizefun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetFramebufferSizeCallback;
		[NativeType("GLFWwindowcontentscalefun glfwSetWindowContentScaleCallback(GLFWwindow* window, GLFWwindowcontentscalefun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetWindowContentScaleCallback;

		[NativeType("void glfwPollEvents()")]
		public delegate* unmanaged<void> glfwPollEvents;
		[NativeType("void glfwWaitEvents()")]
		public delegate* unmanaged<void> glfwWaitEvents;
		[NativeType("void glfwWaitEventsTimeout(double timeout)")]
		public delegate* unmanaged<double, void> glfwWaitEventsTimeout;
		[NativeType("void glfwPostEmptyEvent()")]
		public delegate* unmanaged<void> glfwPostEmptyEvent;
		[NativeType("int glfwGetInputMode(GLFWwindow* window, int mode)")]
		public delegate* unmanaged<IntPtr, GLFWInputMode, int> glfwGetInputMode;
		[NativeType("void glfwSetInputMode(GLFWwindow* window, int mode, int value)")]
		public delegate* unmanaged<IntPtr, GLFWInputMode, int, void> glfwSetInputMode;
		[NativeType("bool glfwRawMouseMotionSupported()")]
		public delegate* unmanaged<bool> glfwRawMouseMotionSupported;

		[NativeType("const char* glfwGetKeyName(int key, int scancode)")]
		public delegate* unmanaged<GLFWKey, int, IntPtr> glfwGetKeyName;
		[NativeType("int glfwGetKeyScancode(int key)")]
		public delegate* unmanaged<GLFWKey, int> glfwGetKeyScancode;
		[NativeType("int glfwGetKey(GLFWwindow* window, int key)")]
		public delegate* unmanaged<IntPtr, GLFWKey, GLFWButtonState> glfwGetKey;

		[NativeType("int glfwGetMouseButton(GLFWwindow* window, int button)")]
		public delegate* unmanaged<IntPtr, int, GLFWButtonState> glfwGetMouseButton;
		[NativeType("void glfwGetCursorPos(GLFWwindow* window, double* xpos, double* ypos)")]
		public delegate* unmanaged<IntPtr, out double, out double, void> glfwGetCursorPos;
		[NativeType("void glfwSetCursorPos(GLFWwindow* window, double xpos, double ypos)")]
		public delegate* unmanaged<IntPtr, double, double, void> glfwSetCursorPos;

		[NativeType("GLFWcursor* glfwCreateCursor(const GLFWimage* image, int xhot, int yhot)")]
		public delegate* unmanaged<in GLFWImage, int, int, IntPtr> glfwCreateCursor;
		[NativeType("GLFWcursor* glfwCreateStandardCursor(int shape)")]
		public delegate* unmanaged<GLFWCursorShape, IntPtr> glfwCreateStandardCursor;
		[NativeType("void glfwDestroyCursor(GLFWcursor* cursor)")]
		public delegate* unmanaged<IntPtr, void> glfwDestroyCursor;
		[NativeType("void glfwSetCursor(GLFWwindow* window, GLFWcursor* cursor)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glfwSetCursor;

		[NativeType("GLFWkeyfun glfwSetKeyCallback(GLFWkeyfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetKeyCallback;
		[NativeType("GLFWcharfun glfwSetCharCallback(GLFWcharfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetCharCallback;
		[NativeType("GLFWcharmodsfun glfwSetCharModsCallback(GLFWcharmodsfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetCharModsCallback;
		[NativeType("GLFWmousebuttonfun glfwSetMouseButtonCallback(GLFWmousebuttonfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetMouseButtonCallback;
		[NativeType("GLFWcursorposfun glfwSetMouseButtonCallback(GLFWcursorposfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetCursorPosCallback;
		[NativeType("GLFWcursorenterfun glfwSetCursorEnterCallback(GLFWcursorenterfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetCursorEnterCallback;
		[NativeType("GLFWscrollfun glfwSetScrollCallback(GLFWscrollfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetScrollCallback;
		[NativeType("GLFWdropfun glfwSetDropCallback(GLFWdropfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwSetDropCallback;

		[NativeType("int glfwJoystickPresent(int jid)")]
		public delegate* unmanaged<int, bool> glfwJoystickPresent;
		[NativeType("const float* glfwGetJoystickAxes(int jid, int* count)")]
		public delegate* unmanaged<int, out int, IntPtr> glfwGetJoystickAxes;
		[NativeType("const unsigned char* glfwGetJoystickButtons(int jid, int* count)")]
		public delegate* unmanaged<int, out int, IntPtr> glfwGetJoystickButtons;
		[NativeType("const unsigned char* glfwGetJoystickHats(int jid, int* count)")]
		public delegate* unmanaged<int, out int, IntPtr> glfwGetJoystickHats;
		[NativeType("const char* glfwGetJoystickName(int jid)")]
		public delegate* unmanaged<int, IntPtr> glfwGetJoystickName;
		[NativeType("const char* glfwGetJoystickGUID(int jid)")]
		public delegate* unmanaged<int, IntPtr> glfwGetJoystickGUID;
		[NativeType("void glfwSetJoystickUserPointer(int jid, void* pointer)")]
		public delegate* unmanaged<int, IntPtr, void> glfwSetJoystickUserPointer;
		[NativeType("void* glfwGetJoystickUserPointer(int jid)")]
		public delegate* unmanaged<int, IntPtr> glfwGetJoystickUserPointer;
		[NativeType("int glfwJoystickIsGamepad(int jid)")]
		public delegate* unmanaged<int, bool> glfwJoystickIsGamepad;
		[NativeType("GLFWjoystickfun glfwSetJoystickCallback(GLFWjoystickfun callback)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwSetJoystickCallback;

		[NativeType("int glfwUpdateGamepadMappings(const char* str)")]
		public delegate* unmanaged<IntPtr, bool> glfwUpdateGamepadMappings;
		[NativeType("const char* glfwGetGamepadName(int jid)")]
		public delegate* unmanaged<int, IntPtr> glfwGetGamepadName;
		[NativeType("int glfwGetGamepadState(int jid)")]
		public delegate* unmanaged<int, out GLFWGamepadState, bool> glfwGetGamepadState;

		[NativeType("void glfwSetClipboardString(GLFWwindow* window, const char* str)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> glfwSetClipboardString;
		[NativeType("const char* glfwGetClipboardString(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetClipboardString;

		[NativeType("double glfwGetTime()")]
		public delegate* unmanaged<double> glfwGetTime;
		[NativeType("void glfwSetTime(double time)")]
		public delegate* unmanaged<double, void> glfwSetTime;
		[NativeType("uint64_t glfwGetTimerValue()")]
		public delegate* unmanaged<ulong> glfwGetTimerValue;
		[NativeType("uint64_t glfwGetTimerFrequency()")]
		public delegate* unmanaged<ulong> glfwGetTimerFrequency;

		[NativeType("void glfwMakeContextCurrent(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwMakeContextCurrent;
		[NativeType("GLFWwindow* glfwGetCurrentContext()")]
		public delegate* unmanaged<IntPtr> glfwGetCurrentContext;
		[NativeType("void glfwSwapBuffers(GLFWwindow* window)")]
		public delegate* unmanaged<IntPtr, void> glfwSwapBuffers;
		[NativeType("void glfwSwapInterval(int interval)")]
		public delegate* unmanaged<int, void> glfwSwapInterval;
		[NativeType("int glfwExtensionSupported(const char* extension)")]
		public delegate* unmanaged<IntPtr, bool> glfwExtensionSupported;
		[NativeType("void* glfwGetProcAddress(const char* procname)")]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetProcAddress;

		[NativeType("int glfwVulkanSupported()")]
		public delegate* unmanaged<bool> glfwVulkanSupported;
		[NativeType("const char** glfwGetRequiredInstanceExtensions(uint32_t* count)")]
		public delegate* unmanaged<out uint, IntPtr> glfwGetRequiredInstanceExtensions;
		[NativeType("void* glfwGetInstanceProcAddress(VkInstance instance, const char* procname)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> glfwGetInstanceProcAddress;
		[NativeType("int glfwGetPhysicalDevicePresentationSupport(VkInstance instance, VkPhysicalDevice physicalDevice, uint32_t queueFamily)")]
		public delegate* unmanaged<IntPtr, IntPtr, uint, bool> glfwGetPhysicalDevicePresentationSupport;
		[NativeType("VkResult glfwCreateWindowSurface(VkInstance instance, GLFWwindow* window, const VkAllocationCallbacks* allocator, VkSurfaceKHR* surface)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, out ulong, int> glfwCreateWindowSurface;

		// glfw3native.h

		[NativeType("const char* glfwGetWin32Adapter(GLFWmonitor* monitor)")]
		[ExternFunction(Platform = PlatformType.Windows)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWin32Adapter;
		[NativeType("const char* glfwGetWin32Monitor(GLFWmonitor* monitor)")]
		[ExternFunction(Platform = PlatformType.Windows)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWin32Monitor;
		[NativeType("HWND glfwGetWin32Window(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Windows)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWin32Window;

		[NativeType("HGLRC glfwGetWGLContext(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Windows)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWGLContext;

		[NativeType("CGDirectDisplayID glfwGetCocoaMonitor(GLFWmonitor* monitor)")]
		[ExternFunction(Platform = PlatformType.MacOSX)]
		public delegate* unmanaged<IntPtr, uint> glfwGetCocoaMonitor;
		[NativeType("id glfwGetCocoaWindow(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.MacOSX)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetCocoaWindow;

		[NativeType("id glfwGetNSGLContext(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.MacOSX)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetNSGLContext;

		[NativeType("Display* glfwGetX11Display()")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr> glfwGetX11Display;
		[NativeType("RRCrtc glfwGetX11Adapter(GLFWmonitor* monitor)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, XID> glfwGetX11Adapter;
		[NativeType("RROutput glfwGetX11Monitor(GLFWmonitor* monitor)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, XID> glfwGetX11Monitor;
		[NativeType("Window glfwGetX11Window(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, XID> glfwGetX11Window;
		[NativeType("void glfwSetX11SelectionString(const char* str)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, void> glfwSetX11SelectionString;
		[NativeType("const char* glfwGetX11SelectionString()")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr> glfwGetX11SelectionString;

		[NativeType("GLXContext* glfwGetGLXContext(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetGLXContext;
		[NativeType("GLXWindow glfwGetGLXWindow(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, XID> glfwGetGLXWindow;

		[NativeType("wl_display* glfwGetWaylandDisplay()")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr> glfwGetWaylandDisplay;
		[NativeType("wl_output* glfwGetWaylandMonitor(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWaylandMonitor;
		[NativeType("wl_surface* glfwGetWaylandWindow(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetWaylandWindow;

		[NativeType("EGLDisplay glfwGetEGLDisplay()")]
		[ExternFunction(Relaxed = true)]
		public delegate* unmanaged<IntPtr> glfwGetEGLDisplay;
		[NativeType("EGLContext glfwGetEGLContext(GLFWwindow* window)")]
		[ExternFunction(Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetEGLContext;
		[NativeType("EGLSurface glfwGetEGLSurface(GLFWwindow* window)")]
		[ExternFunction(Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetEGLSurface;

		[NativeType("int glfwGetOSMesaColorBuffer(GLFWwindow* window, int* width, int* height, int* format, void** buffer)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, out int, out int, out int, out IntPtr, bool> glfwGetOSMesaColorBuffer;
		[NativeType("int glfwGetOSMesaDepthBuffer(GLFWwindow* window, int* width, int* height, int* bytesPerValue, void** buffer)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, out int, out int, out int, out IntPtr, bool> glfwGetOSMesaDepthBuffer;
		[NativeType("OSMesaContext glfwGetOSMesaContext(GLFWwindow* window)")]
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> glfwGetOSMesaContext;

	}

}
