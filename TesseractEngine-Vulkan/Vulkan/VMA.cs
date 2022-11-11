using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Vulkan.Native;

namespace Tesseract.Vulkan {

	public delegate void VMAAllocateDeviceMemoryFunction([NativeType("VmaAllocator")] IntPtr allocator, uint memoryType, [NativeType("VkDeviceMemory")] ulong memory, [NativeType("VkDeviceSize")] ulong size, IntPtr userData);
	public delegate void VMAFreeDeviceMemoryFunction([NativeType("VmaAllocator")] IntPtr allocator, uint memoryType, [NativeType("VkDeviceMemory")] ulong memory, [NativeType("VkDeviceSize")] ulong size, IntPtr userData);

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADeviceMemoryCallbacks {

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VMAAllocateDeviceMemoryFunction Allocate;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VMAFreeDeviceMemoryFunction Free;
		public IntPtr UserData;

	}

	public enum VMAAllocatorCreateFlagBits : int {
		ExternallySynchronized = 0x00000001,
		KHRDedicatedAllocation = 0x00000002,
		KHRBindMemory2 = 0x00000004,
		EXTMemoryBudget = 0x00000008,
		AMDDeviceCoherentMemory = 0x00000010,
		BufferDeviceAddress = 0x00000020,
		EXTMemoryPriority = 0x00000040
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAVulkanFunctions {

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10InstanceFunctions.PFN_vkGetPhysicalDeviceProperties vkGetPhysicalDeviceProperties;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10InstanceFunctions.PFN_vkGetPhysicalDeviceMemoryProperties vkGetPhysicalDeviceMemoryProperties;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkAllocateMemory vkAllocateMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkFreeMemory vkFreeMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkMapMemory vkMapMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkUnmapMemory vkUnmapMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkFlushMappedMemoryRanges vkFlushMappedMemoryRanges;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkInvalidateMappedMemoryRanges vkInvalidateMappedMemoryRanges;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkBindBufferMemory vkBindBufferMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkBindImageMemory vkBindImageMemory;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkGetBufferMemoryRequirements vkGetBufferMemoryRequirements;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkGetImageMemoryRequirements vkGetImageMemoryRequirements;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkCreateBuffer vkCreateBuffer;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkDestroyBuffer vkDestroyBuffer;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkCreateImage vkCreateImage;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkDestroyImage vkDestroyImage;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK10DeviceFunctions.PFN_vkCmdCopyBuffer vkCmdCopyBuffer;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK11DeviceFunctions.PFN_vkGetBufferMemoryRequirements2 vkGetBufferMemoryRequirements2;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK11DeviceFunctions.PFN_vkGetImageMemoryRequirements2 vkGetImageMemoryRequirements2;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK11DeviceFunctions.PFN_vkBindBufferMemory2 vkBindBufferMemory2;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK11DeviceFunctions.PFN_vkBindImageMemory2 vkBindImageMemory2;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VK11InstanceFunctions.PFN_vkGetPhysicalDeviceMemoryProperties2 vkGetPhysicalDeviceMemoryProperties2;

	}

