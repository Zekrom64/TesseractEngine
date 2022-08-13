using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.Vulkan.Graphics.Impl {

	/// <summary>
	/// Vulkan vertex array implementation. This is just a data structure holding the format, index, and vertex buffers to bind.
	/// </summary>
	public class VulkanVertexArray : IVertexArray {

		/// <summary>
		/// The format of vertices in this vertex array.
		/// </summary>
		public VertexFormat Format { get; init; } = null!;

		public void Dispose() => GC.SuppressFinalize(this);

		/// <summary>
		/// The index buffer to bind, or null.
		/// </summary>
		public (VulkanBuffer, MemoryRange, VKIndexType)? IndexBuffer { get; init; }

		/// <summary>
		/// The list of vertex buffers to bind, or null.
		/// </summary>
		public (VulkanBuffer, MemoryRange, uint)[]? VertexBuffers { get; init; }

	}

	/// <summary>
	/// Vulkan sampler implementation.
	/// </summary>
	public class VulkanSampler : ISampler {

		/// <summary>
		/// The underlying Vulkan sampler.
		/// </summary>
		public VKSampler Sampler { get; }

		public VulkanSampler(VKSampler sampler) {
			Sampler = sampler;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Sampler.Dispose();
		}

	}

	/// <summary>
	/// Vulkan render pass implementation.
	/// </summary>
	public class VulkanRenderPass : IRenderPass {

		/// <summary>
		/// The underlying Vulkan render pass.
		/// </summary>
		public VKRenderPass RenderPass { get; }

		public VulkanRenderPass(VKRenderPass renderPass) {
			RenderPass = renderPass;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			RenderPass.Dispose();
		}

	}

	/// <summary>
	/// Vulkan framebuffer implementation.
	/// </summary>
	public class VulkanFramebuffer : IFramebuffer {

		/// <summary>
		/// The underlying Vulkan framebuffer.
		/// </summary>
		public VKFramebuffer Framebuffer { get; }

		public Vector2i Size { get; }

		public uint Layers { get; }

		/// <summary>
		/// The list of texture views that are used as attachments.
		/// </summary>
		public VulkanTextureView[] Attachments { get; }

		public VulkanFramebuffer(VKFramebuffer framebuffer, Vector2i size, uint layers, VulkanTextureView[] attachments) {
			Framebuffer = framebuffer;
			Size = size;
			Layers = layers;
			Attachments = attachments;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Framebuffer.Dispose();
		}

	}

	/// <summary>
	/// Vulkan shader module implementation.
	/// </summary>
	public class VulkanShader : IShader {

		/// <summary>
		/// The underlying Vulkan shader module.
		/// </summary>
		public VKShaderModule ShaderModule { get; }

		public VulkanShader(VKShaderModule module) {
			ShaderModule = module;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ShaderModule.Dispose();
		}

		public bool TryFindBinding(string name, out BindSetLayoutBinding binding) {
			// Not supported by Vulkan
			binding = default;
			return false;
		}

	}

	/// <summary>
	/// Vulkan bind set layout implementation.
	/// </summary>
	public class VulkanBindSetLayout : IBindSetLayout {

		/// <summary>
		/// The underlying Vulkan descriptor set layout
		/// </summary>
		public VKDescriptorSetLayout Layout { get; }

		public IReadOnlyList<BindSetLayoutBinding> Bindings { get; }

		public VulkanBindSetLayout(VKDescriptorSetLayout layout, BindSetLayoutCreateInfo createInfo) {
			Layout = layout;
			Bindings = new List<BindSetLayoutBinding>(createInfo.Bindings);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Layout.Dispose();
		}

	}

	/// <summary>
	/// Vulkan bind pool implementation
	/// </summary>
	public class VulkanBindPool : IBindPool {

		// Counts the number of bind types required for the given allocation
		private static Dictionary<BindType,int> CountBindTypes(BindSetAllocateInfo allocateInfo) {
			Dictionary<BindType, int> counts = new();
			foreach(var layout in allocateInfo.Layouts) {
				foreach(var binding in layout.Bindings) {
					int value = counts.GetValueOrDefault(binding.Type, 0);
					value++;
					counts[binding.Type] = value;
				}
			}
			return counts;
		}

		// Bind type info structure
		private struct BindTypeInfo {

			// The type of binding
			public BindType Type;

			// The maximum number of available bindings
			public int Max;

			// The current number of bindings
			public int Count;

		}

		// A slice stores binding information for a single Vulkan descriptor pool
		private class Slice : IDisposable {

			// The underlying Vulkan descriptor pool
			public VKDescriptorPool Pool { get; }

			// The list of bind info for this pool
			private readonly BindTypeInfo[] bindInfos;

			private int setCount = 0;
			private readonly int maxSets;

			// Attempts to allocate a descriptor set for the given bind set allocation info
			public bool TryAlloc(BindSetAllocateInfo allocInfo, (BindType, int)[] typeCounts, [NotNullWhen(true)] out VKDescriptorSet? set) {
				set = null;
				lock(Pool) {
					// If no more sets in the pool cannot alloc
					if (setCount >= maxSets) return false;
					// Check if pool has enough of required descriptor types remaining
					foreach(var typeCount in typeCounts) {
						bool hasCount = false;
						foreach(var info in bindInfos) {
							if (info.Type == typeCount.Item1) {
								hasCount = true;
								int rem = info.Max - info.Count;
								if (rem < typeCount.Item2) return false;
								break;
							}
						}
						if (!hasCount) return false;
					}
					using MemoryStack sp = MemoryStack.Push();
					// Allocate descriptor set
					set = Pool.Allocate(new VKDescriptorSetAllocateInfo() {
						Type = VKStructureType.DescriptorSetLayoutCreateInfo,
						DescriptorPool = Pool,
						DescriptorSetCount = 1,
						SetLayouts = sp.Values(allocInfo.Layouts.ConvertAll(layout => ((VulkanBindSetLayout)layout).Layout))
					})[0];
					// Increment descriptor type counts
					foreach(var typeCount in typeCounts) {
						for(int i = 0; i < bindInfos.Length; i++) {
							ref BindTypeInfo info = ref bindInfos[i];
							if (info.Type == typeCount.Item1) info.Count += typeCount.Item2;
						}
					}
					// Increment set count
					setCount++;
				}
				return true;
			}

			public Slice(VulkanDevice device, BindTypeInfo[] baseInfo, int nTargets, BindSetAllocateInfo allocateInfo) {
				using MemoryStack sp = MemoryStack.Push();
				List<BindTypeInfo> infos = new(baseInfo);
				Dictionary<BindType, int> allocCounts = CountBindTypes(allocateInfo);
				foreach(var reqAlloc in allocCounts) {
					bool hasInfo = false;
					for(int i = 0; i < infos.Count; i++) {
						var info = infos[i];
						if (info.Type == reqAlloc.Key) {
							if (info.Max < reqAlloc.Value) {
								info.Max = reqAlloc.Value;
								infos[i] = info;
							}
							hasInfo = true;
							break;
						}
					}
					if (!hasInfo) infos.Add(new BindTypeInfo() { Type = reqAlloc.Key, Max = reqAlloc.Value });
				}

				bindInfos = infos.ToArray();
				Pool = device.Device.CreateDescriptorPool(new VKDescriptorPoolCreateInfo() {
					Type = VKStructureType.DescriptorPoolCreateInfo,
					MaxSets = (uint)nTargets,
					PoolSizeCount = (uint)infos.Count,
					PoolSizes = sp.Values(infos.ConvertAll(info => new VKDescriptorPoolSize() {
						Type = VulkanConverter.Convert(info.Type),
						DescriptorCount = (uint)info.Max
					}))
				});
				maxSets = nTargets;
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				Pool.Dispose();
			}

			// Frees a bind set
			public void Free(VulkanBindSet set) {
				lock(Pool) {
					foreach(var typeCount in set.TypeCounts) {
						for(int i = 0; i < bindInfos.Length; i++) {
							if (bindInfos[i].Type == typeCount.Item1) {
								bindInfos[i].Count -= typeCount.Item2;
								break;
							}
						}
					}
					set.Set.Dispose();
					setCount--;
				}
			}

		}

		// The list of all loaded slices
		private readonly List<Slice> slices = new();
		// A reader-writer lock for the slice list
		private readonly ReaderWriterLockSlim lockSlices = new();
		// The device this bind pool allocates from
		private readonly VulkanDevice device;
		// The base bind type info
		private readonly BindTypeInfo[] baseInfo;
		// The number of target sets for this pool
		private readonly int nTargets;

		public VulkanBindPool(VulkanDevice device, BindPoolCreateInfo createInfo) {
			this.device = device;
			// Count the total number of bindings to allocate for different types based on the binding type weights and the target pool size
			Dictionary<BindType, int> typeCounts = new();
			foreach (var weight in createInfo.BindTypeWeights) typeCounts[weight.Item1] = (int)MathF.Ceiling(weight.Item2 * createInfo.TargetPoolSize);
			baseInfo = typeCounts.ToArray().ConvertAll(bindcount => new BindTypeInfo() { Type = bindcount.Key, Max = bindcount.Value });
			nTargets = createInfo.TargetPoolSize;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			lockSlices.Dispose();
			foreach (Slice slice in slices) slice.Dispose();
		}

		public IBindSet AllocSet(BindSetAllocateInfo allocateInfo) {
			(BindType, int)[] typeCounts = CountBindTypes(allocateInfo).ToArray().ConvertAll(pair => (pair.Key, pair.Value));
			// Lock list of slices as an upgradeable reader lock first
			lockSlices.EnterUpgradeableReadLock();
			try {
				// Try to allocate from an existing slice first
				foreach(Slice slice in slices) {
					if (slice.TryAlloc(allocateInfo, typeCounts, out VKDescriptorSet? set)) return new VulkanBindSet(this, set, typeCounts);
				}
				// We need to add a new slice, so upgrade to a writer
				lockSlices.EnterWriteLock();
				try {
					// Create a new slice and allocate from it
					Slice slice = new(device, baseInfo, nTargets, allocateInfo);
					slices.Add(slice);
					if (!slice.TryAlloc(allocateInfo, typeCounts, out VKDescriptorSet? set)) throw new InvalidOperationException("Failed to allocate descriptor set");
					return new VulkanBindSet(this, set, typeCounts);
				} finally {
					lockSlices.ExitWriteLock();
				}
			} finally {
				lockSlices.ExitUpgradeableReadLock();
			}
		}

		public void Free(VulkanBindSet set) {
			lockSlices.EnterReadLock();
			try {
				foreach(Slice slice in slices) {
					if (slice.Pool == set.Set.DescriptorPool) slice.Free(set);
				}
			} finally {
				lockSlices.ExitReadLock();
			}
			set.Dispose();
		}

	}

	/// <summary>
	/// Vulkan bind set implementation.
	/// </summary>
	public class VulkanBindSet : IBindSet {

		/// <summary>
		/// The bind pool this set was allocated from.
		/// </summary>
		public VulkanBindPool Pool { get; }

		/// <summary>
		/// The underlying Vulkan descriptor set.
		/// </summary>
		public VKDescriptorSet Set { get; }

		/// <summary>
		/// The list of allocation counts by their binding type.
		/// </summary>
		public (BindType, int)[] TypeCounts { get; }

		public VulkanBindSet(VulkanBindPool pool, VKDescriptorSet set, (BindType, int)[] typeCounts) {
			Pool = pool;
			Set = set;
			TypeCounts = typeCounts;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Pool.Free(this);
		}

		public void Update(in ReadOnlySpan<BindSetWrite> writes) {
			using MemoryStack sp = MemoryStack.Push();
			Span<VKWriteDescriptorSet> vkwrites = stackalloc VKWriteDescriptorSet[writes.Length];
			for (int i = 0; i < vkwrites.Length; i++) vkwrites[i] = VulkanConverter.Convert(sp, writes[i], Set);
			Set.Device.UpdateDescriptorSets(vkwrites, stackalloc VKCopyDescriptorSet[0]);
		}
	}

}
