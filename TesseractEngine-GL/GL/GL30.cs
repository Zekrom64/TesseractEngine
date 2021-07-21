using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.GL {

	public class GL30Functions {

		public delegate void PFN_glClearBufferfi(uint buffer, int drawBuffer, float depth, int stencil);
		public PFN_glClearBufferfi glClearBufferfi;
		public delegate void PFN_glClearBufferfv(uint buffer, int drawBuffer, [NativeType("const GLfloat*")] IntPtr value);
		public PFN_glClearBufferfv glClearBufferfv;
		public delegate void PFN_glClearBufferiv(uint buffer, int drawBuffer, [NativeType("const GLint*")] IntPtr value);
		public PFN_glClearBufferiv glClearBufferiv;
		public delegate void PFN_glClearBufferuiv(uint buffer, int drawBuffer, [NativeType("const GLuint*")] IntPtr value);
		public PFN_glClearBufferuiv glClearBufferuiv;
		public delegate IntPtr PFN_glGetStringi(uint name, uint index);
		public PFN_glGetStringi glGetStringi;

	}

	public class GL30 : GL20 {

		public GL30Functions FunctionsGL30 { get; }

		public GL30(GL gl, IGLContext context) : base(gl, context) { }

		// EXT_gpu_shader4

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIPointer(uint index, int size, GLType type, int stride, nint offset) => GL.EXTGPUShader4.VertexAttribIPointer(index, size, type, stride, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<int> param) => GL.EXTGPUShader4.GetVertexAttrib(index, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetVertexAttrib(uint index, GLGetVertexAttrib pname, Span<uint> param) => GL.EXTGPUShader4.GetVertexAttrib(index, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocation(uint program, uint colorNumber, string name) => GL.EXTGPUShader4.BindFragDataLocation(program, colorNumber, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataLocation(uint program, string name) => GL.EXTGPUShader4.GetFragDataLocation(program, name);

		// NV_conditional_render

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginConditionalRender(uint id, GLQueryMode mode) => GL.NVConditionalRender.BeginConditionalRender(id, mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndConditionalRender() => GL.NVConditionalRender.EndConditionalRender();

		// ARB_map_buffer_range

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapBufferRange(GLBufferTarget target, nint offset, nint length, GLMapAccessFlags access) => GL.ARBMapBufferRange.MapBufferRange(target, offset, length, access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedBufferRange(GLBufferTarget target, nint offset, nint length) => GL.ARBMapBufferRange.FlushMappedBufferRange(target, offset, length);

		// ARB_color_buffer_float

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClampColor(GLClampColorTarget target, GLClampColorMode mode) => GL.ARBColorBufferFloat.ClampColor(target, mode);

		// ARB_framebuffer_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsRenderbuffer(uint renderbuffer) => GL.ARBFramebufferObject.IsRenderbuffer(renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) => GL.ARBFramebufferObject.BindRenderbuffer(target, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(in ReadOnlySpan<uint> renderbuffers) => GL.ARBFramebufferObject.DeleteRenderbuffers(renderbuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(params uint[] renderbuffers) => GL.ARBFramebufferObject.DeleteRenderbuffers(renderbuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(uint renderbuffer) => GL.ARBFramebufferObject.DeleteRenderbuffers(renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenRenderbuffers(Span<uint> renderbuffers) => GL.ARBFramebufferObject.GenRenderbuffers(renderbuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenRenderbuffers(int n) => GL.ARBFramebufferObject.GenRenderbuffers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenRenderbuffers() => GL.ARBFramebufferObject.GenRenderbuffers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorage(GLRenderbufferTarget target, GLInternalFormat internalFormat, int width, int height) => GL.ARBFramebufferObject.RenderbufferStorage(target, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorageMultisample(GLRenderbufferTarget target, int samples, GLInternalFormat internalFormat, int width, int height) => GL.ARBFramebufferObject.RenderbufferStorageMultisample(target, samples, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetRenderbufferParameter(GLRenderbufferTarget target, GLGetRenderbuffer pname) => GL.ARBFramebufferObject.GetRenderbufferParameter(target, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsFramebuffer(uint framebuffer) => GL.ARBFramebufferObject.IsFramebuffer(framebuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) => GL.ARBFramebufferObject.BindFramebuffer(target, framebuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(in ReadOnlySpan<uint> framebuffers) => GL.ARBFramebufferObject.DeleteFramebuffers(framebuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(params uint[] framebuffers) => GL.ARBFramebufferObject.DeleteFramebuffers(framebuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(uint framebuffer) => GL.ARBFramebufferObject.DeleteFramebuffers(framebuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenFramebuffers(Span<uint> framebuffers) => GL.ARBFramebufferObject.GenFramebuffers(framebuffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenFramebuffers(int n) => GL.ARBFramebufferObject.GenFramebuffers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenFramebuffers() => GL.ARBFramebufferObject.GenFramebuffers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckFramebufferStatus(GLFramebufferTarget target) => GL.ARBFramebufferObject.CheckFramebufferStatus(target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFramebufferAttachment GetColorAttachment(int attachment) => ARBFramebufferObject.GetColorAttachment(attachment);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture1D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) => GL.ARBFramebufferObject.FramebufferTexture1D(target, attachment, textarget, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture2D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) => GL.ARBFramebufferObject.FramebufferTexture2D(target, attachment, textarget, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture3D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level, int layer) => GL.ARBFramebufferObject.FramebufferTexture3D(target, attachment, textarget, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureLayer(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, int layer) => GL.ARBFramebufferObject.FramebufferTextureLayer(target, attachment, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferRenderbuffer(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) => GL.ARBFramebufferObject.FramebufferRenderbuffer(target, attachment, renderbufferTarget, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferAttachmentParameter(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname) => GL.ARBFramebufferObject.GetFramebufferAttachmentParameter(target, attachment, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitFramebuffer(Recti src, Recti dst, GLBufferMask mask, GLFilter filter) => GL.ARBFramebufferObject.BlitFramebuffer(src, dst, mask, filter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateMipmap(GLTextureTarget target) => GL.ARBFramebufferObject.GenerateMipmap(target);

		// EXT_texture_integer

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(int r, int g, int b, int a) => GL.EXTTextureInteger.ClearColor(r, g, b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearColor(uint r, uint g, uint b, uint a) => GL.EXTTextureInteger.ClearColor(r, g, b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<int> param) => GL.EXTTextureInteger.GetTexParameter(target, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTexParameter(GLTextureTarget target, GLTexParamter pname, Span<uint> param) => GL.EXTTextureInteger.GetTexParameter(target, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<int> param) => GL.EXTTextureInteger.TexParameter(target, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexParameter(GLTextureTarget target, GLTexParamter pname, in ReadOnlySpan<uint> param) => GL.EXTTextureInteger.TexParameter(target, pname, param);

		// EXT_draw_buffers2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ColorMaskI(uint buf, bool red, bool green, bool blue, bool alpha) => GL.EXTDrawBuffers2.ColorMaskIndexed(buf, red, green, blue, alpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBoolean(GLIndexedCapability pname, uint index) => GL.EXTDrawBuffers2.GetBoolean(pname, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Disable(GLIndexedCapability cap, uint index) => GL.EXTDrawBuffers2.Disable(cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Enable(GLIndexedCapability cap, uint index) => GL.EXTDrawBuffers2.Enable(cap, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEnabled(GLIndexedCapability cap, uint index) => GL.EXTDrawBuffers2.IsEnabled(cap, index);

		// ARB_transform_feedback

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) => GL.ARBTransformFeedback.BindBufferRange(target, index, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferOffset(GLBufferRangeTarget target, uint index, uint buffer, nint offset) => GL.ARBTransformFeedback.BindBufferOffset(target, index, buffer, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) => GL.ARBTransformFeedback.BindBufferBase(target, index, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginTransformFeedback(GLDrawMode mode) => GL.ARBTransformFeedback.BeginTransformFeedback(mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndTransformFeedback() => GL.ARBTransformFeedback.EndTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTransformFeedbackVarying(uint program, uint index, out int size, out GLShaderAttribType type, out string name) => GL.ARBTransformFeedback.GetTransformFeedbackVarying(program, index, out size, out type, out name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackVaryings(uint program, in ReadOnlySpan<string> varyings, GLTransformFeedbackBufferMode bufferMode) => GL.ARBTransformFeedback.TransformFeedbackVaryings(program, varyings, bufferMode);

		// OpenGL 3.0

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => FunctionsGL30.glClearBufferfi((uint)buffer, drawbuffer, depth, stencil);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<int> value) {
			unsafe {
				fixed(int* pValue = value) {
					FunctionsGL30.glClearBufferiv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<float> value) {
			unsafe {
				fixed (float* pValue = value) {
					FunctionsGL30.glClearBufferfv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBuffer(GLClearBuffer buffer, int drawbuffer, in Span<uint> value) {
			unsafe {
				fixed (uint* pValue = value) {
					FunctionsGL30.glClearBufferuiv((uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetString(uint pname, uint index) => MemoryUtil.GetStringASCII(FunctionsGL30.glGetStringi(pname, index));

		// ARB_vertex_array_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexArray(uint array) => GL.ARBVertexArrayObject.BindVertexArray(array);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(in ReadOnlySpan<uint> arrays) => GL.ARBVertexArrayObject.DeleteVertexArrays(arrays);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(params uint[] arrays) => GL.ARBVertexArrayObject.DeleteVertexArrays(arrays);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteVertexArrays(uint array) => GL.ARBVertexArrayObject.DeleteVertexArrays(array);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenVertexArrays(Span<uint> arrays) => GL.ARBVertexArrayObject.GenVertexArrays(arrays);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenVertexArrays(int n) => GL.ARBVertexArrayObject.GenVertexArrays(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenVertexArrays() => GL.ARBVertexArrayObject.GenVertexArrays();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsVertexArray(uint array) => GL.ARBVertexArrayObject.IsVertexArray(array);

	}

}
