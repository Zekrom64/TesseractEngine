using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL.Graphics {

	public class GLBuffer : IBuffer, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public ulong Size { get; }

		public BufferUsage Usage { get; }

		public IMemoryBinding MemoryBinding => null;

		public MemoryMapFlags SupportedMappings { get; }

		public GLBuffer(GLGraphics gl, BufferCreateInfo createInfo) {
			Graphics = gl;
			Size = createInfo.Size;
			Usage = createInfo.Usage;

			GLBufferStorageFlags storageFlags = 0;
			if ((createInfo.MapFlags & MemoryMapFlags.Read) != 0) storageFlags |= GLBufferStorageFlags.MapRead;
			if ((createInfo.MapFlags & MemoryMapFlags.Write) != 0) storageFlags |= GLBufferStorageFlags.MapWrite;
			if ((createInfo.MapFlags & MemoryMapFlags.Persistent) != 0) storageFlags |= GLBufferStorageFlags.MapPersistent;
			if ((createInfo.MapFlags & MemoryMapFlags.Coherent) != 0) storageFlags |= GLBufferStorageFlags.MapCoherent;

			var dsa = GL.ARBDirectStateAccess;
			if (dsa != null) {
				ID = dsa.CreateBuffers();
				dsa.NamedBufferStorage(ID, (nint)Size, storageFlags);
				SupportedMappings = createInfo.MapFlags;
			} else {
				var gl33 = GL.GL33;
				ID = gl33.GenBuffers();
				Graphics.State.BindBuffer(GLBufferTarget.Array, ID);

				var bs = GL.ARBBufferStorage;
				if (bs != null) {
					bs.BufferStorage(GLBufferTarget.Array, (nint)Size, storageFlags);
					SupportedMappings = createInfo.MapFlags;
				} else {
					gl33.BufferData(GLBufferTarget.Array, (nint)Size, IntPtr.Zero, GLBufferUsage.DynamicDraw);
					SupportedMappings = MemoryMapFlags.ReadWrite;
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL15.DeleteBuffers(ID);
		}

		public void FlushGPUToHost(in MemoryRange range = default) {
			var gl33 = GL.GL33;
			var sils = GL.ARBShaderImageLoadStore;
			if (sils != null) {
				sils.MemoryBarrier(GLMemoryBarrier.ClientMappedBuffer);
				// Not sure if a temporary fence for current commands is better than glFinish() (probably)
				nuint fence = gl33.FenceSync(GLSyncCondition.GPUCommandsComplete);
				gl33.ClientWaitSync(fence, GLSyncFlags.FlushCommands, ulong.MaxValue);
				gl33.DeleteSync(fence);
			}
		}

		public void FlushHostToGPU(in MemoryRange range = default) {
			nint offset = (nint)range.Offset;
			nint length = (nint)range.Length;
			if (length == 0) length = (nint)Size - offset;

			var dsa = GL.ARBDirectStateAccess;
			if (dsa != null) dsa.FlushMappedNamedBufferRange(ID, offset, length);
			else {
				GLBufferTarget target = Graphics.State.BindBufferAny(ID);
				GL.GL33.FlushMappedBufferRange(target, offset, length);
			}
		}

		public IPointer<T> Map<T>(MemoryMapFlags flags, in MemoryRange range = default) where T : unmanaged {
			nint offset = (nint)range.Offset;
			nint length = (nint)range.Length;
			if (length == 0) length = (nint)Size - offset;

			GLMapAccessFlags access = GLMapAccessFlags.Unsynchronized;
			if ((flags & MemoryMapFlags.Persistent) != 0) access |= GLMapAccessFlags.Persistent;
			if ((flags & MemoryMapFlags.Coherent) != 0) access |= GLMapAccessFlags.Coherent;
			bool read = (flags & MemoryMapFlags.Read) != 0;
			if (read) access |= GLMapAccessFlags.Read;
			bool write = (flags & MemoryMapFlags.Write) != 0;
			if (write) access |= GLMapAccessFlags.Write;
			if (write && !read) access |= GLMapAccessFlags.InvalidateRange;

			var gl33 = GL.GL33;
			var dsa = GL.ARBDirectStateAccess;
			IntPtr pData;
			if (dsa != null) pData = dsa.MapNamedBufferRange(ID, offset, length, access);
			else {
				GLBufferTarget target = Graphics.State.BindBufferAny(ID);
				pData = gl33.MapBufferRange(target, offset, length, access);
			}
			return new UnmanagedPointer<T>(pData);
		}

		public void Unmap() {
			var dsa = GL.ARBDirectStateAccess;
			if (dsa != null) dsa.UnmapNamedBuffer(ID);
			else {
				GLBufferTarget target = Graphics.State.BindBufferAny(ID);
				GL.GL33.UnmapBuffer(target);
			}
		}

	}

}
