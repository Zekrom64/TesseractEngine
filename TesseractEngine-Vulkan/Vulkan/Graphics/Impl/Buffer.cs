using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;

namespace Tesseract.Vulkan.Graphics.Impl {

	/// <summary>
	/// Implementation of a Vulkan buffer.
	/// </summary>
	public class VulkanBuffer : IBuffer {

		/// <summary>
		/// The graphics context this buffer belongs to.
		/// </summary>
		public VulkanGraphics Graphics { get; }

		/// <summary>
		/// The underlying Vulkan buffer.
		/// </summary>
		public VKBuffer Buffer { get; }


		public ulong Size { get; }

		public BufferUsage Usage { get; }

		/// <summary>
		/// The memory binding for this buffer.
		/// </summary>
		public IVKMemoryBinding MemoryBinding { get; }

		IMemoryBinding IBuffer.MemoryBinding => MemoryBinding;

		public MemoryMapFlags SupportedMappings => MemoryBinding.SupportedMapFlags;

		public void Dispose() {
			GC.SuppressFinalize(this);
			MemoryBinding.Dispose();
			Buffer.Dispose();
		}

		public void FlushGPUToHost(in MemoryRange range = default) {
			MemoryRange range2 = range.Constrain(Size);
			if (range2.Length > 0) MemoryBinding.Invalidate(range2.Offset, range2.Length);
		}

		public void FlushHostToGPU(in MemoryRange range = default) {
			MemoryRange range2 = range.Constrain(Size);
			if (range2.Length > 0) MemoryBinding.Flush(range2.Offset, range2.Length);
		}

		public IPointer<T> Map<T>(MemoryMapFlags flags, in MemoryRange range = default) where T : unmanaged {
			MemoryRange range2 = range.Constrain(Size);
			if (range2.Length == 0) throw new ArgumentException("Cannot map a range of 0 length", nameof(range));
			IntPtr ptr = MemoryBinding.Map();
			unsafe { return new UnmanagedPointer<T>(ptr, (int)(range2.Length / (ulong)sizeof(T))); }
		}

		public void Unmap() => MemoryBinding.Unmap();

		public VulkanBuffer(VulkanGraphics graphics, VKBuffer buffer, in BufferCreateInfo createInfo) {
			Graphics = graphics;
			Buffer = buffer;
			Size = createInfo.Size;
			Usage = createInfo.Usage;

			if (createInfo.MemoryBinding is IVKMemoryBinding vkbinding) MemoryBinding = vkbinding;
			else MemoryBinding = Graphics.Memory.Allocate(createInfo, buffer);
			MemoryBinding.Bind(buffer);
		}

	}

}
