using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL.Graphics {
	
	/// <summary>
	/// The GLInterface class maps more abstract functionality to specific code based on the available
	/// OpenGL version and extensions.
	/// </summary>
	public class GLInterface : IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public GL33 GL33 => GL.GL33!;

		// Buffer
		public Func<uint> CreateBuffer { get; }
		public delegate void OpBufferStorage(uint id, nint size, GLBufferStorageFlags flags);
		public OpBufferStorage BufferStorage { get; }
		public delegate void OpFlushBuffer(uint id, nint offset, nint length);
		public OpFlushBuffer FlushBufferGPUToHost { get; }
		public OpFlushBuffer FlushBufferHostToGPU { get; }
		public delegate IntPtr OpMapBuffer(uint id, GLMapAccessFlags flags, MemoryRange range);
		public OpMapBuffer MapBuffer { get; }
		public delegate void OpUnmapBuffer(uint id);
		public OpUnmapBuffer UnmapBuffer { get; }
		public delegate void OpBufferSubData(uint id, nint offset, nint length, IntPtr pData);
		public OpBufferSubData BufferSubData { get; }
		public delegate void OpCopyBufferSubData(uint srcId, uint dstId, nint srcOffset, nint dstOffset, nint length);
		public OpCopyBufferSubData CopyBufferSubData { get; }
		public delegate void OpFillBufferUInt32(uint id, nint offset, nint length, uint value);
		public OpFillBufferUInt32 FillBufferUInt32 { get; }

		// Texture
		public delegate uint OpCreateTexture(GLTextureTarget target);
		public OpCreateTexture CreateTexture { get; }
		public delegate void OpTextureStorage(GLTextureTarget target, uint id, Vector3i size, int mipLevels, int samples, GLPixelFormat format);
		public OpTextureStorage TextureStorage { get; }
		public delegate void OpCopyImageSubData(GLTexture dstTexture, int dstMipLevel, Vector3i dstPos, GLTexture srcTexture, int srcMipLevel, Vector3i srcPos, Vector3i size);
		public OpCopyImageSubData CopyImageSubData { get; }
		public delegate void OpInvalidateTextureImage(uint tex, uint mipLevel);
		public OpInvalidateTextureImage InvalidateTextureImage { get; }
		public delegate void OpBindTextureUnit(uint unit, GLTextureTarget target, uint id);
		public OpBindTextureUnit BindTextureUnit { get; }
		public delegate void OpGenerateMipmaps(GLTexture texture, GLFilter? filter);
		public OpGenerateMipmaps GenerateMipmaps { get; }
		public delegate void OpGetTextureSubImage(GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels);
		public OpGetTextureSubImage GetTextureSubImage { get; }
		public delegate void OpTextureSubImage(GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, IntPtr pixels, int layerStride);
		public OpTextureSubImage TextureSubImage { get; }

		// Framebuffer
		public Func<uint> CreateFramebuffer { get; }
		public Action<uint[]> CreateFramebuffers { get; }
		public delegate void OpInvalidateSubFramebuffer(uint id, Recti area, GLFramebufferAttachment[] attachments);
		public OpInvalidateSubFramebuffer InvalidateSubFramebuffer { get; }
		public delegate void OpClearFramebufferi(uint id, GLClearBuffer buffer, int drawbuffer, Vector4i value);
		public OpClearFramebufferi ClearFramebufferi { get; }
		public delegate void OpClearFramebufferu(uint id, GLClearBuffer buffer, int drawbuffer, Vector4ui value);
		public OpClearFramebufferu ClearFramebufferu { get; }
		public delegate void OpClearFramebufferf(uint id, GLClearBuffer buffer, int drawbuffer, Vector4 value);
		public OpClearFramebufferf ClearFramebufferf { get; }
		public delegate void OpClearFramebufferfi(uint id, GLClearBuffer buffer, int drawbuffer, float depth, int stencil);
		public OpClearFramebufferfi ClearFramebufferfi { get; }
		public delegate void OpFramebufferTexture(uint id, GLFramebufferAttachment attachment, IGLTexture texture, int mipLevel, int arrayLayer);
		public OpFramebufferTexture FramebufferTexture { get; }

		// Misc. Objects
		public Func<uint> CreateSampler { get; }
		public Func<uint> CreateRenderbuffer { get; }
		public delegate void OpRenderbufferStorage(uint id, int samples, GLInternalFormat format, int x, int y);
		public OpRenderbufferStorage RenderbufferStorage { get; }

		// GL State
		public delegate void OpSetAttachmentsBlendState(GLPipeline.ColorAttachmentState[] attachments);
		public OpSetAttachmentsBlendState SetAttachmentsBlendState { get; }
		public delegate void OpSetPatchControlPoints(uint patchControlPoints);
		public OpSetPatchControlPoints SetPatchControlPoints { get; }
		public delegate void OpSetViewports(uint firstViewport, in ReadOnlySpan<Viewport> viewports);
		public OpSetViewports SetViewports { get; }
		public delegate void OpSetScissors(uint firstScissor, in ReadOnlySpan<Recti> scissors);
		public OpSetScissors SetScissors { get; }
		public delegate void OpSetDepthBias(float constFactor, float slopeFactor, float clamp);
		public OpSetDepthBias SetDepthBias { get; }
		public delegate void OpSetDepthBoundsTestEnable(bool enable);
		public OpSetDepthBoundsTestEnable SetDepthBoundsTestEnable { get; }
		public delegate void OpSetDepthBounds(float min, float max);
		public OpSetDepthBounds SetDepthBounds { get; }

		// Draw/Dispatch
		public delegate void OpDraw(uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);
		public OpDraw Draw;
		public delegate void OpDrawIndexed(uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance);
		public OpDrawIndexed DrawIndexed;
		public delegate void OpDrawIndirect(uint bufferId, nint offset, int drawCount, int stride);
		public OpDrawIndirect DrawIndirect;
		public OpDrawIndirect DrawIndexedIndirect;
		public delegate void OpDispatch(Vector3ui groupCounts);
		public OpDispatch Dispatch;
		public delegate void OpDispatchIndirect(uint bufferId, nint offset);
		public OpDispatchIndirect DispatchIndirect;

		public GLInterface(GLGraphics graphics) {
			Graphics = graphics;
			var state = graphics.State;

			var gl33 = GL33;
			var dsa = GL.ARBDirectStateAccess;
			var bs = GL.ARBBufferStorage;
			var va = GL.ARBViewportArray;
			var isd = GL.ARBInvalidateSubdata;
			var bi = GL.ARBBaseInstance;
			var sils = GL.ARBShaderImageLoadStore;
			var dbb = GL.ARBDrawBuffersBlend;
			var tss = GL.ARBTessellationShader;
			var poc = GL.ARBPolygonOffsetClamp;
			var dbt = GL.EXTDepthBoundsTest;
			var di = GL.ARBDrawIndirect;
			var mdi = GL.ARBMultiDrawIndirect;
			var cs = GL.ARBComputeShader;
			var cbo = GL.ARBClearBufferObject;
			var ts = GL.ARBTextureStorage;
			var tsm = GL.ARBTextureStorageMultisample;
			var ci = GL.ARBCopyImage;
			var gtsi = GL.ARBGetTextureSubImage;

			if (dsa != null) {
				CreateBuffer = dsa.CreateBuffers;
				BufferStorage = dsa.NamedBufferStorage;
				FlushBufferHostToGPU = dsa.FlushMappedNamedBufferRange;
				MapBuffer = (uint id, GLMapAccessFlags flags, MemoryRange range) =>
					dsa.MapNamedBufferRange(id, (nint)range.Offset, (nint)range.Length, flags);
				UnmapBuffer = (uint id) =>
					dsa.UnmapNamedBuffer(id);
				BufferSubData = dsa.NamedBufferSubData;
				CopyBufferSubData = dsa.CopyNamedBufferSubData;
				FillBufferUInt32 = (uint id, nint offset, nint length, uint value) =>
					dsa.ClearNamedBufferSubData<uint>(id, offset, length, GLInternalFormat.R32I, GLFormat.R, GLType.UnsignedInt, stackalloc uint[] { value });

				CreateTexture = dsa.CreateTextures;
				TextureStorage = (GLTextureTarget target, uint id, Vector3i size, int mipLevels, int samples, GLPixelFormat format) => {
					switch (target) {
						case GLTextureTarget.Texture1D:
							dsa.TextureStorage1D(id, mipLevels, format.InternalFormat, size.X);
							break;
						case GLTextureTarget.Texture1DArray:
							dsa.TextureStorage2D(id, mipLevels, format.InternalFormat, size.X, size.Y);
							break;
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
						case GLTextureTarget.CubeMap:
							if (samples > 1) dsa.TextureStorage2DMultisample(id, samples, format.InternalFormat, size.X, size.Y, false);
							else dsa.TextureStorage2D(id, mipLevels, format.InternalFormat, size.X, size.Y);
							break;
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.CubeMapArray:
							if (samples > 1) dsa.TextureStorage3DMultisample(id, samples, format.InternalFormat, size.X, size.Y, size.Z, false);
							else dsa.TextureStorage3D(id, mipLevels, format.InternalFormat, size.X, size.Y, size.Z);
							break;
						case GLTextureTarget.Texture3D:
							if (samples > 1) dsa.TextureStorage3DMultisample(id, samples, format.InternalFormat, size.X, size.Y, size.Z, false);
							else dsa.TextureStorage3D(id, mipLevels, format.InternalFormat, size.X, size.Y, size.Z);
							break;
					}
				};
				BindTextureUnit = (uint unit, GLTextureTarget target, uint id) => dsa.BindTextureUnit(unit, id);
				GenerateMipmaps = (GLTexture texture, GLFilter? filter) => {
					if (filter == null) {
						dsa.GenerateTextureMipmap(texture.ID);
					} else {
						Vector2i size = new((int)texture.Size.X, (int)texture.Size.Y);
						for(uint i = 0; i < texture.MipLevels - 1; i++) {
							uint rdfbo = texture.AcquireBlitFramebuffer(GLTexture.BlitFramebuffer.Read, (int)i, 0, GLBufferMask.Color);
							uint drfbo = texture.AcquireBlitFramebuffer(GLTexture.BlitFramebuffer.Draw, (int)(i + 1), 0, GLBufferMask.Color);
							Recti rdarea = new(size.X, size.Y);
							size /= 2;
							Recti drarea = new(size.X, size.Y);
							dsa.BlitNamedFramebuffer(rdfbo, drfbo, rdarea, drarea, GLBufferMask.Color, filter.Value);
						}
					}
				};
				TextureSubImage = (GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, IntPtr pixels, int layerStride) => {
					switch(texture.GLTarget) {
						case GLTextureTarget.Texture1D:
							dsa.TextureSubImage1D(texture.ID, mipLevel, offset.X, size.X, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
						case GLTextureTarget.Texture1DArray:
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
							dsa.TextureSubImage2D(texture.ID, mipLevel, offset.X, offset.Y, size.X, size.Y, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.Texture3D:
						case GLTextureTarget.CubeMap:
						case GLTextureTarget.CubeMapArray:
							dsa.TextureSubImage3D(texture.ID, mipLevel, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
					}
				};

				CreateFramebuffer = dsa.CreateFramebuffers;
				CreateFramebuffers = ids => dsa.CreateFramebuffers(ids);
				FramebufferTexture = (uint fbo, GLFramebufferAttachment attach, IGLTexture texture, int mipLevel, int arrayLayer) => {
					if (texture.GLObjectType == GLTextureObjectType.Renderbuffer) {
						dsa.NamedFramebufferRenderbuffer(fbo, attach, GLRenderbufferTarget.Renderbuffer, texture.ID);
					} else {
						switch (texture.Type) {
							case TextureType.Texture1D:
							case TextureType.Texture2D:
								dsa.NamedFramebufferTexture(fbo, attach, texture.ID, mipLevel);
								break;
							case TextureType.Texture1DArray:
							case TextureType.Texture2DArray:
							case TextureType.Texture2DCube:
							case TextureType.Texture2DCubeArray:
								dsa.NamedFramebufferTextureLayer(fbo, attach, texture.ID, mipLevel, arrayLayer);
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

				CreateSampler = dsa.CreateSamplers;
				CreateRenderbuffer = dsa.CreateRenderbuffers;
				RenderbufferStorage = (uint id, int samples, GLInternalFormat format, int x, int y) => {
					if (samples > 1) dsa.NamedRenderbufferStorageMultisample(id, samples, format, x, y);
					else dsa.NamedRenderbufferStorage(id, format, x, y);
				};
			} else {
				CreateBuffer = gl33.GenBuffers;
				if (bs != null) {
					BufferStorage = (uint id, nint size, GLBufferStorageFlags flags) => {
						GLBufferTarget target = state.BindBufferAny(id);
						bs.BufferStorage(target, size, flags);
					};
				} else {
					BufferStorage = (uint id, nint size, GLBufferStorageFlags flags) => {
						GLBufferTarget target = state.BindBufferAny(id);
						gl33.BufferData(target, size, IntPtr.Zero, GLBufferUsage.DynamicDraw);
					};
				}
				FlushBufferHostToGPU = (uint id, nint offset, nint length) => {
					GLBufferTarget target = state.BindBufferAny(id);
					gl33.FlushMappedBufferRange(target, offset, length);
				};
				MapBuffer = (uint id, GLMapAccessFlags flags, MemoryRange range) => {
					GLBufferTarget target = state.BindBufferAny(id);
					return gl33.MapBufferRange(target, (nint)range.Offset, (nint)range.Length, flags);
				};
				UnmapBuffer = (uint id) => {
					GLBufferTarget target = state.BindBufferAny(id);
					gl33.UnmapBuffer(target);
				};
				BufferSubData = (uint id, nint offset, nint length, IntPtr data) => {
					GLBufferTarget target = state.BindBufferAny(id, GLBufferTarget.CopyWrite);
					gl33.BufferSubData(target, offset, length, data);
				};
				CopyBufferSubData = (uint srcid, uint dstid, nint srcOffset, nint dstOffset, nint length) => {
					GLBufferTarget srctarget = state.BindBufferAny(srcid, GLBufferTarget.CopyRead);
					GLBufferTarget dsttarget = state.BindBufferAny(dstid, GLBufferTarget.CopyWrite);
					gl33.CopyBufferSubData(srctarget, dsttarget, srcOffset, dstOffset, length);
				};
				if (cbo != null) {
					FillBufferUInt32 = (uint id, nint offset, nint length, uint value) => {
						GLBufferTarget target = state.BindBufferAny(id, GLBufferTarget.CopyWrite);
						cbo.ClearBufferSubData<uint>(target, GLInternalFormat.R32I, offset, length, GLFormat.R, GLType.UnsignedInt, stackalloc uint[] { value });
					};
				} else {
					FillBufferUInt32 = (uint id, nint offset, nint length, uint value) => {
						GLBufferTarget target = state.BindBufferAny(id, GLBufferTarget.CopyWrite);
						UnmanagedPointer<uint> data = new(gl33.MapBufferRange(target, offset, length, GLMapAccessFlags.Write));
						length /= 4;
						for (int i = 0; i < length; i++) data[i] = value;
					};
				}

				CreateTexture = (GLTextureTarget _) => gl33.GenTextures();
				if (ts != null) {
					TextureStorage = (GLTextureTarget target, uint id, Vector3i size, int mipLevels, int samples, GLPixelFormat format) => {
						state.BindTexture(target, id);
						switch (target) {
							case GLTextureTarget.Texture1D:
								ts.TexStorage1D(target, mipLevels, format.InternalFormat, size.X);
								break;
							case GLTextureTarget.Texture1DArray:
								ts.TexStorage2D(target, mipLevels, format.InternalFormat, size.X, size.Y);
								break;
							case GLTextureTarget.Texture2D:
							case GLTextureTarget.Texture2DMultisample:
							case GLTextureTarget.CubeMap:
								if (tsm != null && samples > 1) tsm.TexStorage2DMultisample(target, samples, format.InternalFormat, size.X, size.Y, false);
								else ts.TexStorage2D(target, mipLevels, format.InternalFormat, size.X, size.Y);
								break;
							case GLTextureTarget.Texture2DArray:
							case GLTextureTarget.CubeMapArray:
							case GLTextureTarget.Texture2DMultisampleArray:
								if (tsm != null && samples > 1) tsm.TexStorage3DMultisample(target, samples, format.InternalFormat, size.X, size.Y, size.Z, false);
								ts.TexStorage3D(target, mipLevels, format.InternalFormat, size.X, size.Y, size.Z);
								break;
							case GLTextureTarget.Texture3D:
								if (tsm != null && samples > 1) tsm.TexStorage3DMultisample(target, samples, format.InternalFormat, size.X, size.Y, size.Z, false);
								else ts.TexStorage3D(target, mipLevels, format.InternalFormat, size.X, size.Y, size.Z);
								break;
						}
					};
				} else {
					TextureStorage = (GLTextureTarget target, uint id, Vector3i size, int mipLevels, int samples, GLPixelFormat format) => {
						state.BindTexture(target, id);
						state.BindBuffer(GLBufferTarget.PixelUnpack, 0);
						switch (target) {
							case GLTextureTarget.Texture1D:
								gl33.TexImage1D(target, mipLevels, format.InternalFormat, size.X, 0, format.Format, format.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture1DArray:
								gl33.TexImage2D(target, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture2D:
							case GLTextureTarget.Texture2DMultisample:
								if (samples > 1) gl33.TexImage2DMultisample(target, samples, format.InternalFormat, size.X, size.Y, false);
								else gl33.TexImage2D(target, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture2DArray:
							case GLTextureTarget.CubeMapArray:
								gl33.TexImage3D(target, mipLevels, format.InternalFormat, size.X, size.Y, size.Z, 0, format.Format, format.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.CubeMap:
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveX, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeX, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveY, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeY, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveZ, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeZ, mipLevels, format.InternalFormat, size.X, size.Y, 0, format.Format, format.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture3D:
								gl33.TexImage3D(target, mipLevels, format.InternalFormat, size.X, size.Y, size.Z, 0, format.Format, format.Type, IntPtr.Zero);
								break;
						}
					};
				}
				BindTextureUnit = (uint unit, GLTextureTarget target, uint id) => {
					state.ActiveTextureUnit = unit;
					gl33.BindTexture(target, id);
				};
				GenerateMipmaps = (GLTexture texture, GLFilter? filter) => {
					if (filter == null) {
						state.BindTexture(texture.GLTarget, texture.ID);
						gl33.GenerateMipmap(texture.GLTarget);
					} else {
						Vector2i size = new((int)texture.Size.X, (int)texture.Size.Y);
						for (uint i = 0; i < texture.MipLevels - 1; i++) {
							uint rdfbo = texture.AcquireBlitFramebuffer(GLTexture.BlitFramebuffer.Read, (int)i, 0, GLBufferMask.Color);
							uint drfbo = texture.AcquireBlitFramebuffer(GLTexture.BlitFramebuffer.Draw, (int)(i + 1), 0, GLBufferMask.Color);
							Recti rdarea = new(size.X, size.Y);
							size /= 2;
							Recti drarea = new(size.X, size.Y);
							gl33.BindFramebuffer(GLFramebufferTarget.Read, rdfbo);
							gl33.BindFramebuffer(GLFramebufferTarget.Draw, drfbo);
							gl33.BlitFramebuffer(rdarea, drarea, GLBufferMask.Color, filter.Value);
						}
					}
				};
				TextureSubImage = (GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, IntPtr pixels, int layerStride) => {
					gl33.BindTexture(texture.GLTarget, texture.ID);
					switch (texture.GLTarget) {
						case GLTextureTarget.Texture1D:
							gl33.TexSubImage1D(texture.GLTarget, mipLevel, offset.X, size.X, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
						case GLTextureTarget.Texture1DArray:
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
							gl33.TexSubImage2D(texture.GLTarget, mipLevel, offset.X, offset.Y, size.X, size.Y, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.Texture3D:
						case GLTextureTarget.CubeMapArray:
							gl33.TexSubImage3D(texture.GLTarget, mipLevel, offset.X, offset.Y, offset.Z, size.X, size.Y, size.Z, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
							break;
						case GLTextureTarget.CubeMap: // For old cubemap system upload to each target, adjusting pointer by layer stride
							for(int i = 0; i < size.Z; i++) {
								int layer = offset.Z + i;
								gl33.TexSubImage2D(texture.GetSubresourceTarget(layer), mipLevel, offset.X, offset.Y, size.X, size.Y, texture.GLFormat.Format, texture.GLFormat.Type, pixels + layerStride * i);
							}
							break;
					}
				};

				CreateFramebuffer = gl33.GenFramebuffers;
				CreateFramebuffers = ids => gl33.GenFramebuffers(ids);
				FramebufferTexture = (uint fbo, GLFramebufferAttachment attach, IGLTexture texture, int mipLevel, int arrayLayer) => {
					gl33.BindFramebuffer(GLFramebufferTarget.Read, fbo);
					if (texture.GLObjectType == GLTextureObjectType.Renderbuffer) {
						gl33.FramebufferRenderbuffer(GLFramebufferTarget.Read, attach, GLRenderbufferTarget.Renderbuffer, texture.ID);
					} else {
						switch (texture.Type) {
							case TextureType.Texture1D:
							case TextureType.Texture2D:
								gl33.FramebufferTexture(GLFramebufferTarget.Read, attach, texture.ID, mipLevel);
								break;
							case TextureType.Texture1DArray:
							case TextureType.Texture2DArray:
							case TextureType.Texture2DCube:
							case TextureType.Texture2DCubeArray:
								gl33.FramebufferTextureLayer(GLFramebufferTarget.Read, attach, texture.ID, mipLevel, arrayLayer);
								break;
							default:
								break;
						}
					}
				};
				ClearFramebufferi = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4i value) => {
					state.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferu = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4ui value) => {
					state.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferf = (uint fbo, GLClearBuffer buffer, int drawbuffer, Vector4 value) => {
					state.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, value);
				};
				ClearFramebufferfi = (uint fbo, GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => {
					state.BindFramebuffer(GLFramebufferTarget.Draw, fbo);
					gl33.ClearBuffer(buffer, drawbuffer, depth, stencil);
				};

				CreateSampler = gl33.GenSamplers;
				CreateRenderbuffer = gl33.GenRenderbuffers;
				RenderbufferStorage = (uint id, int samples, GLInternalFormat format, int x, int y) => {
					state.BindRenderbuffer(GLRenderbufferTarget.Renderbuffer, id);
					if (samples > 1) gl33.RenderbufferStorageMultisample(GLRenderbufferTarget.Renderbuffer, samples, format, x, y);
					else gl33.RenderbufferStorage(GLRenderbufferTarget.Renderbuffer, format, x, y);
				};
			}

			if (bs != null && sils != null) {
				FlushBufferGPUToHost = (uint _, nint _, nint _) => sils.MemoryBarrier(GLMemoryBarrier.ClientMappedBuffer);
			} else {
				FlushBufferGPUToHost = (uint _, nint _, nint _) => { }; // No-op, not possible due to lack of persistent mapping
			}

			if (isd != null) {
				if (dsa != null) {
					InvalidateSubFramebuffer = dsa.InvalidateNamedFramebufferSubData;
				} else {
					InvalidateSubFramebuffer = (uint fbo, Recti area, GLFramebufferAttachment[] attachments) => {
						state.BindFramebuffer(GLFramebufferTarget.Read, fbo);
						isd.InvalidateSubFramebuffer(GLFramebufferTarget.Read, area, attachments);
					};
				}
				InvalidateTextureImage = (uint tex, uint mipLevel) => isd.InvalidateTexImage(tex, (int)mipLevel);
			} else {
				// Invalidates can be ignored as no-ops if not available, as they are just hints that resources don't need to be preserved
				InvalidateSubFramebuffer = (uint fbo, Recti area, GLFramebufferAttachment[] attachments) => { };
				InvalidateTextureImage = (uint tex, uint mipLevel) => { };
			}

			if (bi != null) {
				Draw = (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) =>
					bi.DrawArraysInstancedBaseInstance(
						state.DrawMode,
						(int)firstVertex,
						(int)vertexCount,
						(int)instanceCount,
						firstInstance
					);
				DrawIndexed = (uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) =>
					bi.DrawElementsInstancedBaseVertexBaseInstance(
						state.DrawMode,
						(int)indexCount,
						state.IndexType,
						(nint)(state.IndexOffset + (state.IndexStride * firstIndex)),
						(int)instanceCount,
						(int)firstIndex,
						firstInstance
					);
			} else {
				Draw = (uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance) => {
					if (firstInstance == 0)
						gl33.DrawArraysInstanced(
							state.DrawMode,
							(int)firstVertex,
							(int)vertexCount,
							(int)instanceCount
						);
					else
						throw new GLException("Cannot draw with base instance != 0 without GL_ARB_base_instance");
				};
				DrawIndexed = (uint indexCount, uint instanceCount, uint firstIndex, int vertexOffset, uint firstInstance) => {
					if (firstInstance == 0)
						gl33.DrawElementsInstancedBaseVertex(
							state.DrawMode,
							(int)indexCount,
							state.IndexType,
							(nint)(state.IndexOffset + (firstIndex * state.IndexStride)),
							(int)instanceCount,
							(int)firstInstance
						);
					else
						throw new GLException("Cannot draw with base instance != 0 without GL_ARB_base_instance");
				};
			}

			if (mdi != null) {
				DrawIndirect = (uint id, nint offset, int drawCount, int stride) => {
					state.BindBuffer(GLBufferTarget.DrawIndirect, id);
					mdi.MultiDrawArraysIndirect(state.DrawMode, offset, drawCount, stride);
				};
				DrawIndexedIndirect = (uint id, nint offset, int drawCount, int stride) => {
					state.BindBuffer(GLBufferTarget.DrawIndirect, id);
					mdi.MultiDrawElementsIndirect(state.DrawMode, state.IndexType, offset, drawCount, stride);
				};
			} else if (di != null) {
				DrawIndirect = (uint id, nint offset, int drawCount, int stride) => {
					if (drawCount != 1) throw new GLException("Cannot draw indirect with drawCount != 1 without GL_ARB_multi_draw_indirect");
					state.BindBuffer(GLBufferTarget.DrawIndirect, id);
					di.DrawArraysIndirect(state.DrawMode, offset);
				};
				DrawIndexedIndirect = (uint id, nint offset, int drawCount, int stride) => {
					if (drawCount != 1) throw new GLException("Cannot draw indirect with drawCount != 1 without GL_ARB_multi_draw_indirect");
					state.BindBuffer(GLBufferTarget.DrawIndirect, id);
					di.DrawElementsIndirect(state.DrawMode, state.IndexType, offset);
				};
			} else {
				DrawIndirect = DrawIndexedIndirect = (uint id, nint offset, int drawCount, int stride) =>
					throw new GLException("Cannot draw indirect without GL_ARB_draw_indirect or GL_ARB_multi_draw_indirect");
			}

			if (cs != null) {
				Dispatch = cs.DispatchCompute;
				DispatchIndirect = (uint id, nint offset) => {
					state.BindBuffer(GLBufferTarget.DispatchIndirect, id);
					cs.DispatchComputeIndirect(offset);
				};
			} else {
				Dispatch = (Vector3ui _) => throw new GLException("Cannot dispatch without GL_ARB_compute_shader");
				DispatchIndirect = (uint _, nint _) => throw new GLException("Cannot dispatch without GL_ARB_compute_shader");
			}

			if (dbb != null) {
				SetAttachmentsBlendState = (GLPipeline.ColorAttachmentState[] state) => {
					for (int i = 0; i < state.Length; i++) {
						var attach = state[i];
						if (attach.BlendEnable) gl33.Enable(GLIndexedCapability.Blend, (uint)i);
						else gl33.Disable(GLIndexedCapability.Blend, (uint)i);
						dbb.BlendEquationSeparate((uint)i, attach.RGBOp, attach.AlphaOp);
						dbb.BlendFuncSeparate((uint)i, attach.SrcRGB, attach.DstRGB, attach.SrcAlpha, attach.DstAlpha);
					}
				};
			} else {
				SetAttachmentsBlendState = (GLPipeline.ColorAttachmentState[] state) => {
					if (state.Length > 0) {
						var attach = state[0];
						if (attach.BlendEnable) gl33.Enable(GLCapability.Blend);
						else gl33.Disable(GLCapability.Blend);
						gl33.BlendEquationSeparate(attach.RGBOp, attach.AlphaOp);
						gl33.BlendFuncSeparate(attach.SrcRGB, attach.DstRGB, attach.SrcAlpha, attach.DstAlpha);
					}
				};
			}

			if (tss != null) {
				SetPatchControlPoints = (uint cp) => tss.PatchParamter(GLPatchParamteri.Vertices, (int)cp);
			} else {
				SetPatchControlPoints = (uint _) => throw new GLException("Cannot set patch control points without GL_ARB_tessellation_shader");
			}

			if (va != null) {
				SetViewports = (uint first, in ReadOnlySpan<Viewport> viewports) => {
					for (int i = 0; i < viewports.Length; i++) {
						var viewport = viewports[i];
						var area = viewport.Area;
						va.ViewportIndexed((uint)i + first, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
						va.DepthRangeIndexed((uint)i + first, viewport.DepthBounds.Item1, viewport.DepthBounds.Item2);
					}
				};
				SetScissors = (uint first, in ReadOnlySpan<Recti> scissors) => {
					for (int i = 0; i < scissors.Length; i++) {
						var scissor = scissors[i]; ;
						va.ScissorIndexed((uint)i + first, scissor);
					}
				};
			} else {
				SetViewports = (uint first, in ReadOnlySpan<Viewport> viewports) => {
					if (first != 0) return;
					var viewport = viewports[0];
					var area = viewport.Area;
					gl33.Viewport = new Recti((int)area.Position.X, (int)area.Position.Y, (int)area.Size.X, (int)area.Size.Y);
					gl33.DepthRange = (viewport.DepthBounds.Item1, viewport.DepthBounds.Item2);
				};
				SetScissors = (uint first, in ReadOnlySpan<Recti> scissors) => {
					if (first != 0) return;
					var scissor = scissors[0];
					gl33.Scissor(scissor.Position.X, scissor.Position.Y, scissor.Size.X, scissor.Size.Y);
				};
			}

			if (poc != null) {
				SetDepthBias = poc.PolygonOffsetClamp;
			} else {
				SetDepthBias = (float cf, float sf, float _) => gl33.PolygonOffset(cf, sf);
			}

			if (dbt != null) {
				SetDepthBoundsTestEnable = (bool enable) => {
					if (enable) gl33.Enable(GLCapability.DepthBoundsTestEXT);
					else gl33.Disable(GLCapability.DepthBoundsTestEXT);
				};
				SetDepthBounds = (float min, float max) => dbt.DepthBounds = (min, max);
			} else {
				SetDepthBoundsTestEnable = (bool _) => throw new GLException("Cannot set depth bounds test enable without GL_EXT_depth_bounds_test");
				SetDepthBounds = (float _, float _) => throw new GLException("Cannot set depth bounds without GL_EXT_depth_bounds_test");
			}

			if (ci != null) {
				CopyImageSubData = (GLTexture dstTexture, int dstMipLevel, Vector3i dstPos, GLTexture srcTexture, int srcMipLevel, Vector3i srcPos, Vector3i size) =>
					ci.CopyImageSubData(srcTexture.ID, GetCopyTarget(srcTexture), srcMipLevel, srcPos, dstTexture.ID, GetCopyTarget(dstTexture), dstMipLevel, dstPos, size);
			} else {
				CopyImageSubData = (GLTexture dstTexture, int dstMipLevel, Vector3i dstPos, GLTexture srcTexture, int srcMipLevel, Vector3i srcPos, Vector3i size) => {
					
				};
			}

			if (gtsi != null) {
				GetTextureSubImage = (GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels) =>
					gtsi.GetTextureSubImage(texture.ID, mipLevel, offset, size, texture.GLFormat.Format, texture.GLFormat.Type, bufSize, pixels); ;
			} else {
				GetTextureSubImage = (GLTexture texture, int mipLevel, Vector3i offset, Vector3i size, int bufSize, IntPtr pixels) => {
					// Usage validation is done externally, so calls to this function don't repeat validation
					gl33.BindTexture(texture.GLTarget, texture.ID);
					gl33.GetTexImage(texture.GLTarget, mipLevel, texture.GLFormat.Format, texture.GLFormat.Type, pixels);
				};
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static GLCopyImageTarget GetCopyTarget(GLTexture texture) =>
			texture.GLObjectType == GLTextureObjectType.Renderbuffer ? GLCopyImageTarget.Renderbuffer : (GLCopyImageTarget)texture.GLTarget;
	
	}
}
