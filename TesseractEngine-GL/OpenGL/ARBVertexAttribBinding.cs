using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBVertexAttribBindingFunctions {

		[ExternFunction(AltNames = new string[] { "glBindVertexBufferARB" })]
		[NativeType("void glBindVertexBuffer(GLuint bindingIndex, GLuint buffer, GLintptr offset, GLsizei stride)")]
		public delegate* unmanaged<uint, uint, nint, int, void> glBindVertexBuffer;
		[ExternFunction(AltNames = new string[] { "glVertexAttribFormat" })]
		[NativeType("void glVertexAttribFormat(GLuint attribIndex, GLint size, GLenum type, GLboolean normalized, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, int, uint, byte, uint, void> glVertexAttribFormat;
		[ExternFunction(AltNames = new string[] { "glVertexAttribIFormat" })]
		[NativeType("void glVertexAttribIFormat(GLuint attribIndex, GLint size, GLenum type, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, int, uint, uint, void> glVertexAttribIFormat;
		[ExternFunction(AltNames = new string[] { "glVertexAttribLFormat" })]
		[NativeType("void glVertexAttribLFormat(GLuint attribIndex, GLint size, GLenum type, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, int, uint, uint, void> glVertexAttribLFormat;
		[ExternFunction(AltNames = new string[] { "glVertexAttribBindingARB" })]
		[NativeType("void glVertexAttribBinding(GLuint attribIndex, GLuint bindingIndex)")]
		public delegate* unmanaged<uint, uint, void> glVertexAttribBinding;
		[ExternFunction(AltNames = new string[] { "glVertexBindingDivisorARB" })]
		[NativeType("void glVertexBindingDivisor(GLuint bindingIndex, GLuint divisor)")]
		public delegate* unmanaged<uint, uint, void> glVertexBindingDivisor;

	}

	public class ARBVertexAttribBinding : IGLObject {

		public GL GL { get; }
		public ARBVertexAttribBindingFunctions Functions { get; } = new();

		public ARBVertexAttribBinding(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffer(uint bindingIndex, uint buffer, nint offset, int stride) {
			unsafe {
				Functions.glBindVertexBuffer(bindingIndex, buffer, offset, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribFormat(uint attribIndex, int size, GLTextureType type, bool normalized, uint relativeOffset) {
			unsafe {
				Functions.glVertexAttribFormat(attribIndex, size, (uint)type, (byte)(normalized ? 1 : 0), relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIFormat(uint attribIndex, int size, GLTextureType type, uint relativeOffset) {
			unsafe {
				Functions.glVertexAttribIFormat(attribIndex, size, (uint)type, relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLFormat(uint attribIndex, int size, GLTextureType type, uint relativeOffset) {
			unsafe {
				Functions.glVertexAttribLFormat(attribIndex, size, (uint)type, relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribBinding(uint attribIndex, uint bindingIndex) {
			unsafe {
				Functions.glVertexAttribBinding(attribIndex, bindingIndex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexBindingDivisor(uint bindingIndex, uint divisor) {
			unsafe {
				Functions.glVertexBindingDivisor(bindingIndex, divisor);
			}
		}
	}
}
