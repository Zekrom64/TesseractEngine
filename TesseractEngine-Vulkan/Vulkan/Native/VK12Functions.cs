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

	public unsafe class VK12DeviceFunctions {

		[NativeType("void vkCmdDrawIndirectCount(VkCommandBuffer cmdbuf, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferOffset, uint32_t maxDrawCount, uint32_t stride)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, VkBuffer, VkDeviceSize, uint, uint, void> vkCmdDrawIndirectCount;
		[NativeType("void vkCmdDrawIndexedIndirectCount(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferOffset, uint32_t maxDrawCount, uint32_t stride)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, VkBuffer, VkDeviceSize, uint, uint, void> vkCmdDrawIndexedIndirectCount;
		[NativeType("VkResult vkCreateRenderPass2(VkDevice device, const VkRenderPassCreateInfo2* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass)")]
		public delegate* unmanaged<VkDevice, in VKRenderPassCreateInfo2, VKAllocationCallbacks*, out VkRenderPass, VKResult> vkCreateRenderPass2;
		[NativeType("void vkCmdBeginRenderPass2(VkCommandBuffer commandBuffer, const VkRenderPassBeginInfo* pRenderPassBegin, const VkSubpassBeginInfo* pSubpassBeginInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKRenderPassBeginInfo, in VKSubpassBeginInfo, void> vkCmdBeginRenderPass2;
		[NativeType("void vkCmdNextSubpass2(VkCommandBuffer commandBuffer, const VkSubpassBeginInfo* pSubpassBeginInfo, const VkSubpassEndInfo* pSubpassEndInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKSubpassBeginInfo, in VKSubpassEndInfo, void> vkCmdNextSubpass2;
		[NativeType("void vkCmdEndRenderPass2(VkCommandBuffer commandBuffer, const VkSubpassEndInfo* pSubpassEndInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKSubpassEndInfo, void> vkCmdEndRenderPass2;
		[NativeType("void vkResetQueryPool(VkDevice device, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount)")]
		public delegate* unmanaged<VkDevice, VkQueryPool, uint, uint, void> vkResetQueryPool;
		[NativeType("VkResult vkGetSemaphoreCounterValue(VkDevice device, VkSemaphore semaphore, uint64_t* pValue)")]
		public delegate* unmanaged<VkDevice, VkSemaphore, out ulong, VKResult> vkGetSemaphoreCounterValue;
		[NativeType("VkResult vkWaitSemaphores(VkDevice device, const VkSemaphoreWaitInfo* pWaitInfo, uint64_t timeout)")]
		public delegate* unmanaged<VkDevice, in VKSemaphoreWaitInfo, ulong, VKResult> vkWaitSemaphores;
		[NativeType("VkResult vkSignalSemaphore(VkDevice device, const VkSemaphoreSignalInfo* pSignalInfo)")]
		public delegate* unmanaged<VkDevice, in VKSemaphoreSignalInfo, VKResult> vkSignalSemaphore;
		[NativeType("VkDeviceAddress vkGetBufferDeviceAddress(VkDevice device, const VkBufferDeviceAddressInfo* pInfo)")]
		public delegate* unmanaged<VkDevice, in VKBufferDeviceAddressInfo, VkDeviceAddress> vkGetBufferDeviceAddress;
		[NativeType("uint64_t vkGetBufferOpaqueCaptureAddress(VkDevice device, const VkBufferDeviceAddressInfo* pInfo)")]
		public delegate* unmanaged<VkDevice, in VKBufferDeviceAddressInfo, ulong> vkGetBufferOpaqueCaptureAddress;
		[NativeType("uint64_t vkGetDeviceMemoryOpaqueCaptureAddress(VkDevice device, const VkDeviceMemoryOpaqueCaptureAddressInfo* pInfo")]
		public delegate* unmanaged<VkDevice, in VKDeviceMemoryOpaqueCaptureAddressInfo, ulong> vkGetDeviceMemoryOpaqueCaptureAddress;

		public static implicit operator bool(VK12DeviceFunctions? fn) => fn != null;

	}

}
