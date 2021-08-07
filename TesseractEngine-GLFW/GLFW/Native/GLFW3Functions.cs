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

	public class GLFW3Functions {

		// glfw3.h

		public delegate bool PFN_glfwInit();
		public delegate void PFN_glfwTerminate();
		public delegate void PFN_glfwInitHint(GLFWInitHint hint, int value);
		public delegate void PFN_glfwGetVersion(out int major, out int minor, out int rev);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetVersionString();
		public delegate GLFWError PFN_glfwGetError([NativeType("const char**")] out IntPtr description);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWErrorFun PFN_glfwSetErrorCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLFWErrorFun callback);

		public PFN_glfwInit glfwInit;
		public PFN_glfwTerminate glfwTerminate;
		public PFN_glfwInitHint glfwInitHint;
		public PFN_glfwGetVersion glfwGetVersion;
		public PFN_glfwGetVersionString glfwGetVersionString;
		public PFN_glfwGetError glfwGetError;
		public PFN_glfwSetErrorCallback glfwSetErrorCallback;

		[return: NativeType("GLFWmonitor**")]
		public delegate IntPtr PFN_glfwGetMonitors(out int count);
		[return: NativeType("GLFWmonitor*")]
		public delegate IntPtr PFN_glfwGetPrimaryMonitor();
		public delegate void PFN_glfwGetMonitorPos([NativeType("GLFWmonitor*")] IntPtr monitor, out int xpos, out int ypos);
		public delegate void PFN_glfwGetMonitorWorkArea([NativeType("GLFWmonitor*")] IntPtr monitor, out int xpos, out int ypos, out int width, out int height);
		public delegate void PFN_glfwGetMonitorPhysicalSize([NativeType("GLFWmonitor*")] IntPtr monitor, out int widthMM, out int heightMM);
		public delegate void PFN_glfwGetMonitorContentScale([NativeType("GLFWmonitor*")] IntPtr monitor, out float scaleX, out float scaleY);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetMonitorName([NativeType("GLFWmonitor*")] IntPtr monitor);
		public delegate void PFN_glfwSetMonitorUserPointer([NativeType("GLFWmonitor*")] IntPtr monitor, IntPtr pointer);
		public delegate IntPtr PFN_glfwGetMonitorUserPointer([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWMonitorFun PFN_glfwSetMonitorCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLFWMonitorFun callback);
		[return: NativeType("const GLFWvidmode*")]
		public delegate IntPtr PFN_glfwGetVideoModes([NativeType("GLFWmonitor*")] IntPtr monitor, out int count);
		[return: NativeType("const GLFWvidmode*")]
		public delegate IntPtr PFN_glfwGetVideoMode([NativeType("GLFWmonitor*")] IntPtr monitor);
		public delegate void PFN_glfwSetGamma([NativeType("GLFWmonitor*")] IntPtr monitor, float gamma);
		[return: NativeType("const GLFWgammaramp*")]
		public delegate IntPtr PFN_glfwGetGammaRamp([NativeType("GLFWmonitor*")] IntPtr monitor);
		public delegate void PFN_glfwSetGammaRamp([NativeType("GLFWmonitor*")] IntPtr monitor, in GLFWGammaRamp gammaRamp);

		public PFN_glfwGetMonitors glfwGetMonitors;
		public PFN_glfwGetPrimaryMonitor glfwGetPrimaryMonitor;
		public PFN_glfwGetMonitorPos glfwGetMonitorPos;
		public PFN_glfwGetMonitorWorkArea glfwGetMonitorWorkArea;
		public PFN_glfwGetMonitorPhysicalSize glfwGetMonitorPhysicalSize;
		public PFN_glfwGetMonitorContentScale glfwGetMonitorContentScale;
		public PFN_glfwGetMonitorName glfwGetMonitorName;
		public PFN_glfwSetMonitorUserPointer glfwSetMonitorUserPointer;
		public PFN_glfwGetMonitorUserPointer glfwGetMonitorUserPointer;
		public PFN_glfwSetMonitorCallback glfwSetMonitorCallback;
		public PFN_glfwGetVideoModes glfwGetVideoModes;
		public PFN_glfwGetVideoMode glfwGetVideoMode;
		public PFN_glfwSetGamma glfwSetGamma;
		public PFN_glfwGetGammaRamp glfwGetGammaRamp;
		public PFN_glfwSetGammaRamp glfwSetGammaRamp;

		public delegate void PFN_glfwDefaultWindowHints();
		public delegate void PFN_glfwWindowHint(GLFWWindowAttrib hint, int value);
		public delegate void PFN_glfwWindowHintString(GLFWWindowAttrib hint, [MarshalAs(UnmanagedType.LPStr)] string value);
		[return: NativeType("GLFWwindow*")]
		public delegate IntPtr PFN_glfwCreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPUTF8Str)] string title, [NativeType("GLFWmonitor*")] IntPtr monitor, [NativeType("GLFWwindow*")] IntPtr share);
		public delegate void PFN_glfwDestroyWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate bool PFN_glfwWindowShouldClose([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwSetWindowShouldClose([NativeType("GLFWwindow*")] IntPtr window, bool value);
		public delegate void PFN_glfwSetWindowTitle([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string title);
		public delegate void PFN_glfwSetWindowIcon([NativeType("GLFWwindow*")] IntPtr window, int count, [NativeType("const GLFWimage*")] IntPtr images);
		public delegate void PFN_glfwGetWindowPos([NativeType("GLFWwindow*")] IntPtr window, out int xpos, out int ypos);
		public delegate void PFN_glfwSetWindowPos([NativeType("GLFWwindow*")] IntPtr window, int xpos, int ypos);
		public delegate void PFN_glfwGetWindowSize([NativeType("GLFWwindow*")] IntPtr window, out int w, out int h);
		public delegate void PFN_glfwSetWindowSizeLimits([NativeType("GLFWwindow*")] IntPtr window, int minw, int minh, int maxw, int maxh);
		public delegate void PFN_glfwSetWindowAspectRatio([NativeType("GLFWwindow*")] IntPtr window, int numer, int denom);
		public delegate void PFN_glfwSetWindowSize([NativeType("GLFWwindow*")] IntPtr window, int w, int h);
		public delegate void PFN_glfwGetFramebufferSize([NativeType("GLFWwindow*")] IntPtr window, out int w, out int h);
		public delegate void PFN_glfwGetWindowFrameSize([NativeType("GLFWwindow*")] IntPtr window, out int left, out int top, out int right, out int bottom);
		public delegate void PFN_glfwGetWindowContentScale([NativeType("GLFWwindow*")] IntPtr window, out float scaleX, out float scaleY);
		public delegate float PFN_glfwGetWindowOpacity([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwSetWindowOpacity([NativeType("GLFWwindow*")] IntPtr window, float opacity);
		public delegate void PFN_glfwIconifyWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwRestoreWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwMaximizeWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwShowWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwHideWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwFocusWindow([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwRequestWindowAttention([NativeType("GLFWwindow*")] IntPtr window);
		[return: NativeType("GLFWmonitor*")]
		public delegate IntPtr PFN_glfwGetWindowMonitor([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwSetWindowMonitor([NativeType("GLFWwindow*")] IntPtr window, [NativeType("GLFWmonitor*")] IntPtr monitor, int xpos, int ypos, int width, int height, int refreshRate);
		public delegate int PFN_glfwGetWindowAttrib([NativeType("GLFWwindow*")] IntPtr window, GLFWWindowAttrib attrib);
		public delegate void PFN_glfwSetWindowAttrib([NativeType("GLFWwindow*")] IntPtr window, GLFWWindowAttrib attrib, int value);
		public delegate void PFN_glfwSetWindowUserPointer([NativeType("GLFWwindow*")] IntPtr window, IntPtr pointer);
		public delegate IntPtr PFN_glfwGetWindowUserPointer([NativeType("GLFWwindow*")] IntPtr window);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowPosFun PFN_glfwSetWindowPosCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowPosFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowSizeFun PFN_glfwSetWindowSizeCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowSizeFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowCloseFun PFN_glfwSetWindowCloseCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowCloseFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowRefreshFun PFN_glfwSetWindowRefreshCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowRefreshFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowFocusFun PFN_glfwSetWindowFocusCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowFocusFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowIconifyFun PFN_glfwSetWindowIconifyCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowIconifyFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowMaximizeFun PFN_glfwSetWindowMaximizeCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowMaximizeFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWFramebufferSizeFun PFN_glfwSetFramebufferSizeCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWFramebufferSizeFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWWindowContentScaleFun PFN_glfwSetWindowContentScaleCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWWindowContentScaleFun callback);

		public PFN_glfwDefaultWindowHints glfwDefaultWindowHints;
		public PFN_glfwWindowHint glfwWindowHint;
		public PFN_glfwWindowHintString glfwWindowHintString;
		public PFN_glfwCreateWindow glfwCreateWindow;
		public PFN_glfwDestroyWindow glfwDestroyWindow;
		public PFN_glfwWindowShouldClose glfwWindowShouldClose;
		public PFN_glfwSetWindowShouldClose glfwSetWindowShouldClose;
		public PFN_glfwSetWindowTitle glfwSetWindowTitle;
		public PFN_glfwSetWindowIcon glfwSetWindowIcon;
		public PFN_glfwGetWindowPos glfwGetWindowPos;
		public PFN_glfwSetWindowPos glfwSetWindowPos;
		public PFN_glfwGetWindowSize glfwGetWindowSize;
		public PFN_glfwSetWindowSizeLimits glfwSetWindowSizeLimits;
		public PFN_glfwSetWindowAspectRatio glfwSetWindowAspectRatio;
		public PFN_glfwSetWindowSize glfwSetWindowSize;
		public PFN_glfwGetFramebufferSize glfwGetFramebufferSize;
		public PFN_glfwGetWindowFrameSize glfwGetWindowFrameSize;
		public PFN_glfwGetWindowContentScale glfwGetWindowContentScale;
		public PFN_glfwGetWindowOpacity glfwGetWindowOpacity;
		public PFN_glfwSetWindowOpacity glfwSetWindowOpacity;
		public PFN_glfwIconifyWindow glfwIconifyWindow;
		public PFN_glfwRestoreWindow glfwRestoreWindow;
		public PFN_glfwMaximizeWindow glfwMaximizeWindow;
		public PFN_glfwShowWindow glfwShowWindow;
		public PFN_glfwHideWindow glfwHideWindow;
		public PFN_glfwFocusWindow glfwFocusWindow;
		public PFN_glfwRequestWindowAttention glfwRequestWindowAttention;
		public PFN_glfwGetWindowMonitor glfwGetWindowMonitor;
		public PFN_glfwSetWindowMonitor glfwSetWindowMonitor;
		public PFN_glfwGetWindowAttrib glfwGetWindowAttrib;
		public PFN_glfwSetWindowAttrib glfwSetWindowAttrib;
		public PFN_glfwGetWindowUserPointer glfwGetWindowUserPointer;
		public PFN_glfwSetWindowUserPointer glfwSetWindowUserPointer;
		public PFN_glfwSetWindowPosCallback glfwSetWindowPosCallback;
		public PFN_glfwSetWindowSizeCallback glfwSetWindowSizeCallback;
		public PFN_glfwSetWindowCloseCallback glfwSetWindowCloseCallback;
		public PFN_glfwSetWindowRefreshCallback glfwSetWindowRefreshCallback;
		public PFN_glfwSetWindowFocusCallback glfwSetWindowFocusCallback;
		public PFN_glfwSetWindowIconifyCallback glfwSetWindowIconifyCallback;
		public PFN_glfwSetWindowMaximizeCallback glfwSetWindowMaximizeCallback;
		public PFN_glfwSetFramebufferSizeCallback glfwSetFramebufferSizeCallback;
		public PFN_glfwSetWindowContentScaleCallback glfwSetWindowContentScaleCallback;

		public delegate void PFN_glfwPollEvents();
		public delegate void PFN_glfwWaitEvents();
		public delegate void PFN_glfwWaitEventsTimeout(double timeout);
		public delegate void PFN_glfwPostEmptyEvent();
		public delegate int PFN_glfwGetInputMode([NativeType("GLFWwindow*")] IntPtr window, GLFWInputMode mode);
		public delegate void PFN_glfwSetInputMode([NativeType("GLFWwindow*")] IntPtr window, GLFWInputMode mode, int value);
		public delegate bool PFN_glfwRawMouseMotionSupported();

		public PFN_glfwPollEvents glfwPollEvents;
		public PFN_glfwWaitEvents glfwWaitEvents;
		public PFN_glfwWaitEventsTimeout glfwWaitEventsTimeout;
		public PFN_glfwPostEmptyEvent glfwPostEmptyEvent;
		public PFN_glfwGetInputMode glfwGetInputMode;
		public PFN_glfwSetInputMode glfwSetInputMode;
		public PFN_glfwRawMouseMotionSupported glfwRawMouseMotionSupported;

		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetKeyName(GLFWKey key, int scancode);
		public delegate int PFN_glfwGetKeyScancode(GLFWKey key);
		public delegate GLFWButtonState PFN_glfwGetKey([NativeType("GLFWwindow*")] IntPtr window, GLFWKey key);

		public PFN_glfwGetKeyName glfwGetKeyName;
		public PFN_glfwGetKeyScancode glfwGetKeyScancode;
		public PFN_glfwGetKey glfwGetKey;

		public delegate GLFWButtonState PFN_glfwGetMouseButton([NativeType("GLFWwindow*")] IntPtr window, int button);
		public delegate void PFN_glfwGetCursorPos([NativeType("GLFWwindow*")] IntPtr window, out double xpos, out double ypos);
		public delegate void PFN_glfwSetCursorPos([NativeType("GLFWwindow*")] IntPtr window, double xpos, double ypos);

		public PFN_glfwGetMouseButton glfwGetMouseButton;
		public PFN_glfwGetCursorPos glfwGetCursorPos;
		public PFN_glfwSetCursorPos glfwSetCursorPos;
		
		[return: NativeType("GLFWcursor*")]
		public delegate IntPtr PFN_glfwCreateCursor(in GLFWImage image, int xhot, int yhot);
		[return: NativeType("GLFWcursor*")]
		public delegate IntPtr PFN_glfwCreateStandardCursor(GLFWCursorShape shape);
		public delegate void PFN_glfwDestroyCursor([NativeType("GLFWcursor*")] IntPtr cursor);
		public delegate void PFN_glfwSetCursor([NativeType("GLFWwindow*")] IntPtr window, [NativeType("GLFWcursor*")] IntPtr cursor);

		public PFN_glfwCreateCursor glfwCreateCursor;
		public PFN_glfwCreateStandardCursor glfwCreateStandardCursor;
		public PFN_glfwDestroyCursor glfwDestroyCursor;
		public PFN_glfwSetCursor glfwSetCursor;

		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWKeyFun PFN_glfwSetKeyCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWKeyFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWCharFun PFN_glfwSetCharCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWCharFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWCharModsFun PFN_glfwSetCharModsCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWCharModsFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWMouseButtonFun PFN_glfwSetMouseButtonCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWMouseButtonFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWCursorPosFun PFN_glfwSetCursorPosCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWCursorPosFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWCursorEnterFun PFN_glfwSetCursorEnterCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWCursorEnterFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWScrollFun PFN_glfwSetScrollCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWScrollFun callback);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWDropFun PFN_glfwSetDropCallback([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.FunctionPtr)] GLFWDropFun callback);

		public PFN_glfwSetKeyCallback glfwSetKeyCallback;
		public PFN_glfwSetCharCallback glfwSetCharCallback;
		public PFN_glfwSetCharModsCallback glfwSetCharModsCallback;
		public PFN_glfwSetMouseButtonCallback glfwSetMouseButtonCallback;
		public PFN_glfwSetCursorPosCallback glfwSetCursorPosCallback;
		public PFN_glfwSetCursorEnterCallback glfwSetCursorEnterCallback;
		public PFN_glfwSetScrollCallback glfwSetScrollCallback;
		public PFN_glfwSetDropCallback glfwSetDropCallback;

		public delegate bool PFN_glfwJoystickPresent(int jid);
		[return: NativeType("const float*")]
		public delegate IntPtr PFN_glfwGetJoystickAxes(int jid, out int count);
		[return: NativeType("const unsigned char*")]
		public delegate IntPtr PFN_glfwGetJoystickButtons(int jid, out int count);
		[return: NativeType("const unsigned char*")]
		public delegate IntPtr PFN_glfwGetJoystickHats(int jid, out int count);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetJoystickName(int jid);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetJoystickGUID(int jid);
		public delegate void PFN_glfwSetJoystickUserPointer(int jid, IntPtr pointer);
		public delegate IntPtr PFN_glfwGetJoystickUserPointer(int jid);
		public delegate bool PFN_glfwJoystickIsGamepad(int jid);
		[return: MarshalAs(UnmanagedType.FunctionPtr)]
		public delegate GLFWJoystickFun PFN_glfwSetJoystickCallback([MarshalAs(UnmanagedType.FunctionPtr)] GLFWJoystickFun callback);

		public PFN_glfwJoystickPresent glfwJoystickPresent;
		public PFN_glfwGetJoystickAxes glfwGetJoystickAxes;
		public PFN_glfwGetJoystickButtons glfwGetJoystickButtons;
		public PFN_glfwGetJoystickHats glfwGetJoystickHats;
		public PFN_glfwGetJoystickName glfwGetJoystickName;
		public PFN_glfwGetJoystickGUID glfwGetJoystickGUID;
		public PFN_glfwSetJoystickUserPointer glfwSetJoystickUserPointer;
		public PFN_glfwGetJoystickUserPointer glfwGetJoystickUserPointer;
		public PFN_glfwJoystickIsGamepad glfwJoystickIsGamepad;
		public PFN_glfwSetJoystickCallback glfwSetJoystickCallback;

		public delegate bool PFN_glfwUpdateGamepadMappings([MarshalAs(UnmanagedType.LPStr)] string str);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetGamepadName(int jid);
		public delegate bool PFN_glfwGetGamepadState(int jid, out GLFWGamepadState state);

		public PFN_glfwUpdateGamepadMappings glfwUpdateGamepadMappings;
		public PFN_glfwGetGamepadName glfwGetGamepadName;
		public PFN_glfwGetGamepadState glfwGetGamepadState;

		public delegate void PFN_glfwSetClipboardString([NativeType("GLFWwindow*")] IntPtr window, [MarshalAs(UnmanagedType.LPUTF8Str)] string str);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetClipboardString([NativeType("GLFWwindow*")] IntPtr window);

		public PFN_glfwSetClipboardString glfwSetClipboardString;
		public PFN_glfwGetClipboardString glfwGetClipboardString;

		public delegate double PFN_glfwGetTime();
		public delegate void PFN_glfwSetTime(double time);
		public delegate ulong PFN_glfwGetTimerValue();
		public delegate ulong PFN_glfwGetTimerFrequency();

		public PFN_glfwGetTime glfwGetTime;
		public PFN_glfwSetTime glfwSetTime;
		public PFN_glfwGetTimerValue glfwGetTimerValue;
		public PFN_glfwGetTimerFrequency glfwGetTimerFrequency;

		public delegate void PFN_glfwMakeContextCurrent([NativeType("GLFWwindow*")] IntPtr window);
		[return: NativeType("GLFWwindow*")]
		public delegate IntPtr PFN_glfwGetCurrentContext();
		public delegate void PFN_glfwSwapBuffers([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwSwapInterval(int interval);
		public delegate bool PFN_glfwExtensionSupported([MarshalAs(UnmanagedType.LPStr)] string extension);
		public delegate IntPtr PFN_glfwGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string procname);

		public PFN_glfwMakeContextCurrent glfwMakeContextCurrent;
		public PFN_glfwGetCurrentContext glfwGetCurrentContext;
		public PFN_glfwSwapBuffers glfwSwapBuffers;
		public PFN_glfwSwapInterval glfwSwapInterval;
		public PFN_glfwExtensionSupported glfwExtensionSupported;
		public PFN_glfwGetProcAddress glfwGetProcAddress;

		public delegate bool PFN_glfwVulkanSupported();
		[return: NativeType("const char**")]
		public delegate IntPtr PFN_glfwGetRequiredInstanceExtensions(out uint count);
		public delegate IntPtr PFN_glfwGetInstanceProcAddress([NativeType("VkInstance")] IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string procname);
		public delegate bool PFN_glfwGetPhysicalDevicePresentationSupport([NativeType("VkInstance")] IntPtr instance, [NativeType("VkPhysicalDevice")] IntPtr physicalDevice, uint queueFamily);
		[return: NativeType("VkResult")]
		public delegate int PFN_glfwCreateWindowSurface([NativeType("VkInstance")] IntPtr instance, [NativeType("GLFWwindow*")] IntPtr window, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator, [NativeType("VkSurfaceKHR*")] out ulong surface);

		public PFN_glfwVulkanSupported glfwVulkanSupported;
		public PFN_glfwGetRequiredInstanceExtensions glfwGetRequiredInstanceExtensions;
		public PFN_glfwGetInstanceProcAddress glfwGetInstanceProcAddress;
		public PFN_glfwGetPhysicalDevicePresentationSupport glfwGetPhysicalDevicePresentationSupport;
		public PFN_glfwCreateWindowSurface glfwCreateWindowSurface;

		// glfw3native.h

		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetWin32Adapter([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetWin32Monitor([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("HWND")]
		public delegate IntPtr PFN_glfwGetWin32Window([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.Windows)]
		public PFN_glfwGetWin32Adapter glfwGetWin32Adapter;
		[ExternFunction(Platform = PlatformType.Windows)]
		public PFN_glfwGetWin32Monitor glfwGetWin32Monitor;
		[ExternFunction(Platform = PlatformType.Windows)]
		public PFN_glfwGetWin32Window glfwGetWin32Window;

		[return: NativeType("HGLRC")]
		public delegate IntPtr PFN_glfwGetWGLContext([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.Windows)]
		public PFN_glfwGetWGLContext glfwGetWGLContext;

		[return: NativeType("CGDirectDisplayID")]
		public delegate uint PFN_glfwGetCocoaMonitor([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("id")]
		public delegate IntPtr PFN_glfwGetCocoaWindow([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.MacOSX)]
		public PFN_glfwGetCocoaMonitor glfwGetCocoaMonitor;
		[ExternFunction(Platform = PlatformType.MacOSX)]
		public PFN_glfwGetCocoaWindow glfwGetCocoaWindow;

		[return: NativeType("id")]
		public delegate IntPtr PFN_glfwGetNSGLContext([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.MacOSX)]
		public PFN_glfwGetNSGLContext glfwGetNSGLContext;

		[return: NativeType("Display*")]
		public delegate IntPtr PFN_glfwGetX11Display();
		[return: NativeType("RRCrtc")]
		public delegate XID PFN_glfwGetX11Adapter([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("RROutput")]
		public delegate XID PFN_glfwGetX11Monitor([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("Window")]
		public delegate XID PFN_glfwGetX11Window([NativeType("GLFWwindow*")] IntPtr window);
		public delegate void PFN_glfwSetX11SelectionString([MarshalAs(UnmanagedType.LPStr)] string str);
		[return: NativeType("const char*")]
		public delegate IntPtr PFN_glfwGetX11SelectionString();

		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetX11Display glfwGetX11Display;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetX11Adapter glfwGetX11Adapter;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetX11Monitor glfwGetX11Monitor;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetX11Window glfwGetX11Window;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwSetX11SelectionString glfwSetX11SelectionString;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetX11SelectionString glfwGetX11SelectionString;

		[return: NativeType("GLXContext")]
		public delegate IntPtr PFN_glfwGetGLXContext([NativeType("GLFWwindow*")] IntPtr window);
		[return: NativeType("GLXWindow")]
		public delegate XID PFN_glfwGetGLXWindow([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetGLXContext glfwGetGLXContext;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetGLXWindow glfwGetGLXWindow;

		[return: NativeType("wl_display*")]
		public delegate IntPtr PFN_glfwGetWaylandDisplay();
		[return: NativeType("wl_output*")]
		public delegate IntPtr PFN_glfwGetWaylandMonitor([NativeType("GLFWmonitor*")] IntPtr monitor);
		[return: NativeType("wl_surface*")]
		public delegate IntPtr PFN_glfwGetWaylandWindow([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetWaylandDisplay glfwGetWaylandDisplay;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetWaylandMonitor glfwGetWaylandMonitor;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetWaylandWindow glfwGetWaylandWindow;

		[return: NativeType("EGLDisplay")]
		public delegate IntPtr PFN_glfwGetEGLDisplay();
		[return: NativeType("EGLContext")]
		public delegate IntPtr PFN_glfwGetEGLContext([NativeType("GLFWwindow*")] IntPtr window);
		[return: NativeType("EGLSurface")]
		public delegate IntPtr PFN_glfwGetEGLSurface([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Relaxed = true)]
		public PFN_glfwGetEGLDisplay glfwGetEGLDisplay;
		[ExternFunction(Relaxed = true)]
		public PFN_glfwGetEGLContext glfwGetEGLContext;
		[ExternFunction(Relaxed = true)]
		public PFN_glfwGetEGLSurface glfwGetEGLSurface;

		public delegate bool PFN_glfwGetOSMesaColorBuffer([NativeType("GLFWwindow*")] IntPtr window, out int width, out int height, out int format, [NativeType("void**")] out IntPtr buffer);
		public delegate bool PFN_glfwGetOSMesaDepthBuffer([NativeType("GLFWwindow*")] IntPtr window, out int width, out int height, out int bytesPerValue, [NativeType("void**")] out IntPtr buffer);
		[return: NativeType("OSMesaContext")]
		public delegate IntPtr PFN_glfwGetOSMesaContext([NativeType("GLFWwindow*")] IntPtr window);

		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetOSMesaColorBuffer glfwGetOSMesaColorBuffer;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetOSMesaDepthBuffer glfwGetOSMesaDepthBuffer;
		[ExternFunction(Platform = PlatformType.Linux, Relaxed = true)]
		public PFN_glfwGetOSMesaContext glfwGetOSMesaContext;

	}

}
