using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

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

		public VKExtensionProperties[] EnumerateDeviceExtensionProperties(string? layerName) {
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
		public VKDevice CreateDevice(in VKDeviceCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(Instance.VK10Functions.vkCreateDevice(PhysicalDevice, createInfo, allocator, out IntPtr device), "Failed to create logical device");
			return new VKDevice(Instance, device, createInfo, allocator);
		}

		// VK_KHR_surface

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetSurfaceSupportKHR(uint queueFamilyIndex, VKSurfaceKHR surface) {
			VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceSupportKHR(PhysicalDevice, queueFamilyIndex, surface, out bool supported), "Failed to get physical device surface support");
			return supported;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(VKSurfaceKHR surface) {
			VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice, surface, out VKSurfaceCapabilitiesKHR capabilities), "Failed to get physical device surface capabilities");
			return capabilities;
		}

		public VKSurfaceFormatKHR[] GetSurfaceFormatsKHR(VKSurfaceKHR surface) {
			uint count = 0;
			VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device surface formats");
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
			VK.CheckError(Instance.KHRSurfaceFunctions!.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device surface present modes");
			VKPresentModeKHR[] modes = new VKPresentModeKHR[count];
			unsafe {
				fixed(VKPresentModeKHR* pModes = modes) {
					VK.CheckError(Instance.KHRSurfaceFunctions.vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice, surface, ref count, (IntPtr)pModes), "Failed to get physical device surface present modes");
				}
			}
			return modes;
		}

		// Vulkan 1.1
		// VK_KHR_get_physical_device_properties2

		public void GetFeatures2(ref VKPhysicalDeviceFeatures2 features) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceFeatures2(PhysicalDevice, ref features);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceFeatures2KHR(PhysicalDevice, ref features);
		}

		public void GetFormatProperties2(VKFormat format, ref VKFormatProperties2 properties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceFormatProperties2(PhysicalDevice, format, ref properties);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceFormatProperties2KHR(PhysicalDevice, format, ref properties);
		}

		public VKResult GetImageFormatProperties2(in VKPhysicalDeviceImageFormatInfo2 formatInfo, ref VKImageFormatProperties2 properties) {
			if (Instance.VK11Functions != null) return Instance.VK11Functions.vkGetPhysicalDeviceImageFormatProperties2(PhysicalDevice, formatInfo, ref properties);
			else return Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref properties);
		}

		public void GetMemoryProperties2(ref VKPhysicalDeviceMemoryProperties2 properties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceMemoryProperties2(PhysicalDevice, ref properties);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceMemoryProperties2KHR(PhysicalDevice, ref properties);
		}

		public void GetProperties2(ref VKPhysicalDeviceProperties2 properties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceProperties2(PhysicalDevice, ref properties);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceProperties2KHR(PhysicalDevice, ref properties);
		}

		public void GetQueueFamilyProperties2(out uint queueCount) {
			queueCount = 0;
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice, ref queueCount, IntPtr.Zero);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceQueueFamilyProperties2KHR(PhysicalDevice, ref queueCount, IntPtr.Zero);
		}

		public void GetQueueFamilyProperties2(Span<VKQueueFamilyProperties2> properties) {
			uint queueCount = (uint)properties.Length;
			unsafe {
				fixed(VKQueueFamilyProperties2* pProperties = properties) {
					if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceQueueFamilyProperties2(PhysicalDevice, ref queueCount, (IntPtr)pProperties);
					else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceQueueFamilyProperties2KHR(PhysicalDevice, ref queueCount, (IntPtr)pProperties);
				}
			}
		}

		public void GetSparseImageFormatProperties2(in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, out uint propertyCount) {
			propertyCount = 0;
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice, formatInfo, ref propertyCount, IntPtr.Zero);
			else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceSparseImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref propertyCount, IntPtr.Zero);
		}

		public void GetSparseImageFormatProperties2(in VKPhysicalDeviceSparseImageFormatInfo2 formatInfo, Span<VKSparseImageFormatProperties2> properties) {
			uint propertyCount = (uint)properties.Length;
			unsafe {
				fixed (VKSparseImageFormatProperties2* pProperties = properties) {
					if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceSparseImageFormatProperties2(PhysicalDevice, formatInfo, ref propertyCount, (IntPtr)pProperties);
					else Instance.KHRGetPhysicalDeviceProperties2Functions!.vkGetPhysicalDeviceSparseImageFormatProperties2KHR(PhysicalDevice, formatInfo, ref propertyCount, (IntPtr)pProperties);
				}
			}
		}

		// Vulkan 1.1
		// VK_KHR_external_fence_capabilities
		public void GetExternalFenceProperties(in VKPhysicalDeviceExternalFenceInfo externalFenceInfo, ref VKExternalFenceProperties externalFenceProperties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalFenceProperties(PhysicalDevice, externalFenceInfo, ref externalFenceProperties);
			else Instance.KHRExternalFenceCapabilitiesFunctions!.vkGetPhysicalDeviceExternalFencePropertiesKHR(PhysicalDevice, externalFenceInfo, ref externalFenceProperties);
		}

		// Vulkan 1.1
		// VK_KHR_external_memory_capabilities
		public void GetExternalBufferProperties(in VKPhysicalDeviceExternalBufferInfo externalBufferInfo, ref VKExternalBufferProperties externalBufferProperties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalBufferProperties(PhysicalDevice, externalBufferInfo, ref externalBufferProperties);
			else Instance.KHRExternalMemoryCapabilitiesFunctions!.vkGetPhysicalDeviceExternalBufferPropertiesKHR(PhysicalDevice, externalBufferInfo, ref externalBufferProperties);
		}

		// Vulkan 1.1
		// VK_KHR_external_semaphore_capabilities
		public void GetExternalSemaphoreProperties(in VKPhysicalDeviceExternalSemaphoreInfo externalSemaphoreInfo, ref VKExternalSemaphoreProperties externalSemaphoreProperties) {
			if (Instance.VK11Functions != null) Instance.VK11Functions.vkGetPhysicalDeviceExternalSemaphoreProperties(PhysicalDevice, externalSemaphoreInfo, ref externalSemaphoreProperties);
			else Instance.KHRExternalSemaphoreCapabilitiesFunctions!.vkGetPhysicalDeviceExternalSemaphorePropertiesKHR(PhysicalDevice, externalSemaphoreInfo, ref externalSemaphoreProperties);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKPhysicalDevice? pd) => pd!.PhysicalDevice;

	}
}
