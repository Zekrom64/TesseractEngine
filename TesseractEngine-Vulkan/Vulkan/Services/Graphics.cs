using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;
using Tesseract.Vulkan.Services.Objects;

namespace Tesseract.Vulkan.Services {

	/// <summary>
	/// Vulkan graphics implementation.
	/// </summary>
	public class VulkanGraphics : IGraphics {

		/// <summary>
		/// The provider this graphics context was created from.
		/// </summary>
		public VulkanGraphicsProvider Provider { get; }

		/// <summary>
		/// The Vulkan logical device this context uses.
		/// </summary>
		public VulkanDevice Device { get; }

		/// <summary>
		/// The Vulkan memory manager this context uses.
		/// </summary>
		public VulkanMemory Memory { get; }

		/// <summary>
		/// The Vulkan command manager this context uses.
		/// </summary>
		public VulkanCommands Commands { get; }

		public IGraphicsProperites Properties { get; }

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits { get; }

		public IBuffer CreateBuffer(BufferCreateInfo createInfo) {
			VKBufferUsageFlagBits usage = 0;
			if ((createInfo.Usage & BufferUsage.VertexBuffer) != 0) usage |= VKBufferUsageFlagBits.VertexBuffer;
			if ((createInfo.Usage & BufferUsage.IndexBuffer) != 0) usage |= VKBufferUsageFlagBits.IndexBuffer;

			VKBuffer buffer = Device.Device.CreateBuffer(new VKBufferCreateInfo() {
				Type = VKStructureType.BufferCreateInfo,
				Size = createInfo.Size,
				Usage = usage,
				SharingMode = Device.ResourceSharingMode,
				QueueFamilyIndexCount = (uint)Device.ResourceSharingIndices.ArraySize,
				QueueFamilyIndices = Device.ResourceSharingIndices
			});

			return new VulkanBuffer(this, buffer, createInfo);
		}

		public IVertexArray CreateVertexArray(VertexArrayCreateInfo createInfo) {
			return new VulkanVertexArray() {
				Format = createInfo.Format,
				IndexBuffer = createInfo.IndexBuffer != null ? (
					(VulkanBuffer)createInfo.IndexBuffer.Value.Item1.Buffer,
					createInfo.IndexBuffer.Value.Item1.Range,
					VulkanConverter.Convert(createInfo.IndexBuffer.Value.Item2)
				) : null,
				VertexBuffers = createInfo.VertexBuffers != null ? Array.ConvertAll(createInfo.VertexBuffers, binding => (
					(VulkanBuffer)binding.Item1.Buffer,
					binding.Item1.Range, binding.Item2
				)) : null
			};
		}

		public IFramebuffer CreateFramebuffer(FramebufferCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			UnmanagedPointer<ulong> pAttachments = sp.Alloc<ulong>(createInfo.Attachments.Length);
			for (int i = 0; i < pAttachments.ArraySize; i++) pAttachments[i] = ((VulkanTextureView)createInfo.Attachments[i]).ImageView;
			return new VulkanFramebuffer(Device.Device.CreateFramebuffer(new VKFramebufferCreateInfo() {
				Type = VKStructureType.FramebufferCreateInfo,
				RenderPass = ((VulkanRenderPass)createInfo.RenderPass).RenderPass,
				AttachmentCount = (uint)pAttachments.ArraySize,
				Attachments = pAttachments,
				Width = (uint)createInfo.Size.X,
				Height = (uint)createInfo.Size.Y,
				Layers = createInfo.Layers
			}), createInfo.Size, createInfo.Layers, createInfo.Attachments.ConvertAll(item => item as VulkanTextureView)!);
		}

		public IPipeline CreatePipeline(PipelineCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			VKPipelineCache? vkcache = (createInfo.Cache is VulkanPipelineCache cache) ? cache.PipelineCache : null;
			List<IDisposable> disposables = new();
			try {
				if (createInfo.GraphicsInfo != null) return new VulkanPipeline(
					Device.Device.CreateGraphicsPipelines(vkcache, VulkanConverter.ConvertGraphicsPipeline(sp, createInfo, disposables)),
					VKPipelineBindPoint.Graphics
				);
				else if (createInfo.ComputeInfo != null) return new VulkanPipeline(
					Device.Device.CreateComputePipeline(vkcache, VulkanConverter.ConvertComputePipeline(createInfo)),
					VKPipelineBindPoint.Compute
				);
				else throw new VulkanException("Cannot determine type of pipeline to create");
			} finally {
				foreach (IDisposable d in disposables) d.Dispose();
			}
		}

