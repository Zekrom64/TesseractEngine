using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKAccelerationStructureKHR : IDisposable, IVKDeviceObject, IVKAllocatedObject, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.AccelerationStructureKHR;

		public VKDevice Device { get; }

		public ulong PrimitiveHandle => AccelerationStructure;

		public ulong AccelerationStructure { get; }

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKAccelerationStructureKHR(ulong accelerationStructure, VKDevice device, VulkanAllocationCallbacks? allocator = null) {
			AccelerationStructure = accelerationStructure;
			Device = device;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.KHRAccelerationStructure!.vkDestroyAccelerationStructureKHR(Device, AccelerationStructure, Allocator);
		}

		public static implicit operator ulong(VKAccelerationStructureKHR o) => o.AccelerationStructure;

	}

}
