using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	// VK_KHR_dynamic_rendering

	public static class KHRDynamicRendering {

		public const string ExtensionName = "VK_KHR_dynamic_rendering";

	}

#nullable disable
	public class KHRDynamicRenderingDeviceFunctions {

		public delegate void PFN_vkCmdBeginRenderingKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKRenderingInfo renderingInfo);
		public delegate void PFN_vkCmdEndRenderingKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer);

		public PFN_vkCmdBeginRenderingKHR vkCmdBeginRenderingKHR;
		public PFN_vkCmdEndRenderingKHR vkCmdEndRenderingKHR;

	}
#nullable restore

	public static class KHRFormatFeatureFlags2 {

		public const string ExtensionName = "VK_KHR_format_feature_flags2";

	}

	// VK_KHR_copy_commands2

	public static class KHRCopyCommands2 {

		public const string ExtensionName = "VK_KHR_copy_commands2";

	}

#nullable disable
	public class KHRCopyCommands2Functions {

		public delegate void PFN_vkCmdBlitImage2KHR(IntPtr commandBuffer, in VKBlitImageInfo2 blitImageInfo);
		public delegate void PFN_vkCmdCopyBuffer2KHR(IntPtr commandBuffer, in VKCopyBufferInfo2 copyBufferInfo);
		public delegate void PFN_vkCmdCopyBufferToImage2KHR(IntPtr commandBuffer, in VKCopyBufferToImageInfo2 copyBufferToImageInfo);
		public delegate void PFN_vkCmdCopyImage2KHR(IntPtr commandBuffer, in VKCopyImageInfo2 copyImageInfo);
		public delegate void PFN_vkCmdCopyImageToBuffer2KHR(IntPtr commandBuffer, in VKCopyImageToBufferInfo2 copyImageToBufferInfo);
		public delegate void PFN_vkCmdResolveImage2KHR(IntPtr commandBuffer, in VKResolveImageInfo2 resolveImageInfo);

		public PFN_vkCmdBlitImage2KHR vkCmdBlitImage2KHR;
		public PFN_vkCmdCopyBuffer2KHR vkCmdCopyBuffer2KHR;
		public PFN_vkCmdCopyBufferToImage2KHR vkCmdCopyBufferToImage2KHR;
		public PFN_vkCmdCopyImage2KHR vkCmdCopyImage2KHR;
		public PFN_vkCmdCopyImageToBuffer2KHR vkCmdCopyImageToBuffer2KHR;
		public PFN_vkCmdResolveImage2KHR vkCmdResolveImage2;

	}
#nullable restore

	// VK_KHR_maintenance4

	public static class KHRMaintenance4 {

		public const string ExtensionName = "VK_KHR_maintenance4";

	}

#nullable disable
	public class KHRMaintenance4Functions {

		public delegate void PFN_vkGetDeviceBufferMemoryRequirementsKHR(IntPtr device, in VKDeviceBufferMemoryRequirements info, ref VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetDeviceImageMemoryRequirementsKHR(IntPtr device, in VKDeviceImageMemoryRequirements info, ref VKMemoryRequirements2 memoryRequirements);
		public delegate void PFN_vkGetDeviceImageSparseMemoryRequirementsKHR(IntPtr device, in VKDeviceImageMemoryRequirements info, ref uint sparseMemoryRequirementCount, [NativeType("VkSparseImageMemoryRequirements2*")] IntPtr pSparseMemoryRequirements);

		public PFN_vkGetDeviceBufferMemoryRequirementsKHR vkGetDeviceBufferMemoryRequirementsKHR;
		public PFN_vkGetDeviceImageMemoryRequirementsKHR vkGetDeviceImageMemoryRequirementsKHR;
		public PFN_vkGetDeviceImageSparseMemoryRequirementsKHR vkGetDeviceImageSparseMemoryRequirementsKHR;

	}
#nullable restore

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

#nullable disable
	public class KHRSynchronization2Functions {

		public delegate void PFN_vkCmdPipelineBarrier2(IntPtr commandBuffer, in VKDependencyInfo dependencyInfo);
		public delegate void PFN_vkCmdResetEvent2(IntPtr commandBuffer, ulong _event, VKPipelineStageFlagBits2 stageMask);
		public delegate void PFN_vkCmdSetEvent2(IntPtr commandBuffer, ulong _event, in VKDependencyInfo dependencyInfo);
		public delegate void PFN_vkCmdWaitEvents2(IntPtr commandBuffer, uint eventCount, [NativeType("const VkEvent*")] IntPtr pEvents, [NativeType("const VkDependencyInfo*")] IntPtr pDependencyInfos);
		public delegate void PFN_vkCmdWriteTimestamp2(IntPtr commandBuffer, VKPipelineStageFlagBits2 stage, ulong queryPool, uint query);
		public delegate VKResult PFN_vkQueueSubmit2(IntPtr queue, uint submitCount, [NativeType("const VkSubmitInfo2*")] IntPtr pSubmits, ulong fence);

		public PFN_vkCmdPipelineBarrier2 vkCmdPipelineBarrier2;
		public PFN_vkCmdResetEvent2 vkCmdResetEvent2;
		public PFN_vkCmdSetEvent2 vkCmdSetEvent2;
		public PFN_vkCmdWaitEvents2 vkCmdWaitEvents2;
		public PFN_vkCmdWriteTimestamp2 vkCmdWriteTimestamp2;
		public PFN_vkQueueSubmit2 vkQueueSubmit2;

	}
#nullable restore
	
	// VK_KHR_zero_initialize_workgroup_memory

	public static class KHRZeroInitializeWorkgroupMemory {

		public const string ExtensionName = "VK_KHR_zero_initialize_workgroup_memory";

	}

}
