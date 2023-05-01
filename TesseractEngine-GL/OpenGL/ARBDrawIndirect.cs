using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBDrawIndirectFunctions {

		[ExternFunction(AltNames = new string[] { "glDrawArraysIndirectARB" })]
		[NativeType("void glDrawArraysIndirect(GLenum mode, void* pIndirect)")]
		public delegate* unmanaged<uint, IntPtr, void> glDrawArraysIndirect;
		[ExternFunction(AltNames = new string[] { "glDrawElementsIndirectARB" })]
		[NativeType("void glDrawElementsIndirect(GLenum mode, GLenum type, void* pIndirect)")]
		public delegate* unmanaged<uint, uint, IntPtr, void> glDrawElementsIndirect;

	}

	public class ARBDrawIndirect {

		public GL GL { get; }
		public ARBDrawIndirectFunctions Functions { get; } = new();

		public ARBDrawIndirect(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysIndirect(GLDrawMode mode, nint offset) {
			unsafe {
				Functions.glDrawArraysIndirect((uint)mode, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset) {
			unsafe {
				Functions.glDrawElementsIndirect((uint)mode, (uint)type, offset);
			}
		}
	}

}
