using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBInternalFormatQueryFunctions {

		[ExternFunction(AltNames = new string[] { "glGetInternalFormativ" })]
		[NativeType("void glGetInternalFormativ(GLenum target, GLenum internalFormat, GLenum pname, GLsizei bufSize, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int, int*, void> glGetInternalformativ;

	}

	public class ARBInternalFormatQuery : IGLObject {

		public GL GL { get; }
		public ARBInternalFormatQueryFunctions Functions { get; } = new();

		public ARBInternalFormatQuery(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetInternalFormat(GLInternalFormatTarget target, GLInternalFormat internalFormat, GLGetInternalFormat pname, Span<int> v) {
			unsafe {
				fixed(int* pV = v) {
					Functions.glGetInternalformativ((uint)target, (uint)internalFormat, (uint)pname, v.Length, pV);
				}
			}
			return v;
		}

	}

}
