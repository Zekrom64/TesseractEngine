using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;

namespace Tesseract.GL.Graphics {

	public class GLBuffer : IBuffer, IGLObject {

		public GL GL { get; }

		public uint ID { get; }

		public ulong Size { get; }

		public BufferUsage Usage { get; }

		public IMemoryBinding MemoryBinding => null;

		public MemoryMapFlags SupportedMappings { get; }

		public GLBuffer(GL gl, BufferCreateInfo createInfo) {
			GL = gl;
			Size = createInfo.Size;
			Usage = createInfo.Usage;
			// TODO: Implement OpenGL buffer creation
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL15.DeleteBuffers(ID);
		}

		public void FlushGPUToHost(in MemoryRange range = default) {
			throw new NotImplementedException();
		}

		public void FlushHostToGPU(in MemoryRange range = default) {
			throw new NotImplementedException();
		}

		public IPointer<T> Map<T>(MemoryMapFlags flags, in MemoryRange range = default) {
			throw new NotImplementedException();
		}

		public void Unmap() {
			throw new NotImplementedException();
		}

	}

}
