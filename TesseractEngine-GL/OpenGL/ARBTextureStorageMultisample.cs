using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBTextureStorageMultisampleFunctions {

		public delegate void PFN_glTexStorage2DMultisample(uint target, int samples, uint internalFormat, int width, int height, byte fixedSampleLocations);
		[ExternFunction(AltNames = new string[] { "glTexStorage2DMultisampleARB" })]
		public PFN_glTexStorage2DMultisample glTexStorage2DMultisample;
		public delegate void PFN_glTexStorage3DMultisample(uint target, int samples, uint internalFormat, int width, int height, int depth, byte fixedSampleLocations);
		[ExternFunction(AltNames = new string[] { "glTexStorage3DMultisampleARB" })]
		public PFN_glTexStorage3DMultisample glTexStorage3DMultisample;

	}

	public class ARBTextureStorageMultisample : IGLObject {

		public GL GL { get; }
		public ARBTextureStorageMultisampleFunctions Functions { get; } = new();

		public ARBTextureStorageMultisample(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => Functions.glTexStorage2DMultisample((uint)target, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => Functions.glTexStorage3DMultisample((uint)target, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));

	}
}
