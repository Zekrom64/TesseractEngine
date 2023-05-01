using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBGetTextureSubImageFunctions {

		[ExternFunction(AltNames = new string[] { "glGetTextureSubImageARB" })]
		[NativeType("void glGetTextureSubImage(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, GLsizei bufSize, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, uint, uint, int, IntPtr, void> glGetTextureSubImage;
		[ExternFunction(AltNames = new string[] { "glGetCompressedTextureSubImageARB" })]
		[NativeType("void glGetCompressedTextureSubImage(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLsizei bufSize, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, int, IntPtr, void> glGetCompressedTextureSubImage;

	}

	public class ARBGetTextureSubImage : IGLObject {

		public GL GL { get; }
		public ARBGetTextureSubImageFunctions Functions { get; } = new();

		public ARBGetTextureSubImage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, pixels.Length * sizeof(T), (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) {
			unsafe {
				Functions.glGetTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, (uint)format, (uint)type, bufSize, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetCompressedTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, pixels.Length * sizeof(T), (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels) {
			unsafe {
				Functions.glGetCompressedTextureSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, bufSize, pixels);
			}
		}
	}
}
