using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBDrawBuffersBlendFunctions {

		[ExternFunction(AltNames = new string[] { "glBlendEquationiARB" })]
		[NativeType("void glBlendEquationi(GLenum buffer, GLenum mode)")]
		public delegate* unmanaged<uint, uint, void> glBlendEquationi;
		[ExternFunction(AltNames = new string[] { "glBlendEquationSeparateiARB" })]
		[NativeType("void glBlendEquationSeparatei(GLenum buffer, GLenum modeRGB, GLenum modeAlpha)")]
		public delegate* unmanaged<uint, uint, uint, void> glBlendEquationSeparatei;
		[ExternFunction(AltNames = new string[] { "glBlendFunciARB" })]
		[NativeType("void glBlendFunci(GLenum buf, GLenum src, GLenum dst)")]
		public delegate* unmanaged<uint, uint, uint, void> glBlendFunci;
		[ExternFunction(AltNames = new string[] { "glBlendFuncSeparateiARB" })]
		[NativeType("void glBlendFuncSeparatei(GLenum buf, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha)")]
		public delegate* unmanaged<uint, uint, uint, uint, uint, void> glBlendFuncSeparatei;

	}

	public class ARBDrawBuffersBlend : IGLObject {

		public GL GL { get; }
		public ARBDrawBuffersBlendFunctions Functions { get; } = new();

		public ARBDrawBuffersBlend(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquation(uint buffer, GLBlendFunction mode) {
			unsafe {
				Functions.glBlendEquationi(buffer, (uint)mode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(uint buffer, GLBlendFunction modeRGB, GLBlendFunction modeAlpha) {
			unsafe {
				Functions.glBlendEquationSeparatei(buffer, (uint)modeRGB, (uint)modeAlpha);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(uint buffer, GLBlendFactor src, GLBlendFactor dst) {
			unsafe {
				Functions.glBlendFunci(buffer, (uint)src, (uint)dst);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(uint buffer, GLBlendFactor srcRGB, GLBlendFactor dstRGB, GLBlendFactor srcAlpha, GLBlendFactor dstAlpha) {
			unsafe {
				Functions.glBlendFuncSeparatei(buffer, (uint)srcRGB, (uint)dstRGB, (uint)srcAlpha, (uint)dstAlpha);
			}
		}
	}
}
