using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBMultiDrawIndirectFunctions {

		public delegate void PFN_glMultiDrawArraysIndirect(uint mode, IntPtr indirect, int primcount, int stride);
		[ExternFunction(AltNames = new string[] { "glMultiDrawArraysIndirectARB" })]
		public PFN_glMultiDrawArraysIndirect glMultiDrawArraysIndirect;
		public delegate void PFN_glMultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, int primcount, int stride);
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsIndirectARB" })]
		public PFN_glMultiDrawElementsIndirect glMultiDrawElementsIndirect;

	}

	public class ARBMultiDrawIndirect : IGLObject {

		public GL GL { get; }
		public ARBMultiDrawIndirectFunctions Functions { get; } = new();

		public ARBMultiDrawIndirect(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirect(GLDrawMode mode, nint offset, int primcount, int stride) => Functions.glMultiDrawArraysIndirect((uint)mode, offset, primcount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset, int primcount, int stride) => Functions.glMultiDrawElementsIndirect((uint)mode, (uint)type, offset, primcount, stride);

	}
}
