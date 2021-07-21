using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {
	public class GL13 : GL12 {

		public GL13Functions FunctionsGL13 { get; } = new();

		public GL13(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL13);
		}

		public int ActiveTexture {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL13.glActiveTexture((uint)(GLEnums.GL_TEXTURE0 + value));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (int)(GL.GL11.GetInteger(GLEnums.GL_ACTIVE_TEXTURE) - GLEnums.GL_TEXTURE0);
		}

		public int ClientActiveTexture {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL13.glClientActiveTexture((uint)(GLEnums.GL_TEXTURE0 + value));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (int)(GL.GL11.GetInteger(GLEnums.GL_CLIENT_ACTIVE_TEXTURE) - GLEnums.GL_TEXTURE0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, int imageSize, IntPtr data) => FunctionsGL13.glCompressedTexImage1D((uint)target, level, (uint)internalFormat, width, border, imageSize, data);

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
		public void CompressedTexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, int imageSize, IntPtr data) => FunctionsGL13.glCompressedTexImage2D((uint)target, level, (uint)internalFormat, width, height, border, imageSize, data);

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
		public void CompressedTexImage3D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data) => FunctionsGL13.glCompressedTexImage3D((uint)target, level, (uint)internalFormat, width, height, depth, border, imageSize, data);

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
		public void GetCompressedTexImage(GLTextureTarget target, int lod, IntPtr img) => FunctionsGL13.glGetCompressedTexImage((uint)target, lod, img);

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
		public void SampleCoverage(float value, bool invert) => FunctionsGL13.glSampleCoverage(value, (byte)(invert ? 1 : 0));

	}
}
