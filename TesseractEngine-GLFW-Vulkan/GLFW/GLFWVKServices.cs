using Tesseract.Core.Services;
using Tesseract.Vulkan;
using Tesseract.GLFW.Services;
using Tesseract.Core.Graphics;
using Tesseract.GLFW;
using Tesseract.Core.Numerics;
using Tesseract.Vulkan.Services;
using Tesseract.Core.Native;

namespace Tesseract.SDL {

	public class GLFWServiceWindowVKSurfaceProvider : IVKSurfaceProvider {

		public GLFWServiceWindow Window { get; }

		public string[] RequiredInstanceExtensions {
			get {
				IntPtr names = GLFW3.Functions.glfwGetRequiredInstanceExtensions(out uint count);
				UnmanagedPointer<IntPtr> pNames = new(names);
				string[] exts = new string[count];
				for (int i = 0; i < count; i++) exts[i] = MemoryUtil.GetUTF8(pNames[i])!;
				return exts;
			}
		}

		public Vector2i SurfaceExtent => Window.Size;

		public GLFWServiceWindowVKSurfaceProvider(GLFWServiceWindow window) {
			Window = window;
		}

		public VKSurfaceKHR CreateSurface(VKInstance instance, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError((VKResult)GLFW3.Functions.glfwCreateWindowSurface(instance, Window.Window.Window, allocator, out ulong surface), "Failed to create window surface");
			return new VKSurfaceKHR(instance, surface, allocator);
		}
	}

	/// <summary>
	/// Service manager for GLFW's Vulkan features.
	/// </summary>
	public static class GLFWVKServices {

		/// <summary>
		/// Registers all GLFW Vulkan services.
		/// </summary>
		public static void Register() {
			ServiceInjector.Inject(VKServices.SurfaceProvider, (GLFWServiceWindow window) => new GLFWServiceWindowVKSurfaceProvider(window));
			GLFWServiceWindow.OnParseAttributes += (WindowAttributeList attributes) => {
				if (attributes.TryGet(VulkanWindowAttributes.VulkanWindow, out bool vkwindow) && vkwindow) GLFW3.WindowHint(GLFWWindowAttrib.ClientAPI, (int)GLFWClientAPI.NoAPI);
			};
		}

	}
}
