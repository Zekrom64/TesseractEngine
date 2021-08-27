using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan.Native {

	using VkBool32 = Boolean;
	using VkDeviceSize = UInt64;
	using VkSampleMask = UInt32;
	using VkInstance = IntPtr;
	using VkPhysicalDevice = IntPtr;
	using VkDevice = IntPtr;
	using VkQueue = IntPtr;
	using VkSemaphore = UInt64;
	using VkCommandBuffer = IntPtr;
	using VkFence = UInt64;
	using VkDeviceMemory = UInt64;
	using VkBuffer = UInt64;
	using VkImage = UInt64;
	using VkEvent = UInt64;
	using VkQueryPool = UInt64;
	using VkBufferView = UInt64;
	using VkImageView = UInt64;
	using VkShaderModule = UInt64;
	using VkPipelineCache = UInt64;
	using VkPipelineLayout = UInt64;
	using VkRenderPass = UInt64;
	using VkPipeline = UInt64;
	using VkDescriptorSetLayout = UInt64;
	using VkSampler = UInt64;
	using VkDescriptorPool = UInt64;
	using VkDescriptorSet = UInt64;
	using VkFramebuffer = UInt64;
	using VkCommandPool = UInt64;

	using VkSamplerYcbcrConversion = UInt64;
	using VkDescriptorUpdateTemplate = UInt64;

	using VkDeviceAddress = UInt64;

	public class VK12DeviceFunctions {

		public delegate void PFN_vkCmdDrawIndirectCount(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferOffset, uint maxDrawCount, uint stride);
		public delegate void PFN_vkCmdDrawIndexedIndirectCount(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferOffset, uint maxDrawCount, uint stride);
		public delegate void PFN_vkCreateRenderPass2(VkDevice device, in VKRenderPassCreateInfo2 createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkRenderPass renderPass);
		public delegate void PFN_vkCmdBeginRenderPass2(VkCommandBuffer commandBuffer, in VKRenderPassBeginInfo renderPassBegin, in VKSubpassBeginInfo subpassBeginInfo);
		public delegate void PFN_vkCmdNextSubpass2(VkCommandBuffer commandBuffer, in VKSubpassBeginInfo subpassBeginInfo, in VKSubpassEndInfo subpassEndInfo);
		public delegate void PFN_vkCmdEndRenderPass2(VkCommandBuffer commandBuffer, in VKSubpassEndInfo subpassEndInfo);
		public delegate void PFN_vkResetQueryPool(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount);
		public delegate VKResult PFN_vkGetSemaphoreCounterValue(VkDevice device, VkSemaphore semaphore, out ulong value);
		public delegate VKResult PFN_vkWaitSemaphores(VkDevice device, in VKSemaphoreWaitInfo waitInfo, ulong timeout);
		public delegate VKResult PFN_vkSignalSemaphore(VkDevice device, in VKSemaphoreSignalInfo signalInfo);
		public delegate VkDeviceAddress PFN_vkGetBufferDeviceAddress(VkDevice device, in VKBufferDeviceAddressInfo info);
		public delegate ulong PFN_vkGetBufferOpaqueCaptureAddress(VkDevice device, in VKBufferDeviceAddressInfo info);
		public delegate ulong PFN_vkGetDeviceMemoryOpaqueCaptureAddress(VkDevice device, in VKDeviceMemoryOpaqueCaptureAddressInfo info);

		public PFN_vkCmdDrawIndirectCount vkCmdDrawIndirectCount;
		public PFN_vkCmdDrawIndexedIndirectCount vkCmdDrawIndexedIndirectCount;
		public PFN_vkCreateRenderPass2 vkCreateRenderPass2;
		public PFN_vkCmdNextSubpass2 vkCmdNextSubpass2;
		public PFN_vkCmdEndRenderPass2 vkCmdEndRenderPass2;
		public PFN_vkResetQueryPool vkResetQueryPool;
		public PFN_vkGetSemaphoreCounterValue vkGetSemaphoreCounterValue;
		public PFN_vkWaitSemaphores vkWaitSemaphores;
		public PFN_vkSignalSemaphore vkSignalSemaphore;
		public PFN_vkGetBufferDeviceAddress vkGetBufferDeviceAddress;
		public PFN_vkGetBufferOpaqueCaptureAddress vkGetBufferOpaqueCaptureAddress;
		public PFN_vkGetDeviceMemoryOpaqueCaptureAddress vkGetDeviceMemoryOpaqueCaptureAddress;

	}
}
