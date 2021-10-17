using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Util;
using Tesseract.Vulkan.Graphics.Impl;

namespace Tesseract.Vulkan.Graphics {

	public class VulkanGraphicsProperties : IGraphicsProperites {

		public GraphicsType Type => GraphicsType.Vulkan;

		public string TypeInfo => "Tesseract Vulkan Graphics";

		public string RendererName { get; }

		public string VendorName { get; }

		public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.Concurrent;

		private readonly VulkanMemory memory;

		public ulong TotalVideoMemory => memory.TotalVisibleBytes;

		public ulong TotalDeviceMemory => memory.TotalLocalBytes;

		public ulong TotalCommittedMemory => memory.TotalUsedBytes;

		public VulkanGraphicsProperties(VulkanDevice device, VulkanMemory memory) {
			var props = device.PhysicalDevice.Properties;
			RendererName = props.DeviceName;
			VendorName = props.VendorID.ToString();
			this.memory = memory;
		}

	}

	public class VulkanGraphicsFeatures : IGraphicsFeatures {

		public static GraphicsHardwareFeatures FromVK(VulkanPhysicalDeviceInfo info) {
			VKPhysicalDeviceFeatures features = info.Features;
			return new GraphicsHardwareFeatures() {
				RobustBufferAccess = features.RobustBufferAccess,
				FullDrawIndexUInt32 = features.FullDrawIndexUInt32,
				CubeMapArray = features.ImageCubeArray,
				IndependentBlend = features.IndependentBlend,
				GeometryShader = features.GeometryShader,
				TessellationShader = features.TessellationShader,
				SampleRateShading = features.SampleRateShading,
				DualSrcBlend = features.DualSrcBlend,
				LogicOp = features.LogicOp,
				MultiDrawIndirect = features.MultiDrawIndirect,
				DrawIndirectFirstInstance = features.DrawIndirectFirstInstance,
				DepthClamp = features.DepthClamp,
				DepthBiasClamp = features.DepthBiasClamp,
				FillModeNonSolid = features.FillModeNonSolid,
				DepthBounds = features.DepthBounds,
				WideLines = features.WideLines,
				LargePoints = features.LargePoints,
				AlphaToOne = features.AlphaToOne,
				MultiViewport = features.MultiViewport,
				SamplerAnisotropy = features.SamplerAnisotropy,
				TextureCompressionETC2 = features.TextureCompressionETC2,
				TextureCompressionASTC = features.TextureCompressionASTC_LDR,
				TextureCompressionBC = features.TextureCompressionBC,
				OcclusionQueryPrecise = features.OcclusionQueryPrecise,
				PipelineStatisticsQuery = features.PipelineStatisticsQuery,
				VertexPipelineStoresAndAtomics = features.VertexPipelineStoresAndAtomics,
				FragmentStoresAndAtomics = features.FragmentStoresAndAtomics,
				ShaderTessellationAndGeometryPointSize = features.ShaderTessellationAndGeometryPointSize,
				ShaderImageGatherExtended = features.ShaderImageGatherExtended,
				ShaderStorageImageExtendedFormats = features.ShaderStorageImageExtendedFormats,
				ShaderStorageImageMultisample = features.ShaderStorageImageMultisample,
				ShaderStorageImageReadWithoutFormat = features.ShaderStorageImageReadWithoutFormat,
				ShaderStorageImageWriteWithoutFormat = features.ShaderStorageImageWriteWithoutFormat,
				ShaderUniformBufferArrayDynamicIndexing = features.ShaderUniformBufferArrayDynamicIndexing,
				ShaderSampledImageArrayDynamicIndexing = features.ShaderSampledImageArrayDynamicIndexing,
				ShaderStorageBufferArrayDynamicIndexing = features.ShaderStorageBufferArrayDynamicIndexing,
				ShaderStorageImageArrayDynamicIndexing = features.ShaderStorageImageArrayDynamicIndexing,
				ShaderClipDistance = features.ShaderClipDistance,
				ShaderCullDistance = features.ShaderCullDistance,
				ShaderFloat64 = features.ShaderFloat64,
				ShaderInt64 = features.ShaderInt64,
				ShaderInt16 = features.ShaderInt16,
				ShaderResourceResidency = features.ShaderResourceResidency,
				ShaderResourceMinLOD = features.ShaderResourceMinLod,
				SparseBinding = features.SparseBinding,
				SparseResidencyBuffer = features.SparseResidencyBuffer,
				SparseResidencyImage2D = features.SparseResidencyImage2D,
				SparseResidencyImage3D = features.SparseResidencyImage3D,
				SparseResidency2Samples = features.SparseResidency2Samples,
				SparseResidency4Samples = features.SparseResidency4Samples,
				SparseResidency8Samples = features.SparseResidency8Samples,
				SparseResidency16Samples = features.SparseResidency16Samples,
				SparseResidencyAliased = features.SparseResidencyAliased,
				DrawIndirect = true
			};
		}

