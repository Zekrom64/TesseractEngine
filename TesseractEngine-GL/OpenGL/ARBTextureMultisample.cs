using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureMultisampleFunctions {

		[ExternFunction(AltNames = new string[] { "glTexImage2DMultisampleARB" })]
		[NativeType("void glTexImage2DMultisample(GLenum target, GLint samples, GLenum internalFormat, GLint width, GLint height, GLboolean fixedSampleLocations)")]
		public delegate* unmanaged<uint, int, uint, int, int, byte, void> glTexImage2DMultisample;
		[ExternFunction(AltNames = new string[] { "glTexImage3DMultisampleARB" })]
		[NativeType("void glTexImage3DMultisample(GLenum target, GLint samples, GLenum internalFormat, GLsizei wdith, GLsizei height, GLsizei depth, GLboolean fixedSampleLocations)")]
		public delegate* unmanaged<uint, int, uint, int, int, int, byte, void> glTexImage3DMultisample;
		[ExternFunction(AltNames = new string[] { "glGetMultisamplefvARB" })]
		[NativeType("void glGetMultisamplefv(GLenum pname, GLuint index, GLfloat* pValue)")]
		public delegate* unmanaged<uint, uint, float*, void> glGetMultisamplefv;
		[ExternFunction(AltNames = new string[] { "glSampleMaskiARB" })]
		[NativeType("void glSampleMaski(GLuint index, GLuint mask)")]
		public delegate* unmanaged<uint, uint, void> glSampleMaski;

	}

	public class ARBTextureMultisample : IGLObject {

		public GL GL { get; }
		public ARBTextureMultisampleFunctions Functions { get; } = new();

		public ARBTextureMultisample(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) {
			unsafe {
				Functions.glTexImage2DMultisample((uint)target, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) {
			unsafe {
				Functions.glTexImage3DMultisample((uint)target, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetMultisample(GLGetMutlisample pname, uint index, Span<float> vals) {
			unsafe {
				fixed(float* pVals = vals) {
					Functions.glGetMultisamplefv((uint)pname, index, pVals);
				}
			}
			return vals;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SampleMask(uint maskNumber, uint mask) {
			unsafe {
				Functions.glSampleMaski(maskNumber, mask);
			}
		}
	}

}