		public IPipelineCache CreatePipelineCache(PipelineCacheCreateInfo createInfo) {
			byte[]? initData = createInfo.InitialData;
			unsafe {
				fixed (byte* pInitData = initData) {
					return new VulkanPipelineCache(Device.Device.CreatePipelineCache(new VKPipelineCacheCreateInfo() {
						Type = VKStructureType.PipelineCacheCreateInfo,
						InitialDataSize = (nuint)(initData != null ? initData.Length : 0),
						InitialData = (IntPtr)pInitData
					}));
				}
			}
		}

		public IPipelineLayout CreatePipelineLayout(PipelineLayoutCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanPipelineLayout(Device.Device.CreatePipelineLayout(new VKPipelineLayoutCreateInfo() {
				Type = VKStructureType.PipelineLayoutCreateInfo,
				SetLayoutCount = (uint)createInfo.Layouts.Count,
				SetLayouts = sp.Values(createInfo.Layouts.ConvertAll(layout => ((VulkanBindSetLayout)layout).Layout.DescriptorSetLayout)),
				PushConstantRangeCount = (uint)(createInfo.PushConstantRanges?.Count).GetValueOrDefault(0),
				PushConstantRanges = createInfo.PushConstantRanges != null ? sp.Values(createInfo.PushConstantRanges.ConvertAll(VulkanConverter.Convert)) : IntPtr.Zero
			}));
		}

		public IBindSetLayout CreateBindSetLayout(BindSetLayoutCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanBindSetLayout(Device.Device.CreateDescriptorSetLayout(new VKDescriptorSetLayoutCreateInfo() {
				Type = VKStructureType.DescriptorSetLayoutCreateInfo,
				BindingCount = (uint)createInfo.Bindings.Length,
				Bindings = sp.Values(createInfo.Bindings.ConvertAll(VulkanConverter.Convert))
			}), createInfo);
		}

		public IBindPool CreateBindPool(BindPoolCreateInfo createInfo) => new VulkanBindPool(Device, createInfo);

		public IPipelineSet CreatePipelineSet(PipelineSetCreateInfo createInfo) => new VulkanPipelineSet(this, createInfo);

		public IRenderPass CreateRenderPass(RenderPassCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			return new VulkanRenderPass(Device.Device.CreateRenderPass(VulkanConverter.Convert(sp, createInfo)));
		}

		public ISampler CreateSampler(SamplerCreateInfo createInfo) {
			using MemoryStack sp = MemoryStack.Push();
			IntPtr next = IntPtr.Zero;
			if (createInfo.BorderColor == SamplerBorderColor.CustomNorm) {
				if (!Device.Device.EXTCustomBorderColor)
					throw new VulkanException("Cannot define sampler with custom border color without VK_EXT_custom_border_color");
				if (!Device.PhysicalDevice.CustomBorderColorFeaturesEXT!.Value.CustomBorderColors)
					throw new VulkanException("Cannot define sampler with custom border color when feature is not available");
				next = sp.Values(new VKSamplerCustomBorderColorCreateInfoEXT() {
					Type = VKStructureType.SamplerCustomBorderColorCreateInfoEXT,
					Next = next,
					CustomBorderColor = VulkanConverter.ConvertClearColor(createInfo.CustomBorderColor!),
					Format = VulkanConverter.Convert(createInfo.CustomBorderColorFormat!)
				});
			}
			return new VulkanSampler(Device.Device.CreateSampler(new VKSamplerCreateInfo() {
				Type = VKStructureType.SamplerCreateInfo,
				Next = next,
				MagFilter = VulkanConverter.ConvertFilter(createInfo.MagnifyFilter),
				MinFilter = VulkanConverter.ConvertFilter(createInfo.MinifyFilter),
				MipmapMode = VulkanConverter.ConvertMipmapMode(createInfo.MipmapMode),
				AddressModeU = VulkanConverter.Convert(createInfo.AddressMode.X),
				AddressModeV = VulkanConverter.Convert(createInfo.AddressMode.Y),
				AddressModeW = VulkanConverter.Convert(createInfo.AddressMode.Z),
				MipLodBias = createInfo.MipLODBias,
				AnisotropyEnable = createInfo.AnisotropyEnable,
				MaxAnisotropy = createInfo.MaxAnisotropy,
				CompareEnable = createInfo.CompareEnable,
				CompareOp = VulkanConverter.Convert(createInfo.CompareOp),
				MinLod = createInfo.LODRange.Item1,
				MaxLod = createInfo.LODRange.Item2,
				BorderColor = VulkanConverter.Convert(createInfo.BorderColor),
				UnnormalizedCoordinates = false
			}));
		}

