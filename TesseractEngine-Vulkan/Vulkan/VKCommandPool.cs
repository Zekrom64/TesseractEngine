using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKCommandPool : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkCommandPool")]
		public ulong CommandPool { get; }

		public ulong PrimitiveHandle => CommandPool;

		public VulkanAllocationCallbacks Allocator { get; }

		public VKCommandPool(VKDevice device, ulong commandPool, VulkanAllocationCallbacks allocator) {
			Device = device;
			CommandPool = commandPool;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyCommandPool(Device, CommandPool, Allocator);
		}

		// Vulkan 1.0

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset(VKCommandPoolResetFlagBits flags) =>
			VK.CheckError(Device.VK10Functions.vkResetCommandPool(Device, CommandPool, flags), "Failed to reset command pool");

		public VKCommandBuffer[] Allocate(in VKCommandBufferAllocateInfo allocateInfo) {
			Span<IntPtr> commandBuffers = stackalloc IntPtr[(int)allocateInfo.CommandBufferCount];
			unsafe {
				fixed(IntPtr* pCommandBuffers = commandBuffers) {
					VK.CheckError(Device.VK10Functions.vkAllocateCommandBuffers(Device, allocateInfo, (IntPtr)pCommandBuffers), "Failed to allocate command buffers");
				}
			}
			VKCommandBuffer[] cmdbufs = new VKCommandBuffer[commandBuffers.Length];
			for (int i = 0; i < commandBuffers.Length; i++) cmdbufs[i] = new VKCommandBuffer(this, commandBuffers[i]);
			return cmdbufs;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Finalization done in bulk during freeing")]
		public void Free(params VKCommandBuffer[] commandBuffers) {
			Span<IntPtr> cmdbufs = stackalloc IntPtr[commandBuffers.Length];
			for (int i = 0; i < cmdbufs.Length; i++) {
				var cmdbuf = commandBuffers[i];
				GC.SuppressFinalize(cmdbuf);
				cmdbufs[i] = cmdbuf;
			}
			unsafe {
				fixed(IntPtr* pCmdbufs = cmdbufs) {
					Device.VK10Functions.vkFreeCommandBuffers(Device, CommandPool, (uint)cmdbufs.Length, (IntPtr)pCmdbufs);
				}
			}
		}
		
		// Vulkan 1.1
		// VK_KHR_maintenance1

		public void Trim(VKCommandPoolTrimFlags flags = 0) {
			if (Device.VK11Functions) Device.VK11Functions.vkTrimCommandPool(Device, CommandPool, flags);
			else Device.KHRMaintenance1.vkTrimCommandPoolKHR(Device, CommandPool, flags);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKCommandPool commandPool) => commandPool != null ? commandPool.CommandPool : 0;

	}

}
