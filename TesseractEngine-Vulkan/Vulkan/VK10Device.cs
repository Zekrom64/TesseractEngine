using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public class VK10Device {

		public IntPtr Device { get; }

		public VK10DeviceFunctions Functions { get; }

		public void ResetFences(params VKFence[] fences) {
			using MemoryStack sp = MemoryStack.Push();
			foreach (VKFence fence in fences) sp.Alloc(fence.Fence);
			VKResult err = Functions.vkResetFences(Device, (uint)fences.Length, sp.Base + sp.Pointer);
			if (err != VKResult.Success) throw new VulkanException("Failed to reset fences: ", err);
		}

		public bool WaitForFences(bool waitAll, ulong timeout, params VKFence[] fences) {
			using MemoryStack sp = MemoryStack.Push();
			foreach (VKFence fence in fences) sp.Alloc(fence.Fence);
			VKResult err = Functions.vkWaitForFences(Device, (uint)fences.Length, sp.Base + sp.Pointer, waitAll, timeout);
			return err switch {
				VKResult.Success => true,
				VKResult.Timeout => false,
				_ => throw new VulkanException("Failed to wait for fences", err)
			};
		}

	}

}
