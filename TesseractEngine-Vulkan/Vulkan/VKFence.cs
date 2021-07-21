using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKFence : IDisposable, IVK10DeviceObject, IVKAllocatedObject {

		public VulkanAllocationCallbacks Allocator { get; }

		public VK10Device Device { get; }

		public ulong Fence { get; }

		public bool Status {
			get {
				VKResult result = Device.Functions.vkGetFenceStatus(Device.Device, Fence);
				return result switch {
					VKResult.Success => true,
					VKResult.NotReady => false,
					_ => throw new VulkanException("Failed to get fence status", result),
				};
			}
		}

		public VKFence(VK10Device device, in VKFenceCreateInfo createInfo, VulkanAllocationCallbacks allocationCallbacks = null) {
			Device = device;
			Allocator = allocationCallbacks;
			VKResult err = device.Functions.vkCreateFence(device.Device, createInfo, allocationCallbacks, out ulong fence);
			if (err != VKResult.Success) throw new VulkanException("Failed to create fence: ", err);
			Fence = fence;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.Functions.vkDestroyFence(Device.Device, Fence, Allocator);
		}

		public void Reset() {
			unsafe {
				ulong fence = Fence;
				VKResult err = Device.Functions.vkResetFences(Device.Device, 1, (IntPtr)(&fence));
				if (err != VKResult.Success) throw new VulkanException("Failed to reset fence: ", err);
			}
		}

		public bool WaitFor(ulong timeout) {
			unsafe {
				ulong fence = Fence;
				VKResult err = Device.Functions.vkWaitForFences(Device.Device, 1, (IntPtr)(&fence), true, timeout);
				return err switch {
					VKResult.Success => true,
					VKResult.Timeout => false,
					_ => throw new VulkanException("Failed to wait for fence: ", err),
				};
			}
		}

	}

}
