using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
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

	public unsafe class GL12Functions {

		[NativeType("void glCopyTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLint, GLint, GLint, GLsizei, GLsizei, void> glCopyTexSubImage3D;
		[NativeType("void glDrawRangeElements(GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, void* pIndices)")]
		public delegate* unmanaged<GLenum, GLuint, GLuint, GLsizei, GLenum, IntPtr, void> glDrawRangeElements;
		[NativeType("void glTexImage3D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLint border, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLsizei, GLsizei, GLsizei, GLint, GLenum, GLenum, IntPtr, void> glTexImage3D;
		[NativeType("void glTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLint, GLsizei, GLsizei, GLsizei, GLenum, GLenum, IntPtr, void> glTexSubImage3D;

	}

	public class GL12 : GL11 {

		public GL12Functions FunctionsGL12 { get; } = new();

		public GL12(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL12);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage3D(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) {
			unsafe {
				FunctionsGL12.glCopyTexSubImage3D((uint)target, level, xoffset, yoffset, zoffset, x, y, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRangeElements(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, IntPtr indices = default) {
			unsafe {
				FunctionsGL12.glDrawRangeElements((uint)mode, start, end, count, (uint)type, indices);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, GLFormat format, GLTextureType type, IntPtr pixels = default) {
			unsafe {
				FunctionsGL12.glTexImage3D((uint)target, level, (uint)internalFormat, width, height, depth, border, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexImage3D(target, level, internalFormat, width, height, depth, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage3D(target, level, internalFormat, width, height, depth, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage3D(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				FunctionsGL12.glTexSubImage3D((uint)target, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage3D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage3D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, (IntPtr)pPixels);
				}
			}
		}

	}

}
