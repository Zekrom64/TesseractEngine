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

	public class VK11Functions {

		public delegate VKResult PFN_vkEnumerateInstanceVersion(out uint apiVersion);

		public PFN_vkEnumerateInstanceVersion vkEnumerateInstanceVersion;

	}

	public class VK11InstanceFunctions {

		public delegate VKResult PFN_vkEnumeratePhysicalDeviceGroups(VkInstance instance, ref uint physicalDeviceGroupCount, [NativeType("VkPhysicalDeviceGroupProperties*")] IntPtr pPhysicalDeviceGroupProperties);
		public delegate void PFN_vkGetPhysicalDeviceFeatures2(VkPhysicalDevice physicalDevice, out VKPhysicalDeviceFeatures2 features);
		public delegate void PFN_vkGetPhysicalDeviceProperties2(VkPhysicalDevice physicalDevice, out VKPhysicalDeviceProperties2 properties);
		public delegate void PFN_vkGetPhysicalDeviceFormatProperties2(VkPhysicalDevice physicalDevice, VKFormat format, out VKFormatProperties2 formatProperties);
		public delegate VKResult PFN_vkGetPhysicalDeviceImageFormatProperties2(VkPhysicalDevice physicalDevice, in VKPhysicalDeviceImageFormatInfo2 imageFormatInfo, out VKImageFormatProperties2 imageFormatProperties);
		public delegate void PFN_vkGetPhysicalDeviceQueueFamilyProperties2(VkPhysicalDevice physicalDevice, ref uint queueFamilyPropertyCount, [NativeType("VkQueueFamilyProperties2")] IntPtr queueFamilyProperties);
		public delegate void PFN_vkGetPhysicalDeviceMemoryProperties2(VkPhysicalDevice physicalDevice, out VKPhysicalDeviceMemoryProperties2 memoryProperties);
		public delegate void PFN_vkGetPhysicalDeviceSparseImageFormatProperties2(VkPhysicalDevice physicalDevice, in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, ref uint propertyCount, [NativeType("VkSparseImageFormatProperties2*")] IntPtr pProperties);
		public delegate void PFN_vkGetPhysicalDeviceExternalBufferProperties(VkPhysicalDevice physicalDevice, in VKPhysicalDeviceExternalBufferInfo info, out VKExternalBufferProperties externalBufferProperties);
		public delegate void PFN_vkGetPhysicalDeviceExternalFenceProperties(VkPhysicalDevice physicalDevice, in VKPhysicalDeviceExternalFenceInfo info, out VKExternalFenceProperties externalFenceProperties);
		public delegate void PFN_vkGetPhysicalDeviceExternalSemaphoreProperties(VkPhysicalDevice physicalDevice, in VKPhysicalDeviceExternalSemaphoreInfo info, out VKExternalSemaphoreProperties externalSemaphoreProperties);

		public PFN_vkEnumeratePhysicalDeviceGroups vkEnumeratePhysicalDeviceGroups;
		public PFN_vkGetPhysicalDeviceFeatures2 vkGetPhysicalDeviceFeatures2;
		public PFN_vkGetPhysicalDeviceProperties2 vkGetPhysicalDeviceProperties2;
		public PFN_vkGetPhysicalDeviceFormatProperties2 vkGetPhysicalDeviceFormatProperties2;
		public PFN_vkGetPhysicalDeviceImageFormatProperties2 vkGetPhysicalDeviceImageFormatProperties2;
		public PFN_vkGetPhysicalDeviceQueueFamilyProperties2 vkGetPhysicalDeviceQueueFamilyProperties2;
		public PFN_vkGetPhysicalDeviceMemoryProperties2 vkGetPhysicalDeviceMemoryProperties2;
		public PFN_vkGetPhysicalDeviceSparseImageFormatProperties2 vkGetPhysicalDeviceSparseImageFormatProperties2;
		public PFN_vkGetPhysicalDeviceExternalBufferProperties vkGetPhysicalDeviceExternalBufferProperties;
		public PFN_vkGetPhysicalDeviceExternalFenceProperties vkGetPhysicalDeviceExternalFenceProperties;
		public PFN_vkGetPhysicalDeviceExternalSemaphoreProperties vkGetPhysicalDeviceExternalSemaphoreProperties;

	}

	public class VK11DeviceFunctions {

		public delegate VKResult PFN_vkBindBufferMemory2(VkDevice device, uint bindInfoCount, [NativeType("const VkBindBufferMemoryInfo*")] IntPtr pBindInfos);
		public delegate VKResult PFN_vkBindImageMemory2(VkDevice device, uint bindInfoCount, [NativeType("const VkBindImageMemoryInfo*")] IntPtr pBindInfos);
		public delegate void PFN_vkGetDeviceGroupPeerMemoryFeatures(VkDevice device, uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out VKPeerMemoryFeatureFlagBits peerMemoryFeatures);
		public delegate void PFN_vkCmdSetDeviceMask(VkCommandBuffer commandBuffer, uint deviceMask);
		public delegate void PFN_vkCmdDispatchBase(VkCommandBuffer commandBuffer, uint baseGroupX, uint baseGroupY, uint baseGroupZ, uint groupCountX, uint groupCountY, uint groupCountZ);
		public delegate void PFN_vkGetImageMemoryRequirements2(VkDevice device, in VKImageMemoryRequirementsInfo2 info, out VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetBufferMemoryRequirements2(VkDevice device, in VKBufferMemoryRequirementsInfo2 info, out VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetImageSparseMemoryRequirements2(VkDevice device, in VKImageSparseMemoryRequirementsInfo2 info, ref uint SparseMemoryRequirementCount, [NativeType("VkSparseImageMemoryRequirements2*")] IntPtr pSparseMemoryRequirements);
		public delegate void PFN_vkTrimCommandPool(VkDevice device, VkCommandPool commandPool, VKCommandPoolTrimFlags flags);
		public delegate void PFN_vkGetDeviceQueue2(VkDevice device, in VKDeviceQueueInfo2 queueInfo, out VkQueue queue);
		public delegate VKResult PFN_vkCreateSamplerYcbcrConversion(VkDevice device, in VKSamplerYcbcrConversionInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkSamplerYcbcrConversion ycbcrConversion);
		public delegate void PFN_vkDestroySamplerYcbcrConversion(VkDevice device, VkSamplerYcbcrConversion ycbcrConversion, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate VKResult PFN_vkCreateDescriptorUpdateTemplate(VkDevice device, in VKDescriptorUpdateTemplateCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkDescriptorUpdateTemplate descriptorUpdateTemplate);
		public delegate void PFN_vkDestroyDescriptorUpdateTemplate(VkDevice device, VkDescriptorUpdateTemplate descriptorUpdateTemplate, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkUpdateDescriptorSetWithTemplate(VkDevice device, VkDescriptorSet descriptorSet, VkDescriptorUpdateTemplate descriptorUpdateTemplate, IntPtr pData);
		public delegate void PFN_vkGetDescriptorSetLayoutSupport(VkDevice device, in VKDescriptorSetLayoutCreateInfo createInfo, out VKDescriptorSetLayoutSupport support);

		public PFN_vkBindBufferMemory2 vkBindBufferMemory2;
		public PFN_vkBindImageMemory2 vkBindImageMemory2;
		public PFN_vkGetDeviceGroupPeerMemoryFeatures vkGetDeviceGroupPeerMemoryFeatures;
		public PFN_vkCmdSetDeviceMask vkCmdSetDeviceMask;
		public PFN_vkCmdDispatchBase vkCmdDispatchBase;
		public PFN_vkGetImageMemoryRequirements2 vkGetImageMemoryRequirements2;
		public PFN_vkGetBufferMemoryRequirements2 vkGetBufferMemoryRequirements2;
		public PFN_vkGetImageSparseMemoryRequirements2 vkGetImageSparseMemoryRequirements2;
		public PFN_vkTrimCommandPool vkTrimCommandPool;
		public PFN_vkGetDeviceQueue2 vkGetDeviceQueue2;
		public PFN_vkCreateSamplerYcbcrConversion vkCreateSamplerYcbcrConversion;
		public PFN_vkDestroySamplerYcbcrConversion vkDestroySamplerYcbcrConversion;
		public PFN_vkCreateDescriptorUpdateTemplate vkCreateDescriptorUpdateTemplate;
		public PFN_vkDestroyDescriptorUpdateTemplate vkDestroyDescriptorUpdateTemplate;
		public PFN_vkUpdateDescriptorSetWithTemplate vkUpdateDescriptorSetWithTemplate;
		public PFN_vkGetDescriptorSetLayoutSupport vkGetDescriptorSetLayoutSupport;

	}

}
