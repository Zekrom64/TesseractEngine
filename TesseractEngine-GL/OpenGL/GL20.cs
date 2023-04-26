using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Byte;
	using GLbyte = SByte;
	using GLshort = Int16;
	using GLubyte = Byte;
	using GLushort = UInt16;
	using GLulong = UInt64;
	using GLfloat = Single;
	using GLclampf = Single;
	using GLdouble = Double;
	using GLclampd = Double;
	using GLint64 = Int64;
	using GLuint64 = UInt64;
	using GLintptr = IntPtr;
	using GLsizeiptr = IntPtr;

	public unsafe class GL20Functions {

		[NativeType("void glAttachShader(GLuint program, GLuint shader)")]
		public delegate* unmanaged<GLuint, GLuint, void> glAttachShader;
		[NativeType("void glBindAttribLocation(GLuint program, GLuint index, const char* name)")]
		public delegate* unmanaged<GLuint, GLuint, byte*, void> glBindAttribLocation;
		[NativeType("void glBlendEquationSeparate(GLenum modeRGB, GLenum modeAlpha)")]
		public delegate* unmanaged<GLenum, GLenum, void> glBlendEquationSeparate;
		[NativeType("void glCompileShader(GLuint shader)")]
		public delegate* unmanaged<GLuint, void> glCompileShader;
		[NativeType("GLuint glCreateProgram()")]
		public delegate* unmanaged<GLuint> glCreateProgram;
		[NativeType("GLuint glCreateShader(GLenum type)")]
		public delegate* unmanaged<GLenum, GLuint> glCreateShader;
		[NativeType("void glDeleteProgram(GLuint program)")]
		public delegate* unmanaged<GLuint, void> glDeleteProgram;
		[NativeType("void glDeleteShader(GLuint shader)")]
		public delegate* unmanaged<GLuint, void> glDeleteShader;
		[NativeType("void glDetachShader(GLuint program, GLuint shader)")]
		public delegate* unmanaged<GLuint, GLuint, void> glDetachShader;
		[NativeType("void glDisableVertexAttribArray(GLuint index)")]
		public delegate* unmanaged<GLuint, void> glDisableVertexAttribArray;
		[NativeType("void glDrawBuffers(GLsizei n, const GLenum* pBufs)")]
		public delegate* unmanaged<GLsizei, GLenum*, void> glDrawBuffers;
		[NativeType("void glEnableVertexAttribArray(GLuint index)")]
		public delegate* unmanaged<GLuint, void> glEnableVertexAttribArray;
		[NativeType("void glGetActiveAttrib(GLuint program, GLuint index, GLsizei maxLength, GLsize* pLength, GLint* pSize, GLenum* pType, GLchar* pName)")]
		public delegate* unmanaged<GLuint, GLuint, GLsizei, out GLsizei, out GLint, out GLenum, byte*, void> glGetActiveAttrib;
		[NativeType("void glGetActiveUniform(GLuint program, GLuint index, GLsizei maxLength, GLsize* pLength, GLint* pSize, GLenum* pType, GLchar* pName)")]
		public delegate* unmanaged<GLuint, GLuint, GLsizei, out GLsizei, out GLint, out GLenum, byte*, void> glGetActiveUniform;
		[NativeType("void glGetAttachedShaders(GLuint program, GLsizei maxCount, GLsizei* pCount, GLuint* pShaders)")]
		public delegate* unmanaged<GLuint, GLsizei, out GLsizei, GLuint*, void> glGetAttachedShaders;
		[NativeType("GLint glGetAttribLocation(GLuint program, const char* name)")]
		public delegate* unmanaged<GLuint, byte*, GLint> glGetAttribLocation;
		[NativeType("void glGetProgramInfoLog(GLuint shader, GLsizei bufSize, GLsizei* pLength, GLchar* pInfoLog)")]
		public delegate* unmanaged<GLuint, GLsizei, out GLsizei, byte*, void> glGetProgramInfoLog;
		[NativeType("void glGetProgramiv(GLuint program, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<GLuint, GLenum, out GLint, void> glGetProgramiv;
		[NativeType("void glGetShaderInfoLog(GLuint shader, GLsizei bufSize, GLsizei* pLength, GLchar* pInfoLog)")]
		public delegate* unmanaged<GLuint, GLsizei, out GLsizei, byte*, void> glGetShaderInfoLog;
		[NativeType("void glGetShaderSource(GLuint obj, GLsizei maxLength, GLsizei* pLength, GLchar* pSource)")]
		public delegate* unmanaged<GLuint, GLsizei, out GLsizei, byte*, void> glGetShaderSource;
		[NativeType("void glGetShaderiv(GLuint shader, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<GLuint, GLuint, out GLint, void> glGetShaderiv;
		[NativeType("GLint glGetUniformLocation(GLuint program, const char* name)")]
		public delegate* unmanaged<GLuint, byte*, GLint> glGetUniformLocation;
		[NativeType("void glGetUniformfv(GLuint program, GLint location, GLfloat* pParams)")]
		public delegate* unmanaged<GLuint, GLint, GLfloat*, void> glGetUniformfv;
		[NativeType("void glGetUniformiv(GLuint program, GLint location, GLint* pParams)")]
		public delegate* unmanaged<GLuint, GLint, GLint*, void> glGetUniformiv;
		[NativeType("void glGetVertexAttribPointerv(GLuint index, GLenum name, void** pPointer)")]
		public delegate* unmanaged<GLuint, GLenum, out IntPtr, void> glGetVertexAttribPointerv;
		[NativeType("void glGetVertexAttribdv(GLuint index, GLenum pname, GLdouble* pParams)")]
		public delegate* unmanaged<GLuint, GLenum, GLdouble*, void> glGetVertexAttribdv;
		[NativeType("void glGetVertexAttribfv(GLuint index, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<GLuint, GLenum, GLfloat*, void> glGetVertexAttribfv;
		[NativeType("void glGetVertexAttribiv(GLuint index, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLuint, GLenum, GLint*, void> glGetVertexAttribiv;
		[NativeType("GLboolean glIsProgram(GLuint program)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsProgram;
		[NativeType("GLboolean glIsShader(GLuint shader)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsShader;
		[NativeType("void glLinkProgram(GLuint program)")]
		public delegate* unmanaged<GLuint, void> glLinkProgram;
		[NativeType("void glShaderSource(GLuint shader, GLsizei count, const GLchar* const* pString, const GLint* pLength")]
		public delegate* unmanaged<GLuint, GLsizei, byte**, GLint*, void> glShaderSource;
		[NativeType("void glStencilFuncSeparate(GLenum frontFunc, GLenum backFunc, GLint ref, GLuint mask)")]
		public delegate* unmanaged<GLenum, GLenum, GLint, GLuint, void> glStencilFuncSeparate;
		[NativeType("void glStencilMaskSeparate(GLenum face, GLuint mask)")]
		public delegate* unmanaged<GLenum, GLuint, void> glStencilMaskSeparate;
		[NativeType("void glStencilOpSeparate(GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass)")]
		public delegate* unmanaged<GLenum, GLenum, GLenum, GLenum, void> glStencilOpSeparate;
		[NativeType("void glUniform1f(GLint location, GLfloat v0)")]
		public delegate* unmanaged<GLint, GLfloat, void> glUniform1f;
		[NativeType("void glUniform1i(GLint location, GLint v0)")]
		public delegate* unmanaged<GLint, GLint, void> glUniform1i;
		[NativeType("void glUniform2f(GLint location, GLfloat v0, GLfloat v1)")]
		public delegate* unmanaged<GLint, GLfloat, GLfloat, void> glUniform2f;
		[NativeType("void glUniform2i(GLint location, GLint v0, GLint v1)")]
		public delegate* unmanaged<GLint, GLint, GLint, void> glUniform2i;
		[NativeType("void glUniform3f(GLint location, GLfloat v0, GLfloat v1, GLfloat v2)")]
		public delegate* unmanaged<GLint, GLfloat, GLfloat, GLfloat, void> glUniform3f;
		[NativeType("void glUniform3i(GLint location, GLint v0, GLint v1, GLint v2)")]
		public delegate* unmanaged<GLint, GLint, GLint, GLint, void> glUniform3i;
		[NativeType("void glUniform4f(GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3)")]
		public delegate* unmanaged<GLint, GLfloat, GLfloat, GLfloat, GLfloat, void> glUniform4f;
		[NativeType("void glUniform4i(GLint location, GLint v0, GLint v1, GLint v2, GLint v3)")]
		public delegate* unmanaged<GLint, GLint, GLint, GLint, GLint, void> glUniform4i;
		[NativeType("void glUniformMatrix2fv(GLint location, GLsizei count, GLboolean transpose, const GLfloat* pValue)")]
		public delegate* unmanaged<GLint, GLsizei, GLboolean, GLfloat*, void> glUniformMatrix2fv;
		[NativeType("void glUniformMatrix3fv(GLint location, GLsizei count, GLboolean transpose, const GLfloat* pValue)")]
		public delegate* unmanaged<GLint, GLsizei, GLboolean, GLfloat*, void> glUniformMatrix3fv;
		[NativeType("void glUniformMatrix4fv(GLint location, GLsizei count, GLboolean transpose, const GLfloat* pValue)")]
		public delegate* unmanaged<GLint, GLsizei, GLboolean, GLfloat*, void> glUniformMatrix4fv;
		[NativeType("void glUseProgram(GLuint program)")]
		public delegate* unmanaged<GLuint, void> glUseProgram;
		[NativeType("void glValidateProgram(GLuint program)")]
		public delegate* unmanaged<GLuint, void> glValidateProgram;
		[NativeType("void glVertexAttribPointer(GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, void* pointer)")]
		public delegate* unmanaged<GLuint, GLint, GLenum, GLboolean, GLsizei, IntPtr, void> glVertexAttribPointer;
	}

	public class GL20 : GL15 {

		public GL20Functions FunctionsGL20 { get; } = new();

		public GL20(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL20);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AttachShader(uint program, uint shader) {
			unsafe {
				FunctionsGL20.glAttachShader(program, shader);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindAttribLocation(uint program, uint index, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					FunctionsGL20.glBindAttribLocation(program, index, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(GLBlendFunction modeRGB, GLBlendFunction modeAlpha) {
			unsafe {
				FunctionsGL20.glBlendEquationSeparate((uint)modeRGB, (uint)modeAlpha);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompileShader(uint shader) {
			unsafe {
				FunctionsGL20.glCompileShader(shader);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgram() {
			unsafe {
				return FunctionsGL20.glCreateProgram();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShader(GLShaderType type) {
			unsafe {
				return FunctionsGL20.glCreateShader((uint)type);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgram(uint program) {
			unsafe {
				FunctionsGL20.glDeleteProgram(program);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteShader(uint shader) {
			unsafe {
				FunctionsGL20.glDeleteShader(shader);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DetachShader(uint program, uint shader) {
			unsafe {
				FunctionsGL20.glDetachShader(program, shader);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexAttribArray(uint index) {
			unsafe {
				FunctionsGL20.glDisableVertexAttribArray(index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawBuffers(ReadOnlySpan<GLDrawBuffer> bufs) {
			unsafe {
				fixed(GLDrawBuffer* pBufs = bufs) {
					FunctionsGL20.glDrawBuffers(bufs.Length, (uint*)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexAttribArray(uint index) {
			unsafe {
				FunctionsGL20.glEnableVertexAttribArray(index);
			}
		}

		public void GetActiveAttrib(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			unsafe {
				FunctionsGL20.glGetProgramiv(program, GLEnums.GL_ACTIVE_ATTRIBUTE_MAX_LENGTH, out int maxLen);
				Span<byte> nameBytes = stackalloc byte[maxLen];
				uint utype = 0;
				unsafe {
					fixed (byte* pNameBytes = nameBytes) {
						FunctionsGL20.glGetActiveAttrib(program, index, maxLen, out int length, out size, out utype, pNameBytes);
					}
				}
				type = (GLShaderAttribType)utype;
				name = MemoryUtil.GetASCII(nameBytes);
			}
		}

		public void GetActiveUniform(uint program, uint index, out int size, out GLShaderUniformType type, out string name) {
			unsafe {
				FunctionsGL20.glGetProgramiv(program, GLEnums.GL_ACTIVE_UNIFORM_MAX_LENGTH, out int maxLen);
				Span<byte> nameBytes = stackalloc byte[maxLen];
				uint utype = 0;
				unsafe {
					fixed (byte* pNameBytes = nameBytes) {
						FunctionsGL20.glGetActiveUniform(program, index, maxLen, out int length, out size, out utype, pNameBytes);
					}
				}
				type = (GLShaderUniformType)utype;
				name = MemoryUtil.GetASCII(nameBytes);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetAttachedShaders(uint program, in Span<uint> shaders) {
			int count = 0;
			unsafe {
				fixed(uint* pShaders = shaders) {
					FunctionsGL20.glGetAttachedShaders(program, shaders.Length, out count, pShaders);
				}
			}
			return shaders[..count];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetAttribLocation(uint program, string name) {
			unsafe {
				fixed(byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return FunctionsGL20.glGetAttribLocation(program, pName);
				}
			}
		}

		public string GetProgramInfoLog(uint program) {
			unsafe {
				FunctionsGL20.glGetProgramiv(program, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
				Span<byte> logBytes = new(new byte[maxLen]);
				int length = 0;
				unsafe {
					fixed (byte* pInfoLog = logBytes) {
						FunctionsGL20.glGetProgramInfoLog(program, maxLen, out length, pInfoLog);
					}
				}
				return MemoryUtil.GetASCII(logBytes[..length]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgram(uint program, GLGetProgram pname) {
			unsafe {
				FunctionsGL20.glGetProgramiv(program, (uint)pname, out int param);
				return param;
			}
		}

		public string GetShaderInfoLog(uint shader) {
			unsafe {
				FunctionsGL20.glGetShaderiv(shader, GLEnums.GL_INFO_LOG_LENGTH, out int maxLen);
				Span<byte> logBytes = new(new byte[maxLen]);
				int length = 0;
				unsafe {
					fixed (byte* pInfoLog = logBytes) {
						FunctionsGL20.glGetShaderInfoLog(shader, maxLen, out length, pInfoLog);
					}
				}
				return MemoryUtil.GetASCII(logBytes[..length]);
			}
		}

		public string GetShaderSource(uint shader) {
			unsafe {
				FunctionsGL20.glGetShaderiv(shader, GLEnums.GL_SHADER_SOURCE_LENGTH, out int maxLen);
				Span<byte> srcBytes = new(new byte[maxLen]);
				int length = 0;
				unsafe {
					fixed (byte* pSrc = srcBytes) {
						FunctionsGL20.glGetShaderSource(shader, maxLen, out length, pSrc);
					}
				}
				return MemoryUtil.GetASCII(srcBytes[..length]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShader(uint shader, GLGetShader pname) {
			unsafe {
				FunctionsGL20.glGetShaderiv(shader, (uint)pname, out int param);
				return param;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetUniformLocation(uint program, string name) {
			unsafe {
				fixed (byte* pName = MemoryUtil.StackallocUTF8(name, stackalloc byte[256])) {
					return FunctionsGL20.glGetUniformLocation(program, pName);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void LinkProgram(uint program) {
			unsafe {
				FunctionsGL20.glLinkProgram(program);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderSource(uint shader, ReadOnlySpan<byte> source) {
			unsafe {
				int length = source.Length;
				fixed(byte* pSrcBytes = source) {
					FunctionsGL20.glShaderSource(shader, 1, &pSrcBytes, &length);
				}
			}
		}

		public void ShaderSource(uint shader, string source) {
			unsafe {
				Span<byte> strSource = MemoryUtil.StackallocUTF8(source, stackalloc byte[4096]);
				int length = strSource.Length - 1;
				fixed (byte* pSrcBytes = strSource) {
					FunctionsGL20.glShaderSource(shader, 1, &pSrcBytes, &length);
				}
			}
		}

		public void ShaderSource(uint shader, IConstPointer<byte> source) {
			int length = source.ArraySize;
			if (length < 0) length = MemoryUtil.FindFirst<byte>(source.Ptr, 0);
			IntPtr pstr = source.Ptr;
			unsafe {
				FunctionsGL20.glShaderSource(shader, 1, (byte**)&pstr, &length);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFuncSeparate(GLFace face, GLStencilFunc func, int reference, uint mask) {
			unsafe {
				FunctionsGL20.glStencilFuncSeparate((uint)face, (uint)func, reference, mask);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilMaskSeparate(GLFace face, uint mask) {
			unsafe {
				FunctionsGL20.glStencilMaskSeparate((uint)face, mask);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOpSeparate(GLFace face, GLStencilOp sfail, GLStencilOp dpfail, GLStencilOp dppass) {
			unsafe {
				FunctionsGL20.glStencilOpSeparate((uint)face, (uint)sfail, (uint)dpfail, (uint)dppass);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Uniform(int location, int v0) {
			unsafe {
				FunctionsGL20.glUniform1i(location, v0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgram(uint program) {
			unsafe {
				FunctionsGL20.glUseProgram(program);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribPointer(uint index, int size, GLTextureType type, bool normalized, int stride, nint offset) {
			unsafe {
				FunctionsGL20.glVertexAttribPointer(index, size, (uint)type, (byte)(normalized ? 1 : 0), stride, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ValidateProgram(uint program) {
			unsafe {
				FunctionsGL20.glValidateProgram(program);
			}
		}
	}
}
