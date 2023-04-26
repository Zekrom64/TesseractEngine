using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class GL32Functions {

		[NativeType("void glFramebufferTexture(GLenum target, GLenum attachment, GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, uint, uint, int, void> glFramebufferTexture;
		[NativeType("void glGetBufferParameteri64v(GLenum target, GLuint value, GLint64* pData)")]
		public delegate* unmanaged<uint, uint, out long, void> glGetBufferParameteri64v;
		[NativeType("void glGetInteger64i_v(GLenum pname, GLuint index, GLint64* pData)")]
		public delegate* unmanaged<uint, uint, long*, void> glGetInteger64i_v;

	}

	public class GL32 : GL31 {

		public GL32Functions FunctionsGL32 { get; } = new();

		public GL32(GL gl, IGLContext context) : base(gl, context) {
			Library.LoadFunctions(context.GetGLProcAddress, FunctionsGL32);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level) {
			unsafe {
				FunctionsGL32.glFramebufferTexture((uint)target, (uint)attachment, texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long GetBufferParameteri64(GLBufferTarget target, GLGetBufferParameter pname) {
			unsafe {
				FunctionsGL32.glGetBufferParameteri64v((uint)target, (uint)pname, out long value);
				return value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long GetInteger64(uint pname, uint index = 0) {
			unsafe {
				long value = 0;
				FunctionsGL32.glGetInteger64i_v(pname, index, &value);
				return value;
			}
		}

		public Span<long> GetInteger(uint pname, in Span<long> values, uint index = 0) {
			unsafe {
				fixed (long* pValues = values) {
					FunctionsGL32.glGetInteger64i_v(pname, index, pValues);
				}
				return values;
			}
		}

#nullable disable
		// ARB_compatibility

		// ARB_vertex_array_bgra

		// ARB_draw_elements_base_vertex

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawElementsBaseVertex(mode, count, type, offset, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawRangeElementsBaseVertex(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, nint offset, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawRangeElementsBaseVertex(mode, start, end, count, type, offset, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstancedBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int instancecount, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawElementsInstancedBaseVertex(mode, count, type, offset, instancecount, basevertex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsBaseVertex(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<nint> offsets, int drawCount, in ReadOnlySpan<int> baseVertex) => GL.ARBDrawElementsBaseVertex.MultiDrawElementsBaseVertex(mode, count, type, offsets, drawCount, baseVertex);

		// ARB_fragment_coord_conventions

		// ARB_provoking_vertex

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProvokingVertex(GLProvokingVertexConvention mode) => GL.ARBProvokingVertex.ProvokingVertex(mode);

		// ARB_seamless_cube_map

		// ARB_texture_multisample

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => GL.ARBTextureMultisample.TexImage2DMultisample(target, samples, internalFormat, width, height, fixedSampleLocations);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexImage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => GL.ARBTextureMultisample.TexImage3DMultisample(target, samples, internalFormat, width, height, depth, fixedSampleLocations);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetMultisample(GLGetMutlisample pname, uint index, Span<float> values) => GL.ARBTextureMultisample.GetMultisample(pname, index, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SampleMask(uint maskNumber, uint mask) => GL.ARBTextureMultisample.SampleMask(maskNumber, mask);

		// ARB_depth_clamp

		// ARB_sync

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public nuint FenceSync(GLSyncCondition condition, uint flags = 0) => GL.ARBSync.FenceSync(condition, flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSync(nuint sync) => GL.ARBSync.IsSync(sync);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSync(nuint sync) => GL.ARBSync.DeleteSync(sync);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLWaitResult ClientWaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => GL.ARBSync.ClientWaitSync(sync, flags, timeout);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => GL.ARBSync.WaitSync(sync, flags, timeout);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSync(nuint sync, GLGetSync pname) => GL.ARBSync.GetSync(sync, pname);
#nullable restore

	}

}
