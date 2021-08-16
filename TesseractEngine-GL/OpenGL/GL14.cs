using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	public class GL14 : GL13 {

		public GL14Functions FunctionsGL14 { get; } = new();

		public GL14(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL14);
		}

		public Vector4 BlendColor {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL14.glBlendColor(value.X, value.Y, value.Z, value.W);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Vector4 value = Vector4.Zero;
				unsafe {
					GL.GL11.FunctionsGL11.glGetFloatv(GLEnums.GL_BLEND_COLOR, (IntPtr)(&value));
				}
				return value;
			}
		}

		public GLBlendFunction BlendEquation {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL14.glBlendEquation((uint)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFunction)GL.GL11.GetInteger(GLEnums.GL_BLEND_EQUATION);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(GLBlendFactor sfactorRGB, GLBlendFactor dfactorRGB, GLBlendFactor sfactorAlpha, GLBlendFactor dfactorAlpha) => FunctionsGL14.glBlendFuncSeparate((uint)sfactorRGB, (uint)dfactorRGB, (uint)sfactorAlpha, (uint)dfactorAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArrays(GLDrawMode mode, in ReadOnlySpan<int> first, in ReadOnlySpan<int> count) {
			unsafe {
				fixed(int* pFirst = first) {
					fixed(int* pCount = count) {
						FunctionsGL14.glMultiDrawArrays((uint)mode, (IntPtr)pFirst, (IntPtr)pCount, Math.Min(first.Length, count.Length));
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElements(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<IntPtr> indices) {
			unsafe {
				fixed(int* pCount = count) {
					fixed(IntPtr* pIndices = indices) {
						FunctionsGL14.glMultiDrawElements((uint)mode, (IntPtr)pCount, (uint)type, (IntPtr)pIndices, Math.Min(count.Length, indices.Length));
					}
				}
			}
		}

		public float PointFadeThresholdSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL14.glPointParameterf(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE, value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.GL11.GetFloat(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE);
		}

		public GLOrigin PointSpriteCoordOrigin {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => FunctionsGL14.glPointParameteri(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN, (int)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLOrigin)GL.GL11.GetInteger(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN);
		}

	}

}
