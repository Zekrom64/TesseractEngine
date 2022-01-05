using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTextureBufferObjectFunctions {

		public delegate void PFN_glTexBuffer(uint target, uint internalFormat, uint buffer);
		[ExternFunction(AltNames = new string[] { "glTexBuffer" })]
		public PFN_glTexBuffer glTexBuffer;

	}
#nullable restore

	public class ARBTextureBufferObject : IGLObject {

		public GL GL { get; }
		public ARBTextureBufferObjectFunctions Functions { get; } = new();

		public ARBTextureBufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBuffer(GLTextureTarget target, GLInternalFormat internalFormat, uint buffer) => Functions.glTexBuffer((uint)target, (uint)internalFormat, buffer);
		
	}

}
