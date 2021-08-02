using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class ARBSeparateShaderObjectsFunctions {

		public delegate void PFN_glUseProgramStages(uint pipeline, uint stages, uint program);
		[ExternFunction(AltNames = new string[] { "glUseProgramStagesARB" })]
		public PFN_glUseProgramStages glUseProgramStages;
		public delegate void PFN_glActiveShaderProgram(uint pipeline, uint program);
		[ExternFunction(AltNames = new string[] { "glActiveShaderProgramARB" })]
		public PFN_glActiveShaderProgram glActiveShaderProgram;
		public delegate uint PFN_glCreateShaderProgramv(uint type, int count, [NativeType("const char**")] IntPtr strings);
		[ExternFunction(AltNames = new string[] { "glCreateShaderProgramvARB" })]
		public PFN_glCreateShaderProgramv glCreateShaderProgramv;
		public delegate void PFN_glBindProgramPipeline(uint pipeline);
		[ExternFunction(AltNames = new string[] { "glBindProgramPipelineARB" })]
		public PFN_glBindProgramPipeline glBindProgramPipeline;
		public delegate void PFN_glDeleteProgramPipelines(int n, [NativeType("const GLuint*")] IntPtr pipelines);
		[ExternFunction(AltNames = new string[] { "glDeleteProgramPipelinesARB" })]
		public PFN_glDeleteProgramPipelines glDeleteProgramPipelines;
		public delegate void PFN_glGenProgramPipelines(int n, [NativeType("GLuint*")] IntPtr pipelines);
		[ExternFunction(AltNames = new string[] { "glGenProgramPipelinesARB" })]
		public PFN_glGenProgramPipelines glGenProgramPipelines;
		public delegate byte PFN_glIsProgramPipeline(uint pipeline);
		[ExternFunction(AltNames = new string[] { "glIsProgramPipelineARB" })]
		public PFN_glIsProgramPipeline glIsProgramPipeline;
		public delegate void PFN_glProgramParameteri(uint program, uint pname, int value);
		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		public PFN_glProgramParameteri glProgramParameteri;
		public delegate void PFN_glGetProgramPipelineiv(uint pipeline, uint name, out int param);
		[ExternFunction(AltNames = new string[] { "glGetProgramPipelineivARB" })]
		public PFN_glGetProgramPipelineiv glGetProgramPipelineiv;
		public delegate void PFN_glProgramUniform1i(uint program, int location, int x);
		[ExternFunction(AltNames = new string[] { "glProgramUniform1iARB" })]
		public PFN_glProgramUniform1i glProgramUniform1i;
		public delegate void PFN_glValidateProgramPipeline(uint pipeline);
		[ExternFunction(AltNames = new string[] { "glValidateProrgamPipelineARB" })]
		public PFN_glValidateProgramPipeline glValidateProgramPipeline;
		public delegate void PFN_glGetProgramPipelineInfoLog(uint pipeline, int bufSize, out int length, [NativeType("char*")] IntPtr infoLog);
		[ExternFunction(AltNames = new string[] { "glGetProgramPipelineInfoLogARB" })]
		public PFN_glGetProgramPipelineInfoLog glGetProgramPipelineInfoLog;

	}

	public class ARBSeparateShaderObjects : IGLObject {

		public GL GL { get; }
		public ARBSeparateShaderObjectsFunctions Functions { get; } = new();

		public ARBSeparateShaderObjects(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgramStages(uint pipeline, GLShaderStages stages, uint program) => Functions.glUseProgramStages(pipeline, (uint)stages, program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ActiveShaderProgram(uint pipeline, uint program) => Functions.glActiveShaderProgram(pipeline, program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShaderProgram(GLShaderType type, string source) {
			byte[] srcbytes = Encoding.ASCII.GetBytes(source);
			unsafe {
				fixed(byte* pSrcbytes = srcbytes) {
					return Functions.glCreateShaderProgramv((uint)type, 1, (IntPtr)(&pSrcbytes));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindProgramPipeline(uint pipeline) => Functions.glBindProgramPipeline(pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(in ReadOnlySpan<uint> pipelines) {
			unsafe {
				fixed(uint* pPipelines = pipelines) {
					Functions.glDeleteProgramPipelines(pipelines.Length, (IntPtr)pPipelines);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(params uint[] pipelines) {
			unsafe {
				fixed (uint* pPipelines = pipelines) {
					Functions.glDeleteProgramPipelines(pipelines.Length, (IntPtr)pPipelines);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(uint pipeline) {
			unsafe {
				Functions.glDeleteProgramPipelines(1, (IntPtr)(&pipeline));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenProgramPipelines(Span<uint> pipelines) {
			unsafe {
				fixed(uint* pPipelines = pipelines) {
					Functions.glGenProgramPipelines(pipelines.Length, (IntPtr)pPipelines);
				}
			}
			return pipelines;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenProgramPipelines(int n) {
			uint[] pipelines = new uint[n];
			unsafe {
				fixed (uint* pPipelines = pipelines) {
					Functions.glGenProgramPipelines(pipelines.Length, (IntPtr)pPipelines);
				}
			}
			return pipelines;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenProgramPipelines() {
			uint pipeline = 0;
			unsafe {
				Functions.glGenProgramPipelines(1, (IntPtr)(&pipeline));
			}
			return pipeline;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsProgramPipeline(uint pipeline) => Functions.glIsProgramPipeline(pipeline) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParameteri(uint program, GLProgramParameter pname, int value) => Functions.glProgramParameteri(program, (uint)pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramPipelinei(uint program, GLGetProgramPipeline pname) {
			Functions.glGetProgramPipelineiv(program, (uint)pname, out int value);
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramUniform(uint program, int location, int x) => Functions.glProgramUniform1i(program, location, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ValidateProgramPipeline(uint pipeline) => Functions.glValidateProgramPipeline(pipeline);

		public string GetProgramPipelineInfoLog(uint pipeline) {
			int length = GetProgramPipelinei(pipeline, GLGetProgramPipeline.InfoLogLength);
			byte[] bytes = new byte[length];
			unsafe {
				fixed(byte* pBytes = bytes) {
					Functions.glGetProgramPipelineInfoLog(pipeline, length, out length, (IntPtr)pBytes);
				}
			}
			return Encoding.ASCII.GetString(bytes[..length]);
		}

	}

}
