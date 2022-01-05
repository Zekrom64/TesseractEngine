using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.OpenGL {

	public class ARBGPUShaderInt64 : IGLObject {
		
		public GL GL { get; }

		public ARBGPUShaderInt64(GL gl, IGLContext context) {
			GL = gl;
		}

	}
}
