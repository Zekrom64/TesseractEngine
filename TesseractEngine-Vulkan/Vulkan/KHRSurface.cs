using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public enum VKPresentModeKHR {
		Immediate = 0,
		Mailbox,
		FIFO,
		FIFORelaxed,
		SharedDemandRefresh = 1000111000,
		SharedContinuousRefresh = 1000111001
	}

	public enum VKColorSpaceKHR {
		SRGBNonlinear,
		DisplayP3NonlinearEXT = 1000104001,
		ExtendedSRGBLinearEXT = 1000104002,
		DisplayP3LinearEXT = 1000104003,
		DCIP3LinearEXT = 1000104004,
		BT709LinearEXT = 1000104005,
		BT709NonlinearEXT = 1000104006,
		BT2020LinearEXT = 1000104007,
		HDR10_ST2084_EXT = 1000104008,
		DolbyVisionEXT = 1000104009,
		HDR10_HLG_EXT = 1000104010,
		AdobeRGBLinearEXT = 1000104011,
		AdobeRGBNonlinearEXT = 1000104012,
		PassThroughEXT = 1000104013,
		ExtendedSRGBNonlinearEXT = 1000104014,
		DisplayNativeAMD = 1000213000
	}

	public enum VKSurfaceTransformFlagBitsKHR {
		Identity = 0x00000001,
		Rotate90 = 0x00000002,
		Rotate180 = 0x00000004,
		Rotate270 = 0x00000008,
		HorizontalMirror = 0x00000010,
		HorizontalMirrorRotate90 = 0x00000020,
		HorizontalMirrorRotate180 = 0x00000040,
		HorizontalMirrorRotate270 = 0x00000080,
		Inherit = 0x00000100
	}

	public enum	VKCompositeAlphaFlagBitsKHR {
		Opaque = 0x00000001,
		PreMultiplied = 0x00000002,
		PostMultiplied = 0x00000004,
		Inherit = 0x00000008
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSurfaceCapabilitiesKHR {

		public uint MinImageCount;
		public uint MaxImageCount;
		public VKExtent2D CurrentExtent;
		public VKExtent2D MinImageExtent;
		public VKExtent2D MaxImageExtent;
		public uint MaxImageArrayLayers;
		public VKSurfaceTransformFlagBitsKHR SupportedTransforms;
		public VKSurfaceTransformFlagBitsKHR CurrentTransform;
		public VKCompositeAlphaFlagBitsKHR SupportedCompositeAlpha;
		public VKImageUsageFlagBits SupportedUsageFlags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSurfaceFormatKHR {

		public VKFormat Format;
		public VKColorSpaceKHR ColorSpace;

	}

	public class KHRSurfaceInstanceFunctions {

		public delegate void PFN_vkDestroySurfaceKHR(IntPtr instance, ulong surface, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator);
		public delegate VKResult PFN_vkGetPhysicalDeviceSurfaceSupportKHR(IntPtr physicalDevice, uint queueFamilyIndex, ulong surface, out bool supported);
		public delegate VKResult PFN_vkGetPhysicalDeviceSurfaceCapabilitiesKHR(IntPtr physicalDevice, ulong surface, out VKSurfaceCapabilitiesKHR capabilities);
		public delegate VKResult PFN_vkGetPhysicalDeviceSurfaceFormatsKHR(IntPtr physicalDevice, ulong surface, ref uint surfaceFormatCount, [NativeType("VkSurfaceFormatKHR*")] IntPtr pSurfaceFormats);
		public delegate VKResult PFN_vkGetPhysicalDeviceSurfacePresentModesKHR(IntPtr physicalDevice, ulong surface, ref uint presentModeCount, [NativeType("VkPresentModeKHR*")] IntPtr pPresentModes);

		public PFN_vkDestroySurfaceKHR vkDestroySurfaceKHR;
		public PFN_vkGetPhysicalDeviceSurfaceSupportKHR vkGetPhysicalDeviceSurfaceSupportKHR;
		public PFN_vkGetPhysicalDeviceSurfaceCapabilitiesKHR vkGetPhysicalDeviceSurfaceCapabilitiesKHR;
		public PFN_vkGetPhysicalDeviceSurfaceFormatsKHR vkGetPhysicalDeviceSurfaceFormatsKHR;
		public PFN_vkGetPhysicalDeviceSurfacePresentModesKHR vkGetPhysicalDeviceSurfacePresentModesKHR;

	}

	public static class KHRSurface {

		public const string ExtensionName = "VK_KHR_surface";

	}

	public class VKSurfaceKHR : IVKInstanceObject, IVKAllocatedObject, IDisposable {

		public VKInstance Instance { get; }

		public ulong SurfaceKHR { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKSurfaceKHR(VKInstance instance, ulong surface, VulkanAllocationCallbacks allocator) {
			Instance = instance;
			SurfaceKHR = surface;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Instance.KHRSurfaceFunctions.vkDestroySurfaceKHR(Instance, SurfaceKHR, Allocator);
		}

		public static implicit operator ulong(VKSurfaceKHR surface) => surface != null ? surface.SurfaceKHR : 0;

	}

}
