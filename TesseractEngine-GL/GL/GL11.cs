using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;
using Tesseract.Core.Util;
using Tesseract.GL.Native;

namespace Tesseract.GL {

	public class GL11 : IGLObject {

		public GL GL { get; }
		public GL11Functions Functions { get; }

		public GL11(GL gl, IGLContext context) {
			GL = gl;
			Functions = new GL11Functions();
			Library.LoadFunctions(name => context.GetGLProcAddress(name), Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTexture(GLTextureTarget target, uint texture) => Functions.glBindTexture((uint)target, texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(GLBlendFactor sfactor, GLBlendFactor dfactor) => Functions.glBlendFunc((uint)sfactor, (uint)dfactor);

		public GLBlendFactor BlendSrc {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFactor)GetInteger(GLEnums.GL_BLEND_SRC);
		}

		public GLBlendFactor BlendDst {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLBlendFactor)GetInteger(GLEnums.GL_BLEND_DST);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear(GLClearMask mask) => Functions.glClear((uint)mask);

		public Vector4 ColorClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glClearColor(value.X, value.Y, value.Z, value.W);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<float> values = GetFloatv(GLEnums.GL_COLOR_CLEAR_VALUE, stackalloc float[4]);
				return new Vector4(values[0], values[1], values[2], values[3]);
			}
		}

		public double DepthClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glClearDepth(value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetDouble(GLEnums.GL_DEPTH_CLEAR_VALUE);
		}

