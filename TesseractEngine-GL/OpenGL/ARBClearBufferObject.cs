using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBClearBufferObjectFunctions {

		public delegate void PFN_glClearBufferData(uint target, uint internalFormat, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearBufferDataARB" })]
		public PFN_glClearBufferData glClearBufferData;
		public delegate void PFN_glClearBufferSubData(uint target, uint internalFormat, nint offset, nint size, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearBufferSubDataARB" })]
		public PFN_glClearBufferSubData glClearBufferSubData;

	}
#nullable restore

	public class ARBClearBufferObject : IGLObject {

		public GL GL { get; }
		public ARBClearBufferObjectFunctions Functions { get; } = new();

		public ARBClearBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBufferData<T>(GLBufferTarget target, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glClearBufferData((uint)target, (uint)internalFormat, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBufferSubData<T>(GLBufferTarget target, GLInternalFormat internalFormat, nint offset, nint size, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glClearBufferSubData((uint)target, (uint)internalFormat, offset, size, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

	}
}
