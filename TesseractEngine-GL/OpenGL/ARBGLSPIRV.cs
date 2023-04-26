using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBGLSPIRVFunctions {

		public delegate void PFN_glSpecializeShader(uint shader, [MarshalAs(UnmanagedType.LPStr)] string entryPoint, uint numSpecializationConstants, [NativeType("const GLuint*")] IntPtr pConstantIndex, [NativeType("const GLuint*")] IntPtr pConstantValue);
		[ExternFunction(AltNames = new string[] { "glSpecializeShaderARB" })]
		[NativeType("void glSpecializeShader(GLuint shader, const char* entryPoint, GLuint numSpecializationConstants, const GLuint* pConstantIndex, const GLuint* pConstantValue)")]
		public delegate* unmanaged<uint, byte*, uint, uint*, uint*, void> glSpecializeShader;

	}

	public class ARBGLSPIRV : IGLObject {

		public GL GL { get; }
		public ARBGLSPIRVFunctions Functions { get; } = new();

		public ARBGLSPIRV(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SpecializeShader(uint shader, string entryPoint, in ReadOnlySpan<uint> constantIndex, in ReadOnlySpan<uint> constantValue) {
			int n = Math.Min(constantIndex.Length, constantValue.Length);
			unsafe {
				fixed (byte* pEntryPoint = MemoryUtil.StackallocUTF8(entryPoint, stackalloc byte[256])) {
					fixed (uint* pConstantIndex = constantIndex, pConstantValue = constantValue) {
						Functions.glSpecializeShader(shader, pEntryPoint, (uint)n, pConstantIndex, pConstantValue);
					}
				}
			}
		}

	}
}
