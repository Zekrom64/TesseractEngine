using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

	using GLfixed = Int32;
	using GLsync = IntPtr;

	using cl_context = IntPtr;
	using cl_event = IntPtr;

	public delegate void PFN_glBeginConditionalRenderNV(GLuint id, GLenum mode);
	public delegate void PFN_glEndConditionalRenderNV();

	public class NVConditionalRenderFunctions {

		[ExternFunction(Names = new string[] { "glBeginConditionalRender" })]
		public PFN_glBeginConditionalRenderNV glBeginConditionalRenderNV;
		[ExternFunction(Names = new string[] { "glEndConditionalRender" })]
		public PFN_glEndConditionalRenderNV glEndConditionalRenderNV;

	}
}
