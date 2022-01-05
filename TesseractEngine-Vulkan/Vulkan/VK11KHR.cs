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

#nullable disable
	public class KHRBindMemory2DeviceFunctions {

		public delegate VKResult PFN_vkBindBufferMemory2KHR([NativeType("VkDevice")] IntPtr device, uint bindInfoCount, [NativeType("const VkBindBufferMemoryInfo*")] IntPtr pBindInfos);
		public delegate VKResult PFN_vkBindImageMemory2KHR([NativeType("VkDevice")] IntPtr device, uint bindInfoCount, [NativeType("const VkBindImageMemoryInfo*")] IntPtr pBindInfos);

		public PFN_vkBindBufferMemory2KHR vkBindBufferMemory2KHR;
		public PFN_vkBindImageMemory2KHR vkBindImageMemory2KHR;

		public static implicit operator bool(KHRBindMemory2DeviceFunctions fn) => fn != null;

	}
#nullable restore

	public static class KHRBindMemory2 {

		public const string ExtensionName = "VK_KHR_bind_memory2";

	}

	// VK_KHR_dedicated_allocation

	public static class KHRDedicatedAllocation {

		public const string ExtensionName = "VK_KHR_dedicated_allocation";

	}

	// VK_KHR_descriptor_update_template

#nullable disable
	public class KHRDescriptorUpdateTemplateDeviceFunctions {

		public delegate VKResult PFN_vkCreateDescriptorUpdateTemplateKHR([NativeType("VkDevice")] IntPtr device, in VKDescriptorUpdateTemplateCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkDescriptorUpdateTemplate*")] out ulong descriptorUpdateTemplate);
		public delegate void PFN_vkDestroyDescriptorUpdateTemplateKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDescriptorUpdateTemplate")] ulong descriptorUpdateTemplate, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkUpdateDescriptorSetWithTemplateKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDescriptorSet")] ulong descriptorSet, [NativeType("VkDescriptorUpdateTemplate")] ulong descriptorUpdateTemplate, IntPtr data);

		public PFN_vkCreateDescriptorUpdateTemplateKHR vkCreateDescriptorUpdateTemplateKHR;
		public PFN_vkDestroyDescriptorUpdateTemplateKHR vkDestroyDescriptorUpdateTemplateKHR;
		public PFN_vkUpdateDescriptorSetWithTemplateKHR vkUpdateDescriptorSetWithTemplateKHR;

		public static implicit operator bool(KHRDescriptorUpdateTemplateDeviceFunctions fn) => fn != null;

	}
#nullable restore

	public static class KHRDescriptorUpdateTemplate {

		public const string ExtensionName = "VK_KHR_descriptor_update_template";

	}

	// VK_KHR_device_group

#nullable disable
	public class KHRDeviceGroupDeviceFunctions {

		public delegate void PFN_vkCmdDispatchBaseKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint baseGroupX, uint baseGroupY, uint baseGroupZ, uint groupCountX, uint groupCcountY, uint groupCountZ);
		public delegate void PFN_vkCmdSetDeviceMaskKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint deviceMask);
		public delegate void PFN_vkGetDeviceGroupPeerMemoryFeaturesKHR([NativeType("VkDevice")] IntPtr device, uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex, out VKPeerMemoryFeatureFlagBits peerMemoryFeatures);

		public PFN_vkCmdDispatchBaseKHR vkCmdDispatchBaseKHR;
		public PFN_vkCmdSetDeviceMaskKHR vkCmdSetDeviceMaskKHR;
		public PFN_vkGetDeviceGroupPeerMemoryFeaturesKHR vkGetDeviceGroupPeerMemoryFeaturesKHR;

		public static implicit operator bool(KHRDeviceGroupDeviceFunctions fn) => fn != null;

	}
#nullable restore

	public static class KHRDeviceGroup {

		public const string ExtensionName = "VK_KHR_device_group";

	}

	// VK_KHR_device_group_creation [I]

#nullable disable
	public class KHRDeviceGroupCreationInstanceFunctions {

		public delegate VKResult PFN_vkEnumeratePhysicalDeviceGroupsKHR([NativeType("VkInstance")] IntPtr instance, ref uint physicalDeviceGroupCount, [NativeType("VkPhysicalDeviceGroupProperties*")] IntPtr pPhysicalDeviceGroupProperties);

		public PFN_vkEnumeratePhysicalDeviceGroupsKHR vkEnumeratePhysicalDeviceGroupsKHR;

	}
