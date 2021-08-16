using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBIndirectParametersFunctions {

		public delegate void PFN_glMultiDrawArraysIndirectCount(uint mode, IntPtr indirect, nint drawcount, int maxdrawcount, int stride);
		[ExternFunction(AltNames = new string[] { "glMultiDrawArraysIndirectCountARB" })]
		public PFN_glMultiDrawArraysIndirectCount glMultiDrawArraysIndirectCount;
		public delegate void PFN_glMultiDrawElementsIndirectCount(uint mode, uint type, IntPtr indirect, nint drawcount, int maxdrawcount, int stride);
		[ExternFunction(AltNames = new string[] { "glMultiDrawElementsIndirectCountARB" })]
		public PFN_glMultiDrawElementsIndirectCount glMultiDrawElementsIndirectCount;

	}

	public class ARBIndirectParameters : IGLObject {

		public GL GL { get; }
		public ARBIndirectParametersFunctions Functions { get; } = new();

		public ARBIndirectParameters(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirectCount(GLDrawMode mode, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) =>
			Functions.glMultiDrawArraysIndirectCount((uint)mode, indirectOffset, drawCountOffset, maxDrawCount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirectCount(GLDrawMode mode, GLIndexType type, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) =>
			Functions.glMultiDrawElementsIndirectCount((uint)mode, (uint)type, indirectOffset, drawCountOffset, maxDrawCount, stride);

	}
}
