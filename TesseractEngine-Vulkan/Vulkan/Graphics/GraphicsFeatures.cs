using System.Collections.Generic;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;
using Tesseract.Vulkan.Graphics.Impl;

namespace Tesseract.Vulkan.Graphics {
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
				TextureCompressionASTC_LDR = features.TextureCompressionASTC_LDR,
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

		public bool FramebufferBlitTextureSubView => false; // We only have vkCmdBlitImage, so no reinterpreting via image views

		public bool LimitedTextureBlit => false; // We don't have limitations on texture blitting in Vulkan

		public VulkanGraphicsFeatures(VulkanDevice device) {
			HardwareFeatures = FromVK(device.PhysicalDevice);
			SupportedDynamicStates = new FuncReadOnlyIndexer<PipelineDynamicState, bool, HashSet<PipelineDynamicState>>(
				dynamicStates, (HashSet<PipelineDynamicState> set, PipelineDynamicState state) => set.Contains(state)
			);

			VKDevice logicalDevice = device.Device;
			if (logicalDevice.EXTCustomBorderColor) {
				SamplerCustomBorderColor = device.PhysicalDevice.CustomBorderColorFeaturesEXT!.Value.CustomBorderColors;
			}
			if (logicalDevice.EXTLineRasterization != null) {
				var features = device.PhysicalDevice.LineRasterizationFeaturesEXT!.Value;
				StrictLines = !(features.BresenhamLines || features.RectangularLlines || features.SmoothLines);
			}
			if (logicalDevice.EXTExtendedDynamicState != null) {
				var features = device.PhysicalDevice.ExtendedDynamicStateFeaturesEXT!.Value;
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

}
