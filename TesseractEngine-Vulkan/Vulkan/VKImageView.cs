using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKImageView : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkImageView")]
		public ulong ImageView { get; }

		public ulong PrimitiveHandle => ImageView;

		public VulkanAllocationCallbacks Allocator { get; }

		public VKImageView(VKDevice device, ulong imageView, VulkanAllocationCallbacks allocator) {
			Device = device;
			ImageView = imageView;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyImageView(Device, ImageView, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKImageView imageView) => imageView != null ? imageView.ImageView : 0;

	}

}
