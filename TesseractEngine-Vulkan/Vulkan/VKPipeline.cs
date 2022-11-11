using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKPipeline : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.Pipeline;

		public VKDevice Device { get; }

		[NativeType("VkPipeline")]
		public ulong Pipeline { get; }

		public ulong PrimitiveHandle => Pipeline;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKPipeline(VKDevice device, ulong pipeline, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Pipeline = pipeline;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyPipeline(Device, Pipeline, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKPipeline? pipeline) => pipeline != null ? pipeline.Pipeline : 0;

	}

}
