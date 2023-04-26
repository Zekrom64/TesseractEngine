using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTextureViewFunctions {

		[ExternFunction(AltNames = new string[] { "glTextureViewARB" })]
		[NativeType("void glTextureView(GLuint texture, GLenum target, GLuint origTexture, GLenum internalFormat, GLuint minLevel, GLuint numLevels, GLuint minLayer, GLuint numLayers)")]
		public delegate* unmanaged<uint, uint, uint, uint, uint, uint, uint, uint, void> glTextureView;

	}

	public class ARBTextureView : IGLObject {

		public GL GL { get; }
		public ARBTextureViewFunctions Functions { get; } = new();

		public ARBTextureView(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureView(uint texture, GLTextureTarget target, uint origTexture, GLInternalFormat internalFormat, uint minLevel, uint numLevels, uint minLayer, uint numLayers) {
			unsafe {
				Functions.glTextureView(texture, (uint)target, origTexture, (uint)internalFormat, minLevel, numLevels, minLayer, numLayers);
			}
		}
	}
}
