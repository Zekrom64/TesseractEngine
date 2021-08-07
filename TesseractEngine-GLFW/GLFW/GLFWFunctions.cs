using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {

	public delegate void GLFWErrorFun(int error, [MarshalAs(UnmanagedType.LPUTF8Str)] string description);

	public delegate void GLFWWindowPosFun([NativeType("GLFWwindow*")] IntPtr window, int x, int y);

	public delegate void GLFWWindowSizeFun([NativeType("GLFWwindow*")] IntPtr window, int w, int h);

	public delegate void GLFWWindowCloseFun([NativeType("GLFWwindow*")] IntPtr window);

	public delegate void GLFWWindowRefreshFun([NativeType("GLFWwindow*")] IntPtr window);

	public delegate void GLFWWindowFocusFun([NativeType("GLFWwindow*")] IntPtr window, int focused);

	public delegate void GLFWWindowIconifyFun([NativeType("GLFWwindow*")] IntPtr window, int iconified);

	public delegate void GLFWWindowMaximizeFun([NativeType("GLFWwindow*")] IntPtr window, int maximized);

	public delegate void GLFWFramebufferSizeFun([NativeType("GLFWwindow*")] IntPtr window, int w, int h);

	public delegate void GLFWWindowContentScaleFun([NativeType("GLFWwindow*")] IntPtr window, float xscale, float yscale);

	public delegate void GLFWMouseButtonFun([NativeType("GLFWwindow*")] IntPtr window, int button, GLFWButtonState action, GLFWKeyMod mods);

	public delegate void GLFWCursorPosFun([NativeType("GLFWwindow*")] IntPtr window, double x, double y);

	public delegate void GLFWCursorEnterFun([NativeType("GLFWwindow*")] IntPtr window, int entered);

	public delegate void GLFWScrollFun([NativeType("GLFWwindow*")] IntPtr window, double x, double y);

	public delegate void GLFWKeyFun([NativeType("GLFWwindow*")] IntPtr window, GLFWKey key, int scancode, GLFWButtonState action, GLFWKeyMod mods);

	public delegate void GLFWCharFun([NativeType("GLFWwindow*")] IntPtr window, uint codepoint);

	public delegate void GLFWCharModsFun([NativeType("GLFWwindow*")] IntPtr window, uint codepoint, GLFWKeyMod mods);

	public delegate void GLFWDropFun([NativeType("GLFWwindow*")] IntPtr window, int pathCount, [NativeType("const char*[]")] IntPtr paths);

	public delegate void GLFWMonitorFun([NativeType("GLFWmonitor*")] IntPtr window, GLFWConnectState state);

	public delegate void GLFWJoystickFun(int jid, GLFWConnectState state);

}