	public enum VMARecordFlagBits : int {
		FlushAfterCall = 0x00000001
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMARecordSettings {

		public VMARecordFlagBits Flags;

		[MarshalAs(UnmanagedType.LPStr)]
		public string FilePath;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VMAAllocatorCreateInfo {

		private readonly VMAAllocatorCreateFlagBits flags;
		public VMAAllocatorCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly IntPtr physicalDevice;
		[NativeType("VkPhysicalDevice")]
		public IntPtr PhysicalDevice { get => physicalDevice; init => physicalDevice = value; }
		private readonly IntPtr device;
		[NativeType("VkDevice")]
		public IntPtr Device { get => device; init => device = value; }
		private readonly ulong preferredLargeHeapBlockSize;
		public ulong PreferredLargeHeapBlockSize { get => preferredLargeHeapBlockSize; init => preferredLargeHeapBlockSize = value; }
		private readonly IntPtr allocationCallbacks;
		[NativeType("const VkAllocationCallbacks*")]
		public IntPtr AllocationCallbacks { get => allocationCallbacks; init => allocationCallbacks = value; }
		private readonly IntPtr deviceMemoryCallbacks;
		[NativeType("const VmaDeviceMemoryCallbacks*")]
		public IntPtr DeviceMemoryCallbacks { get => deviceMemoryCallbacks; init => deviceMemoryCallbacks = value; }
		private readonly uint frameInUseCount;
		public uint FrameInUseCount { get => frameInUseCount; init => frameInUseCount = value; }
		private readonly IntPtr heapSizeLimit;
		[NativeType("const VkDeviceSize*")]
		public IntPtr HeapSizeLimit { get => heapSizeLimit; init => heapSizeLimit = value; }
		private readonly IntPtr vulkanFunctions;
		[NativeType("const VmaVulkanFunctions*")]
		public IntPtr VulkanFunctions { get => vulkanFunctions; init => vulkanFunctions = value; }
		private readonly IntPtr recordSettings;
		[NativeType("const VmaRecordSettings*")]
		public IntPtr RecordSettings { get => recordSettings; init => recordSettings = value; }
		private readonly IntPtr instance;
		[NativeType("VkInstance")]
		public IntPtr Instance { get => instance; init => instance = value; }
		private readonly uint vulkanApiVerison;
		public uint VulkanApiVersion { get => vulkanApiVerison; init => vulkanApiVerison = value; }
		private readonly IntPtr typeExternalMemoryHandleTypes;
		[NativeType("const VkExternalMemoryHandleTypeFlagBitsKHR*")]
		public IntPtr TypeExternalMemoryHandleTypes { get => typeExternalMemoryHandleTypes; init => typeExternalMemoryHandleTypes = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAAllocatorInfo {

		[NativeType("VkInstance")]
		public IntPtr Instance;

		[NativeType("VkPhysicalDevice")]
		public IntPtr PhysicalDevice;

		[NativeType("VkDevice")]
		public IntPtr Device;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAStatInfo {

		public uint BlockCount;

		public uint AllocationCount;

		public uint UnusedRangeCount;

		[NativeType("VkDeviceSize")]
		public ulong UsedBytes;

		[NativeType("VkDeviceSize")]
		public ulong UnusedBytes;

		[NativeType("VkDeviceSize")]
		public ulong AllocationSizeMin;
		[NativeType("VkDeviceSize")]
		public ulong AllocationSizeAvg;
		[NativeType("VkDeviceSize")]
		public ulong AllocationSizeMax;

		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeMin;
		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeAvg;
		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeMax;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAStats {

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryTypes)]
		public VMAStatInfo[] MemoryType;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryHeaps)]
		public VMAStatInfo[] MemoryHeap;
		public VMAStatInfo Total;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMABudget {

		[NativeType("VkDeviceSize")]
		public ulong BlockBytes;

		[NativeType("VkDeviceSize")]
		public ulong AllocationBytes;

		[NativeType("VkDeviceSize")]
		public ulong Usage;

		[NativeType("VkDeviceSize")]
		public ulong Budget;

	}

	public enum VMAMemoryUsage : int {
		Unknown = 0,
		GPUOnly = 1,
		CPUOnly = 2,
		CPUToGPU = 3,
		GPUToCPU = 4,
		CPUCopy = 5,
		GPULazilyAllocated = 6
	}

	public enum VMAAllocationCreateFlagBits : int {
		DedicatedMemory = 0x00000001,
		NeverAllocate = 0x00000002,
		Mapped = 0x00000004,
		CanBecomeLost = 0x00000008,
		CanMakeOtherLost = 0x00000010,
		UserDataCopyString = 0x00000020,
		UpperAddress = 0x00000040,
		DontBind = 0x00000080,
		WithinBudget = 0x00000100,
		StrategyBestFit = 0x00010000,
		StrategyWorstFit = 0x00020000,
		StrategyFirstFit = 0x00040000,
		StrategyMinMemory = StrategyBestFit,
		StrategyMinTime = StrategyFirstFit,
		StrategyMinFragmentation = StrategyWorstFit
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VMAAllocationCreateInfo {

		private readonly VMAAllocationCreateFlagBits flags;
		public VMAAllocationCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly VMAMemoryUsage usage;
		public VMAMemoryUsage Usage { get => usage; init => usage = value; }
		private readonly VKMemoryPropertyFlagBits requiredFlags;
		public VKMemoryPropertyFlagBits RequiredFlags { get => requiredFlags; init => requiredFlags = value; }
		private readonly VKMemoryPropertyFlagBits preferredFlags;
		public VKMemoryPropertyFlagBits PreferredFlags { get => preferredFlags; init => preferredFlags = value; }
		private readonly uint memoryTypeBits;
		public uint MemoryTypeBits { get => memoryTypeBits; init => memoryTypeBits = value; }
		private readonly IntPtr pool;
		[NativeType("VmaPool")]
		public IntPtr Pool { get => pool; init => pool = value; }
		private readonly IntPtr userData;
		public IntPtr UserData { get => userData; init => userData = value; }
		private readonly float priority;
		public float Priority { get => priority; init => priority = value; }

	}

	public enum VMAPoolCreateFlagBits : int {
		IgnoreBufferImageGranularity = 0x00000002,
		LinearAlgorithm = 0x00000004,
		BuddyAlgorithm = 0x00000008
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VMAPoolCreateInfo {

		private readonly uint memoryTypeIndex;
		public uint MemoryTypeIndex { get => memoryTypeIndex; init => memoryTypeIndex = value; }
		private readonly VMAPoolCreateFlagBits flags;
		public VMAPoolCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly ulong blockSize;
		[NativeType("VkDeviceSize")]
		public ulong BlockSize { get => blockSize; init => blockSize = value; }
		private readonly nuint minBlockCount;
		public nuint MinBlockCount { get => minBlockCount; init => minBlockCount = value; }
		private readonly nuint maxBlockCount;
		public nuint MaxBlockCount { get => maxBlockCount; init => maxBlockCount = value; }
		private readonly uint frameInUseCount;
		public uint FrameInUseCount { get => frameInUseCount; init => frameInUseCount = value; }
		private readonly float priority;
		public float Priority { get => priority; init => priority = value; }
		private readonly ulong minAllocationAlignment;
		[NativeType("VkDeviceSize")]
		public ulong MinAllocationAlignment { get => minAllocationAlignment; init => minAllocationAlignment = value; }
		private readonly IntPtr memoryAllocateNext;
		public IntPtr MemoryAllocateNext { get => memoryAllocateNext; init => memoryAllocateNext = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAPoolStats {

		[NativeType("VkDeviceSize")]
		public ulong Size;

		[NativeType("VkDeviceSize")]
		public ulong UnusedSize;

		public nuint AllocationCount;

		public nuint UnusedRangeCount;

		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeMax;

		public nuint BlockCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAAllocationInfo {

		public uint MemoryType;

		[NativeType("VkDeviceMemory")]
		public ulong DeviceMemory;

		[NativeType("VkDeviceSize")]
		public ulong Offset;

		[NativeType("VkDeviceSize")]
		public ulong Size;

		public IntPtr MappedData;

		public IntPtr UserData;

	}

	public enum VMADefragmentationFlagBits : int {
		Incremental = 0x00000001
	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VMADefragmentationInfo2 {

		private readonly VMADefragmentationFlagBits flags;
		public VMADefragmentationFlagBits Flags { get => flags; init => flags = value; }
		private readonly uint allocationCount;
		public uint AllocationCount { get => allocationCount; init => allocationCount = value; }
		private readonly IntPtr allocations;
		[NativeType("const VmaAllocation*")]
		public IntPtr Allocations { get => allocations; init => allocations = value; }
		private readonly IntPtr allocationsChanged;
		[NativeType("VkBool32*")]
		public IntPtr AllocationsChanged { get => allocationsChanged; init => allocationsChanged = value; }
		private readonly uint poolCount;
		public uint PoolCount { get => poolCount; init => poolCount = value; }
		private readonly IntPtr pools;
		[NativeType("const VmaPool*")]
		public IntPtr Pools { get => pools; init => pools = value; }
		private readonly ulong maxCpuBytesToMove;
		public ulong MaxCpuBytesToMove { get => maxCpuBytesToMove; init => maxCpuBytesToMove = value; }
		private readonly uint maxCpuAllocationsToMove;
		public uint MaxCpuAllocationsToMove { get => maxCpuAllocationsToMove; init => maxCpuAllocationsToMove = value; }
		private readonly ulong maxGpuBytesToMove;
		public ulong MaxGpuBytesToMove { get => maxGpuBytesToMove; init => maxGpuBytesToMove = value; }
		private readonly uint maxGpuAllocationsToMove;
		public uint MaxGpuAllocationsToMove { get => maxGpuAllocationsToMove; init => maxGpuAllocationsToMove = value; }
		private readonly IntPtr commandBuffer;
		[NativeType("VkCommandBuffer")]
		public IntPtr CommandBuffer { get => commandBuffer; init => commandBuffer = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationPassMoveInfo {
		
		[NativeType("VmaAllocation")]
		public IntPtr Allocation;
		
		[NativeType("VkDeviceMemory")]
		public ulong Memory;

		[NativeType("VkDeviceSize")]
		public ulong Offset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationPassInfo {

		public uint MoveCount;

		[NativeType("VmaDefragmentationPassMoveInfo*")]
		public IntPtr Moves;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationInfo {

		[NativeType("VkDeviceSize")]
		public ulong MaxBytesToMove;

		public uint MaxAllocationsToMove;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationStats {

		[NativeType("VkDeviceSize")]
		public ulong BytesMoved;

		[NativeType("VkDeviceSize")]
		public ulong BytesFreed;

		public uint AllocationsMoved;

		public uint DeviceMemoryBlocksFreed;

	}

#nullable disable
	public class VMAFunctions {

		public delegate VKResult PFN_vmaCreateAllocator(in VMAAllocatorCreateInfo createInfo, [NativeType("VmaAllocator*")] out IntPtr allocator);
		public delegate void PFN_vmaDestroyAllocator([NativeType("VmaAllocator")] IntPtr allocator);

		public PFN_vmaCreateAllocator vmaCreateAllocator;
		public PFN_vmaDestroyAllocator vmaDestroyAllocator;

		public delegate void PFN_vmaGetAllocatorInfo([NativeType("VmaAllocator")] IntPtr allocator, out VMAAllocatorInfo info);
		public delegate void PFN_vmaGetPhysicalDeviceProperties([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("const VkPhysicalDeviceProperties**")] out IntPtr properties);
		public delegate void PFN_vmaGetMemoryProperties([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("const VkPhysicalDeviceMemoryProperties**")] out IntPtr properties);
		public delegate void PFN_vmaGetMemoryTypeProperties([NativeType("VmaAllocator")] IntPtr allocator, uint memoryTypeIndex, out VKMemoryPropertyFlagBits flags);
		public delegate void PFN_vmaSetCurrentFrameIndex([NativeType("VmaAllocator")] IntPtr allocator, uint frameIndex);

		public PFN_vmaGetAllocatorInfo vmaGetAllocatorInfo;
		public PFN_vmaGetPhysicalDeviceProperties vmaGetPhysicalDeviceProperties;
		public PFN_vmaGetMemoryProperties vmaGetMemoryProperties;
		public PFN_vmaGetMemoryTypeProperties vmaGetMemoryTypeProperties;
		public PFN_vmaSetCurrentFrameIndex vmaSetCurrentFrameIndex;

		public delegate void PFN_vmaCalculateStats([NativeType("VmaAllocator")] IntPtr allocator, out VMAStats stats);

		public PFN_vmaCalculateStats vmaCalculateStats;

		public delegate void PFN_vmaGetBudget([NativeType("VmaAllocator")] IntPtr allocator, out VMABudget budget);

		public PFN_vmaGetBudget vmaGetBudget;

		public delegate void PFN_vmaBuildStatsString([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("char**")] out IntPtr statsString, bool detailedMap);
		public delegate void PFN_vmaFreeStatsString([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("char*")] IntPtr statsString);

		public PFN_vmaBuildStatsString vmaBuildStatsString;
		public PFN_vmaFreeStatsString vmaFreeStatsString;

		public delegate VKResult PFN_vmaFindMemoryTypeIndex([NativeType("VmaAllocator")] IntPtr allocator, uint memoryTypeBits, in VMAAllocationCreateInfo createInfo, out uint memoryTypeIndex);
		public delegate VKResult PFN_vmaFindMemoryTypeIndexForBufferInfo([NativeType("VmaAllocator")] IntPtr allocator, in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out uint memoryTypeIndex);
		public delegate VKResult PFN_vmaFindMemoryTypeIndexForImageInfo([NativeType("VmaAllocator")] IntPtr allocator, in VKImageCreateInfo imageCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out uint memoryTypeIndex);

		public PFN_vmaFindMemoryTypeIndex vmaFindMemoryTypeIndex;
		public PFN_vmaFindMemoryTypeIndexForBufferInfo vmaFindMemoryTypeIndexForBufferInfo;
		public PFN_vmaFindMemoryTypeIndexForImageInfo vmaFindMemoryTypeIndexForImageInfo;

		public delegate VKResult PFN_vmaCreatePool([NativeType("VmaAllocator")] IntPtr allocator, in VMAPoolCreateInfo createInfo, [NativeType("VmaPool*")] out IntPtr pool);
		public delegate void PFN_vmaDestroyPool([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool);
		public delegate void PFN_vmaGetPoolStats([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool, out VMAPoolStats poolStats);
		public delegate void PFN_vmaMakePoolAllocationsLost([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool, out nuint lostAllocationCount);
		public delegate VKResult PFN_vmaCheckPoolCorruption([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool);
		public delegate void PFN_vmaGetPoolName([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool, [NativeType("const char**")] out IntPtr name);
		public delegate void PFN_vmaSetPoolName([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaPool")] IntPtr pool, [NativeType("const char*")] IntPtr name);

		public PFN_vmaCreatePool vmaCreatePool;
		public PFN_vmaDestroyPool vmaDestroyPool;
		public PFN_vmaGetPoolStats vmaGetPoolStats;
		public PFN_vmaMakePoolAllocationsLost vmaMakePoolAllocationsLost;
		public PFN_vmaCheckPoolCorruption vmaCheckPoolCorruption;
		public PFN_vmaGetPoolName vmaGetPoolName;
		public PFN_vmaSetPoolName vmaSetPoolName;

		public delegate VKResult PFN_vmaAllocateMemory([NativeType("VmaAllocator")] IntPtr allocator, in VKMemoryRequirements memoryRequirements, in VMAAllocationCreateInfo createInfo, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate VKResult PFN_vmaAllocateMemoryPages([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("const VkMemoryRequirements*")] IntPtr memoryRequirements, [NativeType("const VmaAllocationCreateInfo*")] IntPtr createInfo, nuint allocationCount, [NativeType("VmaAllocation*")] IntPtr allocation, [NativeType("VmaAllocationInfo*")] IntPtr allocationInfo);
		public delegate VKResult PFN_vmaAllocateMemoryForBuffer([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VkBuffer")] ulong buffer, in VMAAllocationCreateInfo createInfo, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate VKResult PFN_vmaAllocateMemoryForImage([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VkImage")] ulong buffer, in VMAAllocationCreateInfo createInfo, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate void PFN_vmaFreeMemory([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation);
		public delegate void PFN_vmaFreeMemoryPages([NativeType("VmaAllocator")] IntPtr allocator, nuint allocationCount, [NativeType("const VmaAllocation*")] IntPtr allocation);
		public delegate void PFN_vmaGetAllocationInfo([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate bool PFN_vmaTouchAllocation([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation);
		public delegate void PFN_vmaSetAllocationUserData([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, IntPtr userData);
		public delegate void PFN_vmaCreateLostAllocation([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation*")] out IntPtr allocation);
		public delegate VKResult PFN_vmaMapMemory([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, out IntPtr data);
		public delegate void PFN_vmaUnmapMemory([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation);
		public delegate VKResult PFN_vmaFlushAllocation([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, ulong offset, ulong size);
		public delegate VKResult PFN_vmaInvalidateAllocation([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, ulong offset, ulong size);
		public delegate VKResult PFN_vmaFlushAllocations([NativeType("VmaAllocator")] IntPtr allocator, uint allocationCount, [NativeType("const VmaAllocation")] IntPtr allocations, [NativeType("const VkDeviceSize")] IntPtr offsets, [NativeType("const VkDeviceSize")] IntPtr sizes);
		public delegate VKResult PFN_vmaInvalidateAllocations([NativeType("VmaAllocator")] IntPtr allocator, uint allocationCount, [NativeType("const VmaAllocation")] IntPtr allocations, [NativeType("const VkDeviceSize")] IntPtr offsets, [NativeType("const VkDeviceSize")] IntPtr sizes);
		public delegate VKResult PFN_vmaCheckCorruption([NativeType("VmaAllocator")] IntPtr allocator, uint memoryTypeBits);

		public PFN_vmaAllocateMemory vmaAllocateMemory;
		public PFN_vmaAllocateMemoryPages vmaAllocateMemoryPages;
		public PFN_vmaAllocateMemoryForBuffer vmaAllocateMemoryForBuffer;
		public PFN_vmaAllocateMemoryForImage vmaAllocateMemoryForImage;
		public PFN_vmaFreeMemory vmaFreeMemory;
		public PFN_vmaFreeMemoryPages vmaFreeMemoryPages;
		public PFN_vmaGetAllocationInfo vmaGetAllocationInfo;
		public PFN_vmaTouchAllocation vmaTouchAllocation;
		public PFN_vmaSetAllocationUserData vmaSetAllocationUserData;
		public PFN_vmaCreateLostAllocation vmaCreateLostAllocation;
		public PFN_vmaMapMemory vmaMapMemory;
		public PFN_vmaUnmapMemory vmaUnmapMemory;
		public PFN_vmaFlushAllocation vmaFlushAllocation;
		public PFN_vmaInvalidateAllocation vmaInvalidateAllocation;
		public PFN_vmaFlushAllocations vmaFlushAllocations;
		public PFN_vmaInvalidateAllocations vmaInvalidateAllocations;
		public PFN_vmaCheckCorruption vmaCheckCorruption;

		public delegate VKResult PFN_vmaDefragmentationBegin([NativeType("VmaAllocator")] IntPtr allocator, in VMADefragmentationInfo2 info, [NativeType("VmaDefragmentationStats*")] IntPtr stats, [NativeType("VmaDefragmentationContext*")] out IntPtr context);
		public delegate VKResult PFN_vmaDefragmentationEnd([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaDefragmentationContext")] IntPtr context);
		public delegate VKResult PFN_vmaBeginDefragmentationPass([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaDefragmentationContext")] IntPtr context, [NativeType("VmaDefragmentationPassInfo*")] IntPtr info);
		public delegate VKResult PFN_vmaEndDefragmentationPass([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaDefragmentationContext")] IntPtr context);
		public delegate VKResult PFN_vmaDefragment([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("const VmaAllocation*")] IntPtr allocations, uint allocationCount, [NativeType("VkBool32*")] IntPtr allocationsChanged, in VMADefragmentationInfo info, out VMADefragmentationStats stats);
		public delegate VKResult PFN_vmaBindBufferMemory([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, [NativeType("VkBuffer")] ulong buffer);
		public delegate VKResult PFN_vmaBindBufferMemory2([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, ulong allocationLocalOffset, [NativeType("VkBuffer")] ulong buffer, IntPtr next);
		public delegate VKResult PFN_vmaBindImageMemory([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, [NativeType("VkImage")] ulong image);
		public delegate VKResult PFN_vmaBindImageMemory2([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VmaAllocation")] IntPtr allocation, ulong allocationLocalOffset, [NativeType("VkImage")] ulong image, IntPtr next);
		public delegate VKResult PFN_vmaCreateBuffer([NativeType("VmaAllocator")] IntPtr allocator, in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, [NativeType("VkBuffer*")] out ulong buffer, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate VKResult PFN_vmaCreateBufferWithAlignment([NativeType("VmaAllocator")] IntPtr allocator, in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, ulong minAlignment, [NativeType("VkBuffer*")] out ulong buffer, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate void PFN_vmaDestroyBuffer([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VkBuffer")] ulong buffer, [NativeType("VmaAllocation")] IntPtr allocation);
		public delegate VKResult PFN_vmaCreateImage([NativeType("VmaAllocator")] IntPtr allocator, in VKImageCreateInfo imageCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, [NativeType("VkImage*")] out ulong image, [NativeType("VmaAllocation*")] out IntPtr allocation, out VMAAllocationInfo allocationInfo);
		public delegate void PFN_vmaDestroyImage([NativeType("VmaAllocator")] IntPtr allocator, [NativeType("VkImage")] ulong image, [NativeType("VmaAllocation")] IntPtr allocation);

		public PFN_vmaDefragmentationBegin vmaDefragmentationBegin;
		public PFN_vmaDefragmentationEnd vmaDefragmentationEnd;
		public PFN_vmaBeginDefragmentationPass vmaBeginDefragmentationPass;
		public PFN_vmaEndDefragmentationPass vmaEndDefragmentationPass;
		public PFN_vmaDefragment vmaDefragment;
		public PFN_vmaBindBufferMemory vmaBindBufferMemory;
		public PFN_vmaBindBufferMemory2 vmaBindBufferMemory2;
		public PFN_vmaBindImageMemory vmaBindImageMemory;
		public PFN_vmaBindImageMemory2 vmaBindImageMemory2;
		public PFN_vmaCreateBuffer vmaCreateBuffer;
		public PFN_vmaCreateBufferWithAlignment vmaCreateBufferWithAlignment;
		public PFN_vmaDestroyBuffer vmaDestroyBuffer;
		public PFN_vmaCreateImage vmaCreateImage;
		public PFN_vmaDestroyImage vmaDestroyImage;

	}
#nullable restore

	public static class VMA {

		public static readonly LibrarySpec LibrarySpec = new() { Name = "VulkanMemoryAllocator" };

		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);

		public static VMAFunctions Functions { get; } = new();

		static VMA() {
			Library.LoadFunctions(Functions);
		}

		public static VMAAllocator CreateAllocator(in VMAAllocatorCreateInfo createInfo, VKDevice device) {
			VK.CheckError(Functions.vmaCreateAllocator(createInfo, out IntPtr allocator), "Failed to create VMA allocator");
			if (createInfo.Device != device) throw new ArgumentException("Supplied device does not match that of the creation information", nameof(device));
			return new VMAAllocator(allocator, device, createInfo.AllocationCallbacks);
		}

	}

	public class VMAAllocator : IDisposable, IVKAllocatedObject {

		[NativeType("VmaAllocator")]
		public IntPtr Allocator;

		public VKDevice Device { get; }

		public VulkanAllocationCallbacks? AllocationCallbacks { get; }

		VulkanAllocationCallbacks? IVKAllocatedObject.Allocator => AllocationCallbacks;

		public VMAAllocator(IntPtr allocator, VKDevice device, VulkanAllocationCallbacks? allocationCallbacks) {
			Allocator = allocator;
			Device = device;
			AllocationCallbacks = allocationCallbacks;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			VMA.Functions.vmaDestroyAllocator(Allocator);
		}

		public VMAAllocatorInfo Info {
			get {
				VMA.Functions.vmaGetAllocatorInfo(Allocator, out VMAAllocatorInfo info);
				return info;
			}
		}

		public VKPhysicalDeviceProperties PhysicalDeviceProperties {
			get {
				VMA.Functions.vmaGetPhysicalDeviceProperties(Allocator, out IntPtr properties);
				return Marshal.PtrToStructure<VKPhysicalDeviceProperties>(properties);
			}
		}

		public VKPhysicalDeviceMemoryProperties MemoryProperties {
			get {
				VMA.Functions.vmaGetMemoryProperties(Allocator, out IntPtr properties);
				return Marshal.PtrToStructure<VKPhysicalDeviceMemoryProperties>(properties);
			}
		}

		public VKMemoryPropertyFlagBits GetMemoryTypeProperties(uint memoryTypeIndex) {
			VMA.Functions.vmaGetMemoryTypeProperties(Allocator, memoryTypeIndex, out VKMemoryPropertyFlagBits flags);
			return flags;
		}

		public uint CurrentFrameIndex {
			set => VMA.Functions.vmaSetCurrentFrameIndex(Allocator, value);
		}

		public VMAStats Stats {
			get {
				VMA.Functions.vmaCalculateStats(Allocator, out VMAStats stats);
				return stats;
			}
		}

		public VMABudget Budget {
			get {
				VMA.Functions.vmaGetBudget(Allocator, out VMABudget budget);
				return budget;
			}
		}

		public string BuildStatsString(bool detailedMap) {
			VMA.Functions.vmaBuildStatsString(Allocator, out IntPtr pString, detailedMap);
			string str = MemoryUtil.GetASCII(pString)!;
			VMA.Functions.vmaFreeStatsString(Allocator, pString);
			return str;
		}

		public uint FindMemoryTypeIndex(uint memoryTypeBits, in VMAAllocationCreateInfo createInfo) {
			VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndex(Allocator, memoryTypeBits, createInfo, out uint typeIndex), "Failed to find memory type index");
			return typeIndex;
		}

		public uint FindMemoryTypeIndex(in VKBufferCreateInfo bufferInfo, in VMAAllocationCreateInfo createInfo) {
			VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndexForBufferInfo(Allocator, bufferInfo, createInfo, out uint typeIndex), "Failed to find memory type index for buffer info");
			return typeIndex;
		}

		public uint FindMemoryTypeIndex(in VKImageCreateInfo imageInfo, in VMAAllocationCreateInfo createInfo) {
			VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndexForImageInfo(Allocator, imageInfo, createInfo, out uint typeIndex), "Failed to find memory type index for image info");
			return typeIndex;
		}

		public VMAPool CreatePool(in VMAPoolCreateInfo createInfo) {
			VK.CheckError(VMA.Functions.vmaCreatePool(Allocator, createInfo, out IntPtr pool), "Failed to create VMA pool");
			return new VMAPool(this, pool);
		}

		public VMAAllocation AllocateMemory(in VKMemoryRequirements memoryRequirements, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaAllocateMemory(Allocator, memoryRequirements, createInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory");
			return new VMAAllocation(this, allocation);
		}

		public void AllocateMemoryPages(in ReadOnlySpan<VKMemoryRequirements> memoryRequirements, in ReadOnlySpan<VMAAllocationCreateInfo> createInfo, out VMAAllocation[] allocations, out VMAAllocationInfo[] allocationInfos) {
			int n = Math.Min(memoryRequirements.Length, createInfo.Length);
			Span<IntPtr> allocs = stackalloc IntPtr[n];
			allocationInfos = new VMAAllocationInfo[n];
			unsafe {
				fixed(VKMemoryRequirements* pReqs = memoryRequirements) {
					fixed(VMAAllocationCreateInfo* pCreateInfos = createInfo) {
						fixed(IntPtr* pAllocs = allocs) {
							fixed(VMAAllocationInfo* pInfos = allocationInfos) {
								VK.CheckError(VMA.Functions.vmaAllocateMemoryPages(Allocator, (IntPtr)pReqs, (IntPtr)pCreateInfos, (nuint)n, (IntPtr)pAllocs, (IntPtr)pInfos), "Failed to allocate memory pages");
							}
						}
					}
				}
			}
			allocations = new VMAAllocation[n];
			for (int i = 0; i < n; i++) allocations[i] = new VMAAllocation(this, allocs[i]);
		}

		public VMAAllocation AllocateMemoryForBuffer(VKBuffer buffer, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaAllocateMemoryForBuffer(Allocator, buffer, createInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory for buffer");
			return new VMAAllocation(this, allocation);
		}

		public VMAAllocation AllocateMemoryForImage(VKImage image, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaAllocateMemoryForImage(Allocator, image, createInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory for image");
			return new VMAAllocation(this, allocation);
		}

		public void FreeMemoryPages(in ReadOnlySpan<VMAAllocation> allocations) {
			Span<IntPtr> allocs = stackalloc IntPtr[allocations.Length];
			for (int i = 0; i < allocations.Length; i++) allocs[i] = allocations[i];
			unsafe {
				fixed(IntPtr* pAllocs = allocs) {
					VMA.Functions.vmaFreeMemoryPages(Allocator, (nuint)allocs.Length, (IntPtr)pAllocs);
				}
			}
			for (int i = 0; i < allocations.Length; i++) allocations[i].Allocation = IntPtr.Zero;
		}

		public VMAAllocation CreateLostAllocation() {
			VMA.Functions.vmaCreateLostAllocation(Allocator, out IntPtr allocation);
			return new VMAAllocation(this, allocation);
		}

		public void FlushAllocations(in ReadOnlySpan<VMAAllocation> allocations, in ReadOnlySpan<ulong> offsets, in ReadOnlySpan<ulong> sizes) {
			int n = ExMath.Min(allocations.Length, offsets.Length, sizes.Length);
			Span<IntPtr> allocs = stackalloc IntPtr[n];
			for (int i = 0; i < n; i++) allocs[i] = allocations[i];
			unsafe {
				fixed(IntPtr* pAllocs = allocs) {
					fixed(ulong* pOffsets = offsets) {
						fixed(ulong* pSizes = sizes) {
							VK.CheckError(VMA.Functions.vmaFlushAllocations(Allocator, (uint)n, (IntPtr)pAllocs, (IntPtr)pOffsets, (IntPtr)pSizes), "Failed to flush VMA allocations");
						}
					}
				}
			}
		}

		public void InvalidateAllocations(in ReadOnlySpan<VMAAllocation> allocations, in ReadOnlySpan<ulong> offsets, in ReadOnlySpan<ulong> sizes) {
			int n = ExMath.Min(allocations.Length, offsets.Length, sizes.Length);
			Span<IntPtr> allocs = stackalloc IntPtr[n];
			for (int i = 0; i < n; i++) allocs[i] = allocations[i];
			unsafe {
				fixed (IntPtr* pAllocs = allocs) {
					fixed (ulong* pOffsets = offsets) {
						fixed (ulong* pSizes = sizes) {
							VK.CheckError(VMA.Functions.vmaInvalidateAllocations(Allocator, (uint)n, (IntPtr)pAllocs, (IntPtr)pOffsets, (IntPtr)pSizes), "Failed to invalidate VMA allocations");
						}
					}
				}
			}
		}

		public void CheckCorruption(uint memoryTypeBits) => VK.CheckError(VMA.Functions.vmaCheckCorruption(Allocator, memoryTypeBits), "Failure while checking memory corruption");

		public VMADefragmentationContext BeginDefragmentation(in VMADefragmentationInfo2 info, IPointer<VMADefragmentationStats>? stats = null) {
			VK.CheckError(VMA.Functions.vmaDefragmentationBegin(Allocator, info, stats != null ? stats.Ptr : IntPtr.Zero, out IntPtr context), "Failed to begin defragmentation");
			return new VMADefragmentationContext(this, context);
		}

		public void Defragment(in ReadOnlySpan<VMAAllocation> allocations, out bool[] allocationsChanged, in VMADefragmentationInfo info, out VMADefragmentationStats stats) {
			Span<IntPtr> allocs = stackalloc IntPtr[allocations.Length];
			for (int i = 0; i < allocs.Length; i++) allocs[i] = allocations[i];
			allocationsChanged = new bool[allocations.Length];
			unsafe {
				fixed(IntPtr* pAllocs = allocs) {
					fixed(bool* pAllocsChanged = allocationsChanged) {
						VK.CheckError(VMA.Functions.vmaDefragment(Allocator, (IntPtr)pAllocs, (uint)allocs.Length, (IntPtr)pAllocsChanged, info, out stats), "Failed to defragment memory");
					}
				}
			}
		}

		public VKBuffer CreateBuffer(in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaCreateBuffer(Allocator, bufferCreateInfo, allocationCreateInfo, out ulong buffer, out IntPtr alloc, out allocationInfo), "Failed to create buffer");
			allocation = new VMAAllocation(this, alloc);
			return new VKBuffer(Device, buffer, AllocationCallbacks);
		}

		public VKBuffer CreateBufferWithAlignment(in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, ulong minAlignment, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaCreateBufferWithAlignment(Allocator, bufferCreateInfo, allocationCreateInfo, minAlignment, out ulong buffer, out IntPtr alloc, out allocationInfo), "Failed to create buffer");
			allocation = new VMAAllocation(this, alloc);
			return new VKBuffer(Device, buffer, AllocationCallbacks);
		}

		public VKImage CreateImage(in VKImageCreateInfo imageCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			VK.CheckError(VMA.Functions.vmaCreateImage(Allocator, imageCreateInfo, allocationCreateInfo, out ulong image, out IntPtr alloc, out allocationInfo), "Failed to create image");
			allocation = new VMAAllocation(this, alloc);
			return new VKImage(Device, image, AllocationCallbacks);
		}

		public static implicit operator IntPtr(VMAAllocator allocator) => allocator != null ? allocator.Allocator : IntPtr.Zero;

	}

	public class VMAPool : IDisposable {

		public readonly VMAAllocator Allocator;

		[NativeType("VmaPool")]
		public readonly IntPtr Pool;

		public VMAPool(VMAAllocator allocator, IntPtr pool) {
			Allocator = allocator;
			Pool = pool;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			VMA.Functions.vmaDestroyPool(Allocator, Pool);
		}

		public VMAPoolStats Stats {
			get {
				VMA.Functions.vmaGetPoolStats(Allocator, Pool, out VMAPoolStats stats);
				return stats;
			}
		}

		public nuint MakePoolAllocationsLost() {
			VMA.Functions.vmaMakePoolAllocationsLost(Allocator, Pool, out nuint lostAllocationCount);
			return lostAllocationCount;
		}

		public void CheckPoolCorruption() => VK.CheckError(VMA.Functions.vmaCheckPoolCorruption(Allocator, Pool), "Failure during pool corruption check");

		public string? Name {
			get {
				VMA.Functions.vmaGetPoolName(Allocator, Pool, out IntPtr name);
				return MemoryUtil.GetUTF8(name);
			}
			set {
				if (value == null) throw new ArgumentNullException(nameof(value));
				using var pName = MemoryUtil.AllocUTF8(value);
				VMA.Functions.vmaSetPoolName(Allocator, Pool, pName);
			}
		}

		public static implicit operator IntPtr(VMAPool pool) => pool != null ? pool.Pool : IntPtr.Zero;

	}

	public class VMAAllocation : IDisposable {

		public readonly VMAAllocator Allocator;

		[NativeType("VmaAllocation")]
		public IntPtr Allocation { get; internal set; }

		public VMAAllocation(VMAAllocator allocator, IntPtr allocation) {
			Allocator = allocator;
			Allocation = allocation;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (Allocation != IntPtr.Zero) {
				VMA.Functions.vmaFreeMemory(Allocator, Allocation);
				Allocation = IntPtr.Zero;
			}
		}

		public VMAAllocationInfo Info {
			get {
				VMA.Functions.vmaGetAllocationInfo(Allocator, Allocation, out VMAAllocationInfo info);
				return info;
			}
		}

		public void Touch() => VMA.Functions.vmaTouchAllocation(Allocator, Allocation);

		public IntPtr UserData {
			get => Info.UserData;
			set => VMA.Functions.vmaSetAllocationUserData(Allocator, Allocation, value);
		}

		public IntPtr MapMemory() {
			VK.CheckError(VMA.Functions.vmaMapMemory(Allocator, Allocation, out IntPtr data), "Failed to map VMA allocation");
			return data;
		}

		public void UnmapMemory() => VMA.Functions.vmaUnmapMemory(Allocator, Allocation);

		public void Flush(ulong offset, ulong size) => VK.CheckError(VMA.Functions.vmaFlushAllocation(Allocator, Allocation, offset, size), "Failed to flush VMA allocation memory");

		public void Invalidate(ulong offset, ulong size) => VK.CheckError(VMA.Functions.vmaInvalidateAllocation(Allocator, Allocation, offset, size), "Failed to invalidate VMA allocation memory");

		public void BindBufferMemory(VKBuffer buffer) => VK.CheckError(VMA.Functions.vmaBindBufferMemory(Allocator, Allocation, buffer), "Failed to bind VMA allocation to buffer");

		public void BindBufferMemory2(ulong localOffset, VKBuffer buffer, IntPtr next) => VK.CheckError(VMA.Functions.vmaBindBufferMemory2(Allocator, Allocation, localOffset, buffer, next), "Failed to bind VMA allocation to buffer");

		public void BindImageMemory(VKImage image) => VK.CheckError(VMA.Functions.vmaBindImageMemory(Allocator, Allocation, image), "Failed to bind VMA allocation to image");

		public void BindImageMemory2(ulong localOffset, VKImage image, IntPtr next) => VK.CheckError(VMA.Functions.vmaBindImageMemory2(Allocator, Allocation, localOffset, image, next), "Failed to bind VMA allocation to image");

		public void DestroyBuffer(VKBuffer buffer) => VMA.Functions.vmaDestroyBuffer(Allocator, buffer, Allocation);

		public void DestroyImage(VKImage image) => VMA.Functions.vmaDestroyImage(Allocator, image, Allocation);

		public static implicit operator IntPtr(VMAAllocation allocation) => allocation != null ? allocation.Allocation : IntPtr.Zero;

	}

	public struct VMADefragmentationContext {

		public readonly VMAAllocator Allocator;

		[NativeType("VmaDefragmentationContext")]
		public readonly IntPtr Context;

		public VMADefragmentationContext(VMAAllocator allocator, IntPtr context) {
			Allocator = allocator;
			Context = context;
		}

		public void End() => VK.CheckError(VMA.Functions.vmaDefragmentationEnd(Allocator, Context), "Failure while ending defragmentation");

		public void BeginPass(IPointer<VMADefragmentationPassInfo>? info = null) => VK.CheckError(VMA.Functions.vmaBeginDefragmentationPass(Allocator, Context, info != null ? info.Ptr : IntPtr.Zero), "Failed to begin defragmentation pass");

		public void EndPass() => VK.CheckError(VMA.Functions.vmaEndDefragmentationPass(Allocator, Context), "Failure while ending defragmentation pass");

	}

}