		public IShader CreateShader(ShaderCreateInfo createInfo) {
			if (createInfo.SourceType != ShaderSourceType.SPIRV) throw new VulkanException("Vulkan graphics only supports SPIR-V shader sources");

			ReadOnlyMemory<int> spirv = default;
			IConstPointer<int>? pspirv = default;
			if (createInfo.Source is int[] arr) spirv = arr;
			else if (createInfo.Source is IReadOnlyList<int> lst) spirv = lst.ToArray();
			else if (createInfo.Source is ReadOnlyMemory<int> mem) spirv = mem;
			else if (createInfo.Source is IConstPointer<int> ptr) {
				pspirv = ptr;
				if (pspirv.ArraySize < 0) throw new ArgumentException("Pointer SPIR-V source type must have explicit length", nameof(createInfo));
			} else throw new ArgumentException("Supplied shader source is not a valid SPIR-V source type", nameof(createInfo));

			unsafe {
				fixed(int* pSpirv = spirv.Span) {
					IntPtr pCode = (IntPtr)pSpirv;
					int length = spirv.Length;
					if (pspirv != null) {
						pCode = pspirv.Ptr;
						length = pspirv.ArraySize;
					}

					return new VulkanShader(Device.Device.CreateShaderModule(new VKShaderModuleCreateInfo() {
						Type = VKStructureType.ShaderModuleCreateInfo,
						Code = pCode,
						CodeSize = (nuint)length
					}));
				}
			}

		}

		public ISync CreateSync(SyncCreateInfo createInfo) {
			bool OnlyHasFeatures(SyncFeatures features) => (createInfo.Features & ~features) == 0;
			bool HasCompatibleGranularity(SyncGranularity granularity) => granularity switch {
				SyncGranularity.CommandBuffer => createInfo.Granularity == SyncGranularity.CommandBuffer,
				SyncGranularity.Command => createInfo.Granularity != SyncGranularity.PipelineStage,
				SyncGranularity.PipelineStage => true,
				_ => false,
			};
			switch (createInfo.Direction) {
				case SyncDirection.GPUToHost: // Fence
					if (!OnlyHasFeatures(SyncFeatures.GPUWorkSignaling | SyncFeatures.HostPolling | SyncFeatures.HostWaiting)) break;
					if (!HasCompatibleGranularity(SyncGranularity.CommandBuffer)) break;
					return new VulkanFenceSync(Device.Device.CreateFence(new VKFenceCreateInfo() {
						Type = VKStructureType.FenceCreateInfo
					}));
				case SyncDirection.GPUToGPU: // Semaphore
					if (!OnlyHasFeatures(SyncFeatures.GPUWorkSignaling | SyncFeatures.GPUWorkWaiting | SyncFeatures.GPUMultiQueue)) break;
					if (!HasCompatibleGranularity(SyncGranularity.CommandBuffer)) break;
					return new VulkanSemaphoreSync(Device.Device.CreateSemaphore(new VKSemaphoreCreateInfo() {
						Type = VKStructureType.SemaphoreCreateInfo
					}));
				case SyncDirection.Any: // Event
					if (!OnlyHasFeatures(SyncFeatures.GPUSignaling | SyncFeatures.GPUWaiting | SyncFeatures.HostPolling | SyncFeatures.HostSignaling | SyncFeatures.HostWaiting)) break;
					if (!HasCompatibleGranularity(SyncGranularity.PipelineStage)) break;
					return new VulkanEventSync(Device.Device.CreateEvent(new VKEventCreateInfo() {
						Type = VKStructureType.EventCreateInfo
					}));
				default:
					break;
			}
			throw new ArgumentException("Unsupported combination of sync creation information", nameof(createInfo));
		}