		public GraphicsHardwareFeatures HardwareFeatures { get; }

		public bool StandardSampleLocations => true; // Uses standard in the base spec

		public bool StrictLines { get; } = false;

		private readonly HashSet<PipelineDynamicState> dynamicStates = new() {
			PipelineDynamicState.BlendConstants,
			PipelineDynamicState.DepthBias,
			PipelineDynamicState.DepthBounds,
			PipelineDynamicState.LineWidth,
			PipelineDynamicState.Scissor,
			PipelineDynamicState.StencilCompareMask,
			PipelineDynamicState.StencilReference,
			PipelineDynamicState.StencilWriteMask,
			PipelineDynamicState.Viewport,
		};

		public IReadOnlyIndexer<PipelineDynamicState, bool> SupportedDynamicStates { get; }

		public bool PushConstants => true; // Always have push constants

		public bool TextureSubView => true; // Sub-views are always available

		public bool SamplerCustomBorderColor { get; } = false;

		// Don't yet support anything other than SPIR-V
		public IReadOnlyIndexer<ShaderSourceType, bool> SupportedShaderSourceTypes { get; } = new FuncReadOnlyIndexer<ShaderSourceType, bool>(src => src == ShaderSourceType.SPIRV);

		public ShaderSourceType PreferredShaderSourceType => ShaderSourceType.SPIRV; // Prefer SPIR-V

		public VulkanGraphicsFeatures(VulkanDevice device) {
			HardwareFeatures = FromVK(device.PhysicalDevice);
			SupportedDynamicStates = new FuncReadOnlyIndexer<PipelineDynamicState, bool, HashSet<PipelineDynamicState>>(
				dynamicStates, (HashSet<PipelineDynamicState> set, PipelineDynamicState state) => set.Contains(state)
			);

			VKDevice logicalDevice = device.Device;
			if (logicalDevice.EXTCustomBorderColor) {
				SamplerCustomBorderColor = device.PhysicalDevice.CustomBorderColorFeaturesEXT.Value.CustomBorderColors;
			}
			if (logicalDevice.EXTLineRasterization != null) {
				var features = device.PhysicalDevice.LineRasterizationFeaturesEXT.Value;
				StrictLines = !(features.BresenhamLines || features.RectangularLlines || features.SmoothLines);
			}
			if (logicalDevice.EXTExtendedDynamicState != null) {
				var features = device.PhysicalDevice.ExtendedDynamicStateFeaturesEXT.Value;
				if (features.ExtendedDynamicState) {
					dynamicStates.Add(PipelineDynamicState.CullMode);
					dynamicStates.Add(PipelineDynamicState.FrontFace);
					dynamicStates.Add(PipelineDynamicState.DrawMode);
					dynamicStates.Add(PipelineDynamicState.DepthTest);
					dynamicStates.Add(PipelineDynamicState.DepthWriteEnable);
					dynamicStates.Add(PipelineDynamicState.DepthCompareOp);
					dynamicStates.Add(PipelineDynamicState.DepthBoundsTestEnable);
					dynamicStates.Add(PipelineDynamicState.StencilTestEnable);
					dynamicStates.Add(PipelineDynamicState.StencilOp);
				}
			}
		}

	}

	public class VulkanGraphicsLimits : IGraphicsLimits {

