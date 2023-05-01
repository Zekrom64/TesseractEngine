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

	public unsafe class ARBShaderSubroutineFunctions {

		[NativeType("GLint glGetSubroutineUniformLocation(GLuint program, GLenum shaderType, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, int> glGetSubroutineUniformLocation;
		[NativeType("GLuint glGetSubroutineIndex(GLuint program, GLenum shaderTYpe, const char* name)")]
		public delegate* unmanaged<uint, uint, byte*, uint> glGetSubroutineIndex;
		[NativeType("void glGetActiveSubroutineUniformiv(GLuint program, GLenum shaderType, GLuint index, GLenum pname, GLint* pValues)")]
		public delegate* unmanaged<uint, uint, uint, uint, int*, void> glGetActiveSubroutineUniformiv;
		[NativeType("void glGetActiveSubroutineUniformName(GLuint program, GLenum shaderType, GLuint index, GLsizei bufSize, GLsizei* pLength, GLchar* pName)")]
		public delegate* unmanaged<uint, uint, uint, int, out int, byte*, void> glGetActiveSubroutineUniformName;
		[NativeType("void glGetActiveSubroutineName(GLuint program, GLenum shaderType, GLuint index, GLsizei bufSize, GLsizei* pLength, GLchar* pName)")]
		public delegate* unmanaged<uint, uint, uint, int, out int, byte*, void> glGetActiveSubroutineName;
		[NativeType("void glUniformSubroutinesuiv(GLenum shaderType, GLsizei count, const GLuint* pIndices)")]
		public delegate* unmanaged<uint, int, uint*, void> glUniformSubroutinesuiv;
		[NativeType("void glGetUniformSubroutineuiv(GLenum shaderType, GLint location, GLuint* pParams)")]
		public delegate* unmanaged<uint, int, uint*, void> glGetUniformSubroutineuiv;
		[NativeType("void glGetProgramStageiv(GLuint program, GLenum shaderType, GLenum pname, GLint* pValues)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetProgramStageiv;

	}

	public class ARBShaderSubroutine : IGLObject {

		public GL GL { get; }
		public ARBShaderSubroutineFunctions Functions { get; } = new();

		public ARBShaderSubroutine(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSubroutineUniformLocation(uint program, GLShaderType shaderType, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetSubroutineUniformLocation(program, (uint)shaderType, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetSubroutineIndex(uint program, GLShaderType shaderType, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return Functions.glGetSubroutineIndex(program, (uint)shaderType, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname) {
			int value = 0;
			unsafe {
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, (uint)pname, &value);
			}
			return value;
		}

		public string GetActiveSubroutineUniformName(uint program, GLShaderType shaderType, uint index) {
			unsafe {
				int len = 0;
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, GLEnums.GL_UNIFORM_NAME_LENGTH, &len);
				Span<byte> name = stackalloc byte[len];
				fixed(byte* pName = name) {
					Functions.glGetActiveSubroutineUniformName(program, (uint)shaderType, index, len, out len, pName);
				}
				return MemoryUtil.GetASCII(name[..len]);
			}
		}

		public string GetActiveSubroutineName(uint program, GLShaderType shaderType, uint index) {
			unsafe {
				int len = 0;
				Functions.glGetActiveSubroutineUniformiv(program, (uint)shaderType, index, GLEnums.GL_UNIFORM_NAME_LENGTH, &len);
				Span<byte> name = stackalloc byte[len];
				fixed (byte* pName = name) {
					Functions.glGetActiveSubroutineName(program, (uint)shaderType, index, len, out len, pName);
				}
				return MemoryUtil.GetASCII(name[..len]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, in ReadOnlySpan<uint> indices) {
			unsafe {
				fixed(uint* pIndices = indices) {
					Functions.glUniformSubroutinesuiv((uint)shaderType, indices.Length, pIndices);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, params uint[] indices) {
			unsafe {
				fixed (uint* pIndices = indices) {
					Functions.glUniformSubroutinesuiv((uint)shaderType, indices.Length, pIndices);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformSubroutine(GLShaderType shaderType, int location) {
			uint value = 0;
			unsafe {
				Functions.glGetUniformSubroutineuiv((uint)shaderType, location, &value);
			}
			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramStage(uint program, GLShaderType type, GLGetProgramStage pname) {
			int value = 0;
			unsafe {
				Functions.glGetProgramStageiv(program, (uint)type, (uint)pname, &value);
			}
			return value;
		}

	}

}
