using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of general types of graphics environments.
	/// </summary>
	public enum GraphicsType {
		/// <summary>
		/// Unknown type.
		/// </summary>
		Unknown,
		/// <summary>
		/// OpenGL graphics.
		/// </summary>
		OpenGL,
		/// <summary>
		/// Vulkan graphics.
		/// </summary>
		Vulkan,
		/// <summary>
		/// DirectX graphics.
		/// </summary>
		DirectX
	}

	/// <summary>
	/// Interface describing properties about a graphics object.
	/// </summary>
	public interface IGraphicsProperites {

		/// <summary>
		/// The type of graphics API this object uses.
		/// </summary>
		public GraphicsType Type { get; }

		/// <summary>
		/// String describing additional information about the type of graphics.
		/// </summary>
		public string TypeInfo { get; }

		/// <summary>
		/// The name of the rendering device or system used by the underlying API (often but not explicitly a hardware device).
		/// </summary>
		public string RendererName { get; }

		/// <summary>
		/// The name of the vendor supplying the implementation of the underlying API or hardware.
		/// </summary>
		public string VendorName { get; }

		/// <summary>
		/// The thread safety level of the graphics API.
		/// </summary>
		public ThreadSafetyLevel APIThreadSafety { get; }


		/// <summary>
		/// The total amount of available video memory. Note that some of this will be in use by other software so not all of it will actually be available.
		/// This statistic also includes host-side system memory usable by the GPU, but some of this will also be in use by other software. If this statistic
		/// is not available this value is set to 0. The precision of this value varies between implementation.
		/// </summary>
		public ulong TotalVideoMemory { get; }

		/// <summary>
		/// The total amount of available device memory. Note that some of this will be in use by other software so not all of it will actually be available.
		/// This statistic only includes memory known to be local to the device (for GPUs this is their dedicated memory, while for APUs this is the total
		/// amount of accessible memory. Other types of devices may report different values). If this statistic is not available this value is set to 0.
		/// The precision of this value varies between implementation.
		/// </summary>
		public ulong TotalDeviceMemory { get; }

		/// <summary>
		/// The total amount of memory this graphics instance has actually committed to it. If this statistic is not available this value is set to 0.
		/// The precision of this value varies between implementation.
		/// </summary>
		public ulong TotalCommittedMemory { get; }

	}

	/// <summary>
	/// Flags for features whose support is hardware/driver dependent and must be specified
	/// during graphics creation.
	/// </summary>
	public record GraphicsHardwareFeatures {

		/// <summary>
		/// Indicates buffer accesses by GPU commands are bounds-checked and will not cause undefined behavior when out of bounds access occurs.
		/// </summary>
		public bool RobustBufferAccess { get; init; }

		/// <summary>
		/// The full 32-bit range of an index value is usable when 32-bit indices are specified. Otherwise, only up to 24-bits of the index value is guarenteed to be used.
		/// </summary>
		public bool FullDrawIndexUInt32 { get; init; }

		/// <summary>
		/// Cube map textures with arrays may be created.
		/// </summary>
		public bool CubeMapArray { get; init; }

		/// <summary>
		/// Attachment blending settings may be independently controlled. Otherwise, all attachments must have the same blending settings.
		/// </summary>
		public bool IndependentBlend { get; init; }

		/// <summary>
		/// Geometry shaders may be used.
		/// </summary>
		public bool GeometryShader { get; init; }

		/// <summary>
		/// Tessellation sahders may be used.
		/// </summary>
		public bool TessellationShader { get; init; }

		/// <summary>
		/// A sample rate may be set for multisampling.
		/// </summary>
		public bool SampleRateShading { get; init; }

		/// <summary>
		/// Blend operations may take two source values.
		/// </summary>
		public bool DualSrcBlend { get; init; }

		/// <summary>
		/// Color attachment logic operations are supported.
		/// </summary>
		public bool LogicOp { get; init; }

		/// <summary>
		/// Draw indirect counts may be greater than 1, otherwise they can only be 1.
		/// </summary>
		public bool MultiDrawIndirect { get; init; }

		/// <summary>
		/// Draw indirects support unqie first instance parameters, otherwise they must be 0.
		/// </summary>
		public bool DrawIndirectFirstInstance { get; init; }

		/// <summary>
		/// If depth clamping is supported.
		/// </summary>
		public bool DepthClamp { get; init; }

		/// <summary>
		/// If depth bias clamping is supported.
		/// </summary>
		public bool DepthBiasClamp { get; init; }

		/// <summary>
		/// If polygon modes other than <see cref="PolygonMode.Fill"/> are supported.
		/// </summary>
		public bool FillModeNonSolid { get; init; }

		/// <summary>
		/// If depth bounds tests are supported.
		/// </summary>
		public bool DepthBounds { get; init; }

		/// <summary>
		/// Line widths other than 1.0 are supported.
		/// </summary>
		public bool WideLines { get; init; }

		/// <summary>
		/// Point sizes greater than 1.0 are supported.
		/// </summary>
		public bool LargePoints { get; init; }

		/// <summary>
		/// Alpha-to-one behavior is supported for multisampling.
		/// </summary>
		public bool AlphaToOne { get; init; }

		/// <summary>
		/// Multiple independent viewports and scissors are supported.
		/// </summary>
		public bool MultiViewport { get; init; }

		/// <summary>
		/// Anisotropic filtering is supported.
		/// </summary>
		public bool SamplerAnisotropy { get; init; }

		/// <summary>
		/// ETC2 and EAC compressed formats are supported.
		/// </summary>
		public bool TextureCompressionETC2 { get; init; }

		/// <summary>
		/// ASTC LDR compressed formats are supported.
		/// </summary>
		public bool TextureCompressionASTC { get; init; }

		/// <summary>
		/// BC compressed formats are supported.
		/// </summary>
		public bool TextureCompressionBC { get; init; }

		/// <summary>
		/// Occlusion queries returning actual sample counts are supported.
		/// </summary>
		public bool OcclusionQueryPrecise { get; init; }

		/// <summary>
		/// If pipeline statistics queries are supported.
		/// </summary>
		public bool PipelineStatisticsQuery { get; init; }

		/// <summary>
		/// If vertex-processing shader stages (vertex, tessellation, geometry) support stores and atomic operations.
		/// </summary>
		public bool VertexPipelineStoresAndAtomics { get; init; }

		/// <summary>
		/// If fragment shaders support stores and atomics.
		/// </summary>
		public bool FragmentStoresAndAtomics { get; init; }

		/// <summary>
		/// If the <c>PointSize</c> built-in is available in tessellation and geometry shader stages.
		/// </summary>
		public bool ShaderTessellationAndGeometryPointSize { get; init; }

		/// <summary>
		/// If extended SPIR-V image gather instructions are available.
		/// </summary>
		public bool ShaderImageGatherExtended { get; init; }

		/// <summary>
		/// Storage images may use the following formats:
		/// <list type="bullet">
		/// <item>R16G16SFloat</item>
		/// <item>B10G11R11UFloat</item>
		/// <item>R16G16B16A16UNorm</item>
		/// <item>A1B10G10R10UNorm</item>
		/// <item>R16G16UNorm</item>
		/// <item>R8G8UNorm</item>
		/// <item>R16UNorm</item>
		/// <item>R8UNorm</item>
		/// <item>R16G16B16A16SNorm</item>
		/// <item>R16G16SNorm</item>
		/// <item>R8G8SNorm</item>
		/// <item>R16SNorm</item>
		/// <item>R8SNorm</item>
		/// <item>R16G16SInt</item>
		/// <item>R8G8SInt</item>
		/// <item>R16SInt</item>
		/// <item>R8SInt</item>
		/// <item>A1B10G10R10UInt</item>
		/// <item>R16G16UInt</item>
		/// <item>R8G8UInt</item>
		/// <item>R16UInt</item>
		/// <item>R8UInt</item>
		/// </list>
		/// </summary>
		public bool ShaderStorageImageExtendedFormats { get; init; }

		/// <summary>
		/// If storage images may be multisampled.
		/// </summary>
		public bool ShaderStorageImageMultisample { get; init; }

		/// <summary>
		/// If storage images can be read without specifying a format.
		/// </summary>
		public bool ShaderStorageImageReadWithoutFormat { get; init; }

		/// <summary>
		/// If storage images can be written without specifying a format.
		/// </summary>
		public bool ShaderStorageImageWriteWithoutFormat { get; init; }

		/// <summary>
		/// If arrays of uniform buffers can be indexed by uniform variables.
		/// </summary>
		public bool ShaderUniformBufferArrayDynamicIndexing { get; init; }

		/// <summary>
		/// If arrays of samplers or sampled images can be indexed by uniform variables.
		/// </summary>
		public bool ShaderSampledImageArrayDynamicIndexing { get; init; }

		/// <summary>
		/// If arrays of storage buffers can be indexed by uniform variables.
		/// </summary>
		public bool ShaderStorageBufferArrayDynamicIndexing { get; init; }

		/// <summary>
		/// If arrays of storage images can be indexed by uniform variables.
		/// </summary>
		public bool ShaderStorageImageArrayDynamicIndexing { get; init; }

		/// <summary>
		/// If <c>ClipDistance</c> built-in values are supported by shader code.
		/// </summary>
		public bool ShaderClipDistance { get; init; }

		/// <summary>
		/// If <c>CullDistance</c> built-in values are supported by shader code.
		/// </summary>
		public bool ShaderCullDistance { get; init; }

		/// <summary>
		/// If 64-bit floats are supported by shaders.
		/// </summary>
		public bool ShaderFloat64 { get; init; }

		/// <summary>
		/// If 64-bit integers are supported by shaders.
		/// </summary>
		public bool ShaderInt64 { get; init; }

		/// <summary>
		/// If 16-bit integers are supported by shaders.
		/// </summary>
		public bool ShaderInt16 { get; init; }

		/// <summary>
		/// If shaders can query residency information for shader resources.
		/// </summary>
		public bool ShaderResourceResidency { get; init; }

		/// <summary>
		/// If shaders can specify minimum level-of-detail values in image operations.
		/// </summary>
		public bool ShaderResourceMinLOD { get; init; }

		/// <summary>
		/// If sparse memory binding is supported.
		/// </summary>
		public bool SparseBinding { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for buffers.
		/// </summary>
		public bool SparseResidencyBuffer { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 2D images with 1 sample per pixel.
		/// </summary>
		public bool SparseResidencyImage2D { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 3D images with 1 sample per pixel.
		/// </summary>
		public bool SparseResidencyImage3D { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 2D images with 2 samples per pixel.
		/// </summary>
		public bool SparseResidency2Samples { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 2D images with 4 samples per pixel.
		/// </summary>
		public bool SparseResidency4Samples { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 2D images with 8 samples per pixel.
		/// </summary>
		public bool SparseResidency8Samples { get; init; }

		/// <summary>
		/// If sparse memory residency is supported for 2D images with 16 samples per pixel.
		/// </summary>
		public bool SparseResidency16Samples { get; init; }

		/// <summary>
		/// If sparsely bound memory may be bound to multiple locations simultaneously.
		/// </summary>
		public bool SparseResidencyAliased { get; init; }

		/*
		/// <summary>
		/// 
		/// </summary>
		public bool VariableMultisampleRate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool InheritedQueries { get; set; }
		*/

		/// <summary>
		/// If draw indirection is supported in some form.
		/// </summary>
		public bool DrawIndirect { get; init; }

	}

	public interface IGraphicsFeatures {

		public GraphicsHardwareFeatures HardwareFeatures { get; }

		/// <summary>
		/// If multisampling uses "standard" sample locations (see the <see href="https://www.khronos.org/registry/vulkan/specs/1.2-extensions/html/vkspec.html#primsrast-multisampling">
		/// Vulkan Spec</see> for details on what standard locations are). This value may be false if the location of sample positions cannot be determined, even if they may match
		/// the standard locations.
		/// </summary>
		public bool StandardSampleLocations { get; }

		/// <summary>
		/// If the rasterization of lines is limited to a backend-specific method. Otherwise, the method of line rasterization may be specified.
		/// </summary>
		public bool StrictLines { get; }

		/// <summary>
		/// Getter to test if different dynamic pipeline states are available.
		/// </summary>
		public IReadOnlyIndexer<PipelineDynamicState,bool> SupportedDynamicStates { get; }

		/// <summary>
		/// If push constants are supported.
		/// </summary>
		public bool PushConstants { get; }

		/// <summary>
		/// If sub-views of a texture are supported.
		/// </summary>
		public bool TextureSubView { get; }

	}

	public interface IGraphicsLimits {

		/// <summary>
		/// Maximum dimension for a 1D texture.
		/// </summary>
		public uint MaxTextureDimension1D { get; }

		/// <summary>
		/// Maximum dimensions for a 2D texture.
		/// </summary>
		public uint MaxImageDimension2D { get; }

		/// <summary>
		/// Maximum dimensions for a 3D texture.
		/// </summary>
		public uint MaxTextureDimension3D { get; }

		/// <summary>
		/// Maximum dimensions for cube map textures.
		/// </summary>
		public uint MaxTextureDimensionCube { get; }

		/// <summary>
		/// Maximum number of layers in arrayed textures.
		/// </summary>
		public uint MaxTextureArrayLayers { get; }

		/// <summary>
		/// Maximum number of addressable texels in a buffer view.
		/// </summary>
		public uint MaxTexelBufferElements { get; }

		/// <summary>
		/// Maximum number of bytes that can be bound as a uniform buffer.
		/// </summary>
		public uint MaxUniformBufferRange { get; }

		/// <summary>
		/// Maximum number of bytes that can be bound as a storage buffer.
		/// </summary>
		public uint MaxStorageBufferRange { get; }

		/// <summary>
		/// Maximum number of bytes that can be used in push constants.
		/// </summary>
		public uint MaxPushConstantSize { get; }

		/// <summary>
		/// The maximum number of sampler objects that can exist simultaneously.
		/// </summary>
		public uint MaxSamplerObjects { get; }

		/// <summary>
		/// The total amount of address space available for sparse resources.
		/// </summary>
		public ulong SparseAddressSpaceSize { get; }

		/// <summary>
		/// The maximum number of bind sets that can be concurrently bound.
		/// </summary>
		public uint MaxBoundSets { get; }

		/// <summary>
		/// The maximum number of samplers that are available to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageSamplers { get; }

		/// <summary>
		/// The maximum number of uniform buffers that are available to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageUniformBuffers { get; }

		/// <summary>
		/// The maximum number of shader storage buffers that are available to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageStorageBuffers { get; }

		/// <summary>
		/// The maximum number of sampled images that are accessible to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageSampledImages { get; }

		/// <summary>
		/// The maximum number of storage images that are accessible to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageStorageImages { get; }

		/// <summary>
		/// The maximum number of input attachments that are accessible to a single pipeline stage.
		/// </summary>
		public uint MaxPerStageInputAttachments { get; }

		/// <summary>
		/// The maximum number of shader resources (ie. uniform/storage/texel buffers and samplers/images/sampled images) that are accessible to a pipeline stage.
		/// </summary>
		public uint MaxPerStageResources { get; }

		/// <summary>
		/// The maximum number of samplers that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutSamplers { get; }

		/// <summary>
		/// The maximum number of uniform buffers that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutUniformBuffers { get; }

		/// <summary>
		/// The maximum number of dynamically indexable uniform buffers that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutDynamicUniformBuffers { get; }

		/// <summary>
		/// The maximum number of storage buffers that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutStorageBuffers { get; }

		/// <summary>
		/// The maximum number of dynamically indexable storage buffers that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutDynamicStorageBuffers { get; }

		/// <summary>
		/// The maximum number of sampled images that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutSampledImages { get; }

		/// <summary>
		/// The maximum number of storage images that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutStorageImages { get; }

		/// <summary>
		/// The maximum number of input attachments that can be included in a pipeline layout.
		/// </summary>
		public uint MaxPerLayoutInputAttachments { get; }

		/// <summary>
		/// The maximum number of vertex attributes that can be specified.
		/// </summary>
		public uint MaxVertexAttribs { get; }

		/// <summary>
		/// The maximum number of vertex bindings that can be specified.
		/// </summary>
		public uint MaxVertexBindings { get; }

		/// <summary>
		/// The maximum offset of a vertex attribute.
		/// </summary>
		public uint MaxVertexAttribOffset { get; }

		/// <summary>
		/// The maximum stride between vertices in a binding.
		/// </summary>
		public uint MaxVertexBindingStride { get; }

		/// <summary>
		/// The maximum number of total components that can be output from a vertex shader.
		/// </summary>
		public uint MaxVertexStageOutputComponents { get; }

		// TODO: GL_MAX_TESS_GEN_LEVEL
		/// <summary>
		/// The maximum tessellation generation level supported by the fixed-function tesellator.
		/// </summary>
		public uint MaxTessellationGenerationLevel { get; }

		// TODO: GL_MAX_PATCH_VERTICES
		/// <summary>
		/// The maximum patch size, in vertices, of patches that can be processed in tessellation shaders.
		/// </summary>
		public uint MaxTessellationPatchSize { get; }

		// TODO: GL_MAX_TESS_CONTROL_INPUT_COMPONENTS
		/// <summary>
		/// The maximum number of total components that can be input to a tessellation control shader.
		/// </summary>
		public uint MaxTessellationControlInputComponents { get; }

		// TODO: GL_MAX_TESS_CONTROL_OUITPUT_COMPONENTS
		/// <summary>
		/// The maximum number of total components that can be output per-vertex from a tessellation control shader.
		/// </summary>
		public uint MaxTessellationControlPerVertexOutputComponents { get; }

		// TODO: GL_MAX_TESS_PATCH_COMPONENTS
		/// <summary>
		/// The maximum number of total components that can be output per-patch from a tessellation control shader.
		/// </summary>
		public uint MaxTessellationControlPerPatchOutputComponents { get; }

		// TODO: GL_MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS
		/// <summary>
		/// The maximum number of total components that can be output both per-vertex and per-patch from a tessellation control shader.
		/// </summary>
		public uint MaxTessellationControlTotalOutputComponents { get; }

		/// <summary>
		/// The maximum number of total components that can be input to a tessellation evaluation shader.
		/// </summary>
		public uint MaxTessellationEvaluationInputComponents { get; }

		/// <summary>
		/// The maximum number of total components that can be output from a tessellation evaluation sahder.
		/// </summary>
		public uint MaxTessellationEvaluationOutputComponents { get; }

		// TODO: GL_MAX_GEOMETRY_SHADER_INVOCATIONS
		/// <summary>
		/// The maximum invocation count for instanced geometry shaders.
		/// </summary>
		public uint MaxGeometryShaderInvocations { get; }

		public uint MaxGeometryInputComponents { get; }

		public uint MaxGeometryOutputComponents { get; }

		public uint MaxGeometryOutputVertices { get; }

		public uint MaxGeometryTotalOutputComponents { get; }

		/// <summary>
		/// The range of accepted values for the size of point geometry. The first value is the lower bound and the second is the upper.
		/// </summary>
		public (float, float) PointSizeRange { get; }

		/// <summary>
		/// The range of accepted values for the width of line geometry. The first value is the lower bound and the second is the upper.
		/// </summary>
		public (float, float) LineWidthRange { get; }

		/// <summary>
		/// Fraction specifying the granularity of point size values.
		/// </summary>
		public float PointSizeGranularity { get; }

		/// <summary>
		/// Fraction specifying the granularity of line width values.
		/// </summary>
		public float LineWidthGranularity { get; }

	}

	/// <summary>
	/// An graphics object provides an interface for rendering 3D graphics
	/// using a lower-level API (OpenGL, Vulkan, etc.).
	/// </summary>
	public interface IGraphics {

		/// <summary>
		/// The properties of this graphics object.
		/// </summary>
		public IGraphicsProperites Properties { get; }

		/// <summary>
		/// The features supported by this graphics object.
		/// </summary>
		public IGraphicsFeatures Features { get; }

		/// <summary>
		/// The limits of this graphics object.
		/// </summary>
		public IGraphicsLimits Limits { get; }

		//=================//
		// Object Creation //
		//=================//

		/// <summary>
		/// 
		/// </summary>
		/// <param name="createInfo"></param>
		/// <returns></returns>
		public IBuffer CreateBuffer(BufferCreateInfo createInfo);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="createInfo"></param>
		/// <returns></returns>
		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo);

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo);

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo);

		//==============================//
		// Command Buffers & Submission //
		//==============================//

		/// <summary>
		/// 
		/// </summary>
		/// <param name="createInfo"></param>
		/// <returns></returns>
		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo);

		/// <summary>
		/// Command buffer submission information.
		/// </summary>
		public readonly ref struct CommandBufferSubmitInfo {

			/// <summary>
			/// The command buffers to submit.
			/// </summary>
			public ReadOnlySpan<ICommandBuffer> CommandBuffer { get; init; }
			
			/// <summary>
			/// List of sync objects to wait on and their respective pipeline stages.
			/// </summary>
			public ReadOnlySpan<(ISync, PipelineStage)> WaitSync { get; init; }

			/// <summary>
			/// List of sync objects to signal once all commands in the buffer are completed.
			/// </summary>
			public ReadOnlySpan<ISync> SignalSync { get; init; }

		}
		
		/// <summary>
		/// Runs the supplied commands once. Note that the command buffers in the supplied submission
		/// info are ignored and the provided method is used to generate the commands instead. The synchronization
		/// parameters provided in the submission info are respected for the generated commands.
		/// </summary>
		/// <param name="cmdSink">The method that will supply the commands</param>
		/// <param name="submitInfo">Submission info for the commands</param>
		public void RunCommands(Action<ICommandSink> cmdSink, in CommandBufferSubmitInfo submitInfo);

		/// <summary>
		/// Submits command buffers for execution, setting up the required synchronization for the commands.
		/// <para>
		/// </para>
		/// </summary>
		/// <param name="submitInfo"></param>
		public void SubmitCommands(in CommandBufferSubmitInfo submitInfo);

		/// <summary>
		/// <para>Trims the memory used by command buffers.</para>
		/// <para>
		///		Memory for command buffers is managed internally by the implementation, and even when a command buffer is disposed
		///		its memory may not be released back to the system. Instead, the memory will be returned to an internal command pool
		///		where it may be reused for other commands. When this function is called the implementation will attempt to return
		///		this pooled memory back to the system without disturbing any existing command buffers. While this may reduce the
		///		program's memory usage the actual benefits will depend on how command buffers are used and attempting to trim
		///		memory has its own performance overhead.
		/// </para>
		/// </summary>
		public void TrimCommandBufferMemory();

	}

}
