using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureStorageFunctions {

		[ExternFunction(AltNames = new string[] { "glTexStorage1DARB" })]
		[NativeType("void glTexStorage1D(GLenum target, GLint levels, GLenum internalFormat, GLsizei width)")]
		public delegate* unmanaged<uint, int, uint, int, void> glTexStorage1D;
		[ExternFunction(AltNames = new string[] { "glTexStorage2DARB" })]
		[NativeType("void glTexStorage2D(GLenum target, GLint levels, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint, int, int, void> glTexStorage2D;
		[ExternFunction(AltNames = new string[] { "glTexStorage3DARB" })]
		[NativeType("void glTexStorage3D(GLenum target, GLint levels, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth)")]
		public delegate* unmanaged<uint, int, uint, int, int, int, void> glTexStorage3D;

	}

	public class ARBTextureStorage {

		public GL GL { get; }
		public ARBTextureStorageFunctions Functions { get; } = new();

		public ARBTextureStorage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage1D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width) {
			unsafe {
				Functions.glTexStorage1D((uint)target, levels, (uint)internalFormat, width);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glTexStorage2D((uint)target, levels, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height, int depth) {
			unsafe {
				Functions.glTexStorage3D((uint)target, levels, (uint)internalFormat, width, height, depth);
			}
		}
	}

}
