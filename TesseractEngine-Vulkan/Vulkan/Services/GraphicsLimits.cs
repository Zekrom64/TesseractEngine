using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Vulkan.Services.Objects;

namespace Tesseract.Vulkan.Services {
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

}
