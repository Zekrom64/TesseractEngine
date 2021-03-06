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

#nullable disable
	public class VK10Functions {

		public delegate VKResult PFN_vkEnumerateInstanceExtensionProperties([MarshalAs(UnmanagedType.LPStr)] string layerName, ref uint propertyCount, [NativeType("VkExtensionProperties*")] IntPtr pProperties);
		public delegate VKResult PFN_vkEnumerateInstanceLayerProperties(ref uint propertyCount, [NativeType("VkLayerProperties*")] IntPtr pProperties);
		public delegate VKResult PFN_vkCreateInstance(in VKInstanceCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkInstance instance);

		public PFN_vkEnumerateInstanceExtensionProperties vkEnumerateInstanceExtensionProperties;
		public PFN_vkEnumerateInstanceLayerProperties vkEnumerateInstanceLayerProperties;
		public PFN_vkCreateInstance vkCreateInstance;

	}
#nullable restore

#nullable disable
	public class VK10InstanceFunctions {

		public delegate void PFN_vkDestroyInstance(VkInstance instance, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkEnumeratePhysicalDevices(VkInstance instnce, ref uint physicalDeviceCount, [NativeType("VkPhysicalDevice*")] IntPtr pPhysicalDevices);
		public delegate void PFN_vkGetPhysicalDeviceFeatures(VkPhysicalDevice phyiscalDevice, out VKPhysicalDeviceFeatures features);
		public delegate void PFN_vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VKFormat format, out VKFormatProperties formatProperties);
		public delegate VKResult PFN_vkGetPhysicalDeviceImageFormatProperties(VkPhysicalDevice physicalDevice, VKFormat format, VKImageType type, VKImageTiling tiling, VKImageUsageFlagBits usage, VKImageCreateFlagBits flags, out VKImageFormatProperties imageFormatProperties);
		public delegate void PFN_vkGetPhysicalDeviceProperties(VkPhysicalDevice physicalDevice, out VKPhysicalDeviceProperties properties);
		public delegate void PFN_vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice physicalDevice, ref uint queueFamilyPropertyCount, [NativeType("VkQueueFamilyProperties*")] IntPtr pQueueFamilyProperties);
		public delegate void PFN_vkGetPhysicalDeviceMemoryProperties(VkPhysicalDevice physicalDevice, out VKPhysicalDeviceMemoryProperties memoryProperties);
		public delegate VKResult PFN_vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, [MarshalAs(UnmanagedType.LPStr)] string layerName, ref uint propertyCount, [NativeType("VkExtensionProperties*")] IntPtr pProperties);
		public delegate VKResult PFN_vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, ref uint propertyCount, [NativeType("VkLayerProperties*")] IntPtr pProperties);
		public delegate VKResult PFN_vkCreateDevice(VkPhysicalDevice physicalDevice, in VKDeviceCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkDevice device);
		public delegate void PFN_vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VKFormat format, VKImageType type, VKSampleCountFlagBits samples, VKImageUsageFlagBits usage, VKImageTiling tiling, ref uint propertyCount, [NativeType("VkSparseImageFormatProperties*")] IntPtr pProperties);

		public PFN_vkDestroyInstance vkDestroyInstance;
		public PFN_vkEnumeratePhysicalDevices vkEnumeratePhysicalDevices;
		public PFN_vkGetPhysicalDeviceFeatures vkGetPhysicalDeviceFeatures;
		public PFN_vkGetPhysicalDeviceFormatProperties vkGetPhysicalDeviceFormatProperties;
		public PFN_vkGetPhysicalDeviceImageFormatProperties vkGetPhysicalDeviceImageFormatProperties;
		public PFN_vkGetPhysicalDeviceProperties vkGetPhysicalDeviceProperties;
		public PFN_vkGetPhysicalDeviceQueueFamilyProperties vkGetPhysicalDeviceQueueFamilyProperties;
		public PFN_vkGetPhysicalDeviceMemoryProperties vkGetPhysicalDeviceMemoryProperties;
		public PFN_vkEnumerateDeviceExtensionProperties vkEnumerateDeviceExtensionProperties;
		public PFN_vkEnumerateDeviceLayerProperties vkEnumerateDeviceLayerProperties;
		public PFN_vkCreateDevice vkCreateDevice;
		public PFN_vkGetPhysicalDeviceSparseImageFormatProperties vkGetPhysicalDeviceSparseImageFormatProperties;

	}
