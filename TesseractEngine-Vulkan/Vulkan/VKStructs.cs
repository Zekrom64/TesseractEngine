using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

namespace Tesseract.Vulkan {

	using VkBool32 = Boolean;
	using VkDeviceSize = UInt64;
	using VkSampleMask = UInt32;
	using VkInstance = IntPtr;
	using VkPhysicalDevice = IntPtr;
	using VkDevice = IntPtr;
	using VkQueue = IntPtr;
	using VkSemaphore = UInt64;
	using VkCommandBuffer = IntPtr;
	using VkFence = UInt64;
	using VkDeviceMemory = UInt64;
	using VkBuffer = UInt64;
	using VkImage = UInt64;
	using VkEvent = UInt64;
	using VkQueryPool = UInt64;
	using VkBufferView = UInt64;
	using VkImageView = UInt64;
	using VkShaderModule = UInt64;
	using VkPipelineCache = UInt64;
	using VkPipelineLayout = UInt64;
	using VkRenderPass = UInt64;
	using VkPipeline = UInt64;
	using VkDescriptorSetLayout = UInt64;
	using VkSampler = UInt64;
	using VkDescriptorPool = UInt64;
	using VkDescriptorSet = UInt64;
	using VkFramebuffer = UInt64;
	using VkCommandPool = UInt64;

	using VkSamplerYcbcrConversion = UInt64;
	using VkDescriptorUpdateTemplate = UInt64;
	
	/* Notes:
	 * 
	 *     Some structs are passed as 'in' to native methods, equivalent to passing by const pointer. However,
	 *   to avoid expensive defensive copies these structs are readonly. To maintain field order
	 */

	// Vulkan 1.0

	[StructLayout(LayoutKind.Sequential)]
	public struct VKApplicationInfo {

