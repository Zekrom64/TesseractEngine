using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBDrawInstancedFunctions {

		public delegate void PFN_glDrawArraysInstanced(uint mode, int first, int count, int primcount);
		[ExternFunction(AltNames = new string[] { "glDrawArraysInstancedARB" })]
		public PFN_glDrawArraysInstanced glDrawArraysInstanced;
		public delegate void PFN_glDrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int primcount);
		[ExternFunction(AltNames = new string[] { "glDrawElementsInstancedARB" })]
		public PFN_glDrawElementsInstanced glDrawElementsInstanced;

	}

	public class ARBDrawInstanced : IGLObject {

		public GL GL { get; }
		public ARBDrawInstancedFunctions Functions { get; } = new();

		public ARBDrawInstanced(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstanced(GLDrawMode mode, int first, int count, int primcount) => Functions.glDrawArraysInstanced((uint)mode, first, count, primcount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstanced(GLDrawMode mode, int count, GLIndexType type, nint offset, int primcount) => Functions.glDrawElementsInstanced((uint)mode, count, (uint)type, offset, primcount);

	}
}
