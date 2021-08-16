using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class ARBViewportArrayFunctions {

		public delegate void PFN_glViewportArrayv(uint first, int count, [NativeType("const GLfloat*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glViewportArrayvARB" })]
		public PFN_glViewportArrayv glViewportArrayv;
		public delegate void PFN_glViewportIndexedf(uint index, float x, float y, float w, float h);
		[ExternFunction(AltNames = new string[] { "glViewportIndexedfARB" })]
		public PFN_glViewportIndexedf glViewportIndexedf;
		public delegate void PFN_glViewportIndexedfv(uint index, [NativeType("const GLfloat*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glViewportIndexedfvARB" })]
		public PFN_glViewportIndexedfv glViewportIndexedfv;
		public delegate void PFN_glScissorArrayv(uint first, int count, [NativeType("const GLint*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glScissorArrayvARB" })]
		public PFN_glScissorArrayv glScissorArrayv;
		public delegate void PFN_glScissorIndexed(uint index, int left, int bottom, int width, int height);
		[ExternFunction(AltNames = new string[] { "glScissorIndexedARB" })]
		public PFN_glScissorIndexed glScissorIndexed;
		public delegate void PFN_glScissorIndexedv(uint index, [NativeType("const GLint*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glScsissorIndexedvARB" })]
		public PFN_glScissorIndexedv glScissorIndexedv;
		public delegate void PFN_glDepthRangeArrayv(uint first, int count, [NativeType("const GLclampd*")] IntPtr v);
		[ExternFunction(AltNames = new string[] { "glDepthRangeArrayvARB" })]
		public PFN_glDepthRangeArrayv glDepthRangeArrayv;
		public delegate void PFN_glDepthRangeIndexed(uint index, double n, double f);
		[ExternFunction(AltNames = new string[] { "glDepthRangeIndexedARB" })]
		public PFN_glDepthRangeIndexed glDepthRangeIndexed;
		public delegate void PFN_glGetFloati_v(uint target, uint index, [NativeType("GLfloat*")] IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetFloati_vARB" })]
		public PFN_glGetFloati_v glGetFloati_v;
		public delegate void PFN_glGetDoublei_v(uint target, uint index, [NativeType("GLdouble*")] IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetDoublei_vARB" })]
		public PFN_glGetDoublei_v glGetDoublei_v;

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
					Functions.glViewportArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportArray(uint first, params Rectf[] v) {
			unsafe {
				fixed (Rectf* pV = v) {
					Functions.glViewportArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, float x, float y, float w, float h) => Functions.glViewportIndexedf(index, x, y, w, h);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, Rectf v) {
			unsafe {
				Functions.glViewportIndexedfv(index, (IntPtr)(&v));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(in ReadOnlySpan<Recti> v, uint first = 0) {
			unsafe {
				fixed(Recti* pV = v) {
					Functions.glScissorArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(uint first, params Recti[] v) {
			unsafe {
				fixed (Recti* pV = v) {
					Functions.glScissorArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, int left, int bottom, int width, int height) => Functions.glScissorIndexed(index, left, bottom, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, Recti v) {
			unsafe {
				Functions.glScissorIndexedv(index, (IntPtr)(&v));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(in ReadOnlySpan<Vector2d> v, uint first = 0) {
			unsafe {
				fixed(Vector2d* pV = v) {
					Functions.glDepthRangeArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(uint first, params Vector2d[] v) {
			unsafe {
				fixed (Vector2d* pV = v) {
					Functions.glDepthRangeArrayv(first, v.Length, (IntPtr)pV);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeIndexed(uint index, double n, double f) => Functions.glDepthRangeIndexed(index, n, f);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetFloat(uint pname, uint index, Span<float> v) {
			unsafe {
				fixed(float* pV = v) {
					Functions.glGetFloati_v(pname, index, (IntPtr)pV);
				}
			}
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<double> GetDouble(uint pname, uint index, Span<double> v) {
			unsafe {
				fixed(double* pV = v) {
					Functions.glGetDoublei_v(pname, index, (IntPtr)pV);
				}
			}
			return v;
		}

	}

}
