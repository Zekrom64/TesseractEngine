using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBMapBufferRangeFunctions {

		public delegate IntPtr PFN_glMapBufferRange(uint target, nint offset, nint length, uint access);
		[ExternFunction(AltNames = new string[] { "glMapBufferRangeARB" })]
		public PFN_glMapBufferRange glMapBufferRange;
		public delegate void PFN_glFlushMappedBufferRange(uint target, nint offset, nint length);
		[ExternFunction(AltNames = new string[] { "glFlushMappedBufferRangeARB" })]
		public PFN_glFlushMappedBufferRange glFlushMappedBufferRange;

	}

	public class ARBMapBufferRange : IGLObject {

		public GL GL { get; }
		public ARBMapBufferRangeFunctions Functions { get; } = new();

		public ARBMapBufferRange(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBufferRange(GLBufferTarget target, nint offset, nint length, GLMapAccessFlags access) => Functions.glMapBufferRange((uint)target, offset, length, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedBufferRange(GLBufferTarget target, nint offset, nint length) => Functions.glFlushMappedBufferRange((uint)target, offset, length);

	}
}
