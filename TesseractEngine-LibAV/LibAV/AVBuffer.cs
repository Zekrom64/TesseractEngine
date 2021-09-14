using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {

	public class AVBuffer : IDisposable, ICloneable {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Variable is modified indirectly by pointer in the libav API")]
		private IntPtr buffer;
		public IPointer<AVBufferRef> Buffer => new UnmanagedPointer<AVBufferRef>(buffer);

		public AVBuffer([NativeType("AVBufferRef*")] IntPtr buffer) {
			this.buffer = buffer;
		}

		public AVBuffer(int size, bool zeroinit = true) {
			if (zeroinit) buffer = LibAVUtil.Functions.av_buffer_allocz(size);
			else buffer = LibAVUtil.Functions.av_buffer_alloc(size);
		}

		public AVBuffer(IPointer<byte> data, int size, AVFree free = null, IntPtr opaque = default, int flags = 0) {
			buffer = LibAVUtil.Functions.av_buffer_create(data.Ptr, size, free, opaque, flags);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (buffer != IntPtr.Zero) {
				unsafe {
					fixed (IntPtr* pBuffer = &buffer) {
						LibAVUtil.Functions.av_buffer_unref((IntPtr)pBuffer);
					}
				}
			}
		}

		~AVBuffer() {
			Dispose();
		}

		public AVBuffer Clone() => new(LibAVUtil.Functions.av_buffer_ref(buffer));

		object ICloneable.Clone() => Clone();

		public bool Writable => LibAVUtil.Functions.av_buffer_is_writable(buffer);

		public AVError MakeWritable() {
			unsafe {
				fixed(IntPtr* pBuffer = &buffer) {
					return (AVError)LibAVUtil.Functions.av_buffer_make_writable((IntPtr)pBuffer);
				}
			}
		}

		public AVError Realloc(int size) {
			unsafe {
				fixed(IntPtr* pBuffer = &buffer) {
					return (AVError)LibAVUtil.Functions.av_buffer_realloc((IntPtr)pBuffer, size);
				}
			}
		}

	}

	public class AVBufferPool : IDisposable {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Variable is modified indirectly by pointer in the libav API")]
		private IntPtr pool;
		[NativeType("AVBufferPool*")]
		public IntPtr Pool => Pool;

		public AVBufferPool(int size, AVBufferAlloc alloc = null) {
			pool = LibAVUtil.Functions.av_buffer_pool_init(size, alloc);
		}

		public AVBufferPool(int size, IntPtr opaque, AVBufferAlloc2 alloc, AVPoolFree poolFree) {
			pool = LibAVUtil.Functions.av_buffer_pool_init2(size, opaque, alloc, poolFree);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (pool != IntPtr.Zero) {
				unsafe {
					fixed(IntPtr* pPool = &pool) {
						LibAVUtil.Functions.av_buffer_pool_uninit((IntPtr)pPool);
					}
				}
			}
		}

		~AVBufferPool() {
			Dispose();
		}

		public AVBuffer Get() => new(LibAVUtil.Functions.av_buffer_pool_get(pool));

	}

}
