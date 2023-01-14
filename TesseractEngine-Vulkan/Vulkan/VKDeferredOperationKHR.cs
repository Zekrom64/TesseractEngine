using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	public class VKDeferredOperationKHR : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public ulong DeferredOperation { get; }

		public VKObjectType ObjectType => VKObjectType.DeferredOperationKHR;

		public VKDevice Device { get; }

		public ulong PrimitiveHandle => DeferredOperation;

		public VulkanAllocationCallbacks? Allocator { get; }

		public uint MaxConcurrency => Device.KHRDeferredHostOperations!.vkGetDeferredOperationMaxConcurrencyKHR(Device, DeferredOperation);

		public VKResult Result => Device.KHRDeferredHostOperations!.vkGetDeferredOperationResultKHR(Device, DeferredOperation);


		public VKDeferredOperationKHR(VKDevice device, ulong deferredOp, VulkanAllocationCallbacks? allocator) {
			Device = device;
			DeferredOperation = deferredOp;
			Allocator = allocator;
		}

		public void Join() => VK.CheckError(Device.KHRDeferredHostOperations!.vkDeferredOperationJoinKHR(Device, DeferredOperation));

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public static implicit operator ulong(VKDeferredOperationKHR? operation) => operation != null ? operation.DeferredOperation : 0;

	}

}
