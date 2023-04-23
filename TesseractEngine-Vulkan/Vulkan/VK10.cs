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
			unsafe {
				uint propCount = 0;
				Span<byte> strLayerName = layerName != null ? MemoryUtil.StackallocUTF8(layerName, stackalloc byte[1024]) : Span<byte>.Empty;
				fixed(byte* pStrLayerName = strLayerName) {
					IntPtr pLayerName = layerName != null ? (IntPtr)pStrLayerName : IntPtr.Zero;
					VK.CheckError(Functions.vkEnumerateInstanceExtensionProperties(pLayerName, ref propCount, (VKExtensionProperties*)0), "Failed to enumerate instance extension properties");
					VKExtensionProperties[] properties = new VKExtensionProperties[propCount];
					fixed (VKExtensionProperties* pProperties = properties) {
						VK.CheckError(Functions.vkEnumerateInstanceExtensionProperties(pLayerName, ref propCount, pProperties), "Failed to enumerate instance extension properties");
					}
					return properties;
				}
			}
		}

		public VKLayerProperties[] InstanceLayerProperties {
			get {
				uint propCount = 0;
				unsafe {
					VK.CheckError(Functions.vkEnumerateInstanceLayerProperties(ref propCount, (VKLayerProperties*)0), "Failed to enumerate instance layers");
					VKLayerProperties[] properties = new VKLayerProperties[propCount];
					fixed (VKLayerProperties* pProperties = properties) {
						VK.CheckError(Functions.vkEnumerateInstanceLayerProperties(ref propCount, pProperties));
					}
					return properties;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKInstance CreateInstance(in VKInstanceCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			unsafe {
				VK.CheckError(Functions.vkCreateInstance(createInfo, allocator, out IntPtr pInstance));
				return new VKInstance(VK, pInstance, createInfo, allocator);
			}
		}

	}
}
