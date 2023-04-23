using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;
using System.Numerics;

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

	using VKExtent2D = Vector2ui;

	public unsafe class VK10Functions {

		[NativeType("VkResult vkEnumerateInstanceExtensionProperties(const char* layerName, uint32_t* pPropertyCount, VkExtensionProperties* pProperties)")]
		public delegate* unmanaged<IntPtr, ref uint, VKExtensionProperties*, VKResult> vkEnumerateInstanceExtensionProperties;
		[NativeType("VkResult vkEnumerateInstanceLayerProperties(uint32_t* pPropertyCount, VkLayerProperties* pProperties)")]
		public delegate* unmanaged<ref uint, VKLayerProperties*, VKResult> vkEnumerateInstanceLayerProperties;
		[NativeType("VkResult vkCreateInstance(const VkInstanceCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkInstance* pInstance)")]
		public delegate* unmanaged<in VKInstanceCreateInfo, VKAllocationCallbacks*, out VkInstance, VKResult> vkCreateInstance;

	}

	public unsafe class VK10InstanceFunctions {

		[NativeType("void vkDestroyInstance(VkInstance instance, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkInstance, VKAllocationCallbacks*, void> vkDestroyInstance;
		[NativeType("VkResult vkEnumeratePhysicalDevices(VkInstance instance, uint32_t* pPhysicalDeviceCount, VkPhysicalDevice* pPhysicalDevices)")]
		public delegate* unmanaged<VkInstance, ref uint, VkPhysicalDevice*, VKResult> vkEnumeratePhysicalDevices;
		[NativeType("void vkGetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures* pFeatures)")]
		public delegate* unmanaged<VkPhysicalDevice, out VKPhysicalDeviceFeatures, void> vkGetPhysicalDeviceFeatures;
		[NativeType("void vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties* pFormatProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, VKFormat, out VKFormatProperties, void> vkGetPhysicalDeviceFormatProperties;
		[NativeType("VkResult vkGetPhysicalDeviceImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkImageTiling tiling, VkImageUsageFlags usage, VkImageCreateFlags flags, VkImageFormatProperties* pImageFormatProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, VKFormat, VKImageType, VKImageTiling, VKImageUsageFlagBits, VKImageCreateFlagBits, out VKImageFormatProperties, VKResult> vkGetPhysicalDeviceImageFormatProperties;
		[NativeType("void vkGetPhysicalDeviceProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, out VKPhysicalDeviceProperties, void> vkGetPhysicalDeviceProperties;
		[NativeType("void vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice physicalDevice, uint32_t* pQueueFamilyPropertyCount, VkQueueFamilyProperties* pQueueFamilyProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, ref uint, VKQueueFamilyProperties*, void> vkGetPhysicalDeviceQueueFamilyProperties;
		[NativeType("void vkGetPhysicalDeviceMemoryProperties(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties* pMemoryProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, out VKPhysicalDeviceMemoryProperties, void> vkGetPhysicalDeviceMemoryProperties;
		[NativeType("VkResult vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, const char* layerName, uint32_t* pPropertyCount, VkExtensionProperties* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, IntPtr, ref uint, VKExtensionProperties*, VKResult> vkEnumerateDeviceExtensionProperties;
		[NativeType("VKResult vkEnumerateDeviceLayerProperties(VkPhysicalDevice physicalDevice, uint32_t* pPropertyCount, VkLayerProperties* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, ref uint, VKLayerProperties*, VKResult> vkEnumerateDeviceLayerProperties;
		[NativeType("VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, const VkDeviceCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDevice* pDevice)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKDeviceCreateInfo, VKAllocationCallbacks*, out VkDevice, VKResult> vkCreateDevice;
		[NativeType("void vkGetPhysicalDeviceSparseImageFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, VkImageType type, VkSampleCountFlags samples, VkImageUsageFlags usage, VkImageTiling tiling, uint32_t* pPropertyCount, VkSparseImageFormatProperties* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, VKFormat, VKImageType, VKSampleCountFlagBits, VKImageUsageFlagBits, VKImageTiling, ref uint, VKSparseImageFormatProperties*, void> vkGetPhysicalDeviceSparseImageFormatProperties;

	}

	public unsafe class VK10DeviceFunctions {

		[NativeType("void vkDestroyDevice(VkDevice device, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VKAllocationCallbacks*, void> vkDestroyDevice;
		[NativeType("void vkGetDeviceQueue(VkDevice device, uint32_t queueFamilyIndex, uint32_t queueIndex, VkQueue* pQueue)")]
		public delegate* unmanaged<VkDevice, uint, uint, out VkQueue, void> vkGetDeviceQueue;
		[NativeType("VkResult vkQueueSubmit(VkQueue queue, uint32_t submitCount, const VkSubmitInfo* pSubmits, VkFence fence)")]
		public delegate* unmanaged<VkQueue, uint, VKSubmitInfo*, VkFence, VKResult> vkQueueSubmit;
		[NativeType("VkResult vkQueueWaitIdle(VkQueue queue)")]
		public delegate* unmanaged<VkQueue, VKResult> vkQueueWaitIdle;
		[NativeType("VkResult vkDeviceWaitIdle(VkDevice device)")]
		public delegate* unmanaged<VkDevice, VKResult> vkDeviceWaitIdle;

		[NativeType("VkResult vkAllocateMemory(VkDevice device, const VkMemoryAllocateInfo* pAllocateInfo, const VkAllocationCallbacks* pAllocator, VkDeviceMemory* pMemory)")]
		public delegate* unmanaged<VkDevice, in VKMemoryAllocateInfo, VKAllocationCallbacks*, out VkDeviceMemory, VKResult> vkAllocateMemory;
		[NativeType("void vkFreeMemory(VkDevice device, VkDeviceMemory memory, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkDeviceMemory, VKAllocationCallbacks*, void> vkFreeMemory;
		[NativeType("VkResult vkMapMemory(VkDevice device, VkDeviceMemory memory, VkDeviceSize offset, VkDeviceSize size, VkMemoryMapFlags flags, void** pData)")]
		public delegate* unmanaged<VkDevice, VkDeviceMemory, VkDeviceSize, VkDeviceSize, VKMemoryMapFlagBits, out IntPtr, VKResult> vkMapMemory;
		[NativeType("void vkUnmapMemory(VkDevice device, VkDeviceMemory memory)")]
		public delegate* unmanaged<VkDevice, VkDeviceMemory, void> vkUnmapMemory;
		[NativeType("VkResult vkFlushMappedMemoryRanges(VkDevice device, uint32_t memoryRangeCount, const VkMappedMemoryRange* pMemoryRanges)")]
		public delegate* unmanaged<VkDevice, uint, VKMappedMemoryRange*, VKResult> vkFlushMappedMemoryRanges;
		[NativeType("VkResult vkInvalidateMappedMemoryRanges(VkDevice device, uint32_t memoryRangeCount, const VkMappedMemoryRange* pMemoryRanges)")]
		public delegate* unmanaged<VkDevice, uint, VKMappedMemoryRange*, VKResult> vkInvalidateMappedMemoryRanges;
		[NativeType("void vkGetDeviceMemoryCommitment(VkDevice device, VkDeviceMemory memory, VkDeviceSize* pCommittedMemoryInBytes)")]
		public delegate* unmanaged<VkDevice, VkDeviceMemory, out VkDeviceSize, void> vkGetDeviceMemoryCommitment;
		[NativeType("VkResult vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, VkDeviceSize memoryOffset)")]
		public delegate* unmanaged<VkDevice, VkBuffer, VkDeviceMemory, VkDeviceSize, VKResult> vkBindBufferMemory;
		[NativeType("VkResult vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, VkDeviceSize memoryOffset)")]
		public delegate* unmanaged<VkDevice, VkImage, VkDeviceMemory, VkDeviceSize, VKResult> vkBindImageMemory;
		[NativeType("void vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, VkMemoryRequirements* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, VkBuffer, out VKMemoryRequirements, void> vkGetBufferMemoryRequirements;
		[NativeType("void vkGetImageMemoryRequirements(VkDevice device, VkImage image, VkMemoryRequirements* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, VkImage, out VKMemoryRequirements, void> vkGetImageMemoryRequirements;
		[NativeType("void vkGetImageSparseMemoryRequirements(VkDevice device, VkImage image, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements* pSparseMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, VkImage, ref uint, VKSparseImageMemoryRequirements*, void> vkGetImageSparseMemoryRequirements;
		[NativeType("VkResult vkQueueBindSparse(VkQueue queue, uint32_t bindInfoCount, VkBindSparseInfo* pBindInfo, VkFence fence)")]
		public delegate* unmanaged<VkQueue, uint, VKBindSparseInfo*, VkFence, VKResult> vkQueueBindSparse;

		[NativeType("VkResult vkCreateFence(VkDevice device, const VkFenceCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkFence* pFence)")]
		public delegate* unmanaged<VkDevice, in VKFenceCreateInfo, VKAllocationCallbacks*, out VkFence, VKResult> vkCreateFence;
		[NativeType("void vkDestroyFence(VkDevice device, VkFence fence, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkFence, VKAllocationCallbacks*, void> vkDestroyFence;
		[NativeType("VkResult vkResetFences(VkDevice device, uint32_t fenceCount, const VkFence* pFences)")]
		public delegate* unmanaged<VkDevice, uint, VkFence*, VKResult> vkResetFences;
		[NativeType("VkResult vkGetFenceStatus(VkDevice device, VkFence fence)")]
		public delegate* unmanaged<VkDevice, VkFence, VKResult> vkGetFenceStatus;
		[NativeType("VkResult vkWaitForFences(VkDevice device, uint32_t fenceCount, const VkFence* pFences, VkBool32 waitAll, uint64_t timeout)")]
		public delegate* unmanaged<VkDevice, uint, VkFence*, VkBool32, ulong, VKResult> vkWaitForFences;
		[NativeType("VkResult vkCreateSemaphore(VkDevice device, const VkSemaphoreCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSemaphore* pSemaphore)")]
		public delegate* unmanaged<VkDevice, in VKSemaphoreCreateInfo, VKAllocationCallbacks*, out VkSemaphore, VKResult> vkCreateSemaphore;
		[NativeType("void vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkSemaphore, VKAllocationCallbacks*, void> vkDestroySemaphore;
		[NativeType("VkResult vkCreateEvent(VkDevice device, const VkEventCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkEvent* pEvent)")]
		public delegate* unmanaged<VkDevice, in VKEventCreateInfo, VKAllocationCallbacks*, out VkEvent, VKResult> vkCreateEvent;
		[NativeType("void vkDestroyEvent(VkDevice device, VkEvent event, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkEvent, VKAllocationCallbacks*, void> vkDestroyEvent;
		[NativeType("VkResult vkGetEventStatus(VkDevice device, VkEvent event)")]
		public delegate* unmanaged<VkDevice, VkEvent, VKResult> vkGetEventStatus;
		[NativeType("VkResult vkSetEvent(VkDevice device, VkEvent event)")]
		public delegate* unmanaged<VkDevice, VkEvent, VKResult> vkSetEvent;
		[NativeType("VkResult vkResetEvent(VkDevice device, VkEvent event)")]
		public delegate* unmanaged<VkDevice, VkEvent, VKResult> vkResetEvent;

		[NativeType("VkResult vkCreateQueryPool(VkDevice device, const VkQueryPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkQueryPool* pQueryPool)")]
		public delegate* unmanaged<VkDevice, in VKQueryPoolCreateInfo, VKAllocationCallbacks*, out VkQueryPool, VKResult> vkCreateQueryPool;
		[NativeType("void vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkQueryPool, VKAllocationCallbacks*, void> vkDestroyQueryPool;
		[NativeType("VkResult vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount, size_t dataSize, void* data, VkDeviceSize stride, VkQueryResultFlags flags)")]
		public delegate* unmanaged<VkDevice, VkQueryPool, uint, uint, nuint, IntPtr, VkDeviceSize, VKQueryResultFlagBits, VKResult> vkGetQueryPoolResults;

		[NativeType("VkResult vkCreateBuffer(VkDevice device, const VkBufferCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkBuffer* pBuffer)")]
		public delegate* unmanaged<VkDevice, in VKBufferCreateInfo, VKAllocationCallbacks*, out VkBuffer, VKResult> vkCreateBuffer;
		[NativeType("void vkDestroyBuffer(VkDevice device, VkBuffer buffer, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkBuffer, VKAllocationCallbacks*, void> vkDestroyBuffer;
		[NativeType("VkResult vkCreateBufferView(VkDevice device, const VkBufferViewCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkBufferView* pBufferView)")]
		public delegate* unmanaged<VkDevice, in VKBufferViewCreateInfo, VKAllocationCallbacks*, out VkBufferView, VKResult> vkCreateBufferView;
		[NativeType("void vkDestroyBufferView(VkDevice device, VkBufferView bufferView, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkBufferView, VKAllocationCallbacks*, void> vkDestroyBufferView;
		[NativeType("VkResult vkCreateImage(VkDevice device, const VkImageCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkImage* pImage)")]
		public delegate* unmanaged<VkDevice, in VKImageCreateInfo, VKAllocationCallbacks*, out VkImage, VKResult> vkCreateImage;
		[NativeType("void vkDestroyImage(VkDevice device, VkImage image, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkImage, VKAllocationCallbacks*, void> vkDestroyImage;
		[NativeType("void vkGetImageSubresourceLayout(VkDevice device, VkImage image, const VkImageSubresource* pSubresource, VkSubresourceLayout* pLayout)")]
		public delegate* unmanaged<VkDevice, VkImage, in VKImageSubresource, out VKSubresourceLayout, void> vkGetImageSubresourceLayout;
		[NativeType("VkResult vkCreateImageView(VkDevice device, const VkImageViewCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkImageView* pView)")]
		public delegate* unmanaged<VkDevice, in VKImageViewCreateInfo, VKAllocationCallbacks*, out VkImageView, VKResult> vkCreateImageView;
		[NativeType("void vkDestroyImageView(VkDevice device, VkImageView imageView, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkImageView, VKAllocationCallbacks*, void> vkDestroyImageView;

		[NativeType("VkResult vkCreateShaderModule(VkDevice device, const VkShaderModuleCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkShaderModule* pShaderModule)")]
		public delegate* unmanaged<VkDevice, in VKShaderModuleCreateInfo, VKAllocationCallbacks*, out VkShaderModule, VKResult> vkCreateShaderModule;
		[NativeType("void vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkShaderModule, VKAllocationCallbacks*, void> vkDestroyShaderModule;
		[NativeType("VkResult vkCreatePipelineCache(VkDevice device, const VkPipelineCacheCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPipelineCache* pPipelineCache)")]
		public delegate* unmanaged<VkDevice, in VKPipelineCacheCreateInfo, VKAllocationCallbacks*, out VkPipelineCache, VKResult> vkCreatePipelineCache;
		[NativeType("void vkDestroyPipelineCache(VkDevice device, VkPipelineCache pipelineCache, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkPipelineCache, VKAllocationCallbacks*, void> vkDestroyPipelineCache;
		[NativeType("VkResult vkGetPipelineCacheData(VkDevice device, VkPipelineCache cache, size_t* pDataSize, void* pData)")]
		public delegate* unmanaged<VkDevice, VkPipelineCache, ref nuint, IntPtr, VKResult> vkGetPipelineCacheData;
		[NativeType("VkResult vkMergePipelineCaches(VkDevice device, VkPipelineCache dstCache, uint32_t srcCacheCount, const VkPipelineCache* pSrcCaches)")]
		public delegate* unmanaged<VkDevice, VkPipelineCache, uint, VkPipelineCache*, VKResult> vkMergePipelineCaches;
		[NativeType("VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, uint32_t createInfoCount, const VkGraphicsPipelineCreateInfo* pCreateInfos, const VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines)")]
		public delegate* unmanaged<VkDevice, VkPipelineCache, uint, VKGraphicsPipelineCreateInfo*, VKAllocationCallbacks*, VkPipeline*, VKResult> vkCreateGraphicsPipelines;
		[NativeType("VkResult vkCreateComputePipelines(VkDevice device, VkPipelineCache pipelineCache, uint32_t createInfoCount, const VkComputePipelineCreateInfo* pCreateInfos, const VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines)")]
		public delegate* unmanaged<VkDevice, VkPipelineCache, uint, VKComputePipelineCreateInfo*, VKAllocationCallbacks*, VkPipeline*, VKResult> vkCreateComputePipelines;
		[NativeType("void vkDestroyPipeline(VkDevice device, VkPipeline pipeline, const VkAllocationCallback* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkPipeline, VKAllocationCallbacks*, void> vkDestroyPipeline;
		[NativeType("VkResult vkCreatePipelineLayout(VkDevice device, const VkPipelineLayoutCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPipelineLayout* pPipelineLayout)")]
		public delegate* unmanaged<VkDevice, in VKPipelineLayoutCreateInfo, VKAllocationCallbacks*, out VkPipelineLayout, VKResult> vkCreatePipelineLayout;
		[NativeType("void vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkPipelineLayout, VKAllocationCallbacks*, void> vkDestroyPipelineLayout;

		[NativeType("VkResult vkCreateSampler(VkDevice device, const VkSamplerCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSampler* pSampler)")]
		public delegate* unmanaged<VkDevice, in VKSamplerCreateInfo, VKAllocationCallbacks*, out VkSampler, VKResult> vkCreateSampler;
		[NativeType("void vkDestroySampler(VkDevice device, VkSampler sampler, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkSampler, VKAllocationCallbacks*, void> vkDestroySampler;

		[NativeType("VkResult vkCreateDescriptorSetLayout(VkDevice device, const VkDescriptorSetLayoutCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorSetLayout* pSetLayout)")]
		public delegate* unmanaged<VkDevice, in VKDescriptorSetLayoutCreateInfo, VKAllocationCallbacks*, out VkDescriptorSetLayout, VKResult> vkCreateDescriptorSetLayout;
		[NativeType("void vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkDescriptorSetLayout, VKAllocationCallbacks*, void> vkDestroyDescriptorSetLayout;
		[NativeType("VkResult vkCreateDescriptorPool(VkDevice device, const VkDescriptorPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorPool* pDescriptorPool)")]
		public delegate* unmanaged<VkDevice, in VKDescriptorPoolCreateInfo, VKAllocationCallbacks*, out VkDescriptorPool, VKResult> vkCreateDescriptorPool;
		[NativeType("void vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkDescriptorPool, VKAllocationCallbacks*, void> vkDestroyDescriptorPool;
		[NativeType("VkResult vkResetDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, VkDescriptorPoolResetFlags flags)")]
		public delegate* unmanaged<VkDevice, VkDescriptorPool, VKDescriptorPoolResetFlagBits, VKResult> vkResetDescriptorPool;
		[NativeType("VkResult vkAllocateDescriptorSets(VkDevice device, const VkDescriptorSetAllocateInfo* pAllocateInfo, VkDescriptorSet* pDescriptorSets)")]
		public delegate* unmanaged<VkDevice, in VKDescriptorSetAllocateInfo, VkDescriptorSet*, VKResult> vkAllocateDescriptorSets;
		[NativeType("VkResult vkFreeDescriptorSets(VkDevice device, VkDescriptorPool descriptorPool, uint32_t descriptorSetCount, const VkDescriptorSet* pDescriptorSets)")]
		public delegate* unmanaged<VkDevice, VkDescriptorPool, uint, VkDescriptorSet*, VKResult> vkFreeDescriptorSets;
		[NativeType("void vkUpdateDescriptorSets(VkDevice device, uint32_t descriptorWriteCount, const VkWriteDescriptorSet* pDescriptorWrites, uint32_t descriptorCopyCount, const VkCopyDescriptorSet* pDescriptorCopies)")]
		public delegate* unmanaged<VkDevice, uint, VKWriteDescriptorSet*, uint, VKCopyDescriptorSet*, void> vkUpdateDescriptorSets;

		[NativeType("VkResult vkCreateFramebuffer(VkDevice device, const VkFramebufferCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkFramebuffer* pFramebuffer)")]
		public delegate* unmanaged<VkDevice, in VKFramebufferCreateInfo, VKAllocationCallbacks*, out VkFramebuffer, VKResult> vkCreateFramebuffer;
		[NativeType("void vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkFramebuffer, VKAllocationCallbacks*, void> vkDestroyFramebuffer;
		[NativeType("VkResult vkCreateRenderPass(VkDevice device, const VkRenderPassCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass)")]
		public delegate* unmanaged<VkDevice, in VKRenderPassCreateInfo, VKAllocationCallbacks*, out VkRenderPass, VKResult> vkCreateRenderPass;
		[NativeType("void vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkRenderPass, VKAllocationCallbacks*, void> vkDestroyRenderPass;
		[NativeType("void vkGetRenderAreaGranularity(VkDevice device, VkRenderPass renderPass, VkExtent2D* pGranularity)")]
		public delegate* unmanaged<VkDevice, VkRenderPass, out VKExtent2D, void> vkGetRenderAreaGranularity;

		[NativeType("VkResult vkCreateCommandPool(VkDevice device, const VkCommandPoolCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkCommandPool* pCommandPool)")]
		public delegate* unmanaged<VkDevice, in VKCommandPoolCreateInfo, VKAllocationCallbacks*, out VkCommandPool, VKResult> vkCreateCommandPool;
		[NativeType("void vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkCommandPool, VKAllocationCallbacks*, void> vkDestroyCommandPool;
		[NativeType("VkResult vkResetCommandPool(VkDevice device, VkCommandPool commandPool, VkCommandPoolResetFlags flags)")]
		public delegate* unmanaged<VkDevice, VkCommandPool, VKCommandPoolResetFlagBits, VKResult> vkResetCommandPool;
		[NativeType("VkResult vkAllocateCommandBuffers(VkDevice device, const VkCommandBufferAllocateInfo* pAllocateInfo, VkCommandBuffer* pCommandBuffers)")]
		public delegate* unmanaged<VkDevice, in VKCommandBufferAllocateInfo, VkCommandBuffer*, VKResult> vkAllocateCommandBuffers;
		[NativeType("void vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, uint32_t commandBufferCount, const VkCommandBuffer* pCommandBuffers)")]
		public delegate* unmanaged<VkDevice, VkCommandPool, uint, VkCommandBuffer*, void> vkFreeCommandBuffers;
		[NativeType("VkResult vkBeginCommandBuffer(VkCommandBuffer commandBuffer, const VkCommandBufferBeginInfo* pBeginInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKCommandBufferBeginInfo, VKResult> vkBeginCommandBuffer;
		[NativeType("VkResult vkEndCommandBuffer(VkCommandBuffer commandBuffer)")]
		public delegate* unmanaged<VkCommandBuffer, VKResult> vkEndCommandBuffer;
		[NativeType("VkResult vkResetCommandBuffer(VkCommandBuffer commandBuffer, VkCommandBufferResetFlags flags)")]
		public delegate* unmanaged<VkCommandBuffer, VKCommandBufferResetFlagBits, VKResult> vkResetCommandBuffer;

		[NativeType("void vkCmdBindPipeline(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)")]
		public delegate* unmanaged<VkCommandBuffer, VKPipelineBindPoint, VkPipeline, void> vkCmdBindPipeline;
		[NativeType("void vkCmdSetViewport(VkCommandBuffer commandBuffer, uint32_t firstViewport, uint32_t viewportCount, const VkViewport* pViewports)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, VKViewport*, void> vkCmdSetViewport;
		[NativeType("void vkCmdSetScissor(VkCommandBuffer commandBuffer, uint32_t firstScissor, uint32_t scissorCount, const VkRect2D* pScissors)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, VKRect2D*, void> vkCmdSetScissor;
		[NativeType("void vkCmdSetLineWidth(VkCommandBuffer commandBuffer, float lineWidth)")]
		public delegate* unmanaged<VkCommandBuffer, float, void> vkCmdSetLineWidth;
		[NativeType("void vkCmdSetDepthBias(VkCommandBuffer commandBuffer, float depthBiasFactor, float depthBiasClamp, float depthBiasSlopeFactor)")]
		public delegate* unmanaged<VkCommandBuffer, float, float, float, void> vkCmdSetDepthBias;
		[NativeType("void vkCmdSetBlendConstants(VkCommandBuffer commandBuffer, const float[4] blendConstants)")]
		public delegate* unmanaged<VkCommandBuffer, in Vector4, void> vkCmdSetBlendConstants;
		[NativeType("void vkCmdSetDepthBounds(VkCommandBuffer commandBuffer, float minDepthBounds, float maxDepthBounds)")]
		public delegate* unmanaged<VkCommandBuffer, float, float, void> vkCmdSetDepthBounds;
		[NativeType("void vkCmdSetStencilCompareMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t compareMask)")]
		public delegate* unmanaged<VkCommandBuffer, VKStencilFaceFlagBits, uint, void> vkCmdSetStencilCompareMask;
		[NativeType("void vkCmdSetStencilWriteMask(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t writeMask)")]
		public delegate* unmanaged<VkCommandBuffer, VKStencilFaceFlagBits, uint, void> vkCmdSetStencilWriteMask;
		[NativeType("void vkCmdSetStencilReference(VkCommandBuffer commandBuffer, VkStencilFaceFlags faceMask, uint32_t reference)")]
		public delegate* unmanaged<VkCommandBuffer, VKStencilFaceFlagBits, uint, void> vkCmdSetStencilReference;
		[NativeType("void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, uint32_t firstSet, uint32_t descriptorSetCount, const VkDescriptorSet* pDescriptorSets, uint32_t dynamicOffsetCount, const uint32_t* pDynamicOffsets)")]
		public delegate* unmanaged<VkCommandBuffer, VKPipelineBindPoint, VkPipelineLayout, uint, uint, VkDescriptorSet*, uint, uint*, void> vkCmdBindDescriptorSets;
		[NativeType("void vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkIndexType indexType)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, VKIndexType, void> vkCmdBindIndexBuffer;
		[NativeType("void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, uint32_t firstBinding, uint32_t bindingCount, const VkBuffer* pBuffers, const VkDeviceSize* pOffsets)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, VkBuffer*, VkDeviceSize*, void> vkCmdBindVertexBuffers;
		[NativeType("void vkCmdDraw(VkCommandBuffer commandBuffer, uint32_t vertexCount, uint32_t instanceCount, uint32_t firstVertex, uint32_t firstInstance)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, uint, uint, void> vkCmdDraw;
		[NativeType("void vkCmdDrawIndexed(VkCommandBuffer commandBuffer, uint32_t indexCount, uint32_t instanceCount, uint32_t firstIndex, int32_t vertexOffset, uint32_t firstInstance)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, uint, int, uint, void> vkCmdDrawIndexed;
		[NativeType("void vkCmdDrawIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint32_t drawCount, uint32_t stride)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, uint, uint, void> vkCmdDrawIndirect;
		[NativeType("void vkCmdDrawIndexedIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, uint32_t drawCount, uint32_t stride)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, uint, uint, void> vkCmdDrawIndexedIndirect;
		[NativeType("void vkCmdDispatch(VkCommandBuffer commandBuffer, uint32_t groupCountX, uint32_t groupCountY, uint32_t groupCountZ)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, uint, void> vkCmdDispatch;
		[NativeType("void vkCmdDispatchIndirect(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, void> vkCmdDispatchIndirect;
		[NativeType("void vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, uint32_t regionCount, const VkBufferCopy* pRegions)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkBuffer, uint, VKBufferCopy*, void> vkCmdCopyBuffer;
		[NativeType("void vkCmdCopyImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkImageCopy* pRegions)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, VkImage, VKImageLayout, uint, VKImageCopy*, void> vkCmdCopyImage;
		[NativeType("void vkCmdBlitImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkImageBlit* pRegions, VkFilter filter)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, VkImage, VKImageLayout, uint, VKImageBlit*, VKFilter, void> vkCmdBlitImage;
		[NativeType("void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, const VkBufferImageCopy* pRegions)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkImage, VKImageLayout, uint, VKBufferImageCopy*, void> vkCmdCopyBufferToImage;
		[NativeType("void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout, VkBuffer dstBuffer, uint32_t regionCount, const VkBufferImageCopy* pRegions)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, VkBuffer, uint, VKBufferImageCopy*, void> vkCmdCopyImageToBuffer;
		[NativeType("void vkCmdUpdateBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize dataSize, void* pData)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, VkDeviceSize, IntPtr, void> vkCmdUpdateBuffer;
		[NativeType("void vkCmdFillBuffer(VkCommandBuffer commandBuffer, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize size, uint32_t data)")]
		public delegate* unmanaged<VkCommandBuffer, VkBuffer, VkDeviceSize, VkDeviceSize, uint, void> vkCmdFillBuffer;
		[NativeType("void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, const VkClearColorValue* pColor, uint32_t rangeCount, const VkImageSubresourceRange* pRanges)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, in VKClearColorValue, uint, VKImageSubresourceRange*, void> vkCmdClearColorImage;
		[NativeType("void vkCmdClearColorImage(VkCommandBuffer commandBuffer, VkImage image, VkImageLayout imageLayout, const VkClearDepthStencilValue* pDepthStencil, uint32_t rangeCount, const VkImageSubresourceRange* pRanges)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, in VKClearDepthStencilValue, uint, VKImageSubresourceRange*, void> vkCmdClearDepthStencilImage;
		[NativeType("void vkCmdClearAttachments(VkCommandBuffer commandBuffer, uint32_t attachmentCount, const VkClearAttachment* pAttachments, uint32_t rectCount, const VkClearRect* pRects)")]
		public delegate* unmanaged<VkCommandBuffer, uint, VKClearAttachment*, uint, VKClearRect*, void> vkCmdClearAttachments;
		[NativeType("void vkCmdResolve(VkCommandBuffer commandBuffer, VkImage srcImage, VkImageLayout srcImageLayout VkImage dstImage, VkImageLayout dstImageLayout, uint32_t regionCount, VkImageResolve* pRegions)")]
		public delegate* unmanaged<VkCommandBuffer, VkImage, VKImageLayout, VkImage, VKImageLayout, uint, VKImageResolve*, void> vkCmdResolveImage;
		[NativeType("void vkCmdSetEvent(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags stageMask)")]
		public delegate* unmanaged<VkCommandBuffer, VkEvent, VKPipelineStageFlagBits, void> vkCmdSetEvent;
		[NativeType("void vkCmdResetEvent(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags stageMask)")]
		public delegate* unmanaged<VkCommandBuffer, VKEvent, VKPipelineStageFlagBits, void> vkCmdResetEvent;
		[NativeType("void vkCmdWaitEvents(VkCommandBuffer commandBuffer, uint32_t eventCount, const VkEvent* pEvents, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, uint32_t memoryBarrierCount, const VkMemoryBarrier* pMemoryBarriers, uint32_t bufferMemoryBarrierCount, const VkBufferMemoryBarrier* pBufferMemoryBarriers, uint32_t imageMemoryBarrierCount, const VkImageMemoryBarrier* pImageMemoryBarriers)")]
		public delegate* unmanaged<VkCommandBuffer, uint, VkEvent*, VKPipelineStageFlagBits, VKPipelineStageFlagBits, uint, VKMemoryBarrier*, uint, VKBufferMemoryBarrier*, uint, VKImageMemoryBarrier*, void> vkCmdWaitEvents;
		[NativeType("void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlags srcStageMask, VkPipelineStageFlags dstStageMask, VkDependencyFlags dependencyFlags, uint32_t memoryBarrierCount, const VkMemoryBarrier* pMemoryBarriers, uint32_t bufferMemoryBarrierCount, const VkBufferMemoryBarrier* pBufferMemoryBarriers, uint32_t imageMemoryBarrierCount, const VkImageMemoryBarrier* pImageMemoryBarriers)")]
		public delegate* unmanaged<VkCommandBuffer, VKPipelineStageFlagBits, VKPipelineStageFlagBits, VKDependencyFlagBits, uint, VKMemoryBarrier*, uint, VKBufferMemoryBarrier*, uint, VKImageMemoryBarrier*, void> vkCmdPipelineBarrier;
		[NativeType("void vkCmdBeginQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool uint32_t query, VkQueryControlFlags flags)")]
		public delegate* unmanaged<VkCommandBuffer, VkQueryPool, uint, VKQueryControlFlagBits, void> vkCmdBeginQuery;
		[NativeType("void vkCmdEndQuery(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t query)")]
		public delegate* unmanaged<VkCommandBuffer, VkQueryPool, uint, void> vkCmdEndQuery;
		[NativeType("void vkCmdResetQueryPool(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount)")]
		public delegate* unmanaged<VkCommandBuffer, VkQueryPool, uint, uint, void> vkCmdResetQueryPool;
		[NativeType("void vkCmdWriteTimestamp(VkCommandBuffer commandBuffer, VkPipelineStageFlags pipelineStage, VkQueryPool queryPool, uint32_t query)")]
		public delegate* unmanaged<VkCommandBuffer, VKPipelineStageFlagBits, VkQueryPool, uint, void> vkCmdWriteTimestamp;
		[NativeType("void vkCmdCopyQueryPoolResults(VkCommandBuffer commandBuffer, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount, VkBuffer dstBuffer, VkDeviceSize dstOffset, VkDeviceSize stride, VkQueryResultFlags flags)")]
		public delegate* unmanaged<VkCommandBuffer, VkQueryPool, uint, uint, VkBuffer, VkDeviceSize, VkDeviceSize, VKQueryResultFlagBits, void> vkCmdCopyQueryPoolResults;
		[NativeType("void vkCmdPushConstants(VkCommandBuffer commandBuffer, VkPipelineLayout layout, VkShaderStageFlags stageFlags, uint32_t offset, uint32_t size, void* pValues)")]
		public delegate* unmanaged<VkCommandBuffer, VkPipelineLayout, VKShaderStageFlagBits, uint, uint, IntPtr, void> vkCmdPushConstants;
		[NativeType("void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, const VkRenderPassBeginInfo* pRenderPassBegin, VkSubpassContents contents)")]
		public delegate* unmanaged<VkCommandBuffer, in VKRenderPassBeginInfo, VKSubpassContents, void> vkCmdBeginRenderPass;
		[NativeType("void vkCmdNextSubpass(VkCommandBuffer commandBuffer, VkSubpassContents contents)")]
		public delegate* unmanaged<VkCommandBuffer, VKSubpassContents, void> vkCmdNextSubpass;
		[NativeType("void vkCmdEndRenderPass(VkCommandBuffer commandBuffer)")]
		public delegate* unmanaged<VkCommandBuffer, void> vkCmdEndRenderPass;
		[NativeType("void vkCmdExecuteCommands(VkCommandBuffer commandBuffer, uint32_t commandBufferCount, const VkCommandBuffer* pCommandBuffers)")]
		public delegate* unmanaged<VkCommandBuffer, uint, IntPtr*, void> vkCmdExecuteCommands;

		public static implicit operator bool(VK10DeviceFunctions? fn) => fn != null;

	}

}
