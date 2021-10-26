using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKDeviceMemory : IDisposable, IVKDeviceObject, IVKAllocatedObject, IEquatable<VKDeviceMemory>, IPrimitiveHandle<ulong> {

		[NativeType("VkDeviceMemory")]
		public ulong DeviceMemory;

		public VKDevice Device { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public ulong PrimitiveHandle => DeviceMemory;

		public ulong MemoryCommitment {
			get {
				Device.VK10Functions.vkGetDeviceMemoryCommitment(Device, DeviceMemory, out ulong commit);
				return commit;
			}
		}

		public ulong OpaqueCaptureAddress {
			get {
				var info = new VKDeviceMemoryOpaqueCaptureAddressInfo() {
					Type = VKStructureType.DEVICE_MEMORY_OPAQUE_CAPTURE_ADDRESS_INFO,
					Memory = DeviceMemory
				};
				if (Device.VK12Functions) return Device.VK12Functions.vkGetDeviceMemoryOpaqueCaptureAddress(Device, info);
				else return Device.KHRBufferDeviceAddress.vkGetDeviceMemoryOpaqueCaptureAddressKHR(Device, info);
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


		public override bool Equals(object obj) => obj is VKDeviceMemory mem && Equals(mem);

		public override int GetHashCode() => (int)DeviceMemory;

		public bool Equals(VKDeviceMemory mem) {
			if (mem == null) return false;
			return DeviceMemory == mem.DeviceMemory;
		}

		public static bool operator==(VKDeviceMemory m1, VKDeviceMemory m2) {
			if (Equals(m1, null) ^ Equals(m2, null)) return false;
			if (!Equals(m1, null)) return m1.Equals(m2);
			return true;
		}

		public static bool operator !=(VKDeviceMemory m1, VKDeviceMemory m2) => !(m1 == m2);

	}

}