#nullable restore

	public static class KHRDeviceGroupCreation {

		public const string ExtensionName = "VK_KHR_device_group_creation";

	}

	// VK_KHR_external_fence

	public static class KHRExternalFence {

		public const string ExtensionName = "VK_KHR_external_fence";

	}

	// VK_KHR_external_fence_capabilities [I]

#nullable disable
	public class KHRExternalFenceCapabilitiesInstanceFunctions {

		public delegate void PFN_vkGetPhysicalDeviceExternalFencePropertiesKHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, in VKPhysicalDeviceExternalFenceInfo externalFenceInfo, ref VKExternalFenceProperties externalFenceProperties);

		public PFN_vkGetPhysicalDeviceExternalFencePropertiesKHR vkGetPhysicalDeviceExternalFencePropertiesKHR;

	}
#nullable restore

	public static class KHRExternalFenceCapabilities {

		public const string ExtensionName = "VK_KHR_external_fence_capabilities";

	}

	// VK_KHR_external_memory

	public static class KHRExternalMemory {

		public const string ExtensionName = "VK_KHR_external_memory";

	}

	// VK_KHR_external_memory_capabilities [I]

#nullable disable
	public class KHRExternalMemoryCapabilitiesInstanceFunctions {

		public delegate void PFN_vkGetPhysicalDeviceExternalBufferPropertiesKHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, in VKPhysicalDeviceExternalBufferInfo externalBufferInfo, ref VKExternalBufferProperties externalBufferProperties);

		public PFN_vkGetPhysicalDeviceExternalBufferPropertiesKHR vkGetPhysicalDeviceExternalBufferPropertiesKHR;

	}
#nullable restore

	public static class KHRExternalMemoryCapabilities {

		public const string ExtensionName = "VK_KHR_external_memory_capabilities";

	}

	// VK_KHR_external_semaphore

	public static class KHRExternalSemaphore {

		public const string ExtensionName = "VK_KHR_external_semaphore";

	}

	// VK_KHR_external_semaphore_capabilities [I]

#nullable disable
	public class KHRExternalSemaphoreCapabilitiesInstanceFunctions {

		public delegate void PFN_vkGetPhysicalDeviceExternalSemaphorePropertiesKHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, in VKPhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, ref VKExternalSemaphoreProperties externalSemaphoreProperties);

		public PFN_vkGetPhysicalDeviceExternalSemaphorePropertiesKHR vkGetPhysicalDeviceExternalSemaphorePropertiesKHR;

	}
#nullable restore

	public class KHRExternalSemaphoreCapabilities {

		public const string ExtensionName = "VK_KHR_external_semaphore_capabilities";

	}

	// VK_KHR_get_memory_requirements2

