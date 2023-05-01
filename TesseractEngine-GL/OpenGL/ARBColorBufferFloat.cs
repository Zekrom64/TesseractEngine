using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBColorBufferFloatFunctions {

		[ExternFunction(AltNames = new string[] { "glClampColorARB" })]
		[NativeType("void glClampColor(GLenum target, GLenum clamp)")]
		public delegate* unmanaged<uint, uint, void> glClampColor;

	}

	public class ARBColorBufferFloat : IGLObject {

		public GL GL { get; }
		public ARBColorBufferFloatFunctions Functions { get; } = new();

		public ARBColorBufferFloat(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampColor(GLClampColorTarget target, GLClampColorMode mode) {
			unsafe {
				Functions.glClampColor((uint)target, (uint)mode);
			}
		}
	}

}
