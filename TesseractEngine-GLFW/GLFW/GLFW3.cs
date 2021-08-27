using System;
using System.Collections.Generic;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.GLFW.Native;

namespace Tesseract.GLFW {

	public static class GLFW3 {

		public const int DontCare = -1;

		public static readonly LibrarySpec LibrarySpec = new() { Name = "glfw3" };

		private static Library library;
		public static Library Library {
			get {
				if (library == null) library = LibraryManager.Load(LibrarySpec);
				return library;
			}
		}

		private static GLFW3Functions functions;
		public static GLFW3Functions Functions {
			get {
				if (functions == null) {
					functions = new();
					Library.LoadFunctions(functions);
				}
				return functions;
			}
		}

		static GLFW3() {
			AppDomain.CurrentDomain.ProcessExit += (e, a) => Terminate();
		}

		public static bool Init() => Functions.glfwInit();

		public static void Terminate() => Functions.glfwTerminate();

		public static void InitHint(GLFWInitHint hint, int value) => Functions.glfwInitHint(hint, value);

		public static (int, int, int) Version {
			get {
				Functions.glfwGetVersion(out int major, out int minor, out int rev);
				return (major, minor, rev);
			}
		}

		public static string VersionString => MemoryUtil.GetUTF8(Functions.glfwGetVersionString());

		public static (GLFWError, string) GetError() {
			GLFWError err = Functions.glfwGetError(out IntPtr desc);
			return (err, MemoryUtil.GetUTF8(desc));
		}

		public static GLFWErrorFun ErrorCallback {
			set => Functions.glfwSetErrorCallback(value);
		}

		public static GLFWMonitor[] Monitors {
			get {
				UnmanagedPointer<IntPtr> pMonitors = new(Functions.glfwGetMonitors(out int count));
				GLFWMonitor[] monitors = new GLFWMonitor[count];
				for (int i = 0; i < count; i++) monitors[i] = new() { Monitor = pMonitors[i] };
				return monitors;
			}
		}

		public static GLFWMonitor PrimaryMonitor => new() { Monitor = Functions.glfwGetPrimaryMonitor() };

		public static GLFWMonitorFun MonitorCallback {
			set => Functions.glfwSetMonitorCallback(value);
		}

		public static void DefaultWindowHints() => Functions.glfwDefaultWindowHints();

		public static void WindowHint(GLFWWindowAttrib hint, int value) => Functions.glfwWindowHint(hint, value);

		public static void WindowHintString(GLFWWindowAttrib hint, string value) => Functions.glfwWindowHintString(hint, value);

		public static void PollEvents() => Functions.glfwPollEvents();

		public static void WaitEvents() => Functions.glfwWaitEvents();

		public static void WaitEvents(double timeout) => Functions.glfwWaitEventsTimeout(timeout);

		public static void PostEmptyEvent() => Functions.glfwPostEmptyEvent();

		public static bool RawMouseMotionSupported => Functions.glfwRawMouseMotionSupported();

		public static string GetKeyName(GLFWKey key, int scancode) => MemoryUtil.GetUTF8(Functions.glfwGetKeyName(key, scancode));

		public static int GetKeyScancode(GLFWKey key) => Functions.glfwGetKeyScancode(key);

		public const int MaxJoysticks = 16;

		public static GLFWJoystick[] Joysticks {
			get {
				List<GLFWJoystick> joysticks = new();
				for (int i = 0; i < MaxJoysticks; i++) if (Functions.glfwJoystickPresent(i)) joysticks.Add(new() { ID = i });
				return joysticks.ToArray();
			}
		}

		public static GLFWJoystickFun JoystickCallback {
			set => Functions.glfwSetJoystickCallback(value);
		}

		public static bool UpdateGamepadMappings(string str) => Functions.glfwUpdateGamepadMappings(str);

		public static string ClipboardString {
			get => MemoryUtil.GetUTF8(Functions.glfwGetClipboardString(IntPtr.Zero));
			set => Functions.glfwSetClipboardString(IntPtr.Zero, value);
		}

		public static double Time {
			get => Functions.glfwGetTime();
			set => Functions.glfwSetTime(value);
		}

		public static ulong TimerValue => Functions.glfwGetTimerValue();

		public static ulong TimerFrequency => Functions.glfwGetTimerFrequency();

		public static GLFWWindow CurrentContext {
			get {
				IntPtr ctx = Functions.glfwGetCurrentContext();
				return ctx == IntPtr.Zero ? null : new GLFWWindow(ctx);
			}
			set => Functions.glfwMakeContextCurrent(value != null ? value.Window : IntPtr.Zero);
		}

		public static int SwapInterval {
			set => Functions.glfwSwapInterval(value);
		}

		public static bool ExtensionSupported(string str) => Functions.glfwExtensionSupported(str);

		public static IntPtr GetProcAddress(string str) => Functions.glfwGetProcAddress(str);

		public static bool VulkanSupported => Functions.glfwVulkanSupported();

		public static string[] RequiredInstanceExtensions {
			get {
				UnmanagedPointer<IntPtr> pExts = new(Functions.glfwGetRequiredInstanceExtensions(out uint count));
				string[] exts = new string[count];
				for (int i = 0; i < count; i++) exts[i] = MemoryUtil.GetUTF8(pExts[i]);
				return exts;
			}
		}

	}
}
