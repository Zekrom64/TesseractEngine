using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBES2CompatbilityFunctions {

		[ExternFunction(AltNames = new string[] { "glReleaseShaderCompilerARB" })]
		[NativeType("void glReleaseShaderCompiler()")]
		public delegate* unmanaged<void> glReleaseShaderCompiler;
		[ExternFunction(AltNames = new string[] { "glShaderBinaryARB" })]
		[NativeType("void glShaderBinary(GLsizei count, const GLuint* pShaders, GLenum binaryFormat, void* pBinary, GLsizei length)")]
		public delegate* unmanaged<int, uint*, uint, IntPtr, int, void> glShaderBinary;
		[ExternFunction(AltNames = new string[] { "glGetShaderPrecisionFormatARB" })]
		[NativeType("void glGetShaderPrecisionFormat(GLenum shaderType, GLenum precisionType, const GLint range[2], GLint* pPrecision)")]
		public delegate* unmanaged<uint, uint, in Vector2i, out int, void> glGetShaderPrecisionFormat;
		[ExternFunction(AltNames = new string[] { "glDepthRangefARB" })]
		[NativeType("void glDepthRangef(GLfloat near, GLfloat far)")]
		public delegate* unmanaged<float, float, void> glDepthRangef;
		[ExternFunction(AltNames = new string[] { "glClearDepthfARB" })]
		[NativeType("void glClearDepthf(GLfloat depth)")]
		public delegate* unmanaged<float, void> glClearDepthf;

	}

	public class ARBES2Compatbility : IGLObject {

		public GL GL { get; }
		public ARBES2CompatbilityFunctions Functions { get; } = new();

		public ARBES2Compatbility(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReleaseShaderCompiler() {
			unsafe {
				Functions.glReleaseShaderCompiler();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderBinary(in ReadOnlySpan<uint> shaders, uint binaryFormat, in ReadOnlySpan<byte> binary) {
			unsafe {
				fixed(uint* pShaders = shaders) {
					fixed(byte* pBinary = binary) {
						Functions.glShaderBinary(shaders.Length, pShaders, binaryFormat, (IntPtr)pBinary, binary.Length);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShaderPrecisionFormat(GLShaderType shaderType, GLPrecisionType precisionType, Vector2i range) {
			unsafe {
				Functions.glGetShaderPrecisionFormat((uint)shaderType, (uint)precisionType, range, out int precision);
				return precision;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRange(float near, float far) {
			unsafe {
				Functions.glDepthRangef(near, far);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepth(float d) {
			unsafe {
				Functions.glClearDepthf(d);
			}
		}
	}

}
