using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBBaseInstanceFunctions {

		[ExternFunction(AltNames = new string[] { "glDrawArraysInstancedBaseInstanceARB" })]
		[NativeType("void glDrawArraysInstancedBaseInstance(GLenum mode, GLint first, GLint count, GLint primCount, GLuint baseInstance)")]
		public delegate* unmanaged<uint, int, int, int, uint, void> glDrawArraysInstancedBaseInstance;
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseInstanceARB" })]
		[NativeType("void glDrawElementsInstancedBaseInstance(GLenum mode, GLint count, GLenum type, void* pIndices, GLint primCount, GLuint baseInstance)")]
		public delegate* unmanaged<uint, int, uint, IntPtr, int, uint, void> glDrawElementsInstancedBaseInstance;
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedBaseVertexBaseInstanceARB" })]
		[NativeType("void glDrawElementsInstancedBaseVertexBaseInstanceARB(GLenum mode, GLint count, GLenum type, void* pIndices, GLint primCount, GLint baseVertex, GLuint baseInstance)")]
		public delegate* unmanaged<uint, int, uint, IntPtr, int, int, uint, void> glDrawElementsInstancedBaseVertexBaseInstance;

	}

	public class ARBBaseInstance : IGLObject {

		public GL GL { get; }
		public ARBBaseInstanceFunctions Functions { get; } = new();

		public ARBBaseInstance(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstancedBaseInstance(GLDrawMode mode, int first, int count, int primCount, uint baseInstance) {
			unsafe {
				Functions.glDrawArraysInstancedBaseInstance((uint)mode, first, count, primCount, baseInstance);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, uint baseInstance) {
			unsafe {
				Functions.glDrawElementsInstancedBaseInstance((uint)mode, count, (uint)type, offset, primCount, baseInstance);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertexBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, int baseVertex, uint baseInstance) {
			unsafe {
				Functions.glDrawElementsInstancedBaseVertexBaseInstance((uint)mode, count, (uint)type, offset, primCount, baseVertex, baseInstance);
			}
		}
	}

}
