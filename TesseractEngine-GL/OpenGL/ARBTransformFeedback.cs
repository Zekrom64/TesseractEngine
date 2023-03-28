using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBTransformFeedbackFunctions {

		public delegate void PFN_glBindBufferRange(uint target, uint index, uint buffer, nint offset, nint size);
		[ExternFunction(AltNames = new string[] { "glBindBufferRangeARB", "glBindBufferRangeEXT" })]
		public PFN_glBindBufferRange glBindBufferRange;
		public delegate void PFN_glBindBufferOffset(uint target, uint index, uint buffer, nint offset);
		[ExternFunction(AltNames = new string[] { "glBindBufferOffsetARB", "glBindBufferOffsetEXT", "glBindBufferOffsetNV" })]
		public PFN_glBindBufferOffset glBindBufferOffset;
		public delegate void PFN_glBindBufferBase(uint target, uint index, uint buffer);
		[ExternFunction(AltNames = new string[] { "glBindBufferBaseARB", "glBindBufferBaseEXT" })]
		public PFN_glBindBufferBase glBindBufferBase;
		public delegate void PFN_glBeginTransformFeedback(uint primitiveMode);
		[ExternFunction(AltNames = new string[] { "glBeginTransformFeedbackARB", "glBeginTransformFeedbackEXT" })]
		public PFN_glBeginTransformFeedback glBeginTransformFeedback;
		public delegate void PFN_glEndTransformFeedback();
		[ExternFunction(AltNames = new string[] { "glEndTransformFeedbackARB", "glEndTransformFeedbackEXT" })]
		public PFN_glEndTransformFeedback glEndTransformFeedback;
		public delegate void PFN_glTransformFeedbackVaryings(uint program, int count, [NativeType("const char* const*")] IntPtr varyings, uint bufferMode);
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackVaryingsARB", "glTransformFeedbackVaryingsEXT" })]
		public PFN_glTransformFeedbackVaryings glTransformFeedbackVaryings;
		public delegate void PFN_glGetTransformFeedbackVarying(uint program, uint index, int bufSize, out int length, out int size, out uint type, [NativeType("char*")] IntPtr name);
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbackVaryingARB", "glGetTransformFeedbackVaryingEXT" })]
		public PFN_glGetTransformFeedbackVarying glGetTransformFeedbackVarying;

	}
#nullable restore

	public class ARBTransformFeedback : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedbackFunctions Functions { get; } = new();

		public ARBTransformFeedback(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) => Functions.glBindBufferRange((uint)target, index, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferOffset(GLBufferRangeTarget target, uint index, uint buffer, nint offset) => Functions.glBindBufferOffset((uint)target, index, buffer, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) => Functions.glBindBufferBase((uint)target, index, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginTransformFeedback(GLDrawMode mode) => Functions.glBeginTransformFeedback((uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndTransformFeedback() => Functions.glEndTransformFeedback();
		
		public void TransformFeedbackVaryings(uint program, in ReadOnlySpan<string> varyings, GLTransformFeedbackBufferMode bufferMode) {
			using MemoryStack sp = MemoryStack.Push();
			Functions.glTransformFeedbackVaryings(program, varyings.Length, sp.ASCIIArray(varyings), (uint)bufferMode);
		}

		public void GetTransformFeedbackVarying(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			using MemoryStack sp = MemoryStack.Push();
			int maxLen = GL.GL20!.GetProgram(program, GLGetProgram.TransformFeedbackVaryingMaxLength);
			IntPtr pName = sp.Alloc<byte>(maxLen);
			Functions.glGetTransformFeedbackVarying(program, index, maxLen, out int length, out size, out uint utype, pName);
			type = (GLShaderAttribType)utype;
			name = MemoryUtil.GetASCII(pName, length)!;
		}

	}
}
