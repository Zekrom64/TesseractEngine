using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBMultiDrawIndirectFunctions {

		[ExternFunction(AltNames = new string[] { "glMultiDrawArraysIndirectARB" })]
		[NativeType("void glMultiDrawArraysIndirect(GLenum mode, void* pIndirect, GLsizei primCount, GLsizei stride)")]
		public delegate* unmanaged<uint, IntPtr, int, int, void> glMultiDrawArraysIndirect;
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsIndirectARB" })]
		[NativeType("void glMultiDrawElementsIndirect(GLenum mode, GLenum type, void* pIndirect, GLsizei primCount, GLsizei stride)")]
		public delegate* unmanaged<uint, uint, IntPtr, int, int, void> glMultiDrawElementsIndirect;

	}

	public class ARBMultiDrawIndirect : IGLObject {

		public GL GL { get; }
		public ARBMultiDrawIndirectFunctions Functions { get; } = new();

		public ARBMultiDrawIndirect(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirect(GLDrawMode mode, nint offset, int primcount, int stride) {
			unsafe {
				Functions.glMultiDrawArraysIndirect((uint)mode, offset, primcount, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset, int primcount, int stride) {
			unsafe {
				Functions.glMultiDrawElementsIndirect((uint)mode, (uint)type, offset, primcount, stride);
			}
		}
	}
}
