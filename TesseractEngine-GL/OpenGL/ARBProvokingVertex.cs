using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBProvokingVertexFunctions {

		public delegate void PFN_glProvokingVertex(uint mode);
		[ExternFunction(AltNames = new string[] { "glProvokingVertexARB" })]
		public PFN_glProvokingVertex glProvokingVertex;

	}

	public class ARBProvokingVertex : IGLObject {

		public GL GL { get; }
		public ARBProvokingVertexFunctions Functions { get; } = new();

		public ARBProvokingVertex(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProvokingVertex(GLProvokingVertexConvention mode) => Functions.glProvokingVertex((uint)mode);

	}
}
