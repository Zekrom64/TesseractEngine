using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKQueryPool : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkQueryPool")]
		public ulong QueryPool { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKQueryPool(VKDevice device, ulong queryPool, VulkanAllocationCallbacks allocator) {
			Device = device;
			QueryPool = queryPool;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyQueryPool(Device, QueryPool, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetResults<T>(uint firstQuery, uint queryCount, ulong stride, VKQueryResultFlagBits flags, Span<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					VK.CheckError(Device.VK10Functions.vkGetQueryPoolResults(Device, QueryPool, firstQuery, queryCount, (nuint)(sizeof(T) * data.Length), (IntPtr)pData, stride, flags));
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKQueryPool queryPool) => queryPool != null ? queryPool.QueryPool : 0;

	}
}
