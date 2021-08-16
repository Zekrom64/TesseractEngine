using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class ARBDrawBuffersBlendFunctions {

		public delegate void PFN_glBlendEquationi(uint buffer, uint mode);
		[ExternFunction(AltNames = new string[] { "glBlendEquationiARB" })]
		public PFN_glBlendEquationi glBlendEquationi;
		public delegate void PFN_glBlendEquationSeparatei(uint buffer, uint modeRGB, uint modeAlpha);
		[ExternFunction(AltNames = new string[] { "glBlendEquationSeparateiARB" })]
		public PFN_glBlendEquationSeparatei glBlendEquationSeparatei;
		public delegate void PFN_glBlendFunci(uint buf, uint src, uint dst);
		[ExternFunction(AltNames = new string[] { "glBlendFunciARB" })]
		public PFN_glBlendFunci glBlendFunci;
		public delegate void PFN_glBlendFuncSeparatei(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);
		[ExternFunction(AltNames = new string[] { "glBlendFuncSeparateiARB" })]
		public PFN_glBlendFuncSeparatei glBlendFuncSeparatei;

	}

	public class ARBDrawBuffersBlend : IGLObject {

		public GL GL { get; }
		public ARBDrawBuffersBlendFunctions Functions { get; } = new();

		public ARBDrawBuffersBlend(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquation(uint buffer, GLBlendFunction mode) => Functions.glBlendEquationi(buffer, (uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(uint buffer, GLBlendFunction modeRGB, GLBlendFunction modeAlpha) => Functions.glBlendEquationSeparatei(buffer, (uint)modeRGB, (uint)modeAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(uint buffer, GLBlendFactor src, GLBlendFactor dst) => Functions.glBlendFunci(buffer, (uint)src, (uint)dst);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(uint buffer, GLBlendFactor srcRGB, GLBlendFactor dstRGB, GLBlendFactor srcAlpha, GLBlendFactor dstAlpha) => Functions.glBlendFuncSeparatei(buffer, (uint)srcRGB, (uint)dstRGB, (uint)srcAlpha, (uint)dstAlpha);

	}
}
