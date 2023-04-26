using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.OpenGL.Native;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBFramebufferObjectFunctions {

		[ExternFunction(AltNames = new string[] { "glIsRenderbufferARB", "glIsRenderbufferEXT" })]
		[NativeType("GLboolean glIsRenderbuffer(GLuint renderbuffer)")]
		public delegate* unmanaged<uint, byte> glIsRenderbuffer;
		[ExternFunction(AltNames = new string[] { "glBindRenderbufferARB", "glBindRenderbufferEXT" })]
		[NativeType("void glBindRenderbuffer(GLenum target, GLuint renderbuffer)")]
		public delegate* unmanaged<uint, uint, void> glBindRenderbuffer;
		[ExternFunction(AltNames = new string[] { "glDeleteRenderbuffersARB", "glDeleteRenderbuffersEXT" })]
		[NativeType("void glDeleteRenderbuffers(GLsizei n, const GLuint* pRenderbuffers)")]
		public delegate* unmanaged<int, uint*, void> glDeleteRenderbuffers;
		[ExternFunction(AltNames = new string[] { "glGenRenderbuffersARB", "glGenRenderbuffersEXT" })]
		[NativeType("void glGenRenderbuffers(GLsizei n, GLuint* pRenderbuffers)")]
		public delegate* unmanaged<int, uint*, void> glGenRenderbuffers;
		[ExternFunction(AltNames = new string[] { "glRenderbufferStorageARB", "glRenderbufferStorageEXT" })]
		[NativeType("void glRenderbufferStorage(GLenum target, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, uint, int, int, void> glRenderbufferStorage;
		[ExternFunction(AltNames = new string[] { "glRenderbufferStorageMultisampleARB", "glRenderbufferStorageMultisampleEXT" })]
		[NativeType("void glRenderbufferStorageMultisample(GLenum target, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint, int, int, void> glRenderbufferStorageMultisample;
		[ExternFunction(AltNames = new string[] { "glGetRenderbufferParameterivARB", "glGetRenderbufferParameterivEXT" })]
		[NativeType("void glGetRenderbufferParameteriv(GLenum target, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetRenderbufferParameteriv;
		[ExternFunction(AltNames = new string[] { "glIsFramebufferARB", "glIsFramebufferEXT" })]
		[NativeType("GLboolean glIsFramebuffer(GLuint framebuffer)")]
		public delegate* unmanaged<uint, byte> glIsFramebuffer;
		[ExternFunction(AltNames = new string[] { "glBindFramebufferARB", "glBindFramebufferEXT" })]
		[NativeType("void glBindFramebuffer(GLenum target, GLuint framebuffer)")]
		public delegate* unmanaged<uint, uint, void> glBindFramebuffer;
		[ExternFunction(AltNames = new string[] { "glDeleteFramebuffersARB", "glDeleteFramebuffersEXT" })]
		[NativeType("void glDeleteFramebuffers(GLsizei n, const GLuint* pFramebuffers)")]
		public delegate* unmanaged<int, uint*, void> glDeleteFramebuffers;
		[ExternFunction(AltNames = new string[] { "glGenFramebuffersARB", "glGenFramebuffersEXT" })]
		[NativeType("void glGenFramebuffers(GLsizei n, GLuint* pFramebuffers)")]
		public delegate* unmanaged<int, uint*, void> glGenFramebuffers;
		[ExternFunction(AltNames = new string[] { "glCheckFramebufferStatusARB", "glCheckFramebufferStatusEXT" })]
		[NativeType("GLenum glCheckFramebufferStatus(GLenum target)")]
		public delegate* unmanaged<uint, uint> glCheckFramebufferStatus;
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture1DARB", "glFramebufferTexture1DEXT" })]
		[NativeType("void glFramebufferTexture1D(GLenum target, GLenum attachment, GLenum texTarget, GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, uint, uint, uint, int, void> glFramebufferTexture1D;
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture2DARB", "glFramebufferTexture2DEXT" })]
		[NativeType("void glFramebufferTexture2D(GLenum target, GLenum attachment, GLenum texTarget, GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, uint, uint, uint, int, void> glFramebufferTexture2D;
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture3DARB", "glFramebufferTexture3DEXT" })]
		[NativeType("void glFramebufferTexture3D(GLenum target, GLenum attachment, GLenum texTarget, GLuint texture, GLint level, GLint layer)")]
		public delegate* unmanaged<uint, uint, uint, uint, int, int, void> glFramebufferTexture3D;
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureLayerARB", "glFramebufferTextureLayerEXT" })]
		[NativeType("void glFramebufferTextureLayer(GLenum target, GLenum attachment, GLuint texture, GLint level, GLint layer)")]
		public delegate* unmanaged<uint, uint, uint, int, int, void> glFramebufferTextureLayer;
		[ExternFunction(AltNames = new string[] { "glFramebufferRenderbufferARB", "glFramebufferRenderbufferEXT" })]
		[NativeType("void glFramebufferRenderbuffer(GLenum target, GLenum attachment, GLenum renderbufferTarget, GLuint renderbuffer)")]
		public delegate* unmanaged<uint, uint, uint, uint, void> glFramebufferRenderbuffer;
		[ExternFunction(AltNames = new string[] { "glGetFramebufferAttachmentParameterivARB", "glGetFramebufferAttachmentParameterEXT" })]
		[NativeType("void glGetFramebufferAttachmentParameteriv(GLenum target, GLenum attachment, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetFramebufferAttachmentParameteriv;
		[ExternFunction(AltNames = new string[] { "glBlitFramebufferARB", "glBlitFramebufferEXT" })]
		[NativeType("void glBlitFramebuffer(GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter)")]
		public delegate* unmanaged<int, int, int, int, int, int, int, int, uint, uint, void> glBlitFramebuffer;
		[ExternFunction(AltNames = new string[] { "glGenerateMipmapARB", "glGenerateMipmapEXT" })]
		[NativeType("void glGenerateMipmap(GLenum target)")]
		public delegate* unmanaged<uint, void> glGenerateMipmap;

	}

	public class ARBFramebufferObject : IGLObject {

		public GL GL { get; }
		public ARBFramebufferObjectFunctions Functions { get; } = new();

		public ARBFramebufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsRenderbuffer(uint renderbuffer) {
			unsafe {
				return Functions.glIsRenderbuffer(renderbuffer) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) {
			unsafe {
				Functions.glBindRenderbuffer((uint)target, renderbuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(in ReadOnlySpan<uint> renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glDeleteRenderbuffers(renderbuffers.Length, pRenderbuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(params uint[] renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glDeleteRenderbuffers(renderbuffers.Length, pRenderbuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(uint renderbuffer) {
			unsafe {
				Functions.glDeleteRenderbuffers(1, &renderbuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenRenderbuffers(Span<uint> renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glGenRenderbuffers(renderbuffers.Length, pRenderbuffers);
				}
			}
			return renderbuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenRenderbuffers(int n) {
			uint[] renderbuffers = new uint[n];
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glGenRenderbuffers(n, pRenderbuffers);
				}
			}
			return renderbuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenRenderbuffers() {
			uint renderbuffer = 0;
			unsafe {
				Functions.glGenRenderbuffers(1, &renderbuffer);
			}
			return renderbuffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorage(GLRenderbufferTarget target, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glRenderbufferStorage((uint)target, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorageMultisample(GLRenderbufferTarget target, int samples, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glRenderbufferStorageMultisample((uint)target, samples, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetRenderbufferParameter(GLRenderbufferTarget target, GLGetRenderbuffer pname) {
			int i = 0;
			unsafe {
				Functions.glGetRenderbufferParameteriv((uint)target, (uint)pname, &i);
			}
			return i;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsFramebuffer(uint framebuffer) {
			unsafe {
				return Functions.glIsFramebuffer(framebuffer) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) {
			unsafe {
				Functions.glBindFramebuffer((uint)target, framebuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(in ReadOnlySpan<uint> framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glDeleteFramebuffers(framebuffers.Length, pFramebuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(params uint[] framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glDeleteFramebuffers(framebuffers.Length, pFramebuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(uint framebuffer) {
			unsafe {
				Functions.glDeleteFramebuffers(1, &framebuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenFramebuffers(Span<uint> framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glGenFramebuffers(1, pFramebuffers);
				}
			}
			return framebuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenFramebuffers(int n) {
			uint[] framebuffers = new uint[n];
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glGenFramebuffers(n, pFramebuffers);
				}
			}
			return framebuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenFramebuffers() {
			uint framebuffer = 0;
			unsafe {
				Functions.glGenFramebuffers(1, &framebuffer);
			}
			return framebuffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckFramebufferStatus(GLFramebufferTarget target) {
			unsafe {
				return (GLFramebufferStatus)Functions.glCheckFramebufferStatus((uint)target);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFramebufferAttachment GetColorAttachment(int attachment) => (GLFramebufferAttachment)(GLEnums.GL_COLOR_ATTACHMENT0 + attachment);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture1D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) {
			unsafe {
				Functions.glFramebufferTexture1D((uint)target, (uint)attachment, (uint)textarget, texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture2D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) {
			unsafe {
				Functions.glFramebufferTexture2D((uint)target, (uint)attachment, (uint)textarget, texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture3D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level, int layer) {
			unsafe {
				Functions.glFramebufferTexture3D((uint)target, (uint)attachment, (uint)textarget, texture, level, layer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureLayer(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, int layer) {
			unsafe {
				Functions.glFramebufferTextureLayer((uint)target, (uint)attachment, texture, level, layer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferRenderbuffer(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) {
			unsafe {
				Functions.glFramebufferRenderbuffer((uint)target, (uint)attachment, (uint)renderbufferTarget, renderbuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferAttachmentParameter(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname) {
			int param = 0;
			unsafe {
				Functions.glGetFramebufferAttachmentParameteriv((uint)target, (uint)attachment, (uint)pname, &param);
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitFramebuffer(Recti src, Recti dst, GLBufferMask mask, GLFilter filter) {
			Vector2i srcmin = src.Minimum, srcmax = src.Maximum;
			Vector2i dstmin = dst.Minimum, dstmax = dst.Maximum;
			unsafe {
				Functions.glBlitFramebuffer(srcmin.X, srcmin.Y, srcmax.X, srcmax.Y, dstmin.X, dstmin.Y, dstmax.X, dstmax.Y, (uint)mask, (uint)filter);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateMipmap(GLTextureTarget target) {
			unsafe {
				Functions.glGenerateMipmap((uint)target);
			}
		}
	}

}
