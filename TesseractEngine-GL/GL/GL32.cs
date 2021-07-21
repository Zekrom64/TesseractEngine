using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.GL {

	public class GL32 : GL31 {

		public GL32(GL gl, IGLContext context) : base(gl, context) { }

		// ARB_compatibility

		// ARB_vertex_array_bgra

		// ARB_draw_elements_base_vertex

		public void DrawElementsBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawElementsBaseVertex(mode, count, type, offset, basevertex);

		public void DrawRangeElementsBaseVertex(GLDrawMode mode, uint start, uint end, int count, GLIndexType type, nint offset, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawRangeElementsBaseVertex(mode, start, end, count, type, offset, basevertex);

		public void DrawElementsInstancedBaseVertex(GLDrawMode mode, int count, GLIndexType type, nint offset, int instancecount, int basevertex) => GL.ARBDrawElementsBaseVertex.DrawElementsInstancedBaseVertex(mode, count, type, offset, instancecount, basevertex);

		public void MultiDrawElementsBaseVertex(GLDrawMode mode, in ReadOnlySpan<int> count, GLIndexType type, in ReadOnlySpan<nint> offsets, int drawCount, in ReadOnlySpan<int> baseVertex) => GL.ARBDrawElementsBaseVertex.MultiDrawElementsBaseVertex(mode, count, type, offsets, drawCount, baseVertex);

		// ARB_fragment_coord_conventions

		// ARB_provoking_vertex

		public void ProvokingVertex(GLProvokingVertexConvention mode) => GL.ARBProvokingVertex.ProvokingVertex(mode);

		// ARB_seamless_cube_map

		// ARB_texture_multisample

		public void TexImage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => GL.ARBTextureMultisample.TexImage2DMultisample(target, samples, internalFormat, width, height, fixedSampleLocations);

		public void TexImage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => GL.ARBTextureMultisample.TexImage3DMultisample(target, samples, internalFormat, width, height, depth, fixedSampleLocations);

		public Span<float> GetMultisample(GLGetMutlisample pname, uint index, Span<float> values) => GL.ARBTextureMultisample.GetMultisample(pname, index, values);

		public void SampleMask(uint maskNumber, uint mask) => GL.ARBTextureMultisample.SampleMask(maskNumber, mask);

		// ARB_depth_clamp

		// ARB_sync

		public nuint FenceSync(GLSyncCondition condition, uint flags = 0) => GL.ARBSync.FenceSync(condition, flags);

		public bool IsSync(nuint sync) => GL.ARBSync.IsSync(sync);

		public void DeleteSync(nuint sync) => GL.ARBSync.DeleteSync(sync);

		public void ClientWaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => GL.ARBSync.ClientWaitSync(sync, flags, timeout);

		public void WaitSync(nuint sync, GLSyncFlags flags, ulong timeout) => GL.ARBSync.WaitSync(sync, flags, timeout);

		public ulong GetInteger64(uint pname, uint index) => GL.ARBSync.GetInteger64(pname, index);

		public int GetSync(nuint sync, GLGetSync pname) => GL.ARBSync.GetSync(sync, pname);

	}

}
