using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {
	
	public class NVPrimitiveRestartFunctions {

		public delegate void PFN_glPrimitiveRestart();
		[ExternFunction(AltNames = new string[] { "glPrimitiveRestartNV" })]
		public PFN_glPrimitiveRestart glPrimitiveRestart;
		public delegate void PFN_glPrimitiveRestartIndex(uint index);
		[ExternFunction(AltNames = new string[] { "glPrimitiveRestartIndexNV" })]
		public PFN_glPrimitiveRestartIndex glPrimitiveRestartIndex;

	}

	public class NVPrimitiveRestart : IGLObject {

		public GL GL { get; }
		public NVPrimitiveRestartFunctions Functions { get; } = new();

		public NVPrimitiveRestart(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestart() => Functions.glPrimitiveRestart();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestartIndex(uint index) => Functions.glPrimitiveRestartIndex(index);

	}

}
