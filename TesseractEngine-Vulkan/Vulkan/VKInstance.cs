using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public class VKInstance : IDisposable, IVKAllocatedObject, IPrimitiveHandle<IntPtr> {

		public VK VK { get; }

		public uint APIVersion { get; }

		[NativeType("VkInstance")]
		public IntPtr Instance { get; }

		public IntPtr PrimitiveHandle => Instance;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VK10InstanceFunctions VK10Functions { get; } = new();
		public VK11InstanceFunctions? VK11Functions { get; }

		public KHRSurfaceInstanceFunctions? KHRSurfaceFunctions { get; }
		public KHRDeviceGroupCreationInstanceFunctions? KHRDeviceGroupCreationFunctions { get; }
		public KHRGetPhysicalDeviceProperties2InstanceFunctions? KHRGetPhysicalDeviceProperties2Functions { get; }
		public KHRExternalFenceCapabilitiesInstanceFunctions? KHRExternalFenceCapabilitiesFunctions { get; }
		public KHRExternalMemoryCapabilitiesInstanceFunctions? KHRExternalMemoryCapabilitiesFunctions { get; }
		public KHRExternalSemaphoreCapabilitiesInstanceFunctions? KHRExternalSemaphoreCapabilitiesFunctions { get; }

		public EXTDebugReportInstanceFunctions? EXTDebugReportFunctions { get; }
		public EXTDebugUtilsInstanceFunctions? EXTDebugUtilsFunctions { get; }

		public VKGetInstanceProcAddr InstanceGetProcAddr {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => VK.InstanceGetProcAddr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetProcAddr(string name) => InstanceGetProcAddr(Instance, name);

		public VKInstance(VK vk, IntPtr pInstance, in VKInstanceCreateInfo createInfo, VulkanAllocationCallbacks? allocator) {
			VK = vk;
			// Ugly, but a workaround for pointer-based struct field
			APIVersion = new ManagedPointer<VKApplicationInfo>(createInfo.ApplicationInfo).Value.APIVersion;
			Instance = pInstance;
			Allocator = allocator;

			// Always load Vulkan 1.0 functions
			Library.LoadFunctions(GetProcAddr, VK10Functions);
			// If newer versions are available load them too
			if (APIVersion >= VK11.ApiVersion) Library.LoadFunctions(GetProcAddr, VK11Functions = new());
			
			// A bit ugly to convert back from strings provided in create info but simplifies parameter passing
			UnmanagedPointer<IntPtr> pExts = new(createInfo.EnabledExtensionNames);
			HashSet<string> exts = new();
			for (int i = 0; i < createInfo.EnabledExtensionCount; i++) exts.Add(MemoryUtil.GetASCII(pExts[i])!);

			// Load instance extensions
			// Vulkan 1.1
			if (exts.Contains(KHRDeviceGroupCreation.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRDeviceGroupCreationFunctions = new());
			if (exts.Contains(KHRExternalFenceCapabilities.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRExternalFenceCapabilitiesFunctions = new());
			if (exts.Contains(KHRExternalMemoryCapabilities.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRExternalMemoryCapabilitiesFunctions = new());
			if (exts.Contains(KHRExternalSemaphoreCapabilities.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRExternalSemaphoreCapabilitiesFunctions = new());
			if (exts.Contains(KHRGetPhysicalDeviceProperties2.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRGetPhysicalDeviceProperties2Functions = new());

			if (exts.Contains(KHRSurface.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRSurfaceFunctions = new());

			if (exts.Contains(EXTDebugReport.ExtensionName)) Library.LoadFunctions(GetProcAddr, EXTDebugReportFunctions = new());
		}

		public VKPhysicalDevice[] PhysicalDevices {
			get {
				unsafe {
					uint count = 0;
					VK.CheckError(VK10Functions.vkEnumeratePhysicalDevices(Instance, ref count, (IntPtr*)0));
					Span<IntPtr> devs = stackalloc IntPtr[(int)count];
					unsafe {
						fixed (IntPtr* pDevs = devs) {
							VK.CheckError(VK10Functions.vkEnumeratePhysicalDevices(Instance, ref count, pDevs));
						}
					}
					VKPhysicalDevice[] devices = new VKPhysicalDevice[count];
					for (int i = 0; i < count; i++) devices[i] = new VKPhysicalDevice(this, devs[i]);
					return devices;
				}
			}
		}

		// Vulkan 1.1

		public VKPhysicalDeviceGroupProperties[] PhysicalDeviceGroups {
			get {
				unsafe {
					var vkEnumeratePhysicalDeviceGroups = VK11Functions != null ? VK11Functions.vkEnumeratePhysicalDeviceGroups : KHRDeviceGroupCreationFunctions!.vkEnumeratePhysicalDeviceGroupsKHR;
					uint count = 0;
					VK.CheckError(vkEnumeratePhysicalDeviceGroups(Instance, ref count, (VKPhysicalDeviceGroupProperties*)0));
					VKPhysicalDeviceGroupProperties[] groups = new VKPhysicalDeviceGroupProperties[count];
					fixed(VKPhysicalDeviceGroupProperties* pGroups = groups) {
						VK.CheckError(vkEnumeratePhysicalDeviceGroups(Instance, ref count, pGroups));
					}
					return groups;
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				VK10Functions.vkDestroyInstance(Instance, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKInstance instance) => instance != null ? instance.Instance : IntPtr.Zero;

		// EXT_debug_report

		public VKDebugReportCallbackEXT CreateDebugReportCallbackEXT(in VKDebugReportCallbackCreateInfoEXT createInfo, VulkanAllocationCallbacks? allocator = null) {
			unsafe {
				VK.CheckError(EXTDebugReportFunctions!.vkCreateDebugReportCallbackEXT(Instance, createInfo, allocator, out ulong callback));
				return new VKDebugReportCallbackEXT(this, callback, allocator);
			}
		}

		public void DebugReportMessageEXT(VKDebugReportFlagBitsEXT flags, VKDebugReportObjectTypeEXT objectType, ulong obj, nuint location, int messageCode, string layerPrefix, string message) {
			unsafe {
				Span<byte> strLayerPrefix = MemoryUtil.StackallocUTF8(layerPrefix, stackalloc byte[256]);
				Span<byte> strMessage = MemoryUtil.StackallocUTF8(message, stackalloc byte[4096]);
				fixed(byte* pLayerPrefix = strLayerPrefix) {
					fixed(byte* pMessage = strMessage) {
						EXTDebugReportFunctions!.vkDebugReportMessageEXT(Instance, flags, objectType, obj, location, messageCode, (IntPtr)pLayerPrefix, (IntPtr)pMessage);
					}
				}
			}
		}

		// EXT_debug_utils

		public VKDebugUtilsMessengerEXT CreateDebugUtilsMessengerEXT(in VKDebugUtilsMessengerCreateInfoEXT createInfo, VulkanAllocationCallbacks? allocator = null) {
			unsafe {
				VK.CheckError(EXTDebugUtilsFunctions!.vkCreateDebugUtilsMessengerEXT(Instance, createInfo, allocator, out ulong messenger));
				return new VKDebugUtilsMessengerEXT(this, messenger, allocator);
			}
		}

		public void SubmitDebugUtilsMessageEXT(VKDebugUtilsMessageSeverityFlagBigsEXT messageSeverity, VKDebugUtilsMessageTypeFlagBitsEXT messageTypes, in VKDebugUtilsMessengerCallbackDataEXT callbackData) {
			unsafe {
				EXTDebugUtilsFunctions!.vkSubmitDebugUtilsMessageEXT(Instance, messageSeverity, messageTypes, callbackData);
			}
		}
	}

}
