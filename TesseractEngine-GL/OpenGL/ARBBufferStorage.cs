using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBBufferStorageFunctions {

		public delegate void PFN_glBufferStorage(uint target, nint size, IntPtr data, uint flags);
		[ExternFunction(AltNames = new string[] { "glBufferStorageARB" })]
		public PFN_glBufferStorage glBufferStorage;

	}
#nullable restore

	public class ARBBufferStorage : IGLObject {

		public GL GL { get; }
		public ARBBufferStorageFunctions Functions { get; } = new();

		public ARBBufferStorage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage(GLBufferTarget target,  nint size, IntPtr data, GLBufferStorageFlags flags) => Functions.glBufferStorage((uint)target, size, data, (uint)flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage(GLBufferTarget target, nint size, GLBufferStorageFlags flags) => Functions.glBufferStorage((uint)target, size, IntPtr.Zero, (uint)flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage<T>(GLBufferTarget target, in ReadOnlySpan<T> data, GLBufferStorageFlags flags) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glBufferStorage((uint)target, sizeof(T) * data.Length, (IntPtr)pData, (uint)flags);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage<T>(GLBufferTarget target, GLBufferStorageFlags flags, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glBufferStorage((uint)target, sizeof(T) * data.Length, (IntPtr)pData, (uint)flags);
				}
			}
		}

	}

}