		public VKStructureType Type;
		public IntPtr Next;
		[MarshalAs(UnmanagedType.LPStr)]
		public string ApplicationName;
		public uint ApplicationVersion;
		[MarshalAs(UnmanagedType.LPStr)]
		public string EngineName;
		public uint EngineVersion;
		public uint APIVersion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKInstanceCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKInstanceCreateFlagBits flags;
		public VKInstanceCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly IntPtr applicationInfo;
		[NativeType("const VkApplicationInfo*")]
		public IntPtr ApplicationInfo { get => applicationInfo; init { applicationInfo = value; } }
		private readonly uint enabledLayerCount;
		public uint EnabledLayerCount { get => enabledLayerCount; init { enabledLayerCount = value; } }
		private readonly IntPtr enabledLayerNames;
		[NativeType("const char* const*")]
		public IntPtr EnabledLayerNames { get => enabledLayerNames; init { enabledLayerNames = value; } }
		private readonly uint enabledExtensionCount;
		public uint EnabledExtensionCount { get => enabledExtensionCount; init { enabledExtensionCount = value; } }
		private readonly IntPtr enabledExtensionNames;
		[NativeType("const char* const*")]
		public IntPtr EnabledExtensionNames { get => enabledExtensionNames; init { enabledExtensionNames = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAllocationCallbacks {

		public IntPtr UserData;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKAllocationFunction Allocation;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKReallocationFunction Reallocation;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKFreeFunction Free;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKInternalAllocationNotification InternalAllocationNotification;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public VKInternalFreeNotification InternalFreeNotification;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceFeatures {

		public VkBool32 RobustBufferAccess;
		public VkBool32 FullDrawIndexUInt32;
		public VkBool32 ImageCubeArray;
		public VkBool32 IndependentBlend;
		public VkBool32 GeometryShader;
		public VkBool32 TessellationShader;
		public VkBool32 SampleRateShading;
		public VkBool32 DualSrcBlend;
		public VkBool32 LogicOp;
		public VkBool32 MultiDrawIndirect;
		public VkBool32 DrawIndirectFirstInstance;
		public VkBool32 DepthClamp;
		public VkBool32 DepthBiasClamp;
		public VkBool32 FillModeNonSolid;
		public VkBool32 DepthBounds;
		public VkBool32 WideLines;
		public VkBool32 LargePoints;
		public VkBool32 AlphaToOne;
		public VkBool32 MultiViewport;
		public VkBool32 SamplerAnisotropy;
		public VkBool32 TextureCompressionETC2;
		public VkBool32 TextureCompressionASTC_LDR;
		public VkBool32 TextureCompressionBC;
		public VkBool32 OcclusionQueryPrecise;
		public VkBool32 PipelineStatisticsQuery;
		public VkBool32 VertexPipelineStoresAndAtomics;
		public VkBool32 ShaderImageGatherExtended;
		public VkBool32 ShaderStorageImageExtendedFormats;
		public VkBool32 ShaderStorageImageMultisample;
		public VkBool32 ShaderStorageImageReadWithoutFormat;
		public VkBool32 ShaderStorageImageWriteWithoutFormat;
		public VkBool32 ShaderUniformBufferArrayDynamicIndexing;
		public VkBool32 ShaderSampledImageArrayDynamicIndexing;
		public VkBool32 ShaderStorageBufferArrayDynamicIndexing;
		public VkBool32 ShaderStorageImageArrayDynamicIndexing;
		public VkBool32 ShaderClipDistance;
		public VkBool32 ShaderCullDistance;
		public VkBool32 ShaderFloat64;
		public VkBool32 ShaderInt64;
		public VkBool32 ShaderInt16;
		public VkBool32 ShaderResourceResidency;
		public VkBool32 ShaderResourceMinLod;
		public VkBool32 SparseBinding;
		public VkBool32 SparseResidencyBuffer;
		public VkBool32 SparseResidencyImage2D;
		public VkBool32 SparseResidencyImage3D;
		public VkBool32 SparseResidency2Samples;
		public VkBool32 SparseResidency4Samples;
		public VkBool32 SparseResidency8Samples;
		public VkBool32 SparseResidency16Samples;
		public VkBool32 SparseResidencyAliased;
		public VkBool32 VariableMultisampleRate;
		public VkBool32 InheritedQueries;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFormatProperties {

		public VKFormatFeatureFlagBits LinearTilingFeatures;
		public VKFormatFeatureFlagBits OptimalTilingFeatures;
		public VKFormatFeatureFlagBits BufferFeatures;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExtent3D : ITuple3<uint> {

		public uint Width;
		public uint Height;
		public uint Depth;

		public VKExtent3D(uint width, uint height, uint depth) {
			Width = width;
			Height = height;
			Depth = depth;
		}

		public VKExtent3D(IReadOnlyTuple3<uint> tuple) {
			Width = tuple.X;
			Height = tuple.Y;
			Depth = tuple.Z;
		}

		public uint X { get => Width; set => Width = value; }
		public uint Y { get => Height; set => Height = value; }
		public uint Z { get => Depth; set => Depth = value; }

		uint IReadOnlyTuple2<uint>.X => Width;
		uint IReadOnlyTuple2<uint>.Y => Height;
		uint IReadOnlyTuple3<uint>.Z => Depth;

		public uint this[int key] {
			get => key switch {
				0 => Width,
				1 => Height,
				2 => Depth,
				_ => 0
			};
			set {
				switch (key) {
					case 0: Width = value; break;
					case 1: Height = value; break;
					case 2: Depth = value; break;
				}
			}
		}

		uint IReadOnlyIndexer<int, uint>.this[int key] => this[key];

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageFormatProperties {

		public VKExtent3D MaxExtent;
		public uint MaxMipLevels;
		public uint MaxArrayLayers;
		public VKSampleCountFlagBits SampleCounts;
		public VkDeviceSize maxResourceSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceLimits {

		public uint MaxImageDimension1D;
		public uint MaxImageDimension2D;
		public uint MaxImageDimension3D;
		public uint MaxImageDimensionCube;
		public uint MaxImageArrayLayers;
		public uint MaxTexelBufferElements;
		public uint MaxUniformBufferRange;
		public uint MaxStorageBufferRange;
		public uint MaxPushConstantsSize;
		public uint MaxMemoryAllocationCount;
		public uint MaxSamplerAllocationCount;
		public VkDeviceSize BufferImageGranularity;
		public VkDeviceSize SparseAddressSpaceSize;
		public uint MaxBoundDescriptorSets;
		public uint MaxPerStageDescriptorSamplers;
		public uint MaxPerStageDescriptorUniformBuffers;
		public uint MaxPerStageDescriptorStorageBuffers;
		public uint MaxPerStageDescriptorSampledImages;
		public uint MaxPerStageDescriptorStorageImages;
		public uint MaxPerStageDescriptorInputAttachments;
		public uint MaxPerStageResources;
		public uint MaxDescriptorSetSamplers;
		public uint MaxDescriptorSetUniformBuffers;
		public uint MaxDescriptorSetUniformBuffersDynamic;
		public uint MaxDescriptorSetStorageBuffers;
		public uint MaxDescriptorSetStorageBuffersDynamic;
		public uint MaxDescriptorSetSampledImages;
		public uint MaxDescriptorSetStorageImages;
		public uint MaxDescriptorSetInputAttachments;
		public uint MaxVertexInputAttributes;
		public uint MaxVertexInputBindings;
		public uint MaxVertexInputBindingStride;
		public uint MaxVertexOutputComponents;
		public uint MaxTessellationGenerationLevel;
		public uint MaxTessellationPatchSize;
		public uint MaxTessellationControlPerVertexInputComponents;
		public uint MaxTessellationControlPerVertexOutputComponents;
		public uint MaxTessellationControlPerPatchOutputComponents;
		public uint MaxTessellationControlTotalOutputComponents;
		public uint MaxTessellationEvaluationInputComponents;
		public uint MaxTessellationEvaluationOutputComponents;
		public uint MaxGeometryShaderInvocations;
		public uint MaxGeometryInputComponents;
		public uint MaxGeometryOutputComponents;
		public uint MaxGeometryOutputVertices;
		public uint MaxGeometryTotalOutputComponents;
		public uint MaxFragmentInputComponents;
		public uint MaxFragmentOutputAttachments;
		public uint MaxFragmentDualSrcAttachments;
		public uint MaxFragmentCombinedOutputResources;
		public uint MaxComputeSharedMemorySize;
		private uint maxComputeWorkGroupCount0;
		private uint maxComputeWorkGroupCount1;
		private uint maxComputeWorkGroupCount2;
		public Vector3ui MaxComputeWorkGroupCount {
			get => new(maxComputeWorkGroupCount0, maxComputeWorkGroupCount1, maxComputeWorkGroupCount2);
			set {
				maxComputeWorkGroupCount0 = value.X;
				maxComputeWorkGroupCount1 = value.Y;
				maxComputeWorkGroupCount2 = value.Z;
			}
		}
		public uint MaxComputeWorkGroupInvocations;
		private uint maxComputeWorkGroupSize0;
		private uint maxComputeWorkGroupSize1;
		private uint maxComputeWorkGroupSize2;
		public Vector3ui MaxComputeWorkGroupSize {
			get => new(maxComputeWorkGroupSize0, maxComputeWorkGroupSize1, maxComputeWorkGroupSize2);
			set {
				maxComputeWorkGroupSize0 = value.X;
				maxComputeWorkGroupSize1 = value.Y;
				maxComputeWorkGroupSize2 = value.Z;
			}
		}
		public uint SubPixelPrecisionBits;
		public uint SubTexelPrecisionBits;
		public uint MipmapPrecisionBits;
		public uint MaxDrawIndexedIndexValue;
		public uint MaxDrawIndirectCount;
		public float MaxSamplerLodBias;
		public float MaxSamplerAnisotropy;
		public uint MaxViewports;
		private uint maxViewportDimensions0;
		private uint maxViewportDimensions1;
		public Vector2ui MaxViewportDimensions {
			get => new(maxViewportDimensions0, maxViewportDimensions1);
			set {
				maxViewportDimensions0 = value.X;
				maxViewportDimensions1 = value.Y;
			}
		}
		private float viewportBoundsRange0;
		private float viewportBoundsRange1;
		public Vector2 ViewportBoundsRange {
			get => new(viewportBoundsRange0, viewportBoundsRange1);
			set {
				viewportBoundsRange0 = value.X;
				viewportBoundsRange1 = value.Y;
			}
		}
		public uint ViewportSubPixelBits;
		public nuint MinMemoryMapAlignment;
		public VkDeviceSize MinTexelBufferOffsetAlignment;
		public VkDeviceSize MinUniformBufferOffseAlignment;
		public VkDeviceSize MinStorageBufferOffsetAlignment;
		public int MinTexelOffset;
		public uint MaxTexelOffset;
		public int MinTexelGatherOffset;
		public uint MaxTexelGatherOffset;
		public float MinInterpolationOffset;
		public float MaxInterpolationOffset;
		public uint SubPixelInterpolationOffsetBits;
		public uint MaxFramebufferWidth;
		public uint MaxFramebufferHeight;
		public uint MaxFramebufferLayers;
		public VKSampleCountFlagBits FramebufferColorSampleCounts;
		public VKSampleCountFlagBits FramebufferDepthSampleCounts;
		public VKSampleCountFlagBits FramebufferStencilSampleCounts;
		public VKSampleCountFlagBits FramebufferNoAttachmentsSampleCounts;
		public uint MaxColorAttachments;
		public VKSampleCountFlagBits SampledImageColorSampleCounts;
		public VKSampleCountFlagBits SampledImageIntegerSampleCounts;
		public VKSampleCountFlagBits SampledImageDepthSampleCounts;
		public VKSampleCountFlagBits SampledImageStencilSampleCounts;
		public VKSampleCountFlagBits StorageImageSampleCounts;
		public uint MaxSampleMaskWords;
		public VkBool32 TimestampComputeAndGraphics;
		public float TimestampPeriod;
		public uint MaxClipDistances;
		public uint MaxCullDistances;
		public uint MaxCombinedClipAndCullDistances;
		public uint DiscreteQueuePriorities;
		private float pointSizeRange0;
		private float pointSizeRange1;
		public Vector2 PointSizeRange {
			get => new(pointSizeRange0, pointSizeRange1);
			set {
				pointSizeRange0 = value.X;
				pointSizeRange1 = value.Y;
			}
		}
		private float lineWidthRange0;
		private float lineWidthRange1;
		public Vector2 LineWidthRange {
			get => new(lineWidthRange0, lineWidthRange1);
			set {
				lineWidthRange0 = value.X;
				lineWidthRange1 = value.Y;
			}
		}
		public float PointSizeGranularity;
		public float LineWidthGranularity;
		public VkBool32 StrictLines;
		public VkBool32 StandardSampleLocations;
		public VkDeviceSize OptimalBufferCopyOffsetAlignment;
		public VkDeviceSize OptimalBufferCopyRowPitchAlignment;
		public VkDeviceSize NonCoherentAtomSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSparseProperties {

		public VkBool32 ResidencyStandard2DBlockShape;
		public VkBool32 ResidencyStandard2DMultisampleBlockShape;
		public VkBool32 ResidencyStandard3DBlockShape;
		public VkBool32 ResidencyAlignedMipSize;
		public VkBool32 ResidencyNonResidentStrict;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProperties {

		public uint APIVersion;
		public uint DriverVersion;
		public uint VendorID;
		public uint DeviceID;
		public VKPhysicalDeviceType DeviceType;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxPhysicalDeviceNameSize)]
		private readonly byte[] deviceName;
		public string DeviceName => MemoryUtil.GetStringUTF8(deviceName);
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.UUIDSize)]
		private readonly byte[] pipelineCacheUUID;
		public ReadOnlySpan<byte> PipelineCacheUUID => new(pipelineCacheUUID);
		public VKPhysicalDeviceLimits Limits;
		public VKPhysicalDeviceSparseProperties SparseProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKQueueFamilyProperties {

		public VKQueueFlagBits QueueFlags;
		public uint QueueCount;
		public uint TimestampValidBits;
		public VKExtent3D MinImageTransferGranularity;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryType {

		public VKMemoryPropertyFlagBits PropertyFlags;
		public uint HeapIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryHeap {

		public VkDeviceSize Size;
		public VKMemoryHeapFlagBits Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMemoryProperties {

		public uint MemoryTypeCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryTypes)]
		private readonly VKMemoryType[] memoryTypes;
		public ReadOnlySpan<VKMemoryType> MemoryTypes => new(memoryTypes);
		public uint MemoryHeapCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryHeaps)]
		private readonly VKMemoryHeap[] memoryHeaps;
		public ReadOnlySpan<VKMemoryHeap> MemoryHeaps => new(memoryHeaps);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceQueueCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDeviceQueueCreateFlagBits Flags;
		public uint QueueFamilyIndex;
		public uint QueueCount;
		[NativeType("const float*")]
		public IntPtr QueuePriorities;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDeviceCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKDeviceCreateFlagBits flags;
		public VKDeviceCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly uint queueCreateInfoCount;
		public uint QueueCreateInfoCount { get => queueCreateInfoCount; init { queueCreateInfoCount = value; } }
		private readonly IntPtr queueCreateInfos;
		[NativeType("const VkDeviceQueueCreateInfo*")]
		public IntPtr QueueCreateInfos { get => queueCreateInfos; init { queueCreateInfos = value; } }
		private readonly uint enabledLayerCount;
		public uint EnabledLayerCount { get => enabledLayerCount; init { enabledLayerCount = value; } }
		private readonly IntPtr enabledLayerNames;
		[NativeType("const char* const*")]
		public IntPtr EnabledLayerNames { get => enabledLayerNames; init { enabledLayerNames = value; } }
		private readonly uint enabledExtensionCount;
		public uint EnabledExtensionCount { get => enabledExtensionCount; init { enabledExtensionCount = value; } }
		private readonly IntPtr enabledExtensionNames;
		[NativeType("const char* const*")]
		public IntPtr EnabledExtensionNames { get => enabledExtensionNames; init { enabledExtensionNames = value; } }
		private readonly IntPtr enabledFeatures;
		[NativeType("const VkPhysicalDeviceFeatures*")]
		public IntPtr EnabledFeatures { get => enabledFeatures; init { enabledFeatures = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExtensionProperties {

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxExtensionNameSize)]
		private readonly byte[] extensionName;
		public string ExtensionName => MemoryUtil.GetStringUTF8(extensionName);
		public uint SpecVersion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKLayerProperties {

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxExtensionNameSize)]
		private readonly byte[] layerName;
		public string LayerName => MemoryUtil.GetStringUTF8(layerName);
		public uint SpecVersion;
		public uint ImplementationVersion;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxDescriptionSize)]
		private readonly byte[] description;
		public string Description => MemoryUtil.GetStringUTF8(description);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint WaitSemaphoreCount;
		[NativeType("const VkSemaphore*")]
		public IntPtr WaitSemaphores;
		[NativeType("const VkPipelineStageFlags*")]
		public IntPtr WaitDstStageMask;
		public uint CommandBufferCount;
		[NativeType("const VkCommandBuffer*")]
		public IntPtr CommandBuffers;
		public uint SignalSemaphoreCount;
		[NativeType("const VkSemaphore*")]
		public IntPtr SignalSemaphores;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKMemoryAllocateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VkDeviceSize allocationSize;
		public VkDeviceSize AllocationSize { get => allocationSize; init { allocationSize = value; } }
		private readonly uint memoryTypeIndex;
		public uint MemoryTypeIndex { get => memoryTypeIndex; init { memoryTypeIndex = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMappedMemoryRange {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceMemory Memory;
		public VkDeviceSize Offset;
		public VkDeviceSize Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryRequirements {

		public VkDeviceSize Size;
		public VkDeviceSize Alignment;
		public uint MemoryTypeBits;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageFormatProperties {

		public VKImageAspectFlagBits AspectMask;
		public VKExtent3D ImageGranularity;
		public VKSparseImageFormatFlagBits Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageMemoryRequirements {

		public VKSparseImageFormatProperties FormatProperties;
		public uint ImageMipTailFirstLod;
		public VkDeviceSize ImageMipTailSize;
		public VkDeviceSize ImageMipTailOffset;
		public VkDeviceSize ImageMipTailStride;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseMemoryBind {

		public VkDeviceSize ResourceOffset;
		public VkDeviceSize Size;
		public VkDeviceMemory Memory;
		public VkDeviceSize MemoryOffset;
		public VKSparseMemoryBindFlagBits Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseBufferMemoryBindInfo {

		public VkBuffer Buffer;
		public uint BindCount;
		[NativeType("const VkSparseMemoryBind*")]
		public IntPtr Binds;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageOpaqueMemoryBindInfo {

		public VkImage Image;
		public uint BindCount;
		[NativeType("const VkSparseMemoryBind*")]
		public IntPtr Binds;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageSubresource {

		public VKImageAspectFlagBits AspectMask;
		public uint MipLevel;
		public uint ArrayLayer;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKOffset3D : ITuple3<int> {

		public int X;
		public int Y;
		public int Z;

		public VKOffset3D(int x, int y, int z) {
			X = x;
			Y = y;
			Z = z;
		}

		public VKOffset3D(IReadOnlyTuple3<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
			Z = tuple.Z;
		}

		public int this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				2 => Z,
				_ => 0
			};
			set => throw new NotImplementedException();
		}

		int IReadOnlyIndexer<int, int>.this[int key] => this[key];

		int ITuple2<int>.X { get => X; set => X = value; }

		int ITuple2<int>.Y { get => Y; set => Y = value; }

		int ITuple3<int>.Z { get => Z; set => Z = value; }

		int IReadOnlyTuple2<int>.X => X;

		int IReadOnlyTuple2<int>.Y => Y;

		int IReadOnlyTuple3<int>.Z => Z;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageMemoryBind {

		public VKImageSubresource Subresource;
		public VKOffset3D Offset;
		public VKExtent3D Extent;
		public VkDeviceMemory Memory;
		public VkDeviceSize MemoryOffset;
		public VKSparseMemoryBindFlagBits Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageMemoryBindInfo {

		public VkImage Image;
		public uint BindCount;
		[NativeType("const VkSparseImageMemoryBind*")]
		public IntPtr Binds;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindSparseInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint WaitSemaphoreCount;
		[NativeType("const VkSemaphore*")]
		public IntPtr WaitSemaphores;
		public uint BufferBindCount;
		[NativeType("const VkSparseBufferMemoryBindInfo*")]
		public IntPtr BufferBinds;
		public uint ImageOpaqueBindCount;
		[NativeType("const VkSparseImageOpaqueMemoryBindInfo*")]
		public IntPtr ImageOpaqueBinds;
		public uint ImageBindCount;
		[NativeType("const VkSparseImageMemoryBindInfo*")]
		public IntPtr ImageBinds;
		public uint SignalSemaphoreCount;
		[NativeType("const VkSemaphore*")]
		public IntPtr SignalSemaphores;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKFenceCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKFenceCreateFlagBits flags;
		public VKFenceCreateFlagBits Flags { get => flags; init { flags = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKSemaphoreCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKSemaphoreCreateFlagBits flags;
		public VKSemaphoreCreateFlagBits Flags { get => flags; init { flags = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKEventCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKEventCreateFlagBits flags;
		public VKEventCreateFlagBits Flags { get => flags; init { flags = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKQueryPoolCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKQueryPoolCreateFlagBits flags;
		public VKQueryPoolCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VKQueryType queryType;
		public VKQueryType QueryType { get => queryType; init { queryType = value; } }
		private readonly uint queryCount;
		public uint QueryCount { get => queryCount; init { queryCount = value; } }
		private readonly VKQueryPipelineStatisticFlagBits pipelineStatistics;
		public VKQueryPipelineStatisticFlagBits PipelineStatistics { get => pipelineStatistics; init { pipelineStatistics = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKBufferCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKBufferCreateFlagBits flags;
		public VKBufferCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VkDeviceSize size;
		public VkDeviceSize Size { get => size; init { size = value; } }
		private readonly VKBufferUsageFlagBits usage;
		public VKBufferUsageFlagBits Usage { get => usage; init { usage = value; } }
		private readonly VKSharingMode sharingMode;
		public VKSharingMode SharingMode { get => sharingMode; init { sharingMode = value; } }
		private readonly uint queueFamilyIndexCount;
		public uint QueueFamilyIndexCount { get => queueFamilyIndexCount; init { queueFamilyIndexCount = value; } }
		private readonly IntPtr queueFamilyIndices;
		[NativeType("const uint32_t*")]
		public IntPtr QueueFamilyIndices { get => queueFamilyIndices; init { queueFamilyIndices = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKBufferViewCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKBufferViewCreateFlagBits flags;
		public VKBufferViewCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VkBuffer buffer;
		public VkBuffer Buffer { get => buffer; init { buffer = value; } }
		private readonly VKFormat format;
		public VKFormat Format { get => format; init { format = value; } }
		private readonly VkDeviceSize offset;
		public VkDeviceSize Offset { get => offset; init { offset = value; } }
		private readonly VkDeviceSize range;
		public VkDeviceSize Range { get => range; init { range = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKImageCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKImageCreateFlagBits flags;
		public VKImageCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VKImageType imageType;
		public VKImageType ImageType { get => imageType; init { imageType = value; } }
		private readonly VKFormat format;
		public VKFormat Format { get => format; init { format = value; } }
		private readonly VKExtent3D extent;
		public VKExtent3D Extent { get => extent; init { extent = value; } }
		private readonly uint mipLevels;
		public uint MipLevels { get => mipLevels; init { mipLevels = value; } }
		private readonly uint arrayLayers;
		public uint ArrayLayers { get => arrayLayers; init { arrayLayers = value; } }
		private readonly VKSampleCountFlagBits samples;
		public VKSampleCountFlagBits Samples { get => samples; init { samples = value; } }
		private readonly VKImageTiling tiling;
		public VKImageTiling Tiling { get => tiling; init { tiling = value; } }
		private readonly VKImageUsageFlagBits usage;
		public VKImageUsageFlagBits Usage { get => usage; init { usage = value; } }
		private readonly VKSharingMode sharingMode;
		public VKSharingMode SharingMode { get => sharingMode; init { sharingMode = value; } }
		private readonly uint queueFamilyIndexCount;
		public uint QueueFamilyIndexCount { get => queueFamilyIndexCount; init { queueFamilyIndexCount = value; } }
		private readonly IntPtr queueFamilyIndices;
		[NativeType("const uint32_t*")]
		public IntPtr QueueFamilyIndices { get => queueFamilyIndices; init { queueFamilyIndices = value; } }
		private readonly VKImageLayout initialLayout;
		public VKImageLayout InitialLayout { get => initialLayout; init { initialLayout = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubresourceLayout {

		public VkDeviceSize Offset;
		public VkDeviceSize Size;
		public VkDeviceSize RowPitch;
		public VkDeviceSize ArrayPitch;
		public VkDeviceSize DepthPitch;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKComponentMapping {

		public VKComponentSwizzle R;
		public VKComponentSwizzle G;
		public VKComponentSwizzle B;
		public VKComponentSwizzle A;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageSubresourceRange {

		public VKImageAspectFlagBits AspectMask;
		public uint BaseMipLevel;
		public uint LevelCount;
		public uint BaseArrayLayer;
		public uint LayerCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKImageViewCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next; 
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKImageViewCreateFlagBits flags;
		public VKImageViewCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VkImage image;
		public VkImage Image { get => image; init { image = value; } }
		private readonly VKImageViewType viewType;
		public VKImageViewType ViewType { get => viewType; init { viewType = value; } }
		private readonly VKFormat format;
		public VKFormat Format { get => format; init { format = value; } }
		private readonly VKComponentMapping components;
		public VKComponentMapping Components { get => components; init { components = value; } }
		private readonly VKImageSubresourceRange subresourceRange;
		public VKImageSubresourceRange SubresourceRange { get => subresourceRange; init { subresourceRange = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKShaderModuleCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKShaderModuleCreateFlagBits flags;
		public VKShaderModuleCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly nuint codeSize;
		public nuint CodeSize { get => codeSize; init { codeSize = value; } }
		private readonly IntPtr code;
		[NativeType("const uint32_t*")]
		public IntPtr Code { get => code; init { code = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKPipelineCacheCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKPipelineCacheCreateFlagBits flags;
		public VKPipelineCacheCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly nuint initialDataSize;
		public nuint InitialDataSize { get => initialDataSize; init { initialDataSize = value; } }
		private readonly IntPtr initialData;
		public IntPtr InitialData { get => initialData; init { initialData = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSpecializationMapEntry {

		public uint ConstantID;
		public uint Offset;
		public nuint Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSpecializationInfo {

		public uint MapEntryCount;
		[NativeType("const VkSpecializationMapEntry*")]
		public IntPtr MapEntries;
		public nuint DataSize;
		public IntPtr Data;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineShaderStageCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineSHaderStageCreateFlagBits Flags;
		public VKShaderStageFlagBits Stage;
		public VkShaderModule Module;
		[MarshalAs(UnmanagedType.LPStr)]
		public string Name;
		[NativeType("const VkSpecializationInfo*")]
		public IntPtr SpecializationInfo;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKVertexInputBindingDescription {

		public uint Binding;
		public uint Stride;
		public VKVertexInputRate InputRate;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKVertexInputAttributeDescription {

		public uint Location;
		public uint Binding;
		public VKFormat Format;
		public uint Offset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineVertexInputStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineVertexInputStateCreateFlagBits Flags;
		public uint VertexBindingDescriptionCount;
		[NativeType("const VkVertexInputBindingDescription*")]
		public IntPtr VertexBindingDescriptions;
		public uint VertexAttributeDescriptionCount;
		[NativeType("const VkVertexInputAttributeDescription*")]
		public IntPtr VertexAttributeDescriptions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineInputAssemblyStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineInputAssemblyStateCreateFlagBits Flags;
		public VKPrimitiveTopology Topology;
		public VkBool32 PrimitiveRestartEnable;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineTessellationStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineTessellationStateCreateFlagBits Flags;
		public uint PatchControlPoints;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKViewport {

		public float X;
		public float Y;
		public float Width;
		public float Height;
		public float MinDepth;
		public float MaxDepth;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKOffset2D : ITuple2<int> {

		public int X;
		public int Y;

		int ITuple2<int>.X { get => X; set => X = value; }
		int ITuple2<int>.Y { get => Y; set => Y = value; }

		int IReadOnlyTuple2<int>.X => X;
		int IReadOnlyTuple2<int>.Y => Y;

		public VKOffset2D(int x, int y) {
			X = x;
			Y = y;
		}

		public VKOffset2D(IReadOnlyTuple2<int> tuple) {
			X = tuple.X;
			Y = tuple.Y;
		}

		int IReadOnlyIndexer<int, int>.this[int key] => this[key];

		public int this[int key] {
			get => key switch {
				0 => X,
				1 => Y,
				_ => 0
			};
			set {
				switch (key) {
					case 0: X = value; break;
					case 1: Y = value; break;
				}
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExtent2D : ITuple2<uint> {

		public uint Width;
		public uint Height;

		public uint X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public uint Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		uint IReadOnlyTuple2<uint>.X => throw new NotImplementedException();

		uint IReadOnlyTuple2<uint>.Y => throw new NotImplementedException();

		public VKExtent2D(uint width, uint height) {
			Width = width;
			Height = height;
		}

		public VKExtent2D(IReadOnlyTuple2<uint> tuple) {
			Width = tuple.X;
			Height = tuple.Y;
		}

		uint IReadOnlyIndexer<int, uint>.this[int key] => this[key];

		public uint this[int key] {
			get => key switch {
				0 => Width,
				1 => Height,
				_ => 0
			};
			set {
				switch (key) {
					case 0: Width = value; break;
					case 1: Height = value; break;
				}
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRect2D {

		public VKOffset2D Offset;
		public VKExtent2D Extent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineViewportStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint ViewportCount;
		[NativeType("const VkViewport*")]
		public IntPtr Viewports;
		public uint ScissorCount;
		[NativeType("const VkRect2D*")]
		public IntPtr Scissors;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineRasterizationStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineRasterizationStateCreateFlagBits Flags;
		public VkBool32 DepthClampEnable;
		public VkBool32 RasterizerDiscardEnable;
		public VKPolygonMode PolygonMode;
		public VKCullModeFlagBits CullMode;
		public VKFrontFace FrontFace;
		public VkBool32 DepthBiasEnable;
		public float DepthBiasConstantFactor;
		public float DepthBiasClamp;
		public float DepthBiasSlopeFactor;
		public float LineWidth;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineMultisampleStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineMultisampleStateCreateFlagBits Flags;
		public VKSampleCountFlagBits RasterizationSamples;
		public VkBool32 SampleShadingEnable;
		public float MinSampleShading;
		[NativeType("const VkSampleMask*")]
		public IntPtr SampleMask;
		public VkBool32 AlphaToCoverageEnable;
		public VkBool32 AlphaToOneEnable;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKStencilOpState {

		public VKStencilOp FailOp;
		public VKStencilOp PassOp;
		public VKStencilOp DepthFailOp;
		public VKCompareOp CompareOp;
		public uint CompareMask;
		public uint WriteMask;
		public uint Reference;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineDepthStencilStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineDepthStencilStateCreateFlagBits Flags;
		public VkBool32 DepthTestEnable;
		public VkBool32 DepthWriteEnable;
		public VKCompareOp DepthCompareOp;
		public VkBool32 DepthBoundsTestEnable;
		public VkBool32 StencilTestEnable;
		public VKStencilOpState Front;
		public VKStencilOpState Back;
		public float MinDepthBounds;
		public float MaxDepthBounds;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineColorBlendAttachmentState {

		public VkBool32 BlendEnable;
		public VKBlendFactor SrcColorBlendFactor;
		public VKBlendFactor DstColorBlendFactor;
		public VKBlendOp ColorBlendOp;
		public VKBlendFactor SrcAlphaBlendFactor;
		public VKBlendFactor DstAlphaBlendFactor;
		public VKBlendOp AlphaBlendOp;
		public VKColorComponentFlagBits ColorWriteMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineColorBlendStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineColorBlendStateCreateFlagBits Flags;
		public VkBool32 LogicOpEnable;
		public VKLogicOp LogicOp;
		public uint AttachmentCount;
		[NativeType("const VkPipelineColorBlendAttachmentState*")]
		public IntPtr Attachment;
		private float blendConstants0;
		private float blendConstants1;
		private float blendConstants2;
		private float blendConstants3;
		public Vector4 BlendConstants {
			get => new(blendConstants0, blendConstants1, blendConstants2, blendConstants3);
			set {
				blendConstants0 = value.X;
				blendConstants1 = value.Y;
				blendConstants2 = value.Z;
				blendConstants3 = value.W;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineDynamicStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineDynamicStateCreateFlagBits Flags;
		public uint DynamicStateCount;
		[NativeType("const VkDynamicState*")]
		public IntPtr DynamicStates;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKGraphicsPipelineCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKPipelineCreateFlagBits flags;
		public VKPipelineCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly uint stageCount;
		public uint StageCount { get => stageCount; init { stageCount = value; } }
		private readonly IntPtr stages;
		[NativeType("const VkPipelineShaderStageCreateInfo*")]
		public IntPtr Stages { get => stages; init { stages = value; } }
		private readonly IntPtr vertexInputState;
		[NativeType("const VkPipelineVertexInputStateCreateInfo*")]
		public IntPtr VertexInputState { get => vertexInputState; init { vertexInputState = value; } }
		private readonly IntPtr inputAssemblyState;
		[NativeType("const VkPipelineInputAssemblyStateCreateInfo*")]
		public IntPtr InputAssemblyState { get => inputAssemblyState; init { inputAssemblyState = value; } }
		private readonly IntPtr tessellationState;
		[NativeType("const VkPipelineTessellationStateCreateInfo*")]
		public IntPtr TessellationState { get => tessellationState; init { tessellationState = value; } }
		private readonly IntPtr viewportState;
		[NativeType("const VkPipelineViewportStateCreateInfo*")]
		public IntPtr ViewportState { get => viewportState; init { viewportState = value; } }
		private readonly IntPtr rasterizationState;
		[NativeType("const VkPipelineRasterizationStateCreateInfo*")]
		public IntPtr RasterizationState { get => rasterizationState; init { rasterizationState = value; } }
		private readonly IntPtr multisampleState;
		[NativeType("const VkPipelineMultisampleStateCreateInfo*")]
		public IntPtr MultisampleState { get => multisampleState; init { multisampleState = value; } }
		private readonly IntPtr depthStencilState;
		[NativeType("const VkPipelineDepthStencilStateCreateInfo*")]
		public IntPtr DepthStencilState { get => depthStencilState; init { depthStencilState = value; } }
		private readonly IntPtr colorBlendState;
		[NativeType("const VkPipelineColorBlendStateCreateInfo*")]
		public IntPtr ColorBlendState { get => colorBlendState; init { colorBlendState = value; } }
		private readonly IntPtr dynamicState;
		[NativeType("const VkPipelineDynamicStateCreateInfo*")]
		public IntPtr DynamicState { get => dynamicState; init { dynamicState = value; } }
		private readonly VkPipelineLayout layout;
		public VkPipelineLayout Layout { get => layout; init { layout = value; } }
		private readonly VkRenderPass renderPass;
		public VkRenderPass RenderPass { get => renderPass; init { renderPass = value; } }
		private readonly uint subpass;
		public uint Subpass { get => subpass; init { subpass = value; } }
		private readonly VkPipeline basePipelineHandle;
		public VkPipeline BasePipelineHandle { get => basePipelineHandle; init { basePipelineHandle = value; } }
		private readonly int basePipelineIndex;
		public int BasePipelineIndex { get => basePipelineIndex; init { basePipelineIndex = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKComputePipelineCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKPipelineCreateFlagBits flags;
		public VKPipelineCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VKPipelineShaderStageCreateInfo stage;
		public VKPipelineShaderStageCreateInfo Stage { get => stage; init { stage = value; } }
		private readonly VkPipelineLayout layout;
		public VkPipelineLayout Layout { get => layout; init { layout = value; } }
		private readonly VkPipeline basePipelineHandle;
		public VkPipeline BasePipelineHandle { get => basePipelineHandle; init { basePipelineHandle = value; } }
		private readonly int basePipelineIndex;
		public int BasePipelineIndex { get => basePipelineIndex; init { basePipelineIndex = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPushConstantRange {

		public VKShaderStageFlagBits StageFlags;
		public uint Offset;
		public uint Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKPipelineLayoutCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKPipelineLayoutCreateFlagBits flags;
		public VKPipelineLayoutCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly uint setLayoutCount;
		public uint SetLayoutCount { get => setLayoutCount; init { setLayoutCount = value; } }
		private readonly IntPtr setLayouts;
		[NativeType("const VkDescriptorSetLayout*")]
		public IntPtr SetLayouts { get => setLayouts; init { setLayouts = value; } }
		private readonly uint pushConstantRangeCount;
		public uint PushConstantRangeCount { get => pushConstantRangeCount; init { pushConstantRangeCount = value; } }
		private readonly IntPtr pushConstantRanges;
		[NativeType("const VkPushConstantRange*")]
		public IntPtr PushConstantRanges { get => pushConstantRanges; init { pushConstantRanges = value; } }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKSamplerCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init { type = value; } }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init { next = value; } }
		private readonly VKSamplerCreateFlagBits flags;
		public VKSamplerCreateFlagBits Flags { get => flags; init { flags = value; } }
		private readonly VKFilter magFilter;
		public VKFilter MagFilter { get => magFilter; init { magFilter = value; } }
		private readonly VKFilter minFilter;
		public VKFilter MinFilter { get => minFilter; init { minFilter = value; } }
		private readonly VKSamplerMipmapMode mipmapMode;
		public VKSamplerMipmapMode MipmapMode { get => MipmapMode; init { mipmapMode = value; } }
		private readonly VKSamplerAddressMode addressModeU;
		public VKSamplerAddressMode AddressModeU { get => addressModeU; init { addressModeU = value; } }
		private readonly VKSamplerAddressMode addressModeV;
		public VKSamplerAddressMode AddressModeV { get => addressModeV; init { addressModeV = value; } }
		private readonly VKSamplerAddressMode addressModeW;
		public VKSamplerAddressMode AddressModeW { get => addressModeW; init { addressModeW = value; } }
		private readonly float mipLodBias;
		public float MipLodBias { get => mipLodBias; init => mipLodBias = value; }
		private readonly VkBool32 anisotropyEnable;
		public bool AnisotropyEnable { get => anisotropyEnable; init => anisotropyEnable = value; }
		private readonly float maxAnisotropy;
		public float MaxAnisotropy { get => maxAnisotropy; init => maxAnisotropy = value; }
		private readonly VkBool32 compareEnable;
		public VkBool32 CompareEnable { get => compareEnable; init => compareEnable = value; }
		private readonly VKCompareOp compareOp;
		public VKCompareOp CompareOp { get => compareOp; init => compareOp = value; }
		private readonly float minLod;
		public float MinLod { get => minLod; init => minLod = value; }
		private readonly float maxLod;
		public float MaxLod { get => maxLod; init => maxLod = value; }
		private readonly VKBorderColor borderColor;
		public VKBorderColor BorderColor { get => borderColor; init => borderColor = value; }
		private readonly VkBool32 unnormalizedCoordinates;
		public VkBool32 UnnormalizedCoordinates { get => unnormalizedCoordinates; init => unnormalizedCoordinates = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetLayoutBinding {

		public uint Binding;
		public VKDescriptorType DescriptorType;
		public uint DescriptorCount;
		public VKShaderStageFlagBits StageFlags;
		[NativeType("const VkSampler*")]
		public IntPtr ImmutableSamplers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetLayoutCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDescriptorSetLayoutCreateFlagBits Flags;
		public uint BindingCount;
		[NativeType("const VkDescriptorSetLayoutBinding*")]
		public IntPtr Bindings;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorPoolSize {

		public VKDescriptorType Type;
		public uint DescriptorType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorPoolCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDescriptorPoolCreateFlagBits Flags;
		public uint MaxSets;
		public uint PoolSizeCount;
		[NativeType("const VkDescriptorPoolSize*")]
		public IntPtr PoolSizes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDescriptorPool DescriptorPool;
		public uint DescriptorSetCount;
		[NativeType("const VkDescriptorSetLayout*")]
		public IntPtr SetLayouts;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorImageInfo {

		public VkSampler Sampler;
		public VkImageView ImageView;
		public VKImageLayout ImageLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorBufferInfo {

		public VkBuffer Buffer;
		public VkDeviceSize Offset;
		public VkDeviceSize Range;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKWriteDescriptorSet {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDescriptorSet DstSet;
		public uint DstBinding;
		public uint DstArrayElement;
		public uint DescriptorCount;
		public VKDescriptorType DescriptorType;
		[NativeType("const VkDescriptorImageInfo*")]
		public IntPtr ImageInfo;
		[NativeType("const VkDescriptorBufferInfo*")]
		public IntPtr BufferInfo;
		[NativeType("const VkBufferView*")]
		public IntPtr TexelBufferView;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyDescriptorSet {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDescriptorSet SrcSet;
		public uint SrcBinding;
		public uint SrcArrayElement;
		public VkDescriptorSet DstSet;
		public uint DstBinding;
		public uint DstArrayElement;
		public uint DescriptorCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFramebufferCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKFramebufferCreateFlagBits Flags;
		public VkRenderPass RenderPass;
		public uint AttachmentCount;
		[NativeType("const VkImageView*")]
		public IntPtr Attachments;
		public uint Width;
		public uint Height;
		public uint Layers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentDescription {

		public VKAttachmentDescriptionFlagBits Flags;
		public VKFormat Format;
		public VKSampleCountFlagBits Samples;
		public VKAttachmentLoadOp LoadOp;
		public VKAttachmentStoreOp StoreOp;
		public VKAttachmentLoadOp StencilLoadOp;
		public VKAttachmentStoreOp StencilStoreOp;
		public VKImageLayout InitialLayout;
		public VKImageLayout FinalLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentReference {

		public uint Attachment;
		public VKImageLayout Layout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassDescription {

		public VKSubpassDescriptionFlagBits Flags;
		public VKPipelineBindPoint PipelineBindPoint;
		public uint InputAttachmentCount;
		[NativeType("const VkAttachmentReference*")]
		public IntPtr InputAttachments;
		public uint ColorAttachmentCount;
		[NativeType("const VkAttachmentReference*")]
		public IntPtr ColorAttachments;
		[NativeType("const VkAttachmentReference*")]
		public IntPtr ResolveAttachments;
		[NativeType("const VkAttachmentReference*")]
		public IntPtr DepthStencilAttachment;
		public uint PreserveAttachmentCount;
		[NativeType("const VkAttachmentReference*")]
		public IntPtr PreserveAttachments;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassDependency {

		public uint SrcSubpass;
		public uint DstSubpass;
		public VKPipelineStageFlagBits SrcStageMask;
		public VKPipelineStageFlagBits DstStageMask;
		public VKAccessFlagBits SrcAccessMask;
		public VKAccessFlagBits DstAccessMask;
		public VKDependencyFlagBits DependencyFlags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKRenderPassCreateFlagBits Flags;
		public uint AttachmentCount;
		[NativeType("const VkAttachmentDescription*")]
		public IntPtr Attachments;
		public uint SubpassCount;
		[NativeType("const VkSubpassDescription*")]
		public IntPtr Subpasses;
		public uint DependencyCount;
		[NativeType("const VkSubpassDependency*")]
		public IntPtr Dependencies;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandPoolCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKCommandPoolCreateFlagBits Flags;
		public uint QueueFamilyIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkCommandPool CommandPool;
		public VKCommandBufferLevel Level;
		public uint CommandBufferCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferInheritanceInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkRenderPass RenderPass;
		public uint Subpass;
		public VkFramebuffer Framebuffer;
		public VkBool32 OcclusionQueryEnable;
		public VKQueryControlFlagBits QueryFlags;
		public VKQueryPipelineStatisticFlagBits PipelineStatistics;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKCommandBufferUsageFlagBits Flags;
		[NativeType("const VkCommandBufferInheritanceInfo*")]
		public IntPtr InheritanceInfo;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferCopy {

		public VkDeviceSize SrcOffset;
		public VkDeviceSize DstOffset;
		public VkDeviceSize Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageSubresourceLayers {

		public VKImageAspectFlagBits AspectMask;
		public uint MipLevel;
		public uint BaseArrayLayer;
		public uint LayerCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageCopy {

		public VKImageSubresourceLayers SrcSubresource;
		public VKOffset3D SrcOffset;
		public VKImageSubresourceLayers DstSubresource;
		public VKOffset3D DstOffset;
		public VKExtent3D Extent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageBlit {

		public VKImageSubresourceLayers SrcSubresource;
		private VKOffset3D srcOffsets0;
		private VKOffset3D srcOffsets1;
		public Tuple2<VKOffset3D> SrcOffsets {
			get => new(srcOffsets0, srcOffsets1);
			set {
				srcOffsets0 = value.X;
				srcOffsets1 = value.Y;
			}
		}
		public VKImageSubresourceLayers DstSubresource;
		private VKOffset3D dstOffsets0;
		private VKOffset3D dstOffsets1;
		public Tuple2<VKOffset3D> DstOffsets {
			get => new(dstOffsets0, dstOffsets1);
			set {
				dstOffsets0 = value.X;
				dstOffsets1 = value.Y;
			}
		}

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferImageCopy {

		public VkDeviceSize BufferOffset;
		public uint BufferRowLength;
		public uint BufferImageHeight;
		public VKImageSubresourceLayers ImageSubresource;
		public VKOffset3D ImageOffset;
		public VKExtent3D ImageExtent;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct VKClearColorValue {

		[FieldOffset(0)]
		public Vector4 Float32;
		[FieldOffset(0)]
		public Vector4i Int32;
		[FieldOffset(0)]
		public Vector4ui UInt32;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKClearDepthStencilValue {

		public float Depth;
		public uint Stencil;

	}

	[StructLayout(LayoutKind.Explicit)]
	public struct VKClearValue {

		[FieldOffset(0)]
		public VKClearColorValue Color;
		[FieldOffset(0)]
		public VKClearDepthStencilValue DepthStencil;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKClearAttachment {

		public VKImageAspectFlagBits AspectMask;
		public uint ColorAttachment;
		public VKClearValue ClearValue;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKClearRect {

		public VKRect2D Rect;
		public uint BaseArrayLayer;
		public uint LayerCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageResolve {

		public VKImageSubresourceLayers SrcSubresource;
		public VKOffset3D SrcOffset;
		public VKImageSubresourceLayers DstSubresource;
		public VKOffset3D DstOffset;
		public VKExtent3D Extent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryBarrier {

		public VKStructureType Type;
		public IntPtr Next;
		public VKAccessFlagBits SrcAccessMask;
		public VKAccessFlagBits DstAccessMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferMemoryBarrier {

		public VKStructureType Type;
		public IntPtr Next;
		public VKAccessFlagBits SrcAccessMask;
		public VKAccessFlagBits DstAccessMask;
		public uint SrcQueueFamilyIndex;
		public uint DstQueueFamilyIndex;
		public VkBuffer Buffer;
		public VkDeviceSize Offset;
		public VkDeviceSize Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageMemoryBarrier {

		public VKStructureType Type;
		public IntPtr Next;
		public VKAccessFlagBits SrcAccessMask;
		public VKAccessFlagBits DstAccessMask;
		public VKImageLayout OldLayout;
		public VKImageLayout NewLayout;
		public uint SrcQueueFamilyIndex;
		public uint DstQueueFamilyIndex;
		public VkImage Image;
		public VKImageSubresourceRange SubresourceRange;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkRenderPass RenderPass;
		public VkFramebuffer Framebuffer;
		public VKRect2D RenderArea;
		public uint ClearValueCount;
		[NativeType("const VkClearValue*")]
		public IntPtr ClearValues;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDispatchIndirectCommand {

		public uint X;
		public uint Y;
		public uint Z;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDrawIndexedIndirectCommand {

		public uint IndexCount;
		public uint InstanceCount;
		public uint FirstIndex;
		public int VertexOffset;
		public uint FirstInstance;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDrawIndirectCommand {

		public uint VertexCount;
		public uint InstanceCount;
		public uint FirstVertex;
		public uint FirstInstance;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBaseOutStructure {

		public VKStructureType Type;
		[NativeType("const VkBaseOutStructure*")]
		public IntPtr Next;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBaseInStructure {

		public VKStructureType Type;
		[NativeType("const VkBaseInStructure*")]
		public IntPtr Next;

	}

	// Vulkan 1.1

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSubgroupProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint SubgroupSize;
		public VKShaderStageFlagBits SupportedStages;
		public VKSubgroupFeatureFlagBits SupportedOperations;
		public VkBool32 QuadOperationsInAllStages;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindBufferMemoryInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBuffer Buffer;
		public VkDeviceMemory Memory;
		public VkDeviceSize MemoryOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindImageMemoryInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage Image;
		public VkDeviceMemory Memory;
		public VkDeviceSize MemoryOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevice16BitStorageFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 StorageBuffer16BitAccess;
		public VkBool32 UniformAndStorageBuffer16BitAccess;
		public VkBool32 StoragePushConstant16;
		public VkBool32 StorageInputOutput16;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryDedicatedRequirements {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 PrefersDedicatedAllocation;
		public VkBool32 RequiresDedicatedAllocation;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryDedicatedAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage Image;
		public VkBuffer Buffer;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryAllocateFlagsInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKMemoryAllocateFlagBits Flags;
		public uint DeviceMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupRenderPassBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DeviceMask;
		public uint DeviceRenderAreaCount;
		[NativeType("const VkRect2D*")]
		public IntPtr DeviceRenderAreas;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupCommandBufferBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DeviceMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint WaitSemaphoreCount;
		[NativeType("const uint32_t*")]
		public IntPtr WaitSemaphoreDeviceIndices;
		public uint CommandBufferCount;
		[NativeType("const uint32_t*")]
		public IntPtr CommandBufferDeviceMasks;
		public uint SignalSemaphoreCount;
		[NativeType("const uint32_t*")]
		public IntPtr SignalSemaphoreDeviceIndices;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupBindSparseInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint ResourceDeviceIndex;
		public uint MemoryDeviceIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindBufferMemoryDeviceGroupInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DeviceIndexCount;
		[NativeType("const uint32_t*")]
		public IntPtr DeviceIndices;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindImageMemoryDeviceGroupInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DeviceIndexCount;
		[NativeType("const uint32_t*")]
		public IntPtr DeviceIndices;
		public uint SplitInstanceBindRegionCount;
		[NativeType("const VkRect2D*")]
		public IntPtr SplitInstanceBindRegions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceGroupProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint PhysicalDeviceCount;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.MaxDeviceGroupSize)]
		public VkPhysicalDevice[] PhysicalDevices;
		public VkBool32 SubsetAllocation;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceGroupDeviceCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint PhysicalDeviceCount;
		[NativeType("const VkPhysicalDevice*")]
		public IntPtr PhysicalDevices;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferMemoryRequirementsInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBuffer Buffer;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageMemoryRequirementsInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage Image;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageSparseMemoryRequirementsInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage Image;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryRequirements2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKMemoryRequirements MemoryRequirements;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageMemoryRequirements2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSparseImageMemoryRequirements MemoryRequirements;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceFeatures2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPhysicalDeviceFeatures Features;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPhysicalDeviceProperties Properties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFormatProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKFormatProperties FormatProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageFormatProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageFormatProperties ImageFormatProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceImageFormatInfo2 {

		public VKStructureType SType;
		public IntPtr Next;
		public VKFormat Format;
		public VKImageType Type;
		public VKImageTiling Tiling;
		public VKImageUsageFlagBits Usage;
		public VKImageCreateFlagBits Flags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKQueueFamilyProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKQueueFamilyProperties QueueFamilyProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMemoryProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPhysicalDeviceMemoryProperties MemoryProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSparseImageFormatProperties2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSparseImageFormatProperties Properties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSparseImageFormatInfo2 {

		public VKStructureType SType;
		public IntPtr Next;
		public VKFormat Format;
		public VKImageType Type;
		public VKSampleCountFlagBits Samples;
		public VKImageUsageFlagBits Usage;
		public VKImageTiling Tiling;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevicePointClippingProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPointClippingBehavior PointClippingBehavior;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKInputAttachmentAspectReference {

		public uint Subpass;
		public uint InputAttachmentIndex;
		public VKImageAspectFlagBits AspectMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassInputAttachmentAspectCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint AspectReferenceCount;
		[NativeType("const VkInputAttachmentAspectReference*")]
		public IntPtr AspectReferences;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageViewUsageCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageUsageFlagBits Usage;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineTessellationDomainOriginStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKTessellationDomainOrigin DomainOrigin;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassMultiviewCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint SubpassCount;
		[NativeType("const uint32_t*")]
		public IntPtr ViewMasks;
		public uint DependencyCount;
		[NativeType("const int32_t*")]
		public IntPtr ViewOffsets;
		public uint CorrelationMaskCount;
		[NativeType("const uint32_t*")]
		public IntPtr CorrelationMasks;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMultiviewFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 Multiview;
		public VkBool32 MultiviewGeometryShader;
		public VkBool32 MultiviewTessellationShader;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMultiviewProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxMultiviewViewCount;
		public uint MaxMultiviewInstanceIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVariablePointersFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 VariablePointersStorageBuffer;
		public VkBool32 VariablePointers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProtectedMemoryFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ProtectedMemory;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProtectedMemoryProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ProtectedNoFault;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceQueueInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDeviceQueueCreateFlagBits Flags;
		public uint QueueFamilyIndex;
		public uint QueueIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKProtectedSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ProtectedSubmit;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSamplerYcbcrConversionCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKFormat Format;
		public VKSamplerYcbcrModelConversion YcbcrModel;
		public VKSamplerYcbcrRange YcbcrRange;
		public VKComponentMapping Components;
		public VKChromaLocation XChromaOffset;
		public VKChromaLocation YChromaOffset;
		public VKFilter ChromaFilter;
		public VkBool32 ForceExplicitReconstruction;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSamplerYcbcrConversionInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkSamplerYcbcrConversion Conversion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBindImagePlaneMemoryInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageAspectFlagBits PlaneAspect;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImagePlaneMemoryRequirementsInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageAspectFlagBits PlaneAspect;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSamplerYcbcrConversionFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 SamplerYcbcrConversion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSamplerYcbcrConversionImageFormatProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint CombinedImageSamplerDescriptorCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorUpdateTemplateEntry {

		public uint DstBinding;
		public uint DstArrayElement;
		public uint DescriptorCount;
		public VKDescriptorType DescriptorType;
		public nuint Offset;
		public nuint Stride;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorUpdateTemplateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDescriptorUpdateTemplateCreateFlags Flags;
		public uint DescriptorUpdateEntryCount;
		[NativeType("const VkDescriptorUpdateTemplateEntry*")]
		public IntPtr DescriptorUpdateEntries;
		public VkDescriptorSetLayout DescriptorSetLayout;
		public VKPipelineBindPoint PipelineBindPoint;
		public VkPipelineLayout PipelineLayout;
		public uint Set;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalMemoryProperties {

		public VKExternalMemoryFeatureFlagBits ExternalMemoryFeatureFlags;
		public VKExternalMemoryHandleTypeFlagBits ExportFromImportedHandleTypes;
		public VKExternalMemoryHandleTypeFlagBits CompatibleTypeHandles;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExternalImageFormatInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryHandleTypeFlagBits HandleType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalImageFormatProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryProperties ExternalMemoryProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExternalBufferInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBufferCreateFlagBits Flags;
		public VKBufferUsageFlagBits Usage;
		public VKExternalMemoryHandleTypeFlagBits HandleType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalBufferProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryProperties ExternalMemoryProperties;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceIDProperties {

		public VKStructureType Type;
		public IntPtr Next;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.UUIDSize)]
		public byte[] DeviceUUID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.UUIDSize)]
		public byte[] DriverUUID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.LUIDSize)]
		public byte[] DeviceLUID;
		public uint DeviceNodeMask;
		public VkBool32 DeviceLUIDValid;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalMemoryImageCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryHandleTypeFlagBits HandleTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalMemoryBufferCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryHandleTypeFlagBits HandleTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExportMemoryAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalMemoryHandleTypeFlagBits HandleTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExternalFenceInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalFenceHandleTypeFlagBits HandleType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalFenceProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalFenceHandleTypeFlagBits ExportFromImportedHandleTypes;
		public VKExternalFenceHandleTypeFlagBits CompatibleHandleTypes;
		public VKExternalFenceFeatureFlagBits ExternalFenceFeatures;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExportFenceCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalFenceHandleTypeFlagBits HandleTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExportSemaphoreCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalSemaphoreHandleTypeFlagBits HandleTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceExternalSemaphoreInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalSemaphoreHandleTypeFlagBits HandleType;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExternalSemaphoreProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKExternalSemaphoreHandleTypeFlagBits ExportFromImportedHandleTypes;
		public VKExternalSemaphoreHandleTypeFlagBits CompatibleHandleTypes;
		public VKExternalSemaphoreFeatureFlagBits ExternalSemaphoreFeatures;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMaintenance3Properties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxPerSetDescriptors;
		public VkDeviceSize MaxMemoryAllocationSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetLayoutSupport {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 Supported;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderDrawParametersFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ShaderDrawParameters;

	}

	// Vulkan 1.2

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan11Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 StorageBuffer16BitAccess;
		public VkBool32 UniformAndStorageBuffer16BitAccess;
		public VkBool32 StoragePushConstant16;
		public VkBool32 StorageInputOutput16;
		public VkBool32 Multiview;
		public VkBool32 MultiviewGeometryShader;
		public VkBool32 MultiviewTessellationShader;
		public VkBool32 VariablePointersStorageBuffer;
		public VkBool32 VariablePointers;
		public VkBool32 ProtectedMemory;
		public VkBool32 SamplerYcbcrConversion;
		public VkBool32 ShaderDrawParameters;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan11Properties {

		public VKStructureType Type;
		public IntPtr Next;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.UUIDSize)]
		public byte[] DeviceUUID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.UUIDSize)]
		public byte[] DriverUUID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.LUIDSize)]
		public byte[] DeviceLUID;
		public uint DeviceNodeMask;
		public VkBool32 DeviceLUIDValid;
		public uint SubgroupSize;
		public VKShaderStageFlagBits SubgroupSupportedStages;
		public VKSubgroupFeatureFlagBits SubgroupSupportedOperations;
		public VkBool32 SubgroupQuadOperationsInAllStages;
		public VKPointClippingBehavior PointClippingBehavior;
		public uint MaxMultiviewViewCount;
		public uint MAxMultiviewInstanceIndex;
		public VkBool32 ProtectedNoFault;
		public uint MaxPerSetDescriptors;
		public VkDeviceSize MaxMemoryAllocationSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan12Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 SamplerMirrorClampToEdge;
		public VkBool32 DrawIndirectCount;
		public VkBool32 StorageBuffer8BitAccess;
		public VkBool32 UniformAndStorageBuffer8BitAccess;
		public VkBool32 StoragePushConstant8;
		public VkBool32 ShaderBufferInt64Atomics;
		public VkBool32 ShaderSharedInt64Atomics;
		public VkBool32 ShaderFloat16;
		public VkBool32 ShaderInt8;
		public VkBool32 DescriptorIndexing;
		public VkBool32 ShaderInputAttachmentArrayDynamicIndexing;
		public VkBool32 ShaderUniformTexelBufferArrayDynamicIndexing;
		public VkBool32 ShaderStorageTexelBufferArrayDynamicIndexing;
		public VkBool32 ShaderUniformBufferArrayNonUniformIndexing;
		public VkBool32 ShaderSampledImageArrayNonUniformIndexing;
		public VkBool32 ShaderStorageBufferArrayNonUniformIndexing;
		public VkBool32 ShaderStorageImageArrayNonUniformIndexing;
		public VkBool32 ShaderInputAttachmentArrayNonUniformIndexing;
		public VkBool32 ShaderUniformTexelBufferArrayNonUniformIndexing;
		public VkBool32 ShaderStorageTexelBufferArrayNonUniformIndexing;
		public VkBool32 DescriptorBindingUniformBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingSampledImageUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageImageUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingUniformTexelBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageTexelBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingUpdateUnusedWhilePending;
		public VkBool32 DescriptorBindingPartiallyBound;
		public VkBool32 DescriptorBindingVariableDescriptorCount;
		public VkBool32 RuntimeDescriptorArray;
		public VkBool32 SamplerFilterMinmax;
		public VkBool32 ScalarBlockLayout;
		public VkBool32 ImagelessFramebuffer;
		public VkBool32 UniformBufferStandardLayout;
		public VkBool32 ShaderSubgroupExtendedTypes;
		public VkBool32 SeparateDepthStencilLayouts;
		public VkBool32 HostQueryRequest;
		public VkBool32 TimelineSemaphore;
		public VkBool32 BufferDeviceAddress;
		public VkBool32 BufferDeviceAddressCaptureReplay;
		public VkBool32 BufferDeviceAddressMultiDevice;
		public VkBool32 VulkanMemoryModel;
		public VkBool32 VulkanMemoryModelDeviceScope;
		public VkBool32 VulkanMemoryModelAvailabilityVisibilityChains;
		public VkBool32 ShaderOutputViewportIndex;
		public VkBool32 ShaderOutputLayer;
		public VkBool32 SubgroupBroadcastDynamicId;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKConformanceVersion {

		public byte Major;
		public byte Minor;
		public byte Subminor;
		public byte Patch;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan12Properties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDriverId DriverID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK12.MaxDriverNameSize)]
		private readonly byte[] driverName;
		public string DriverName => MemoryUtil.GetStringASCII(driverName);
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK12.MaxDriverInfoSize)]
		private readonly byte[] driverInfo;
		public string DriverInfo => MemoryUtil.GetStringASCII(driverInfo);
		public VKConformanceVersion ConformanceVersion;
		public VKShaderFloatControlsIndependence DenormBehaviorIndependence;
		public VKShaderFloatControlsIndependence RoundingModeIndependence;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat16;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat32;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat64;
		public VkBool32 ShaderDenormPreserveFloat16;
		public VkBool32 ShaderDenormPreserveFloat32;
		public VkBool32 ShaderDenormPreserveFloat64;
		public VkBool32 ShaderDenormFlushToZeroFloat16;
		public VkBool32 ShaderDenormFlushToZeroFloat32;
		public VkBool32 ShaderDenormFlushToZeroFloat64;
		public VkBool32 ShaderRoundingModeRTEFloat16;
		public VkBool32 ShaderRoundingModeRTEFloat32;
		public VkBool32 ShaderRoundingModeRTEFloat64;
		public VkBool32 ShaderRoundingModeRTZFloat16;
		public VkBool32 ShaderRoundingModeRTZFloat32;
		public VkBool32 ShaderRoundingModeRTZFloat64;
		public uint MaxUpdateAfterBindDescriptorsInAllPools;
		public VkBool32 ShaderUniformBufferArrayNonUniformIndexingNative;
		public VkBool32 ShaderSampledImageArrayNonUniformIndexingNative;
		public VkBool32 ShaderStorageBufferArrayNonUniformIndexingNative;
		public VkBool32 ShaderStorageImageArrayNonUniformIndexingNative;
		public VkBool32 ShaderInputAttachmentArrayNonUniformIndexingNative;
		public VkBool32 RobustBufferAccessUpdateAfterBind;
		public VkBool32 QuadDivergentImplicitLod;
		public uint MaxPerStageDescriptorUpdateAfterBindSamplers;
		public uint MaxPerStageDescriptorUpdateAfterBindUniformBuffers;
		public uint MaxPerStageDescriptorUpdateAfterBindStorageBuffers;
		public uint MaxPerStageDescriptorUpdateAfterBindSampledImages;
		public uint MaxPerStageDescriptorUpdateAfterBindStorageImages;
		public uint MaxPerStageDescriptorUpdateAfterBindInputAttachments;
		public uint MaxPerStageUpdateAfterBindResources;
		public uint MaxDescriptorSetUpdateAfterBindSamplers;
		public uint MaxDescriptorSetUpdateAfterBindUniformBuffers;
		public uint MaxDescriptorSetUpdateAfterBindUniformBuffersDynamic;
		public uint MaxDescriptorSetUpdateAfterBindStorageBuffers;
		public uint MaxDescriptorSetUpdateAfterBindStorageBuffersDynamic;
		public uint MaxDescriptorSetUpdateAfterBindSampledImages;
		public uint MaxDescriptorSetUpdateAfterBindInputAttachments;
		public VKResolveModeFlagBits SupportedDepthResolveModes;
		public VKResolveModeFlagBits SupportedStencilResolveModes;
		public VkBool32 IndependentResolveNone;
		public VkBool32 IndependentResolve;
		public VkBool32 FilterMinmaxSingleComponentFormats;
		public VkBool32 FilterMinmaxImageComponentMapping;
		public ulong MaxTimelineSemaphoreValueDifference;
		public VKSampleCountFlagBits FramebufferIntegerColorSampleCounts;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageFormatListCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint ViewFormatCount;
		[NativeType("const VkFormat*")]
		public IntPtr ViewFormats;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentDescription2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKAttachmentDescriptionFlagBits Flags;
		public VKFormat Format;
		public VKSampleCountFlagBits Samples;
		public VKAttachmentLoadOp LoadOp;
		public VKAttachmentStoreOp StoreOp;
		public VKAttachmentLoadOp StencilLoadOp;
		public VKAttachmentStoreOp StencilStoreOp;
		public VKImageLayout InitialLayout;
		public VKImageLayout FinalLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentReference2 {

		public VKStructureType Type;
		public IntPtr Next;
		public uint Attachment;
		public VKImageLayout Layout;
		public VKImageAspectFlagBits AspectMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassDescription2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSubpassDescriptionFlagBits Flags;
		public VKPipelineBindPoint PipelineBindPoint;
		public uint ViewMask;
		public uint InputAttachmentCount;
		[NativeType("const VkAttachmentReference2*")]
		public IntPtr InputAttachments;
		public uint ColorAttachmentCount;
		[NativeType("const VkAttachmentReference2*")]
		public IntPtr ColorAttachments;
		[NativeType("const VkAttachmentReference2*")]
		public IntPtr ResolveAttachments;
		[NativeType("const VkAttachmentReference2*")]
		public IntPtr DEpthStencilAttachment;
		public uint PreserveAttachmentCount;
		[NativeType("const uint32_t*")]
		public IntPtr PreserveAttachments;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassDependency2 {

		public VKStructureType Type;
		public IntPtr Next;
		public uint SrcSubpass;
		public uint DstSubpass;
		public VKPipelineStageFlagBits SrcStageMask;
		public VKPipelineStageFlagBits DstStageMask;
		public VKAccessFlagBits SrcAccessMask;
		public VKAccessFlagBits DstAccessMask;
		public VKDependencyFlagBits DependencyFlags;
		public int ViewOffset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassCreateInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKRenderPassCreateFlagBits Flags;
		public uint AttachmentCount;
		[NativeType("const VkAttachmentDescription2*")]
		public IntPtr Attachments;
		public uint SubpassCount;
		[NativeType("const VkSubpassDescription2*")]
		public IntPtr Subpasses;
		public uint DependencyCount;
		[NativeType("const VkSubpassDependency2*")]
		public IntPtr Dependencies;
		public uint CorrelatedViewMaskCount;
		[NativeType("const uint32_t*")]
		public IntPtr CorrelatedViewMasks;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSubpassContents Contents;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassEndInfo {

		public VKStructureType Type;
		public IntPtr Next;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevice8BitStorageFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 StorageBuffer8BitAccess;
		public VkBool32 UniformAndStorageBuffer8BitAccess;
		public VkBool32 StoragePushConstant8;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDriverProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDriverId DriverID;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK12.MaxDriverNameSize)]
		private readonly byte[] driverName;
		public string DriverName => MemoryUtil.GetStringASCII(driverName);
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK12.MaxDriverInfoSize)]
		private readonly byte[] driverInfo;
		public string DriverInfo => MemoryUtil.GetStringASCII(driverInfo);
		public VKConformanceVersion ConformanceVersion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderAtomicInt64Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ShaderBufferInt64Atomics;
		public VkBool32 ShaderSharedInt64Atomics;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderFloat16Int8Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ShaderFloat16;
		public VkBool32 ShaderInt8;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceFloatControlsProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKShaderFloatControlsIndependence DenormBehaviorIndependence;
		public VKShaderFloatControlsIndependence RoundingModeIndependence;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat16;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat32;
		public VkBool32 ShaderSignedZeroInfNanPreserveFloat64;
		public VkBool32 ShaderDenormPreserveFloat16;
		public VkBool32 ShaderDenormPreserveFloat32;
		public VkBool32 ShaderDenormPreserveFloat64;
		public VkBool32 ShaderDenormFlushToZeroFloat16;
		public VkBool32 ShaderDenormFlushToZeroFloat32;
		public VkBool32 ShaderDenormFlushToZeroFloat64;
		public VkBool32 ShaderRoundingModeRTEFloat16;
		public VkBool32 ShaderRoundingModeRTEFloat32;
		public VkBool32 ShaderRoundingModeRTEFloat64;
		public VkBool32 ShaderRoundingModeRTZFloat16;
		public VkBool32 ShaderRoundingModeRTZFloat32;
		public VkBool32 ShaderRoundingModeRTZFloat64;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetLayoutBindingFlagsCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint BindingCount;
		[NativeType("const VkDescriptorBindingFlags*")]
		public IntPtr BindingFlags;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDescriptorIndexingFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ShaderInputAttachmentArrayDynamicIndexing;
		public VkBool32 ShaderUniformTexelBufferArrayDynamicIndexing;
		public VkBool32 ShaderStorageTexelBufferArrayDynamicIndexing;
		public VkBool32 ShaderUniformBufferArrayNonUniformIndexing;
		public VkBool32 ShaderSampledImageArrayNonUniformIndexing;
		public VkBool32 ShaderStorageBufferArrayNonUniformIndexing;
		public VkBool32 ShaderStorageImageArrayNonUniformIndexing;
		public VkBool32 ShaderInputAttachmentArrayNonUniformIndexing;
		public VkBool32 ShaderUniformTexelBufferArrayNonUniformIndexing;
		public VkBool32 ShaderStorageTexelBufferArrayNonUniformIndexing;
		public VkBool32 DescriptorBindingUniformBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingSampledImageUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageImageUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingUniformTexelBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingStorageTexelBufferUpdateAfterBind;
		public VkBool32 DescriptorBindingUpdateUnusedWhilePending;
		public VkBool32 DescriptorBindingPartiallyBound;
		public VkBool32 DescriptorBindingVariableDescriptorCount;
		public VkBool32 RuntimeDescriptorArray;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDescriptorIndexingProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxUpdateAfterBindDescriptorsInAllPools;
		public VkBool32 ShaderUniformBufferArrayNonUniformIndexingNative;
		public VkBool32 ShaderSampledImageArrayNonUniformIndexingNative;
		public VkBool32 ShaderStorageBufferArrayNonUniformIndexingNative;
		public VkBool32 ShaderStorageImageArrayNonUniformIndexingNative;
		public VkBool32 ShaderInputAttachmentArrayNonUniformIndexingNative;
		public VkBool32 RobustBufferAccessUpdateAfterBind;
		public VkBool32 QuadDivergentImplicitLod;
		public uint MaxPerStageDescriptorUpdateAfterBindSamplers;
		public uint MaxPerStageDescriptorUpdateAfterBindUniformBuffers;
		public uint MaxPerStageDescriptorUpdateAfterBindStorageBuffers;
		public uint MaxPerStageDescriptorUpdateAfterBindSampledImages;
		public uint MaxPerStageDescriptorUpdateAfterBindStorageImages;
		public uint MaxPerStageDescriptorUpdateAfterBindInputAttachments;
		public uint MaxPerStageUpdateAfterBindResources;
		public uint MaxDescriptorSetUpdateAfterBindSamplers;
		public uint MaxDescriptorSetUpdateAfterBindUniformBuffers;
		public uint MaxDescriptorSetUpdateAfterBindUniformBuffersDynamic;
		public uint MaxDescriptorSetUpdateAfterBindStorageBuffers;
		public uint MaxDescriptorSetUpdateAfterBindStorageBuffersDynamic;
		public uint MaxDescriptorSetUpdateAfterBindSampledImages;
		public uint MaxDescriptorSetUpdateAfterBindInputAttachments;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetVariableDescriptorCountAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DescriptorSetCount;
		[NativeType("const uint32_t*")]
		public IntPtr DescriptorCounts;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorSetVariableDescriptorCountLayoutSupport {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxVariableDescriptorCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubpassDescriptionDepthStencilResolve {

		public VKStructureType Type;
		public IntPtr Next;
		public VKResolveModeFlagBits DepthResolveMode;
		public VKResolveModeFlagBits StencilResolveMode;
		[NativeType("const VkAttachmentReference2*")]
		public IntPtr DepthStencilResolveAttachment;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDepthStencilResolveProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKResolveModeFlagBits SupportedDepthResolveModes;
		public VKResolveModeFlagBits SupportedStencilResolveModes;
		public VkBool32 IndependentResolveNone;
		public VkBool32 IndependentResolve;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceScalarBlockLayoutFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ScalarBlockLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageStencilUsageCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageUsageFlagBits StencilUsage;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSamplerReductionModeCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSamplerReductionMode ReductionMode;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSamplerFilterMinmaxProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 FilterMinmaxSingleComponentFormats;
		public VkBool32 FilterMinmaxImageComponentMapping;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkanMemoryModelFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 VulkanMemoryModel;
		public VkBool32 VulkanMemoryModelDeviceScope;
		public VkBool32 VulkanMemoryModelAvailabilityVisibilityChains;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceImagelessFramebufferFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ImagelessFramebuffer;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFramebufferAttachmentImageInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageCreateFlagBits Flags;
		public VKImageUsageFlagBits Usage;
		public uint Width;
		public uint Height;
		public uint LayerCount;
		public uint ViewFormatCount;
		[NativeType("const VkFormat*")]
		public IntPtr ViewFormats;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFramebufferAttachmentsCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint AttachmentImageInfoCount;
		[NativeType("const VkFramebufferAttachmentImageInfo*")]
		public IntPtr AttachmentImageInfos;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderPassAttachmentBeginInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint AttachmentCount;
		[NativeType("const VkImageView*")]
		public IntPtr Attachments;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceUniformBufferStandardLayoutFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 UniformBufferStandardLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderSubgroupExtendedTypesFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 ShaderSubgroupExtendedTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSeparateDepthStencilLayoutsFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 SeparateDepthStencilLayouts;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentReferenceStencilLayout {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageLayout StencilLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKAttachmentDescriptionStencilLayout {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageLayout StencilInitialLayout;
		public VKImageLayout StencilFinalLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceHostQueryResetFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 HostQueryReset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTimelineSemaphoreFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 TimelineSemaphore;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTimelineSemaphoreProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public ulong MaxTimelineSemaphoreValueDifference;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSemaphoreTypeCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSemaphoreType SemaphoreType;
		public ulong InitialValue;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKTimelineSemaphoreSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint WaitSemaphoreValueCount;
		[NativeType("const uint64_t*")]
		public IntPtr WaitSemaphoreValues;
		public uint SignalSemaphoreValueCount;
		[NativeType("const uint64_t*")]
		public IntPtr SignalSemaphoreValues;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSemaphoreWaitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSemaphoreWaitFlagBits Flags;
		public uint SemaphoreCount;
		[NativeType("const VkSemaphore*")]
		public IntPtr Semaphores;
		[NativeType("const uint64_t*")]
		public IntPtr Values;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSemaphoreSignalInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkSemaphore Semaphore;
		public ulong Value;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceBufferDeviceAddressFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBool32 BufferDeviceAddress;
		public VkBool32 BufferDeviceAddressCaptureReplay;
		public VkBool32 BufferDeviceAddressMultiDevice;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferDeviceAddressInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBuffer Buffer;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferOpaqueCaptureAddressCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public ulong OpaqueCaptureAddress;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryOpaqueCaptureAddressAllocateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public ulong OpaqueCaptureAddress;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceMemoryOpaqueCaptureAddressInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceMemory Memory;

	}

}
