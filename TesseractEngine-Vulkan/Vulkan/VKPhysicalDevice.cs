using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

namespace Tesseract.Vulkan {

	public class VKPhysicalDevice : IVKInstanceObject {

		public VKInstance Instance { get; }

		[NativeType("VkPhysicalDevice")]
		public IntPtr PhysicalDevice { get; }

		public IReadOnlyIndexer<VKFormat, VKFormatProperties> FormatProperties { get; }

		public VKPhysicalDevice(VKInstance instance, IntPtr physicalDevice) {
			Instance = instance;
			PhysicalDevice = physicalDevice;

			VKFormatProperties GetFormatProperties(VKFormat format) {
				Instance.VK10Functions.vkGetPhysicalDeviceFormatProperties(PhysicalDevice, format, out VKFormatProperties properties);
				return properties;
			}
			FormatProperties = new FuncReadOnlyIndexer<VKFormat, VKFormatProperties>(GetFormatProperties);
		}

		// Vulkan 1.0

		public VKPhysicalDeviceFeatures Features {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Instance.VK10Functions.vkGetPhysicalDeviceFeatures(PhysicalDevice, out VKPhysicalDeviceFeatures features);
				return features;
			}
		}
		
		public VKPhysicalDeviceProperties Properties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Instance.VK10Functions.vkGetPhysicalDeviceProperties(PhysicalDevice, out VKPhysicalDeviceProperties properties);
				return properties;
			}
		}

		public VKQueueFamilyProperties[] QueueFamilyProperties {
			get {
				uint propCount = 0;
				Instance.VK10Functions.vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, ref propCount, IntPtr.Zero);
				VKQueueFamilyProperties[] props = new VKQueueFamilyProperties[propCount];
				unsafe {
					fixed(VKQueueFamilyProperties* pProps = props) {
						Instance.VK10Functions.vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, ref propCount, (IntPtr)pProps);
					}
				}
				return props;
			}
		}

		public VKPhysicalDeviceMemoryProperties MemoryProperties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Instance.VK10Functions.vkGetPhysicalDeviceMemoryProperties(PhysicalDevice, out VKPhysicalDeviceMemoryProperties properties);
				return properties;
			}
		}

		public VKExtensionProperties[] DeviceExtensions {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => EnumerateDeviceExtensionProperties(null);
		}

		public VKLayerProperties[] DeviceLayers {
			get {
				uint propCount = 0;
				VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceLayerProperties(PhysicalDevice, ref propCount, IntPtr.Zero), "Failed to enumerate physical device layers");
				using ManagedPointer<VKLayerProperties> pProps = new((int)propCount);
				VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceLayerProperties(PhysicalDevice, ref propCount, pProps), "Failed to enumerate physical device layers");
				VKLayerProperties[] props = new VKLayerProperties[propCount];
				for (uint i = 0; i < propCount; i++) props[i] = pProps[(int)i];
				return props;
			}
		}

		public VKExtensionProperties[] EnumerateDeviceExtensionProperties(string layerName) {
			uint propCount = 0;
			VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceExtensionProperties(PhysicalDevice, layerName, ref propCount, IntPtr.Zero), "Failed to enumerate physical device extensions");
			using ManagedPointer<VKExtensionProperties> pProps = new((int)propCount);
			VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceExtensionProperties(PhysicalDevice, layerName, ref propCount, pProps), "Failed to enumerate physical device extensions");
			VKExtensionProperties[] props = new VKExtensionProperties[propCount];
			for (uint i = 0; i < propCount; i++) props[i] = pProps[(int)i];
			return props;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageFormatProperties GetImageFormatProperties(VKFormat format, VKImageType type, VKImageTiling tiling, VKImageUsageFlagBits usage, VKImageCreateFlagBits flags) {
			VK.CheckError(Instance.VK10Functions.vkGetPhysicalDeviceImageFormatProperties(PhysicalDevice, format, type, tiling, usage, flags, out VKImageFormatProperties properties), "Failed to get physical device image format properties");
			return properties;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageFormatProperties GetImageFormatProperties(in VKImageCreateInfo info) => GetImageFormatProperties(info.Format, info.ImageType, info.Tiling, info.Usage, info.Flags);

		public VKSparseImageFormatProperties[] GetSparseImageFormatProperties(VKFormat format, VKImageType type, VKSampleCountFlagBits samples, VKImageUsageFlagBits usage, VKImageTiling tiling, VKImageCreateFlagBits flags) {
			uint propCount = 0;
			Instance.VK10Functions.vkGetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice, format, type, samples, usage, tiling, ref propCount, IntPtr.Zero);
			VKSparseImageFormatProperties[] props = new VKSparseImageFormatProperties[propCount];
			unsafe {
				fixed(VKSparseImageFormatProperties* pProps = props) {
					Instance.VK10Functions.vkGetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice, format, type, samples, usage, tiling, ref propCount, (IntPtr)pProps);
				}
			}
			return props;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSparseImageFormatProperties[] GetSparseImageFormatProperties(in VKImageCreateInfo info) => GetSparseImageFormatProperties(info.Format, info.ImageType, info.Samples, info.Usage, info.Tiling, info.Flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDevice CreateDevice(in VKDeviceCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(Instance.VK10Functions.vkCreateDevice(PhysicalDevice, createInfo, allocator, out IntPtr device), "Failed to create logical device");
			return new VKDevice(Instance, device, createInfo, allocator);
		}

		// VK_KHR_surface

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetSurfaceSupportKHR(uint queueFamilyIndex, VKSurfaceKHR surface) {
			VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfaceSupportKHR(PhysicalDevice, queueFamilyIndex, surface, out bool supported), "Failed to get physical device surface support");
			return supported;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(VKSurfaceKHR surface) {
			VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice, surface, out VKSurfaceCapabilitiesKHR capabilities), "Failed to get physical device surface capabilities");
			return capabilities;
		}

		public VKSurfaceFormatKHR[] GetSurfaceFormatsKHR(VKSurfaceKHR surface) {
			uint count = 0;
			VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device surface formats");
			VKSurfaceFormatKHR[] formats = new VKSurfaceFormatKHR[count];
			unsafe {
				fixed(VKSurfaceFormatKHR* pFormats = formats) {
					VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice, surface, ref count, (IntPtr)pFormats), "Failed to get physical device surface formats");
				}
			}
			return formats;
		}

		public VKPresentModeKHR[] GetSurfacePresentModesKHR(VKSurfaceKHR surface) {
			uint count = 0;
			VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device surface present modes");
			VKPresentModeKHR[] modes = new VKPresentModeKHR[count];
			unsafe {
				fixed(VKPresentModeKHR* pModes = modes) {
					VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, (IntPtr)pModes), "Failed to get physical device surface present modes");
				}
			}
			return modes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKPhysicalDevice pd) => pd.PhysicalDevice;

	}
}
