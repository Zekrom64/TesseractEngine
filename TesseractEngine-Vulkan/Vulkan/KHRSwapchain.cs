using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

namespace Tesseract.Vulkan {

	using VKExtent2D = Vector2ui;

	public enum VKSwapchainCreateFlagBitsKHR {
		SplitInstanceBindRegions = 0x00000001,
		Protected = 0x00000002,
		MutableFormat = 0x00000004
	}

	public enum VKDeviceGroupPresentModeFlagBitsKHR {
		Local = 0x00000001,
		Remote = 0x00000002,
		Sum = 0x00000004,
		LocalMultiDevice = 0x00000008
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKSwapchainCreateInfoKHR {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKSwapchainCreateFlagBitsKHR flags;
		public VKSwapchainCreateFlagBitsKHR Flags { get => flags; init => flags = value; }
		private readonly ulong surface;
		[NativeType("VkSurfaceKHR")]
		public ulong Surface { get => surface; init => surface = value; }
		private readonly uint minImageCount;
		public uint MinImageCount { get => minImageCount; init => minImageCount = value; }
		private readonly VKFormat imageFormat;
		public VKFormat ImageFormat { get => imageFormat; init => imageFormat = value; }
		private readonly VKColorSpaceKHR imageColorSpace;
		public VKColorSpaceKHR ImageColorSpace { get => imageColorSpace; init => imageColorSpace = value; }
		private readonly VKExtent2D imageExtent;
		public VKExtent2D ImageExtent { get => imageExtent; init => imageExtent = value; }
		private readonly uint imageArrayLayers;
		public uint ImageArrayLayers { get => imageArrayLayers; init => imageArrayLayers = value; }
		private readonly VKImageUsageFlagBits imageUsage;
		public VKImageUsageFlagBits ImageUsage { get => imageUsage; init => imageUsage = value; }
		private readonly VKSharingMode imageSharingMode;
		public VKSharingMode ImageSharingMode { get => imageSharingMode; init => imageSharingMode = value; }
		private readonly uint queueFamilyIndexCount;
		public uint QueueFamilyIndexCount { get => queueFamilyIndexCount; init => queueFamilyIndexCount = value; }
		private readonly IntPtr queueFamilyIndices;
		[NativeType("const uint32_t*")]
		public IntPtr QueueFamilyIndices { get => queueFamilyIndices; init => queueFamilyIndices = value; }
		private readonly VKSurfaceTransformFlagBitsKHR preTransform;
		public VKSurfaceTransformFlagBitsKHR PreTransform { get => preTransform; init => preTransform = value; }
		private readonly VKCompositeAlphaFlagBitsKHR compositeAlpha;
		public VKCompositeAlphaFlagBitsKHR CompositeAlpha { get => compositeAlpha; init => compositeAlpha = value; }
		private readonly VKPresentModeKHR presentMode;
		public VKPresentModeKHR PresentMode { get => presentMode; init => presentMode = value; }
		private readonly bool clipped;
		public bool Clipped { get => clipped; init => clipped = value; }
		private readonly ulong oldSwapchain;
		[NativeType("VkSwapchainKHR")]
		public ulong OldSwapchain { get => oldSwapchain; init => oldSwapchain = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKPresentInfoKHR {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly uint waitSemaphoreCount;
		public uint WaitSemaphoreCount { get => waitSemaphoreCount; init => waitSemaphoreCount = value; }
		private readonly IntPtr waitSemaphores;
		[NativeType("const VkSemaphore*")]
		public IntPtr WaitSemaphores { get => waitSemaphores; init => waitSemaphores = value; }
		private readonly uint swapchainCount;
		public uint SwapchainCount { get => swapchainCount; init => swapchainCount = value; }
		private readonly IntPtr swapchains;
		[NativeType("const VkSwapchainKHR*")]
		public IntPtr Swapchains { get => swapchains; init => swapchains = value; }
		private readonly IntPtr imageIndices;
		[NativeType("const uint32_t*")]
		public IntPtr ImageIndices { get => imageIndices; init => imageIndices = value; }
		private readonly IntPtr results;
		[NativeType("VkResult*")]
		public IntPtr Results { get => results; init => results = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageSwapchainCreateInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("VkSwapchainKHR")]
		public ulong Swapchain;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindImageMemorySwapchainInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("VkSwapchainKHR")]
		public ulong Swapchain;
		public uint ImageIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKAcquireNextImageInfoKHR {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly ulong swapchain;
		[NativeType("VkSwapchainKHR")]
		public ulong Swapchain { get => swapchain; init => swapchain = value; }
		private readonly ulong fence;
		[NativeType("VkFence")]
		public ulong Fence { get => fence; init => fence = value; }
		private readonly uint deviceMask;
		public uint DeviceMask { get => deviceMask; init => deviceMask = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupPresentCapabilitiesKHR {

		public VKStructureType Type;
		public IntPtr Next;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.MaxDeviceGroupSize)]
		public uint[] PresentMask;
		public VKDeviceGroupPresentModeFlagBitsKHR Modes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupPresentInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public uint SwapchainCount;
		[NativeType("const uint32_t*")]
		public IntPtr DeviceMasks;
		public VKDeviceGroupPresentModeFlagBitsKHR Mode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupSwapchainCreateInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDeviceGroupPresentModeFlagBitsKHR Modes;

	}

#nullable disable
	public class KHRSwapchainDeviceFunctions {

		public delegate VKResult PFN_vkCreateSwapchainKHR([NativeType("VkDevice")] IntPtr device, in VKSwapchainCreateInfoKHR createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator, out ulong swapchain);
		public delegate void PFN_vkDestroySwapchainKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSwapchainKHR")] ulong swapchain, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator);
		public delegate VKResult PFN_vkGetSwapchainImagesKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSwapchainKHR")] ulong swapchain, ref uint swapchainImageCount, [NativeType("VkImage*")] IntPtr swapchainImages);
		public delegate VKResult PFN_vkAcquireNextImageKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSwapchainKHR")] ulong swapchain, ulong timeout, [NativeType("VkSemaphore")] ulong semaphore, [NativeType("VkFence")] ulong fence, out uint imageIndex);
		public delegate VKResult PFN_vkQueuePresentKHR([NativeType("VkQueue")] IntPtr queue, in VKPresentInfoKHR presentInfo);
		public delegate VKResult PFN_vkGetDeviceGroupPresentCapabilitiesKHR([NativeType("VkDevice")] IntPtr device, out VKDeviceGroupPresentCapabilitiesKHR deviceGroupPresentCapabilities);
		public delegate VKResult PFN_vkGetDeviceGroupSurfacePresentModesKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkSurfaceKHR")] ulong surface, out VKDeviceGroupPresentModeFlagBitsKHR modes);
		public delegate VKResult PFN_vkGetPhysicalDevicePresentRectanglesKHR([NativeType("VkPhysicalDevice")] IntPtr physicalDevice, [NativeType("VkSurfaceKHR")] ulong surface, ref uint rectCount, [NativeType("VkRect2D*")] IntPtr rects);
		public delegate VKResult PFN_vkAcquireNextImage2KHR([NativeType("VkDevice")] IntPtr device, in VKAcquireNextImageInfoKHR acquireInfo, out uint imageIndex);

		public PFN_vkCreateSwapchainKHR vkCreateSwapchainKHR;
		public PFN_vkDestroySwapchainKHR vkDestroySwapchainKHR;
		public PFN_vkGetSwapchainImagesKHR vkGetSwapchainImagesKHR;
		public PFN_vkAcquireNextImageKHR vkAcquireNextImageKHR;
		public PFN_vkQueuePresentKHR vkQueuePresentKHR;
		// With Vulkan 1.1 or VK_KHR_device_group
		[ExternFunction(Relaxed = true)]
		public PFN_vkGetDeviceGroupPresentCapabilitiesKHR vkGetDeviceGroupPresentCapabilitiesKHR;
		[ExternFunction(Relaxed = true)]
		public PFN_vkGetDeviceGroupSurfacePresentModesKHR vkGetDeviceGroupSurfacePresentModesKHR;
		[ExternFunction(Relaxed = true)]
		public PFN_vkGetPhysicalDevicePresentRectanglesKHR vkGetPhysicalDevicePresentRectanglesKHR;
		[ExternFunction(Relaxed = true)]
		public PFN_vkAcquireNextImage2KHR vkAcquireNextImage2KHR;

	}
#nullable restore

	public static class KHRSwapchain {

		public const string ExtensionName = "VK_KHR_swapchain";

	}

	public class VKSwapchainKHR : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.SwapchainKHR;

		public VKDevice Device { get; }

		[NativeType("VkSwapchainKHR")]
		public ulong SwapchainKHR { get; }

		public ulong PrimitiveHandle => SwapchainKHR;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKSwapchainKHR(VKDevice device, ulong swapchain, VulkanAllocationCallbacks? allocator) {
			Device = device;
			SwapchainKHR = swapchain;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.KHRSwapchain!.vkDestroySwapchainKHR(Device, SwapchainKHR, Allocator);
		}

		public VKImage[] Images {
			get {
				uint count = 0;
				VK.CheckError(Device.KHRSwapchain!.vkGetSwapchainImagesKHR(Device, SwapchainKHR, ref count, IntPtr.Zero), "Failed to get swapchain images");
				Span<ulong> images = stackalloc ulong[(int)count];
				unsafe {
					fixed(ulong* pImages = images) {
						VK.CheckError(Device.KHRSwapchain.vkGetSwapchainImagesKHR(Device, SwapchainKHR, ref count, (IntPtr)pImages), "Failed to get swapchain images");
					}
				}
				VKImage[] imgs = new VKImage[count];
				for (int i = 0; i < count; i++) imgs[i] = new VKImage(Device, images[i], null);
				return imgs;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKResult AcquireNextImage(ulong timeout, VKSemaphore? semaphore, VKFence? fence, out uint imageIndex) =>
			Device.KHRSwapchain!.vkAcquireNextImageKHR(Device, SwapchainKHR, timeout, semaphore, fence, out imageIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKResult AcquireNextImage2(in VKAcquireNextImageInfoKHR acquireInfo, out uint imageIndex) =>
			Device.KHRSwapchain!.vkAcquireNextImage2KHR(Device, acquireInfo, out imageIndex);

		public static implicit operator ulong(VKSwapchainKHR? swapchain) => swapchain != null ? swapchain.SwapchainKHR : 0;

	}

}
