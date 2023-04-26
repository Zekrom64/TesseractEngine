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

	public unsafe class GL13Functions {

		public delegate void PFN_glActiveTexture(GLenum texture);
		public delegate void PFN_glClientActiveTexture(GLenum texture);
		public delegate void PFN_glCompressedTexImage1D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexImage2D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexImage3D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glGetCompressedTexImage(GLenum target, GLint lod, IntPtr img);
		public delegate void PFN_glSampleCoverage(GLclampf value, GLboolean invert);

		[NativeType("void glActiveTexture(GLenum texture)")]
		public delegate* unmanaged<GLenum, void> glActiveTexture;
		[NativeType("void glClientActiveTexture(GLenum texture)")]
		public delegate* unmanaged<GLenum, void> glClientActiveTexture;
		[NativeType("void glCompressedTexImage1D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLint border, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLsizei, GLint, GLsizei, IntPtr, void> glCompressedTexImage1D;
		[NativeType("void glCompressedTexImage2D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLint border, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLsizei, GLsizei, GLint, GLsizei, IntPtr, void> glCompressedTexImage2D;
		[NativeType("void glCompressedTexImage3D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLint border, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLenum, GLsizei, GLsizei, GLsizei, GLint, GLsizei, IntPtr, void> glCompressedTexImage3D;
		[NativeType("void glCompressedTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLsizei, GLenum, GLsizei, IntPtr, void> glCompressedTexSubImage1D;
		[NativeType("void glCompressedTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLsizei, GLsizei, GLenum, GLsizei, IntPtr, void> glCompressedTexSubImage2D;
		[NativeType("void glCompressedTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<GLenum, GLint, GLint, GLint, GLint, GLsizei, GLsizei, GLsizei, GLenum, GLsizei, IntPtr, void> glCompressedTexSubImage3D;
		[NativeType("void glGetCompressedTexImage(GLenum target, GLint lod, void* pImage)")]
		public delegate* unmanaged<GLenum, GLint, IntPtr, void> glGetCompressedTexImage;
		[NativeType("void glSampleCoverage(GLclampf value, GLboolean invert)")]
		public delegate* unmanaged<GLclampf, GLboolean, void> glSampleCoverage;

	}

	public class GL13 : GL12 {

		public GL13Functions FunctionsGL13 { get; } = new();

		public GL13(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL13);
		}

		public int ActiveTexture {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL13.glActiveTexture((uint)(GLEnums.GL_TEXTURE0 + value));
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (int)(GL.GL11.GetInteger(GLEnums.GL_ACTIVE_TEXTURE) - GLEnums.GL_TEXTURE0);
		}

		public int ClientActiveTexture {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL13.glClientActiveTexture((uint)(GLEnums.GL_TEXTURE0 + value));
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (int)(GL.GL11.GetInteger(GLEnums.GL_CLIENT_ACTIVE_TEXTURE) - GLEnums.GL_TEXTURE0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data) {
			unsafe {
				FunctionsGL13.glCompressedTexImage1D((uint)target, level, (uint)internalFormat, width, border, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					CompressedTexImage1D(target, level, internalFormat, width, border, data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, params T[] data) where T : unmanaged => CompressedTexImage1D(target, level, internalFormat, width, border, new ReadOnlySpan<T>(data));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data) {
			unsafe {
				FunctionsGL13.glCompressedTexImage2D((uint)target, level, (uint)internalFormat, width, height, border, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					CompressedTexImage2D(target, level, internalFormat, width, height, border, data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, params T[] data) where T : unmanaged => CompressedTexImage2D(target, level, internalFormat, width, height, border, new ReadOnlySpan<T>(data));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage3D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data) {
			unsafe {
				FunctionsGL13.glCompressedTexImage3D((uint)target, level, (uint)internalFormat, width, height, depth, border, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage3D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					CompressedTexImage3D(target, level, internalFormat, width, height, depth, border, data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage3D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, params T[] data) where T : unmanaged => CompressedTexImage3D(target, level, internalFormat, width, height, depth, border, new ReadOnlySpan<T>(data));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTexImage(GLTextureTarget target, int lod, IntPtr img) {
			unsafe {
				FunctionsGL13.glGetCompressedTexImage((uint)target, lod, img);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTexImage<T>(GLTextureTarget target, int lod, in Span<T> img) where T : unmanaged {
			unsafe {
				fixed(T* pImg = img) {
					FunctionsGL13.glGetCompressedTexImage((uint)target, lod, (IntPtr)pImg);
				}
			}
			return img;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetCompressedTexImage<T>(GLTextureTarget target, int lod, T[] img) where T : unmanaged {
			unsafe {
				fixed (T* pImg = img) {
					FunctionsGL13.glGetCompressedTexImage((uint)target, lod, (IntPtr)pImg);
				}
			}
			return img;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetCompressedTexImage<T>(GLTextureTarget target, int lod, int nimg) where T : unmanaged => GetCompressedTexImage(target, lod, new T[nimg]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SampleCoverage(float value, bool invert) {
			unsafe {
				FunctionsGL13.glSampleCoverage(value, (byte)(invert ? 1 : 0));
			}
		}
	}
}
