using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBVertexAttrib64BitFunctions {

		public delegate void PFN_glVertexAttribLPointer(uint index, int size, uint type, int stride, IntPtr pointer);
		[ExternFunction(AltNames = new string[] { "glVertexAttribLPointerARB" })]
		public PFN_glVertexAttribLPointer glVertexAttribLPointer;

		public delegate void PFN_glVertexArrayVertexAttribLOffsetEXT(uint vaobj, uint buffer, uint index, int size, uint type, int stride, IntPtr pointer);
		[ExternFunction(Relaxed = true)]
		public PFN_glVertexArrayVertexAttribLOffsetEXT glVertexArrayVertexAttribLOffsetEXT;

	}
#nullable restore

	public class ARBVertexAttrib64Bit : IGLObject {

		public GL GL { get; }
		public ARBVertexAttrib64BitFunctions Functions { get; } = new();

		public ARBVertexAttrib64Bit(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLPointer(uint index, int size, GLType type, int stride, nint offset) => Functions.glVertexAttribLPointer(index, size, (uint)type, stride, offset);

		public void VertexArrayVertexAttribLOffset(uint vaobj, uint buffer, uint index, int size, GLType type, int stride, nint offset) =>
			Functions.glVertexArrayVertexAttribLOffsetEXT(vaobj, buffer, index, size, (uint)type, stride, offset);

	}

}