		public int StencilClearValue {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glClearStencil(value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetInteger(GLEnums.GL_STENCIL_CLEAR_VALUE);
		}

		public int StencilBits {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetInteger(GLEnums.GL_STENCIL_BITS);
		}

		public Tuple4<bool> ColorMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glColorMask((byte)(value.X ? 1 : 0), (byte)(value.Y ? 1 : 0), (byte)(value.Z ? 1 : 0), (byte)(value.W ? 1 : 0));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<byte> values = GetBooleanv(GLEnums.GL_COLOR_WRITEMASK, stackalloc byte[4]);
				return new(values[0] != 0, values[1] != 0, values[2] != 0, values[3] != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int x, int y, int width, int border) => Functions.glCopyTexImage1D((uint)target, level, (uint)internalFormat, x, y, width, border);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int x, int y, int width, int height, int border) => Functions.glCopyTexImage2D((uint)target, level, (uint)internalFormat, x, y, width, height, border);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage1D(GLTextureTarget target, int level, int xoffset, int x, int y, int width) => Functions.glCopyTexSubImage1D((uint)target, level, xoffset, x, y, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTexSubImage2D(GLTextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => Functions.glCopyTexSubImage2D((uint)target, level, xoffset, yoffset, x, y, width, height);

		public bool CullFace {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetBoolean(GLEnums.GL_CULL_FACE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				if (value) Functions.glEnable(GLEnums.GL_CULL_FACE);
				else Functions.glDisable(GLEnums.GL_CULL_FACE);
			}
		}

		public GLFace CullFaceMode {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLFace)GetInteger(GLEnums.GL_CULL_FACE_MODE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glCullFace((uint)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(in ReadOnlySpan<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glDeleteTextures(textures.Length, (IntPtr)pTextures);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTextures(params uint[] textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glDeleteTextures(textures.Length, (IntPtr)pTextures);
				}
			}
		}

		public GLCompareFunc DepthFunc {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glDepthFunc((uint)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLCompareFunc)GetInteger(GLEnums.GL_DEPTH_FUNC);
		}

		public bool DepthMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glDepthMask((byte)(value ? 1 : 0));
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetBoolean(GLEnums.GL_DEPTH_WRITEMASK);
		}

		public Vector2d DepthRange {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glDepthRange(value.X, value.Y);
			get {
				Span<double> values = GetDoublev(GLEnums.GL_DEPTH_RANGE, stackalloc double[2]);
				return new Vector2d(values[0], values[1]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLCapability cap) => Functions.glDisable((uint)cap);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArrays(GLDrawMode mode, int first, int count) => Functions.glDrawArrays((uint)mode, first, count);

		public GLDrawBuffer DrawBuffer {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLDrawBuffer)GetInteger(GLEnums.GL_DRAW_BUFFER);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glDrawBuffer((uint)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElements(GLDrawMode mode, int count, GLIndexType type, nint offset = 0) => Functions.glDrawElements((uint)mode, count, (uint)type, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLCapability cap) => Functions.glEnable((uint)cap);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Finish() => Functions.glFinish();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flush() => Functions.glFlush();

		public GLCullFace FrontFace {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glFrontFace((uint)value);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLCullFace)GetInteger(GLEnums.GL_FRONT_FACE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenTextures(in Span<uint> textures) {
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glGenTextures(textures.Length, (IntPtr)pTextures);
				}
			}
			return textures;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenTextures(int n) {
			uint[] textures = new uint[n];
			unsafe {
				fixed (uint* pTextures = textures) {
					Functions.glGenTextures(textures.Length, (IntPtr)pTextures);
				}
			}
			return textures;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenTexture() => GenTextures(stackalloc uint[1])[0];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(uint pname) {
			unsafe {
				byte value = 0;
				Functions.glGetBooleanv(pname, (IntPtr)(&value));
				return value != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<byte> GetBooleanv(uint pname, in Span<byte> values) {
			unsafe {
				fixed (byte* pValues = values) {
					Functions.glGetBooleanv(pname, (IntPtr)pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetDouble(uint pname) {
			unsafe {
				double value = 0;
				Functions.glGetDoublev(pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<double> GetDoublev(uint pname, in Span<double> values) {
			unsafe {
				fixed (double* pValues = values) {
					Functions.glGetDoublev(pname, (IntPtr)pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLError GetError() => (GLError)Functions.glGetError();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetFloat(uint pname) {
			unsafe {
				float value = 0;
				Functions.glGetFloatv(pname, (IntPtr)(&value));
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetFloatv(uint pname, in Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glGetFloatv(pname, (IntPtr)pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInteger(uint pname) {
			unsafe {
				int value = 0;
				Functions.glGetIntegerv(pname, (IntPtr)(&value));
				return value;
			}
		}

		public Span<int> GetIntegerv(uint pname, in Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetIntegerv(pname, (IntPtr)pValues);
				}
				return values;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr GetPointer(uint pname) {
			unsafe {
				IntPtr value = IntPtr.Zero;
				Functions.glGetPointerv(pname, (IntPtr)(&value));
				return value;
			}
		}

		public float PointSize {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetFloat(GLEnums.GL_POINT_SIZE);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glPointSize(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonMode(GLFace face, GLPolygonMode mode) => Functions.glPolygonMode((uint)face, (uint)mode);

		public GLDrawBuffer ReadBuffer {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (GLDrawBuffer)GetInteger(GLEnums.GL_READ_BUFFER);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glReadBuffer((uint)value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Scissor(int x, int y, int width, int height) => Functions.glScissor(x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilFunc(GLStencilFunc func, int reference, uint mask) => Functions.glStencilFunc((uint)func, reference, mask);

		public uint StencilMask {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (uint)GetInteger(GLEnums.GL_STENCIL_VALUE_MASK);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glStencilMask(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void StencilOp(GLStencilOp sfail, GLStencilOp dpfail, GLStencilOp dppass) => Functions.glStencilOp((uint)sfail, (uint)dpfail, (uint)dppass);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLType type, IntPtr pixels) => Functions.glTexImage1D((uint)target, level, (int)internalFormat, width, border, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexImage1D(target, level, internalFormat, width, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage1D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int border, GLFormat format, GLType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage1D(target, level, internalFormat, width, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLType type, IntPtr pixels) => Functions.glTexImage2D((uint)target, level, (int)internalFormat, width, height, border, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage2D(target, level, internalFormat, width, height, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2D<T>(GLTextureTarget target, int level, GLInternalFormat internalFormat, int width, int height, int border, GLFormat format, GLType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexImage2D(target, level, internalFormat, width, height, border, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, float param) => Functions.glTexParameterf((uint)target, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<float> param) {
			unsafe {
				fixed(float* pParam = param) {
					Functions.glTexParameterfv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, params float[] param) {
			unsafe {
				fixed (float* pParam = param) {
					Functions.glTexParameterfv((uint)target, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, int param) => Functions.glTexParameteri((uint)target, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLType type, IntPtr pixels) => Functions.glTexSubImage1D((uint)target, level, xoffset, width, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D<T>(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage1D(target, level, xoffset, width, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage1D<T>(GLTextureTarget target, int level, int xoffset, int width, GLFormat format, GLType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage1D(target, level, xoffset, width, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLType type, IntPtr pixels) => Functions.glTexSubImage2D((uint)target, level, xoffset, yoffset, width, height, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					TexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexSubImage2D<T>(GLTextureTarget target, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLType type, params T[] pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					TexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, (IntPtr)pPixels);
				}
			}
		}

		public Recti Viewport {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				Span<int> viewport = GetIntegerv(GLEnums.GL_VIEWPORT, stackalloc int[4]);
				return new Recti(viewport[0], viewport[1], viewport[2], viewport[3]);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Functions.glViewport(value.Position.X, value.Position.Y, value.Size.X, value.Size.Y);
		}

	}
}
