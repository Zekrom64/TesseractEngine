using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public class VKDevice : IDisposable, IVKInstanceObject, IVKAllocatedObject {

		public VKInstance Instance { get; }

		[NativeType("VkDevice")]
		public IntPtr Device { get; }

		public VulkanAllocationCallbacks Allocator { get; }

		public VK10DeviceFunctions VK10Functions { get; } = new();
		public VK11DeviceFunctions VK11Functions { get; }
		public VK12DeviceFunctions VK12Functions { get; }

		public KHRSwapchainDeviceFunctions KHRSwapchainFunctions { get; }

		public VKGetDeviceProcAddr DeviceGetProcAddress;

		public IntPtr GetProcAddr(string name) => DeviceGetProcAddress(Device, name);

		public VKDevice(VKInstance instance, IntPtr device, in VKDeviceCreateInfo createInfo, VulkanAllocationCallbacks allocator) {
			Instance = instance;
			Device = device;
			Allocator = allocator;

			IntPtr pDevFunc = Instance.VK.InstanceGetProcAddr(Instance, "vkGetDeviceProcAddr");
			if (pDevFunc != IntPtr.Zero) DeviceGetProcAddress = Marshal.GetDelegateForFunctionPointer<VKGetDeviceProcAddr>(pDevFunc);
			else DeviceGetProcAddress = (IntPtr _, string name) => Instance.InstanceGetProcAddr(Instance, name);

			Library.LoadFunctions(GetProcAddr, VK10Functions);
			if (Instance.APIVersion >= VK11.ApiVersion) Library.LoadFunctions(GetProcAddr, VK11Functions = new());
			if (Instance.APIVersion >= VK12.ApiVersion) Library.LoadFunctions(GetProcAddr, VK12Functions = new());

			// A bit ugly to convert back from strings provided in create info but simplifies parameter passing
			UnmanagedPointer<IntPtr> pExts = new(createInfo.EnabledExtensionNames);
			HashSet<string> exts = new();
			for (int i = 0; i < createInfo.EnabledExtensionCount; i++) exts.Add(MemoryUtil.GetUTF8(pExts[i]));

			// Load device extensions
			if (exts.Contains(KHRSwapchain.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRSwapchainFunctions = new());
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			VK10Functions.vkDestroyDevice(Device, Allocator);
		}

		// Vulkan 1.0

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitIdle() => VK.CheckError(VK10Functions.vkDeviceWaitIdle(Device));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKQueue GetQueue(uint family, uint index) {
			VK10Functions.vkGetDeviceQueue(Device, family, index, out IntPtr queue);
			return new VKQueue(queue, this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDeviceMemory AllocateMemory(in VKMemoryAllocateInfo allocateInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkAllocateMemory(Device, allocateInfo, allocator, out ulong memory), "Failed to allocate device memory");
			return new VKDeviceMemory(this, memory, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedMemoryRanges(in ReadOnlySpan<VKMappedMemoryRange> ranges) {
			unsafe {
				fixed(VKMappedMemoryRange* pRanges = ranges) {
					VK.CheckError(VK10Functions.vkFlushMappedMemoryRanges(Device, (uint)ranges.Length, (IntPtr)pRanges), "Failed to flush mapped memory ranges");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateMappedMemoryRanges(in ReadOnlySpan<VKMappedMemoryRange> ranges) {
			unsafe {
				fixed (VKMappedMemoryRange* pRanges = ranges) {
					VK.CheckError(VK10Functions.vkInvalidateMappedMemoryRanges(Device, (uint)ranges.Length, (IntPtr)pRanges), "Failed to invalidate mapped memory ranges");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKFence CreateFence(in VKFenceCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateFence(Device, createInfo, allocator, out ulong fence), "Failed to create fence");
			return new VKFence(this, fence, allocator);
		}

		public void ResetFences(params VKFence[] fences) {
			using MemoryStack sp = MemoryStack.Push();
			foreach (VKFence fence in fences) sp.Values(fence.Fence);
			VK.CheckError(VK10Functions.vkResetFences(Device, (uint)fences.Length, sp.Base + sp.Pointer), "Failed to reset fences");
		}

		public bool WaitForFences(bool waitAll, ulong timeout, params VKFence[] fences) {
			using MemoryStack sp = MemoryStack.Push();
			foreach (VKFence fence in fences) sp.Values(fence.Fence);
			VKResult err = VK10Functions.vkWaitForFences(Device, (uint)fences.Length, sp.Base + sp.Pointer, waitAll, timeout);
			return err switch {
				VKResult.Success => true,
				VKResult.Timeout => false,
				_ => throw new VulkanException("Failed to wait for fences", err)
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSemaphore CreateSemaphore(in VKSemaphoreCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateSemaphore(Device, createInfo, allocator, out ulong semaphore), "Failed to create semaphore");
			return new VKSemaphore(this, semaphore);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKEvent CreateEvent(in VKEventCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateEvent(Device, createInfo, Allocator, out ulong _event), "Failed to create event");
			return new VKEvent(this, _event, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKQueryPool CreateQueryPool(in VKQueryPoolCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateQueryPool(Device, createInfo, allocator, out ulong queryPool), "Failed to create query pool");
			return new VKQueryPool(this, queryPool, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKBuffer CreateBuffer(in VKBufferCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateBuffer(Device, createInfo, allocator, out ulong buffer), "Failed to create buffer");
			return new VKBuffer(this, buffer, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKBufferView CreateBufferView(in VKBufferViewCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateBufferView(Device, createInfo, allocator, out ulong bufferView), "Failed to create buffer view");
			return new VKBufferView(this, bufferView, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImage CreateImage(in VKImageCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateImage(Device, createInfo, allocator, out ulong image), "Failed to create image");
			return new VKImage(this, image, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageView CreateImageView(in VKImageViewCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateImageView(Device, createInfo, allocator, out ulong imageView), "Failed to create image view");
			return new VKImageView(this, imageView, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKShaderModule CreateShaderModule(in VKShaderModuleCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateShaderModule(Device, createInfo, allocator, out ulong shaderModule), "Failed to create shader module");
			return new VKShaderModule(this, shaderModule, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKPipeline CreateGraphicsPipelines(VKPipelineCache pipelineCache, in VKGraphicsPipelineCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			ulong pipeline = 0;
			unsafe {
				fixed (VKGraphicsPipelineCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VK10Functions.vkCreateGraphicsPipelines(Device, pipelineCache, 1, (IntPtr)(pCreateInfo), allocator, (IntPtr)(&pipeline)), "Failed to create graphics pipeline(s)");
				}
			}
			return new VKPipeline(this, pipeline, allocator);
		}

		public VKPipeline[] CreateGraphicsPipelines(VKPipelineCache pipelineCache, in ReadOnlySpan<VKGraphicsPipelineCreateInfo> createInfos, VulkanAllocationCallbacks allocator = null) {
			ulong[] pipelines = new ulong[createInfos.Length];
			unsafe {
				fixed(ulong* pPipelines = pipelines) {
					fixed(VKGraphicsPipelineCreateInfo* pCreateInfos = createInfos) {
						VK.CheckError(VK10Functions.vkCreateGraphicsPipelines(Device, pipelineCache, (uint)createInfos.Length, (IntPtr)pCreateInfos, allocator, (IntPtr)pPipelines), "Failed to create graphics pipeline(s)");
					}
				}
			}
			return Array.ConvertAll(pipelines, pipeline => new VKPipeline(this, pipeline, allocator));
		}

		public VKPipeline CreateComputePipeline(VKPipelineCache pipelineCache, in VKComputePipelineCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			ulong pipeline = 0;
			using ManagedPointer<VKComputePipelineCreateInfo> pCreateInfo = new(createInfo);
			unsafe {
				VK.CheckError(VK10Functions.vkCreateComputePipelines(Device, pipelineCache, 1, pCreateInfo, allocator, (IntPtr)(&pipeline)), "Failed to create graphics pipeline(s)");
			}
			return new VKPipeline(this, pipeline, allocator);
		}

		/*
		public VKPipeline[] CreateGraphicsPipelines(VKPipelineCache pipelineCache, in ReadOnlySpan<VKGraphicsPipelineCreateInfo> createInfos, VulkanAllocationCallbacks allocator = null) {
			ulong[] pipelines = new ulong[createInfos.Length];
			using ManagedPointer<VKComputePipelineCreateInfo> pCreateInfos = new(createInfos.Length);
			for (int i = 0; i < createInfos.Length; i++) pCreateInfos[i] = createInfos[i];
			unsafe {
				fixed (ulong* pPipelines = pipelines) {
					VK.CheckError(VK10Functions.vkCreateGraphicsPipelines(Device, pipelineCache, (uint)createInfos.Length, (IntPtr)pCreateInfos, allocator, (IntPtr)pPipelines), "Failed to create graphics pipeline(s)");
				}
			}
			return Array.ConvertAll(pipelines, pipeline => new VKPipeline(this, pipeline, allocator));
		}
		*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKPipelineLayout CreatePipelineLayout(in VKPipelineLayoutCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreatePipelineLayout(Device, createInfo, allocator, out ulong pipelineLayout), "Failed to create pipeline layout");
			return new VKPipelineLayout(this, pipelineLayout, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSampler CreateSampler(in VKSamplerCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateSampler(Device, createInfo, allocator, out ulong sampler), "Failed to create sampler");
			return new VKSampler(this, sampler, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDescriptorSetLayout CreateDescriptorSetLayout(in VKDescriptorSetLayoutCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateDescriptorSetLayout(Device, createInfo, allocator, out ulong layout), "Failed to create descriptor set layout");
			return new VKDescriptorSetLayout(this, layout, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDescriptorPool CreateDescriptorPool(in VKDescriptorPoolCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateDescriptorPool(Device, createInfo, allocator, out ulong descriptorPool), "Failed to create descriptor pool");
			return new VKDescriptorPool(this, descriptorPool, allocator);
		}

		public void UpdateDescriptorSets(in ReadOnlySpan<VKWriteDescriptorSet> writes, in ReadOnlySpan<VKCopyDescriptorSet> copies) {
			unsafe {
				fixed(VKWriteDescriptorSet* pWrites = writes) {
					fixed(VKCopyDescriptorSet* pCopies = copies) {
						VK10Functions.vkUpdateDescriptorSets(Device, (uint)writes.Length, (IntPtr)pWrites, (uint)copies.Length, (IntPtr)pCopies);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKFramebuffer CreateFramebuffer(in VKFramebufferCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateFramebuffer(Device, createInfo, allocator, out ulong framebuffer), "Failed to create framebuffer");
			return new VKFramebuffer(this, framebuffer, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKRenderPass CreateRenderPass(in VKRenderPassCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateRenderPass(Device, createInfo, allocator, out ulong renderPass), "Failed to create render pass");
			return new VKRenderPass(this, renderPass, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKCommandPool CreateCommandPool(in VKCommandPoolCreateInfo createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(VK10Functions.vkCreateCommandPool(Device, createInfo, allocator, out ulong commandPool), "Failed to create command pool");
			return new VKCommandPool(this, commandPool, allocator);
		}

		// VK_KHR_swapchain

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSwapchainKHR CreateSwapchainKHR(in VKSwapchainCreateInfoKHR createInfo, VulkanAllocationCallbacks allocator = null) {
			VK.CheckError(KHRSwapchainFunctions.vkCreateSwapchainKHR(Device, createInfo, allocator, out ulong swapchain), "Failed to create swapchain");
			return new VKSwapchainKHR(this, swapchain, allocator);
		}

		public VKDeviceGroupPresentCapabilitiesKHR DeviceGroupPresentCapabilitiesKHR {
			get {
				VK.CheckError(KHRSwapchainFunctions.vkGetDeviceGroupPresentCapabilitiesKHR(Device, out VKDeviceGroupPresentCapabilitiesKHR deviceGroupPresentCapabilities), "Failed to get device group present capabilities");
				return deviceGroupPresentCapabilities;
			}
		}

		public VKDeviceGroupPresentModeFlagBitsKHR GetDeviceGroupSurfacePresentModesKHR(VKSurfaceKHR surface) {
			VK.CheckError(KHRSwapchainFunctions.vkGetDeviceGroupSurfacePresentModesKHR(Device, surface, out VKDeviceGroupPresentModeFlagBitsKHR modes), "Failed to get device group surface present modes");
			return modes;
		}

		public VKRect2D[] GetPhysicalDevicePresentRectanglesKHR(VKPhysicalDevice physicalDevice, VKSurfaceKHR surface) {
			uint count = 0;
			VK.CheckError(KHRSwapchainFunctions.vkGetPhysicalDevicePresentRectanglesKHR(physicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device present rectangles");
			VKRect2D[] rects = new VKRect2D[count];
			unsafe {
				fixed(VKRect2D* pRects = rects) {
					VK.CheckError(KHRSwapchainFunctions.vkGetPhysicalDevicePresentRectanglesKHR(physicalDevice, surface, ref count, (IntPtr)pRects), "Failed to get physical device present rectangles");
				}
			}
			return rects;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKDevice device) => device != null ? device.Device : IntPtr.Zero;

	}

}
