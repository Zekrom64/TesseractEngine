using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.GL {

	public class GL42 : GL41 {

		public GL42(GL gl, IGLContext context) : base(gl, context) { }

		// ARB_texture_compression_bptc

		// ARB_compressed_texture_pixel_storage

		// ARB_shader_atomic_counters

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveAtomicCounterBuffer(uint program, uint bufferIndex, GLGetActiveAtomicCounterBuffer pname, Span<int> values) => GL.ARBShaderAtomicCounters.GetActiveAtomicCounterBuffer(program, bufferIndex, pname, values);

		// ARB_texture_storage

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage1D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width) => GL.ARBTextureStorage.TexStorage1D(target, levels, internalFormat, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height) => GL.ARBTextureStorage.TexStorage2D(target, levels, internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3D(GLTextureTarget target, int levels, GLInternalFormat internalFormat, int width, int height, int depth) => GL.ARBTextureStorage.TexStorage3D(target, levels, internalFormat, width, height, depth);

		// ARB_transform_feedback_instanced

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackInstanced(GLDrawMode mode, uint id, int primCount) => GL.ARBTransformFeedbackInstanced.DrawTransformFeedbackInstanced(mode, id, primCount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStreamInstanced(GLDrawMode mode, uint id, uint stream, int primCount) => GL.ARBTransformFeedbackInstanced.DrawTransformFeedbackStreamInstanced(mode, id, stream, primCount);

		// ARB_base_instance

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstancedBaseInstance(GLDrawMode mode, int first, int count, int primCount, uint baseInstance) => GL.ARBBaseInstance.DrawArraysInstancedBaseInstance(mode, first, count, primCount, baseInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, uint baseInstance) => GL.ARBBaseInstance.DrawElementsInstancedBaseInstance(mode, count, type, offset, primCount, baseInstance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertexBaseInstance(GLDrawMode mode, int count, GLIndexType type, nint offset, int primCount, int baseVertex, uint baseInstance) => GL.ARBBaseInstance.DrawElementsInstancedBaseVertexBaseInstance(mode, count, type, offset, primCount, baseVertex, baseInstance);

		// ARB_shader_image_load_store

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, GLAccess access, GLInternalFormat format) => GL.ARBShaderImageLoadStore.BindImageTexture(unit, texture, level, layered, layer, access, format);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MemoryBarrier(GLMemoryBarrier barrier) => GL.ARBShaderImageLoadStore.MemoryBarrier(barrier);

		// ARB_conservative_depth

		// ARB_shading_language_420pack

		// ARB_internalformat_query

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetInternalFormat(GLInternalFormatTarget target, GLInternalFormat internalFormat, GLGetInternalFormat pname, Span<int> v) => GL.ARBInternalFormatQuery.GetInternalFormat(target, internalFormat, pname, v);

		// ARB_map_buffer_alignment

		// ARB_shading_language_packing

	}

}
