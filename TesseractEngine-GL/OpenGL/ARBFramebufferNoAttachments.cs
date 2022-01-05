using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBFramebufferNoAttachmentsFunctions {

		public delegate void PFN_glFramebufferParameteri(uint target, uint pname, int param);
		[ExternFunction(AltNames = new string[] { "glFramebufferParameteriARB" })]
		public PFN_glFramebufferParameteri glFramebufferParameteri;
		public delegate void PFN_glGetFramebufferParameteriv(uint target, uint pname, [NativeType("GLint*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glGetFramebufferParameterivARB" })]
		public PFN_glGetFramebufferParameteriv glGetFramebufferParameteriv;

	}
#nullable restore

	public class ARBFramebufferNoAttachments : IGLObject {

		public GL GL { get; }
		public ARBFramebufferNoAttachmentsFunctions Functions { get; } = new();

		public ARBFramebufferNoAttachments(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferParameter(GLFramebufferTarget target, GLFramebufferParameter pname, int param) => Functions.glFramebufferParameteri((uint)target, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferParameteri(GLFramebufferTarget target, GLFramebufferParameter pname) {
			unsafe {
				int v = 0;
				Functions.glGetFramebufferParameteriv((uint)target, (uint)pname, (IntPtr)(&v));
				return v;
			}
		}

	}
}