		public VulkanPhysicalDeviceInfo DeviceInfo { get; }

		public uint MaxTextureDimension1D => DeviceInfo.Limits.MaxImageDimension1D;
		public uint MaxImageDimension2D => DeviceInfo.Limits.MaxImageDimension2D;
		public uint MaxTextureDimension3D => DeviceInfo.Limits.MaxImageDimension3D;
		public uint MaxTextureDimensionCube => DeviceInfo.Limits.MaxImageDimensionCube;
		public uint MaxTextureArrayLayers => DeviceInfo.Limits.MaxImageArrayLayers;
		public uint MaxTexelBufferElements => DeviceInfo.Limits.MaxTexelBufferElements;
		public uint MaxUniformBufferRange => DeviceInfo.Limits.MaxUniformBufferRange;
		public uint MaxStorageBufferRange => DeviceInfo.Limits.MaxStorageBufferRange;
		public uint MaxPushConstantSize => DeviceInfo.Limits.MaxPushConstantsSize;
		public uint MaxSamplerObjects => DeviceInfo.Limits.MaxSamplerAllocationCount;
		public ulong SparseAddressSpaceSize => DeviceInfo.Limits.SparseAddressSpaceSize;
		public uint MaxBoundSets => DeviceInfo.Limits.MaxBoundDescriptorSets;
		public uint MaxPerStageSamplers => DeviceInfo.Limits.MaxPerStageDescriptorSamplers;
		public uint MaxPerStageUniformBuffers => DeviceInfo.Limits.MaxPerStageDescriptorUniformBuffers;
		public uint MaxPerStageStorageBuffers => DeviceInfo.Limits.MaxPerStageDescriptorSamplers;
		public uint MaxPerStageSampledImages => DeviceInfo.Limits.MaxPerStageDescriptorSampledImages;
		public uint MaxPerStageStorageImages => DeviceInfo.Limits.MaxPerStageDescriptorStorageImages;
		public uint MaxPerStageInputAttachments => DeviceInfo.Limits.MaxPerStageDescriptorInputAttachments;
		public uint MaxPerStageResources => DeviceInfo.Limits.MaxPerStageResources;
		public uint MaxPerLayoutSamplers => DeviceInfo.Limits.MaxDescriptorSetSamplers;
		public uint MaxPerLayoutUniformBuffers => DeviceInfo.Limits.MaxDescriptorSetUniformBuffers;
		public uint MaxPerLayoutDynamicUniformBuffers => DeviceInfo.Limits.MaxDescriptorSetUniformBuffersDynamic;
		public uint MaxPerLayoutStorageBuffers => DeviceInfo.Limits.MaxDescriptorSetStorageBuffers;
		public uint MaxPerLayoutDynamicStorageBuffers => DeviceInfo.Limits.MaxDescriptorSetStorageBuffersDynamic;
		public uint MaxPerLayoutSampledImages => DeviceInfo.Limits.MaxDescriptorSetSampledImages;
		public uint MaxPerLayoutStorageImages => DeviceInfo.Limits.MaxDescriptorSetStorageImages;
		public uint MaxPerLayoutInputAttachments => DeviceInfo.Limits.MaxDescriptorSetInputAttachments;
		public uint MaxVertexAttribs => DeviceInfo.Limits.MaxVertexInputAttributes;
		public uint MaxVertexBindings => DeviceInfo.Limits.MaxVertexInputBindings;
		public uint MaxVertexAttribOffset => DeviceInfo.Limits.MaxVertexInputAttributeOffset;
		public uint MaxVertexBindingStride => DeviceInfo.Limits.MaxVertexInputBindingStride;
		public uint MaxVertexStageOutputComponents => DeviceInfo.Limits.MaxVertexOutputComponents;
		public uint MaxTessellationGenerationLevel => DeviceInfo.Limits.MaxTessellationGenerationLevel;
		public uint MaxTessellationPatchSize => DeviceInfo.Limits.MaxTessellationPatchSize;
		public uint MaxTessellationControlInputComponents => DeviceInfo.Limits.MaxTessellationControlPerVertexInputComponents;
		public uint MaxTessellationControlPerVertexOutputComponents => DeviceInfo.Limits.MaxTessellationControlPerVertexOutputComponents;
		public uint MaxTessellationControlPerPatchOutputComponents => DeviceInfo.Limits.MaxTessellationControlPerPatchOutputComponents;
		public uint MaxTessellationControlTotalOutputComponents => DeviceInfo.Limits.MaxTessellationControlTotalOutputComponents;
		public uint MaxTessellationEvaluationInputComponents => DeviceInfo.Limits.MaxTessellationEvaluationInputComponents;
		public uint MaxTessellationEvaluationOutputComponents => DeviceInfo.Limits.MaxTessellationEvaluationOutputComponents;
		public uint MaxGeometryShaderInvocations => DeviceInfo.Limits.MaxGeometryShaderInvocations;
		public uint MaxGeometryInputComponents => DeviceInfo.Limits.MaxGeometryInputComponents;
		public uint MaxGeometryOutputComponents => DeviceInfo.Limits.MaxGeometryOutputComponents;
		public uint MaxGeometryOutputVertices => DeviceInfo.Limits.MaxGeometryOutputVertices;
		public uint MaxGeometryTotalOutputComponents => DeviceInfo.Limits.MaxGeometryTotalOutputComponents;

