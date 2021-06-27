using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Boolean;
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

	public class GLException : Exception {
	
		public GLException(string msg) : base(msg) { }

	}


	/// <summary>
	/// A GL object is an object that stores a reference to an instance of the OpenGL API.
	/// </summary>
	public interface IGLObject {

		/// <summary>
		/// The OpenGL API this object references.
		/// </summary>
		public GL GL { get; }

	}

	public class GL : IGLContextObject {

		public IGLContext Context { get; }

		public GL11 GL11 { get; }
		public GL12 GL12 { get; }
		public GL13 GL13 { get; }
		public GL14 GL14 { get; }
		public GL15 GL15 { get; }
		public GL20 GL20 { get; }
		public GL30 GL30 { get; }

		private HashSet<string> extensions = new();
		public IReadOnlySet<string> Extensions => extensions;

		// OpenGL 3.0 Extensions
		// GL_EXT_gpu_shader4
		public NVConditionalRender NVConditionalRender { get; }
		// GL_APPLE_flush_buffer_range
		// GL_ARB_color_buffer_float
		// GL_NV_depth_buffer_float,
		// GL_ARB_texture_float
		// GL_EXT_packed_float
		// GL_EXT_texture_shared_exponent

		public GL(IGLContext context) {
			Context = context;
			// Initialize OpenGL support based on 
			int major = Context.MajorVersion;
			int minor = Context.MinorVersion;
			bool hasGLXX = major >= 5;
			bool hasGL4X = major == 4;
			bool hasGL46 = hasGLXX || (hasGL4X && minor >= 6);
			bool hasGL45 = hasGL46 || (hasGL4X && minor >= 5);
			bool hasGL44 = hasGL45 || (hasGL4X && minor >= 4);
			bool hasGL43 = hasGL44 || (hasGL4X && minor >= 3);
			bool hasGL42 = hasGL43 || (hasGL4X && minor >= 2);
			bool hasGL41 = hasGL42 || (hasGL4X && minor >= 1);
			bool hasGL40 = hasGL41 || hasGL4X;
			bool hasGL3X = major == 3;
			bool hasGL33 = hasGL40 || (hasGL3X && minor >= 3);
			bool hasGL32 = hasGL33 || (hasGL3X && minor >= 2);
			bool hasGL31 = hasGL32 || (hasGL3X && minor >= 1);
			bool hasGL30 = hasGL31 || hasGL3X;
			bool hasGL2X = major == 2;
			GL30 = new GL30(this, context);
			GL20 = new GL20(this, context);
			GL15 = new GL15(this, context);
			GL14 = new GL14(this, context);
			GL13 = new GL13(this, context);
			GL12 = new GL12(this, context);
			GL11 = new GL11(this, context);

			if (GL30 != null) {
				int nexts = GL11.GetInteger(GLEnums.GL_NUM_EXTENSIONS);
				for (int i = 0; i < nexts; i++) extensions.Add(GL30.GetString(GLEnums.GL_EXTENSIONS, (uint)i));
			} else {

			}
		}

	}

}
