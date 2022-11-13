using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.Vulkan.Graphics.Impl {

	/// <summary>
	/// Vulkan memory binding interface.
	/// </summary>
	public interface IVKMemoryBinding : IMemoryBinding, IDisposable {

		/// <summary>
		/// The underlying device memory for the binding.
		/// </summary>
		public VKDeviceMemory DeviceMemory { get; }

		/// <summary>
		/// The size of the memory binding.
		/// </summary>
		public ulong Size { get; }

		/// <summary>
		/// The supported memory mapping flags.
		/// </summary>
		public MemoryMapFlags SupportedMapFlags { get; }

		/// <summary>
		/// Binds this memory to a buffer.
		/// </summary>
		/// <param name="buffer">Buffer to bind to</param>
		public void Bind(VKBuffer buffer);

		/// <summary>
		/// Binds this memory to an image.
		/// </summary>
		/// <param name="image">Iamge to bind to</param>
		public void Bind(VKImage image);

		/// <summary>
		/// Maps the memory from this binding.
		/// </summary>
		/// <returns>Mapped memory pointer</returns>
		public IntPtr Map();

		/// <summary>
		/// Unmaps memory from this binding.
		/// </summary>
		public void Unmap();

		/// <summary>
		/// Flushes the memory for this binding.
		/// </summary>
		/// <param name="offset">Offset to flush at</param>
		/// <param name="length">Number of bytes to flush</param>
		public void Flush(ulong offset, ulong length);

		/// <summary>
		/// Invalidates the memory for this binding.
		/// </summary>
		/// <param name="offset">Offset to invalidate at</param>
		/// <param name="length">Number of bytes to invalidate</param>
		public void Invalidate(ulong offset, ulong length);

	}

	/// <summary>
	/// Internally used Vulkan memory binding.
	/// </summary>
	public class VulkanMemoryBinding : IVKMemoryBinding {

		/// <summary>
		/// The memory manager for this binding.
		/// </summary>
		public required VulkanMemory MemoryManager { get; init; }

		/// <summary>
		/// The underlying VMA allocation for this binding.
		/// </summary>
		public required VMAAllocation Allocation { get; init; }

		public required VKDeviceMemory DeviceMemory { get; init; }

		public ulong Size => Allocation.Info.Size;

		public MemoryMapFlags SupportedMapFlags { get; init; }

		public void Bind(VKBuffer buffer) => Allocation.BindBufferMemory(buffer);

		public void Bind(VKImage image) => Allocation.BindImageMemory(image);

		public void Dispose() {
			GC.SuppressFinalize(this);
			Allocation.Dispose();
		}

		public IntPtr Map() => Allocation.MapMemory();

		public void Unmap() => Allocation.UnmapMemory();

		public void Flush(ulong offset, ulong length) => Allocation.Flush(offset, length);

		public void Invalidate(ulong offset, ulong length) => Allocation.Invalidate(offset, length);

	}

	/// <summary>
	/// Internal Vulkan memory manager.
	/// </summary>
	public class VulkanMemory : IDisposable, IVulkanMemory {

		public ulong TotalDeviceMemory { get; }

		public ulong TotalVideoMemory { get; }

		public ulong TotalCommittedMemory => allocator.Budget.AllocationBytes;

		/// <summary>
		/// The device this memory manager will allocate from.
		/// </summary>
		public VulkanDevice Device { get; }

		private readonly VMAAllocator allocator;

		public VulkanMemory(VulkanDevice device, IVulkanMemory deviceMemory) {
			Device = device;
			TotalDeviceMemory = deviceMemory.TotalDeviceMemory;
			TotalVideoMemory = deviceMemory.TotalVideoMemory;

			// Create allocator
			allocator = VMA.CreateAllocator(new VMAAllocatorCreateInfo() {
				PhysicalDevice = device.PhysicalDevice.PhysicalDevice,
				Device = device.Device,
				Instance = device.Device.Instance,
				VulkanApiVersion = device.Device.Instance.APIVersion
			}, device.Device);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			allocator.Dispose();
		}

		/// <summary>
		/// Allocates a memory binding for a buffer.
		/// </summary>
		/// <param name="createInfo">The buffer's creation information</param>
		/// <param name="buffer">The buffer to allocate memory for</param>
		/// <returns>The allocated memory binding</returns>
		public VulkanMemoryBinding Allocate(in BufferCreateInfo createInfo, VKBuffer buffer) {
			VKMemoryPropertyFlagBits reqProps = 0;
			MemoryMapFlags mapReqs = createInfo.MapFlags;
			// Determine required properties from map flags
			if ((mapReqs & MemoryMapFlags.ReadWrite) != 0) reqProps |= VKMemoryPropertyFlagBits.HostVisible;
			if ((mapReqs & MemoryMapFlags.Coherent) != 0) reqProps |= VKMemoryPropertyFlagBits.HostCoherent;
			VKMemoryPropertyFlagBits prefProps = reqProps;

			// Determine memory usage
			VMAMemoryUsage usage = VMAMemoryUsage.GPUOnly; // Assume GPU-only first
			bool rd = (mapReqs & MemoryMapFlags.Read) != 0, wr = (mapReqs & MemoryMapFlags.Write) != 0;
			if (rd && !wr) usage = VMAMemoryUsage.GPUToCPU; // Detect CPU <-> GPU based on read/writablility
			else if (wr && !rd) usage = VMAMemoryUsage.CPUToGPU;
			else if (wr && rd) usage = VMAMemoryUsage.Unknown; // If read & write we don't know

			// Prefer device-local if GPU-only
			if (usage == VMAMemoryUsage.GPUOnly) prefProps |= VKMemoryPropertyFlagBits.DeviceLocal;

			// Allocate memory
			VMAAllocation alloc = allocator.AllocateMemoryForBuffer(buffer, new VMAAllocationCreateInfo() {
				RequiredFlags = reqProps,
				PreferredFlags = prefProps,
				Usage = usage
			}, out VMAAllocationInfo allocInfo);

			var memType = Device.PhysicalDevice.MemoryProperties.MemoryTypes[(int)allocInfo.MemoryType];
			var memFlags = memType.PropertyFlags;

			MemoryMapFlags mapFlags = MemoryMapFlags.Persistent; // Persistent mapping is ok in Vulkan
			if ((memFlags & VKMemoryPropertyFlagBits.HostVisible) != 0) mapFlags |= MemoryMapFlags.ReadWrite;
			if ((memFlags & VKMemoryPropertyFlagBits.HostCoherent) != 0) mapFlags |= MemoryMapFlags.Coherent;

			VKDeviceMemory deviceMemory = new(Device.Device, allocInfo.DeviceMemory, null);

			// Return binding
			return new VulkanMemoryBinding() {
				MemoryManager = this,
				DeviceMemory = deviceMemory,
				Allocation = alloc,
				SupportedMapFlags = mapFlags
			};
		}

		/// <summary>
		/// Allocates a memory binding for an image.
		/// </summary>
		/// <param name="createInfo">The image's creation information</param>
		/// <param name="image">The image to allocate memory for</param>
		/// <returns>The allocated memory binding</returns>
		public VulkanMemoryBinding Allocate(in TextureCreateInfo createInfo, VKImage image) {
			VKMemoryPropertyFlagBits reqProps = 0;
			VKMemoryPropertyFlagBits prefProps = VKMemoryPropertyFlagBits.DeviceLocal;
			VMAMemoryUsage usage = VMAMemoryUsage.GPUOnly;

			if ((createInfo.Usage & TextureUsage.TransientAttachment) != 0) {
				prefProps |= VKMemoryPropertyFlagBits.LazilyAllocated;
				usage = VMAMemoryUsage.GPULazilyAllocated;
			}

			VMAAllocation alloc = allocator.AllocateMemoryForImage(image, new VMAAllocationCreateInfo() {
				RequiredFlags = reqProps,
				PreferredFlags = prefProps,
				Usage = usage
			}, out VMAAllocationInfo allocInfo);

			VKDeviceMemory deviceMemory = new(Device.Device, allocInfo.DeviceMemory, null);

			return new VulkanMemoryBinding() {
				MemoryManager = this,
				DeviceMemory = deviceMemory,
				Allocation = alloc,
				SupportedMapFlags = 0
			};
		}

	}

}
