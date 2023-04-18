using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace Tesseract.Vulkan {

	#region Type alias declarations
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

	using VKExtent3D = Vector3ui;
	using VKOffset3D = Vector3i;
	using VKExtent2D = Vector2ui;
	using VKOffset2D = Vector2i;
	
	public struct VKBool32 {

		public int Value;

		public static implicit operator VKBool32(bool b) => new() { Value = b ? 1 : 0 };
		public static implicit operator bool(VKBool32 b) => b.Value != 0;

	}
	#endregion

	/* Notes:
	 * 
	 *     Some structs are passed as 'in' to native methods, equivalent to passing by const pointer. However,
	 *   to avoid expensive defensive copies these structs are readonly. To maintain field order there are private
	 *   readonly fields in the correct order and public accessors for get and init.
	 */

	#region Vulkan 1.0

	[StructLayout(LayoutKind.Sequential)]
	public struct VKApplicationInfo {

		public VKStructureType Type;
		public IntPtr Next;
		[MarshalAs(UnmanagedType.LPUTF8Str)]
		public string ApplicationName;
		public uint ApplicationVersion;
		[MarshalAs(UnmanagedType.LPUTF8Str)]
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
		private readonly IntPtr allocation;
		public VKAllocationFunction Allocation { init => allocation = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr reallocation;
		public VKReallocationFunction Reallocation { init => reallocation = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr free;
		public VKFreeFunction Free { init => free = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr internalAllocationNotification;
		public VKInternalAllocationNotification InternalAllocationNotification { init => internalAllocationNotification = Marshal.GetFunctionPointerForDelegate(value); }
		private readonly IntPtr internalFreeNotification;
		public VKInternalFreeNotification InternalFreeNotification { init => internalFreeNotification = Marshal.GetFunctionPointerForDelegate(value); }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceFeatures {

		public VKBool32 RobustBufferAccess;
		public VKBool32 FullDrawIndexUInt32;
		public VKBool32 ImageCubeArray;
		public VKBool32 IndependentBlend;
		public VKBool32 GeometryShader;
		public VKBool32 TessellationShader;
		public VKBool32 SampleRateShading;
		public VKBool32 DualSrcBlend;
		public VKBool32 LogicOp;
		public VKBool32 MultiDrawIndirect;
		public VKBool32 DrawIndirectFirstInstance;
		public VKBool32 DepthClamp;
		public VKBool32 DepthBiasClamp;
		public VKBool32 FillModeNonSolid;
		public VKBool32 DepthBounds;
		public VKBool32 WideLines;
		public VKBool32 LargePoints;
		public VKBool32 AlphaToOne;
		public VKBool32 MultiViewport;
		public VKBool32 SamplerAnisotropy;
		public VKBool32 TextureCompressionETC2;
		public VKBool32 TextureCompressionASTC_LDR;
		public VKBool32 TextureCompressionBC;
		public VKBool32 OcclusionQueryPrecise;
		public VKBool32 PipelineStatisticsQuery;
		public VKBool32 VertexPipelineStoresAndAtomics;
		public VKBool32 FragmentStoresAndAtomics;
		public VKBool32 ShaderTessellationAndGeometryPointSize;
		public VKBool32 ShaderImageGatherExtended;
		public VKBool32 ShaderStorageImageExtendedFormats;
		public VKBool32 ShaderStorageImageMultisample;
		public VKBool32 ShaderStorageImageReadWithoutFormat;
		public VKBool32 ShaderStorageImageWriteWithoutFormat;
		public VKBool32 ShaderUniformBufferArrayDynamicIndexing;
		public VKBool32 ShaderSampledImageArrayDynamicIndexing;
		public VKBool32 ShaderStorageBufferArrayDynamicIndexing;
		public VKBool32 ShaderStorageImageArrayDynamicIndexing;
		public VKBool32 ShaderClipDistance;
		public VKBool32 ShaderCullDistance;
		public VKBool32 ShaderFloat64;
		public VKBool32 ShaderInt64;
		public VKBool32 ShaderInt16;
		public VKBool32 ShaderResourceResidency;
		public VKBool32 ShaderResourceMinLod;
		public VKBool32 SparseBinding;
		public VKBool32 SparseResidencyBuffer;
		public VKBool32 SparseResidencyImage2D;
		public VKBool32 SparseResidencyImage3D;
		public VKBool32 SparseResidency2Samples;
		public VKBool32 SparseResidency4Samples;
		public VKBool32 SparseResidency8Samples;
		public VKBool32 SparseResidency16Samples;
		public VKBool32 SparseResidencyAliased;
		public VKBool32 VariableMultisampleRate;
		public VKBool32 InheritedQueries;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFormatProperties {

		public VKFormatFeatureFlagBits LinearTilingFeatures;
		public VKFormatFeatureFlagBits OptimalTilingFeatures;
		public VKFormatFeatureFlagBits BufferFeatures;

	}

	/*
	[StructLayout(LayoutKind.Sequential)]
	public struct VKExtent3D : IVector3Int<VKExtent3D, uint> {

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

		uint IReadOnlyTuple<uint, uint>.X => Width;
		uint IReadOnlyTuple<uint, uint>.Y => Height;
		uint IReadOnlyTuple<uint, uint, uint>.Z => Depth;

		public Span<uint> AsSpan => throw new NotImplementedException();

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

		public override bool Equals([NotNullWhen(true)] object? obj) => obj is VKExtent3D ext && Equals(ext);

		public override int GetHashCode() => (int)(Width + Height * 5 + Depth * 7);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3ui(VKExtent3D e) => new(e.Width, e.Height, e.Depth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator VKExtent3D(Vector3ui v) => new(v.X, v.Y, v.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator %(VKExtent3D left, uint right) => new(left.X % right, left.Y % right, left.Z % right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator &(VKExtent3D left, uint right) => new(left.X & right, left.Y & right, left.Z & right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator |(VKExtent3D left, uint right) => new(left.X | right, left.Y | right, left.Z | right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator ^(VKExtent3D left, uint right) => new(left.X ^ right, left.Y ^ right, left.Z ^ right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator +(VKExtent3D left, uint right) => new(left.X + right, left.Y + right, left.Z + right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator -(VKExtent3D left, uint right) => new(left.X - right, left.Y - right, left.Z - right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator *(VKExtent3D left, uint right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator /(VKExtent3D left, uint right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator +(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator -(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator *(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator /(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator -(VKExtent3D value) => new((uint)(-value.X), (uint)(-value.Y), (uint)(-value.Z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator +(VKExtent3D value) => value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator ++(VKExtent3D value) => new(value.X + 1, value.Y + 1, value.Z + 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator --(VKExtent3D value) => new(value.X - 1, value.Y - 1, value.Z - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator %(VKExtent3D left, VKExtent3D right) => new(left.X % right.X, left.Y % right.Y, left.Z % right.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator <<(VKExtent3D value, int shiftAmount) => new(value.X << shiftAmount, value.Y << shiftAmount, value.Z << shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator >>(VKExtent3D value, int shiftAmount) => new(value.X >> shiftAmount, value.Y >> shiftAmount, value.Z >> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator >>>(VKExtent3D value, int shiftAmount) => new(value.X >>> shiftAmount, value.Y >>> shiftAmount, value.Z >>> shiftAmount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator &(VKExtent3D left, VKExtent3D right) => new(left.X & )

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator |(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator ^(VKExtent3D left, VKExtent3D right) {
			throw new NotImplementedException();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D operator ~(VKExtent3D value) => new(~value.X, ~value.Y, ~value.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VKExtent3D Create(uint x, uint y, uint z) => new(x, y, z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MinMax(ref VKExtent3D min, ref VKExtent3D max) {
			ExMath.MinMax(ref min.Width, ref max.Width);
			ExMath.MinMax(ref min.Height, ref max.Height);
			ExMath.MinMax(ref min.Depth, ref max.Depth);
		}

		public bool Equals(VKExtent3D other) => Width == other.Width && Height == other.Height && Depth == other.Depth;
	}
	*/

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
		public uint MaxVertexInputAttributeOffset;
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
		public VKBool32 TimestampComputeAndGraphics;
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
		public VKBool32 StrictLines;
		public VKBool32 StandardSampleLocations;
		public VkDeviceSize OptimalBufferCopyOffsetAlignment;
		public VkDeviceSize OptimalBufferCopyRowPitchAlignment;
		public VkDeviceSize NonCoherentAtomSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSparseProperties {

		public VKBool32 ResidencyStandard2DBlockShape;
		public VKBool32 ResidencyStandard2DMultisampleBlockShape;
		public VKBool32 ResidencyStandard3DBlockShape;
		public VKBool32 ResidencyAlignedMipSize;
		public VKBool32 ResidencyNonResidentStrict;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProperties {

		public uint APIVersion;
		public uint DriverVersion;
		public VKVendorID VendorID;
		public uint DeviceID;
		public VKPhysicalDeviceType DeviceType;
		private unsafe fixed byte deviceName[VK10.MaxPhysicalDeviceNameSize];
		public string DeviceName {
			get {
				unsafe {
					fixed (byte* pDeviceName = deviceName) {
						return MemoryUtil.GetUTF8((IntPtr)pDeviceName)!;
					}
				}
			}
		}
		private unsafe fixed byte pipelineCacheUUID[VK10.UUIDSize];
		public Guid PipelineCacheUUID {
			get {
				unsafe {
					fixed(byte* pPipelineCacheUUID = pipelineCacheUUID) {
						return new Guid(new ReadOnlySpan<byte>(pPipelineCacheUUID, VK10.UUIDSize));
					}
				}
			}
		}
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

		public uint MemoryTypeCount = 0;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryTypes)]
		private readonly VKMemoryType[] memoryTypes = new VKMemoryType[VK10.MaxMemoryTypes];
		public ReadOnlySpan<VKMemoryType> MemoryTypes => new(memoryTypes);
		public uint MemoryHeapCount = 0;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK10.MaxMemoryHeaps)]
		private readonly VKMemoryHeap[] memoryHeaps = new VKMemoryHeap[VK10.MaxMemoryHeaps];
		public ReadOnlySpan<VKMemoryHeap> MemoryHeaps => new(memoryHeaps);

		public VKPhysicalDeviceMemoryProperties() { }

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

		private unsafe fixed byte extensionName[VK10.MaxExtensionNameSize];
		public string ExtensionName {
			get {
				unsafe {
					fixed (byte* pExtensionName = extensionName) {
						return MemoryUtil.GetUTF8((IntPtr)pExtensionName, VK10.MaxExtensionNameSize)!;
					}
				}
			}
		}
		public uint SpecVersion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKLayerProperties {

		private unsafe fixed byte layerName[VK10.MaxExtensionNameSize];
		public string LayerName {
			get {
				unsafe {
					fixed (byte* pLayerName = layerName) {
						return MemoryUtil.GetUTF8((IntPtr)pLayerName, VK10.MaxExtensionNameSize)!;
					}
				}
			}
		}
		public uint SpecVersion;
		public uint ImplementationVersion;
		private unsafe fixed byte description[VK10.MaxDescriptionSize];
		public string Description {
			get {
				unsafe {
					fixed (byte* pDescription = description) {
						return MemoryUtil.GetUTF8((IntPtr)pDescription, VK10.MaxDescriptionSize)!;
					}
				}
			}
		}

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
	
	/*
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
			set {
				switch (key) {
					case 0: X = value; break;
					case 1: Y = value; break;
					case 2: Z = value; break;
				}
			}
		}

		int IReadOnlyIndexer<int, int>.this[int key] => this[key];

		int ITuple<int, int>.X { get => X; set => X = value; }

		int ITuple<int, int>.Y { get => Y; set => Y = value; }

		int ITuple<int, int, int>.Z { get => Z; set => Z = value; }

		int IReadOnlyTuple<int, int>.X => X;

		int IReadOnlyTuple<int, int>.Y => Y;

		int IReadOnlyTuple<int, int, int>.Z => Z;

		public static implicit operator VKOffset3D(Vector3i v) => new(v.X, v.Y, v.Z);
		public static implicit operator Vector3i(VKOffset3D o) => new(o.X, o.Y, o.Z);

	}
	*/

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
		public VKPipelineShaderStageCreateFlagBits Flags;
		public VKShaderStageFlagBits Stage;
		public VkShaderModule Module;
		public IntPtr Name;
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
		public VKBool32 PrimitiveRestartEnable;

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

		public static implicit operator VKViewport(Viewport v) => new() {
			X = v.Area.Position.X,
			Y = v.Area.Position.Y,
			Width = v.Area.Size.X,
			Height = v.Area.Size.Y,
			MinDepth = v.DepthBounds.Min,
			MaxDepth = v.DepthBounds.Max
		};
		public static implicit operator Viewport(VKViewport v) => new() {
			Area = new() {
				Position = new(v.X, v.Y),
				Size = new(v.Width, v.Height)
			},
			DepthBounds = (v.MinDepth, v.MaxDepth)
		};

	}

	/*
	[StructLayout(LayoutKind.Sequential)]
	public struct VKOffset2D : ITuple2<int> {

		public int X;
		public int Y;

		int ITuple<int, int>.X { get => X; set => X = value; }
		int ITuple<int, int>.Y { get => Y; set => Y = value; }

		int IReadOnlyTuple<int, int>.X => X;
		int IReadOnlyTuple<int, int>.Y => Y;

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

		public static implicit operator VKOffset2D(Vector2i v) => new(v.X, v.Y);
		public static implicit operator Vector2i(VKOffset2D o) => new(o.X, o.Y);

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKExtent2D : ITuple2<uint> {

		public uint Width;
		public uint Height;

		public uint X { get => Width; set => Width = value; }
		public uint Y { get => Height; set => Height = value; }

		uint IReadOnlyTuple<uint, uint>.X => Width;

		uint IReadOnlyTuple<uint, uint>.Y => Height;

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

		public static implicit operator VKExtent2D(Vector2ui v) => new(v.X, v.Y);
		public static implicit operator Vector2ui(VKExtent2D e) => new(e.Width, e.Height);

	}
	*/

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRect2D {

		public VKOffset2D Offset;
		public VKExtent2D Extent;

		public static implicit operator VKRect2D(Recti r) => new() { Offset = r.Position, Extent = (Vector2ui)r.Size };
		public static implicit operator Recti(VKRect2D r) => new() { Position = r.Offset, Size = (Vector2i)(Vector2ui)r.Extent };

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineViewportStateCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineViewportStateCreateFlagBits Flags;
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
		public VKBool32 DepthClampEnable;
		public VKBool32 RasterizerDiscardEnable;
		public VKPolygonMode PolygonMode;
		public VKCullModeFlagBits CullMode;
		public VKFrontFace FrontFace;
		public VKBool32 DepthBiasEnable;
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
		public VKBool32 SampleShadingEnable;
		public float MinSampleShading;
		[NativeType("const VkSampleMask*")]
		public IntPtr SampleMask;
		public VKBool32 AlphaToCoverageEnable;
		public VKBool32 AlphaToOneEnable;

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
		public VKBool32 DepthTestEnable;
		public VKBool32 DepthWriteEnable;
		public VKCompareOp DepthCompareOp;
		public VKBool32 DepthBoundsTestEnable;
		public VKBool32 StencilTestEnable;
		public VKStencilOpState Front;
		public VKStencilOpState Back;
		public float MinDepthBounds;
		public float MaxDepthBounds;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineColorBlendAttachmentState {

		public VKBool32 BlendEnable;
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
		public VKBool32 LogicOpEnable;
		public VKLogicOp LogicOp;
		public uint AttachmentCount;
		[NativeType("const VkPipelineColorBlendAttachmentState*")]
		public IntPtr Attachments;
		[NativeType("float[4]")]
		public Vector4 BlendConstant;

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
		private readonly VKBool32 anisotropyEnable;
		public bool AnisotropyEnable { get => anisotropyEnable; init => anisotropyEnable = value; }
		private readonly float maxAnisotropy;
		public float MaxAnisotropy { get => maxAnisotropy; init => maxAnisotropy = value; }
		private readonly VKBool32 compareEnable;
		public VKBool32 CompareEnable { get => compareEnable; init => compareEnable = value; }
		private readonly VKCompareOp compareOp;
		public VKCompareOp CompareOp { get => compareOp; init => compareOp = value; }
		private readonly float minLod;
		public float MinLod { get => minLod; init => minLod = value; }
		private readonly float maxLod;
		public float MaxLod { get => maxLod; init => maxLod = value; }
		private readonly VKBorderColor borderColor;
		public VKBorderColor BorderColor { get => borderColor; init => borderColor = value; }
		private readonly VKBool32 unnormalizedCoordinates;
		public VKBool32 UnnormalizedCoordinates { get => unnormalizedCoordinates; init => unnormalizedCoordinates = value; }

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
	public readonly struct VKDescriptorSetLayoutCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKDescriptorSetLayoutCreateFlagBits flags;
		public VKDescriptorSetLayoutCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly uint bindingCount;
		public uint BindingCount { get => bindingCount; init => bindingCount = value; }
		private readonly IntPtr bindings;
		[NativeType("const VkDescriptorSetLayoutBinding*")]
		public IntPtr Bindings { get => bindings; init => bindings = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDescriptorPoolSize {

		public VKDescriptorType Type;
		public uint DescriptorCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDescriptorPoolCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKDescriptorPoolCreateFlagBits flags;
		public VKDescriptorPoolCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly uint maxSets;
		public uint MaxSets { get => maxSets; init => maxSets = value; }
		private readonly uint poolSizeCount;
		public uint PoolSizeCount { get => poolSizeCount; init => poolSizeCount = value; }
		private readonly IntPtr poolSizes;
		[NativeType("const VkDescriptorPoolSize*")]
		public IntPtr PoolSizes { get => poolSizes; init => poolSizes = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKDescriptorSetAllocateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VkDescriptorPool descriptorPool;
		public VkDescriptorPool DescriptorPool { get => descriptorPool; init => descriptorPool = value; }
		private readonly uint descriptorSetCount;
		public uint DescriptorSetCount { get => descriptorSetCount; init => descriptorSetCount = value; }
		private readonly IntPtr setLayouts;
		[NativeType("const VkDescriptorSetLayout*")]
		public IntPtr SetLayouts { get => setLayouts; init => setLayouts = value; }

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
	public readonly struct VKFramebufferCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKFramebufferCreateFlagBits flags;
		public VKFramebufferCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly VkRenderPass renderPass;
		public VkRenderPass RenderPass { get => renderPass; init => renderPass = value; }
		private readonly uint attachmentCount;
		public uint AttachmentCount { get => attachmentCount; init => attachmentCount = value; }
		private readonly IntPtr attachments;
		[NativeType("const VkImageView*")]
		public IntPtr Attachments { get => attachments; init => attachments = value; }
		private readonly uint width;
		public uint Width { get => width; init => width = value; }
		private readonly uint height;
		public uint Height { get => height; init => height = value; }
		private readonly uint layers;
		public uint Layers { get => layers; init => layers = value; }

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
	public readonly struct VKRenderPassCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKRenderPassCreateFlagBits flags;
		public VKRenderPassCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly uint attachmentCount;
		public uint AttachmentCount { get => attachmentCount; init => attachmentCount = value; }
		private readonly IntPtr attachments;
		[NativeType("const VkAttachmentDescription*")]
		public IntPtr Attachments { get => attachments; init => attachments = value; }
		private readonly uint subpassCount;
		public uint SubpassCount { get => subpassCount; init => subpassCount = value; }
		private readonly IntPtr subpasses;
		[NativeType("const VkSubpassDescription*")]
		public IntPtr Subpasses { get => subpasses; init => subpasses = value; }
		private readonly uint dependencyCount;
		public uint DependencyCount { get => dependencyCount; init => dependencyCount = value; }
		private readonly IntPtr dependencies;
		[NativeType("const VkSubpassDependency*")]
		public IntPtr Dependencies { get => dependencies; init => dependencies = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKCommandPoolCreateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKCommandPoolCreateFlagBits flags;
		public VKCommandPoolCreateFlagBits Flags { get => flags; init => flags = value; }
		private readonly uint queueFamilyIndex;
		public uint QueueFamilyIndex { get => queueFamilyIndex; init => queueFamilyIndex = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKCommandBufferAllocateInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VkCommandPool commandPool;
		public VkCommandPool CommandPool { get => commandPool; init => commandPool = value; }
		private readonly VKCommandBufferLevel level;
		public VKCommandBufferLevel Level { get => level; init => level = value; }
		private readonly uint commandBufferCount;
		public uint CommandBufferCount { get => commandBufferCount; init => commandBufferCount = value; }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferInheritanceInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkRenderPass RenderPass;
		public uint Subpass;
		public VkFramebuffer Framebuffer;
		public VKBool32 OcclusionQueryEnable;
		public VKQueryControlFlagBits QueryFlags;
		public VKQueryPipelineStatisticFlagBits PipelineStatistics;

	}

	[StructLayout(LayoutKind.Sequential)]
	public readonly struct VKCommandBufferBeginInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VKCommandBufferUsageFlagBits flags;
		public VKCommandBufferUsageFlagBits Flags { get => flags; init => flags = value; }
		private readonly IntPtr inheritanceInfo;
		[NativeType("const VkCommandBufferInheritanceInfo*")]
		public IntPtr InheritanceInfo { get => inheritanceInfo; init => inheritanceInfo = value; }

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
	public readonly struct VKRenderPassBeginInfo {

		private readonly VKStructureType type;
		public VKStructureType Type { get => type; init => type = value; }
		private readonly IntPtr next;
		public IntPtr Next { get => next; init => next = value; }
		private readonly VkRenderPass renderPass;
		public VkRenderPass RenderPass { get => renderPass; init => renderPass = value; }
		private readonly VkFramebuffer framebuffer;
		public VkFramebuffer Framebuffer { get => framebuffer; init => framebuffer = value; }
		private readonly VKRect2D renderArea;
		public VKRect2D RenderArea { get => renderArea; init => renderArea = value; }
		private readonly uint clearValueCount;
		public uint ClearValueCount { get => clearValueCount; init => clearValueCount = value; }
		private readonly IntPtr clearValues;
		[NativeType("const VkClearValue*")]
		public IntPtr ClearValues { get => clearValues; init => clearValues = value; }

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

	#endregion

	#region Vulkan 1.1

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSubgroupProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint SubgroupSize;
		public VKShaderStageFlagBits SupportedStages;
		public VKSubgroupFeatureFlagBits SupportedOperations;
		public VKBool32 QuadOperationsInAllStages;

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
		public VKBool32 StorageBuffer16BitAccess;
		public VKBool32 UniformAndStorageBuffer16BitAccess;
		public VKBool32 StoragePushConstant16;
		public VKBool32 StorageInputOutput16;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryDedicatedRequirements {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 PrefersDedicatedAllocation;
		public VKBool32 RequiresDedicatedAllocation;

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

		public VKStructureType Type = default;
		public IntPtr Next = default;
		public uint PhysicalDeviceCount = 0;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.MaxDeviceGroupSize)]
		private readonly VkPhysicalDevice[] physicalDevices = new VkPhysicalDevice[VK11.MaxDeviceGroupSize];
		public Span<VkPhysicalDevice> PhysicalDevices => physicalDevices;
		public VKBool32 SubsetAllocation = default;

		public VKPhysicalDeviceGroupProperties() { }

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
		public VKBool32 Multiview;
		public VKBool32 MultiviewGeometryShader;
		public VKBool32 MultiviewTessellationShader;

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
		public VKBool32 VariablePointersStorageBuffer;
		public VKBool32 VariablePointers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProtectedMemoryFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ProtectedMemory;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceProtectedMemoryProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ProtectedNoFault;

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
		public VKBool32 ProtectedSubmit;

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
		public VKBool32 ForceExplicitReconstruction;

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
		public VKBool32 SamplerYcbcrConversion;

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

		public VKStructureType Type = default;
		public IntPtr Next = default;
		private unsafe fixed byte deviceUUID[VK10.UUIDSize];
		public Guid DeviceUUID {
			get {
				unsafe {
					fixed (byte* pDeviceUUID = deviceUUID) {
						return new Guid(new ReadOnlySpan<byte>(pDeviceUUID, VK10.UUIDSize));
					}
				}
			}
		}
		private unsafe fixed byte driverUUID[VK10.UUIDSize];
		public Guid DriverUUID {
			get {
				unsafe {
					fixed (byte* pDriverUUID = driverUUID) {
						return new Guid(new ReadOnlySpan<byte>(pDriverUUID, VK10.UUIDSize));
					}
				}
			}
		}
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.LUIDSize)]
		public readonly byte[] DeviceLUID = new byte[VK11.LUIDSize];
		public uint DeviceNodeMask = 0;
		public VKBool32 DeviceLUIDValid = default;

		public VKPhysicalDeviceIDProperties() { }

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
		public VKBool32 Supported;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderDrawParametersFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderDrawParameters;

	}

	#endregion

	#region Vulkan 1.2

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan11Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 StorageBuffer16BitAccess;
		public VKBool32 UniformAndStorageBuffer16BitAccess;
		public VKBool32 StoragePushConstant16;
		public VKBool32 StorageInputOutput16;
		public VKBool32 Multiview;
		public VKBool32 MultiviewGeometryShader;
		public VKBool32 MultiviewTessellationShader;
		public VKBool32 VariablePointersStorageBuffer;
		public VKBool32 VariablePointers;
		public VKBool32 ProtectedMemory;
		public VKBool32 SamplerYcbcrConversion;
		public VKBool32 ShaderDrawParameters;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan11Properties {

		public VKStructureType Type = default;
		public IntPtr Next = default;
		private unsafe fixed byte deviceUUID[VK10.UUIDSize];
		public Guid DeviceUUID {
			get {
				unsafe {
					fixed(byte* pDeviceUUID = deviceUUID) {
						return new Guid(new ReadOnlySpan<byte>(pDeviceUUID, VK10.UUIDSize));
					}
				}
			}
		}
		private unsafe fixed byte driverUUID[VK10.UUIDSize];
		public Guid DriverUUID {
			get {
				unsafe {
					fixed (byte* pDeviceUUID = driverUUID) {
						return new Guid(new ReadOnlySpan<byte>(pDeviceUUID, VK10.UUIDSize));
					}
				}
			}
		}
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = VK11.LUIDSize)]
		public readonly byte[] DeviceLUID = new byte[VK11.LUIDSize];
		public uint DeviceNodeMask = 0;
		public VKBool32 DeviceLUIDValid = default;
		public uint SubgroupSize = 0;
		public VKShaderStageFlagBits SubgroupSupportedStages = default;
		public VKSubgroupFeatureFlagBits SubgroupSupportedOperations = default;
		public VKBool32 SubgroupQuadOperationsInAllStages = default;
		public VKPointClippingBehavior PointClippingBehavior = default;
		public uint MaxMultiviewViewCount = 0;
		public uint MAxMultiviewInstanceIndex = 0;
		public VKBool32 ProtectedNoFault = default;
		public uint MaxPerSetDescriptors = 0;
		public VkDeviceSize MaxMemoryAllocationSize = default;

		public VKPhysicalDeviceVulkan11Properties() { }

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan12Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 SamplerMirrorClampToEdge;
		public VKBool32 DrawIndirectCount;
		public VKBool32 StorageBuffer8BitAccess;
		public VKBool32 UniformAndStorageBuffer8BitAccess;
		public VKBool32 StoragePushConstant8;
		public VKBool32 ShaderBufferInt64Atomics;
		public VKBool32 ShaderSharedInt64Atomics;
		public VKBool32 ShaderFloat16;
		public VKBool32 ShaderInt8;
		public VKBool32 DescriptorIndexing;
		public VKBool32 ShaderInputAttachmentArrayDynamicIndexing;
		public VKBool32 ShaderUniformTexelBufferArrayDynamicIndexing;
		public VKBool32 ShaderStorageTexelBufferArrayDynamicIndexing;
		public VKBool32 ShaderUniformBufferArrayNonUniformIndexing;
		public VKBool32 ShaderSampledImageArrayNonUniformIndexing;
		public VKBool32 ShaderStorageBufferArrayNonUniformIndexing;
		public VKBool32 ShaderStorageImageArrayNonUniformIndexing;
		public VKBool32 ShaderInputAttachmentArrayNonUniformIndexing;
		public VKBool32 ShaderUniformTexelBufferArrayNonUniformIndexing;
		public VKBool32 ShaderStorageTexelBufferArrayNonUniformIndexing;
		public VKBool32 DescriptorBindingUniformBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingSampledImageUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageImageUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingUniformTexelBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageTexelBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingUpdateUnusedWhilePending;
		public VKBool32 DescriptorBindingPartiallyBound;
		public VKBool32 DescriptorBindingVariableDescriptorCount;
		public VKBool32 RuntimeDescriptorArray;
		public VKBool32 SamplerFilterMinmax;
		public VKBool32 ScalarBlockLayout;
		public VKBool32 ImagelessFramebuffer;
		public VKBool32 UniformBufferStandardLayout;
		public VKBool32 ShaderSubgroupExtendedTypes;
		public VKBool32 SeparateDepthStencilLayouts;
		public VKBool32 HostQueryRequest;
		public VKBool32 TimelineSemaphore;
		public VKBool32 BufferDeviceAddress;
		public VKBool32 BufferDeviceAddressCaptureReplay;
		public VKBool32 BufferDeviceAddressMultiDevice;
		public VKBool32 VulkanMemoryModel;
		public VKBool32 VulkanMemoryModelDeviceScope;
		public VKBool32 VulkanMemoryModelAvailabilityVisibilityChains;
		public VKBool32 ShaderOutputViewportIndex;
		public VKBool32 ShaderOutputLayer;
		public VKBool32 SubgroupBroadcastDynamicId;

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
		private unsafe fixed byte driverName[VK12.MaxDriverNameSize];
		public string DriverName {
			get {
				unsafe {
					fixed (byte* pDriverName = driverName) {
						return MemoryUtil.GetUTF8((IntPtr)pDriverName)!;
					}
				}
			}
		}
		private unsafe fixed byte driverInfo[VK12.MaxDriverInfoSize];
		public string DriverInfo {
			get {
				unsafe {
					fixed (byte* pDriverInfo = driverInfo) {
						return MemoryUtil.GetUTF8((IntPtr)pDriverInfo)!;
					}
				}
			}
		}
		public VKConformanceVersion ConformanceVersion;
		public VKShaderFloatControlsIndependence DenormBehaviorIndependence;
		public VKShaderFloatControlsIndependence RoundingModeIndependence;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat16;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat32;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat64;
		public VKBool32 ShaderDenormPreserveFloat16;
		public VKBool32 ShaderDenormPreserveFloat32;
		public VKBool32 ShaderDenormPreserveFloat64;
		public VKBool32 ShaderDenormFlushToZeroFloat16;
		public VKBool32 ShaderDenormFlushToZeroFloat32;
		public VKBool32 ShaderDenormFlushToZeroFloat64;
		public VKBool32 ShaderRoundingModeRTEFloat16;
		public VKBool32 ShaderRoundingModeRTEFloat32;
		public VKBool32 ShaderRoundingModeRTEFloat64;
		public VKBool32 ShaderRoundingModeRTZFloat16;
		public VKBool32 ShaderRoundingModeRTZFloat32;
		public VKBool32 ShaderRoundingModeRTZFloat64;
		public uint MaxUpdateAfterBindDescriptorsInAllPools;
		public VKBool32 ShaderUniformBufferArrayNonUniformIndexingNative;
		public VKBool32 ShaderSampledImageArrayNonUniformIndexingNative;
		public VKBool32 ShaderStorageBufferArrayNonUniformIndexingNative;
		public VKBool32 ShaderStorageImageArrayNonUniformIndexingNative;
		public VKBool32 ShaderInputAttachmentArrayNonUniformIndexingNative;
		public VKBool32 RobustBufferAccessUpdateAfterBind;
		public VKBool32 QuadDivergentImplicitLod;
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
		public VKBool32 IndependentResolveNone;
		public VKBool32 IndependentResolve;
		public VKBool32 FilterMinmaxSingleComponentFormats;
		public VKBool32 FilterMinmaxImageComponentMapping;
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
		public VKBool32 StorageBuffer8BitAccess;
		public VKBool32 UniformAndStorageBuffer8BitAccess;
		public VKBool32 StoragePushConstant8;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDriverProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDriverId DriverID;
		private unsafe fixed byte driverName[VK12.MaxDriverNameSize];
		public string DriverName {
			get {
				unsafe {
					fixed(byte* pDriverName = driverName) {
						return MemoryUtil.GetUTF8((IntPtr)pDriverName)!;
					}
				}
			}
		}
		private unsafe fixed byte driverInfo[VK12.MaxDriverInfoSize];
		public string DriverInfo {
			get {
				unsafe {
					fixed (byte* pDriverInfo = driverInfo) {
						return MemoryUtil.GetUTF8((IntPtr)pDriverInfo)!;
					}
				}
			}
		}
		public VKConformanceVersion ConformanceVersion;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderAtomicInt64Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderBufferInt64Atomics;
		public VKBool32 ShaderSharedInt64Atomics;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderFloat16Int8Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderFloat16;
		public VKBool32 ShaderInt8;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceFloatControlsProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKShaderFloatControlsIndependence DenormBehaviorIndependence;
		public VKShaderFloatControlsIndependence RoundingModeIndependence;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat16;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat32;
		public VKBool32 ShaderSignedZeroInfNanPreserveFloat64;
		public VKBool32 ShaderDenormPreserveFloat16;
		public VKBool32 ShaderDenormPreserveFloat32;
		public VKBool32 ShaderDenormPreserveFloat64;
		public VKBool32 ShaderDenormFlushToZeroFloat16;
		public VKBool32 ShaderDenormFlushToZeroFloat32;
		public VKBool32 ShaderDenormFlushToZeroFloat64;
		public VKBool32 ShaderRoundingModeRTEFloat16;
		public VKBool32 ShaderRoundingModeRTEFloat32;
		public VKBool32 ShaderRoundingModeRTEFloat64;
		public VKBool32 ShaderRoundingModeRTZFloat16;
		public VKBool32 ShaderRoundingModeRTZFloat32;
		public VKBool32 ShaderRoundingModeRTZFloat64;

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
		public VKBool32 ShaderInputAttachmentArrayDynamicIndexing;
		public VKBool32 ShaderUniformTexelBufferArrayDynamicIndexing;
		public VKBool32 ShaderStorageTexelBufferArrayDynamicIndexing;
		public VKBool32 ShaderUniformBufferArrayNonUniformIndexing;
		public VKBool32 ShaderSampledImageArrayNonUniformIndexing;
		public VKBool32 ShaderStorageBufferArrayNonUniformIndexing;
		public VKBool32 ShaderStorageImageArrayNonUniformIndexing;
		public VKBool32 ShaderInputAttachmentArrayNonUniformIndexing;
		public VKBool32 ShaderUniformTexelBufferArrayNonUniformIndexing;
		public VKBool32 ShaderStorageTexelBufferArrayNonUniformIndexing;
		public VKBool32 DescriptorBindingUniformBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingSampledImageUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageImageUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingUniformTexelBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingStorageTexelBufferUpdateAfterBind;
		public VKBool32 DescriptorBindingUpdateUnusedWhilePending;
		public VKBool32 DescriptorBindingPartiallyBound;
		public VKBool32 DescriptorBindingVariableDescriptorCount;
		public VKBool32 RuntimeDescriptorArray;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDescriptorIndexingProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxUpdateAfterBindDescriptorsInAllPools;
		public VKBool32 ShaderUniformBufferArrayNonUniformIndexingNative;
		public VKBool32 ShaderSampledImageArrayNonUniformIndexingNative;
		public VKBool32 ShaderStorageBufferArrayNonUniformIndexingNative;
		public VKBool32 ShaderStorageImageArrayNonUniformIndexingNative;
		public VKBool32 ShaderInputAttachmentArrayNonUniformIndexingNative;
		public VKBool32 RobustBufferAccessUpdateAfterBind;
		public VKBool32 QuadDivergentImplicitLod;
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
		public VKBool32 IndependentResolveNone;
		public VKBool32 IndependentResolve;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceScalarBlockLayoutFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ScalarBlockLayout;

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
		public VKBool32 FilterMinmaxSingleComponentFormats;
		public VKBool32 FilterMinmaxImageComponentMapping;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkanMemoryModelFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 VulkanMemoryModel;
		public VKBool32 VulkanMemoryModelDeviceScope;
		public VKBool32 VulkanMemoryModelAvailabilityVisibilityChains;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceImagelessFramebufferFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ImagelessFramebuffer;

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
		public VKBool32 UniformBufferStandardLayout;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderSubgroupExtendedTypesFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderSubgroupExtendedTypes;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSeparateDepthStencilLayoutsFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 SeparateDepthStencilLayouts;

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
		public VKBool32 HostQueryReset;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTimelineSemaphoreFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 TimelineSemaphore;

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
		public VKBool32 BufferDeviceAddress;
		public VKBool32 BufferDeviceAddressCaptureReplay;
		public VKBool32 BufferDeviceAddressMultiDevice;

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

	#endregion

	#region Vulkan 1.3

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan13Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 RobustImageAccess;
		public VKBool32 InlineUniformBlock;
		public VKBool32 DescriptorBindingInlineUniformBlockUpdateAfterBind;
		public VKBool32 PipelineCreationCacheControl;
		public VKBool32 PrivateData;
		public VKBool32 ShaderDemoteToHelperInvocation;
		public VKBool32 ShaderTerminateInvocation;
		public VKBool32 SubgroupSizeControl;
		public VKBool32 ComputeFullSubgroups;
		public VKBool32 Synchronization2;
		public VKBool32 TextureCompressionASTC_HDR;
		public VKBool32 ShaderZeroInitializeWorkgroupMemory;
		public VKBool32 DynamicRendering;
		public VKBool32 ShaderIntegerDotProduct;
		public VKBool32 Maintenance4;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceVulkan13Properties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MinSubgroupSize;
		public uint MaxSubgroupSize;
		public uint MaxComputeWorkgroupSubgroups;
		public VKShaderStageFlagBits RequiredSubgroupSizeStages;
		public uint MaxInlineUniformBlockSize;
		public uint MaxPerStageDescriptorInlineUniformBlocks;
		public uint MaxPerStageDescriptorUpdateAfterBindInlineUniformBlocks;
		public uint MaxInlineUniformTotalSize;
		public VKBool32 IntegerDotProduct8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct8BitSignedAccelerated;
		public VKBool32 IntegerDotProduct8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct4x8BitPackedUnsignedAccelerated;
		public VKBool32 IntegerDotProduct4x8BitPackedSignedAccelerated;
		public VKBool32 IntegerDotProduct4x8BitPackedMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct16BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct16BitSignedAccelerated;
		public VKBool32 IntegerDotProduct16BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct32BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct32BitSignedAccelerated;
		public VKBool32 IntegerDotProduct32BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct64BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct64BitSignedAccelerated;
		public VKBool32 IntegerDotProduct64BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitPackedUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitPackedSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitPackedMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitMixedSignednessAccelerated;
		public VkDeviceSize StorageTexelBufferOffsetAlignmentBytes;
		public VKBool32 StorageTexelBufferOffsetSingleTexelAlignment;
		public VkDeviceSize UniformTexelBufferOffsetAlignmentBytes;
		public VKBool32 UniformTexelBufferOffsetSingleTexelAlignment;
		public VkDeviceSize MaxBufferSize;

	}

	// VK_EXT_pipeline_creation_feedback

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineCreationFeedback {

		public VKPipelineCreationFeedbackFlagBits Flags;
		public ulong Duration;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineCreationFeedbackCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("VkPipelineCreationFeedback*")]
		public IntPtr PipelineCreationFeedback;
		public uint PipelineStageCreationFeedbackCount;
		[NativeType("VkPipelineCreationFeedback*")]
		public IntPtr PipelineStageCreationFeedbacks;

	}

	// VK_KHR_shader_terminate_invocation

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderTerminateInvocationFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderTerminateInvocation;

	}

	// VK_EXT_tooling_info

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceToolProperties {

		public VKStructureType Type;
		public IntPtr Next;
		private unsafe fixed byte name[VK10.MaxExtensionNameSize];
		public string Name {
			get {
				unsafe {
					fixed(byte* pName = name) {
						return MemoryUtil.GetUTF8((IntPtr)pName)!;
					}
				}
			}
		}
		private unsafe fixed byte version[VK10.MaxExtensionNameSize];
		public string Version {
			get {
				unsafe {
					fixed(byte* pVersion = version) {
						return MemoryUtil.GetUTF8((IntPtr)pVersion)!;
					}
				}
			}
		}
		public VKToolPurposeFlagBits Purposes;
		private unsafe fixed byte description[VK10.MaxDescriptionSize];
		public string Description {
			get {
				unsafe {
					fixed (byte* pDescription = description) {
						return MemoryUtil.GetUTF8((IntPtr)pDescription)!;
					}
				}
			}
		}
		private unsafe fixed byte layer[VK10.MaxExtensionNameSize];
		public string Layer {
			get {
				unsafe {
					fixed(byte* pLayer = layer) {
						return MemoryUtil.GetUTF8((IntPtr)pLayer)!;
					}
				}
			}
		}

	}

	// VK_EXT_shader_demote_to_helper_invocation

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderDemoteToHelperInvocationFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderDemoteToHelperInvocation;

	}

	// VK_EXT_private_data

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevicePrivateDataFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 PrivateData;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDevicePrivateDataCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint PrivateDataSlotRequestCount;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPrivateDataSlotCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPrivateDataSlotCreateFlagBits Flags;

	}

	// VK_EXT_pipeline_creation_cache_control

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDevicePipelineCreationCacheControlFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 PipelineCreationCacheControl;

	}

	// VK_KHR_synchronization2

	[StructLayout(LayoutKind.Sequential)]
	public struct VKMemoryBarrier2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineStageFlagBits2 SrcStageMask;
		public VKAccessFlagBits2 SrcAccessMask;
		public VKPipelineStageFlagBits2 DstStageMask;
		public VKAccessFlagBits2 DstAccessMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferMemoryBarrier2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineStageFlagBits2 SrcStageMask;
		public VKAccessFlagBits2 SrcAccessMask;
		public VKPipelineStageFlagBits2 DstStageMask;
		public VKAccessFlagBits2 DstAccessMask;
		public uint SrcQueueFamilyIndex;
		public uint DstQueueFamilyIndex;
		public VkBuffer Buffer;
		public VkDeviceSize Offset;
		public VkDeviceSize Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageMemoryBarrier2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKPipelineStageFlagBits2 SrcStageMask;
		public VKAccessFlagBits2 SrcAccessMask;
		public VKPipelineStageFlagBits2 DstStageMask;
		public VKAccessFlagBits2 DstAccessMask;
		public uint SrcQueueFamilyIndex;
		public uint DstQueueFamilyIndex;
		public VkImage Image;
		public VKImageSubresourceRange SubresourceRange;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDependencyInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKDependencyFlagBits Flags;
		public uint MemoryBarrierCount;
		[NativeType("const VkMemoryBarrier2*")]
		public IntPtr MemoryBarriers;
		[NativeType("const VkBufferMemoryBarrier2*")]
		public IntPtr BufferMemoryBarriers;
		[NativeType("const VkImageMemoryBarrier2*")]
		public IntPtr ImageMemoryBarriers;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSemaphoreSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkSemaphore Semaphore;
		public ulong Value;
		public VKPipelineStageFlagBits2 StageMask;
		public uint DeviceIndex;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferSubmitInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkCommandBuffer CommandBuffer;
		public uint DeviceMask;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKSubmitInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKSubmitFlagBits Flags;
		public uint WaitSemaphoreInfoCount;
		[NativeType("const VkSemaphoreSubmitInfo*")]
		public IntPtr WaitSemaphoreInfos;
		public uint CommandBufferInfoCount;
		[NativeType("const VkCommandBufferSubmitInfo*")]
		public IntPtr CommandBufferInfos;
		public uint SignalSemaphoreInfoCount;
		[NativeType("const VkSemaphoreSubmitInfo*")]
		public IntPtr SignalSemaphoreInfos;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VkPhysicalDeviceSynchronization2Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 Synchronization2;

	}

	// VK_KHR_zero_initialize_workgroup_memory

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceZeroInitializeWorkgroupMemoryFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderZeroInitializeWorkgroupMemory;

	}

	// VK_EXT_image_robustness

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceImageRobustnessFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 RobustImageAccess;

	}

	// VK_KHR_copy_commands2

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferCopy2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceSize SrcOffset;
		public VkDeviceSize DstOffset;
		public VkDeviceSize Size;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyBufferInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBuffer SrcBuffer;
		public VkBuffer DstBuffer;
		public uint RegionCount;
		[NativeType("const VkBufferCopy2*")]
		public IntPtr Regions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageCopy2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageSubresourceLayers SrcSubresource;
		public VKOffset3D SrcOffset;
		public VKImageSubresourceLayers DstSubresource;
		public VKOffset3D DstOffset;
		public VKExtent3D Extent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyImageInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage SrcImage;
		public VKImageLayout SrcImageLayout;
		public VkImage DstImage;
		public VKImageLayout DstImageLayout;
		public uint RegionCount;
		[NativeType("const VkImageCopy2*")]
		public IntPtr Regions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBufferImageCopy2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceSize BufferOffset;
		public uint BufferRowLength;
		public uint BufferImageHeight;
		public VKImageSubresourceLayers ImageSubresource;
		public VKOffset3D ImageOffset;
		public VKExtent3D ImageExtent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyBufferToImageInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkBuffer SrcBuffer;
		public VkImage DstImage;
		public VKImageLayout DstImageLayout;
		public uint RegionCount;
		[NativeType("const VkBufferImageCopy2*")]
		public IntPtr Regions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCopyImageToBufferInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage SrcImage;
		public VKImageLayout SrcImageLayout;
		public VkBuffer DstBuffer;
		public uint RegionCount;
		[NativeType("const VkBufferImageCopy2*")]
		public IntPtr Regions;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageBlit2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageSubresourceLayers SrcSubresource;
		public VKOffset3D SrcOffsets0;
		public VKOffset3D SrcOffsets1;
		public VKImageSubresourceLayers DstSubresource;
		public VKOffset3D DstOffsets0;
		public VKOffset3D DstOffsets1;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKBlitImageInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage SrcImage;
		public VKImageLayout SrcImageLayout;
		public VkImage DstImage;
		public VKImageLayout DstImageLayout;
		public uint RegionCount;
		[NativeType("const VkImageBlit2*")]
		public IntPtr Regions;
		public VKFilter Filter;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKImageResolve2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKImageSubresourceLayers SrcSubresource;
		public VKOffset3D SrcOffset;
		public VKImageSubresourceLayers DstSubresource;
		public VKOffset3D DstOffset;
		public VKExtent3D Extent;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKResolveImageInfo2 {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImage SrcImage;
		public VKImageLayout SrcImageLayout;
		public VkImage DstImage;
		public VKImageLayout DstImageLayout;
		public uint RegionCount;
		[NativeType("const VkImageResolve2*")]
		public IntPtr Regions;

	}

	// VK_EXT_subgroup_size_control

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSubgroupSizeControlFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 SubgroupSizeControl;
		public VKBool32 ComputeFullSubgroups;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceSubgroupSizeControlProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MinSubgroupSize;
		public uint MaxSubgroupSize;
		public uint MaxCOmputeWorkgroupSubgroups;
		public VKShaderStageFlagBits RequiredSubgroupSizeStages;
		
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineShaderStageRequiredSubgroupSizeCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint RequiredSubgroupSize;

	}

	// VK_EXT_inline_uniform_block

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceInlineUniformBlockFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 InlineUniformBlock;
		public VKBool32 DescriptorBindingInlineUniformBlockUpdateAfterBind;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceInlineUniformBlockProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxInlineUniformBlockSize;
		public uint MaxPerStageDescriptorInlineUniformBlocks;
		public uint MaxPerStageDescriptorUpdateAfterBindInlineUniformBlocks;
		public uint MaxDescriptorSetInlineUniformBlocks;
		public uint MaxDescriptorSetUpdateAfterBindInlineUniformBlocks;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKWriteDescriptorSetInlineUniformBlock {

		public VKStructureType Type;
		public IntPtr Next;
		public uint DataSize;
		public IntPtr Data;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKWriteDescriptorSetInlineUniformBlockCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint MaxInlineUniformBlockBindings;

	}

	// VK_EXT_texture_compression_astc_hdr

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTextureCompressionASTCHDRFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 TextureCompressionASTC_HDR;

	}
	
	// VK_KHR_dynamic_rendering

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderingAttachmentInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VkImageView ImageView;
		public VKImageLayout ImageLayout;
		public VKResolveModeFlagBits ResolveMode;
		public VkImageView ResolveImageView;
		public VKImageLayout ResolveImageLayout;
		public VKAttachmentLoadOp LoadOp;
		public VKAttachmentStoreOp StoreOp;
		public VKClearValue ClearValue;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKRenderingInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKRenderingFlagBits Flags;
		public VKRect2D RenderArea;
		public uint LayerCount;
		public uint ViewMask;
		public uint ColorAttachmentCount;
		[NativeType("const VkRenderingAttachmentInfo*")]
		public IntPtr ColorAttachments;
		[NativeType("const VkRenderingAttachmentInfo*")]
		public IntPtr DepthAttachment;
		[NativeType("const VkRenderingAttachmentInfo*")]
		public IntPtr StencilAttachment;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPipelineRenderingCreateInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public uint ViewMask;
		public uint ColorAttachmentCount;
		[NativeType("const VkFormat*")]
		public IntPtr ColorAttachmentFormats;
		public VKFormat DepthAttachmentFormat;
		public VKFormat StencilAttachmentFormat;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceDynamicRenderingFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 DynamicRendering;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKCommandBufferInheritanceRenderingInfo {

		public VKStructureType Type;
		public IntPtr Next;
		public VKRenderingFlagBits Flags;
		public uint ViewMask;
		public uint ColorAttachmentCount;
		[NativeType("const VkFormat*")]
		public IntPtr ColorAttachmentFormats;
		public VKFormat DepthAttachmentFormat;
		public VKFormat StencilAttachmentFormat;
		public VKSampleCountFlagBits RasterizationSamples;

	}

	// VK_KHR_shader_integer_dot_product

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderIntegerDotProductFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 ShaderIntegerDotProduct;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceShaderIntegerDotProductProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 IntegerDotProduct8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct8BitSignedAccelerated;
		public VKBool32 IntegerDotProduct8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct4x8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct4x8BitSignedAccelerated;
		public VKBool32 IntegerDotProduct4x8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct16BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct16BitSignedAccelerated;
		public VKBool32 IntegerDotProduct16BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct32BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct32BitSignedAccelerated;
		public VKBool32 IntegerDotProduct32BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProduct64BitUnsignedAccelerated;
		public VKBool32 IntegerDotProduct64BitSignedAccelerated;
		public VKBool32 IntegerDotProduct64BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating4x8BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating16BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating32BitMixedSignednessAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitUnsignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitSignedAccelerated;
		public VKBool32 IntegerDotProductAccumulatingSaturating64BitMixedSignednessAccelerated;

	}

	// VK_EXT_texel_buffer_alignment

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTexelBufferAlignmentFeatures {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 TexelBufferAlignment;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceTexelBufferAlignmentProperties {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceSize StorageTexelBufferOffsetAlignmentBytes;
		public VKBool32 StorageTexelBufferOffsetSingleTexelAlignment;
		public VkDeviceSize UniformTexelBufferOffsetAlignmentBytes;
		public VKBool32 UniformTexelBufferOffsetSingleTexelAlignment;

	}

	// VK_KHR_format_feature_flags2

	[StructLayout(LayoutKind.Sequential)]
	public struct VKFormatProperties3 {

		public VKStructureType Type;
		public IntPtr Next;
		public VKFormatFeatureFlagBits2 LinearTilingFeatures;
		public VKFormatFeatureFlagBits2 OptimalTilingFeatures;
		public VKFormatFeatureFlagBits2 BufferFeatures;

	}

	// VK_KHR_maintenance4

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMaintenance4Features {

		public VKStructureType Type;
		public IntPtr Next;
		public VKBool32 Maintenance4;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKPhysicalDeviceMaintenance4Properties {

		public VKStructureType Type;
		public IntPtr Next;
		public VkDeviceSize MaxBufferSize;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceBufferMemoryRequirements {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("const VkBufferCreateInfo*")]
		public IntPtr CreateInfo;

	}

	[StructLayout(LayoutKind.Sequential)]
	public struct VKDeviceImageMemoryRequirements {

		public VKStructureType Type;
		public IntPtr Next;
		[NativeType("const VkImageCreateInfo*")]
		public IntPtr CreateInfo;
		public VKImageAspectFlagBits PlaneAspect;

	}

	#endregion

}
