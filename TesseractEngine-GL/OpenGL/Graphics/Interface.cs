using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLInterface : IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public GL33 GL33 => GL.GL33!;


		// Creators
		public Func<uint> CreateFramebuffer { get; }
		public Action<uint[]> CreateFramebuffers { get; }
		
		// Framebuffer
		public Action<uint, GLFramebufferAttachment, GLTextureView> SetFramebufferAttachment { get; }
		public Action<uint, Recti, GLFramebufferAttachment[]> InvalidateSubFramebuffer { get; }
		public Action<uint, GLClearBuffer, int, Vector4i> ClearFramebufferi { get; }
		public Action<uint, GLClearBuffer, int, Vector4ui> ClearFramebufferu { get; }
		public Action<uint, GLClearBuffer, int, Vector4> ClearFramebufferf { get; }
		public Action<uint, GLClearBuffer, int, float, int> ClearFramebufferfi { get; }

		// Texture
		public Action<uint, uint> InvalidateTextureImage { get; }

		// GL State
		public Action<uint, Recti> SetViewportLayered { get; }

		// Draw/Dispatch
		public Action<uint, uint, uint, uint> Draw;

		public GLInterface(GLGraphics graphics) {
			Graphics = graphics;

			var gl33 = GL33;

			var dsa = GL.ARBDirectStateAccess;
			if (dsa != null) {
				CreateFramebuffer = dsa.CreateFramebuffers;
				CreateFramebuffers = ids => dsa.CreateFramebuffers(ids);

				SetFramebufferAttachment = (uint fbo, GLFramebufferAttachment attach, GLTextureView view) => {
					if (view.GLObjectType == GLTextureObjectType.Renderbuffer) {
						dsa.NamedFramebufferRenderbuffer(fbo, attach, GLRenderbufferTarget.Renderbuffer, view.ID);
					} else {
						switch (view.Type) {
							case TextureType.Texture1D:
							case TextureType.Texture1DArray:
							case TextureType.Texture2D:
							case TextureType.Texture2DArray:
							case TextureType.Texture2DCube:
							case TextureType.Texture2DCubeArray:
								break;
							default:
								break;
						}
					}
				};
				ClearFramebufferi = dsa.ClearNamedFramebuffer;
				ClearFramebufferu = dsa.ClearNamedFramebuffer;
				ClearFramebufferf = dsa.ClearNamedFramebuffer;
				ClearFramebufferfi = dsa.ClearNamedFramebuffer;
			} else {
				CreateFramebuffer = gl33.GenFramebuffers;
				CreateFramebuffers = ids => gl33.GenFramebuffers(ids);

				SetFramebufferAttachment = (uint fbo, GLFramebufferAttachment attach, GLTextureView view) => {
					if (view.GLObjectType == GLTextureObjectType.Renderbuffer) {
						Graphics.State.BindRenderbuffer(GLRenderbufferTarget.Renderbuffer, view.ID);
						Graphics.State.BindFramebuffer(GLFramebufferTarget.Read, fbo);
						gl33.FramebufferRenderbuffer(GLFramebufferTarget.Read, attach, GLRenderbufferTarget.Renderbuffer, view.ID);
					} else {
						Graphics.State.BindFramebuffer(GLFramebufferTarget.Read, fbo);
						switch (view.Type) {
							case TextureType.Texture1D:
							case TextureType.Texture1DArray:
							case TextureType.Texture2D:
							case TextureType.Texture2DArray:
							case TextureType.Texture2DCube:
							case TextureType.Texture2DCubeArray:
								break;
							default:
								break;
						}
					}
				};
				ClearFramebufferi = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4i value) => {
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferu = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4ui value) => {
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferf = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4 value) => {
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferfi = (uint fbo, GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => {
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, depth, stencil);
				};
			}

			var va = GL.ARBViewportArray;
			if (va != null) {
				SetViewportLayered = (uint layers, Recti area) => {
					if (layers > 1) {
						for (uint i = 0; i < layers; i++)
							va.ViewportIndexed(i, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
					} else gl33.Viewport = area;
				};
			} else {
				SetViewportLayered = (uint layers, Recti area) => gl33.Viewport = area;
			}

			var isd = GL.ARBInvalidateSubdata;
			if (isd != null) {
				if (dsa != null) {
					InvalidateSubFramebuffer = dsa.InvalidateNamedFramebufferSubData;
				} else {
					InvalidateSubFramebuffer = (uint fbo, Recti area, GLFramebufferAttachment[] attachments) => {
						Graphics.State.BindFramebuffer(GLFramebufferTarget.Read, fbo);
						isd.InvalidateSubFramebuffer(GLFramebufferTarget.Read, area, attachments);
					};
				}
				InvalidateTextureImage = (uint tex, uint mipLevel) => isd.InvalidateTexImage(tex, (int)mipLevel);
			} else {
				// Invalidates can be ignored as no-ops if not available, as they are just hints that resources don't need to be preserved
				InvalidateSubFramebuffer = (uint fbo, Recti area, GLFramebufferAttachment[] attachments) => { };
				InvalidateTextureImage = (uint tex, uint mipLevel) => { };
			}

			var bi = GL.ARBBaseInstance;
			if (bi != null) {
				Draw = (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) => bi.DrawArraysInstancedBaseInstance(Graphics.State.DrawMode, (int)firstVertex, (int)vertexCount, (int)instanceCount, firstInstance);
			} else {
				Draw = (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) => {
					if (firstInstance == 0)
						gl33.DrawArraysInstanced(Graphics.State.DrawMode, (int)firstVertex, (int)vertexCount, (int)instanceCount);
					else
						throw new GLException("Cannot draw with base instance != 0 without GL_ARB_base_instance");
				};
			}
		}
	
	}
}
