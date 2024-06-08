using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExtendedDynamicState2FeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public VKBool32 ExtendedDynamicState2;

		public VKBool32 ExtendedDynamicState2LogicOp;

		public VKBool32 ExtendedDyanmicState2PatchControlPoints;

	}

	public unsafe class EXTExtendedDynamicStateDeviceFunctions2 {

		[NativeType("void vkCmdSetDepthBiasEnableEXT(VkCommandBuffer cmdbuf, VkBool32 depthBiasEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetDepthBiasEnableEXT;
		[NativeType("void vkCmdSetLogicOpEXT(VkCommandBuffer cmdbuf, VkLogicOp logicOp)")]
		public delegate* unmanaged<IntPtr, VKLogicOp, void> vkCmdSetLogicOpEXT;
		[NativeType("void vkCmdSetPatchControlPointsEXT(VkCommandBuffer cmdbuf, uint32_t patchControlPoints)")]
		public delegate* unmanaged<IntPtr, uint, void> vkCmdSetPatchControlPointsEXT;
		[NativeType("void vkCmdSetPrimitiveRestartEnableEXT(VkCommandBuffer cmdbuf, VkBool32 primitiveRestartEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetPrimitiveRestartEnableEXT;
		[NativeType("void vkCmdSetRasterizerDiscardEnableEXT(VkCommandBuffer cmdbuf, VkBool32 rasterizerDiscardEnable)")]
		public delegate* unmanaged<IntPtr, bool, void> vkCmdSetRasterizerDiscardEnableEXT;

		public static implicit operator bool(EXTExtendedDynamicStateDeviceFunctions2? fn) => fn != null;

	}

	public static class EXTExtendedDynamicState2 {

		public const string ExtensionName = "VK_EXT_extended_dynamic_state2";

	}

}
