using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBCopyBufferFunctions {

		public delegate void PFN_glCopyBufferSubData(uint readTarget, uint writeTarget, nint readOffset, nint writeOffset, nint size);
		[ExternFunction(AltNames = new string[] { "glCopyBufferSubDataARB", "glCopyBufferSubDataEXT" })]
		public PFN_glCopyBufferSubData glCopyBufferSubData;

	}

	public class ARBCopyBuffer : IGLObject {

		public GL GL { get; }
		public ARBCopyBufferFunctions Functions { get; } = new();

		public ARBCopyBuffer(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferSubData(GLBufferTarget readTarget, GLBufferTarget writeTarget, nint readOffset, nint writeOffset, nint size) => Functions.glCopyBufferSubData((uint)readTarget, (uint)writeTarget, readOffset, writeOffset, size);

	}
}
