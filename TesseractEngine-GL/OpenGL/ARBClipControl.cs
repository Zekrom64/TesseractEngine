using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBClipControlFunctions {

		[ExternFunction(AltNames = new string[] { "glClipControlARB" })]
		[NativeType("void glClipControl(GLenum origin, GLenum depth)")]
		public delegate* unmanaged<uint, uint, void> glClipControl;

	}

	public class ARBClipControl : IGLObject {

		public GL GL { get; }
		public ARBClipControlFunctions Functions { get; } = new();

		public ARBClipControl(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClipControl(GLOrigin origin, GLClipDepth depth) {
			unsafe {
				Functions.glClipControl((uint)origin, (uint)depth);
			}
		}
	}
}
