using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBTimerQueryFunctions {

		[ExternFunction(AltNames = new string[] { "glQueryCounterARB" })]
		[NativeType("void glQueryCOunter(GLuint id, GLenum target)")]
		public delegate* unmanaged<uint, uint, void> glQueryCounter;
		[ExternFunction(AltNames = new string[] { "glGetQueryObjecti64vARB" })]
		[NativeType("void glGetQueryObjecti64v(GLuint id, GLenum pname, GLint64* pParams)")]
		public delegate* unmanaged<uint, uint, long*, void> glGetQueryObjecti64v;
		[ExternFunction(AltNames = new string[] { "glGetQueryObjectui64vARB" })]
		[NativeType("void glGetQueryObjectui64v(GLuint id, GLenum pname, GLuint64* pParams)")]
		public delegate* unmanaged<uint, uint, ulong*, void> glGetQueryObjectui64v;

	}

	public class ARBTimerQuery : IGLObject {

		public GL GL { get; }
		public ARBTimerQueryFunctions Functions { get; } = new();

		public ARBTimerQuery(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void QueryCounter(uint id, GLQueryCounterTarget target) {
			unsafe {
				Functions.glQueryCounter(id, (uint)target);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long GetQueryObjecti64(uint id, GLGetQueryObject pname) {
			long val = 0;
			unsafe {
				Functions.glGetQueryObjecti64v(id, (uint)pname, &val);
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti64(uint id, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryObjecti64v(id, (uint)pname, (long*)offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetQueryObjectui64(uint id, GLGetQueryObject pname) {
			ulong val = 0;
			unsafe {
				Functions.glGetQueryObjectui64v(id, (uint)pname, &val);
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui64(uint id, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryObjectui64v(id, (uint)pname, (ulong*)offset);
			}
		}
	}

}
