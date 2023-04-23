using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	// VK_KHR_16bit_storage

	public static class KHR16BitStorage {

		public const string ExtensionName = "VK_KHR_16bit_storage";

	}

	// VK_KHR_bind_memory2

	public unsafe class KHRBindMemory2DeviceFunctions {

		[NativeType("VkResult vkBindBufferMemory2KHR(VKDevice device, uitn32_t bindInfoCount, const VkBindBufferMemoryInfo* pBindInfos)")]
		public delegate* unmanaged<IntPtr, uint, VKBindBufferMemoryInfo*, VKResult> vkBindBufferMemory2KHR;
		[NativeType("VkResult vkBindImageMemory2KHR(VkDevice device, uint32_t bindInfoCount, const VkBindImageMemoryInfo* pBindInfos)")]
		public delegate* unmanaged<IntPtr, uint, VKBindImageMemoryInfo*, VKResult> vkBindImageMemory2KHR;

		public static implicit operator bool(KHRBindMemory2DeviceFunctions fn) => fn != null;

	}

	public static class KHRBindMemory2 {

		public const string ExtensionName = "VK_KHR_bind_memory2";

	}

	// VK_KHR_dedicated_allocation

	public static class KHRDedicatedAllocation {

		public const string ExtensionName = "VK_KHR_dedicated_allocation";

	}

	// VK_KHR_descriptor_update_template

	public unsafe class KHRDescriptorUpdateTemplateDeviceFunctions {

		[NativeType("VkResult vkCreateDescriptorUpdateTemplateKHR(VkDevice device, const VkDescriptorUpdateTemplateCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDescriptorUpdateTemplate* pDescriptorUpdateTemplate)")]
		public delegate* unmanaged<IntPtr, in VKDescriptorUpdateTemplateCreateInfo, VKAllocationCallbacks*, out ulong, VKResult> vkCreateDescriptorUpdateTemplateKHR;
		[NativeType("void vkDestroyDescriptorUpdateTemplateKHR(VkDevice device, VkDescriptorUpdateTemplate descriptorUpdateTemplate, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroyDescriptorUpdateTemplateKHR;
		[NativeType("void vkUpdateDescriptorSetWithTemplateKHR(VkDevice device, VkDescriptorSet descriptorSet, VkDescriptorUpdateTemplate descriptorUpdateTemplate, void* pData)")]
		public delegate* unmanaged<IntPtr, ulong, ulong, IntPtr, void> vkUpdateDescriptorSetWithTemplateKHR;

		public static implicit operator bool(KHRDescriptorUpdateTemplateDeviceFunctions fn) => fn != null;

	}

	public static class KHRDescriptorUpdateTemplate {

		public const string ExtensionName = "VK_KHR_descriptor_update_template";

	}

	// VK_KHR_device_group

	public unsafe class KHRDeviceGroupDeviceFunctions {

		[NativeType("void vkCmdDispatchBaseKHR(VkCommandBuffer commandBuffer, uint32_t baseGroupX, uint32_t baseGroupY, uint32_t baseGroupZ, uint32_t groupCountX, uint32_t groupCcountY, uint32_t groupCountZ)")]
		public delegate* unmanaged<IntPtr, uint, uint, uint, uint, uint, uint, void> vkCmdDispatchBaseKHR;
		[NativeType("void vkCmdSetDeviceMaskKHR(VkCommandBuffer commandBuffer, uint32_t deviceMask)")]
		public delegate* unmanaged<IntPtr, uint, void> vkCmdSetDeviceMaskKHR;
		[NativeType("void vkGetDeviceGroupPeerMemoryFeaturesKHR(VkDevice device, uint32_t heapIndex, uint32_t localDeviceIndex, uint32_t remoteDeviceIndex, VkPeerMemoryFeatureFlags* pPeerMemoryFeatures)")]
		public delegate* unmanaged<IntPtr, uint, uint, uint, out VKPeerMemoryFeatureFlagBits, void> vkGetDeviceGroupPeerMemoryFeaturesKHR;

		public static implicit operator bool(KHRDeviceGroupDeviceFunctions fn) => fn != null;

	}

	public static class KHRDeviceGroup {

		public const string ExtensionName = "VK_KHR_device_group";

	}

	// VK_KHR_device_group_creation [I]

	public unsafe class KHRDeviceGroupCreationInstanceFunctions {

		[NativeType("VkResult vkEnumeratePhysicalDeviceGroupsKHR(VkInstance instance, uint32_t* pPhysicalDeviceGroupCount, VkPhysicalDeviceGroupProperties* pPhysicalDeviceGroupProperties)")]
		public delegate* unmanaged<IntPtr, ref uint, VKPhysicalDeviceGroupProperties*, VKResult> vkEnumeratePhysicalDeviceGroupsKHR;

	}

	public static class KHRDeviceGroupCreation {

		public const string ExtensionName = "VK_KHR_device_group_creation";

	}

	// VK_KHR_external_fence

	public static class KHRExternalFence {

		public const string ExtensionName = "VK_KHR_external_fence";

	}

	// VK_KHR_external_fence_capabilities [I]

	public unsafe class KHRExternalFenceCapabilitiesInstanceFunctions {

		[NativeType("void vkGetPhysicalDeviceExternalFencePropertiesKHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalFenceInfo* pExternalFenceInfo, VKExternalFenceProperties* pExternalFenceProperties)")]
		public delegate* unmanaged<IntPtr, in VKPhysicalDeviceExternalFenceInfo, ref VKExternalFenceProperties, void> vkGetPhysicalDeviceExternalFencePropertiesKHR;

	}

	public static class KHRExternalFenceCapabilities {

		public const string ExtensionName = "VK_KHR_external_fence_capabilities";

	}

	// VK_KHR_external_memory

	public static class KHRExternalMemory {

		public const string ExtensionName = "VK_KHR_external_memory";

	}

	// VK_KHR_external_memory_capabilities [I]

	public unsafe class KHRExternalMemoryCapabilitiesInstanceFunctions {

		[NativeType("void vkGetPhysicalDeviceExternalBufferPropertiesKHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalBufferInfo* pExternalBufferInfo, VkExternalBufferProperties* pExternalBufferProperties)")]
		public delegate* unmanaged<IntPtr, in VKPhysicalDeviceExternalBufferInfo, ref VKExternalBufferProperties, void> vkGetPhysicalDeviceExternalBufferPropertiesKHR;

	}

	public static class KHRExternalMemoryCapabilities {

		public const string ExtensionName = "VK_KHR_external_memory_capabilities";

	}

	// VK_KHR_external_semaphore

	public static class KHRExternalSemaphore {

		public const string ExtensionName = "VK_KHR_external_semaphore";

	}

	// VK_KHR_external_semaphore_capabilities [I]

	public unsafe class KHRExternalSemaphoreCapabilitiesInstanceFunctions {

		[NativeType("void vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceExternalSemaphoreInfo* pExternalSemaphoreInfo, VkExternalSemaphoreProperties* pExternalSemaphoreProperties)")]
		public delegate* unmanaged<IntPtr, in VKPhysicalDeviceExternalSemaphoreInfo, ref VKExternalSemaphoreProperties, void> vkGetPhysicalDeviceExternalSemaphorePropertiesKHR;

	}

	public class KHRExternalSemaphoreCapabilities {

		public const string ExtensionName = "VK_KHR_external_semaphore_capabilities";

	}

	// VK_KHR_get_memory_requirements2

	public unsafe class KHRGetMemoryRequirements2DeviceFunctions {

		[NativeType("void vkGetBufferMemoryRequirements2KHR(VkDevice device, const VkBufferMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<IntPtr, in VKBufferMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetBufferMemoryRequirements2KHR;
		[NativeType("void vkGetImageMemoryRequirements2KHR(VkDevice device, const VkImageMemoryRequirementsInfo2* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<IntPtr, in VKImageMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetImageMemoryRequirements2KHR;
		[NativeType("void vkGetImageSparseMemoryRequirements2KHR(VkDevice device, const VkImageSparseMemoryRequirementsInfo2* pInfo, uint32_t* pSparseRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements)")]
		public delegate* unmanaged<IntPtr, in VKImageSparseMemoryRequirementsInfo2, ref uint, VKSparseImageMemoryRequirements2*, void> vkGetImageSparseMemoryRequirements2KHR;

	}

	public static class KHRGetMemoryRequirements2 {

		public const string ExtensionName = "VK_KHR_get_memory_requirements2";

	}

	// VK_KHR_get_physical_device_properties2 [I]

	public unsafe class KHRGetPhysicalDeviceProperties2InstanceFunctions {

		[NativeType("void vkGetPhysicalDeviceFeatures2KHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceFeatures2* pFeatures)")]
		public delegate* unmanaged<IntPtr, ref VKPhysicalDeviceFeatures2, void> vkGetPhysicalDeviceFeatures2KHR;
		[NativeType("void vkGetPhysicalDeviceProperties2KHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceProperties2* pProperties)")]
		public delegate* unmanaged<IntPtr, ref VKPhysicalDeviceProperties2, void> vkGetPhysicalDeviceProperties2KHR;
		[NativeType("void vkGetPhysicalDeviceFormatProperties2KHR(VkPhysicalDevice physicalDevice, VkFormat format, VkFormatProperties2* pProperties)")]
		public delegate* unmanaged<IntPtr, VKFormat, ref VKFormatProperties2, void> vkGetPhysicalDeviceFormatProperties2KHR;
		[NativeType("Vkresult vkGetPhysicalDeviceImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceImageFormatInfo2* pFormatInfo, VkImageFormatProperties2* pProperties)")]
		public delegate* unmanaged<IntPtr, in VKPhysicalDeviceImageFormatInfo2, ref VKImageFormatProperties2, VKResult> vkGetPhysicalDeviceImageFormatProperties2KHR;
		[NativeType("void vkGetPhysicalDeviceQueueFamilyProperties2KHR(VkPhysicalDevice, uint32_t* pQueueFamilyCount, VkQueueFamilyProperties2* pQueueFamilyProperties)")]
		public delegate* unmanaged<IntPtr, ref uint, VKQueueFamilyProperties2*, void> vkGetPhysicalDeviceQueueFamilyProperties2KHR;
		[NativeType("void vkGetPhysicalDeviceMemoryProperties2KHR(VkPhysicalDevice physicalDevice, VkPhysicalDeviceMemoryProperties2* pMemoryProperties)")]
		public delegate* unmanaged<IntPtr, ref VKPhysicalDeviceMemoryProperties2, void> vkGetPhysicalDeviceMemoryProperties2KHR;
		[NativeType("void vkGetPhysicalDeviceSparseImageFormatProperties2KHR(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceSparseImageFormatInfo2* pFormatInfo, uint32_t* pPropertyCount, VkSparseImageFormatProperties2* pProperties)")]
		public delegate* unmanaged<IntPtr, in VKPhysicalDeviceSparseImageFormatInfo2, ref uint, VKSparseImageFormatProperties2*, void> vkGetPhysicalDeviceSparseImageFormatProperties2KHR;

	}

	public static class KHRGetPhysicalDeviceProperties2 {

		public const string ExtensionName = "VK_KHR_get_physical_device_properties2";

	}

	// VK_KHR_maintenance1

	public unsafe class KHRMaintenance1DeviceFunctions {

		[NativeType("void vkTrimCommandPoolKHR(VkDevice device, VkCommandPool commandPool VkCommandPoolTrimFlags flags)")]
		public delegate* unmanaged<IntPtr, ulong, VKCommandPoolTrimFlags, void> vkTrimCommandPoolKHR;

		public static implicit operator bool(KHRMaintenance1DeviceFunctions fn) => fn != null;

	}

	public static class KHRMaintenance1 {

		public const string ExtensionName = "VK_KHR_maintenance1";

	}

	// VK_KHR_maintenance2

	public static class KHRMaintenance2 {

		public const string ExtensionName = "VK_KHR_maintenance2";

	}

	// VK_KHR_maintenance3

	public unsafe class KHRMaintenance3DeviceFunctions {

		[NativeType("void vkGetDescriptorSetLayoutSupportKHR(VkDevice device, const VkDescriptorSetLayoutCreateInfo* pCreateInfo, VkDescriptorSetLayoutSupport* pSupport)")]
		public delegate* unmanaged<IntPtr, in VKDescriptorSetLayoutCreateInfo, ref VKDescriptorSetLayoutSupport, void> vkGetDescriptorSetLayoutSupportKHR;

		public static implicit operator bool(KHRMaintenance3DeviceFunctions fn) => fn != null;

	}

	public static class KHRMaintenance3 {

		public const string ExtensionName = "VK_KHR_maintenance3";

	}

	// VK_KHR_multiview

	public static class KHRMultiview {

		public const string ExtensionName = "VK_KHR_multiview";

	}

	// VK_KHR_relaxed_block_layout

	public static class KHRRelaxedBlockLayout {

		public const string ExtensionName = "VK_KHR_relaxed_block_layout";

	}

	// VK_KHR_sampler_ycbcr_conversion

	public unsafe class KHRSamplerYcbcrConversionDeviceFunctions {

		[NativeType("VkResult vkCreateSamplerYcbcrConversionKHR(VkDevice device, const VkSamplerYcbcrConversionCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkSamplerYcbcrConversion* pYcbcrConversion)")]
		public delegate* unmanaged<IntPtr, in VKSamplerYcbcrConversionCreateInfo, VKAllocationCallbacks*, out ulong, VKResult> vkCreateSamplerYcbcrConversionKHR;
		[NativeType("void vkDestroySamplerYcbcrConversionKHR(VkDevice device, VkSamplerYcbcrConversion, VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroySamplerYcbcrConversionKHR;

	}

	public static class KHRSamplerYcbcrConversion {

		public const string ExtensionName = "VK_KHR_sampler_ycbcr_conversion";

	}

	// VK_KHR_shader_draw_parameters

	public static class KHRShaderDrawParameters {

		public const string ExtensionName = "VK_KHR_shader_draw_parameters";

	}

	// VK_KHR_storage_buffer_storage_class

	public static class KHRStorageBufferStorageClass {

		public const string ExtensionName = "VK_KHR_storage_buffer_storage_class";

	}

	// VK_KHR_variable_pointers

	public static class KHRVariablePointers {

		public const string ExtensionName = "VK_KHR_variable_pointers";

	}

}
