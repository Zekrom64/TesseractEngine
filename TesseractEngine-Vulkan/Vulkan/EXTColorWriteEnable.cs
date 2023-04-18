using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceColorWriteEnableFeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public VKBool32 ColoWriteEnable;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineColorWriteCreateInfoEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public uint AttachmentCount;

		[NativeType("const VkBool32*")]
		public IntPtr ColorWriteEnables;

	}

	public unsafe class EXTColorWriteEnableDeviceFunctions {

		[NativeType("void vkCmdSetColorWriteEnableEXT(VkCommandBuffer commandBuffer, uint32_t attachmentCount, const VkBool32* pColorWriteEnables)")]
		public delegate* unmanaged<IntPtr, uint, IntPtr, VKBool32*> vkCmdSetColorWriteEnableEXT;

	}

	public class EXTColorWriteEnable {

		public const string ExtensionName = "VK_EXT_color_write_enable";

	}

}
