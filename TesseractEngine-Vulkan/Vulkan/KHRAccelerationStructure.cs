using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	using VkAccelerationStructureKHR = UInt64;
	using VKDeviceOrHostAddressConstKHR = VKDeviceOrHostAddressKHR;

	public static class KHRAccelerationStructure {

		public const string ExtensionName = "VK_KHR_acceleration_structure";

	}

	public unsafe class KHRAccelerationStructureFunctions {

		[NativeType("VkResult vkBuildAccelerationStructuresKHR(VkDevice device, VkDeferredOperation deferredOperation, uint32_t infoCount, const VkAccelerationStructureBuildGeometryInfoKHR* pInfos, const VkAccelerationStructureBuildRangeInfoKHR* const* ppBuildRangeInfos)")]
		public delegate* unmanaged<IntPtr, ulong, uint, VKAccelerationStructureBuildGeometryInfoKHR*, VkAccelerationStructureBuildRangeInfoKHR**, VKResult> vkBuildAccelerationStructuresKHR;
		[NativeType("void vkCmdBuildAccelerationStructuresIndirectKHR(VkCommandBuffer cmdbuf, uint32_t infoCount, const VkAccelerationStructureBuildGeometryInfoKHR* pInfos, const VkDeviceAddress* pIndirectDeviceAddresses, const uint32_t* pIndirectStrides, const uint32_t* const* ppMaxPrimitiveCounts)")]
		public delegate* unmanaged<IntPtr, uint, VKAccelerationStructureBuildGeometryInfoKHR*, ulong*, uint*, uint**, void> vkCmdBuildAccelerationStructuresIndirectKHR;
		[NativeType("void vkCmdBuildAccelerationStructuresKHR(VkCommandBuffer cmdbuf, uint32_t infoCount, const VkAccelerationStructureBuildGeometryInfoKHR* pInfos, const VkAccelerationStructureBuildRangeInfoKHR* const* ppBuildRangeInfos)")]
		public delegate* unmanaged<IntPtr, uint, VKAccelerationStructureBuildGeometryInfoKHR*, VkAccelerationStructureBuildRangeInfoKHR**, void> vkCmdBuildAccelerationStructuresKHR;
		[NativeType("void vkCmdCopyAccelerationStructureKHR(VkCommandBuffer cmdbuf, const VkCopyAccelerationStructureInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, VKCopyAccelerationStructureInfoKHR*, void> vkCmdCopyAccelerationStructureKHR;
		[NativeType("void vkCmdCopyAccelerationStructureToMemoryKHR(VkCommandBuffer cmdbuf, const VkCopyAccelerationStructureToMemoryInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, VKCopyAccelerationStructureToMemoryInfoKHR*, void> vkCmdCopyAccelerationStructureToMemoryKHR;
		[NativeType("void vkCmdCopyMemoryToAccelerationStructureKHR(VkCommandBuffer cmdbuf, const VkCopyMemoryToAccelerationStructureInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, VKCopyMemoryToAccelerationStructureInfoKHR*, void> vkCmdCopyMemoryToAccelerationStructureKHR;
		[NativeType("void vkCmdWriteAccelerationStructuresPropertiesKHR(VkCommandBuffer cmdbuf, uint32_t accelerationStructureCount, const VkAccelerationStructureKHR* pAccelerationStructures, VkQueryType queryType, VkQueryPool queryPool, uint32_t firstQuery)")]
		public delegate* unmanaged<IntPtr, uint, ulong*, VKQueryType, ulong, uint, void> vkCmdWriteAccelerationStructuresPropertiesKHR;
		[NativeType("VkResult vkCopyAccelerationStructuresKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, const VkCopyAccelerationStructureInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, ulong, VKCopyAccelerationStructureInfoKHR*, VKResult> vkCopyAccelerationStructureKHR;
		[NativeType("VkResult vkCopyAccelerationStructureToMemoryKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, const VkCopyAccelerationStructureToMemoryInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, ulong, VKCopyAccelerationStructureToMemoryInfoKHR*, VKResult> vkCopyAccelerationStructureToMemoryKHR;
		[NativeType("VkResult vkCopyMemoryToAccelerationStructureKHR(VkDevice device, VkDeferredOperationKHR deferredOperation, const VkCopyMemoryToAccelerationStructureInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, ulong, VKCopyMemoryToAccelerationStructureInfoKHR*, VKResult> vkCopyMemoryToAccelerationStructureKHR;
		[NativeType("VkResult vkCreateAccelerationStructureKHR(VkDevice device, const VkAccelerationStructureCreateInfoKHR* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkAccelerationStructure* pAccelerationStructure)")]
		public delegate* unmanaged<IntPtr, VKAccelerationStructureCreateInfoKHR*, VKAllocationCallbacks*, ulong*, VKResult> vkCreateAccelerationStructureKHR;
		[NativeType("void vkDestroyAccelerationStructureKHR(VkDevice device, VkAccelerationStructureKHR accelerationStructure, const VkAllocationCallbacks* pAllocator)")]
		public delegate* unmanaged<IntPtr, ulong, VKAllocationCallbacks*, void> vkDestroyAccelerationStructureKHR;
		[NativeType("void vkGetAccelerationStructureBuildSizesKHR(VkDevice device, VkAccelerationStructureBuildTypeKHR buildType, const VkAccelerationStructureBuildGeometryInfoKHR* pBuildInfo, const uint32_t* pMaxPrimitiveCounts, VkAccelerationStructureBuildSizesInfoKHR* pSizeInfo)")]
		public delegate* unmanaged<IntPtr, VKAccelerationStructureBuildTypeKHR, VKAccelerationStructureBuildGeometryInfoKHR*, uint*, VkAccelerationStructureBuildSizesInfoKHR*, void> vkGetAccelerationStructureBuildSizesKHR;
		[NativeType("uint64_t vkGetAccelerationStructureDeviceAddressKHR(VkDevice device, const VkAccelerationStructureDeviceAddressInfoKHR* pInfo)")]
		public delegate* unmanaged<IntPtr, VKAccelerationStructureDeviceAddressInfoKHR*, ulong> vkGetAccelerationStructureDeviceAddressKHR;
		[NativeType("void vkGetDeviceAccelerationStructureCompatibilityKHR(VkDevice device, const VkAccelerationStructureVersionInfoKHR* pVersionInfo, VkAccelerationStructureCompatibilityKHR* pCompatibility)")]
		public delegate* unmanaged<IntPtr, VKAccelerationStructureVersionInfoKHR*, VKAccelerationStructureCompatibilityKHR*, void> vkGetDeviceAccelerationStructureCompatibilityKHR;
		[NativeType("VkResult vkWriteAccelerationStructuresPropertiesKHR(VkDevice device, uint32_t accelerationStructureCount, const VkAccelerationStructureKHR* pAccelerationStructures, VkQueryType queryType, size_t dataSize, void* data, size_t stride)")]
		public delegate* unmanaged<IntPtr, uint, ulong*, VKQueryType, nuint, IntPtr, nuint, VKResult> vkWriteAccelerationStructuresPropertiesKHR;

	}

	public enum VKAccelerationStructureBuildTypeKHR {
		Host = 0,
		Device,
		HostOrDevice
	}

	public enum VKAccelerationStructureCompatibilityKHR {
		Compatible = 0,
		Incompatible
	}

	[Flags]
	public enum VKAccelerationStructureCreateFlagBitsKHR {
		DeviceAddressCaptureReplay = 0x00000001,
		// VK_EXT_descriptor_buffer
		DescriptorBufferCaptureReplayEXT = 0x00000008,
		// VK_NV_ray_tracing_motion_blur
		MotionNV = 0x00000004
	}

	public enum VKAccelerationStructureTypeKHR {
		TopLevel = 0,
		BottomLevel,
		Generic
	}

	[Flags]
	public enum VKBuildAccelerationStructureFlagBitsKHR {
		AllowUpdate = 0x00000001,
		AllowCompaction = 0x00000002,
		PreferFastTrace = 0x00000004,
		PreferFastBuild = 0x00000008,
		LowMemory = 0x00000010,
		// VK_NV_ray_tracing_motion_blur
		MotionNV = 0x00000020,
		// VK_EXT_opacity_micromap
		AllowOpacityMicromapUpdateEXT = 0x00000040,
		AllowDisableOpacityMicromapsEXT = 0x00000080,
		AllowOpacityMicromapDataUpdateEXT = 0x00000100
	}

	public enum VKBuildAccelerationStructureModeKHR {
		Build = 0,
		Update
	}

	public enum VKCopyAccelerationStructureModeKHR {
		Clone = 0,
		Compact,
		Serialize,
		Deserialize
	}

	[Flags]
	public enum VKGeometryFlagBitsKHR {
		Opaque = 0x00000001,
		NoDuplicateAnyHitInvocation = 0x00000002
	}

	[Flags]
	public enum VKGeometryInstanceFlagBitsKHR {
		TriangleFacingCullDisable = 0x00000001,
		TriangleFlipFacing = 0x00000002,
		ForceOpaque = 0x00000004,
		ForceNoOpaque = 0x00000008,
		TriangleFrontCounterclockwise = TriangleFlipFacing,
		// VK_EXT_opacity_micromap
		ForceOpacityMicromap2StateEXT = 0x00000010,
		DisableOpacityMicromapsEXT = 0x00000020
	}

	public enum VKGeometryTypeKHR {
		Triangles = 0,
		AABBs,
		Instances
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct VKDeviceOrHostAddressKHR {

		[FieldOffset(0)]
		public ulong DeviceAddress;
		[FieldOffset(0)]
		public IntPtr HostAddress;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAabbPositionsKHR {

		public float MinX;
		public float MinY;
		public float MinZ;
		public float MaxX;
		public float MaxY;
		public float MaxZ;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureBuildGeometryInfoKHR {

		public VKStructureType SType;
		public IntPtr Next;
		public VKAccelerationStructureTypeKHR Type;
		public VKAccelerationStructureCreateFlagBitsKHR Flags;
		public VKBuildAccelerationStructureModeKHR Mode;
		public VkAccelerationStructureKHR SrcAccelerationStructure;
		public VkAccelerationStructureKHR DstAccelerationStructure;
		public uint GeometryCount;
		[NativeType("const VkAccelerationStructureGeometryKHR*")]
		public IntPtr PGeometries;
		[NativeType("const VkAccelerationStructureGeometryKHR* const*")]
		public IntPtr PPGeometries;
		public VKDeviceOrHostAddressKHR ScratchData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VkAccelerationStructureBuildRangeInfoKHR {

		public uint PrimitiveCount;
		public uint PrimitiveOffset;
		public uint FirstVertex;
		public uint TransformOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VkAccelerationStructureBuildSizesInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public ulong AccelerationStructureSize;
		public ulong UpdateScratchSize;
		public ulong BuildScratchSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureCreateInfoKHR {

		public VKStructureType SType;
		public IntPtr Next;
		public VKAccelerationStructureCreateFlagBitsKHR CreateFlags;
		[NativeType("VkBuffer")]
		public ulong Buffer;
		public ulong Offset;
		public ulong Size;
		public VKAccelerationStructureTypeKHR Type;
		[NativeType("VkDeviceAddress")]
		public ulong DeviceAddress;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureDeviceAddressInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VkAccelerationStructureKHR AccelerationStructure;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureGeometryAabbsDataKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDeviceOrHostAddressConstKHR Data;
		public ulong Stride;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureGeometryInstancesDataKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ArrayOfPointers;
		public VKDeviceOrHostAddressConstKHR Data;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureGeometryTrianglesDataKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKFormat VertexFormat;
		public VKDeviceOrHostAddressConstKHR VertexData;
		public ulong VertexStride;
		public uint MaxVertex;
		public VKIndexType IndexType;
		public VKDeviceOrHostAddressConstKHR IndexData;
		public VKDeviceOrHostAddressConstKHR TransformData;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct VKAccelerationStructureGeometryDataKHR {

		[FieldOffset(0)]
		public VKAccelerationStructureGeometryTrianglesDataKHR Triangles;
		[FieldOffset(0)]
		public VKAccelerationStructureGeometryAabbsDataKHR Aabbs;
		[FieldOffset(0)]
		public VKAccelerationStructureGeometryInstancesDataKHR Instances;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureGeometryKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKGeometryTypeKHR GeometryType;
		public VKAccelerationStructureGeometryDataKHR Geometry;
		public VKGeometryFlagBitsKHR Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureInstanceKHR {

		public VKTransformMatrixKHR Transform;
		private uint pack0;
		public uint InstanceCustomIndex {
			get => pack0 & 0xFFFFFF;
			set => pack0 = (pack0 & 0xFF000000) | (value & 0xFFFFFF);
		}
		public uint Mask {
			get => pack0 >> 24;
			set => pack0 = (pack0 & 0xFFFFFF) | (value << 24);
		}
		private uint pack1;
		public uint InstanceShaderBindingTableRecordOffset {
			get => pack1 & 0xFFFFFF;
			set => pack1 = (pack1 & 0xFF000000) | (value & 0xFFFFFF);
		}
		public VKGeometryInstanceFlagBitsKHR Flags {
			get => (VKGeometryInstanceFlagBitsKHR)(pack1 >> 24);
			set => pack1 = (pack1 & 0xFFFFFF) | ((uint)value << 24);
		}
		public ulong AccelerationStructureReference;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAccelerationStructureVersionInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("const uint8_t*")]
		public IntPtr VersionData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyAccelerationStructureInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VkAccelerationStructureKHR Src;
		public VkAccelerationStructureKHR Dst;
		public VKCopyAccelerationStructureModeKHR Mode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyAccelerationStructureToMemoryInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VkAccelerationStructureKHR Src;
		public VKDeviceOrHostAddressKHR Dst;
		public VKCopyAccelerationStructureModeKHR Mode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyMemoryToAccelerationStructureInfoKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDeviceOrHostAddressConstKHR Src;
		public VkAccelerationStructureKHR Dst;
		public VKCopyAccelerationStructureModeKHR Mode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKTransformMatrixKHR {

		private unsafe fixed float matrix[3 * 4];

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceAccelerationStructureFeaturesKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 AccelerationStructure;
		public VKBool32 AccelerationStructureCaptureReplay;
		public VKBool32 AccelerationStructureIndirectBuild;
		public VKBool32 AccelerationStructureHostCommands;
		public VKBool32 DescriptorBindingAccelerationStructureUpdateAfterBind;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceAccelerationStructurePropertiesKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public ulong MaxGeometryCount;
		public ulong MaxInstanceCount;
		public ulong MaxPrimitiveCount;
		public uint MaxPerStageDescriptorAccelerationStructures;
		public uint MaxPerStageDescriptorUpdateAfterBindAccelerationStructures;
		public uint MaxDescriptorSetAccelerationStructures;
		public uint MaxDescriptorSetUpdateAfterBindAccelerationStructures;
		public uint MinAccelerationStructuresScratchOffsetAlignment;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKWriteDescriptorSetAccelerationStructureKHR {

		public VKStructureType Type;
		public IntPtr Next;
		public uint AccelerationStructureCount;
		[NativeType("const VkAccelerationStructureKHR*")]
		public IntPtr AccelerationStructures;

	}


}
