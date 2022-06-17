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

#nullable disable
	public class EXTExtendedDynamicStateDeviceFunctions2 {

		public delegate void PFN_vkCmdSetDepthBiasEnableEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, bool depthBiasEnable);
		public delegate void PFN_vkCmdSetLogicOpEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, VKLogicOp logicOp);
		public delegate void PFN_vkCmdSetPatchControlPointsEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint patchControlPoints);
		public delegate void PFN_vkCmdSetPrimitiveRestartEnableEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, bool primitiveRestartEnable);
		public delegate void PFN_vkCmdSetRasterizerDiscardEnableEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, bool rasterizerDiscardEnable);

		public PFN_vkCmdSetDepthBiasEnableEXT vkCmdSetDepthBiasEnableEXT;
		public PFN_vkCmdSetLogicOpEXT vkCmdSetLogicOpEXT;
		public PFN_vkCmdSetPatchControlPointsEXT vkCmdSetPatchControlPointsEXT;
		public PFN_vkCmdSetPrimitiveRestartEnableEXT vkCmdSetPrimitiveRestartEnableEXT;
		public PFN_vkCmdSetRasterizerDiscardEnableEXT vkCmdSetRasterizerDiscardEnableEXT;

		public static implicit operator bool(EXTExtendedDynamicStateDeviceFunctions2 fn) => fn != null;

	}
#nullable restore

	public static class EXTExtendedDynamicState2 {

		public const string ExtensionName = "VK_EXT_extended_dynamic_state2";

	}

}
