using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {
	
	public class ARBSamplerObjectsFunctions {

		public delegate void PFN_glGenSamplers(int count, [NativeType("GLuint*")] IntPtr samplers);
		[ExternFunction(AltNames = new string[] { "glGenSamplersARB" })]
		public PFN_glGenSamplers glGenSamplers;
		public delegate void PFN_glDeleteSamplers(int count, [NativeType("const GLuint*")] IntPtr samplers);
		[ExternFunction(AltNames = new string[] { "glDeleteSamplersARB" })]
		public PFN_glDeleteSamplers glDeleteSamplers;
		public delegate byte PFN_glIsSampler(uint sampler);
		[ExternFunction(AltNames = new string[] { "glIsSamplerARB" })]
		public PFN_glIsSampler glIsSampler;
		public delegate void PFN_glBindSmapler(uint unit, uint sampler);
		[ExternFunction(AltNames = new string[] { "glBindSamplerARB" })]
		public PFN_glBindSmapler glBindSmapler;
		public delegate void PFN_glSamplerParameteri(uint sampler, uint pname, int param);
		[ExternFunction(AltNames = new string[] { "glSamplerParameteriARB" })]
		public PFN_glSamplerParameteri glSamplerParameteri;
		public delegate void PFN_glSamplerParameterf(uint sampler, uint pname, float param);
		[ExternFunction(AltNames = new string[] { "glSamplerParameterfARB" })]
		public PFN_glSamplerParameterf glSamplerParameterf;
		public delegate void PFN_glSamplerParameteriv(uint sampler, uint pname, [NativeType("const GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glSamplerParameterivARB" })]
		public PFN_glSamplerParameteriv glSamplerParameteriv;
		public delegate void PFN_glSamplerParameterfv(uint sampler, uint pname, [NativeType("const GLfloat*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glSamplerParameterfvARB" })]
		public PFN_glSamplerParameterfv glSamplerParameterfv;
		public delegate void PFN_glSamplerParameterIiv(uint sampler, uint pname, [NativeType("const GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glSamplerParameterIivARB" })]
		public PFN_glSamplerParameterIiv glSamplerParameterIiv;
		public delegate void PFN_glSamplerParameterIuiv(uint sampler, uint pname, [NativeType("const GLuint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glSamplerParameterIuivARB" })]
		public PFN_glSamplerParameterIuiv glSamplerParameterIuiv;
		public delegate void PFN_glGetSamplerParameteriv(uint sampler, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameteriv" })]
		public PFN_glGetSamplerParameteriv glGetSamplerParameteriv;
		public delegate void PFN_glGetSamplerParameterfv(uint sampler, uint pname, [NativeType("GLfloat*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterfv" })]
		public PFN_glGetSamplerParameterfv glGetSamplerParameterfv;
		public delegate void PFN_glGetSamplerParameterIiv(uint sampler, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterIiv" })]
		public PFN_glGetSamplerParameterIiv glGetSamplerParameterIiv;
		public delegate void PFN_glGetSamplerParameterIuiv(uint sampler, uint pname, [NativeType("GLuint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetSamplerParameterIuiv" })]
		public PFN_glGetSamplerParameterIuiv glGetSamplerParameterIuiv;

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
					Functions.glGenSamplers(samplers.Length, (IntPtr)pSamplers);
				}
			}
			return samplers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenSamplers(int n) {
			uint[] samplers = new uint[n];
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glGenSamplers(n, (IntPtr)pSamplers);
				}
			}
			return samplers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenSamplers() {
			uint sampler = 0;
			unsafe {
				Functions.glGenSamplers(1, (IntPtr)(&sampler));
			}
			return sampler;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(in ReadOnlySpan<uint> samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glDeleteSamplers(samplers.Length, (IntPtr)pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(params uint[] samplers) {
			unsafe {
				fixed(uint* pSamplers = samplers) {
					Functions.glDeleteSamplers(samplers.Length, (IntPtr)pSamplers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(uint sampler) {
			unsafe {
				Functions.glDeleteSamplers(1, (IntPtr)(&sampler));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSampler(uint sampler) => Functions.glIsSampler(sampler) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSampler(uint unit, uint sampler) => Functions.glBindSmapler(unit, sampler);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, int value) => Functions.glSamplerParameteri(sampler, (uint)pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, float value) => Functions.glSamplerParameterf(sampler, (uint)pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glSamplerParameteriv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params int[] values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameteriv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<float> values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glSamplerParameterfv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params float[] values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glSamplerParameterfv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameterIiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params int[] values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glSamplerParameterIiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glSamplerParameterIuiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params uint[] values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glSamplerParameterIuiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSamplerParameteri(uint sampler, GLSamplerParameter pname) {
			int value = 0;
			unsafe {
				Functions.glGetSamplerParameteriv(sampler, (uint)pname, (IntPtr)(&value));
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetSamplerParamterf(uint sampler, GLSamplerParameter pname) {
			float value = 0;
			unsafe {
				Functions.glGetSamplerParameterfv(sampler, (uint)pname, (IntPtr)(&value));
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetSamplerParameteriv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<float> values) {
			unsafe {
				fixed (float* pValues = values) {
					Functions.glGetSamplerParameterfv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetSamplerParameterIiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glGetSamplerParameterIuiv(sampler, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

	}

}
