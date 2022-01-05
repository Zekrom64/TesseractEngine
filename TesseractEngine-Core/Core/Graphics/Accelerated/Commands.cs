using System;
using System.Numerics;
using Tesseract.Core.Math;
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
		public void SetViewport(Viewport viewport, uint firstViewport = 0);

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
		/// Sets the given viewports on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="viewports">Viewports to set</param>
		/// <param name="firstViewport">The index of the first viewport in multi-view rendering</param>
		/// <seealso cref="SetViewport(in Viewport, uint)"/>
		/// <seealso cref="SetViewports(in ReadOnlySpan{Viewport}, uint)"/>
		public void SetViewports(uint firstViewport, params Viewport[] viewports);

		/// <summary>
		/// Sets the given scissor on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissor">Scissor area to set</param>
		/// <param name="firstScissor">The index of the scissor in multi-view rendering</param>
		public void SetScissor(Recti scissor, uint firstScissor = 0);

		/// <summary>
		/// Sets the given scissors on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissors">Scissor areas to set</param>
		/// <param name="firstScissor">The index of the first scissor in multi-view rendering</param>
		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0);

		/// <summary>
		/// Sets the given scissors on the currently bound pipeline.
		/// This operation is only supported if the current pipeline was created with the corresponding dynamic state.
		/// </summary>
		/// <param name="scissors">Scissor areas to set</param>
		/// <param name="firstScissor">The index of the first scissor in multi-view rendering</param>
		public void SetScissors(uint firstScissor, params Recti[] scissors);

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
		/// Binds a vertex array to the currently bound pipeline.
		/// </summary>
		/// <param name="array">Vertex array to bind</param>
		public void BindVertexArray(IVertexArray array);

		/// <summary>
		/// Binds a set of resources described by a list of bind sets to the current pipeline.
		/// </summary>
		/// <param name="bindPoint">The binding point, determined by the current pipeline's type</param>
		/// <param name="layout">The current pipeline's layout</param>
		/// <param name="sets">The list of sets to bind</param>
		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, params IBindSet[] sets);

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
		public void DrawIndexed(DrawIndexedParams drawParams) => DrawIndexed(drawParams.IndexCount, drawParams.InstanceCount, drawParams.FirstIndex, drawParams.VertexOffset, drawParams.FirstInstance);

		/// <summary>
		/// Indirectly draws vertices based on the current binding state.
		/// </summary>
		/// <param name="buffer">Buffer to fetch indirected draw parameters from</param>
		/// <param name="offset">Offset of the first block of draw parameters in the buffer</param>
		/// <param name="drawCount">Number of draw calls to perform</param>
		/// <param name="stride">Stride between blocks of draw parameters in the buffer</param>
		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride);

		/// <summary>
		/// Indirectly draws indexed vertices based on the current binding state.
		/// </summary>
		/// <param name="buffer">Buffer to fetch indirected draw parameters from</param>
		/// <param name="offset">Offset of the first block of draw parameters in the buffer</param>
		/// <param name="drawCount">Number of draw calls to perform</param>
		/// <param name="stride">Stride between blocks of draw parameters in the buffer</param>
		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride);

		/// <summary>
		/// Dispatches work for compute shaders.
		/// </summary>
		/// <param name="groupCounts">Number of work groups for each dimension</param>
		public void Dispatch(Vector3i groupCounts);

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
			public nuint SrcOffset { get; init; }

			/// <summary>
			/// Byte offset of the region in the destination buffer.
			/// </summary>
			public nuint DstOffset { get; init; }

			/// <summary>
			/// Byte length of the region.
			/// </summary>
			public nuint Length { get; init; }

		}

		/// <summary>
		/// Copies regions from the source buffer to the destination buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source buffer</param>
		/// <param name="regions">Regions to copy</param>
		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<CopyBufferRegion> regions);

		/// <summary>
		/// Copies regions from the source buffer to the destination buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source buffer</param>
		/// <param name="regions">Regions to copy</param>
		public void CopyBuffer(IBuffer dst, IBuffer src, params CopyBufferRegion[] regions);

		/// <summary>
		/// Copies a region of the source buffer to the destination buffer.
		/// </summary>
		/// <param name="dst">Destination buffer</param>
		/// <param name="src">Source buffer</param>
		/// <param name="region">Region to copy</param>
		public void CopyBuffer(IBuffer dst, IBuffer src, in CopyBufferRegion region);

		public readonly struct CopyTextureRegion {

			public Vector3i SrcOffset { get; init; }

			/// <summary>
			/// The source subresource layers.
			/// </summary>
			public TextureSubresourceLayers SrcSubresource { get; init; }

			public Vector3i DstOffset { get; init; }

			public TextureSubresourceLayers DstSubresource { get; init; }

			public Vector3i Size { get; init; }

			public TextureAspect Aspect { get; init; }

		}

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyTextureRegion> regions);

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params CopyTextureRegion[] regions);

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in CopyTextureRegion copy);

		public readonly struct BlitTextureRegion {

			public Vector3i SrcOffset0 { get; init; }

			public Vector3i SrcOffset1 { get; init; }

			public uint SrcLevel { get; init; }

			public Vector3i DstOffset0 { get; init; }

			public Vector3i DstOffset1 { get; init; }

			public uint DstLevel { get; init; }

			public TextureAspect Aspect { get; init; }

		}

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<BlitTextureRegion> regions);

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, params BlitTextureRegion[] regions);

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in BlitTextureRegion blit);

		public readonly struct CopyBufferTexture {

			public nuint BufferOffset { get; init; }

			public uint BufferRowLength { get; init; }

			public uint BufferImageHeight { get; init; }

			public Vector3i TextureOffset { get; init; }

			public Vector3i TextureSize { get; init; }

			public TextureSubresourceLayers TextureSubresource { get; init; }

		}

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<CopyBufferTexture> copies);

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, params CopyBufferTexture[] copies);

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in CopyBufferTexture copy);

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyBufferTexture> copies);

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, params CopyBufferTexture[] copies);

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in CopyBufferTexture copy);

		public void UpdateBuffer(IBuffer dst, nuint dstOffset, nuint dstSize, IntPtr pData);

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, nuint dstSize, IConstPointer<T> pData) => UpdateBuffer(dst, dstOffset, dstSize, pData.Ptr);

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, in ReadOnlySpan<T> data) where T : unmanaged;

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, params T[] data) where T : unmanaged;

		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data);

		public readonly struct ClearColorValue {

			public PixelFormat Format { get; init; }

			public Vector4 Float32 { get; init; }

			public Vector4i Int32 { get; init; }

			public Vector4ui UInt32 { get; init; }

		}

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions);

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ClearColorValue color, params TextureSubresourceRange[] regions);

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ClearColorValue color, in TextureSubresourceRange region);

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in ReadOnlySpan<TextureSubresourceRange> regions);

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, params TextureSubresourceRange[] regions);

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in TextureSubresourceRange region);

		public readonly struct ClearValue {

			public TextureAspect Aspect { get; init; }

			public ClearColorValue Color { get; init; }

			public float Depth { get; init; }

			public int Stencil { get; init; }

		}

		public readonly struct ClearAttachment {

			public int Attachment { get; init; }

			public ClearValue Value { get; init; }

		}

		public readonly struct ClearRect {

			public Recti Rect { get; init; }

			public uint BaseArrayLayer { get; init; }

			public uint LayerCount { get; init; }

		}

		public void ClearAttachments(in ReadOnlySpan<ClearAttachment> values, in ReadOnlySpan<ClearRect> regions);

		public void ClearAttachments(in ReadOnlySpan<ClearAttachment> values, params ClearRect[] regions);

		public void ClearAttachments(in ReadOnlySpan<ClearAttachment> values, in ClearRect region);

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<CopyTextureRegion> regions);

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params CopyTextureRegion[] regions);

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in CopyTextureRegion region);

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

		public void SetSync(ISync dst, PipelineStage stage);

		public void ResetSync(ISync dst, PipelineStage stage);

		public struct MemoryBarrier {

			public MemoryAccess ProvokingAccess { get; set; }

			public MemoryAccess AwaitingAccess { get; set; }

		}

		public struct BufferMemoryBarrier {

			public MemoryAccess ProvokingAccess { get; set; }

			public MemoryAccess AwaitingAccess { get; set; }

			public IBuffer Buffer { get; set; }

			public MemoryRange Range { get; set; }

		}

		public struct TextureMemoryBarrier {

			public MemoryAccess ProvokingAccess { get; set; }

			public MemoryAccess AwaitingAccess { get; set; }

			public TextureLayout OldLayout { get; set; }

			public TextureLayout NewLayout { get; set; }

			public ITexture Texture { get; set; }

			public TextureSubresourceRange SubresourceRange { get; set; }

		}

		public readonly ref struct PipelineBarriers {

			public PipelineStage ProvokingStages { get; init; }

			public PipelineStage AwaitingStages { get; init; }

			public ReadOnlySpan<MemoryBarrier> MemoryBarriers { get; init; }

			public ReadOnlySpan<BufferMemoryBarrier> BufferMemoryBarriers { get; init; }

			public ReadOnlySpan<TextureMemoryBarrier> TextureMemoryBarriers { get; init; }

		}

		public void WaitSync(in PipelineBarriers barriers, in ReadOnlySpan<ISync> syncs);

		public void WaitSync(in PipelineBarriers barriers, params ISync[] syncs);

		public void WaitSync(in PipelineBarriers barriers, ISync sync);

		public void Barrier(in PipelineBarriers barriers);

		//================//
		// Push Constants //
		//================//

		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues);

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IConstPointer<T> pValues) => PushConstants(layout, stages, offset, size, pValues.Ptr);

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged;

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, params T[] values) where T : unmanaged;

		//===============//
		// Render Passes //
		//===============//

		public readonly ref struct RenderPassBegin {

			public IRenderPass RenderPass { get; init; }

			public IFramebuffer Framebuffer { get; init; }

			public Recti RenderArea { get; init; }

			public ReadOnlySpan<ClearValue> ClearValues { get; init; }

		}

		public void BeginRenderPass(in RenderPassBegin begin, SubpassContents contents);

		public void NextSubpass(SubpassContents contents);

		public void EndRenderPass();

		//=============================//
		// Secondary Command Execution //
		//=============================//

		public void ExecuteCommands(in ReadOnlySpan<ICommandBuffer> buffers);

		public void ExecuteCommands(params ICommandBuffer[] buffers);

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
	///			<see cref="ICommandSink.EndRenderPass">EndRenderPass</see>
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
		public Vector3ui RequiredTransferGranularity { get; init; }

		/// <summary>
		/// The type of command buffer to create.
		/// </summary>
		public CommandBufferType Type { get; init; }

		/// <summary>
		/// Usage flags for the command buffer.
		/// </summary>
		public CommandBufferUsage Usage { get; init; }

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
