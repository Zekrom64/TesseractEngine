using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class NVConditionalRenderFunctions {

		[ExternFunction(AltNames = new string[] { "glBeginConditionalRenderNV", "glBeginConditionalRenderNVX" })]
		[NativeType("void glBeginConditionalRender(GLuint id, GLenum mode)")]
		public delegate* unmanaged<uint, uint, void> glBeginConditionalRender;
		[ExternFunction(AltNames = new string[] { "glEndConditionalRenderNV", "glEndConditionalRenderNVX" })]
		[NativeType("void glEndConditionalRender()")]
		public delegate* unmanaged<void> glEndConditionalRender;

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
		public void BeginConditionalRender(uint id, GLQueryMode mode) {
			unsafe {
				Functions.glBeginConditionalRender(id, (uint)mode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndConditionalRender() {
			unsafe {
				Functions.glEndConditionalRender();
			}
		}
	}

}
