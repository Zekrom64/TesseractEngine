﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKCommandBuffer : IVKDeviceObject, IDisposable {

		public VKCommandPool CommandPool { get; }

		public VKDevice Device => CommandPool.Device;
		
		[NativeType("VkCommandBuffer")]
		public IntPtr CommandBuffer { get; }

		public VKCommandBuffer(VKCommandPool commandPool, IntPtr commandBuffer) {
			CommandPool = commandPool;
			CommandBuffer = commandBuffer;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			IntPtr cmdbuf = CommandBuffer;
			unsafe {
				Device.VK10Functions.vkFreeCommandBuffers(Device, CommandPool, 1, (IntPtr)(&cmdbuf));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(in VKCommandBufferBeginInfo beginInfo) =>
			VK.CheckError(Device.VK10Functions.vkBeginCommandBuffer(CommandBuffer, beginInfo), "Failed to begin command buffer recording");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End() => VK.CheckError(Device.VK10Functions.vkEndCommandBuffer(CommandBuffer), "Failed to end command buffer recording");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset(VKCommandBufferResetFlagBits flags) =>
			VK.CheckError(Device.VK10Functions.vkResetCommandBuffer(CommandBuffer, flags), "Failed to reset command buffer");

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindPipeline(VKPipelineBindPoint bindPoint, VKPipeline pipeline) => Device.VK10Functions.vkCmdBindPipeline(CommandBuffer, bindPoint, pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(VKViewport viewport, uint offset = 0) {
			unsafe {
				Device.VK10Functions.vkCmdSetViewport(CommandBuffer, offset, 1, (IntPtr)(&viewport));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(in ReadOnlySpan<VKViewport> viewports, uint offset = 0) {
			unsafe {
				fixed (VKViewport* pViewports = viewports) {
					Device.VK10Functions.vkCmdSetViewport(CommandBuffer, offset, (uint)viewports.Length, (IntPtr)pViewports);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(uint offset, params VKViewport[] viewports) => SetViewport(viewports, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(params VKViewport[] viewports) => SetViewport(viewports);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(VKRect2D scissor, uint offset = 0) {
			unsafe {
				Device.VK10Functions.vkCmdSetScissor(CommandBuffer, offset, 1, (IntPtr)(&scissor));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(in ReadOnlySpan<VKRect2D> scissors, uint offset = 0) {
			unsafe {
				fixed(VKRect2D* pScissors = scissors) {
					Device.VK10Functions.vkCmdSetScissor(CommandBuffer, offset, (uint)scissors.Length, (IntPtr)pScissors);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(uint offset, params VKRect2D[] scissors) => SetScissor(scissors, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(params VKRect2D[] scissors) => SetScissor(scissors);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetLineWidth(float width) => Device.VK10Functions.vkCmdSetLineWidth(CommandBuffer, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) => Device.VK10Functions.vkCmdSetDepthBias(CommandBuffer, constFactor, clamp, slopeFactor);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBlendConstants(Vector4 blendConstants) {
			unsafe {
				Device.VK10Functions.vkCmdSetBlendConstants(CommandBuffer, (IntPtr)(&blendConstants));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthBounds(float minDepth, float maxDepth) => Device.VK10Functions.vkCmdSetDepthBounds(CommandBuffer, minDepth, maxDepth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilCompareMask(VKStencilFaceFlagBits face, uint mask) => Device.VK10Functions.vkCmdSetStencilCompareMask(CommandBuffer, face, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilWriteMask(VKStencilFaceFlagBits face, uint mask) => Device.VK10Functions.vkCmdSetStencilWriteMask(CommandBuffer, face, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilReference(VKStencilFaceFlagBits face, uint reference) => Device.VK10Functions.vkCmdSetStencilReference(CommandBuffer, face, reference);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, VKDescriptorSet descriptorSet) {
			ulong descSet = descriptorSet;
			unsafe {
				Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, 1, (IntPtr)(&descSet), 0, IntPtr.Zero);
			}
		}

		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, in ReadOnlySpan<VKDescriptorSet> descriptorSets, in ReadOnlySpan<uint> dynamicOffsets) {
			Span<ulong> descSets = stackalloc ulong[descriptorSets.Length];
			for (int i = 0; i < descriptorSets.Length; i++) descSets[i] = descriptorSets[i];
			unsafe {
				fixed (ulong* pDescSets = descSets) {
					fixed (uint* pDynamicOffsets = dynamicOffsets) {
						Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, (uint)descSets.Length, (IntPtr)pDescSets, (uint)dynamicOffsets.Length, (IntPtr)pDynamicOffsets);
					}
				}
			}
		}

		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, params VKDescriptorSet[] descriptorSets) {
			Span<ulong> descSets = stackalloc ulong[descriptorSets.Length];
			for (int i = 0; i < descriptorSets.Length; i++) descSets[i] = descriptorSets[i];
			unsafe {
				fixed (ulong* pDescSets = descSets) {
					Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, (uint)descSets.Length, (IntPtr)pDescSets, 0, IntPtr.Zero);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, params VKDescriptorSet[] descriptorSets) => BindDescriptorSets(bindPoint, layout, 0, descriptorSets);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindIndexBuffer(VKBuffer buffer, ulong offset, VKIndexType indexType) => Device.VK10Functions.vkCmdBindIndexBuffer(CommandBuffer, buffer, offset, indexType);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(VKBuffer buffer, ulong offset = 0) => BindVertexBuffers(0, buffer, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(uint firstBinding, VKBuffer buffer, ulong offset = 0) {
			ulong buf = buffer;
			unsafe {
				Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, 1, (IntPtr)(&buf), (IntPtr)(&offset));
			}
		}

		public void BindVertexBuffers(uint firstBinding, in ReadOnlySpan<VKBuffer> buffers, in ReadOnlySpan<ulong> offsets) {
			Span<ulong> bufs = stackalloc ulong[buffers.Length];
			for (int i = 0; i < buffers.Length; i++) bufs[i] = buffers[i];
			unsafe {
				fixed (ulong* pBufs = bufs, pOffsets = offsets) {
					Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, (uint)Math.Min(bufs.Length, offsets.Length), (IntPtr)pBufs, (IntPtr)pOffsets);
				}
			}
		}

		public void BindVertexBuffers(uint firstBinding, params VKBuffer[] buffers) {
			Span<ulong> bufs = stackalloc ulong[buffers.Length], offsets = stackalloc ulong[buffers.Length];
			for (int i = 0; i < buffers.Length; i++) {
				bufs[i] = buffers[i];
				offsets[i] = 0; // Zero initialize to be safe
			}
			unsafe {
				fixed (ulong* pBufs = bufs, pOffsets = offsets) {
					Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, (uint)Math.Min(bufs.Length, offsets.Length), (IntPtr)pBufs, (IntPtr)pOffsets);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Draw(uint vertexCount, uint instanceCount = 1, uint firstVertex = 0, uint firstInstance = 0) => Device.VK10Functions.vkCmdDraw(CommandBuffer, vertexCount, instanceCount, firstVertex, firstInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndexed(uint indexCount, uint instanceCount = 1, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0) => Device.VK10Functions.vkCmdDrawIndexed(CommandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndirect(VKBuffer buffer, ulong offset, uint drawCount, uint stride) => Device.VK10Functions.vkCmdDrawIndirect(CommandBuffer, buffer, offset, drawCount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndexedIndirect(VKBuffer buffer, ulong offset, uint drawCount, uint stride) => Device.VK10Functions.vkCmdDrawIndexedIndirect(CommandBuffer, buffer, offset, drawCount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispatch(uint groupCountX, uint groupCountY, uint groupCountZ) => Device.VK10Functions.vkCmdDispatch(CommandBuffer, groupCountX, groupCountY, groupCountZ);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispatch(Vector3ui groupCount) => Device.VK10Functions.vkCmdDispatch(CommandBuffer, groupCount.X, groupCount.Y, groupCount.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchIndirect(VKBuffer buffer, ulong offset) => Device.VK10Functions.vkCmdDispatchIndirect(CommandBuffer, buffer, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(VKBuffer srcBuffer, VKBuffer dstBuffer, VKBufferCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyBuffer(CommandBuffer, srcBuffer, dstBuffer, 1, (IntPtr)(&region));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(VKBuffer srcBuffer, VKBuffer dstBuffer, in ReadOnlySpan<VKBufferCopy> regions) {
			unsafe {
				fixed(VKBufferCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBuffer(CommandBuffer, srcBuffer, dstBuffer, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(VKBuffer srcBuffer, VKBuffer dstBuffer, params VKBufferCopy[] regions) {
			unsafe {
				fixed (VKBufferCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBuffer(CommandBuffer, srcBuffer, dstBuffer, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, VKImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, 1, (IntPtr)(&region));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKImageCopy> regions) {
			unsafe {
				fixed (VKImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, params VKImageCopy[] regions) {
			unsafe {
				fixed (VKImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, VKImageBlit region, VKFilter filter) {
			unsafe {
				Device.VK10Functions.vkCmdBlitImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, 1, (IntPtr)(&region), filter);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKImageBlit> regions, VKFilter filter) {
			unsafe {
				fixed (VKImageBlit* pRegions = regions) {
					Device.VK10Functions.vkCmdBlitImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions, filter);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, VKFilter filter, params VKImageBlit[] regions) {
			unsafe {
				fixed (VKImageBlit* pRegions = regions) {
					Device.VK10Functions.vkCmdBlitImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions, filter);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToImage(VKBuffer srcBuffer, VKImage dstImage, VKImageLayout dstLayout, VKBufferImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyBufferToImage(CommandBuffer, srcBuffer, dstImage, dstLayout, 1, (IntPtr)(&region));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToImage(VKBuffer srcBuffer, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKBufferImageCopy> regions) {
			unsafe {
				fixed(VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBufferToImage(CommandBuffer, srcBuffer, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToImage(VKBuffer srcBuffer, VKImage dstImage, VKImageLayout dstLayout, params VKBufferImageCopy[] regions) {
			unsafe {
				fixed (VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBufferToImage(CommandBuffer, srcBuffer, dstImage, dstLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageToBuffer(VKImage srcImage, VKImageLayout srcLayout, VKBuffer dstBuffer, VKBufferImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyImageToBuffer(CommandBuffer, srcImage, srcLayout, dstBuffer, 1, (IntPtr)(&region));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageToBuffer(VKImage srcImage, VKImageLayout srcLayout, VKBuffer dstBuffer, in ReadOnlySpan<VKBufferImageCopy> regions) {
			unsafe {
				fixed (VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImageToBuffer(CommandBuffer, srcImage, srcLayout, dstBuffer, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageToBuffer(VKImage srcImage, VKImageLayout srcLayout, VKBuffer dstBuffer, params VKBufferImageCopy[] regions) {
			unsafe {
				fixed (VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImageToBuffer(CommandBuffer, srcImage, srcLayout, dstBuffer, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer(VKBuffer dstBuffer, ulong dstOffset, ulong dataSize, IntPtr data) => Device.VK10Functions.vkCmdUpdateBuffer(CommandBuffer, dstBuffer, dstOffset, dataSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer<T>(VKBuffer dstBuffer, ulong dstOffset, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Device.VK10Functions.vkCmdUpdateBuffer(CommandBuffer, dstBuffer, dstOffset, (ulong)(sizeof(T) * data.Length), (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer<T>(VKBuffer dstBuffer, ulong dstOffset, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Device.VK10Functions.vkCmdUpdateBuffer(CommandBuffer, dstBuffer, dstOffset, (ulong)(sizeof(T) * data.Length), (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FillBuffer(VKBuffer dstBuffer, ulong dstOffset, ulong size, uint data) => Device.VK10Functions.vkCmdFillBuffer(CommandBuffer, dstBuffer, dstOffset, size, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorImage(VKImage image, VKImageLayout imageLayout, in VKClearColorValue color, VKImageSubresourceRange range) {
			unsafe {
				Device.VK10Functions.vkCmdClearColorImage(CommandBuffer, image, imageLayout, color, 1, (IntPtr)(&range));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorImage(VKImage image, VKImageLayout imageLayout, in VKClearColorValue color, in ReadOnlySpan<VKImageSubresourceRange> range) {
			unsafe {
				fixed(VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearColorImage(CommandBuffer, image, imageLayout, color, (uint)range.Length, (IntPtr)pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorImage(VKImage image, VKImageLayout imageLayout, in VKClearColorValue color, params VKImageSubresourceRange[] range) {
			unsafe {
				fixed (VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearColorImage(CommandBuffer, image, imageLayout, color, (uint)range.Length, (IntPtr)pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilImage(VKImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, VKImageSubresourceRange range) {
			unsafe {
				Device.VK10Functions.vkCmdClearDepthStencilImage(CommandBuffer, image, imageLayout, depthStencil, 1, (IntPtr)(&range));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilImage(VKImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, in ReadOnlySpan<VKImageSubresourceRange> range) {
			unsafe {
				fixed (VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearDepthStencilImage(CommandBuffer, image, imageLayout, depthStencil, (uint)range.Length, (IntPtr)pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilImage(VKImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, params VKImageSubresourceRange[] range) {
			unsafe {
				fixed (VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearDepthStencilImage(CommandBuffer, image, imageLayout, depthStencil, (uint)range.Length, (IntPtr)pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearAttachments(in ReadOnlySpan<VKClearAttachment> attachments, in ReadOnlySpan<VKClearRect> rects) {
			unsafe {
				fixed(VKClearAttachment* pAttachments = attachments) {
					fixed(VKClearRect* pRects = rects) {
						Device.VK10Functions.vkCmdClearAttachments(CommandBuffer, (uint)attachments.Length, (IntPtr)pAttachments, (uint)rects.Length, (IntPtr)pRects);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveImage(VKImage srcImage, VKImageLayout srcImageLayout, VKImage dstImage, VKImageLayout dstImageLayout, VKImageResolve region) {
			unsafe {
				Device.VK10Functions.vkCmdResolveImage(CommandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, 1, (IntPtr)(&region));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveImage(VKImage srcImage, VKImageLayout srcImageLayout, VKImage dstImage, VKImageLayout dstImageLayout, in ReadOnlySpan<VKImageResolve> regions) {
			unsafe {
				fixed (VKImageResolve* pRegions = regions) {
					Device.VK10Functions.vkCmdResolveImage(CommandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveImage(VKImage srcImage, VKImageLayout srcImageLayout, VKImage dstImage, VKImageLayout dstImageLayout, params VKImageResolve[] regions) {
			unsafe {
				fixed (VKImageResolve* pRegions = regions) {
					Device.VK10Functions.vkCmdResolveImage(CommandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, (uint)regions.Length, (IntPtr)pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetEvent(VKEvent _event, VKPipelineStageFlagBits stageMask) => Device.VK10Functions.vkCmdSetEvent(CommandBuffer, _event, stageMask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResetEvent(VKEvent _event, VKPipelineStageFlagBits stageMask) => Device.VK10Functions.vkCmdResetEvent(CommandBuffer, _event, stageMask);

		public void WaitEvents(in ReadOnlySpan<VKEvent> events, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, in ReadOnlySpan<VKMemoryBarrier> memoryBarriers, in ReadOnlySpan<VKBufferMemoryBarrier> bufferMemoryBarriers, in ReadOnlySpan<VKImageMemoryBarrier> imageMemoryBarriers) {
			Span<ulong> evts = stackalloc ulong[events.Length];
			for (int i = 0; i < events.Length; i++) evts[i] = events[i];
			unsafe {
				fixed(ulong* pEvts = evts) {
					fixed(VKMemoryBarrier* pMemBarrier = memoryBarriers) {
						fixed(VKBufferMemoryBarrier* pBufMemBarrier = bufferMemoryBarriers) {
							fixed(VKImageMemoryBarrier* pImgMemBarrier = imageMemoryBarriers) {
								Device.VK10Functions.vkCmdWaitEvents(CommandBuffer, (uint)events.Length, (IntPtr)pEvts, srcStageMask, dstStageMask, (uint)memoryBarriers.Length, (IntPtr)pMemBarrier, (uint)bufferMemoryBarriers.Length, (IntPtr)pBufMemBarrier, (uint)imageMemoryBarriers.Length, (IntPtr)pImgMemBarrier);
							}
 						}
					}
				}
			}
		}

		public void WaitEvents(VKEvent _event, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, in ReadOnlySpan<VKMemoryBarrier> memoryBarriers, in ReadOnlySpan<VKBufferMemoryBarrier> bufferMemoryBarriers, in ReadOnlySpan<VKImageMemoryBarrier> imageMemoryBarriers) {
			Span<ulong> evts = stackalloc ulong[1] { _event.Event };
			unsafe {
				fixed (ulong* pEvts = evts) {
					fixed (VKMemoryBarrier* pMemBarrier = memoryBarriers) {
						fixed (VKBufferMemoryBarrier* pBufMemBarrier = bufferMemoryBarriers) {
							fixed (VKImageMemoryBarrier* pImgMemBarrier = imageMemoryBarriers) {
								Device.VK10Functions.vkCmdWaitEvents(CommandBuffer, 1, (IntPtr)pEvts, srcStageMask, dstStageMask, (uint)memoryBarriers.Length, (IntPtr)pMemBarrier, (uint)bufferMemoryBarriers.Length, (IntPtr)pBufMemBarrier, (uint)imageMemoryBarriers.Length, (IntPtr)pImgMemBarrier);
							}
						}
					}
				}
			}
		}

		public void PipelineBarrier(VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, VKDependencyFlagBits dependencies, in ReadOnlySpan<VKMemoryBarrier> memoryBarriers, in ReadOnlySpan<VKBufferMemoryBarrier> bufferMemoryBarriers, in ReadOnlySpan<VKImageMemoryBarrier> imageMemoryBarriers) {
			unsafe {
				fixed (VKMemoryBarrier* pMemBarrier = memoryBarriers) {
					fixed (VKBufferMemoryBarrier* pBufMemBarrier = bufferMemoryBarriers) {
						fixed (VKImageMemoryBarrier* pImgMemBarrier = imageMemoryBarriers) {
							Device.VK10Functions.vkCmdPipelineBarrier(CommandBuffer, srcStageMask, dstStageMask, dependencies, (uint)memoryBarriers.Length, (IntPtr)pMemBarrier, (uint)bufferMemoryBarriers.Length, (IntPtr)pBufMemBarrier, (uint)imageMemoryBarriers.Length, (IntPtr)pImgMemBarrier);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(VKQueryPool queryPool, uint query, VKQueryControlFlagBits flags) => Device.VK10Functions.vkCmdBeginQuery(CommandBuffer, queryPool, query, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(VKQueryPool queryPool, uint query) => Device.VK10Functions.vkCmdEndQuery(CommandBuffer, queryPool, query);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResetQueryPool(VKQueryPool queryPool, uint firstQuery, uint queryCount) => Device.VK10Functions.vkCmdResetQueryPool(CommandBuffer, queryPool, firstQuery, queryCount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteTimestamp(VKPipelineStageFlagBits pipelineStage, VKQueryPool queryPool, uint query) => Device.VK10Functions.vkCmdWriteTimestamp(CommandBuffer, pipelineStage, queryPool, query);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyQueryPoolResults(VKQueryPool queryPool, uint firstQuery, uint queryCount, VKBuffer dstBuffer, ulong dstOffset, ulong stride, VKQueryResultFlagBits flags) => Device.VK10Functions.vkCmdCopyQueryPoolResults(CommandBuffer, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants(VKPipelineLayout pipelineLayout, VKShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues) => Device.VK10Functions.vkCmdPushConstants(CommandBuffer, pipelineLayout, stageFlags, offset, size, pValues);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants<T>(VKPipelineLayout pipelineLayout, VKShaderStageFlagBits stageFlags, uint offset, in ReadOnlySpan<T> values) where T : unmanaged {
			unsafe {
				fixed(T* pValues = values) {
					Device.VK10Functions.vkCmdPushConstants(CommandBuffer, pipelineLayout, stageFlags, offset, (uint)(values.Length * sizeof(T)), (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants<T>(VKPipelineLayout pipelineLayout, VKShaderStageFlagBits stageFlags, uint offset, params T[] values) where T : unmanaged {
			unsafe {
				fixed (T* pValues = values) {
					Device.VK10Functions.vkCmdPushConstants(CommandBuffer, pipelineLayout, stageFlags, offset, (uint)(values.Length * sizeof(T)), (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginRenderPass(in VKRenderPassBeginInfo beginInfo, VKSubpassContents contents) => Device.VK10Functions.vkCmdBeginRenderPass(CommandBuffer, beginInfo, contents);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NextSubpass(VKSubpassContents contents) => Device.VK10Functions.vkCmdNextSubpass(CommandBuffer, contents);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndRenderPass() => Device.VK10Functions.vkCmdEndRenderPass(CommandBuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(VKCommandBuffer commandBuffer) {
			IntPtr cmdbuf = commandBuffer;
			unsafe {
				Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, 1, (IntPtr)(&cmdbuf));
			}
		}

		public void ExecuteCommands(in ReadOnlySpan<VKCommandBuffer> commandBuffers) {
			Span<IntPtr> cmdbufs = stackalloc IntPtr[commandBuffers.Length];
			for (int i = 0; i < commandBuffers.Length; i++) cmdbufs[i] = commandBuffers[i];
			unsafe {
				fixed (IntPtr* pCmdBufs = cmdbufs) {
					Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, (uint)cmdbufs.Length, (IntPtr)pCmdBufs);
				}
			}
		}

		public void ExecuteCommands(params VKCommandBuffer[] commandBuffers) {
			Span<IntPtr> cmdbufs = stackalloc IntPtr[commandBuffers.Length];
			for (int i = 0; i < commandBuffers.Length; i++) cmdbufs[i] = commandBuffers[i];
			unsafe {
				fixed (IntPtr* pCmdBufs = cmdbufs) {
					Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, (uint)cmdbufs.Length, (IntPtr)pCmdBufs);
				}
			}
		}

		public static implicit operator IntPtr(VKCommandBuffer commandBuffer) => commandBuffer != null ? commandBuffer.CommandBuffer : IntPtr.Zero;

	}

}
