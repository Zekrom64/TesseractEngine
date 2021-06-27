using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public class GL12 : IGLObject {

		public GL GL { get; }
		public GL12Functions Functions { get; }

		public GL12(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL12Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage3D(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => Functions.glCopyTexSubImage3D((uint)target, level, xoffset, yoffset, zoffset, x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRangeElements(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, IntPtr indices = default) => Functions.glDrawRangeElements((uint)mode, start, end, count, (uint)type, indices);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int depth, int border, GLFormat format, GLTextureType type, IntPtr pixels = default) => Functions.glTexImage3D((uint)target, level, (int)internalFormat, width, height, depth, border, (uint)format, (uint)type, pixels);

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
		public void TexSubImage3D(GLTextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, IntPtr pixels) => Functions.glTexSubImage3D((uint)target, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, pixels);

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
