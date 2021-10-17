using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKShaderModule : IVKDeviceObject, IVKAllocatedObject, IDisposable, IPrimitiveHandle<ulong> {

		public VKDevice Device { get; }

		[NativeType("VkShaderModule")]
		public ulong ShaderModule { get; }

		public ulong PrimitiveHandle => ShaderModule;

		public VulkanAllocationCallbacks Allocator { get; }

		public VKShaderModule(VKDevice device, ulong shaderModule, VulkanAllocationCallbacks allocator) {
			Device = device;
			ShaderModule = shaderModule;
			Allocator = allocator;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Device.VK10Functions.vkDestroyShaderModule(Device, ShaderModule, Allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ulong(VKShaderModule shaderModule) => shaderModule != null ? shaderModule.ShaderModule : 0;

	}
}
