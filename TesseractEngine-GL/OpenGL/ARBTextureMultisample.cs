using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBTextureMultisampleFunctions {

		public delegate void PFN_glTexImage2DMultisample(uint target, int samples, uint internalFormat, int width, int height, byte fixedSampleLocations);
		[ExternFunction(AltNames = new string[] { "glTexImage2DMultisampleARB" })]
		public PFN_glTexImage2DMultisample glTexImage2DMultisample;
		public delegate void PFN_glTexImage3DMultisample(uint target, int samples, uint internalFormat, int width, int height, int depth, byte fixedSampleLocations);
		[ExternFunction(AltNames = new string[] { "glTexImage3DMultisampleARB" })]
		public PFN_glTexImage3DMultisample glTexImage3DMultisample;
		public delegate void PFN_glGetMultisamplefv(uint pname, uint index, [NativeType("GLfloat*")] IntPtr val);
		[ExternFunction(AltNames = new string[] { "glGetMultisamplefvARB" })]
		public PFN_glGetMultisamplefv glGetMultisamplefv;
		public delegate void PFN_glSampleMaski(uint index, uint mask);
		[ExternFunction(AltNames = new string[] { "glSampleMaskiARB" })]
		public PFN_glSampleMaski glSampleMaski;

	}

	public class ARBTextureMultisample : IGLObject {

		public GL GL { get; }
		public ARBTextureMultisampleFunctions Functions { get; } = new();

		public ARBTextureMultisample(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => Functions.glTexImage2DMultisample((uint)target, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => Functions.glTexImage3DMultisample((uint)target, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetMultisample(GLGetMutlisample pname, uint index, Span<float> vals) {
			unsafe {
				fixed(float* pVals = vals) {
					Functions.glGetMultisamplefv((uint)pname, index, (IntPtr)pVals);
				}
			}
			return vals;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SampleMask(uint maskNumber, uint mask) => Functions.glSampleMaski(maskNumber, mask);

	}

}
