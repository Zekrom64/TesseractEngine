using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.Vulkan.Graphics.Impl {
	
	/// <summary>
	/// Command manager for Vulkan commands.
	/// </summary>
	public class VulkanCommands : IDisposable {

		/// <summary>
		/// Container class for a Vulkan command pool.
		/// </summary>
		public class CommandPool : IDisposable {

			/// <summary>
			/// The command bank the pool belongs to.
			/// </summary>
			public CommandBank Bank { get; }

			/// <summary>
			/// The command pool.
			/// </summary>
			public VKCommandPool Pool { get; }

			/// <summary>
			/// Semaphore gating access to the pool.
			/// </summary>
			public SemaphoreSlim PoolSemaphore { get; } = new(1);

			internal CommandPool(CommandBank bank, VKCommandPool pool) {
				Bank = bank;
				Pool = pool;
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				Pool.Dispose();
				PoolSemaphore.Dispose();
			}

			/// <summary>
			/// Trims the command pool.
			/// </summary>
			public void Trim() {
				// Gate access to pool
				PoolSemaphore.Wait();
				try {
					// Trim pool
					Pool.Trim();
				} finally {
					PoolSemaphore.Release();
				}
			}

			/// <summary>
			/// Allocates a command buffer from this pool.
			/// </summary>
			/// <param name="createInfo">Command buffer creation information</param>
			/// <returns>Allocated command buffer</returns>
			public VKCommandBuffer Allocate(CommandBufferCreateInfo createInfo) {
				// Gate access to pool
				PoolSemaphore.Wait();
				try {
					// Allocate a single buffer
					return Pool.Allocate(new VKCommandBufferAllocateInfo() {
						Type = VKStructureType.CommandBufferAllocateInfo,
						CommandBufferCount = 1,
						CommandPool = Pool,
						Level = VulkanConverter.Convert(createInfo.Type)
					})[0];
				} finally {
					PoolSemaphore.Release();
				}
			}

			/// <summary>
			/// Frees the given command buffers back to this pool.
			/// </summary>
			/// <param name="cmdbuf">Command buffers to free</param>
			public void Free(params VKCommandBuffer[] cmdbuf) {
				PoolSemaphore.Wait();
				try {
					Pool.Free(cmdbuf);
				} finally {
					PoolSemaphore.Release();
				}
			}

		}

		/// <summary>
		/// A command bank contains a number of pools for a single queue, which
		/// are rotated between to provide concurrent command recording ability.
		/// </summary>
		public class CommandBank : IDisposable {

			/// <summary>
			/// The queue the bank is bound to.
			/// </summary>
			public VulkanDeviceQueueInfo QueueInfo { get; }

			// All of the command pools in this bank
			private readonly CommandPool[] commandPools;

			// Counter for the next command pool
			private uint nextCommandPool = 0;

			internal CommandBank(int parallelism, VulkanDevice device, VulkanDeviceQueueInfo queueInfo) {
				QueueInfo = queueInfo;
				commandPools = new CommandPool[parallelism];
				VKCommandPoolCreateInfo createInfo = new() {
					Type = VKStructureType.CommandPoolCreateInfo,
					Flags = VKCommandPoolCreateFlagBits.ResetCommandBuffer,
					QueueFamilyIndex = queueInfo.QueueFamily
				};
				for(int i = 0; i < parallelism; i++) commandPools[i] = new CommandPool(this, device.Device.CreateCommandPool(createInfo));
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				foreach (CommandPool pool in commandPools) pool.Dispose();
			}

			/// <summary>
			/// Acquires the next command pool in this bank.
			/// </summary>
			/// <returns>Next command pool</returns>
			public CommandPool Acquire() => commandPools[Interlocked.Increment(ref nextCommandPool) % commandPools.Length];

			/// <summary>
			/// Trims the command pools in this bank.
			/// </summary>
			public void Trim() {
				foreach(CommandPool pool in commandPools) pool.Trim();
			}

			internal void Submit(in VKSubmitInfo info, VKFence? fence) {
				QueueInfo.QueueSemaphore?.Wait();
				try {
					QueueInfo.Queue.Submit(info, fence);
				} finally {
					QueueInfo.QueueSemaphore?.Release();
				}
			}

			internal VKResult Present(in VKPresentInfoKHR info) {
				QueueInfo.QueueSemaphore?.Wait();
				try {
					return QueueInfo.Queue.PresentKHR(info);
				} finally {
					QueueInfo.QueueSemaphore?.Release();
				}
			}

			internal void WaitIdle() {
				QueueInfo.QueueSemaphore?.Wait();
				try {
					QueueInfo.Queue.WaitIdle();
				} finally {
					QueueInfo.QueueSemaphore?.Release();
				}
			}

		}
	
		/// <summary>
		/// The command bank for graphics commands.
		/// </summary>
		public CommandBank CommandBankGraphics { get; }

		/// <summary>
		/// The command bank for transfer commands.
		/// </summary>
		public CommandBank CommandBankTransfer { get; }

		/// <summary>
		/// The command bank for compute commands.
		/// </summary>
		public CommandBank CommandBankCompute { get; }

		// If command banks can be trimmed (ie. VK >= 1.2 or VK_KHR_maintenance)
		private readonly bool canTrim = false;
		
		// Orphaned command buffer holder
		private struct OrphanedCommmandBuffer : IDisposable {

			// The orphaned buffer
			public VulkanCommandBuffer CommandBuffer;

			// The fence indicating when the buffer is free
			public VulkanFenceSync Fence;

			// If the fence should be disposed along with the command buffer
			public bool DisposeFence;

			public void Dispose() {
				CommandBuffer.Dispose();
				if (DisposeFence) Fence.Dispose();
			}

		}

		// List of orphaned command buffers
		private readonly List<OrphanedCommmandBuffer> orphanedCommmandBuffers = new();
		// GC threshold for orphaned command buffers
		private readonly int gcThreshold;

		// Reader-writer lock for access to the command bank queues
		// When "read" locked submission is allowed concurrently
		// "Write" locked when queues are accessed w/o submission (ex. wait idle)
		private readonly ReaderWriterLockSlim rwQueuesLock = new();

		/// <summary>
		/// Creates a new command manager in the given context.
		/// </summary>
		/// <param name="device">Vulkan device</param>
		/// <param name="poolParallelism">The degree of parallelism for command pools (number of concurrent pools per bank)</param>
		/// <param name="gcThreshold">The threshold of command buffers above which garbage should be collected</param>
		public VulkanCommands(VulkanDevice device, int poolParallelism, int gcThreshold) {
			CommandBankGraphics = new(poolParallelism, device, device.QueueGraphics);
			CommandBankTransfer = new(poolParallelism, device, device.QueueTransfer);
			CommandBankCompute = new(poolParallelism, device, device.QueueCompute);
			canTrim = device.Device.APIVersion >= VK12.ApiVersion || device.EnabledExtensions.Contains(KHRMaintenance1.ExtensionName);
			this.gcThreshold = gcThreshold;
		}

		/// <summary>
		/// Attempts to trim command pools.
		/// </summary>
		public void Trim() {
			if (canTrim) {
				CommandBankGraphics.Trim();
				CommandBankTransfer.Trim();
				CommandBankCompute.Trim();
			}
		}

		// GCs any orphaned command buffers once they are free
		private void GCOrphanedBuffers() {
			for(int i = 0; i < orphanedCommmandBuffers.Count; i++) {
				var cmdbuf = orphanedCommmandBuffers[i];
				if (cmdbuf.Fence.Fence.Status) {
					cmdbuf.Dispose();
					orphanedCommmandBuffers.RemoveAt(i);
					i--;
				}
			}
		}

		// Attempts to GC orphaned command buffers if above the threshold
		private void TryGCOrphanedBuffers() {
			if (orphanedCommmandBuffers.Count > gcThreshold) GCOrphanedBuffers();
		}

		// Checks that the requested image granularity is compatible with the minimum in every axis
		private static bool CheckGranularity(Vector3ui min, Vector3ui requested) =>
			(requested.X == 0 || min.X <= requested.X) &&
			(requested.Y == 0 || min.Y <= requested.Y) &&
			(requested.Z == 0 || min.Z <= requested.Z);

		/// <summary>
		/// Allocates a command buffer.
		/// </summary>
		/// <param name="createInfo">Command buffer creation informaiton</param>
		/// <returns>Allocated command buffer</returns>
		public VulkanCommandBuffer Alloc(CommandBufferCreateInfo createInfo) {
			bool graphics = (createInfo.Usage & CommandBufferUsage.Graphics) != 0;
			bool transfer = (createInfo.Usage & CommandBufferUsage.Transfer) != 0;
			bool compute = (createInfo.Usage & CommandBufferUsage.Compute) != 0;

			// Need to decide the command bank to use
			CommandBank? cmdbank = null;
			// If graphics commands required
			if (graphics) {
				// Use graphics command bank
				cmdbank = CommandBankGraphics;
				// If compute requested but not available drop the bank
				if (compute && (cmdbank.QueueInfo.QueueFlags & VKQueueFlagBits.Compute) == 0) cmdbank = null;
				// Else if granularity not supported drop the bank
				else if (!CheckGranularity(cmdbank.QueueInfo.MinImageTransferGranularity, createInfo.RequiredTransferGranularity)) cmdbank = null;
			}
			// If compute commands requested and not filled
			if (compute && cmdbank == null) {
				cmdbank = CommandBankCompute;
				// If graphics requested but not available drop the bank
				if (graphics && (cmdbank.QueueInfo.QueueFlags & VKQueueFlagBits.Graphics) == 0) cmdbank = null;
				// Else if granularity not supported drop the bank
				else if (!CheckGranularity(cmdbank.QueueInfo.MinImageTransferGranularity, createInfo.RequiredTransferGranularity)) cmdbank = null;
			}
			// If transfer commands requested and not filled
			if (transfer && cmdbank == null) {
				cmdbank = CommandBankTransfer;
				// If graphics requested but not available drop the bank
				if (graphics && (cmdbank.QueueInfo.QueueFlags & VKQueueFlagBits.Graphics) == 0) cmdbank = null;
				// If compute requested but not available drop the bank
				else if (compute && (cmdbank.QueueInfo.QueueFlags & VKQueueFlagBits.Compute) == 0) cmdbank = null;
				// Else if granularity not supported switch to the graphics queue bank
				else if (!CheckGranularity(cmdbank.QueueInfo.MinImageTransferGranularity, createInfo.RequiredTransferGranularity)) cmdbank = CommandBankGraphics;
				// If granularity not supported drop the bank
				if (cmdbank != null && !CheckGranularity(cmdbank.QueueInfo.MinImageTransferGranularity, createInfo.RequiredTransferGranularity)) cmdbank = null;
			}

			if (cmdbank == null) throw new VulkanException("Could not find suitable command bank to allocate command buffer from");

			CommandPool cmdpool = cmdbank.Acquire();
			return new VulkanCommandBuffer(cmdpool, cmdpool.Allocate(createInfo), createInfo.Type);
		}

		/// <summary>
		/// Schedules the given command buffer to be disposed of when it is no longer in use.
		/// </summary>
		/// <param name="cmdbuf">The command buffer to dispose</param>
		/// <param name="fence">The fence to wait on to guarentee the commands are not in use</param>
		/// <param name="delFence">If the fence should be deleted with the command buffer</param>
		public void DisposeWhenFree(VulkanCommandBuffer cmdbuf, VulkanFenceSync fence, bool delFence) {
			// Append the orphaned buffer
			lock(orphanedCommmandBuffers) {
				orphanedCommmandBuffers.Add(new OrphanedCommmandBuffer() {
					CommandBuffer = cmdbuf,
					Fence = fence,
					DisposeFence = delFence
				});
				// Try to GC orphaned buffers
				TryGCOrphanedBuffers();
			}
		}

		internal void Submit(CommandBank cmdBank, in ReadOnlySpan<IntPtr> commandBuffers, in ReadOnlySpan<ulong> waitSem, in ReadOnlySpan<VKPipelineStageFlagBits> waitStages, in ReadOnlySpan<ulong> signalSem, VKFence? fence) {
			using MemoryStack sp = MemoryStack.Push();
			// Acquire concurrent lock
			rwQueuesLock.EnterReadLock();
			try {
				// Submit commands via command bank
				cmdBank.Submit(new VKSubmitInfo() {
					Type = VKStructureType.SubmitInfo,
					CommandBufferCount = (uint)commandBuffers.Length,
					CommandBuffers = sp.Values(commandBuffers),
					SignalSemaphoreCount = (uint)signalSem.Length,
					SignalSemaphores = sp.Values(signalSem),
					WaitSemaphoreCount = (uint)Math.Min(waitSem.Length, waitStages.Length),
					WaitSemaphores = sp.Values(waitSem),
					WaitDstStageMask = sp.Values(waitStages)
				}, fence);
			} finally {
				rwQueuesLock.ExitReadLock();
			}
			// Try to GC orphaned buffers
			lock (orphanedCommmandBuffers) {
				TryGCOrphanedBuffers();
			}
		}

		/// <summary>
		/// Waits until all command banks are idle.
		/// </summary>
		public void WaitIdle() {
			// Exclude any command submission while waiting
			rwQueuesLock.EnterWriteLock();
			try {
				// Wait for each command bank's queue
				CommandBankGraphics.QueueInfo.Queue.WaitIdle();
				CommandBankTransfer.QueueInfo.Queue.WaitIdle();
				CommandBankCompute.QueueInfo.Queue.WaitIdle();
				// GC any orphaned buffers
				lock (orphanedCommmandBuffers) {
					GCOrphanedBuffers();
				}
			} finally {
				rwQueuesLock.ExitWriteLock();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			// Make sure nothing is executing anymore
			WaitIdle();
			// Destroy any orphaned buffers
			foreach (var orphanedBuf in orphanedCommmandBuffers) orphanedBuf.CommandBuffer.Dispose();
			// Destroy command banks
			CommandBankGraphics.Dispose();
			CommandBankTransfer.Dispose();
			CommandBankCompute.Dispose();
			// Destroy queue lock
			rwQueuesLock.Dispose();
		}

	}

	/// <summary>
	/// Vulkan command buffer.
	/// </summary>
	public class VulkanCommandBuffer : ICommandBuffer, ICommandSink {

		public ulong QueueID { get; }

		/// <summary>
		/// The command pool the buffer was allocated form.
		/// </summary>
		public VulkanCommands.CommandPool CommandPool { get; }

		/// <summary>
		/// The underlying command buffer.
		/// </summary>
		public VKCommandBuffer CommandBuffer { get; }

		public CommandBufferType Type { get; }

		public VulkanCommandBuffer(VulkanCommands.CommandPool commandPool, VKCommandBuffer cmdbuf, CommandBufferType type) {
			QueueID = commandPool.Bank.QueueInfo.QueueID;
			CommandPool = commandPool;
			CommandBuffer = cmdbuf;
			Type = type;
		}

		public CommandMode Mode => CommandMode.Buffered;

		public void Dispose() {
			GC.SuppressFinalize(this);
			var sem = CommandPool.PoolSemaphore;
			sem.Wait();
			try {
				CommandBuffer.Dispose();
			} finally {
				sem.Release();
			}
		}

		private bool recorded;

		public ICommandSink BeginRecording() {
			CommandPool.PoolSemaphore.Wait();
			if (recorded) CommandBuffer.Reset();
			CommandBuffer.Begin(new VKCommandBufferBeginInfo() {
				Type = VKStructureType.CommandBufferBeginInfo,
			});
			return this;
		}

		public void EndRecording() {
			CommandBuffer.End();
			recorded = true;
			CommandPool.PoolSemaphore.Release();
		}


		public void Barrier(in ICommandSink.PipelineBarriers barriers) {
			Span<VKMemoryBarrier> mems = stackalloc VKMemoryBarrier[barriers.MemoryBarriers.Length];
			for (int i = 0; i < mems.Length; i++) mems[i] = VulkanConverter.Convert(barriers.MemoryBarriers[i]);
			Span<VKBufferMemoryBarrier> bufs = stackalloc VKBufferMemoryBarrier[barriers.BufferMemoryBarriers.Length];
			for (int i = 0; i < bufs.Length; i++) bufs[i] = VulkanConverter.Convert(barriers.BufferMemoryBarriers[i]);
			Span<VKImageMemoryBarrier> imgs = stackalloc VKImageMemoryBarrier[barriers.TextureMemoryBarriers.Length];
			for (int i = 0; i < imgs.Length; i++) imgs[i] = VulkanConverter.Convert(barriers.TextureMemoryBarriers[i]);

			CommandBuffer.PipelineBarrier(VulkanConverter.Convert(barriers.ProvokingStages), VulkanConverter.Convert(barriers.AwaitingStages), 0, mems, bufs, imgs);
		}

		public void BeginRenderPass(in ICommandSink.RenderPassBegin begin, SubpassContents contents) {
			using MemoryStack sp = MemoryStack.Push();
			CommandBuffer.BeginRenderPass(VulkanConverter.Convert(sp, begin), VulkanConverter.Convert(contents));
		}

		public void BindPipeline(IPipeline pipeline) {
			VulkanPipeline vkpipeline = (VulkanPipeline)pipeline;
			CommandBuffer.BindPipeline(vkpipeline.BindPoint, vkpipeline.Pipeline);
		}

		public void BindPipelineWithState(IPipelineSet set, PipelineDynamicCreateInfo state) {
			// Bind the pipeline from the set
			VulkanPipelineSet vkpipelineset = (VulkanPipelineSet)set;
			CommandBuffer.BindPipeline(vkpipelineset.BindPoint, vkpipelineset.Pipelines[state]);
			if (vkpipelineset.BindPoint == VKPipelineBindPoint.Graphics) {
				// Set any dynamic state on the bound pipeline
				foreach (PipelineDynamicState dyn in vkpipelineset.BaseInfo.GraphicsInfo!.DynamicState) {
					switch (dyn) {
						case PipelineDynamicState.Viewport:
							SetViewports(0, state.Viewports.ToArray());
							break;
						case PipelineDynamicState.Scissor:
							SetScissors(0, state.Scissors.ToArray());
							break;
						case PipelineDynamicState.LineWidth:
							SetLineWidth(state.LineWidth);
							break;
						case PipelineDynamicState.DepthBias:
							SetDepthBias(state.DepthBiasConstantFactor, state.DepthBiasClamp, state.DepthBiasSlopeFactor);
							break;
						case PipelineDynamicState.BlendConstants:
							SetBlendConstants(state.BlendConstant);
							break;
						case PipelineDynamicState.DepthBounds:
							SetDepthBounds(state.DepthBounds.Min, state.DepthBounds.Max);
							break;
						case PipelineDynamicState.StencilCompareMask:
							SetStencilCompareMask(CullFace.Front, state.FrontStencilState.CompareMask);
							SetStencilCompareMask(CullFace.Back, state.BackStencilState.CompareMask);
							break;
						case PipelineDynamicState.StencilWriteMask:
							SetStencilWriteMask(CullFace.Front, state.FrontStencilState.WriteMask);
							SetStencilWriteMask(CullFace.Back, state.BackStencilState.WriteMask);
							break;
						case PipelineDynamicState.StencilReference:
							SetStencilReference(CullFace.Front, state.FrontStencilState.Reference);
							SetStencilReference(CullFace.Back, state.FrontStencilState.Reference);
							break;
						case PipelineDynamicState.CullMode:
							SetCullMode(state.CullMode);
							break;
						case PipelineDynamicState.FrontFace:
							SetFrontFace(state.FrontFace);
							break;
						case PipelineDynamicState.DrawMode:
							SetDrawMode(state.DrawMode);
							break;
						case PipelineDynamicState.DepthTestEnable:
							SetDepthTestEnable(state.DepthTestEnable);
							break;
						case PipelineDynamicState.DepthWriteEnable:
							SetDepthWriteEnable(state.DepthWriteEnable);
							break;
						case PipelineDynamicState.DepthCompareOp:
							SetDepthCompareOp(state.DepthCompareOp);
							break;
						case PipelineDynamicState.DepthBoundsTestEnable:
							SetDepthBoundsTestEnable(state.DepthBoundsTestEnable);
							break;
						case PipelineDynamicState.StencilTestEnable:
							SetStencilTestEnable(state.StencilTestEnable);
							break;
						case PipelineDynamicState.StencilOp:
							SetStencilOp(CullFace.Front, state.FrontStencilState.FailOp, state.FrontStencilState.PassOp, state.FrontStencilState.DepthFailOp, state.FrontStencilState.CompareOp);
							SetStencilOp(CullFace.Back, state.BackStencilState.FailOp, state.BackStencilState.PassOp, state.BackStencilState.DepthFailOp, state.BackStencilState.CompareOp);
							break;
						case PipelineDynamicState.PatchControlPoints:
							SetPatchControlPoints(state.PatchControlPoints);
							break;
						case PipelineDynamicState.RasterizerDiscardEnable:
							SetRasterizerDiscardEnable(state.RasterizerDiscardEnable);
							break;
						case PipelineDynamicState.DepthBiasEnable:
							SetDepthBiasEnable(state.DepthBiasEnable);
							break;
						case PipelineDynamicState.LogicOp:
							SetLogicOp(state.LogicOp);
							break;
						case PipelineDynamicState.PrimitiveRestartEnable:
							SetPrimitiveRestartEnable(state.PrimitiveRestartEnable);
							break;
						case PipelineDynamicState.VertexFormat:
							SetVertexFormat(state.VertexFormat);
							break;
						case PipelineDynamicState.ColorWrite:
							SetColorWriteEnable(state.ColorWriteEnable.ToArray());
							break;
						case PipelineDynamicState.ViewportCount:
							SetViewportsWithCount(state.Viewports.List.ToArray());
							break;
						case PipelineDynamicState.ScissorCount:
							SetScissorsWithCount(state.Scissors.List.ToArray());
							break;
						default: break;
					}
				}
			}
		}

		public void BindVertexArray(IVertexArray array) {
			var vkarray = (VulkanVertexArray)array;
			if (vkarray.VertexBuffers != null) {
				foreach (var binding in vkarray.VertexBuffers) {
					CommandBuffer.BindVertexBuffers(binding.Item3, binding.Item1.Buffer, binding.Item2.Offset);
				}
			}
			if (vkarray.IndexBuffer != null) {
				var index = vkarray.IndexBuffer.Value;
				CommandBuffer.BindIndexBuffer(index.Item1.Buffer, index.Item2.Offset, index.Item3);
			}
		}

		public void BindResources(PipelineType bindPoint, IPipelineLayout layout, params IBindSet[] sets) =>
			CommandBuffer.BindDescriptorSets(VulkanConverter.Convert(bindPoint), ((VulkanPipelineLayout)layout).Layout, sets.ConvertAll(set => ((VulkanBindSet)set).Set));

		public void BlitFramebuffer(IFramebuffer dst, int dstAttachment, TextureLayout dstLayout, Recti dstArea, IFramebuffer src, int srcAttachment, TextureLayout srcLayout, Recti srcArea, TextureAspect aspect, TextureFilter filter) {
			VulkanFramebuffer vkdst = (VulkanFramebuffer)dst, vksrc = (VulkanFramebuffer)src;
			VulkanTextureView vkdstview = vkdst.Attachments[dstAttachment], vksrcview = vksrc.Attachments[srcAttachment];
			VulkanTexture vkdstimg = vkdstview.Texture, vksrcimg = vksrcview.Texture;
			VKImageAspectFlagBits vkaspect = VulkanConverter.Convert(aspect);
			VKImageBlit blit = new() {
				SrcOffsets = new() {
					X = new Vector3i(srcArea.Minimum, 0),
					Y = new Vector3i(srcArea.Maximum, 1)
				},
				SrcSubresource = new() {
					AspectMask = vkaspect,
					BaseArrayLayer = vksrcview.SubresourceRange.BaseArrayLayer,
					LayerCount = vksrcview.SubresourceRange.ArrayLayerCount,
					MipLevel = vksrcview.SubresourceRange.BaseMipLevel
				},
				DstOffsets = new() {
					X = new Vector3i(dstArea.Minimum, 0),
					Y = new Vector3i(dstArea.Maximum, 1)
				},
				DstSubresource = new() {
					AspectMask = vkaspect,
					BaseArrayLayer = vkdstview.SubresourceRange.BaseArrayLayer,
					LayerCount = vkdstview.SubresourceRange.ArrayLayerCount,
					MipLevel = vksrcview.SubresourceRange.BaseMipLevel
				}
			};
			CommandBuffer.BlitImage(vksrcimg.Image, VulkanConverter.Convert(srcLayout), vkdstimg.Image, VulkanConverter.Convert(dstLayout), VulkanConverter.Convert(filter), blit);
		}

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ReadOnlySpan<ICommandSink.BlitTextureRegion> regions) {
			VulkanTexture vkdst = (VulkanTexture)dst, vksrc = (VulkanTexture)src;
			Span<VKImageBlit> blits = stackalloc VKImageBlit[regions.Length];
			for (int i = 0; i < blits.Length; i++) blits[i] = VulkanConverter.Convert(regions[i], vksrc, vkdst);
			CommandBuffer.BlitImage(vksrc.Image, VulkanConverter.Convert(srcLayout), vkdst.Image, VulkanConverter.Convert(dstLayout), blits, VulkanConverter.Convert(filter));
		}

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, params ICommandSink.BlitTextureRegion[] regions) {
			VulkanTexture vkdst = (VulkanTexture)dst, vksrc = (VulkanTexture)src;
			Span<VKImageBlit> blits = stackalloc VKImageBlit[regions.Length];
			for (int i = 0; i < blits.Length; i++) blits[i] = VulkanConverter.Convert(regions[i], vksrc, vkdst);
			CommandBuffer.BlitImage(vksrc.Image, VulkanConverter.Convert(srcLayout), vkdst.Image, VulkanConverter.Convert(dstLayout), blits, VulkanConverter.Convert(filter));
		}

		public void BlitTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, TextureFilter filter, in ICommandSink.BlitTextureRegion blit) {
			VulkanTexture vkdst = (VulkanTexture)dst, vksrc = (VulkanTexture)src;
			VKImageBlit blit2 = VulkanConverter.Convert(blit, vksrc, vkdst);
			CommandBuffer.BlitImage(vksrc.Image, VulkanConverter.Convert(srcLayout), vkdst.Image, VulkanConverter.Convert(dstLayout), blit2, VulkanConverter.Convert(filter));
		}

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ReadOnlySpan<ICommandSink.ClearRect> regions) {
			Span<VKClearRect> rects = stackalloc VKClearRect[regions.Length];
			for (int i = 0; i < rects.Length; i++) rects[i] = VulkanConverter.Convert(regions[i]);
			Span<VKClearAttachment> attachments = stackalloc VKClearAttachment[values.Length];
			for (int i = 0; i < values.Length; i++) attachments[i] = VulkanConverter.Convert(values[i]);
			CommandBuffer.ClearAttachments(attachments, rects);
		}

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, params ICommandSink.ClearRect[] regions) {
			Span<VKClearRect> rects = stackalloc VKClearRect[regions.Length];
			for (int i = 0; i < rects.Length; i++) rects[i] = VulkanConverter.Convert(regions[i]);
			Span<VKClearAttachment> attachments = stackalloc VKClearAttachment[values.Length];
			for (int i = 0; i < values.Length; i++) attachments[i] = VulkanConverter.Convert(values[i]);
			CommandBuffer.ClearAttachments(attachments, rects);
		}

		public void ClearAttachments(in ReadOnlySpan<ICommandSink.ClearAttachment> values, in ICommandSink.ClearRect region) {
			Span<VKClearAttachment> attachments = stackalloc VKClearAttachment[values.Length];
			for (int i = 0; i < values.Length; i++) attachments[i] = VulkanConverter.Convert(values[i]);
			CommandBuffer.ClearAttachments(attachments, stackalloc VKClearRect[] { VulkanConverter.Convert(region) });
		}

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in ReadOnlySpan<TextureSubresourceRange> regions) {
			Span<VKImageSubresourceRange> ranges = stackalloc VKImageSubresourceRange[regions.Length];
			for (int i = 0; i < ranges.Length; i++) ranges[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.ClearColorImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.Convert(color), ranges);
		}

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, params TextureSubresourceRange[] regions) {
			Span<VKImageSubresourceRange> ranges = stackalloc VKImageSubresourceRange[regions.Length];
			for (int i = 0; i < ranges.Length; i++) ranges[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.ClearColorImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.Convert(color), ranges);
		}

		public void ClearColorTexture(ITexture dst, TextureLayout dstLayout, ICommandSink.ClearColorValue color, in TextureSubresourceRange region) =>
			CommandBuffer.ClearColorImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.Convert(color), VulkanConverter.Convert(region));

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in ReadOnlySpan<TextureSubresourceRange> regions) {
			Span<VKImageSubresourceRange> ranges = stackalloc VKImageSubresourceRange[regions.Length];
			for (int i = 0; i < ranges.Length; i++) ranges[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.ClearDepthStencilImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), new() { Depth = depth, Stencil = stencil }, ranges);
		}

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, params TextureSubresourceRange[] regions) {
			Span<VKImageSubresourceRange> ranges = stackalloc VKImageSubresourceRange[regions.Length];
			for (int i = 0; i < ranges.Length; i++) ranges[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.ClearDepthStencilImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), new() { Depth = depth, Stencil = stencil }, ranges);
		}

		public void ClearDepthStencilTexture(ITexture dst, TextureLayout dstLayout, float depth, uint stencil, in TextureSubresourceRange region) =>
			CommandBuffer.ClearDepthStencilImage(((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), new() { Depth = depth, Stencil = stencil }, VulkanConverter.Convert(region));

		public void CopyBuffer(IBuffer dst, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferRegion> regions) {
			Span<VKBufferCopy> copies = stackalloc VKBufferCopy[regions.Length];
			for (int i = 0; i < copies.Length; i++) copies[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.CopyBuffer(((VulkanBuffer)src).Buffer, ((VulkanBuffer)dst).Buffer, copies);
		}

		public void CopyBuffer(IBuffer dst, IBuffer src, params ICommandSink.CopyBufferRegion[] regions) {
			Span<VKBufferCopy> copies = stackalloc VKBufferCopy[regions.Length];
			for (int i = 0; i < copies.Length; i++) copies[i] = VulkanConverter.Convert(regions[i]);
			CommandBuffer.CopyBuffer(((VulkanBuffer)src).Buffer, ((VulkanBuffer)dst).Buffer, copies);
		}

		public void CopyBuffer(IBuffer dst, IBuffer src, in ICommandSink.CopyBufferRegion region) => CommandBuffer.CopyBuffer(((VulkanBuffer)src).Buffer, ((VulkanBuffer)dst).Buffer, VulkanConverter.Convert(region));

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			Span<VKBufferImageCopy> vkcopies = stackalloc VKBufferImageCopy[copies.Length];
			for (int i = 0; i < copies.Length; i++) vkcopies[i] = VulkanConverter.Convert(copies[i]);
			CommandBuffer.CopyBufferToImage(((VulkanBuffer)src).Buffer, ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), vkcopies);
		}

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, params ICommandSink.CopyBufferTexture[] copies) {
			Span<VKBufferImageCopy> vkcopies = stackalloc VKBufferImageCopy[copies.Length];
			for (int i = 0; i < copies.Length; i++) vkcopies[i] = VulkanConverter.Convert(copies[i]);
			CommandBuffer.CopyBufferToImage(((VulkanBuffer)src).Buffer, ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), vkcopies);
		}

		public void CopyBufferToTexture(ITexture dst, TextureLayout dstLayout, IBuffer src, in ICommandSink.CopyBufferTexture copy) =>
			CommandBuffer.CopyBufferToImage(((VulkanBuffer)src).Buffer, ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.Convert(copy));

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) {
			Span<VKImageCopy> vkcopies = stackalloc VKImageCopy[regions.Length];
			for (int i = 0; i < regions.Length; i++) vkcopies[i] = VulkanConverter.ConvertImageCopy(regions[i]);
			CommandBuffer.CopyImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), vkcopies);
		}

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyTextureRegion[] regions) {
			Span<VKImageCopy> vkcopies = stackalloc VKImageCopy[regions.Length];
			for (int i = 0; i < regions.Length; i++) vkcopies[i] = VulkanConverter.ConvertImageCopy(regions[i]);
			CommandBuffer.CopyImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), vkcopies);
		}

		public void CopyTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyTextureRegion copy) =>
			CommandBuffer.CopyImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.ConvertImageCopy(copy));

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyBufferTexture> copies) {
			Span<VKBufferImageCopy> vkcopies = stackalloc VKBufferImageCopy[copies.Length];
			for (int i = 0; i < copies.Length; i++) vkcopies[i] = VulkanConverter.Convert(copies[i]);
			CommandBuffer.CopyImageToBuffer(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanBuffer)dst).Buffer, vkcopies);
		}

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyBufferTexture[] copies) {
			Span<VKBufferImageCopy> vkcopies = stackalloc VKBufferImageCopy[copies.Length];
			for (int i = 0; i < copies.Length; i++) vkcopies[i] = VulkanConverter.Convert(copies[i]);
			CommandBuffer.CopyImageToBuffer(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanBuffer)dst).Buffer, vkcopies);
		}

		public void CopyTextureToBuffer(IBuffer dst, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyBufferTexture copy) =>
			CommandBuffer.CopyImageToBuffer(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanBuffer)dst).Buffer, VulkanConverter.Convert(copy));

		public void Dispatch(Vector3ui groupCounts) => CommandBuffer.Dispatch(groupCounts);

		public void DispatchIndirect(IBuffer buffer, nuint offset) => CommandBuffer.DispatchIndirect(((VulkanBuffer)buffer).Buffer, offset);

		public void Draw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) => CommandBuffer.Draw(vertexCount, instanceCount, firstVertex, firstInstance);

		public void DrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) => CommandBuffer.DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);

		public void DrawIndexedIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride) => CommandBuffer.DrawIndexedIndirect(((VulkanBuffer)buffer).Buffer, offset, drawCount, stride);

		public void DrawIndirect(IBuffer buffer, nuint offset, uint drawCount, uint stride) => CommandBuffer.DrawIndirect(((VulkanBuffer)buffer).Buffer, offset, drawCount, stride);

		public void EndRenderPass() => CommandBuffer.EndRenderPass();

		public void ExecuteCommands(in ReadOnlySpan<ICommandBuffer> buffers) {
			Span<VKCommandBuffer> cmdbufs = new VKCommandBuffer[buffers.Length];
			for (int i = 0; i < buffers.Length; i++) cmdbufs[i] = ((VulkanCommandBuffer)buffers[i]).CommandBuffer;
			CommandBuffer.ExecuteCommands(cmdbufs);
		}

		public void ExecuteCommands(params ICommandBuffer[] buffers) {
			Span<VKCommandBuffer> cmdbufs = new VKCommandBuffer[buffers.Length];
			for (int i = 0; i < buffers.Length; i++) cmdbufs[i] = ((VulkanCommandBuffer)buffers[i]).CommandBuffer;
			CommandBuffer.ExecuteCommands(cmdbufs);
		}

		public void ExecuteCommands(ICommandBuffer buffer) => CommandBuffer.ExecuteCommands(((VulkanCommandBuffer)buffer).CommandBuffer);

		public void FillBufferUInt32(IBuffer dst, nuint dstOffset, nuint dstSize, uint data) => CommandBuffer.FillBuffer(((VulkanBuffer)dst).Buffer, dstOffset, dstSize, data);

		public void GenerateMipmaps(ITexture dst, TextureLayout initialLayout, TextureLayout finalLayout, TextureFilter? filter = null) {
			// If no more than 1 mip level its not possible to generate mipmaps
			if (dst.MipLevels < 2) return;

			VKFilter vkfilter = filter.HasValue ? VulkanConverter.Convert(filter.Value) : VKFilter.Linear;
			VulkanTexture vkdst = (VulkanTexture)dst;
			// Transition from initial layout to standard setup: layer0 = src, layer1-max = dst
			switch (initialLayout) {
				case TextureLayout.TransferDst:
					CommandBuffer.PipelineBarrier(VKPipelineStageFlagBits.TopOfPipe, VKPipelineStageFlagBits.Transfer, 0,
						Span<VKMemoryBarrier>.Empty,
						Span<VKBufferMemoryBarrier>.Empty,
						stackalloc VKImageMemoryBarrier[] {
						new VKImageMemoryBarrier() {
							Type = VKStructureType.ImageMemoryBarrier,
							SrcAccessMask = VKAccessFlagBits.MemoryWrite,
							DstAccessMask = VKAccessFlagBits.TransferRead,
							SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
							DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
							Image = vkdst.Image,
							OldLayout = VKImageLayout.TransferDstOptimal,
							NewLayout = VKImageLayout.TransferSrcOptimal,
							SubresourceRange = new() { // Transition level 0 to SRC
								AspectMask = VKImageAspectFlagBits.Color,
								BaseArrayLayer = 0,
								BaseMipLevel = 0,
								LayerCount = dst.ArrayLayers,
								LevelCount = 1
							}
						}}
					);
					break;
				case TextureLayout.TransferSrc:
					CommandBuffer.PipelineBarrier(VKPipelineStageFlagBits.TopOfPipe, VKPipelineStageFlagBits.Transfer, 0,
						Span<VKMemoryBarrier>.Empty,
						Span<VKBufferMemoryBarrier>.Empty,
						stackalloc VKImageMemoryBarrier[] {
						new VKImageMemoryBarrier() {
							Type = VKStructureType.ImageMemoryBarrier,
							SrcAccessMask = VKAccessFlagBits.MemoryWrite,
							DstAccessMask = VKAccessFlagBits.TransferRead,
							SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
							DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
							Image = vkdst.Image,
							OldLayout = VKImageLayout.TransferSrcOptimal,
							NewLayout = VKImageLayout.TransferDstOptimal,
							SubresourceRange = new() { // Transition levels 1-max to DST
								AspectMask = VKImageAspectFlagBits.Color,
								BaseArrayLayer = 0,
								BaseMipLevel = 1,
								LayerCount = dst.ArrayLayers,
								LevelCount = dst.MipLevels - 1
							}
						}}
					);
					break;
				default:
					VKImageLayout vkinital = VulkanConverter.Convert(initialLayout);
					CommandBuffer.PipelineBarrier(VKPipelineStageFlagBits.TopOfPipe, VKPipelineStageFlagBits.Transfer, 0,
						Span<VKMemoryBarrier>.Empty,
						Span<VKBufferMemoryBarrier>.Empty,
						stackalloc VKImageMemoryBarrier[] {
						new VKImageMemoryBarrier() {
							Type = VKStructureType.ImageMemoryBarrier,
							SrcAccessMask = VKAccessFlagBits.MemoryWrite,
							DstAccessMask = VKAccessFlagBits.TransferRead,
							SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
							DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
							Image = vkdst.Image,
							OldLayout = vkinital,
							NewLayout = VKImageLayout.TransferSrcOptimal,
							SubresourceRange = new() { // Transition level 0 to SRC
								AspectMask = VKImageAspectFlagBits.Color,
								BaseArrayLayer = 0,
								BaseMipLevel = 0,
								LayerCount = dst.ArrayLayers,
								LevelCount = 1
							}
						},
						new VKImageMemoryBarrier() {
							Type = VKStructureType.ImageMemoryBarrier,
							SrcAccessMask = VKAccessFlagBits.MemoryWrite,
							DstAccessMask = VKAccessFlagBits.TransferRead,
							SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
							DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
							Image = vkdst.Image,
							OldLayout = vkinital,
							NewLayout = VKImageLayout.TransferDstOptimal,
							SubresourceRange = new() { // Transition level 1-max to DST
								AspectMask = VKImageAspectFlagBits.Color,
								BaseArrayLayer = 0,
								BaseMipLevel = 1,
								LayerCount = dst.ArrayLayers,
								LevelCount = dst.MipLevels - 1
							}
						}}
					);
					break;
			}
			Span<VKImageMemoryBarrier> imgBarrier = stackalloc VKImageMemoryBarrier[] {
				new VKImageMemoryBarrier() {
					Type = VKStructureType.ImageMemoryBarrier,
					SrcAccessMask = VKAccessFlagBits.TransferWrite,
					DstAccessMask = VKAccessFlagBits.TransferRead,
					SrcQueueFamilyIndex = VK10.QueueFamilyIgnored,
					DstQueueFamilyIndex = VK10.QueueFamilyIgnored,
					Image = vkdst.Image,
					OldLayout = VKImageLayout.TransferDstOptimal,
					NewLayout = VKImageLayout.TransferSrcOptimal,
				}
			};
			Vector3ui size = dst.Size;
			// For each mip level from 1-max
			for(uint level = 1; level < dst.MipLevels; level++) {
				// Blit from previous level
				Vector3ui newsize = size / 2;
				CommandBuffer.BlitImage(vkdst.Image, VKImageLayout.TransferSrcOptimal, vkdst.Image, VKImageLayout.TransferDstOptimal, new VKImageBlit() {
					SrcOffsets = (Vector3i.Zero, (Vector3i)size),
					SrcSubresource = new() {
						AspectMask = VKImageAspectFlagBits.Color,
						BaseArrayLayer = 0,
						MipLevel = (level - 1),
						LayerCount = dst.ArrayLayers
					},
					DstOffsets = (Vector3i.Zero, (Vector3i)newsize),
					DstSubresource = new() {
						AspectMask = VKImageAspectFlagBits.Color,
						BaseArrayLayer = 0,
						MipLevel = level,
						LayerCount = dst.ArrayLayers
					}
				}, vkfilter);
				// If not on last level
				if (level != dst.MipLevels - 1) {
					// Barrier writes and transition layout of current level to SRC
					imgBarrier[0].SubresourceRange = new() {
						AspectMask = VKImageAspectFlagBits.Color,
						BaseArrayLayer = 0,
						LayerCount = dst.ArrayLayers,
						BaseMipLevel = level,
						LevelCount = 1
					};
					CommandBuffer.PipelineBarrier(VKPipelineStageFlagBits.Transfer, VKPipelineStageFlagBits.Transfer, 0,
						Span<VKMemoryBarrier>.Empty, Span<VKBufferMemoryBarrier>.Empty, imgBarrier);
				}
				// Update size
				size = newsize;
			}
		}

		public void NextSubpass(SubpassContents contents) => CommandBuffer.NextSubpass(VulkanConverter.Convert(contents));

		public void PushConstants(IPipelineLayout layout, ShaderType stages, uint offset, uint size, IntPtr pValues) => CommandBuffer.PushConstants(((VulkanPipelineLayout)layout).Layout, VulkanConverter.Convert(stages), offset, size, pValues);

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, in ReadOnlySpan<T> values) where T : unmanaged => CommandBuffer.PushConstants(((VulkanPipelineLayout)layout).Layout, VulkanConverter.Convert(stages), offset, values);

		public void PushConstants<T>(IPipelineLayout layout, ShaderType stages, uint offset, params T[] values) where T : unmanaged => CommandBuffer.PushConstants(((VulkanPipelineLayout)layout).Layout, VulkanConverter.Convert(stages), offset, values);

		public void ResetSync(ISync dst, PipelineStage stage) => CommandBuffer.ResetEvent(((VulkanEventSync)dst).Event, VulkanConverter.Convert(stage));

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ReadOnlySpan<ICommandSink.CopyTextureRegion> regions) {
			Span<VKImageResolve> resolves = stackalloc VKImageResolve[regions.Length];
			for (int i = 0; i < resolves.Length; i++) resolves[i] = VulkanConverter.ConvertImageResolve(regions[i]);
			CommandBuffer.ResolveImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), resolves);
		}

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, params ICommandSink.CopyTextureRegion[] regions) {
			Span<VKImageResolve> resolves = stackalloc VKImageResolve[regions.Length];
			for (int i = 0; i < resolves.Length; i++) resolves[i] = VulkanConverter.ConvertImageResolve(regions[i]);
			CommandBuffer.ResolveImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), resolves);
		}

		public void ResolveTexture(ITexture dst, TextureLayout dstLayout, ITexture src, TextureLayout srcLayout, in ICommandSink.CopyTextureRegion region) =>
			CommandBuffer.ResolveImage(((VulkanTexture)src).Image, VulkanConverter.Convert(srcLayout), ((VulkanTexture)dst).Image, VulkanConverter.Convert(dstLayout), VulkanConverter.ConvertImageResolve(region));

		public void SetBlendConstants(Vector4 blendConst) => CommandBuffer.SetBlendConstants(blendConst);

		public void SetColorWriteEnable(in ReadOnlySpan<bool> enables) {
			Span<VKBool32> vkenables = stackalloc VKBool32[enables.Length];
			for (int i = 0; i < enables.Length; i++) vkenables[i] = enables[i];
			CommandBuffer.SetColorWriteEnableEXT(vkenables);
		}

		public void SetCullMode(CullFace culling) => CommandBuffer.SetCullModeEXT(VulkanConverter.ConvertCullMode(culling));

		public void SetDepthBias(float constFactor, float clamp, float slopeFactor) => CommandBuffer.SetDepthBias(constFactor, clamp, slopeFactor);

		public void SetDepthBiasEnable(bool enabled) => CommandBuffer.SetDepthBiasEnableEXT(enabled);

		public void SetDepthBounds(float min, float max) => CommandBuffer.SetDepthBounds(min, max);

		public void SetDepthBoundsTestEnable(bool enabled) => CommandBuffer.SetDepthBoundsTestEnableEXT(enabled);

		public void SetDepthCompareOp(CompareOp op) => CommandBuffer.SetDepthCompareOpEXT(VulkanConverter.Convert(op));

		public void SetDepthTestEnable(bool enabled) => CommandBuffer.SetDepthTestEnableEXT(enabled);

		public void SetDepthWriteEnable(bool enabled) => CommandBuffer.SetDepthWriteEnableEXT(enabled);

		public void SetDrawMode(DrawMode mode) => CommandBuffer.SetPrimitiveTopologyEXT(VulkanConverter.Convert(mode));

		public void SetFrontFace(FrontFace face) => CommandBuffer.SetFrontFaceEXT(VulkanConverter.Convert(face));

		public void SetLineWidth(float lineWidth) => CommandBuffer.SetLineWidth(lineWidth);

		public void SetLogicOp(LogicOp op) => CommandBuffer.SetLogicOpEXT(VulkanConverter.Convert(op));

		public void SetPatchControlPoints(uint controlPoints) => CommandBuffer.SetPatchControlPointsEXT(controlPoints);

		public void SetPrimitiveRestartEnable(bool enabled) => CommandBuffer.SetPrimitiveRestartEnableEXT(enabled);

		public void SetRasterizerDiscardEnable(bool enabled) => CommandBuffer.SetRasterizerDiscardEnableEXT(enabled);

		public void SetScissor(Recti scissor, uint firstScissor = 0) => CommandBuffer.SetScissor(scissor, firstScissor);

		public void SetScissors(in ReadOnlySpan<Recti> scissors, uint firstScissor = 0) {
			Span<VKRect2D> rects = stackalloc VKRect2D[scissors.Length];
			for (int i = 0; i < rects.Length; i++) rects[i] = scissors[i];
			CommandBuffer.SetScissor(rects, firstScissor);
		}

		public void SetScissors(uint firstScissor, params Recti[] scissors) {
			Span<VKRect2D> rects = stackalloc VKRect2D[scissors.Length];
			for (int i = 0; i < rects.Length; i++) rects[i] = scissors[i];
			CommandBuffer.SetScissor(rects, firstScissor);
		}

		public void SetScissorsWithCount(in ReadOnlySpan<Recti> scissors) {
			Span<VKRect2D> rects = stackalloc VKRect2D[scissors.Length];
			for (int i = 0; i < rects.Length; i++) rects[i] = scissors[i];
			CommandBuffer.SetScissorWithCountEXT(rects);
		}

		public void SetStencilCompareMask(CullFace face, uint compareMask) => CommandBuffer.SetStencilCompareMask(VulkanConverter.ConvertToStencilFace(face), compareMask);

		public void SetStencilOp(CullFace faces, StencilOp failOp, StencilOp passOp, StencilOp depthFailOp, CompareOp compareOp) =>
			CommandBuffer.SetStencilOpEXT(VulkanConverter.ConvertToStencilFace(faces), VulkanConverter.Convert(failOp), VulkanConverter.Convert(passOp), VulkanConverter.Convert(depthFailOp), VulkanConverter.Convert(compareOp));

		public void SetStencilReference(CullFace face, uint reference) => CommandBuffer.SetStencilCompareMask(VulkanConverter.ConvertToStencilFace(face), reference);

		public void SetStencilTestEnable(bool enabled) => CommandBuffer.SetStencilTestEnableEXT(enabled);

		public void SetStencilWriteMask(CullFace face, uint writeMask) => CommandBuffer.SetStencilCompareMask(VulkanConverter.ConvertToStencilFace(face), writeMask);

		public void SetSync(ISync dst, PipelineStage stage) => CommandBuffer.SetEvent(((VulkanEventSync)dst).Event, VulkanConverter.Convert(stage));

		public void SetVertexFormat(VertexFormat format) {
			Span<VKVertexInputBindingDescription2EXT> bindings = stackalloc VKVertexInputBindingDescription2EXT[format.Bindings.Count];
			Span<VKVertexInputAttributeDescription2EXT> attributes = stackalloc VKVertexInputAttributeDescription2EXT[format.Attributes.Count];
			int i = 0;
			var eb = format.Bindings.GetEnumerator();
			while (eb.MoveNext()) bindings[i++] = VulkanConverter.Convert(eb.Current);
			i = 0;
			var ea = format.Attributes.GetEnumerator();
			while (ea.MoveNext()) attributes[i++] = VulkanConverter.Convert(ea.Current);
			CommandBuffer.SetVertexInputEXT(bindings, attributes);
		}

		public void SetViewport(Viewport viewport, uint firstViewport = 0) => CommandBuffer.SetViewport(viewport, firstViewport);

		public void SetViewports(in ReadOnlySpan<Viewport> viewports, uint firstViewport = 0) {
			Span<VKViewport> views = stackalloc VKViewport[viewports.Length];
			for (int i = 0; i < views.Length; i++) views[i] = viewports[i];
			CommandBuffer.SetViewport(views, firstViewport);
		}

		public void SetViewports(uint firstViewport, params Viewport[] viewports) {
			Span<VKViewport> views = stackalloc VKViewport[viewports.Length];
			for (int i = 0; i < views.Length; i++) views[i] = viewports[i];
			CommandBuffer.SetViewport(views, firstViewport);
		}

		public void SetViewportsWithCount(in ReadOnlySpan<Viewport> viewports) {
			Span<VKViewport> views = stackalloc VKViewport[viewports.Length];
			for (int i = 0; i < views.Length; i++) views[i] = viewports[i];
			CommandBuffer.SetViewportWithCountEXT(views);
		}

		public void UpdateBuffer(IBuffer dst, nuint dstOffset, nuint dstSize, IntPtr pData) => CommandBuffer.UpdateBuffer(((VulkanBuffer)dst).Buffer, dstOffset, dstSize, pData);

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, in ReadOnlySpan<T> data) where T : unmanaged => CommandBuffer.UpdateBuffer(((VulkanBuffer)dst).Buffer, dstOffset, data);

		public void UpdateBuffer<T>(IBuffer dst, nuint dstOffset, params T[] data) where T : unmanaged => CommandBuffer.UpdateBuffer(((VulkanBuffer)dst).Buffer, dstOffset, data);

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, in ReadOnlySpan<ISync> syncs) {
			Span<VKEvent> events = new VKEvent[syncs.Length];
			for (int i = 0; i < events.Length; i++) events[i] = ((VulkanEventSync)syncs[i]).Event;

			Span<VKMemoryBarrier> mems = stackalloc VKMemoryBarrier[barriers.MemoryBarriers.Length];
			for (int i = 0; i < mems.Length; i++) mems[i] = VulkanConverter.Convert(barriers.MemoryBarriers[i]);
			Span<VKBufferMemoryBarrier> bufs = stackalloc VKBufferMemoryBarrier[barriers.BufferMemoryBarriers.Length];
			for (int i = 0; i < bufs.Length; i++) bufs[i] = VulkanConverter.Convert(barriers.BufferMemoryBarriers[i]);
			Span<VKImageMemoryBarrier> imgs = stackalloc VKImageMemoryBarrier[barriers.TextureMemoryBarriers.Length];
			for (int i = 0; i < imgs.Length; i++) imgs[i] = VulkanConverter.Convert(barriers.TextureMemoryBarriers[i]);

			CommandBuffer.WaitEvents(events, VulkanConverter.Convert(barriers.ProvokingStages), VulkanConverter.Convert(barriers.AwaitingStages), mems, bufs, imgs);
		}

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, params ISync[] syncs) {
			Span<VKEvent> events = new VKEvent[syncs.Length];
			for (int i = 0; i < events.Length; i++) events[i] = ((VulkanEventSync)syncs[i]).Event;

			Span<VKMemoryBarrier> mems = stackalloc VKMemoryBarrier[barriers.MemoryBarriers.Length];
			for (int i = 0; i < mems.Length; i++) mems[i] = VulkanConverter.Convert(barriers.MemoryBarriers[i]);
			Span<VKBufferMemoryBarrier> bufs = stackalloc VKBufferMemoryBarrier[barriers.BufferMemoryBarriers.Length];
			for (int i = 0; i < bufs.Length; i++) bufs[i] = VulkanConverter.Convert(barriers.BufferMemoryBarriers[i]);
			Span<VKImageMemoryBarrier> imgs = stackalloc VKImageMemoryBarrier[barriers.TextureMemoryBarriers.Length];
			for (int i = 0; i < imgs.Length; i++) imgs[i] = VulkanConverter.Convert(barriers.TextureMemoryBarriers[i]);

			CommandBuffer.WaitEvents(events, VulkanConverter.Convert(barriers.ProvokingStages), VulkanConverter.Convert(barriers.AwaitingStages), mems, bufs, imgs);
		}

		public void WaitSync(in ICommandSink.PipelineBarriers barriers, ISync sync) {
			VKEvent evt = ((VulkanEventSync)sync).Event;

			Span<VKMemoryBarrier> mems = stackalloc VKMemoryBarrier[barriers.MemoryBarriers.Length];
			for (int i = 0; i < mems.Length; i++) mems[i] = VulkanConverter.Convert(barriers.MemoryBarriers[i]);
			Span<VKBufferMemoryBarrier> bufs = stackalloc VKBufferMemoryBarrier[barriers.BufferMemoryBarriers.Length];
			for (int i = 0; i < bufs.Length; i++) bufs[i] = VulkanConverter.Convert(barriers.BufferMemoryBarriers[i]);
			Span<VKImageMemoryBarrier> imgs = stackalloc VKImageMemoryBarrier[barriers.TextureMemoryBarriers.Length];
			for (int i = 0; i < imgs.Length; i++) imgs[i] = VulkanConverter.Convert(barriers.TextureMemoryBarriers[i]);

			CommandBuffer.WaitEvents(evt, VulkanConverter.Convert(barriers.ProvokingStages), VulkanConverter.Convert(barriers.AwaitingStages), mems, bufs, imgs);
		}

		public void BeginRendering(in ICommandSink.RenderingInfo renderingInfo) {
			using MemoryStack sp = MemoryStack.Push();

			VKRenderingInfo vkInfo = new() {
				Type = VKStructureType.RenderingInfo,
				RenderArea = renderingInfo.RenderArea,
				LayerCount = 1,
				ViewMask = 0
			};

			if (renderingInfo.ColorAttachments != null) {
				var colors = renderingInfo.ColorAttachments;
				vkInfo.ColorAttachmentCount = (uint)colors.Length;
				vkInfo.ColorAttachments = sp.Values<VKRenderingAttachmentInfo>(colors.ConvertAll(info => VulkanConverter.Convert(info)));
			}
			if (renderingInfo.DepthAttachment != null)
				vkInfo.DepthAttachment = sp.Values(VulkanConverter.Convert(renderingInfo.DepthAttachment.Value));
			if (renderingInfo.StencilAttachment != null)
				vkInfo.StencilAttachment = sp.Values(VulkanConverter.Convert(renderingInfo.StencilAttachment.Value));

			CommandBuffer.BeginRendering(vkInfo);
		}

		public void EndRendering() => CommandBuffer.EndRendering();
	}

}
