using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTextureViewFunctions {

		public delegate void PFN_glTextureView(uint texture, uint target, uint origTexture, uint internalFormat, uint minLevel, uint numLevels, uint minLayer, uint numLayers);
		[ExternFunction(AltNames = new string[] { "glTextureViewARB" })]
		public PFN_glTextureView glTextureView;

	}
#nullable restore

	public class ARBTextureView : IGLObject {

		public GL GL { get; }
		public ARBTextureViewFunctions Functions { get; } = new();

		public ARBTextureView(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureView(uint texture, GLTextureTarget target, uint origTexture, GLInternalFormat internalFormat, uint minLevel, uint numLevels, uint minLayer, uint numLayers) => Functions.glTextureView(texture, (uint)target, origTexture, (uint)internalFormat, minLevel, numLevels, minLayer, numLayers);

	}
}
