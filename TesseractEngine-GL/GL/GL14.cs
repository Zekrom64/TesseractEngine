using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public class GL14 : IGLObject {

		public GL GL { get; }
		public GL14Functions Functions { get; }

		public GL14(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL14Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		public Vector4 BlendColor {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glBlendColor(value.X, value.Y, value.Z, value.W);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Vector4 value = Vector4.Zero;
				unsafe {
					GL.GL11.Functions.glGetFloatv(GLEnums.GL_BLEND_COLOR, (IntPtr)(&value));
				}
				return value;
			}
		}

		public GLBlendFunction BlendEquation {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glBlendEquation((uint)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFunction)GL.GL11.GetInteger(GLEnums.GL_BLEND_EQUATION);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(GLBlendFactor sfactorRGB, GLBlendFactor dfactorRGB, GLBlendFactor sfactorAlpha, GLBlendFactor dfactorAlpha) => Functions.glBlendFuncSeparate((uint)sfactorRGB, (uint)dfactorRGB, (uint)sfactorAlpha, (uint)dfactorAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArrays(GLDrawMode mode, in ReadOnlySpan<int> first, in ReadOnlySpan<int> count) {
			unsafe {
				fixed(int* pFirst = first) {
					fixed(int* pCount = count) {
						Functions.glMultiDrawArrays((uint)mode, (IntPtr)pFirst, (IntPtr)pCount, Math.Min(first.Length, count.Length));
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElements(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<IntPtr> indices) {
			unsafe {
				fixed(int* pCount = count) {
					fixed(IntPtr* pIndices = indices) {
						Functions.glMultiDrawElements((uint)mode, (IntPtr)pCount, (uint)type, (IntPtr)pIndices, Math.Min(count.Length, indices.Length));
					}
				}
			}
		}

		public float PointFadeThresholdSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glPointParameterf(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE, value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.GL11.GetFloat(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE);
		}

		public GLPointOrigin PointSpriteCoordOrigin {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glPointParameteri(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN, (int)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLPointOrigin)GL.GL11.GetInteger(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN);
		}

	}

}
