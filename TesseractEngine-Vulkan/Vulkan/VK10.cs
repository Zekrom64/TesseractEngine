using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public class VK10 {

		[Obsolete("Deprecated in the Vulkan spec, use MakeApiVersion instead")]
		public static uint MakeVersion(uint major, uint minor, uint patch) => (major << 22) | (minor << 12) | patch;
		[Obsolete("Deprecated in the Vulkan spec, use ApiVersionMajor instead")]
		public static uint VersionMajor(uint version) => version >> 22;
		[Obsolete("Deprecated in the Vulkan spec, use ApiVersionMinor instead")]
		public static uint VersionMinor(uint version) => (version >> 12) & 0x3FF;
		[Obsolete("Deprecated in the Vulkan spec, use ApiVersionPatch instead")]
		public static uint VersionPatch(uint version) => version & 0xFFF;

		public static uint MakeApiVersion(uint variant, uint major, uint minor, uint patch) => (variant << 29) | (major << 22) | (minor << 12) | patch;
		public static uint ApiVersionVariant(uint version) => version >> 29;
		public static uint ApiVersionMajor(uint version) => (version >> 22) & 0x7F;
		public static uint ApiVersionMinor(uint version) => (version >> 12) & 0x3FF;
		public static uint ApiVersionPatch(uint version) => version & 0xFFF;

		public static readonly uint ApiVersion = MakeApiVersion(0, 1, 0, 0);

		public const float LodClampNone = 1000.0f;
		public const uint RemainingMipLevels = ~0U;
		public const uint RemainingArrayLayers = ~0U;
		public const ulong WholeSize = ~0UL;
		public const uint AttachmentUnused = ~0U;
		public const uint QueueFamilyIgnored = ~0U;
		public const uint SubpassExternal = ~0U;
		public const int MaxPhysicalDeviceNameSize = 256;
		public const int UUIDSize = 16;
		public const int MaxMemoryTypes = 32;
		public const int MaxMemoryHeaps = 16;
		public const int MaxExtensionNameSize = 256;
		public const int MaxDescriptionSize = 256;

		public VK VK { get; }
		public VK10Functions Functions { get; } = new();

		public VK10(VK vk) {
			VK = vk;
			Library.LoadFunctions(name => vk.InstanceGetProcAddr(IntPtr.Zero, name), Functions);
		}

		public VKExtensionProperties[] InstanceExtensionProperties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => EnumerateInstanceExtensionProperties(null);
		}

		public VKExtensionProperties[] EnumerateInstanceExtensionProperties(string? layerName) {
			uint propCount = 0;
			VK.CheckError(Functions.vkEnumerateInstanceExtensionProperties(layerName, ref propCount, IntPtr.Zero), "Failed to enumerate instance extension properties");
			using ManagedPointer<VKExtensionProperties> pProps = new((int)propCount);
			VK.CheckError(Functions.vkEnumerateInstanceExtensionProperties(layerName, ref propCount, pProps), "Failed to enumerate instance extension properties");
			VKExtensionProperties[] props = new VKExtensionProperties[propCount];
			for (int i = 0; i < propCount; i++) props[i] = pProps[i];
			return props;
		}

		public VKLayerProperties[] InstanceLayerProperties {
			get {
				uint propCount = 0;
				VK.CheckError(Functions.vkEnumerateInstanceLayerProperties(ref propCount, IntPtr.Zero), "Failed to enumerate instance layers");
				using ManagedPointer<VKLayerProperties> pProps = new((int)propCount);
				VK.CheckError(Functions.vkEnumerateInstanceLayerProperties(ref propCount, pProps));
				VKLayerProperties[] props = new VKLayerProperties[propCount];
				for (int i = 0; i < propCount; i++) props[i] = pProps[i];
				return props;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKInstance CreateInstance(in VKInstanceCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(Functions.vkCreateInstance(createInfo, allocator, out IntPtr pInstance));
			return new VKInstance(VK, pInstance, createInfo, allocator);
		}

	}
}
