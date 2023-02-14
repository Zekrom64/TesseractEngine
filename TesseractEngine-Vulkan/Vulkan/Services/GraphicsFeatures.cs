using System.Collections.Generic;
using Tesseract.Core.Collections;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Vulkan.Services.Objects;

namespace Tesseract.Vulkan.Services {
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
				DrawIndirect = true,

				DynamicRendering = info.Extensions.Contains(KHRDynamicRendering.ExtensionName)
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

		public bool LimitedTextureCopy => false; // We don't have limitations on texture copying in Vulkan

		public bool LimitedTextureCopyToBuffer => false; // We don't have limitations on texture-to-buffer copying in Vulkan

		public VulkanGraphicsFeatures(VulkanPhysicalDeviceInfo device, VulkanDevice? logicalDevice = null) {
			HardwareFeatures = FromVK(device);
			SupportedDynamicStates = new FuncReadOnlyIndexer<PipelineDynamicState, bool, HashSet<PipelineDynamicState>>(
				dynamicStates, (HashSet<PipelineDynamicState> set, PipelineDynamicState state) => set.Contains(state)
			);

			SamplerCustomBorderColor = device.CustomBorderColorFeaturesEXT?.CustomBorderColors ?? false;
			if (device.LineRasterizationPropertiesEXT != null) {
				if (logicalDevice == null || logicalDevice.Device.EXTLineRasterization != null) {
					var features = device.LineRasterizationFeaturesEXT!.Value;
					StrictLines = !(features.BresenhamLines || features.RectangularLlines || features.SmoothLines);
				}
			}
			if (device.ExtendedDynamicStateFeaturesEXT != null) {
				if (logicalDevice == null || logicalDevice.Device.EXTExtendedDynamicState != null) {
					var features = device.ExtendedDynamicStateFeaturesEXT!.Value;
					if (features.ExtendedDynamicState) {
						dynamicStates.Add(PipelineDynamicState.CullMode);
						dynamicStates.Add(PipelineDynamicState.FrontFace);
						dynamicStates.Add(PipelineDynamicState.DrawMode);
						dynamicStates.Add(PipelineDynamicState.DepthTestEnable);
						dynamicStates.Add(PipelineDynamicState.DepthWriteEnable);
						dynamicStates.Add(PipelineDynamicState.DepthCompareOp);
						dynamicStates.Add(PipelineDynamicState.DepthBoundsTestEnable);
						dynamicStates.Add(PipelineDynamicState.StencilTestEnable);
						dynamicStates.Add(PipelineDynamicState.StencilOp);
					}
				}
			}
			if (device.ExtendedDynamicState2FeaturesEXT != null) {
				if (logicalDevice == null || logicalDevice.Device.EXTExtendedDynamicState2) {
					var features = device.ExtendedDynamicState2FeaturesEXT!.Value;
					if (features.ExtendedDynamicState2) {
						dynamicStates.Add(PipelineDynamicState.DepthBiasEnable);
						dynamicStates.Add(PipelineDynamicState.PrimitiveRestartEnable);
						dynamicStates.Add(PipelineDynamicState.RasterizerDiscardEnable);
					}
					if (features.ExtendedDynamicState2LogicOp) dynamicStates.Add(PipelineDynamicState.LogicOp);
					if (features.ExtendedDyanmicState2PatchControlPoints) dynamicStates.Add(PipelineDynamicState.PatchControlPoints);
				}
			}
		}

	}

}
