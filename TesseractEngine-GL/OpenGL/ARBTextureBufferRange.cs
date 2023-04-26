using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureBufferRangeFunctions {

		[ExternFunction(AltNames = new string[] { "glTexBufferRangeARB" })]
		[NativeType("void glTexBufferRange(GLenum target, GLenum internalFormat, GLuint buffer, GLintptr offset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, uint, nint, nint, void> glTexBufferRange;

	}

	public class ARBTextureBufferRange : IGLObject {

		public GL GL { get; }
		public ARBTextureBufferRangeFunctions Functions { get; } = new();

		public ARBTextureBufferRange(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBufferRange(GLBufferTarget target, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) {
			unsafe {
				Functions.glTexBufferRange((uint)target, (uint)internalFormat, buffer, offset, size);
			}
		}
	}
}
