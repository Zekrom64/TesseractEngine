using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDeviceMemory : IDisposable, IVKDeviceObject, IVKAllocatedObject {

		[NativeType("VkDeviceMemory")]
		public ulong DeviceMemory;

		public VKDevice Device { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public ulong MemoryCommitment {
			get {
				Device.VK10Functions.vkGetDeviceMemoryCommitment(Device, DeviceMemory, out ulong commit);
				return commit;
			}
		}

		public VKDeviceMemory(VKDevice device, ulong deviceMemory, VulkanAllocationCallbacks allocator) {
			Device = device;
			DeviceMemory = deviceMemory;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkFreeMemory(Device, DeviceMemory, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapMemory(ulong offset, ulong size, VKMemoryMapFlagBits flags) {
			VK.CheckError(Device.VK10Functions.vkMapMemory(Device, DeviceMemory, offset, size, flags, out IntPtr data));
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UnmapMemory() => Device.VK10Functions.vkUnmapMemory(Device, DeviceMemory);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKDeviceMemory memory) => memory != null ? memory.DeviceMemory : 0;

	}

}
