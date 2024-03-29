﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Collections;

namespace Tesseract.Vulkan {

    public class VKPhysicalDevice : IVKInstanceObject, IPrimitiveHandle<IntPtr> {

		public VKInstance Instance { get; }

		[NativeType("VkPhysicalDevice")]
		public IntPtr PhysicalDevice { get; }

		public IntPtr PrimitiveHandle => PhysicalDevice;

		public IReadOnlyIndexer<VKFormat, VKFormatProperties> FormatProperties { get; }

		public VKPhysicalDevice(VKInstance instance, IntPtr physicalDevice) {
			Instance = instance;
			PhysicalDevice = physicalDevice;

			VKFormatProperties GetFormatProperties(VKFormat format) {
				unsafe {
					Instance.VK10Functions.vkGetPhysicalDeviceFormatProperties(PhysicalDevice, format, out VKFormatProperties properties);
					return properties;
				}
			}
			FormatProperties = new FuncReadOnlyIndexer<VKFormat, VKFormatProperties>(GetFormatProperties);
		}

		// Vulkan 1.0

		public VKPhysicalDeviceFeatures Features {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					Instance.VK10Functions.vkGetPhysicalDeviceFeatures(PhysicalDevice, out VKPhysicalDeviceFeatures features);
					return features;
				}
			}
		}
		
		public VKPhysicalDeviceProperties Properties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					Instance.VK10Functions.vkGetPhysicalDeviceProperties(PhysicalDevice, out VKPhysicalDeviceProperties properties);
					return properties;
				}
			}
		}

		public VKQueueFamilyProperties[] QueueFamilyProperties {
			get {
				unsafe {
					uint propCount = 0;
					Instance.VK10Functions.vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, ref propCount, (VKQueueFamilyProperties*)0);
					VKQueueFamilyProperties[] props = new VKQueueFamilyProperties[propCount];
					fixed(VKQueueFamilyProperties* pProps = props) {
						Instance.VK10Functions.vkGetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, ref propCount, pProps);
					}
					return props;
				}
			}
		}

		public VKPhysicalDeviceMemoryProperties MemoryProperties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					Instance.VK10Functions.vkGetPhysicalDeviceMemoryProperties(PhysicalDevice, out VKPhysicalDeviceMemoryProperties properties);
					return properties;
				}
			}
		}

		public VKExtensionProperties[] DeviceExtensions {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => EnumerateDeviceExtensionProperties(null);
		}

		public VKLayerProperties[] DeviceLayers {
			get {
				unsafe {
					uint propCount = 0;
					VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceLayerProperties(PhysicalDevice, ref propCount, (VKLayerProperties*)0), "Failed to enumerate physical device layers");
					VKLayerProperties[] properties = new VKLayerProperties[propCount];
					fixed(VKLayerProperties* pProperties = properties) {
						VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceLayerProperties(PhysicalDevice, ref propCount, pProperties), "Failed to enumerate physical device layers");
					}
					return properties;
				}
			}
		}

		public VKExtensionProperties[] EnumerateDeviceExtensionProperties(string? layerName) {
			unsafe {
				uint propCount = 0;
				Span<byte> strLayerName = layerName != null ? MemoryUtil.StackallocUTF8(layerName, stackalloc byte[256]) : Span<byte>.Empty;
				fixed(byte* pLayerName = strLayerName) {
					VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceExtensionProperties(PhysicalDevice, (IntPtr)pLayerName, ref propCount, (VKExtensionProperties*)0), "Failed to enumerate physical device extensions");
					VKExtensionProperties[] properties = new VKExtensionProperties[propCount];
					fixed(VKExtensionProperties* pProperties = properties) {
						VK.CheckError(Instance.VK10Functions.vkEnumerateDeviceExtensionProperties(PhysicalDevice, (IntPtr)pLayerName, ref propCount, pProperties), "Failed to enumerate physical device extensions");
					}
					return properties;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageFormatProperties GetImageFormatProperties(VKFormat format, VKImageType type, VKImageTiling tiling, VKImageUsageFlagBits usage, VKImageCreateFlagBits flags) {
			unsafe {
				VK.CheckError(Instance.VK10Functions.vkGetPhysicalDeviceImageFormatProperties(PhysicalDevice, format, type, tiling, usage, flags, out VKImageFormatProperties properties), "Failed to get physical device image format properties");
				return properties;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageFormatProperties GetImageFormatProperties(in VKImageCreateInfo info) => GetImageFormatProperties(info.Format, info.ImageType, info.Tiling, info.Usage, info.Flags);

		public VKSparseImageFormatProperties[] GetSparseImageFormatProperties(VKFormat format, VKImageType type, VKSampleCountFlagBits samples, VKImageUsageFlagBits usage, VKImageTiling tiling, VKImageCreateFlagBits flags) {
			unsafe {
				uint propCount = 0;
				Instance.VK10Functions.vkGetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice, format, type, samples, usage, tiling, ref propCount, (VKSparseImageFormatProperties*)0);
				VKSparseImageFormatProperties[] props = new VKSparseImageFormatProperties[propCount];
				fixed (VKSparseImageFormatProperties* pProperties = props) {
					Instance.VK10Functions.vkGetPhysicalDeviceSparseImageFormatProperties(PhysicalDevice, format, type, samples, usage, tiling, ref propCount, pProperties);
				}
				return props;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSparseImageFormatProperties[] GetSparseImageFormatProperties(in VKImageCreateInfo info) => GetSparseImageFormatProperties(info.Format, info.ImageType, info.Samples, info.Usage, info.Tiling, info.Flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDevice CreateDevice(in VKDeviceCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			unsafe {
				VK.CheckError(Instance.VK10Functions.vkCreateDevice(PhysicalDevice, createInfo, allocator, out IntPtr device), "Failed to create logical device");
				return new VKDevice(Instance, device, createInfo, allocator, this);
			}
		}

		// VK_KHR_surface

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetSurfaceSupportKHR(uint queueFamilyIndex, VKSurfaceKHR surface) {
			unsafe {
				VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceSupportKHR(PhysicalDevice, queueFamilyIndex, surface, out VKBool32 supported), "Failed to get physical device surface support");
				return supported;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(VKSurfaceKHR surface) {
			unsafe {
				VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice, surface, out VKSurfaceCapabilitiesKHR capabilities), "Failed to get physical device surface capabilities");
				return capabilities;
			}
		}

		public VKSurfaceFormatKHR[] GetSurfaceFormatsKHR(VKSurfaceKHR surface) {
			unsafe {
				uint count = 0;
				VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice, surface, ref count, (VKSurfaceFormatKHR*)0), "Failed to get physical device surface formats");
				VKSurfaceFormatKHR[] formats = new VKSurfaceFormatKHR[count];
				fixed(VKSurfaceFormatKHR* pFormats = formats) {
					VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice, surface, ref count, pFormats), "Failed to get physical device surface formats");
				}
				return formats;
			}
		}

		public VKPresentModeKHR[] GetSurfacePresentModesKHR(VKSurfaceKHR surface) {
			unsafe {
				uint count = 0;
				VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, (VKPresentModeKHR*)0), "Failed to get physical device surface present modes");
				VKPresentModeKHR[] modes = new VKPresentModeKHR[count];
				unsafe {
					fixed (VKPresentModeKHR* pModes = modes) {
						VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, pModes), "Failed to get physical device surface present modes");
					}
				}
				return modes;
			}
		}

		// Vulkan 1.1
		// VK_KHR_get_physical_device_properties2

		public void GetFeatures2(ref VKPhysicalDeviceFeatures2 features) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceFeatures2(PhysicalDevice, ref features);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceFeatures2KHR(PhysicalDevice, ref features);
			}
		}

		public void GetFormatProperties2(VKFormat format, ref VKFormatProperties2 properties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceFormatProperties2(PhysicalDevice, format, ref properties);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceFormatProperties2KHR(PhysicalDevice, format, ref properties);
			}
		}

		public VKResult GetImageFormatProperties2(in VKPhysicalDeviceImageFormatInfo2 formatInfo, ref VKImageFormatProperties2 properties) {
			unsafe {
				if (Instance.VK11Functions != null) return Instance.VK11Functions.vkGetPhysicalDeviceImageFormatProperties2(PhysicalDevice, formatInfo, ref properties);
				else return Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref properties);
			}
		}

		public void GetMemoryProperties2(ref VKPhysicalDeviceMemoryProperties2 properties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceMemoryProperties2(PhysicalDevice, ref properties);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceMemoryProperties2KHR(PhysicalDevice, ref properties);
			}
		}

		public void GetProperties2(ref VKPhysicalDeviceProperties2 properties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceProperties2(PhysicalDevice, ref properties);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceProperties2KHR(PhysicalDevice, ref properties);
			}
		}

		public void GetQueueFamilyProperties2(out uint queueCount) {
			unsafe {
				queueCount = 0;
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice, ref queueCount, (VKQueueFamilyProperties2*)0);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceQueueFamilyProperties2KHR(PhysicalDevice, ref queueCount, (VKQueueFamilyProperties2*)0);
			}
		}

		public void GetQueueFamilyProperties2(Span<VKQueueFamilyProperties2> properties) {
			uint queueCount = (uint)properties.Length;
			unsafe {
				fixed(VKQueueFamilyProperties2* pProperties = properties) {
					if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice, ref queueCount, pProperties);
					else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceQueueFamilyProperties2KHR(PhysicalDevice, ref queueCount, pProperties);
				}
			}
		}

		public void GetSparseImageFormatProperties2(in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, out uint propertyCount) {
			unsafe {
				propertyCount = 0;
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice, formatInfo, ref propertyCount, (VKSparseImageFormatProperties2*)0);
				else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceSparseImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref propertyCount, (VKSparseImageFormatProperties2*)0);
			}
		}

		public void GetSparseImageFormatProperties2(in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, Span<VKSparseImageFormatProperties2> properties) {
			uint propertyCount = (uint)properties.Length;
			unsafe {
				fixed (VKSparseImageFormatProperties2* pProperties = properties) {
					if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice, formatInfo, ref propertyCount, pProperties);
					else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceSparseImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref propertyCount, pProperties);
				}
			}
		}

		// Vulkan 1.1
		// VK_KHR_external_fence_capabilities
		public void GetExternalFenceProperties(in VKPhysicalDeviceExternalFenceInfo externalFenceInfo, ref VKExternalFenceProperties externalFenceProperties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalFenceProperties(PhysicalDevice, externalFenceInfo, ref externalFenceProperties);
				else Instance.KHRExternalFenceCapabilitiesFunctions!.vkGetPhysicalDeviceExternalFencePropertiesKHR(PhysicalDevice, externalFenceInfo, ref externalFenceProperties);
			}
		}

		// Vulkan 1.1
		// VK_KHR_external_memory_capabilities
		public void GetExternalBufferProperties(in VKPhysicalDeviceExternalBufferInfo externalBufferInfo, ref VKExternalBufferProperties externalBufferProperties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalBufferProperties(PhysicalDevice, externalBufferInfo, ref externalBufferProperties);
				else Instance.KHRExternalMemoryCapabilitiesFunctions!.vkGetPhysicalDeviceExternalBufferPropertiesKHR(PhysicalDevice, externalBufferInfo, ref externalBufferProperties);
			}
		}

		// Vulkan 1.1
		// VK_KHR_external_semaphore_capabilities
		public void GetExternalSemaphoreProperties(in VKPhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, ref VKExternalSemaphoreProperties externalSemaphoreProperties) {
			unsafe {
				if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalSemaphoreProperties(PhysicalDevice, externalSemaphoreInfo, ref externalSemaphoreProperties);
				else Instance.KHRExternalSemaphoreCapabilitiesFunctions!.vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(PhysicalDevice, externalSemaphoreInfo, ref externalSemaphoreProperties);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKPhysicalDevice? pd) => pd!.PhysicalDevice;

	}
}
