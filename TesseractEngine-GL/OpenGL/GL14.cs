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

	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Byte;
	using GLbyte = SByte;
	using GLshort = Int16;
	using GLubyte = Byte;
	using GLushort = UInt16;
	using GLulong = UInt64;
	using GLfloat = Single;
	using GLclampf = Single;
	using GLdouble = Double;
	using GLclampd = Double;
	using GLint64 = Int64;
	using GLuint64 = UInt64;
	using GLintptr = IntPtr;
	using GLsizeiptr = IntPtr;

	public unsafe class GL14Functions {

		public delegate void PFN_glBlendColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha);
		public delegate void PFN_glBlendEquation(GLenum mode);
		public delegate void PFN_glBlendFuncSeparate(GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha);
		public delegate void PFN_glMultiDrawArrays(GLenum mode, [NativeType("const GLint*")] IntPtr first, [NativeType("const GLint*")] IntPtr count, GLsizei drawcount);
		public delegate void PFN_glMultiDrawElements(GLenum mode, [NativeType("const GLsizei*")] IntPtr count, GLenum type, [NativeType("const void* const*")] IntPtr indices, GLsizei drawcount);
		public delegate void PFN_glPointParameterf(GLenum pname, GLfloat param);
		public delegate void PFN_glPointParameterfv(GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glPointParameteri(GLenum pname, GLint param);
		public delegate void PFN_glPointParameteriv(GLenum pname, [NativeType("const GLint*")] IntPtr _params);

		[NativeType("void glBlendColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha)")]
		public delegate* unmanaged<float, float, float, float, void> glBlendColor;
		[NativeType("void glBlendEquation(GLenum mode)")]
		public delegate* unmanaged<uint, void> glBlendEquation;
		[NativeType("void glBlendFuncSeparate(GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha)")]
		public delegate* unmanaged<uint, uint, uint, uint, void> glBlendFuncSeparate;
		[NativeType("void glMultiDrawArrays(GLenum mode, const GLint* pFirst, const GLint* pCount, GLsizei drawCount)")]
		public delegate* unmanaged<uint, int*, int*, int, void> glMultiDrawArrays;
		[NativeType("void glMultiDrawElements(GLenum mode, const GLsizei* pCount, GLenum type, const void* const* ppIndices, GLsizei drawCount)")]
		public delegate* unmanaged<uint, int*, uint, IntPtr*, int, void> glMultiDrawElements;
		[NativeType("void glPointParameterf(GLenum pname, GLfloat param)")]
		public delegate* unmanaged<uint, float, void> glPointParameterf;
		[NativeType("void glPointParameterfv(GLenum pname, const GLfloat* pParams)")]
		public delegate* unmanaged<uint, float*, void> glPointParameterfv;
		[NativeType("void glPointParameteri(GLenum pname, GLint param)")]
		public delegate* unmanaged<uint, int, void> glPointParameteri;
		[NativeType("void glPointParameteriv(GLenum pname, const GLint* pParams)")]
		public delegate* unmanaged<uint, int*, void> glPointParameteriv;

	}

	public class GL14 : GL13 {

		public GL14Functions FunctionsGL14 { get; } = new();

		public GL14(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL14);
		}

		public Vector4 BlendColor {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL14.glBlendColor(value.X, value.Y, value.Z, value.W);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Vector4 value = Vector4.Zero;
				unsafe {
					GL.GL11.FunctionsGL11.glGetFloatv(GLEnums.GL_BLEND_COLOR, (float*)(&value));
				}
				return value;
			}
		}

		public GLBlendFunction BlendEquation {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL14.glBlendEquation((uint)value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFunction)GL.GL11.GetInteger(GLEnums.GL_BLEND_EQUATION);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(GLBlendFactor sfactorRGB, GLBlendFactor dfactorRGB, GLBlendFactor sfactorAlpha, GLBlendFactor dfactorAlpha) {
			unsafe {
				FunctionsGL14.glBlendFuncSeparate((uint)sfactorRGB, (uint)dfactorRGB, (uint)sfactorAlpha, (uint)dfactorAlpha);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArrays(GLDrawMode mode, in ReadOnlySpan<int> first, in ReadOnlySpan<int> count) {
			unsafe {
				fixed(int* pFirst = first) {
					fixed(int* pCount = count) {
						FunctionsGL14.glMultiDrawArrays((uint)mode, pFirst, pCount, Math.Min(first.Length, count.Length));
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElements(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<IntPtr> indices) {
			unsafe {
				fixed(int* pCount = count) {
					fixed(IntPtr* pIndices = indices) {
						FunctionsGL14.glMultiDrawElements((uint)mode, pCount, (uint)type, pIndices, Math.Min(count.Length, indices.Length));
					}
				}
			}
		}

		public float PointFadeThresholdSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL14.glPointParameterf(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE, value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.GL11.GetFloat(GLEnums.GL_POINT_FADE_THRESHOLD_SIZE);
		}

		public GLOrigin PointSpriteCoordOrigin {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					FunctionsGL14.glPointParameteri(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN, (int)value);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLOrigin)GL.GL11.GetInteger(GLEnums.GL_POINT_SPRITE_COORD_ORIGIN);
		}

	}

}
