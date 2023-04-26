using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBMapBufferRangeFunctions {

		[ExternFunction(AltNames = new string[] { "glMapBufferRangeARB" })]
		[NativeType("void* glMapBufferRange(GLenum target, GLintptr offset, GLsizeiptr length, GLbitfield access)")]
		public delegate* unmanaged<uint, nint, nint, uint, IntPtr> glMapBufferRange;
		[ExternFunction(AltNames = new string[] { "glFlushMappedBufferRangeARB" })]
		[NativeType("void glFLushMappedBufferRange(GLenum target, GLintptr offset, GLsizeiptr length)")]
		public delegate* unmanaged<uint, nint, nint, void> glFlushMappedBufferRange;

	}

	public class ARBMapBufferRange : IGLObject {

		public GL GL { get; }
		public ARBMapBufferRangeFunctions Functions { get; } = new();

		public ARBMapBufferRange(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBufferRange(GLBufferTarget target, nint offset, nint length, GLMapAccessFlags access) {
			unsafe {
				return Functions.glMapBufferRange((uint)target, offset, length, (uint)access);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedBufferRange(GLBufferTarget target, nint offset, nint length) {
			unsafe {
				Functions.glFlushMappedBufferRange((uint)target, offset, length);
			}
		}
	}
}
