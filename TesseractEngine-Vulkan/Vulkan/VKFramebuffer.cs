﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKFramebuffer : IVKDeviceObject, IVKAllocatedObject, IDisposable {

		public VKDevice Device { get; }

		[NativeType("VkFramebuffer")]
		public ulong Framebuffer { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VKFramebuffer(VKDevice device, ulong framebuffer, VulkanAllocationCallbacks allocator) {
			Device = device;
			Framebuffer = framebuffer;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyFramebuffer(Device, Framebuffer, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKFramebuffer framebuffer) => framebuffer != null ? framebuffer.Framebuffer : 0;

	}

}