		public (float, float) PointSizeRange {
			get {
				var range = DeviceInfo.Limits.PointSizeRange;
				return (range.X, range.Y);
			}
		}

		public (float, float) LineWidthRange {
			get {
				var range = DeviceInfo.Limits.LineWidthRange;
				return (range.X, range.Y);
			}
		}

		public float PointSizeGranularity => DeviceInfo.Limits.PointSizeGranularity;

		public float LineWidthGranularity => DeviceInfo.Limits.LineWidthGranularity;

		public VulkanGraphicsLimits(VulkanPhysicalDeviceInfo info) {
			DeviceInfo = info;
		}

	}

	public class VulkanGraphics : IGraphics {

		public VulkanGraphicsContext Context { get; }
		public VulkanDevice Device { get; }
		public VulkanMemory Memory { get; }
		public VulkanCommands Commands { get; }

		public IGraphicsProperites Properties { get; }

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits { get; }

		public IBuffer CreateBuffer(BufferCreateInfo createInfo) {
			VKBufferUsageFlagBits usage = 0;
			if ((createInfo.Usage & BufferUsage.VertexBuffer) != 0) usage |= VKBufferUsageFlagBits.VertexBuffer;
			if ((createInfo.Usage & BufferUsage.IndexBuffer) != 0) usage |= VKBufferUsageFlagBits.IndexBuffer;

			VKBuffer buffer = Device.Device.CreateBuffer(new VKBufferCreateInfo() {
				Type = VKStructureType.BufferCreateInfo,
				Size = createInfo.Size,
				Usage = usage,
				SharingMode = Device.ResourceSharingMode,
				QueueFamilyIndexCount = (uint)Device.ResourceSharingIndices.ArraySize,
				QueueFamilyIndices = Device.ResourceSharingIndices
			});

			return new VulkanBuffer(this, buffer, createInfo);
		}

		public IVertexArray CreateVertexArray(VertexArrayCreateInfo createInfo) {
			return new VulkanVertexArray() {
				Format = createInfo.Format,
				IndexBuffer = createInfo.IndexBuffer != null ? (
					(VulkanBuffer)createInfo.IndexBuffer.Value.Item1.Buffer,
					createInfo.IndexBuffer.Value.Item1.Range,
					VulkanConverter.Convert(createInfo.IndexBuffer.Value.Item2)
				) : null,
				VertexBuffers = createInfo.VertexBuffers != null ? Array.ConvertAll(createInfo.VertexBuffers, binding => (
					(VulkanBuffer)binding.Item1.Buffer,
					binding.Item1.Range, binding.Item2
				)) : null
			};
		}

