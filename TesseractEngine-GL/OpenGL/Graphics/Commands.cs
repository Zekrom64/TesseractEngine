using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Collections;

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

		public void Barrier(in ICommandSink.PipelineBarriers barriers) {
			// TODO: Should this call glMemoryBarrier?
		}

		// TODO
		public void BeginRendering(in ICommandSink.RenderingInfo renderingInfo) {
			var fbo = Graphics.TransientFramebufferDynamic;
			var state = Graphics.State;


			throw new NotImplementedException();
		}

		public void BeginRenderPass(in ICommandSink.RenderPassBegin begin, SubpassContents contents) {
			if (indirect != null) {
				IRenderPass renderPass = begin.RenderPass;
				IFramebuffer framebuffer = begin.Framebuffer;
				Recti renderArea = begin.RenderArea;
				ICommandSink.ClearValue[] clearValues = begin.ClearValues.ToArray();
				indirect(() => Graphics.State.BeginRenderPass(new ICommandSink.RenderPassBegin() {
					RenderPass = renderPass,
					Framebuffer = framebuffer,
					RenderArea = renderArea,
					ClearValues = clearValues
				}));
			} else {
				Graphics.State.BeginRenderPass(begin);
			}
		}

		public void BindPipeline(IPipeline pipeline) {
			GLPipeline glpipeline = (GLPipeline)pipeline;
			if (indirect != null) indirect(() => Graphics.State.BindPipeline(glpipeline));
			else Graphics.State.BindPipeline(glpipeline);
		}

		public void BindPipelineWithState(IPipelineSet set, PipelineDynamicCreateInfo state) {
			GLPipelineSet glset = (GLPipelineSet)set;
			BindPipeline(glset.BasePipeline);
			if (glset.IsVariable(PipelineDynamicState.DrawMode)) SetDrawMode(state.DrawMode);
			if (glset.IsVariable(PipelineDynamicState.PrimitiveRestartEnable)) SetPrimitiveRestartEnable(state.PrimitiveRestartEnable);
			if (glset.IsVariable(PipelineDynamicState.PatchControlPoints)) SetPatchControlPoints(state.PatchControlPoints);
			if (glset.IsVariable(PipelineDynamicState.Viewport)) SetViewports(state.Viewports.ToArray());
			if (glset.IsVariable(PipelineDynamicState.Scissor)) SetScissors(state.Scissors.ToArray());
			if (glset.IsVariable(PipelineDynamicState.RasterizerDiscardEnable)) SetRasterizerDiscardEnable(state.RasterizerDiscardEnable);
			if (glset.IsVariable(PipelineDynamicState.CullMode)) SetCullMode(state.CullMode);
			if (glset.IsVariable(PipelineDynamicState.FrontFace)) SetFrontFace(state.FrontFace);
			if (glset.IsVariable(PipelineDynamicState.LineWidth)) SetLineWidth(state.LineWidth);
			if (glset.IsVariable(PipelineDynamicState.DepthBiasEnable)) SetDepthBiasEnable(state.DepthBiasEnable);
			if (glset.IsVariable(PipelineDynamicState.DepthBias)) SetDepthBias(state.DepthBiasConstantFactor, state.DepthBiasClamp, state.DepthBiasSlopeFactor);
			if (glset.IsVariable(PipelineDynamicState.DepthTestEnable)) SetDepthTestEnable(state.DepthTestEnable);
			if (glset.IsVariable(PipelineDynamicState.DepthWriteEnable)) SetDepthWriteEnable(state.DepthWriteEnable);
			if (glset.IsVariable(PipelineDynamicState.DepthCompareOp)) SetDepthCompareOp(state.DepthCompareOp);
			if (glset.IsVariable(PipelineDynamicState.DepthBoundsTestEnable)) SetDepthBoundsTestEnable(state.DepthBoundsTestEnable);
			if (glset.IsVariable(PipelineDynamicState.StencilTestEnable)) SetStencilTestEnable(state.StencilTestEnable);
			if (glset.IsVariable(PipelineDynamicState.StencilReference)) {
				var reference = ((int)state.FrontStencilState.Reference, (int)state.BackStencilState.Reference);
				if (indirect != null) {
					indirect(() => {
						Graphics.State.SetStencilReference(GLFace.Front, reference.Item1);
						Graphics.State.SetStencilReference(GLFace.Back, reference.Item2);
					});
				} else {
					Graphics.State.SetStencilReference(GLFace.Front, reference.Item1);
					Graphics.State.SetStencilReference(GLFace.Back, reference.Item2);
				}
			}
			if (glset.IsVariable(PipelineDynamicState.StencilCompareMask)) {
				var mask = (state.FrontStencilState.CompareMask, state.BackStencilState.CompareMask);
				if (indirect != null) {
					indirect(() => {
						Graphics.State.SetStencilCompareMask(GLFace.Front, mask.Item1);
						Graphics.State.SetStencilCompareMask(GLFace.Back, mask.Item2);
					});
				} else {
					Graphics.State.SetStencilCompareMask(GLFace.Front, mask.Item1);
					Graphics.State.SetStencilCompareMask(GLFace.Back, mask.Item2);
				}
			}
			if (glset.IsVariable(PipelineDynamicState.StencilOp)) {
				var front = state.FrontStencilState;
				SetStencilOp(CullFace.Front, front.FailOp, front.PassOp, front.DepthFailOp, front.CompareOp);
				var back = state.BackStencilState;
				SetStencilOp(CullFace.Back, back.FailOp, back.PassOp, back.DepthFailOp, back.CompareOp);
			}
			if (glset.IsVariable(PipelineDynamicState.StencilWriteMask)) {
				var mask = (state.FrontStencilState.WriteMask, state.BackStencilState.WriteMask);
				if (indirect != null) {
					indirect(() => {
						Graphics.State.SetStencilWriteMask(GLFace.Front, mask.Item1);
						Graphics.State.SetStencilWriteMask(GLFace.Back, mask.Item2);
					});
				} else {
					Graphics.State.SetStencilWriteMask(GLFace.Front, mask.Item1);
					Graphics.State.SetStencilWriteMask(GLFace.Back, mask.Item2);
				}
			}

			if (glset.IsVariable(PipelineDynamicState.DepthBounds)) SetDepthBounds(state.DepthBounds.Min, state.DepthBounds.Max);
			if (glset.IsVariable(PipelineDynamicState.LogicOp)) SetLogicOp(state.LogicOp);
			if (glset.IsVariable(PipelineDynamicState.BlendConstants)) SetBlendConstants(state.BlendConstant);
		}

		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, IBindSet set) {
			GLBindSet glset = (GLBindSet)set;
			if (indirect != null) indirect(glset.Bind);
			else glset.Bind();
		}

		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, IReadOnlyList<IBindSet> sets) {
			foreach (IBindSet set in sets) BindResources(bindPoint, layout, set);
		}

		public void BindVertexArray(IVertexArray array) {
			GLVertexArray glarray = (GLVertexArray)array;
			if (indirect != null) indirect(() => GL.GL33!.BindVertexArray(glarray.ID));
			else GL.GL33!.BindVertexArray(glarray.ID);
		}

		public void BlitFramebuffer(IFramebuffer dst, int dstAttachment, TextureLayout dstLayout, Recti dstArea, IFramebuffer src, int srcAttachment, TextureLayout srcLayout, Recti srcArea, TextureAspect aspect, TextureFilter filter) {
			GLFramebuffer gldst = (GLFramebuffer)dst;
			GLFramebuffer glsrc = (GLFramebuffer)src;
			var glmask = GLEnums.Convert(aspect);
			var glfilter = GLEnums.Convert(filter);

			// Requires glBlit* if either is the default, scaling is required, or we cannot copy textures directly
			bool requiresBlit = gldst.IsDefault || glsrc.IsDefault || dstArea.Size != srcArea.Size || GL.ARBCopyImage == null;
			GLTextureView? viewSrc = null, viewDst = null;
			// Also requires glBlit* if either attachment is a renderbuffer and not a true texture
			if (!requiresBlit) {
				viewSrc = glsrc.AttachmentViews![srcAttachment];
				viewDst = gldst.AttachmentViews![dstAttachment];
			}

			// TODO
			throw new NotImplementedException();
			/*
			if (requiresBlit) {
				if (indirect != null) {
					indirect(() => {
						uint dstfbo = gldst.GetFBOForAttachment(dstAttachment, aspect);
						uint srcfbo = glsrc.GetFBOForAttachment(srcAttachment, aspect);
						Interface.BlitFramebuffer(srcfbo, dstfbo, srcArea, dstArea, glmask, glfilter);
					});
				} else {
					uint dstfbo = gldst.GetFBOForAttachment(dstAttachment, aspect);
					uint srcfbo = glsrc.GetFBOForAttachment(srcAttachment, aspect);
					Interface.BlitFramebuffer(srcfbo, dstfbo, srcArea, dstArea, glmask, glfilter);
				}
			} else {
				if (indirect != null) {
					indirect(() => Interface.CopyImageSubData(viewDst!, 0, new Vector3i(srcArea.Position, 0), viewSrc!, 0, new Vector3i(dstArea.Position, 0), new Vector3i(srcArea.Size, 1)));
				} else {
					Interface.CopyImageSubData(viewDst!, 0, new Vector3i(srcArea.Position, 0), viewSrc!, 0, new Vector3i(dstArea.Position, 0), new Vector3i(srcArea.Size, 1));
				}
				
			}
			*/
		}

		// TODO
		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<ICommandSink.BlitTextureRegion> regions) => throw new NotImplementedException();

		private void ClearAttachmentsImpl(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ReadOnlySpan<ICommandSink.ClearRect> regions) {
			var state = Graphics.State;
			var fb = state.CurrentFramebuffer;
			if (fb == null) throw new InvalidOperationException("Cannot clear attachments with no framebuffer bound");
			var iface = Graphics.Interface;
			foreach (var region in regions) {
				state.SetTempScissor(region.Rect);
				for (int i = 0; i < region.LayerCount; i++) {
					foreach (var attach in values) {
						(uint fbo, GLFramebufferAttachment glattach) = fb.GetFBOForAttachment(attach.Attachment, attach.Value.Aspect, (int)region.BaseArrayLayer + i);
						iface.ClearFramebuffer(fbo, glattach, attach.Value, fb.AttachmentViews![attach.Attachment].Format);
					}
				}
			}
			state.UnsetTempScissor();
		}

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ReadOnlySpan<ICommandSink.ClearRect> regions) {
			if (indirect != null) {
				var values2 = values.ToArray();
				var regions2 = regions.ToArray();
				indirect(() => ClearAttachmentsImpl(values2, regions2));
			} else ClearAttachmentsImpl(values, regions);
		}

		private void ClearColorTextureImpl(GLTexture gldst, ICommandSink.ClearValue value, in ReadOnlySpan<TextureSubresourceRange> regions) {
			var iface = Graphics.Interface;
			foreach (var region in regions) {
				Vector3i offset = Vector3i.Zero;
				Vector3i size = (Vector3i)gldst.Size;
				if (gldst.Type == TextureType.Texture1DArray) {
					offset.Y = (int)region.BaseArrayLayer;
					size.Y = (int)region.ArrayLayerCount;
				} else if (gldst.ArrayLayers > 1) {
					offset.Z = (int)region.BaseArrayLayer;
					size.Z = (int)region.ArrayLayerCount;
				}
				for (int i = 0; i < region.MipLevelCount; i++) {
					iface.ClearTexSubImage(gldst, (int)(region.BaseMipLevel + i), offset, size, value, GLBufferMask.Color);
				}
			}
		}

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions) {
			GLTexture gldst = (GLTexture)dst;
			var iface = Graphics.Interface;
			ICommandSink.ClearValue value = new() {
				Aspect = TextureAspect.Color,
				Color = color
			};
			if (indirect != null) {
				var regions2 = regions.ToArray();
				indirect(() => ClearColorTextureImpl(gldst, value, regions2));
			} else ClearColorTextureImpl(gldst, value, regions);
		}

		private void ClearDepthStencilTextureImpl(GLTexture gldst, ICommandSink.ClearValue value, in ReadOnlySpan<TextureSubresourceRange> regions) {
			var iface = Graphics.Interface;
			foreach (var region in regions) {
				Vector3i offset = Vector3i.Zero;
				Vector3i size = (Vector3i)gldst.Size;
				if (gldst.Type == TextureType.Texture1DArray) {
					offset.Y = (int)region.BaseArrayLayer;
					size.Y = (int)region.ArrayLayerCount;
				} else if (gldst.ArrayLayers > 1) {
					offset.Z = (int)region.BaseArrayLayer;
					size.Z = (int)region.ArrayLayerCount;
				}
				for (int i = 0; i < region.MipLevelCount; i++) {
					iface.ClearTexSubImage(gldst, (int)(region.BaseMipLevel + i), offset, size, value, GLEnums.Convert(value.Aspect));
				}
			}
		}

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, int stencil, in ReadOnlySpan<TextureSubresourceRange> regions) {
			GLTexture gldst = (GLTexture)dst;
			var iface = Graphics.Interface;
			ICommandSink.ClearValue value = new() {
				Aspect = dst.Format.Aspects,
				Depth = depth,
				Stencil = stencil
			};
			if ((value.Aspect & TextureAspect.Color) != 0) return;
			if (indirect != null) {
				var regions2 = regions.ToArray();
				indirect(() => ClearDepthStencilTextureImpl(gldst, value, regions2));
			} else ClearDepthStencilTextureImpl(gldst, value, regions);
		}

		private void CopyBufferImpl(GLBuffer gldst, GLBuffer glsrc, in ReadOnlySpan<ICommandSink.CopyBufferRegion> regions) {
			foreach (ICommandSink.CopyBufferRegion region in regions)
				Interface.CopyBufferSubData(glsrc.ID, gldst.ID, (nint)region.SrcOffset, (nint)region.DstOffset, (nint)region.Length);
		}

		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferRegion> regions) {
			GLBuffer gldst = (GLBuffer)dst, glsrc = (GLBuffer)src;
			if (indirect != null) {
				var regions2 = regions.ToArray();
				indirect(() => CopyBufferImpl(gldst, glsrc, regions2));
			} else CopyBufferImpl(gldst, glsrc, regions);
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

		private void CopyBufferToTextureImpl(GLTexture gldst, GLBuffer glsrc, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			var state = Graphics.State;
			state.BindBuffer(GLBufferTarget.PixelUnpack, glsrc.ID);
			foreach (var copy in copies) {
				var dstsubresource = copy.TextureSubresource;
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
			}
		}

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			GLTexture gldst = (GLTexture)dst;
			GLBuffer glsrc = (GLBuffer)src;
			if (indirect != null) {
				var copies2 = copies.ToArray();
				indirect(() => CopyBufferToTextureImpl(gldst, glsrc, copies2));
			} else CopyBufferToTextureImpl(gldst, glsrc, copies);
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
				if (indirect != null) indirect(() => Interface.CopyImageSubData(gldst, dstMipLevel, dstoffset, glsrc, srcMipLevel, srcoffset, size));
				else Interface.CopyImageSubData(gldst, dstMipLevel, dstoffset, gldst, dstMipLevel, dstoffset, size);
			}
		}

		private void CopyTextureToBufferImpl(GLTexture glsrc, GLBuffer gldst, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			var state = Graphics.State;
			state.BindBuffer(GLBufferTarget.PixelPack, gldst.ID);
			foreach (var copy in copies) {
				Vector3i offset = (Vector3i)copy.TextureOffset;
				SubresourceToOffset(glsrc, copy.TextureSubresource, ref offset);
				Vector3i size = (Vector3i)copy.TextureSize;
				// Setup pixel parameters
				state.PixelStore(GLPixelStoreParam.PackRowLength, (int)copy.BufferRowLength);
				state.PixelStore(GLPixelStoreParam.PackImageHeight, (int)copy.BufferImageHeight);
				// Copy texture to buffer
				Interface.GetTextureSubImage(glsrc, (int)copy.TextureSubresource.MipLevel, offset, size, (int)(gldst.Size - copy.BufferOffset), (IntPtr)(ulong)copy.BufferOffset);
			}
		}

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			GLTexture glsrc = (GLTexture)src;
			GLBuffer gldst = (GLBuffer)dst;
			foreach (var copy in copies) {
				// If limited texture-to-buffer (ie. no GL_ARB_get_texture_sub_image), validate parameters
				if (Graphics.Features.LimitedTextureCopyToBuffer) {
					if (copy.TextureOffset != Vector3ui.Zero) throw new GLException("Cannot copy texture-to-buffer with offset != 0 without GL_ARB_get_texture_sub_image");
					if (copy.TextureSize != glsrc.Size) throw new GLException("Cannot copy texture-to-buffer with size != src.size without GL_ARB_get_texture_sub_image");
				}
			}

			if (indirect != null) {
				var copies2 = copies.ToArray();
				indirect(() => CopyTextureToBufferImpl(glsrc, gldst, copies2));
			} else CopyTextureToBufferImpl(glsrc, gldst, copies);
		}

		public void Dispatch(Vector3ui groupCounts) {
			if (indirect != null) indirect(() => Interface.Dispatch(groupCounts));
			else Interface.Dispatch(groupCounts);
		}

		public void DispatchIndirect(IBuffer buffer, nuint offset) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			if (indirect != null) indirect(() => Interface.DispatchIndirect(glbuffer.ID, (nint)offset));
			else Interface.DispatchIndirect(glbuffer.ID, (nint)offset);
		}

		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) {
			if (indirect != null) indirect(() => Interface.Draw(vertexCount, instanceCount, firstVertex, firstInstance));
			else Interface.Draw(vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) {
			if (indirect != null) indirect(() => Interface.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance));
			else Interface.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawIndexedParams.SizeOf) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			if (indirect != null) indirect(() => Interface.DrawIndexedIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride));
			else Interface.DrawIndexedIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride);
		}

		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride = DrawParams.SizeOf) {
			GLBuffer glbuffer = (GLBuffer)buffer;
			if (indirect != null) indirect(() => Interface.DrawIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride));
			else Interface.DrawIndirect(glbuffer.ID, (nint)offset, (int)drawCount, (int)stride);
		}

		// TODO
		public void EndRendering() { }

		public void EndRenderPass() {
			if (indirect != null) indirect(Graphics.State.EndRenderPass);
			else Graphics.State.EndRenderPass();
		}

		public void ExecuteCommands(IReadOnlyList<ICommandBuffer> buffers) {
			foreach (ICommandBuffer cmd in buffers) ExecuteCommands(cmd);
		}

		public void ExecuteCommands(ICommandBuffer buffer) {
			GLCommandBuffer glbuffer = (GLCommandBuffer)buffer;
			if (indirect != null) indirect(glbuffer.RunCommands);
			else glbuffer.RunCommands();
		}

		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data) {
			GLBuffer gldst = (GLBuffer)dst;
			if (indirect != null) indirect(() => Interface.FillBufferUInt32(gldst.ID, (nint)dstOffset, (nint)dstSize, data));
			else Interface.FillBufferUInt32(gldst.ID, (nint)dstOffset, (nint)dstSize, data);
		}

		public void GenerateMipmaps(ITexture dst, TextureLayout initialLayout, TextureLayout finalLayout, TextureFilter? filter = null) {
			if (dst.MipLevels < 2) return;
			GLTexture gldst = (GLTexture)dst;
			if (indirect != null) indirect(() => Interface.GenerateMipmaps(gldst, filter != null ? GLEnums.Convert(filter.Value) : null));
			else Interface.GenerateMipmaps(gldst, filter != null ? GLEnums.Convert(filter.Value) : null);
		}

		public void NextSubpass(SubpassContents contents) {
			if (indirect != null) indirect(Graphics.State.NextSubpass);
			else Graphics.State.NextSubpass();
		}

		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues) =>
			throw new GLException("Push constants are not supported on OpenGL");

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged =>
			throw new GLException("Push constants are not supported on OpenGL");

		public void ResetSync(ISync dst, PipelineStage stage) => throw new GLException("ResetSync is unsupported on OpenGL");

		private void ResolveTextureImpl(GLTexture gldst, GLTexture glsrc, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) {
			foreach (var region in regions) {
				int layers = (int)Math.Min(region.SrcSubresource.LayerCount, region.DstSubresource.LayerCount);
				Recti srcRegion = new((Vector2i)region.SrcOffset.Swizzle(0, 1), (Vector2i)region.Size.Swizzle(0, 1));
				Recti dstRegion = new((Vector2i)region.DstOffset.Swizzle(0, 1), srcRegion.Size);
				for (int i = 0; i < layers; i++) {
					var srcFbo = Graphics.TransientFramebufferSrc;
					var dstFbo = Graphics.TransientFramebufferDst;
					var mask = GLEnums.Convert(region.Aspect);
					Graphics.SetAttachmentsForAspect(srcFbo, glsrc, region.Aspect, (int)region.SrcSubresource.MipLevel, (int)region.SrcSubresource.BaseArrayLayer + i);
					Graphics.SetAttachmentsForAspect(dstFbo, gldst, region.Aspect, (int)region.DstSubresource.MipLevel, (int)region.DstSubresource.BaseArrayLayer + i);
					Graphics.Interface.BlitFramebuffer(srcFbo, dstFbo, srcRegion, dstRegion, mask, GLFilter.Nearest);
				}
			}
		}

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) {
			GLTexture gldst = (GLTexture)dst, glsrc = (GLTexture)src;
			if (indirect != null) {
				var regions2 = regions.ToArray();
				indirect(() => ResolveTextureImpl(gldst, glsrc, regions2));
			} else ResolveTextureImpl(gldst, glsrc, regions);
		}

		public void SetBlendConstants(Vector4 blendConst) {
			if (indirect != null) indirect(() => Graphics.State.SetBlendConstants(blendConst));
			else Graphics.State.SetBlendConstants(blendConst);
		}

		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) {
			GLPipeline.DepthBiasFactors biasFactors = new() {
				ConstantFactor = constFactor,
				SlopeFactor = slopeFactor,
				Clamp = clamp
			};
			if (indirect != null) indirect(() => Graphics.State.SetDepthBias(biasFactors));
			else Graphics.State.SetDepthBias(biasFactors);
		}

		public void SetDepthBounds(float min, float max) {
			if (indirect != null) indirect(() => Graphics.State.SetDepthBounds(min, max));
			else Graphics.State.SetDepthBounds(min, max);
		}

		public void SetLineWidth(float lineWidth) {
			if (indirect != null) indirect(() => Graphics.State.SetLineWidth(lineWidth));
			else Graphics.State.SetLineWidth(lineWidth);
		}

		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0) {
			if (indirect != null) {
				Recti[] scissors2 = scissors.ToArray();
				indirect(() => Graphics.State.SetScissors(scissors2, firstScissor));
			} else Graphics.State.SetScissors(scissors, firstScissor);
		}

		public void SetStencilCompareMask(CullFace face, uint compareMask) {
			if (face == CullFace.None) return;
			var glface = GLEnums.Convert(face);
			if (indirect != null) indirect(() => Graphics.State.SetStencilCompareMask(glface, compareMask));
			else Graphics.State.SetStencilCompareMask(glface, compareMask);
		}

		public void SetStencilReference(CullFace face, uint reference) {
			if (face == CullFace.None) return;
			var glface = GLEnums.Convert(face);
			if (indirect != null) indirect(() => Graphics.State.SetStencilReference(glface, (int)reference));
			else Graphics.State.SetStencilReference(glface, (int)reference);
		}

		public void SetStencilWriteMask(CullFace face, uint writeMask) {
			if (face == CullFace.None) return;
			var glface = GLEnums.Convert(face);
			if (indirect != null) indirect(() => Graphics.State.SetStencilWriteMask(glface, writeMask));
			else Graphics.State.SetStencilWriteMask(glface, writeMask);
		}

		public void SetSync(ISync dst, PipelineStage stage) => throw new GLException("SetSync is unsupported on OpenGL");

		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint firstViewport = 0) {
			if (viewports.Length == 0) return;
			if (indirect != null) {
				Viewport[] viewports2 = viewports.ToArray();
				indirect(() => Graphics.State.SetViewports(viewports2, firstViewport));
			} else Graphics.State.SetViewports(viewports, firstViewport);
		}

		public void SetCullMode(CullFace culling) {
			GLFace face = GLEnums.Convert(culling);
			if (indirect != null) indirect(() => Graphics.State.SetCullMode(culling != CullFace.None, face));
			else Graphics.State.SetCullMode(culling != CullFace.None, face);
		}

		public void SetDepthBoundsTestEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetDepthBoundsTestEnable(enabled));
			else Graphics.State.SetDepthBoundsTestEnable(enabled);
		}

		public void SetDepthCompareOp(CompareOp op) {
			GLCompareFunc glop = GLEnums.Convert(op);
			if (indirect != null) indirect(() => Graphics.State.SetDepthCompareOp(glop));
			else Graphics.State.SetDepthCompareOp(glop);
		}

		public void SetDepthTestEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetDepthTestEnable(enabled));
			else Graphics.State.SetDepthTestEnable(enabled);
		}

		public void SetDepthWriteEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetDepthWriteEnable(enabled));
			else Graphics.State.SetDepthWriteEnable(enabled);
		}

		public void SetFrontFace(FrontFace face) {
			GLCullFace glface = GLEnums.Convert(face);
			if (indirect != null) indirect(() => Graphics.State.SetFrontFace(glface));
			else Graphics.State.SetFrontFace(glface);
		}

		public void SetDrawMode(DrawMode mode) {
			GLDrawMode glmode = GLEnums.Convert(mode);
			if (indirect != null) indirect(() => Graphics.State.SetDrawMode(glmode));
			else Graphics.State.SetDrawMode(glmode);
		}

		public void SetScissorsWithCount(in ReadOnlySpan<Recti> scissors) => SetScissors(scissors);

		public void SetStencilOp(CullFace faces, StencilOp failOp, StencilOp passOp, StencilOp depthFailOp, CompareOp compareOp) {
			if (faces == CullFace.None) return;
			GLPipeline.StencilOpState state = new() {
				FailOp = GLEnums.Convert(failOp),
				PassOp = GLEnums.Convert(passOp),
				DepthFailOp = GLEnums.Convert(depthFailOp),
				CompareOp = GLEnums.ConvertStencilFunc(compareOp)
			};
			GLFace glfaces = GLEnums.Convert(faces);
			if (indirect != null) indirect(() => Graphics.State.SetStencilOp(glfaces, state));
			else Graphics.State.SetStencilOp(glfaces, state);
		}

		public void SetStencilTestEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetStencilTestEnable(enabled));
			else Graphics.State.SetStencilTestEnable(enabled);
		}

		public void SetViewportsWithCount(in ReadOnlySpan<Viewport> viewports) => SetViewports(viewports);

		public void SetDepthBiasEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetDepthBiasEnable(enabled));
			else Graphics.State.SetDepthBiasEnable(enabled);
		}

		public void SetLogicOp(LogicOp op) {
			GLLogicOp glop = GLEnums.Convert(op);
			if (indirect != null) indirect(() => Graphics.State.SetLogicOp(glop));
			else Graphics.State.SetLogicOp(glop);
		}

		public void SetPatchControlPoints(uint controlPoints) {
			if (indirect != null) indirect(() => Graphics.State.SetPatchControlPoints(controlPoints));
			else Graphics.State.SetPatchControlPoints(controlPoints);
		}

		public void SetPrimitiveRestartEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetPrimitiveRestartEnable(enabled));
			else Graphics.State.SetPrimitiveRestartEnable(enabled);
		}

		public void SetRasterizerDiscardEnable(bool enabled) {
			if (indirect != null) indirect(() => Graphics.State.SetRasterizerDiscardEnable(enabled));
			else Graphics.State.SetRasterizerDiscardEnable(enabled);
		}

		public void SetVertexFormat(VertexFormat format) { } // No-op, vertex specification is done via vertex arrays

		public void SetColorWriteEnable(in ReadOnlySpan<bool> enables) {
			if (indirect != null) {
				bool[] enables2 = enables.ToArray();
				indirect(() => Graphics.State.SetColorWriteEnable(enables2));
			} else Graphics.State.SetColorWriteEnable(enables);
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

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, ISync sync) { }

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, IReadOnlyList<ISync> syncs) { }

	}

}
