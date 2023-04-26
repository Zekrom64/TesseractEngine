using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBInternalFormatQuery2Functions {

		[ExternFunction(AltNames = new string[] { "glGetInternalformati64vARB" })]
		[NativeType("void glGetInternalFormati64v(GLenum target, GLenum internalFormat, GLenum pname, GLsizei bufSize, GLint64* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int, long*, void> glGetInternalformati64v;

	}

	public class ARBInternalFormatQuery2 : IGLObject {

		public GL GL { get; }
		public ARBInternalFormatQuery2Functions Functions { get; } = new();

		public ARBInternalFormatQuery2(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetInternalFormat(GLInternalFormatTarget target, GLInternalFormat internalFormat, GLGetInternalFormat pname, Span<long> values) {
			unsafe {
				fixed(long* pValues = values) {
					Functions.glGetInternalformati64v((uint)target, (uint)internalFormat, (uint)pname, values.Length, pValues);
				}
			}
			return values;
		}

	}
}
