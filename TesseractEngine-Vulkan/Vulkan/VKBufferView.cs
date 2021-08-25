using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKBufferView : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkBufferView")]
		public ulong BufferView { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKBufferView(VKDevice device, ulong bufferView, VulkanAllocationCallbacks allocator) {
			Device = device;
			BufferView = bufferView;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyBufferView(Device, BufferView, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKBufferView bufferView) => bufferView != null ? bufferView.BufferView : 0;

	}

}
