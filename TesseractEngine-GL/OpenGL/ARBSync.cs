using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBSyncFunctions {

		[ExternFunction(AltNames = new string[] { "glFenceSyncARB" })]
		[NativeType("GLsync glFenceSync(GLenum condition, GLbitfield flags)")]
		public delegate* unmanaged<uint, uint, nuint> glFenceSync;
		public delegate byte PFN_glIsSync(nuint sync);
		[ExternFunction(AltNames = new string[] { "glIsSync" })]
		[NativeType("GLboolean glIsSync(GLsync sync)")]
		public delegate* unmanaged<nuint, byte> glIsSync;
		public delegate void PFN_glDeleteSync(nuint sync);
		[ExternFunction(AltNames = new string[] { "glDeleteSync" })]
		[NativeType("void glDeleteSync(GLsync sync)")]
		public delegate* unmanaged<nuint, void> glDeleteSync;
		[ExternFunction(AltNames = new string[] { "glClientWaitSync" })]
		[NativeType("GLenum glClientWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)")]
		public delegate* unmanaged<nuint, uint, ulong, GLWaitResult> glClientWaitSync;
		[ExternFunction(AltNames = new string[] { "glWaitSync" })]
		[NativeType("void glWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)")]
		public delegate* unmanaged<nuint, uint, ulong, void> glWaitSync;
		[ExternFunction(AltNames = new string[] { "glGetInteger64v" })]
		[NativeType("void glGetInteger64v(GLenum pname, GLint64* pParams)")]
		public delegate* unmanaged<uint, ulong*, void> glGetInteger64v;
		[ExternFunction(AltNames = new string[] { "glGetSynciv" })]
		[NativeType("void glGetSynciv(GLsync sync, GLenum pname, GLsizei bufSize, GLsizei* pLength, GLint* pValues)")]
		public delegate* unmanaged<nuint, uint, int, out int, int*, void> glGetSynciv;

	}

	public class ARBSync : IGLObject {

		public GL GL { get; }
		public ARBSyncFunctions Functions { get; } = new();

		public ARBSync(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public nuint FenceSync(GLSyncCondition condition, uint flags = 0) {
			unsafe {
				return Functions.glFenceSync((uint)condition, flags);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSync(nuint sync) {
			unsafe {
				return Functions.glIsSync(sync) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSync(nuint sync) {
			unsafe {
				Functions.glDeleteSync(sync);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLWaitResult ClientWaitSync(nuint sync, GLSyncFlags flags, ulong timeout) {
			unsafe {
				return Functions.glClientWaitSync(sync, (uint)flags, timeout);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitSync(nuint sync, GLSyncFlags flags, ulong timeout) {
			unsafe {
				Functions.glWaitSync(sync, (uint)flags, timeout);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetInteger64(uint pname) {
			ulong val = 0;
			unsafe {
				Functions.glGetInteger64v(pname, &val);
			}
			return val;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSync(nuint sync, GLGetSync pname) {
			int val = 0;
			unsafe {
				Functions.glGetSynciv(sync, (uint)pname, 1, out int _, &val);
			}
			return val;
		}

	}

}
