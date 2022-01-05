using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	// VK_KHR_8bit_storage

	public static class KHR8BitStorage {

		public const string ExtensionName = "VK_KHR_8bit_storage";

	}

	// VK_KHR_buffer_device_address

#nullable disable
	public class KHRBufferDeviceAddressDeviceFunctions {

		[return: NativeType("VkDeviceAddress")]
		public delegate ulong PFN_vkGetBufferDeviceAddressKHR([NativeType("VkDevice")] IntPtr device, in VKBufferDeviceAddressInfo info);
		public delegate ulong PFN_vkGetBufferOpaqueCaptureAddressKHR([NativeType("VkDevice")] IntPtr device, in VKBufferDeviceAddressInfo info);
		public delegate ulong PFN_vkGetDeviceMemoryOpaqueCaptureAddressKHR([NativeType("VkDevice")] IntPtr device, in VKDeviceMemoryOpaqueCaptureAddressInfo info);

		public PFN_vkGetBufferDeviceAddressKHR vkGetBufferDeviceAddressKHR;
		public PFN_vkGetBufferOpaqueCaptureAddressKHR vkGetBufferOpaqueCaptureAddressKHR;
		public PFN_vkGetDeviceMemoryOpaqueCaptureAddressKHR vkGetDeviceMemoryOpaqueCaptureAddressKHR;

	}
#nullable restore

	public static class KHRBufferDeviceAddress {

		public const string ExtensionName = "VK_KHR_buffer_device_address";

	}

	// VK_KHR_create_renderpass2

#nullable disable
	public class KHRCreateRenderpass2DeviceFunctions {

		public delegate void PFN_vkCmdBeginRenderPass2KHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKRenderPassBeginInfo renderPassBegin, in VKSubpassBeginInfo subpassBeginInfo);
		public delegate void PFN_vkCmdEndRenderPass2KHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKSubpassEndInfo subpassEndInfo);
		public delegate void PFN_vkCmdNextSubpass2KHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKSubpassBeginInfo subpassBeginInfo, in VKSubpassEndInfo subpassEndInfo);
		public delegate VKResult PFN_vkCreateRenderPass2KHR([NativeType("VkDevice")] IntPtr device, in VKRenderPassCreateInfo2 createInfo2, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkRenderPass")] out ulong renderPass);

		public PFN_vkCmdBeginRenderPass2KHR vkCmdBeginRenderPass2KHR;
		public PFN_vkCmdEndRenderPass2KHR vkCmdEndRenderPass2KHR;
		public PFN_vkCmdNextSubpass2KHR vkCmdNextSubpass2KHR;
		public PFN_vkCreateRenderPass2KHR vkCreateRenderPass2KHR;

	}
#nullable restore

	public static class KHRCreateRenderpass2 {

		public const string ExtensionName = "VK_KHR_create_renderpass2";

	}

	// VK_KHR_depth_stencil_resolve

	public static class KHRDepthStencilResolve {

		public const string ExtensionName = "VK_KHR_depth_stencil_resolve";

	}

	// VK_KHR_draw_indirect_count

#nullable disable
	public class KHRDrawIndirectCountDeviceFunctions {

		public delegate void PFN_vkCmdDrawIndexedIndirectCountKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, [NativeType("VkBuffer")] ulong buffer, ulong offset, [NativeType("VkBuffer")] ulong countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);
		public delegate void PFN_vkCmdDrawIndirectCountKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, [NativeType("VkBuffer")] ulong buffer, ulong offset, [NativeType("VkBuffer")] ulong countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride);

		public PFN_vkCmdDrawIndexedIndirectCountKHR vkCmdDrawIndexedIndirectCountKHR;
		public PFN_vkCmdDrawIndirectCountKHR vkCmdDrawIndirectCountKHR;

	}
