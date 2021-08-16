using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBInstancedArraysFunctions {

		public delegate void PFN_glVertexAttribDivisor(uint index, uint divisor);
		[ExternFunction(AltNames = new string[] { "glVertexAttribDivisorARB" })]
		public PFN_glVertexAttribDivisor glVertexAttribDivisor;

	}

	public class ARBInstancedArrays : IGLObject {

		public GL GL { get; }
		public ARBInstancedArraysFunctions Functions { get; } = new();

		public ARBInstancedArrays(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribDivisor(uint index, uint divisor) => Functions.glVertexAttribDivisor(index, divisor);

	}

}
