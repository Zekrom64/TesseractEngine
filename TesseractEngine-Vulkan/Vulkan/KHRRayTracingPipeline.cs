using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public static class KHRRayTracingPipeline {

		public const string ExtensionName = "VK_KHR_ray_tracing_pipeline";

	}

	public unsafe class KHRRayTracingPipelineFunctions {

		[NativeType("void vkCmdSetRayTracingPipelineStackSizeKHR(VkCommandBuffer cmdbuf, uint32_t pipelineStackSize)")]
		public delegate* unmanaged<IntPtr, uint, void> vkCmdSetRayTracingPipelineStackSizeKHR;
		[NativeType("void vkCmdTraceRaysIndirectKHR(VkCommandBuffer cmdbuf, const VkStridedDeviceAddressRegionKHR* pRaygenShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pMissShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pHitShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pCallableShaderBindingTable, VkDeviceAddress indirectDeviceAddress)")]
		public delegate* unmanaged<IntPtr, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, ulong, void> vkCmdTraceRaysIndirectKHR;
		[NativeType("void vkCmdTraceRaysKHR(VkCommandBuffer cmdbuf, const VkStridedDeviceAddressRegionKHR* pRaygenShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pMissShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pHitShaderBindingTable, const VkStridedDeviceAddressRegionKHR* pCallableShaderBindingTable, uint32_t width, uint32_t height, uint32_t depth")]
		public delegate* unmanaged<IntPtr, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, in VKStridedDeviceAddressRegionKHR, uint, uint, uint, void> vkCmdTraceRaysKHR;
		[NativeType("VkResult vkCreateRayTracingPipelinesKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, VkPipelineCache pipelineCache, uint32_t createInfoCount, const VkRayTracingPipelineCreateInfoKHR* pCreateInfos, const VkAllocationCallbacks* pAllocator, VkPipeline* pPipelines)")]
		public delegate* unmanaged<IntPtr, ulong, ulong, uint, VKRayTracingPipelineCreateInfoKHR*, VKAllocationCallbacks*, ulong*, VKResult> vkCreateRayTracingPipelinesKHR;
		[NativeType("VkResult vkGetRayTracingCaptureReplayShaderGroupHandlesKHR(VkDevice device, VkPipeline pipeline, uint32_t firstGroup, uint32_t groupCount, size_t dataSize, void* data)")]
		public delegate* unmanaged<IntPtr, ulong, uint, uint, nuint, IntPtr, VKResult> vkGetRayTracingCaptureReplayShaderGroupHandlesKHR;
		[NativeType("VkResult vkGetRayTracingShaderGroupHandlesKHR(VkDevice device, VkPipeline pipeline, uint32_t firstGroup, uint32_t groupCount, size_t dataSize, void* data)")]
		public delegate* unmanaged<IntPtr, ulong, uint, uint, nuint, IntPtr, VKResult> vkGetRayTracingShaderGroupHandlesKHR;
		[NativeType("VkDeviceSize vkGetRayTracingShaderGroupStackSizeKHR(VkDevice device, VkPipeline pipeline, uint32_t group, VkShaderGroupShaderKHR groupShader)")]
		public delegate* unmanaged<IntPtr, ulong, uint, VKShaderGroupShaderKHR, ulong> vkGetRayTracingShaderGroupStackSizeKHR;

	}

	public enum VKRayTracingShaderGroupTypeKHR {
		General = 0,
		TrianglesHitGroup,
		ProceduralHitGroup
	}

	public enum VKShaderGroupShaderKHR {
		General = 0,
		ClosestHit,
		AnyHit,
		Intersection
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRayTracingPipelineCreateInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineCreateFlagBits Flags;
		public uint StageCount;
		[NativeType("const VkPipelineShaderStageCreateInfo*")]
		public IntPtr Stages;
		public uint GroupCount;
		[NativeType("const VkRayTracingShaderGroupCreateInfoKHR*")]
		public IntPtr Groups;
		public uint MaxPipelineRayRecursionDepth;
		[NativeType("const VkPipelineLibraryCreateInfoKHR*")]
		public IntPtr LibraryInfo;
		[NativeType("const VkRayTracingPipelineInterfaceCreateInfoKHR*")]
		public IntPtr LibraryInterface;
		[NativeType("const VkPipelineDynamicStateCreateInfo*")]
		public IntPtr DynamicState;
		[NativeType("VkPipelineLayout")]
		public ulong Layout;
		[NativeType("VkPipeline")]
		public ulong BasePipelineHandle;
		public int BasePipelineIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRayTracingPipelineInterfaceCreateInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxPipelineRayPayloadSize;
		public uint MaxPipelineRayHitAttributeSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRayTracingShaderGroupCreateInfoKHR {

		public VKStructureType SType;
		public IntPtr Next;
		public VKRayTracingShaderGroupTypeKHR Type;
		public uint GeneralShader;
		public uint ClosestHitShader;
		public uint AnyHitShader;
		public uint IntersectionShader;
		public IntPtr ShaderGroupCaptureReplayHandle;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKStridedDeviceAddressRegionKHR {

		[NativeType("VkDeviceAddress")]
		public ulong DeviceAddress;
		[NativeType("VkDeviceSize")]
		public ulong Stride;
		[NativeType("VkDeviceSize")]
		public ulong Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKTraceRaysIndirectCommandKHR {

		public uint Width;
		public uint Height;
		public uint Depth;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceRayTracingPipelineFeaturesKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 RayTracingPipeline;
		public VKBool32 RayTracingPipelineShaderGroupHandleCaptureReplay;
		public VKBool32 RayTracingPipelineShaderGroupHandleCaptureReplayMixed;
		public VKBool32 RayTracingPipelineTraceRaysIndirect;
		public VKBool32 RayTraversalPrimitiveCulling;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceRayTracingPipelinePropertiesKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public uint ShaderGroupHandleSize;
		public uint MaxRayRecursionDepth;
		public uint MaxShaderGroupStride;
		public uint ShaderGroupBaseAlignment;
		public uint ShaderGroupHandleCaptureReplaySize;
		public uint MaxRayDispatchInvocationCount;
		public uint ShaderGroupHandleAlignment;
		public uint MaxRayHitAttributeSize;

	}

}
