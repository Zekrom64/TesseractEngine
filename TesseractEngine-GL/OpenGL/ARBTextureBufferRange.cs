using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBTextureBufferRangeFunctions {

		public delegate void PFN_glTexBufferRange(uint target, uint internalFormat, uint buffer, nint offset, nint size);
		[ExternFunction(AltNames = new string[] { "glTexBufferRangeARB" })]
		public PFN_glTexBufferRange glTexBufferRange;

	}

	public class ARBTextureBufferRange : IGLObject {

		public GL GL { get; }
		public ARBTextureBufferRangeFunctions Functions { get; } = new();

		public ARBTextureBufferRange(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBufferRange(GLBufferTarget target, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) => Functions.glTexBufferRange((uint)target, (uint)internalFormat, buffer, offset, size);

	}
}
