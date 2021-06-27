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

	public class EXTFramebufferObjectFunctions {

		public PFN_glIsRenderbufferEXT glIsRenderbufferEXT;
		public PFN_glBindRenderbufferEXT glBindRenderbufferEXT;
		public PFN_glDeleteRenderbuffersEXT glDeleteRenderbuffersEXT;
		public PFN_glGenRenderbuffersEXT glGenRenderbuffersEXT;
		public PFN_glRenderbufferStorageEXT glRenderbufferStorageEXT;
		public PFN_glGetRenderbufferParameterivEXT glGetRenderbufferParameterivEXT;
		public PFN_glIsFramebufferEXT glIsFramebufferEXT;
		public PFN_glBindFramebufferEXT glBindFramebufferEXT;
		public PFN_glDeleteFramebuffersEXT glDeleteFramebuffersEXT;
		public PFN_glGenFramebuffersEXT glGenFramebuffersEXT;
		public PFN_glCheckFramebufferStatusEXT glCheckFramebufferStatusEXT;
		public PFN_glFramebufferTexture1DEXT glFramebufferTexture1DEXT;
		public PFN_glFramebufferTexture2DEXT glFramebufferTexture2DEXT;
		public PFN_glFramebufferTexture3DEXT glFramebufferTexture3DEXT;
		public PFN_glFramebufferRenderbufferEXT glFramebufferRenderbufferEXT;
		public PFN_glGetFramebufferAttachmentParameterivEXT glGetFramebufferAttachmentParameterivEXT;
		public PFN_glGenerateMipmapEXT glGenerateMipmapEXT;

	}

	public delegate GLboolean PFN_glIsRenderbufferEXT(GLuint renderbuffer);
	public delegate void PFN_glBindRenderbufferEXT(GLenum target, GLuint renderbuffer);
	public delegate void PFN_glDeleteRenderbuffersEXT(GLsizei n, [NativeType("const GLuint*")] IntPtr renderbuffers);
	public delegate void PFN_glGenRenderbuffersEXT(GLsizei n, [NativeType("GLuint*")] IntPtr renderbuffers);
	public delegate void PFN_glRenderbufferStorageEXT(GLenum target, GLenum internalformat, GLsizei width, GLsizei height);
	public delegate void PFN_glGetRenderbufferParameterivEXT(GLenum target, GLenum pname, [NativeType("int*")] IntPtr _params);
	public delegate GLboolean PFN_glIsFramebufferEXT(GLuint framebuffer);
	public delegate void PFN_glBindFramebufferEXT(GLenum target, GLuint framebuffer);
	public delegate void PFN_glDeleteFramebuffersEXT(GLsizei n, [NativeType("const GLuint*")] IntPtr framebuffers);
	public delegate void PFN_glGenFramebuffersEXT(GLsizei n, [NativeType("GLuint*")] IntPtr framebuffers);
	public delegate GLenum PFN_glCheckFramebufferStatusEXT(GLenum target);
	public delegate void PFN_glFramebufferTexture1DEXT(GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level);
	public delegate void PFN_glFramebufferTexture2DEXT(GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level);
	public delegate void PFN_glFramebufferTexture3DEXT(GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level, GLint zoffset);
	public delegate void PFN_glFramebufferRenderbufferEXT(GLenum target, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer);
	public delegate void PFN_glGetFramebufferAttachmentParameterivEXT(GLenum target, GLenum attachment, GLenum pname, [NativeType("int*")] IntPtr _params);
	public delegate void PFN_glGenerateMipmapEXT(GLenum target);

	public class EXTFramebufferBlitFunctions {

		public PFN_glBlitFramebufferEXT glBlitFramebufferEXT;

	}

	public delegate void PFN_glBlitFramebufferEXT(GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter);

	public class EXTFramebufferMultisampleFunctions {

		public PFN_glRenderbufferStorageMultisampleEXT glRenderbufferStorageMultisampleEXT;

	}

	public delegate void PFN_glRenderbufferStorageMultisampleEXT(GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height);

	
}
