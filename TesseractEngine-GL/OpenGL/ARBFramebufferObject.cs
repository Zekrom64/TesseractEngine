using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.OpenGL.Native;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public class ARBFramebufferObjectFunctions {

		public delegate byte PFN_glIsRenderbuffer(uint renderbuffer);
		[ExternFunction(AltNames = new string[] { "glIsRenderbufferARB", "glIsRenderbufferEXT" })]
		public PFN_glIsRenderbuffer glIsRenderbuffer;
		public delegate void PFN_glBindRenderbuffer(uint target, uint renderbuffer);
		[ExternFunction(AltNames = new string[] { "glBindRenderbufferARB", "glBindRenderbufferEXT" })]
		public PFN_glBindRenderbuffer glBindRenderbuffer;
		public delegate void PFN_glDeleteRenderbuffers(int n, [NativeType("const GLuint*")] IntPtr renderbuffers);
		[ExternFunction(AltNames = new string[] { "glDeleteRenderbuffersARB", "glDeleteRenderbuffersEXT" })]
		public PFN_glDeleteRenderbuffers glDeleteRenderbuffers;
		public delegate void PFN_glGenRenderbuffers(int n, [NativeType("GLuint*")] IntPtr renderbuffers);
		[ExternFunction(AltNames = new string[] { "glGenRenderbuffersARB", "glGenRenderbuffersEXT" })]
		public PFN_glGenRenderbuffers glGenRenderbuffers;
		public delegate void PFN_glRenderbufferStorage(uint target, uint internalformat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glRenderbufferStorageARB", "glRenderbufferStorageEXT" })]
		public PFN_glRenderbufferStorage glRenderbufferStorage;
		public delegate void PFN_glRenderbufferStorageMultisample(uint target, int samples, uint internalformat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glRenderbufferStorageMultisampleARB", "glRenderbufferStorageMultisampleEXT" })]
		public PFN_glRenderbufferStorageMultisample glRenderbufferStorageMultisample;
		public delegate void PFN_glGetRenderbufferParameteriv(uint target, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetRenderbufferParameterivARB", "glGetRenderbufferParameterivEXT" })]
		public PFN_glGetRenderbufferParameteriv glGetRenderbufferParameteriv;
		public delegate byte PFN_glIsFramebuffer(uint framebuffer);
		[ExternFunction(AltNames = new string[] { "glIsFramebufferARB", "glIsFramebufferEXT" })]
		public PFN_glIsFramebuffer glIsFramebuffer;
		public delegate void PFN_glBindFramebuffer(uint target, uint framebuffer);
		[ExternFunction(AltNames = new string[] { "glBindFramebufferARB", "glBindFramebufferEXT" })]
		public PFN_glBindFramebuffer glBindFramebuffer;
		public delegate void PFN_glDeleteFramebuffers(int n, [NativeType("const GLuint*")] IntPtr framebuffers);
		[ExternFunction(AltNames = new string[] { "glDeleteFramebuffersARB", "glDeleteFramebuffersEXT" })]
		public PFN_glDeleteFramebuffers glDeleteFramebuffers;
		public delegate void PFN_glGenFramebuffers(int n, [NativeType("GLuint*")] IntPtr framebuffers);
		[ExternFunction(AltNames = new string[] { "glGenFramebuffersARB", "glGenFramebuffersEXT" })]
		public PFN_glGenFramebuffers glGenFramebuffers;
		public delegate uint PFN_glCheckFramebufferStatus(uint target);
		[ExternFunction(AltNames = new string[] { "glCheckFramebufferStatusARB", "glCheckFramebufferStatusEXT" })]
		public PFN_glCheckFramebufferStatus glCheckFramebufferStatus;
		public delegate void PFN_glFramebufferTexture1D(uint target, uint attachment, uint textarget, uint texture, int level);
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture1DARB", "glFramebufferTexture1DEXT" })]
		public PFN_glFramebufferTexture1D glFramebufferTexture1D;
		public delegate void PFN_glFramebufferTexture2D(uint target, uint attachment, uint textarget, uint texture, int level);
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture2DARB", "glFramebufferTexture2DEXT" })]
		public PFN_glFramebufferTexture2D glFramebufferTexture2D;
		public delegate void PFN_glFramebufferTexture3D(uint target, uint attachment, uint textarget, uint texture, int level, int layer);
		[ExternFunction(AltNames = new string[] { "glFramebufferTexture3DARB", "glFramebufferTexture3DEXT" })]
		public PFN_glFramebufferTexture3D glFramebufferTexture3D;
		public delegate void PFN_glFramebufferTextureLayer(uint target, uint attachment, uint texture, int level, int layer);
		[ExternFunction(AltNames = new string[] { "glFramebufferTextureLayerARB", "glFramebufferTextureLayerEXT" })]
		public PFN_glFramebufferTextureLayer glFramebufferTextureLayer;
		public delegate void PFN_glFramebufferRenderbuffer(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
		[ExternFunction(AltNames = new string[] { "glFramebufferRenderbufferARB", "glFramebufferRenderbufferEXT" })]
		public PFN_glFramebufferRenderbuffer glFramebufferRenderbuffer;
		public delegate void PFN_glGetFramebufferAttachmentParameteriv(uint target, uint attachment, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetFramebufferAttachmentParameterivARB", "glGetFramebufferAttachmentParameterEXT" })]
		public PFN_glGetFramebufferAttachmentParameteriv glGetFramebufferAttachmentParameteriv;
		public delegate void PFN_glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
		[ExternFunction(AltNames = new string[] { "glBlitFramebufferARB", "glBlitFramebufferEXT" })]
		public PFN_glBlitFramebuffer glBlitFramebuffer;
		public delegate void PFN_glGenerateMipmap(uint target);
		[ExternFunction(AltNames = new string[] { "glGenerateMipmapARB", "glGenerateMipmapEXT" })]
		public PFN_glGenerateMipmap glGenerateMipmap;

	}

	public class ARBFramebufferObject : IGLObject {

		public GL GL { get; }
		public ARBFramebufferObjectFunctions Functions { get; } = new();

		public ARBFramebufferObject(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsRenderbuffer(uint renderbuffer) => Functions.glIsRenderbuffer(renderbuffer) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindRenderbuffer(GLRenderbufferTarget target, uint renderbuffer) => Functions.glBindRenderbuffer((uint)target, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(in ReadOnlySpan<uint> renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glDeleteRenderbuffers(renderbuffers.Length, (IntPtr)pRenderbuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(params uint[] renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glDeleteRenderbuffers(renderbuffers.Length, (IntPtr)pRenderbuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteRenderbuffers(uint renderbuffer) {
			unsafe {
				Functions.glDeleteRenderbuffers(1, (IntPtr)(&renderbuffer));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenRenderbuffers(Span<uint> renderbuffers) {
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glGenRenderbuffers(renderbuffers.Length, (IntPtr)pRenderbuffers);
				}
			}
			return renderbuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenRenderbuffers(int n) {
			uint[] renderbuffers = new uint[n];
			unsafe {
				fixed(uint* pRenderbuffers = renderbuffers) {
					Functions.glGenRenderbuffers(n, (IntPtr)pRenderbuffers);
				}
			}
			return renderbuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenRenderbuffers() {
			uint renderbuffer = 0;
			unsafe {
				Functions.glGenRenderbuffers(1, (IntPtr)(&renderbuffer));
			}
			return renderbuffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorage(GLRenderbufferTarget target, GLInternalFormat internalFormat, int width, int height) => Functions.glRenderbufferStorage((uint)target, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RenderbufferStorageMultisample(GLRenderbufferTarget target, int samples, GLInternalFormat internalFormat, int width, int height) => Functions.glRenderbufferStorageMultisample((uint)target, samples, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetRenderbufferParameter(GLRenderbufferTarget target, GLGetRenderbuffer pname) {
			int i = 0;
			unsafe {
				Functions.glGetRenderbufferParameteriv((uint)target, (uint)pname, (IntPtr)(&i));
			}
			return i;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsFramebuffer(uint framebuffer) => Functions.glIsFramebuffer(framebuffer) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindFramebuffer(GLFramebufferTarget target, uint framebuffer) => Functions.glBindFramebuffer((uint)target, framebuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(in ReadOnlySpan<uint> framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glDeleteFramebuffers(framebuffers.Length, (IntPtr)pFramebuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(params uint[] framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glDeleteFramebuffers(framebuffers.Length, (IntPtr)pFramebuffers);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DeleteFramebuffers(uint framebuffer) {
			unsafe {
				Functions.glDeleteFramebuffers(1, (IntPtr)(&framebuffer));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GenFramebuffers(Span<uint> framebuffers) {
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glGenFramebuffers(1, (IntPtr)pFramebuffers);
				}
			}
			return framebuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] GenFramebuffers(int n) {
			uint[] framebuffers = new uint[n];
			unsafe {
				fixed(uint* pFramebuffers = framebuffers) {
					Functions.glGenFramebuffers(n, (IntPtr)pFramebuffers);
				}
			}
			return framebuffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GenFramebuffers() {
			uint framebuffer = 0;
			unsafe {
				Functions.glGenFramebuffers(1, (IntPtr)(&framebuffer));
			}
			return framebuffer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckFramebufferStatus(GLFramebufferTarget target) => (GLFramebufferStatus)Functions.glCheckFramebufferStatus((uint)target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GLFramebufferAttachment GetColorAttachment(int attachment) => (GLFramebufferAttachment)(GLEnums.GL_COLOR_ATTACHMENT0 + attachment);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture1D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) => Functions.glFramebufferTexture1D((uint)target, (uint)attachment, (uint)textarget, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture2D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level) => Functions.glFramebufferTexture2D((uint)target, (uint)attachment, (uint)textarget, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTexture3D(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLTextureTarget textarget, uint texture, int level, int layer) => Functions.glFramebufferTexture3D((uint)target, (uint)attachment, (uint)textarget, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferTextureLayer(GLFramebufferTarget target, GLFramebufferAttachment attachment, uint texture, int level, int layer) => Functions.glFramebufferTextureLayer((uint)target, (uint)attachment, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferRenderbuffer(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) => Functions.glFramebufferRenderbuffer((uint)target, (uint)attachment, (uint)renderbufferTarget, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferAttachmentParameter(GLFramebufferTarget target, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname) {
			int param = 0;
			unsafe {
				Functions.glGetFramebufferAttachmentParameteriv((uint)target, (uint)attachment, (uint)pname, (IntPtr)(&param));
			}
			return param;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitFramebuffer(Recti src, Recti dst, GLBufferMask mask, GLFilter filter) => Functions.glBlitFramebuffer(src.X0, src.Y0, src.X1, src.Y1, dst.X0, dst.Y0, dst.X1, dst.Y1, (uint)mask, (uint)filter);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateMipmap(GLTextureTarget target) => Functions.glGenerateMipmap((uint)target);

	}

}
