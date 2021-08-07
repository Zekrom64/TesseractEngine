using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.GLFW {

	public class GLFWWindow : IDisposable {

		[NativeType("GLFWwindow*")]
		public IntPtr Window { get; }

		public GLFWWindow(IntPtr window) {
			Window = window;
		}

		public GLFWWindow(Vector2i size, string title, GLFWMonitor monitor = default, GLFWWindow share = null) {
			Window = GLFW3.Functions.glfwCreateWindow(size.X, size.Y, title, monitor.Monitor, share != null ? share.Window : IntPtr.Zero);
		}

		public bool ShouldClose {
			get => GLFW3.Functions.glfwWindowShouldClose(Window);
			set => GLFW3.Functions.glfwSetWindowShouldClose(Window, value);
		}

		public string Title {
			set => GLFW3.Functions.glfwSetWindowTitle(Window, value);
		}

		public ReadOnlySpan<GLFWImage> Icon {
			set {
				unsafe {
					fixed (GLFWImage* pIcon = value) {
						GLFW3.Functions.glfwSetWindowIcon(Window, value.Length, (IntPtr)pIcon);
					}
				}
			}
		}

		public Vector2i Pos {
			get {
				GLFW3.Functions.glfwGetWindowPos(Window, out int x, out int y);
				return new(x, y);
			}
			set => GLFW3.Functions.glfwSetWindowPos(Window, value.X, value.Y);
		}

		public Vector2i Size {
			get {
				GLFW3.Functions.glfwGetWindowSize(Window, out int w, out int h);
				return new(w, h);
			}
			set => GLFW3.Functions.glfwSetWindowSize(Window, value.X, value.Y);
		}

		public (Vector2i, Vector2i) SizeLimits {
			set => GLFW3.Functions.glfwSetWindowSizeLimits(Window, value.Item1.X, value.Item1.Y, value.Item2.X, value.Item2.Y);
		}

		public (int, int) AspectRatio {
			set => GLFW3.Functions.glfwSetWindowAspectRatio(Window, value.Item1, value.Item2);
		}

		public Vector2i FramebufferSize {
			get {
				GLFW3.Functions.glfwGetFramebufferSize(Window, out int w, out int h);
				return new(w, h);
			}
		}

		public Recti WindowFrame {
			get {
				GLFW3.Functions.glfwGetWindowFrameSize(Window, out int left, out int top, out int right, out int bottom);
				return new(left, top, right - left, bottom - top);
			}
		}

		public Vector2 ContentScale {
			get {
				GLFW3.Functions.glfwGetWindowContentScale(Window, out float x, out float y);
				return new(x, y);
			}
		}

		public float Opacity {
			get => GLFW3.Functions.glfwGetWindowOpacity(Window);
			set => GLFW3.Functions.glfwSetWindowOpacity(Window, value);
		}

		public void Iconify() => GLFW3.Functions.glfwIconifyWindow(Window);

		public void Restore() => GLFW3.Functions.glfwRestoreWindow(Window);

		public void Maximize() => GLFW3.Functions.glfwMaximizeWindow(Window);

		public void Show() => GLFW3.Functions.glfwShowWindow(Window);

		public void Hide() => GLFW3.Functions.glfwHideWindow(Window);

		public void Focus() => GLFW3.Functions.glfwFocusWindow(Window);

		public void RequestAttention() => GLFW3.Functions.glfwRequestWindowAttention(Window);

		public GLFWMonitor Monitor {
			get => new() { Monitor = GLFW3.Functions.glfwGetWindowMonitor(Window) };
		}

		public void SetMonitor(GLFWMonitor monitor, Vector2i pos, Vector2i size, int refreshRate) =>
			GLFW3.Functions.glfwSetWindowMonitor(Window, monitor.Monitor, pos.X, pos.Y, size.X, size.Y, refreshRate);

		public int GetAttrib(GLFWWindowAttrib attrib) => GLFW3.Functions.glfwGetWindowAttrib(Window, attrib);

		public T GetAttrib<T>(GLFWWindowAttrib attrib) where T : Enum => (T)(object)GetAttrib(attrib);

		public void SetAttrib(GLFWWindowAttrib attrib, int value) => GLFW3.Functions.glfwSetWindowAttrib(Window, attrib, value);

		public void SetAttrib<T>(GLFWWindowAttrib attrib, T value) where T : Enum => SetAttrib(attrib, (int)(object)value);

		public IntPtr UserPointer {
			get => GLFW3.Functions.glfwGetWindowUserPointer(Window);
			set => GLFW3.Functions.glfwSetWindowUserPointer(Window, value);
		}

		public GLFWWindowPosFun PosCallback {
			set => GLFW3.Functions.glfwSetWindowPosCallback(Window, value);
		}

		public GLFWWindowSizeFun SizeCallback {
			set => GLFW3.Functions.glfwSetWindowSizeCallback(Window, value);
		}

		public GLFWWindowCloseFun CloseCallback {
			set => GLFW3.Functions.glfwSetWindowCloseCallback(Window, value);
		}

		public GLFWWindowRefreshFun RefreshCallback {
			set => GLFW3.Functions.glfwSetWindowRefreshCallback(Window, value);
		}

		public GLFWWindowFocusFun FocusCallback {
			set => GLFW3.Functions.glfwSetWindowFocusCallback(Window, value);
		}

		public GLFWWindowIconifyFun IconifyCallback {
			set => GLFW3.Functions.glfwSetWindowIconifyCallback(Window, value);
		}

		public GLFWWindowMaximizeFun MaximizeCallback {
			set => GLFW3.Functions.glfwSetWindowMaximizeCallback(Window, value);
		}

		public GLFWFramebufferSizeFun FramebufferSizeCallback {
			set => GLFW3.Functions.glfwSetFramebufferSizeCallback(Window, value);
		}

		public GLFWWindowContentScaleFun ContentScaleCallback {
			set => GLFW3.Functions.glfwSetWindowContentScaleCallback(Window, value);
		}

		public int GetInputMode(GLFWInputMode mode) => GLFW3.Functions.glfwGetInputMode(Window, mode);

		public T GetInputMode<T>(GLFWInputMode mode) where T : Enum => (T)(object)GetInputMode(mode);

		public void SetInputMode(GLFWInputMode mode, int value) => GLFW3.Functions.glfwSetInputMode(Window, mode, value);

		public void SetInputMode<T>(GLFWInputMode mode, T value) where T : Enum => SetInputMode(mode, (int)(object)value);

		public GLFWButtonState GetKey(GLFWKey key) => GLFW3.Functions.glfwGetKey(Window, key);

		public GLFWButtonState GetMouseButton(int button) => GLFW3.Functions.glfwGetMouseButton(Window, button);

		public GLFWButtonState GetMouseButton(GLFWMouseButton button) => GLFW3.Functions.glfwGetMouseButton(Window, (int)button);

		public Vector2d CursorPos {
			get {
				GLFW3.Functions.glfwGetCursorPos(Window, out double x, out double y);
				return new(x, y);
			}
			set => GLFW3.Functions.glfwSetCursorPos(Window, value.X, value.Y);
		}

		public GLFWCursor Cursor {
			set => GLFW3.Functions.glfwSetCursor(Window, value != null ? value.Cursor : IntPtr.Zero);
		}

		public GLFWKeyFun KeyCallback {
			set => GLFW3.Functions.glfwSetKeyCallback(Window, value);
		}

		public GLFWCharFun CharCallback {
			set => GLFW3.Functions.glfwSetCharCallback(Window, value);
		}

		public GLFWCharModsFun CharModsCallback {
			set => GLFW3.Functions.glfwSetCharModsCallback(Window, value);
		}

		public GLFWMouseButtonFun MouseButtonCallback {
			set => GLFW3.Functions.glfwSetMouseButtonCallback(Window, value);
		}

		public GLFWCursorPosFun CursorPosCallback {
			set => GLFW3.Functions.glfwSetCursorPosCallback(Window, value);
		}

		public GLFWCursorEnterFun CursorEnterCallback {
			set => GLFW3.Functions.glfwSetCursorEnterCallback(Window, value);
		}

		public GLFWScrollFun ScrollCallback {
			set => GLFW3.Functions.glfwSetScrollCallback(Window, value);
		}

		public GLFWDropFun DropCallback {
			set => GLFW3.Functions.glfwSetDropCallback(Window, value);
		}

		public void SwapBuffers() => GLFW3.Functions.glfwSwapBuffers(Window);

		public void Dispose() {
			GC.SuppressFinalize(this);
			GLFW3.Functions.glfwDestroyWindow(Window);
		}

	}
}
