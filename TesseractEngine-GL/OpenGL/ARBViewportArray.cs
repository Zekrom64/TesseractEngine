using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBViewportArrayFunctions {

		[ExternFunction(AltNames = new string[] { "glViewportArrayvARB" })]
		[NativeType("void glViewportArrayv(GLuint first, GLsizei count, const GLfloat* pValues)")]
		public delegate* unmanaged<uint, int, float*, void> glViewportArrayv;
		[ExternFunction(AltNames = new string[] { "glViewportIndexedfARB" })]
		[NativeType("void glViewportIndexedf(GLuint index, GLfloat x, GLfloat y, GLfloat w, GLfloat h)")]
		public delegate* unmanaged<uint, float, float, float, float, void> glViewportIndexedf;
		[ExternFunction(AltNames = new string[] { "glViewportIndexedfvARB" })]
		[NativeType("void glViewportIndexedfv(GLuint index, const GLfloat* pValues)")]
		public delegate* unmanaged<uint, float*, void> glViewportIndexedfv;
		[ExternFunction(AltNames = new string[] { "glScissorArrayvARB" })]
		[NativeType("void glScissorArrayv(GLuint first, GLsizei count, const GLint* pValues)")]
		public delegate* unmanaged<uint, int, int*, void> glScissorArrayv;
		[ExternFunction(AltNames = new string[] { "glScissorIndexedARB" })]
		[NativeType("void glScissorIndexed(GLuint index, GLint left, GLint bottom, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, int, int, int, void> glScissorIndexed;
		[ExternFunction(AltNames = new string[] { "glScsissorIndexedvARB" })]
		[NativeType("void glScissorIndexedv(GLuint index, const GLint* pValues)")]
		public delegate* unmanaged<uint, int*, void> glScissorIndexedv;
		[ExternFunction(AltNames = new string[] { "glDepthRangeArrayvARB" })]
		[NativeType("void glDepthRangeArrayv(GLuint first, GLsizei count, const GLclampd* pValues)")]
		public delegate* unmanaged<uint, int, double*, void> glDepthRangeArrayv;
		[ExternFunction(AltNames = new string[] { "glDepthRangeIndexedARB" })]
		[NativeType("void glDepthRangeIndexed(GLuint index, GLclampd near, GLclampd far)")]
		public delegate* unmanaged<uint, double, double, void> glDepthRangeIndexed;
		[ExternFunction(AltNames = new string[] { "glGetFloati_vARB" })]
		[NativeType("void glGetFloati_v(GLenum target, GLuint index, GLfloat* pData)")]
		public delegate* unmanaged<uint, uint, float*, void> glGetFloati_v;
		[ExternFunction(AltNames = new string[] { "glGetDoublei_vARB" })]
		[NativeType("void glGetDoublei_v(GLenum target, GLuint index, GLdouble* pData)")]
		public delegate* unmanaged<uint, uint, double*, void> glGetDoublei_v;

	}

	public class ARBViewportArray : IGLObject {

		public GL GL { get; }
		public ARBViewportArrayFunctions Functions { get; } = new();

		public ARBViewportArray(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportArray(in ReadOnlySpan<Rectf> v, uint first = 0) {
			unsafe {
				fixed(Rectf* pV = v) {
					Functions.glViewportArrayv(first, v.Length, (float*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportArray(uint first, params Rectf[] v) {
			unsafe {
				fixed (Rectf* pV = v) {
					Functions.glViewportArrayv(first, v.Length, (float*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, float x, float y, float w, float h) {
			unsafe {
				Functions.glViewportIndexedf(index, x, y, w, h);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, Rectf v) {
			unsafe {
				Functions.glViewportIndexedfv(index, (float*)(&v));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(in ReadOnlySpan<Recti> v, uint first = 0) {
			unsafe {
				fixed(Recti* pV = v) {
					Functions.glScissorArrayv(first, v.Length, (int*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(uint first, params Recti[] v) {
			unsafe {
				fixed (Recti* pV = v) {
					Functions.glScissorArrayv(first, v.Length, (int*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, int left, int bottom, int width, int height) {
			unsafe {
				Functions.glScissorIndexed(index, left, bottom, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, Recti v) {
			unsafe {
				Functions.glScissorIndexedv(index, (int*)&v);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(in ReadOnlySpan<Vector2d> v, uint first = 0) {
			unsafe {
				fixed(Vector2d* pV = v) {
					Functions.glDepthRangeArrayv(first, v.Length, (double*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(uint first, params Vector2d[] v) {
			unsafe {
				fixed (Vector2d* pV = v) {
					Functions.glDepthRangeArrayv(first, v.Length, (double*)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeIndexed(uint index, double n, double f) {
			unsafe {
				Functions.glDepthRangeIndexed(index, n, f);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetFloat(uint pname, uint index, Span<float> v) {
			unsafe {
				fixed(float* pV = v) {
					Functions.glGetFloati_v(pname, index, pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<double> GetDouble(uint pname, uint index, Span<double> v) {
			unsafe {
				fixed(double* pV = v) {
					Functions.glGetDoublei_v(pname, index, pV);
				}
			}
			return v;
		}

	}

}
