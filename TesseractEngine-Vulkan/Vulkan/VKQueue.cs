using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKQueue : IVKDeviceObject, IPrimitiveHandle<IntPtr> {

		public VKDevice Device { get; }

		[NativeType("VkQueue")]
		public IntPtr Queue { get; }

		public IntPtr PrimitiveHandle => Queue;

		public VKQueue(IntPtr queue, VKDevice device) {
			Queue = queue;
			Device = device;
		}

		// Vulkan 1.0

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Submit(VKSubmitInfo submitInfo, VKFence? fence = null) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkQueueSubmit(Queue, 1, (IntPtr)(&submitInfo), fence), "Failed to submit to queue");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Submit(in ReadOnlySpan<VKSubmitInfo> infos, VKFence? fence = null) {
			unsafe {
				fixed(VKSubmitInfo* pInfos = infos) {
					VK.CheckError(Device.VK10Functions.vkQueueSubmit(Queue, (uint)infos.Length, (IntPtr)pInfos, fence), "Failed to submit to queue");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitIdle() => VK.CheckError(Device.VK10Functions.vkQueueWaitIdle(Queue));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSparse(VKBindSparseInfo info, VKFence? fence = null) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkQueueBindSparse(Queue, 1, (IntPtr)(&info), fence), "Failed to bind sparse memory");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSparse(in ReadOnlySpan<VKBindSparseInfo> infos, VKFence? fence = null) {
			unsafe {
				fixed(VKBindSparseInfo* pInfos = infos) {
					VK.CheckError(Device.VK10Functions.vkQueueBindSparse(Queue, (uint)infos.Length, (IntPtr)pInfos, fence != null ? fence.Fence : 0), "Failed to bind sparse memory");
				}
			}
		}

		// VK_KHR_swapchain

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKResult PresentKHR(in VKPresentInfoKHR presentInfo) =>
			Device.KHRSwapchain!.vkQueuePresentKHR(Queue, presentInfo);

		// VK_EXT_debug_utils

		public void BeginDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) => Device.Instance.EXTDebugUtilsFunctions!.vkQueueBeginDebugUtilsLabelEXT(Queue, labelInfo);

		public void EndDebugUtilsLabelEXT() => Device.Instance.EXTDebugUtilsFunctions!.vkQueueEndDebugUtilsLabelEXT(Queue);

		public void InsertDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) => Device.Instance.EXTDebugUtilsFunctions!.vkQueueInsertDebugUtilsLabelEXT(Queue, labelInfo);

	}

}
