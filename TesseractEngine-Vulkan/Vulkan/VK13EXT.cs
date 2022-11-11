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

#nullable disable
	public class EXTPrivateDataFunctions {

		public delegate VKResult PFN_vkCreatePrivateDataSlotEXT([NativeType("VkDevice")] IntPtr device, in VKPrivateDataSlotCreateInfo createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, out VkPrivateDataSlotEXT privateDataSlot);
		public delegate void PFN_vkDestroyPrivateDataSlotEXT([NativeType("VkDevice")] IntPtr device, VkPrivateDataSlotEXT privateDataSlot, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator);
		public delegate void PFN_vkGetPrivateDataEXT([NativeType("VkDevice")] IntPtr device, VKObjectType objectType, ulong objectHandle, VkPrivateDataSlotEXT privateDataSlot, out ulong data);
		public delegate void PFN_vkSetPrivateDataEXT([NativeType("VkDevice")] IntPtr device, VKObjectType objectType, ulong objectHandle, VkPrivateDataSlotEXT privateDataSlot, ulong data);

		public PFN_vkCreatePrivateDataSlotEXT vkCreatePrivateDataSlotEXT;
		public PFN_vkDestroyPrivateDataSlotEXT vkDestroyPrivateDataSlotEXT;
		public PFN_vkGetPrivateDataEXT vkGetPrivateDataEXT;
		public PFN_vkSetPrivateDataEXT vkSetPrivateDataEXT;

	}
#nullable restore

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

#nullable disable
	public class EXTToolingInfoFunctions {

		public delegate VKResult PFN_vkGetPhysicalDeviceToolPropertiesEXT([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, ref uint toolCount, [NativeType("VkPhysicalDeviceToolProperties")] IntPtr pToolProperties);

		public PFN_vkGetPhysicalDeviceToolPropertiesEXT vkGetPhysicalDeviceToolPropertiesEXT;

	}
#nullable restore

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
