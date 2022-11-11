using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKBuffer : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.Buffer;

		public VKDevice Device { get; }

		[NativeType("VkBuffer")]
		public ulong Buffer { get; }

		public ulong PrimitiveHandle => Buffer;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKMemoryRequirements MemoryRequirements {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Device.VK10Functions.vkGetBufferMemoryRequirements(Device, Buffer, out VKMemoryRequirements requirements);
				return requirements;
			}
		}

		public ulong DeviceAddress {
			get {
				var info = new VKBufferDeviceAddressInfo() {
					Type = VKStructureType.BufferDeviceAddressInfo,
					Buffer = Buffer
				};
				if (Device.VK12Functions) return Device.VK12Functions!.vkGetBufferDeviceAddress(Device, info);
				else return Device.KHRBufferDeviceAddress!.vkGetBufferDeviceAddressKHR(Device, info);
			}
		}

		public ulong OpaqueCaptureAddress {
			get {
				var info = new VKBufferDeviceAddressInfo() {
					Type = VKStructureType.BufferDeviceAddressInfo,
					Buffer = Buffer
				};
				if (Device.VK12Functions) return Device.VK12Functions!.vkGetBufferOpaqueCaptureAddress(Device, info);
				else return Device.KHRBufferDeviceAddress!.vkGetBufferOpaqueCaptureAddressKHR(Device, info);
			}
		}

		public VKBuffer(VKDevice device, ulong buffer, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Buffer = buffer;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyBuffer(Device, Buffer, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindMemory(VKDeviceMemory memory, ulong memoryOffset) =>
			VK.CheckError(Device.VK10Functions.vkBindBufferMemory(Device, Buffer, memory, memoryOffset), "Failed to bind buffer memory");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKBuffer? buffer) => buffer != null ? buffer.Buffer : 0;

	}

}
