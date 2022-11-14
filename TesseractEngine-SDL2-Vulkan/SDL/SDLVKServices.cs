using Tesseract.Core.Graphics;
using Tesseract.Core.Services;
using Tesseract.Vulkan;
using Tesseract.SDL.Services;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Graphics;

namespace Tesseract.SDL {

	public class SDLServiceWindowVKSurfaceProvider : IVKSurfaceProvider {

		public SDLServiceWindow Window { get; }

		public string[] RequiredInstanceExtensions {
			get {
				if (!SDL2.Functions.SDL_Vulkan_GetInstanceExtensions(Window.Window.Window.Ptr, out int count, out IntPtr names)) throw new SDLException(SDL2.GetError());
				UnmanagedPointer<IntPtr> pNames = new(names);
				string[] exts = new string[count];
				for (int i = 0; i < count; i++) exts[i] = MemoryUtil.GetUTF8(pNames[i])!;
				return exts;
			}
		}

		public Vector2i SurfaceExtent => Window.Size;

		public SDLServiceWindowVKSurfaceProvider(SDLServiceWindow window) {
			Window = window;
		}

		public VKSurfaceKHR CreateSurface(VKInstance instance, VulkanAllocationCallbacks? allocator = null) {
			if (!SDL2.Functions.SDL_Vulkan_CreateSurface(Window.Window.Window.Ptr, instance, out ulong surface)) throw new SDLException(SDL2.GetError());
			return new VKSurfaceKHR(instance, surface, null);
		}

	}

	/// <summary>
	/// Service manager for SDL2's Vulkan features.
	/// </summary>
	public static class SDLVKServices {

		/// <summary>
		/// Registers all SDL2 Vulkan services.
		/// </summary>
		public static void Register() {
			// Inject surface provider into the window service
			ServiceInjector.Inject(VKServices.SurfaceProvider, (SDLServiceWindow window) => new SDLServiceWindowVKSurfaceProvider(window));
			// Inject window setup code
			SDLServiceWindow.OnParseAttributes += (WindowAttributeList attributes, ref SDLWindowFlags flags) => {
				if (attributes.TryGet(VulkanWindowAttributes.VulkanWindow, out bool vkwindow) && vkwindow) flags |= SDLWindowFlags.Vulkan;
			};
		}

	}
}
