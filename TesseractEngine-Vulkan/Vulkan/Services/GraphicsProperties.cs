using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Utilities;
using Tesseract.Vulkan.Services.Objects;

namespace Tesseract.Vulkan.Services {

	/// <summary>
	/// An interface for objects which can provide memory information.
	/// </summary>
	public interface IVulkanMemory {

		/// <summary>
		/// The total amount of video memory, as defined in <see cref="IGraphicsProperites.TotalVideoMemory"/>.
		/// </summary>
		public ulong TotalVideoMemory { get; }

		/// <summary>
		/// The total amount of device-local memory, as defined in <see cref="IGraphicsProperites.TotalDeviceMemory"/>.
		/// </summary>
		public ulong TotalDeviceMemory { get; }

		/// <summary>
		/// The total amount of committed memory, as defined in <see cref="IGraphicsProperites.TotalCommittedMemory"/>.
		/// </summary>
		public ulong TotalCommittedMemory { get; }

	}

	/// <summary>
	/// An implementation of <see cref="IVulkanMemory"/> which reflects the values provided by a physical device.
	/// </summary>
	public class VulkanDeviceMemory : IVulkanMemory {

		public ulong TotalVideoMemory { get; }

		public ulong TotalDeviceMemory { get; }

		public ulong TotalCommittedMemory { get; } = 0;

		public VulkanDeviceMemory(VulkanPhysicalDeviceInfo device) {
			// Enumerate memory heaps and count available memory
			ulong totalLocal = 0;
			ulong totalVisible = 0;
			var memProps = device.PhysicalDevice.MemoryProperties;
			var heaps = memProps.MemoryHeaps;
			for (int i = 0; i < memProps.MemoryHeapCount; i++) {
				var heap = heaps[i];
				totalVisible += heap.Size;
				if ((heap.Flags & VKMemoryHeapFlagBits.DeviceLocal) != 0) totalLocal += heap.Size;
			}

			// Adjust values based on device types before finalizing values
			switch (device.PhysicalDevice.Properties.DeviceType) {
				case VKPhysicalDeviceType.CPU:
				case VKPhysicalDeviceType.IntegratedGPU:
					// If the device is a CPU or an integrated GPU all memory is technically local
					totalLocal = totalVisible;
					break;
			}

			TotalDeviceMemory = totalLocal;
			TotalVideoMemory = totalVisible;
		}

	}

	/// <summary>
	/// Vulkan graphics properties instance.
	/// </summary>
	public class VulkanGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.Vulkan;

		public string TypeInfo => "Tesseract Vulkan Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.Concurrent;

		public readonly IVulkanMemory Memory;

		public ulong TotalVideoMemory => Memory.TotalVideoMemory;

		public ulong TotalDeviceMemory => Memory.TotalDeviceMemory;

		public ulong TotalCommittedMemory => Memory.TotalCommittedMemory;

		public CoordinateSystem CoordinateSystem => CoordinateSystem.RightHanded;

		/// <summary>
		/// Creates a new set of Vulkan graphics properties for the given physical device, with
		/// either a provided memory interface or initializes one from the physical device.
		/// </summary>
		/// <param name="device">The Vulkan physical device</param>
		/// <param name="memory">The memory interface, or null</param>
		public VulkanGraphicsProperties(VulkanPhysicalDeviceInfo device, IVulkanMemory? memory = null) {
			var props = device.PhysicalDevice.Properties;
			RendererName = props.DeviceName;
			VendorName = props.VendorID.ToString();
			this.Memory = memory ?? new VulkanDeviceMemory(device);
		}

	}

}
