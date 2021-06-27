using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.GL.Native {

	// Typedefs from OpenGL headers
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

	public class AMDDebugOutputFunctions {

		public PFN_glDebugMessageCallbackAMD glDebugMessageCallbackAMD;
		public PFN_glDebugMessageEnableAMD glDebugMessageEnableAMD;
		public PFN_glDebugMessageInsertAMD glDebugMessageInsertAMD;
		public PFN_glGetDebugMessageLogAMD glGetDebugMessageLogAMD;

	}

	public delegate void PFN_glDebugMessageCallbackAMD([MarshalAs(UnmanagedType.FunctionPtr)] GLDebugProcAMD callback, IntPtr userParam);
	public delegate void PFN_glDebugMessageEnableAMD(GLenum category, GLenum severity, GLsizei count, [NativeType("const GLuint*")] IntPtr ids, GLboolean enable);
	public delegate void PFN_glDebugMessageInsertAMD(GLenum category, GLenum severity, GLuint id, GLsizei length, [MarshalAs(UnmanagedType.LPStr)] string buf);
	public delegate GLuint PFN_glGetDebugMessageLogAMD(GLuint count, GLsizei bufsize, [NativeType("GLenum*")] IntPtr categories, [NativeType("GLuint*")] IntPtr severities, [NativeType("GLuint*")] IntPtr ids, [NativeType("GLsizei*")] IntPtr lengths, [NativeType("GLchar*")] IntPtr message);

	public class AMDDrawBuffersBlendFunctions {

		public PFN_glBlendEquationIndexedAMD glBlendEquationIndexedAMD;
		public PFN_glBlendEquationsSeparateIndexedAMD glBlendEquationsSeparateIndexedAMD;
		public PFN_glBlendFuncIndexedAMD glBlendFuncIndexedAMD;
		public PFN_glBlendFuncSeparateIndexedAMD glBlendFuncSeparateIndexedAMD;

	}

	public delegate void PFN_glBlendEquationIndexedAMD(GLuint buf, GLenum mode);
	public delegate void PFN_glBlendEquationsSeparateIndexedAMD(GLuint buf, GLenum modeRGB, GLenum modeAlpha);
	public delegate void PFN_glBlendFuncIndexedAMD(GLuint buf, GLenum src, GLenum dst);
	public delegate void PFN_glBlendFuncSeparateIndexedAMD(GLuint buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha);

}
