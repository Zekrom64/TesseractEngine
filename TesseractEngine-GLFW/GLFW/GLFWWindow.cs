using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using System.Runtime.InteropServices;

namespace Tesseract.GLFW {

	public class GLFWWindow : IDisposable {

		[NativeType("GLFWwindow*")]
		public IntPtr Window { get; }

		public GLFWWindow(IntPtr window) {
			Window = window;
		}

		public GLFWWindow(Vector2i size, string title, GLFWMonitor monitor = default, GLFWWindow? share = null) {
			Span<byte> str = MemoryUtil.StackallocUTF8(title, stackalloc byte[1024]);
			unsafe {
				fixed (byte* pstr = str) {
					Window = GLFW3.Functions.glfwCreateWindow(size.X, size.Y, (IntPtr)pstr, monitor.Monitor, share != null ? share.Window : IntPtr.Zero);
				}
			}
		}

		public GLFWWindow(Vector2i size, ReadOnlySpan<byte> title, GLFWMonitor monitor = default, GLFWWindow? share = null) {
			unsafe {
				fixed (byte* pstr = title) {
					Window = GLFW3.Functions.glfwCreateWindow(size.X, size.Y, (IntPtr)pstr, monitor.Monitor, share != null ? share.Window : IntPtr.Zero);
				}
			}
		}

		public bool ShouldClose {
			get {
				unsafe {
					return GLFW3.Functions.glfwWindowShouldClose(Window);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowShouldClose(Window, value);
				}
			}
		}

		public string Title {
			set {
				Span<byte> str = MemoryUtil.StackallocUTF8(value, stackalloc byte[1024]);
				unsafe {
					fixed (byte* pstr = str) {
						GLFW3.Functions.glfwSetWindowTitle(Window, (IntPtr)pstr);
					}
				}
			}
		}

		public ReadOnlySpan<GLFWImage> Icon {
			set {
				unsafe {
					fixed (GLFWImage* pIcon = value) {
						GLFW3.Functions.glfwSetWindowIcon(Window, value.Length, pIcon);
					}
				}
			}
		}

		public Vector2i Pos {
			get {
				unsafe {
					GLFW3.Functions.glfwGetWindowPos(Window, out int x, out int y);
					return new(x, y);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowPos(Window, value.X, value.Y);
				}
			}
		}

		public Vector2i Size {
			get {
				unsafe {
					GLFW3.Functions.glfwGetWindowSize(Window, out int w, out int h);
					return new(w, h);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowSize(Window, value.X, value.Y);
				}
			}
		}

