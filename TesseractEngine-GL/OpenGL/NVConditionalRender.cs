using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class NVConditionalRenderFunctions {

		public delegate void PFN_glBeginConditionalRender(uint id, uint mode);
		[ExternFunction(AltNames = new string[] { "glBeginConditionalRenderNV", "glBeginConditionalRenderNVX" })]
		public PFN_glBeginConditionalRender glBeginConditionalRender;
		public delegate void PFN_glEndConditionalRender();
		[ExternFunction(AltNames = new string[] { "glEndConditionalRenderNV", "glEndConditionalRenderNVX" })]
		public PFN_glEndConditionalRender glEndConditionalRender;

	}

	public class NVConditionalRender : IGLObject {

		public GL GL { get; }
		public NVConditionalRenderFunctions Functions { get; }

		public NVConditionalRender(GL gl, IGLContext context) {
			GL = gl;
			Functions = new NVConditionalRenderFunctions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginConditionalRender(uint id, GLQueryMode mode) => Functions.glBeginConditionalRender(id, (uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndConditionalRender() => Functions.glEndConditionalRender();

	}

}
