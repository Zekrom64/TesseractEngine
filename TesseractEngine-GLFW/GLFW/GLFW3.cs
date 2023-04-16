using System;
using System.Collections.Generic;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.GLFW.Native;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.PixelFormats;

namespace Tesseract.GLFW {

	public static class GLFW3 {

		public const int DontCare = -1;

		public static readonly LibrarySpec LibrarySpec = new() { Name = "glfw3" };

		private static Library? library;
		public static Library Library {
			get {
				library ??= LibraryManager.Load(LibrarySpec);
				return library;
			}
		}

		private static GLFW3Functions? functions;
		public static GLFW3Functions Functions {
			get {
				if (functions == null) {
					functions = new();
					Library.LoadFunctions(functions);
				}
				return functions;
			}
		}

		public static bool Init() {
			unsafe {
				return Functions.glfwInit();
			}
		}

		public static void Terminate() {
			unsafe {
				Functions.glfwTerminate();
			}
		}

		public static void InitHint(GLFWInitHint hint, int value) {
			unsafe {
				Functions.glfwInitHint(hint, value);
			}
		}

		public static (int, int, int) Version {
			get {
				unsafe {
					Functions.glfwGetVersion(out int major, out int minor, out int rev);
					return (major, minor, rev);
				}
			}
		}

