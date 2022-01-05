using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.OpenGL {
	
	public class GL33 : GL32 {

		public GL33(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_shader_bit_encoding

		// ARB_blend_func_extended

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, string name) => GL.ARBBlendFuncExtended.BindFragDataLocationIndexed(program, colorNumber, index, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFragDataIndex(uint program, string name) => GL.ARBBlendFuncExtended.GetFragDataIndex(program, name);

		// ARB_explicit_attrib_location

		// ARB_occlusion_query2

		// ARB_sampler_objects

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenSamplers(Span<uint> samplers) => GL.ARBSamplerObjects.GenSamplers(samplers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenSamplers(int n) => GL.ARBSamplerObjects.GenSamplers(n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenSamplers() => GL.ARBSamplerObjects.GenSamplers();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(in ReadOnlySpan<uint> samplers) => GL.ARBSamplerObjects.DeleteSamplers(samplers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(params uint[] samplers) => GL.ARBSamplerObjects.DeleteSamplers(samplers);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteSamplers(uint sampler) => GL.ARBSamplerObjects.DeleteSamplers(sampler);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSampler(uint sampler) => GL.ARBSamplerObjects.IsSampler(sampler);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindSampler(uint unit, uint sampler) => GL.ARBSamplerObjects.BindSampler(unit, sampler);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, int param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, float param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params int[] param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<float> param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameter(uint sampler, GLSamplerParameter pname, params float[] param) => GL.ARBSamplerObjects.SamplerParameter(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<int> param) => GL.ARBSamplerObjects.SamplerParameterI(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params int[] param) => GL.ARBSamplerObjects.SamplerParameterI(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, in ReadOnlySpan<uint> param) => GL.ARBSamplerObjects.SamplerParameterI(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SamplerParameterI(uint sampler, GLSamplerParameter pname, params uint[] param) => GL.ARBSamplerObjects.SamplerParameterI(sampler, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetSamplerParameteri(uint sampler, GLSamplerParameter pname) => GL.ARBSamplerObjects.GetSamplerParameteri(sampler, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float GetSamplerParameterf(uint sampler, GLSamplerParameter pname) => GL.ARBSamplerObjects.GetSamplerParamterf(sampler, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<int> values) => GL.ARBSamplerObjects.GetSamplerParameter(sampler, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetSamplerParameter(uint sampler, GLSamplerParameter pname, Span<float> values) => GL.ARBSamplerObjects.GetSamplerParameter(sampler, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<int> values) => GL.ARBSamplerObjects.GetSamplerParameterI(sampler, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetSamplerParameterI(uint sampler, GLSamplerParameter pname, Span<uint> values) => GL.ARBSamplerObjects.GetSamplerParameterI(sampler, pname, values);

		// ARB_texture_rgb10_a2ui

		// ARB_texture_swizzle

		// ARB_timer_query

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void QueryCounter(uint id, GLQueryCounterTarget target) => GL.ARBTimerQuery.QueryCounter(id, target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public long GetQueryObjecti64(uint id, GLGetQueryObject pname) => GL.ARBTimerQuery.GetQueryObjecti64(id, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjecti64(uint id, GLGetQueryObject pname, nint offset) => GL.ARBTimerQuery.GetQueryObjecti64(id, pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong GetQueryObjectui64(uint id, GLGetQueryObject pname) => GL.ARBTimerQuery.GetQueryObjectui64(id, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryObjectui64(uint id, GLGetQueryObject pname, nint offset) => GL.ARBTimerQuery.GetQueryObjectui64(id, pname, offset);

		// ARB_instanced_arrays

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribDivisor(uint index, uint divisor) => GL.ARBInstancedArrays.VertexAttribDivisor(index, divisor);

		// ARB_vertex_type_2_10_10_10_rev
#nullable restore

	}

}
