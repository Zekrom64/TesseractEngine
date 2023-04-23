using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKEvent : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.Event;

		public VKDevice Device { get; }

		[NativeType("VkEvent")]
		public ulong Event { get; }

		public ulong PrimitiveHandle => Event;

		public VulkanAllocationCallbacks? Allocator { get; }

		public bool Status {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					VKResult status = Device.VK10Functions.vkGetEventStatus(Device, Event);
					return status switch {
						VKResult.EventSet => true,
						VKResult.EventReset => false,
						_ => throw new VulkanException("Failed to get event status", status),
					};
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					if (value) VK.CheckError(Device.VK10Functions.vkSetEvent(Device, Event), "Failed to set event");
					else VK.CheckError(Device.VK10Functions.vkResetEvent(Device, Event), "Failed to reset event");
				}
			}
		}

		public VKEvent(VKDevice device, ulong _event, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Event = _event;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyEvent(Device, Event, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKEvent? evt) => evt != null ? evt.Event : 0;

	}

}
