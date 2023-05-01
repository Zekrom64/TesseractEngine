using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

namespace Tesseract.Vulkan {

	using VKExtent2D = Vector2ui;

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

	public unsafe class KHRSurfaceInstanceFunctions {

		[NativeType("void vkDestroySurfaceKHR(VkInstance instance, VkSurfaceKHR surface, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroySurfaceKHR;
		[NativeType("VkResult vkGetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDevice physicalDevice, uint32_t queueFamilyIndex, VkSurfaceKHR surface, VkBool32* pSupported)")]
		public delegate* unmanaged<IntPtr, uint, ulong, out VKBool32, VKResult> vkGetPhysicalDeviceSurfaceSupportKHR;
		[NativeType("VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceCapabilitiesKHR* pCapabilities)")]
		public delegate* unmanaged<IntPtr, ulong, out VKSurfaceCapabilitiesKHR, VKResult> vkGetPhysicalDeviceSurfaceCapabilitiesKHR;
		[NativeType("VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32_t* pSurfaceFormatCount, VkSurfaceFormatKHR* pSurfaceFormats)")]
		public delegate* unmanaged<IntPtr, ulong, ref uint, VKSurfaceFormatKHR*, VKResult> vkGetPhysicalDeviceSurfaceFormatsKHR;
		[NativeType("VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32_t* pPresentModeCount, VkPresentModeKHR* pPresentModes)")]
		public delegate* unmanaged<IntPtr, ulong, ref uint, VKPresentModeKHR*, VKResult> vkGetPhysicalDeviceSurfacePresentModesKHR;

	}

	public static class KHRSurface {

		public const string ExtensionName = "VK_KHR_surface";

	}

	public class VKSurfaceKHR : IVKInstanceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKInstance Instance { get; }

		public ulong SurfaceKHR { get; }

		public ulong PrimitiveHandle => SurfaceKHR;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKSurfaceKHR(VKInstance instance, ulong surface, VulkanAllocationCallbacks? allocator) {
			Instance = instance;
			SurfaceKHR = surface;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Instance.KHRSurfaceFunctions!.vkDestroySurfaceKHR(Instance, SurfaceKHR, Allocator);
			}
		}

		public static implicit operator ulong(VKSurfaceKHR surface) => surface != null ? surface.SurfaceKHR : 0;

	}

}
