using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	[StructLayout(LayoutKind.Sequential)]
	public struct VKVertexInputAttributeDescription2EXT {

		public VKStructureType Type;

		public IntPtr Next;

		public uint Location;

		public uint Binding;

		public VKFormat Format;

		public uint Offset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKVertexInputBindingDescription2EXT {

		public VKStructureType Type;

		public IntPtr Next;

		public uint Binding;

		public uint Stride;

		public VKVertexInputRate InputRate;

		public uint Divisor;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVertexInputDynamicStateFeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public VKBool32 VertexInputDynamicState;

	}

#nullable disable
	public class EXTVertexInputDynamicStateDeviceFunctions {

		public delegate void PFN_vkCmdSetVertexInputEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint vertexBindingDescriptionCount, [NativeType("const VkVertexInputBindingDescription2EXT*")] IntPtr pVertexBindingDescriptions, uint vertexAttributeDescriptionCount, [NativeType("const VkVertexInputAttributeDescription2EXT*")] IntPtr pVertexAttributeDescriptions);

		public PFN_vkCmdSetVertexInputEXT vkCmdSetVertexInputEXT;

	}
#nullable restore

	public class EXTVertexInputDynamicState {

		public const string ExtensionName = "VK_EXT_vertex_input_dynamic_state";

	}

}
