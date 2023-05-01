using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class EXTTextureIntegerFunctions {

		[ExternFunction(AltNames = new string[] { "glClearColorIiEXT" })]
		[NativeType("void glClearColorIi(GLint r, GLint g, GLint b, GLint a)")]
		public delegate* unmanaged<int, int, int, int, void> glClearColorIi;

		[ExternFunction(AltNames = new string[] { "glClearColorIuiEXT" })]
		[NativeType("void glClearColorIui(GLuint r, GLuint g, GLuint b, GLuint a)")]
		public delegate* unmanaged<uint, uint, uint, uint, void> glClearColorIui;

		[ExternFunction(AltNames = new string[] { "glTexParameterIivEXT" })]
		[NativeType("void glTexParameterIiv(GLenum target, GLenum pname, const GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glTexParameterIiv;

		[ExternFunction(AltNames = new string[] { "glTexParameterIuivEXT" })]
		[NativeType("void glTexParameterIuiv(GLenum target, GLenum pname, const GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glTexParameterIuiv;

		[ExternFunction(AltNames = new string[] { "glGetTexParameterIivEXT" })]
		[NativeType("void glGetTexParameterIiv(GLenum target, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetTexParameterIiv;

		[ExternFunction(AltNames = new string[] { "glGetTexParameterIuivEXT" })]
		[NativeType("void glGetTexParameterIuiv(GLenum target, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glGetTexParameterIuiv;

	}

	public class EXTTextureInteger : IGLObject {

		public GL GL { get; }
		public EXTTextureIntegerFunctions Functions { get; } = new();

		public EXTTextureInteger(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(int r, int g, int b, int a) {
			unsafe {
				Functions.glClearColorIi(r, g, b, a);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(uint r, uint g, uint b, uint a) {
			unsafe {
				Functions.glClearColorIui(r, g, b, a);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<int> param) {
			unsafe {
				fixed (int* pParams = param) {
					Functions.glGetTexParameterIiv((uint)target, (uint)pname, pParams);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<uint> param) {
			unsafe {
				fixed (uint* pParams = param) {
					Functions.glGetTexParameterIuiv((uint)target, (uint)pname, pParams);
				}
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTexParameterIiv((uint)target, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glTexParameterIuiv((uint)target, (uint)pname, pParam);
				}
			}
		}

	}

}
