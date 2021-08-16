using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public class ARBShaderSubroutineFunctions {

		public delegate int PFN_glGetSubroutineUniformLocation(uint program, uint shadertype, [MarshalAs(UnmanagedType.LPStr)] string name);
		public PFN_glGetSubroutineUniformLocation glGetSubroutineUniformLocation;
		public delegate uint PFN_glGetSubroutineIndex(uint program, uint shadertype, [MarshalAs(UnmanagedType.LPStr)] string name);
		public PFN_glGetSubroutineIndex glGetSubroutineIndex;
		public delegate void PFN_glGetActiveSubroutineUniformiv(uint program, uint shadertype, uint index, uint pname, [NativeType("GLint*")] IntPtr values);
		public PFN_glGetActiveSubroutineUniformiv glGetActiveSubroutineUniformiv;
		public delegate void PFN_glGetActiveSubroutineUniformName(uint program, uint shadertype, uint index, int bufsize, out int length, [NativeType("GLchar*")] IntPtr name);
		public PFN_glGetActiveSubroutineUniformName glGetActiveSubroutineUniformName;
		public delegate void PFN_glGetActiveSubroutineName(uint program, uint shadertype, uint index, int bufsize, out int length, [NativeType("GLchar*")] IntPtr name);
		public PFN_glGetActiveSubroutineName glGetActiveSubroutineName;
		public delegate void PFN_glUniformSubroutinesuiv(uint shadertype, int count, [NativeType("const GLuint*")] IntPtr indices);
		public PFN_glUniformSubroutinesuiv glUniformSubroutinesuiv;
		public delegate void PFN_glGetUniformSubroutineuiv(uint shadertype, int location, [NativeType("GLuint*")] IntPtr _params);
		public PFN_glGetUniformSubroutineuiv glGetUniformSubroutineuiv;
		public delegate void PFN_glGetProgramStageiv(uint program, uint shadertype, uint pname, [NativeType("GLint*")] IntPtr values);
		public PFN_glGetProgramStageiv glGetProgramStageiv;

	}

	public class ARBShaderSubroutine : IGLObject {

		public GL GL { get; }
		public ARBShaderSubroutineFunctions Functions { get; } = new();

		public ARBShaderSubroutine(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSubroutineUniformLocation(uint program, GLShaderType shaderType, string name) => Functions.glGetSubroutineUniformLocation(program, (uint)shaderType, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetSubroutineIndex(uint program, GLShaderType shaderType, string name) => Functions.glGetSubroutineIndex(program, (uint)shaderType, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname) {
			int value = 0;
			unsafe {
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, (uint)pname, (IntPtr)(&value));
			}
			return value;
		}

		public string GetActiveSubroutineUniformName(uint program, GLShaderType shaderType, uint index) {
			unsafe {
				int len = 0;
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, GLEnums.GL_UNIFORM_NAME_LENGTH, (IntPtr)(&len));
				Span<byte> name = stackalloc byte[len];
				fixed(byte* pName = name) {
					Functions.glGetActiveSubroutineUniformName(program, (uint)shaderType, index, len, out len, (IntPtr)pName);
				}
				return MemoryUtil.GetStringASCII(name[..len]);
			}
		}

		public string GetActiveSubroutineName(uint program, GLShaderType shaderType, uint index) {
			unsafe {
				int len = 0;
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, GLEnums.GL_UNIFORM_NAME_LENGTH, (IntPtr)(&len));
				Span<byte> name = stackalloc byte[len];
				fixed (byte* pName = name) {
					Functions.glGetActiveSubroutineName(program, (uint)shaderType, index, len, out len, (IntPtr)pName);
				}
				return MemoryUtil.GetStringASCII(name[..len]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, in ReadOnlySpan<uint> indices) {
			unsafe {
				fixed(uint* pIndices = indices) {
					Functions.glUniformSubroutinesuiv((uint)shaderType, indices.Length, (IntPtr)pIndices);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, params uint[] indices) {
			unsafe {
				fixed (uint* pIndices = indices) {
					Functions.glUniformSubroutinesuiv((uint)shaderType, indices.Length, (IntPtr)pIndices);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformSubroutine(GLShaderType shaderType, int location) {
			uint value = 0;
			unsafe {
				Functions.glGetUniformSubroutineuiv((uint)shaderType, location, (IntPtr)(&value));
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramStage(uint program, GLShaderType type, GLGetProgramStage pname) {
			int value = 0;
			unsafe {
				Functions.glGetProgramStageiv(program, (uint)type, (uint)pname, (IntPtr)(&value));
			}
			return value;
		}

	}

}
