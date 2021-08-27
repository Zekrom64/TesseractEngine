using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {
	public class GL20 : GL15 {

		public GL20Functions FunctionsGL20 { get; } = new();

		public GL20(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL20);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AttachShader(uint program, uint shader) => FunctionsGL20.glAttachShader(program, shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindAttribLocation(uint program, uint index, string name) => FunctionsGL20.glBindAttribLocation(program, index, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(GLBlendFunction modeRGB, GLBlendFunction modeAlpha) => FunctionsGL20.glBlendEquationSeparate((uint)modeRGB, (uint)modeAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompileShader(uint shader) => FunctionsGL20.glCompileShader(shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgram() => FunctionsGL20.glCreateProgram();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShader(GLShaderType type) => FunctionsGL20.glCreateShader((uint)type);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgram(uint program) => FunctionsGL20.glDeleteProgram(program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteShader(uint shader) => FunctionsGL20.glDeleteShader(shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DetachShader(uint program, uint shader) => FunctionsGL20.glDetachShader(program, shader);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexAttribArray(uint index) => FunctionsGL20.glDisableVertexAttribArray(index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawBuffers(ReadOnlySpan<GLDrawBuffer> bufs) {
			unsafe {
				fixed(GLDrawBuffer* pBufs = bufs) {
					FunctionsGL20.glDrawBuffers(bufs.Length, (IntPtr)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexAttribArray(uint index) => FunctionsGL20.glEnableVertexAttribArray(index);

		public void GetActiveAttrib(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			FunctionsGL20.glGetProgramiv(program, GLEnums.GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, out int maxLen);
			Span<byte> nameBytes = stackalloc byte[maxLen];
			uint utype = 0;
			unsafe {
				fixed(byte* pNameBytes = nameBytes) {
					FunctionsGL20.glGetActiveAttrib(program, index, maxLen, out int length, out size, out utype, (IntPtr)pNameBytes);
				}
			}
			type = (GLShaderAttribType)utype;
			name = MemoryUtil.GetASCII(nameBytes);
		}

		public void GetActiveUniform(uint program, uint index, out int size, out GLShaderUniformType type, out string name) {
			FunctionsGL20.glGetProgramiv(program, GLEnums.GL_ACTIVE_UNIFORM_MAX_LENGTH, out int maxLen);
			Span<byte> nameBytes = stackalloc byte[maxLen];
			uint utype = 0;
			unsafe {
				fixed (byte* pNameBytes = nameBytes) {
					FunctionsGL20.glGetActiveUniform(program, index, maxLen, out int length, out size, out utype, (IntPtr)pNameBytes);
				}
			}
			type = (GLShaderUniformType)utype;
			name = MemoryUtil.GetASCII(nameBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetAttachedShaders(uint program, in Span<uint> shaders) {
			int count = 0;
			unsafe {
				fixed(uint* pShaders = shaders) {
					FunctionsGL20.glGetAttachedShaders(program, shaders.Length, out count, (IntPtr)pShaders);
				}
			}
			return shaders.Slice(0, count);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetAttribLocation(uint program, string name) => FunctionsGL20.glGetAttribLocation(program, name);

		public string GetProgramInfoLog(uint program) {
			FunctionsGL20.glGetProgramiv(program, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
			Span<byte> logBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pInfoLog = logBytes) {
					FunctionsGL20.glGetProgramInfoLog(program, maxLen, out length, (IntPtr)pInfoLog);
				}
			}
			return MemoryUtil.GetASCII(logBytes.Slice(0, length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgram(uint program, GLGetProgram pname) {
			FunctionsGL20.glGetProgramiv(program, (uint)pname, out int param);
			return param;
		}

		public string GetShaderInfoLog(uint shader) {
			FunctionsGL20.glGetShaderiv(shader, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
			Span<byte> logBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pInfoLog = logBytes) {
					FunctionsGL20.glGetShaderInfoLog(shader, maxLen, out length, (IntPtr)pInfoLog);
				}
			}
			return MemoryUtil.GetASCII(logBytes.Slice(0, length));
		}

		public string GetShaderSource(uint shader) {
			FunctionsGL20.glGetShaderiv(shader, GLEnums.GL_SHADER_SOURCE_LENGTH, out int maxLen);
			Span<byte> srcBytes = new(new byte[maxLen]);
			int length = 0;
			unsafe {
				fixed(byte* pSrc = srcBytes) {
					FunctionsGL20.glGetShaderSource(shader, maxLen, out length, (IntPtr)pSrc);
				}
			}
			return MemoryUtil.GetASCII(srcBytes.Slice(0, length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShader(uint shader, GLGetShader pname) {
			FunctionsGL20.glGetShaderiv(shader, (uint)pname, out int param);
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetUniformLocation(uint program, string name) => FunctionsGL20.glGetUniformLocation(program, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LinkProgram(uint program) => FunctionsGL20.glLinkProgram(program);

		public void ShaderSource(uint shader, string source) {
			byte[] srcBytes = Encoding.ASCII.GetBytes(source);
			unsafe {
				int length = srcBytes.Length;
				fixed (byte* pSrcBytes = srcBytes) {
					FunctionsGL20.glShaderSource(shader, 1, (IntPtr)(&pSrcBytes), (IntPtr)(&length));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFuncSeparate(GLStencilFunc frontFunc, GLStencilFunc backFunc, int reference, uint mask) => FunctionsGL20.glStencilFuncSeparate((uint)frontFunc, (uint)backFunc, reference, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilMaskSeparate(GLFace face, uint mask) => FunctionsGL20.glStencilMaskSeparate((uint)face, mask);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOpSeparate(GLFace face, GLStencilOp sfail, GLStencilOp dpfail, GLStencilOp dppass) => FunctionsGL20.glStencilOpSeparate((uint)face, (uint)sfail, (uint)dpfail, (uint)dppass);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Uniform(int location, int v0) => FunctionsGL20.glUniform1i(location, v0);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgram(uint program) => FunctionsGL20.glUseProgram(program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribPointer(uint index, int size, GLType type, bool normalized, int stride, nint offset) => FunctionsGL20.glVertexAttribPointer(index, size, (uint)type, (byte)(normalized ? 1 : 0), stride, offset);

	}
}
