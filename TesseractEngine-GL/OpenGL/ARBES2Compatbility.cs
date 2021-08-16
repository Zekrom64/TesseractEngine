using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBES2CompatbilityFunctions {

		public delegate void PFN_glReleaseShaderCompiler();
		[ExternFunction(AltNames = new string[] { "glReleaseShaderCompilerARB" })]
		public PFN_glReleaseShaderCompiler glReleaseShaderCompiler;
		public delegate void PFN_glShaderBinary(int count, [NativeType("const GLuint*")] IntPtr shaders, uint binaryFormat, IntPtr binary, int length);
		[ExternFunction(AltNames = new string[] { "glShaderBinaryARB" })]
		public PFN_glShaderBinary glShaderBinary;
		public delegate void PFN_glGetShaderPrecisionFormat(uint shaderType, uint precisionType, in Vector2i range, out int precision);
		[ExternFunction(AltNames = new string[] { "glGetShaderPrecisionFormatARB" })]
		public PFN_glGetShaderPrecisionFormat glGetShaderPrecisionFormat;
		public delegate void PFN_glDepthRangef(float n, float f);
		[ExternFunction(AltNames = new string[] { "glDepthRangefARB" })]
		public PFN_glDepthRangef glDepthRangef;
		public delegate void PFN_glClearDepthf(float d);
		[ExternFunction(AltNames = new string[] { "glClearDepthfARB" })]
		public PFN_glClearDepthf glClearDepthf;

	}

	public class ARBES2Compatbility : IGLObject {

		public GL GL { get; }
		public ARBES2CompatbilityFunctions Functions { get; } = new();

		public ARBES2Compatbility(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReleaseShaderCompiler() => Functions.glReleaseShaderCompiler();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderBinary(in ReadOnlySpan<uint> shaders, uint binaryFormat, in ReadOnlySpan<byte> binary) {
			unsafe {
				fixed(uint* pShaders = shaders) {
					fixed(byte* pBinary = binary) {
						Functions.glShaderBinary(shaders.Length, (IntPtr)pShaders, binaryFormat, (IntPtr)pBinary, binary.Length);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShaderPrecisionFormat(GLShaderType shaderType, GLPrecisionType precisionType, Vector2i range) {
			Functions.glGetShaderPrecisionFormat((uint)shaderType, (uint)precisionType, range, out int precision);
			return precision;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRange(float near, float far) => Functions.glDepthRangef(near, far);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepth(float d) => Functions.glClearDepthf(d);

	}

}
