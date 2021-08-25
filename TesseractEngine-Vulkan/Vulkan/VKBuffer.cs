using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKBuffer : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkBuffer")]
		public ulong Buffer { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKMemoryRequirements MemoryRequirements {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Device.VK10Functions.vkGetBufferMemoryRequirements(Device, Buffer, out VKMemoryRequirements requirements);
				return requirements;
			}
		}

		public VKBuffer(VKDevice device, ulong buffer, VulkanAllocationCallbacks allocator) {
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
		public static implicit operator ulong(VKBuffer buffer) => buffer != null ? buffer.Buffer : 0;

	}

}
