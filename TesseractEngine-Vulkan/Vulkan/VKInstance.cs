using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public class VKInstance : IDisposable, IVKAllocatedObject {

		public VK VK { get; }

		public uint APIVersion { get; }

		[NativeType("VkInstance")]
		public IntPtr Instance { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VK10InstanceFunctions VK10Functions { get; } = new();
		public VK11InstanceFunctions VK11Functions { get; }

		public VKGetInstanceProcAddr InstanceGetProcAddr {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => VK.InstanceGetProcAddr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetProcAddr(string name) => InstanceGetProcAddr(Instance, name);

		public VKInstance(VK vk, IntPtr pInstance, in VKInstanceCreateInfo createInfo, VulkanAllocationCallbacks allocator) {
			VK = vk;
			// Ugly, but a workaround for pointer-based struct field
			APIVersion = new UnmanagedPointer<VKApplicationInfo>(createInfo.ApplicationInfo).Value.APIVersion;
			Instance = pInstance;
			Allocator = allocator;

			// Always load Vulkan 1.0 functions
			Library.LoadFunctions(GetProcAddr, VK10Functions);
			// If newer versions are available load them too
			if (APIVersion >= VK11.ApiVersion) Library.LoadFunctions(GetProcAddr, VK11Functions = new());
			
			// A bit ugly to convert back from strings provided in create info but simplifies parameter passing
			UnmanagedPointer<IntPtr> pExts = new(createInfo.EnabledExtensionNames);
			HashSet<string> exts = new();
			for (int i = 0; i < createInfo.EnabledExtensionCount; i++) exts.Add(MemoryUtil.GetStringASCII(pExts[i]));

		}

		public VKPhysicalDevice[] PhysicalDevices {
			get {
				uint count = 0;
				VK.CheckError(VK10Functions.vkEnumeratePhysicalDevices(Instance, ref count, IntPtr.Zero));
				Span<IntPtr> devs = stackalloc IntPtr[(int)count];
				unsafe {
					fixed(IntPtr* pDevs = devs) {
						VK.CheckError(VK10Functions.vkEnumeratePhysicalDevices(Instance, ref count, (IntPtr)pDevs));
					}
				}
				VKPhysicalDevice[] devices = new VKPhysicalDevice[count];
				for (int i = 0; i < count; i++) devices[i] = new VKPhysicalDevice(this, devs[i]);
				return devices;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			VK10Functions.vkDestroyInstance(Instance, Allocator != null ? Allocator.Pointer.Ptr : IntPtr.Zero);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKInstance instance) => instance != null ? instance.Instance : IntPtr.Zero;

	}

}
