using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBColorBufferFloatFunctions {

		public delegate void PFN_glClampColor(uint target, uint clamp);
		[ExternFunction(AltNames = new string[] { "glClampColorARB" })]
		public PFN_glClampColor glClampColor;

	}
#nullable restore

	public class ARBColorBufferFloat : IGLObject {

		public GL GL { get; }
		public ARBColorBufferFloatFunctions Functions { get; } = new();

		public ARBColorBufferFloat(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampColor(GLClampColorTarget target, GLClampColorMode mode) => Functions.glClampColor((uint)target, (uint)mode);

	}

}
