using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBIndirectParametersFunctions {

		[ExternFunction(AltNames = new string[] { "glMultiDrawArraysIndirectCountARB" })]
		[NativeType("void glMultiDrawArraysIndirectCount(GLenum mode, void* pIndirect, GLsizeiptr drawCount, GLsizei maxDrawCount, GLsizei stride)")]
		public delegate* unmanaged<uint, IntPtr, nint, int, int, void> glMultiDrawArraysIndirectCount;
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsIndirectCountARB" })]
		[NativeType("void glMultiDrawElementsIndirectCount(GLenum mode, GLenum type, void* pIndirect, GLsizeiptr drawCount, GLsizei maxDrawCount, GLsizei stride)")]
		public delegate* unmanaged<uint, uint, IntPtr, nint, int, int, void> glMultiDrawElementsIndirectCount;

	}

	public class ARBIndirectParameters : IGLObject {

		public GL GL { get; }
		public ARBIndirectParametersFunctions Functions { get; } = new();

		public ARBIndirectParameters(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirectCount(GLDrawMode mode, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) {
			unsafe {
				Functions.glMultiDrawArraysIndirectCount((uint)mode, indirectOffset, drawCountOffset, maxDrawCount, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirectCount(GLDrawMode mode, GLIndexType type, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) {
			unsafe {
				Functions.glMultiDrawElementsIndirectCount((uint)mode, (uint)type, indirectOffset, drawCountOffset, maxDrawCount, stride);
			}
		}
	}
}
