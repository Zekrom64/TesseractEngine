using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Tesseract.Vulkan {

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSamplerCustomBorderColorCreateInfoEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public VKClearColorValue CustomBorderColor;

		public VKFormat Format;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceCustomBorderColorPropertiesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public uint MaxCustomBorderColorSamplers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceCustomBorderColorFeaturesEXT {

		public VKStructureType Type;

		public IntPtr Next;

		public VKBool32 CustomBorderColors;

		public VKBool32 CustomBorderColorsWithoutFormat;

	}

	public static class EXTCustomBorderColor {

		public const string ExtensionName = "VK_EXT_custom_border_color";
	
	}
}
