using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenGL {

	public class GL44 : GL43 {

		public GL44(GL gl, IGLContext context) : base(gl, context) { }

		// ARB_buffer_storage

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage(GLBufferTarget target, nint size, IntPtr data, GLBufferStorageFlags flags) => GL.ARBBufferStorage!.BufferStorage(target, size, data, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage(GLBufferTarget target, nint size, GLBufferStorageFlags flags) => GL.ARBBufferStorage!.BufferStorage(target, size, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage<T>(GLBufferTarget target, in ReadOnlySpan<T> data, GLBufferStorageFlags flags) where T : unmanaged => GL.ARBBufferStorage!.BufferStorage(target, data, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BufferStorage<T>(GLBufferTarget target, GLBufferStorageFlags flags, params T[] data) where T : unmanaged => GL.ARBBufferStorage!.BufferStorage(target, flags, data);

		// ARB_clear_texture

		public void ClearTexImage(uint texture, int level, GLFormat format, GLTextureType type, IntPtr data) => GL.ARBClearTexture!.ClearTexImage(texture, level, format, type, data);

		public void ClearTexImage<T>(uint texture, int level, GLFormat format, GLTextureType type, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBClearTexture!.ClearTexImage(texture, level, format, type, data);

		public void ClearTexSubImage(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, IntPtr data) => GL.ARBClearTexture!.ClearTexSubImage(texture, level, offset, size, format, type, data);

		public void ClearTexSubImage<T>(uint texture, int level, Vector3i offset, Vector3i size, GLFormat format, GLTextureType type, in ReadOnlySpan<T> data) where T : unmanaged => GL.ARBClearTexture!.ClearTexSubImage(texture, level, offset, size, format, type, data);

		// ARB_enhanced_layouts

		// ARB_multi_bind

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersBase(GLBufferRangeTarget target, uint first, in ReadOnlySpan<uint> buffers) => GL.ARBMultiBind!.BindBuffersBase(target, first, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersBase(GLBufferRangeTarget target, uint first, params uint[] buffers) => GL.ARBMultiBind!.BindBuffersBase(target, first, buffers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBuffersRange(GLBufferRangeTarget target, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<nint> sizes) => GL.ARBMultiBind!.BindBuffersRange(target, first, buffers, offsets, sizes);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, in ReadOnlySpan<uint> textures) => GL.ARBMultiBind!.BindImageTextures(first, textures);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTextures(uint first, params uint[] textures) => GL.ARBMultiBind!.BindImageTextures(first, textures);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, in ReadOnlySpan<uint> samplers) => GL.ARBMultiBind!.BindSamplers(first, samplers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSamplers(uint first, params uint[] samplers) => GL.ARBMultiBind!.BindSamplers(first, samplers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, in ReadOnlySpan<uint> textures) => GL.ARBMultiBind!.BindTextures(first, textures);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextures(uint first, params uint[] textures) => GL.ARBMultiBind!.BindTextures(first, textures);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffers(uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<int> strides) => GL.ARBMultiBind!.BindVertexBuffers(first, buffers, offsets, strides);

		// ARB_query_buffer_object

		// ARB_texture_mirror_clamp_to_edge

		// ARB_texture_stencil8

		//  ARB_vertex_type_10f_11f_11f_rev

	}

}