		public IFramebuffer CreateFramebuffer(FramebufferCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			UnmanagedPointer<ulong> pAttachments = sp.Alloc<ulong>(createInfo.Attachments.Length);
			for (int i = 0; i < pAttachments.ArraySize; i++) pAttachments[i] = ((VulkanTextureView)createInfo.Attachments[i]).ImageView;
			return new VulkanFramebuffer(Device.Device.CreateFramebuffer(new VKFramebufferCreateInfo() {
				Type = VKStructureType.FramebufferCreateInfo,
				RenderPass = ((VulkanRenderPass)createInfo.RenderPass).RenderPass,
				AttachmentCount = (uint)pAttachments.ArraySize,
				Attachments = pAttachments,
				Width = (uint)createInfo.Size.X,
				Height = (uint)createInfo.Size.Y,
				Layers = createInfo.Layers
			}), createInfo.Size, createInfo.Layers);
		}

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			VKPipelineCache vkcache = (createInfo.Cache is VulkanPipelineCache cache) ? cache.PipelineCache : null;
			List<IDisposable> disposables = new();
			try {
				if (createInfo.GraphicsInfo != null) return new VulkanPipeline(
					Device.Device.CreateGraphicsPipelines(vkcache, VulkanConverter.ConvertGraphicsPipeline(sp, createInfo, disposables)),
					VKPipelineBindPoint.Graphics
				);
				else if (createInfo.ComputeInfo != null) return new VulkanPipeline(
					Device.Device.CreateComputePipeline(vkcache, VulkanConverter.ConvertComputePipeline(createInfo)),
					VKPipelineBindPoint.Compute
				);
				else throw new VulkanException("Cannot determine type of pipeline to create");
			} finally {
				foreach (IDisposable d in disposables) d.Dispose();
			}
		}

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo) {
			byte[] initData = createInfo.InitialData;
			unsafe {
				fixed (byte* pInitData = initData) {
					return new VulkanPipelineCache(Device.Device.CreatePipelineCache(new VKPipelineCacheCreateInfo() {
						Type = VKStructureType.PipelineCacheCreateInfo,
						InitialDataSize = (nuint)(initData != null ? initData.Length : 0),
						InitialData = (IntPtr)pInitData
					}));
				}
			}
		}

		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanPipelineLayout(Device.Device.CreatePipelineLayout(new VKPipelineLayoutCreateInfo() {
				Type = VKStructureType.PipelineLayoutCreateInfo,
				SetLayoutCount = (uint)createInfo.Layouts.Length,
				SetLayouts = sp.Values(createInfo.Layouts.ConvertAll(layout => ((VulkanBindSetLayout)layout).Layout)),
				PushConstantRangeCount = (uint)(createInfo.PushConstantRanges?.Length).GetValueOrDefault(0),
				PushConstantRanges = createInfo.PushConstantRanges != null ? sp.Values(createInfo.PushConstantRanges.ConvertAll(VulkanConverter.Convert)) : IntPtr.Zero
			}));
		}

		public IBindSetLayout CreateBindSetLayout(BindSetLayoutCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanBindSetLayout(Device.Device.CreateDescriptorSetLayout(new VKDescriptorSetLayoutCreateInfo() {
				Type = VKStructureType.DescriptorSetLayoutCreateInfo,
				BindingCount = (uint)createInfo.Bindings.Length,
				Bindings = sp.Values(createInfo.Bindings.ConvertAll(VulkanConverter.Convert))
			}), createInfo);
		}

		public IBindPool CreateBindPool(BindPoolCreateInfo createInfo) => new VulkanBindPool(Device, createInfo);

		public IPipelineSet CreatePipelineSet(PipelineSetCreateInfo createInfo) => new VulkanPipelineSet(this, createInfo);

		public IRenderPass CreateRenderPass(RenderPassCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanRenderPass(Device.Device.CreateRenderPass(VulkanConverter.Convert(sp, createInfo)));
		}

		public ISampler CreateSampler(SamplerCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			IntPtr next = IntPtr.Zero;
			if (createInfo.BorderColor == SamplerBorderColor.Custom) {
				if (!Device.Device.EXTCustomBorderColor)
					throw new VulkanException("Cannot define sampler with custom border color without VK_EXT_custom_border_color");
				if (!Device.PhysicalDevice.CustomBorderColorFeaturesEXT.Value.CustomBorderColors)
					throw new VulkanException("Cannot define sampler with custom border color when feature is not available");
				next = sp.Values(new VKSamplerCustomBorderColorCreateInfoEXT() {
					Type = VKStructureType.SAMPLER_CUSTOM_BORDER_COLOR_CREATE_INFO_EXT,
					Next = next,
					CustomBorderColor = VulkanConverter.ConvertClearColor(createInfo.CustomBorderColor),
					Format = VulkanConverter.Convert(createInfo.Format)
				});
			}
			return new VulkanSampler(Device.Device.CreateSampler(new VKSamplerCreateInfo() {
				Type = VKStructureType.SamplerCreateInfo,
				Next = next,
				MagFilter = VulkanConverter.ConvertFilter(createInfo.MagnifyFilter),
				MinFilter = VulkanConverter.ConvertFilter(createInfo.MinifyFilter),
				MipmapMode = VulkanConverter.ConvertMipmapMode(createInfo.MipmapMode),
				AddressModeU = VulkanConverter.Convert(createInfo.AddressMode.X),
				AddressModeV = VulkanConverter.Convert(createInfo.AddressMode.Y),
				AddressModeW = VulkanConverter.Convert(createInfo.AddressMode.Z),
				MipLodBias = createInfo.MipLODBias,
				AnisotropyEnable = createInfo.AnisotropyEnable,
				MaxAnisotropy = createInfo.MaxAnisotropy,
				CompareEnable = createInfo.CompareEnable,
				CompareOp = VulkanConverter.Convert(createInfo.CompareOp),
				MinLod = createInfo.LODRange.Item1,
				MaxLod = createInfo.LODRange.Item2,
				BorderColor = VulkanConverter.Convert(createInfo.BorderColor, createInfo.Format),
				UnnormalizedCoordinates = false
			}));
		}

		public IShader CreateShader(ShaderCreateInfo createInfo) {
			if (createInfo.SourceType != ShaderSourceType.SPIRV) throw new VulkanException("Vulkan graphics only supports SPIR-V shader sources");

			ReadOnlyMemory<int> spirv = default;
			IConstPointer<int> pspirv = default;
			if (createInfo.Source is int[] arr) spirv = arr;
			else if (createInfo.Source is IReadOnlyList<int> lst) spirv = lst.ToArray();
			else if (createInfo.Source is ReadOnlyMemory<int> mem) spirv = mem;
			else if (createInfo.Source is IConstPointer<int> ptr) {
				pspirv = ptr;
				if (pspirv.ArraySize < 0) throw new ArgumentException("Pointer SPIR-V source type must have explicit length", nameof(createInfo));
			} else throw new ArgumentException("Supplied shader source is not a valid SPIR-V source type", nameof(createInfo));

			unsafe {
				fixed(int* pSpirv = spirv.Span) {
					IntPtr pCode = (IntPtr)pSpirv;
					int length = spirv.Length;
					if (pspirv != null) {
						pCode = pspirv.Ptr;
						length = pspirv.ArraySize;
					}

					return new VulkanShader(Device.Device.CreateShaderModule(new VKShaderModuleCreateInfo() {
						Type = VKStructureType.ShaderModuleCreateInfo,
						Code = pCode,
						CodeSize = (nuint)length
					}));
				}
			}

		}

		public ISync CreateSync(SyncCreateInfo createInfo) {
			bool OnlyHasFeatures(SyncFeatures features) => (createInfo.Features & ~features) == 0;
			bool HasCompatibleGranularity(SyncGranularity granularity) => granularity switch {
				SyncGranularity.CommandBuffer => createInfo.Granularity == SyncGranularity.CommandBuffer,
				SyncGranularity.Command => createInfo.Granularity != SyncGranularity.PipelineStage,
				SyncGranularity.PipelineStage => true,
				_ => false,
			};
			switch (createInfo.Direction) {
				case SyncDirection.GPUToHost: // Fence
					if (!OnlyHasFeatures(SyncFeatures.GPUWorkSignaling | SyncFeatures.HostPolling | SyncFeatures.HostWaiting)) break;
					if (!HasCompatibleGranularity(SyncGranularity.CommandBuffer)) break;
					return new VulkanFenceSync(Device.Device.CreateFence(new VKFenceCreateInfo() {
						Type = VKStructureType.FenceCreateInfo
					}));
				case SyncDirection.GPUToGPU: // Semaphore
					if (!OnlyHasFeatures(SyncFeatures.GPUWorkSignaling | SyncFeatures.GPUWorkWaiting | SyncFeatures.GPUMultiQueue)) break;
					if (!HasCompatibleGranularity(SyncGranularity.CommandBuffer)) break;
					return new VulkanSemaphoreSync(Device.Device.CreateSemaphore(new VKSemaphoreCreateInfo() {
						Type = VKStructureType.SemaphoreCreateInfo
					}));
				case SyncDirection.Any: // Event
					if (!OnlyHasFeatures(SyncFeatures.GPUSignaling | SyncFeatures.GPUWaiting | SyncFeatures.HostPolling | SyncFeatures.HostSignaling | SyncFeatures.HostWaiting)) break;
					if (!HasCompatibleGranularity(SyncGranularity.PipelineStage)) break;
					return new VulkanEventSync(Device.Device.CreateEvent(new VKEventCreateInfo() {
						Type = VKStructureType.EventCreateInfo
					}));
				default:
					break;
			}
			throw new ArgumentException("Unsupported combination of sync creation information", nameof(createInfo));
		}

		public ITexture CreateTexture(TextureCreateInfo createInfo) {
			VKImage image = Device.Device.CreateImage(new VKImageCreateInfo() {
				Type = VKStructureType.ImageCreateInfo,
				Flags = VulkanConverter.ConvertImageCreateFlags(createInfo.Type),
				ImageType = VulkanConverter.ConvertImageType(createInfo.Type),
				Format = VulkanConverter.Convert(createInfo.Format),
				Extent = (Vector3ui)createInfo.Size,
				MipLevels = createInfo.MipLevels,
				ArrayLayers = createInfo.ArrayLayers,
				Samples = VulkanConverter.ConvertSampleCount(createInfo.Samples),
				Tiling = VKImageTiling.Optimal,
				Usage = VulkanConverter.Convert(createInfo.Usage),
				SharingMode = Device.ResourceSharingMode,
				QueueFamilyIndexCount = (uint)Device.ResourceSharingIndices.ArraySize,
				QueueFamilyIndices = Device.ResourceSharingIndices,
				InitialLayout = VKImageLayout.Undefined
			});

			VulkanMemoryBinding binding;
			if (createInfo.MemoryBinding is VulkanMemoryBinding b) binding = b;
			else binding = Memory.Allocate(createInfo, image);

			binding.Bind(image);

			return new VulkanTexture(image, true) {
				Type = createInfo.Type,
				Format = createInfo.Format,
				Size = createInfo.Size,
				MipLevels = createInfo.MipLevels,
				ArrayLayers = createInfo.ArrayLayers,
				Samples = createInfo.Samples,
				Usage = createInfo.Usage,
				MemoryBinding = binding
			};
		}

		public ITextureView CreateTextureView(TextureViewCreateInfo createInfo) {
			return new VulkanTextureView(Device.Device.CreateImageView(new VKImageViewCreateInfo() {
				Type = VKStructureType.ImageViewCreateInfo,
				Image = ((VulkanTexture)createInfo.Texture).Image,
				ViewType = VulkanConverter.ConvertImageViewType(createInfo.Type),
				Format = VulkanConverter.Convert(createInfo.Format),
				Components = VulkanConverter.Convert(createInfo.Mapping),
				SubresourceRange = VulkanConverter.Convert(createInfo.SubresourceRange)
			}), createInfo);
		}

		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo) => Commands.Alloc(createInfo);

		public void RunCommands(Action<ICommandSink> cmdSink, CommandBufferUsage usage, in IGraphics.CommandBufferSubmitInfo submitInfo) {
			// Allocate and record command buffer
			usage |= CommandBufferUsage.OneTimeSubmit;
			VulkanCommandBuffer cmdbuf = Commands.Alloc(new CommandBufferCreateInfo() {
				Type = CommandBufferType.Primary,
				Usage = usage
			});
			cmdSink(cmdbuf.BeginRecording());
			cmdbuf.EndRecording();
			
			// Determine if a fence already exists
			VulkanFenceSync fence = null;
			foreach(ISync sig in submitInfo.SignalSync) {
				if (sig is VulkanFenceSync fenceSync) {
					if (fence != null) throw new VulkanException("Can only signal a single fence during command submission");
					else fence = fenceSync;
				}
			}

			// If no fence, create a new one and add to the list of signal syncs
			ISync[] signalSyncs = submitInfo.SignalSync.ToArray();

			bool disposeFence = fence == null;
			if (disposeFence) {
				fence = new VulkanFenceSync(Device.Device.CreateFence(new VKFenceCreateInfo() {
					Type = VKStructureType.FenceCreateInfo
				}));
				Array.Resize(ref signalSyncs, signalSyncs.Length + 1);
				signalSyncs[^1] = fence;
			}

			// Make sure the fence is reset and submit the commands
			fence.HostReset();
			var submitInfo2 = new IGraphics.CommandBufferSubmitInfo() {
				CommandBuffer = new ICommandBuffer[] { cmdbuf },
				SignalSync = submitInfo.SignalSync,
				WaitSync = submitInfo.WaitSync
			};
			SubmitCommands(submitInfo2);

			// Schedule the command buffer to be disposed when the fence indicates it is free
			Commands.DisposeWhenFree(cmdbuf, fence, disposeFence);
		}

		public void SubmitCommands(in IGraphics.CommandBufferSubmitInfo submitInfo) {
			
		}

		public void TrimCommandBufferMemory() => Commands.Trim();

		public void WaitIdle() => Commands.WaitIdle();

		public VulkanGraphics(VulkanGraphicsContext context) {
			Context = context;
			Device = new VulkanDevice(context);
			Memory = new VulkanMemory(Device);
			Commands = new VulkanCommands(context, Device);

			Properties = new VulkanGraphicsProperties(Device, Memory);
			Features = new VulkanGraphicsFeatures(Device);
			Limits = new VulkanGraphicsLimits(Device.PhysicalDevice);
		}

	}

	public class VulkanGraphicsContext {

		/// <summary>
		/// The instance used to create the graphics.
		/// </summary>
		[DisallowNull]
		public VKInstance Instance { get; init; }

		/// <summary>
		/// The preferred physical device to use, or null.
		/// </summary>
		public VKPhysicalDevice PreferredPhysicalDevice { get; init; } = null;

		/// <summary>
		/// The collection of required device extensions, or null.
		/// </summary>
		public IReadOnlyCollection<string> RequiredDeviceExtensions { get; init; } = null;

		/// <summary>
		/// The collection of preferred device extensions, or null.
		/// </summary>
		public IReadOnlyCollection<string> PreferredDeviceExtensions { get; init; } = null;

		/// <summary>
		/// The weighting of preferred extensions used when calculating device scores.
		/// </summary>
		public float ExtensionWeight { get; init; } = 1.0f;

		/// <summary>
		/// Function to use to score physical devices, or null to use the built-in function.
		/// </summary>
		public Func<VulkanPhysicalDeviceInfo, float> ScoreFunc { get; init; } = null;

		/// <summary>
		/// THe amount of parallelism to use when creating command pools. By default this
		/// is set to the processor count so that threads have a low chance of blocking
		/// waiting for exclusive access to a command pool.
		/// </summary>
		public int CommandPoolParallelism { get; init; } = Environment.ProcessorCount;

		/// <summary>
		/// A collection of required surfaces to test for compatibility with physical devices.
		/// </summary>
		public IReadOnlyCollection<VKSurfaceKHR> RequiredCompatibleSurfaces { get; init; } = null;

		/// <summary>
		/// The threshold of orphaned command buffers above which they will attempted to be garbage collected.
		/// </summary>
		public int OrphanedCommandGCThreshold { get; init; } = 256;

	}

}