		public static string VersionString {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(Functions.glfwGetVersionString())!;
				}
			}
		}

		public static (GLFWError, string) GetError() {
			unsafe {
				GLFWError err = Functions.glfwGetError(out IntPtr desc);
				return (err, MemoryUtil.GetUTF8(desc)!);
			}
		}

		public static GLFWErrorFun ErrorCallback {
			set {
				unsafe {
					Functions.glfwSetErrorCallback(Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public static GLFWMonitor[] Monitors {
			get {
				unsafe {
					UnmanagedPointer<IntPtr> pMonitors = new(Functions.glfwGetMonitors(out int count));
					GLFWMonitor[] monitors = new GLFWMonitor[count];
					for (int i = 0; i < count; i++) monitors[i] = new() { Monitor = pMonitors[i] };
					return monitors;
				}
			}
		}

		public static GLFWMonitor PrimaryMonitor {
			get {
				unsafe {
					return new() { Monitor = Functions.glfwGetPrimaryMonitor() };
				}
			}
		}

		public static GLFWMonitorFun MonitorCallback {
			set {
				unsafe {
					Functions.glfwSetMonitorCallback(Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public static void DefaultWindowHints() {
			unsafe {
				Functions.glfwDefaultWindowHints();
			}
		}

		public static void WindowHint(GLFWWindowAttrib hint, int value) {
			unsafe {
				Functions.glfwWindowHint(hint, value);
			}
		}

		public static void WindowHintString(GLFWWindowAttrib hint, string value) {
			Span<byte> str = MemoryUtil.StackallocUTF8(value, stackalloc byte[1024]);
			unsafe {
				fixed (byte* pstr = str) {
					Functions.glfwWindowHintString(hint, (IntPtr)pstr);
				}
			}
		}

		public static void WindowHintString(GLFWWindowAttrib hint, ReadOnlySpan<byte> value) {
			unsafe {
				fixed (byte* pstr = value) {
					Functions.glfwWindowHintString(hint, (IntPtr)pstr);
				}
			}
		}

		public static void PollEvents() {
			unsafe {
				Functions.glfwPollEvents();
			}
		}

		public static void WaitEvents() {
			unsafe {
				Functions.glfwWaitEvents();
			}
		}

		public static void WaitEvents(double timeout) {
			unsafe {
				Functions.glfwWaitEventsTimeout(timeout);
			}
		}

		public static void PostEmptyEvent() {
			unsafe {
				Functions.glfwPostEmptyEvent();
			}
		}

		public static bool RawMouseMotionSupported {
			get {
				unsafe {
					return Functions.glfwRawMouseMotionSupported();
				}
			}
		}

		public static string? GetKeyName(GLFWKey key, int scancode) {
			unsafe {
				return MemoryUtil.GetUTF8(Functions.glfwGetKeyName(key, scancode));
			}
		}

		public static int GetKeyScancode(GLFWKey key) {
			unsafe {
				return Functions.glfwGetKeyScancode(key);
			}
		}

		public const int MaxJoysticks = 16;

		public static GLFWJoystick[] Joysticks {
			get {
				List<GLFWJoystick> joysticks = new();
				unsafe {
					for (int i = 0; i < MaxJoysticks; i++) {
						if (Functions.glfwJoystickPresent(i)) joysticks.Add(new() { ID = i });
					}
				}
				return joysticks.ToArray();
			}
		}

		public static GLFWJoystickFun JoystickCallback {
			set {
				unsafe {
					Functions.glfwSetJoystickCallback(Marshal.GetFunctionPointerForDelegate(value));
				}
			}
		}

		public static bool UpdateGamepadMappings(string str) {
			unsafe {
				Span<byte> bytes = MemoryUtil.StackallocUTF8(str, stackalloc byte[4096]);
				fixed (byte* pBytes = bytes) {
					return Functions.glfwUpdateGamepadMappings((IntPtr)pBytes);
				}
			}
		}

		public static string? ClipboardString {
			get {
				unsafe {
					return MemoryUtil.GetUTF8(Functions.glfwGetClipboardString(IntPtr.Zero));
				}
			}
			set {
				Span<byte> str = MemoryUtil.StackallocUTF8(value ?? string.Empty, stackalloc byte[1024]);
				unsafe {
					fixed (byte* ptr = str) {
						GLFW3.Functions.glfwSetClipboardString(IntPtr.Zero, (IntPtr)ptr);
					}
				}
			}
		}

		public static double Time {
			get {
				unsafe {
					return Functions.glfwGetTime();
				}
			}
			set {
				unsafe {
					Functions.glfwSetTime(value);
				}
			}
		}

		public static ulong TimerValue {
			get {
				unsafe {
					return Functions.glfwGetTimerValue();
				}
			}
		}

		public static ulong TimerFrequency {
			get {
				unsafe {
					return Functions.glfwGetTimerFrequency();
				}
			}
		}

		public static GLFWWindow? CurrentContext {
			get {
				unsafe {
					IntPtr ctx = Functions.glfwGetCurrentContext();
					return ctx == IntPtr.Zero ? null : new GLFWWindow(ctx);
				}
			}
			set {
				unsafe {
					Functions.glfwMakeContextCurrent(value != null ? value.Window : IntPtr.Zero);
				}
			}
		}

		public static int SwapInterval {
			set {
				unsafe {
					Functions.glfwSwapInterval(value);
				}
			}
		}

		public static bool ExtensionSupported(ReadOnlySpan<byte> str) {
			unsafe {
				fixed (byte* ptr = str) {
					return Functions.glfwExtensionSupported((IntPtr)ptr);
				}
			}
		}

		public static bool ExtensionSupported(string str) {
			Span<byte> bytes = MemoryUtil.StackallocUTF8(str, stackalloc byte[1024]);
			unsafe {
				fixed (byte* ptr = bytes) {
					return Functions.glfwExtensionSupported((IntPtr)ptr);
				}
			}
		}

		public static IntPtr GetProcAddress(ReadOnlySpan<byte> str) {
			unsafe {
				fixed (byte* ptr = str) {
					return Functions.glfwGetProcAddress((IntPtr)ptr);
				}
			}
		}

		public static IntPtr GetProcAddress(string str) {
			Span<byte> bytes = MemoryUtil.StackallocUTF8(str, stackalloc byte[1024]);
			unsafe {
				fixed (byte* ptr = bytes) {
					return Functions.glfwGetProcAddress((IntPtr)ptr);
				}
			}
		}

		public static bool VulkanSupported {
			get {
				unsafe {
					return Functions.glfwVulkanSupported();
				}
			}
		}

		public static string[] RequiredInstanceExtensions {
			get {
				unsafe {
					UnmanagedPointer<IntPtr> pExts = new(Functions.glfwGetRequiredInstanceExtensions(out uint count));
					string[] exts = new string[count];
					for (int i = 0; i < count; i++) exts[i] = MemoryUtil.GetUTF8(pExts[i])!;
					return exts;
				}
			}
		}

	}
}
