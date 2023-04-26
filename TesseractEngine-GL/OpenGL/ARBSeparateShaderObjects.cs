using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBSeparateShaderObjectsFunctions {

		[ExternFunction(AltNames = new string[] { "glUseProgramStagesARB" })]
		[NativeType("void glUseProgramStages(GLuint pipeline, GLbitfield stages, GLuint program)")]
		public delegate* unmanaged<uint, uint, uint, void> glUseProgramStages;
		[ExternFunction(AltNames = new string[] { "glActiveShaderProgramARB" })]
		[NativeType("void glActiveShaderProgram(GLuint pipeline, GLuint program)")]
		public delegate* unmanaged<uint, uint, void> glActiveShaderProgram;
		[ExternFunction(AltNames = new string[] { "glCreateShaderProgramvARB" })]
		[NativeType("GLuint glCreateShaderProgramv(GLenum type, GLsizei count, const char* const* pStrings)")]
		public delegate* unmanaged<uint, int, byte**, uint> glCreateShaderProgramv;
		[ExternFunction(AltNames = new string[] { "glBindProgramPipelineARB" })]
		[NativeType("void glBindProgramPipeline(GLuint pipeline)")]
		public delegate* unmanaged<uint, void> glBindProgramPipeline;
		[ExternFunction(AltNames = new string[] { "glDeleteProgramPipelinesARB" })]
		[NativeType("void glDeleteProgramPipelines(GLsizei n, const GLuint* pPipelines)")]
		public delegate* unmanaged<int, uint*, void> glDeleteProgramPipelines;
		[ExternFunction(AltNames = new string[] { "glGenProgramPipelinesARB" })]
		[NativeType("void glGenProgramPipelines(GLsizei n, GLuint* pPipelines)")]
		public delegate* unmanaged<int, uint*, void> glGenProgramPipelines;
		[ExternFunction(AltNames = new string[] { "glIsProgramPipelineARB" })]
		[NativeType("GLboolean glIsProgramPipeline(GLuint pipeline)")]
		public delegate* unmanaged<uint, byte> glIsProgramPipeline;
		[ExternFunction(AltNames = new string[] { "glProgramParameteriARB" })]
		[NativeType("void glProgramParameteri(GLuint program, GLenum pname, GLint value)")]
		public delegate* unmanaged<uint, uint, int, void> glProgramParameteri;
		[ExternFunction(AltNames = new string[] { "glGetProgramPipelineivARB" })]
		[NativeType("void glGetProgramPipelineiv(GLuint pipeline, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, out int, void> glGetProgramPipelineiv;
		[ExternFunction(AltNames = new string[] { "glProgramUniform1iARB" })]
		[NativeType("void glProgramUniform1i(GLuint program, GLint location, GLint x)")]
		public delegate* unmanaged<uint, int, int, void> glProgramUniform1i;
		[ExternFunction(AltNames = new string[] { "glValidateProrgamPipelineARB" })]
		[NativeType("void glValidateProgramPipeline(GLuint pipeline)")]
		public delegate* unmanaged<uint, void> glValidateProgramPipeline;
		[ExternFunction(AltNames = new string[] { "glGetProgramPipelineInfoLogARB" })]
		[NativeType("void glGetProgramPipelineInfoLog(GLuint pipeline, GLsizei bufSize, GLsizei* pLength, char* pInfoLog)")]
		public delegate* unmanaged<uint, int, out int, byte*, void> glGetProgramPipelineInfoLog;

	}

	public class ARBSeparateShaderObjects : IGLObject {

		public GL GL { get; }
		public ARBSeparateShaderObjectsFunctions Functions { get; } = new();

		public ARBSeparateShaderObjects(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgramStages(uint pipeline, GLShaderStages stages, uint program) {
			unsafe {
				Functions.glUseProgramStages(pipeline, (uint)stages, program);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ActiveShaderProgram(uint pipeline, uint program) {
			unsafe {
				Functions.glActiveShaderProgram(pipeline, program);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShaderProgram(GLShaderType type, string source) {
			byte[] srcbytes = Encoding.ASCII.GetBytes(source);
			unsafe {
				fixed(byte* pSrcbytes = srcbytes) {
					return Functions.glCreateShaderProgramv((uint)type, 1, &pSrcbytes);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindProgramPipeline(uint pipeline) {
			unsafe {
				Functions.glBindProgramPipeline(pipeline);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(in ReadOnlySpan<uint> pipelines) {
			unsafe {
				fixed(uint* pPipelines = pipelines) {
					Functions.glDeleteProgramPipelines(pipelines.Length, pPipelines);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(params uint[] pipelines) {
			unsafe {
				fixed (uint* pPipelines = pipelines) {
					Functions.glDeleteProgramPipelines(pipelines.Length, pPipelines);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(uint pipeline) {
			unsafe {
				Functions.glDeleteProgramPipelines(1, &pipeline);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenProgramPipelines(Span<uint> pipelines) {
			unsafe {
				fixed(uint* pPipelines = pipelines) {
					Functions.glGenProgramPipelines(pipelines.Length, pPipelines);
				}
			}
			return pipelines;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenProgramPipelines(int n) {
			uint[] pipelines = new uint[n];
			unsafe {
				fixed (uint* pPipelines = pipelines) {
					Functions.glGenProgramPipelines(pipelines.Length, pPipelines);
				}
			}
			return pipelines;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenProgramPipelines() {
			uint pipeline = 0;
			unsafe {
				Functions.glGenProgramPipelines(1, &pipeline);
			}
			return pipeline;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsProgramPipeline(uint pipeline) {
			unsafe {
				return Functions.glIsProgramPipeline(pipeline) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParameteri(uint program, GLProgramParameter pname, int value) {
			unsafe {
				Functions.glProgramParameteri(program, (uint)pname, value);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramPipelinei(uint program, GLGetProgramPipeline pname) {
			unsafe {
				Functions.glGetProgramPipelineiv(program, (uint)pname, out int value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramUniform(uint program, int location, int x) {
			unsafe {
				Functions.glProgramUniform1i(program, location, x);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ValidateProgramPipeline(uint pipeline) {
			unsafe {
				Functions.glValidateProgramPipeline(pipeline);
			}
		}

		public string GetProgramPipelineInfoLog(uint pipeline) {
			int length = GetProgramPipelinei(pipeline, GLGetProgramPipeline.InfoLogLength);
			byte[] bytes = new byte[length];
			unsafe {
				fixed(byte* pBytes = bytes) {
					Functions.glGetProgramPipelineInfoLog(pipeline, length, out length, pBytes);
				}
			}
			return Encoding.ASCII.GetString(bytes[..length]);
		}

	}

}
