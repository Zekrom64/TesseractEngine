using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Tesseract.OpenGL {
	
	public class GL31 : GL30 {

		public GL31(GL gl, IGLContext context) : base(gl, context) { }

#nullable disable
		// ARB_draw_instanced

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawArraysInstanced(GLDrawMode mode, int first, int count, int primcount) => GL.ARBDrawInstanced.DrawArraysInstanced(mode, first, count, primcount);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DrawElementsInstanced(GLDrawMode mode, int count, GLIndexType type, nint offset, int primcount) => GL.ARBDrawInstanced.DrawElementsInstanced(mode, count, type, offset, primcount);

		// ARB_copy_buffer

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyBufferSubData(GLBufferTarget readTarget, GLBufferTarget writeTarget, nint readOffset, nint writeOffset, nint size) => GL.ARBCopyBuffer.CopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);

		// NV_primitive_restart

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestart() => GL.NVPrimitiveRestart.PrimitiveRestart();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PrimitiveRestartIndex(uint index) => GL.NVPrimitiveRestart.PrimitiveRestartIndex(index);

		// ARB_texture_buffer_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBuffer(GLTextureTarget target, GLInternalFormat internalFormat, uint buffer) => GL.ARBTextureBufferObject.TexBuffer(target, internalFormat, buffer);

		// ARB_texture_rectangle

		// ARB_uniform_buffer_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GetUniformIndices(uint program, params string[] names) => GL.ARBUniformBufferObject.GetUniformIndices(program, names);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int[] GetActiveUniforms(uint program, in ReadOnlySpan<uint> uniformIndices, GLGetActiveUniform pname) => GL.ARBUniformBufferObject.GetActiveUniforms(program, uniformIndices, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int[] GetActiveUniforms(uint program, GLGetActiveUniform pname, params uint[] uniformIndices) => GL.ARBUniformBufferObject.GetActiveUniforms(program, pname, uniformIndices);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetActiveUniformName(uint program, uint uniformIndex) => GL.ARBUniformBufferObject.GetActiveUniformName(program, uniformIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetUniformBlockIndex(uint program, string uniformBlockName) => GL.ARBUniformBufferObject.GetUniformBlockIndex(program, uniformBlockName);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetActiveUniformBlock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname) => GL.ARBUniformBufferObject.GetActiveUniformBlock(program, uniformBlockIndex, pname);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetActiveUniformBLock(uint program, uint uniformBlockIndex, GLGetActiveUniformBlock pname, Span<int> vals) => GL.ARBUniformBufferObject.GetActiveUniformBlock(program, uniformBlockIndex, pname, vals);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetActiveUniformBlockName(uint program, uint uniformBlockIndex) => GL.ARBUniformBufferObject.GetActiveUniformBlockName(program, uniformBlockIndex);

		/* Already present in core OpenGL, but included separately in the ARB extension
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) => GL.ARBUniformBufferObject.BindBufferRange(target, index, buffer, offset, size);

		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) => GL.ARBUniformBufferObject.BindBufferBase(target, index, buffer);
		*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetInteger(uint pname, uint index, Span<int> val) => GL.ARBUniformBufferObject.GetInteger(pname, index, val);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInteger(uint pname, uint index) => GL.ARBUniformBufferObject.GetInteger(pname, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => GL.ARBUniformBufferObject.UniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
#nullable restore

	}

}
