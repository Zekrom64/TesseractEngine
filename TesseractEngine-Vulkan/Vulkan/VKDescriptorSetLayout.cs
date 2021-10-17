using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDescriptorSetLayout : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkDescriptorSetLayout")]
		public ulong DescriptorSetLayout { get; }

		public ulong PrimitiveHandle => DescriptorSetLayout;

		public VulkanAllocationCallbacks Allocator { get; }

		public VKDescriptorSetLayout(VKDevice device, ulong descriptorSetLayout, VulkanAllocationCallbacks allocator) {
			Device = device;
			DescriptorSetLayout = descriptorSetLayout;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyDescriptorSetLayout(Device, DescriptorSetLayout, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKDescriptorSetLayout layout) => layout != null ? layout.DescriptorSetLayout : 0;

	}

}