		public ITexture CreateTexture(TextureCreateInfo createInfo) {
			VKImage image = Device.Device.CreateImage(new VKImageCreateInfo() {
				Type = VKStructureType.ImageCreateInfo,
				Flags = VulkanConverter.ConvertImageCreateFlags(createInfo.Type),
				ImageType = VulkanConverter.ConvertImageType(createInfo.Type),
				Format = VulkanConverter.Convert(createInfo.Format),
				Extent = (Vector3ui)createInfo.Size,
				MipLevels = createInfo.MipLevels,
				ArrayLayers = createInfo.ArrayLayers,
				Samples = VulkanConverter.ConvertSampleCount(createInfo.Samples),
				Tiling = VKImageTiling.Optimal,
				Usage = VulkanConverter.Convert(createInfo.Usage),
				SharingMode = Device.ResourceSharingMode,
				QueueFamilyIndexCount = (uint)Device.ResourceSharingIndices.ArraySize,
				QueueFamilyIndices = Device.ResourceSharingIndices,
				InitialLayout = VKImageLayout.Undefined
			});

			VulkanMemoryBinding binding;
			if (createInfo.MemoryBinding is VulkanMemoryBinding b) binding = b;
			else binding = Memory.Allocate(createInfo, image);

			binding.Bind(image);

			return new VulkanTexture(image, true) {
				Type = createInfo.Type,
				Format = createInfo.Format,
				Size = createInfo.Size,
				MipLevels = createInfo.MipLevels,
				ArrayLayers = createInfo.ArrayLayers,
				Samples = createInfo.Samples,
				Usage = createInfo.Usage,
				MemoryBinding = binding
			};
		}

		public ITextureView CreateTextureView(TextureViewCreateInfo createInfo) {
			return new VulkanTextureView(Device.Device.CreateImageView(new VKImageViewCreateInfo() {
				Type = VKStructureType.ImageViewCreateInfo,
				Image = ((VulkanTexture)createInfo.Texture).Image,
				ViewType = VulkanConverter.ConvertImageViewType(createInfo.Type),
				Format = VulkanConverter.Convert(createInfo.Format),
				Components = VulkanConverter.Convert(createInfo.Mapping),
				SubresourceRange = VulkanConverter.Convert(createInfo.SubresourceRange)
			}), createInfo);
		}

		public ICommandBuffer CreateCommandBuffer(CommandBufferCreateInfo createInfo) => Commands.Alloc(createInfo);

		public void RunCommands(Action<ICommandSink> cmdSink, CommandBufferUsage usage, in IGraphics.CommandBufferSubmitInfo submitInfo) {
			// Allocate and record command buffer
			usage |= CommandBufferUsage.OneTimeSubmit;
			VulkanCommandBuffer cmdbuf = Commands.Alloc(new CommandBufferCreateInfo() {
				Type = CommandBufferType.Primary,
				Usage = usage
			});
			cmdSink(cmdbuf.BeginRecording());
			cmdbuf.EndRecording();
			
			// Determine if a fence already exists
			VulkanFenceSync? fence = null;
			foreach(ISync sig in submitInfo.SignalSync) {
				if (sig is VulkanFenceSync fenceSync) {
					if (fence != null) throw new VulkanException("Can only signal a single fence during command submission");
					else fence = fenceSync;
				}
			}

			// If no fence, create a new one and add to the list of signal syncs
			ISync[] signalSyncs = submitInfo.SignalSync.ToArray();

			bool disposeFence = fence == null;
			if (disposeFence) {
				fence = new VulkanFenceSync(Device.Device.CreateFence(new VKFenceCreateInfo() {
					Type = VKStructureType.FenceCreateInfo
				}));
				Array.Resize(ref signalSyncs, signalSyncs.Length + 1);
				signalSyncs[^1] = fence;
			}

			// Make sure the fence is reset and submit the commands
			fence!.HostReset();
			var submitInfo2 = new IGraphics.CommandBufferSubmitInfo() {
				CommandBuffer = new ICommandBuffer[] { cmdbuf },
				SignalSync = submitInfo.SignalSync,
				WaitSync = submitInfo.WaitSync
			};
			SubmitCommands(submitInfo2);

			// Schedule the command buffer to be disposed when the fence indicates it is free
			Commands.DisposeWhenFree(cmdbuf, fence, disposeFence);
		}

