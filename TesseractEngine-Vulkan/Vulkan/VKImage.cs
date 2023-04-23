using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKImage : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.Image;

		public VKDevice Device { get; }

		[NativeType("VkImage")]
		public ulong Image { get; }

		public ulong PrimitiveHandle => Image;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKMemoryRequirements MemoryRequirements {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					Device.VK10Functions.vkGetImageMemoryRequirements(Device, Image, out VKMemoryRequirements requirements);
					return requirements;
				}
			}
		}

		public VKSparseImageMemoryRequirements[] SparseMemoryRequirements {
			get {
				unsafe {
					uint count = 0;
					Device.VK10Functions.vkGetImageSparseMemoryRequirements(Device, Image, ref count, (VKSparseImageMemoryRequirements*)0);
					VKSparseImageMemoryRequirements[] reqs = new VKSparseImageMemoryRequirements[count];
					fixed (VKSparseImageMemoryRequirements* pReqs = reqs) {
						Device.VK10Functions.vkGetImageSparseMemoryRequirements(Device, Image, ref count, pReqs);
					}
					return reqs;
				}
			}
		}

		public VKImage(VKDevice device, ulong image, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Image = image;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyImage(Device, Image, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindMemory(VKDeviceMemory memory, ulong memoryOffset) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkBindImageMemory(Device, Image, memory, memoryOffset), "Failed to bind image memory");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSubresourceLayout GetSubresourceLayout(VKImageSubresource subresource) {
			unsafe {
				Device.VK10Functions.vkGetImageSubresourceLayout(Device, Image, subresource, out VKSubresourceLayout layout);
				return layout;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKImage? image) => image != null ? image.Image : 0;

	}

}
