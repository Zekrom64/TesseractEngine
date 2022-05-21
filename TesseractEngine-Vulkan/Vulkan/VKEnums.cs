using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Vulkan {

	// Vulkan 1.0

	using VkFlags = UInt32;

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

		DebugMarkerObjectNameInfoEXT = 1000022000,
		DebugMarkerObjectTagInfoEXT = 1000022001,
		DebugMarkerMarkerInfoEXT = 1000022002,
		DedicatedAllocationImageCreateInfoNV = 1000026000,
		DedicatedAllocationBufferCreateInfoNV = 1000026001,
		DedicatedAllocationMemoryAllocateCreateInfoNV = 1000026002,
		PhysicalDeviceTransformFeedbackFeaturesEXT = 1000028000,
		PhysicalDeviceTransformFeedbackPropertiesEXT = 1000028001,
		PipelineRasterizationStateStreamCreateInfoEXT = 1000028002,
		ImageViewHandleInfoNVX = 1000030000,
		ImageViewAddressPropertiesNVX = 1000030001,
		TextureLODGatherFormatPropertiesAMD = 1000041000,
		StreamDescriptorSurfaceCreateInfoGGP = 1000049000,
		PhysicalDeviceCornerSampledImageFeaturesNV = 1000050000,
		ExternalMemoryImageCreateInfoNV = 1000056000,
		ExportMemoryAllocateInfoNV = 1000056001,
		ImportMemoryWin32HandleInfoNV = 1000057000,
		ExportMemoryWin32HandleInfoNV = 1000057001,
		Win32KeyedMutexAcquireReleaseInfoNV = 1000058000,
		ValidationFlagsEXT = 1000061000,
		VISurfaceCreateInfoNN = 1000062000,
		PhysicalDeviceTextureCompressionASTC_HDRFeaturesEXT = 1000066000,
		ImageViewASTCDecodeModeEXT = 1000067000,
		PhysicalDeviceASTCDecodeFeaturesEXT = 1000067001,
		ImportMemoryWin32HandleInfoKHR = 1000073000,
		ExportMemoryWin32HandleInfoKHR = 1000073001,
		MemoryWin32HandlePropertiesKHR = 1000073002,
		MemoryGetWin32HandleInfoKHR = 1000073003,
		ImportMemoryFDInfoKHR = 1000074000,
		MemoryFDPropertiesKHR = 1000074001,
		MemoryGetFDInfoKHR = 1000074002,
		Win32KeyedMutexAcquireReleaseInfoKHR = 1000075000,
		ImportSemaphoreWin32HandleInfoKHR = 1000078000,
		ExportSemaphoreWin32HandleInfoKHR = 1000078001,
		D3D12FenceSubmitInfoKHR = 1000078002,
		SemaphoreGetWin32HandleInfoKHR = 1000078003,
		ImportSemaphoreFDInfoKHR = 1000079000,
		SemaphoreGetFDInfoKHR = 1000079001,
		PhysicalDevicePushDescriptorPropertiesKHR = 1000080000,
		CommandBufferInheritanceConditionalRenderingInfoEXT = 1000081000,
		PhysicalDeviceConditionalRenderingFeaturesEXT = 1000081001,
		ConditionalRenderingBeginInfoEXT = 1000081002,
		PresentRegionsKHR = 1000084000,
		PipelineViewportWScalingStateCreateInfoNV = 1000087000,
		SurfaceCapabilities2EXT = 1000090000,
		DisplayPowerInfoEXT = 1000091000,
		DeviceEventInfoEXT = 1000091001,
		DisplayEventInfoEXT = 1000091002,
		SwapchainCounterCreateInfoEXT = 1000091003,
		PresentTimesInfoGOOGLE = 1000092000,
		PhysicalDeviceMultiviewPerViewAttributesPropertiesNVX = 1000097000,
		PipelineViewportSwizzleStateCreateInfoNV = 1000098000,
		PhysicalDeviceDiscardRectanglePropertiesEXT = 1000099000,
		PipelineDiscardRectangleStateCreateInfoEXT = 1000099001,
		PhysicalDeviceConservativeRasterizationPropertiesEXT = 1000101000,
		PipelineRasterizationConservativeStateCreateInfoEXT = 1000101001,
		PhysicalDeviceDepthClipEnableFeaturesEXT = 1000102000,
		PipelineRasterizationDepthClipStateCreateInfoEXT = 1000102001,
		HDRMetadataEXT = 1000105000,
		SharedPresentSurfaceCapabilitiesKHR = 1000111000,
		ImportFenceWin32HandleInfoKHR = 1000114000,
		ExportFenceWin32HandleInfoKHR = 1000114001,
		FenceGetWin32HandleInfoKHR = 1000114002,
		ImportFenceFDInfoKHR = 1000115000,
		FenceGetFDInfoKHR = 1000115001,
		PhysicalDevicePerformanceQueryFeaturesKHR = 1000116000,
		PhysicalDevicePerformanceQueryPropertiesKHR = 1000116001,
		QueryPoolPerformanceCreateInfoKHR = 1000116002,
		PerformanceQuerySubmitInfoKHR = 1000116003,
		AcquireProfilingLockInfoKHR = 1000116004,
		PerformanceCounterKHR = 1000116005,
		PerformanceCounterDescriptionKHR = 1000116006,
		PhysicalDeviceSurfaceInfo2KHR = 1000119000,
		SurfaceCapabilities2KHR = 1000119001,
		SurfaceFormat2KHR = 1000119002,
		DisplayProperties2KHR = 1000121000,
		DisplayPlaneProperties2KHR = 1000121001,
		DisplayModeProperties2KHR = 1000121002,
		DisplayPlaneInfo2KHR = 1000121003,
		DisplayPlaneCapabilities2KHR = 1000121004,
		IOSSurfaceCreateInfoMVK = 1000122000,
		MacOSSurfaceCreateInfoMVK = 1000123000,
		DebugUtilsObjectNameInfoEXT = 1000128000,
		DebugUtilsObjectTagInfoEXT = 1000128001,
		DebugUtilsLabelEXT = 1000128002,
		DebugUtilsMessengerCallbackDataEXT = 1000128003,
		DebugUtilsMessengerCreateInfoEXT = 1000128004,
		AndroidHardwareBufferUsageANDROID = 1000129000,
		AndroidHadrwareBufferPropertiesANDROID = 1000129001,
		AndroidHardwareBufferFormatPropertiesANDROID = 1000129002,
		ImportAndroidHardwareBufferInfoANDROID = 1000129003,
		MemoryGetAndroidHardwareBufferInfoANDROID = 1000129004,
		ExternalFormatANDROID = 1000129005,
		PhysicalDeviceInlineUniformBlockFeaturesEXT = 1000138000,
		PhysicalDeviceInlineUniformBlockPropertiesEXT = 1000138001,
		WriteDescriptorSetInlineUniformBlockEXT = 1000138002,
		DescriptorPoolInlineUniformBlockCreateInfoEXT = 1000138003,
		SampleLocationsInfoEXT = 1000143000,
		RenderPassSampleLocationsBeginInfoEXT = 1000143001,
		PipelineSampleLocationsStateCreateInfoEXT = 1000143002,
		PhysicalDeviceSampleLocationsPropertiesEXT = 1000143003,
		MultisamplePropertiesEXT = 1000143004,
		PhysicalDeviceBlendOperationAdvancedFeaturesEXT = 1000148000,
		PhysicalDeviceBlendOperationAdvancedPropertiesEXT = 1000148001,
		PipelineColorBlendAdvancedStateCreateInfoEXT = 1000148002,
		PipelineCoverageToColorStateCreateInfo = 1000149000,
		BindAccelerationStructureMemoryInfoKHR = 1000165006,
		WriteDescriptorSetAccelerationStructureKHR = 1000165007,
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
		PhysicalDeviceRayTracingFeaturesKHR = 1000150013,
		PhysicalDeviceRayTracingPropertiesKHR = 1000150014,
		RayTracingPipelineCreateInfoKHR = 1000150015,
		RayTracingShaderGroupCreateInfoKHR = 1000150016,
		AccelerationStructureCreateInfoKHR = 1000150017,
		RayTracingPipelineInterfaceCreateInfoKHR = 1000150018,
		PipelineCoverageModulationStateCreateInfoNV = 1000152000,
		PhysicalDeviceShaderSMBuiltinsFeaturesNV = 1000154000,
		PhysicalDeviceShaderSMBuiltinsPropertiesNV = 1000154001,
		DRMFormatModifierPropertiesListEXT = 1000158000,
		DRMFormatModifierPropertiesEXT = 1000158001,
		PhysicalDeviceImageDRMFormatModifierInfoEXT = 1000158002,
		ImageDRMFormatModifierListCreateInfoEXT = 1000158003,
		ImageDRMFormatModifierExplicitCreateInfoEXT = 1000158004,
		ImageDRMFormatModifierPropertiesEXT = 1000158005,
		ValidationCacheCreateInfoEXT = 1000160000,
		ShaderModuleValidationCacheCreateInfoEXT = 1000160001,
		PipelineViewportShadingRateImageStateCreateInfoNV = 1000164000,
		PhysicalDeviceShadingRateImageFeaturesNV = 1000164001,
		PhysicalDeviceShadingRateImagePropertiesNV = 1000164002,
		PipelineViewportCoarseSampleOrderStateCreateInfoNV = 1000164005,
		RayTracingPipelineCreateInfoNV = 1000165000,
		AccelerationStructureCreateInfoNV = 1000165001,
		GeometryNV = 1000165003,
		GeometryTrianglesNV = 1000165004,
		GeometryAABB_NV = 1000165005,
		AccelerationStructureMemoryRequirementsInfoNV = 1000165008,
		PhysicalDeviceRayTracingPropertiesNV = 1000165009,
		RayTracingShaderGroupCreateInfoNV = 1000165011,
		AccelerationStructureInfoNV = 1000165012,
		PhysicalDeviceRepresentativeFragmentTestFeaturesNV = 1000166000,
		PipelineRepresentativeFragmentTestStateCreateInfoNV = 1000166001,
		PhysicalDeviceImageViewImageFormatInfoEXT = 1000170000,
		FilterCubicImageViewFormatPropertiesEXT = 1000170001,
		DeviceQueueGlobalPriorityCreateInfoEXT = 1000174000,
		ImportMemoryHostPointerInfoEXT = 1000178000,
		MemoryHostPointerPropertiesEXT = 1000178001,
		PhysicalDeviceExternalMemoryHostPropertiesEXT = 1000178002,
		PhysicalDeviceShaderClockFeaturesKHR = 1000181000,
		PipelineCompilerControlCreateInfoAMD = 1000183000,
		CalibratedTimestampInfoEXT = 1000184000,
		PhysicalDeviceShaderCorePropertiesAMD = 1000185000,
		DeviceMemoryOverallocationCreateInfoAMD = 1000189000,
		PhysicalDeviceVertexAttributeDivisorPropertiesEXT = 1000190000,
		PipelineVertexInputDivisorStateCreateInfoEXT = 1000190001,
		PhysicalDeviceVertexAttributeDivisorFeaturesEXT = 1000190002,
		PresentFrameTokenGGP = 1000191000,
		PipelineCreationFeedbackCreateInfoEXT = 1000192000,
		PhysicalDeviceComputeShaderDerivativesFeaturesNV = 1000201000,
		PhysicalDeviceMeshShaderFeaturesNV = 1000202000,
		PhysicalDeviceMeshShaderPropertiesNV = 1000202001,
		PhysicalDeviceFragmentShaderBarycentricFeaturesNV = 1000203000,
		PhysicalDeviceShaderImageFootprintFeaturesNV = 1000204000,
		PipelineViewportExclusiveScissorStateCreateInfoNV = 1000205000,
		PhysicalDeviceExclusiveScissorFeaturesNV = 1000205002,
		CheckpointDataNV = 1000206000,
		QueueFamilyCheckpointPropertiesNV = 1000206001,
		PhysicalDeviceShaderIntegerFunctions2FeaturesINTEL = 1000209000,
		QueryPoolPerformanceQueryCreateInfoINTEL = 1000210000,
		InitializePerformanceAPIInfoIntel = 1000210001,
		PerformanceMarkerInfoINTEL = 1000210002,
		PerformanceStreamMarkerInfoINTEL = 1000210003,
		PerformanceOverrideInfoINTEL = 1000210004,
		PerformanceConfigurationAcquireInfoINTEL = 1000210005,
		PhysicalDevicePCIBusInfoPropertiesEXT = 1000212000,
		DisplayNativeHDRSurfaceCapabilitiesAMD = 1000213000,
		SwapchainDisplayNativeHDRCreateInfoAMD = 1000213001,
		ImagepipeSurfaceCreateInfoFUCHISA = 1000214000,
		MetalSurfaceCreateInfoEXT = 1000217000,
		PhysicalDeviceFragmentDensityMapFeaturesEXT = 1000218000,
		PhysicalDeviceFragmentDensityMapPropertiesEXT = 1000218001,
		RenderPassFragmentDensityMapCreateInfoEXT = 1000218002,
		PhysicalDeviceSubgroupSizeControlPropertiesEXT = 1000225000,
		PipelineShaderStageRequiredSubgroupSizeCreateInfoEXT = 1000225001,
		PhysicalDeviceSubgroupSizeControlFeaturesEXT = 1000225002,
		PhysicalDeviceShaderCoreProperties2AMD = 1000227000,
		PhysicalDeviceCoherentMemoryFeaturesAMD = 1000229000,
		PhysicalDeviceMemoryBudgetPropertiesEXT = 1000237000,
		PhysicalDeviceMemoryPriorityFeaturesEXT = 1000238000,
		MemoryPriorityAllocateInfoEXT = 1000238001,
		SurfaceProtectedCapabilitiesKHR = 1000239000,
		PhysicalDeviceDedicatedAllocationImageAliasingFeaturesNV = 1000240000,
		PhysicalDeviceBufferDeviceAddressFeaturesEXT = 1000244000,
		BufferDeviceAddressCreateInfoEXT = 1000244002,
		PhysicalDeviceToolPropertiesEXT = 1000245000,
		ValidationFeaturesEXT = 1000247000,
		PhysicalDeviceCooperativeMatrixFeaturesNV = 1000249000,
		CooperativeMatrixPropertiesNV = 1000249001,
		PhysicalDeviceCooperativeMatrixPropertiesNV = 1000249002,
		PhysicalDeviceCoverageReductionModeFeaturesNV = 1000250000,
		PipelineCoverageReductionStateCreateInfoNV = 1000250001,
		FramebufferMixedSamplesCombinationNV = 1000250002,
		PhysicalDeviceFragmentShaderInterlockFeaturesEXT = 1000251000,
		PhysicalDeviceYcbcrImageArraysFeaturesEXT = 1000252000,
		SurfaceFullScreenExclusiveInfoEXT = 1000255000,
		SurfaceCapabilitiesFullScreenExclusiveEXT = 1000255002,
		SurfaceFullScreenExclusiveWin32InfoEXT = 1000255001,
		HeadlessSurfaceCreateInfoEXT = 1000256000,
		PhysicalDeviceLineRasterizationFeaturesEXT = 1000259000,
		PipelineRasterizationLineStateCreateInfoEXT = 1000259001,
		PhysicalDeviceLineRasterizationPropertiesEXT = 1000259002,
		PhysicalDeviceIndexTypeUInt8FeaturesEXT = 1000265000,
		PhysicalDeviceExtendedDynamicStateFeaturesEXT = 1000267000,
		DeferredOperationInfoKHR = 1000268000,
		PhysicalDevicePipelineExecutablePropertiesFeaturesKHR = 1000269000,
		PipelineInfoKHR = 1000269001,
		PiplineExecutablePropertiesKHR = 1000269002,
		PipelineExecutableInfoKHR = 1000269003,
		PipelineExecutableStatisticKHR = 1000269004,
		PipelineExecutableInternalRepresentationKHR = 1000269005,
		PhysicalDeviceShaderDemoteToHelperInvocationFeaturesEXT = 1000276000,
		PhysicalDeviceDeviceGeneratedCommandsPropertiesNV = 1000277000,
		GraphicsShaderGroupCreateInfoNV = 1000277001,
		GraphicsPipelineShaderGroupsCreateInfoNV = 1000277002,
		IndirectCommandsLayoutTokenNV = 1000277003,
		IndirectCommandsLayoutCreateInfoNV = 1000277004,
		GeneratedCommandsInfoNV = 1000277005,
		GeneratedCommandsMemoryRequirementsInfoNV = 1000277006,
		PhysicalDeviceGeneratedCommandsFeaturesNV = 1000277007,
		PhysicalDeviceTexelBufferAlignmentFeaturesEXT = 1000281000,
		PhysicalDeviceTexelBufferAlignmentPropertiesEXT = 1000281001,
		CommandBufferInheritanceRenderPassTransformInfoQCOM = 1000282000,
		RenderPassTransformBeginInfoQCOM = 1000282001,
		PhysicalDeviceRobustness2FeaturesEXT = 1000286000,
		PhysicalDeviceRobustness2PropertiesEXT = 1000286001,
		SamplerCustomBorderColorCreateInfoEXT = 1000287000,
		PhysicalDeviceCustomBorderColorPropertiesEXT = 1000287001,
		PhysicalDeviceCustomBorderColorFeaturesEXT = 1000287002,
		PipelineLibraryCreateInfoEXT = 1000290000,
		PhysicalDevicePrivateDataFeaturesEXT = 1000295000,
		DevicePrivateDataCreateInfoEXT = 1000295001,
		PrivateDataSlotCreateInfoEXT = 1000295002,
		PhysicalDevicePipelineCreationCacheControlFeaturesEXT = 1000297000,
		PhysicalDeviceDiagnosticsConfigFeaturesNV = 1000300000,
		DeviceDiagnosticsConfigCreateInfoNV = 1000300001,
		MemoryBarrier2KHR = 1000314000,
		BufferMemoryBarrier2KHR = 1000314001,
		ImageMemoryBarrier2KHR = 1000314002,
		DependencyInfoKHR = 1000314003,
		SubmitInfo2KHR = 1000314004,
		SemaphoreSubmitInfoKHR = 1000314005,
		CommandBufferSubmitInfoKHR = 1000314006,
		PhysicalDeviceSynchronization2FeaturesKHR = 1000314007,
		QueueFamilyCheckpointProperties2NV = 1000314008,
		CheckpointData2NV = 1000314009,
		PhysicalDeviceShaderSubgroupUniformControlFlowFeaturesKHR = 1000323000,
		PhysicalDeviceZeroInitializeWorkgroupMemoryFeaturesKHR = 1000325000,
		PhysicalDeviceFragmentShadingRateEnumsPropertiesNV = 1000326000,
		PhysicalDeviceFragmentShadingRateEnumsFeaturesNV = 1000326001,
		PipelineFragmentShadingRateEnumStateCreateInfoNV = 1000326002,
		AccelerationStructureGeometryMotionTrianglesDataNV = 1000327000,
		PhysicalDeviceRayTracingMotionBlurFeaturesNV = 1000327001,
		AccelerationStructureMotionInfoNV = 1000327002,
		PhysicalDeviceYcbcr2Plane444FormatsFeaturesEXT = 1000330000,
		PhysicalDeviceFragmentDensityMap2FeaturesEXT = 1000332000,
		PhysicalDeviceFragmentDensityMap2PropertiesEXT = 1000332001,
		CopyCommandTransformInfoQCOM = 1000333000,
		PhysicalDeviceImageRobustnessFeaturesEXT = 1000335000,
		PhysicalDeviceWorkgroupMemoryExplicitLayoutFeaturesKHR = 1000336000,
		CopyBufferInfo2KHR = 1000337000,
		CopyImageInfo2KHR = 1000337001,
		CopyBufferToImageInfo2KHR = 1000337002,
		CopyImageToBufferInfo2KHR = 1000337003,
		BlitImageInfo2KHR = 1000337004,
		ResolveImageInfo2KHR = 1000337005,
		BufferCopy2KHR = 1000337006,
		ImageCopy2KHR = 1000337007,
		ImageBlit2KHR = 1000337008,
		BufferImageCopy2KHR = 1000337009,
		ImageResolve2KHR = 1000337010,
		PhysicalDevice4444FormatsFeaturesEXT = 1000340000,
		DirectFBSurfaceCreateInfoEXT = 1000346000,
		PhysicalDeviceMutableDescriptorTypeFeaturesVALVE = 1000351000,
		MutableDescriptorTypeCreateInfoVALVE = 1000351002,
		PhysicalDeviceVertexInputDynamicStateFeaturesEXT = 1000352000,
		VertexInputBindingDescription2EXT = 1000352001,
		VertexInputAttributeDescription2EXT = 1000352002,
		PhysicalDeviceDRMPropertiesEXT = 1000353000,
		ImportMemoryZirconHandleInfoFUCHISA = 1000364000,
		MemoryZirconHandlePropertiesFUCHISA = 1000364001,
		MemoryGetZirconHandleInfoFUCHISA = 1000364002,
		ImportSemaphoreZirconHandleInfoFUCHISA = 1000365000,
		SemaphoreGetZirconHandleInfoFUCHISA = 1000365001,
		SubpassShadingPipelineCreateInfoHUAWEI = 1000369000,
		PhysicalDeviceSubpassShadingFeaturesHUAWEI = 1000369001,
		PhysicalDeviceSubpassShadingPropertiesHUAWEI = 1000369002,
		PhysicalDeviceInvocationMaskFeaturesHUAWEI = 1000370000,
		MemoryGetRemoteAddressInfoNV = 1000371000,
		PhysicalDeviceExternalMemoryRDMAFeaturesNV = 1000371001,
		PhysicalDeviceExtendedDynamicState2FeaturesEXT = 1000377000,
		ScreenSurfaceCreateInfoQNX = 1000378000,
		PhysicalDeviceColorWriteEnableFeaturesEXT = 1000381000,
		PipelineColorWriteCreateInfoEXT = 1000381001,
		PhysicalDeviceGlobalPriorityQueryFeaturesEXT = 1000388000,
		QueueFamilyGlobalPriorityPropertiesEXT = 1000388001,
		PhysicalDeviceMultiDrawFeaturesEXT = 1000392000,
		PhysicalDeviceMultiDrawPropertiesEXT = 1000392001
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
		PVRTC1_2BPPUNormBlockIMG = 1000054000,
		PVRTC1_4BPPUNormBlockIMG = 1000054001,
		PVRTC2_2BPPUNormBlockIMG = 1000054002,
		PVRTC2_4BPPUNormBlockIMG = 1000054003,
		PVRTC1_2BPPSRGBBlockIMG = 1000054004,
		PVRTC1_4BPPSRGBBlockIMG = 1000054005,
		PVRTC2_2BPPSRGBBlockIMG = 1000054006,
		PVRTC2_4BPPSRGBBlockIMG = 1000054007,
		ASTC4x4SFloatBlockEXT = 1000066000,
		ASTC5x4SFloatBlockEXT = 1000066001,
		ASTC5x5SFloatBlockEXT = 1000066002,
		ASTC6x5SFloatBlockEXT = 1000066003,
		ASTC6x6SFloatBlockEXT = 1000066004,
		ASTC8x5SFloatBlockEXT = 1000066005,
		ASTC8x6SFloatBlockEXT = 1000066006,
		ASTC8x8SFloatBlockEXT = 1000066007,
		ASTC10x5SFloatBlockEXT = 1000066008,
		ASTC10x6SFloatBlockEXT = 1000066009,
		ASTC10x8SFloatBlockEXT = 1000066010,
		ASTC10x10SFloatBlockEXT = 1000066011,
		ASTC12x10SFloatBlockEXT = 1000066012,
		ASTC12x12SFloatBlockEXT = 1000066013
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
		NoneQCOM = 1000301000
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

	public enum VKInstanceCreateFlagBits : VkFlags { }

	public enum VKFormatFeatureFlagBits : VkFlags {
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

	public enum VKImageUsageFlagBits : VkFlags {
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

	public enum VKImageCreateFlagBits : VkFlags {
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

	public enum VKSampleCountFlagBits : VkFlags {
		Count1Bit = 0x00000001,
		Count2Bit = 0x00000002,
		Count4Bit = 0x00000004,
		Count8Bit = 0x00000008,
		Count16Bit = 0x00000010,
		Count32Bit = 0x00000020,
		Count64Bit = 0x00000040
	}

	public enum VKQueueFlagBits : VkFlags {
		Graphics = 0x00000001,
		Compute = 0x00000002,
		Transfer = 0x00000004,
		SparseBinding = 0x00000008,
		Protected = 0x00000010
	}

	public enum VKMemoryPropertyFlagBits : VkFlags {
		DeviceLocal = 0x00000001,
		HostVisible = 0x00000002,
		HostCoherent = 0x00000004,
		HostCached = 0x00000008,
		LazilyAllocated = 0x00000010,
		ProtectedBit = 0x00000020,
		DeviceCoherentAMD = 0x00000040,
		DeviceUncachedAMD = 0x00000080
	}

	public enum VKMemoryHeapFlagBits : VkFlags {
		DeviceLocal = 0x00000001,
		MultiInstance = 0x00000002
	}

	public enum VKDeviceCreateFlagBits : VkFlags { }

	public enum VKDeviceQueueCreateFlagBits : VkFlags {
		Protected = 0x00000001
	}

	public enum VKPipelineStageFlagBits : VkFlags {
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

	public enum VKMemoryMapFlagBits : VkFlags { }

	public enum VKImageAspectFlagBits : VkFlags {
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

	public enum VKSparseImageFormatFlagBits : VkFlags {
		SingleMipTail = 0x00000001,
		AlignedMipSize = 0x00000002,
		NonStandardBlockSize = 0x00000004
	}

	public enum VKSparseMemoryBindFlagBits : VkFlags {
		Metadata = 0x00000001
	}

	public enum VKFenceCreateFlagBits : VkFlags {
		Signaled = 0x00000001
	}

	public enum VKSemaphoreCreateFlagBits : VkFlags { }

	public enum VKEventCreateFlagBits : VkFlags { }

	public enum VKQueryPoolCreateFlagBits : VkFlags { }

	public enum VKQueryPipelineStatisticFlagBits : VkFlags {
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

	public enum VKQueryResultFlagBits : VkFlags {
		Result64Bit = 0x00000001,
		Wait = 0x00000002,
		WithAvailability = 0x00000004,
		Partial = 0x00000008
	}

	public enum VKBufferCreateFlagBits : VkFlags {
		SparseBinding = 0x00000001,
		SparseResidency = 0x00000002,
		SparseAliased = 0x00000004,
		Protected = 0x000000008,
		DeviceAddressCaptureReplay = 0x00000010
	}

	public enum VKBufferUsageFlagBits : VkFlags {
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

	public enum VKBufferViewCreateFlagBits : VkFlags { }

	public enum VKImageViewCreateFlagBits : VkFlags {
		FragmentDensityMapDynamicEXT = 0x00000001
	}

	public enum VKShaderModuleCreateFlagBits : VkFlags { }

	public enum VKPipelineCacheCreateFlagBits : VkFlags {
		ExternallySynchronizedEXT = 0x00000001
	}

	public enum VKPipelineCreateFlagBits : VkFlags {
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

	public enum VKPipelineSHaderStageCreateFlagBits : VkFlags {
		AllowVaryingSubgroupSizeEXT = 0x00000001,
		RequireFullSubgroupsEXT = 0x00000002
	}

	public enum VKShaderStageFlagBits : VkFlags {
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

	public enum VKPipelineVertexInputStateCreateFlagBits : VkFlags { }

	public enum VKPipelineInputAssemblyStateCreateFlagBits : VkFlags { }

	public enum VKPipelineTessellationStateCreateFlagBits : VkFlags { }

	public enum VKPipelineViewportStateCreateFlagBits : VkFlags { }

	public enum VKPipelineRasterizationStateCreateFlagBits : VkFlags { }

	public enum VKCullModeFlagBits : VkFlags {
		None = 0,
		Front = 0x00000001,
		Back = 0x00000002,
		FrontAndBack = 0x00000003
	}

	public enum VKPipelineMultisampleStateCreateFlagBits : VkFlags { }

	public enum VKPipelineDepthStencilStateCreateFlagBits : VkFlags { }

	public enum VKPipelineColorBlendStateCreateFlagBits : VkFlags { }

	public enum VKColorComponentFlagBits : VkFlags {
		R = 0x00000001,
		G = 0x00000002,
		B = 0x00000004,
		A = 0x00000008
	}

	public enum VKPipelineDynamicStateCreateFlagBits : VkFlags { }

	public enum VKPipelineLayoutCreateFlagBits : VkFlags { }

	public enum VKSamplerCreateFlagBits : VkFlags {
		SubsampledEXT = 0x00000001,
		SubsampledCoarseReconstructionEXT = 0x00000002
	}

	public enum VKDescriptorSetLayoutCreateFlagBits : VkFlags {
		UpdateAfterBindPool = 0x00000002,
		PushDescriptorKHR = 0x00000001
	}

	public enum VKDescriptorPoolCreateFlagBits : VkFlags {
		FreeDescriptorSet = 0x00000001,
		UpdateAfterBind = 0x00000002
	}

	public enum VKDescriptorPoolResetFlagBits : VkFlags { }

	public enum VKFramebufferCreateFlagBits : VkFlags {
		Imageless = 0x00000001
	}

	public enum VKRenderPassCreateFlagBits : VkFlags {
		TransformQCOM = 0x00000002
	}

	public enum VKAttachmentDescriptionFlagBits : VkFlags {
		MayAlias = 0x00000001
	}

	public enum VKSubpassDescriptionFlagBits : VkFlags {
		PerViewAttributesNVX = 0x00000001,
		PerViewPositionXOnlyNVX = 0x00000002,
		FragmentRegionQCOM = 0x00000004,
		ShaderResolveQCOM = 0x00000008
	}

	public enum VKAccessFlagBits : VkFlags {
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

	public enum VKDependencyFlagBits : VkFlags {
		ByRegion = 0x00000001,
		DeviceGroup = 0x0000004,
		ViewLocal = 0x00000002
	}

	public enum VKCommandPoolCreateFlagBits : VkFlags {
		Transient = 0x00000001,
		ResetCommandBuffer = 0x00000002,
		Protected = 0x00000004
	}

	public enum VKCommandPoolResetFlagBits : VkFlags {
		ReleaseResources = 0x00000001
	}

	public enum VKCommandBufferUsageFlagBits : VkFlags {
		OneTimeSubmit = 0x00000001,
		RenderPassContinue = 0x00000002,
		SimultaneousUse = 0x00000004
	}

	public enum VKQueryControlFlagBits : VkFlags {
		Precise = 0x00000001
	}

	public enum VKCommandBufferResetFlagBits : VkFlags {
		ReleaseResources = 0x00000001
	}

	public enum VKStencilFaceFlagBits : VkFlags {
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

	public enum VKSubgroupFeatureFlagBits : VkFlags {
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

	public enum VKPeerMemoryFeatureFlagBits : VkFlags {
		CopySrc = 0x00000001,
		CopyDst = 0x00000002,
		GenericSrc = 0x00000004,
		GenericDst = 0x00000008
	}

	public enum VKMemoryAllocateFlagBits : VkFlags {
		DeviceMask = 0x00000001,
		DeviceAddress = 0x00000002,
		DeviceAddressCaptureReplay = 0x00000004
	}

	public enum VKCommandPoolTrimFlags : VkFlags { }

	public enum VKDescriptorUpdateTemplateCreateFlags : VkFlags { }

	public enum VKExternalMemoryHandleTypeFlagBits : VkFlags {
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

	public enum VKExternalMemoryFeatureFlagBits : VkFlags {
		DedicatedOnly = 0x00000001,
		Exportable = 0x00000002,
		Importable = 0x00000004
	}

	public enum VKExternalFenceHandleTypeFlagBits : VkFlags {
		OpaqueFD = 0x00000001,
		OpaqueWin32 = 0x00000002,
		OpaqueWin32KMT = 0x00000004,
		SyncFD = 0x00000008
	}

	public enum VKExternalFenceFeatureFlagBits : VkFlags {
		Exportable = 0x00000001,
		Importable = 0x00000002
	}

	public enum VKFenceImportFlagBits : VkFlags {
		Temporary = 0x00000001
	}

	public enum VKSemaphoreImportFlagBits : VkFlags {
		Temporary = 0x00000001
	}

	public enum VKExternalSemaphoreHandleTypeFlagBits : VkFlags {
		OpaqueFD = 0x00000001,
		OpaqueWin32 = 0x00000002,
		OpaqueWin32KMT = 0x00000004,
		D3D12Fence = 0x00000008,
		SyncFD = 0x00000010
	}

	public enum VKExternalSemaphoreFeatureFlagBits : VkFlags {
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

	public enum VKResolveModeFlagBits : VkFlags {
		None = 0,
		SampleZero = 0x00000001,
		Average = 0x00000002,
		Min = 0x00000004,
		Max = 0x00000008
	}

	public enum VKDescriptorBindingFlagBits : VkFlags {
		UpdateAfterBind = 0x00000001,
		UpdateUnusedWhilePending = 0x00000002,
		PartiallyBound = 0x00000004,
		VariableDescriptorCount = 0x00000008
	}

	public enum VKSemaphoreWaitFlagBits {
		Any = 0x00000001
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
