using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.Vulkan.Graphics.Impl {

	public interface IVKMemoryBinding : IMemoryBinding, IDisposable {

		public VKDeviceMemory DeviceMemory { get; }

		public ulong Size { get; }

		public MemoryMapFlags SupportedMapFlags { get; }

		public void Bind(VKBuffer buffer);

		public void Bind(VKImage image);

		public IntPtr Map();

		public void Unmap();

		public void Flush(ulong offset, ulong length);

		public void Invalidate(ulong offset, ulong length);

	}

	public class VulkanMemoryBinding : IVKMemoryBinding {

		public VulkanMemory MemoryManager { get; init; } = null!;

		public VMAAllocation Allocation { get; init; } = null!;

		public VKDeviceMemory DeviceMemory { get; init; } = null!;

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

	public class VulkanMemory : IDisposable {

		public ulong TotalLocalBytes { get; }

		public ulong TotalVisibleBytes { get; }

		public ulong TotalUsedBytes => allocator.Budget.AllocationBytes;

		public VulkanDevice Device { get; }

		private readonly VMAAllocator allocator;

		public VulkanMemory(VulkanDevice device) {
			Device = device;

			// Enumerate memory heaps and count available memory
			ulong totalLocal = 0;
			ulong totalVisible = 0;
			var memProps = device.PhysicalDevice.MemoryProperties;
			var heaps = memProps.MemoryHeaps;
			for (int i = 0; i < memProps.MemoryHeapCount; i++) {
				var heap = heaps[i];
				totalVisible += heap.Size;
				if ((heap.Flags & VKMemoryHeapFlagBits.DeviceLocal) != 0) totalLocal += heap.Size;
			}

			// Adjust values based on device types before finalizing values
			switch (device.PhysicalDevice.Properties.DeviceType) {
				case VKPhysicalDeviceType.CPU:
				case VKPhysicalDeviceType.IntegratedGPU:
					totalLocal = totalVisible;
					break;
			}

			TotalLocalBytes = totalLocal;
			TotalVisibleBytes = totalVisible;

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
