using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBInternalFormatQueryFunctions {

		public delegate void PFN_glGetInternalformativ(uint target, uint internalFormat, uint pname, int bufSize, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetInternalFormativ" })]
		public PFN_glGetInternalformativ glGetInternalformativ;

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
					Functions.glGetInternalformativ((uint)target, (uint)internalFormat, (uint)pname, v.Length, (IntPtr)pV);
				}
			}
			return v;
		}

	}

}
