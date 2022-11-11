using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKQueue : IVKDeviceObject, IPrimitiveHandle<IntPtr> {

		public VKObjectType ObjectType => VKObjectType.Queue;

		public VKDevice Device { get; }

		[NativeType("VkQueue")]
		public IntPtr Queue { get; }

		public IntPtr PrimitiveHandle => Queue;

		ulong IPrimitiveHandle<ulong>.PrimitiveHandle => (ulong)PrimitiveHandle;

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

		public void BindSparse(IReadOnlyCollection<VKBindSparseInfo> infos, VKFence? fence = null) {
			Span<VKBindSparseInfo> sInfos = stackalloc VKBindSparseInfo[infos.Count];
			int i = 0;
			foreach (VKBindSparseInfo info in infos) sInfos[i++] = info;
			BindSparse(sInfos, fence);
		}

		// VK_KHR_swapchain

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKResult PresentKHR(in VKPresentInfoKHR presentInfo) =>
			Device.KHRSwapchain!.vkQueuePresentKHR(Queue, presentInfo);

		// VK_EXT_debug_utils

		public void BeginDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) => Device.Instance.EXTDebugUtilsFunctions!.vkQueueBeginDebugUtilsLabelEXT(Queue, labelInfo);

		public void EndDebugUtilsLabelEXT() => Device.Instance.EXTDebugUtilsFunctions!.vkQueueEndDebugUtilsLabelEXT(Queue);

		public void InsertDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) => Device.Instance.EXTDebugUtilsFunctions!.vkQueueInsertDebugUtilsLabelEXT(Queue, labelInfo);

		// VK_KHR_synchronization2

		public void Submit2(VKSubmitInfo2 submit, VKFence? fence = null) {
			unsafe {
				Device.KHRSynchronization2!.vkQueueSubmit2(Queue, 1, (IntPtr)(&submit), fence);
			}
		}

		public void Submit2(in ReadOnlySpan<VKSubmitInfo2> submits, VKFence? fence = null) {
			unsafe {
				fixed(VKSubmitInfo2* pSubmits = submits) {
					Device.KHRSynchronization2!.vkQueueSubmit2(Queue, (uint)submits.Length, (IntPtr)pSubmits, fence);
				}
			}
		}

		public void Submit2(IReadOnlyCollection<VKSubmitInfo2> submits, VKFence? fence = null) {
			Span<VKSubmitInfo2> sSubmits = stackalloc VKSubmitInfo2[submits.Count];
			int i = 0;
			foreach (VKSubmitInfo2 submit in submits) sSubmits[i++] = submit;
			Submit2(sSubmits, fence);
		}

		public void WritePrimitiveHandle(IntPtr ptr) {
			throw new NotImplementedException();
		}
	}

}
