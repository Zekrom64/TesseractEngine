using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {
	
	public unsafe class ARBTransformFeedbackFunctions {

		[ExternFunction(AltNames = new string[] { "glBindBufferRangeARB", "glBindBufferRangeEXT" })]
		[NativeType("void glBindBufferRange(GLenum target, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, uint, nint, nint, void> glBindBufferRange;
		[ExternFunction(AltNames = new string[] { "glBindBufferOffsetARB", "glBindBufferOffsetEXT", "glBindBufferOffsetNV" })]
		[NativeType("void glBindBufferOffset(GLenum target, GLuint buffer, GLintptr offset)")]
		public delegate* unmanaged<uint, uint, uint, nint, void> glBindBufferOffset;
		[ExternFunction(AltNames = new string[] { "glBindBufferBaseARB", "glBindBufferBaseEXT" })]
		[NativeType("void glBindBufferBase(GLenum target, GLuint index, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, uint, void> glBindBufferBase;
		[ExternFunction(AltNames = new string[] { "glBeginTransformFeedbackARB", "glBeginTransformFeedbackEXT" })]
		[NativeType("void glBeginTransformFeedback(GLenum primitiveMode)")]
		public delegate* unmanaged<uint, void> glBeginTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glEndTransformFeedbackARB", "glEndTransformFeedbackEXT" })]
		[NativeType("void glEndTransformFeedback()")]
		public delegate* unmanaged<void> glEndTransformFeedback;
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackVaryingsARB", "glTransformFeedbackVaryingsEXT" })]
		[NativeType("void glTransformFeedbackVaryings(GLuint program, GLsizei count, const char* const* pVaryings, GLenum bufferMode)")]
		public delegate* unmanaged<uint, int, byte**, uint, void> glTransformFeedbackVaryings;
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbackVaryingARB", "glGetTransformFeedbackVaryingEXT" })]
		[NativeType("void glGetTransformFeedbackVarying(GLuint program, GLuint index, GLsizei bufSize, GLsizei* pLength, GLsizei* pLength, GLenum* pType, char* pName)")]
		public delegate* unmanaged<uint, uint, int, out int, out int, out uint, byte*, void> glGetTransformFeedbackVarying;

	}

	public class ARBTransformFeedback : IGLObject {

		public GL GL { get; }
		public ARBTransformFeedbackFunctions Functions { get; } = new();

		public ARBTransformFeedback(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferRange(GLBufferRangeTarget target, uint index, uint buffer, nint offset, nint size) {
			unsafe {
				Functions.glBindBufferRange((uint)target, index, buffer, offset, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferOffset(GLBufferRangeTarget target, uint index, uint buffer, nint offset) {
			unsafe {
				Functions.glBindBufferOffset((uint)target, index, buffer, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindBufferBase(GLBufferRangeTarget target, uint index, uint buffer) {
			unsafe {
				Functions.glBindBufferBase((uint)target, index, buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BeginTransformFeedback(GLDrawMode mode) {
			unsafe {
				Functions.glBeginTransformFeedback((uint)mode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EndTransformFeedback() {
			unsafe {
				Functions.glEndTransformFeedback();
			}
		}

		public void TransformFeedbackVaryings(uint program, IReadOnlyCollection<string> varyings, GLTransformFeedbackBufferMode bufferMode) {
			using MemoryStack sp = MemoryStack.Push();
			unsafe {
				fixed (IntPtr* ppVaryings = MemoryUtil.StackallocUTF8Array(varyings, stackalloc IntPtr[varyings.Count], sp)) {
					Functions.glTransformFeedbackVaryings(program, varyings.Count, (byte**)ppVaryings, (uint)bufferMode);
				}
			}
		}

		public void GetTransformFeedbackVarying(uint program, uint index, out int size, out GLShaderAttribType type, out string name) {
			int maxLen = GL.GL20!.GetProgram(program, GLGetProgram.TransformFeedbackVaryingMaxLength);
			Span<byte> strName = stackalloc byte[maxLen];
			unsafe {
				fixed(byte* pName = strName) {
					Functions.glGetTransformFeedbackVarying(program, index, maxLen, out int length, out size, out uint utype, pName);
					type = (GLShaderAttribType)utype;
					name = Encoding.UTF8.GetString(strName[..length]);
				}
			}
		}

	}
}
