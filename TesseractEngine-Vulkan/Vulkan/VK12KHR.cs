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

	public unsafe class KHRBufferDeviceAddressDeviceFunctions {

		[NativeType("VkDeviceSize vkGetBufferDeviceAddressKHR(VkDevice device, const VkBufferDeviceAddressInfo* pInfo)")]
		public delegate* unmanaged<IntPtr, in VKBufferDeviceAddressInfo, ulong> vkGetBufferDeviceAddressKHR;
		[NativeType("uint64_t vkGetBufferOpaqueCaptureAddressKHR(VkDevice device, const VkBufferDeviceAddressInfo* pInfo)")]
		public delegate* unmanaged<IntPtr, in VKBufferDeviceAddressInfo, ulong> vkGetBufferOpaqueCaptureAddressKHR;
		[NativeType("uint64_t vkGetDeviceMemoryOpaqueCaptureAddressKHR(VkDevice device, const VKDeviceMemoryOpaqueCaptureAddressInfo* pInfo)")]
		public delegate* unmanaged<IntPtr, in VKDeviceMemoryOpaqueCaptureAddressInfo, ulong> vkGetDeviceMemoryOpaqueCaptureAddressKHR;

	}

	public static class KHRBufferDeviceAddress {

		public const string ExtensionName = "VK_KHR_buffer_device_address";

	}

	// VK_KHR_create_renderpass2

	public unsafe class KHRCreateRenderpass2DeviceFunctions {

		[NativeType("void vkCmdBeginRenderPass2KHR(VkCommandBuffer commandBuffer, const VkRenderPassBeginInfo* pRenderPassBegin, const VkSubpassBeginInfo* pSubpassBeginInfo)")]
		public delegate* unmanaged<IntPtr, in VKRenderPassBeginInfo, in VKSubpassBeginInfo, void> vkCmdBeginRenderPass2KHR;
		[NativeType("void vkCmdEndRenderPass2KHR(VkCommandBuffer commandBuffer, const VkSubpassEndInfo* pSubpassEndInfo)")]
		public delegate* unmanaged<IntPtr, in VKSubpassEndInfo, void> vkCmdEndRenderPass2KHR;
		[NativeType("void vkCmdNextSubpass2KHR(VkCommandBuffer commandBuffer, const VkSubpassBeginInfo* pSubpassBeginInfo, const VkSubpassEndInfo* pSubpassEndInfo)")]
		public delegate* unmanaged<IntPtr, in VKSubpassBeginInfo, in VKSubpassEndInfo, void> vkCmdNextSubpass2KHR;
		[NativeType("VkResult vkCreateRenderPass2KHR(VkDevice device, const VkRenderPassCreateInfo2* pCreateInfo2, const VkAllocationCallbacks* pAllocator, VkRenderPass* pRenderPass)")]
		public delegate* unmanaged<IntPtr, in VKRenderPassCreateInfo2, VKAllocationCallbacks*, out ulong, VKResult> vkCreateRenderPass2KHR;

	}

	public static class KHRCreateRenderpass2 {

		public const string ExtensionName = "VK_KHR_create_renderpass2";

	}

	// VK_KHR_depth_stencil_resolve

	public static class KHRDepthStencilResolve {

		public const string ExtensionName = "VK_KHR_depth_stencil_resolve";

	}

	// VK_KHR_draw_indirect_count

	public unsafe class KHRDrawIndirectCountDeviceFunctions {

		[NativeType("void vkCmdDrawIndexedIndirectCountKHR(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferOffset, uint32_t maxDrawCount, uint32_t stride)")]
		public delegate* unmanaged<IntPtr, ulong, ulong, ulong, ulong, uint, uint, void> vkCmdDrawIndexedIndirectCountKHR;
		[NativeType("void vkCmdDrawIndirectCountKHR(VkCommandBuffer commandBuffer, VkBuffer buffer, VkDeviceSize offset, VkBuffer countBuffer, VkDeviceSize countBufferoffset, uint32_t maxDrawCount, uint32_t stride)")]
		public delegate* unmanaged<IntPtr, ulong, ulong, ulong, ulong, uint, uint, void> vkCmdDrawIndirectCountKHR;

	}

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

	public unsafe class KHRTimelineSemaphoreDeviceFunctions {

		[NativeType("VkResult vkGetSemaphoreCounterValueKHR(VkDevice device, VkSemaphore semaphore, uint64_t* pValue)")]
		public delegate* unmanaged<IntPtr, ulong, out ulong, VKResult> vkGetSemaphoreCounterValueKHR;
		[NativeType("VkResult vkSignalSemaphoreKHR(VkDevice device, const VkSemaphoreSignalInfo* pSignalInfo)")]
		public delegate* unmanaged<IntPtr, in VKSemaphoreSignalInfo, VKResult> vkSignalSemaphoreKHR;
		[NativeType("VkResult vkWaitSemaphoresKHR(VkDevice device, const VkSemaphoreWaitInfo* pWaitInfo, uint64_t timeout)")]
		public delegate* unmanaged<IntPtr, in VKSemaphoreWaitInfo, ulong, VKResult> vkWaitSemaphoresKHR;

	}

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

	public unsafe class EXTHostQueryResetDeviceFunctions {

		[NativeType("void vkResetQueryPoolEXT(VkDevice device, VkQueryPool queryPool, uint32_t firstQuery, uint32_t queryCount)")]
		public delegate* unmanaged<IntPtr, ulong, uint, uint, void> vkResetQueryPoolEXT;

	}

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
