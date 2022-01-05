using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBClipControlFunctions {

		public delegate void PFN_glClipControl(uint origin, uint depth);
		[ExternFunction(AltNames = new string[] { "glClipControlARB" })]
		public PFN_glClipControl glClipControl;

	}
#nullable restore

	public class ARBClipControl : IGLObject {

		public GL GL { get; }
		public ARBClipControlFunctions Functions { get; } = new();

		public ARBClipControl(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClipControl(GLOrigin origin, GLClipDepth depth) => Functions.glClipControl((uint)origin, (uint)depth);

	}
}
