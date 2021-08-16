using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBShaderAtomicCountersFunctions {

		public delegate void PFN_glGetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetActiveAtomicCounterBufferivARB" })]
		public PFN_glGetActiveAtomicCounterBufferiv glGetActiveAtomicCounterBufferiv;

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
					Functions.glGetActiveAtomicCounterBufferiv(program, bufferIndex, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

	}

}
