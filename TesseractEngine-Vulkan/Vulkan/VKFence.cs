﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKFence : IDisposable, IVKDeviceObject, IVKAllocatedObject, IPrimitiveHandle<ulong> {

		public VKObjectType ObjectType => VKObjectType.Fence;

		public VulkanAllocationCallbacks? Allocator { get; }

		public VKDevice Device { get; }

		[NativeType("VkFence")]
		public ulong Fence { get; }

		public ulong PrimitiveHandle => Fence;

		public bool Status {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					VKResult result = Device.VK10Functions.vkGetFenceStatus(Device, Fence);
					return result switch {
						VKResult.Success => true,
						VKResult.NotReady => false,
						_ => throw new VulkanException("Failed to get fence status", result),
					};
				}
			}
		}

		public VKFence(VKDevice device, ulong fence, VulkanAllocationCallbacks? allocator) {
			Device = device;
			Fence = fence;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				Device.VK10Functions.vkDestroyFence(Device, Fence, Allocator);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset() {
			unsafe {
				ulong fence = Fence;
				VK.CheckError(Device.VK10Functions.vkResetFences(Device, 1, &fence));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool WaitFor(ulong timeout) {
			unsafe {
				unsafe {
					ulong fence = Fence;
					VKResult err = Device.VK10Functions.vkWaitForFences(Device, 1, &fence, true, timeout);
					return err switch {
						VKResult.Success => true,
						VKResult.Timeout => false,
						_ => throw new VulkanException("Failed to wait for fence: ", err),
					};
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKFence? fence) => fence != null ? fence.Fence : 0;

	}

}
