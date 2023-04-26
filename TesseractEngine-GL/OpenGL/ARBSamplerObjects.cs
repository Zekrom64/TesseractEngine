using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBSamplerObjectsFunctions {

		[ExternFunction(AltNames = new string[] { "glGenSamplersARB" })]
		[NativeType("void glGenSamplers(GLsizei count, GLuint* pSamplers)")]
		public delegate* unmanaged<int, uint*, void> glGenSamplers;
		[ExternFunction(AltNames = new string[] { "glDeleteSamplersARB" })]
		[NativeType("void glDeleteSamplers(GLsizei count, const GLuint* pSamplers)")]
		public delegate* unmanaged<int, uint*, void> glDeleteSamplers;
		[ExternFunction(AltNames = new string[] { "glIsSamplerARB" })]
		[NativeType("GLboolean glIsSampler(GLuint sampler)")]
		public delegate* unmanaged<uint, byte> glIsSampler;
		[ExternFunction(AltNames = new string[] { "glBindSamplerARB" })]
		[NativeType("void glBindSampler(GLuint unit, GLuint sampler)")]
		public delegate* unmanaged<uint, uint, void> glBindSampler;
		[ExternFunction(AltNames = new string[] { "glSamplerParameteriARB" })]
		[NativeType("void glSamplerParameteri(GLuint sampler, GLenum pname, GLint param)")]
		public delegate* unmanaged<uint, uint, int, void> glSamplerParameteri;
		[ExternFunction(AltNames = new string[] { "glSamplerParameterfARB" })]
		[NativeType("void glSamplerParameterf(GLuint sampler, GLenum pname, GLfloat param)")]
		public delegate* unmanaged<uint, uint, float, void> glSamplerParameterf;
		[ExternFunction(AltNames = new string[] { "glSamplerParameterivARB" })]
		[NativeType("void glSamplerParameteriv(GLuint sampler, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glSamplerParameteriv;
		[ExternFunction(AltNames = new string[] { "glSamplerParameterfvARB" })]
		[NativeType("void glSamplerParameterfv(GLuint sampler, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<uint, uint, float*, void> glSamplerParameterfv;
		[ExternFunction(AltNames = new string[] { "glSamplerParameterIivARB" })]
		[NativeType("void glSamplerParameterIiv(GLuint sampler, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glSamplerParameterIiv;
		[ExternFunction(AltNames = new string[] { "glSamplerParameterIuivARB" })]
		[NativeType("void glSamplerParameterIuiv(GLuint sampler, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glSamplerParameterIuiv;
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameteriv" })]
		[NativeType("void glGetSamplerParameteriv(GLuint sampler, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetSamplerParameteriv;
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterfv" })]
		[NativeType("void glGetSamplerParameterfv(GLuint sampler, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<uint, uint, float*, void> glGetSamplerParameterfv;
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterIiv" })]
		[NativeType("void glGetSamplerParameterIiv(GLuint sampler, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetSamplerParameterIiv;
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterIuiv" })]
		[NativeType("void glGetSamplerParameterIuiv(GLuint sampler, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glGetSamplerParameterIuiv;

	}

	public class ARBSamplerObjects {
		
		public GL GL { get; }
		public ARBSamplerObjectsFunctions Functions { get; } = new();

		public ARBSamplerObjects(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenSamplers(Span<uint> samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glGenSamplers(samplers.Length, pSamplers);
				}
			}
			return samplers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenSamplers(int n) {
			uint[] samplers = new uint[n];
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glGenSamplers(n, pSamplers);
				}
			}
			return samplers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenSamplers() {
			uint sampler = 0;
			unsafe {
				Functions.glGenSamplers(1, &sampler);
			}
			return sampler;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(in ReadOnlySpan<uint> samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glDeleteSamplers(samplers.Length, pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(params uint[] samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glDeleteSamplers(samplers.Length, pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(uint sampler) {
			unsafe {
				Functions.glDeleteSamplers(1, &sampler);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSampler(uint sampler) {
			unsafe {
				return Functions.glIsSampler(sampler) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSampler(uint unit, uint sampler) {
			unsafe {
				Functions.glBindSampler(unit, sampler);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, int value) {
			unsafe {
				Functions.glSamplerParameteri(sampler, (uint)pname, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, float value) {
			unsafe {
				Functions.glSamplerParameterf(sampler, (uint)pname, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glSamplerParameteriv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params int[] values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameteriv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<float> values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glSamplerParameterfv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params float[] values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glSamplerParameterfv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameterIiv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params int[] values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameterIiv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glSamplerParameterIuiv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params uint[] values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glSamplerParameterIuiv(sampler, (uint)pname, pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSamplerParameteri(uint sampler, GLSamplerParameter pname) {
			int value = 0;
			unsafe {
				Functions.glGetSamplerParameteriv(sampler, (uint)pname, &value);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetSamplerParamterf(uint sampler, GLSamplerParameter pname) {
			float value = 0;
			unsafe {
				Functions.glGetSamplerParameterfv(sampler, (uint)pname, &value);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetSamplerParameteriv(sampler, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<float> values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glGetSamplerParameterfv(sampler, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetSamplerParameterIiv(sampler, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glGetSamplerParameterIuiv(sampler, (uint)pname, pValues);
				}
			}
			return values;
		}

	}

}