#nullable restore

#nullable disable
	public class VK10DeviceFunctions {

		public delegate void PFN_vkDestroyDevice(VkDevice device, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkGetDeviceQueue(VkDevice device, uint queueFamilyIndex, uint queueIndex, out VkQueue queue);
		public delegate VKResult PFN_vkQueueSubmit(VkQueue queue, uint submitCount, [NativeType("const VkSubmitInfo*")] IntPtr pSubmits, VkFence fence);
		public delegate VKResult PFN_vkQueueWaitIdle(VkQueue queue);
		public delegate VKResult PFN_vkDeviceWaitIdle(VkDevice device);
		public delegate VKResult PFN_vkAllocateMemory(VkDevice device, in VKMemoryAllocateInfo allocateInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkDeviceMemory memory);
		public delegate void PFN_vkFreeMemory(VkDevice device, VkDeviceMemory memory, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkMapMemory(VkDevice device, VkDeviceMemory memory, VkDeviceSize offset, VkDeviceSize size, VKMemoryMapFlagBits flags, out IntPtr pData);
		public delegate void PFN_vkUnmapMemory(VkDevice device, VkDeviceMemory memory);
		public delegate VKResult PFN_vkFlushMappedMemoryRanges(VkDevice device, uint memoryRangeCount, [NativeType("const VkMappedMemoryRange*")] IntPtr pMemoryRanges);
		public delegate VKResult PFN_vkInvalidateMappedMemoryRanges(VkDevice device, uint memoryRangeCount, [NativeType("const VkMappedMemoryRange*")] IntPtr pMemoryRanges);
		public delegate void PFN_vkGetDeviceMemoryCommitment(VkDevice device, VkDeviceMemory memory, out VkDeviceSize committedMemoryInBytes);
		public delegate VKResult PFN_vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, VkDeviceSize memoryOffset);
		public delegate VKResult PFN_vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, VkDeviceSize memoryOffset);
		public delegate void PFN_vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, out VKMemoryRequirements memoryRequirements);
		public delegate void PFN_vkGetImageMemoryRequirements(VkDevice device, VkImage image, out VKMemoryRequirements memoryRequirements);
		public delegate void PFN_vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, ref uint sparseMemoryRequirementCount, [NativeType("VkSparseImageMemoryRequirements*")] IntPtr pSparseMemoryRequirements);
		public delegate VKResult PFN_vkQueueBindSparse(VkQueue queue, uint bindInfoCount, [NativeType("const VkBindSparseInfo*")] IntPtr pBindInfo, VkFence fence);
		public delegate VKResult PFN_vkCreateFence(VkDevice device, in VKFenceCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkFence fence);
		public delegate void PFN_vkDestroyFence(VkDevice device, VkFence fence, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkResetFences(VkDevice device, uint fenceCount, [NativeType("const VkFence*")] IntPtr pFences);
		public delegate VKResult PFN_vkGetFenceStatus(VkDevice deivce, VkFence fence);
		public delegate VKResult PFN_vkWaitForFences(VkDevice device, uint fenceCount, [NativeType("const VkFence*")] IntPtr pFences, VkBool32 waitAll, ulong timeout);
		public delegate VKResult PFN_vkCreateSemaphore(VkDevice device, in VKSemaphoreCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkSemaphore semaphore);
		public delegate void PFN_vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateEvent(VkDevice device, in VKEventCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkEvent _event);
		public delegate void PFN_vkDestroyEvent(VkDevice device, VkEvent _event, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkGetEventStatus(VkDevice device, VkEvent _event);
		public delegate VKResult PFN_vkSetEvent(VkDevice device, VkEvent _event);
		public delegate VKResult PFN_vkResetEvent(VkDevice device, VkEvent _event);
		public delegate VKResult PFN_vkCreateQueryPool(VkDevice device, in VKQueryPoolCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkQueryPool queryPool);
		public delegate void PFN_vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount, nuint dataSize, IntPtr pData, VkDeviceSize stride, VKQueryResultFlagBits flags);
		public delegate VKResult PFN_vkCreateBuffer(VkDevice device, in VKBufferCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkBuffer buffer);
		public delegate void PFN_vkDestroyBuffer(VkDevice device, VkBuffer buffer, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateBufferView(VkDevice device, in VKBufferViewCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkBufferView view);
		public delegate void PFN_vkDestroyBufferView(VkDevice device, VkBufferView bufferView, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateImage(VkDevice device, in VKImageCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkImage image);
		public delegate void PFN_vkDestroyImage(VkDevice device, VkImage image, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkGetImageSubresourceLayout(VkDevice device, VkImage image, in VKImageSubresource subresource, out VKSubresourceLayout layout);
		public delegate VKResult PFN_vkCreateImageView(VkDevice device, in VKImageViewCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkImageView view);
		public delegate void PFN_vkDestroyImageView(VkDevice device, VkImageView imageView, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateShaderModule(VkDevice device, in VKShaderModuleCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkShaderModule shaderModule);
		public delegate void PFN_vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreatePipelineCache(VkDevice device, in VKPipelineCacheCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkPipelineCache pipelineCache);
		public delegate void PFN_vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkGetPipelineCacheData(VkDevice device, VkPipelineCache cache, ref nuint dataSize, IntPtr pData);
		public delegate VKResult PFN_vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint srcCacheCount, [NativeType("const VkPipelineCache*")] IntPtr pSrcCaches);
		public delegate VKResult PFN_vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, [NativeType("const VkGraphicsPipelineCreateInfo*")] IntPtr pCreateInfos, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkPipeline*")] IntPtr pPipelines);
		public delegate VKResult PFN_vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint createInfoCount, [NativeType("const VkComputePipelineCreateInfo*")] IntPtr pCreateInfos, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkPipeline*")] IntPtr pPipelines);
		public delegate void PFN_vkDestroyPipeline(VkDevice device, VkPipeline pipeline, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreatePipelineLayout(VkDevice device, in VKPipelineLayoutCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkPipelineLayout pipelineLayout);
		public delegate void PFN_vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateSampler(VkDevice device, in VKSamplerCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkSampler sampler);
		public delegate void PFN_vkDestroySampler(VkDevice device, VkSampler sampler, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateDescriptorSetLayout(VkDevice device, in VKDescriptorSetLayoutCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkDescriptorSetLayout setLayout);
		public delegate void PFN_vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateDescriptorPool(VkDevice device, in VKDescriptorPoolCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkDescriptorPool descriptorPool);
		public delegate void PFN_vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkResetDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, VKDescriptorPoolResetFlagBits flags);
		public delegate VKResult PFN_vkAllocateDescriptorSets(VkDevice device, in VKDescriptorSetAllocateInfo allocateInfo, [NativeType("VkDescriptorSet*")] IntPtr pDescriptorSets);
		public delegate VKResult PFN_vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint descriptorSetCount, [NativeType("const VkDescriptorSet*")] IntPtr pDescriptorSets);
		public delegate void PFN_vkUpdateDescriptorSets(VkDevice device, uint descriptorWriteCount, [NativeType("const VkWriteDescriptorSet*")] IntPtr pDescriptorWrites, uint descriptorCopyCount, [NativeType("const VkCopyDescriptorSet*")] IntPtr pDescriptorCopies);
		public delegate VKResult PFN_vkCreateFramebuffer(VkDevice device, in VKFramebufferCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkFramebuffer framebuffer);
		public delegate void PFN_vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateRenderPass(VkDevice device, in VKRenderPassCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkRenderPass renderPass);
		public delegate void PFN_vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkGetRenderAreaGranularity(VkDevice device, VkRenderPass renderPass, out VKExtent2D granularity);
		public delegate VKResult PFN_vkCreateCommandPool(VkDevice device, in VKCommandPoolCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkCommandPool commandPool);
		public delegate void PFN_vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkResetCommandPool(VkDevice device, VkCommandPool commandPool, VKCommandPoolResetFlagBits flags);
		public delegate VKResult PFN_vkAllocateCommandBuffers(VkDevice device, in VKCommandBufferAllocateInfo allocateInfo, [NativeType("VkCommandBuffer*")] IntPtr pCommandBuffers);
		public delegate void PFN_vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint commandBufferCount, [NativeType("const VkCommandBuffer*")] IntPtr pCommandBuffers);
		public delegate VKResult PFN_vkBeginCommandBuffer(VkCommandBuffer commandBuffer, in VKCommandBufferBeginInfo beginInfo);
		public delegate VKResult PFN_vkEndCommandBuffer(VkCommandBuffer commandBuffer);
		public delegate VKResult PFN_vkResetCommandBuffer(VkCommandBuffer commandBuffer, VKCommandBufferResetFlagBits flags);
		public delegate void PFN_vkCmdBindPipeline(VkCommandBuffer commandBuffer, VKPipelineBindPoint pipelineBindPoint, VkPipeline pipeline);
		public delegate void PFN_vkCmdSetViewport(VkCommandBuffer commandBuffer, uint firstViewport, uint viewportCount, [NativeType("const VkViewport*")] IntPtr pViewports);
		public delegate void PFN_vkCmdSetScissor(VkCommandBuffer commandBuffer, uint firstScissor, uint scissorCount, [NativeType("const VkRect2D*")] IntPtr pScissors);
		public delegate void PFN_vkCmdSetLineWidth(VkCommandBuffer commandBuffer, float lineWidth);
		public delegate void PFN_vkCmdSetDepthBias(VkCommandBuffer commandBuffer, float depthBiasConstantFactor, float depthBiasClamp, float depthBiasSlopeFactor);
		public delegate void PFN_vkCmdSetBlendConstants(VkCommandBuffer commandBuffer, [NativeType("const float[4]")] IntPtr blendConstants);
		public delegate void PFN_vkCmdSetDepthBounds(VkCommandBuffer commandBuffer, float minDepthBounds, float maxDepthBounds);
		public delegate void PFN_vkCmdSetStencilCompareMask(VkCommandBuffer commandBuffer, VKStencilFaceFlagBits faceMask, uint compareMask);
		public delegate void PFN_vkCmdSetStencilWriteMask(VkCommandBuffer commandBuffer, VKStencilFaceFlagBits faceMask, uint writeMask);
		public delegate void PFN_vkCmdSetStencilReference(VkCommandBuffer commandBuffer, VKStencilFaceFlagBits faceMask, uint reference);
		public delegate void PFN_vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VKPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint firstSet, uint descriptorSetCount, [NativeType("const VkDescriptorSet*")] IntPtr pDescriptorSets, uint dynamicOffsetCount, [NativeType("const uint32_t*")] IntPtr pDynamicOffsets);
		public delegate void PFN_vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VKIndexType indexType);
		public delegate void PFN_vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint firstBinding, uint bindingCount, [NativeType("const VkBuffer*")] IntPtr pBuffers, [NativeType("const VkDeviceSize*")] IntPtr pOffsets);
		public delegate void PFN_vkCmdDraw(VkCommandBuffer commandBuffer, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);
		public delegate void PFN_vkCmdDrawIndexed(VkCommandBuffer commandBuffer, uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);
		public delegate void PFN_vkCmdDrawIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint drawCount, uint stride);
		public delegate void PFN_vkCmdDrawIndexedIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint drawCount, uint stride);
		public delegate void PFN_vkCmdDispatch(VkCommandBuffer commandBuffer, uint groupCountX, uint groupCountY, uint groupCountZ);
		public delegate void PFN_vkCmdDispatchIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset);
		public delegate void PFN_vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint regionCount, [NativeType("const VkBufferCopy*")] IntPtr pRegions);
		public delegate void PFN_vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VKImageLayout srcImageLayout, VkImage dstImage, VKImageLayout dstImageLayout, uint regionCount, [NativeType("const VkImageCopy*")] IntPtr pRegions);
		public delegate void PFN_vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VKImageLayout srcImageLayout, VkImage dstImage, VKImageLayout dstImageLayout, uint regionCount, [NativeType("const VkImageBlit*")] IntPtr pRegions, VKFilter filter);
		public delegate void PFN_vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VKImageLayout dstImageLayout, uint regionCount, [NativeType("const VkBufferImageCopy*")] IntPtr pRegions);
		public delegate void PFN_vkCmdCopyImageToBuffer(VkCommandBuffer commandBuffer, VkImage srcImage, VKImageLayout srcImageLayout, VkBuffer dstBuffer, uint regionCount, [NativeType("const VkBufferImageCopy*")] IntPtr pRegions);
		public delegate void PFN_vkCmdUpdateBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize dataSize, IntPtr pData);
		public delegate void PFN_vkCmdFillBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize size, uint data);
		public delegate void PFN_vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VKImageLayout imageLayout, in VKClearColorValue color, uint rangeCount, [NativeType("const VkImageSubresourceRange*")] IntPtr pRanges);
		public delegate void PFN_vkCmdClearDepthStencilImage(VkCommandBuffer commandBuffer, VkImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, uint rangeCount, [NativeType("const VkImageSubresourceRange*")] IntPtr pRanges);
		public delegate void PFN_vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint attachmentCount, [NativeType("const VkClearAttachment*")] IntPtr pAttachments, uint rectCount, [NativeType("const VkClearRect*")] IntPtr pRects);
		public delegate void PFN_vkCmdResolveImage(VkCommandBuffer commandBuffer, VkImage srcImage, VKImageLayout srcImageLayout, VkImage dstImage, VKImageLayout dstIamgeLayout, uint regionCount, [NativeType("const VkImageResolve*")] IntPtr pRegions);
		public delegate void PFN_vkCmdSetEvent(VkCommandBuffer commandBuffer, VkEvent _event, VKPipelineStageFlagBits stageMask);
		public delegate void PFN_vkCmdResetEvent(VkCommandBuffer commandBuffer, VkEvent _event, VKPipelineStageFlagBits stageMask);
		public delegate void PFN_vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint eventCount, [NativeType("const VkEvent*")] IntPtr pEvents, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, uint memoryBarrierCount, [NativeType("const VkMemoryBarrier*")] IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, [NativeType("const VkBufferMemoryBarrier*")] IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, [NativeType("const VkImageMemoryBarrier*")] IntPtr pImageMemoryBarriers);
		public delegate void PFN_vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, VKDependencyFlagBits dependencyFlags, uint memoryBarrierCount, [NativeType("const VkMemoryBarrier*")] IntPtr pMemoryBarriers, uint bufferMemoryBarrierCount, [NativeType("const VkBufferMemoryBarrier*")] IntPtr pBufferMemoryBarriers, uint imageMemoryBarrierCount, [NativeType("const VkImageMemoryBarrier*")] IntPtr pImageMemoryBarriers);
		public delegate void PFN_vkCmdBeginQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query, VKQueryControlFlagBits flags);
		public delegate void PFN_vkCmdEndQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint query);
		public delegate void PFN_vkCmdResetQueryPool(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint firstQuery, uint queryCount);
		public delegate void PFN_vkCmdWriteTimestamp(VkCommandBuffer commandBuffer, VKPipelineStageFlagBits pipelineStage, VkQueryPool queryPool, uint query);
		public delegate void PFN_vkCmdCopyQueryPoolResults(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint firstQuery, uint queryCount, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize stride, VKQueryResultFlagBits flags);
		public delegate void PFN_vkCmdPushConstants(VkCommandBuffer commandBuffer, VkPipelineLayout layout, VKShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues);
		public delegate void PFN_vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, in VKRenderPassBeginInfo renderPassBegin, VKSubpassContents contents);
		public delegate void PFN_vkCmdNextSubpass(VkCommandBuffer commandBuffer, VKSubpassContents contents);
		public delegate void PFN_vkCmdEndRenderPass(VkCommandBuffer commandBuffer);
		public delegate void PFN_vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint commandBufferCount, [NativeType("const VkCommandBuffer*")] IntPtr pCommandBuffers);

		public PFN_vkDestroyDevice vkDestroyDevice;
		public PFN_vkGetDeviceQueue vkGetDeviceQueue;
		public PFN_vkQueueSubmit vkQueueSubmit;
		public PFN_vkQueueWaitIdle vkQueueWaitIdle;
		public PFN_vkDeviceWaitIdle vkDeviceWaitIdle;
		public PFN_vkAllocateMemory vkAllocateMemory;
		public PFN_vkFreeMemory vkFreeMemory;
		public PFN_vkMapMemory vkMapMemory;
		public PFN_vkUnmapMemory vkUnmapMemory;
		public PFN_vkFlushMappedMemoryRanges vkFlushMappedMemoryRanges;
		public PFN_vkInvalidateMappedMemoryRanges vkInvalidateMappedMemoryRanges;
		public PFN_vkGetDeviceMemoryCommitment vkGetDeviceMemoryCommitment;
		public PFN_vkBindBufferMemory vkBindBufferMemory;
		public PFN_vkBindImageMemory vkBindImageMemory;
		public PFN_vkGetBufferMemoryRequirements vkGetBufferMemoryRequirements;
		public PFN_vkGetImageMemoryRequirements vkGetImageMemoryRequirements;
		public PFN_vkGetImageSparseMemoryRequirements vkGetImageSparseMemoryRequirements;
		public PFN_vkQueueBindSparse vkQueueBindSparse;
		public PFN_vkCreateFence vkCreateFence;
		public PFN_vkDestroyFence vkDestroyFence;
		public PFN_vkResetFences vkResetFences;
		public PFN_vkGetFenceStatus vkGetFenceStatus;
		public PFN_vkWaitForFences vkWaitForFences;
		public PFN_vkCreateSemaphore vkCreateSemaphore;
		public PFN_vkDestroySemaphore vkDestroySemaphore;
		public PFN_vkCreateEvent vkCreateEvent;
		public PFN_vkDestroyEvent vkDestroyEvent;
		public PFN_vkGetEventStatus vkGetEventStatus;
		public PFN_vkSetEvent vkSetEvent;
		public PFN_vkResetEvent vkResetEvent;
		public PFN_vkCreateQueryPool vkCreateQueryPool;
		public PFN_vkDestroyQueryPool vkDestroyQueryPool;
		public PFN_vkGetQueryPoolResults vkGetQueryPoolResults;
		public PFN_vkCreateBuffer vkCreateBuffer;
		public PFN_vkDestroyBuffer vkDestroyBuffer;
		public PFN_vkCreateBufferView vkCreateBufferView;
		public PFN_vkDestroyBufferView vkDestroyBufferView;
		public PFN_vkCreateImage vkCreateImage;
		public PFN_vkDestroyImage vkDestroyImage;
		public PFN_vkGetImageSubresourceLayout vkGetImageSubresourceLayout;
		public PFN_vkCreateImageView vkCreateImageView;
		public PFN_vkDestroyImageView vkDestroyImageView;
		public PFN_vkCreateShaderModule vkCreateShaderModule;
		public PFN_vkDestroyShaderModule vkDestroyShaderModule;
		public PFN_vkCreatePipelineCache vkCreatePipelineCache;
		public PFN_vkDestroyPipelineCache vkDestroyPipelineCache;
		public PFN_vkGetPipelineCacheData vkGetPipelineCacheData;
		public PFN_vkMergePipelineCaches vkMergePipelineCaches;
		public PFN_vkCreateGraphicsPipelines vkCreateGraphicsPipelines;
		public PFN_vkCreateComputePipelines vkCreateComputePipelines;
		public PFN_vkDestroyPipeline vkDestroyPipeline;
		public PFN_vkCreatePipelineLayout vkCreatePipelineLayout;
		public PFN_vkDestroyPipelineLayout vkDestroyPipelineLayout;
		public PFN_vkCreateSampler vkCreateSampler;
		public PFN_vkDestroySampler vkDestroySampler;
		public PFN_vkCreateDescriptorSetLayout vkCreateDescriptorSetLayout;
		public PFN_vkDestroyDescriptorSetLayout vkDestroyDescriptorSetLayout;
		public PFN_vkCreateDescriptorPool vkCreateDescriptorPool;
		public PFN_vkDestroyDescriptorPool vkDestroyDescriptorPool;
		public PFN_vkResetDescriptorPool vkResetDescriptorPool;
		public PFN_vkAllocateDescriptorSets vkAllocateDescriptorSets;
		public PFN_vkFreeDescriptorSets vkFreeDescriptorSets;
		public PFN_vkUpdateDescriptorSets vkUpdateDescriptorSets;
		public PFN_vkCreateFramebuffer vkCreateFramebuffer;
		public PFN_vkDestroyFramebuffer vkDestroyFramebuffer;
		public PFN_vkCreateRenderPass vkCreateRenderPass;
		public PFN_vkDestroyRenderPass vkDestroyRenderPass;
		public PFN_vkGetRenderAreaGranularity vkGetRenderAreaGranularity;
		public PFN_vkCreateCommandPool vkCreateCommandPool;
		public PFN_vkDestroyCommandPool vkDestroyCommandPool;
		public PFN_vkResetCommandPool vkResetCommandPool;
		public PFN_vkAllocateCommandBuffers vkAllocateCommandBuffers;
		public PFN_vkFreeCommandBuffers vkFreeCommandBuffers;
		public PFN_vkBeginCommandBuffer vkBeginCommandBuffer;
		public PFN_vkEndCommandBuffer vkEndCommandBuffer;
		public PFN_vkResetCommandBuffer vkResetCommandBuffer;
		public PFN_vkCmdBindPipeline vkCmdBindPipeline;
		public PFN_vkCmdSetViewport vkCmdSetViewport;
		public PFN_vkCmdSetScissor vkCmdSetScissor;
		public PFN_vkCmdSetLineWidth vkCmdSetLineWidth;
		public PFN_vkCmdSetDepthBias vkCmdSetDepthBias;
		public PFN_vkCmdSetBlendConstants vkCmdSetBlendConstants;
		public PFN_vkCmdSetDepthBounds vkCmdSetDepthBounds;
		public PFN_vkCmdSetStencilCompareMask vkCmdSetStencilCompareMask;
		public PFN_vkCmdSetStencilWriteMask vkCmdSetStencilWriteMask;
		public PFN_vkCmdSetStencilReference vkCmdSetStencilReference;
		public PFN_vkCmdBindDescriptorSets vkCmdBindDescriptorSets;
		public PFN_vkCmdBindIndexBuffer vkCmdBindIndexBuffer;
		public PFN_vkCmdBindVertexBuffers vkCmdBindVertexBuffers;
		public PFN_vkCmdDraw vkCmdDraw;
		public PFN_vkCmdDrawIndexed vkCmdDrawIndexed;
		public PFN_vkCmdDrawIndirect vkCmdDrawIndirect;
		public PFN_vkCmdDrawIndexedIndirect vkCmdDrawIndexedIndirect;
		public PFN_vkCmdDispatch vkCmdDispatch;
		public PFN_vkCmdDispatchIndirect vkCmdDispatchIndirect;
		public PFN_vkCmdCopyBuffer vkCmdCopyBuffer;
		public PFN_vkCmdCopyImage vkCmdCopyImage;
		public PFN_vkCmdBlitImage vkCmdBlitImage;
		public PFN_vkCmdCopyBufferToImage vkCmdCopyBufferToImage;
		public PFN_vkCmdCopyImageToBuffer vkCmdCopyImageToBuffer;
		public PFN_vkCmdUpdateBuffer vkCmdUpdateBuffer;
		public PFN_vkCmdFillBuffer vkCmdFillBuffer;
		public PFN_vkCmdClearColorImage vkCmdClearColorImage;
		public PFN_vkCmdClearDepthStencilImage vkCmdClearDepthStencilImage;
		public PFN_vkCmdClearAttachments vkCmdClearAttachments;
		public PFN_vkCmdResolveImage vkCmdResolveImage;
		public PFN_vkCmdSetEvent vkCmdSetEvent;
		public PFN_vkCmdResetEvent vkCmdResetEvent;
		public PFN_vkCmdWaitEvents vkCmdWaitEvents;
		public PFN_vkCmdPipelineBarrier vkCmdPipelineBarrier;
		public PFN_vkCmdBeginQuery vkCmdBeginQuery;
		public PFN_vkCmdEndQuery vkCmdEndQuery;
		public PFN_vkCmdResetQueryPool vkCmdResetQueryPool;
		public PFN_vkCmdWriteTimestamp vkCmdWriteTimestamp;
		public PFN_vkCmdCopyQueryPoolResults vkCmdCopyQueryPoolResults;
		public PFN_vkCmdPushConstants vkCmdPushConstants;
		public PFN_vkCmdBeginRenderPass vkCmdBeginRenderPass;
		public PFN_vkCmdNextSubpass vkCmdNextSubpass;
		public PFN_vkCmdEndRenderPass vkCmdEndRenderPass;
		public PFN_vkCmdExecuteCommands vkCmdExecuteCommands;

		public static implicit operator bool(VK10DeviceFunctions fn) => fn != null;

	}
#nullable restore

}
