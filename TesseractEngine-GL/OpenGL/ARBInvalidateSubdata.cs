using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBInvalidateSubdataFunctions {

		public delegate void PFN_glInvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
		[ExternFunction(AltNames = new string[] { "glInvalidateTexSubImageARB" })]
		public PFN_glInvalidateTexSubImage glInvalidateTexSubImage;
		public delegate void PFN_glInvalidateTexImage(uint texture, int level);
		[ExternFunction(AltNames = new string[] { "glInvalidateTexImageARB" })]
		public PFN_glInvalidateTexImage glInvalidateTexImage;
		public delegate void PFN_glInvalidateBufferSubData(uint buffer, nint offset, nint length);
		[ExternFunction(AltNames = new string[] { "glInvalidateBufferSubDataARB" })]
		public PFN_glInvalidateBufferSubData glInvalidateBufferSubData;
		public delegate void PFN_glInvalidateBufferData(uint buffer);
		[ExternFunction(AltNames = new string[] { "glInvalidateBufferDataARB" })]
		public PFN_glInvalidateBufferData glInvalidateBufferData;
		public delegate void PFN_glInvalidateFramebuffer(uint target, int numAttachments, [NativeType("const GLenum*")] IntPtr attachments);
		[ExternFunction(AltNames = new string[] { "glInvalidateFramebufferARB" })]
		public PFN_glInvalidateFramebuffer glInvalidateFramebuffer;
		public delegate void PFN_glInvalidateSubFramebuffer(uint target, int numAttachments, [NativeType("const GLenum*")] IntPtr attachments, int x, int y, int width, int height);
		[ExternFunction(AltNames = new string[] { "glInvalidateSubFramebufferARB" })]
		public PFN_glInvalidateSubFramebuffer glInvalidateSubFramebuffer;

	}

	public class ARBInvalidateSubdata : IGLObject {

		public GL GL { get; }
		public ARBInvalidateSubdataFunctions Functions { get; } = new();

		public ARBInvalidateSubdata(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexSubImage(uint texture, int level, Vector3i offset, Vector3i size) => Functions.glInvalidateTexSubImage(texture, level, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexImage(uint texture, int level) => Functions.glInvalidateTexImage(texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferSubData(uint buffer, nint offset, nint length) => Functions.glInvalidateBufferSubData(buffer, offset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferData(uint buffer) => Functions.glInvalidateBufferData(buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments) {
			unsafe {
				fixed(GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateFramebuffer((uint)target, attachments.Length, (IntPtr)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateSubFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateSubFramebuffer((uint)target, attachments.Length, (IntPtr)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

	}
}
