using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL {

	public class GL41 : GL40 {

		public GL41(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_ES2_compatibility

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ReleaseShaderCompiler() => GL.ARBES2Compatbility.ReleaseShaderCompiler();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderBinary(in ReadOnlySpan<uint> shaders, uint binaryFormat, in ReadOnlySpan<byte> binary) => GL.ARBES2Compatbility.ShaderBinary(shaders, binaryFormat, binary);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetShaderPrecisionFormat(GLShaderType shaderType, GLPrecisionType precisionType, Vector2i range) => GL.ARBES2Compatbility.GetShaderPrecisionFormat(shaderType, precisionType, range);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new void DepthRange(double near, double far) => base.DepthRange = new(near, far);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new void DepthRange(float near, float far) => GL.ARBES2Compatbility.DepthRange(near, far);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearDepth(float d) => GL.ARBES2Compatbility.ClearDepth(d);

		// ARB_get_program_binary

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<byte> GetProgramBinary(uint program, out uint binaryFormat, Span<byte> binary) => GL.ARBGetProgramBinary.GetProgramBinary(program, out binaryFormat, binary);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte[] GetProgramBinary(uint program, out uint binaryFormat) => GL.ARBGetProgramBinary.GetProgramBinary(program, out binaryFormat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramBinary(uint program, uint binaryFormat, in ReadOnlySpan<byte> binary) => GL.ARBGetProgramBinary.ProgramBinary(program, binaryFormat, binary);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParameter(uint program, GLProgramParameter pname, int value) => GL.ARBGetProgramBinary.ProgramParameter(program, pname, value);

		// ARB_separate_shader_objects

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UseProgramStages(uint pipeline, GLShaderStages stages, uint program) => GL.ARBSeparateShaderObjects.UseProgramStages(pipeline, stages, program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ActiveShaderProgram(uint pipeline, uint program) => GL.ARBSeparateShaderObjects.ActiveShaderProgram(pipeline, program);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateShaderProgram(GLShaderType type, string source) => GL.ARBSeparateShaderObjects.CreateShaderProgram(type, source);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindProgramPipeline(uint pipeline) => GL.ARBSeparateShaderObjects.BindProgramPipeline(pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(in ReadOnlySpan<uint> pipelines) => GL.ARBSeparateShaderObjects.DeleteProgramPipelines(pipelines);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(params uint[] pipelines) => GL.ARBSeparateShaderObjects.DeleteProgramPipelines(pipelines);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteProgramPipelines(uint pipeline) => GL.ARBSeparateShaderObjects.DeleteProgramPipelines(pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenProgramPipelines(Span<uint> pipelines) => GL.ARBSeparateShaderObjects.GenProgramPipelines(pipelines);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenProgramPipelines(int n) => GL.ARBSeparateShaderObjects.GenProgramPipelines(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenProgramPipelines() => GL.ARBSeparateShaderObjects.GenProgramPipelines();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsProgramPipeline(uint pipeline) => GL.ARBSeparateShaderObjects.IsProgramPipeline(pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramParameteri(uint program, GLProgramParameter pname, int value) => GL.ARBSeparateShaderObjects.ProgramParameteri(program, pname, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramPipelinei(uint program, GLGetProgramPipeline pname) => GL.ARBSeparateShaderObjects.GetProgramPipelinei(program, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ProgramUniform(uint program, int location, int x) => GL.ARBSeparateShaderObjects.ProgramUniform(program, location, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ValidateProgramPipeline(uint pipeline) => GL.ARBSeparateShaderObjects.ValidateProgramPipeline(pipeline);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetProgramPipelineInfoLog(uint pipeline) => GL.ARBSeparateShaderObjects.GetProgramPipelineInfoLog(pipeline);

		// ARB_shader_precision

		// ARB_vertex_attrib_64bit

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLPointer(uint index, int size, GLType type, int stride, nint offset) => GL.ARBVertexAttrib64Bit.VertexAttribLPointer(index, size, type, stride, offset);

		// ARB_viewport_array

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportArray(in ReadOnlySpan<Rectf> v, uint first = 0) => GL.ARBViewportArray.ViewportArray(v, first);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportArray(uint first, params Rectf[] v) => GL.ARBViewportArray.ViewportArray(first, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, float x, float y, float w, float h) => GL.ARBViewportArray.ViewportIndexed(index, x, y, w, h);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ViewportIndexed(uint index, Rectf v) => GL.ARBViewportArray.ViewportIndexed(index, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(in ReadOnlySpan<Recti> v, uint first = 0) => GL.ARBViewportArray.ScissorArray(v, first);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorArray(uint first, params Recti[] v) => GL.ARBViewportArray.ScissorArray(first, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, int left, int bottom, int width, int height) => GL.ARBViewportArray.ScissorIndexed(index, left, bottom, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ScissorIndexed(uint index, Recti v) => GL.ARBViewportArray.ScissorIndexed(index, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(in ReadOnlySpan<Vector2d> v, uint first = 0) => GL.ARBViewportArray.DepthRangeArray(v, first);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeArray(uint first, params Vector2d[] v) => GL.ARBViewportArray.DepthRangeArray(first, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DepthRangeIndexed(uint index, double n, double f) => GL.ARBViewportArray.DepthRangeIndexed(index, n, f);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetFloat(uint pname, uint index, Span<float> v) => GL.ARBViewportArray.GetFloat(pname, index, v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<double> GetDouble(uint pname, uint index, Span<double> v) => GL.ARBViewportArray.GetDouble(pname, index, v);
#nullable restore

	}

}
