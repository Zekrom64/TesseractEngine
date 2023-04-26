using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBInstancedArraysFunctions {

		[ExternFunction(AltNames = new string[] { "glVertexAttribDivisorARB" })]
		[NativeType("void glVertexAttribDivisor(GLuint index, GLuint divisor)")]
		public delegate* unmanaged<uint, uint, void> glVertexAttribDivisor;

	}

	public class ARBInstancedArrays : IGLObject {

		public GL GL { get; }
		public ARBInstancedArraysFunctions Functions { get; } = new();

		public ARBInstancedArrays(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribDivisor(uint index, uint divisor) {
			unsafe {
				Functions.glVertexAttribDivisor(index, divisor);
			}
		}
	}

}
