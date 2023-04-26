using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBClearTextureFunctions {

		[ExternFunction(AltNames = new string[] { "glClearTexImageARB" })]
		[NativeType("void glClearTexImage(GLuint texture, GLint level, GLenum format, GLenum type, void* pData)")]
		public delegate* unmanaged<uint, int, uint, uint, IntPtr, void> glClearTexImage;
		[ExternFunction(AltNames = new string[] { "glClearTexSubImageARB" })]
		[NativeType("void glClearTexSubImage(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pData)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, uint, uint, IntPtr, void> glClearTexSubImage;

	}

	public class ARBClearTexture : IGLObject {

		public GL GL { get; }
		public ARBClearTextureFunctions Functions { get; } = new();

		public ARBClearTexture(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexImage(uint texture, int level, GLFormat format, GLTextureType type, IntPtr data) {
			unsafe {
				Functions.glClearTexImage(texture, level, (uint)format, (uint)type, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexImage<T>(uint texture, int level, GLFormat format, GLTextureType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed(T* pData = data) {
					Functions.glClearTexImage(texture, level, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexSubImage(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, IntPtr data) {
			unsafe {
				Functions.glClearTexSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearTexSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glClearTexSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

	}
}
