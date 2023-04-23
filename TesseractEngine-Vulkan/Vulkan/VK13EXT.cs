using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	using VkPrivateDataSlotEXT = UInt64;

	// VK_EXT_4444_formats

	public static class EXT4444Formats {

		public const string ExtensionName = "VK_EXT_4444_formats";

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevice4444FormatsFeaturesEXT {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 FormatA4R4G4B4;
		public VKBool32 FormatA4B4G4R4;

	}

	// VK_EXT_image_robustness

	public static class EXTImageRobustness {

		public const string ExtensionName = "VK_EXT_image_robustness";

	}

	// VK_EXT_inline_uniform_block

	public static class EXTInlineUniformBlock {

		public const string ExtensionName = "VK_EXT_inline_uniform_block";

	}

	// VK_EXT_pipeline_creation_cache_control

	public static class EXTPipelineCreationCacheControl {

		public const string ExtensionName = "VK_EXT_pipeline_creation_cache_control";

	}

	// VK_EXT_pipeline_creation_feedback

	public static class EXTPipelineCreationFeedback {

		public const string ExtensionName = "VK_EXT_pipeline_creation_feedback";

	}

	// VK_EXT_private_data

	public static class EXTPrivateData {

		public const string ExtensionName = "VK_EXT_private_data";

	}

	public unsafe class EXTPrivateDataFunctions {

		[NativeType("VkResult vkCreatePrivateDataSlotEXT(VkDevice device, const VkPrivateDataSlotCreateInfo* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkPrivateDataSlotEXT* pPrivateDataSlot)")]
		public delegate* unmanaged<IntPtr, in VKPrivateDataSlotCreateInfo, VKAllocationCallbacks*, out VkPrivateDataSlotEXT, VKResult> vkCreatePrivateDataSlotEXT;
		[NativeType("void vkDestroyPrivateDataSlotEXT(VkDevice device, VkPrivateDataSlotEXT privateDataSlot, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, VkPrivateDataSlotEXT, VKAllocationCallbacks*, void> vkDestroyPrivateDataSlotEXT;
		[NativeType("void vkGetPrivateDataEXT(VkDevice device, VkObjectType objectType, uint64_t objectHandle, VkPrivateDataSlotEXT privateDataSlot, uint64_t* pData)")]
		public delegate* unmanaged<IntPtr, VKObjectType, ulong, VkPrivateDataSlotEXT, out ulong, void> vkGetPrivateDataEXT;
		[NativeType("void vkSetPrivateDataEXT(VkDevice device, VkObjectType objectType, uint64_t objectHandle, VkPrivateDataSlotEXT privateDataSlot, uint64_t data)")]
		public delegate* unmanaged<IntPtr, VKObjectType, ulong, VkPrivateDataSlotEXT, ulong, void> vkSetPrivateDataEXT;

	}

	// VK_EXT_shader_demote_to_helper_invocation

	public static class EXTShaderDemoteToHelperInvocation {

		public const string ExtensionName = "VK_EXT_shader_demote_to_helper_invocation";

	}

	// VK_EXT_subgroup_size_control

	public static class EXTSubgroupSizeControl {

		public const string ExtensionName = "VK_EXT_subgroup_size_control";

	}

	// VK_EXT_texel_buffer_alignment

	public static class EXTTexelBufferAlignment {

		public const string ExtensionName = "VK_EXT_texel_buffer_alignment";

	}

	// VK_EXT_texture_compression_astc_hdr

	public static class EXTTextureCompressionASTCHDR {

		public const string ExtensionName = "VK_EXT_texture_compression_astc_hdr";

	}

	// VK_EXT_tooling_info

	public static class EXTToolingInfo {

		public const string ExtensionName = "VK_EXT_tooling_info";

	}

	public unsafe class EXTToolingInfoFunctions {

		[NativeType("VkResult vkGetPhysicalDeviceToolPropertiesEXT(VkPhysicalDevice physicalDevice, uint32_t* pToolCount, VkPhysicalDeviceToolProperties* pToolProperties)")]
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
		// C# thinks VKPhysicalDeviceToolProperties is a managed type because it has string properties, but these are just getters and *should* be blittable
		public delegate* unmanaged<IntPtr, ref uint, VKPhysicalDeviceToolProperties*, VKResult> vkGetPhysicalDeviceToolPropertiesEXT;
#pragma warning restore CS8500

	}

	// VK_EXT_ycbcr_2plane_444_formats

	public static class EXTYcbcr2Plane444Formats {

		public const string ExtensionName = "VK_EXT_ycbcr_2plane_444_formats";

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceYcbcr2Plane444FormatsFeaturesEXT {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 Ycbcr2Plane444Formats;

	}

}
