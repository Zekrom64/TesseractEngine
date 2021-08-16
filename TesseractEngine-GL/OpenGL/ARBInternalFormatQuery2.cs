using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBInternalFormatQuery2Functions {

		public delegate void PFN_glGetInternalformat64v(uint target, uint internalFormat, uint pname, int bufSize, [NativeType("GLint64*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetInternalformat64vARB" })]
		public PFN_glGetInternalformat64v glGetInternalformat64v;

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
					Functions.glGetInternalformat64v((uint)target, (uint)internalFormat, (uint)pname, values.Length, (IntPtr)pValues);
				}
			}
			return values;
		}

	}
}
