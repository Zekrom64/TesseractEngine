using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKSemaphore : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkSemaphore")]
		public ulong Semaphore { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKSemaphore(VKDevice device, ulong semaphore) {
			Device = device;
			Semaphore = semaphore;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroySemaphore(Device, Semaphore, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKSemaphore semaphore) => semaphore != null ? semaphore.Semaphore : 0;

	}

}
