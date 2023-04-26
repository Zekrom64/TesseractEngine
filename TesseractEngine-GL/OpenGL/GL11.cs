using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

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

	public unsafe class GL11Functions {

		// Note: We strip out any deprecated functionality from OpenGL 1.1
		// People who want to use these functions will have to get the function pointers themselves

		[NativeType("void glBindTexture(GLenum target, GLuint texture)")]
		public delegate* unmanaged<GLenum, GLuint, void> glBindTexture;
		[NativeType("void glBlendFunc(GLenum sfactor, GLuint texture)")]
		public delegate* unmanaged<GLenum, GLenum, void> glBlendFunc;

		[NativeType("void glClear(GLbitfield mask)")]
		public delegate* unmanaged<GLbitfield, void> glClear;
		[NativeType("void glClearColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha)")]
		public delegate* unmanaged<GLclampf, GLclampf, GLclampf, GLclampf, void> glClearColor;
		[NativeType("void glClearDepth(GLclampd depth)")]
		public delegate* unmanaged<GLclampd, void> glClearDepth;
		[NativeType("void glClearStencil(GLint s)")]
		public delegate* unmanaged<GLint, void> glClearStencil;
		[NativeType("void glClipPlane(GLenum plane, const GLdouble* pEquation)")]
		public delegate* unmanaged<GLenum, GLdouble*, void> glClipPlane;
		[NativeType("void glColorMask(GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha)")]
		public delegate* unmanaged<GLboolean, GLboolean, GLboolean, GLboolean, void> glColorMask;
		[NativeType("void glCopyTexImage1D(GLenum target, GLint level, GLenum internalFormat, GLint x, GLint y, GLsizei width, GLsizei border)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLint, GLint, GLsizei, GLsizei, void> glCopyTexImage1D;
		[NativeType("void glCopyTexImage2D(GLenum target, GLint level, GLenum internalFormat, GLint x, GLint y, GLsizei width, GLsizei height, GLsizei border)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLint, GLint, GLsizei, GLsizei, GLsizei, void> glCopyTexImage2D;
		[NativeType("void glCopyTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLint, GLsizei, void> glCopyTexSubImage1D;
		[NativeType("void glCopyTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLint, GLint, GLsizei, GLsizei, void> glCopyTexSubImage2D;
		[NativeType("void glCullFace(GLenum mode)")]
		public delegate* unmanaged<GLenum, void> glCullFace;

		[NativeType("void glDeleteTextures(GLsizei n, const GLuint* pTextures)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glDeleteTextures;
		[NativeType("void glDepthFunc(GLenum func)")]
		public delegate* unmanaged<GLenum, void> glDepthFunc;
		[NativeType("void glDepthMask(GLboolean flag)")]
		public delegate* unmanaged<GLboolean, void> glDepthMask;
		[NativeType("void glDepthRange(GLclampd zNear, GLclampd zFar)")]
		public delegate* unmanaged<GLclampd, GLclampd, void> glDepthRange;
		[NativeType("void glDisable(GLenum cap)")]
		public delegate* unmanaged<GLenum, void> glDisable;
		[NativeType("void glDisableClientState(GLenum array)")]
		public delegate* unmanaged<GLenum, void> glDisableClientState;
		[NativeType("void glDrawArrays(GLenum mode, GLint first, GLsizei count)")]
		public delegate* unmanaged<GLenum, GLint, GLsizei, void> glDrawArrays;
		[NativeType("void glDrawBuffer(GLenum mode)")]
		public delegate* unmanaged<GLenum, void> glDrawBuffer;
		[NativeType("void glDrawElements(GLenum mode, GLsizei count, GLenum type, void* pIndices)")]
		public delegate* unmanaged<GLenum, GLsizei, GLenum, IntPtr, void> glDrawElements;

		[NativeType("void glEnable(GLenum cap)")]
		public delegate* unmanaged<GLenum, void> glEnable;
		[NativeType("void glEnableClientState(GLenum array)")]
		public delegate* unmanaged<GLenum, void> glEnableClientState;

		[NativeType("void glFinish()")]
		public delegate* unmanaged<void> glFinish;
		[NativeType("void glFlush()")]
		public delegate* unmanaged<void> glFlush;
		[NativeType("void glFrontFace(GLenum mode)")]
		public delegate* unmanaged<GLenum, void> glFrontFace;

		[NativeType("void glGenTextures(GLsizei n, GLuint* pTextures)")]
		public delegate* unmanaged<GLsizei, GLuint*, void> glGenTextures;
		[NativeType("void glGetBooleanv(GLenum pname, GLboolean* pParams)")]
		public delegate* unmanaged<GLenum, GLboolean*, void> glGetBooleanv;
		[NativeType("void glGetClipPlane(GLenum plane, GLdouble* pEquation)")]
		public delegate* unmanaged<GLenum, GLdouble*, void> glGetClipPlane;
		[NativeType("void glGetDoublev(GLenum pname, GLdouble* pParams)")]
		public delegate* unmanaged<GLenum, GLdouble*, void> glGetDoublev;
		[NativeType("GLenum glGetError()")]
		public delegate* unmanaged<GLenum> glGetError;
		[NativeType("void glGetFloatv(GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<GLenum, GLfloat*, void> glGetFloatv;
		[NativeType("void glGetIntegerv(GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLint*, void> glGetIntegerv;
		[NativeType("void glGetPointerv(GLenum pname, void** pParams)")]
		public delegate* unmanaged<GLenum, IntPtr*, void> glGetPointerv;
		[NativeType("const GLubyte* glGetString(GLenum pname)")]
		public delegate* unmanaged<GLenum, byte*> glGetString;
		[NativeType("void glGetTexImage(GLenum target, GLint level, GLenum format, GLenum type, void* pixels)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLenum, IntPtr, void> glGetTexImage;
		[NativeType("void glGetTexLevelParameterfv(GLenum target, GLint level, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLfloat*, void> glGetTexLevelParameterfv;
		[NativeType("void glGetTexLevelParameteriv(GLenum target, GLint level, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLint*, void> glGetTexLevelParameteriv;
		[NativeType("void glGetTexParameterfv(GLenum target, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLfloat*, void> glGetTexParameterfv;
		[NativeType("void glGetTexParameteriv(GLenum target, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLint*, void> glGetTexParameteriv;

		[NativeType("GLboolean glIsEnabled(GLenum cap)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsEnabled;
		[NativeType("GLboolean glIsTexture(GLuint texture)")]
		public delegate* unmanaged<GLuint, GLboolean> glIsTexture;

		[NativeType("void glLineWidth(GLfloat width)")]
		public delegate* unmanaged<GLfloat, void> glLineWidth;
		[NativeType("void glLogicOp(GLenum opcode)")]
		public delegate* unmanaged<GLenum, void> glLogicOp;

		[NativeType("void glPixelStoref(GLenum pname, GLfloat param)")]
		public delegate* unmanaged<GLenum, GLfloat, void> glPixelStoref;
		[NativeType("void glPixelStorei(GLenum pname, GLint param)")]
		public delegate* unmanaged<GLenum, GLint, void> glPixelStorei;
		[NativeType("void glPointSize(GLfloat size)")]
		public delegate* unmanaged<GLfloat, void> glPointSize;
		[NativeType("void glPolygonMode(GLenum face, GLenum mode)")]
		public delegate* unmanaged<GLenum, GLenum, void> glPolygonMode;
		[NativeType("void glPolygonOffset(GLfloat factor, GLfloat units)")]
		public delegate* unmanaged<GLfloat, GLfloat, void> glPolygonOffset;

		[NativeType("void glReadBuffer(GLenum mode)")]
		public delegate* unmanaged<GLenum, void> glReadBuffer;
		[NativeType("void glReadPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<GLint, GLint, GLsizei, GLsizei, GLenum, GLenum, IntPtr, void> glReadPixels;

		[NativeType("void glScissor(GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<GLint, GLint, GLsizei, GLsizei, void> glScissor;
		[NativeType("void glStencilFunc(GLenum func, GLint ref, GLuint mask)")]
		public delegate* unmanaged<GLenum, GLint, GLuint, void> glStencilFunc;
		[NativeType("void glStencilMask(GLint mask)")]
		public delegate* unmanaged<GLuint, void> glStencilMask;
		[NativeType("void glStencilOp(GLenum fail, GLenum zfail, GLenum zpass)")]
		public delegate* unmanaged<GLenum, GLenum, GLenum, void> glStencilOp;

		[NativeType("void glTexImage1D(GLenum target, GLint level, GLint internalFormat, GLsizei width, GLint border, GLenum format, GLenum type, void* pixels)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLsizei, GLint, GLenum, GLenum, IntPtr, void> glTexImage1D;
		[NativeType("void glTexImage2D(GLenum target, GLint level, GLint internalFormat, GLsizei width, GLsizei height, GLint border, GLenum format, GLenum type, void* pixels)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLsizei, GLsizei, GLint, GLenum, GLenum, IntPtr, void> glTexImage2D;
		[NativeType("void glTexParameterf(GLenum target, GLenum pname, GLfloat param)")]
		public delegate* unmanaged<GLenum, GLenum, GLfloat, void> glTexParameterf;
		[NativeType("void glTexParameterfv(GLenum target, GLenum pname, const GLfloat* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLfloat*, void> glTexParameterfv;
		[NativeType("void glTexParameteri(GLenum target, GLenum pname, GLint param)")]
		public delegate* unmanaged<GLenum, GLenum, GLint, void> glTexParameteri;
		[NativeType("void glTexParameteriv(GLenum target, GLenum pname, const GLint* pParams)")]
		public delegate* unmanaged<GLenum, GLenum, GLint*, void> glTexParameteriv;
		[NativeType("void glTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pixels)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLsizei, GLenum, GLenum, IntPtr, void> glTexSubImage1D;
		[NativeType("void glTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLsizei, GLsizei, GLenum, GLenum, IntPtr, void> glTexSubImage2D;

		public delegate* unmanaged<GLint, GLint, GLsizei, GLsizei, void> glViewport;

	}

	public class GL11 : IGLObject {

		public GL GL { get; }
		public GL11Functions FunctionsGL11 { get; } = new();

		public GL11(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL11);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTexture(GLTextureTarget target, uint texture) {
			unsafe {
				FunctionsGL11.glBindTexture((uint)target, texture);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor) {
			unsafe {
				FunctionsGL11.glBlendFunc((uint)sfactor, (uint)dfactor);
			}
		}

		public GLBlendFactor BlendSrc {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFactor)GetInteger(GLEnums.GL_BLEND_SRC);
		}

		public GLBlendFactor BlendDst {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFactor)GetInteger(GLEnums.GL_BLEND_DST);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear(GLBufferMask mask) {
			unsafe {
				FunctionsGL11.glClear((uint)mask);
			}
		}

		public Vector4 ColorClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glClearColor(value.X, value.Y, value.Z, value.W);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<float> values = GetFloat(GLEnums.GL_COLOR_CLEAR_VALUE, stackalloc float[4]);
				return new Vector4(values[0], values[1], values[2], values[3]);
			}
		}

		public double DepthClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glClearDepth(value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetDouble(GLEnums.GL_DEPTH_CLEAR_VALUE);
		}

		public int StencilClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glClearStencil(value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetInteger(GLEnums.GL_STENCIL_CLEAR_VALUE);
		}

		public int StencilBits {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetInteger(GLEnums.GL_STENCIL_BITS);
		}

		public Tuple4<bool> ColorMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glColorMask((byte)(value.X ? 1 : 0), (byte)(value.Y ? 1 : 0), (byte)(value.Z ? 1 : 0), (byte)(value.W ? 1 : 0));
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<byte> values = GetBoolean(GLEnums.GL_COLOR_WRITEMASK, stackalloc byte[4]);
				return new(values[0] != 0, values[1] != 0, values[2] != 0, values[3] != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int x, int y, int width, int border) {
			unsafe {
				FunctionsGL11.glCopyTexImage1D((uint)target, level, (uint)internalFormat, x, y, width, border);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int x, int y, int width, int height, int border) {
			unsafe {
				FunctionsGL11.glCopyTexImage2D((uint)target, level, (uint)internalFormat, x, y, width, height, border);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage1D(GLTextureTarget target, int level, int xoffset, int x, int y, int width) {
			unsafe {
				FunctionsGL11.glCopyTexSubImage1D((uint)target, level, xoffset, x, y, width);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage2D(GLTextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height) {
			unsafe {
				FunctionsGL11.glCopyTexSubImage2D((uint)target, level, xoffset, yoffset, x, y, width, height);
			}
		}

		public bool CullFace {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetBoolean(GLEnums.GL_CULL_FACE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					if (value) FunctionsGL11.glEnable(GLEnums.GL_CULL_FACE);
					else FunctionsGL11.glDisable(GLEnums.GL_CULL_FACE);
				}
			}
		}

		public GLFace CullFaceMode {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLFace)GetInteger(GLEnums.GL_CULL_FACE_MODE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glCullFace((uint)value);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(int n, IntPtr textures) {
			unsafe {
				FunctionsGL11.glDeleteTextures(n, (GLuint*)textures);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					FunctionsGL11.glDeleteTextures(textures.Length, pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					FunctionsGL11.glDeleteTextures(textures.Length, pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(uint texture) {
			unsafe {
				FunctionsGL11.glDeleteTextures(1, &texture);
			}
		}

		public GLCompareFunc DepthFunc {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glDepthFunc((uint)value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLCompareFunc)GetInteger(GLEnums.GL_DEPTH_FUNC);
		}

		public bool DepthMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glDepthMask((byte)(value ? 1 : 0));
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetBoolean(GLEnums.GL_DEPTH_WRITEMASK);
		}

		public (double, double) DepthRange {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glDepthRange(value.Item1, value.Item2);
				}
			}

			get {
				Span<double> values = GetDouble(GLEnums.GL_DEPTH_RANGE, stackalloc double[2]);
				return (values[0], values[1]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLCapability cap) {
			unsafe {
				FunctionsGL11.glDisable((uint)cap);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArrays(GLDrawMode mode, int first, int count) {
			unsafe {
				FunctionsGL11.glDrawArrays((uint)mode, first, count);
			}
		}

		public GLDrawBuffer DrawBuffer {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLDrawBuffer)GetInteger(GLEnums.GL_DRAW_BUFFER);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glDrawBuffer((uint)value);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElements(GLDrawMode mode, int count, GLIndexType type, nint offset = 0) {
			unsafe {
				FunctionsGL11.glDrawElements((uint)mode, count, (uint)type, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLCapability cap) {
			unsafe {
				FunctionsGL11.glEnable((uint)cap);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Finish() {
			unsafe {
				FunctionsGL11.glFinish();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flush() {
			unsafe {
				FunctionsGL11.glFlush();
			}
		}

		public GLCullFace FrontFace {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glFrontFace((uint)value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLCullFace)GetInteger(GLEnums.GL_FRONT_FACE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenTextures(int n, IntPtr textures) {
			unsafe {
				FunctionsGL11.glGenTextures(n, (uint*)textures);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenTextures(in Span<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					FunctionsGL11.glGenTextures(textures.Length, pTextures);
				}
			}
			return textures;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenTextures(int n) {
			uint[] textures = new uint[n];
			unsafe {
				fixed (uint* pTextures = textures) {
					FunctionsGL11.glGenTextures(textures.Length, pTextures);
				}
			}
			return textures;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenTextures() {
			uint texture = 0;
			unsafe {
				FunctionsGL11.glGenTextures(1, &texture);
			}
			return texture;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(uint pname) {
			unsafe {
				byte value = 0;
				FunctionsGL11.glGetBooleanv(pname, &value);
				return value != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<byte> GetBoolean(uint pname, in Span<byte> values) {
			unsafe {
				fixed (byte* pValues = values) {
					FunctionsGL11.glGetBooleanv(pname, pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetDouble(uint pname) {
			unsafe {
				double value = 0;
				FunctionsGL11.glGetDoublev(pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<double> GetDouble(uint pname, in Span<double> values) {
			unsafe {
				fixed (double* pValues = values) {
					FunctionsGL11.glGetDoublev(pname, pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLError GetError() {
			unsafe {
				return (GLError)FunctionsGL11.glGetError();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetFloat(uint pname) {
			unsafe {
				float value = 0;
				FunctionsGL11.glGetFloatv(pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetFloat(uint pname, in Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					FunctionsGL11.glGetFloatv(pname, pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInteger(uint pname) {
			unsafe {
				int value = 0;
				FunctionsGL11.glGetIntegerv(pname, &value);
				return value;
			}
		}

		public Span<int> GetInteger(uint pname, in Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					FunctionsGL11.glGetIntegerv(pname, pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetPointer(uint pname) {
			unsafe {
				IntPtr value = IntPtr.Zero;
				FunctionsGL11.glGetPointerv(pname, &value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? GetString(uint pname) {
			unsafe {
				return MemoryUtil.GetUTF8((IntPtr)FunctionsGL11.glGetString(pname));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTexImage<T>(GLTextureTarget target, int level, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					FunctionsGL11.glGetTexImage((uint)target, level, (uint)format, (uint)type, (IntPtr)pPixels);
				}
				return pixels;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTexImage(GLTextureTarget target, int level, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL11.glGetTexImage((uint)target, level, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTexLevelParameter(GLTextureTarget target, int level, GLGetTexLevelParameter pname, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					FunctionsGL11.glGetTexLevelParameterfv((uint)target, level, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexLevelParameter(GLTextureTarget target, int level, GLGetTexLevelParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					FunctionsGL11.glGetTexLevelParameteriv((uint)target, level, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<float> values) {
			unsafe {
				fixed (float* pValues = values) {
					FunctionsGL11.glGetTexParameterfv((uint)target, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					FunctionsGL11.glGetTexParameteriv((uint)target, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(GLCapability cap) {
			unsafe {
				return FunctionsGL11.glIsEnabled((uint)cap) != 0;
			}
		}

		public float LineWidth {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glLineWidth(value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(GLEnums.GL_LINE_WIDTH);
		}

		public GLLogicOp LogicOp {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glLogicOp((uint)value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLLogicOp)GetInteger(GLEnums.GL_LOGIC_OP_MODE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PixelStore(GLPixelStoreParam pname, int param) {
			unsafe {
				FunctionsGL11.glPixelStorei((uint)pname, param);
			}
		}

		public float PointSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(GLEnums.GL_POINT_SIZE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glPointSize(value);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonMode(GLFace face, GLPolygonMode mode) {
			unsafe {
				FunctionsGL11.glPolygonMode((uint)face, (uint)mode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonOffset(float factor, float units) {
			unsafe {
				FunctionsGL11.glPolygonOffset(factor, units);
			}
		}

		public GLDrawBuffer ReadBuffer {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLDrawBuffer)GetInteger(GLEnums.GL_READ_BUFFER);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glReadBuffer((uint)value);
				}
			}
		}

		public void ReadPixels(int x, int y, int width, int height, GLFormat format, GLType type, IntPtr data) {
			unsafe {
				FunctionsGL11.glReadPixels(x, y, width, height, (uint)format, (uint)type, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scissor(int x, int y, int width, int height) {
			unsafe {
				FunctionsGL11.glScissor(x, y, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFunc(GLStencilFunc func, int reference, uint mask) {
			unsafe {
				FunctionsGL11.glStencilFunc((uint)func, reference, mask);
			}
		}

		public uint StencilMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (uint)GetInteger(GLEnums.GL_STENCIL_VALUE_MASK);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glStencilMask(value);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOp(GLStencilOp sfail, GLStencilOp dpfail, GLStencilOp dppass) {
			unsafe {
				FunctionsGL11.glStencilOp((uint)sfail, (uint)dpfail, (uint)dppass);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL11.glTexImage1D((uint)target, level, (int)internalFormat, width, border, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexImage1D(target, level, internalFormat, width, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage1D(target, level, internalFormat, width, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL11.glTexImage2D((uint)target, level, (int)internalFormat, width, height, border, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage2D(target, level, internalFormat, width, height, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage2D(target, level, internalFormat, width, height, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, float param) {
			unsafe {
				FunctionsGL11.glTexParameterf((uint)target, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<float> param) {
			unsafe {
				fixed(float* pParam = param) {
					FunctionsGL11.glTexParameterfv((uint)target, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, params float[] param) {
			unsafe {
				fixed (float* pParam = param) {
					FunctionsGL11.glTexParameterfv((uint)target, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, int param) {
			unsafe {
				FunctionsGL11.glTexParameteri((uint)target, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL11.glTexSubImage1D((uint)target, level, xoffset, width, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D<T>(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage1D(target, level, xoffset, width, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D<T>(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage1D(target, level, xoffset, width, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL11.glTexSubImage2D((uint)target, level, xoffset, yoffset, width, height, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pPixels);
				}
			}
		}

		public Recti Viewport {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<int> viewport = GetInteger(GLEnums.GL_VIEWPORT, stackalloc int[4]);
				return new Recti(viewport[0], viewport[1], viewport[2], viewport[3]);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL11.glViewport(value.Position.X, value.Position.Y, value.Size.X, value.Size.Y);
				}
			}
		}

	}
}
