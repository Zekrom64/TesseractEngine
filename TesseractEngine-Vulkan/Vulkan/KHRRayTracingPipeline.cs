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

#nullable disable
	public class KHRRayTracingPipelineFunctions {

		public delegate void PFN_vkCmdSetRayTracingPipelineStackSizeKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint pipelineStackSize);
		public delegate void PFN_vkCmdTraceRaysIndirectKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKStridedDeviceAddressRegionKHR raygenShaderBindingTable, in VKStridedDeviceAddressRegionKHR missShaderBindingTable, in VKStridedDeviceAddressRegionKHR hitShaderBindingTable, in VKStridedDeviceAddressRegionKHR callableShaderBindingTable, [NativeType("VkDeviceAddress")] ulong indirectDeviceAddress);
		public delegate void PFN_vkCmdTraceRaysKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKStridedDeviceAddressRegionKHR raygenShaderBindingTable, in VKStridedDeviceAddressRegionKHR missShaderBindingTable, in VKStridedDeviceAddressRegionKHR hitShaderBindingTable, in VKStridedDeviceAddressRegionKHR callableShaderBindingTable, uint width, uint height, uint depth);
		public delegate VKResult PFN_vkCreateRayTracingPipelinesKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong deferredOperation, [NativeType("VkPipelineCache")] ulong pipelineCache, uint createInfoCount, [NativeType("const VkRayTracingPipelineCreateInfoKHR*")] IntPtr pCreateInfos, [NativeType("const VkAllocationCallbacks*")] IntPtr pAllocator, [NativeType("VkPipeline*")] IntPtr pPipelines);
		public delegate VKResult PFN_vkGetRayTracingCaptureReplayShaderGroupHandlesKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkPipeline")] ulong pipeline, uint firstGroup, uint groupCount, nuint dataSize, IntPtr pData);
		public delegate VKResult PFN_vkGetRayTracingShaderGroupHandlesKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkPipeline")] ulong pipeline, uint firstGroup, uint groupCount, nuint dataSize, IntPtr pData);
		public delegate ulong PFN_vkGetRayTracingShaderGroupStackSizeKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkPipeline")] ulong pipeline, uint group, VKShaderGroupShaderKHR groupShader);

		public PFN_vkCmdSetRayTracingPipelineStackSizeKHR vkCmdSetRayTracingPipelineStackSizeKHR;
		public PFN_vkCmdTraceRaysIndirectKHR vkCmdTraceRaysIndirectKHR;
		public PFN_vkCmdTraceRaysKHR vkCmdTraceRaysKHR;
		public PFN_vkCreateRayTracingPipelinesKHR vkCreateRayTracingPipelinesKHR;
		public PFN_vkGetRayTracingCaptureReplayShaderGroupHandlesKHR vkGetRayTracingCaptureReplayShaderGroupHandlesKHR;
		public PFN_vkGetRayTracingShaderGroupHandlesKHR vkGetRayTracingShaderGroupHandlesKHR;
		public PFN_vkGetRayTracingShaderGroupStackSizeKHR vkGetRayTracingShaderGroupStackSizeKHR;

	}
#nullable disable

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