		public void SubmitCommands(in IGraphics.CommandBufferSubmitInfo submitInfo) {
			VulkanCommands.CommandBank? cmdbank = null;
			ulong queueid = 0;
			Span<IntPtr> cmds = stackalloc IntPtr[submitInfo.CommandBuffer.Count];
			for (int i = 0; i < cmds.Length; i++) {
				var vkcmd = ((VulkanCommandBuffer)submitInfo.CommandBuffer[i]);
				cmds[i] = vkcmd.CommandBuffer.CommandBuffer;
				if (cmdbank == null) {
					cmdbank = vkcmd.CommandPool.Bank;
					queueid = vkcmd.QueueID;
				} else if (queueid != vkcmd.QueueID) throw new VulkanException("Cannot submit command buffers which target different queues");
			}
			if (cmdbank == null) return;

			VulkanFenceSync? fence = null;
			int sigcount = 0, waitcount = 0;
			Span<ulong> sigs = stackalloc ulong[submitInfo.SignalSync.Count];
			Span<ulong> waits = stackalloc ulong[submitInfo.WaitSync.Count];
			Span<VKPipelineStageFlagBits> waitStages = stackalloc VKPipelineStageFlagBits[waits.Length];

			// Parse the signal sync list
			foreach(ISync sync in submitInfo.SignalSync) {
				if (sync is VulkanFenceSync fsync) {
					if (fence != null) throw new VulkanException("Cannot submit commands while signaling more than one fence");
					else fence = fsync;
				} else if (sync is VulkanSemaphoreSync ssync) {
					sigs[sigcount++] = ssync.Semaphore.Semaphore;
				} else throw new VulkanException("Unsupported command submit sync object in signaling list");
			}

			// Parse the wait sync list
			foreach(var wait in submitInfo.WaitSync) {
				if (wait.Item1 is VulkanSemaphoreSync ssync) {
					waits[waitcount++] = ssync.Semaphore.Semaphore;
				} else throw new VulkanException("Unsupported command submit sync object in waiting list");
			}

			Commands.Submit(cmdbank, cmds, waits, waitStages, sigs, fence?.Fence);
		}

		public void TrimCommandBufferMemory() => Commands.Trim();

		public void WaitIdle() => Commands.WaitIdle();

		public VulkanGraphics(VulkanGraphicsProvider provider, VulkanDevice device) {
			Provider = provider;
			Device = device;
			Memory = new VulkanMemory(Device, ((VulkanGraphicsProperties)provider.Properties).Memory);
			int poolParallelism = Environment.ProcessorCount, gcThreshold = 100;
			var exinfo = provider.Enumerator.ExtendedInfo;
			if (exinfo != null) {
				if (exinfo.CommandPoolParallelism > 0) poolParallelism = exinfo.CommandPoolParallelism;
				if (exinfo.CommandBufferGCThreshold > 0) gcThreshold = exinfo.CommandBufferGCThreshold;
			}
			Commands = new VulkanCommands(Device, poolParallelism, gcThreshold);

			Properties = new VulkanGraphicsProperties(Device.PhysicalDevice, Memory);
			Features = new VulkanGraphicsFeatures(Device.PhysicalDevice, Device);
			Limits = new VulkanGraphicsLimits(Device.PhysicalDevice);
		}

	}

}
