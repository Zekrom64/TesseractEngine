using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBShaderAtomicCountersFunctions {

		[ExternFunction(AltNames = new string[] { "glGetActiveAtomicCounterBufferivARB" })]
		[NativeType("void glGetActiveAtomicCounterBufferiv(GLuint program, GLuint bufferIndex, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetActiveAtomicCounterBufferiv;

	}

	public class ARBShaderAtomicCounters {

		public GL GL { get; }
		public ARBShaderAtomicCountersFunctions Functions { get; } = new();

		public ARBShaderAtomicCounters(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveAtomicCounterBuffer(uint program, uint bufferIndex, GLGetActiveAtomicCounterBuffer pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetActiveAtomicCounterBufferiv(program, bufferIndex, (uint)pname, pValues);
				}
			}
			return values;
		}

	}

}
