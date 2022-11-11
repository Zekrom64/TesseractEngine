using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

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
		public VulkanAllocationCallbacks? Allocator { get; }

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
	public interface IVKDeviceObject : IPrimitiveHandle<ulong> {

		/// <summary>
		/// The type of the object.
		/// </summary>
		public VKObjectType ObjectType { get; }

		/// <summary>
		/// The device associated with this object.
		/// </summary>
		public VKDevice Device { get; }

		/// <summary>
		/// Shortcut for <see cref="VKDevice.GetPrivateData(VKObjectType, ulong, ulong)"/>.
		/// </summary>
		/// <param name="privateDataSlot">The private data slot to get</param>
		/// <returns>The private data in the slot</returns>
		public ulong GetPrivateData(ulong privateDataSlot) => Device.GetPrivateData(ObjectType, PrimitiveHandle, privateDataSlot);

		/// <summary>
		/// Shortcut for <see cref="VKDevice.SetPrivateData(VKObjectType, ulong, ulong, ulong)"/>.
		/// </summary>
		/// <param name="privateDataSlot">The private data slot to set</param>
		/// <param name="data">The private data to set</param>
		public void SetPrivateData(ulong privateDataSlot, ulong data) => Device.SetPrivateData(ObjectType, PrimitiveHandle, privateDataSlot, data);

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
		/// Gets the extent as reported by the surface. Swapchains created from this surface should attempt
		/// to match this extent.
		/// </summary>
		public Vector2i SurfaceExtent { get; }

		/// <summary>
		/// Creates a new Vulkan surface from this object.
		/// </summary>
		/// <param name="instance">The Vulkan instance</param>
		/// <param name="allocator">The allocator to use. This may be ignored by some implementations</param>
		/// <returns>A new surface targeting this object</returns>
		public VKSurfaceKHR CreateSurface(VKInstance instance, VulkanAllocationCallbacks? allocator = null);

	}

}
