using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBShaderImageLoadStoreFunctions {

		public delegate void PFN_glBindImageTexture(uint unit, uint texture, int level, byte layered, int layer, uint access, uint format);
		[ExternFunction(AltNames = new string[] { "glBindImageTextureARB" })]
		public PFN_glBindImageTexture glBindImageTexture;
		public delegate void PFN_glMemoryBarrier(uint barriers);
		[ExternFunction(AltNames = new string[] { "glMemoryBarrierARB" })]
		public PFN_glMemoryBarrier glMemoryBarrier;

	}
#nullable restore

	public class ARBShaderImageLoadStore {

		public GL GL { get; }
		public ARBShaderImageLoadStoreFunctions Functions { get; } = new();

		public ARBShaderImageLoadStore(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, GLAccess access, GLInternalFormat format) => Functions.glBindImageTexture(unit, texture, level, (byte)(layered ? 1 : 0), layer, (uint)access, (uint)format);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrier(GLMemoryBarrier barrier) => Functions.glMemoryBarrier((uint)barrier);

	}

}
