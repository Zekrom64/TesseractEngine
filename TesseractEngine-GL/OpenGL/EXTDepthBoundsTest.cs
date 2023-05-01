using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public unsafe class EXTDepthBoundsTestFunctions {

		[NativeType("void glDepthBoundsEXT(GLdouble zmin, GLdouble zmax)")]
		public delegate* unmanaged<double, double, void> glDepthBoundsEXT;

	}

	public class EXTDepthBoundsTest : IGLObject {

		public GL GL { get; }
		public EXTDepthBoundsTestFunctions Functions { get; } = new();

		public EXTDepthBoundsTest(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public (double Min, double Max) DepthBounds {
			get {
				Span<double> bounds = GL.GL11.GetDouble(GLEnums.GL_DEPTH_BOUNDS_EXT, stackalloc double[2]);
				return (bounds[0], bounds[1]);
			}
			set {
				unsafe {
					Functions.glDepthBoundsEXT(value.Min, value.Max);
				}
			}
		}

	}
}
