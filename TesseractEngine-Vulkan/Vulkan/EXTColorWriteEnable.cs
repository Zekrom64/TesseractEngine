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

#nullable disable
	public class EXTColorWriteEnableDeviceFunctions {

		public delegate void PFN_vkCmdSetColorWriteEnableEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint attachmentCount, [NativeType("const VkBool32*")] IntPtr pColorWriteEnables);

		public PFN_vkCmdSetColorWriteEnableEXT vkCmdSetColorWriteEnableEXT;

	}
#nullable restore

	public class EXTColorWriteEnable {

		public const string ExtensionName = "VK_EXT_color_write_enable";

	}

}
