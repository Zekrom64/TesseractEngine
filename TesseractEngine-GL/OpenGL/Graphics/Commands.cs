using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// OpenGL command buffer implementation.
	/// </summary>
	public class GLCommandBuffer : ICommandBuffer {

		public GLGraphics Graphics { get; }

		public ulong QueueID => 0;

		public CommandBufferType Type { get; }

		// The list of command delegates
		private readonly List<Action> commands = new();

		public GLCommandBuffer(GLGraphics graphics, CommandBufferCreateInfo createInfo) {
			Graphics = graphics;
			Type = createInfo.Type;
		}

		public ICommandSink BeginRecording() => new GLCommandSink(Graphics, commands.Add); // Sink into command buffer

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public void EndRecording() { } // No-op

		/// <summary>
		/// Runs the list of commands stored in the command buffer.
		/// </summary>
		public void RunCommands() {
			foreach (Action cmd in commands) cmd();
		}

	}

	public class GLCommandSink : ICommandSink {

		public CommandMode Mode => CommandMode.Immediate;

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		// The OpenGL interface
		private GLInterface Interface => Graphics.Interface;

		// Delegate to indirect commands through (usually a command buffer).
		private readonly Action<Action>? indirect;

		public GLCommandSink(GLGraphics graphics, Action<Action>? indirect = null) {
			Graphics = graphics;
			this.indirect = indirect;
		}

		// Performs the implementation of a command based on the sink's configuration (indirect or not).
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DoImpl(Action act) {
			if (indirect != null) indirect(act);
			else act();
		}

		public void Barrier(in ICommandSink.PipelineBarriers barriers) {
			
		} // TODO: Should this call glMemoryBarrier?

		// TODO
		public void BeginRendering(in ICommandSink.RenderingInfo renderingInfo) {
			throw new NotImplementedException();
		}

		public void BeginRenderPass(in ICommandSink.RenderPassBegin begin, SubpassContents contents) {
			IRenderPass renderPass = begin.RenderPass;
			IFramebuffer framebuffer = begin.Framebuffer;
			Recti renderArea = begin.RenderArea;
			ICommandSink.ClearValue[] clearValues = begin.ClearValues.ToArray();
			DoImpl(() => Graphics.State.BeginRenderPass(new ICommandSink.RenderPassBegin() {
				RenderPass = renderPass,
				Framebuffer = framebuffer,
				RenderArea = renderArea,
				ClearValues = clearValues
			}));
		}

		public void BindPipeline(IPipeline pipeline) {
			GLPipeline glpipeline = (GLPipeline)pipeline;
			DoImpl(() => Graphics.State.BindPipeline(glpipeline));
		}

		// TODO
		public void BindPipelineWithState(IPipelineSet set, PipelineDynamicCreateInfo state) {
			GLPipelineSet glset = (GLPipelineSet)set;
			BindPipeline(glset.BasePipeline);

			if (glset.IsVariable(PipelineDynamicState.Viewport)) SetViewports(state.Viewports.ToArray());
		}

		// TODO
		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, params IBindSet[] sets) => throw new NotImplementedException();

		public void BindVertexArray(IVertexArray array) {
			GLVertexArray glarray = (GLVertexArray)array;
			DoImpl(() => GL.GL33!.BindVertexArray(glarray.ID));
		}

		// TODO
		public void BlitFramebuffer(IFramebuffer dst, int dstAttachment, TextureLayout dstLayout, Recti dstArea, IFramebuffer src, int srcAttachment, TextureLayout srcLayout, Recti srcArea, TextureAspect aspect, TextureFilter filter) => throw new NotImplementedException();

		// TODO
		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<ICommandSink.BlitTextureRegion> regions) => throw new NotImplementedException();

		// TODO
		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ReadOnlySpan<ICommandSink.ClearRect> regions) {
			GLFramebuffer? fb = Graphics.State.CurrentFramebuffer;

		}

		// TODO
		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions) => throw new NotImplementedException();

		// TODO
		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in ReadOnlySpan<TextureSubresourceRange> regions) => throw new NotImplementedException();

		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferRegion> regions) {
			GLBuffer gldst = (GLBuffer)dst, glsrc = (GLBuffer)src;
			foreach (ICommandSink.CopyBufferRegion region in regions)
				DoImpl(() => Interface.CopyBufferSubData(glsrc.ID, gldst.ID, (nint)region.SrcOffset, (nint)region.DstOffset, (nint)region.Length));
		}

		private static void SubresourceToOffset(GLTexture tex, TextureSubresourceLayers layers, ref Vector3i offset) {
			switch(tex.GLTarget) {
				case GLTextureTarget.Texture1DArray:
					offset.Y = (int)layers.BaseArrayLayer;
					break;
				case GLTextureTarget.Texture2DArray:
				case GLTextureTarget.Texture2DMultisampleArray:
				case GLTextureTarget.CubeMap:
				case GLTextureTarget.CubeMapArray:
					offset.Z = (int)layers.BaseArrayLayer;
					break;
				case GLTextureTarget.Texture1D:
				case GLTextureTarget.Texture2D:
				case GLTextureTarget.Texture2DMultisample:
				default:
					break;
			}
		}

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			GLTexture gldst = (GLTexture)dst;
			GLBuffer glsrc = (GLBuffer)src;
			var state = Graphics.State;
			DoImpl(() => state.BindBuffer(GLBufferTarget.PixelUnpack, glsrc.ID));
			foreach (var copy in copies) {
				var dstsubresource = copy.TextureSubresource;
				DoImpl(() => {
					state.PixelStore(GLPixelStoreParam.UnpackRowLength, (int)copy.BufferRowLength);
					state.PixelStore(GLPixelStoreParam.UnpackImageHeight, (int)copy.BufferImageHeight);
					Interface.TextureSubImage(
						gldst,
						(int)dstsubresource.MipLevel,
						(Vector3i)copy.TextureOffset,
						(Vector3i)copy.TextureSize,
						(IntPtr)(nint)copy.BufferOffset,
						(int)(copy.BufferImageHeight * copy.BufferRowLength * gldst.Format.SizeOf)
					);
				});
			}
		}

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) {
			GLTexture gldst = (GLTexture)dst, glsrc = (GLTexture)src;
			foreach (var region in regions) {
				Vector3i srcoffset = (Vector3i)region.SrcOffset, dstoffset = (Vector3i)region.DstOffset;
				SubresourceToOffset(glsrc, region.SrcSubresource, ref srcoffset);
				SubresourceToOffset(gldst, region.DstSubresource, ref dstoffset);
				int dstMipLevel = (int)region.DstSubresource.MipLevel;
				int srcMipLevel = (int)region.SrcSubresource.MipLevel;
				Vector3i size = (Vector3i)region.Size;
				DoImpl(() => Interface.CopyImageSubData(gldst, dstMipLevel, dstoffset, glsrc, srcMipLevel, srcoffset, size));
			}
		}

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			GLTexture glsrc = (GLTexture)src;
			GLBuffer gldst = (GLBuffer)dst;
			var state = Graphics.State;
			DoImpl(() => {
				state.BindBuffer(GLBufferTarget.PixelPack, gldst.ID);
			});
			foreach(var copy in copies) {
				Vector3i offset = (Vector3i)copy.TextureOffset;
				SubresourceToOffset(glsrc, copy.TextureSubresource, ref offset);
				Vector3i size = (Vector3i)copy.TextureSize;
				// If limited texture-to-buffer (ie. no GL_ARB_get_texture_sub_image), validate parameters
				if (Graphics.Features.LimitedTextureCopyToBuffer) {
					if (offset != Vector3i.Zero) throw new GLException("Cannot copy texture-to-buffer with offset != 0 without GL_ARB_get_texture_sub_image");
					if (size != (Vector3i)glsrc.Size) throw new GLException("Cannot copy texture-to-buffer with size != src.size without GL_ARB_get_texture_sub_image");
				}
				DoImpl(() => {
					// Setup pixel parameters
					state.PixelStore(GLPixelStoreParam.PackRowLength, (int)copy.BufferRowLength);
					state.PixelStore(GLPixelStoreParam.PackImageHeight, (int)copy.BufferImageHeight);
					// Copy texture to buffer
					Interface.GetTextureSubImage(glsrc, (int)copy.TextureSubresource.MipLevel, offset, size, (int)(dst.Size - copy.BufferOffset), (IntPtr)(ulong)copy.BufferOffset);
				});
			}
		}

		public void Dispatch(Vector3ui groupCounts) =>
			DoImpl(() => Interface.Dispatch(groupCounts));

		public void DispatchIndirect(IBuffer buffer, nuint offset) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			DoImpl(() => Interface.DispatchIndirect(glbuffer.ID, (nint)offset));
		}

		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) =>
			DoImpl(() => Interface.Draw(vertexCount, instanceCount, firstVertex, firstInstance));

		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) =>
			DoImpl(() => Interface.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance));

		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawIndexedParams.SizeOf) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			DoImpl(() => Interface.DrawIndexedIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride));
		}

		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawParams.SizeOf) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			DoImpl(() => Interface.DrawIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride));
		}

		// TODO
		public void EndRendering() {
			throw new NotImplementedException();
		}

		public void EndRenderPass() =>
			DoImpl(Graphics.State.EndRenderPass);

		public void ExecuteCommands(in ReadOnlySpan<ICommandBuffer> buffers) {
			foreach (ICommandBuffer cmd in buffers) ExecuteCommands(cmd);
		}

		public void ExecuteCommands(ICommandBuffer buffer) =>
			DoImpl(((GLCommandBuffer)buffer).RunCommands);

		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data) {
			GLBuffer gldst = (GLBuffer)dst;
			DoImpl(() => Interface.FillBufferUInt32(gldst.ID, (nint)dstOffset, (nint)dstSize, data));
		}

		public void GenerateMipmaps(ITexture dst, TextureLayout initialLayout, TextureLayout finalLayout, TextureFilter? filter = null) {
			if (dst.MipLevels < 2) return;
			GLTexture gldst = (GLTexture)dst;
			DoImpl(() => Interface.GenerateMipmaps(gldst, filter != null ? GLEnums.Convert(filter.Value) : null));
		}

		public void NextSubpass(SubpassContents contents) =>
			DoImpl(Graphics.State.NextSubpass);

		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues) =>
			throw new GLException("Push constants are not supported on OpenGL");

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged =>
			throw new GLException("Push constants are not supported on OpenGL");

		// TODO
		public void ResetSync(ISync dst, PipelineStage stage) => throw new NotImplementedException();

		// TODO
		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) => throw new NotImplementedException();

		public void SetBlendConstants(Vector4 blendConst) =>
			DoImpl(() => Graphics.State.SetBlendConstants(blendConst));

		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) =>
			DoImpl(() => Graphics.State.SetDepthBias(new GLPipeline.DepthBiasFactors() {
				ConstantFactor = constFactor,
				SlopeFactor = slopeFactor,
				Clamp = clamp
			}));

		public void SetDepthBounds(float min, float max) =>
			DoImpl(() => Graphics.State.SetDepthBounds(min, max));

		public void SetLineWidth(float lineWidth) =>
			DoImpl(() => Graphics.State.SetLineWidth(lineWidth));

		public void SetScissor(Recti scissor, uint firstScissor = 0) =>
			DoImpl(() => Graphics.State.SetScissors(stackalloc Recti[] { scissor }, firstScissor));

		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0) {
			if (indirect != null) {
				Recti[] scissors2 = scissors.ToArray();
				indirect(() => Graphics.State.SetScissors(scissors2, firstScissor));
			} else Graphics.State.SetScissors(scissors, firstScissor);
		}

		public void SetStencilCompareMask(CullFace face, uint compareMask) {
			if (face == CullFace.None) return;
			DoImpl(() => Graphics.State.SetStencilCompareMask(GLEnums.Convert(face), compareMask));
		}

		public void SetStencilReference(CullFace face, uint reference) {
			if (face == CullFace.None) return;
			DoImpl(() => Graphics.State.SetStencilReference(GLEnums.Convert(face), (int)reference));
		}

		public void SetStencilWriteMask(CullFace face, uint writeMask) {
			if (face == CullFace.None) return;
			DoImpl(() => Graphics.State.SetStencilWriteMask(GLEnums.Convert(face), writeMask));
		}

		public void SetSync(ISync dst, PipelineStage stage) {
			GLSync gldst = (GLSync)dst;
			gldst.GenerateFence(); // TODO: Generate based on sync type?
		}

		public void SetViewport(Viewport viewport, uint firstViewport = 0) =>
			DoImpl(() => Graphics.State.SetViewports(stackalloc Viewport[] { viewport }, firstViewport));

		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint firstViewport = 0) {
			if (viewports.Length == 0) return;
			if (indirect != null) {
				Viewport[] viewports2 = viewports.ToArray();
				indirect(() => Graphics.State.SetViewports(viewports2, firstViewport));
			} else Graphics.State.SetViewports(viewports, firstViewport);
		}

		public void UpdateBuffer(IBuffer dst, nuint dstOffset, nuint dstSize, IntPtr pData) {
			GLBuffer gldst = (GLBuffer)dst;
			if (indirect != null) {
				byte[] data2 = new byte[dstSize];
				MemoryUtil.Copy(data2, new UnmanagedPointer<byte>(pData), dstSize);
				indirect(() => {
					unsafe {
						fixed(byte* pData2 = data2) {
							Graphics.Interface.BufferSubData(gldst.ID, (nint)dstOffset, (nint)dstSize, (IntPtr)pData2);
						}
					}
				});
			} else Graphics.Interface.BufferSubData(gldst.ID, (nint)dstOffset, (nint)dstSize, pData);
		}

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, in ReadOnlySpan<T> data) where T : unmanaged {
			GLBuffer gldst = (GLBuffer)dst;
			if (indirect != null) {
				T[] data2 = data.ToArray();
				indirect(() => {
					unsafe {
						fixed(T* pData2 = data2) {
							Graphics.Interface.BufferSubData(gldst.ID, (nint)dstOffset, data2.Length * sizeof(T), (IntPtr)pData2);
						}
					}
				});
			} else {
				unsafe {
					fixed (T* pData = data) {
						Graphics.Interface.BufferSubData(gldst.ID, (nint)dstOffset, data.Length * sizeof(T), (IntPtr)pData);
					}
				}
			}
		}

		// TODO
		public void WaitSync(in ICommandSink.PipelineBarriers barriers, in ReadOnlySpan<ISync> syncs) => throw new NotImplementedException();

		// TODO
		public void WaitSync(in ICommandSink.PipelineBarriers barriers, ISync sync) => throw new NotImplementedException();
	}

}
