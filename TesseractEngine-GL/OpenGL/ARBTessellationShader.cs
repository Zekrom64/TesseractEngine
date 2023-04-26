using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTessellationShaderFunctions {

		[ExternFunction(AltNames = new string[] { "glPatchParameteriARB" })]
		[NativeType("void glPatchParameteri(GLenum pname, GLint value)")]
		public delegate* unmanaged<uint, int, void> glPatchParameteri;
		[ExternFunction(AltNames = new string[] { "glPatchParameterfv" })]
		[NativeType("void glPatchParameterfv(GLenum pname, const GLfloat* pValues)")]
		public delegate* unmanaged<uint, float*, void> glPatchParameterfv;

	}

	public class ARBTessellationShader : IGLObject {

		public GL GL { get; }
		public ARBTessellationShaderFunctions Functions { get; } = new();

		public ARBTessellationShader(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParamter(GLPatchParamteri pname, int value) {
			unsafe {
				Functions.glPatchParameteri((uint)pname, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParamter(GLPatchParamterfv pname, in ReadOnlySpan<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glPatchParameterfv((uint)pname, pValues);
				}
			}
		}

	}
}
