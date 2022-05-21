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

	public class VKDevice : IDisposable, IVKInstanceObject, IVKAllocatedObject, IPrimitiveHandle<IntPtr> {

		public VKInstance Instance { get; }

		public uint APIVersion => Instance.APIVersion;

		[NativeType("VkDevice")]
		public IntPtr Device { get; }

		public IntPtr PrimitiveHandle => Device;

		public VulkanAllocationCallbacks? Allocator { get; }

		// Vulkan versions
		public VK10DeviceFunctions VK10Functions { get; } = new();
		public VK11DeviceFunctions? VK11Functions { get; }
		public VK12DeviceFunctions? VK12Functions { get; }

		// KHR Extensions
		// Vulkan 1.1
		public bool KHR16BitStorage { get; }
		public KHRBindMemory2DeviceFunctions? KHRBindMemory2 { get; }
		public bool KHRDedicatedAllocation { get; }
		public KHRDescriptorUpdateTemplateDeviceFunctions? KHRDescriptorUpdateTemplate { get; }
		public KHRDeviceGroupDeviceFunctions? KHRDeviceGroup { get; }
		public bool KHRExternalFence { get; }
		public bool KHRExternalMemory { get; }
		public bool KHRExternalSemaphore { get; }
		public KHRGetMemoryRequirements2DeviceFunctions? KHRGetMemoryRequirements2 { get; }
		public KHRMaintenance1DeviceFunctions? KHRMaintenance1 { get; }
		public bool KHRMaintenance2 { get; }
		public KHRMaintenance3DeviceFunctions? KHRMaintenance3 { get; }
		public bool KHRMultiview { get; }
		public bool KHRRelaxedBlockLayout { get; }
		public KHRSamplerYcbcrConversionDeviceFunctions? KHRSamplerYcbcrConversion { get; }
		public bool KHRShaderDrawParameters { get; }
		public bool KHRStorageBufferStorageClass { get; }
		public bool KHRVariablePointers { get; }
		// Vulkan 1.2
		public bool KHR8BitStorage { get; }
		public KHRBufferDeviceAddressDeviceFunctions? KHRBufferDeviceAddress { get; }
		public KHRCreateRenderpass2DeviceFunctions? KHRCreateRenderpass2 { get; }
		public bool KHRDepthStencilResolve { get; }
		public KHRDrawIndirectCountDeviceFunctions? KHRDrawIndirectCount { get; }
		public bool KHRDriverProperties { get; }
		public bool KHRImageFormatList { get; }
		public bool KHRImagelessFramebuffer { get; }
		public bool KHRSamplerMirrorClampToEdge { get; }
		public bool KHRSeparateDepthStencilLayouts { get; }
		public bool KHRShaderAtomicInt64 { get; }
		public bool KHRShaderFloat16Int8 { get; }
		public bool KHRShaderFloatControls { get; }
		public bool KHRShaderSubgroupExtendedTypes { get; }
		public bool KHRSPIRV14 { get; }
		public KHRTimelineSemaphoreDeviceFunctions? KHRTimelineSemaphore { get; }
		public bool KHRUniformBufferStandardLayout { get; }
		public bool KHRVulkanMemoryModel { get; }
		// Miscellaneous
		public KHRSwapchainDeviceFunctions? KHRSwapchain { get; }

		// EXT Extensions
		// Vulkan 1.2
		public bool EXTDescriptorIndexing { get; }
		public EXTHostQueryResetDeviceFunctions? EXTHostQueryReset { get; }
		public bool EXTSamplerFilterMinmax { get; }
		public bool EXTScalarBlockLayout { get; }
		public bool EXTSeparateStencilUsage { get; }
		public bool EXTShaderViewportIndexLayer { get; }
		// Miscellaneous
		public bool EXTCustomBorderColor { get; }
		public EXTLineRasterizationDeviceFunctions? EXTLineRasterization { get; }
		public EXTExtendedDynamicStateDeviceFunctions? EXTExtendedDynamicState { get; }

		public VKGetDeviceProcAddr DeviceGetProcAddress;

		public IntPtr GetProcAddr(string name) => DeviceGetProcAddress(Device, name);

		public VKDevice(VKInstance instance, IntPtr device, in VKDeviceCreateInfo createInfo, VulkanAllocationCallbacks? allocator) {
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
			for (int i = 0; i < createInfo.EnabledExtensionCount; i++) exts.Add(MemoryUtil.GetUTF8(pExts[i])!);

			// Load device extensions
			// Vulkan 1.1
			KHR16BitStorage = exts.Contains(Vulkan.KHR16BitStorage.ExtensionName);
			if (exts.Contains(Vulkan.KHRBindMemory2.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRBindMemory2 = new());
			KHRDedicatedAllocation = exts.Contains(Vulkan.KHRDedicatedAllocation.ExtensionName);
			if (exts.Contains(Vulkan.KHRDescriptorUpdateTemplate.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRDescriptorUpdateTemplate = new());
			if (exts.Contains(Vulkan.KHRDeviceGroup.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRDescriptorUpdateTemplate = new());
			KHRExternalFence = exts.Contains(Vulkan.KHRExternalFence.ExtensionName);
			KHRExternalMemory = exts.Contains(Vulkan.KHRExternalMemory.ExtensionName);
			KHRExternalSemaphore = exts.Contains(Vulkan.KHRExternalSemaphore.ExtensionName);
			if (exts.Contains(Vulkan.KHRGetMemoryRequirements2.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRGetMemoryRequirements2 = new());
			if (exts.Contains(Vulkan.KHRMaintenance1.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRMaintenance1 = new());
			KHRMaintenance2 = exts.Contains(Vulkan.KHRMaintenance2.ExtensionName);
			if (exts.Contains(Vulkan.KHRMaintenance3.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRMaintenance3 = new());
			KHRMultiview = exts.Contains(Vulkan.KHRMultiview.ExtensionName);
			KHRRelaxedBlockLayout = exts.Contains(Vulkan.KHRRelaxedBlockLayout.ExtensionName);
			if (exts.Contains(Vulkan.KHRSamplerYcbcrConversion.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRSamplerYcbcrConversion = new());
			KHRShaderDrawParameters = exts.Contains(Vulkan.KHRShaderDrawParameters.ExtensionName);
			KHRStorageBufferStorageClass = exts.Contains(Vulkan.KHRStorageBufferStorageClass.ExtensionName);
			KHRVariablePointers = exts.Contains(Vulkan.KHRVariablePointers.ExtensionName);
			// Vulkan 1.2
			KHR8BitStorage = exts.Contains(Vulkan.KHR8BitStorage.ExtensionName);
			if (exts.Contains(Vulkan.KHRBufferDeviceAddress.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRBufferDeviceAddress = new());
			if (exts.Contains(Vulkan.KHRCreateRenderpass2.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRCreateRenderpass2 = new());
			KHRDepthStencilResolve = exts.Contains(Vulkan.KHRDepthStencilResolve.ExtensionName);
			if (exts.Contains(Vulkan.KHRDrawIndirectCount.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRDrawIndirectCount = new());
			KHRDriverProperties = exts.Contains(Vulkan.KHRDriverProperties.ExtensionName);
			KHRImageFormatList = exts.Contains(Vulkan.KHRImageFormatList.ExtensionName);
			KHRImagelessFramebuffer = exts.Contains(Vulkan.KHRImagelessFramebuffer.ExtensionName);
			KHRSamplerMirrorClampToEdge = exts.Contains(Vulkan.KHRSamplerMirrorClampToEdge.ExtesionName);
			KHRSeparateDepthStencilLayouts = exts.Contains(Vulkan.KHRSeparateDepthStencilLayouts.ExtensionName);
			KHRShaderAtomicInt64 = exts.Contains(Vulkan.KHRShaderAtomicInt64.ExtensionName);
			KHRShaderFloat16Int8 = exts.Contains(Vulkan.KHRShaderFloat16Int8.ExtensionName);
			KHRShaderFloatControls = exts.Contains(Vulkan.KHRShaderFloatControls.ExtensionName);
			KHRShaderSubgroupExtendedTypes = exts.Contains(Vulkan.KHRShaderSubgroupExtendedTypes.ExtensionName);
			KHRSPIRV14 = exts.Contains(Vulkan.KHRSPIRV14.ExtensionName);
			if (exts.Contains(Vulkan.KHRTimelineSemaphore.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRTimelineSemaphore = new());
			KHRUniformBufferStandardLayout = exts.Contains(Vulkan.KHRUniformBufferStandardLayout.ExtensionName);
			KHRVulkanMemoryModel = exts.Contains(Vulkan.KHRVulkanMemoryModel.ExtensionName);
			EXTDescriptorIndexing = exts.Contains(Vulkan.EXTDescriptorIndexing.ExtensionName);
			if (exts.Contains(Vulkan.EXTHostQueryReset.ExtensionName)) Library.LoadFunctions(GetProcAddr, EXTHostQueryReset = new());
			EXTSamplerFilterMinmax = exts.Contains(Vulkan.EXTSamplerFilterMinmax.ExtensionName);
			EXTScalarBlockLayout = exts.Contains(Vulkan.EXTScalarBlockLayout.ExtensionName);
			EXTSeparateStencilUsage = exts.Contains(Vulkan.EXTSeparateStencilUsage.ExtensionName);
			EXTShaderViewportIndexLayer = exts.Contains(Vulkan.EXTShaderViewportIndexLayer.ExtensionName);

			if (exts.Contains(Vulkan.KHRSwapchain.ExtensionName)) Library.LoadFunctions(GetProcAddr, KHRSwapchain = new());

			EXTCustomBorderColor = exts.Contains(Vulkan.EXTCustomBorderColor.ExtensionName);
			if (exts.Contains(Vulkan.EXTLineRasterization.ExtensionName)) Library.LoadFunctions(this.GetProcAddr, EXTLineRasterization = new());
			if (exts.Contains(Vulkan.EXTExtendedDynamicState.ExtensionName)) Library.LoadFunctions(this.GetProcAddr, EXTExtendedDynamicState = new());
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
		public VKDeviceMemory AllocateMemory(in VKMemoryAllocateInfo allocateInfo, VulkanAllocationCallbacks? allocator = null) {
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
		public VKFence CreateFence(in VKFenceCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
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
		public VKSemaphore CreateSemaphore(in VKSemaphoreCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateSemaphore(Device, createInfo, allocator, out ulong semaphore), "Failed to create semaphore");
			return new VKSemaphore(this, semaphore, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKEvent CreateEvent(in VKEventCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateEvent(Device, createInfo, Allocator, out ulong _event), "Failed to create event");
			return new VKEvent(this, _event, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKQueryPool CreateQueryPool(in VKQueryPoolCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateQueryPool(Device, createInfo, allocator, out ulong queryPool), "Failed to create query pool");
			return new VKQueryPool(this, queryPool, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKBuffer CreateBuffer(in VKBufferCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateBuffer(Device, createInfo, allocator, out ulong buffer), "Failed to create buffer");
			return new VKBuffer(this, buffer, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKBufferView CreateBufferView(in VKBufferViewCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateBufferView(Device, createInfo, allocator, out ulong bufferView), "Failed to create buffer view");
			return new VKBufferView(this, bufferView, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImage CreateImage(in VKImageCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateImage(Device, createInfo, allocator, out ulong image), "Failed to create image");
			return new VKImage(this, image, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKImageView CreateImageView(in VKImageViewCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateImageView(Device, createInfo, allocator, out ulong imageView), "Failed to create image view");
			return new VKImageView(this, imageView, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKShaderModule CreateShaderModule(in VKShaderModuleCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateShaderModule(Device, createInfo, allocator, out ulong shaderModule), "Failed to create shader module");
			return new VKShaderModule(this, shaderModule, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKPipelineCache CreatePipelineCache(in VKPipelineCacheCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreatePipelineCache(Device, createInfo, allocator, out ulong pipelineCache), "Failed to create pipeline cache");
			return new VKPipelineCache(this, pipelineCache, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKPipeline CreateGraphicsPipelines(VKPipelineCache? pipelineCache, in VKGraphicsPipelineCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			ulong pipeline = 0;
			unsafe {
				fixed (VKGraphicsPipelineCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VK10Functions.vkCreateGraphicsPipelines(Device, pipelineCache, 1, (IntPtr)(pCreateInfo), allocator, (IntPtr)(&pipeline)), "Failed to create graphics pipeline(s)");
				}
			}
			return new VKPipeline(this, pipeline, allocator);
		}

		public VKPipeline[] CreateGraphicsPipelines(VKPipelineCache? pipelineCache, in ReadOnlySpan<VKGraphicsPipelineCreateInfo> createInfos, VulkanAllocationCallbacks? allocator = null) {
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

		public VKPipeline CreateComputePipeline(VKPipelineCache? pipelineCache, in VKComputePipelineCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
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
		public VKPipelineLayout CreatePipelineLayout(in VKPipelineLayoutCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreatePipelineLayout(Device, createInfo, allocator, out ulong pipelineLayout), "Failed to create pipeline layout");
			return new VKPipelineLayout(this, pipelineLayout, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSampler CreateSampler(in VKSamplerCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateSampler(Device, createInfo, allocator, out ulong sampler), "Failed to create sampler");
			return new VKSampler(this, sampler, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDescriptorSetLayout CreateDescriptorSetLayout(in VKDescriptorSetLayoutCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateDescriptorSetLayout(Device, createInfo, allocator, out ulong layout), "Failed to create descriptor set layout");
			return new VKDescriptorSetLayout(this, layout, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKDescriptorPool CreateDescriptorPool(in VKDescriptorPoolCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
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
		public VKFramebuffer CreateFramebuffer(in VKFramebufferCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateFramebuffer(Device, createInfo, allocator, out ulong framebuffer), "Failed to create framebuffer");
			return new VKFramebuffer(this, framebuffer, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKRenderPass CreateRenderPass(in VKRenderPassCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateRenderPass(Device, createInfo, allocator, out ulong renderPass), "Failed to create render pass");
			return new VKRenderPass(this, renderPass, allocator);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKCommandPool CreateCommandPool(in VKCommandPoolCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(VK10Functions.vkCreateCommandPool(Device, createInfo, allocator, out ulong commandPool), "Failed to create command pool");
			return new VKCommandPool(this, commandPool, allocator);
		}

		// VK_KHR_swapchain

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public VKSwapchainKHR CreateSwapchainKHR(in VKSwapchainCreateInfoKHR createInfo, VulkanAllocationCallbacks? allocator = null) {
			VK.CheckError(KHRSwapchain!.vkCreateSwapchainKHR(Device, createInfo, allocator, out ulong swapchain), "Failed to create swapchain");
			return new VKSwapchainKHR(this, swapchain, allocator);
		}

		public VKDeviceGroupPresentCapabilitiesKHR DeviceGroupPresentCapabilitiesKHR {
			get {
				VK.CheckError(KHRSwapchain!.vkGetDeviceGroupPresentCapabilitiesKHR(Device, out VKDeviceGroupPresentCapabilitiesKHR deviceGroupPresentCapabilities), "Failed to get device group present capabilities");
				return deviceGroupPresentCapabilities;
			}
		}

		public VKDeviceGroupPresentModeFlagBitsKHR GetDeviceGroupSurfacePresentModesKHR(VKSurfaceKHR surface) {
			VK.CheckError(KHRSwapchain!.vkGetDeviceGroupSurfacePresentModesKHR(Device, surface, out VKDeviceGroupPresentModeFlagBitsKHR modes), "Failed to get device group surface present modes");
			return modes;
		}

		public VKRect2D[] GetPhysicalDevicePresentRectanglesKHR(VKPhysicalDevice physicalDevice, VKSurfaceKHR surface) {
			uint count = 0;
			VK.CheckError(KHRSwapchain!.vkGetPhysicalDevicePresentRectanglesKHR(physicalDevice, surface, ref count, IntPtr.Zero), "Failed to get physical device present rectangles");
			VKRect2D[] rects = new VKRect2D[count];
			unsafe {
				fixed (VKRect2D* pRects = rects) {
					VK.CheckError(KHRSwapchain.vkGetPhysicalDevicePresentRectanglesKHR(physicalDevice, surface, ref count, (IntPtr)pRects), "Failed to get physical device present rectangles");
				}
			}
			return rects;
		}

		// Vulkan 1.1
		// VK_KHR_bind_memory2

		public void BindBufferMemory2(VKBindBufferMemoryInfo bindInfo) {
			var vkBindBufferMemory2 = VK11Functions?.vkBindBufferMemory2;
			if (vkBindBufferMemory2 == null) vkBindBufferMemory2 = new(KHRBindMemory2!.vkBindBufferMemory2KHR);
			unsafe {
				VK.CheckError(vkBindBufferMemory2(Device, 1, (IntPtr)(&bindInfo)));
			}
		}

		public void BindBufferMemory2(params VKBindBufferMemoryInfo[] bindInfos) {
			var vkBindBufferMemory2 = VK11Functions?.vkBindBufferMemory2;
			if (vkBindBufferMemory2 == null) vkBindBufferMemory2 = new(KHRBindMemory2!.vkBindBufferMemory2KHR);
			unsafe {
				fixed (VKBindBufferMemoryInfo* pBindInfos = bindInfos) {
					VK.CheckError(vkBindBufferMemory2(Device, (uint)bindInfos.Length, (IntPtr)pBindInfos));
				}
			}
		}

		public void BindBufferMemory2(ReadOnlySpan<VKBindBufferMemoryInfo> bindInfos) {
			var vkBindBufferMemory2 = VK11Functions?.vkBindBufferMemory2;
			if (vkBindBufferMemory2 == null) vkBindBufferMemory2 = new(KHRBindMemory2!.vkBindBufferMemory2KHR);
			unsafe {
				fixed (VKBindBufferMemoryInfo* pBindInfos = bindInfos) {
					VK.CheckError(vkBindBufferMemory2(Device, (uint)bindInfos.Length, (IntPtr)pBindInfos));
				}
			}
		}

		public void BindImageMemory2(VKBindImageMemoryInfo bindInfo) {
			var vkBindImageMemory2 = VK11Functions?.vkBindImageMemory2;
			if (vkBindImageMemory2 == null) vkBindImageMemory2 = new(KHRBindMemory2!.vkBindImageMemory2KHR);
			unsafe {
				VK.CheckError(vkBindImageMemory2(Device, 1, (IntPtr)(&bindInfo)));
			}
		}

		public void BindImageMemory2(params VKBindImageMemoryInfo[] bindInfos) {
			var vkBindImageMemory2 = VK11Functions?.vkBindImageMemory2;
			if (vkBindImageMemory2 == null) vkBindImageMemory2 = new(KHRBindMemory2!.vkBindImageMemory2KHR);
			unsafe {
				fixed(VKBindImageMemoryInfo* pBindInfos = bindInfos) {
					VK.CheckError(vkBindImageMemory2(Device, 1, (IntPtr)pBindInfos));
				}
			}
		}

		public void BindImageMemory2(ReadOnlySpan<VKBindImageMemoryInfo> bindInfos) {
			var vkBindImageMemory2 = VK11Functions?.vkBindImageMemory2;
			if (vkBindImageMemory2 == null) vkBindImageMemory2 = new(KHRBindMemory2!.vkBindImageMemory2KHR);
			unsafe {
				fixed (VKBindImageMemoryInfo* pBindInfos = bindInfos) {
					VK.CheckError(vkBindImageMemory2(Device, 1, (IntPtr)pBindInfos));
				}
			}
		}

		// Vulkan 1.1		
		// VK_KHR_descriptor_update_template
		public VKDescriptorUpdateTemplate CreateDescriptorUpdateTemplate(in VKDescriptorUpdateTemplateCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			ulong descriptorUpdateTemplate;
			VKResult err;
			if (VK11Functions) err = VK11Functions!.vkCreateDescriptorUpdateTemplate(Device, createInfo, allocator, out descriptorUpdateTemplate);
			else err = KHRDescriptorUpdateTemplate!.vkCreateDescriptorUpdateTemplateKHR(Device, createInfo, allocator, out descriptorUpdateTemplate);
			VK.CheckError(err);
			return new VKDescriptorUpdateTemplate(this, descriptorUpdateTemplate, allocator);
		}

		// Vulkan 1.1
		// VK_KHR_device_group
		public VKPeerMemoryFeatureFlagBits GetGroupPeerMemoryFeatures(uint heapIndex, uint localDeviceIndex, uint remoteDeviceIndex) {
			VKPeerMemoryFeatureFlagBits flags;
			if (VK11Functions) VK11Functions!.vkGetDeviceGroupPeerMemoryFeatures(Device, heapIndex, localDeviceIndex, remoteDeviceIndex, out flags);
			else KHRDeviceGroup!.vkGetDeviceGroupPeerMemoryFeaturesKHR(Device, heapIndex, localDeviceIndex, remoteDeviceIndex, out flags);
			return flags;
		}

		// Vulkan 1.1
		// VK_KHR_maintenance3

		public void GetDescriptorSetLayoutSupport(in VKDescriptorSetLayoutCreateInfo createInfo, ref VKDescriptorSetLayoutSupport support) {
			if (VK11Functions) VK11Functions!.vkGetDescriptorSetLayoutSupport(Device, createInfo, ref support);
			else KHRMaintenance3!.vkGetDescriptorSetLayoutSupportKHR(Device, createInfo, ref support);
		}

		// Vulkan 1.1
		// VK_KHR_sampler_ycbcr_conversion
		public VKSamplerYcbcrConversion CreateSamplerYcbcrConversion(in VKSamplerYcbcrConversionCreateInfo createInfo, VulkanAllocationCallbacks? allocator = null) {
			ulong samplerYcbcrConversion;
			VKResult err;
			if (VK11Functions) err = VK11Functions!.vkCreateSamplerYcbcrConversion(Device, createInfo, allocator, out samplerYcbcrConversion);
			else err = KHRSamplerYcbcrConversion!.vkCreateSamplerYcbcrConversionKHR(Device, createInfo, allocator, out samplerYcbcrConversion);
			VK.CheckError(err);
			return new VKSamplerYcbcrConversion(this, samplerYcbcrConversion, allocator);
		}

		// Vulkan 1.2
		// VK_KHR_create_renderpass2
		public VKRenderPass CreateRenderPass2(in VKRenderPassCreateInfo2 createInfo, VulkanAllocationCallbacks? allocator = null) {
			ulong renderpass;
			VKResult err;
			if (VK12Functions) err = VK12Functions!.vkCreateRenderPass2(Device, createInfo, allocator, out renderpass);
			else err = KHRCreateRenderpass2!.vkCreateRenderPass2KHR(Device, createInfo, allocator, out renderpass);
			VK.CheckError(err);
			return new VKRenderPass(this, renderpass, allocator);
		}

		// Vulkan 1.2
		// VK_KHR_timeline_semaphore
		public void SignalSemaphore(in VKSemaphoreSignalInfo signalInfo) {
			VKResult err;
			if (VK12Functions) err = VK12Functions!.vkSignalSemaphore(Device, signalInfo);
			else err = KHRTimelineSemaphore!.vkSignalSemaphoreKHR(Device, signalInfo);
			VK.CheckError(err);
		}

		public VKResult WaitSemaphores(in VKSemaphoreWaitInfo waitInfo, ulong timeout) {
			VKResult err;
			if (VK12Functions) err = VK12Functions!.vkWaitSemaphores(Device, waitInfo, timeout);
			else err = KHRTimelineSemaphore!.vkWaitSemaphoresKHR(Device, waitInfo, timeout);
			switch(err) {
				case VKResult.Success:
				case VKResult.Timeout:
					break;
				default:
					VK.CheckError(err);
					break;
			}
			return err;
		}

		// VK_EXT_debug_utils

		public void SetDebugUtilsObjectNameEXT(in VKDebugUtilsObjectNameInfoEXT nameInfo) => VK.CheckError(Instance.EXTDebugUtilsFunctions.vkSetDebugUtilsObjectNameEXT(Device, nameInfo));

		public void SetDebugUtilsObjectTagEXT(in VKDebugUtilsObjectTagInfoEXT tagInfo) => VK.CheckError(Instance.EXTDebugUtilsFunctions.vkSetDebugUtilsObjectTagEXT(Device, tagInfo));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntPtr(VKDevice device) => device != null ? device.Device : IntPtr.Zero;

	}

}
