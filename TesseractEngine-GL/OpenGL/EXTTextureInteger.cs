using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class EXTTextureIntegerFunctions {

		public delegate void PFN_glClearColorIi(int r, int g, int b, int a);
		[ExternFunction(AltNames = new string[] { "glClearColorIiEXT" })]
		public PFN_glClearColorIi glClearColorIi;
		public delegate void PFN_glClearColorIui(uint r, uint g, uint b, uint a);
		[ExternFunction(AltNames = new string[] { "glClearColorIuiEXT" })]
		public PFN_glClearColorIui glClearColorIui;
		public delegate void PFN_glTexParameterIiv(uint target, uint pname, IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glTexParameterIivEXT" })]
		public PFN_glTexParameterIiv glTexParameterIiv;
		public delegate void PFN_glTexParameterIuiv(uint target, uint pname, IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glTexParameterIuivEXT" })]
		public PFN_glTexParameterIuiv glTexParameterIuiv;
		public delegate void PFN_glGetTexParameterIiv(uint target, uint pname, IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTexParameterIivEXT" })]
		public PFN_glGetTexParameterIiv glGetTexParameterIiv;
		public delegate void PFN_glGetTexParameterIuiv(uint target, uint pname, IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTexParameterIuivEXT" })]
		public PFN_glGetTexParameterIuiv glGetTexParameterIuiv;

	}
#nullable restore

	public class EXTTextureInteger : IGLObject {

		public GL GL { get; }
		public EXTTextureIntegerFunctions Functions { get; } = new();

		public EXTTextureInteger(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(int r, int g, int b, int a) => Functions.glClearColorIi(r, g, b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(uint r, uint g, uint b, uint a) => Functions.glClearColorIui(r, g, b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<int> param) {
			unsafe {
				fixed (int* pParams = param) {
					Functions.glGetTexParameterIiv((uint)target, (uint)pname, (IntPtr)pParams);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<uint> param) {
			unsafe {
				fixed (uint* pParams = param) {
					Functions.glGetTexParameterIuiv((uint)target, (uint)pname, (IntPtr)pParams);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTexParameterIiv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glTexParameterIuiv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

	}

}
