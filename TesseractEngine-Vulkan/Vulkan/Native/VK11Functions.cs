using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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

	public unsafe class VK11Functions {

		[NativeType("VkResult vkEnumerateInstanceVersion(uint32_t* pApiVersion)")]
		public delegate* unmanaged<out uint, VKResult> vkEnumerateInstanceVersion;

	}

	public unsafe class VK11InstanceFunctions {

		[NativeType("VkResult vkEnumeratePhysicalDeviceGroups(VkInstance instance, uint32_t* pPhysicalDeviceGroupCount, VkPhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties)")]
		public delegate* unmanaged<VkInstance, ref uint, VKPhysicalDeviceGroupProperties*, VKResult> vkEnumeratePhysicalDeviceGroups;
		[NativeType("void vkGetPhysicalDeviceFeatures2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures2* pFeatures)")]
		public delegate* unmanaged<VkPhysicalDevice, ref VKPhysicalDeviceFeatures2, void> vkGetPhysicalDeviceFeatures2;
		[NativeType("void vkGetPhysicalDeviceProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties2* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, ref VKPhysicalDeviceProperties2, void> vkGetPhysicalDeviceProperties2;
		[NativeType("void vkGetPhysicalDeviceFormatProperties2(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties2* pFormatProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, VKFormat, ref VKFormatProperties2, void> vkGetPhysicalDeviceFormatProperties2;
		[NativeType("VkResult vkGetPhysicalDeviceImageFormatProperties2(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceImageFormatInfo2* pImageFormatInfo, VkImageFormatProperties2* pImageFormatProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKPhysicalDeviceImageFormatInfo2, ref VKImageFormatProperties2, VKResult> vkGetPhysicalDeviceImageFormatProperties2;
		[NativeType("void vkGetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDevice physicalDevice, uint32_t* pQueueFamilyPropertyCount, VkQueueFamilyProperties2* pQueueFamilyProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, ref uint, VKQueueFamilyProperties2*, void> vkGetPhysicalDeviceQueueFamilyProperties2;
		[NativeType("void vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties2* pMemoryProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, ref VKPhysicalDeviceMemoryProperties2, void> vkGetPhysicalDeviceMemoryProperties2;
		[NativeType("void vkGetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint32_t* pPropertyCount, VkSparseImageFormatProperties* pProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKPhysicalDeviceSparseImageFormatInfo2, ref uint, VKSparseImageFormatProperties2*, void> vkGetPhysicalDeviceSparseImageFormatProperties2;
		[NativeType("void vkGetPhysicalDeviceExternalBufferProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalBufferInfo* pInfo, VkExternalBufferProperties* pExternalBufferProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKPhysicalDeviceExternalBufferInfo, ref VKExternalBufferProperties, void> vkGetPhysicalDeviceExternalBufferProperties;
		[NativeType("void vkGetPhysicalDeviceExternalFenceProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalFenceInfo* pInfo, VkExternalFenceProperties* pExternalFenceProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKPhysicalDeviceExternalFenceInfo, ref VKExternalFenceProperties, void> vkGetPhysicalDeviceExternalFenceProperties;
		[NativeType("void vkGetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalSemaphoreInfo* pInfo, VkExternalSemaphoreProperties* pExternalSemaphoreProperties)")]
		public delegate* unmanaged<VkPhysicalDevice, in VKPhysicalDeviceExternalSemaphoreInfo, ref VKExternalSemaphoreProperties, void> vkGetPhysicalDeviceExternalSemaphoreProperties;

	}

	public unsafe class VK11DeviceFunctions {

		[NativeType("VkResult vkBindBufferMemory2(VkDevice device, uint32_t bindInfoCount, const VkBindBufferMemoryInfo* pBindInfos)")]
		public delegate* unmanaged<VkDevice, uint, VKBindBufferMemoryInfo*, VKResult> vkBindBufferMemory2;
		[NativeType("VkResult vkBindImageMemory2(VkDevice device, uint32_t bindInfoCount, const VkBindImageMemoryInfo* pBindInfos)")]
		public delegate* unmanaged<VkDevice, uint, VKBindImageMemoryInfo*, VKResult> vkBindImageMemory2;
		[NativeType("void vkGetDeviceGroupPeerMemoryFeatures(VkDevice device, uint32_t heapIndex, uint32_t localDeviceIndex, uint32_t remoteDeviceIndex, VkPeerMemoryFeatureFlags* pPeerMemoryFeatures)")]
		public delegate* unmanaged<VkDevice, uint, uint, uint, out VKPeerMemoryFeatureFlagBits, void> vkGetDeviceGroupPeerMemoryFeatures;
		[NativeType("void vkCmdSetDeviceMask(VkCommandBuffer commandBuffer, uint32_t deviceMask)")]
		public delegate* unmanaged<VkCommandBuffer, uint, void> vkCmdSetDeviceMask;
		[NativeType("void vkCmdDispatchBase(VkCommandBuffer commandBuffer, uint32_t baseGroupX, uint32_t baseGroupY, uint32_t baseGroupZ, uint32_t groupCountX, uint32_t groupCountY, uint32_t groupCountZ)")]
		public delegate* unmanaged<VkCommandBuffer, uint, uint, uint, uint, uint, uint, void> vkCmdDispatchBase;
		[NativeType("void vkGetImageMemoryRequirements2(VkDevice device, const VkImageMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKImageMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetImageMemoryRequirements2;
		[NativeType("void vkGetBufferMemoryRequirements2(VkDevice device, const VkBufferMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKBufferMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetBufferMemoryRequirements2;
		[NativeType("void vkGetImageSparseMemoryRequirements2(VkDevice device, const VkImageSparseMemoryRequirementsInfo2* pInfo, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKImageSparseMemoryRequirementsInfo2, ref uint, VKSparseImageMemoryRequirements2*, void> vkGetImageSparseMemoryRequirements2;
		[NativeType("void vkTrimCommandPool(VkDevice device, VkCommandPool commandPool, VkCommandPoolTrimFlags flags)")]
		public delegate* unmanaged<VkDevice, VkCommandPool, VKCommandPoolTrimFlags, void> vkTrimCommandPool;
		[NativeType("void vkGetDeviceQueue2(VkDevice device, const VkDeviceQueueInfo2* pQueueInfo, VkQueue* pQueue)")]
		public delegate* unmanaged<VkDevice, in VKDeviceQueueInfo2, out VkQueue, void> vkGetDeviceQueue2;
		[NativeType("VkResult vkCreateSamplerYcbcrConversion(VkDevice device, const VkSamplerYcbcrConversionCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSamplerYcbcrConversion* pYcbcrConversion)")]
		public delegate* unmanaged<VkDevice, in VKSamplerYcbcrConversionCreateInfo, VKAllocationCallbacks*, out VkSamplerYcbcrConversion, VKResult> vkCreateSamplerYcbcrConversion;
		[NativeType("void vkDestroySamplerYcbcrConversion(VkDevice device, VkSamplerYcbcrConversion, VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkSamplerYcbcrConversion, VKAllocationCallbacks*, void> vkDestroySamplerYcbcrConversion;
		[NativeType("VkResult vkCreateDescriptorUpdateTemplate(VkDevice device, const VkDescriptorUpdateTemplateCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorUpdateTemplate* pDescriptorUpdateTemplate)")]
		public delegate* unmanaged<VkDevice, in VKDescriptorUpdateTemplateCreateInfo, VKAllocationCallbacks*, out VkDescriptorUpdateTemplate, VKResult> vkCreateDescriptorUpdateTemplate;
		[NativeType("void vkDestroyDescriptorUpdateTemplate(VkDevice device, VkDescriptorUpdateTemplate descriptorUpdateTemplate, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<VkDevice, VkDescriptorUpdateTemplate, VKAllocationCallbacks*, void> vkDestroyDescriptorUpdateTemplate;
		[NativeType("void vkUpdateDescriptorSetWithTemplate(VkDevice device, VkDescriptorSet descriptorSet, VkDescriptorUpdateTemplate descriptorUpdateTemplate, void* pData)")]
		public delegate* unmanaged<VkDevice, VkDescriptorSet, VkDescriptorUpdateTemplate, IntPtr, void> vkUpdateDescriptorSetWithTemplate;
		[NativeType("void vkGetDescriptorSetLayoutSupport(VkDevice device, const VkDescriptorSetLayoutCreateInfo* pCreateInfo, VkDescriptorSetLayoutSupport* pSupport)")]
		public delegate* unmanaged<VkDevice, in VKDescriptorSetLayoutCreateInfo, ref VKDescriptorSetLayoutSupport, void> vkGetDescriptorSetLayoutSupport;

		public static implicit operator bool(VK11DeviceFunctions? fn) => fn != null;

	}

}
