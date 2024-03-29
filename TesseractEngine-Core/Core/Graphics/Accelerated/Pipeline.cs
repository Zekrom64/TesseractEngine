﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Tesseract.Core.Collections;
using Tesseract.Core.Numerics;

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
	[Flags]
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
		/// Pipeline stage for early fragment tests before fragment shading is performed.
		/// </summary>
		EarlyFragmentTests = 0x00000100,
		/// <summary>
		/// Pipeline stage for late fragment tests after fragment shading is performed.
		/// </summary>
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

	/// <summary>
	/// A pipeline layout describes how bind sets and push constants are connected to a pipeline.
	/// </summary>
	public interface IPipelineLayout : IDisposable { }

	/// <summary>
	/// Push constant range descriptor.
	/// </summary>
	public struct PushConstantRange {

		/// <summary>
		/// The shader stages this range is used in.
		/// </summary>
		public required ShaderType Stages;

		/// <summary>
		/// The offset of this range of push constants.
		/// </summary>
		public uint Offset;

		/// <summary>
		/// The size of this range of push constants.
		/// </summary>
		public required uint Size;

	}

	/// <summary>
	/// Pipeline layout creation information.
	/// </summary>
	public record PipelineLayoutCreateInfo {

		/// <summary>
		/// The collection of bind set layouts that are part of the entire pipeline layout.
		/// </summary>
		public IReadOnlyCollection<IBindSetLayout> Layouts { get; init; } = Array.Empty<IBindSetLayout>();

		/// <summary>
		/// The collection of push constant ranges used by the pipeline layout.
		/// </summary>
		public IReadOnlyCollection<PushConstantRange> PushConstantRanges { get; init; } = Array.Empty<PushConstantRange>();

	}

	/// <summary>
	/// <para>
	/// A pipeline cache can be supplied during pipeline creation to provide some reuse of
	/// resources used by pipelines. Doing so may be more efficient than creating each
	/// pipeline separately.
	/// </para>
	/// <para>
	/// Some caches support storing cache data persistently via <see cref="Data"/> which can
	/// be passed during creation to preinitialize the cache. However, this data is
	/// implementation-dependent and should only be provided if the graphics provider's
	/// GUIDs are the same between the cached data and the current graphics.
	/// </para>
	/// </summary>
	public interface IPipelineCache : IDisposable {

		/// <summary>
		/// The data stored by the current pipeline cache.
		/// </summary>
		public byte[] Data { get; }

	}

	/// <summary>
	/// Pipeline cache creation information.
	/// </summary>
	public record PipelineCacheCreateInfo {

		/// <summary>
		/// The initial data to create the pipeline cache with.
		/// </summary>
		public byte[]? InitialData { get; init; } = null;

	}

	/// <summary>
	/// A pipeline object specifies how drawing and dispatch commands issued to the GPU are processed.
	/// </summary>
	public interface IPipeline : IDisposable { }

	/// <summary>
	/// The stencil test state for a pipeline.
	/// </summary>
	public readonly record struct PipelineStencilState {

		/// <summary>
		/// The stencil operation to perform if the stencil test fails.
		/// </summary>
		public StencilOp FailOp { get; init; } = StencilOp.Keep;

		/// <summary>
		/// The stencil operation to perform if the depth and stencil test pass.
		/// </summary>
		public StencilOp PassOp { get; init; } = StencilOp.Keep;

		/// <summary>
		/// The stencil operation to perform if the stencil test passes but the depth test fails.
		/// </summary>
		public StencilOp DepthFailOp { get; init; } = StencilOp.Keep;

		/// <summary>
		/// The comparison operation to perform for the stencil test.
		/// </summary>
		public CompareOp CompareOp { get; init; } = CompareOp.Never;

		/// <summary>
		/// The bitwise mask to apply to values before comparison.
		/// </summary>
		public uint CompareMask { get; init; } = 0;

		/// <summary>
		/// The bitwise mask specifying which bits are written back to the stencil buffer.
		/// </summary>
		public uint WriteMask { get; init; } = 0;

		/// <summary>
		/// Reference value used in stencil comparison.
		/// </summary>
		public uint Reference { get; init; } = 0;

		public PipelineStencilState() { }

	}

	/// <summary>
	/// The pipeline state information for a color attachment.
	/// </summary>
	public readonly record struct PipelineColorAttachmentState {

		/// <summary>
		/// If color blending is enabled, otherwise the source color values are passed through unmodified.
		/// </summary>
		public bool BlendEnable { get; init; } = false;

		/// <summary>
		/// The blending equation to use if blending is enabled.
		/// </summary>
		public BlendEquation BlendEquation { get; init; } = BlendEquation.Passthrough;

		/// <summary>
		/// Bitmask of color components that will be written to the destination.
		/// </summary>
		public ColorComponent ColorWriteMask { get; init; } = ColorComponent.All;

		public PipelineColorAttachmentState() { }

	}

	/// <summary>
	/// Enumeration of pipeline dynamic state properties.
	/// </summary>
	public enum PipelineDynamicState {
		/// <summary>
		/// The viewport(s) may be dynamically modified.
		/// </summary>
		Viewport,
		/// <summary>
		/// The scissor region(s) may be dynamically modified.
		/// </summary>
		Scissor,
		/// <summary>
		/// The width of lines may be dynamically modified.
		/// </summary>
		LineWidth,
		/// <summary>
		/// The depth bias amount may be dynamically modified.
		/// </summary>
		DepthBias,
		/// <summary>
		/// The blend color constant may be dynamically modified.
		/// </summary>
		BlendConstants,
		/// <summary>
		/// The depth bounds may be dynamically modified.
		/// </summary>
		DepthBounds,
		/// <summary>
		/// The stencil test compare mask may be dynamically modified.
		/// </summary>
		StencilCompareMask,
		/// <summary>
		/// The stencil test write mask may be dynamically modified.
		/// </summary>
		StencilWriteMask,
		/// <summary>
		/// The stencil test reference value may be dynamically modified.
		/// </summary>
		StencilReference,
		// TODO: VK_EXT_extended_dynamic_state
		/// <summary>
		/// The culling mode may be dynamically modified.
		/// </summary>
		CullMode,
		/// <summary>
		/// The front face mode may be dynamically modified.
		/// </summary>
		FrontFace,
		/// <summary>
		/// The draw mode may be dynamically modified.
		/// </summary>
		DrawMode,
		/// <summary>
		/// The depth test may be dynamically enabled or disabled.
		/// </summary>
		DepthTestEnable,
		/// <summary>
		/// Depth writing may be dynamically enabled or disabled.
		/// </summary>
		DepthWriteEnable,
		/// <summary>
		/// The depth compare operation may be dynamically modified.
		/// </summary>
		DepthCompareOp,
		/// <summary>
		/// Depth bounds testing may be dynamically enabled or disabled.
		/// </summary>
		DepthBoundsTestEnable,
		/// <summary>
		/// The stencil test may be dynamically enabled or disabled.
		/// </summary>
		StencilTestEnable,
		/// <summary>
		/// The stencil operations performed on each face may be dynamically modified
		/// </summary>
		StencilOp,
		// TODO: VK_EXT_extended_dynamic_state2
		/// <summary>
		/// The number of control points per tessellation patch may be dynamically modified.
		/// </summary>
		PatchControlPoints,
		/// <summary>
		/// The discard state of the rasterization stage may be dynamically enabled or disabled.
		/// </summary>
		RasterizerDiscardEnable,
		/// <summary>
		/// Depth biasing may be dynamically enabled or disabled.
		/// </summary>
		DepthBiasEnable,
		/// <summary>
		/// The color logic op performed may be dynamically enabled or disabled.
		/// </summary>
		LogicOp,
		/// <summary>
		/// Primitive restart may be dynamically enabled or disabled.
		/// </summary>
		PrimitiveRestartEnable,
		// TODO: VK_EXT_vertex_input_dynamic_state
		/// <summary>
		/// The vertex input format of the pipeline may be dynamically modified.
		/// </summary>
		VertexFormat,
		// TODO: VK_EXT_color_write_enable
		/// <summary>
		/// The color write state for the output attachments may be dynamically enabled or disabled.
		/// </summary>
		ColorWrite,
		/// <summary>
		/// The number of viewports may be dynamically modified.
		/// </summary>
		ViewportCount,
		/// <summary>
		/// The number of scissors may be dynamically modified.
		/// </summary>
		ScissorCount
	}

	/// <summary>
	/// Pipeline creation information that may be dynamically variable. 
	/// </summary>
	public record PipelineDynamicCreateInfo {

		// Vertex input state

		/// <summary>
		/// The format that vertex attributes are fetched into the pipeline from vertex buffers.
		/// </summary>
		public required VertexFormat VertexFormat { get; init; }

		// Input assembly state

		/// <summary>
		/// The drawing mode of primitives rendered by the pipeline.
		/// </summary>
		public DrawMode DrawMode { get; init; } = DrawMode.TriangleList;

		/// <summary>
		/// If vertex indices of -1 can be used to restart rendering of compound draw modes (ie. strips, fans, lists).
		/// </summary>
		public bool PrimitiveRestartEnable { get; init; } = false;

		// Tessellation state

		/// <summary>
		/// The number of control points for every patch for tessellation.
		/// </summary>
		public uint PatchControlPoints { get; init; } = 0;

		// Viewport state

		/// <summary>
		/// The viewports used by this pipeline. Ignored if <see cref="PipelineDynamicState.Viewport"/> is specified.
		/// </summary>
		public EquatableList<Viewport> Viewports { get; init; } = new EquatableList<Viewport>(Collection<Viewport>.EmptyList);

		/// <summary>
		/// The scissors used by this pipeline. Ignored if <see cref="PipelineDynamicState.Scissor"/> is specified.
		/// </summary>
		public EquatableList<Recti> Scissors { get; init; } = new EquatableList<Recti>(Collection<Recti>.EmptyList);

		// Rasterization state

		/// <summary>
		/// If geometry should be discarded instead of being passed to the rasterizer.
		/// </summary>
		public bool RasterizerDiscardEnable { get; init; } = false;

		/// <summary>
		/// The culling mode of the pipeline.
		/// </summary>
		public CullFace CullMode { get; init; } = CullFace.None;

		/// <summary>
		/// The front face specification for culling.
		/// </summary>
		public FrontFace FrontFace { get; init; } = FrontFace.Clockwise;

		/// <summary>
		/// The width of generated lines when in drawing and polygon modes which generate lines.
		/// </summary>
		public float LineWidth { get; init; } = 1;

		/// <summary>
		/// If depth biasing is enabled.
		/// </summary>
		public bool DepthBiasEnable { get; init; } = false;

		/// <summary>
		/// The constant factor to bias depth values by.
		/// </summary>
		public float DepthBiasConstantFactor { get; init; } = 0;

		/// <summary>
		/// The absolute maximum value of depth bias to apply.
		/// </summary>
		public float DepthBiasClamp { get; init; } = 0;

		/// <summary>
		/// The constant factor to scale depth values by.
		/// </summary>
		public float DepthBiasSlopeFactor { get; init; } = 0;

		// Depth/stencil state

		/// <summary>
		/// If depth testing is enabled.
		/// </summary>
		public bool DepthTestEnable { get; init; } = false;

		/// <summary>
		/// If writes to the depth buffer are enabled.
		/// </summary>
		public bool DepthWriteEnable { get; init; } = false;

		/// <summary>
		/// The comparison to use when performing the depth test.
		/// </summary>
		public CompareOp DepthCompareOp { get; init; } = CompareOp.Never;

		/// <summary>
		/// If depth bounds testing is enabled.
		/// </summary>
		public bool DepthBoundsTestEnable { get; init; } = false;

		/// <summary>
		/// If stencil testing is enabled.
		/// </summary>
		public bool StencilTestEnable { get; init; } = false;

		/// <summary>
		/// The stencil state for front-facing geometry.
		/// </summary>
		public PipelineStencilState FrontStencilState { get; init; } = new();

		/// <summary>
		/// The stencil state for back-facing geometry.
		/// </summary>
		public PipelineStencilState BackStencilState { get; init; } = new();

		/// <summary>
		/// The minimum and maximum values for depth bounds testing.
		/// </summary>
		public (float Min, float Max) DepthBounds { get; init; } = (0, 0);

		// Color blend state

		/// <summary>
		/// The logic operation to perform on color attachments.
		/// </summary>
		public LogicOp LogicOp { get; init; } = LogicOp.Clear;

		/// <summary>
		/// The blend constant values to use for color blending.
		/// </summary>
		public Vector4 BlendConstant { get; init; } = Vector4.Zero;

		/// <summary>
		/// The color write enable flags used for color output.
		/// </summary>
		public EquatableList<bool> ColorWriteEnable { get; init; } = new EquatableList<bool>(Collection<bool>.EmptyList);

	}

	/// <summary>
	/// Graphics pipeline creation information.
	/// </summary>
	public record PipelineGraphicsCreateInfo {

		// Viewport state

		/// <summary>
		/// The number of viewports used by this pipeline. Required if <see cref="PipelineDynamicState.ViewportCount"/> is not specified.
		/// </summary>
		public uint ViewportCount { get; init; } = 0;

		/// <summary>
		/// The number of scissors used by this pipeline. Required if <see cref="PipelineDynamicState.ScissorCount"/> is not specified.
		/// </summary>
		public uint ScissorCount { get; init; } = 0;

		// Rasterization state

		/// <summary>
		/// If depth clamping is enabled.
		/// </summary>
		public bool DepthClampEnable { get; init; } = false;

		/// <summary>
		/// The rasterization mode for polygons.
		/// </summary>
		public PolygonMode PolygonMode { get; init; } = PolygonMode.Fill;

		// Color blend state

		/// <summary>
		/// If color logic is enabled.
		/// </summary>
		public bool LogicOpEnable { get; init; } = false;

		/// <summary>
		/// The list of color attachments bound to the pipeline.
		/// </summary>
		public IReadOnlyCollection<PipelineColorAttachmentState> Attachments { get; init; } = Array.Empty<PipelineColorAttachmentState>();

		// Dynamic state

		/// <summary>
		/// The initial dynamic state to create the pipeline with.
		/// </summary>
		public required PipelineDynamicCreateInfo DynamicInfo { get; init; }

		/// <summary>
		/// The list of dynamic properties the pipeline has.
		/// </summary>
		public IReadOnlyCollection<PipelineDynamicState> DynamicState { get; init; } = Array.Empty<PipelineDynamicState>();

		/// <summary>
		/// The render pass this pipeline must be used with. The pipeline can be used
		/// with other render passes as long as they are compatible.
		/// </summary>
		public required IRenderPass RenderPass { get; init; }

		/// <summary>
		/// The subpass of the <see cref="RenderPass"/> this pipeline must be used with.
		/// </summary>
		public required uint Subpass { get; init; }

	}

	/// <summary>
	/// Compute pipeline creation information.
	/// </summary>
	public record PipelineComputeCreateInfo { }

	/// <summary>
	/// Pipeline creation information.
	/// </summary>
	public record PipelineCreateInfo {

		/// <summary>
		/// The pipeline cache to generate this pipeline from.
		/// </summary>
		public IPipelineCache? Cache { get; init; } = null;

		/// <summary>
		/// The layout of the pipeline.
		/// </summary>
		public required IPipelineLayout Layout { get; init; }

		/// <summary>
		/// The shader program for the pipeline.
		/// </summary>
		public required IShaderProgram ShaderProgram { get; init; }

		/// <summary>
		/// Graphics pipeline creation information.
		/// </summary>
		public PipelineGraphicsCreateInfo? GraphicsInfo { get; init; } = null;

		/// <summary>
		/// Compute pipeline creation information.
		/// </summary>
		public PipelineComputeCreateInfo? ComputeInfo { get; init; } = null;

		/// <summary>
		/// A base pipeline to create the pipeline from. Providing a base pipeline may make pipeline creation and binding
		/// more efficient for congruent pipelines.
		/// </summary>
		public IPipeline? BasePipeline { get; init; } = null;

		/// <summary>
		/// The index of the creation information for the base pipeline similar to <see cref="BasePipeline"/>.
		/// This is only checked during bulk creation of pipelines.
		/// </summary>
		public int? BasePipelineIndex { get; init; } = null;

	}

	/// <summary>
	/// <para>
	/// A pipeline set provides a way of managing multiple similar pipelines with different dynamic parameters.
	/// </para>
	/// <para>
	/// The basic creation information for the pipeline is provided when the set is created and used to derive
	/// how binding is performed with dynamic parameters. If a pipeline cache is provided all new pipelines will
	/// be created from the
	/// cache as well.
	/// </para>
	/// <para>
	/// It is suggested to only use pipeline sets with parameters that vary only a little such as boolean
	/// values and enumerations, as a new pipeline will be created for every used variation of non-dynamic
	/// state. In cases where more complex dynamic state is needed (such as numeric values or complex state),
	/// regular dynamic state should be used or the problem should be re-evaluated.
	/// </para>
	/// </summary>
	public interface IPipelineSet : IDisposable { }

	/// <summary>
	/// Creation information for a pipeline set.
	/// </summary>
	public record PipelineSetCreateInfo {

		/// <summary>
		/// The pipeline creation information.
		/// </summary>
		public required PipelineCreateInfo CreateInfo { get; init; }

		/// <summary>
		/// The set of states that are allowed to vary within this pipeline set. Any
		/// sets marked as dynamic in the creation information will be ignored as
		/// unique pipelines do not need to be created for them.
		/// </summary>
		public IReadOnlyCollection<PipelineDynamicState> VariableStates { get; init; } = Array.Empty<PipelineDynamicState>();

	}

}
