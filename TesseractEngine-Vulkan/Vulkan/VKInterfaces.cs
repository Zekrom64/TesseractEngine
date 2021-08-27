using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	/// <summary>
	/// A Vulkan loader provides a method to load Vulkan API functions.
	/// </summary>
	public interface IVKLoader {

		/// <summary>
		/// Gets the address of a Vulkan API function given its name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IntPtr GetVKProcAddress(string name);

	}

	/// <summary>
	/// An allocated object stores a reference to the allocation callbacks used during its creation.
	/// </summary>
	public interface IVKAllocatedObject {

		/// <summary>
		/// The allocator used to create this object.
		/// </summary>
		public VulkanAllocationCallbacks Allocator { get; }

	}

	/// <summary>
	/// An instance object stores a reference to the instance it is associated with.
	/// </summary>
	public interface IVKInstanceObject {

		/// <summary>
		/// The instance associated with this object.
		/// </summary>
		public VKInstance Instance { get; }

	}

	/// <summary>
	/// A device object stores a reference to the device it is associated with.
	/// </summary>
	public interface IVKDeviceObject {

		/// <summary>
		/// The device associated with this object.
		/// </summary>
		public VKDevice Device { get; }

	}

	/// <summary>
	/// A Vulkan surface provider has a method to create Vulkan surfaces targeting itself. This is
	/// normally implemented by Vulkan-capable windows.
	/// </summary>
	public interface IVKSurfaceProvider {

		/// <summary>
		/// The Vulkan instance extensions required to use the surface provider.
		/// </summary>
		public string[] RequiredInstanceExtensions { get; }

		/// <summary>
		/// Creates a new Vulkan surface from this object.
		/// </summary>
		/// <param name="instance">The Vulkan instance</param>
		/// <param name="allocator">The allocator to use. This may be ignored by some implementations</param>
		/// <returns>A new surface targeting this object</returns>
		public VKSurfaceKHR CreateSurface(VKInstance instance, VulkanAllocationCallbacks allocator = null);

	}

}
