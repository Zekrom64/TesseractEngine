using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// OpenGL buffer implementation.
	/// </summary>
	public class GLBuffer : IBuffer, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public ulong Size { get; }

		public BufferUsage Usage { get; }

		public IMemoryBinding? MemoryBinding => null;

		public MemoryMapFlags SupportedMappings { get; }

		/// <summary>
		/// If <see cref="GLBufferStorageFlags.DynamicStorage"/> is supported on this buffer.
		/// </summary>
		public bool DynamicStorage { get; }

		public GLBuffer(GLGraphics gl, BufferCreateInfo createInfo) {
			Graphics = gl;
			Size = createInfo.Size;
			Usage = createInfo.Usage;

			GLBufferStorageFlags storageFlags = 0;
			if ((createInfo.MapFlags & MemoryMapFlags.Read) != 0) storageFlags |= GLBufferStorageFlags.MapRead;
			if ((createInfo.MapFlags & MemoryMapFlags.Write) != 0) storageFlags |= GLBufferStorageFlags.MapWrite;
			if ((createInfo.MapFlags & MemoryMapFlags.Persistent) != 0) storageFlags |= GLBufferStorageFlags.MapPersistent;
			if ((createInfo.MapFlags & MemoryMapFlags.Coherent) != 0) storageFlags |= GLBufferStorageFlags.MapCoherent;
			if (DynamicStorage = (createInfo.Usage & BufferUsage.UpdateByCommand) != 0) storageFlags |= GLBufferStorageFlags.DynamicStorage;

			var iface = Graphics.Interface;
			iface.BufferStorage(ID = iface.CreateBuffer(), (nint)Size, storageFlags);
			SupportedMappings = GL.ARBBufferStorage != null ? createInfo.MapFlags : MemoryMapFlags.ReadWrite;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL15!.DeleteBuffers(ID);
		}

		public void FlushGPUToHost(in MemoryRange range = default) {
			MemoryRange range2 = range.Constrain(Size);
			Graphics.Interface.FlushBufferGPUToHost(ID, (nint)range2.Offset, (nint)range2.Length);
		}

		public void FlushHostToGPU(in MemoryRange range = default) {
			MemoryRange range2 = range.Constrain(Size);
			Graphics.Interface.FlushBufferHostToGPU(ID, (nint)range2.Offset, (nint)range2.Length);
		}

		public IPointer<T> Map<T>(MemoryMapFlags flags, in MemoryRange range = default) where T : unmanaged {
			GLMapAccessFlags access = GLMapAccessFlags.Unsynchronized;
			if ((flags & MemoryMapFlags.Persistent) != 0) access |= GLMapAccessFlags.Persistent;
			if ((flags & MemoryMapFlags.Coherent) != 0) access |= GLMapAccessFlags.Coherent;
			bool read = (flags & MemoryMapFlags.Read) != 0;
			if (read) access |= GLMapAccessFlags.Read;
			bool write = (flags & MemoryMapFlags.Write) != 0;
			if (write) access |= GLMapAccessFlags.Write;
			if (write && !read) access |= GLMapAccessFlags.InvalidateRange;

			return new UnmanagedPointer<T>(Graphics.Interface.MapBuffer(ID, access, range.Constrain(Size)));
		}

		public void Unmap() => Graphics.Interface.UnmapBuffer(ID);

	}

}
