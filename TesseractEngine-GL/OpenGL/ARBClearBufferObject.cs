using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBClearBufferObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glClearBufferDataARB" })]
		[NativeType("void glClearBufferData(GLenum target, GLenum internalFormat, GLenum format, GLenum type, void* pData)")]
		public delegate* unmanaged<uint, uint, uint, uint, IntPtr, void> glClearBufferData;
		[ExternFunction(AltNames = new string[] { "glClearBufferSubDataARB" })]
		[NativeType("void glClearBufferSubData(GLenum target, GLenum internalFormat, GLintptr offset, GLsizeiptr, GLenum, GLenum, void* pData)")]
		public delegate* unmanaged<uint, uint, nint, nint, uint, uint, IntPtr, void> glClearBufferSubData;

	}

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
