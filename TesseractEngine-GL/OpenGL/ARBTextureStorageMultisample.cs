using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureStorageMultisampleFunctions {

		[ExternFunction(AltNames = new string[] { "glTexStorage2DMultisampleARB" })]
		[NativeType("void glTexStorage2DMultisample(GLenum target, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height, GLboolean fixedSampleLocations)")]
		public delegate* unmanaged<uint, int, uint, int, int, byte, void> glTexStorage2DMultisample;
		[NativeType("void glTexStorage3DMultisample(GLenum target, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedSampleLocations)")]
		[ExternFunction(AltNames = new string[] { "glTexStorage3DMultisampleARB" })]
		public delegate* unmanaged<uint, int, uint, int, int, int, byte, void> glTexStorage3DMultisample;

	}

	public class ARBTextureStorageMultisample : IGLObject {

		public GL GL { get; }
		public ARBTextureStorageMultisampleFunctions Functions { get; } = new();

		public ARBTextureStorageMultisample(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) {
			unsafe {
				Functions.glTexStorage2DMultisample((uint)target, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) {
			unsafe {
				Functions.glTexStorage3DMultisample((uint)target, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}
	}
}
