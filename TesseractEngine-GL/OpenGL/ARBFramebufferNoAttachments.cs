using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBFramebufferNoAttachmentsFunctions {

		[ExternFunction(AltNames = new string[] { "glFramebufferParameteriARB" })]
		[NativeType("void glFramebufferParameteri(GLenum target, GLenum pname, GLint param)")]
		public delegate* unmanaged<uint, uint, int, void> glFramebufferParameteri;
		[ExternFunction(AltNames = new string[] { "glGetFramebufferParameterivARB" })]
		[NativeType("void glGetFramebufferParameteriv(GLenum target, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, out int, void> glGetFramebufferParameteriv;

	}

	public class ARBFramebufferNoAttachments : IGLObject {

		public GL GL { get; }
		public ARBFramebufferNoAttachmentsFunctions Functions { get; } = new();

		public ARBFramebufferNoAttachments(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferParameter(GLFramebufferTarget target, GLFramebufferParameter pname, int param) {
			unsafe {
				Functions.glFramebufferParameteri((uint)target, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferParameteri(GLFramebufferTarget target, GLFramebufferParameter pname) {
			unsafe {
				Functions.glGetFramebufferParameteriv((uint)target, (uint)pname, out int v);
				return v;
			}
		}

	}
}
