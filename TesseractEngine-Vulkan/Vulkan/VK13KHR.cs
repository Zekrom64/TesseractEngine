using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	using VkCommandBuffer = IntPtr;
	using VkDevice = IntPtr;
	using VkQueue = IntPtr;

	// VK_KHR_dynamic_rendering

	public static class KHRDynamicRendering {

		public const string ExtensionName = "VK_KHR_dynamic_rendering";

	}

	public unsafe class KHRDynamicRenderingDeviceFunctions {

		[NativeType("void vkCmdBeginRenderingKHR(VkCommandBuffer commandBuffer, const VkRenderingInfoKHR* pRenderingInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKRenderingInfo, void> vkCmdBeginRenderingKHR;
		[NativeType("void vkCmdEndRenderingKHR(VkCommandBuffer commandBuffer)")]
		public delegate* unmanaged<VkCommandBuffer, void> vkCmdEndRenderingKHR;

	}

	public static class KHRFormatFeatureFlags2 {

		public const string ExtensionName = "VK_KHR_format_feature_flags2";

	}

	// VK_KHR_copy_commands2

	public static class KHRCopyCommands2 {

		public const string ExtensionName = "VK_KHR_copy_commands2";

	}

	public unsafe class KHRCopyCommands2Functions {

		[NativeType("void vkCmdBlitImage2KHR(VkCommandBuffer commandBuffer, const VkBlitImageInfo2* pBlitImageInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKBlitImageInfo2, void> vkCmdBlitImage2KHR;
		[NativeType("void vkCmdCopyBuffer2KHR(VkCommandBuffer commandBuffer, const VkCopyBufferInfo2* pCopyBufferInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKCopyBufferInfo2, void> vkCmdCopyBuffer2KHR;
		[NativeType("void vkCmdCopyBufferToImage2KHR(VkCommandBuffer commandBuffer, const VkCopyBufferToImage2* pCopyBufferToImageInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKCopyBufferToImageInfo2, void> vkCmdCopyBufferToImage2KHR;
		[NativeType("void vkCmdCopyImage2KHR(VkCommandBuffer commandBuffer, const VkCopyImageInfo* pCopyImageInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKCopyImageInfo2, void> vkCmdCopyImage2KHR;
		[NativeType("void vkCmdCopyImageToBuffer2KHR(VkCommandBuffer commandBuffer, const VkCopyImageToBufferInfo2* pCopyImageToBufferInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKCopyImageToBufferInfo2, void> vkCmdCopyImageToBuffer2KHR;
		[NativeType("void vkCmdResolveImage2KHR(VkCommandBuffer commandBuffer, const VkResolveImageInfo2* pResolveImageInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKResolveImageInfo2, void> vkCmdResolveImage2;

	}

	// VK_KHR_maintenance4

	public static class KHRMaintenance4 {

		public const string ExtensionName = "VK_KHR_maintenance4";

	}

	public unsafe class KHRMaintenance4Functions {

		[NativeType("void vkGetDeviceBufferMemoryRequirementsKHR(VkDevice device, const VkDeviceBufferMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKDeviceBufferMemoryRequirements, ref VKMemoryRequirements2, void> vkGetDeviceBufferMemoryRequirementsKHR;
		[NativeType("void vkGetDeviceImageMemoryRequirementsKHR(VkDevice device, const VkDeviceImageMemoryRequirements* pInfo, VkMemoryRequirements2* pMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKDeviceImageMemoryRequirements, ref VKMemoryRequirements2, void> vkGetDeviceImageMemoryRequirementsKHR;
		[NativeType("void vkGetDeviceImageSparseMemoryRequirementsKHR(VkDevice device, const VkDeviceImageMemoryRequirements* pInfo, uint32_t* pSparseMemoryRequirementCount, VkSparseImageMemoryRequirements2* pSparseMemoryRequirements)")]
		public delegate* unmanaged<VkDevice, in VKDeviceImageMemoryRequirements, ref uint, VKSparseImageMemoryRequirements2*, void>  vkGetDeviceImageSparseMemoryRequirementsKHR;

	}

	// VK_KHR_shader_integer_dot_product

	public static class KHRShaderIntegerDotProduct {

		public const string ExtensionName = "VK_KHR_shader_integer_dot_product";

	}

	// VK_KHR_shader_non_semantic_info

	public static class KHRShaderNonSemanticInfo {

		public const string ExtensionName = "VK_KHR_shader_non_semantic_info";

	}

	// VK_KHR_shader_terminate_invocation

	public static class KHRShaderTerminateInvocation {

		public const string ExtensionName = "VK_KHR_shader_terminate_invocation";

	}

	// VK_KHR_synchronization2

	public static class KHRSynchronization2 {

		public const string ExtensionName = "VK_KHR_synchronization2";

	}

	public unsafe class KHRSynchronization2Functions {

		[NativeType("void vkCmdPipelineBarrier2(VkCommandBuffer commandBuffer, const VkDependencyInfo* pDependencyInfo)")]
		public delegate* unmanaged<VkCommandBuffer, in VKDependencyInfo, void> vkCmdPipelineBarrier2;
		[NativeType("void vkCmdResetEvent2(VkCommandBuffer commandBuffer, VkEvent event, VkPipelineStageFlags2 stageMask)")]
		public delegate* unmanaged<VkCommandBuffer, ulong, VKPipelineStageFlagBits2, void> vkCmdResetEvent2;
		[NativeType("void vkCmdSetEvent2(VkCommandBuffer commandBuffer, VkEvent event, const VkDepepdencyInfo* pDependencyInfo)")]
		public delegate* unmanaged<VkCommandBuffer, ulong, in VKDependencyInfo, void> vkCmdSetEvent2;
		[NativeType("void vkCmdWaitEvents2(VkCommandBuffer commandBufer, uint32_t eventCount, const VkEvent* pEvents, const VkDependencyInfo* pDependencyInfos)")]
		public delegate* unmanaged<VkCommandBuffer, uint, ulong*, VKDependencyInfo*, void> vkCmdWaitEvents2;
		[NativeType("void vkCmdWriteTimestamp2(VkCommandBuffer commandBuffer, VkPipelineStageFlags stage, VkQueryPool queryPool, uint32_t query)")]
		public delegate* unmanaged<VkCommandBuffer, VKPipelineStageFlagBits2, ulong, uint, void> vkCmdWriteTimestamp2;
		[NativeType("VkResult vkQueueSubmit2(VkQueue queue, uint32_t submitCount, const VkSubmitInfo2* pSubmits, VkFence fence)")]
		public delegate* unmanaged<VkQueue, uint, VKSubmitInfo2*, ulong, VKResult> vkQueueSubmit2;

	}
	
	// VK_KHR_zero_initialize_workgroup_memory

	public static class KHRZeroInitializeWorkgroupMemory {

		public const string ExtensionName = "VK_KHR_zero_initialize_workgroup_memory";

	}

}
