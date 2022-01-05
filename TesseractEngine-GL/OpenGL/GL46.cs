using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.OpenGL {

	public class GL46 : GL45 {

		public GL46(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_indirect_parameters

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirectCount(GLDrawMode mode, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) => GL.ARBIndirectParameters.MultiDrawArraysIndirectCount(mode, indirectOffset, drawCountOffset, maxDrawCount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirectCount(GLDrawMode mode, GLIndexType type, nint indirectOffset, nint drawCountOffset, int maxDrawCount, int stride) => GL.ARBIndirectParameters.MultiDrawElementsIndirectCount(mode, type, indirectOffset, drawCountOffset, maxDrawCount, stride);

		// ARB_pipeline_statistics_query

		// ARB_polygon_offset_clamp

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PolygonOffsetClamp(float factor, float units, float clamp) => GL.ARBPolygonOffsetClamp.PolygonOffsetClamp(factor, units, clamp);

		// KHR_no_error

		// ARB_shader_atomic_counter_ops

		// ARB_shader_draw_parameters

		// ARB_shader_group_vote

		// ARB_gl_spirv

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SpecializeShader(uint shader, string entryPoint, in ReadOnlySpan<uint> constantIndex, in ReadOnlySpan<uint> constantValue) => GL.ARBGLSPIRV.SpecializeShader(shader, entryPoint, constantIndex, constantValue);

		// ARB_spirv_extensions

		// ARB_texture_filter_anisotropic

		// ARB_transform_feedback_overflow_query
#nullable restore

	}

}
