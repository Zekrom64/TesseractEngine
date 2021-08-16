using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBTextureBarrierFunctions {

		public delegate void PFN_glTextureBarrier();
		[ExternFunction(AltNames = new string[] { "glTextureBarrierARB" })]
		public PFN_glTextureBarrier glTextureBarrier;

	}

	public class ARBTextureBarrier : IGLObject {

		public GL GL { get; }
		public ARBTextureBarrierFunctions Functions { get; } = new();

		public ARBTextureBarrier(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBarrier() => Functions.glTextureBarrier();

	}
}
