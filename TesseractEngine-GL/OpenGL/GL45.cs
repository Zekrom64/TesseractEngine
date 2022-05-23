using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenGL {

	public class GL45 : GL44 {

		public GL45(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_clip_control

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClipControl(GLOrigin origin, GLClipDepth depth) => GL.ARBClipControl.ClipControl(origin, depth);

		// ARB_cull_distance

		// ARB_ES3_1_compatibility

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrierByRegion(GLMemoryBarrier barriers) => GL.ARBES31Compatibility.MemoryBarrierByRegion(barriers);

		// ARB_conditional_render_inverted

		// KHR_context_flush_control

		// ARB_derivative_control

		// ARB_direct_state_access

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateTransformFeedbacks(Span<uint> ids) => GL.ARBDirectStateAccess.CreateTransformFeedbacks(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTransformFeedbacks(int n) => GL.ARBDirectStateAccess.CreateTransformFeedbacks(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTransformFeedbacks() => GL.ARBDirectStateAccess.CreateTransformFeedbacks();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => GL.ARBDirectStateAccess.TransformFeedbackBufferBase(xfb, index, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, nint offset, nint size) => GL.ARBDirectStateAccess.TransformFeedbackBufferRange(xfb, index, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, Span<int> values) => GL.ARBDirectStateAccess.GetTransformFeedback(xfb, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<int> values) => GL.ARBDirectStateAccess.GetTransformFeedback(xfb, pname, index, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<long> values) => GL.ARBDirectStateAccess.GetTransformFeedback(xfb, pname, index, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateBuffers(Span<uint> buffers) => GL.ARBDirectStateAccess.CreateBuffers(buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateBuffers(int n) => GL.ARBDirectStateAccess.CreateBuffers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateBuffers() => GL.ARBDirectStateAccess.CreateBuffers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, IntPtr data, GLBufferStorageFlags flags) => GL.ARBDirectStateAccess.NamedBufferStorage(buffer, size, data, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, GLBufferStorageFlags flags) => GL.ARBDirectStateAccess.NamedBufferStorage(buffer, size, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage<T>(uint buffer, in ReadOnlySpan<T> data, GLBufferStorageFlags flags) where T : unmanaged => GL.ARBDirectStateAccess.NamedBufferStorage(buffer, data, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage<T>(uint buffer, GLBufferStorageFlags flags, params T[] data) where T : unmanaged => GL.ARBDirectStateAccess.NamedBufferStorage(buffer, flags, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData(uint buffer, nint size, IntPtr data, GLBufferUsage usage) => GL.ARBDirectStateAccess.NamedBufferData(buffer, size, data, usage);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData(uint buffer, nint size, GLBufferUsage usage) => GL.ARBDirectStateAccess.NamedBufferData(buffer, size, usage);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData<T>(uint buffer, in ReadOnlySpan<T> data, GLBufferUsage usage) where T : unmanaged => NamedBufferData(buffer, data, usage);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData<T>(uint buffer, GLBufferUsage usage, params T[] data) where T : unmanaged => NamedBufferData(buffer, usage, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) => GL.ARBDirectStateAccess.NamedBufferSubData(buffer, offset, size, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData<T>(uint buffer, nint offset, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.NamedBufferSubData(buffer, offset, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData<T>(uint buffer, nint offset, params T[] data) where T : unmanaged => GL.ARBDirectStateAccess.NamedBufferSubData(buffer, offset, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, nint readOffset, nint writeOffset, nint size) => GL.ARBDirectStateAccess.CopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedBufferData<T>(uint buffer, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.ClearNamedBufferData(buffer, internalFormat, format, type, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedBufferSubData<T>(uint buffer, nint offset, nint size, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.ClearNamedBufferSubData(buffer, offset, size, internalFormat, format, type, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapNamedBuffer(uint buffer, GLAccess access) => GL.ARBDirectStateAccess.MapNamedBuffer(buffer, access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapNamedBufferRange(uint buffer, nint offset, nint length, GLMapAccessFlags access) => GL.ARBDirectStateAccess.MapNamedBufferRange(buffer, offset, length, access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapNamedBuffer(uint buffer) => GL.ARBDirectStateAccess.UnmapNamedBuffer(buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedNamedBufferRange(uint buffer, nint offset, nint length) => GL.ARBDirectStateAccess.FlushMappedNamedBufferRange(buffer, offset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<int> values) => GL.ARBDirectStateAccess.GetNamedBufferParameter(buffer, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<long> values) => GL.ARBDirectStateAccess.GetNamedBufferParameter(buffer, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<IntPtr> GetNamedBufferPointer(uint buffer, GLGetBufferPointer pname, Span<IntPtr> values) => GL.ARBDirectStateAccess.GetNamedBufferPointer(buffer, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetNamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) => GL.ARBDirectStateAccess.GetNamedBufferSubData(buffer, offset, size, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetNamedBufferSubData<T>(uint buffer, nint offset, Span<T> data) where T : unmanaged => GL.ARBDirectStateAccess.GetNamedBufferSubData(buffer, offset, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateFramebuffers(Span<uint> ids) => GL.ARBDirectStateAccess.CreateFramebuffers(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateFramebuffers(int n) => GL.ARBDirectStateAccess.CreateFramebuffers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateFramebuffers() => GL.ARBDirectStateAccess.CreateFramebuffers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferRenderbuffer(uint framebuffer, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) => GL.ARBDirectStateAccess.NamedFramebufferRenderbuffer(framebuffer, attachment, renderbufferTarget, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, int param) => GL.ARBDirectStateAccess.NamedFramebufferParameter(framebuffer, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTexture(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level) => GL.ARBDirectStateAccess.NamedFramebufferTexture(framebuffer, attachment, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTextureLayer(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level, int layer) => GL.ARBDirectStateAccess.NamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffer(uint framebuffer, GLDrawBuffer mode) => GL.ARBDirectStateAccess.NamedFramebufferDrawBuffer(framebuffer, mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, in ReadOnlySpan<GLDrawBuffer> bufs) => GL.ARBDirectStateAccess.NamedFramebufferDrawBuffers(framebuffer, bufs);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, params GLDrawBuffer[] bufs) => GL.ARBDirectStateAccess.NamedFramebufferDrawBuffers(framebuffer, bufs);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferReadBuffer(uint framebuffer, GLDrawBuffer buf) => GL.ARBDirectStateAccess.NamedFramebufferReadBuffer(framebuffer, buf);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments) => GL.ARBDirectStateAccess.InvalidateNamedFramebufferData(framebuffer, attachments);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, params GLFramebufferAttachment[] attachments) => GL.ARBDirectStateAccess.InvalidateNamedFramebufferData(framebuffer, attachments);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) => GL.ARBDirectStateAccess.InvalidateNamedFramebufferSubData(framebuffer, attachments, area);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, Recti area, params GLFramebufferAttachment[] attachments) => GL.ARBDirectStateAccess.InvalidateNamedFramebufferSubData(framebuffer, area, attachments);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<int> value) => GL.ARBDirectStateAccess.ClearNamedFramebuffer(framebuffer, buffer, drawbuffer, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<uint> value) => GL.ARBDirectStateAccess.ClearNamedFramebuffer(framebuffer, buffer, drawbuffer, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<float> value) => GL.ARBDirectStateAccess.ClearNamedFramebuffer(framebuffer, buffer, drawbuffer, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => GL.ARBDirectStateAccess.ClearNamedFramebuffer(framebuffer, buffer, drawbuffer, depth, stencil);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, Recti src, Recti dst, GLBufferMask mask, GLFilter filter) => GL.ARBDirectStateAccess.BlitNamedFramebuffer(readFramebuffer, drawFramebuffer, src, dst, mask, filter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckNamedFramebufferStatus(uint framebuffer, GLFramebufferTarget target) => GL.ARBDirectStateAccess.CheckNamedFramebufferStatus(framebuffer, target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, Span<int> values) => GL.ARBDirectStateAccess.GetNamedFramebufferParameter(framebuffer, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferAttachmentParameter(uint framebuffer, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname, Span<int> values) => GL.ARBDirectStateAccess.GetNamedFramebufferAttachmentParameter(framebuffer, attachment, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateRenderbuffers(Span<uint> ids) => GL.ARBDirectStateAccess.CreateRenderbuffers(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateRenderbuffers(int n) => GL.ARBDirectStateAccess.CreateRenderbuffers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateRenderbuffers() => GL.ARBDirectStateAccess.CreateRenderbuffers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorage(uint renderbuffer, GLInternalFormat internalFormat, int width, int height) => GL.ARBDirectStateAccess.NamedRenderbufferStorage(renderbuffer, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, GLInternalFormat internalFormat, int width, int height) => GL.ARBDirectStateAccess.NamedRenderbufferStorageMultisample(renderbuffer, samples, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedRenderbufferParameter(uint renderbuffer, GLGetRenderbuffer pname, Span<int> values) => GL.ARBDirectStateAccess.GetNamedRenderbufferParameter(renderbuffer, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateTextures(GLTextureTarget target, Span<uint> ids) => GL.ARBDirectStateAccess.CreateTextures(target, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTextures(GLTextureTarget target, int n) => GL.ARBDirectStateAccess.CreateTextures(target, n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTextures(GLTextureTarget target) => GL.ARBDirectStateAccess.CreateTextures(target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBuffer(uint texture, GLInternalFormat internalFormat, uint buffer) => GL.ARBDirectStateAccess.TextureBuffer(texture, internalFormat, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBufferRange(uint texture, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) => GL.ARBDirectStateAccess.TextureBufferRange(texture, internalFormat, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage1D(uint texture, int levels, GLInternalFormat internalFormat, int width) => GL.ARBDirectStateAccess.TextureStorage1D(texture, levels, internalFormat, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height) => GL.ARBDirectStateAccess.TextureStorage2D(texture, levels, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height, int depth) => GL.ARBDirectStateAccess.TextureStorage3D(texture, levels, internalFormat, width, height, depth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => GL.ARBDirectStateAccess.TextureStorage2DMultisample(texture, samples, internalFormat, width, height, fixedSampleLocations);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => GL.ARBDirectStateAccess.TextureStorage3DMultisample(texture, samples, internalFormat, width, height, depth, fixedSampleLocations);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, IntPtr pixels) => GL.ARBDirectStateAccess.TextureSubImage1D(texture, level, xoffset, width, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged => GL.ARBDirectStateAccess.TextureSubImage1D(texture, level, xoffset, width, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, IntPtr pixels) => GL.ARBDirectStateAccess.TextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged => GL.ARBDirectStateAccess.TextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, IntPtr pixels) => GL.ARBDirectStateAccess.TextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged => GL.ARBDirectStateAccess.TextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, GLInternalFormat format, int imageSize, IntPtr data) => GL.ARBDirectStateAccess.CompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.CompressedTextureSubImage1D(texture, level, xoffset, width, format, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, int imageSize, IntPtr data) => GL.ARBDirectStateAccess.CompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.CompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, int imageSize, IntPtr data) => GL.ARBDirectStateAccess.CompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBDirectStateAccess.CompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => GL.ARBDirectStateAccess.CopyTextureSubImage1D(texture, level, xoffset, x, y, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => GL.ARBDirectStateAccess.CopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => GL.ARBDirectStateAccess.CopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, float param) => GL.ARBDirectStateAccess.TextureParameter(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<float> param) => GL.ARBDirectStateAccess.TextureParameter(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, int param) => GL.ARBDirectStateAccess.TextureParameter(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) => GL.ARBDirectStateAccess.TextureParameter(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) => GL.ARBDirectStateAccess.TextureParameterI(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<uint> param) => GL.ARBDirectStateAccess.TextureParameterI(texture, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateTextureMipmap(uint texture) => GL.ARBDirectStateAccess.GenerateTextureMipmap(texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextureUnit(uint unit, uint texture) => GL.ARBDirectStateAccess.BindTextureUnit(unit, texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureImage(uint texture, int level, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) => GL.ARBDirectStateAccess.GetTextureImage(texture, level, format, type, bufSize, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTextureImage<T>(uint texture, int level, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged => GL.ARBDirectStateAccess.GetTextureImage(texture, level, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels) => GL.ARBDirectStateAccess.GetCompressedTextureImage(texture, level, bufSize, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTextureImage<T>(uint texture, int level, Span<T> pixels) where T : unmanaged => GL.ARBDirectStateAccess.GetCompressedTextureImage(texture, level, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTextureLevelParameter(uint texture, int level, GLGetTexLevelParameter pname, Span<float> values) => GL.ARBDirectStateAccess.GetTextureLevelParameter(texture, level, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureLevelParameter(uint texture, int level, GLGetTexLevelParameter pname, Span<int> values) => GL.ARBDirectStateAccess.GetTextureLevelParameter(texture, level, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTextureParameter(uint texture, GLTexParamter pname, Span<float> values) => GL.ARBDirectStateAccess.GetTextureParameter(texture, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameter(uint texture, GLTexParamter pname, Span<int> values) => GL.ARBDirectStateAccess.GetTextureParameter(texture, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameterI(uint texture, GLTexParamter pname, Span<int> values) => GL.ARBDirectStateAccess.GetTextureParameterI(texture, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTextureParameterI(uint texture, GLTexParamter pname, Span<uint> values) => GL.ARBDirectStateAccess.GetTextureParameterI(texture, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(Span<uint> ids) => GL.ARBDirectStateAccess.CreateVertexArrays(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(int n) => GL.ARBDirectStateAccess.CreateVertexArrays(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateVertexArrays() => GL.ARBDirectStateAccess.CreateVertexArrays();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexArrayAttrib(uint vaobj, uint index) => GL.ARBDirectStateAccess.DisableVertexArrayAttrib(vaobj, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexArrayAttrib(uint vaobj, uint index) => GL.ARBDirectStateAccess.EnableVertexArrayAttrib(vaobj, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayElementBuffer(uint vaobj, uint buffer) => GL.ARBDirectStateAccess.VertexArrayElementBuffer(vaobj, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffer(uint vaobj, uint bindingIndex, uint buffer, nint offset, int stride) => GL.ARBDirectStateAccess.VertexArrayVertexBuffer(vaobj, bindingIndex, buffer, offset, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffers(uint vaobj, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<int> strides) => GL.ARBDirectStateAccess.VertexArrayVertexBuffers(vaobj, first, buffers, offsets, strides);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribFormat(uint vaobj, uint attribIndex, int size, GLType type, bool normalized, uint relativeOffset) => GL.ARBDirectStateAccess.VertexArrayAttribFormat(vaobj, attribIndex, size, type, normalized, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribIFormat(uint vaobj, uint attribIndex, int size, GLType type, uint relativeOffset) => GL.ARBDirectStateAccess.VertexArrayAttribIFormat(vaobj, attribIndex, size, type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribLFormat(uint vaobj, uint attribIndex, int size, GLType type, uint relativeOffset) => GL.ARBDirectStateAccess.VertexArrayAttribLFormat(vaobj, attribIndex, size, type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribBinding(uint vaobj, uint attribIndex, uint bindingIndex) => GL.ARBDirectStateAccess.VertexArrayAttribBinding(vaobj, attribIndex, bindingIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayBindingDivisor(uint vaobj, uint bindingIndex, uint divisor) => GL.ARBDirectStateAccess.VertexArrayBindingDivisor(vaobj, bindingIndex, divisor);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArray(uint vaobj, GLGetVertexArray pname, Span<int> values) => GL.ARBDirectStateAccess.GetVertexArray(vaobj, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<int> values) => GL.ARBDirectStateAccess.GetVertexArrayIndexed(vaobj, index, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<long> values) => GL.ARBDirectStateAccess.GetVertexArrayIndexed(vaobj, index, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateSamplers(Span<uint> ids) => GL.ARBDirectStateAccess.CreateSamplers(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateSamplers(int n) => GL.ARBDirectStateAccess.CreateSamplers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateSamplers() => GL.ARBDirectStateAccess.CreateSamplers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateProgramPipelines(Span<uint> ids) => GL.ARBDirectStateAccess.CreateProgramPipelines(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateProgramPipelines(int n) => GL.ARBDirectStateAccess.CreateProgramPipelines(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgramPipelines() => GL.ARBDirectStateAccess.CreateProgramPipelines();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateQueries(GLQueryTarget target, Span<uint> ids) => GL.ARBDirectStateAccess.CreateQueries(target, ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateQueries(GLQueryTarget target, int n) => GL.ARBDirectStateAccess.CreateQueries(target, n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateQueries(GLQueryTarget target) => GL.ARBDirectStateAccess.CreateQueries(target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) => GL.ARBDirectStateAccess.GetQueryBufferObjectiv(id, buffer, pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectuiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) => GL.ARBDirectStateAccess.GetQueryBufferObjectuiv(id, buffer, pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjecti64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) => GL.ARBDirectStateAccess.GetQueryBufferObjecti64v(id, buffer, pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectui64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) => GL.ARBDirectStateAccess.GetQueryBufferObjectui64v(id, buffer, pname, offset);

		// ARB_get_texture_sub_image

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged => GL.ARBGetTextureSubImage.GetTextureSubImage(texture, level, offset, size, format, type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) => GL.ARBGetTextureSubImage.GetTextureSubImage(texture, level, offset, size, format, type, bufSize, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTextureSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, Span<T> pixels) where T : unmanaged => GL.ARBGetTextureSubImage.GetCompressedTextureSubImage(texture, level, offset, size, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTextureSubImage(uint texture, int level, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels) => GL.ARBGetTextureSubImage.GetCompressedTextureSubImage(texture, level, offset, size, bufSize, pixels);

		// KHR_robustness

		public GLGraphicsResetStatus GraphicsResetStatus {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.KHRRobustness.GraphicsResetStatus;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> ReadnPixels<T>(Recti area, GLFormat format, GLType type, Span<T> data) where T : unmanaged => GL.KHRRobustness.ReadnPixels(area, format, type, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReadnPixels(Recti area, GLFormat format, GLType type, int bufSize, IntPtr data) => GL.KHRRobustness.ReadnPixels(area, format, type, bufSize, data);

		// ARB_shader_texture_image_samples

		// ARB_texture_barrier

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBarrier() => GL.ARBTextureBarrier.TextureBarrier();
#nullable restore

	}

}
