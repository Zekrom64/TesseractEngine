using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBES31CompatibilityFunctions {

		[ExternFunction(AltNames = new string[] { "glMemoryBarrierByRegionARB" })]
		[NativeType("void glMemoryBarrierByRegion(GLbitfield barriers)")]
		public delegate* unmanaged<uint, void> glMemoryBarrierByRegion;

	}

	public class ARBES31Compatibility : IGLObject {

		public GL GL { get; }
		public ARBES31CompatibilityFunctions Functions { get; } = new();

		public ARBES31Compatibility(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrierByRegion(GLMemoryBarrier barriers) {
			unsafe {
				Functions.glMemoryBarrierByRegion((uint)barriers);
			}
		}
	}
}
