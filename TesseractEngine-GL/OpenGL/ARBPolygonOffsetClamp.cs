using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBPolygonOffsetClampFunctions {

		[ExternFunction(AltNames = new string[] { "glPolygonOffsetClampARB" })]
		[NativeType("void glPolygonOffsetClamp(GLfloat factor, GLfloat units, GLfloat clamp)")]
		public delegate* unmanaged<float, float, float, void> glPolygonOffsetClamp;

	}

	public class ARBPolygonOffsetClamp : IGLObject {

		public GL GL { get; }
		public ARBPolygonOffsetClampFunctions Functions { get; } = new();

		public ARBPolygonOffsetClamp(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonOffsetClamp(float factor, float units, float clamp) {
			unsafe {
				Functions.glPolygonOffsetClamp(factor, units, clamp);
			}
		}
	}
}
