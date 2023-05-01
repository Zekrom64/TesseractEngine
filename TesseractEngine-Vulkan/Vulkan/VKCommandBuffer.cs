using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan {

	public class VKCommandBuffer : IVKDeviceObject, IDisposable, IPrimitiveHandle<IntPtr> {

		public VKObjectType ObjectType => VKObjectType.CommandBuffer;

		public VKCommandPool CommandPool { get; }

		public VKDevice Device => CommandPool.Device;
		
		[NativeType("VkCommandBuffer")]
		public IntPtr CommandBuffer { get; }

		public IntPtr PrimitiveHandle => CommandBuffer;

		ulong IPrimitiveHandle<ulong>.PrimitiveHandle => (ulong)PrimitiveHandle;

		public VKCommandBuffer(VKCommandPool commandPool, IntPtr commandBuffer) {
			CommandPool = commandPool;
			CommandBuffer = commandBuffer;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			IntPtr cmdbuf = CommandBuffer;
			unsafe {
				Device.VK10Functions.vkFreeCommandBuffers(Device, CommandPool, 1, &cmdbuf);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Begin(in VKCommandBufferBeginInfo beginInfo) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkBeginCommandBuffer(CommandBuffer, beginInfo), "Failed to begin command buffer recording");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void End() {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkEndCommandBuffer(CommandBuffer), "Failed to end command buffer recording");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset(VKCommandBufferResetFlagBits flags = 0) {
			unsafe {
				VK.CheckError(Device.VK10Functions.vkResetCommandBuffer(CommandBuffer, flags), "Failed to reset command buffer");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindPipeline(VKPipelineBindPoint bindPoint, VKPipeline pipeline) {
			unsafe {
				Device.VK10Functions.vkCmdBindPipeline(CommandBuffer, bindPoint, pipeline);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(VKViewport viewport, uint offset = 0) {
			unsafe {
				Device.VK10Functions.vkCmdSetViewport(CommandBuffer, offset, 1, &viewport);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewport(in ReadOnlySpan<VKViewport> viewports, uint offset = 0) {
			unsafe {
				fixed (VKViewport* pViewports = viewports) {
					Device.VK10Functions.vkCmdSetViewport(CommandBuffer, offset, (uint)viewports.Length, pViewports);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(VKRect2D scissor, uint offset = 0) {
			unsafe {
				Device.VK10Functions.vkCmdSetScissor(CommandBuffer, offset, 1, &scissor);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissor(in ReadOnlySpan<VKRect2D> scissors, uint offset = 0) {
			unsafe {
				fixed(VKRect2D* pScissors = scissors) {
					Device.VK10Functions.vkCmdSetScissor(CommandBuffer, offset, (uint)scissors.Length, pScissors);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetLineWidth(float width) {
			unsafe {
				Device.VK10Functions.vkCmdSetLineWidth(CommandBuffer, width);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) {
			unsafe {
				Device.VK10Functions.vkCmdSetDepthBias(CommandBuffer, constFactor, clamp, slopeFactor);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBlendConstants(Vector4 blendConstants) {
			unsafe {
				Device.VK10Functions.vkCmdSetBlendConstants(CommandBuffer, blendConstants);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthBounds(float minDepth, float maxDepth) {
			unsafe {
				Device.VK10Functions.vkCmdSetDepthBounds(CommandBuffer, minDepth, maxDepth);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilCompareMask(VKStencilFaceFlagBits face, uint mask) {
			unsafe {
				Device.VK10Functions.vkCmdSetStencilCompareMask(CommandBuffer, face, mask);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilWriteMask(VKStencilFaceFlagBits face, uint mask) {
			unsafe {
				Device.VK10Functions.vkCmdSetStencilWriteMask(CommandBuffer, face, mask);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilReference(VKStencilFaceFlagBits face, uint reference) {
			unsafe {
				Device.VK10Functions.vkCmdSetStencilReference(CommandBuffer, face, reference);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, VKDescriptorSet descriptorSet) {
			ulong descSet = descriptorSet;
			unsafe {
				Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, 1, &descSet, 0, (uint*)0);
			}
		}

		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, IReadOnlyList<VKDescriptorSet> descriptorSets, in ReadOnlySpan<uint> dynamicOffsets) {
			Span<ulong> descSets = stackalloc ulong[descriptorSets.Count];
			for (int i = 0; i < descriptorSets.Count; i++) descSets[i] = descriptorSets[i];
			unsafe {
				fixed (ulong* pDescSets = descSets) {
					fixed (uint* pDynamicOffsets = dynamicOffsets) {
						Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, (uint)descSets.Length, pDescSets, (uint)dynamicOffsets.Length, pDynamicOffsets);
					}
				}
			}
		}

		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, params VKDescriptorSet[] descriptorSets) {
			Span<ulong> descSets = stackalloc ulong[descriptorSets.Length];
			for (int i = 0; i < descriptorSets.Length; i++) descSets[i] = descriptorSets[i];
			unsafe {
				fixed (ulong* pDescSets = descSets) {
					Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, (uint)descSets.Length, pDescSets, 0, (uint*)0);
				}
			}
		}

		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, uint firstSet, in ReadOnlySpan<ulong> descriptorSets) {
			unsafe {
				fixed (ulong* pDescSets = descriptorSets) {
					Device.VK10Functions.vkCmdBindDescriptorSets(CommandBuffer, bindPoint, layout, firstSet, (uint)descriptorSets.Length, pDescSets, 0, (uint*)0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindDescriptorSets(VKPipelineBindPoint bindPoint, VKPipelineLayout layout, params VKDescriptorSet[] descriptorSets) => BindDescriptorSets(bindPoint, layout, 0, descriptorSets);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindIndexBuffer(VKBuffer buffer, ulong offset, VKIndexType indexType) {
			unsafe {
				Device.VK10Functions.vkCmdBindIndexBuffer(CommandBuffer, buffer, offset, indexType);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(VKBuffer buffer, ulong offset = 0) => BindVertexBuffers(0, buffer, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(uint firstBinding, VKBuffer buffer, ulong offset = 0) {
			ulong buf = buffer;
			unsafe {
				Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, 1, &buf, &offset);
			}
		}

		public void BindVertexBuffers(uint firstBinding, IReadOnlyList<VKBuffer> buffers, in ReadOnlySpan<ulong> offsets) {
			Span<ulong> bufs = stackalloc ulong[buffers.Count];
			for (int i = 0; i < buffers.Count; i++) bufs[i] = buffers[i];
			unsafe {
				fixed (ulong* pBufs = bufs, pOffsets = offsets) {
					Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, (uint)Math.Min(bufs.Length, offsets.Length), pBufs, pOffsets);
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
					Device.VK10Functions.vkCmdBindVertexBuffers(CommandBuffer, firstBinding, (uint)Math.Min(bufs.Length, offsets.Length), pBufs, pOffsets);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Draw(uint vertexCount, uint instanceCount = 1, uint firstVertex = 0, uint firstInstance = 0) {
			unsafe {
				Device.VK10Functions.vkCmdDraw(CommandBuffer, vertexCount, instanceCount, firstVertex, firstInstance);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndexed(uint indexCount, uint instanceCount = 1, uint firstIndex = 0, int vertexOffset = 0, uint firstInstance = 0) {
			unsafe {
				Device.VK10Functions.vkCmdDrawIndexed(CommandBuffer, indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndirect(VKBuffer buffer, ulong offset, uint drawCount, uint stride) {
			unsafe {
				Device.VK10Functions.vkCmdDrawIndirect(CommandBuffer, buffer, offset, drawCount, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawIndexedIndirect(VKBuffer buffer, ulong offset, uint drawCount, uint stride) {
			unsafe {
				Device.VK10Functions.vkCmdDrawIndexedIndirect(CommandBuffer, buffer, offset, drawCount, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispatch(uint groupCountX, uint groupCountY, uint groupCountZ) {
			unsafe {
				Device.VK10Functions.vkCmdDispatch(CommandBuffer, groupCountX, groupCountY, groupCountZ);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispatch(Vector3ui groupCount) {
			unsafe {
				Device.VK10Functions.vkCmdDispatch(CommandBuffer, groupCount.X, groupCount.Y, groupCount.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchIndirect(VKBuffer buffer, ulong offset) {
			unsafe {
				Device.VK10Functions.vkCmdDispatchIndirect(CommandBuffer, buffer, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(VKBuffer srcBuffer, VKBuffer dstBuffer, VKBufferCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyBuffer(CommandBuffer, srcBuffer, dstBuffer, 1, &region);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBuffer(VKBuffer srcBuffer, VKBuffer dstBuffer, in ReadOnlySpan<VKBufferCopy> regions) {
			unsafe {
				fixed(VKBufferCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBuffer(CommandBuffer, srcBuffer, dstBuffer, (uint)regions.Length, pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, VKImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, 1, &region);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKImageCopy> regions) {
			unsafe {
				fixed (VKImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, VKImageBlit region, VKFilter filter) {
			unsafe {
				Device.VK10Functions.vkCmdBlitImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, 1, &region, filter);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitImage(VKImage srcImage, VKImageLayout srcLayout, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKImageBlit> regions, VKFilter filter) {
			unsafe {
				fixed (VKImageBlit* pRegions = regions) {
					Device.VK10Functions.vkCmdBlitImage(CommandBuffer, srcImage, srcLayout, dstImage, dstLayout, (uint)regions.Length, pRegions, filter);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToImage(VKBuffer srcBuffer, VKImage dstImage, VKImageLayout dstLayout, VKBufferImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyBufferToImage(CommandBuffer, srcBuffer, dstImage, dstLayout, 1, &region);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferToImage(VKBuffer srcBuffer, VKImage dstImage, VKImageLayout dstLayout, in ReadOnlySpan<VKBufferImageCopy> regions) {
			unsafe {
				fixed(VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyBufferToImage(CommandBuffer, srcBuffer, dstImage, dstLayout, (uint)regions.Length, pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageToBuffer(VKImage srcImage, VKImageLayout srcLayout, VKBuffer dstBuffer, VKBufferImageCopy region) {
			unsafe {
				Device.VK10Functions.vkCmdCopyImageToBuffer(CommandBuffer, srcImage, srcLayout, dstBuffer, 1, &region);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageToBuffer(VKImage srcImage, VKImageLayout srcLayout, VKBuffer dstBuffer, in ReadOnlySpan<VKBufferImageCopy> regions) {
			unsafe {
				fixed (VKBufferImageCopy* pRegions = regions) {
					Device.VK10Functions.vkCmdCopyImageToBuffer(CommandBuffer, srcImage, srcLayout, dstBuffer, (uint)regions.Length, pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBuffer(VKBuffer dstBuffer, ulong dstOffset, ulong dataSize, IntPtr data) {
			unsafe {
				Device.VK10Functions.vkCmdUpdateBuffer(CommandBuffer, dstBuffer, dstOffset, dataSize, data);
			}
		}

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
		public void FillBuffer(VKBuffer dstBuffer, ulong dstOffset, ulong size, uint data) {
			unsafe {
				Device.VK10Functions.vkCmdFillBuffer(CommandBuffer, dstBuffer, dstOffset, size, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorImage(VKImage image, VKImageLayout imageLayout, in VKClearColorValue color, VKImageSubresourceRange range) {
			unsafe {
				Device.VK10Functions.vkCmdClearColorImage(CommandBuffer, image, imageLayout, color, 1, &range);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColorImage(VKImage image, VKImageLayout imageLayout, in VKClearColorValue color, in ReadOnlySpan<VKImageSubresourceRange> range) {
			unsafe {
				fixed(VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearColorImage(CommandBuffer, image, imageLayout, color, (uint)range.Length, pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilImage(VKImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, VKImageSubresourceRange range) {
			unsafe {
				Device.VK10Functions.vkCmdClearDepthStencilImage(CommandBuffer, image, imageLayout, depthStencil, 1, &range);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepthStencilImage(VKImage image, VKImageLayout imageLayout, in VKClearDepthStencilValue depthStencil, in ReadOnlySpan<VKImageSubresourceRange> range) {
			unsafe {
				fixed (VKImageSubresourceRange* pRange = range) {
					Device.VK10Functions.vkCmdClearDepthStencilImage(CommandBuffer, image, imageLayout, depthStencil, (uint)range.Length, pRange);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearAttachments(in ReadOnlySpan<VKClearAttachment> attachments, in ReadOnlySpan<VKClearRect> rects) {
			unsafe {
				fixed(VKClearAttachment* pAttachments = attachments) {
					fixed(VKClearRect* pRects = rects) {
						Device.VK10Functions.vkCmdClearAttachments(CommandBuffer, (uint)attachments.Length, pAttachments, (uint)rects.Length, pRects);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveImage(VKImage srcImage, VKImageLayout srcImageLayout, VKImage dstImage, VKImageLayout dstImageLayout, VKImageResolve region) {
			unsafe {
				Device.VK10Functions.vkCmdResolveImage(CommandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, 1, &region);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResolveImage(VKImage srcImage, VKImageLayout srcImageLayout, VKImage dstImage, VKImageLayout dstImageLayout, in ReadOnlySpan<VKImageResolve> regions) {
			unsafe {
				fixed (VKImageResolve* pRegions = regions) {
					Device.VK10Functions.vkCmdResolveImage(CommandBuffer, srcImage, srcImageLayout, dstImage, dstImageLayout, (uint)regions.Length, pRegions);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetEvent(VKEvent _event, VKPipelineStageFlagBits stageMask) {
			unsafe {
				Device.VK10Functions.vkCmdSetEvent(CommandBuffer, _event, stageMask);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResetEvent(VKEvent _event, VKPipelineStageFlagBits stageMask) {
			unsafe {
				Device.VK10Functions.vkCmdResetEvent(CommandBuffer, _event, stageMask);
			}
		}

		public void WaitEvents(IReadOnlyList<VKEvent> events, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, in ReadOnlySpan<VKMemoryBarrier> memoryBarriers, in ReadOnlySpan<VKBufferMemoryBarrier> bufferMemoryBarriers, in ReadOnlySpan<VKImageMemoryBarrier> imageMemoryBarriers) {
			Span<ulong> evts = stackalloc ulong[events.Count];
			for (int i = 0; i < events.Count; i++) evts[i] = events[i];
			unsafe {
				fixed(ulong* pEvts = evts) {
					fixed(VKMemoryBarrier* pMemBarrier = memoryBarriers) {
						fixed(VKBufferMemoryBarrier* pBufMemBarrier = bufferMemoryBarriers) {
							fixed(VKImageMemoryBarrier* pImgMemBarrier = imageMemoryBarriers) {
								Device.VK10Functions.vkCmdWaitEvents(CommandBuffer, (uint)events.Count, pEvts, srcStageMask, dstStageMask, (uint)memoryBarriers.Length, pMemBarrier, (uint)bufferMemoryBarriers.Length, pBufMemBarrier, (uint)imageMemoryBarriers.Length, pImgMemBarrier);
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
								Device.VK10Functions.vkCmdWaitEvents(CommandBuffer, 1, pEvts, srcStageMask, dstStageMask, (uint)memoryBarriers.Length, pMemBarrier, (uint)bufferMemoryBarriers.Length, pBufMemBarrier, (uint)imageMemoryBarriers.Length, pImgMemBarrier);
							}
						}
					}
				}
			}
		}

		public void WaitEvents(in ReadOnlySpan<ulong> events, VKPipelineStageFlagBits srcStageMask, VKPipelineStageFlagBits dstStageMask, in ReadOnlySpan<VKMemoryBarrier> memoryBarriers, in ReadOnlySpan<VKBufferMemoryBarrier> bufferMemoryBarriers, in ReadOnlySpan<VKImageMemoryBarrier> imageMemoryBarriers) {
			unsafe {
				fixed (ulong* pEvts = events) {
					fixed (VKMemoryBarrier* pMemBarrier = memoryBarriers) {
						fixed (VKBufferMemoryBarrier* pBufMemBarrier = bufferMemoryBarriers) {
							fixed (VKImageMemoryBarrier* pImgMemBarrier = imageMemoryBarriers) {
								Device.VK10Functions.vkCmdWaitEvents(CommandBuffer, (uint)events.Length, pEvts, srcStageMask, dstStageMask, (uint)memoryBarriers.Length, pMemBarrier, (uint)bufferMemoryBarriers.Length, pBufMemBarrier, (uint)imageMemoryBarriers.Length, pImgMemBarrier);
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
							Device.VK10Functions.vkCmdPipelineBarrier(CommandBuffer, srcStageMask, dstStageMask, dependencies, (uint)memoryBarriers.Length, pMemBarrier, (uint)bufferMemoryBarriers.Length, pBufMemBarrier, (uint)imageMemoryBarriers.Length, pImgMemBarrier);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQuery(VKQueryPool queryPool, uint query, VKQueryControlFlagBits flags) {
			unsafe {
				Device.VK10Functions.vkCmdBeginQuery(CommandBuffer, queryPool, query, flags);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQuery(VKQueryPool queryPool, uint query) {
			unsafe {
				Device.VK10Functions.vkCmdEndQuery(CommandBuffer, queryPool, query);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResetQueryPool(VKQueryPool queryPool, uint firstQuery, uint queryCount) {
			unsafe {
				Device.VK10Functions.vkCmdResetQueryPool(CommandBuffer, queryPool, firstQuery, queryCount);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteTimestamp(VKPipelineStageFlagBits pipelineStage, VKQueryPool queryPool, uint query) {
			unsafe {
				Device.VK10Functions.vkCmdWriteTimestamp(CommandBuffer, pipelineStage, queryPool, query);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyQueryPoolResults(VKQueryPool queryPool, uint firstQuery, uint queryCount, VKBuffer dstBuffer, ulong dstOffset, ulong stride, VKQueryResultFlagBits flags) {
			unsafe {
				Device.VK10Functions.vkCmdCopyQueryPoolResults(CommandBuffer, queryPool, firstQuery, queryCount, dstBuffer, dstOffset, stride, flags);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushConstants(VKPipelineLayout pipelineLayout, VKShaderStageFlagBits stageFlags, uint offset, uint size, IntPtr pValues) {
			unsafe {
				Device.VK10Functions.vkCmdPushConstants(CommandBuffer, pipelineLayout, stageFlags, offset, size, pValues);
			}
		}

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
		public void BeginRenderPass(in VKRenderPassBeginInfo beginInfo, VKSubpassContents contents) {
			unsafe {
				Device.VK10Functions.vkCmdBeginRenderPass(CommandBuffer, beginInfo, contents);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NextSubpass(VKSubpassContents contents) {
			unsafe {
				Device.VK10Functions.vkCmdNextSubpass(CommandBuffer, contents);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndRenderPass() {
			unsafe {
				Device.VK10Functions.vkCmdEndRenderPass(CommandBuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(VKCommandBuffer commandBuffer) {
			IntPtr cmdbuf = commandBuffer;
			unsafe {
				Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, 1, &cmdbuf);
			}
		}

		public void ExecuteCommands(IReadOnlyList<VKCommandBuffer> commandBuffers) {
			Span<IntPtr> cmdbufs = stackalloc IntPtr[commandBuffers.Count];
			for (int i = 0; i < commandBuffers.Count; i++) cmdbufs[i] = commandBuffers[i];
			unsafe {
				fixed (IntPtr* pCmdBufs = cmdbufs) {
					Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, (uint)cmdbufs.Length, pCmdBufs);
				}
			}
		}

		public void ExecuteCommands(params VKCommandBuffer[] commandBuffers) {
			Span<IntPtr> cmdbufs = stackalloc IntPtr[commandBuffers.Length];
			for (int i = 0; i < commandBuffers.Length; i++) cmdbufs[i] = commandBuffers[i];
			unsafe {
				fixed (IntPtr* pCmdBufs = cmdbufs) {
					Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, (uint)cmdbufs.Length, pCmdBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ExecuteCommands(in ReadOnlySpan<IntPtr> commandBuffers) {
			unsafe {
				fixed (IntPtr* pCmdBufs = commandBuffers) {
					Device.VK10Functions.vkCmdExecuteCommands(CommandBuffer, (uint)commandBuffers.Length, pCmdBufs);
				}
			}
		}

		// Vulkan 1.1
		// VK_KHR_device_group

		public void DispatchBase(Vector3ui bases, Vector3ui count) {
			unsafe {
				if (Device.VK11Functions) Device.VK11Functions!.vkCmdDispatchBase(CommandBuffer, bases.X, bases.Y, bases.Z, count.X, count.Y, count.Z);
				else Device.KHRDeviceGroup!.vkCmdDispatchBaseKHR(CommandBuffer, bases.X, bases.Y, bases.Z, count.X, count.Y, count.Z);
			}
		}

		public void SetDeviceMask(uint deviceMask) {
			unsafe {
				if (Device.VK11Functions) Device.VK11Functions!.vkCmdSetDeviceMask(CommandBuffer, deviceMask);
				else Device.KHRDeviceGroup!.vkCmdSetDeviceMaskKHR(CommandBuffer, deviceMask);
			}
		}

		// EXT_line_rasterization

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetLineStippleEXT(uint lineStippleFactor, ushort lineStipplePattern) {
			unsafe {
				Device.EXTLineRasterization!.vkCmdSetLineStippleEXT(CommandBuffer, lineStippleFactor, lineStipplePattern);
			}
		}

		// EXT_extended_dynamic_state
		// Vulkan 1.3

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetCullModeEXT(VKCullModeFlagBits cullMode) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetCullModeEXT(CommandBuffer, cullMode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFrontFaceEXT(VKFrontFace frontFace) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetFrontFaceEXT(CommandBuffer, frontFace);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetPrimitiveTopologyEXT(VKPrimitiveTopology primitiveTopology) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetPrimitiveTopologyEXT(CommandBuffer, primitiveTopology);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetViewportWithCountEXT(in ReadOnlySpan<VKViewport> viewports) {
			unsafe {
				fixed(VKViewport* pViewports = viewports) {
					Device.EXTExtendedDynamicState!.vkCmdSetViewportWithCountEXT(CommandBuffer, (uint)viewports.Length, pViewports);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScissorWithCountEXT(in ReadOnlySpan<VKRect2D> scissors) {
			unsafe {
				fixed (VKRect2D* pScissors = scissors) {
					Device.EXTExtendedDynamicState!.vkCmdSetScissorWithCountEXT(CommandBuffer, (uint)scissors.Length, pScissors);
				}
			}
		}

		public void BindVertexBuffers2EXT(uint firstBinding, IReadOnlyList<VKBuffer> buffers, in ReadOnlySpan<ulong> offsets, in ReadOnlySpan<ulong> sizes, in ReadOnlySpan<ulong> strides) {
			uint n = (uint)ExMath.Min(buffers.Count, offsets.Length, sizes.Length, strides.Length);
			Span<ulong> bufs = stackalloc ulong[(int)n];
			for (int i = 0; i < n; i++) bufs[i] = buffers[i];
			unsafe {
				fixed(ulong* pBuffers = bufs, pOffsets = offsets, pSizes = sizes, pStrides = strides) {
					Device.EXTExtendedDynamicState!.vkCmdBindVertexBuffers2EXT(CommandBuffer, firstBinding, n, pBuffers, pOffsets, pSizes, pStrides);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthTestEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetDepthTestEnableEXT(CommandBuffer, enable);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthWriteEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetDepthWriteEnableEXT(CommandBuffer, enable);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthCompareOpEXT(VKCompareOp compareOp) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetDepthCompareOpEXT(CommandBuffer, compareOp);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDepthBoundsTestEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetDepthBoundsTestEnableEXT(CommandBuffer, enable);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilTestEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetStencilTestEnableEXT(CommandBuffer, enable);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetStencilOpEXT(VKStencilFaceFlagBits faceMask, VKStencilOp failOp, VKStencilOp passOp, VKStencilOp depthFailOp, VKCompareOp compareOp) {
			unsafe {
				Device.EXTExtendedDynamicState!.vkCmdSetStencilOpEXT(CommandBuffer, faceMask, failOp, passOp, depthFailOp, compareOp);
			}
		}

		// VK_KHR_create_renderpass2
		// Vulkan 1.2
		public void BeginRenderPass2(in VKRenderPassBeginInfo renderPassBegin, in VKSubpassBeginInfo subpassBegin) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkCmdBeginRenderPass2(CommandBuffer, renderPassBegin, subpassBegin);
				else Device.KHRCreateRenderpass2!.vkCmdBeginRenderPass2KHR(CommandBuffer, renderPassBegin, subpassBegin);
			}
		}

		public void NextSubpass2(in VKSubpassBeginInfo beginInfo, in VKSubpassEndInfo endInfo) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkCmdNextSubpass2(CommandBuffer, beginInfo, endInfo);
				else Device.KHRCreateRenderpass2!.vkCmdNextSubpass2KHR(CommandBuffer, beginInfo, endInfo);
			}
		}

		public void EndRenderPass2(in VKSubpassEndInfo endInfo) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkCmdEndRenderPass2(CommandBuffer, endInfo);
				else Device.KHRCreateRenderpass2!.vkCmdEndRenderPass2KHR(CommandBuffer, endInfo);
			}
		}
		
		// VK_KHR_draw_indirect_count
		// Vulkan 1.2

		public void DrawIndexedIndirectCount(VKBuffer buffer, ulong offset, VKBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkCmdDrawIndexedIndirectCount(CommandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
				else Device.KHRDrawIndirectCount!.vkCmdDrawIndexedIndirectCountKHR(CommandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
			}
		}

		public void DrawIndirectCount(VKBuffer buffer, ulong offset, VKBuffer countBuffer, ulong countBufferOffset, uint maxDrawCount, uint stride) {
			unsafe {
				if (Device.VK12Functions) Device.VK12Functions!.vkCmdDrawIndirectCount(CommandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
				else Device.KHRDrawIndirectCount!.vkCmdDrawIndirectCountKHR(CommandBuffer, buffer, offset, countBuffer, countBufferOffset, maxDrawCount, stride);
			}
		}

		// VK_EXT_debug_utils

		public void BeginDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) {
			unsafe {
				Device.Instance.EXTDebugUtilsFunctions!.vkCmdBeginDebugUtilsLabelEXT(CommandBuffer, labelInfo);
			}
		}

		public void EndDebugUtilsLabelEXT() {
			unsafe {
				Device.Instance.EXTDebugUtilsFunctions!.vkCmdEndDebugUtilsLabelEXT(CommandBuffer);
			}
		}

		public void InsertDebugUtilsLabelEXT(in VKDebugUtilsLabelEXT labelInfo) {
			unsafe {
				Device.Instance.EXTDebugUtilsFunctions!.vkCmdInsertDebugUtilsLabelEXT(CommandBuffer, labelInfo);
			}
		}

		// VK_KHR_dynamic_rendering
		// Vulkan 1.3

		public void BeginRendering(in VKRenderingInfo renderingInfo) {
			unsafe {
				Device.KHRDynamicRendering!.vkCmdBeginRenderingKHR(CommandBuffer, renderingInfo);
			}
		}

		public void EndRendering() {
			unsafe {
				Device.KHRDynamicRendering!.vkCmdEndRenderingKHR(CommandBuffer);
			}
		}

		// VK_EXT_extended_dynamic_state2
		// Vulkan 1.3

		public void SetDepthBiasEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState2!.vkCmdSetDepthBiasEnableEXT(CommandBuffer, enable);
			}
		}

		public void SetLogicOpEXT(VKLogicOp op) {
			unsafe {
				Device.EXTExtendedDynamicState2!.vkCmdSetLogicOpEXT(CommandBuffer, op);
			}
		}

		public void SetPatchControlPointsEXT(uint patchControlPoints) {
			unsafe {
				Device.EXTExtendedDynamicState2!.vkCmdSetPatchControlPointsEXT(CommandBuffer, patchControlPoints);
			}
		}

		public void SetPrimitiveRestartEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState2!.vkCmdSetPrimitiveRestartEnableEXT(CommandBuffer, enable);
			}
		}

		public void SetRasterizerDiscardEnableEXT(bool enable) {
			unsafe {
				Device.EXTExtendedDynamicState2!.vkCmdSetRasterizerDiscardEnableEXT(CommandBuffer, enable);
			}
		}

		// VK_EXT_vertex_input_dynamic_state

		public void SetVertexInputEXT(in ReadOnlySpan<VKVertexInputBindingDescription2EXT> bindings, in ReadOnlySpan<VKVertexInputAttributeDescription2EXT> attributes) {
			unsafe {
				fixed(VKVertexInputBindingDescription2EXT* pBindings = bindings) {
					fixed(VKVertexInputAttributeDescription2EXT* pAttributes = attributes) {
						Device.EXTVertexInputDynamicState!.vkCmdSetVertexInputEXT(CommandBuffer, (uint)bindings.Length, pBindings, (uint)attributes.Length, pAttributes);
					}
				}
			}
		}

		// VK_EXT_color_write_enable

		public void SetColorWriteEnableEXT(in ReadOnlySpan<VKBool32> colorWriteEnables) {
			unsafe {
				fixed(VKBool32* pColorWriteEnables = colorWriteEnables) {
					Device.EXTColorWriteEnable!.vkCmdSetColorWriteEnableEXT(CommandBuffer, (uint)colorWriteEnables.Length, (IntPtr)pColorWriteEnables);
				}
			}
		}

		// VK_KHR_copy_commands2
		// Vulkan 1.3

		public void BlitImage2(in VKBlitImageInfo2 blitImageInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdBlitImage2KHR(CommandBuffer, blitImageInfo);
			}
		}

		public void CopyBuffer2(in VKCopyBufferInfo2 copyBufferInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdCopyBuffer2KHR(CommandBuffer, copyBufferInfo);
			}
		}

		public void CopyBufferToImage2(in VKCopyBufferToImageInfo2 copyBufferToImageInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdCopyBufferToImage2KHR(CommandBuffer, copyBufferToImageInfo);
			}
		}

		public void CopyImage2(in VKCopyImageInfo2 copyImageInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdCopyImage2KHR(CommandBuffer, copyImageInfo);
			}
		}

		public void CopyImageToBuffer2(in VKCopyImageToBufferInfo2 copyImageToBufferInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdCopyImageToBuffer2KHR(CommandBuffer, copyImageToBufferInfo);
			}
		}

		public void ResolveImage2(in VKResolveImageInfo2 resolveImageInfo) {
			unsafe {
				Device.KHRCopyCommands2!.vkCmdResolveImage2(CommandBuffer, resolveImageInfo);
			}
		}

		// VK_KHR_synchronization2
		// Vulkan 1.3

		public void PipelineBarrier2(in VKDependencyInfo dependencyInfo) {
			unsafe {
				Device.KHRSynchronization2!.vkCmdPipelineBarrier2(CommandBuffer, dependencyInfo);
			}
		}

		public void ResetEvent2(VKEvent _event, VKPipelineStageFlagBits2 stageMask) {
			unsafe {
				Device.KHRSynchronization2!.vkCmdResetEvent2(CommandBuffer, _event, stageMask);
			}
		}

		public void SetEvent2(VKEvent _event, in VKDependencyInfo dependencyInfo) {
			unsafe {
				Device.KHRSynchronization2!.vkCmdSetEvent2(CommandBuffer, _event, dependencyInfo);
			}
		}

		public void WaitEvents2(VKEvent _event, VKDependencyInfo dependencyInfo) {
			ulong uevent = _event;
			unsafe {
				Device.KHRSynchronization2!.vkCmdWaitEvents2(CommandBuffer, 1, &uevent, &dependencyInfo);
			}
		}

		public void WaitEvents2(in ReadOnlySpan<ulong> events, in ReadOnlySpan<VKDependencyInfo> dependencyInfos) {
			unsafe {
				fixed (ulong* pEvents = events) {
					fixed (VKDependencyInfo* pDependencyInfos = dependencyInfos) {
						Device.KHRSynchronization2!.vkCmdWaitEvents2(CommandBuffer, (uint)Math.Min(events.Length, dependencyInfos.Length), pEvents, pDependencyInfos);
					}
				}
			}
		}

		public void WaitEvents2(IReadOnlyList<VKEvent> events, IReadOnlyList<VKDependencyInfo> dependencyInfos) {
			int count = Math.Min(events.Count, dependencyInfos.Count);
			Span<ulong> sEvents = stackalloc ulong[count];
			Span<VKDependencyInfo> sDependencyInfos = stackalloc VKDependencyInfo[count];
			for(int i = 0; i < count; i++) {
				sEvents[i] = events[i];
				sDependencyInfos[i] = dependencyInfos[i];
			}
			WaitEvents2(sEvents, sDependencyInfos);
		}

		public void WaitEvents2(IReadOnlyCollection<(VKEvent, VKDependencyInfo)> eventsAndDependencies) {
			int count = eventsAndDependencies.Count;
			Span<ulong> sEvents = stackalloc ulong[count];
			Span<VKDependencyInfo> sDependencyInfos = stackalloc VKDependencyInfo[count];
			int i = 0;
			foreach(var x in eventsAndDependencies) {
				sEvents[i] = x.Item1;
				sDependencyInfos[i] = x.Item2;
				i++;
			}
			WaitEvents2(sEvents, sDependencyInfos);
		}

		public void WriteTimestamp2(VKPipelineStageFlagBits2 stage, VKQueryPool queryPool, uint query) {
			unsafe {
				Device.KHRSynchronization2!.vkCmdWriteTimestamp2(CommandBuffer, stage, queryPool, query);
			}
		}

		// VK_KHR_acceleration_structure

		public void BuildAccelerationStructuresIndirect(in ReadOnlySpan<VKAccelerationStructureBuildGeometryInfoKHR> infos, in ReadOnlySpan<ulong> indirectDeviceAddresses, in ReadOnlySpan<uint> indirectStrides, IReadOnlyList<IConstPointer<uint>> maxPrimitiveCounts) {
			Span<IntPtr> pMaxPrimitiveCounts = stackalloc IntPtr[maxPrimitiveCounts.Count];
			for (int i = 0; i < pMaxPrimitiveCounts.Length; i++) pMaxPrimitiveCounts[i] = maxPrimitiveCounts[i].Ptr;
			unsafe {
				fixed(VKAccelerationStructureBuildGeometryInfoKHR* pInfos = infos) {
					fixed(ulong* pIndirectDeviceAddresses = indirectDeviceAddresses) {
						fixed(uint* pIndirectStrides = indirectStrides) {
							fixed(IntPtr* ppMaxPrimitiveCounts = pMaxPrimitiveCounts) {
								Device.KHRAccelerationStructure!.vkCmdBuildAccelerationStructuresIndirectKHR(CommandBuffer, (uint)infos.Length, pInfos, pIndirectDeviceAddresses, pIndirectStrides, (uint**)ppMaxPrimitiveCounts);
							}
						}
					}
				}
			}
		}

		public void BuildAccelerationStructuresKHR(in ReadOnlySpan<VKAccelerationStructureBuildGeometryInfoKHR> infos, IReadOnlyList<IConstPointer<VkAccelerationStructureBuildRangeInfoKHR>> buildRangeInfos) {
			Span<IntPtr> pBuildRangeInfos = stackalloc IntPtr[buildRangeInfos.Count];
			for(int i = 0; i < pBuildRangeInfos.Length; i++) pBuildRangeInfos[i] = buildRangeInfos[i].Ptr;
			unsafe {
				fixed(VKAccelerationStructureBuildGeometryInfoKHR* pInfos = infos) {
					fixed(IntPtr* ppBuildRangeInfos = pBuildRangeInfos) {
						Device.KHRAccelerationStructure!.vkCmdBuildAccelerationStructuresKHR(CommandBuffer, (uint)infos.Length, pInfos, (VkAccelerationStructureBuildRangeInfoKHR**)ppBuildRangeInfos);
					}
				}
			}
		}

		public void CopyAccelerationStructure(in VKCopyAccelerationStructureInfoKHR info) {
			unsafe {
				Device.KHRAccelerationStructure!.vkCmdCopyAccelerationStructureKHR(CommandBuffer, info);
			}
		}

		public void CopyAccelerationStructureToMemory(in VKCopyAccelerationStructureToMemoryInfoKHR info) {
			unsafe {
				Device.KHRAccelerationStructure!.vkCmdCopyAccelerationStructureToMemoryKHR(CommandBuffer, info);
			}
		}

		public void CopyMemoryToAccelerationStructure(in VKCopyMemoryToAccelerationStructureInfoKHR info) {
			unsafe {
				Device.KHRAccelerationStructure!.vkCmdCopyMemoryToAccelerationStructureKHR(CommandBuffer, info);
			}
		}

		public void WriteAccelerationStructuresProperties(VKAccelerationStructureKHR accelerationStructure, VKQueryType queryType, VKQueryPool queryPool, uint firstQuery) {
			ulong accelStruct = accelerationStructure.AccelerationStructure;
			unsafe {
				Device.KHRAccelerationStructure!.vkCmdWriteAccelerationStructuresPropertiesKHR(CommandBuffer, 1, &accelStruct, queryType, queryPool.QueryPool, firstQuery);
			}
		}

		public void WriteAccelerationStructuresProperties(IReadOnlyList<VKAccelerationStructureKHR> accelerationStructures, VKQueryType queryType, VKQueryPool queryPool, uint firstQuery) {
			Span<ulong> accelStructs = stackalloc ulong[accelerationStructures.Count];
			for (int i = 0; i < accelStructs.Length; i++) accelStructs[i] = accelerationStructures[i].AccelerationStructure;
			unsafe {
				fixed(ulong* pAccelerationStructures = accelStructs) {
					Device.KHRAccelerationStructure!.vkCmdWriteAccelerationStructuresPropertiesKHR(CommandBuffer, (uint)accelStructs.Length, pAccelerationStructures, queryType, queryPool.QueryPool, firstQuery);
				}
			}
		}

		// VK_KHR_ray_tracing_pipeline

		public void SetRayTracingPipelineStackSize(uint pipelineStackSize) {
			unsafe {
				Device.KHRRayTracingPipeline!.vkCmdSetRayTracingPipelineStackSizeKHR(CommandBuffer, pipelineStackSize);
			}
		}

		public void TraceRaysIndirect(in VKStridedDeviceAddressRegionKHR raygenShaderBindingTable, in VKStridedDeviceAddressRegionKHR missShaderBindingTable, in VKStridedDeviceAddressRegionKHR hitShaderBindingTable, in VKStridedDeviceAddressRegionKHR callableShaderBindingTable, ulong indirectDeviceAddress) {
			unsafe {
				Device.KHRRayTracingPipeline!.vkCmdTraceRaysIndirectKHR(CommandBuffer, raygenShaderBindingTable, missShaderBindingTable, hitShaderBindingTable, callableShaderBindingTable, indirectDeviceAddress);
			}
		}

		public void TraceRays(in VKStridedDeviceAddressRegionKHR raygenShaderBindingTable, in VKStridedDeviceAddressRegionKHR missShaderBindingTable, in VKStridedDeviceAddressRegionKHR hitShaderBindingTable, in VKStridedDeviceAddressRegionKHR callableShaderBindingTable, Vector3ui size) {
			unsafe {
				Device.KHRRayTracingPipeline!.vkCmdTraceRaysKHR(CommandBuffer, raygenShaderBindingTable, missShaderBindingTable, hitShaderBindingTable, callableShaderBindingTable, size.X, size.Y, size.Z);
			}
		}

		public static implicit operator IntPtr(VKCommandBuffer? commandBuffer) => commandBuffer != null ? commandBuffer.CommandBuffer : IntPtr.Zero;

	}

}
