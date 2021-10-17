using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public enum VKLineRasterizationModeEXT : int {
		Default = 0,
		Rectangular = 1,
		Bresenham = 2,
		RectangularSmooth = 3
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceLineRasterizationFeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public bool RectangularLlines;

		public bool BresenhamLines;

		public bool SmoothLines;

		public bool StippledRectangularLines;

		public bool StippledBresenhamLines;

		public bool StippledSmoothLines;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceLineRasterizationPropertiesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public uint LineSubPixelPrecisionBits;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineRasterizationLineStateCreateInfo {

		public VKStructureType Type;

		public IntPtr Next;

		public VKLineRasterizationModeEXT LineRasterizationMode;

		public bool StippleLineEnabled;

		public uint LineStippleFactor;

		public ushort LineStipplePattern;

	}

	public class EXTLineRasterizationDeviceFunctions {

		public delegate void PFN_vkCmdSetLineStippleEXT([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint lineStippleFactor, ushort lineStipplePattern);

		public PFN_vkCmdSetLineStippleEXT vkCmdSetLineStippleEXT;

		public static implicit operator bool(EXTLineRasterizationDeviceFunctions fn) => fn != null;

	}

	public static class EXTLineRasterization {

		public const string ExtensionName = "VK_EXT_line_rasterization";

	}

}
