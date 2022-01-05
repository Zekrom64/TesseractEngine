using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;

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
			MaxPushConstantSize = 0;
			MaxSamplerObjects = uint.MaxValue; // OpenGL defines no explicit limit on the number of objects
			SparseAddressSpaceSize = 0; // Sparse resources not supported in OpenGL
			MaxBoundSets = uint.MaxValue; // Technically no limit since OpenGL's binding works differently

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

			MaxGeometryOutputVertices = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_OUTPUT_VERTICES);
			MaxGeometryTotalOutputComponents = (uint)gl.GL11.GetInteger(Native.GLEnums.GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS);

			Span<float> pointSizeRange = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_RANGE, (stackalloc float[2]));
			PointSizeRange = (pointSizeRange[0], pointSizeRange[1]);
			Span<float> lineWidthRange = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_RANGE, (stackalloc float[2]));
			LineWidthRange = (lineWidthRange[0], lineWidthRange[1]);
			PointSizeGranularity = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_GRANULARITY);
			LineWidthGranularity = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_GRANULARITY);
		}

	}

}
