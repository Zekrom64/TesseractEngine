using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKPipelineLayout : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.PipelineLayout;

		public VKDevice Device { get; }

		[NativeType("VkPipelineLayout")]
		public ulong PipelineLayout { get; }

		public ulong PrimitiveHandle => PipelineLayout;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKPipelineLayout(VKDevice device, ulong pipelineLayout, VulkanAllocationCallbacks? allocator) {
			Device = device;
			PipelineLayout = pipelineLayout;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyPipelineLayout(Device, PipelineLayout, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKPipelineLayout? layout) => layout != null ? layout.PipelineLayout : 0;

	}

}
