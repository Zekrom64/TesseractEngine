using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKQueue : IVKDeviceObject {

		public VKDevice Device { get; }

		[NativeType("VkQueue")]
		public IntPtr Queue { get; }

		public VKQueue(IntPtr queue, VKDevice device) {
			Queue = queue;
			Device = device;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Submit(VKSubmitInfo submitInfo, VKFence fence = null) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkQueueSubmit(Queue, 1, (IntPtr)(&submitInfo), fence), "Failed to submit to queue");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Submit(in ReadOnlySpan<VKSubmitInfo> infos, VKFence fence = null) {
			unsafe {
				fixed(VKSubmitInfo* pInfos = infos) {
					VK.CheckError(Device.VK10Functions.vkQueueSubmit(Queue, (uint)infos.Length, (IntPtr)pInfos, fence), "Failed to submit to queue");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitIdle() => VK.CheckError(Device.VK10Functions.vkQueueWaitIdle(Queue));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSparse(VKBindSparseInfo info, VKFence fence = null) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkQueueBindSparse(Queue, 1, (IntPtr)(&info), fence), "Failed to bind sparse memory");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSparse(in ReadOnlySpan<VKBindSparseInfo> infos, VKFence fence = null) {
			unsafe {
				fixed(VKBindSparseInfo* pInfos = infos) {
					VK.CheckError(Device.VK10Functions.vkQueueBindSparse(Queue, (uint)infos.Length, (IntPtr)pInfos, fence != null ? fence.Fence : 0), "Failed to bind sparse memory");
				}
			}
		}

	}

}
