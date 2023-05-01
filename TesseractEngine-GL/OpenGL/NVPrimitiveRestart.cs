using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class NVPrimitiveRestartFunctions {

		[ExternFunction(AltNames = new string[] { "glPrimitiveRestartNV" })]
		[NativeType("void glPrimitiveRestart()")]
		public delegate* unmanaged<void> glPrimitiveRestart;
		[ExternFunction(AltNames = new string[] { "glPrimitiveRestartIndexNV" })]
		[NativeType("void glPrimitiveRestartIndex(GLuint index)")]
		public delegate* unmanaged<uint, void> glPrimitiveRestartIndex;

	}

	public class NVPrimitiveRestart : IGLObject {

		public GL GL { get; }
		public NVPrimitiveRestartFunctions Functions { get; } = new();

		public NVPrimitiveRestart(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestart() {
			unsafe {
				Functions.glPrimitiveRestart();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestartIndex(uint index) {
			unsafe {
				Functions.glPrimitiveRestartIndex(index);
			}
		}
	}

}
