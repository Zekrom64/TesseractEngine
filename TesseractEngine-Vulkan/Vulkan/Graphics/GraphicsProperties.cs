using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;
using Tesseract.Vulkan.Graphics.Impl;

namespace Tesseract.Vulkan.Graphics {

	public interface IVulkanMemory {

		public ulong TotalVideoMemory { get; }

		public ulong TotalDeviceMemory { get; }

		public ulong TotalCommittedMemory { get; }

	}

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
					totalLocal = totalVisible;
					break;
			}

			TotalDeviceMemory = totalLocal;
			TotalVideoMemory = totalVisible;
		}

	}

	public class VulkanGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.Vulkan;

		public string TypeInfo => "Tesseract Vulkan Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.Concurrent;

		private readonly IVulkanMemory memory;

		public ulong TotalVideoMemory => memory.TotalVideoMemory;

		public ulong TotalDeviceMemory => memory.TotalDeviceMemory;

		public ulong TotalCommittedMemory => memory.TotalCommittedMemory;

		public CoordinateSystem CoordinateSystem => CoordinateSystem.RightHanded;

		public VulkanGraphicsProperties(VulkanPhysicalDeviceInfo device, VulkanMemory? memory = null) {
			var props = device.PhysicalDevice.Properties;
			RendererName = props.DeviceName;
			VendorName = props.VendorID.ToString();
			this.memory = memory != null ? memory : new VulkanDeviceMemory(device);
		}

	}

}
