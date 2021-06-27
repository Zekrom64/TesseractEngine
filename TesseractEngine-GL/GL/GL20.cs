using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {
	public class GL20 : IGLObject {

		public GL GL { get; }
		public GL20Functions Functions { get; }

		public GL20(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL20Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AttachShader(uint program, uint shader) => Functions.glAttachShader(program, shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindAttribLocation(uint program, uint index, string name) => Functions.glBindAttribLocation(program, index, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(GLBlendFunction modeRGB, GLBlendFunction modeAlpha) => Functions.glBlendEquationSeparate((uint)modeRGB, (uint)modeAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompileShader(uint shader) => Functions.glCompileShader(shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgram() => Functions.glCreateProgram();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShader(GLShaderType type) => Functions.glCreateShader((uint)type);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgram(uint program) => Functions.glDeleteProgram(program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteShader(uint shader) => Functions.glDeleteShader(shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DetachShader(uint program, uint shader) => Functions.glDetachShader(program, shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexAttribArray(uint index) => Functions.glDisableVertexAttribArray(index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawBuffers(ReadOnlySpan<GLDrawBuffer> bufs) {
			unsafe {
				fixed(GLDrawBuffer* pBufs = bufs) {
					Functions.glDrawBuffers(bufs.Length, (IntPtr)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexAttribArray(uint index) => Functions.glEnableVertexAttribArray(index);

		public void GetActiveAttrib(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			Functions.glGetProgramiv(program, GLEnums.GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, out int maxLen);
			Span<byte> nameBytes = stackalloc byte[maxLen];
			uint utype = 0;
			unsafe {
				fixed(byte* pNameBytes = nameBytes) {
					Functions.glGetActiveAttrib(program, index, maxLen, out int length, out size, out utype, (IntPtr)pNameBytes);
				}
			}
			type = (GLShaderAttribType)utype;
			name = MemoryUtil.GetStringASCII(nameBytes);
		}

		public void GetActiveUniform(uint program, uint index, out int size, out GLShaderUniformType type, out string name) {
			Functions.glGetProgramiv(program, GLEnums.GL_ACTIVE_UNIFORM_MAX_LENGTH, out int maxLen);
			Span<byte> nameBytes = stackalloc byte[maxLen];
			uint utype = 0;
			unsafe {
				fixed (byte* pNameBytes = nameBytes) {
					Functions.glGetActiveUniform(program, index, maxLen, out int length, out size, out utype, (IntPtr)pNameBytes);
				}
			}
			type = (GLShaderUniformType)utype;
			name = MemoryUtil.GetStringASCII(nameBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetAttachedShaders(uint program, in Span<uint> shaders) {
			int count = 0;
			unsafe {
				fixed(uint* pShaders = shaders) {
					Functions.glGetAttachedShaders(program, shaders.Length, out count, (IntPtr)pShaders);
				}
			}
			return shaders.Slice(0, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetAttribLocation(uint program, string name) => Functions.glGetAttribLocation(program, name);

		public string GetProgramInfoLog(uint program) {
			Functions.glGetProgramiv(program, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
			Span<byte> logBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pInfoLog = logBytes) {
					Functions.glGetProgramInfoLog(program, maxLen, out length, (IntPtr)pInfoLog);
				}
			}
			return MemoryUtil.GetStringASCII(logBytes.Slice(0, length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgram(uint program, GLGetProgram pname) {
			Functions.glGetProgramiv(program, (uint)pname, out int param);
			return param;
		}

		public string GetShaderInfoLog(uint shader) {
			Functions.glGetShaderiv(shader, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
			Span<byte> logBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pInfoLog = logBytes) {
					Functions.glGetShaderInfoLog(shader, maxLen, out length, (IntPtr)pInfoLog);
				}
			}
			return MemoryUtil.GetStringASCII(logBytes.Slice(0, length));
		}

		public string GetShaderSource(uint shader) {
			Functions.glGetShaderiv(shader, GLEnums.GL_SHADER_SOURCE_LENGTH, out int maxLen);
			Span<byte> srcBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pSrc = srcBytes) {
					Functions.glGetShaderSource(shader, maxLen, out length, (IntPtr)pSrc);
				}
			}
			return MemoryUtil.GetStringASCII(srcBytes.Slice(0, length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShader(uint shader, GLGetShader pname) {
			Functions.glGetShaderiv(shader, (uint)pname, out int param);
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetUniformLocation(uint program, string name) => Functions.glGetUniformLocation(program, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LinkProgram(uint program) => Functions.glLinkProgram(program);

		public void ShaderSource(uint shader, string source) {
			byte[] srcBytes = Encoding.ASCII.GetBytes(source);
			unsafe {
				int length = srcBytes.Length;
				fixed (byte* pSrcBytes = srcBytes) {
					Functions.glShaderSource(shader, 1, (IntPtr)(&pSrcBytes), (IntPtr)(&length));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFuncSeparate(GLStencilFunc frontFunc, GLStencilFunc backFunc, int reference, uint mask) => Functions.glStencilFuncSeparate((uint)frontFunc, (uint)backFunc, reference, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilMaskSeparate(GLFace face, uint mask) => Functions.glStencilMasksSeparate((uint)face, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOpSeparate(GLFace face, GLStencilOp sfail, GLStencilOp dpfail, GLStencilOp dppass) => Functions.glStencilOpSeparate((uint)face, (uint)sfail, (uint)dpfail, (uint)dppass);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Uniform(int location, int v0) => Functions.glUniform1i(location, v0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgram(uint program) => Functions.glUseProgram(program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribPointer(uint index, int size, GLType type, bool normalized, int stride, nint offset) => Functions.glVertexAttribPointer(index, size, (uint)type, (byte)(normalized ? 1 : 0), stride, offset);

	}
}
