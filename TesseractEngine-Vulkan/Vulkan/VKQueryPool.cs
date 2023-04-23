using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKQueryPool : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.QueryPool;

		public VKDevice Device { get; }

		[NativeType("VkQueryPool")]
		public ulong QueryPool { get; }

		public ulong PrimitiveHandle => QueryPool;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKQueryPool(VKDevice device, ulong queryPool, VulkanAllocationCallbacks? allocator) {
			Device = device;
			QueryPool = queryPool;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyQueryPool(Device, QueryPool, Allocator);
			}
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

		// Vulkan 1.2
		// VK_EXT_host_query_reset
		public void Reset(uint firstQuery, uint queryCount) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkResetQueryPool(Device, QueryPool, firstQuery, queryCount);
				else Device.EXTHostQueryReset!.vkResetQueryPoolEXT(Device, QueryPool, firstQuery, queryCount);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKQueryPool? queryPool) => queryPool != null ? queryPool.QueryPool : 0;

	}
}
