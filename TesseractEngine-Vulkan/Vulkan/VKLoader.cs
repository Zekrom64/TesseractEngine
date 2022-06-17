using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	/// <summary>
	/// An implementation of a Vulkan loader that attempts to use a predefined loader library that should
	/// be installed on the system to access the Vulkan API. If such a loader library is not defined
	/// or does not exist an exception will be thrown when a loader is created.
	/// </summary>
	public class VulkanPlatformLoader : IVKLoader {

		private static string GetLibraryName() {
			// Follows GLFW's method for detecting the library name, although done at runtime
			// This avoids requiring another dependency just to load the Vulkan API
			return Platform.CurrentPlatformType switch {
				PlatformType.Windows => "vulkan-1.dll",
				PlatformType.MacOSX => "libvulkan.1.dylib",
				PlatformType.Linux => "libvulkan.1.so",
				_ => throw new InvalidOperationException("No known native Vulkan library for platform"),
			};
		}

		private readonly Library library;

		public VulkanPlatformLoader() {
			library = new Library(GetLibraryName());
		}

		public IntPtr GetVKProcAddress(string name) => library.GetExport(name);

	}

}
