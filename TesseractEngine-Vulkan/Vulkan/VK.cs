using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Vulkan {

	public class VK {

		public static void CheckError(VKResult result) {
			if (result == VKResult.Success) return;
			else throw new VulkanException(result);
		}

		public static void CheckError(VKResult result, string msg) {
			if (result == VKResult.Success) return;
			else throw new VulkanException(msg, result);
		}

		public IVKLoader Loader { get; }

		public VK10 VK10 { get; }
		public VK11 VK11 { get; }

		public VKGetInstanceProcAddr InstanceGetProcAddr { get; }

		public uint MaxInstanceVersion { get; } = VK10.ApiVersion;

		public VK(IVKLoader loader) {
			Loader = loader;

			IntPtr pInstFunc = loader.GetVKProcAddress("vkGetInstanceProcAddr");
			// We don't have any advanced loader functions, just use the known exports from the loader library
			if (pInstFunc == IntPtr.Zero) InstanceGetProcAddr = (IntPtr _, string name) => loader.GetVKProcAddress(name);
			// Else use the provided export
			else InstanceGetProcAddr = Marshal.GetDelegateForFunctionPointer<VKGetInstanceProcAddr>(pInstFunc);
			
			VK10 = new(this);
			IntPtr pEnumerateInstanceVersion = loader.GetVKProcAddress("vkEnumerateInstanceVersion");
			if (pEnumerateInstanceVersion != IntPtr.Zero) {
				VK11 = new(this);
				MaxInstanceVersion = VK11.EnumerateInstanceVersion();
			}
		}

		// Vulkan 1.0

		public VKExtensionProperties[] InstanceExtensionProperties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => VK10.InstanceExtensionProperties;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKExtensionProperties[] EnumerateInstanceExtensionProperties(string layer) => VK10.EnumerateInstanceExtensionProperties(layer);

		public VKLayerProperties[] InstanceLayerProperties {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => VK10.InstanceLayerProperties;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKInstance CreateInstance(in VKInstanceCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) => VK10.CreateInstance(createInfo, allocator);

		// Vulkan 1.1

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint EnumerateInstanceVersion() => VK11.EnumerateInstanceVersion();

	}

}
