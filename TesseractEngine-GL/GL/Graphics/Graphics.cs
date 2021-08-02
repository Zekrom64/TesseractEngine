using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;
using Tesseract.GL.Native;

namespace Tesseract.GL.Graphics {

	public class GLGraphics : IGraphics {

		public class GLGraphicsProperties : IGraphicsProperites {

			public GraphicsType Type => GraphicsType.OpenGL;

			public string TypeInfo => "Tesseract GLCore Graphics";

			public string RendererName { get; }

			public string VendorName { get; }

			public ThreadSafetyLevel APIThreadSafety => ThreadSafetyLevel.SingleThread;

			// TODO: Use extensions to reflect video memory info
			public ulong TotalVideoMemory => 0;

			public ulong TotalDeviceMemory => 0;

			public ulong TotalCommittedMemory => 0;

			public GLGraphicsProperties(GL gl) {
				RendererName = gl.GL11.GetString(Native.GLEnums.GL_RENDERER);
				VendorName = gl.GL11.GetString(Native.GLEnums.GL_VENDOR);
			}

		}

		public IGraphicsProperites Properties { get; }

		public class GLGraphicsFeatures : IGraphicsFeatures {

			public static uint GetRequiredContextFlags(GraphicsHardwareFeatures features) {
				uint flags = 0;
				if (features.RobustBufferAccess) flags |= Native.GLEnums.GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT;
				return flags;
			}

			public GraphicsHardwareFeatures HardwareFeatures { get; }

			public bool StandardSampleLocations => false; // *Could* be determined by ARB_sample_locations but for now this is too complex

			public bool StrictLines => true; // Line drawing is hidden in OpenGL

			// All dynamic states are supported
			public IReadOnlyIndexer<PipelineDynamicState, bool> SupportedDynamicStates { get; } = new FuncReadOnlyIndexer<PipelineDynamicState, bool>((PipelineDynamicState _) => true);

			public bool PushConstants => false; // No push constants

			public bool TextureSubView { get; } // Supported by ARB_texture_view

			public GLGraphicsFeatures(GL gl, GraphicsHardwareFeatures features) {
				int contextFlags = gl.GL11.GetInteger(Native.GLEnums.GL_CONTEXT_FLAGS);

				Span<float> lineWidthRange = gl.GL11.GetFloat(Native.GLEnums.GL_LINE_WIDTH_RANGE, (stackalloc float[2]));
				Span<float> pointSizeRange = gl.GL11.GetFloat(Native.GLEnums.GL_POINT_SIZE_RANGE, (stackalloc float[2]));
				HardwareFeatures = features with {
					RobustBufferAccess = features.RobustBufferAccess && ((contextFlags & Native.GLEnums.GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT) != 0),
					TessellationShader = features.TessellationShader && gl.ARBTessellationShader != null,
					MultiDrawIndirect = features.MultiDrawIndirect && false, // TODO: Check against ARB_multi_draw_indirect
					DrawIndirectFirstInstance = features.DrawIndirectFirstInstance && false, // TODO: Check against ARB_base_instance
					DepthClamp = features.DepthClamp && false, // TODO: Check against ARB_depth_clamp
					DepthBiasClamp = features.DepthBiasClamp && false, // TODO: Check against ARB_polygon_offset_clamp
					WideLines = features.WideLines && lineWidthRange[1] > 1.0f,
					LargePoints = features.LargePoints && pointSizeRange[1] > 1.0f,
					MultiViewport = features.MultiViewport && false, // TODO: Check against ARB_viewport_array
					SamplerAnisotropy = features.SamplerAnisotropy && false, // TODO: Check against ARB/EXT_texture_filter_anisotropic

				};

				TextureSubView = gl.Extensions.Contains("GL_ARB_texture_view"); // TODO: Check against extension object
			}

		}

		public IGraphicsFeatures Features { get; }

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
				SparseAddressSpaceSize = 0; // TODO: Sparse resources
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

		public IGraphicsLimits Limits { get; }

		public IGLContext Context { get; }

		public GL GL { get; }

		public GLGraphics(IGLContext context, GraphicsHardwareFeatures features) {
			if (context.MajorVersion < 3 || (context.MajorVersion == 3 && context.MinorVersion < 3)) throw new ArgumentException("OpenGL accelerated graphics requires OpenGL version >=3.3", nameof(context));
			Context = context;
			GL = new GL(context);

			Properties = new GLGraphicsProperties(GL);
			Features = new GLGraphicsFeatures(GL, features);
			Limits = new GLGraphicsLimits(GL);
		}

		public IBuffer CreateBuffer(BufferCreateInfo createInfo) => new GLBuffer(GL, createInfo);

		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo) {
			throw new NotImplementedException();
		}

		public void RunCommands(Action<ICommandSink> cmdSink, in IGraphics.CommandBufferSubmitInfo submitInfo) {
			throw new NotImplementedException();
		}

		public void SubmitCommands(in IGraphics.CommandBufferSubmitInfo submitInfo) {
			throw new NotImplementedException();
		}

		public void TrimCommandBufferMemory() { } // No-op

	}

}