		public (Vector2i, Vector2i) SizeLimits {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowSizeLimits(Window, value.Item1.X, value.Item1.Y, value.Item2.X, value.Item2.Y);
				}
			}
		}

		public (int, int) AspectRatio {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowAspectRatio(Window, value.Item1, value.Item2);
				}
			}
		}

		public Vector2i FramebufferSize {
			get {
				unsafe {
					GLFW3.Functions.glfwGetFramebufferSize(Window, out int w, out int h);
					return new(w, h);
				}
			}
		}

		public Recti WindowFrame {
			get {
				unsafe {
					GLFW3.Functions.glfwGetWindowFrameSize(Window, out int left, out int top, out int right, out int bottom);
					return new(left, top, right - left, bottom - top);
				}
			}
		}

		public Vector2 ContentScale {
			get {
				unsafe {
					GLFW3.Functions.glfwGetWindowContentScale(Window, out float x, out float y);
					return new(x, y);
				}
			}
		}

		public float Opacity {
			get {
				unsafe {
					return GLFW3.Functions.glfwGetWindowOpacity(Window);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowOpacity(Window, value);
				}
			}
		}

		public void Iconify() {
			unsafe {
				GLFW3.Functions.glfwIconifyWindow(Window);
			}
		}

		public void Restore() {
			unsafe {
				GLFW3.Functions.glfwRestoreWindow(Window);
			}
		}

		public void Maximize() {
			unsafe {
				GLFW3.Functions.glfwMaximizeWindow(Window);
			}
		}

		public void Show() {
			unsafe {
				GLFW3.Functions.glfwShowWindow(Window);
			}
		}

		public void Hide() {
			unsafe {
				GLFW3.Functions.glfwHideWindow(Window);
			}
		}

		public void Focus() {
			unsafe {
				GLFW3.Functions.glfwFocusWindow(Window);
			}
		}

		public void RequestAttention() {
			unsafe {
				GLFW3.Functions.glfwRequestWindowAttention(Window);
			}
		}

		public GLFWMonitor Monitor {
			get {
				unsafe {
					return new() { Monitor = GLFW3.Functions.glfwGetWindowMonitor(Window) };
				}
			}
		}

		public void SetMonitor(GLFWMonitor monitor, Vector2i pos, Vector2i size, int refreshRate) {
			unsafe {
				GLFW3.Functions.glfwSetWindowMonitor(Window, monitor.Monitor, pos.X, pos.Y, size.X, size.Y, refreshRate);
			}
		}

		public int GetAttrib(GLFWWindowAttrib attrib) {
			unsafe {
				return GLFW3.Functions.glfwGetWindowAttrib(Window, attrib);
			}
		}

		public T GetAttrib<T>(GLFWWindowAttrib attrib) where T : Enum => (T)(object)GetAttrib(attrib);

		public void SetAttrib(GLFWWindowAttrib attrib, int value) {
			unsafe {
				GLFW3.Functions.glfwSetWindowAttrib(Window, attrib, value);
			}
		}

		public void SetAttrib<T>(GLFWWindowAttrib attrib, T value) where T : Enum => SetAttrib(attrib, (int)(object)value);

		public IntPtr UserPointer {
			get {
				unsafe {
					return GLFW3.Functions.glfwGetWindowUserPointer(Window);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowUserPointer(Window, value);
				}
			}
		}

		public GLFWWindowPosFun PosCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowPosCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowSizeFun SizeCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowSizeCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowCloseFun CloseCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowCloseCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowRefreshFun RefreshCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowRefreshCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowFocusFun FocusCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowFocusCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowIconifyFun IconifyCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowIconifyCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowMaximizeFun MaximizeCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowMaximizeCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWFramebufferSizeFun FramebufferSizeCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetFramebufferSizeCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWWindowContentScaleFun ContentScaleCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetWindowContentScaleCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public int GetInputMode(GLFWInputMode mode) {
			unsafe {
				return GLFW3.Functions.glfwGetInputMode(Window, mode);
			}
		}

		public T GetInputMode<T>(GLFWInputMode mode) where T : Enum => (T)(object)GetInputMode(mode);

		public void SetInputMode(GLFWInputMode mode, int value) {
			unsafe {
				GLFW3.Functions.glfwSetInputMode(Window, mode, value);
			}
		}

		public void SetInputMode<T>(GLFWInputMode mode, T value) where T : Enum => SetInputMode(mode, (int)(object)value);

		public GLFWButtonState GetKey(GLFWKey key) {
			unsafe {
				return GLFW3.Functions.glfwGetKey(Window, key);
			}
		}

		public GLFWButtonState GetMouseButton(int button) {
			unsafe {
				return GLFW3.Functions.glfwGetMouseButton(Window, button);
			}
		}

		public GLFWButtonState GetMouseButton(GLFWMouseButton button) {
			unsafe {
				return GLFW3.Functions.glfwGetMouseButton(Window, (int)button);
			}
		}

		public Vector2d CursorPos {
			get {
				unsafe {
					GLFW3.Functions.glfwGetCursorPos(Window, out double x, out double y);
					return new(x, y);
				}
			}
			set {
				unsafe {
					GLFW3.Functions.glfwSetCursorPos(Window, value.X, value.Y);
				}
			}
		}

		public GLFWCursor? Cursor {
			set {
				unsafe {
					GLFW3.Functions.glfwSetCursor(Window, value != null ? value.Cursor : IntPtr.Zero);
				}
			}
		}

		public GLFWKeyFun KeyCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetKeyCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWCharFun CharCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetCharCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWCharModsFun CharModsCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetCharModsCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWMouseButtonFun MouseButtonCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetMouseButtonCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWCursorPosFun CursorPosCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetCursorPosCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWCursorEnterFun CursorEnterCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetCursorEnterCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWScrollFun ScrollCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetScrollCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public GLFWDropFun DropCallback {
			set {
				unsafe {
					GLFW3.Functions.glfwSetDropCallback(Window, Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public string? Clipboard {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(GLFW3.Functions.glfwGetClipboardString(Window));
				}
			}
			set {
				Span<byte> str = MemoryUtil.StackallocUTF8(value ?? string.Empty, stackalloc byte[1024]);
				unsafe {
					fixed (byte* ptr = str) {
						GLFW3.Functions.glfwSetClipboardString(Window, (IntPtr)ptr);
					}
				}
			}
		}

		public void SwapBuffers() {
			unsafe {
				GLFW3.Functions.glfwSwapBuffers(Window);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				GLFW3.Functions.glfwDestroyWindow(Window);
			}
		}

	}
}
