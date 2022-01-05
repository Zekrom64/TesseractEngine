using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.OpenGL {

	public class GL40 : GL33 {

		public GL40(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_texture_query_lod

		// ARB_draw_buffers_blend

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new void BlendEquation(GLBlendFunction mode) => base.BlendEquation = mode;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new void BlendEquation(uint buffer, GLBlendFunction mode) => GL.ARBDrawBuffersBlend.BlendEquation(buffer, mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendEquationSeparate(uint buffer, GLBlendFunction modeRGB, GLBlendFunction modeAlpha) => GL.ARBDrawBuffersBlend.BlendEquationSeparate(buffer, modeRGB, modeAlpha);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFunc(uint buffer, GLBlendFactor src, GLBlendFactor dst) => GL.ARBDrawBuffersBlend.BlendFunc(buffer, src, dst);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlendFuncSeparate(uint buffer, GLBlendFactor srcRGB, GLBlendFactor srcAlpha, GLBlendFactor dstRGB, GLBlendFactor dstAlpha) => GL.ARBDrawBuffersBlend.BlendFuncSeparate(buffer, srcRGB, srcAlpha, dstRGB, dstAlpha);

		// ARB_draw_indirect

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysIndirect(GLDrawMode mode, nint offset) => GL.ARBDrawIndirect.DrawArraysIndirect(mode, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset) => GL.ARBDrawIndirect.DrawElementsIndirect(mode, type, offset);

		// ARB_gpu_shader5

		// ARB_gpu_shader_fp64

		// ARB_sample_shading

		public float MinSampleShading {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GL.ARBSampleShading.MinSampleShading;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => GL.ARBSampleShading.MinSampleShading = value;
		}

		// ARB_shader_subroutine

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSubroutineUniformLocation(uint program, GLShaderType shaderType, string name) => GL.ARBShaderSubroutine.GetSubroutineUniformLocation(program, shaderType, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetSubroutineIndex(uint program, GLShaderType shaderType, string name) => GL.ARBShaderSubroutine.GetSubroutineIndex(program, shaderType, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname, Span<int> values) => GL.ARBShaderSubroutine.GetActiveSubroutineUniform(program, shaderType, index, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveSubroutineUniform(uint program, GLShaderType shaderType, uint index, GLGetActiveSubroutineUniform pname) => GL.ARBShaderSubroutine.GetActiveSubroutineUniform(program, shaderType, index, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetActiveSubroutineUniformName(uint program, GLShaderType shaderType, uint index) => GL.ARBShaderSubroutine.GetActiveSubroutineUniformName(program, shaderType, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetActiveSubroutineName(uint program, GLShaderType shaderType, uint index) => GL.ARBShaderSubroutine.GetActiveSubroutineName(program, shaderType, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, in ReadOnlySpan<uint> indices) => GL.ARBShaderSubroutine.UniformSubroutines(shaderType, indices);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformSubroutines(GLShaderType shaderType, params uint[] indices) => GL.ARBShaderSubroutine.UniformSubroutines(shaderType, indices);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformSubroutine(GLShaderType shaderType, int location) => GL.ARBShaderSubroutine.GetUniformSubroutine(shaderType, location);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramStage(uint program, GLShaderType shaderType, GLGetProgramStage pname) => GL.ARBShaderSubroutine.GetProgramStage(program, shaderType, pname);

		// ARB_tessellation_shader

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParameter(GLPatchParamteri pname, int value) => GL.ARBTessellationShader.PatchParamter(pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PatchParameter(GLPatchParamterfv pname, in ReadOnlySpan<float> values) => GL.ARBTessellationShader.PatchParamter(pname, values);

		// ARB_texture_buffer_object_rgb32

		// ARB_texture_cube_map_array

		// ARB_texture_gather

		// ARB_transform_feedback2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTransformFeedback(GLTransformFeedbackTarget target, uint id) => GL.ARBTransformFeedback2.BindTransformFeedback(target, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(in ReadOnlySpan<uint> ids) => GL.ARBTransformFeedback2.DeleteTransformFeedbacks(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(params uint[] ids) => GL.ARBTransformFeedback2.DeleteTransformFeedbacks(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteTransformFeedbacks(uint id) => GL.ARBTransformFeedback2.DeleteTransformFeedbacks(id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenTransformFeedbacks(Span<uint> ids) => GL.ARBTransformFeedback2.GenTransformFeedbacks(ids);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenTransformFeedbacks(int n) => GL.ARBTransformFeedback2.GenTransformFeedbacks(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenTransformFeedbacks() => GL.ARBTransformFeedback2.GenTransformFeedbacks();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTransformFeedback(uint id) => GL.ARBTransformFeedback2.IsTransformFeedback(id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PauseTransformFeedback() => GL.ARBTransformFeedback2.PauseTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ResumeTransformFeedback() => GL.ARBTransformFeedback2.ResumeTransformFeedback();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedback(GLDrawMode mode, uint id) => GL.ARBTransformFeedback2.DrawTransformFeedback(mode, id);

		// ARB_transform_feedback3

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawTransformFeedbackStream(GLDrawMode mode, uint id, uint stream) => GL.ARBTransformFeedback3.DrawTransformFeedbackStream(mode, id, stream);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginQueryIndexed(GLQueryTarget target, uint index, uint id) => GL.ARBTransformFeedback3.BeginQueryIndexed(target, index, id);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndQueryIndexed(GLQueryTarget target, uint index) => GL.ARBTransformFeedback3.EndQueryIndexed(target, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetQueryIndexed(GLQueryTarget target, uint index, GLGetQuery pname) => GL.ARBTransformFeedback3.GetQueryIndexed(target, index, pname);
#nullable restore

	}

}
