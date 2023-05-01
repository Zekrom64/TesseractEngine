using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBCopyBufferFunctions {

		[ExternFunction(AltNames = new string[] { "glCopyBufferSubDataARB", "glCopyBufferSubDataEXT" })]
		[NativeType("void glCopyBufferSubData(GLenum readTarget, GLenum writeTarget, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, nint, nint, nint, void> glCopyBufferSubData;

	}

	public class ARBCopyBuffer : IGLObject {

		public GL GL { get; }
		public ARBCopyBufferFunctions Functions { get; } = new();

		public ARBCopyBuffer(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferSubData(GLBufferTarget readTarget, GLBufferTarget writeTarget, nint readOffset, nint writeOffset, nint size) {
			unsafe {
				Functions.glCopyBufferSubData((uint)readTarget, (uint)writeTarget, readOffset, writeOffset, size);
			}
		}
	}
}
