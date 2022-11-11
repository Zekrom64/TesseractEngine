using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	using VKFlags = UInt32;
	using VKFlags64 = UInt64;

	// Vulkan 1.0

	/// <summary>
	/// Encoded pipeline cache header version.
	/// </summary>
	public enum VKPipelineCacheHeaderVersion : int {
		/// <summary>
		/// Version one of the pipeline cache.
		/// </summary>
		One = 1
	}

	/// <summary>
	/// <para>The result returned by some Vulkan commands.</para>
	/// <para>
	/// While the Vulkan API does very little error checking for invalid usage, return codes
	/// are still used to communicate status information or errors that can only be detected
	/// at runtime.
	/// </para>
	/// </summary>
	public enum VKResult : int {
		/// <summary>
		/// The command completed successfully.
		/// </summary>
		Success = 0,
		/// <summary>
		/// A fence or query has not yet completed.
		/// </summary>
		NotReady = 1,
		/// <summary>
		/// A wait operation has not completed before the specified time.
		/// </summary>
		Timeout = 2,
		/// <summary>
		/// An event is signaled.
		/// </summary>
		EventSet = 3,
		/// <summary>
		/// An event is unsignaled.
		/// </summary>
		EventReset = 4,
		/// <summary>
		/// A return array was too small for a result.
		/// </summary>
		Incomplete = 5,
		/// <summary>
		/// A host memory allocation has failed.
		/// </summary>
		ErrorOutOfHostMemory = -1,
		/// <summary>
		/// A device memory allocation has failed.
		/// </summary>
		ErrorOutOfDeviceMemory = -2,
		/// <summary>
		/// Initialization of an object could not be completed for implementation-specific reasons.
		/// </summary>
		ErrorInitializationFailed = -3,
		/// <summary>
		/// The logical or physical device has been lost.
		/// </summary>
		ErrorDeviceLost = -4,
		/// <summary>
		/// Mapping of a memory object has failed.
		/// </summary>
		ErrorMemoryMapFailed = -5,
		/// <summary>
		/// A requested layer is not present or could not be loaded.
		/// </summary>
		ErrorLayerNotPresent = -6,
		/// <summary>
		/// A requested extension is not supported.
		/// </summary>
		ErrorExtensionNotPresent = -7,
		/// <summary>
		/// A requested feature is not supported.
		/// </summary>
		ErrorFeatureNotPresent = -8,
		/// <summary>
		/// The requested version of Vulkan is not supported by the driver or is otherwise incompatible for implementation-specific reasons.
		/// </summary>
		ErrorIncompatibleDriver = -9,
		/// <summary>
		/// Too many objects of the type have already been created.
		/// </summary>
		ErrorTooManyObjects = -10,
		/// <summary>
		/// A requested format is not supported on this device.
		/// </summary>
		ErrorFormatNotSupported = -11,
		/// <summary>
		/// A pool allocation has failed due to fragmentation of the pool's memory. This must only be returned if no attempt to allocate
		/// host or device memory was made to accomodate the new allocation. This should be returned in preference to <see cref="ErrorOutOfPoolMemory"/>,
		/// but only if the implementation is certain that the pool allocation failure was due to fragmentation.
		/// </summary>
		ErrorFragmentedPool = -12,
		/// <summary>
		/// An unknown error occurred; either the application has provided invalid input, or an implementation failure has occurred.
		/// </summary>
		ErrorUnknown = -13,
		/// <summary>
		/// A pool memory allocation has failed. This must only be returned if no attempt to allocate host or device memory was made
		/// to accommodate the new allocation. If the failure was definitely due to fragmentation of the pool, <see cref="ErrorFragmentedPool"/>
		/// should be returned instead.
		/// </summary>
		ErrorOutOfPoolMemory = -1000069000,
		/// <summary>
		/// An external handle is not a valid handle of the specified type.
		/// </summary>
		ErrorInvalidExternalHandle = -1000072003,
		/// <summary>
		/// A descriptor pool creation has failed due to fragmentation.
		/// </summary>
		ErrorFragmentation = -1000161000,
		/// <summary>
		/// A buffer creation or memory allocation failed because the requested address is not available. A shader group handle assignment
		/// failed because the requested shader group handle information is no longer valid.
		/// </summary>
		ErrorInvalidOpaqueCaptureAddress = -1000257000,
		/// <summary>
		/// A surface is no longer available.
		/// </summary>
		ErrorSurfaceLostKHR = -1000000000,
		/// <summary>
		/// The requested window is already in use by Vulkan or another API in a manner which prevents it from being used again.
		/// </summary>
		ErrorNativeWindowInUseKHR = -1000000001,
		/// <summary>
		/// A swapchain no longer matches the surface properties exactly, but can still present to the surface successfully.
		/// </summary>
		SuboptimalKHR = 1000001003,
		/// <summary>
		/// A surface has changed in such a way that it is no longer compatible with the swapchain, and further presentation requests
		/// using the swapchain will fail. Applications must query the new surface properties and recreate their swapchain if they
		/// wish to continue presenting to the surface.
		/// </summary>
		ErrorOutOfDateKHR = -1000001004,
		/// <summary>
		/// A display used by a swapchain does not use the same presentable image layout, or is incompatible in some way that prevents
		/// sharing an image.
		/// </summary>
		ErrorIncompatibleDisplayKHR = -1000003001,
		ErrorValidationFailedEXT = -1000011001,
		/// <summary>
		/// One or more shaders failed to compile or link.
		/// </summary>
		ErrorInvalidShaderNV = -1000012000,
		ErrorIncompatibleVersionKHR = -1000150000,
		ErrorInvalidDRMFormatModifierPlaneLayoutEXT = -1000158000,
		ErrorNotPermittedEXT = -1000174001,
		ErrorFullScreenExclusiveLostEXT = -1000255000,
		/// <summary>
		/// A deferred operation is not complete but there is currently no work for this thread to do at the time of the call.
		/// </summary>
		ThreadIdleKHR = 1000268000,
		/// <summary>
		/// A deferred operation is not complete but there is no work remaining to assign to additional threads.
		/// </summary>
		ThreadDoneKHR = 1000268001,
		/// <summary>
		/// A deferred operation was requested and at least some of the work was deferred.
		/// </summary>
		OperationDeferredKHR = 1000268002,
		/// <summary>
		/// A deferred operation was requested and no operations were deferred.
		/// </summary>
		OperationNotDeferredKHR = 1000268003,
		/// <summary>
		/// A requested pipeline creation would have required compilation, but the application requested compilation not be performed.
		/// </summary>
		PipelineCompileRequiredEXT = 1000297000
	}

	public enum VKStructureType : int {
		ApplicationInfo = 0,
		InstanceCreateInfo = 1,
		DeviceQueueCreateInfo = 2,
		DeviceCreateInfo = 3,
		SubmitInfo = 4,
		MemoryAllocateInfo = 5,
		MappedMemoryRange = 6,
		BindSparseInfo = 7,
		FenceCreateInfo = 8,
		SemaphoreCreateInfo = 9,
		EventCreateInfo = 10,
		QueryPoolCreateInfo = 11,
		BufferCreateInfo = 12,
		BufferViewCreateInfo = 13,
		ImageCreateInfo = 14,
		ImageViewCreateInfo = 15,
		ShaderModuleCreateInfo = 16,
		PipelineCacheCreateInfo = 17,
		PipelineShaderStageCreateInfo = 18,
		PipelineVertexInputStateCreateInfo = 19,
		PipelineInputAssemblyStateCreateInfo = 20,
		PipelineTesselationStateCreateInfo = 21,
		PipelineViewportStateCreateInfo = 22,
		PipelineRasterizationStateCreateInfo = 23,
		PipelineMultisampleStateCreateInfo = 24,
		PipelineDepthStencilStateCreateInfo = 25,
		PipelineColorBlendStateCreateInfo = 26,
		PipelineDynamicStateCreateInfo = 27,
		GraphicsPipelineCreateInfo = 28,
		ComputePipelineCreateInfo = 29,
		PipelineLayoutCreateInfo = 30,
		SamplerCreateInfo = 31,
		DescriptorSetLayoutCreateInfo = 32,
		DescriptorPoolCreateInfo = 33,
		DescriptorSetAllocateInfo = 34,
		WriteDescriptorSet = 35,
		CopyDescriptorSet = 36,
		FramebufferCreateInfo = 37,
		RenderPassCreateInfo = 38,
		CommandPoolCreateInfo = 39,
		CommandBufferAllocateInfo = 40,
		CommandBufferInheritanceInfo = 41,
		CommandBufferBeginInfo = 42,
		RenderPassBeginInfo = 43,
		BufferMemoryBarrier = 44,
		ImageMemoryBarrier = 45,
		MemoryBarrier = 46,
		LoaderInstanceCreateInfo = 47,
		LoaderDeviceCreateInfo = 48,
		// Vulkan 1.1
		PhysicalDeviceSubgroupProperties = 1000094000,
		BindBufferMemoryInfo = 1000157000,
		BindImageMemoryInfo = 1000157001,
		PhysicalDevice16BitStorageFeatures = 1000083000,
		MemoryDedicatedRequirements = 1000127000,
		MemoryDedicatedAllocateInfo = 1000127001,
		MemoryAllocateFlagsInfo = 1000060000,
		DeviceGroupRenderPassBeginInfo = 1000060003,
		DeviceGroupCommandBufferBeginInfo = 1000060004,
		DeviceGroupSubmitInfo = 1000060005,
		DeviceGroupBindSparseInfo = 1000060006,
		BindBufferMemoryDeviceGroupInfo = 1000060013,
		BindImageMemoryDeviceGroupInfo = 1000060014,
		PhysicalDeviceGroupProperties = 1000070000,
		DeviceGroupDeviceCreateInfo = 1000070001,
		BufferMemoryRequirementsInfo2 = 1000146000,
		ImageMemoryRequirementsInfo2 = 1000146001,
		ImageSparseMemoryRequirements2 = 1000146002,
		MemoryRequirements2 = 1000146003,
		SparseImageMemoryRequirements2 = 1000146004,
		PhysicalDeviceFeatures2 = 1000059000,
		PhysicalDeviceProperties2 = 1000059001,
		FormatProperties2 = 1000059002,
		ImageFormatProperties2 = 1000059003,
		PhysicalDeviceImageFormaInfo2 = 1000059004,
		QueueFamilyProperties2 = 1000059005,
		PhysicalDeviceMemoryProperties2 = 1000059006,
		SparseImageFormatProperties2 = 1000059007,
		PhysicalDeviceSparseImageFormatInfo2 = 1000059008,
		PhysicalDevicePointClippingProperties = 1000117000,
		RenderPassInputAttachmentAspectCreateInfo = 1000117001,
		ImageViewUsageCreateInfo = 1000117002,
		PipelineTessellationDomainOriginStateCreateInfo = 1000117003,
		RnederPassMultiviewCreateInfo = 1000053000,
		PhysicalDeviceMultiviewFeatures = 1000053001,
		PhysicalDeviceMultiviewProperties = 1000053002,
		PhysicalDeviceVariablePointersFeatures = 1000120000,
		ProtectedSubmitInfo = 1000145000,
		PhysicalDeviceProtectedMemoryFeatures = 1000145001,
		PhysicalDeviceProtectedMemoryProperties = 1000145002,
		DeviceQueueInfo2 = 1000145003,
		SamplerYcbcrConversionCreateInfo = 1000156000,
		SamplerYcbcrConversionInfo = 1000156001,
		BindImagePlaneMemoryInfo = 1000156002,
		ImagePlaneMemoryRequirementsInfo = 1000156003,
		PhysicalDeviceSamplerYcbcrConversionFeatures = 1000156004,
		SamplerYcbcrConversionImageFormatProperties = 1000156005,
		DescriptorUpdateTemplateCreateInfo = 1000085000,
		PhysicalDeviceExternalImageFormatInfo = 1000071000,
		ExternalImageFormatProperties = 1000071001,
		PhysicalDeviceExternalBufferInfo = 1000071002,
		ExternalBufferProperties = 1000071003,
		PhysicalDeviceIDProperties = 1000071004,
		ExternalMemoryBufferCreateInfo = 1000072000,
		ExternalMemoryImageCreateInfo = 1000072001,
		ExportMemoryAllocateInfo = 1000072002,
		PhysicalDeviceExternalFenceInfo = 1000112000,
		ExternalFenceProperties = 1000112001,
		ExportFenceCreateInfo = 1000113000,
		ExportSemaphoreCreateInfo = 1000077000,
		PhysicalDeviceExternalSemaphoreInfo = 1000076000,
		ExternalSemaphoreProperties = 1000076001,
		PhysicalDeviceMaintenance3Properties = 1000168000,
		DescriptorSetLayoutSupport = 1000168001,
		PhysicalDeviceShaderDrawParametersFeatures = 1000063000,
		// Vulkan 1.2
		PhysicalDeviceVulkan1_1_Features = 49,
		PhysicalDeviceVulkan1_1_Properties = 50,
		PhysicalDeviceVulkan1_2_Features = 51,
		PhysicalDeviceVulkan1_2_Properties = 52,
		ImageFormatListCreateInfo = 1000147000,
		AttachmentDescription2 = 1000109000,
		AttachmentReference2 = 1000109001,
		SubpassDescription2 = 1000109002,
		SubpassDependency2 = 1000109003,
		RenderPassCreateInfo2 = 1000109004,
		SubpassBeginInfo = 1000109005,
		SubpassEndInfo = 1000109006,
		PhysicalDevice8BitStorageFeatures = 1000177000,
		PhysicalDeviceDriverProperties = 1000196000,
		PhysicalDeviceShaderAtomicInt64Features = 1000180000,
		PhysicalDeviceShaderFloat16Int8Features = 1000082000,
		PhysicalDeviceFloatControlsProperties = 1000197000,
		DescriptorSetLayoutBindingFlagsCreateInfo = 1000161000,
		PhysicalDeviceDescriptorIndexingFeatures = 1000161001,
		PhysicalDeviceDescriptorIndexingProperties = 1000161002,
		DescriptorSetVariableDescriptorCountAllocateInfo = 1000161003,
		DescriptorSetVariableDescriptorCountLayoutSupport = 1000161004,
		PhysicalDeviceDepthStencilResolveProperties = 1000199000,
		SubpassDescriptionDepthStencilResolve = 1000199001,
		PhysiaclDeviceScalarBlockLayoutFeatures = 1000221000,
		ImageStencilUsageCreateInfo = 1000246000,
		PhysicalDeviceSamplerFilterMinmaxProperties = 1000130000,
		SamplerReductionModeCreateInfo = 1000130001,
		PhysicalDeviceVulkanMemoryModelFeatures = 1000211000,
		PhysicalDeviceImagelessFramebufferFeatures = 1000108000,
		FramebufferAttachmentsCreateInfo = 1000108001,
		FramebufferAttachmentImageInfo = 1000108002,
		RenderPassAttachmentBeginInfo = 1000108003,
		PhysicalDeviceUniformBufferStandardLayoutFeatures = 1000253000,
		PhysicalDeviceShaderSubgroupExtendedTypesFeatures = 1000175000,
		PhysicalDeviceSeparateDepthStencilLayoutsFeatures = 1000241000,
		AttachmentReferenceStencilLayout = 1000241001,
		AttachmentDescriptionStencilLayout = 1000241002,
		PhysicalDeviceHostQueryResetFeatures = 1000261000,
		PhysicalDeviceTimelineSemaphoreFeatures = 1000207000,
		PhysicalDeviceTimelineSemaphoreProperties = 1000207001,
		SemaphoreTypeCreateInfo = 1000207002,
		TimelineSemaphoreSubmitInfo = 1000207003,
		SemaphoreWaitInfo = 1000207004,
		SemaphoreSignalInfo = 1000207005,
		PhysicalDeviceBufferDeviceAddressFeatures = 1000257000,
		BufferDeviceAddressInfo = 1000244001,
		BufferOpaqueCaptureAddressCreateInfo = 1000257002,
		MemoryOpaqueCaptureAddressAllocateInfo = 1000257003,
		DeviceMemoryOpaqueCaptureAddressInfo = 1000257004,
		// Vulkan 1.3
		PhysicalDeviceVulkan1_3_Features = 53,
		PhysicalDeviceVulkan1_3_Properties = 54,
		PipelineCreationFeedbackCreateInfo = 1000192000,
		PhysicalDeviceShaderTerminateInvocationFeatures = 1000215000,
		PhysicalDeviceToolProperties = 1000245000,
		PhysicalDeviceShaderDemoteToHelperInvocationFeatures = 1000276000,
		PhysicalDevicePrivateDataFeatures = 1000295000,
		DevicePrivateDataCreateInfo = 1000295001,
		PrivateDataSlotCreateInfo = 1000295002,
		PhysicalDevicePipelineCreationCacheControlFeatures = 1000297000,
		MemoryBarrier2 = 1000314000,
		BufferMemoryBarrier2 = 1000314001,
		ImageMemoryBarrier2 = 1000314002,
		DependencyInfo = 1000314003,
		SubmitInfo2 = 1000314004,
		SemaphoreSubmitInfo = 1000314005,
		CommandBufferSubmitInfo = 1000314006,
		PhysicalDeviceSynchronization2Features = 1000314007,
		PhysicalDeviceZeroInitializeWorkgroupMemoryFeatures = 1000325000,
		PhysicalDeviceImageRobustnessFeatures = 1000335000,
		CopyBufferInfo2 = 1000337000,
		CopyImageInfo2 = 1000337001,
		CopyBufferToImageInfo2 = 1000337002,
		CopyImageToBufferInfo2 = 1000337003,
		BlitImageInfo2 = 1000337004,
		ResolveImageInfo2 = 1000337005,
		BufferCopy2 = 1000337006,
		ImageCopy2 = 1000337007,
		ImageBlit2 = 1000337008,
		BufferImageCopy2 = 1000337009,
		ImageResolve2 = 1000337010,
		PhysicalDeviceSubgroupSizeControlProperties = 1000225000,
		PipelineShaderStageRequiredSubgroupSizeCreateInfo = 1000225001,
		PhysicalDeviceSubgroupSizeControlFeatures = 1000225002,
		PhysicalDeviceInlineUniformBlockFeatures = 1000138000,
		PhysicalDeviceInlineUniformBlockProperties = 1000138001,
		WriteDescriptorSetInlineUniformBlock = 1000138002,
		DescriptorPoolInlineUniformBlockCreateInfo = 1000138003,
		PhysicalDeviceTextureCompressionASTC_HDR_Features = 1000066000,
		RenderingInfo = 1000044000,
		RenderingAttachmentInfo = 1000044001,
		PipelineRenderingCreateInfo = 1000044002,
		PhysicalDeviceDynamicRenderingFeatures = 1000044003,
		CommandBufferInheritanceRenderingInfo = 1000044004,
		PhysicalDeviceShaderIntegerDotProductFeatures = 1000280000,
		PhysicalDeviceShaderIntegerDotProductProperties = 1000280001,
		PhysicalDeviceTexelBufferAlignmentProperties = 1000281001,
		FormatProperties3 = 1000360000,
		PhysicalDeviceMaintenance4Features = 1000413000,
		PhysicalDeviceMaintenance4Properties = 1000413001,
		DeviceBufferMemoryRequirements = 1000413002,
		DeviceImageMemoryRequirements = 1000413003,
		// VK_KHR_swapchain
		SwapchainCreateInfoKHR = 1000001000,
		PresentInfoKHR = 1000001001,
		// VK_KHR_swapchain w/ VK_KHR_device_group or Vulkan 1.1
		DeviceGroupPresentCapabilitiesKHR = 1000060007,
		ImageSwapchainCreateInfoKHR = 1000060008,
		BindImageMemorySwapchainInfoKHR = 1000060009,
		AcquireNextImageInfoKHR = 1000060010,
		DeviceGroupPresentInfoKHR = 1000060011,
		DeviceGroupSwapchainCreateInfoKHR = 1000060012,
		// VK_KHR_display
		DisplayModeCreateInfoKHR = 1000002000,
		DisplaySurfaceCreateInfoKHR = 1000002001,
		// VK_KHR_display_swapchain
		DisplayPresentInfoKHR = 1000003000,
		// VK_KHR_xlib_surface
		XLibSurfaceCreateInfoKHR = 1000004000, 
		// VK_KHR_xcb_surface
		XCBSurfaceCreateInfoKHR = 1000005000,
		// VK_KHR_wayland_surface
		WaylandSurfaceCreateInfoKHR = 1000006000,
		// VK_KHR_android_surface
		AndroidSurfaceCreateInfoKHR = 1000008000,
		// VK_KHR_win32_surface
		Win32SurfaceCreateInfoKHR = 1000009000,
		// VK_EXT_debug_report
		DebugReportCallbackCreateInfoEXT = 1000011000,
		// VK_AMD_rasterization_order
		PipelineRasterizationStateRasterizationOrderAMD = 1000018000,
		// VK_EXT_debug_marker
		DebugMarkerObjectNameInfoEXT = 1000022000,
		DebugMarkerObjectTagInfoEXT = 1000022001,
		DebugMarkerMarkerInfoEXT = 1000022002,
		// VK_KHR_video_queue (Beta)
		VideoProfileKHR = 1000023000,
		VideoCapabilitiesKHR = 1000023001,
		VideoPictureResourceKHR = 1000023002,
		VideoGetMemoryPropertiesKHR = 1000023003,
		VideoBindMemoryKHR = 1000023004,
		VideoSessionCreateInfoKHR = 1000023005,
		VideoSessionParametersCreateInfoKHR = 1000023006,
		VideoSessionParametersUpdateInfoKHR = 1000023007,
		VideoBeginCodingInfoKHR = 1000023008,
		VideoEndCodingInfoKHR = 1000023009,
		VideoCodingControlInfoKHR = 1000023010,
		VideoReferenceSlotKHR = 1000023011,
		VideoQueueFamilyPropertiesKHR = 1000023012,
		VideoProfilesKHR = 1000023013,
		PhysicalDeviceVideoFormatInfoKHR = 1000023014,
		VideoFormatPropertiesKHR = 1000023015,
		QueueFamilyQueryResultStatusProperties2KHR = 1000023016,
		// VK_KHR_video_decode_queue (Beta)
		VideoDecodeInfoKHR = 1000024000,
		VideoDecodeCapabilitiesKHR = 1000024001,
		// VK_NV_dedicated_allocation
		DedicatedAllocationImageCreateInfoNV = 1000026000,
		DedicatedAllocationBufferCreateInfoNV = 1000026001,
		DedicatedAllocationMemoryAllocateCreateInfoNV = 1000026002,
		// VK_EXT_transform_feedback
		PhysicalDeviceTransformFeedbackFeaturesEXT = 1000028000,
		PhysicalDeviceTransformFeedbackPropertiesEXT = 1000028001,
		PipelineRasterizationStateStreamCreateInfoEXT = 1000028002,
		// VK_NVX_binary_import
		CUModuleCreateInfoNVX = 1000029000,
		CUFunctionCreateInfoNVX = 1000029001,
		CULaunchInfoNVX = 1000029002,
		// VK_NVX_image_view_handle
		ImageViewHandleInfoNVX = 1000030000,
		ImageViewAddressPropertiesNVX = 1000030001,
		// VK_EXT_video_encode_h264 (Beta)
		VideoEncodeH264CapabilitiesEXT = 1000038000,
		VideoEncodeH264SessionParametersCreateInfoEXT = 1000038001,
		VideoEncodeH264SessionParametersAddInfoEXT = 1000038002,
		VideoEncodeH264VCLFrameInfoEXT = 1000038003,
		VideoEncodeH264DPBSlotInfoEXT = 1000038004,
		VideoEncodeH264NALUSliceEXT = 1000038005,
		VideoEncodeH264EmitPictureParametersEXT = 1000038006,
		VideoEncodeH264ProfileEXT = 1000038007,
		VideoEncodeH264RateControlInfoEXT = 1000038008,
		VideoEncodeH264RateControlLayerInfoEXT = 1000038009,
		VideoEncodeH264ReferenceListsEXT = 1000038010,
		// VK_EXT_video_encode_h265 (Beta)
		VideoEncodeH265CapabilitiesEXT = 1000039000,
		VideoEncodeH265SessionParametersCreateInfoEXT = 1000039001,
		VideoEncodeH265SessionParametersAddInfoEXT = 1000039002,
		VideoEncodeH265VCLFrameInfoEXT = 1000039003,
		VideoEncodeH265DPBSlotInfoEXT = 1000039004,
		VideoEncodeH265NALUSliceEXT = 1000039005,
		VideoEncodeH265EmitPictureParametersEXT = 1000039006,
		VideoEncodeH265ProfileEXT = 1000039007,
		VideoEncodeH265RateControlInfoEXT = 1000039008,
		VideoEncodeH265RateControlLayerInfoEXT = 1000039009,
		VideoEncodeH265ReferenceListsEXT = 1000039010,
		// VK_EXT_video_decode_h264
		VideoDecodeH264CapabilitiesEXT = 1000040000,
		VideoDecodeH264PictureInfoEXT = 1000040001,
		VideoDecodeH264MVC_EXT = 1000040002,
		VideoDecodeH264ProfileEXT = 1000040003,
		VideoDecodeH264SessionParametersCreateInfoEXT = 1000040004,
		VideoDecodeH264SessionParametersAddInfoEXT = 1000040005,
		VideoDecodeH264DPBSlotInfoEXT = 1000040006,
		// VK_AMD_texture_gather_bias_lod
		TextureLODGatherFormatPropertiesAMD = 1000041000,
		// VK_KHR_dynamic_rendering + VK_KHR_fragment_shading_rate
		RenderingFragmentShadingRateAttachmentInfoKHR = 1000044006,
		// VK_KHR_dynamic_rendering + VK_EXT_fragment_density_map
		RenderingFragmentDensityMapAttachmentInfoEXT = 1000044007,
		// VK_KHR_dynamic_rendering + VK_AMD_mixed_attachment_samples
		AttachmentSampleCountInfoKHR = 1000044008,
		// VK_KHR_dynamic_rendering + VK_NVX_multiview_per_view_attributes
		MultiviewPerViewAttributesInfoNVX = 1000044009,
		// VK_GGP_stream_descriptor_surface
		StreamDescriptorSurfaceCreateInfoGGP = 1000049000,
		// VK_NV_corner_sampled_image
		PhysicalDeviceCornerSampledImageFeaturesNV = 1000050000,
		// VK_NV_external_memory
		ExternalMemoryImageCreateInfoNV = 1000056000,
		ExportMemoryAllocateInfoNV = 1000056001,
		// VK_NV_external_memory_win32
		ImportMemoryWin32HandleInfoNV = 1000057000,
		ExportMemoryWin32HandleInfoNV = 1000057001,
		// VK_NV_win32_keyed_mutex
		Win32KeyedMutexAcquireReleaseInfoNV = 1000058000,
		// VK_EXT_validation_flags
		ValidationFlagsEXT = 1000061000,
		// VK_NN_vi_surface
		VISurfaceCreateInfoNN = 1000062000,
		// VK_EXT_astc_decode_mode
		ImageViewASTCDecodeModeEXT = 1000067000,
		PhysicalDeviceASTCDecodeFeaturesEXT = 1000067001,
		// VK_KHR_external_memory_win32
		ImportMemoryWin32HandleInfoKHR = 1000073000,
		ExportMemoryWin32HandleInfoKHR = 1000073001,
		MemoryWin32HandlePropertiesKHR = 1000073002,
		MemoryGetWin32HandleInfoKHR = 1000073003,
		// VK_KHR_external_memory_fd
		ImportMemoryFDInfoKHR = 1000074000,
		MemoryFDPropertiesKHR = 1000074001,
		MemoryGetFDInfoKHR = 1000074002,
		// VK_KHR_win32_keyed_mutex
		Win32KeyedMutexAcquireReleaseInfoKHR = 1000075000,
		// VK_KHR_external_semaphore_win32
		ImportSemaphoreWin32HandleInfoKHR = 1000078000,
		ExportSemaphoreWin32HandleInfoKHR = 1000078001,
		D3D12FenceSubmitInfoKHR = 1000078002,
		SemaphoreGetWin32HandleInfoKHR = 1000078003,
		// VK_KHR_external_semaphore_fd
		ImportSemaphoreFDInfoKHR = 1000079000,
		SemaphoreGetFDInfoKHR = 1000079001,
		// VK_KHR_push_descriptor
		PhysicalDevicePushDescriptorPropertiesKHR = 1000080000,
		// VK_EXT_conditional_rendering
		CommandBufferInheritanceConditionalRenderingInfoEXT = 1000081000,
		PhysicalDeviceConditionalRenderingFeaturesEXT = 1000081001,
		ConditionalRenderingBeginInfoEXT = 1000081002,
		// VK_KHR_incremental_present
		PresentRegionsKHR = 1000084000,
		// VK_NV_clip_space_w_scaling
		PipelineViewportWScalingStateCreateInfoNV = 1000087000,
		// VK_EXT_display_surface_counter
		SurfaceCapabilities2EXT = 1000090000,
		// VK_EXT_display_control
		DisplayPowerInfoEXT = 1000091000,
		DeviceEventInfoEXT = 1000091001,
		DisplayEventInfoEXT = 1000091002,
		SwapchainCounterCreateInfoEXT = 1000091003,
		// VK_GOOGLE_display_timing
		PresentTimesInfoGOOGLE = 1000092000,
		// VK_NVX_multiview_per_view_attributes
		PhysicalDeviceMultiviewPerViewAttributesPropertiesNVX = 1000097000,
		// VK_NV_viewport_swizzle
		PipelineViewportSwizzleStateCreateInfoNV = 1000098000,
		// VK_EXT_discard_rectangles
		PhysicalDeviceDiscardRectanglePropertiesEXT = 1000099000,
		PipelineDiscardRectangleStateCreateInfoEXT = 1000099001,
		// VK_EXT_conservative_rasterization
		PhysicalDeviceConservativeRasterizationPropertiesEXT = 1000101000,
		PipelineRasterizationConservativeStateCreateInfoEXT = 1000101001,
		// VK_EXT_depth_clip_enable
		PhysicalDeviceDepthClipEnableFeaturesEXT = 1000102000,
		PipelineRasterizationDepthClipStateCreateInfoEXT = 1000102001,
		// VK_EXT_hdr_metadata
		HDRMetadataEXT = 1000105000,
		// VK_KHR_shared_presentable_image
		SharedPresentSurfaceCapabilitiesKHR = 1000111000,
		// VK_KHR_external_fence_win32
		ImportFenceWin32HandleInfoKHR = 1000114000,
		ExportFenceWin32HandleInfoKHR = 1000114001,
		FenceGetWin32HandleInfoKHR = 1000114002,
		// VK_KHR_external_fence_fd
		ImportFenceFDInfoKHR = 1000115000,
		FenceGetFDInfoKHR = 1000115001,
		// VK_KHR_performance_query
		PhysicalDevicePerformanceQueryFeaturesKHR = 1000116000,
		PhysicalDevicePerformanceQueryPropertiesKHR = 1000116001,
		QueryPoolPerformanceCreateInfoKHR = 1000116002,
		PerformanceQuerySubmitInfoKHR = 1000116003,
		AcquireProfilingLockInfoKHR = 1000116004,
		PerformanceCounterKHR = 1000116005,
		PerformanceCounterDescriptionKHR = 1000116006,
		// VK_KHR_get_surface_capabilities2
		PhysicalDeviceSurfaceInfo2KHR = 1000119000,
		SurfaceCapabilities2KHR = 1000119001,
		SurfaceFormat2KHR = 1000119002,
		// VK_KHR_get_display_properties2
		DisplayProperties2KHR = 1000121000,
		DisplayPlaneProperties2KHR = 1000121001,
		DisplayModeProperties2KHR = 1000121002,
		DisplayPlaneInfo2KHR = 1000121003,
		DisplayPlaneCapabilities2KHR = 1000121004,
		// VK_MVK_ios_surface
		IOSSurfaceCreateInfoMVK = 1000122000,
		// VK_MVK_macos_surface
		MacOSSurfaceCreateInfoMVK = 1000123000,
		// VK_EXT_debug_utils
		DebugUtilsObjectNameInfoEXT = 1000128000,
		DebugUtilsObjectTagInfoEXT = 1000128001,
		DebugUtilsLabelEXT = 1000128002,
		DebugUtilsMessengerCallbackDataEXT = 1000128003,
		DebugUtilsMessengerCreateInfoEXT = 1000128004,
		// VK_ANDROID_external_memory_android_hardware_buffer
		AndroidHardwareBufferUsageANDROID = 1000129000,
		AndroidHadrwareBufferPropertiesANDROID = 1000129001,
		AndroidHardwareBufferFormatPropertiesANDROID = 1000129002,
		ImportAndroidHardwareBufferInfoANDROID = 1000129003,
		MemoryGetAndroidHardwareBufferInfoANDROID = 1000129004,
		ExternalFormatANDROID = 1000129005,
		// VK_KHR_format_feature_flags2 + VK_ANDROID_external_memory_android_hardware_buffer
		AndroidHardwareBufferFormatProperties2ANDROID = 1000129006,
		// VK_EXT_sample_locations
		SampleLocationsInfoEXT = 1000143000,
		RenderPassSampleLocationsBeginInfoEXT = 1000143001,
		PipelineSampleLocationsStateCreateInfoEXT = 1000143002,
		PhysicalDeviceSampleLocationsPropertiesEXT = 1000143003,
		MultisamplePropertiesEXT = 1000143004,
		// VK_EXT_blend_operation_advanced
		PhysicalDeviceBlendOperationAdvancedFeaturesEXT = 1000148000,
		PhysicalDeviceBlendOperationAdvancedPropertiesEXT = 1000148001,
		PipelineColorBlendAdvancedStateCreateInfoEXT = 1000148002,
		// VK_NV_fragment_coverage_to_color
		PipelineCoverageToColorStateCreateInfoNV = 1000149000,
		// VK_KHR_acceleration_structure
		AccelerationStructureBuildGeomtryInfoKHR = 1000150000,
		AccelerationStructureCreateGeometryTypeInfoKHR = 1000150001,
		AccelerationStructureDeviceAddressInfoKHR = 1000150002,
		AccelerationStructureGeometryAABBsDataKHR = 1000150003,
		AccelerationStructureGeometryInstancesDataKHR = 1000150004,
		AccelerationStructureGeometryTrianglesDataKHR = 1000150005,
		AccelerationStructureGeometryKHR = 1000150006,
		AccelerationStructureMemoryRequirementsInfoKHR = 1000150008,
		AccelerationStructureVersionKHR = 1000150009,
		CopyAccelerationStructureInfoKHR = 1000150010,
		CopyAccelerationStructureToMemoryInfoKHR = 1000150011,
		CopyMemoryToAccelerationStructureInfoKHR = 1000150012,
		PhysicalDeviceAccelerationStructureFeaturesKHR = 1000150013,
		PhysicalDeviceAccelerationStructurePropertiesKHR = 1000150014,
		AccelerationStructureCreateInfoKHR = 1000150017,
		AccelerationStructureBuildSizesInfoKHR = 1000150020,
		// VK_KHR_ray_tracing_pipeline
		PhysicalDeviceRayTracingPipelineFeaturesKHR = 1000347000,
		PhysicalDeviceRayTracingPipelinePropertiesKHR = 1000347001,
		RayTracingPipelineCreateInfoKHR = 1000150015,
		RayTracingShaderGroupCreateInfoKHR = 1000150016,
		RayTracingPipelineInterfaceCreateInfoKHR = 1000150018,
		// VK_KHR_ray_query
		PhysicalDeviceRayQueryFeaturesKHR = 1000348013,
		// VK_NV_framebuffer_mixed_samples
		PipelineCoverageModulationStateCreateInfoNV = 1000152000,
		// VK_NV_shader_sm_builtins
		PhysicalDeviceShaderSMBuiltinsFeaturesNV = 1000154000,
		PhysicalDeviceShaderSMBuiltinsPropertiesNV = 1000154001,
		// VK_EXT_image_drm_format_modifier
		DRMFormatModifierPropertiesListEXT = 1000158000,
		DRMFormatModifierPropertiesEXT = 1000158001,
		PhysicalDeviceImageDRMFormatModifierInfoEXT = 1000158002,
		ImageDRMFormatModifierListCreateInfoEXT = 1000158003,
		ImageDRMFormatModifierExplicitCreateInfoEXT = 1000158004,
		ImageDRMFormatModifierPropertiesEXT = 1000158005,
		// VK_KHR_format_feature_flags2 + VK_EXT_image_drm_format_modifier
		DRMFormatModifierPropertiesList2EXT = 1000158006,
		// VK_EXT_validation_cache
		ValidationCacheCreateInfoEXT = 1000160000,
		ShaderModuleValidationCacheCreateInfoEXT = 1000160001,
		// VK_KHR_portability_subset (Beta)
		PhysicalDevicePortabilitySubsetFeaturesKHR = 1000163000,
		PhysicalDevicePortabilitySubsetPropertiesKHR = 1000163001,
		// VK_NV_shading_rate_image
		PipelineViewportShadingRateImageStateCreateInfoNV = 1000164000,
		PhysicalDeviceShadingRateImageFeaturesNV = 1000164001,
		PhysicalDeviceShadingRateImagePropertiesNV = 1000164002,
		PipelineViewportCoarseSampleOrderStateCreateInfoNV = 1000164005,
		// VK_NV_ray_tracing
		RayTracingPipelineCreateInfoNV = 1000165000,
		AccelerationStructureCreateInfoNV = 1000165001,
		GeometryNV = 1000165003,
		GeometryTrianglesNV = 1000165004,
		GeometryAABB_NV = 1000165005,
		BindAccelerationStructureMemoryInfoNV = 1000165006,
		WriteDescriptorSetAccelerationStructureNV = 1000165007,
		AccelerationStructureMemoryRequirementsInfoNV = 1000165008,
		PhysicalDeviceRayTracingPropertiesNV = 1000165009,
		RayTracingShaderGroupCreateInfoNV = 1000165011,
		AccelerationStructureInfoNV = 1000165012,
		// VK_NV_representative_fragment_test
		PhysicalDeviceRepresentativeFragmentTestFeaturesNV = 1000166000,
		PipelineRepresentativeFragmentTestStateCreateInfoNV = 1000166001,
		PhysicalDeviceImageViewImageFormatInfoEXT = 1000170000,
		// VK_EXT_filter_cubic
		FilterCubicImageViewFormatPropertiesEXT = 1000170001,
		// VK_EXT_external_memory_host
		ImportMemoryHostPointerInfoEXT = 1000178000,
		MemoryHostPointerPropertiesEXT = 1000178001,
		PhysicalDeviceExternalMemoryHostPropertiesEXT = 1000178002,
		// VK_KHR_shader_clock
		PhysicalDeviceShaderClockFeaturesKHR = 1000181000,
		// VK_AMD_pipeline_compiler_control
		PipelineCompilerControlCreateInfoAMD = 1000183000,
		// VK_EXT_calibrated_timestamps
		CalibratedTimestampInfoEXT = 1000184000,
		// VK_AMD_shader_core_properties
		PhysicalDeviceShaderCorePropertiesAMD = 1000185000,
		// VK_EXT_video_decode_h265 (Beta)
		VideoDecodeH265CapabilitiesEXT = 1000187000,
		VideoDecodeH265SessionParametersCreateInfoEXT = 1000187001,
		VideoDecodeH265SessionParametersAddInfoEXT = 1000187002,
		VideoDecodeH265ProfileEXT = 1000187003,
		VideoDecodeH265PictureInfoEXT = 1000187004,
		VideoDecodeH265DPBSlotInfoEXT = 1000187005,
		// VK_KHR_global_priority
		DeviceQueueGlobalPriorityCreateInfoKHR = 1000174000,
		PhysicalDeviceGlobalPriorityQueryFeaturesKHR = 1000388000,
		QueueFamilyGlobalPriorityQueryFeaturesKHR = 1000388001,
		// VK_AMD_memory_overallocation_behavior
		DeviceMemoryOverallocationCreateInfoAMD = 1000189000,
		// VK_EXT_vertex_attribute_divisor
		PhysicalDeviceVertexAttributeDivisorPropertiesEXT = 1000190000,
		PipelineVertexInputDivisorStateCreateInfoEXT = 1000190001,
		PhysicalDeviceVertexAttributeDivisorFeaturesEXT = 1000190002,
		// VK_GGP_frame_token
		PresentFrameTokenGGP = 1000191000,
		// VK_NV_compute_shader_derivatives
		PhysicalDeviceComputeShaderDerivativesFeaturesNV = 1000201000,
		// VK_NV_mesh_shader
		PhysicalDeviceMeshShaderFeaturesNV = 1000202000,
		PhysicalDeviceMeshShaderPropertiesNV = 1000202001,
		// VK_NV_shader_image_footprint
		PhysicalDeviceShaderImageFootprintFeaturesNV = 1000204000,
		// VK_NV_scissor_exclusive
		PipelineViewportExclusiveScissorStateCreateInfoNV = 1000205000,
		PhysicalDeviceExclusiveScissorFeaturesNV = 1000205002,
		// VK_NV_device_diagnostic_checkpoints
		CheckpointDataNV = 1000206000,
		QueueFamilyCheckpointPropertiesNV = 1000206001,
		// VK_INTEL_shdaer_integer_functions2
		PhysicalDeviceShaderIntegerFunctions2FeaturesINTEL = 1000209000,
		// VK_INTEL_performance_query
		QueryPoolPerformanceQueryCreateInfoINTEL = 1000210000,
		InitializePerformanceAPIInfoIntel = 1000210001,
		PerformanceMarkerInfoINTEL = 1000210002,
		PerformanceStreamMarkerInfoINTEL = 1000210003,
		PerformanceOverrideInfoINTEL = 1000210004,
		PerformanceConfigurationAcquireInfoINTEL = 1000210005,
		// VK_EXT_pci_bus_info
		PhysicalDevicePCIBusInfoPropertiesEXT = 1000212000,
		// VK_AMD_display_native_hdr
		DisplayNativeHDRSurfaceCapabilitiesAMD = 1000213000,
		SwapchainDisplayNativeHDRCreateInfoAMD = 1000213001,
		// VK_FUCHISA_imagepipe_surface
		ImagepipeSurfaceCreateInfoFUCHISA = 1000214000,
		// VK_EXT_metal_surface
		MetalSurfaceCreateInfoEXT = 1000217000,
		// VK_EXT_fragment_density_map
		PhysicalDeviceFragmentDensityMapFeaturesEXT = 1000218000,
		PhysicalDeviceFragmentDensityMapPropertiesEXT = 1000218001,
		RenderPassFragmentDensityMapCreateInfoEXT = 1000218002,
		// VK_KHR_fragment_shading_rate
		FragmentShadingRateAttachmentInfoKHR = 1000226000,
		PipelineFragmentShadingRateStateCreateInfoKHR = 1000226001,
		PhysicalDeviceFragmentRateShadingPropertiesKHR = 1000226002,
		PhysicalDeviceFragmentRateShadingFeaturesKHR = 1000226003,
		PhysicalDeviceFramgentShadingRateKHR = 1000226004,
		// VK_AMD_shader_core_properties2
		PhysicalDeviceShaderCoreProperties2AMD = 1000227000,
		// VK_AMD_device_coherent_memory
		PhysicalDeviceCoherentMemoryFeaturesAMD = 1000229000,
		// VK_EXT_shader_image_atomic_int64
		PhysicalDeviceShaderImageAtomicInt64FeaturesEXT = 1000234000,
		// VK_EXT_memory_budget
		PhysicalDeviceMemoryBudgetPropertiesEXT = 1000237000,
		// VK_EXT_memory_priority
		PhysicalDeviceMemoryPriorityFeaturesEXT = 1000238000,
		MemoryPriorityAllocateInfoEXT = 1000238001,
		// VK_KHR_surface_protected_capabilities
		SurfaceProtectedCapabilitiesKHR = 1000239000,
		// VK_NV_dedicated_allocation_image_aliasing
		PhysicalDeviceDedicatedAllocationImageAliasingFeaturesNV = 1000240000,
		// VK_EXT_buffer_device_address
		PhysicalDeviceBufferDeviceAddressFeaturesEXT = 1000244000,
		BufferDeviceAddressCreateInfoEXT = 1000244002,
		// VK_EXT_validation_features
		ValidationFeaturesEXT = 1000247000,
		// VK_KHR_present_wait
		PhysicalDevicePresentWaitFeaturesKHR = 1000248000,
		// VK_NV_cooperative_matrix
		PhysicalDeviceCooperativeMatrixFeaturesNV = 1000249000,
		CooperativeMatrixPropertiesNV = 1000249001,
		PhysicalDeviceCooperativeMatrixPropertiesNV = 1000249002,
		// VK_NV_coverage_reduction_mode
		PhysicalDeviceCoverageReductionModeFeaturesNV = 1000250000,
		PipelineCoverageReductionStateCreateInfoNV = 1000250001,
		FramebufferMixedSamplesCombinationNV = 1000250002,
		// VK_EXT_fragment_shader_interlock
		PhysicalDeviceFragmentShaderInterlockFeaturesEXT = 1000251000,
		// VK_EXT_ycbcr_image_arrays
		PhysicalDeviceYcbcrImageArraysFeaturesEXT = 1000252000,
		// VK_EXT_provoking_vertex
		PhysicalDeviceProvokingVertexFeaturesEXT = 1000254000,
		PipelineRasterizationProvokingVertexStateCreateInfoEXT = 1000254001,
		PhysicalDeviceProvokingVertexPropertiesEXT = 1000254002,
		// VK_EXT_full_screen_exclusive
		SurfaceFullScreenExclusiveInfoEXT = 1000255000,
		SurfaceCapabilitiesFullScreenExclusiveEXT = 1000255002,
		// VK_KHR_win32_surface + VK_EXT_full_screen_exclusive
		SurfaceFullScreenExclusiveWin32InfoEXT = 1000255001,
		// VK_EXT_headless_surface
		HeadlessSurfaceCreateInfoEXT = 1000256000,
		// VK_EXT_line_rasterization
		PhysicalDeviceLineRasterizationFeaturesEXT = 1000259000,
		PipelineRasterizationLineStateCreateInfoEXT = 1000259001,
		PhysicalDeviceLineRasterizationPropertiesEXT = 1000259002,
		// VK_EXT_shader_atomic_float
		PhysicalDeviceShaderAtomicFloatFeaturesEXT = 1000260000,
		// VK_EXT_index_type_uint8
		PhysicalDeviceIndexTypeUInt8FeaturesEXT = 1000265000,
		// VK_EXT_extended_dynamic_state
		PhysicalDeviceExtendedDynamicStateFeaturesEXT = 1000267000,
		// VK_KHR_pipeline_executable_properties
		PhysicalDevicePipelineExecutablePropertiesFeaturesKHR = 1000269000,
		PipelineInfoKHR = 1000269001,
		PipelineExecutablePropertiesKHR = 1000269002,
		PipelineExecutableInfoKHR = 1000269003,
		PipelineExecutableStatisticKHR = 1000269004,
		PipelineExecutableInternalRepresentationKHR = 1000269005,
		// VK_EXT_shader_atomic_float2
		PhysicalDeviceShaderAtomicFloat2FeaturesEXT = 1000273000,
		// VK_NV_device_generated_commands
		PhysicalDeviceDeviceGeneratedCommandsPropertiesNV = 1000277000,
		GraphicsShaderGroupCreateInfoNV = 1000277001,
		GraphicsPipelineShaderGroupsCreateInfoNV = 1000277002,
		IndirectCommandsLayoutTokenNV = 1000277003,
		IndirectCommandsLayoutCreateInfoNV = 1000277004,
		GeneratedCommandsInfoNV = 1000277005,
		GeneratedCommandsMemoryRequirementsInfoNV = 1000277006,
		PhysicalDeviceGeneratedCommandsFeaturesNV = 1000277007,
		// VK_NV_inherited_viewport_scissor
		PhysicalDeviceInheritedViewportScissorFeaturesNV = 1000278000,
		CommandBufferInheritanceViewportScissorInfoNV = 1000278001,
		// VK_EXT_texel_buffer_alignment
		PhysicalDeviceTexelBufferAlignmentFeaturesEXT = 1000281000,
		// VK_QCOM_render_pass_transform
		CommandBufferInheritanceRenderPassTransformInfoQCOM = 1000282000,
		RenderPassTransformBeginInfoQCOM = 1000282001,
		// VK_EXT_device_memory_report
		PhysicalDeviceDeviceMemoryReportFeaturesEXT = 1000284000,
		DeviceDeviceMemoryReportCreateInfoEXT = 1000284001,
		DeviceMemoryReportCallbackDataEXT = 1000284002,
		// VK_EXT_robustness2
		PhysicalDeviceRobustness2FeaturesEXT = 1000286000,
		PhysicalDeviceRobustness2PropertiesEXT = 1000286001,
		// VK_EXT_custom_border_color
		SamplerCustomBorderColorCreateInfoEXT = 1000287000,
		PhysicalDeviceCustomBorderColorPropertiesEXT = 1000287001,
		PhysicalDeviceCustomBorderColorFeaturesEXT = 1000287002,
		// VK_KHR_pipeline_library
		PipelineLibraryCreateInfoKHR = 1000290000,
		// VK_KHR_present_id
		PresentIdKHR = 1000294000,
		PhysicalDevicePresentIdFeaturesKHR = 1000294001,
		// VK_KHR_video_encode_queue (Beta)
		VideoEncodeInfoKHR = 1000299000,
		VideoEncodeRateControlInfoKHR = 1000299001,
		VideoEncodeRateControlLayerInfoKHR = 1000299002,
		VideoEncodeCapabilitiesKHR = 1000299003,
		// VK_NV_device_diagnostics_config
		PhysicalDeviceDiagnosticsConfigFeaturesNV = 1000300000,
		DeviceDiagnosticsConfigCreateInfoNV = 1000300001,
		// VK_KHR_synchronization2 + VK_NV_device_diagnostic_checkpoints
		QueueFamilyCheckpointProperties2NV = 1000314008,
		CheckpointData2NV = 1000314009,
		// VK_EXT_graphics_pipeline_library
		PhysicalDeviceGraphicsPipelineLibraryFeaturesEXT = 1000320000,
		PhysicalDeviceGraphicsPipelineLibraryPropertiesEXT = 1000320001,
		GraphicsPipelineLibraryCreateInfoEXT = 1000320002,
		// VK_AMD_shader_early_late_fragment_tests
		PhysicalDeviceShaderEarlyLateFragmentTestsFeaturesAMD = 1000321000,
		// VK_KHR_fragment_shader_barycentric
		PhysicalDeviceFragmentShaderBarycentricFeaturesKHR = 1000203000,
		PhysicalDeviceFragmentShaderBarycentricPropertiesKHR = 1000322000,
		// VK_KHR_shader_subgroup_uniform_flow_control
		PhysicalDeviceShaderSubgroupUniformControlFlowFeaturesKHR = 1000323000,
		// VK_NV_fragment_shading_rate_enums
		PhysicalDeviceFragmentShadingRateEnumsPropertiesNV = 1000326000,
		PhysicalDeviceFragmentShadingRateEnumsFeaturesNV = 1000326001,
		PipelineFragmentShadingRateEnumStateCreateInfoNV = 1000326002,
		// VK_NV_ray_tracing_motion_blur
		AccelerationStructureGeometryMotionTrianglesDataNV = 1000327000,
		PhysicalDeviceRayTracingMotionBlurFeaturesNV = 1000327001,
		AccelerationStructureMotionInfoNV = 1000327002,
		// VK_EXT_ycbcr_2plane_444_formats
		PhysicalDeviceYcbcr2Plane444FormatsFeaturesEXT = 1000330000,
		// VK_EXT_fragment_density_map2
		PhysicalDeviceFragmentDensityMap2FeaturesEXT = 1000332000,
		PhysicalDeviceFragmentDensityMap2PropertiesEXT = 1000332001,
		// VK_QCOM_rotated_copy_commands
		CopyCommandTransformInfoQCOM = 1000333000,
		// VK_KHR_workgroup_memory_explicit_layout
		PhysicalDeviceWorkgroupMemoryExplicitLayoutFeaturesKHR = 1000336000,
		// VK_EXT_image_compression_control
		PhysicalDeviceImageCompressionControlFeaturesEXT = 1000338000,
		ImageCompressionControlEXT = 1000338001,
		SubresourceLayout2EXT = 1000338002,
		ImageSubresource2EXT = 1000338003,
		ImageCompressionPropertiesEXT = 1000338004,
		// VK_EXT_4444_formats
		PhysicalDevice4444FormatsFeaturesEXT = 1000340000,
		// VK_ARM_rasterization_order_attachment_access
		PhysicalDeviceRasterizationOrderAttachmentAccessFeaturesARM = 1000342000,
		// VK_EXT_rgba10x6_formats
		PhysicalDeviceRGBA10X6FormatsFeaturesEXT = 1000344000,
		// VK_EXT_directfb_surface
		DirectFBSurfaceCreateInfoEXT = 1000346000,
		// VK_VALVE_mutable_descriptor_type
		PhysicalDeviceMutableDescriptorTypeFeaturesVALVE = 1000351000,
		MutableDescriptorTypeCreateInfoVALVE = 1000351002,
		// VK_EXT_vertex_input_dynamic_state
		PhysicalDeviceVertexInputDynamicStateFeaturesEXT = 1000352000,
		VertexInputBindingDescription2EXT = 1000352001,
		VertexInputAttributeDescription2EXT = 1000352002,
		// VK_EXT_physical_device_drm
		PhysicalDeviceDRMPropertiesEXT = 1000353000,
		// VK_EXT_depth_clip_control
		PhysicalDeviceDepthClipControlFeaturesEXT = 1000355000,
		PipelineViewportDepthClipControlCreateInfoEXT = 1000355001,
		// VK_EXT_primitive_topology_list_restart
		PhysicalDevicePrimitiveTopologyListRestartFeaturesEXT = 1000356000,
		// VK_FUCHISA_external_memory
		ImportMemoryZirconHandleInfoFUCHISA = 1000364000,
		MemoryZirconHandlePropertiesFUCHISA = 1000364001,
		MemoryGetZirconHandleInfoFUCHISA = 1000364002,
		ImportSemaphoreZirconHandleInfoFUCHISA = 1000365000,
		SemaphoreGetZirconHandleInfoFUCHISA = 1000365001,
		// VK_FUCHISA_buffer_collection
		BufferCollectionCreateInfoFUCHISA = 1000366000,
		ImportMemoryBufferCollectionFUCHISA = 1000366001,
		BufferCollectionImageCreateInfoFUCHISA = 1000366002,
		BufferCollectionPropertiesFUCHISA = 1000366003,
		BufferConstraintsInfoFUCHISA = 1000366004,
		BufferCollectionBufferCreateInfoFUCHISA = 1000366005,
		ImageConstraintsInfoFUCHISA = 1000366006,
		ImageFormatConstraintsInfoFUCHISA = 1000366007,
		SystemColorSpaceFUCHISA = 1000366008,
		BufferCollectionConstraintsInfoFUCHISA = 1000366009,
		// VK_HUAWEI_subpass_shading
		SubpassShadingPipelineCreateInfoHUAWEI = 1000369000,
		PhysicalDeviceSubpassShadingFeaturesHUAWEI = 1000369001,
		PhysicalDeviceSubpassShadingPropertiesHUAWEI = 1000369002,
		// VK_HUAWEI_invocation_mask
		PhysicalDeviceInvocationMaskFeaturesHUAWEI = 1000370000,
		// VK_NV_external_memory_rdma
		MemoryGetRemoteAddressInfoNV = 1000371000,
		PhysicalDeviceExternalMemoryRDMAFeaturesNV = 1000371001,
		// VK_EXT_pipeline_properties
		PipelinePropertiesIdentifierEXT = 1000372000,
		PhysicalDevicePipelinePropertiesFeaturesEXT = 1000372001,
		// VK_EXT_extended_dynamic_state2
		PhysicalDeviceExtendedDynamicState2FeaturesEXT = 1000377000,
		// VK_QNX_screen_surface
		ScreenSurfaceCreateInfoQNX = 1000378000,
		// VK_EXT_color_write_enable
		PhysicalDeviceColorWriteEnableFeaturesEXT = 1000381000,
		PipelineColorWriteCreateInfoEXT = 1000381001,
		// VK_EXT_primitives_generated_query
		PhysicalDevicePrimitivesGeneratedQueryFeaturesEXT = 1000382000,
		// VK_KHR_ray_tracing_maintenance1
		PhysicalDeviceRayTracingMaintenance1FeatuersKHR = 1000386000,
		// VK_EXT_image_view_min_lod
		PhysicalDeviceImageViewMinLodFeaturesEXT = 1000391000,
		ImageViewMinLodCreateInfoEXT = 1000391001,
		// VK_EXT_multi_draw
		PhysicalDeviceMultiDrawFeaturesEXT = 1000392000,
		PhysicalDeviceMultiDrawPropertiesEXT = 1000392001,
		// VK_EXT_image_2d_view_of_3d
		PhysicalDeviceImage2DViewOf3DFeaturesEXT = 1000393000,
		// VK_EXT_border_color_swizzle
		PhysicalDeviceBorderColorSwizzleFeaturesEXT = 1000411000,
		SamplerBorderColorComponentMappingCreateInfoEXT = 1000411001,
		// VK_EXT_pageable_device_local_memory
		PhysicalDevicePageableDeviceLocalMemoryFeaturesEXT = 1000412000,
		// VK_VALVE_descriptor_set_host_mapping
		PhysicalDeviceDescriptorSetHostMappingFeaturesVALVE = 1000420000,
		DescriptorSetBindingReferenceVALVE = 1000420001,
		DescriptorSetLayoutHostMappingInfoVALVE = 1000420002,
		// VK_QCOM_fragment_density_map_offset
		PhysicalDeviceFragmentDensityMapOffsetFeaturesQCOM = 1000425000,
		PhysicalDeviceFragmentDensityMapOffsetPropertiesQCOM = 1000425001,
		SubpassFragmentDensityMapOffsetEndInfoQCOM = 1000425002,
		// VK_NV_linear_color_attachment
		PhysicalDeviceLinearColorAttachmentFeaturesNV = 1000430000,
		// VK_EXT_image_compression_control_swapchain
		PhysicalDeviceImageCompressionControlSwapchainFeaturesEXT = 1000437000,
		// VK_EXT_subpass_merge_feedback
		PhysicalDeviceSubpassMergeFeedbackFeaturesEXT = 1000458000,
		RenderPassCreationControlEXT = 1000458001,
		RenderPassCreationFeedbackCreateInfoEXT = 1000458002,
		RenderPassSubpassFeedbackCreateInfoEXT = 1000458003
	}

	public enum VKSystemAllocationScope : int {
		Command = 0,
		Object = 1,
		Cache = 2,
		Device = 3,
		Instance = 4
	}

	public enum VKInternalAllocationType : int {
		Executable = 0
	}

	public enum VKFormat : int {
		// Vulkan 1.0
		Undefined = 0,
		R4G4UnormPack8 = 1,
		R4G4B4A4UNormPack16 = 2,
		B4G4R4A4UNormPack16 = 3,
		R5G6B5UNormPack16 = 4,
		B5G6R5UNormPack16 = 5,
		R5G5B5A1UNormPack16 = 6,
		B5G5R5A1UNormPack16 = 7,
		A1R5G5B5UNormPack16 = 8,
		R8UNorm = 9,
		R8SNorm = 10,
		R8UScaled = 11,
		R8SScaled = 12,
		R8UInt = 13,
		R8SInt = 14,
		R8SRGB = 15,
		R8G8UNorm = 16,
		R8G8SNorm = 17,
		R8G8UScaled = 18,
		R8G8SScaled = 19,
		R8G8UInt = 20,
		R8G8SInt = 21,
		R8G8SRGB = 22,
		R8G8B8UNorm = 23,
		R8G8B8SNorm = 24,
		R8G8B8UScaled = 25,
		R8G8B8SScaled = 26,
		R8G8B8UInt = 27,
		R8G8B8SInt = 28,
		R8G8B8SRGB = 29,
		B8G8R8UNorm = 30,
		B8G8R8SNorm = 31,
		B8G8R8UScaled = 32,
		B8G8R8SScaled = 33,
		B8G8R8UInt = 34,
		B8G8R8SInt = 35,
		B8G8R8SRGB = 36,
		R8G8B8A8UNorm = 37,
		R8G8B8A8SNorm = 38,
		R8G8B8A8UScaled = 39,
		R8G8B8A8SScaled = 40,
		R8G8B8A8UInt = 41,
		R8G8B8A8SInt = 42,
		R8G8B8A8SRGB = 43,
		B8G8R8A8UNorm = 44,
		B8G8R8A8SNorm = 45,
		B8G8R8A8UScaled = 46,
		B8G8R8A8SScaled = 47,
		B8G8R8A8UInt = 48,
		B8G8R8A8SInt = 49,
		B8G8R8A8SRGB = 50,
		A8B8G8R8UNormPack32 = 51,
		A8B8G8R8SNormPack32 = 52,
		A8B8G8R8UScaledPack32 = 53,
		A8B8G8R8SScaledPack32 = 54,
		A8B8G8R8UIntPack32 = 55,
		A8B8G8R8SIntPack32 = 56,
		A8B8G8R8SRGBPack32 = 57,
		A2R10G10B10UNormPack32 = 58,
		A2R10G10B10SNormPack32 = 59,
		A2R10G10B10UScaledPack32 = 60,
		A2R10G10B10SScaledPack32 = 61,
		A2R10G10B10UIntPack32 = 62,
		A2R10G10B10SIntPack32 = 63,
		A2B10G10R10UNormPack32 = 64,
		A2B10G10R10SNormPack32 = 65,
		A2B10G10R10UScaledPack32 = 66,
		A2B10G10R10SScaledPack32 = 67,
		A2B10G10R10UIntPack32 = 68,
		A2B10G10R10SIntPack32 = 69,
		R16UNorm = 70,
		R16SNorm = 71,
		R16UScaled = 72,
		R16SScaled = 73,
		R16UInt = 74,
		R16SInt = 75,
		R16SFloat = 76,
		R16G16UNorm = 77,
		R16G16SNorm = 78,
		R16G16UScaled = 79,
		R16G16SScaled = 80,
		R16G16UInt = 81,
		R16G16SInt = 82,
		R16G16SFloat = 83,
		R16G16B16UNorm = 84,
		R16G16B16SNorm = 85,
		R16G16B16UScaled = 86,
		R16G16B16SScaled = 87,
		R16G16B16UInt = 88,
		R16G16B16SInt = 89,
		R16G16B16SFloat = 90,
		R16G16B16A16UNorm = 91,
		R16G16B16A16SNorm = 92,
		R16G16B16A16UScaled = 93,
		R16G16B16A16SScaled = 94,
		R16G16B16A16UInt = 95,
		R16G16B16A16SInt = 96,
		R16G16B16A16SFloat = 97,
		R32UInt = 98,
		R32SInt = 99,
		R32SFloat = 100,
		R32G32UInt = 101,
		R32G32SInt = 102,
		R32G32SFloat = 103,
		R32G32B32UInt = 104,
		R32G32B32SInt = 105,
		R32G32B32SFloat = 106,
		R32G32B32A32UInt = 107,
		R32G32B32A32SInt = 108,
		R32G32B32A32SFloat = 109,
		R64UInt = 110,
		R64SInt = 111,
		R64SFloat = 112,
		R64G64UInt = 113,
		R64G64SInt = 114,
		R64G64SFloat = 115,
		R64G64B64UInt = 116,
		R64G64B64SInt = 117,
		R64G64B64SFloat = 118,
		R64G64B64A64UInt = 119,
		R64G64B64A64SInt = 120,
		R64G64B64A64SFloat = 121,
		B10G11R11UFloatPack32 = 122,
		E5B9G9R9UFloatPack32 = 123,
		D16UNorm = 124,
		X8D24UNormPack32 = 125,
		D32SFloat = 126,
		S8UInt = 127,
		D16UNormS8UInt = 128,
		D24UNormS8UInt = 129,
		D32SFloatS8UInt = 130,
		BC1RGBUNormBlock = 131,
		BC1RGBSRGBBlock = 132,
		BC1RGBAUNormBlock = 133,
		BC1RGBASRGBBlock = 134,
		BC2UNormBlock = 135,
		BC2SRGBBlock = 136,
		BC3UNormBlock = 137,
		BC3SRGBBlock = 138,
		BC4UNormBlock = 139,
		BC4SNormBlock = 140,
		BC5UNormBlock = 141,
		BC5SNormBlock = 142,
		BC6HUFloatBlock = 143,
		BC6HSFloatBlock = 144,
		BC7UNormBlock = 145,
		BC7SRGBBlock = 146,
		ETC2R8G8B8UNormBlock = 147,
		ETC2R8G8B8SRGBBlock = 148,
		ETC2R8G8B8A1UNormBlock = 149,
		ETC2R8G8B8A1SRGBBlock = 150,
		ETC2R8G8B8A8UNormBlock = 151,
		ETC2R8G8B8A8SRGBBlock = 152,
		EACR11UNormBlock = 153,
		EACR11SNormBlock = 154,
		EACR11G11UNormBlock = 155,
		EACR11G11SNormBlock = 156,
		ASTC4x4UNormBlock = 157,
		ASTC4x4SRGBBlock = 158,
		ASTC5x4UNormBlock = 159,
		ASTC5x4SRGBBlock = 160,
		ASTC5x5UNormBlock = 161,
		ASTC5x5SRGBBlock = 162,
		ASTC6x5UNormBlock = 163,
		ASTC6x5SRGBBlock = 164,
		ASTC6x6UNormBlock = 165,
		ASTC6x6SRGBBlock = 166,
		ASTC8x5UNormBlock = 167,
		ASTC8x5SRGBBlock = 168,
		ASTC8x6UNormBlock = 169,
		ASTC8x6SRGBBlock = 170,
		ASTC8x8UNormBlock = 171,
		ASTC8x8SRGBBlock = 172,
		ASTC10x5UNormBlock = 173,
		ASTC10x5SRGBBlock = 174,
		ASTC10x6UNormBlock = 175,
		ASTC10x6SRGBBlock = 176,
		ASTC10x8UNormBlock = 177,
		ASTC10x8SRGBBlock = 178,
		ASTC10x10UNormBlock = 179,
		ASTC10x10SRGBBlock = 180,
		ASTC12x10UNormBlock = 181,
		ASTC12x10SRGBBlock = 182,
		ASTC12x12UNormBlock = 183,
		ASTC12x12SRGBBlock = 184,
		// Vulkan 1.1
		G8B8G8R8_422UNorm = 1000156000,
		B8G8R8G8_422UNorm = 1000156001,
		G8_B8_R8_3Plane420UNorm = 1000156002,
		G8_B8R8_2Plane420UNorm = 1000156003,
		G8_B8_R8_3Plane422UNorm = 1000156004,
		G8_B8R8_2Plane422UNorm = 1000156005,
		G8_B8_R8_3Plane444UNorm = 1000156006,
		R10X6UNormPack16 = 1000156007,
		R10X6G10X6UNorm_2Pack16 = 1000156008,
		R10X6G10X6B10X6A10X6UNorm_4Pack16 = 1000156009,
		G10X6B10X6G10X6R10X6_422UNorm_4Pack16 = 1000156010,
		B10X6G10X6R10X6G10X6_422UNorm_4Pack16 = 1000156011,
		G10X6_B10X6_R10X6_3Plane420UNorm_3Pack16 = 1000156012,
		G10X6_B10X6R10X6_2Plane420UNorm_3Pack16 = 1000156013,
		G10X6_B10X6_R10X6_3Plane422UNorm_3Pack16 = 1000156014,
		G10X6_B10X6R10X6_2Plane422UNorm_3Pack16 = 1000156015,
		G10X6_B10X6_R10X6_3Plane444UNorm_3Pack16 = 1000156016,
		R12X4UNormPack16 = 1000156017,
		R12X4G12X4UNorm_2Pack16 = 1000156018,
		R12X4G12X4B12X4A12X4UNorm_4Pack16 = 1000156019,
		G12X4B12X4G12X4R12X4_422UNorm_4Pack16 = 1000156020,
		B12X4G12X4R12X4G12X4_422UNorm_4Pack16 = 1000156021,
		G12X4_B12X4_R12X4_3Plane420UNorm_3Pack16 = 1000156022,
		G12X4_B12X4R12X4_2Plane420UNorm_3Pack16 = 1000156023,
		G12X4_B12X4_R12X4_3Plane422UNorm_3Pack16 = 1000156024,
		G12X4_B12X4R12X4_2Plane422UNorm_3Pack16 = 1000156025,
		G12X4_B12X4_R12X4_3Plane444UNorm_3Pack16 = 1000156026,
		G16B16G16R16_422UNorm = 1000156027,
		B16G16R16G16_422UNorm = 1000156028,
		G16_B16_R16_3Plane420UNorm = 1000156029,
		G16_B16R16_2Plane420UNorm = 1000156030,
		G16_B16_R16_3Plane422UNorm = 1000156031,
		G16_B16R16_2Plane422UNorm = 1000156032,
		G16_B16_R16_3Plane444UNorm = 1000156033,
		// Vulkan 1.3
		G8_B8R8_2Plane444UNorm = 1000330000,
		G10X6_B10X6R10X6_2Plane444UNorm3Pack16 = 1000330001,
		G12X4_B12X4R12X4_2Plane444UNorm3Pack16 = 1000330002,
		G16_B16R16_2Plane444UNorm = 1000330003,
		A4R4G4B4UNormPack16 = 1000340000,
		A4B4G4R4UNormPack16 = 1000340001,
		ASTC4x4SFloatBlock = 1000066000,
		ASTC5x4SFloatBlock = 1000066001,
		ASTC5x5SFloatBlock = 1000066002,
		ASTC6x5SFloatBlock = 1000066003,
		ASTC6x6SFloatBlock = 1000066004,
		ASTC8x5SFloatBlock = 1000066005,
		ASTC8x6SFloatBlock = 1000066006,
		ASTC8x8SFloatBlock = 1000066007,
		ASTC10x5SFloatBlock = 1000066008,
		ASTC10x6SFloatBlock = 1000066009,
		ASTC10x8SFloatBlock = 1000066010,
		ASTC10x10SFloatBlock = 1000066011,
		ASTC12x10SFloatBlock = 1000066012,
		ASTC12x12SFloatBlock = 1000066013,
		// VK_IMG_format_pvrtc
		PVRTC1_2BPPUNormBlockIMG = 1000054000,
		PVRTC1_4BPPUNormBlockIMG = 1000054001,
		PVRTC2_2BPPUNormBlockIMG = 1000054002,
		PVRTC2_4BPPUNormBlockIMG = 1000054003,
		PVRTC1_2BPPSRGBBlockIMG = 1000054004,
		PVRTC1_4BPPSRGBBlockIMG = 1000054005,
		PVRTC2_2BPPSRGBBlockIMG = 1000054006,
		PVRTC2_4BPPSRGBBlockIMG = 1000054007,
		// VK_NV_optical_flow
		R16G16S10_5NV = 1000464000,
		// VK_EXT_texture_compression_astc_hdr
		ASTC4x4SFloatBlockEXT = ASTC4x4SFloatBlock,
		ASTC5x4SFloatBlockEXT = ASTC5x4SFloatBlock,
		ASTC5x5SFloatBlockEXT = ASTC5x5SFloatBlock,
		ASTC6x5SFloatBlockEXT = ASTC6x5SFloatBlock,
		ASTC6x6SFloatBlockEXT = ASTC6x6SFloatBlock,
		ASTC8x5SFloatBlockEXT = ASTC8x5SFloatBlock,
		ASTC8x6SFloatBlockEXT = ASTC8x6SFloatBlock,
		ASTC8x8SFloatBlockEXT = ASTC8x8SFloatBlock,
		ASTC10x5SFloatBlockEXT = ASTC10x5SFloatBlock,
		ASTC10x6SFloatBlockEXT = ASTC10x6SFloatBlock,
		ASTC10x8SFloatBlockEXT = ASTC10x8SFloatBlock,
		ASTC10x10SFloatBlockEXT = ASTC10x10SFloatBlock,
		ASTC12x10SFloatBlockEXT = ASTC12x10SFloatBlock,
		ASTC12x12SFloatBlockEXT = ASTC12x12SFloatBlock,
		// VK_KHR_sampler_ycbcr_conversion
		G8B8G8R8_422UNormKHR = G8B8G8R8_422UNorm,
		B8G8R8G8_422UNormKHR = B8G8R8G8_422UNorm,
		G8_B8_R8_3Plane420UNormKHR = G8_B8_R8_3Plane420UNorm,
		G8_B8R8_2Plane420UNormKHR = G8_B8R8_2Plane420UNorm,
		G8_B8_R8_3Plane422UNormKHR = G8_B8_R8_3Plane422UNorm,
		G8_B8R8_2Plane422UNormKHR = G8_B8R8_2Plane422UNorm,
		G8_B8_R8_3Plane444UNormKHR = G8_B8_R8_3Plane444UNorm,
		R10X6UNormPack16KHR = R10X6UNormPack16,
		R10X6G10X6UNorm_2Pack16KHR = R10X6G10X6UNorm_2Pack16,
		R10X6G10X6B10X6A10X6UNorm_4Pack16KHR = R10X6G10X6B10X6A10X6UNorm_4Pack16,
		G10X6B10X6G10X6R10X6_422UNorm_4Pack16KHR = G10X6B10X6G10X6R10X6_422UNorm_4Pack16,
		B10X6G10X6R10X6G10X6_422UNorm_4Pack16KHR = B10X6G10X6R10X6G10X6_422UNorm_4Pack16,
		G10X6_B10X6_R10X6_3Plane420UNorm_3Pack16KHR = G10X6_B10X6_R10X6_3Plane420UNorm_3Pack16,
		G10X6_B10X6R10X6_2Plane420UNorm_3Pack16KHR = G10X6_B10X6R10X6_2Plane420UNorm_3Pack16,
		G10X6_B10X6_R10X6_3Plane422UNorm_3Pack16KHR = G10X6_B10X6_R10X6_3Plane422UNorm_3Pack16,
		G10X6_B10X6R10X6_2Plane422UNorm_3Pack16KHR = G10X6_B10X6R10X6_2Plane422UNorm_3Pack16,
		G10X6_B10X6_R10X6_3Plane444UNorm_3Pack16KHR = G10X6_B10X6_R10X6_3Plane444UNorm_3Pack16,
		R12X4UNormPack16KHR = R12X4UNormPack16,
		R12X4G12X4UNorm_2Pack16KHR = R12X4G12X4UNorm_2Pack16,
		R12X4G12X4B12X4A12X4UNorm_4Pack16KHR = R12X4G12X4B12X4A12X4UNorm_4Pack16,
		G12X4B12X4G12X4R12X4_422UNorm_4Pack16KHR = G12X4B12X4G12X4R12X4_422UNorm_4Pack16,
		B12X4G12X4R12X4G12X4_422UNorm_4Pack16KHR = B12X4G12X4R12X4G12X4_422UNorm_4Pack16,
		G12X4_B12X4_R12X4_3Plane420UNorm_3Pack16KHR = G12X4_B12X4_R12X4_3Plane420UNorm_3Pack16,
		G12X4_B12X4R12X4_2Plane420UNorm_3Pack16KHR = G12X4_B12X4R12X4_2Plane420UNorm_3Pack16,
		G12X4_B12X4_R12X4_3Plane422UNorm_3Pack16KHR = G12X4_B12X4_R12X4_3Plane422UNorm_3Pack16,
		G12X4_B12X4R12X4_2Plane422UNorm_3Pack16KHR = G12X4_B12X4R12X4_2Plane422UNorm_3Pack16,
		G12X4_B12X4_R12X4_3Plane444UNorm_3Pack16KHR = G12X4_B12X4_R12X4_3Plane444UNorm_3Pack16,
		G16B16G16R16_422UNormKHR = G16B16G16R16_422UNorm,
		B16G16R16G16_422UNormKHR = B16G16R16G16_422UNorm,
		G16_B16_R16_3Plane420UNormKHR = G16_B16_R16_3Plane420UNorm,
		G16_B16R16_2Plane420UNormKHR = G16_B16R16_2Plane420UNorm,
		G16_B16_R16_3Plane422UNormKHR = G16_B16_R16_3Plane422UNorm,
		G16_B16R16_2Plane422UNormKHR = G16_B16R16_2Plane422UNorm,
		G16_B16_R16_3Plane444UNormKHR = G16_B16_R16_3Plane444UNorm,
		// VK_EXT_ycbcr_2plane_444_formats
		G8_B8R8_2Plane444UNorm_3Pack16EXT = G8_B8R8_2Plane444UNorm,
		G10X6_B10X6R10X6_2Plane444UNorm_3Pack16EXT = G10X6_B10X6R10X6_2Plane444UNorm3Pack16,
		G12X4_B12X4R12X4_2Plane444UNorm3Pack16EXT = G12X4_B12X4R12X4_2Plane444UNorm3Pack16,
		G16_B16R16_2Plane444UNormEXT = G16_B16R16_2Plane444UNorm,
		// VK_EXT_4444_formats
		A4R4G4B4UNormPack16EXT = A4R4G4B4UNormPack16,
		A4B4G4R4UNormPack16EXT = A4B4G4R4UNormPack16,
	}

	public enum VKImageType : int {
		Type1D = 0,
		Type2D = 1,
		Type3D = 2
	}

	public enum VKImageTiling : int {
		Optimal = 0,
		Linear = 1,
		DRMFormatModifierEXT = 1000158000
	}

	public enum VKPhysicalDeviceType : int {
		Other = 0,
		IntegratedGPU = 1,
		DiscreteGPU = 2,
		VirtualGPU = 3,
		CPU = 4
	}

	public enum VKQueryType : int {
		Occlusion = 0,
		PipelineStatistics = 1,
		Timestamp = 2,
		TransformFeedbackStreamEXT = 1000028004,
		PerformanceQueryKHR = 1000116000,
		AccelerationStructureCompactedSizeKHR = 1000165000,
		AccelerationStructureSerializationSizeKHR = 1000150000,
		PerformanceQueryINTEL = 1000210000
	}

	public enum VKSharingMode : int {
		Exclusive = 0,
		Concurrent = 1
	}

	public enum VKImageLayout : int {
		Undefined = 0,
		General = 1,
		ColorAttachmentOptimal = 2,
		DepthStencilAttachmentOptimal = 3,
		DepthStencilReadOnlyOptimal = 4,
		ShaderReadOnlyOptimal = 5,
		TransferSrcOptimal = 6,
		TransferDstOptimal = 7,
		PreInitialized = 8,
		DepthReadOnlyStencilAttachmentOptimal = 1000117000,
		DepthAttachmentStencilReadOnlyOptimal = 1000117001,
		DepthAttachmentOptimal = 1000241000,
		DepthReadOnlyOptimal = 1000241001,
		StencilAttachmentOptimal = 1000241002,
		StencilReadOnlyOptimal = 1000241003,
		PresentSrcKHR = 1000001002,
		SharedPresentKHR = 1000111000,
		ShadingRateOptimalNV = 1000164003,
		FragmentDensityMapOptimalEXT = 1000218000
	}

	public enum VKImageViewType : int {
		Type1D = 0,
		Type2D = 1,
		Type3D = 2,
		Cube = 3,
		Array1D = 4,
		Array2D = 5,
		CubeArray = 6
	}

	public enum VKComponentSwizzle : int {
		Identity = 0,
		Zero = 1,
		One = 2,
		R = 3,
		G = 4,
		B = 5,
		A = 6
	}

	public enum VKVertexInputRate : int {
		Vertex = 0,
		Instance = 1
	}

	public enum VKPrimitiveTopology : int {
		PointList = 0,
		LineList = 1,
		LineStrip = 2,
		TriangleList = 3,
		TriangleStrip = 4,
		TriangleFan = 5,
		LineListWithAdjacency = 6,
		LineStripWithAdjacency = 7,
		TriangleListWithAdjacency = 8,
		TriangleStripWithAdjacency = 9,
		PatchList = 10
	}

	public enum VKPolygonMode : int {
		Fill = 0,
		Line = 1,
		Point = 2,
		FillRectangleNV = 1000153000
	}

	public enum VKFrontFace : int {
		CounterClockwise = 0,
		Clockwise = 1
	}

	public enum VKCompareOp : int {
		Never = 0,
		Less = 1,
		Equal = 2,
		LessOrEqual = 3,
		Greater = 4,
		NotEqual = 5,
		GreaterOrEqual = 6,
		Always = 7
	}

	public enum VKStencilOp : int {
		Keep = 0,
		Zero = 1,
		Replace = 2,
		IncrementAndClamp = 3,
		DecrementAndClamp = 4,
		Invert = 5,
		IncrementAndWrap = 6,
		DecrementAndWrap = 7
	}

	public enum VKLogicOp : int {
		Clear = 0,
		And = 1,
		AndReverse = 2,
		Copy = 3,
		AndInverted = 4,
		NoOp = 5,
		Xor = 6,
		Or = 7,
		Nor = 8,
		Equivalent = 9,
		Invert = 10,
		OrReverse = 11,
		CopyInverted = 12,
		OrInverted = 13,
		Nand = 14,
		Set = 15
	}

	public enum VKBlendFactor : int {
		Zero = 0,
		One = 1,
		SrcColor = 2,
		OneMinusSrcColor = 3,
		DstColor = 4,
		OneMinusDstColor = 5,
		SrcAlpha = 6,
		OneMinusSrcAlpha = 7,
		DstAlpha = 8,
		OneMinusDstAlpha = 9,
		ConstantColor = 10,
		OneMinusConstantColor = 11,
		ConstantAlpha = 12,
		OneMinusConstantAlpha = 13,
		SrcAlphaSaturate = 14,
		Src1Color = 15,
		OneMinusSrc1Color = 16,
		Src1Alpha = 17,
		OneMinusSrc1Alpha = 18
	}

	public enum VKBlendOp : int {
		Add = 0,
		Subtract = 1,
		ReverseSubtract = 2,
		Min = 3,
		Max = 4,
		ZeroEXT = 1000148000,
		SrcEXT = 1000148001,
		DstEXT = 1000148002,
		SrcOverEXT = 1000148003,
		DstOverEXT = 1000148004,
		SrcInEXT = 1000148005,
		DstInExt = 1000148006,
		SrcOutEXT = 1000148007,
		DstOutEXT = 1000148008,
		SrcAtopEXT = 1000148009,
		DstAtopEXT = 1000148010,
		XorEXT = 1000148011,
		MultiplyEXT = 1000148012,
		ScreenEXT = 1000148013,
		OverlayEXT = 1000148014,
		DarkenEXT = 1000148015,
		LightenEXT = 1000148016,
		ColorDodgeEXT = 1000148017,
		ColorBurnEXT = 1000148018,
		HardLightEXT = 1000148019,
		SoftLightEXT = 1000148020,
		DifferenceEXT = 1000148021,
		ExclusionEXT = 1000148022,
		InvertEXT = 1000148023,
		InvertRGBEXT = 1000148024,
		LinearDodgeEXT = 1000148025,
		LinearBurnEXT = 1000148026,
		VividLightEXT = 1000148027,
		LinearLightEXT = 1000148028,
		PinLightEXT = 1000148029,
		HardMixEXT = 1000148030,
		HSLHueEXT = 1000148031,
		HSLSaturationEXT = 1000148032,
		HSLColorEXT = 1000148033,
		HSLLuminosityEXT = 1000148034,
		PlusEXT = 1000148035,
		PlusClampedEXT = 1000148036,
		PlusClampedAlphaEXT = 1000148037,
		PlusDarkerEXT = 1000148038,
		MinusEXT = 1000148039,
		MinusClampedEXT = 1000148040,
		ContrastEXT = 1000148041,
		InvertOVGEXT = 1000148042,
		RedEXT = 1000148043,
		GreenEXT = 1000148044,
		BlueEXT = 1000148045,
	}

	public enum VKDynamicState : int {
		Viewport = 0,
		Scissor = 1,
		LineWidth = 2,
		DepthBias = 3,
		BlendConstants = 4,
		DepthBounds = 5,
		StencilCompareMask = 6,
		StencilWriteMask = 7,
		StencilReference = 8,
		// VK_NV_clip_space_w_scaling
		ViewportWScalingNV = 1000087000,
		// VK_EXT_discard_rectangles
		DiscardRectangleEXT = 1000099000,
		// VK_EXT_sample_locations
		SampleLocationsEXT = 1000143000,
		// VK_KHR_ray_tracing_pipeline
		RayTracingPipelineStackSizeKHR = 1000347000,
		// VK_NV_shading_rate_image
		ViewportShadingRatePaletteNV = 1000164004,
		ViewportCoarseSampleOrderNV = 1000164006,
		// VK_NV_scissor_exclusive
		ExclusiveScissorNV = 1000205001,
		// VK_KHR_fragment_shading_rate
		FragmentShadingRate = 1000226000,
		// VK_EXT_line_rasterization
		LineStippleEXT = 1000259000,
		// VK_EXT_extended_dynamic_state
		CullModeEXT = 1000267000,
		FrontFaceEXT,
		PrimitiveTopologyEXT,
		ViewportWithCountEXT,
		ScissorWithCountEXT,
		VertexInputBindingStrideEXT,
		DepthTestEnableEXT,
		DepthWriteEnableEXT,
		DepthCompareOpEXT,
		DepthBoundsTestEnableEXT,
		StencilTestEnableEXT,
		StencilOpEXT,
		// VK_EXT_vertex_input_dynamic_state
		VertexInputEXT = 1000352000,
		// VK_EXT_extended_dynamic_state2
		PatchControlPointsEXT = 1000377000,
		RasterizerDiscardEnableEXT,
		DepthBiasEnableEXT,
		LogicOpEXT,
		PrimitiveRestartEnableEXT,
		// VK_EXT_color_write_enable
		ColorWriteEnableEXT = 1000381000
	}

	public enum VKFilter : int {
		Nearest = 0,
		Linear = 1,
		CubicIMG = 1000015000
	}

	public enum VKSamplerMipmapMode : int {
		Nearest = 0,
		Linear = 1
	}

	public enum VKSamplerAddressMode : int {
		Repeat = 0,
		MirroredRepeat = 1,
		ClampToEdge = 2,
		ClampToBorder = 3,
		MirrorClampToEdge = 4
	}

	public enum VKBorderColor : int {
		FloatTransparentBlack = 0,
		IntTransparentBlack = 1,
		FloatOpaqueBlack = 2,
		IntOpaqueBlack = 3,
		FloatOpaqueWhite = 4,
		IntOpaqueWhite = 5,
		FloatCustomEXT = 1000287003,
		IntCustomEXT = 1000287004
	}

	public enum VKDescriptorType : int {
		Sampler = 0,
		CombinedImageSampler = 1,
		SampledImage = 2,
		StorageImage = 3,
		UniformTexelBuffer = 4,
		StorageTexelBuffer = 5,
		UniformBuffer = 6,
		StorageBuffer = 7,
		UniformBufferDynamic = 8,
		StorageBufferDynamic = 9,
		InputAttachment = 10,
		InlineUniformBlockEXT = 1000138000,
		AccelerationStructureKHR = 1000165000
	}

	public enum VKAttachmentLoadOp : int {
		Load = 0,
		Clear = 1,
		DontCare = 2
	}

	public enum VKAttachmentStoreOp : int {
		Store = 0,
		DontCare = 1,
		// Vulkan 1.3
		None = 1000301000
	}

	public enum VKPipelineBindPoint : int {
		Graphics = 0,
		Compute = 1,
		RayTracingKHR = 1000165000
	}

	public enum VKCommandBufferLevel : int {
		Primary = 0,
		Secondary = 1
	}

	public enum VKIndexType : int {
		UInt16 = 0,
		UInt32 = 1,
		NoneKHR = 1000165000,
		UInt8EXT = 1000265000
	}

	public enum VKSubpassContents : int {
		Inline = 0,
		SecondaryCommandBuffers = 1
	}

	public enum VKObjectType {
		Unknown = 0,
		Instance = 1,
		PhysicalDevice = 2,
		Device = 3,
		Queue = 4,
		Semaphore = 5,
		CommandBuffer = 6,
		Fence = 7,
		DeviceMemory = 8,
		Buffer = 9,
		Image = 10,
		Event = 11,
		QueryPool = 12,
		BufferView = 13,
		ImageView = 14,
		ShaderModule = 15,
		PipelineCache = 16,
		PipelineLayout = 17,
		RenderPass = 18,
		Pipeline = 19,
		DescriptorSetLayout = 20,
		Sampler = 21,
		DescriptorPool = 22,
		DescriptorSet = 23,
		Framebuffer = 24,
		CommandPool = 25,
		SamplerYCbCrConversion = 1000156000,
		DescriptorUpdateTemplate = 1000085000,
		SurfaceKHR = 1000000000,
		SwapchainKHR = 1000001000,
		DisplayKHR = 1000002000,
		DisplayModeKHR = 1000002001,
		DebugReportCallbackEXT = 1000011000,
		DebugUtilsMessengerEXT = 1000128000,
		AccelerationStructureKHR = 1000165000,
		ValidationCacheEXT = 1000160000,
		PerformanceConfigurationINTEL = 1000210000,
		DeferredOperationKHR = 1000268000,
		IndirectCommandsLayoutNV = 1000277000,
		PrivateDataSlotEXT = 1000295000
	}

	public enum VKVendorID : int {
		// PCI-base vendor IDs (non-exhaustive)
		Intel = 0x8086,
		AMD = 0x1002, // AMD/ATI vendor id
		NVidia = 0x10DE,
		// Non-PCI vendor IDs
		VIV = 0x10001,
		VSI = 0x10002,
		Kazan = 0x10003,
		Codeplay = 0x10004,
		Mesa = 0x10005
	}

	[Flags]
	public enum VKInstanceCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKFormatFeatureFlagBits : VKFlags {
		SampledImage = 0x00000001,
		StorageImage = 0x00000002,
		StorageImageAtomic = 0x00000004,
		UniformTexelBuffer = 0x00000008,
		StorageTexelBuffer = 0x00000010,
		StorageTexelBufferAtomic = 0x00000020,
		VertexBuffer = 0x00000040,
		ColorAttachment = 0x00000080,
		ColorAttachmentBlend = 0x00000100,
		DepthStencilAttachment = 0x00000200,
		BlitSrc = 0x00000400,
		BlitDst = 0x00000800,
		SampledImageFilterLinear = 0x00001000,
		TransferSrc = 0x00004000,
		TransferDst = 0x00008000,
		MidpointChromaSamples = 0x00020000,
		SampledImageYCbCrConversionLinearFilter = 0x00040000,
		SampledImageYCbCrConversionSeparateReconstructionFilter = 0x00080000,
		SampledImageYCbCrConversionChromaReconstructionExplicit = 0x00100000,
		SampledImageYCbCrConversionChromaReconstructionExplicitForceable = 0x00200000,
		Disjoint = 0x00400000,
		CositedChromaSamples = 0x00800000,
		SampledImageFitlerMinmax = 0x00010000,
		SampledImageFilterCubicIMG = 0x00002000,
		AccelerationStructureVertexBufferKHR = 0x10000000,
		FragmentDensityMapEXT = 0x01000000
	}

	[Flags]
	public enum VKImageUsageFlagBits : VKFlags {
		TransferSrc = 0x00000001,
		TransferDst = 0x00000002,
		Sampled = 0x00000004,
		Storage = 0x00000008,
		ColorAttachment = 0x00000010,
		DepthStencilAttachment = 0x00000020,
		TransientAttachment = 0x00000040,
		InputAttachment = 0x00000080,
		ShadingRateImageNV = 0x00000100,
		FragmentDensityMapEXT = 0x00000200
	}

	[Flags]
	public enum VKImageCreateFlagBits : VKFlags {
		SparseBinding = 0x00000001,
		SparseResidency = 0x00000002,
		SparseAliased = 0x00000004,
		MutableFormat = 0x00000008,
		CubeCompatible = 0x00000010,
		Alias = 0x00000400,
		SplitInstanceBindRegions = 0x00000040,
		Create2DArrayCompatible = 0x00000020,
		BlockTexelViewCompatible = 0x00000080,
		ExtendedUsage = 0x00000100,
		Protected = 0x00000800,
		Disjoint = 0x00000200,
		CornerSampledNV = 0x00002000,
		SampleLocationsCompatibleDepthEXT = 0x00001000,
		SubsampledEXT = 0x00004000
	}

	[Flags]
	public enum VKSampleCountFlagBits : VKFlags {
		Count1Bit = 0x00000001,
		Count2Bit = 0x00000002,
		Count4Bit = 0x00000004,
		Count8Bit = 0x00000008,
		Count16Bit = 0x00000010,
		Count32Bit = 0x00000020,
		Count64Bit = 0x00000040
	}

	[Flags]
	public enum VKQueueFlagBits : VKFlags {
		Graphics = 0x00000001,
		Compute = 0x00000002,
		Transfer = 0x00000004,
		SparseBinding = 0x00000008,
		Protected = 0x00000010
	}

	[Flags]
	public enum VKMemoryPropertyFlagBits : VKFlags {
		DeviceLocal = 0x00000001,
		HostVisible = 0x00000002,
		HostCoherent = 0x00000004,
		HostCached = 0x00000008,
		LazilyAllocated = 0x00000010,
		ProtectedBit = 0x00000020,
		DeviceCoherentAMD = 0x00000040,
		DeviceUncachedAMD = 0x00000080
	}

	[Flags]
	public enum VKMemoryHeapFlagBits : VKFlags {
		DeviceLocal = 0x00000001,
		MultiInstance = 0x00000002
	}

	[Flags]
	public enum VKDeviceCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKDeviceQueueCreateFlagBits : VKFlags {
		Protected = 0x00000001
	}

	[Flags]
	public enum VKPipelineStageFlagBits : VKFlags {
		TopOfPipe = 0x00000001,
		DrawIndirect = 0x00000002,
		VertexInput = 0x00000004,
		VertexShader = 0x00000008,
		TessellationControlShader = 0x00000010,
		TessellationEvaluationShader = 0x00000020,
		GeometryShader = 0x00000040,
		FragmentShader = 0x00000080,
		EarlyFragmentTests = 0x00000100,
		LateFragmentTests = 0x00000200,
		ColorAttachmentOutput = 0x00000400,
		ComputeShader = 0x00000800,
		Transfer = 0x00001000,
		BottomOfPipe = 0x00002000,
		Host = 0x00004000,
		AllGraphics = 0x00008000,
		AllCommands = 0x00010000,
		TransformFeedbackEXT = 0x01000000,
		ConditionalRenderingEXT = 0x00040000,
		RayTracingShaderKHR = 0x00200000,
		AccelerationStructureBuildKHR = 0x002000000,
		ShadingRateImageNV = 0x00400000,
		TaskShaderNV = 0x00080000,
		MeshShaderNV = 0x00100000,
		FragmentDensityProcessEXT = 0x00800000,
		CommandPreProcessNV = 0x00020000
	}

	[Flags]
	public enum VKMemoryMapFlagBits : VKFlags { }

	[Flags]
	public enum VKImageAspectFlagBits : VKFlags {
		Color = 0x00000001,
		Depth = 0x00000002,
		Stencil = 0x00000004,
		Metadata = 0x000000008,
		Plane0 = 0x00000010,
		Plane1 = 0x00000020,
		Plane2 = 0x00000040,
		MemoryPlane0EXT = 0x00000080,
		MemoryPlane1EXT = 0x00000100,
		MemoryPlane2EXT = 0x00000200,
		MemoryPlane3EXT = 0x00000400
	}

	[Flags]
	public enum VKSparseImageFormatFlagBits : VKFlags {
		SingleMipTail = 0x00000001,
		AlignedMipSize = 0x00000002,
		NonStandardBlockSize = 0x00000004
	}

	[Flags]
	public enum VKSparseMemoryBindFlagBits : VKFlags {
		Metadata = 0x00000001
	}

	[Flags]
	public enum VKFenceCreateFlagBits : VKFlags {
		Signaled = 0x00000001
	}

	[Flags]
	public enum VKSemaphoreCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKEventCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKQueryPoolCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKQueryPipelineStatisticFlagBits : VKFlags {
		InputAssemblyVertices = 0x00000001,
		InputAssemblyPrimitives = 0x000000002,
		VertexShaderInvocations = 0x000000004,
		GeometryShaderInvocations = 0x00000008,
		GeometryShaderPrimitives = 0x00000010,
		ClippingInvocations = 0x00000020,
		ClippingPrimitives = 0x00000040,
		FragmentShaderInvocations = 0x00000080,
		TessellationControlShaderPatches = 0x00000100,
		TessellationEvaluationShaderInvocations = 0x00000200,
		ComputeShaderInvocations = 0x00000400
	}

	[Flags]
	public enum VKQueryResultFlagBits : VKFlags {
		Result64Bit = 0x00000001,
		Wait = 0x00000002,
		WithAvailability = 0x00000004,
		Partial = 0x00000008
	}

	[Flags]
	public enum VKBufferCreateFlagBits : VKFlags {
		SparseBinding = 0x00000001,
		SparseResidency = 0x00000002,
		SparseAliased = 0x00000004,
		Protected = 0x000000008,
		DeviceAddressCaptureReplay = 0x00000010
	}

	[Flags]
	public enum VKBufferUsageFlagBits : VKFlags {
		TransferSrc = 0x00000001,
		TransferDst = 0x00000002,
		UniformTexelBuffer = 0x00000004,
		StorageTexelBuffer = 0x00000008,
		UniformBuffer = 0x00000010,
		StorageBuffer = 0x00000020,
		IndexBuffer = 0x00000040,
		VertexBuffer = 0x00000080,
		IndirectBuffer = 0x00000100,
		ShaderDeviceAddress = 0x00020000,
		TransformFeedbackBufferEXT = 0x00000800,
		TransformFeedbackCounterBufferEXT = 0x00001000,
		ConditionalRenderingEXT = 0x00000200,
		RayTracingKHR = 0x00000400
	}

	[Flags]
	public enum VKBufferViewCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKImageViewCreateFlagBits : VKFlags {
		FragmentDensityMapDynamicEXT = 0x00000001
	}

	[Flags]
	public enum VKShaderModuleCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineCacheCreateFlagBits : VKFlags {
		ExternallySynchronizedEXT = 0x00000001
	}

	[Flags]
	public enum VKPipelineCreateFlagBits : VKFlags {
		DisableOptimization = 0x00000001,
		AllowDerivatives = 0x00000002,
		Derivative = 0x00000004,
		ViewIndexFromDeviceIndex = 0x00000008,
		DispatchBase = 0x00000010,
		RayTracingNoNullAnyHitShadersKHR = 0x00004000,
		RayTracingNoNullClosestHitShadersKHR = 0x00008000,
		RayTracingNoNullMissShadersKHR = 0x00010000,
		RayTracingNoNullIntersectionShadersKHR = 0x00020000,
		RayTracingSkipTrianglesKHR = 0x00001000,
		RayTracingSkipAABBsKHR = 0x00002000,
		DeferCompileNV = 0x00000020,
		CaptureStatisticsKHR = 0x00000040,
		CaptureInternalRepresentationsKHR = 0x00000080,
		IndirectBindableNV = 0x00040000,
		LibraryKHR = 0x00000800,
		FailOnPipelineCompileRequiredEXT = 0x00000100,
		EarlyReturnOnFailureEXT = 0x00000200
	}

	[Flags]
	public enum VKPipelineShaderStageCreateFlagBits : VKFlags {
		AllowVaryingSubgroupSizeEXT = 0x00000001,
		RequireFullSubgroupsEXT = 0x00000002
	}

	[Flags]
	public enum VKShaderStageFlagBits : VKFlags {
		Vertex = 0x00000001,
		TessellationControl = 0x00000002,
		TessellationEvaluation = 0x00000004,
		Geometry = 0x00000008,
		Fragment = 0x00000010,
		Compute = 0x00000020,
		AllGraphics = 0x0000001F,
		All = 0x7FFFFFFF,
		RaygenKHR = 0x00000100,
		AnyHitKHR = 0x00000200,
		ClosestHitKHR = 0x00000400,
		MissKHR = 0x00000800,
		IntersectionKHR = 0x00001000,
		CallableKHR = 0x00002000,
		TaskNV = 0x00000040,
		MeshNV = 0x00000080
	}

	[Flags]
	public enum VKPipelineVertexInputStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineInputAssemblyStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineTessellationStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineViewportStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineRasterizationStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKCullModeFlagBits : VKFlags {
		None = 0,
		Front = 0x00000001,
		Back = 0x00000002,
		FrontAndBack = 0x00000003
	}

	[Flags]
	public enum VKPipelineMultisampleStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineDepthStencilStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineColorBlendStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKColorComponentFlagBits : VKFlags {
		R = 0x00000001,
		G = 0x00000002,
		B = 0x00000004,
		A = 0x00000008
	}

	[Flags]
	public enum VKPipelineDynamicStateCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKPipelineLayoutCreateFlagBits : VKFlags { }

	[Flags]
	public enum VKSamplerCreateFlagBits : VKFlags {
		SubsampledEXT = 0x00000001,
		SubsampledCoarseReconstructionEXT = 0x00000002
	}

	[Flags]
	public enum VKDescriptorSetLayoutCreateFlagBits : VKFlags {
		UpdateAfterBindPool = 0x00000002,
		PushDescriptorKHR = 0x00000001
	}

	[Flags]
	public enum VKDescriptorPoolCreateFlagBits : VKFlags {
		FreeDescriptorSet = 0x00000001,
		UpdateAfterBind = 0x00000002
	}

	[Flags]
	public enum VKDescriptorPoolResetFlagBits : VKFlags { }

	[Flags]
	public enum VKFramebufferCreateFlagBits : VKFlags {
		Imageless = 0x00000001
	}

	[Flags]
	public enum VKRenderPassCreateFlagBits : VKFlags {
		TransformQCOM = 0x00000002
	}

	[Flags]
	public enum VKAttachmentDescriptionFlagBits : VKFlags {
		MayAlias = 0x00000001
	}

	[Flags]
	public enum VKSubpassDescriptionFlagBits : VKFlags {
		PerViewAttributesNVX = 0x00000001,
		PerViewPositionXOnlyNVX = 0x00000002,
		FragmentRegionQCOM = 0x00000004,
		ShaderResolveQCOM = 0x00000008
	}

	[Flags]
	public enum VKAccessFlagBits : VKFlags {
		IndirectCommandRead = 0x00000001,
		IndexRead = 0x00000002,
		VertexAttributeRead = 0x00000004,
		UniformRead = 0x00000008,
		InputAttachmentRead = 0x00000010,
		ShaderRead = 0x00000020,
		ShaderWrite = 0x00000040,
		ColorAttachmentRead = 0x00000080,
		ColorAttachmentWrite = 0x00000100,
		DepthStencilAttachmentRead = 0x00000200,
		DepthStencilAttachmentWrite = 0x00000400,
		TransferRead = 0x00000800,
		TransferWrite = 0x00001000,
		HostRead = 0x00002000,
		HostWrite = 0x00004000,
		MemoryRead = 0x00008000,
		MemoryWrite = 0x00010000,
		TransformFeedbackWriteEXT = 0x02000000,
		TransformFeedbackCounterReadEXT = 0x04000000,
		TransformFeedbackCounterWriteEXT = 0x08000000,
		ConditionalRenderingReadEXT = 0x00100000,
		ColorAttachmentReadNonCoherentEXT = 0x00080000,
		AccelerationStructureReadKHR = 0x00200000,
		AccelerationStructureWriteKHR = 0x00400000,
		ShadingRateImageReadNV = 0x00800000,
		FragmentDensityMapReadEXT = 0x01000000,
		CommandPreProcessReadNV = 0x00020000,
		CommandPreProcessWriteNV = 0x00040000
	}

	[Flags]
	public enum VKDependencyFlagBits : VKFlags {
		ByRegion = 0x00000001,
		DeviceGroup = 0x0000004,
		ViewLocal = 0x00000002
	}

	[Flags]
	public enum VKCommandPoolCreateFlagBits : VKFlags {
		Transient = 0x00000001,
		ResetCommandBuffer = 0x00000002,
		Protected = 0x00000004
	}

	[Flags]
	public enum VKCommandPoolResetFlagBits : VKFlags {
		ReleaseResources = 0x00000001
	}

	[Flags]
	public enum VKCommandBufferUsageFlagBits : VKFlags {
		OneTimeSubmit = 0x00000001,
		RenderPassContinue = 0x00000002,
		SimultaneousUse = 0x00000004
	}

	[Flags]
	public enum VKQueryControlFlagBits : VKFlags {
		Precise = 0x00000001
	}

	[Flags]
	public enum VKCommandBufferResetFlagBits : VKFlags {
		ReleaseResources = 0x00000001
	}

	[Flags]
	public enum VKStencilFaceFlagBits : VKFlags {
		Front = 0x00000001,
		Back = 0x00000002,
		FrontAndBack = 0x00000003
	}

	// Vulkan 1.1

	public enum VKPointClippingBehavior : int {
		AllClipPlanes = 0,
		UserClipPlanesOnly = 1
	}

	public enum VKTessellationDomainOrigin : int {
		UpperLeft = 0,
		LowerLeft = 1
	}

	public enum VKSamplerYcbcrModelConversion : int {
		RGBIdentity = 0,
		YcbcrIdentity = 1,
		Ycbcr709 = 2,
		Ycbcr601 = 3,
		Ycbcr2020 = 4
	}

	public enum VKSamplerYcbcrRange : int {
		ITUFull = 0,
		ITUNarrow = 1
	}

	public enum VKChromaLocation : int {
		CositedEven = 0,
		Midpoint = 1
	}

	public enum VKDescriptorUpdateTemplateType : int {
		DescriptorSet = 0,
		PushDescriptorsKHR = 1
	}

	[Flags]
	public enum VKSubgroupFeatureFlagBits : VKFlags {
		Basic = 0x00000001,
		Vote = 0x00000002,
		Arithmetic = 0x00000004,
		Ballot = 0x00000008,
		Shuffle = 0x00000010,
		ShuffleRelative = 0x00000020,
		Clustered = 0x00000040,
		Quad = 0x00000080,
		PartitionedNV = 0x00000100
	}

	[Flags]
	public enum VKPeerMemoryFeatureFlagBits : VKFlags {
		CopySrc = 0x00000001,
		CopyDst = 0x00000002,
		GenericSrc = 0x00000004,
		GenericDst = 0x00000008
	}

	[Flags]
	public enum VKMemoryAllocateFlagBits : VKFlags {
		DeviceMask = 0x00000001,
		DeviceAddress = 0x00000002,
		DeviceAddressCaptureReplay = 0x00000004
	}

	[Flags]
	public enum VKCommandPoolTrimFlags : VKFlags { }

	[Flags]
	public enum VKDescriptorUpdateTemplateCreateFlags : VKFlags { }

	[Flags]
	public enum VKExternalMemoryHandleTypeFlagBits : VKFlags {
		OpaqueFD = 0x00000001,
		OpaqueWin32 = 0x00000002,
		OpaqueWin32KMT = 0x00000004,
		D3D11Texture = 0x00000008,
		D3D11TextureKMT = 0x00000010,
		D3D12Heap = 0x00000020,
		D3D12Resource = 0x00000040,
		DMABufEXT = 0x00000200,
		AndroidHardwareBufferANDROID = 0x00000400,
		HostAllocationEXT = 0x00000080,
		HostMappedForeignMemoryEXT = 0x00000100
	}

	[Flags]
	public enum VKExternalMemoryFeatureFlagBits : VKFlags {
		DedicatedOnly = 0x00000001,
		Exportable = 0x00000002,
		Importable = 0x00000004
	}

	[Flags]
	public enum VKExternalFenceHandleTypeFlagBits : VKFlags {
		OpaqueFD = 0x00000001,
		OpaqueWin32 = 0x00000002,
		OpaqueWin32KMT = 0x00000004,
		SyncFD = 0x00000008
	}

	[Flags]
	public enum VKExternalFenceFeatureFlagBits : VKFlags {
		Exportable = 0x00000001,
		Importable = 0x00000002
	}

	[Flags]
	public enum VKFenceImportFlagBits : VKFlags {
		Temporary = 0x00000001
	}

	[Flags]
	public enum VKSemaphoreImportFlagBits : VKFlags {
		Temporary = 0x00000001
	}

	[Flags]
	public enum VKExternalSemaphoreHandleTypeFlagBits : VKFlags {
		OpaqueFD = 0x00000001,
		OpaqueWin32 = 0x00000002,
		OpaqueWin32KMT = 0x00000004,
		D3D12Fence = 0x00000008,
		SyncFD = 0x00000010
	}

	[Flags]
	public enum VKExternalSemaphoreFeatureFlagBits : VKFlags {
		Exportable = 0x00000001,
		Importable = 0x00000002
	}

	// Vulkan 1.2

	public enum VKDriverId : int {
		AMDProprietary = 1,
		AMDOpenSource = 2,
		MesaRADV = 3,
		NVIDIAProprietary = 4,
		IntelProprietaryWindows = 5,
		IntelOpenSourceMesa = 6,
		ImaginationProprietary = 7,
		QualcommProprietary = 8,
		ARMProprietary = 9,
		GoogleSwiftshader = 10,
		GGPProprietary = 11,
		BroadcomProprietary = 12,
		MesaLLVMPipe = 13
	}

	public enum VKShaderFloatControlsIndependence : int {
		_32BitOnly = 0,
		All = 1,
		None = 2
	}

	public enum VKSamplerReductionMode : int {
		WeightedAverage = 0,
		Min = 1,
		Max = 2
	}

	public enum VKSemaphoreType : int {
		Binary = 0,
		Timeline = 1
	}

	[Flags]
	public enum VKResolveModeFlagBits : VKFlags {
		None = 0,
		SampleZero = 0x00000001,
		Average = 0x00000002,
		Min = 0x00000004,
		Max = 0x00000008
	}

	[Flags]
	public enum VKDescriptorBindingFlagBits : VKFlags {
		UpdateAfterBind = 0x00000001,
		UpdateUnusedWhilePending = 0x00000002,
		PartiallyBound = 0x00000004,
		VariableDescriptorCount = 0x00000008
	}

	[Flags]
	public enum VKSemaphoreWaitFlagBits : VKFlags {
		Any = 0x00000001
	}

	// Vulkan 1.3

	// VK_EXT_pipeline_creation_feedback

	[Flags]
	public enum VKPipelineCreationFeedbackFlagBits : VKFlags {
		Valid = 0x00000001,
		ApplicationPipelineCacheHit = 0x00000002,
		BasePipelineAcceleration = 0x00000004
	}

	// VK_EXT_tooling_info

	[Flags]
	public enum VKToolPurposeFlagBits : VKFlags {
		Validation = 0x00000001,
		Profiling = 0x00000002,
		Tracing = 0x00000004,
		AdditionalFeatures = 0x00000008,
		ModifyingFeatures = 0x00000010,
		DebugReporting = 0x00000020,
		DebugMarkers = 0x00000040
	}

	// VK_EXT_private_data

	[Flags]
	public enum VKPrivateDataSlotCreateFlagBits : VKFlags { }

	// VK_KHR_synchronization2

	[Flags]
	public enum VKPipelineStageFlagBits2 : VKFlags64 {
		None = 0,
		TopOfPipe = 0x000000001,
		DrawIndirect = 0x00000002,
		VertexInput = 0x00000004,
		VertexShader = 0x00000008,
		TessellationControlShader = 0x00000010,
		TessellationEvaluationShader = 0x00000020,
		GeometryShader = 0x00000040,
		FragmentShader = 0x00000080,
		EarlyFragmentTests = 0x00000100,
		LateFragmentTests = 0x00000200,
		ColorAttachmentOutput = 0x00000400,
		ComputeShader = 0x00000800,
		AllTransfer = 0x00001000,
		Transfer = AllTransfer,
		BottomOfPipe = 0x00002000,
		Host = 0x00004000,
		AllGraphics = 0x00008000,
		AllCommands = 0x00010000,
		Copy = 0x100000000,
		Resolve = 0x200000000,
		Blit = 0x400000000,
		Clear = 0x800000000,
		IndexInput = 0x1000000000,
		VertexAttributeInput = 0x2000000000,
		PreRasterizationShaders = 0x4000000000,
		// VK_KHR_video_decode_queue
		VideoDecodeBitKHR = 0x04000000,
		// VK_KHR_video_encode_queue
		VideoEncodeBitKHR = 0x08000000,
		// VK_KHR_synchronization2 + VK_EXT_transform_feedback
		TransformFeedbackEXT = 0x01000000,
		// VK_KHR_synchronization2 + VK_EXT_conditional_rendering
		ConditionalRenderingEXT = 0x00040000,
		// VK_KHR_synchronization2 + VK_NV_device_generated_commands
		CommandPreprocessNV = 0x00020000,
		// VK_KHR_fragment_shading_rate + VK_KHR_synchronization2
		FragmentShadingRateAttachmentKHR = 0x00400000,
		// VK_KHR_synchronization2 + VK_NV_shading_rate_image
		ShadingRateImageNV = FragmentShadingRateAttachmentKHR,
		// VK_KHR_acceleration_structure + VK_KHR_synchronization2
		AccelerationStructureBuildKHR = 0x02000000,
		// VK_KHR_ray_tracing_pipeline + VK_KHR_synchronization2
		RayTracingShaderKHR = 0x00200000,
		// VK_KHR_synchronization2 + VK_NV_ray_tracing
		RayTracingShaderNV = RayTracingShaderKHR,
		AccelerationStructureBuildNV = AccelerationStructureBuildKHR,
		// VK_KHR_synchronization2 + VK_EXT_fragment_density_map
		FragmentDensityProcessEXT = 0x00800000,
		// VK_KHR_synchronization2 + VK_EXT_mesh_shader
		TaskShaderEXT = 0x00080000,
		MeshShaderEXT = 0x00100000,
		// VK_KHR_synchronization2 + VK_NV_mesh_shader
		TaskShaderNV = TaskShaderEXT,
		MeshShaderNV = MeshShaderEXT,
		// VK_HUAWEI_subpass_shading
		SubpassShadingHUAWEI = 0x8000000000,
		// VK_HUAWEI_invocation_mask
		InvocationMaskHUAWEI = 0x10000000000,
		// VK_KHR_ray_tracing_maintenance1 + VK_KHR_synchronization2
		AccelerationStructureCopyKHR = 0x10000000,
		// VK_EXT_opacity_micromap
		MicromapBuildEXT = 0x40000000,
		// VK_NV_optical_flow
		OpticalFlow = 0x20000000
	}

	[Flags]
	public enum VKAccessFlagBits2 : VKFlags64 {
		None = 0,
		IndirectCommandRead = 0x00000001,
		IndexRead = 0x00000002,
		VertexAttributeRead = 0x00000004,
		UniformRead = 0x00000008,
		InputAttachmentRead = 0x00000010,
		ShaderRead = 0x00000020,
		ShaderWrite = 0x00000040,
		ColorAttachmentRead = 0x00000080,
		ColorAttachmentWrite = 0x00000100,
		DepthStencilAttachmentRead = 0x00000200,
		DepthStencilAttachmentWrite = 0x00000400,
		TransferRead = 0x00000800,
		TransferWrite = 0x00001000,
		HostRead = 0x00002000,
		HostWrite = 0x00004000,
		MemoryRead = 0x00008000,
		MemoryWrite = 0x00010000,
		ShaderSampledRead = 0x100000000,
		ShaderStorageRead = 0x200000000,
		ShaderStorageWrite = 0x400000000,
		// VK_KHR_video_decode_queue
		VideoDecodeReadKHR = 0x800000000,
		VideoDecodeWriteKHR = 0x1000000000,
		// VK_KHR_video_encode_queue
		VideoEncodeReadKHR = 0x2000000000,
		VideoEncodeWriteKHR = 0x4000000000,
		// VK_KHR_synchronization2 + VK_EXT_transform_feedback
		TransformFeedbackWriteEXT = 0x02000000,
		TransformFeedbackCounterReadEXT = 0x04000000,
		TransformFeedbackCounterWriteEXT = 0x08000000,
		// VK_KHR_synchronization2 + VK_EXT_conditional_rendering
		ConditionalRenderingReadEXT = 0x00100000,
		// VK_KHR_synchronization2 + VK_NV_device_generated_commands
		CommandPreprocessReadNV = 0x00020000,
		CommandPreprocessWriteNV = 0x00040000,
		// VK_KHR_fragment_shading_rate + VK_KHR_synchronization2
		FragmentShadingRateAttachmentReadKHR = 0x00800000,
		// VK_KHR_synchronization2 + VK_NV_shading_rate_image
		ShadingRateImageReadNV = FragmentShadingRateAttachmentReadKHR,
		// VK_KHR_acceleration_structure + VK_KHR_synchronization2
		AccelerationStructureReadKHR = 0x00200000,
		AccelerationStructureWriteKHR = 0x00400000,
		// VK_KHR_synchronization2 + VK_NV_ray_tracing
		AccelerationStructureReadNV = AccelerationStructureReadKHR,
		AccelerationStructureWriteNV = AccelerationStructureWriteKHR,
		// VK_KHR_synchronization2 + VK_EXT_fragment_density_map
		FragmentDensityMapReadEXT = 0x01000000,
		// VK_KHR_synchronization2 + VK_EXT_blend_operation_advanced
		ColorAttachmentReadNoncoherentEXT = 0x00080000,
		// VK_HUAWEI_invocation_mask
		InvocationMaskReadHUAWEI = 0x8000000000,
		// VK_KHR_ray_tracing_maintenance1 + VK_KHR_synchronization2 + VK_KHR_ray_tracing_pipeline
		ShaderBindingTableReadKHR = 0x10000000000,
		// VK_EXT_opacity_micromap
		MicromapReadEXT = 0x100000000000,
		MicromapWriteEXT = 0x200000000000,
		// VK_NV_optical_flow
		OpticalFlowReadNV = 0x40000000000,
		OpticalFlowWriteNV = 0x80000000000
	}

	[Flags]
	public enum VKSubmitFlagBits : VKFlags {
		Protected = 0x00000001
	}

	// VK_KHR_dynamic_rendering
	
	[Flags]
	public enum VKRenderingFlagBits : VKFlags {
		ContentsSecondaryCommandBuffers = 0x00000001,
		Suspending = 0x00000002,
		Resuming = 0x00000004
	}

	// VK_KHR_format_feature_flags2

	[Flags]
	public enum VKFormatFeatureFlagBits2 : VKFlags64 {
		SampledImage = 0x00000001,
		StorageImage = 0x00000002,
		StorageImageAtomic = 0x00000004,
		UniformTexelBuffer = 0x00000008,
		StorageTexelBuffer = 0x00000010,
		StorageTexelBufferAtomic = 0x00000020,
		VertexBuffer = 0x00000040,
		ColorAttachment = 0x00000080,
		ColorAttachmentBlend = 0x00000100,
		DepthStencilAttachment = 0x00000200,
		BlitSrc = 0x00000400,
		BlitDst = 0x00000800,
		SampledImageFilterLinear = 0x00001000,
		SampledImageFilterCubic = 0x00002000,
		TransferSrc = 0x00004000,
		TransferDst = 0x00008000,
		SampledImageFilterMinmax = 0x00010000,
		MidpointChromaSamples = 0x00020000,
		SampledImageYCbCrConversionLinearFilter = 0x00040000,
		SampledImageYCbCrConversionSeparateReconstructionFilter = 0x00080000,
		SampledImageYCbCrConversionChromaReconstructionExplicit = 0x00100000,
		SampledImageYCbCrConversionChromaReconstructionExplicitForceable = 0x00200000,
		Disjoint = 0x00400000,
		CositedChromaSamples = 0x00800000,
		StorageReadWithoutFormat = 0x80000000,
		StorageWriteWithoutFormat = 0x100000000,
		SampledImageDepthComparison = 0x200000000,
		// VK_KHR_format_feature_flags2 + VK_KHR_video_decode_queue
		VideoDecodeOutputKHR = 0x02000000,
		VideoDecodeDPBKHR = 0x04000000,
		// VK_KHR_format_feature_flags2 + VK_KHR_acceleration_structure
		AccelerationStructureVertexBufferKHR = 0x20000000,
		// VK_KHR_format_feature_flags2 + VK_EXT_fragment_density_map
		FragmentDensityMapEXT = 0x01000000,
		// VK_KHR_format_feature_flags2 + VK_KHR_video_encode_queue
		VideoEncodeInputKHR = 0x08000000,
		VideoEncodeDPBKHR = 0x10000000,
		// VK_KHR_format_feature_flags2 + VK_NV_linear_color_attachment
		LinearColorAttachmentNV = 0x4000000000,
		// VK_KHR_format_feature_flags2 + VK_QCOM_image_processing
		WeightImageQCOM = 0x400000000,
		WeightSampledImageQCOM = 0x800000000,
		BlockMatchingQCOM = 0x1000000000,
		BoxFilterSampledQCOM = 0x2000000000,
		// VK_NV_optical_flow
		OpticalFlowImageNV = 0x10000000000,
		OpticalFlowVectorNV = 0x20000000000,
		OpticalFlowCostNV = 0x40000000000
	}

	// Miscellaneous

	public enum VKVendorId : uint {
		// PCI-based IDs
		Intel = 0x8086,
		AMD = 0x1022,
		NVidia = 0x10DE,

		// Unique Khronos-assigned IDs
		VIV = 0x10001,
		VSI = 0x10002,
		Kazan = 0x10003,
		Codeplay = 0x10004,
		Mesa = 0x10005,
		POCL = 0x10006
	}

}
