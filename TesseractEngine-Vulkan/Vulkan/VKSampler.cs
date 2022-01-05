using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKSampler : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkSampler")]
		public ulong Sampler { get; }

		public ulong PrimitiveHandle => Sampler;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKSampler(VKDevice device, ulong sampler, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Sampler = sampler;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroySampler(Device, Sampler, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKSampler? sampler) => sampler != null ? sampler.Sampler : 0;

	}

}
