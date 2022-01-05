using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBDrawIndirectFunctions {

		public delegate void PFN_glDrawArraysIndirect(uint mode, IntPtr indirect);
		[ExternFunction(AltNames = new string[] { "glDrawArraysIndirectARB" })]
		public PFN_glDrawArraysIndirect glDrawArraysIndirect;
		public delegate void PFN_glDrawElementsIndirect(uint mode, uint type, IntPtr indirect);
		[ExternFunction(AltNames = new string[] { "glDrawElementsIndirectARB" })]
		public PFN_glDrawElementsIndirect glDrawElementsIndirect;

	}
#nullable restore

	public class ARBDrawIndirect {

		public GL GL { get; }
		public ARBDrawIndirectFunctions Functions { get; } = new();

		public ARBDrawIndirect(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysIndirect(GLDrawMode mode, nint offset) => Functions.glDrawArraysIndirect((uint)mode, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset) => Functions.glDrawElementsIndirect((uint)mode, (uint)type, offset);

	}

}
