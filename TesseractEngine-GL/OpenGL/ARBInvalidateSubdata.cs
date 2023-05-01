using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBInvalidateSubdataFunctions {

		[ExternFunction(AltNames = new string[] { "glInvalidateTexSubImageARB" })]
		[NativeType("void glInvalidateTexSubImage(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, void> glInvalidateTexSubImage;
		[ExternFunction(AltNames = new string[] { "glInvalidateTexImageARB" })]
		[NativeType("void glInvalidateTexImage(GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, int, void> glInvalidateTexImage;
		[ExternFunction(AltNames = new string[] { "glInvalidateBufferSubDataARB" })]
		[NativeType("void glInvalidateBufferSubData(GLuint texture, GLintptr offset, GLsizeiptr length)")]
		public delegate* unmanaged<uint, nint, nint, void> glInvalidateBufferSubData;
		[ExternFunction(AltNames = new string[] { "glInvalidateBufferDataARB" })]
		[NativeType("void glInvalidateBufferData(GLuint buffer)")]
		public delegate* unmanaged<uint, void> glInvalidateBufferData;
		[ExternFunction(AltNames = new string[] { "glInvalidateFramebufferARB" })]
		[NativeType("void glInvalidateFramebuffer(GLenum target, GLsizei numAttachments, const GLenum* pAttachments)")]
		public delegate* unmanaged<uint, int, uint*, void> glInvalidateFramebuffer;
		[ExternFunction(AltNames = new string[] { "glInvalidateSubFramebufferARB" })]
		[NativeType("void glInvalidateSubFramebuffer(GLenum target, GLsizei numAttachments, const GLenum* pAttachments, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint*, int, int, int, int, void> glInvalidateSubFramebuffer;

	}

	public class ARBInvalidateSubdata : IGLObject {

		public GL GL { get; }
		public ARBInvalidateSubdataFunctions Functions { get; } = new();

		public ARBInvalidateSubdata(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexSubImage(uint texture, int level, Vector3i offset, Vector3i size) {
			unsafe {
				Functions.glInvalidateTexSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexImage(uint texture, int level) {
			unsafe {
				Functions.glInvalidateTexImage(texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferSubData(uint buffer, nint offset, nint length) {
			unsafe {
				Functions.glInvalidateBufferSubData(buffer, offset, length);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferData(uint buffer) {
			unsafe {
				Functions.glInvalidateBufferData(buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments) {
			unsafe {
				fixed(GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateFramebuffer((uint)target, attachments.Length, (uint*)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateFramebuffer(GLFramebufferTarget target, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateFramebuffer((uint)target, attachments.Length, (uint*)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateSubFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateSubFramebuffer((uint)target, attachments.Length, (uint*)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateSubFramebuffer(GLFramebufferTarget target, Recti area, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateSubFramebuffer((uint)target, attachments.Length, (uint*)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

	}
}
