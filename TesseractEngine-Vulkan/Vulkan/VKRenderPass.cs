using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.Core.Numerics;

namespace Tesseract.Vulkan {

	public class VKRenderPass : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.RenderPass;

		public VKDevice Device { get; }

		[NativeType("VkRenderPass")]
		public ulong RenderPass { get; }

		public ulong PrimitiveHandle => RenderPass;

		public VulkanAllocationCallbacks? Allocator { get; }

		public Vector2ui RenderAreaGranularity {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					Device.VK10Functions.vkGetRenderAreaGranularity(Device, RenderPass, out Vector2ui granularity);
					return granularity;
				}
			}
		}

		public VKRenderPass(VKDevice device, ulong renderPass, VulkanAllocationCallbacks? allocator) {
			Device = device;
			RenderPass = renderPass;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyRenderPass(Device, RenderPass, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKRenderPass? renderPass) => renderPass != null ? renderPass.RenderPass : 0;

	}

}
