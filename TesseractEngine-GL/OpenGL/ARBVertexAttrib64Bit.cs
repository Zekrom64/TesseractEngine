using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBVertexAttrib64BitFunctions {

		[ExternFunction(AltNames = new string[] { "glVertexAttribLPointerARB" })]
		[NativeType("void glVertexAttribLPointer(GLuint index, GLint size, GLenum type, GLsizei stride, void* pPointer)")]
		public delegate* unmanaged<uint, int, uint, int, IntPtr, void> glVertexAttribLPointer;

		[ExternFunction(Relaxed = true)]
		[NativeType("void glVertexArrayVertexAttribLOffsetEXT(GLuint vertexArray, GLuint buffer, GLuint index, GLsizei index, GLenum type, GLsizei stride, void* pPointer)")]
		public delegate* unmanaged<uint, uint, uint, int, uint, int, IntPtr, void> glVertexArrayVertexAttribLOffsetEXT;

	}

	public class ARBVertexAttrib64Bit : IGLObject {

		public GL GL { get; }
		public ARBVertexAttrib64BitFunctions Functions { get; } = new();

		public ARBVertexAttrib64Bit(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLPointer(uint index, int size, GLTextureType type, int stride, nint offset) {
			unsafe {
				Functions.glVertexAttribLPointer(index, size, (uint)type, stride, offset);
			}
		}

		public void VertexArrayVertexAttribLOffset(uint vaobj, uint buffer, uint index, int size, GLTextureType type, int stride, nint offset) {
			unsafe {
				Functions.glVertexArrayVertexAttribLOffsetEXT(vaobj, buffer, index, size, (uint)type, stride, offset);
			}
		}
	}

}
