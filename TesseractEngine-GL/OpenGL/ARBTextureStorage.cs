using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {
	
	public class ARBTextureStorageFunctions {

		public delegate void PFN_glTexStorage1D(uint target, int levels, uint internalFormat, int width);
		[ExternFunction(AltNames = new string[] { "glTexStorage1DARB" })]
		public PFN_glTexStorage1D glTexStorage1D;
		public delegate void PFN_glTexStorage2D(uint target, int levels, uint internalFormat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glTexStorage2DARB" })]
		public PFN_glTexStorage2D glTexStorage2D;
		public delegate void PFN_glTexStorage3D(uint target, int levels, uint internalFormat, int width, int height, int depth);
		[ExternFunction(AltNames = new string[] { "glTexStorage3DARB" })]
		public PFN_glTexStorage3D glTexStorage3D;

	}

	public class ARBTextureStorage {

		public GL GL { get; }
		public ARBTextureStorageFunctions Functions { get; } = new();

		public ARBTextureStorage(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage1D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width) => Functions.glTexStorage1D((uint)target, levels, (uint)internalFormat, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height) => Functions.glTexStorage2D((uint)target, levels, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height, int depth) => Functions.glTexStorage3D((uint)target, levels, (uint)internalFormat, width, height, depth);

	}

}
