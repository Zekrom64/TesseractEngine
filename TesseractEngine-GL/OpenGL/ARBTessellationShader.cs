using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTessellationShaderFunctions {

		public delegate void PFN_glPatchParameteri(uint pname, int value);
		[ExternFunction(AltNames = new string[] { "glPatchParameteriARB" })]
		public PFN_glPatchParameteri glPatchParameteri;
		public delegate void PFN_glPatchParameterfv(uint pname, [NativeType("const float*")] IntPtr values);
		[ExternFunction(AltNames = new string[] { "glPatchParameterfv" })]
		public PFN_glPatchParameterfv glPatchParameterfv;

	}
#nullable restore

	public class ARBTessellationShader : IGLObject {

		public GL GL { get; }
		public ARBTessellationShaderFunctions Functions { get; } = new();

		public ARBTessellationShader(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParamter(GLPatchParamteri pname, int value) => Functions.glPatchParameteri((uint)pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParamter(GLPatchParamterfv pname, in ReadOnlySpan<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glPatchParameterfv((uint)pname, (IntPtr)pValues);
				}
			}
		}

	}
}
