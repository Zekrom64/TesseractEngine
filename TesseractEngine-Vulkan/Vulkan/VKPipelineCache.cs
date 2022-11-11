using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKPipelineCache : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.PipelineCache;

		public VKDevice Device { get; }

		[NativeType("VkPipelineCache")]
		public ulong PipelineCache { get; }

		public ulong PrimitiveHandle => PipelineCache;

		public VulkanAllocationCallbacks? Allocator { get; }

		public byte[] Data {
			get {
				nuint size = 0;
				VK.CheckError(Device.VK10Functions.vkGetPipelineCacheData(Device, PipelineCache, ref size, IntPtr.Zero), "Failed to get pipeline cache data");
				byte[] data = new byte[size];
				unsafe {
					fixed(byte* pData = data) {
						VK.CheckError(Device.VK10Functions.vkGetPipelineCacheData(Device, PipelineCache, ref size, (IntPtr)pData), "Failed to get pipeline cache data");
					}
				}
				return data;
			}
		}

		public VKPipelineCache(VKDevice device, ulong pipelineCache, VulkanAllocationCallbacks? allocator) {
			Device = device;
			PipelineCache = pipelineCache;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyPipelineCache(Device, PipelineCache, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Merge([NativeType("VkPipelineCache[]")] in ReadOnlySpan<ulong> caches) {
			unsafe {
				fixed(ulong* pCaches = caches) {
					VK.CheckError(Device.VK10Functions.vkMergePipelineCaches(Device, PipelineCache, (uint)caches.Length, (IntPtr)pCaches), "Failed to merge pipeline caches");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Merge([NativeType("VkPipelineCache[]")] params ulong[] caches) {
			unsafe {
				fixed(ulong* pCaches = caches) {
					VK.CheckError(Device.VK10Functions.vkMergePipelineCaches(Device, PipelineCache, (uint)caches.Length, (IntPtr)pCaches), "Failed to merge pipeline caches");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKPipelineCache? pipelineCache) => pipelineCache != null ? pipelineCache.PipelineCache : 0;

	}

}
