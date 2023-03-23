using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenGL.Graphics {

	public class GLGraphicsLimits : IGraphicsLimits {

		public uint MaxTextureDimension1D { get; }

		public uint MaxImageDimension2D { get; }

		public uint MaxTextureDimension3D { get; }

		public uint MaxTextureDimensionCube { get; }

		public uint MaxTextureArrayLayers { get; }

		public uint MaxTexelBufferElements { get; }

		public uint MaxUniformBufferRange { get; }

		public uint MaxStorageBufferRange { get; }

		public uint MaxPushConstantSize { get; }

		public uint MaxSamplerObjects { get; }

		public ulong SparseAddressSpaceSize { get; }

		public uint MaxBoundSets { get; }

		public uint MaxPerStageSamplers { get; }

		public uint MaxPerStageUniformBuffers { get; }

		public uint MaxPerStageStorageBuffers { get; }

		public uint MaxPerStageSampledImages { get; }

		public uint MaxPerStageStorageImages { get; }

		public uint MaxPerStageInputAttachments { get; }

		public uint MaxPerStageResources { get; }

		public uint MaxPerLayoutSamplers { get; }

		public uint MaxPerLayoutUniformBuffers { get; }

		public uint MaxPerLayoutDynamicUniformBuffers { get; }

		public uint MaxPerLayoutStorageBuffers { get; }

		public uint MaxPerLayoutDynamicStorageBuffers { get; }

		public uint MaxPerLayoutSampledImages { get; }

		public uint MaxPerLayoutStorageImages { get; }

		public uint MaxPerLayoutInputAttachments { get; }

		public uint MaxVertexAttribs { get; }

		public uint MaxVertexBindings { get; }

		public uint MaxVertexAttribOffset { get; }

		public uint MaxVertexBindingStride { get; }

		public uint MaxVertexStageOutputComponents { get; }

		public uint MaxTessellationGenerationLevel { get; }

		public uint MaxTessellationPatchSize { get; }

		public uint MaxTessellationControlInputComponents { get; }

		public uint MaxTessellationControlPerVertexOutputComponents { get; }

		public uint MaxTessellationControlPerPatchOutputComponents { get; }

		public uint MaxTessellationControlTotalOutputComponents { get; }

		public uint MaxTessellationEvaluationInputComponents { get; }

		public uint MaxTessellationEvaluationOutputComponents { get; }

		public uint MaxGeometryShaderInvocations { get; }

		public uint MaxGeometryInputComponents { get; }

		public uint MaxGeometryOutputComponents { get; }

		public uint MaxGeometryOutputVertices { get; }

		public uint MaxGeometryTotalOutputComponents { get; }

		public uint MaxFragmentInputComponents { get; }

		public uint MaxFragmentOutputAttachments { get; }

		public uint MaxFragmentDualSrcAttachments { get; }

		public (float, float) PointSizeRange { get; }

		public (float, float) LineWidthRange { get; }

		public float PointSizeGranularity { get; }

		public float LineWidthGranularity { get; }

		public GLGraphicsLimits(GL gl) {
			MaxTextureDimension1D = MaxImageDimension2D = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TEXTURE_SIZE);
			MaxTextureDimension3D = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_3D_TEXTURE_SIZE);
			MaxTextureDimensionCube = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_CUBE_MAP_TEXTURE_SIZE);
			MaxTextureArrayLayers = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_ARRAY_TEXTURE_LAYERS);
			MaxTexelBufferElements = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TEXTURE_BUFFER_SIZE);
			MaxUniformBufferRange = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_UNIFORM_BLOCK_SIZE);
			MaxStorageBufferRange = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_SHADER_STORAGE_BLOCK_SIZE);

			MaxPushConstantSize = 0; // Push constants not supported in OpenGL
			MaxSamplerObjects = uint.MaxValue; // OpenGL defines no explicit limit on the number of objects
			SparseAddressSpaceSize = 0; // Sparse resources not supported in OpenGL
			MaxBoundSets = uint.MaxValue; // Technically no limit since OpenGL's binding works differently

			MaxFragmentInputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_FRAGMENT_INPUT_COMPONENTS);
			MaxFragmentOutputAttachments = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_COLOR_ATTACHMENTS);
			MaxFragmentDualSrcAttachments = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_DUAL_SOURCE_DRAW_BUFFERS);

			bool hasTessellationShaders = gl.ARBTessellationShader != null;
			bool hasGeometryShader = gl.ARBGeometryShader4 != null;

			/*
			int maxPerStageSamplers = Math.Min(
				gl.GL11.GetInteger(GLEnums.GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS),
				gl.GL11.GetInteger(GLEnums.GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS)
			);
			maxPerStageSamplers = Math.Min(maxPerStageSamplers, gl.GL11.GetInteger(GLEnums.GL_MAX_FRAGMENT_IMAGE_UNIFORMS));
			MaxPerStageSamplers = (uint)maxPerStageSamplers;
			gl.GL11.GetInteger(GLEnums.GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS),
				gl.GL11.GetInteger(GLEnums.GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS),
			);
			*/

			int maxPerStageUniformBuffers = Math.Min(
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_UNIFORM_BLOCKS),
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_FRAGMENT_UNIFORM_BLOCKS)
			);
			int maxPerStageStorageBuffers = Math.Min(
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS),
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS)
			);
			int maxPerStageTextureUnits = Math.Min(
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS),
				gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TEXTURE_IMAGE_UNITS)
			);

			if (hasTessellationShaders) {
				maxPerStageUniformBuffers = ExMath.Min(maxPerStageUniformBuffers, gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_UNIFORM_BLOCKS), gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_EVALUATION_UNIFORM_BLOCKS));
				maxPerStageStorageBuffers = ExMath.Min(maxPerStageStorageBuffers, gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS), gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS));
				maxPerStageTextureUnits = ExMath.Min(maxPerStageStorageBuffers, gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS), gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS));
			}

			if (hasGeometryShader) {
				maxPerStageUniformBuffers = Math.Min(maxPerStageUniformBuffers, gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_UNIFORM_BLOCKS));
			}

			MaxPerStageUniformBuffers = (uint)maxPerStageUniformBuffers;
			MaxPerStageStorageBuffers = (uint)maxPerStageStorageBuffers;
			MaxPerStageSamplers = MaxPerStageSampledImages = MaxPerStageStorageImages = (uint)maxPerStageTextureUnits;
			MaxPerStageInputAttachments = 0;

			MaxPerStageResources = MaxPerStageUniformBuffers + MaxPerStageStorageBuffers + MaxPerStageSampledImages + MaxFragmentOutputAttachments;

			// Assign max layout limits based on max per stage values (since these are shared between all stages in OpenGL)
			MaxPerLayoutSamplers = MaxPerStageSamplers;
			MaxPerLayoutUniformBuffers = MaxPerStageUniformBuffers;
			MaxPerLayoutDynamicUniformBuffers = 0; // Not supported yet
			MaxPerLayoutStorageBuffers = MaxPerStageStorageBuffers;
			MaxPerLayoutDynamicStorageBuffers = 0; // Not supported yet
			MaxPerLayoutSampledImages = MaxPerStageSampledImages;
			MaxPerLayoutStorageImages = MaxPerStageStorageImages;
			MaxPerLayoutInputAttachments = 0; // Not supported

			MaxVertexAttribs = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_ATTRIBS);
			MaxVertexBindings = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_ATTRIB_BINDINGS);
			MaxVertexAttribOffset = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET);
			MaxVertexBindingStride = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_ATTRIB_STRIDE);
			MaxVertexStageOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_VERTEX_OUTPUT_COMPONENTS);

			if (hasTessellationShaders) {
				MaxTessellationGenerationLevel = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_GEN_LEVEL);
				MaxTessellationPatchSize = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_PATCH_VERTICES);
				MaxTessellationControlInputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_INPUT_COMPONENTS);
				MaxTessellationControlPerVertexOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_OUTPUT_COMPONENTS);
				MaxTessellationControlPerPatchOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_PATCH_COMPONENTS);
				MaxTessellationControlTotalOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS);
				MaxTessellationEvaluationInputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_EVALUATION_INPUT_COMPONENTS);
				MaxTessellationEvaluationOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_TESS_EVALUATION_OUTPUT_COMPONENTS);
			} else {
				MaxTessellationGenerationLevel = 0;
				MaxTessellationPatchSize = 0;
				MaxTessellationControlInputComponents = 0;
				MaxTessellationControlPerVertexOutputComponents = 0;
				MaxTessellationControlPerPatchOutputComponents = 0;
				MaxTessellationControlTotalOutputComponents = 0;
				MaxTessellationEvaluationInputComponents = 0;
				MaxTessellationEvaluationOutputComponents = 0;
			}

			if (hasGeometryShader) {
				MaxGeometryShaderInvocations = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_SHADER_INVOCATIONS);
				MaxGeometryInputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_INPUT_COMPONENTS);
				MaxGeometryOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_OUTPUT_COMPONENTS);
				MaxGeometryOutputVertices = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_OUTPUT_VERTICES);
				MaxGeometryTotalOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS);
			} else {
				MaxGeometryShaderInvocations = 0;
				MaxGeometryInputComponents = 0;
				MaxGeometryOutputComponents = 0;
				MaxGeometryOutputVertices = 0;
				MaxGeometryTotalOutputComponents = 0;
			}

			Span<float> pointSizeRange = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_RANGE, (stackalloc float[2]));
			PointSizeRange = (pointSizeRange[0], pointSizeRange[1]);
			Span<float> lineWidthRange = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_RANGE, (stackalloc float[2]));
			LineWidthRange = (lineWidthRange[0], lineWidthRange[1]);
			PointSizeGranularity = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_GRANULARITY);
			LineWidthGranularity = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_GRANULARITY);
		}

	}

}
