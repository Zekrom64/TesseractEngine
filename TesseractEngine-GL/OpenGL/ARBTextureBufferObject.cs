using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureBufferObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glTexBuffer" })]
		[NativeType("void glTexBuffer(GLenum target, GLenum internalFormat, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, uint, void> glTexBuffer;

	}

	public class ARBTextureBufferObject : IGLObject {

		public GL GL { get; }
		public ARBTextureBufferObjectFunctions Functions { get; } = new();

		public ARBTextureBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBuffer(GLTextureTarget target, GLInternalFormat internalFormat, uint buffer) {
			unsafe {
				Functions.glTexBuffer((uint)target, (uint)internalFormat, buffer);
			}
		}
	}

}
