using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// The command mode determines how commands are forwareded from a command sink.
	/// </summary>
	public enum CommandMode {
		/// <summary>
		/// The effect of the commands is immediate.
		/// </summary>
		Immediate,
		/// <summary>
		/// The commands are stored in a buffer for later submission.
		/// </summary>
		Buffered
	}

	/// <summary>
	/// A command sink is an object that can receive commands for accelerated graphics.
	/// </summary>
	public interface ICommandSink {

		/// <summary>
		/// The mode this command sink is operating in.
		/// </summary>
		public CommandMode Mode { get; }

		//================//
		// Pipeline State //
		//================//

		/// <summary>
		/// Binds a pipeline to the current rendering state.
		/// </summary>
		/// <param name="pipeline"></param>
		/// <seealso cref="BindPipelineWithState(IPipelineSet, in PipelineDynamicState)"/>
		public void BindPipeline(IPipeline pipeline);

		/// <summary>
		/// Binds a pipeline from a pipeline set with the given dynamic state.
		/// </summary>
		/// <param name="set">Pipeline set</param>
		/// <param name="state">Dynamic state to bind the pipeline with</param>
		/// <seealso cref="BindPipeline(IPipeline)"/>
		public void BindPipelineWithState(IPipelineSet set, PipelineDynamicCreateInfo state);

		/// <summary>
		/// Sets the given viewport on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="viewport">Viewport to set</param>
		/// <param name="firstViewport">The index of the viewport in multi-view rendering</param>
		/// <seealso cref="SetViewports(in ReadOnlySpan{Viewport}, uint)"/>
		/// <seealso cref="SetViewports(uint, Viewport[])"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(Viewport viewport, uint firstViewport = 0) => SetViewports(stackalloc Viewport[] { viewport }, firstViewport);

		/// <summary>
		/// Sets the given viewports on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="viewports">Viewports to set</param>
		/// <param name="firstViewport">The index of the first viewport in multi-view rendering</param>
		/// <seealso cref="SetViewport(in Viewport, uint)"/>
		/// <seealso cref="SetViewports(uint, Viewport[])"/>
		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint firstViewport = 0);

		/// <summary>
		/// Sets the given scissor on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissor">Scissor area to set</param>
		/// <param name="firstScissor">The index of the scissor in multi-view rendering</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(Recti scissor, uint firstScissor = 0) => SetScissors(stackalloc Recti[] { scissor }, firstScissor);

		/// <summary>
		/// Sets the given scissors on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissors">Scissor areas to set</param>
		/// <param name="firstScissor">The index of the first scissor in multi-view rendering</param>
		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0);

		/// <summary>
		/// Sets the line width of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="lineWidth">Line width</param>
		public void SetLineWidth(float lineWidth);

		/// <summary>
		/// Sets the depth bias values of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="constFactor">Constant depth bias factor</param>
		/// <param name="clamp">Depth bias clamp value</param>
		/// <param name="slopeFactor">Slope depth bias factor</param>
		public void SetDepthBias(float constFactor, float clamp, float slopeFactor);

		/// <summary>
		/// Sets the blend constants of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="blendConst">Blend constant value</param>
		public void SetBlendConstants(Vector4 blendConst);

		/// <summary>
		/// Sets the depth bounds of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="min">Minimum depth bounds</param>
		/// <param name="max">Maximum depth bounds</param>
		public void SetDepthBounds(float min, float max);

		/// <summary>
		/// Sets the stencil test compare mask of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="face">Faces to set the stencil compare mask for</param>
		/// <param name="compareMask">Stencil test compare mask value</param>
		public void SetStencilCompareMask(CullFace face, uint compareMask);

		/// <summary>
		/// Sets the stencil test write mask of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="face">Face to set the stencil write mask for</param>
		/// <param name="writeMask">Stencil test write mask value</param>
		public void SetStencilWriteMask(CullFace face, uint writeMask);

		/// <summary>
		/// Sets the stencil test reference value of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="face">Face to set the stencil reference value for</param>
		/// <param name="reference">Stencil test reference value</param>
		public void SetStencilReference(CullFace face, uint reference);

		/// <summary>
		/// Sets the culling mode of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="culling">Faces to cull</param>
		public void SetCullMode(CullFace culling);

		/// <summary>
		/// Sets if the depth bounds test is enabled for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If the depth bounds test should be enabled</param>
		public void SetDepthBoundsTestEnable(bool enabled);

		/// <summary>
		/// Sets the depth comparison operation of the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="op">Depth comparison operation</param>
		public void SetDepthCompareOp(CompareOp op);

		/// <summary>
		/// Sets if the depth test is enabled for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If the depth test should be enabled</param>
		public void SetDepthTestEnable(bool enabled);

		/// <summary>
		/// Sets if depth value writing is enabled for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If depth writing should be enabled</param>
		public void SetDepthWriteEnable(bool enabled);

		/// <summary>
		/// Sets the front face to use for culling for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="face">Front face to use for culling</param>
		public void SetFrontFace(FrontFace face);

		/// <summary>
		/// Sets the draw mode to use for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="mode">Draw mode to use</param>
		public void SetDrawMode(DrawMode mode);

		/// <summary>
		/// Sets the number of and values for scissors on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissors">Scissor areas to set</param>
		public void SetScissorsWithCount(in ReadOnlySpan<Recti> scissors);

		/// <summary>
		/// Sets the stencil operation values for the given stencil faces on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="faces">Faces to set stencil operation for</param>
		/// <param name="failOp">Stencil test failure operation value</param>
		/// <param name="passOp">Stencil test pass operation value</param>
		/// <param name="depthFailOp">Depth test failure operation value</param>
		/// <param name="compareOp">Stencil comparison operation value</param>
		public void SetStencilOp(CullFace faces, StencilOp failOp, StencilOp passOp, StencilOp depthFailOp, CompareOp compareOp);

		/// <summary>
		/// Sets if the stencil test should be enabled on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If the stencil test should be enabled</param>
		public void SetStencilTestEnable(bool enabled);

		/// <summary>
		/// Sets the number of and values for viewports on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="viewports">Viewport areas to set</param>
		public void SetViewportsWithCount(in ReadOnlySpan<Viewport> viewports);

		/// <summary>
		/// Sets if depth biasing is enabled on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If depth biasing should be enabled</param>
		public void SetDepthBiasEnable(bool enabled);

		/// <summary>
		/// Sets the color logic operation to use for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="op">Color logic operation to use</param>
		public void SetLogicOp(LogicOp op);

		/// <summary>
		/// Sets the number of patch control points to use for tessellation on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="controlPoints">Patch control point value</param>
		public void SetPatchControlPoints(uint controlPoints);

		/// <summary>
		/// Sets if primitive restart is enabled for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If primitive restart should be enabledd</param>
		/// <remarks>
		/// <seealso cref="PipelineDynamicCreateInfo.PrimitiveRestartEnable"/>
		/// </remarks>
		public void SetPrimitiveRestartEnable(bool enabled);

		/// <summary>
		/// Sets if rasterizer discard should be enabled for the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enabled">If rasterizer discard should be enabled</param>
		/// <remarks>
		/// <seealso cref="PipelineDynamicCreateInfo.RasterizerDiscardEnable"/>
		/// </remarks>
		public void SetRasterizerDiscardEnable(bool enabled);

		/// <summary>
		/// Sets the vertex format to use with the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="format">Vertex format to use</param>
		public void SetVertexFormat(VertexFormat format);

		/// <summary>
		/// Sets if color writing is enabled on each attachment of the currently bound framebuffer.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="enables">List of color write enable flags</param>
		public void SetColorWriteEnable(in ReadOnlySpan<bool> enables);

		/// <summary>
		/// Binds a vertex array to the currently bound pipeline.
		/// </summary>
		/// <param name="array">Vertex array to bind</param>
		public void BindVertexArray(IVertexArray array);

		/// <summary>
		/// Binds a set of resources described by a list of bind sets to the current pipeline.
		/// </summary>
		/// <param name="bindPoint">The binding point, determined by the current pipeline's type</param>
		/// <param name="layout">The current pipeline's layout</param>
		/// <param name="set">The set to bind</param>
		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, IBindSet set);

		/// <summary>
		/// Binds a set of resources described by a list of bind sets to the current pipeline.
		/// </summary>
		/// <param name="bindPoint">The binding point, determined by the current pipeline's type</param>
		/// <param name="layout">The current pipeline's layout</param>
		/// <param name="sets">The list of sets to bind</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, params IBindSet[] sets) => BindResources(bindPoint, layout, (IReadOnlyList<IBindSet>)sets);

		/// <summary>
		/// Binds a set of resources described by a list of bind sets to the current pipeline.
		/// </summary>
		/// <param name="bindPoint">The binding point, determined by the current pipeline's type</param>
		/// <param name="layout">The current pipeline's layout</param>
		/// <param name="sets">The list of sets to bind</param>
		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, IReadOnlyList<IBindSet> sets);

		//=============//
		// Dispatching //
		//=============//

		/// <summary>
		/// Draws vertices based on the current binding state.
		/// </summary>
		/// <param name="vertexCount">Number of vertices to draw</param>
		/// <param name="instanceCount">Number of instances to draw</param>
		/// <param name="firstVertex">Offset of the first vertex to draw</param>
		/// <param name="firstInstance">Offset of the first instance to draw</param>
		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);

		/// <summary>
		/// Draws vertices based on the current binding state.
		/// </summary>
		/// <param name="drawParams">Parameters to use for drawing</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Draw(DrawParams drawParams) => Draw(drawParams.VertexCount, drawParams.InstanceCount, drawParams.FirstInstance, drawParams.FirstInstance);

		/// <summary>
		/// Draws indexed vertices based on the current binding state.
		/// </summary>
		/// <param name="indexCount">Number of indices to draw</param>
		/// <param name="instanceCount">Number of instances to draw</param>
		/// <param name="firstIndex">Offset of the first index to draw</param>
		/// <param name="vertexOffset">Offset to add to each index</param>
		/// <param name="firstInstance">Offset of the first instance to draw</param>
		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);

		/// <summary>
		/// Draws indexed vertices based on the current binding state.
		/// </summary>
		/// <param name="drawParams">Parameters to use for drawing</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndexed(uint elemCount, DrawIndexedParams drawParams) => DrawIndexed(drawParams.IndexCount, drawParams.InstanceCount, drawParams.FirstIndex, drawParams.VertexOffset, drawParams.FirstInstance);

		/// <summary>
		/// Indirectly draws vertices based on the current binding state.
		/// </summary>
		/// <param name="buffer">Buffer to fetch indirected draw parameters from</param>
		/// <param name="offset">Offset of the first block of draw parameters in the buffer</param>
		/// <param name="drawCount">Number of draw calls to perform</param>
		/// <param name="stride">Stride between blocks of draw parameters in the buffer</param>
		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawParams.SizeOf);

		/// <summary>
		/// Indirectly draws indexed vertices based on the current binding state.
		/// </summary>
		/// <param name="buffer">Buffer to fetch indirected draw parameters from</param>
		/// <param name="offset">Offset of the first block of draw parameters in the buffer</param>
		/// <param name="drawCount">Number of draw calls to perform</param>
		/// <param name="stride">Stride between blocks of draw parameters in the buffer</param>
		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawIndexedParams.SizeOf);

		/// <summary>
		/// Dispatches work for compute shaders.
		/// </summary>
		/// <param name="groupCounts">Number of work groups for each dimension</param>
		public void Dispatch(Vector3ui groupCounts);

		/// <summary>
		/// Indirectly dispatches work for compute shaders.
		/// </summary>
		/// <param name="buffer">Buffer to fetch indirected dispatch parameters from</param>
		/// <param name="offset">Offset of the parameter block in the buffer</param>
		public void DispatchIndirect(IBuffer buffer, nuint offset);

		//=============================//
		// Buffer / Texture Operations //
		//=============================//

		/// <summary>
		/// A copy buffer region describes a region to copy between buffers.
		/// </summary>
		public readonly struct CopyBufferRegion {

			/// <summary>
			/// Byte offset of the region in the source buffer.
			/// </summary>
			public nuint SrcOffset { get; init; } = 0;

			/// <summary>
			/// Byte offset of the region in the destination buffer.
			/// </summary>
			public nuint DstOffset { get; init; } = 0;

			/// <summary>
			/// Byte length of the region.
			/// </summary>
			public required nuint Length { get; init; }

			public CopyBufferRegion() { }

		}

		/// <summary>
		/// Copies regions from the source buffer to the destination buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source buffer</param>
		/// <param name="regions">Regions to copy</param>
		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<CopyBufferRegion> regions);

		/// <summary>
		/// Copies a region of the source buffer to the destination buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source buffer</param>
		/// <param name="region">Region to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(IBuffer dst, IBuffer src, in CopyBufferRegion region) => CopyBuffer(dst, src, stackalloc CopyBufferRegion[] { region });

		/// <summary>
		/// A descriptor for a region to copy between textures.
		/// </summary>
		public readonly struct CopyTextureRegion {

			/// <summary>
			/// The source offset in pixels.
			/// </summary>
			public Vector3ui SrcOffset { get; init; } = Vector3ui.Zero;

			/// <summary>
			/// The source subresource layers.
			/// </summary>
			public required TextureSubresourceLayers SrcSubresource { get; init; }

			/// <summary>
			/// The destination offset in pixels.
			/// </summary>
			public Vector3ui DstOffset { get; init; } = Vector3ui.Zero;

			/// <summary>
			/// The destination subresource layers.
			/// </summary>
			public required TextureSubresourceLayers DstSubresource { get; init; }

			/// <summary>
			/// The size of the region in pixels.
			/// </summary>
			public required Vector3ui Size { get; init; }

			/// <summary>
			/// A bitmask of aspects which are included in the region.
			/// </summary>
			public required TextureAspect Aspect { get; init; }

			public CopyTextureRegion() { }

		}

		/// <summary>
		/// Copies a set of texture regions from a source to a destination texture.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current layout of the destination texture</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current layout of the source texture</param>
		/// <param name="regions">Texture regions to copy</param>
		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyTextureRegion> regions);

		/// <summary>
		/// Copies a texture region from a source to a destination texture.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current layout of the destination texture</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current layout of the source texture</param>
		/// <param name="copy">Texture region to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in CopyTextureRegion copy) => CopyTexture(dst, dstLayout, src, srcLayout, stackalloc CopyTextureRegion[] { copy });

		/// <summary>
		/// A descriptor for a region to "blit" (copy potentially performing scaling/conversion) between textures.
		/// </summary>
		public readonly struct BlitTextureRegion {

			/// <summary>
			/// The offset defining the first corner of the source region to copy.
			/// </summary>
			public Vector3ui SrcOffset0 { get; init; } = Vector3ui.Zero;

			/// <summary>
			/// The offset defining the second corner of the source region to copy.
			/// </summary>
			public required Vector3ui SrcOffset1 { get; init; }

			/// <summary>
			/// The source mip level to copy from.
			/// </summary>
			public uint SrcLevel { get; init; } = 0;

			/// <summary>
			/// The offset defining the first corner of the destination region.
			/// </summary>
			public Vector3ui DstOffset0 { get; init; } = Vector3ui.Zero;

			/// <summary>
			/// The offset defining the second corner of the destination region.
			/// </summary>
			public required Vector3ui DstOffset1 { get; init; }

			/// <summary>
			/// The destination mip level to copy to.
			/// </summary>
			public uint DstLevel { get; init; } = 0;

			/// <summary>
			/// A bitmask of texture aspects to copy.
			/// </summary>
			public required TextureAspect Aspect { get; init; }

			public BlitTextureRegion() { }

		}

		/// <summary>
		/// Performs a "blit" (block transfer) between textures, potentially performing scaling or format conversion.
		/// </summary>
		/// <param name="dst">Desintation texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="filter">Filtering to apply if scaling is performed</param>
		/// <param name="regions">Texture regions to blit</param>
		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<BlitTextureRegion> regions);

		/// <summary>
		/// Performs a "blit" (block transfer) between textures, potentially performing scaling or format conversion.
		/// </summary>
		/// <param name="dst">Desintation texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="filter">Filtering to apply if scaling is performed</param>
		/// <param name="blit">Texture region to blit</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in BlitTextureRegion blit) => BlitTexture(dst, dstLayout, src, srcLayout, filter, stackalloc BlitTextureRegion[] { blit });

		/// <summary>
		/// A descriptor for a copy between a texture and a buffer.
		/// </summary>
		public readonly struct CopyBufferTexture {

			/// <summary>
			/// The offset to perform the copy at in the buffer.
			/// </summary>
			public nuint BufferOffset { get; init; } = 0;

			/// <summary>
			/// The number of pixels between rows of a larger image in the buffer, or 0 to indicate that the row length
			/// is equal to the texture width.
			/// </summary>
			public uint BufferRowLength { get; init; } = 0;

			/// <summary>
			/// The number of pixels between layers of a larger image in the buffer, or 0 to indicate that the image height
			/// is equal to the texture height.
			/// </summary>
			public uint BufferImageHeight { get; init; } = 0;

			/// <summary>
			/// The offset of the copy region in the texture.
			/// </summary>
			public Vector3ui TextureOffset { get; init; } = Vector3ui.Zero;

			/// <summary>
			/// The size of the copy region in the texture.
			/// </summary>
			public required Vector3ui TextureSize { get; init; }

			/// <summary>
			/// The subresource to be copied in the texture.
			/// </summary>
			public required TextureSubresourceLayers TextureSubresource { get; init; }

			public CopyBufferTexture() { }

		}

		/// <summary>
		/// Copies pixels from a buffer to a texture.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source buffer</param>
		/// <param name="copies">Regions to copy</param>
		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<CopyBufferTexture> copies);

		/// <summary>
		/// Copies pixels from a buffer to a texture.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source buffer</param>
		/// <param name="copy">Region to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in CopyBufferTexture copy) => CopyBufferToTexture(dst, dstLayout, src, stackalloc CopyBufferTexture[] { copy });

		/// <summary>
		/// Copies pixels from texture to a buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="copies">Regions to copy</param>
		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyBufferTexture> copies);

		/// <summary>
		/// Copies pixels from texture to a buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="copy">Region to copy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in CopyBufferTexture copy) => CopyTextureToBuffer(dst, src, srcLayout, stackalloc CopyBufferTexture[] { copy });

		/// <summary>
		/// Updates a region of a buffer with the supplied data. The supplied data is recorded with the command stream
		/// and is generally limited by implementations to be below 64 KiB.
		/// </summary>
		/// <param name="dst">Buffer to update</param>
		/// <param name="dstOffset">Offset of the region to update in bytes</param>
		/// <param name="dstSize">Size of the region to update in bytes</param>
		/// <param name="pData">Pointer to update data</param>
		public void UpdateBuffer(IBuffer dst, nuint dstOffset, nuint dstSize, IntPtr pData);

		/// <summary>
		/// Updates a region of a buffer with the supplied data. The supplied data is recorded with the command stream
		/// and is generally limited by implementations to be below 64 KiB.
		/// </summary>
		/// <param name="dst">Buffer to update</param>
		/// <param name="dstOffset">Offset of the region to update in bytes</param>
		/// <param name="dstSize">Size of the region to update in bytes</param>
		/// <param name="pData">Pointer to update data</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, nuint dstSize, IConstPointer<T> pData) => UpdateBuffer(dst, dstOffset, dstSize, pData.Ptr);

		/// <summary>
		/// Updates a region of a buffer with the supplied data. The supplied data is recorded with the command stream
		/// and is generally limited by implementations to be below 64 KiB.
		/// </summary>
		/// <param name="dst">Buffer to update</param>
		/// <param name="dstOffset">Offset of the region to update in bytes</param>
		/// <param name="data">Updated data</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					UpdateBuffer(dst, dstOffset, (nuint)(data.Length * sizeof(T)), (IntPtr)pData);
				}
			}
		}

		/// <summary>
		/// Updates a region of a buffer with the supplied data. The supplied data is recorded with the command stream
		/// and is generally limited by implementations to be below 64 KiB.
		/// </summary>
		/// <param name="dst">Buffer to update</param>
		/// <param name="dstOffset">Offset of the region to update in bytes</param>
		/// <param name="data">Updated data</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, params T[] data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					UpdateBuffer(dst, dstOffset, (nuint)(data.Length * sizeof(T)), (IntPtr)pData);
				}
			}
		}

		/// <summary>
		/// Fills a region of a buffer with a constant 32-bit integer value.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="dstOffset">Offset to fill at in bytes</param>
		/// <param name="dstSize">Number of bytes to fill</param>
		/// <param name="data">Integer value to fill with</param>
		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data);

		/// <summary>
		/// A color value used for clearing attachments. Note that this structure acts as a
		/// bitwise union of different datatypes, and the intended representation is determined
		/// by the color format of the texture/framebuffer attachment it is used with.
		/// </summary>
		public struct ClearColorValue {

			private Vector4 data;

			/// <summary>
			/// The float components of the clear value.
			/// </summary>
			public Vector4 AsFloat32 {
				get => data;
				set => data = value;
			}

			/// <summary>
			/// The signed integer components of the clear value.
			/// </summary>
			public Vector4i AsInt32 {
				get => MemoryUtil.BitwiseCast<Vector4, Vector4i>(data);
				set => data = MemoryUtil.BitwiseCast<Vector4i, Vector4>(value);
			}

			/// <summary>
			/// The unsigned integer components of the clear value.
			/// </summary>
			public Vector4ui AsUInt32 {
				get => MemoryUtil.BitwiseCast<Vector4, Vector4ui>(data);
				set => data = MemoryUtil.BitwiseCast<Vector4ui, Vector4>(value);
			}

			public static implicit operator ClearColorValue(Vector4 value) => new() { AsFloat32 = value };

			public static implicit operator ClearColorValue(Vector4i value) => new() { AsInt32 = value };

			public static implicit operator ClearColorValue(Vector4ui value) => new() { AsUInt32 = value };

		}

		/// <summary>
		/// Clears parts of the specified color texture to a constant value.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="color">The color value to clear to</param>
		/// <param name="regions">The regions of the texture to clear</param>
		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions);

		/// <summary>
		/// Clears parts of the specified color texture to a constant value.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="color">The color value to clear to</param>
		/// <param name="region">The region of the texture to clear</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ClearColorValue color, in TextureSubresourceRange region) => ClearColorTexture(dst, dstLayout, color, stackalloc TextureSubresourceRange[] { region });

		/// <summary>
		/// Clears parts of the specified depth/stencil texture to a constant value.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="depth">The depth value to clear to</param>
		/// <param name="stencil">The stencil value to clear to</param>
		/// <param name="regions">The regions of the texture to clear</param>
		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, int stencil, in ReadOnlySpan<TextureSubresourceRange> regions);

		/// <summary>
		/// Clears parts of the specified depth/stencil texture to a constant value.
		/// </summary>
		/// <param name="dst">Destination texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="depth">The depth value to clear to</param>
		/// <param name="stencil">The stencil value to clear to</param>
		/// <param name="region">The region of the texture to clear</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, int stencil, in TextureSubresourceRange region) => ClearDepthStencilTexture(dst, dstLayout, depth, stencil, stackalloc TextureSubresourceRange[] { region });

		/// <summary>
		/// A generic clear value for clearing attachments.
		/// </summary>
		public readonly struct ClearValue {

			/// <summary>
			/// A bitmask of texture aspects to clear.
			/// </summary>
			public required TextureAspect Aspect { get; init; }

			/// <summary>
			/// The color clear value.
			/// </summary>
			public ClearColorValue Color { get; init; }

			/// <summary>
			/// The depth clear value.
			/// </summary>
			public float Depth { get; init; }

			/// <summary>
			/// The stencil clear value.
			/// </summary>
			public int Stencil { get; init; }

		}

		/// <summary>
		/// A descriptor for clearing a framebuffer attachment.
		/// </summary>
		public readonly struct ClearAttachment {

			/// <summary>
			/// The index of the attachment to clear.
			/// </summary>
			public required int Attachment { get; init; }

			/// <summary>
			/// The value to clear the attachment with.
			/// </summary>
			public required ClearValue Value { get; init; }

		}

		/// <summary>
		/// An extended descriptor for the area of an attachment to clear.
		/// </summary>
		public readonly struct ClearRect {

			/// <summary>
			/// The 2D area within the attachment to clear.
			/// </summary>
			public required Recti Rect { get; init; }

			/// <summary>
			/// The base array layer of the attachment to clear.
			/// </summary>
			public uint BaseArrayLayer { get; init; } = 0;

			/// <summary>
			/// The number of array layers within the attachment to clear, or 0 to clear just one.
			/// </summary>
			public uint LayerCount { get; init; } = 0;

			public ClearRect() { }

		}

		/// <summary>
		/// Clears the set of specified attachments in the framebuffer currently used for rendering.
		/// </summary>
		/// <param name="values">Attachment clear values</param>
		/// <param name="regions">Attachment clear regions</param>
		public void ClearAttachments(in ReadOnlySpan<ClearAttachment> values, in ReadOnlySpan<ClearRect> regions);

		/// <summary>
		/// Clears the set of specified attachments in the framebuffer currently used for rendering.
		/// </summary>
		/// <param name="values">Attachment clear values</param>
		/// <param name="region">Attachment clear region</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearAttachments(in ReadOnlySpan<ClearAttachment> values, in ClearRect region) => ClearAttachments(values, stackalloc ClearRect[] { region });

		/// <summary>
		/// Clears the set of specified attachments in the framebuffer currently used for rendering.
		/// </summary>
		/// <param name="value">Attachment clear value</param>
		/// <param name="region">Attachment clear region</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearAttachments(in ClearAttachment value, in ClearRect region) => ClearAttachments(stackalloc ClearAttachment[] { value }, stackalloc ClearRect[] { region });

		/// <summary>
		/// Resolves the contents of a multisample texture to a regular texture.
		/// </summary>
		/// <param name="dst">Desintation texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source multisample texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="regions">Texture regions to resolve</param>
		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyTextureRegion> regions);

		/// <summary>
		/// Resolves the contents of a multisample texture to a regular texture.
		/// </summary>
		/// <param name="dst">Desintation texture</param>
		/// <param name="dstLayout">Current destination texture layout</param>
		/// <param name="src">Source multisample texture</param>
		/// <param name="srcLayout">Current source texture layout</param>
		/// <param name="region">Texture region to resolve</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in CopyTextureRegion region) => ResolveTexture(dst, dstLayout, src, srcLayout, stackalloc CopyTextureRegion[] { region });

		/// <summary>
		/// Generates mipmap levels for a texture using the first mip level. The filtering method for minifying the texture images may
		/// be provided explicitly or use an implementation-specific method.
		/// </summary>
		/// <param name="dst">The texture to generate mipmaps for</param>
		/// <param name="initialLayout">The initial layout of the texture</param>
		/// <param name="finalLayout">The final layout of the texture</param>
		/// <param name="filter">The filtering method to use for minification, or null to use a default method</param>
		public void GenerateMipmaps(ITexture dst, TextureLayout initialLayout, TextureLayout finalLayout, TextureFilter? filter = null);

		/// <summary>
		/// Copies a region from one framebuffer attachment to another, potentially performing scaling or format conversion. This is effectively the same as blitting
		/// between the underlying framebuffer images, but will be applied using the texture views used as attachments (including format reinterpretation, component
		/// mapping, subresource range) and it will always be done in 2D. Note that this requires the textures used as attachments to be created with transfer source
		/// and destination usages.
		/// </summary>
		/// <param name="dst">Destination framebuffer</param>
		/// <param name="dstAttachment">Destination framebuffer attachment</param>
		/// <param name="dstLayout">Destination texture layout</param>
		/// <param name="dstArea">Destination area</param>
		/// <param name="src">Source framebuffer</param>
		/// <param name="srcAttachment">Source framebuffer attachment</param>
		/// <param name="srcLayout">Source texture layout</param>
		/// <param name="srcArea">Source area</param>
		/// <param name="aspect">Bitmask of aspects to copy</param>
		/// <param name="filter">Filter to apply during scaling</param>
		public void BlitFramebuffer(IFramebuffer dst, int dstAttachment, TextureLayout dstLayout, Recti dstArea, IFramebuffer src, int srcAttachment, TextureLayout srcLayout, Recti srcArea, TextureAspect aspect, TextureFilter filter);

		//===================================//
		// Synchronization / Memory Barriers //
		//===================================//

		/// <summary>
		/// Sets the state of the specified sync object once prior commands submitted to this sink have
		/// reached the specified pipeline stage. The sync object must be an "event" object.
		/// </summary>
		/// <param name="dst">Sync object to set</param>
		/// <param name="stage">Stage which prior commands must reach before the operation</param>
		public void SetSync(ISync dst, PipelineStage stage);

		/// <summary>
		/// Resets the state of the specified sync object once prior commands submitted to this sink have
		/// reached the specified pipeline stage. The sync object must be an "event" object.
		/// </summary>
		/// <param name="dst">Sync object to reset</param>
		/// <param name="stage">Stage which prior commands must reach before the operation</param>
		public void ResetSync(ISync dst, PipelineStage stage);

		/// <summary>
		/// A generic global memory barrier descriptor.
		/// </summary>
		public readonly struct MemoryBarrier {

			/// <summary>
			/// Bitmask of memory accesses that must occur before the barrier.
			/// </summary>
			public required MemoryAccess ProvokingAccess { get; init; }

			/// <summary>
			/// Bitmask of memory access that must occur after the barrier.
			/// </summary>
			public required MemoryAccess AwaitingAccess { get; init; }

		}

		/// <summary>
		/// A memory barrier descriptor for the contents of a buffer object.
		/// </summary>
		public readonly struct BufferMemoryBarrier {

			/// <summary>
			/// Bitmask of memory accesses that must occur before the barrier.
			/// </summary>
			public required MemoryAccess ProvokingAccess { get; init; }

			/// <summary>
			/// Bitmask of memory access that must occur after the barrier.
			/// </summary>
			public required MemoryAccess AwaitingAccess { get; init; }

			/// <summary>
			/// The buffer whose contents are the subject of the barrier.
			/// </summary>
			public required IBuffer Buffer { get; init; }

			/// <summary>
			/// The range of memory in the buffer subject to the barrier.
			/// </summary>
			public required MemoryRange Range { get; init; }

		}

		/// <summary>
		/// A memory barrier descriptor for the contents of a texture object.
		/// </summary>
		public readonly struct TextureMemoryBarrier {

			/// <summary>
			/// Bitmask of memory accesses that must occur before the barrier.
			/// </summary>
			public required MemoryAccess ProvokingAccess { get; init; }

			/// <summary>
			/// Bitmask of memory access that must occur after the barrier.
			/// </summary>
			public required MemoryAccess AwaitingAccess { get; init; }

			/// <summary>
			/// The layout of the texture before the barrier.
			/// </summary>
			public required TextureLayout OldLayout { get; init; }

			/// <summary>
			/// The layout to assign to the texture after the barrier.
			/// </summary>
			public required TextureLayout NewLayout { get; init; }

			/// <summary>
			/// The texture whose contents are the subject of the barrier.
			/// </summary>
			public required ITexture Texture { get; init; }

			/// <summary>
			/// The subresource range within the texture subject to the barrier.
			/// </summary>
			public required TextureSubresourceRange SubresourceRange { get; init; }

		}

		/// <summary>
		/// A descriptor containing a set of memory barriers to enforce command order based on.
		/// </summary>
		public readonly struct PipelineBarriers {

			/// <summary>
			/// Bitmask of pipeline stages in which operations occur that must come before any of the barriers.
			/// </summary>
			public required PipelineStage ProvokingStages { get; init; }

			/// <summary>
			/// Bitmask of pipeline stages in which operations occur that must come after any of the barriers.
			/// </summary>
			public required PipelineStage AwaitingStages { get; init; }

			/// <summary>
			/// List of global memory barriers.
			/// </summary>
			public IReadOnlyList<MemoryBarrier> MemoryBarriers { get; init; } = Array.Empty<MemoryBarrier>();

			/// <summary>
			/// List of buffer memory barriers.
			/// </summary>
			public IReadOnlyList<BufferMemoryBarrier> BufferMemoryBarriers { get; init; } = Array.Empty<BufferMemoryBarrier>();

			/// <summary>
			/// List of texture memory barriers.
			/// </summary>
			public IReadOnlyList<TextureMemoryBarrier> TextureMemoryBarriers { get; init; } = Array.Empty<TextureMemoryBarrier>();

			public PipelineBarriers() { }

		}

		/// <summary>
		/// Waits on the specified sync objects while introducing a set of memory barriers.
		/// </summary>
		/// <param name="barriers">Pipeline memory barriers</param>
		/// <param name="syncs">Sync objects to wait on</param>
		public void WaitSync(in PipelineBarriers barriers, IReadOnlyList<ISync> syncs);

		/// <summary>
		/// Waits on the specified sync objects while introducing a set of memory barriers.
		/// </summary>
		/// <param name="barriers">Pipeline memory barriers</param>
		/// <param name="syncs">Sync objects to wait on</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitSync(in PipelineBarriers barriers, params ISync[] syncs) => WaitSync(barriers, (IReadOnlyList<ISync>)syncs);

		/// <summary>
		/// Waits on the specified sync objects while introducing a set of memory barriers.
		/// </summary>
		/// <param name="barriers">Pipeline memory barriers</param>
		/// <param name="syncs">Sync objects to wait on</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitSync(in PipelineBarriers barriers, IEnumerable<ISync> syncs) {
			if (syncs is IReadOnlyList<ISync> list) WaitSync(barriers, list);
			else WaitSync(barriers, syncs.ToArray());
		}

		/// <summary>
		/// Waits on the specified sync object while introducing a set of memory barriers.
		/// </summary>
		/// <param name="barriers">Pipeline memory barriers</param>
		/// <param name="sync">Sync object to wait on</param>
		public void WaitSync(in PipelineBarriers barriers, ISync sync);

		/// <summary>
		/// Introduces the specified barriers into the pipeline, performing synchronization as necessary.
		/// </summary>
		/// <param name="barriers">Pipeline barriers</param>
		public void Barrier(in PipelineBarriers barriers);

		//================//
		// Push Constants //
		//================//

		/// <summary>
		/// Updates the current push constants using the provided data.
		/// </summary>
		/// <param name="layout">The pipeline layout to use</param>
		/// <param name="stages">A bitmask of shader stages whose push constants should be updated</param>
		/// <param name="offset">The offset in bytes within the push constant block to update</param>
		/// <param name="size">The number of bytes to update</param>
		/// <param name="pValues">Pointer to memory to update with</param>
		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues);

		/// <summary>
		/// Updates the current push constants using the provided data.
		/// </summary>
		/// <typeparam name="T">Pointer element type</typeparam>
		/// <param name="layout">The pipeline layout to use</param>
		/// <param name="stages">A bitmask of shader stages whose push constants should be updated</param>
		/// <param name="offset">The offset in bytes within the push constant block to update</param>
		/// <param name="size">The number of bytes to update</param>
		/// <param name="pValues">Pointer to memory to update with</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IConstPointer<T> pValues) => PushConstants(layout, stages, offset, size, pValues.Ptr);

		/// <summary>
		/// Updates the current push constants using the provided data.
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		/// <param name="layout">The pipeline layout to use</param>
		/// <param name="stages">A bitmask of shader stages whose push constants should be updated</param>
		/// <param name="offset">The offset in bytes within the push constant block to update</param>
		/// <param name="values">Values to update with</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged {
			unsafe {
				fixed(T* pValues = values) {
					PushConstants(layout, stages, offset, (uint)(values.Length * sizeof(T)), (IntPtr)pValues);
				}
			}
		}

		/// <summary>
		/// Updates the current push constants using the provided data.
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		/// <param name="layout">The pipeline layout to use</param>
		/// <param name="stages">A bitmask of shader stages whose push constants should be updated</param>
		/// <param name="offset">The offset in bytes within the push constant block to update</param>
		/// <param name="values">Values to update with</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, params T[] values) where T : unmanaged {
			unsafe {
				fixed (T* pValues = values) {
					PushConstants(layout, stages, offset, (uint)(values.Length * sizeof(T)), (IntPtr)pValues);
				}
			}
		}

		//===============//
		// Render Passes //
		//===============//

		/// <summary>
		/// Render pass begin information.
		/// </summary>
		public readonly struct RenderPassBegin {

			/// <summary>
			/// The render pass to being rendering with.
			/// </summary>
			public required IRenderPass RenderPass { get; init; }

			/// <summary>
			/// The framebuffer to begin rendering to.
			/// </summary>
			public required IFramebuffer Framebuffer { get; init; }

			/// <summary>
			/// The area within the framebuffer that will be rendered to.
			/// </summary>
			public required Recti RenderArea { get; init; }

			/// <summary>
			/// The list of clear values for each attachment, corresponding by index. If an attachment
			/// is not cleared the value is ignored.
			/// </summary>
			public IReadOnlyList<ClearValue> ClearValues { get; init; } = Array.Empty<ClearValue>();

			public RenderPassBegin() { }

		}

		/// <summary>
		/// Begins the specified render pass.
		/// </summary>
		/// <param name="begin">Render pass begin information</param>
		/// <param name="contents">The type of commands that will be submitted in the first subpass</param>
		public void BeginRenderPass(in RenderPassBegin begin, SubpassContents contents);

		/// <summary>
		/// Begins the next subpass in the current render pass.
		/// </summary>
		/// <param name="contents">The type of commands that will be submitted in the next subpass</param>
		public void NextSubpass(SubpassContents contents);

		/// <summary>
		/// Ends the current render pass.
		/// </summary>
		public void EndRenderPass();

		/// <summary>
		/// Attachment information for a framebuffer attachment used in dynamic rendering.
		/// </summary>
		public readonly struct RenderingAttachmentInfo {

			/// <summary>
			/// The index of the attachment within the framebuffer.
			/// </summary>
			public required uint Index { get; init; }

			/// <summary>
			/// The texture view to use as an attachment.
			/// </summary>
			public required ITextureView TextureView { get; init; }

			/// <summary>
			/// The current layout of the attachment's texture.
			/// </summary>
			public required TextureLayout TextureLayout { get; init; }

			/// <summary>
			/// The resolution mode to use for any resolve texture.
			/// </summary>
			public ResolveMode ResolveMode { get; init; } = ResolveMode.Default;

			/// <summary>
			/// The texture view to resolve a multisample attachment to at the end of rendering.
			/// </summary>
			public ITextureView? ResolveTextureView { get; init; } = null;

			/// <summary>
			/// The current layout of the resolve texture if there is one.
			/// </summary>
			public TextureLayout ResolveTextureLayout { get; init; }

			/// <summary>
			/// The load operation to perform on the attachment at the start of rendering.
			/// </summary>
			public required AttachmentLoadOp LoadOp { get; init; }

			/// <summary>
			/// The store operation to perform on the attachment at the end of rendering.
			/// </summary>
			public required AttachmentStoreOp StoreOp { get; init; }

			/// <summary>
			/// The clear value to use if the <see cref="LoadOp"/> is <see cref="AttachmentLoadOp.Clear"/>.
			/// </summary>
			public ClearValue ClearValue { get; init; }

			public RenderingAttachmentInfo() { }

		}

		/// <summary>
		/// Dynamic rendering information.
		/// </summary>
		public readonly struct RenderingInfo {

			/// <summary>
			/// The area to render to.
			/// </summary>
			public Recti RenderArea { get; init; } = default;

			/// <summary>
			/// The framebuffer containing the attachments to render to.
			/// </summary>
			public required IFramebuffer Framebuffer { get; init; }

			/// <summary>
			/// The list of color attachments to use during rendering, or null if there are no color attachments.
			/// </summary>
			public IReadOnlyList<RenderingAttachmentInfo> ColorAttachments { get; init; } = Array.Empty<RenderingAttachmentInfo>();

			/// <summary>
			/// The depth attachment to use during rendering, or null if there is no depth attachment.
			/// </summary>
			public RenderingAttachmentInfo? DepthAttachment { get; init; } = null;

			/// <summary>
			/// The stencil attachment to use during rendering, or null if there is no stencil attachment.
			/// </summary>
			public RenderingAttachmentInfo? StencilAttachment { get; init; } = null;

			public RenderingInfo() { }

		}

		/// <summary>
		/// Begins dynamic rendering according to the supplied information.
		/// </summary>
		/// <param name="renderingInfo">Dynamic rendering begin information</param>
		public void BeginRendering(in RenderingInfo renderingInfo);

		/// <summary>
		/// Ends dynamic rendering.
		/// </summary>
		public void EndRendering();

		//=============================//
		// Secondary Command Execution //
		//=============================//

		/// <summary>
		/// Sequentially executes the commands in a list of secondary command buffers.
		/// </summary>
		/// <param name="buffers">Secondary command buffer list</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(params ICommandBuffer[] buffers) => ExecuteCommands((IReadOnlyList<ICommandBuffer>)buffers);

		/// <summary>
		/// Sequentially executes the commands in a list of secondary command buffers.
		/// </summary>
		/// <param name="buffers">Secondary command buffer list</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(IEnumerable<ICommandBuffer> buffers) {
			if (buffers is IReadOnlyList<ICommandBuffer> list) ExecuteCommands(list);
			else ExecuteCommands(buffers.ToArray());
		}

		/// <summary>
		/// Sequentially executes the commands in a list of secondary command buffers.
		/// </summary>
		/// <param name="buffers">Secondary command buffer list</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(IReadOnlyList<ICommandBuffer> buffers);

		/// <summary>
		/// Executes the commands in the given secondary command buffer.
		/// </summary>
		/// <param name="buffer">Secondary command buffer</param>
		public void ExecuteCommands(ICommandBuffer buffer);

	}

	/// <summary>
	/// Enumeration of types of command buffers.
	/// </summary>
	public enum CommandBufferType {
		/// <summary>
		/// A primary command buffer can include any kind of commands and may execute
		/// secondary command buffers.
		/// </summary>
		Primary,
		/// <summary>
		/// Secondary command buffers can only include commands that are valid inside a render
		/// pass, but may be executed by primary command buffers.
		/// </summary>
		Secondary
	}

	/// <summary>
	/// <para>Bitmask of command buffer usages.</para>
	/// <para>
	/// The usage of command buffers determines how they can be submitted to the GPU; the GPU may
	/// have independent queues that support only a subset of operations. The categories of
	/// command usages are graphics, transfer, compute, and sparse binding.
	/// </para>
	/// <para>
	/// Graphics commands setup and perform draw calls or other graphical operations such as texture clearing
	/// or resolution. Transfer commands copy memory between objects or perform simple memory operations. Compute
	/// commands operate on compute shaders and associated objects.
	/// </para>
	/// <para>Transfer commands are supported by graphics and compute queues and may be used interchangibly
	/// with the respective commands, but independent transfer-only queues may be prioritized if a command
	/// buffer uses only transfer commands. A few commands are usable from both graphics and compute
	/// queues that are necessary for both.</para>
	/// <para>
	/// A reference table for <see cref="ICommandSink"/> is as follows:
	/// <list type="table">
	/// <item>
	///		<term>Any</term>
	///		<description>
	///			<see cref="ICommandSink.Barrier(in ICommandSink.PipelineBarriers)">Barrier</see>,
	///			<see cref="ICommandSink.ExecuteCommands(ICommandBuffer)">ExecuteCommands</see>
	///		</description>
	///	</item>
	/// <item>
	///		<term>Graphics or Compute</term>
	///		<description>
	///			<see cref="ICommandSink.BindPipeline(IPipeline)">BindPipeline</see>,
	///			<see cref="ICommandSink.BindPipelineWithState(IPipelineSet, in PipelineDynamicState)">BindPipelineWithState</see>,
	///			<see cref="ICommandSink.ClearColorTexture(ITexture, TextureLayout, Vector4, in TextureSubresourceRange)">ClearColorTexture</see>,
	///			<see cref="ICommandSink.SetSync(ISync, PipelineStage)">SetSync</see>,
	///			<see cref="ICommandSink.ResetSync(ISync, PipelineStage)">ResetSync</see>,
	///			<see cref="ICommandSink.WaitSync(in ICommandSink.PipelineBarriers, ISync)">WaitSync</see>,
	///			<see cref="ICommandSink.PushConstants{T}(IPipelineLayout, PipelineStage, uint, uint, in ReadOnlySpan{T})">PushConstants</see>
	///		</description>
	/// </item>
	/// <item>
	///		<term>Graphics</term>
	///		<description>
	///			<see cref="ICommandSink.SetViewport(in Viewport, uint)">SetViewport</see>,
	///			<see cref="ICommandSink.SetScissor(in Recti, uint)">SetScissor</see>,
	///			<see cref="ICommandSink.SetLineWidth(float)">SetLineWidth</see>,
	///			<see cref="ICommandSink.SetDepthBias(float, float, float)">SetDepthBias</see>,
	///			<see cref="ICommandSink.SetBlendConstants(Vector4)">SetBlendConstants</see>,
	///			<see cref="ICommandSink.SetDepthBounds(float, float)">SetDepthBounds</see>,
	///			<see cref="ICommandSink.SetStencilCompareMask(CullFace, uint)">SetStencilCompareMask</see>,
	///			<see cref="ICommandSink.SetStencilWriteMask(CullFace, uint)">SetStencilWriteMask</see>,
	///			<see cref="ICommandSink.SetStencilReference(CullFace, uint)">SetStencilReference</see>,
	///			<see cref="ICommandSink.BindVertexArray(IVertexArray)">BindVertexArray</see>,
	///			<see cref="ICommandSink.Draw(uint, uint, uint, uint)">Draw</see>,
	///			<see cref="ICommandSink.DrawIndexed(uint, uint, uint, int, uint)">DrawIndexed</see>,
	///			<see cref="ICommandSink.DrawIndirect(IBuffer, nuint, uint, uint)">DrawIndirect</see>,
	///			<see cref="ICommandSink.DrawIndexedIndirect(IBuffer, nuint, uint, uint)">DrawIndexedIndirect</see>,
	///			<see cref="ICommandSink.BlitTexture(ITexture, TextureLayout, ITexture, TextureLayout, TextureFilter, in ICommandSink.BlitTextureRegion)">BlitTexture</see>,
	///			<see cref="ICommandSink.ClearDepthStencilTexture(ITexture, TextureLayout, float, uint, in TextureSubresourceRange)">ClearDepthStencilTexture</see>,
	///			<see cref="ICommandSink.ClearAttachments(in ICommandSink.ClearValues, in ICommandSink.ClearRect)">ClearAttachments</see>,
	///			<see cref="ICommandSink.ResolveTexture(ITexture, TextureLayout, ITexture, TextureLayout, in ICommandSink.BlitTextureRegion)">ResolveTexture</see>,
	///			<see cref="ICommandSink.BeginRenderPass(in ICommandSink.RenderPassBegin, SubpassContents)">BeginRenderPass</see>,
	///			<see cref="ICommandSink.NextSubpass(SubpassContents)">NextSubpass</see>,
	///			<see cref="ICommandSink.EndRenderPass">EndRenderPass</see>,
	///			<see cref="ICommandSink.BeginRendering(in ICommandSink.RenderingInfo)">BeginRendering</see>,
	///			<see cref="ICommandSink.EndRendering">EndRendering</see>
	///		</description>
	/// </item>
	/// <item>
	///		<term>Transfer</term>
	///		<description>
	///			<see cref="ICommandSink.CopyBuffer(IBuffer, IBuffer, in ICommandSink.CopyBufferRegion)">CopyBuffer</see>,
	///			<see cref="ICommandSink.CopyTexture(ITexture, TextureLayout, ITexture, TextureLayout, in ICommandSink.CopyTextureRegion)">CopyTexture</see>,
	///			<see cref="ICommandSink.CopyBufferToTexture(ITexture, TextureLayout, IBuffer, in ICommandSink.CopyBufferTexture)">CopyBufferToTexture</see>,
	///			<see cref="ICommandSink.CopyTextureToBuffer(IBuffer, ITexture, TextureLayout, in ICommandSink.CopyBufferTexture)">CopyTextureToBuffer</see>,
	///			<see cref="ICommandSink.UpdateBuffer{T}(IBuffer, nuint, in ReadOnlySpan{T})">UpdateBuffer</see>,
	///			<see cref="ICommandSink.FillBufferUInt32(IBuffer, nuint, nuint, uint)">FillBufferUInt32</see>
	///		</description>
	///	</item>
	///	<item>
	///		<term>Compute</term>
	///		<description>
	///			<see cref="ICommandSink.Dispatch(Vector3i)">Dispatch</see>,
	///			<see cref="ICommandSink.DispatchIndirect(IBuffer, nuint)">DispatchIndirect</see>
	///		</description>
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// Additional usage flags specify the contents of the command buffer and how it will be used.
	/// </para>
	/// </summary>
	[Flags]
	public enum CommandBufferUsage {
		/// <summary>
		/// The command buffer will use graphics commands.
		/// </summary>
		Graphics = 0x0001,
		/// <summary>
		/// The command buffer will use transfer commands.
		/// </summary>
		Transfer = 0x0002,
		/// <summary>
		/// The command buffer will use compute commands.
		/// </summary>
		Compute = 0x0004,
		/// <summary>
		/// The command buffer will be submitted once before being re-recorded or destroyed.
		/// </summary>
		OneTimeSubmit = 0x0008,
		/// <summary>
		/// For secondary command buffers, the command buffer will be entirely inside a render pass.
		/// </summary>
		RenderPassContinue = 0x0010,
		/// <summary>
		/// The command buffer may be submitted concurrently and persist multiple times in command queues.
		/// </summary>
		Concurrent = 0x0020,
		/// <summary>
		/// The command buffer may re-recorded when not in use.
		/// </summary>
		Rerecordable = 0x0040
	}

	/// <summary>
	/// Command buffer creation information.
	/// </summary>
	public record CommandBufferCreateInfo {

		/// <summary>
		/// The required granularity of potential texture transfers done by commands in this
		/// command buffer. A granularity of (0,0,0) will be interpreted as a "don't care".
		/// </summary>
		public Vector3ui RequiredTransferGranularity { get; init; } = Vector3ui.Zero;

		/// <summary>
		/// The type of command buffer to create.
		/// </summary>
		public required CommandBufferType Type { get; init; }

		/// <summary>
		/// Usage flags for the command buffer.
		/// </summary>
		public required CommandBufferUsage Usage { get; init; }

	}

	/// <summary>
	/// A command buffer stores a list of commands that can be submitted for execution.
	/// </summary>
	public interface ICommandBuffer : IDisposable {

		/// <summary>
		/// An opaque ID identifying the queue that the command buffer must be submitted to.
		/// </summary>
		public ulong QueueID { get; }

		/// <summary>
		/// The type of command buffer.
		/// </summary>
		public CommandBufferType Type { get; }

		/// <summary>
		/// Begins recording into this command buffer. If this command buffer is already recorded and is
		/// re-recordable this resets the command buffer for re-recording.
		/// </summary>
		/// <returns>The command sink to record into</returns>
		public ICommandSink BeginRecording();

		/// <summary>
		/// Finishes recording into this command buffer. Once called any command sinks returned for recording
		/// are no longer valid.
		/// </summary>
		public void EndRecording();

	}

}