#nullable disable
	public class KHRGetMemoryRequirements2DeviceFunctions {

		public delegate void PFN_vkGetBufferMemoryRequirements2KHR([NativeType("VkDevice")] IntPtr device, in VKBufferMemoryRequirementsInfo2 info, ref VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetImageMemoryRequirements2KHR([NativeType("VkDevice")] IntPtr device, in VKImageMemoryRequirementsInfo2 info, ref VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetImageSparseMemoryRequirements2KHR([NativeType("VkDevice")] IntPtr device, in VKImageSparseMemoryRequirementsInfo2 info, ref uint sparseRequirementCount, [NativeType("VKSparseImageMemoryRequirements2*")] IntPtr pSparseMemoryRequirements);

		public PFN_vkGetBufferMemoryRequirements2KHR vkGetBufferMemoryRequirements2KHR;
		public PFN_vkGetImageMemoryRequirements2KHR vkGetImageMemoryRequirements2KHR;
		public PFN_vkGetImageSparseMemoryRequirements2KHR vkGetImageSparseMemoryRequirements2KHR;

	}
#nullable restore

	public static class KHRGetMemoryRequirements2 {

		public const string ExtensionName = "VK_KHR_get_memory_requirements2";

	}

	// VK_KHR_get_physical_device_properties2 [I]

#nullable disable
	public class KHRGetPhysicalDeviceProperties2InstanceFunctions {

		public delegate void PFN_vkGetPhysicalDeviceFeatures2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, ref VKPhysicalDeviceFeatures2 features);
		public delegate void PFN_vkGetPhysicalDeviceProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, ref VKPhysicalDeviceProperties2 properties);
		public delegate void PFN_vkGetPhysicalDeviceFormatProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, VKFormat format, ref VKFormatProperties2 properties);
		public delegate VKResult PFN_vkGetPhysicalDeviceImageFormatProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, in VKPhysicalDeviceImageFormatInfo2 formatInfo, ref VKImageFormatProperties2 properties);
		public delegate void PFN_vkGetPhysicalDeviceQueueFamilyProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, ref uint queueFamilyCount, [NativeType("VkQueueFamilyProperties2*")] IntPtr pQueueFamilyProperties);
		public delegate void PFN_vkGetPhysicalDeviceMemoryProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, ref VKPhysicalDeviceMemoryProperties2 memoryProperties);
		public delegate void PFN_vkGetPhysicalDeviceSparseImageFormatProperties2KHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, ref uint propertyCount, [NativeType("VkSparseImageFormatProperties2*")] IntPtr pProperties);

		public PFN_vkGetPhysicalDeviceFeatures2KHR vkGetPhysicalDeviceFeatures2KHR;
		public PFN_vkGetPhysicalDeviceProperties2KHR vkGetPhysicalDeviceProperties2KHR;
		public PFN_vkGetPhysicalDeviceFormatProperties2KHR vkGetPhysicalDeviceFormatProperties2KHR;
		public PFN_vkGetPhysicalDeviceImageFormatProperties2KHR vkGetPhysicalDeviceImageFormatProperties2KHR;
		public PFN_vkGetPhysicalDeviceQueueFamilyProperties2KHR vkGetPhysicalDeviceQueueFamilyProperties2KHR;
		public PFN_vkGetPhysicalDeviceMemoryProperties2KHR vkGetPhysicalDeviceMemoryProperties2KHR;
		public PFN_vkGetPhysicalDeviceSparseImageFormatProperties2KHR vkGetPhysicalDeviceSparseImageFormatProperties2KHR;

	}
#nullable restore

	public static class KHRGetPhysicalDeviceProperties2 {

		public const string ExtensionName = "VK_KHR_get_physical_device_properties2";

	}

	// VK_KHR_maintenance1

#nullable disable
	public class KHRMaintenance1DeviceFunctions {

		public delegate void PFN_vkTrimCommandPoolKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkCommandPool")] ulong commandPool, VKCommandPoolTrimFlags flags);

		public PFN_vkTrimCommandPoolKHR vkTrimCommandPoolKHR;

		public static implicit operator bool(KHRMaintenance1DeviceFunctions fn) => fn != null;

	}
#nullable restore

	public static class KHRMaintenance1 {

		public const string ExtensionName = "VK_KHR_maintenance1";

	}

	// VK_KHR_maintenance2

	public static class KHRMaintenance2 {

		public const string ExtensionName = "VK_KHR_maintenance2";

	}

	// VK_KHR_maintenance3

#nullable disable
	public class KHRMaintenance3DeviceFunctions {

		public delegate void PFN_vkGetDescriptorSetLayoutSupportKHR([NativeType("VkDevice")] IntPtr device, in VKDescriptorSetLayoutCreateInfo createInfo, ref VKDescriptorSetLayoutSupport support);

		public PFN_vkGetDescriptorSetLayoutSupportKHR vkGetDescriptorSetLayoutSupportKHR;

		public static implicit operator bool(KHRMaintenance3DeviceFunctions fn) => fn != null;

	}
#nullable restore

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

#nullable disable
	public class KHRSamplerYcbcrConversionDeviceFunctions {

		public delegate VKResult PFN_vkCreateSamplerYcbcrConversionKHR([NativeType("VkDevice")] IntPtr device, in VKSamplerYcbcrConversionCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkSamplerYcbcrConversion*")] out ulong ycbcrConversion);
		public delegate void PFN_vkDestroySamplerYcbcrConversionKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSamplerYcbcrConversion")] ulong ycbcrConversion, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);

		public PFN_vkCreateSamplerYcbcrConversionKHR vkCreateSamplerYcbcrConversionKHR;
		public PFN_vkDestroySamplerYcbcrConversionKHR vkDestroySamplerYcbcrConversionKHR;

	}
#nullable restore

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
