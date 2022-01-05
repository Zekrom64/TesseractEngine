using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTimerQueryFunctions {

		public delegate void PFN_glQueryCounter(uint id, uint target);
		[ExternFunction(AltNames = new string[] { "glQueryCounterARB" })]
		public PFN_glQueryCounter glQueryCounter;
		public delegate void PFN_glGetQueryObjecti64v(uint id, uint pname, [NativeType("GLint64*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetQueryObjecti64vARB" })]
		public PFN_glGetQueryObjecti64v glGetQueryObjecti64v;
		public delegate void PFN_glGetQueryObjectui64v(uint id, uint pname, [NativeType("GLuint64*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetQueryObjectui64vARB" })]
		public PFN_glGetQueryObjectui64v glGetQueryObjectui64v;

	}
#nullable restore

	public class ARBTimerQuery : IGLObject {

		public GL GL { get; }
		public ARBTimerQueryFunctions Functions { get; } = new();

		public ARBTimerQuery(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void QueryCounter(uint id, GLQueryCounterTarget target) => Functions.glQueryCounter(id, (uint)target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long GetQueryObjecti64(uint id, GLGetQueryObject pname) {
			long val = 0;
			unsafe {
				Functions.glGetQueryObjecti64v(id, (uint)pname, (IntPtr)(&val));
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti64(uint id, GLGetQueryObject pname, nint offset) => Functions.glGetQueryObjecti64v(id, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetQueryObjectui64(uint id, GLGetQueryObject pname) {
			ulong val = 0;
			unsafe {
				Functions.glGetQueryObjectui64v(id, (uint)pname, (IntPtr)(&val));
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui64(uint id, GLGetQueryObject pname, nint offset) => Functions.glGetQueryObjectui64v(id, (uint)pname, offset);

	}

}
