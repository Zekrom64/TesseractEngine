using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {

	public class GLImmediateCommandSink : ICommandSink {

		public CommandMode Mode => CommandMode.Immediate;

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public readonly Action<GLTexture, GLTexture, GLFilter, ICommandSink.BlitTextureRegion> FnBlitTexture;
		public readonly Action<GLFramebuffer, Recti, GLFramebuffer, Recti, GLFilter, GLBufferMask> FnBlitFramebuffer;
		public readonly Action<GLBuffer, GLBuffer, ICommandSink.CopyBufferRegion> FnCopyBuffer;
		public readonly Action<GLTexture, GLTexture, ICommandSink.CopyTextureRegion> FnCopyTexture;
		public readonly Action<GLBuffer, nuint, nuint, uint> FnFillBufferUInt32;
		public readonly Action<uint, uint, uint, uint> FnDraw;

		public GLImmediateCommandSink(GLGraphics graphics) {
			Graphics = graphics;

			var gl33 = GL.GL33;
			var ci = GL.ARBCopyImage;
			var dsa = GL.ARBDirectStateAccess;

			if (ci != null) {
				FnCopyTexture = (GLTexture dst, GLTexture src, ICommandSink.CopyTextureRegion region) => {
					Vector3i srcPos = region.SrcOffset, dstPos = region.DstOffset;
					// Correct coordinates based on texture type
					switch (src.Type) {
						case TextureType.Texture1D:
							srcPos.Y = srcPos.Z = 0;
							break;
						case TextureType.Texture1DArray:
							srcPos.Y = (int)region.SrcSubresource.BaseArrayLayer;
							srcPos.Z = 0;
							break;
						case TextureType.Texture2D:
							srcPos.Z = 0;
							break;
						case TextureType.Texture2DArray:
						case TextureType.Texture2DCube:
						case TextureType.Texture2DCubeArray:
							srcPos.Z = (int)region.SrcSubresource.BaseArrayLayer;
							break;
						case TextureType.Texture3D:
							break;
					}

					ci.CopyImageSubData(src.ID, src.GLTarget, (int)region.SrcSubresource.MipLevel, srcPos, dst.ID, dst.GLTarget, (int)region.DstSubresource.MipLevel, dstPos, region.Size);
				};
			}

			if (dsa != null) {
				FnBlitFramebuffer = (GLFramebuffer dst, Recti dstArea, GLFramebuffer src, Recti srcArea, GLFilter filter, GLBufferMask mask) =>
					dsa.BlitNamedFramebuffer(src.ID, dst.ID, srcArea, dstArea, mask, filter);
				FnCopyBuffer = (GLBuffer dst, GLBuffer src, ICommandSink.CopyBufferRegion region) =>
					dsa.CopyNamedBufferSubData(src.ID, dst.ID, (nint)region.SrcOffset, (nint)region.DstOffset, (nint)region.Length);
				FnFillBufferUInt32 = (GLBuffer dst, nuint offset, nuint length, uint data) =>
					dsa.ClearNamedBufferSubData<uint>(dst.ID, (nint)offset, (nint)length, GLInternalFormat.R32I, GLFormat.R, GLType.UnsignedInt, stackalloc uint[] { data });
			}
		}

		public void Barrier(in ICommandSink.PipelineBarriers barriers) { } // TODO: Should this call glMemoryBarrier?

		public void BeginRenderPass(in ICommandSink.RenderPassBegin begin, SubpassContents contents) => Graphics.State.BeginRenderPass(begin);

		public void BindPipeline(IPipeline pipeline) => Graphics.State.BindPipeline((GLPipeline)pipeline);

		public void BindPipelineWithState(IPipelineSet set, PipelineDynamicCreateInfo state) => throw new NotImplementedException();

		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, params IBindSet[] sets) => throw new NotImplementedException();

		public void BindVertexArray(IVertexArray array) => GL.GL33!.BindVertexArray(((GLVertexArray)array).ID);

		public void BlitFramebuffer(IFramebuffer dst, int dstAttachment, TextureLayout dstLayout, Recti dstArea, IFramebuffer src, int srcAttachment, TextureLayout srcLayout, Recti srcArea, TextureAspect aspect, TextureFilter filter) =>
			FnBlitFramebuffer((GLFramebuffer)dst, dstArea, (GLFramebuffer)src, srcArea, GLEnums.Convert(filter), GLEnums.Convert(aspect));

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<ICommandSink.BlitTextureRegion> regions) => throw new NotImplementedException();

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, params ICommandSink.BlitTextureRegion[] regions) => throw new NotImplementedException();

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ICommandSink.BlitTextureRegion blit) => throw new NotImplementedException();

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ReadOnlySpan<ICommandSink.ClearRect> regions) => throw new NotImplementedException();

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, params ICommandSink.ClearRect[] regions) => throw new NotImplementedException();

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ICommandSink.ClearRect region) => throw new NotImplementedException();

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions) => throw new NotImplementedException();

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, params TextureSubresourceRange[] regions) => throw new NotImplementedException();

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in TextureSubresourceRange region) => throw new NotImplementedException();

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in ReadOnlySpan<TextureSubresourceRange> regions) => throw new NotImplementedException();

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, params TextureSubresourceRange[] regions) => throw new NotImplementedException();

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in TextureSubresourceRange region) => throw new NotImplementedException();

		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferRegion> regions) {
			GLBuffer gldst = (GLBuffer)dst, glsrc = (GLBuffer)src;
			foreach (ICommandSink.CopyBufferRegion region in regions) FnCopyBuffer(gldst, glsrc, region);
		}

		public void CopyBuffer(IBuffer dst, IBuffer src, params ICommandSink.CopyBufferRegion[] regions) {
			GLBuffer gldst = (GLBuffer)dst, glsrc = (GLBuffer)src;
			foreach (ICommandSink.CopyBufferRegion region in regions) FnCopyBuffer(gldst, glsrc, region);
		}

		public void CopyBuffer(IBuffer dst, IBuffer src, in ICommandSink.CopyBufferRegion region) =>
			FnCopyBuffer((GLBuffer)dst, (GLBuffer)src, region);

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) => throw new NotImplementedException();

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, params ICommandSink.CopyBufferTexture[] copies) => throw new NotImplementedException();

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ICommandSink.CopyBufferTexture copy) => throw new NotImplementedException();

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) => throw new NotImplementedException();

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyTextureRegion[] regions) => throw new NotImplementedException();

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyTextureRegion copy) => throw new NotImplementedException();

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) => throw new NotImplementedException();

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyBufferTexture[] copies) => throw new NotImplementedException();

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyBufferTexture copy) => throw new NotImplementedException();

		public void Dispatch(Vector3i groupCounts) => throw new NotImplementedException();

		public void DispatchIndirect(IBuffer buffer, nuint offset) => throw new NotImplementedException();

		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) => FnDraw(vertexCount, instanceCount, firstVertex, firstInstance);

		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) => throw new NotImplementedException();

		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride) => throw new NotImplementedException();

		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride) => throw new NotImplementedException();

		public void EndRenderPass() => throw new NotImplementedException();

		public void ExecuteCommands(in ReadOnlySpan<ICommandBuffer> buffers) => throw new NotImplementedException();

		public void ExecuteCommands(params ICommandBuffer[] buffers) => throw new NotImplementedException();

		public void ExecuteCommands(ICommandBuffer buffer) => throw new NotImplementedException();

		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data) => throw new NotImplementedException();

		public void GenerateMipmaps(ITexture dst, TextureLayout initialLayout, TextureLayout finalLayout, TextureFilter? filter = null) => throw new NotImplementedException();

		public void NextSubpass(SubpassContents contents) => throw new NotImplementedException();

		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues) => throw new NotImplementedException();

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged => throw new NotImplementedException();

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, params T[] values) where T : unmanaged => throw new NotImplementedException();

		public void ResetSync(ISync dst, PipelineStage stage) => throw new NotImplementedException();

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) => throw new NotImplementedException();

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyTextureRegion[] regions) => throw new NotImplementedException();

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyTextureRegion region) => throw new NotImplementedException();

		public void SetBlendConstants(Vector4 blendConst) => throw new NotImplementedException();

		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) => throw new NotImplementedException();

		public void SetDepthBounds(float min, float max) => throw new NotImplementedException();

		public void SetLineWidth(float lineWidth) => throw new NotImplementedException();

		public void SetScissor(Recti scissor, uint firstScissor = 0) => throw new NotImplementedException();

		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0) => throw new NotImplementedException();

		public void SetScissors(uint firstScissor, params Recti[] scissors) => throw new NotImplementedException();

		public void SetStencilCompareMask(CullFace face, uint compareMask) => throw new NotImplementedException();

		public void SetStencilReference(CullFace face, uint reference) => throw new NotImplementedException();

		public void SetStencilWriteMask(CullFace face, uint writeMask) => throw new NotImplementedException();

		public void SetSync(ISync dst, PipelineStage stage) => throw new NotImplementedException();

		public void SetViewport(Viewport viewport, uint firstViewport = 0) => throw new NotImplementedException();

		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint firstViewport = 0) => throw new NotImplementedException();

		public void SetViewports(uint firstViewport, params Viewport[] viewports) => throw new NotImplementedException();

		public void UpdateBuffer(IBuffer dst, nuint dstOffset, nuint dstSize, IntPtr pData) => throw new NotImplementedException();

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, in ReadOnlySpan<T> data) where T : unmanaged => throw new NotImplementedException();

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, params T[] data) where T : unmanaged => throw new NotImplementedException();

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, in ReadOnlySpan<ISync> syncs) => throw new NotImplementedException();

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, params ISync[] syncs) => throw new NotImplementedException();

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, ISync sync) => throw new NotImplementedException();

	}

}
