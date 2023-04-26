using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBSampleShadingFunctions {

		[ExternFunction(AltNames = new string[] { "glMinSampleShadingARB" })]
		[NativeType("void glMinSampleShading(GLfloat value)")]
		public delegate* unmanaged<float, void> glMinSampleShading;

	}

	public class ARBSampleShading : IGLObject {

		public GL GL { get; }
		public ARBSampleShadingFunctions Functions { get; } = new();

		public ARBSampleShading(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		public float MinSampleShading {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.GL11.GetFloat(GLEnums.GL_MIN_SAMPLE_SHADING_VALUE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Functions.glMinSampleShading(value);
				}
			}
		}

	}
}