#nullable restore

	public static class KHRDrawIndirectCount {

		public const string ExtensionName = "VK_KHR_draw_indirect_count";

	}

	// VK_KHR_driver_properties

	public static class KHRDriverProperties {

		public const string ExtensionName = "VK_KHR_driver_properties";

	}

	// VK_KHR_image_format_last

	public static class KHRImageFormatList {

		public const string ExtensionName = "VK_KHR_image_format_list";

	}

	// VK_KHR_imageless_framebuffer

	public static class KHRImagelessFramebuffer {

		public const string ExtensionName = "VK_KHR_imageless_framebuffer";

	}

	// VK_KHR_sampler_mirror_clamp_to_edge

	public static class KHRSamplerMirrorClampToEdge {

		public const string ExtesionName = "VK_KHR_sampler_mirror_clamp_to_edge";

	}

	// VK_KHR_separate_depth_stencil_layouts

	public static class KHRSeparateDepthStencilLayouts {

		public const string ExtensionName = "VK_KHR_separate_depth_stencil_layouts";

	}

	// VK_KHR_shader_atomic_int64

	public static class KHRShaderAtomicInt64 {

		public const string ExtensionName = "VK_KHR_shader_atomic_int64";

	}

	// VK_KHR_shader_float16_int8

	public static class KHRShaderFloat16Int8 {

		public const string ExtensionName = "VK_KHR_shader_float16_int8";

	}

	// VK_KHR_shader_float_controls

	public static class KHRShaderFloatControls {

		public const string ExtensionName = "VK_KHR_shader_float_controls";

	}

	// VK_KHR_shader_subgroup_extended_types

	public static class KHRShaderSubgroupExtendedTypes {

		public const string ExtensionName = "VK_KHR_shader_subgroup_extended_types";

	}

	// VK_KHR_spirv_1_4

	public static class KHRSPIRV14 {

		public const string ExtensionName = "VK_KHR_spirv_1_4";

	}

	// VK_KHR_timeline_semaphore

#nullable disable
	public class KHRTimelineSemaphoreDeviceFunctions {

		public delegate VKResult PFN_vkGetSemaphoreCounterValueKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSemaphore")] ulong semaphore, out ulong value);
		public delegate VKResult PFN_vkSignalSemaphoreKHR([NativeType("VkDevice")] IntPtr device, in VKSemaphoreSignalInfo signalInfo);
		public delegate VKResult PFN_vkWaitSemaphoresKHR([NativeType("VkDevice")] IntPtr device, in VKSemaphoreWaitInfo waitInfo, ulong timeout);

		public PFN_vkGetSemaphoreCounterValueKHR vkGetSemaphoreCounterValueKHR;
		public PFN_vkSignalSemaphoreKHR vkSignalSemaphoreKHR;
		public PFN_vkWaitSemaphoresKHR vkWaitSemaphoresKHR;

	}
#nullable restore

	public static class KHRTimelineSemaphore {

		public const string ExtensionName = "VK_KHR_timeline_semaphore";

	}

	// VK_KHR_uniform_buffer_standard_layout

	public static class KHRUniformBufferStandardLayout {

		public const string ExtensionName = "VK_KHR_uniform_buffer_standard_layout";

	}

	// VK_KHR_vulkan_memory_model

	public static class KHRVulkanMemoryModel {

		public const string ExtensionName = "VK_KHR_vulkan_memory_model";

	}

	// VK_EXT_descriptor_indexing

	public static class EXTDescriptorIndexing {

		public const string ExtensionName = "VK_EXT_descriptor_indexing";

	}

	// VK_EXT_host_query_reset

#nullable disable
	public class EXTHostQueryResetDeviceFunctions {

		public delegate void PFN_vkResetQueryPoolEXT([NativeType("VkDevice")] IntPtr device, [NativeType("VkQueryPool")] ulong queryPool, uint firstQuery, uint queryCount);

		public PFN_vkResetQueryPoolEXT vkResetQueryPoolEXT;

	}
#nullable restore

	public static class EXTHostQueryReset {

		public const string ExtensionName = "VK_EXT_host_query_reset";

	}

	// VK_EXT_sampler_filter_minmax

	public static class EXTSamplerFilterMinmax {

		public const string ExtensionName = "VK_EXT_sampler_filter_minmax";

	}

	// VK_EXT_scalar_block_layout

	public static class EXTScalarBlockLayout {

		public const string ExtensionName = "VK_EXT_scalar_block_layout";

	}

	// VK_EXT_separate_stencil_usage

	public static class EXTSeparateStencilUsage {

		public const string ExtensionName = "VK_EXT_separate_stencil_usage";

	}

	// VK_EXT_shader_viewport_index_layer

	public static class EXTShaderViewportIndexLayer {

		public const string ExtensionName = "VK_EXT_shader_viewport_index_layer";

	}

}
