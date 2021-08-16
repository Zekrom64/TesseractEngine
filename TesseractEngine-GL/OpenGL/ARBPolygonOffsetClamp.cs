using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBPolygonOffsetClampFunctions {

		public delegate void PFN_glPolygonOffsetClamp(float factor, float units, float clamp);
		[ExternFunction(AltNames = new string[] { "glPolygonOffsetClampARB" })]
		public PFN_glPolygonOffsetClamp glPolygonOffsetClamp;

	}

	public class ARBPolygonOffsetClamp : IGLObject {

		public GL GL { get; }
		public ARBPolygonOffsetClampFunctions Functions { get; } = new();

		public ARBPolygonOffsetClamp(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonOffsetClamp(float factor, float units, float clamp) => Functions.glPolygonOffsetClamp(factor, units, clamp);

	}
}
