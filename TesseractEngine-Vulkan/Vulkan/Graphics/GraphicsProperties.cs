using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;
using Tesseract.Vulkan.Graphics.Impl;

namespace Tesseract.Vulkan.Graphics {
	public class VulkanGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.Vulkan;

		public string TypeInfo => "Tesseract Vulkan Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.Concurrent;

		private readonly VulkanMemory memory;

		public ulong TotalVideoMemory => memory.TotalVisibleBytes;

		public ulong TotalDeviceMemory => memory.TotalLocalBytes;

		public ulong TotalCommittedMemory => memory.TotalUsedBytes;

		public VulkanGraphicsProperties(VulkanDevice device, VulkanMemory memory) {
			var props = device.PhysicalDevice.Properties;
			RendererName = props.DeviceName;
			VendorName = props.VendorID.ToString();
			this.memory = memory;
		}

	}

}
