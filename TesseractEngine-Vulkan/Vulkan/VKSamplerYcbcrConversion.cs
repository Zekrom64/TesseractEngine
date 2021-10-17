using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {
	
	public class VKSamplerYcbcrConversion : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		[NativeType("VkSamplerYcbcrConversion")]
		public ulong SamplerYcbcrConversion { get; }

		public ulong PrimitiveHandle => SamplerYcbcrConversion;

		public VKSamplerYcbcrConversion(VKDevice device, ulong ycbcrConversion, VulkanAllocationCallbacks allocator = null) {
			Device = device;
			SamplerYcbcrConversion = ycbcrConversion;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Device.VK11Functions) Device.VK11Functions.vkDestroySamplerYcbcrConversion(Device, SamplerYcbcrConversion, Allocator);
			else Device.KHRSamplerYcbcrConversion.vkDestroySamplerYcbcrConversionKHR(Device, SamplerYcbcrConversion, Allocator);
		}

	}

}
