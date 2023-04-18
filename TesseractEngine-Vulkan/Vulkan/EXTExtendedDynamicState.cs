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

		public VKBool32 ExtendedDynamicState;

	}

	public unsafe class EXTExtendedDynamicStateDeviceFunctions {

		[NativeType("void vkCmdSetCullModeEXT(VkCommandBuffer cmdbuf, VkCullModeFlags cullMode)")]
		public delegate* unmanaged<IntPtr, VKCullModeFlagBits, void> vkCmdSetCullModeEXT;
		[NativeType("void vkCmdSetFrontFaceEXT(VkCommandBuffer cmdbuf, VkFrontFace frontFace)")]
		public delegate* unmanaged<IntPtr, VKFrontFace, void> vkCmdSetFrontFaceEXT;
		[NativeType("void vkCmdSetPrimitiveTopologyEXT(VkCommandBuffer cmdbuf, VkPrimitiveTopology primitiveTopology)")]
		public delegate* unmanaged<IntPtr, VKPrimitiveTopology, void> vkCmdSetPrimitiveTopologyEXT;
		[NativeType("void vkCmdSetViewportWithCountEXT(VkCommandBuffer cmdbuf, uint32_t viewportCount, const VkViewport* pViewports)")]
		public delegate* unmanaged<IntPtr, uint, VKViewport*, void> vkCmdSetViewportWithCountEXT;
		[NativeType("void vkCmdSetScissorWithCountEXT(VkCommandBuffer cmdbuf, uint32_t scissorCount, const VkRect2D* pScissors)")]
		public delegate* unmanaged<IntPtr, uint, VKRect2D*, void> vkCmdSetScissorWithCountEXT;
		[NativeType("void vkCmdBindVertexBuffers2EXT(VkCommandBuffer cmdbuf, uint32_t firstBinding, uint32_t bindingCount, const VkBuffer* pBuffers, const VkDeviceSize* pOffsets, const VkDeviceSize* pSizes, const VkDeviceSize* pStrides")]
		public delegate* unmanaged<IntPtr, uint, uint, ulong*, ulong*, ulong*, ulong*, void> vkCmdBindVertexBuffers2EXT;
		[NativeType("void vkCmdSetDepthTestEnableEXT(VkCommandBuffer cmdbuf, VkBool32 depthTestEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetDepthTestEnableEXT;
		[NativeType("void vkCmdSetDepthWriteEnableEXT(VkCommandBuffer cmdbuf, VkBool32 depthWriteEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetDepthWriteEnableEXT;
		[NativeType("void vkCmdSetDepthCompareOpEXT(VkCommandBuffer cmdbuf, VkCompareOp depthCompareOp)")]
		public delegate* unmanaged<IntPtr, VKCompareOp, void> vkCmdSetDepthCompareOpEXT;
		[NativeType("void vkCmdSetDepthBoundsTestEnableEXT(VkCommandBuffer cmdbuf, VkBool32 depthBoundsTestEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetDepthBoundsTestEnableEXT;
		[NativeType("void vkCmdSetStencilTestEnableEXT(VkCommandBuffer cmdbuf, VkBool32 stencilTestEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetStencilTestEnableEXT;
		[NativeType("void vkCmdSetStencilOpEXT(VkCommandBuffer cmdbuf, VkStencilFaceFlags faceMask, VkStencilOp failOp, VkStencilOp passOp, VkStencilOp depthFailOp, VkComareOp compareOp)")]
		public delegate* unmanaged<IntPtr, VKStencilFaceFlagBits, VKStencilOp, VKStencilOp, VKStencilOp, VKCompareOp> vkCmdSetStencilOpEXT;

		public static implicit operator bool(EXTExtendedDynamicStateDeviceFunctions fn) => fn != null;

	}

	public static class EXTExtendedDynamicState {

		public const string ExtensionName = "VK_EXT_extended_dynamic_state";

	}
}
