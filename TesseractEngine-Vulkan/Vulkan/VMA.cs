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

	using VkPhysicalDevice = IntPtr;
	using VkDevice = IntPtr;
	using VkCommandBuffer = IntPtr;
	using VkBuffer = UInt64;
	using VkImage = UInt64;
	using VkDeviceMemory = UInt64;
	using VkDeviceSize = UInt64;

	#region Enumerations

	[Flags]
	public enum VMAAllocatorCreateFlagBits : int {
		ExternallySynchronized = 0x00000001,
		KHRDedicatedAllocation = 0x00000002,
		KHRBindMemory2 = 0x00000004,
		EXTMemoryBudget = 0x00000008,
		AMDDeviceCoherentMemory = 0x00000010,
		BufferDeviceAddress = 0x00000020,
		EXTMemoryPriority = 0x00000040
	}

	public enum VMAMemoryUsage : int {
		Unknown = 0,
		GPUOnly = 1,
		CPUOnly = 2,
		CPUToGPU = 3,
		GPUToCPU = 4,
		CPUCopy = 5,
		GPULazilyAllocated = 6,
		Auto = 7,
		AutoPreferDevice = 8,
		AutoPreferHost = 9
	}

	[Flags]
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
		CanAlias = 0x00000200,
		HostAccessSequentialWrite = 0x00000400,
		HostAccessRandom = 0x00000800,
		HostAccessAllowTransferInstead = 0x00001000,
		StrategyMinMemory = 0x00010000,
		StrategyMinTime = 0x00020000,
		StrategyMinOffset = 0x00040000
	}

	[Flags]
	public enum VMAPoolCreateFlagBits : int {
		IgnoreBufferImageGranularity = 0x00000002,
		LinearAlgorithm = 0x00000004
	}

	[Flags]
	public enum VMADefragmentationFlagBits : int {
		AlgorithmFast = 0x00000001,
		AlgorithmBalanced = 0x00000002,
		AlgorithmFull = 0x00000004,
		AlgorithmExtensive = 0x00000008
	}

	public enum VMADefragmentationMoveOperation : int {
		Copy = 0,
		Ignore = 1,
		Destroy = 2
	}

	[Flags]
	public enum VMAVirtualBlockCreateFlagBits : int {
		LinearAlgorithm = 0x00000001
	}

	[Flags]
	public enum VMAVirtualAllocationCreateFlagBits : int {
		UpperAddress = VMAAllocationCreateFlagBits.UpperAddress,
		StrategyMinMemory = VMAAllocationCreateFlagBits.StrategyMinMemory,
		StrategyMinTime = VMAAllocationCreateFlagBits.StrategyMinTime,
		StrategyMinOffset = VMAAllocationCreateFlagBits.StrategyMinOffset
	}

	#endregion

	public delegate void VMAAllocateDeviceMemoryFunction([NativeType("VmaAllocator")] IntPtr allocator, uint memoryType, VkDeviceMemory memory, VkDeviceSize size, IntPtr userData);
	public delegate void VMAFreeDeviceMemoryFunction([NativeType("VmaAllocator")] IntPtr allocator, uint memoryType, VkDeviceMemory memory, VkDeviceSize size, IntPtr userData);

	#region Structures

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADeviceMemoryCallbacks {

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VMAAllocateDeviceMemoryFunction Allocate;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VMAFreeDeviceMemoryFunction Free;
		public IntPtr UserData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAVulkanFunctions {

		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKGetInstanceProcAddr vkGetInstanceProcAddr;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKGetDeviceProcAddr vkGetDeviceProcAddr;

		public unsafe delegate* unmanaged<VkPhysicalDevice, out VKPhysicalDeviceProperties, void> vkGetPhysicalDeviceProperties;
		public unsafe delegate* unmanaged<VkPhysicalDevice, out VKPhysicalDeviceMemoryProperties, void> vkGetPhysicalDeviceMemoryProperties;
		public unsafe delegate* unmanaged<VkDevice, in VKMemoryAllocateInfo, VKAllocationCallbacks*, out VkDeviceMemory, VKResult> vkAllocateMemory;
		public unsafe delegate* unmanaged<VkDevice, VkDeviceMemory, VKAllocationCallbacks*, void> vkFreeMemory;
		public unsafe delegate* unmanaged<VkDevice, VkDeviceMemory, VkDeviceSize, VkDeviceSize, VKMemoryMapFlagBits, out IntPtr, VKResult> vkMapMemory;
		public unsafe delegate* unmanaged<VkDevice, VkDeviceMemory, void> vkUnmapMemory;
		public unsafe delegate* unmanaged<VkDevice, uint, VKMappedMemoryRange*, VKResult> vkFlushMappedMemoryRanges;
		public unsafe delegate* unmanaged<VkDevice, uint, VKMappedMemoryRange*, VKResult> vkInvalidateMappedMemoryRanges;
		public unsafe delegate* unmanaged<VkDevice, VkBuffer, VkDeviceMemory, VkDeviceSize, VKResult> vkBindBufferMemory;
		public unsafe delegate* unmanaged<VkDevice, VkImage, VkDeviceMemory, VkDeviceSize, VKResult> vkBindImageMemory;
		public unsafe delegate* unmanaged<VkDevice, VkBuffer, out VKMemoryRequirements, void> vkGetBufferMemoryRequirements;
		public unsafe delegate* unmanaged<VkDevice, VkImage, out VKMemoryRequirements, void> vkGetImageMemoryRequirements;
		public unsafe delegate* unmanaged<VkDevice, in VKBufferCreateInfo, VKAllocationCallbacks*, out VkBuffer, VKResult> vkCreateBuffer;
		public unsafe delegate* unmanaged<VkDevice, VkBuffer, VKAllocationCallbacks*, void> vkDestroyBuffer;
		public unsafe delegate* unmanaged<VkDevice, in VKImageCreateInfo, VKAllocationCallbacks*, out VkImage, VKResult> vkCreateImage;
		public unsafe delegate* unmanaged<VkDevice, VkImage, VKAllocationCallbacks*, void> vkDestroyImage;
		public unsafe delegate* unmanaged<VkCommandBuffer, VkBuffer, VkBuffer, uint, VKBufferCopy*, void> vkCmdCopyBuffer;
		public unsafe delegate* unmanaged<VkDevice, in VKBufferMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetBufferMemoryRequirements2;
		public unsafe delegate* unmanaged<VkDevice, in VKImageMemoryRequirementsInfo2, ref VKMemoryRequirements2, void> vkGetImageMemoryRequirements2;
		public unsafe delegate* unmanaged<VkDevice, uint, VKBindBufferMemoryInfo*, VKResult> vkBindBufferMemory2;
		public unsafe delegate* unmanaged<VkDevice, uint, VKBindImageMemoryInfo*, VKResult> vkBindImageMemory2;
		public unsafe delegate* unmanaged<VkPhysicalDevice, ref VKPhysicalDeviceMemoryProperties2, void> vkGetPhysicalDeviceMemoryProperties2;
		public unsafe delegate* unmanaged<VkDevice, in VKDeviceBufferMemoryRequirements, ref VKMemoryRequirements2, void> vkGetDeviceBufferMemoryRequirementsKHR;
		public unsafe delegate* unmanaged<VkDevice, in VKDeviceImageMemoryRequirements, ref VKMemoryRequirements2, void> vkGetDeviceImageMemoryRequirementsKHR;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VMAAllocatorCreateInfo {

		private readonly VMAAllocatorCreateFlagBits flags;
		public VMAAllocatorCreateFlagBits Flags { get => flags; init => flags = value; }

		private readonly VkPhysicalDevice physicalDevice;
		public VkPhysicalDevice PhysicalDevice { get => physicalDevice; init => physicalDevice = value; }

		private readonly VkDevice device;
		public VkDevice Device { get => device; init => device = value; }

		private readonly ulong preferredLargeHeapBlockSize;
		public ulong PreferredLargeHeapBlockSize { get => preferredLargeHeapBlockSize; init => preferredLargeHeapBlockSize = value; }

		private readonly IntPtr allocationCallbacks;
		[NativeType("const VkAllocationCallbacks*")]
		public IntPtr AllocationCallbacks { get => allocationCallbacks; init => allocationCallbacks = value; }

		private readonly IntPtr deviceMemoryCallbacks;
		[NativeType("const VmaDeviceMemoryCallbacks*")]
		public IntPtr DeviceMemoryCallbacks { get => deviceMemoryCallbacks; init => deviceMemoryCallbacks = value; }

		private readonly IntPtr heapSizeLimit;
		[NativeType("const VkDeviceSize*")]
		public IntPtr HeapSizeLimit { get => heapSizeLimit; init => heapSizeLimit = value; }

		private readonly IntPtr vulkanFunctions;
		[NativeType("const VmaVulkanFunctions*")]
		public IntPtr VulkanFunctions { get => vulkanFunctions; init => vulkanFunctions = value; }

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
	public struct VMAStatistics {

		internal const int SizeOf = (2 * sizeof(uint)) + (2 * sizeof(ulong)); // 24

		public uint BlockCount;

		public uint AllocationCount;

		[NativeType("VkDeviceSize")]
		public ulong BlockBytes;

		[NativeType("VkDeviceSize")]
		public ulong AllocationBytes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADetailedStatistics {

		internal const int SizeOf = VMAStatistics.SizeOf + sizeof(uint) + 4 /* padding */ + (4 * sizeof(ulong));

		public VMAStatistics Statistics;

		public uint UnusedRangeCount;

		[NativeType("VkDeviceSize")]
		public ulong AllocationSizeMin;
		[NativeType("VkDeviceSize")]
		public ulong AllocationSizeMax;

		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeMin;
		[NativeType("VkDeviceSize")]
		public ulong UnusedRangeSizeMax;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMATotalStatistics {

		private unsafe fixed byte memoryType[VK10.MaxMemoryTypes * VMADetailedStatistics.SizeOf];
		public Span<VMADetailedStatistics> MemoryType {
			get {
				unsafe {
					return MemoryMarshal.Cast<byte, VMADetailedStatistics>(MemoryMarshal.CreateSpan(ref memoryType[0], VK10.MaxMemoryTypes * VMADetailedStatistics.SizeOf));
				}
			}
		}
		private unsafe fixed byte memoryHeap[VK10.MaxMemoryHeaps * VMADetailedStatistics.SizeOf];
		public Span<VMADetailedStatistics> MemoryHeap {
			get {
				unsafe {
					return MemoryMarshal.Cast<byte, VMADetailedStatistics>(MemoryMarshal.CreateSpan(ref memoryHeap[0], VK10.MaxMemoryHeaps * VMADetailedStatistics.SizeOf));
				}
			}
		}
		public VMADetailedStatistics Total;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMABudget {

		public VMAStatistics Statistics;

		[NativeType("VkDeviceSize")]
		public ulong Usage;

		[NativeType("VkDeviceSize")]
		public ulong Budget;

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

		private readonly float priority;
		public float Priority { get => priority; init => priority = value; }

		private readonly ulong minAllocationAlignment;
		[NativeType("VkDeviceSize")]
		public ulong MinAllocationAlignment { get => minAllocationAlignment; init => minAllocationAlignment = value; }

		private readonly IntPtr memoryAllocateNext;
		public IntPtr MemoryAllocateNext { get => memoryAllocateNext; init => memoryAllocateNext = value; }

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

		[NativeType("const char*")]
		public IntPtr Name;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationInfo {

		public VMADefragmentationFlagBits Flags;

		[NativeType("VmaPool")]
		public IntPtr Pool;

		[NativeType("VkDeviceSize")]
		public ulong MaxBytesPerPass;

		public uint MaxAllocationsPerPass;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationMove {

		public VMADefragmentationMoveOperation Operation;

		[NativeType("VmaAllocation")]
		public IntPtr SrcAllocation;

		[NativeType("VmaAllocation")]
		public IntPtr DstTmpAllocation;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMADefragmentationPassMoveInfo {

		public uint MoveCount;

		[NativeType("VmaDefragmentationMove*")]
		public IntPtr Moves;

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

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAVirtualBlockCreateInfo {

		[NativeType("VkDeviceSize")]
		public ulong Size;

		public VMAVirtualBlockCreateFlagBits Flags;

		[NativeType("const VkAllocationCallbacks*")]
		public IntPtr AllocationCallbacks;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAVirtualAllocationCreateInfo {

		[NativeType("VkDeviceSize")]
		public ulong Size;

		[NativeType("VkDeviceSize")]
		public ulong Alignment;

		public VMAVirtualAllocationCreateFlagBits Flags;

		public IntPtr UserData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VMAVirtualAllocationInfo {

		[NativeType("VkDeviceSize")]
		public ulong Offset;

		[NativeType("VkDeviceSize")]
		public ulong Size;

		public IntPtr UserData;

	}

	#endregion

	public unsafe class VMAFunctions {

		[NativeType("VkResult vmaCreateAllocator(const VmaAllocatorCreateInfo* pCreateInfo, VmaAllocator* pAllocator)")]
		public delegate* unmanaged<VMAAllocatorCreateInfo*, out IntPtr, VKResult> vmaCreateAllocator;
		[NativeType("void vmaDestroyAllocator(VmaAllocator allocator)")]
		public delegate* unmanaged<IntPtr, void> vmaDestroyAllocator;

		[NativeType("void vmaGetAllocatorInfo(VmaAllocator allocator, VmaAllocatorInfo* pInfo)")]
		public delegate* unmanaged<IntPtr, out VMAAllocatorInfo, void> vmaGetAllocatorInfo;
		[NativeType("void vmaGetPhysicalDeviceProperties(VmaAllocator allocator, VkPhysicalDeviceProperties* pProperties)")]
		public delegate* unmanaged<IntPtr, out VKPhysicalDeviceProperties, void> vmaGetPhysicalDeviceProperties;
		[NativeType("void vmaGetMemoryProperties(VmaAllocator allocator, const VkPhysicalDeviceMemoryProperties** ppProperties)")]
		public delegate* unmanaged<IntPtr, out VKPhysicalDeviceMemoryProperties*, void> vmaGetMemoryProperties;
		[NativeType("void vmaGetMemoryTypeProperties(VmaAllocator allocator, uint32_t memoryTypeIndex, VkMemoryPropertyFlags* pFlags)")]
		public delegate* unmanaged<IntPtr, uint, out VKMemoryPropertyFlagBits, void> vmaGetMemoryTypeProperties;
		[NativeType("void vmaSetCurrentFrameIndex(VmaAllocator allocator, uint32_t frameIndex)")]
		public delegate* unmanaged<IntPtr, uint, void> vmaSetCurrentFrameIndex;

		[NativeType("void vmaCalculateStatistics(VmaAllocator allocator, VmaTotalStatistics* pStats)")]
		public delegate* unmanaged<IntPtr, out VMATotalStatistics, void> vmaCalculateStatistics;

		[NativeType("void vmaGetHeapBudgets(VmaAllocator allocator, VmaBudget* pBudgets)")]
		public delegate* unmanaged<IntPtr, VMABudget*, void> vmaGetHeapBudgets;

		[NativeType("VkResult vmaFindMemoryTypeIndex(VmaAllocator allocator, uint32_t memoryTypeBits, const VmaAllocationCreateInfo* pCreateInfo, uint32_t* pMemoryTypeIndex)")]
		public delegate* unmanaged<IntPtr, uint, in VMAAllocationCreateInfo, out uint, VKResult> vmaFindMemoryTypeIndex;
		[NativeType("VkResult vmaFindMemoryTypeIndexForBufferInfo(VmaAllocator allocator, const VkBufferCreateInfo* pBufferCreateInfo, const VmaAllocationCreateInfo* pAllocationCreateInfo, uint32_t* pMemoryTypeIndex)")]
		public delegate* unmanaged<IntPtr, in VKBufferCreateInfo, in VMAAllocationCreateInfo, out uint, VKResult> vmaFindMemoryTypeIndexForBufferInfo;
		[NativeType("VkResult vmaFindMemoryTypeIndexForImageInfo(VmaAllocator allocator, const VkImageCreateInfo* pBufferCreateInfo, const VmaAllocationCreateInfo* pAllocationCreateInfo, uint32_t* pMemoryTypeIndex)")]
		public delegate* unmanaged<IntPtr, in VKImageCreateInfo, in VMAAllocationCreateInfo, out uint, VKResult> vmaFindMemoryTypeIndexForImageInfo;

		[NativeType("VkResult vmaCreatePool(VmaAllocator allocator, const VmaPoolCreateInfo* pCreateInfo, VmaPool* pPool)")]
		public delegate* unmanaged<IntPtr, in VMAPoolCreateInfo, out IntPtr, VKResult> vmaCreatePool;
		[NativeType("void vmaDestroyPool(VmaAllocator allocator, VmaPool pool)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaDestroyPool;
		[NativeType("void vmaGetPoolStatistics(VmaAllocator allocator, VmaPool pool, VmaStatistics* pPoolStats)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VMAStatistics, void> vmaGetPoolStatistics;
		[NativeType("void vmaCalculatePoolStatistics(VmaAllocator allocator, VmaPool pool, VmaStatistics* pPoolStats)")]
		public delegate* unmanaged<IntPtr, IntPtr, ref VMAStatistics, void> vmaCalculatePoolStatistics;
		[NativeType("VkResult vmaCheckPoolCorruption(VmaAllocator allocator, VmaPool pool)")]
		public delegate* unmanaged<IntPtr, IntPtr, VKResult> vmaCheckPoolCorruption;
		[NativeType("void vmaGetPoolName(VmaAllocator allocator, VmaPool pool, const char** pName)")]
		public delegate* unmanaged<IntPtr, IntPtr, out IntPtr, void> vmaGetPoolName;
		[NativeType("void vmaSetPoolName(VmaAllocator allocator, VmaPool pool, const char* name)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> vmaSetPoolName;

		[NativeType("VkResult vmaAllocateMemory(VmaAllocator allocator, const VkMemoryRequirements* pMemoryRequirements, const VmaAllocationCreateInfo* pCreateInfo, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, VKMemoryRequirements*, VMAAllocationCreateInfo*, out IntPtr, out VMAAllocationInfo, VKResult> vmaAllocateMemory;
		[NativeType("VkResult vmaAllocateMemoryPages(VmaAllocator allocator, const VkMemoryRequirements* pMemoryRequirements, const VmaAllocationCreateInfo* pCreateInfo, size_t allocationCount, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, VKMemoryRequirements*, VMAAllocationCreateInfo*, nuint, IntPtr*, VMAAllocationInfo*, VKResult> vmaAllocateMemoryPages;
		[NativeType("VkResult vmaAllocateMemoryForBuffer(VmaAllocator allocator, VkBuffer buffer, const VmaAllocationCreateInfo* pCreateInfo, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, ulong, VMAAllocationCreateInfo*, out IntPtr, out VMAAllocationInfo, VKResult> vmaAllocateMemoryForBuffer;
		[NativeType("VkResult vmaAllocateMemoryForImage(VmaAllocator allocator, VkImage image, const VmaAllocationCreateInfo* pCreateInfo, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, ulong, VMAAllocationCreateInfo*, out IntPtr, out VMAAllocationInfo, VKResult> vmaAllocateMemoryForImage;
		[NativeType("void vmaFreeMemory(VmaAllocator allocator, VmaAllocation allocation)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaFreeMemory;
		[NativeType("void vmaFreeMemoryPages(VmaAllocator allocator, size_t allocationCount, const VmaAllocation* pAllocation)")]
		public delegate* unmanaged<IntPtr, nuint, IntPtr*, void> vmaFreeMemoryPages;
		[NativeType("void vmaGetAllocationInfo(VmaAllocator allocator, VmaAllocation allocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VMAAllocationInfo, void> vmaGetAllocationInfo;
		[NativeType("void vmaSetAllocationUserData(VmaAllocator allocator, VmaAllocation allocation, void* userData)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> vmaSetAllocationUserData;
		[NativeType("void vmaSetAllocationName(VmaAllocator allocator, VmaAllocation allocation, const char* name)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> vmaSetAllocationName;
		[NativeType("void vmaGetAllocationMemoryProperties(VmaAllocator allocator, VmaAllocation allocation, VkMemoryPropertyFlags* pFlags)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VKMemoryPropertyFlagBits, void> vmaGetAllocationMemoryProperties;
		[NativeType("VkResult vmaMapMemory(VmaAllocator allocator, VmaAllocation allocation, void** pData)")]
		public delegate* unmanaged<IntPtr, IntPtr, out IntPtr, VKResult> vmaMapMemory;
		[NativeType("void vmaUnmapMemory(VmaAllocator allocator, VmaAllocation allocation)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaUnmapMemory;
		[NativeType("VkResult vmaFlushAllocation(VmaAllocator allocator, VmaAllocation allocation, VkDeviceSize offset, VkDeviceSize size)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, ulong, VKResult> vmaFlushAllocation;
		[NativeType("VkResult vmaInvalidateAllocation(VmaAllocator allocator, VmaAllocation allocation, VkDeviceSize offset, VkDeviceSize size)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, ulong, VKResult> vmaInvalidateAllocation;
		[NativeType("VkResult vmaFlushAllocations(VmaAllocator allocator, uint32_t allocationCount, const VmaAllocation* pAllocations, const VkDeviceSize* pOffsets, const VkDeviceSize* pSizes)")]
		public delegate* unmanaged<IntPtr, uint, IntPtr*, ulong*, ulong*, VKResult> vmaFlushAllocations;
		[NativeType("VkResult vmaInvalidateAllocations(VmaAllocator allocator, uint32_t allocationCount, const VmaAllocation* pAllocations, const VkDeviceSize* pOffsets, const VkDeviceSize* pSizes)")]
		public delegate* unmanaged<IntPtr, uint, IntPtr*, ulong*, ulong*, VKResult> vmaInvalidateAllocations;
		[NativeType("VkResult vmaCheckCorruption(VmaAllocator allocator, uint32_t memoryTypeBits)")]
		public delegate* unmanaged<IntPtr, uint, VKResult> vmaCheckCorruption;

		[NativeType("VkResult vmaBeginDefragmentation(VmaAllocator allocator, const VmaDefragmentationInfo* pInfo, VmaDefragmentationContext* pContext)")]
		public delegate* unmanaged<IntPtr, VMADefragmentationInfo*, out IntPtr, VKResult> vmaBeginDefragmentation;
		[NativeType("VkResult vmaEndDefragmentation(VmaAllocator allocator, VmaDefragmentationContext* pContext, VmaDefragmentationStats* pStats)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VMADefragmentationStats, VKResult> vmaEndDefragmentation;
		[NativeType("VkResult vmaBeginDefragmentationPass(VmaAllocator allocator, VmaDefragmentationContext* pContext, VmaDefragmentationPassMoveInfo* pPassInfo)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VMADefragmentationPassMoveInfo, VKResult> vmaBeginDefragmentationPass;
		[NativeType("VkResult vmaEndDefragmentationPass(VmaAllocator allocator, VmaDefragmentationContext* pContext, VmaDefragmentationPassMoveInfo* pPassInfo)")]
		public delegate* unmanaged<IntPtr, IntPtr, VMADefragmentationPassMoveInfo*, VKResult> vmaEndDefragmentationPass;
		[NativeType("VkResult vmaBindBufferMemory(VmaAllocator allocator, VmaAllocation allocation, VkBuffer buffer)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, VKResult> vmaBindBufferMemory;
		[NativeType("VkResult vmaBindBufferMemory2(VmaAllocator allocator, VmaAllocation allocation, VkDeviceSize allocationLocalOffset, VkBuffer buffer, void* pNext)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, ulong, IntPtr, VKResult> vmaBindBufferMemory2;
		[NativeType("VkResult vmaBindImageMemory(VmaAllocator allocator, VmaAllocation allocation, VkImage image)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, VKResult> vmaBindImageMemory;
		[NativeType("VkResult vmaBindImageMemory2(VmaAllocator allocator, VmaAllocation allocation, VkDeviceSize allocationLocalOffset, VkImage image, void* pNext)")]
		public delegate* unmanaged<IntPtr, IntPtr, ulong, ulong, IntPtr, VKResult> vmaBindImageMemory2;
		[NativeType("VkResult vmaCreateBuffer(VmaAllocator allocator, const VkBufferCreateInfo* pBufferCreateInfo, const VmaAllocationCreateInfo* pAllocationCreateInfo, VkBuffer* pBuffer, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, VKBufferCreateInfo*, VMAAllocationCreateInfo*, out ulong, out IntPtr, out VMAAllocationInfo, VKResult> vmaCreateBuffer;
		[NativeType("VkResult vmaCreateBufferWithAlignment(VmaAllocator allocator, const VkBufferCreateInfo* pBufferCreateInfo, const VmaAllocationCreateInfo* pAllocationCreateInfo, VkDeviceSize minAlignment, VkBuffer* pBuffer, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo")]
		public delegate* unmanaged<IntPtr, VKBufferCreateInfo*, VMAAllocationCreateInfo*, ulong, out ulong, out IntPtr, out VMAAllocationInfo, VKResult> vmaCreateBufferWithAlignment;
		[NativeType("VkResult vmaCreateAliasingBuffer(VmaAllocator allocator, VmaAllocation allocation, const VkBufferCreateInfo* pCreateInfo, VkBuffer* pBuffer)")]
		public delegate* unmanaged<IntPtr, IntPtr, VKBufferCreateInfo*, out ulong, VKResult> vmaCreateAliasingBuffer;
		[NativeType("void vmaDestroyBuffer(VmaAllocator allocator, VkBuffer buffer, VmaAllocation allocation)")]
		public delegate* unmanaged<IntPtr, ulong, IntPtr, void> vmaDestroyBuffer;
		[NativeType("VkResult vmaCreateImage(VmaAllocator allocator, const VkImageCreateInfo* pImageCreateInfo, const VmaAllocationCreateInfo* pAllocationCreateInfo, VkImage* pImage, VmaAllocation* pAllocation, VmaAllocationInfo* pAllocationInfo)")]
		public delegate* unmanaged<IntPtr, VKImageCreateInfo*, VMAAllocationCreateInfo*, out ulong, out IntPtr, out VMAAllocationInfo, VKResult> vmaCreateImage;
		[NativeType("VkResult vmaCreateAliasingImage(VmaAllocator allocator, VmaAllocation allocation, const VkImageCreateInfo* pImageCreateInfo, VkImage* pImage)")]
		public delegate* unmanaged<IntPtr, IntPtr, VKImageCreateInfo*, out ulong, VKResult> vmaCreateAliasingImage;
		[NativeType("void vmaDestroyImage(VmaAllocator allocator, VkImage image, VmaAllocation allocation)")]
		public delegate* unmanaged<IntPtr, ulong, IntPtr, void> vmaDestroyImage;

		[NativeType("VkResult vmaCreateVirtualBlock(const VmaVirtualBlockCreateInfo* pCreateInfo, VmaVirtualBlock* pVirtualBlock)")]
		public delegate* unmanaged<VMAVirtualBlockCreateInfo*, out IntPtr, VKResult> vmaCreateVirtualBlock;
		[NativeType("void vmaDestroyVirtualBlock(VmaVirtualBlock virtualBlock)")]
		public delegate* unmanaged<IntPtr, void> vmaDestroyVirtualBlock;
		[NativeType("VkBool32 vmaIsVirtualBlockEmpty(VmaVirtualBlock virtualBlock)")]
		public delegate* unmanaged<IntPtr, bool> vmaIsVirtualBlockEmpty;
		[NativeType("void vmaGetVirtualAllocationInfo(VmaVirtualBlock virtualBlock, VmaVirtualAllocation allocation, VmaVirtualAllocationInfo* pVirtualAllocInfo)")]
		public delegate* unmanaged<IntPtr, IntPtr, out VMAVirtualAllocationInfo, void> vmaGetVirtualAllocationInfo;
		[NativeType("VkResult vmaVirtualAllocate(VmaVirtualBlock virtualBlock, const VmaVirtualAllocationCreateInfo* pCreateInfo, VmaVirtualAllocation* pAllocation, VkDeviceSize* pOffset)")]
		public delegate* unmanaged<IntPtr, VMAVirtualAllocationCreateInfo*, out IntPtr, out ulong, VKResult> vmaVirtualAllocate;
		[NativeType("void vmaVirtualFree(VmaVirtualBlock virtualBlock, VmaVirtualAllocation virtualAllocation)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaVirtualFree;
		[NativeType("void vmaClearVirtualBlock(VmaVirtualBlock virtualBlock)")]
		public delegate* unmanaged<IntPtr, void> vmaClearVirtualBlock;
		[NativeType("void vmaSetVirtualAllocationUserData(VmaVirtualBlock virtualBlock, VmaVirtualAllocation allocation, void* userData)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> vmaSetVirtualAllocationUserData;
		[NativeType("void vmaGetVirtualBlockStatistics(VmaVirtualBlock virtualBlock, VmaStatistics* pStats)")]
		public delegate* unmanaged<IntPtr, out VMAStatistics, void> vmaGetVirtualBlockStatistics;
		[NativeType("void vmaCalculateVirtualBlockStatistics(VmaVirtualBlock virtualBlock, VmaDetailedStatistics* pStats)")]
		public delegate* unmanaged<IntPtr, out VMADetailedStatistics, void> vmaCalculateVirtualBlockStatistics;
		[NativeType("void vmaBuildVirtualBlockStatsString(VmaVirtualBlock virtualBlock, char** pStatsString, VkBool32 detailedMap)")]
		public delegate* unmanaged<IntPtr, out IntPtr, bool, void> vmaBuildVirtualBlockStatsString;
		[NativeType("void vmaFreeVirtualBlockStatsString(VmaVirtualBlock virtualBlock, char* statsString)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaFreeVirtualBlockStatsString;

		[NativeType("void vmaBuildStatsString(VmaAllocator allocator, char** pStatsString, VkBool32 detailedMap)")]
		public delegate* unmanaged<IntPtr, out IntPtr, bool, void> vmaBuildStatsString;
		[NativeType("void vmaFreeStatsString(VmaAllocator allocator, char* statsString)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> vmaFreeStatsString;

	}

	public static class VMA {

		public static readonly LibrarySpec LibrarySpec = new() { Name = "VulkanMemoryAllocator" };

		public static Library Library { get; } = LibraryManager.Load(LibrarySpec);

		public static VMAFunctions Functions { get; } = new();

		static VMA() {
			Library.LoadFunctions(Functions);
		}

		public static VMAAllocator CreateAllocator(in VMAAllocatorCreateInfo createInfo, VKDevice device) {
			unsafe {
				fixed (VMAAllocatorCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(Functions.vmaCreateAllocator(pCreateInfo, out IntPtr allocator), "Failed to create VMA allocator");
					if (createInfo.Device != device) throw new ArgumentException("Supplied device does not match that of the creation information", nameof(device));
					return new VMAAllocator(allocator, device, createInfo.AllocationCallbacks);
				}
			}
		}

		public static VMAVirtualBlock CreateVirtualBlock(in VMAVirtualBlockCreateInfo createInfo) {
			unsafe {
				fixed(VMAVirtualBlockCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(Functions.vmaCreateVirtualBlock(pCreateInfo, out IntPtr virtualBlock));
					return new VMAVirtualBlock(virtualBlock);
				}
			}
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
			unsafe {
				VMA.Functions.vmaDestroyAllocator(Allocator);
			}
		}

		public VMAAllocatorInfo Info {
			get {
				unsafe {
					VMA.Functions.vmaGetAllocatorInfo(Allocator, out VMAAllocatorInfo info);
					return info;
				}
			}
		}

		public VKPhysicalDeviceProperties PhysicalDeviceProperties {
			get {
				unsafe {
					VMA.Functions.vmaGetPhysicalDeviceProperties(Allocator, out VKPhysicalDeviceProperties properties);
					return properties;
				}
			}
		}

		public UnmanagedPointer<VKPhysicalDeviceMemoryProperties> MemoryProperties {
			get {
				unsafe {
					VMA.Functions.vmaGetMemoryProperties(Allocator, out VKPhysicalDeviceMemoryProperties* pProperties);
					return new UnmanagedPointer<VKPhysicalDeviceMemoryProperties>((IntPtr)pProperties);
				}
			}
		}

		public VKMemoryPropertyFlagBits GetMemoryTypeProperties(uint memoryTypeIndex) {
			unsafe {
				VMA.Functions.vmaGetMemoryTypeProperties(Allocator, memoryTypeIndex, out VKMemoryPropertyFlagBits flags);
				return flags;
			}
		}

		public uint CurrentFrameIndex {
			set {
				unsafe {
					VMA.Functions.vmaSetCurrentFrameIndex(Allocator, value);
				}
			}
		}

		public VMATotalStatistics Statistics {
			get {
				unsafe {
					VMA.Functions.vmaCalculateStatistics(Allocator, out VMATotalStatistics stats);
					return stats;
				}
			}
		}

		public string BuildStatsString(bool detailedMap) {
			unsafe {
				VMA.Functions.vmaBuildStatsString(Allocator, out IntPtr pString, detailedMap);
				string str = MemoryUtil.GetASCII(pString)!;
				VMA.Functions.vmaFreeStatsString(Allocator, pString);
				return str;
			}
		}

		public uint FindMemoryTypeIndex(uint memoryTypeBits, in VMAAllocationCreateInfo createInfo) {
			unsafe {
				fixed (VMAAllocationCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndex(Allocator, memoryTypeBits, createInfo, out uint typeIndex), "Failed to find memory type index");
					return typeIndex;
				}
			}
		}

		public uint FindMemoryTypeIndex(in VKBufferCreateInfo bufferInfo, in VMAAllocationCreateInfo createInfo) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndexForBufferInfo(Allocator, bufferInfo, createInfo, out uint typeIndex), "Failed to find memory type index for buffer info");
				return typeIndex;
			}
		}

		public uint FindMemoryTypeIndex(in VKImageCreateInfo imageInfo, in VMAAllocationCreateInfo createInfo) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaFindMemoryTypeIndexForImageInfo(Allocator, imageInfo, createInfo, out uint typeIndex), "Failed to find memory type index for image info");
				return typeIndex;
			}
		}

		public VMAPool CreatePool(in VMAPoolCreateInfo createInfo) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaCreatePool(Allocator, createInfo, out IntPtr pool), "Failed to create VMA pool");
				return new VMAPool(this, pool);
			}
		}

		public VMAAllocation AllocateMemory(in VKMemoryRequirements memoryRequirements, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed(VKMemoryRequirements* pMemoryRequirements = &memoryRequirements) {
					fixed(VMAAllocationCreateInfo* pCreateInfo = &createInfo) {
						VK.CheckError(VMA.Functions.vmaAllocateMemory(Allocator, pMemoryRequirements, pCreateInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory");
						return new VMAAllocation(this, allocation);
					}
				}
			}
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
								VK.CheckError(VMA.Functions.vmaAllocateMemoryPages(Allocator, pReqs, pCreateInfos, (nuint)n, pAllocs, pInfos), "Failed to allocate memory pages");
							}
						}
					}
				}
			}
			allocations = new VMAAllocation[n];
			for (int i = 0; i < n; i++) allocations[i] = new VMAAllocation(this, allocs[i]);
		}

		public VMAAllocation AllocateMemoryForBuffer(VKBuffer buffer, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed (VMAAllocationCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VMA.Functions.vmaAllocateMemoryForBuffer(Allocator, buffer, pCreateInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory for buffer");
					return new VMAAllocation(this, allocation);
				}
			}
		}

		public VMAAllocation AllocateMemoryForImage(VKImage image, in VMAAllocationCreateInfo createInfo, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed(VMAAllocationCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VMA.Functions.vmaAllocateMemoryForImage(Allocator, image, pCreateInfo, out IntPtr allocation, out allocationInfo), "Failed to allocate memory for image");
					return new VMAAllocation(this, allocation);
				}
			}
		}

		public void FreeMemoryPages(in ReadOnlySpan<VMAAllocation> allocations) {
			Span<IntPtr> allocs = stackalloc IntPtr[allocations.Length];
			for (int i = 0; i < allocations.Length; i++) allocs[i] = allocations[i];
			unsafe {
				fixed(IntPtr* pAllocs = allocs) {
					VMA.Functions.vmaFreeMemoryPages(Allocator, (nuint)allocs.Length, pAllocs);
				}
			}
			for (int i = 0; i < allocations.Length; i++) allocations[i].Allocation = IntPtr.Zero;
		}

		public void FlushAllocations(in ReadOnlySpan<VMAAllocation> allocations, in ReadOnlySpan<ulong> offsets, in ReadOnlySpan<ulong> sizes) {
			int n = ExMath.Min(allocations.Length, offsets.Length, sizes.Length);
			Span<IntPtr> allocs = stackalloc IntPtr[n];
			for (int i = 0; i < n; i++) allocs[i] = allocations[i];
			unsafe {
				fixed(IntPtr* pAllocs = allocs) {
					fixed(ulong* pOffsets = offsets) {
						fixed(ulong* pSizes = sizes) {
							VK.CheckError(VMA.Functions.vmaFlushAllocations(Allocator, (uint)n, pAllocs, pOffsets, pSizes), "Failed to flush VMA allocations");
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
							VK.CheckError(VMA.Functions.vmaInvalidateAllocations(Allocator, (uint)n, pAllocs, pOffsets, pSizes), "Failed to invalidate VMA allocations");
						}
					}
				}
			}
		}

		public void CheckCorruption(uint memoryTypeBits) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaCheckCorruption(Allocator, memoryTypeBits), "Failure while checking memory corruption");
			}
		}

		public VMADefragmentationContext BeginDefragmentation(in VMADefragmentationInfo info) {
			unsafe {
				fixed(VMADefragmentationInfo* pInfo = &info) {
					VK.CheckError(VMA.Functions.vmaBeginDefragmentation(Allocator, pInfo, out IntPtr context), "Failed to begin defragmentation");
					return new VMADefragmentationContext(this, context);
				}
			}
		}

		public VKBuffer CreateBuffer(in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed(VKBufferCreateInfo* pBufferCreateInfo = &bufferCreateInfo) {
					fixed(VMAAllocationCreateInfo* pAllocationCreateInfo = &allocationCreateInfo) {
						VK.CheckError(VMA.Functions.vmaCreateBuffer(Allocator, pBufferCreateInfo, pAllocationCreateInfo, out ulong buffer, out IntPtr alloc, out allocationInfo), "Failed to create buffer");
						allocation = new VMAAllocation(this, alloc);
						return new VKBuffer(Device, buffer, AllocationCallbacks);
					}
				}
			}
		}

		public VKBuffer CreateBufferWithAlignment(in VKBufferCreateInfo bufferCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, ulong minAlignment, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed(VKBufferCreateInfo* pBufferCreateInfo = &bufferCreateInfo) {
					fixed(VMAAllocationCreateInfo* pAllocationCreateInfo = &allocationCreateInfo) {
						VK.CheckError(VMA.Functions.vmaCreateBufferWithAlignment(Allocator, pBufferCreateInfo, pAllocationCreateInfo, minAlignment, out ulong buffer, out IntPtr alloc, out allocationInfo), "Failed to create buffer");
						allocation = new VMAAllocation(this, alloc);
						return new VKBuffer(Device, buffer, AllocationCallbacks);
					}
				}
			}
		}

		public VKBuffer CreateAliasingBuffer(VMAAllocation allocation, in VKBufferCreateInfo bufferCreateInfo) {
			unsafe {
				fixed (VKBufferCreateInfo* pBufferCreateInfo = &bufferCreateInfo) {
					VK.CheckError(VMA.Functions.vmaCreateAliasingBuffer(Allocator, allocation, pBufferCreateInfo, out ulong buffer));
					return new VKBuffer(Device, buffer, AllocationCallbacks);
				}
			}
		}

		public VKImage CreateImage(in VKImageCreateInfo imageCreateInfo, in VMAAllocationCreateInfo allocationCreateInfo, out VMAAllocation allocation, out VMAAllocationInfo allocationInfo) {
			unsafe {
				fixed(VKImageCreateInfo* pImageCreateInfo = &imageCreateInfo) {
					fixed(VMAAllocationCreateInfo* pAllocationCreateInfo = &allocationCreateInfo) {
						VK.CheckError(VMA.Functions.vmaCreateImage(Allocator, pImageCreateInfo, pAllocationCreateInfo, out ulong image, out IntPtr alloc, out allocationInfo), "Failed to create image");
						allocation = new VMAAllocation(this, alloc);
						return new VKImage(Device, image, AllocationCallbacks);
					}
				}
			}
		}

		public VKImage CreateAliasingImage(VMAAllocation allocation, in VKImageCreateInfo imageCreateInfo) {
			unsafe {
				fixed(VKImageCreateInfo* pImageCreateInfo = &imageCreateInfo) {
					VK.CheckError(VMA.Functions.vmaCreateAliasingImage(Allocator, allocation, pImageCreateInfo, out ulong image));
					return new VKImage(Device, image, AllocationCallbacks);
				}
			}
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
			unsafe {
				VMA.Functions.vmaDestroyPool(Allocator, Pool);
			}
		}

		public VMAStatistics Stats {
			get {
				unsafe {
					VMA.Functions.vmaGetPoolStatistics(Allocator, Pool, out VMAStatistics stats);
					return stats;
				}
			}
		}

		public void CheckPoolCorruption() {
			unsafe {
				VK.CheckError(VMA.Functions.vmaCheckPoolCorruption(Allocator, Pool), "Failure during pool corruption check");
			}
		}

		public string? Name {
			get {
				unsafe {
					VMA.Functions.vmaGetPoolName(Allocator, Pool, out IntPtr name);
					return MemoryUtil.GetUTF8(name);
				}
			}
			set {
				unsafe {
					Span<byte> bytes = MemoryUtil.StackallocUTF8(value, stackalloc byte[1024]);
					fixed(byte* pStr = bytes) {
						VMA.Functions.vmaSetPoolName(Allocator, Pool, (IntPtr)pStr);
					}
				}
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
				unsafe {
					VMA.Functions.vmaFreeMemory(Allocator, Allocation);
				}
				Allocation = IntPtr.Zero;
			}
		}

		public VMAAllocationInfo Info {
			get {
				unsafe {
					VMA.Functions.vmaGetAllocationInfo(Allocator, Allocation, out VMAAllocationInfo info);
					return info;
				}
			}
		}

		public IntPtr UserData {
			get => Info.UserData;
			set {
				unsafe {
					VMA.Functions.vmaSetAllocationUserData(Allocator, Allocation, value);
				}
			}
		}

		public string? Name {
			get => MemoryUtil.GetUTF8(Info.Name);
			set {
				unsafe {
					Span<byte> bytes = MemoryUtil.StackallocUTF8(value, stackalloc byte[1024]);
					fixed(byte* pStr = bytes) {
						VMA.Functions.vmaSetAllocationName(Allocator, Allocation, (IntPtr)pStr);
					}
				}
			}
		}

		public VKMemoryPropertyFlagBits MemoryProperties {
			get {
				unsafe {
					VMA.Functions.vmaGetAllocationMemoryProperties(Allocator, Allocation, out VKMemoryPropertyFlagBits flags);
					return flags;
				}
			}
		}

		public IntPtr MapMemory() {
			unsafe {
				VK.CheckError(VMA.Functions.vmaMapMemory(Allocator, Allocation, out IntPtr data), "Failed to map VMA allocation");
				return data;
			}
		}

		public void UnmapMemory() {
			unsafe {
				VMA.Functions.vmaUnmapMemory(Allocator, Allocation);
			}
		}

		public void Flush(ulong offset, ulong size) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaFlushAllocation(Allocator, Allocation, offset, size), "Failed to flush VMA allocation memory");
			}
		}

		public void Invalidate(ulong offset, ulong size) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaInvalidateAllocation(Allocator, Allocation, offset, size), "Failed to invalidate VMA allocation memory");
			}
		}

		public void BindBufferMemory(VKBuffer buffer) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaBindBufferMemory(Allocator, Allocation, buffer), "Failed to bind VMA allocation to buffer");
			}
		}

		public void BindBufferMemory2(ulong localOffset, VKBuffer buffer, IntPtr next) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaBindBufferMemory2(Allocator, Allocation, localOffset, buffer, next), "Failed to bind VMA allocation to buffer");
			}
		}

		public void BindImageMemory(VKImage image) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaBindImageMemory(Allocator, Allocation, image), "Failed to bind VMA allocation to image");
			}
		}

		public void BindImageMemory2(ulong localOffset, VKImage image, IntPtr next) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaBindImageMemory2(Allocator, Allocation, localOffset, image, next), "Failed to bind VMA allocation to image");
			}
		}

		public void DestroyBuffer(VKBuffer buffer) {
			unsafe {
				VMA.Functions.vmaDestroyBuffer(Allocator, buffer, Allocation);
			}
		}

		public void DestroyImage(VKImage image) {
			unsafe {
				VMA.Functions.vmaDestroyImage(Allocator, image, Allocation);
			}
		}

		public static implicit operator IntPtr(VMAAllocation allocation) => allocation != null ? allocation.Allocation : IntPtr.Zero;

	}

	public readonly struct VMADefragmentationContext {

		public readonly VMAAllocator Allocator;

		[NativeType("VmaDefragmentationContext")]
		public readonly IntPtr Context;

		public VMADefragmentationContext(VMAAllocator allocator, IntPtr context) {
			Allocator = allocator;
			Context = context;
		}

		public void End(out VMADefragmentationStats stats) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaEndDefragmentation(Allocator, Context, out stats));
			}
		}

		public void BeginPass(out VMADefragmentationPassMoveInfo passInfo) {
			unsafe {
				VK.CheckError(VMA.Functions.vmaBeginDefragmentationPass(Allocator, Context, out passInfo));
			}
		}

		public void EndPass(in VMADefragmentationPassMoveInfo passInfo) {
			unsafe {
				fixed (VMADefragmentationPassMoveInfo* pPassInfo = &passInfo) {
					VK.CheckError(VMA.Functions.vmaEndDefragmentationPass(Allocator, Context, pPassInfo));
				}
			}
		}

	}

	public class VMAVirtualBlock : IDisposable {

		public IntPtr VirtualBlock { get; }

		public bool IsEmpty {
			get {
				unsafe {
					return VMA.Functions.vmaIsVirtualBlockEmpty(VirtualBlock);
				}
			}
		}

		public VMAStatistics Statistics {
			get {
				unsafe {
					VMA.Functions.vmaGetVirtualBlockStatistics(VirtualBlock, out VMAStatistics statistics);
					return statistics;
				}
			}
		}

		public VMAVirtualBlock(IntPtr virtualBlock) {
			VirtualBlock = virtualBlock;
		}

		public VMAVirtualAllocation Allocate(in VMAVirtualAllocationCreateInfo createInfo, out ulong offset) {
			unsafe {
				fixed(VMAVirtualAllocationCreateInfo* pCreateInfo = &createInfo) {
					VK.CheckError(VMA.Functions.vmaVirtualAllocate(VirtualBlock, pCreateInfo, out IntPtr allocation, out offset));
					return new VMAVirtualAllocation(allocation, this);
				}
			}
		}

		public void Clear() {
			unsafe {
				VMA.Functions.vmaClearVirtualBlock(VirtualBlock);
			}
		}

		public VMADetailedStatistics CalculateStatistics() {
			unsafe {
				VMA.Functions.vmaCalculateVirtualBlockStatistics(VirtualBlock, out VMADetailedStatistics stats);
				return stats;
			}
		}

		public string BuildStatsString(bool detailedMap = false) {
			unsafe {
				VMA.Functions.vmaBuildVirtualBlockStatsString(VirtualBlock, out IntPtr pStatsString, detailedMap);
				string ret = MemoryUtil.GetUTF8(pStatsString)!;
				VMA.Functions.vmaFreeVirtualBlockStatsString(VirtualBlock, pStatsString);
				return ret;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				VMA.Functions.vmaDestroyVirtualBlock(VirtualBlock);
			}
		}

	}

	public class VMAVirtualAllocation : IDisposable {

		public IntPtr Allocation { get; }

		public VMAVirtualBlock VirtualBlock { get; }

		public VMAVirtualAllocationInfo AllocationInfo {
			get {
				unsafe {
					VMA.Functions.vmaGetVirtualAllocationInfo(VirtualBlock.VirtualBlock, Allocation, out VMAVirtualAllocationInfo info);
					return info;
				}
			}
		}

		public IntPtr UserData {
			set {
				unsafe {
					VMA.Functions.vmaSetVirtualAllocationUserData(VirtualBlock.VirtualBlock, Allocation, value);
				}
			}
			get => AllocationInfo.UserData;
		}

		public VMAVirtualAllocation(IntPtr allocation, VMAVirtualBlock virtualBlock) {
			Allocation = allocation;
			VirtualBlock = virtualBlock;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				VMA.Functions.vmaVirtualFree(VirtualBlock.VirtualBlock, Allocation);
			}
		}

	}

}
