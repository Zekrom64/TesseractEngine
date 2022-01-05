using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBSyncFunctions {

		public delegate nuint PFN_glFenceSync(uint condition, uint flags);
		[ExternFunction(AltNames = new string[] { "glFenceSyncARB" })]
		public PFN_glFenceSync glFenceSync;
		public delegate byte PFN_glIsSync(nuint sync);
		[ExternFunction(AltNames = new string[] { "glIsSync" })]
		public PFN_glIsSync glIsSync;
		public delegate void PFN_glDeleteSync(nuint sync);
		[ExternFunction(AltNames = new string[] { "glDeleteSync" })]
		public PFN_glDeleteSync glDeleteSync;
		public delegate GLWaitResult PFN_glClientWaitSync(nuint sync, uint flags, ulong timeout);
		[ExternFunction(AltNames = new string[] { "glClientWaitSync" })]
		public PFN_glClientWaitSync glClientWaitSync;
		public delegate void PFN_glWaitSync(nuint sync, uint flags, ulong timeout);
		[ExternFunction(AltNames = new string[] { "glWaitSync" })]
		public PFN_glWaitSync glWaitSync;
		public delegate void PFN_glGetInteger64v(uint pname, [NativeType("GLint64*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetInteger64v" })]
		public PFN_glGetInteger64v glGetInteger64v;
		public delegate void PFN_glGetSynciv(nuint sync, uint pname, int bufSize, out int length, [NativeType("GLint*")] IntPtr values);
		[ExternFunction(AltNames = new string[] { "glGetSynciv" })]
		public PFN_glGetSynciv glGetSynciv;

	}
#nullable restore

	public class ARBSync : IGLObject {

		public GL GL { get; }
		public ARBSyncFunctions Functions { get; } = new();

		public ARBSync(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public nuint FenceSync(GLSyncCondition condition, uint flags = 0) => Functions.glFenceSync((uint)condition, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSync(nuint sync) => Functions.glIsSync(sync) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSync(nuint sync) => Functions.glDeleteSync(sync);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLWaitResult ClientWaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => Functions.glClientWaitSync(sync, (uint)flags, timeout);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => Functions.glWaitSync(sync, (uint)flags, timeout);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetInteger64(uint pname, uint index) {
			ulong val = 0;
			unsafe {
				Functions.glGetInteger64v(pname, (IntPtr)(&val));
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSync(nuint sync, GLGetSync pname) {
			int val = 0;
			unsafe {
				Functions.glGetSynciv(sync, (uint)pname, 1, out int _, (IntPtr)(&val));
			}
			return val;
		}

	}

}
