using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// Enumeration of pipeline types.
	/// </summary>
	public enum PipelineType {
		/// <summary>
		/// A graphics pipeline.
		/// </summary>
		Graphics,
		/// <summary>
		/// A compute pipeline.
		/// </summary>
		Compute
	}

	/// <summary>
	/// A pipeline stage specifies a part of the pipeline that performs certain operations. Pipeline stages
	/// are stored as a bitmask so multiple stages may be specified in certain parameters.
	/// </summary>
	public enum PipelineStage {
		/// <summary>
		/// Pipeline stage which all commands pass through before any other stage.
		/// </summary>
		Top = 0x00000001,
		/// <summary>
		/// Pipeline stage where indirect drawing parameters are fetched.
		/// </summary>
		DrawIndirect = 0x00000002,
		/// <summary>
		/// Pipeline stage where index values and vertex attributes are fetched.
		/// </summary>
		VertexInput = 0x00000004,
		/// <summary>
		/// Pipeline stage for vertex shader execution.
		/// </summary>
		VertexShader = 0x00000008,
		/// <summary>
		/// Pipeline stage for tesselation control shader execution.
		/// </summary>
		TessellationControlShader = 0x00000010,
		/// <summary>
		/// Pipeline stage for tessellation evaluation shader execution.
		/// </summary>
		TessellationEvaluationShader = 0x00000020,
		/// <summary>
		/// Pipeline stage for geometry shader execution.
		/// </summary>
		GeometryShader = 0x00000040,
		/// <summary>
		/// Pipeline stage for fragment shader execution.
		/// </summary>
		FragmentShader = 0x00000080,
		/// <summary>
		/// 
		/// </summary>
		EarlyFragmentTests = 0x00000100,
		LateFragmentTests = 0x00000200,
		/// <summary>
		/// Pipeline stage where final color values are written back to color attachments and multisampling resolve
		/// operations are performed.
		/// </summary>
		ColorAttachmentOutput = 0x00000400,
		/// <summary>
		/// Pipeline stage for compute shader execution.
		/// </summary>
		ComputeShader = 0x00000800,
		/// <summary>
		/// Pipeline stage which buffer and image copying, blitting, and clearing commands occur in.
		/// </summary>
		Transfer = 0x00001000,
		/// <summary>
		/// Pipeline stage which all commands pass through last.
		/// </summary>
		Bottom = 0x00002000,
		/// <summary>
		/// Pipeline stage performing reads or writes on host-side memory.
		/// </summary>
		Host = 0x00004000,

		/// <summary>
		/// All graphics-related stages.
		/// </summary>
		AllGraphics = 0x00008000,
		/// <summary>
		/// All available pipeline states.
		/// </summary>
		AllCommands = 0x00010000
	}

	public interface IPipelineLayout : IDisposable { }

	/// <summary>
	/// Pipeline layout creation information.
	/// </summary>
	public struct PipelineLayoutCreateInfo {

	}

	public interface IPipelineCache : IDisposable { }

	/// <summary>
	/// Pipeline cache creation information.
	/// </summary>
	public struct PipelineCacheCreateInfo { }

	/// <summary>
	/// A pipeline object specifies how drawing and dispatch commands issued to the GPU are processed.
	/// </summary>
	public interface IPipeline : IDisposable { }

	/// <summary>
	/// The information for a single shader stage for a pipeline.
	/// </summary>
	public struct PipelineShaderStageInfo {

		/// <summary>
		/// The type of shader stage this information is for.
		/// </summary>
		public ShaderType Type { get; set; }

		/// <summary>
		/// The shader to use for this shader stage.
		/// </summary>
		public IShader Shader { get; set; }

		/// <summary>
		/// The name of the entry point for this shader stage in the shader object.
		/// If the entry point is the default value of <c>null</c> it is assumed that
		/// the entry point is called "<c>main</c>".
		/// </summary>
		public string EntryPoint { get; set; }

	}

	/// <summary>
	/// The stencil test state for a pipeline.
	/// </summary>
	public struct PipelineStencilState {

		/// <summary>
		/// The stencil operation to perform if the stencil test fails.
		/// </summary>
		public StencilOp FailOp { get; set; }

		/// <summary>
		/// The stencil operation to perform if the depth and stencil test pass.
		/// </summary>
		public StencilOp PassOp { get; set; }

		/// <summary>
		/// The stencil operation to perform if the stencil test passes but the depth test fails.
		/// </summary>
		public StencilOp DepthFailOp { get; set; }

		/// <summary>
		/// The comparison operation to perform for the stencil test.
		/// </summary>
		public CompareOp CompareOp { get; set; }

		/// <summary>
		/// The bitwise mask to apply to values before comparison.
		/// </summary>
		public uint CompareMask { get; set; }

		/// <summary>
		/// The bitwise mask specifying which bits are written back to the stencil buffer.
		/// </summary>
		public uint WriteMask { get; set; }

		/// <summary>
		/// Reference value used in stencil comparison.
		/// </summary>
		public uint Reference { get; set; }

		public override int GetHashCode() {
			int hash = ((int)FailOp) ^ (((int)PassOp) << 3) ^ (((int)DepthFailOp) << 6) ^ (((int)CompareOp) << 9);
			hash ^= (int)CompareMask;
			hash ^= (int)(WriteMask << 4);
			hash ^= (int)(Reference << 8);
			return hash;
		}

	}

	public struct PipelineColorAttachmentState {

		public bool BlendEnable { get; set; }

		public BlendFactor SrcColorBlendFactor { get; set; }

		public BlendFactor DstColorBlendFactor { get; set; }

		public BlendOp ColorBlendOp { get; set; }

		public BlendFactor SrcAlphaBlendFactor { get; set; }

		public BlendFactor DstAlphaBlendFactor { get; set; }

		public BlendOp AlphaBlendOp { get; set; }

		public ColorComponent ColorWriteMask { get; set; }

		public override int GetHashCode() {
			int hash = ((int)SrcColorBlendFactor) ^ (((int)DstColorBlendFactor) << 4) ^ (((int)SrcAlphaBlendFactor) << 8) ^ (((int)DstAlphaBlendFactor) << 12);
			hash ^= ((int)ColorBlendOp) | (((int)AlphaBlendOp) << 3) | (((int)ColorWriteMask) << 6);
			if (BlendEnable) hash = ~hash;
			return hash;
		}

	}

	public enum PipelineDynamicState {
		Viewport,
		Scissor,
		LineWidth,
		DepthBias,
		BlendConstants,
		DepthBounds,
		StencilCompareMask,
		StencilWriteMask,
		StencilReference,
		// TODO: VK_EXT_extended_dynamic_state
		CullMode,
		FrontFace,
		DrawMode,
		DepthTest,
		DepthWriteEnable,
		DepthCompareOp,
		DepthBoundsTestEnable,
		StencilTestEnable,
		StencilOp,
		// TODO: VK_EXT_extended_dynamic_state2
		PatchControlPoints,
		RasterizerDiscardEnable,
		DepthBiasEnable,
		LogicOp,
		PrimitiveRestartEnable,
		// TODO: VK_EXT_vertex_input_dynamic_state
		VertexFormat,
		// TODO: VK_EXT_color_write_enable
		ColorWrite
	}

	/// <summary>
	/// Pipeline creation information that may be dynamically variable. 
	/// </summary>
	public struct PipelineDynamicCreateInfo {

		// Vertex input state

		/// <summary>
		/// The format that vertex attributes are fetched into the pipeline from vertex buffers.
		/// </summary>
		public VertexFormat VertexFormat { get; set; }

		// Input assembly state

		/// <summary>
		/// The drawing mode of primitives rendered by the pipeline.
		/// </summary>
		public DrawMode DrawMode { get; set; }

		/// <summary>
		/// If vertex indices of -1 can be used to restart rendering of compound draw modes (ie. strips, fans, lists).
		/// </summary>
		public bool PrimitiveRestartEnable { get; set; }

		// Tessellation state

		/// <summary>
		/// The number of control points for every patch for tessellation.
		/// </summary>
		public uint? PatchControlPoints { get; set; }

		// Viewport state

		/// <summary>
		/// The viewports used by this pipeline. Ignored if <see cref="PipelineDynamicState.Viewport"/> is specified.
		/// </summary>
		public Viewport[] Viewports { get; set; }

		/// <summary>
		/// The scissors used by this pipeline. Ignored if <see cref="PipelineDynamicState.Scissor"/> is specified.
		/// </summary>
		public Recti[] Scissors { get; set; }

		// Rasterization state

		/// <summary>
		/// If geometry should be discarded instead of being passed to the rasterizer.
		/// </summary>
		public bool RasterizerDiscardEnable { get; set; }

		/// <summary>
		/// The culling mode of the pipeline.
		/// </summary>
		public CullMode CullMode { get; set; }

		/// <summary>
		/// The front face specification for culling.
		/// </summary>
		public FrontFace FrontFace { get; set; }

		/// <summary>
		/// The width of generated lines when in drawing and polygon modes which generate lines.
		/// </summary>
		public float LineWidth { get; set; }

		/// <summary>
		/// If depth biasing is enabled.
		/// </summary>
		public bool DepthBiasEnable { get; set; }

		// Depth/stencil state

		/// <summary>
		/// If depth testing is enabled.
		/// </summary>
		public bool DepthTestEnable { get; set; }

		/// <summary>
		/// If writes to the depth buffer are enabled.
		/// </summary>
		public bool DepthWriteEnable { get; set; }

		/// <summary>
		/// The comparison to use when performing the depth test.
		/// </summary>
		public CompareOp DepthCompareOp { get; set; }

		/// <summary>
		/// If depth bounds testing is enabled.
		/// </summary>
		public bool DepthBoundsTestEnable { get; set; }

		/// <summary>
		/// If stencil testing is enabled.
		/// </summary>
		public bool StencilTestEnable { get; set; }

		/// <summary>
		/// The stencil state for front-facing geometry.
		/// </summary>
		public PipelineStencilState FrontStencilState { get; set; }

		/// <summary>
		/// The stencil state for back-facing geometry.
		/// </summary>
		public PipelineStencilState BackStencilState { get; set; }

		/// <summary>
		/// The minimum value for depth bounds testing.
		/// </summary>
		public float MinDepthBounds { get; set; }

		/// <summary>
		/// The maximum value for depth bounds testing.
		/// </summary>
		public float MaxDepthBounds { get; set; }

		// Color blend state

		/// <summary>
		/// The logic operation to perform on color attachments.
		/// </summary>
		public LogicOp LogicOp { get; set; }

		/// <summary>
		/// The blend constant values to use for color blending.
		/// </summary>
		public Vector4 BlendConstant { get; set; }

	}

	public struct PipelineGraphicsCreateInfo {
		
		// Shader state

		public PipelineShaderStageInfo[] Shaders { get; set; }

		// Viewport state

		/// <summary>
		/// The number of viewports used by this pipeline. Ignored if <see cref="PipelineDynamicState.Viewport"/> is not specified.
		/// </summary>
		public uint? ViewportCount { get; set; }

		/// <summary>
		/// The number of scissors used by this pipeline. Ignored if <see cref="PipelineDynamicState.Scissor"/> is not specified.
		/// </summary>
		public uint? ScissorCount { get; set; }

		// Rasterization state

		public bool DepthClampEnable { get; set; }

		public PolygonMode PolygonMode { get; set; }

		public float DepthBiasConstantFactor { get; set; }

		public float DepthBiasClamp { get; set; }

		public float DepthBiasSlopeFactor { get; set; }

		// Color blend state

		public bool LogicOpEnable { get; set; }

		public PipelineColorAttachmentState[] Attachments { get; set; }

		// Dynamic state

		public PipelineDynamicCreateInfo DynamicInfo { get; set; }

		public PipelineDynamicState[] DynamicState { get; set; }

	}

	public struct PipelineComputeCreateInfo {

		/// <summary>
		/// The compute shader binding.
		/// </summary>
		public PipelineShaderStageInfo Shader { get; set; } 

	}

	/// <summary>
	/// Pipeline creation information.
	/// </summary>
	public struct PipelineCreateInfo {

		/// <summary>
		/// The pipeline cache to generate this pipeline from.
		/// </summary>
		public IPipelineCache Cache { get; set; }

		/// <summary>
		/// The layout of the pipeline.
		/// </summary>
		public IPipelineLayout Layout { get; set; }

		/// <summary>
		/// Graphics pipeline creation information.
		/// </summary>
		public PipelineGraphicsCreateInfo? GraphicsInfo { get; set; }

		/// <summary>
		/// Compute pipeline creation information.
		/// </summary>
		public PipelineComputeCreateInfo? ComputeInfo { get; set; }

		/// <summary>
		/// A base pipeline to create the pipeline from. Providing a base pipeline may make pipeline creation and binding
		/// more efficient for congruent pipelines.
		/// </summary>
		public IPipeline BasePipeline { get; set; }

		/// <summary>
		/// The index of the creation information for the base pipeline similar to <see cref="BasePipeline"/>.
		/// This is only checked during bulk creation of pipelines.
		/// </summary>
		public int? BasePipelineIndex { get; set; }

	}

	/// <summary>
	/// <para>
	/// A pipeline set provides a way of managing multiple similar pipelines with different dynamic parameters.
	/// </para>
	/// <para>
	/// The basic creation information for the pipeline is provided when the set is created and used to derive
	/// how binding is performed with dynamic parameters and what parameters should be used to generate
	/// new pipelines if changed. If a pipeline cache is provided all new pipelines will be created from the
	/// cache as well.
	/// </para>
	/// </summary>
	public interface IPipelineSet : IDisposable { }
	
	/// <summary>
	/// Creation information for a pipeline set.
	/// </summary>
	public struct PipelineSetCreateInfo {

		/// <summary>
		/// The pipeline creation information.
		/// </summary>
		public PipelineCreateInfo CreateInfo { get; set; }

	}

}
