using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExtendedDynamicStateFeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public bool ExtendedDynamicState;

	}

#nullable disable
	public class EXTExtendedDynamicStateDeviceFunctions {

		public delegate void PFN_vkCmdSetCullModeEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, VKCullModeFlagBits cullMode);
		public delegate void PFN_vkCmdSetFrontFaceEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, VKFrontFace frontFace);
		public delegate void PFN_vkCmdSetPrimitiveTopologyEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, VKPrimitiveTopology primitiveTopology);
		public delegate void PFN_vkCmdSetViewportWithCountEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, uint viewportCount, [NativeType("const VkViewport*")] IntPtr pViewports);
		public delegate void PFN_vkCmdSetScissorWithCountEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, uint scissorCount, [NativeType("const VkRect2D*")] IntPtr pScissors);
		public delegate void PFN_vkCmdBindVertexBuffers2EXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, uint firstBinding, uint bindingCount, [NativeType("const VkBuffer*")] IntPtr pBuffers, [NativeType("const VkDeviceSize*")] IntPtr pOffsets, [NativeType("const VkDeviceSize*")] IntPtr pSizes, [NativeType("const VkDeviceSize*")] IntPtr pStrides);
		public delegate void PFN_vkCmdSetDepthTestEnableEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, bool depthTestEnable);
		public delegate void PFN_vkCmdSetDepthWriteEnableEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, bool depthWriteEnable);
		public delegate void PFN_vkCmdSetDepthCompareOpEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, VKCompareOp depthCompareOp);
		public delegate void PFN_vkCmdSetDepthBoundsTestEnableEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, bool depthBoundsTestEnable);
		public delegate void PFN_vkCmdSetStencilTestEnableEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, bool stencilTestEnable);
		public delegate void PFN_vkCmdSetStencilOpEXT([NativeType("VkCommandBuffer")] IntPtr cmdbuf, VKStencilFaceFlagBits faceMask, VKStencilOp failOp, VKStencilOp passOp, VKStencilOp depthFailOp, VKCompareOp compareOp);

		public PFN_vkCmdSetCullModeEXT vkCmdSetCullModeEXT;
		public PFN_vkCmdSetFrontFaceEXT vkCmdSetFrontFaceEXT;
		public PFN_vkCmdSetPrimitiveTopologyEXT vkCmdSetPrimitiveTopologyEXT;
		public PFN_vkCmdSetViewportWithCountEXT vkCmdSetViewportWithCountEXT;
		public PFN_vkCmdSetScissorWithCountEXT vkCmdSetScissorWithCountEXT;
		public PFN_vkCmdBindVertexBuffers2EXT vkCmdBindVertexBuffers2EXT;
		public PFN_vkCmdSetDepthTestEnableEXT vkCmdSetDepthTestEnableEXT;
		public PFN_vkCmdSetDepthWriteEnableEXT vkCmdSetDepthWriteEnableEXT;
		public PFN_vkCmdSetDepthCompareOpEXT vkCmdSetDepthCompareOpEXT;
		public PFN_vkCmdSetDepthBoundsTestEnableEXT vkCmdSetDepthBoundsTestEnableEXT;
		public PFN_vkCmdSetStencilTestEnableEXT vkCmdSetStencilTestEnableEXT;
		public PFN_vkCmdSetStencilOpEXT vkCmdSetStencilOpEXT;

		public static implicit operator bool(EXTExtendedDynamicStateDeviceFunctions fn) => fn != null;

	}
#nullable restore

	public static class EXTExtendedDynamicState {

		public const string ExtensionName = "VK_EXT_extended_dynamic_state";

	}
}
