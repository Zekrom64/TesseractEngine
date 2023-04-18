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

	public unsafe class EXTVertexInputDynamicStateDeviceFunctions {

		[NativeType("void vkCmdSetVertexInputEXT(VkCommandBuffer commandBuffer, uint32_t vertexBindingDescriptionCount, const VkVertexInputBindingDescription2EXT* pVertexBindingDescriptions, uint32_t vertexAttributeDescriptionCount, const VkVertexInputAttributeDescription2EXT* pVertexAttributeDescriptions)")]
		public delegate* unmanaged<IntPtr, uint, VKVertexInputBindingDescription2EXT*, uint, VKVertexInputAttributeDescription2EXT*, void> vkCmdSetVertexInputEXT;

	}

	public class EXTVertexInputDynamicState {

		public const string ExtensionName = "VK_EXT_vertex_input_dynamic_state";

	}

}
