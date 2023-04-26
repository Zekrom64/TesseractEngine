using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBDrawInstancedFunctions {

		[ExternFunction(AltNames = new string[] { "glDrawArraysInstancedARB" })]
		[NativeType("void glDrawArraysInstanced(GLenum mode, GLint first, GLsizei count, GLsizei primCount)")]
		public delegate* unmanaged<uint, int, int, int, void> glDrawArraysInstanced;
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedARB" })]
		[NativeType("void glDrawElementsInstanced(GLenum mode, GLsizei count, GLenum type, void* pIndices, GLsizei primCount)")]
		public delegate* unmanaged<uint, int, uint, IntPtr, int, void> glDrawElementsInstanced;

	}

	public class ARBDrawInstanced : IGLObject {

		public GL GL { get; }
		public ARBDrawInstancedFunctions Functions { get; } = new();

		public ARBDrawInstanced(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstanced(GLDrawMode mode, int first, int count, int primcount) {
			unsafe {
				Functions.glDrawArraysInstanced((uint)mode, first, count, primcount);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstanced(GLDrawMode mode, int count, GLIndexType type, nint offset, int primcount) {
			unsafe {
				Functions.glDrawElementsInstanced((uint)mode, count, (uint)type, offset, primcount);
			}
		}
	}
}
