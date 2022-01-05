using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBES31CompatibilityFunctions {

		public delegate void PFN_glMemoryBarrierByRegion(uint barriers);
		[ExternFunction(AltNames = new string[] { "glMemoryBarrierByRegionARB" })]
		public PFN_glMemoryBarrierByRegion glMemoryBarrierByRegion;

	}
#nullable restore

	public class ARBES31Compatibility : IGLObject {

		public GL GL { get; }
		public ARBES31CompatibilityFunctions Functions { get; } = new();

		public ARBES31Compatibility(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrierByRegion(GLMemoryBarrier barriers) => Functions.glMemoryBarrierByRegion((uint)barriers);

	}
}
