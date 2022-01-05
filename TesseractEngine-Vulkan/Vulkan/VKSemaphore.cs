using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKSemaphore : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkSemaphore")]
		public ulong Semaphore { get; }

		public VulkanAllocationCallbacks? Allocator { get; }

		public ulong PrimitiveHandle => Semaphore;

		public ulong CounterValue {
			get {
				var vkGetSemaphoreCounterValue = Device.VK12Functions?.vkGetSemaphoreCounterValue;
				if (vkGetSemaphoreCounterValue == null) vkGetSemaphoreCounterValue = new(Device.KHRTimelineSemaphore!.vkGetSemaphoreCounterValueKHR);
				VK.CheckError(vkGetSemaphoreCounterValue(Device, Semaphore, out ulong value));
				return value;
			}
		}

		public VKSemaphore(VKDevice device, ulong semaphore, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Semaphore = semaphore;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroySemaphore(Device, Semaphore, Allocator);
		}

		public void Signal(ulong value) {
			var signalInfo = new VKSemaphoreSignalInfo() {
				Type = VKStructureType.SEMAPHORE_SIGNAL_INFO,
				Semaphore = Semaphore,
				Value = value
			};
			VKResult err;
			if (Device.VK12Functions) err = Device.VK12Functions!.vkSignalSemaphore(Device, signalInfo);
			else err = Device.KHRTimelineSemaphore!.vkSignalSemaphoreKHR(Device, signalInfo);
			VK.CheckError(err);
		}

		public VKResult Wait(ulong value, ulong timeout) {
			ulong semaphore = Semaphore;
			unsafe {
				var waitInfo = new VKSemaphoreWaitInfo() {
					Type = VKStructureType.SEMAPHORE_WAIT_INFO,
					SemaphoreCount = 1,
					Semaphores = (IntPtr)(&semaphore),
					Values = (IntPtr)(&value)
				};
				VKResult err;
				if (Device.VK12Functions) err = Device.VK12Functions!.vkWaitSemaphores(Device, waitInfo, timeout);
				else err = Device.KHRTimelineSemaphore!.vkWaitSemaphoresKHR(Device, waitInfo, timeout);
				switch(err) {
					case VKResult.Success:
					case VKResult.Timeout:
						break;
					default:
						VK.CheckError(err);
						break;
				}
				return err;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKSemaphore? semaphore) => semaphore != null ? semaphore.Semaphore : 0;

	}

}
