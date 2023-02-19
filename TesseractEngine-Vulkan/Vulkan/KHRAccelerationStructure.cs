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

#nullable disable
	public class KHRAccelerationStructureFunctions {

		public delegate VKResult PFN_vkBuildAccelerationStructuresKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong deferredOperation, uint infoCount, [NativeType("const VkAccelerationStructureBuildGeometryInfoKHR*")] IntPtr pInfos, [NativeType("const VkAccelerationStructureBuildRangeInfoKHR* const*")] IntPtr ppBuildRangeInfos);
		public delegate void PFN_vkCmdBuildAccelerationStructuresIndirectKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint infoCount, [NativeType("const VkAccelerationStructureBuildGeometryInfoKHR*")] IntPtr pInfos, [NativeType("const VkDeviceAddress*")] IntPtr pIndirectDeviceAddresses, [NativeType("const uint32_t*")] IntPtr pIndirectStrides, [NativeType("const uint32_t* const*")] IntPtr ppMaxPrimitiveCounts);
		public delegate void PFN_vkCmdBuildAccelerationStructuresKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint infoCount, [NativeType("const VkAccelerationStructureBuildGeometryInfoKHR*")] IntPtr pInfos, [NativeType("const VkAccelerationStructureBuildRangeInfoKHR* const*")] IntPtr ppBuildRangeInfos);
		public delegate void PFN_vkCmdCopyAccelerationStructureKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKCopyAccelerationStructureInfoKHR info);
		public delegate void PFN_vkCmdCopyAccelerationStructureToMemoryKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKCopyAccelerationStructureToMemoryInfoKHR info);
		public delegate void PFN_vkCmdCopyMemoryToAccelerationStructureKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, in VKCopyMemoryToAccelerationStructureInfoKHR info);
		public delegate void PFN_vkCmdWriteAccelerationStructuresPropertiesKHR([NativeType("VkCommandBuffer")] IntPtr commandBuffer, uint accelerationStructureCount, [NativeType("const VkAccelerationStructureKHR*")] IntPtr pAccelerationStructures, VKQueryType queryType, [NativeType("VkQueryPool")] ulong queryPool, uint firstQuery);
		public delegate VKResult PFN_vkCopyAccelerationStructureKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong deferredOperation, in VKCopyAccelerationStructureInfoKHR info);
		public delegate VKResult PFN_vkCopyAccelerationStructureToMemoryKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong deferredOperation, in VKCopyAccelerationStructureToMemoryInfoKHR info);
		public delegate VKResult PFN_vkCopyMemoryToAccelerationStructureKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkDeferredOperationKHR")] ulong deferredOperation, in VKCopyMemoryToAccelerationStructureInfoKHR info);
		public delegate VKResult PFN_vkCreateAccelerationStructureKHR([NativeType("VkDevice")] IntPtr device, in VKAccelerationStructureCreateInfoKHR createInfo, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator, [NativeType("VkAccelerationStructureKHR")] out ulong accelerationStructure);
		public delegate void PFN_vkDestroyAccelerationStructureKHR([NativeType("VkDevice")] IntPtr device, [NativeType("VkAccelerationStructureKHR")] ulong accelerationStructure, [NativeType("const VkAllocationCallbacks*")] IntPtr allocator);
		public delegate void PFN_vkGetAccelerationStructureBuildSizesKHR([NativeType("VkDevice")] IntPtr device, VKAccelerationStructureBuildTypeKHR buildType, in VKAccelerationStructureBuildGeometryInfoKHR buildInfo, [NativeType("const uint32_t*")] IntPtr pMaxPrimitiveCounts, ref VkAccelerationStructureBuildSizesInfoKHR sizeInfo);
		[return: NativeType("VkDeviceAddress")]
		public delegate ulong PFN_vkGetAccelerationStructureDeviceAddressKHR([NativeType("VkDevice")] IntPtr device, in VKAccelerationStructureDeviceAddressInfoKHR info);
		public delegate void PFN_vkGetDeviceAccelerationStructureCompatibilityKHR([NativeType("VkDevice")] IntPtr device, in VKAccelerationStructureVersionInfoKHR versionInfo, ref VKAccelerationStructureCompatibilityKHR compatibility);
		public delegate VKResult PFN_vkWriteAccelerationStructuresPropertiesKHR([NativeType("VkDevice")] IntPtr device, uint accelerationStructureCount, [NativeType("const VkAccelerationStructureKHR*")] IntPtr pAccelerationStructures, VKQueryType queryType, UIntPtr dataSize, IntPtr pData, nuint stride);

		public PFN_vkBuildAccelerationStructuresKHR vkBuildAccelerationStructuresKHR;
		public PFN_vkCmdBuildAccelerationStructuresIndirectKHR vkCmdBuildAccelerationStructuresIndirectKHR;
		public PFN_vkCmdBuildAccelerationStructuresKHR vkCmdBuildAccelerationStructuresKHR;
		public PFN_vkCmdCopyAccelerationStructureKHR vkCmdCopyAccelerationStructureKHR;
		public PFN_vkCmdCopyAccelerationStructureToMemoryKHR vkCmdCopyAccelerationStructureToMemoryKHR;
		public PFN_vkCmdCopyMemoryToAccelerationStructureKHR vkCmdCopyMemoryToAccelerationStructureKHR;
		public PFN_vkCmdWriteAccelerationStructuresPropertiesKHR vkCmdWriteAccelerationStructuresPropertiesKHR;
		public PFN_vkCopyAccelerationStructureKHR vkCopyAccelerationStructureKHR;
		public PFN_vkCopyAccelerationStructureToMemoryKHR vkCopyAccelerationStructureToMemoryKHR;
		public PFN_vkCopyMemoryToAccelerationStructureKHR vkCopyMemoryToAccelerationStructureKHR;
		public PFN_vkCreateAccelerationStructureKHR vkCreateAccelerationStructureKHR;
		public PFN_vkDestroyAccelerationStructureKHR vkDestroyAccelerationStructureKHR;
		public PFN_vkGetAccelerationStructureBuildSizesKHR vkGetAccelerationStructureBuildSizesKHR;
		public PFN_vkGetAccelerationStructureDeviceAddressKHR vkGetAccelerationStructureDeviceAddressKHR;
		public PFN_vkGetDeviceAccelerationStructureCompatibilityKHR vkGetDeviceAccelerationStructureCompatibilityKHR;
		public PFN_vkWriteAccelerationStructuresPropertiesKHR vkWriteAccelerationStructuresPropertiesKHR;

	}
#nullable restore

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
