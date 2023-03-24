using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenGL.Graphics {

	public interface IGLTexture {

		public GLTextureObjectType GLObjectType { get; }

		public uint ID { get; }

		public PixelFormat Format { get; }

		public GLPixelFormat GLFormat { get; }

		public GLTextureTarget GLTarget { get; }


		public TextureType Type { get; }

	}

	public enum GLTextureObjectType {
		Texture,
		Renderbuffer
	}

	public class GLTexture : ITexture, ITextureView, IGLTexture, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		private GLInterface Interface => Graphics.Interface;

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public Vector3ui Size { get; }

		public uint MipLevels { get; }

		public uint ArrayLayers { get; }

		public uint Samples { get; }

		public IMemoryBinding? MemoryBinding => null;

		public TextureUsage Usage { get; }

		public ITextureView IdentityView => this;

		public ComponentMapping Mapping { get; } = new();

		public TextureSubresourceRange SubresourceRange { get; }


		public GLTextureObjectType GLObjectType { get; }

		public uint ID { get; }

		public GLPixelFormat GLFormat { get; }

		public GLTextureTarget GLTarget { get; }

		/// <summary>
		/// Gets the texture target used for the given texture type and sampling.
		/// </summary>
		/// <param name="gl">OpenGL instance</param>
		/// <param name="type">Type of texture</param>
		/// <param name="samples">Number of samples in the texture</param>
		/// <returns>OpenGL texture target</returns>
		public static GLTextureTarget GetGLTarget(GL gl, TextureType type, uint samples) => type switch {
			TextureType.Texture1D => GLTextureTarget.Texture1D,
			TextureType.Texture1DArray => GLTextureTarget.Texture1DArray,
			TextureType.Texture2D => samples > 1 ? GLTextureTarget.Texture2DMultisample : GLTextureTarget.Texture2D,
			TextureType.Texture2DArray => samples > 1 ? GLTextureTarget.Texture2DMultisampleArray : GLTextureTarget.Texture2DArray,
			// We prefer to treat cubemaps as array textures if possible, otherwise we have to fall back to multi-target weirdness
			TextureType.Texture2DCube => gl.ARBTextureCubeMapArray ? GLTextureTarget.CubeMapArray : GLTextureTarget.CubeMap,
			TextureType.Texture2DCubeArray => GLTextureTarget.CubeMapArray,
			_ => GLTextureTarget.Texture2D
		};

		public GLTexture(GLGraphics graphics, TextureCreateInfo info) {
			Graphics = graphics;
			Type = info.Type;
			Format = info.Format;
			Size = info.Size;
			MipLevels = info.MipLevels;
			ArrayLayers = info.ArrayLayers;
			Samples = info.Samples;
			Usage = info.Usage;

			GLFormat = GLEnums.StdToGLFormat(info.Format) ?? throw new GLException("Unsupported pixel format");

			// Check if we can get away with using a renderbuffer based on the texture usage
			bool canUseRenderbuffer = true;
			// Cannot use a renderbuffer for non-attachment images
			if ((info.Usage & ~(TextureUsage.ColorAttachment | TextureUsage.DepthStencilAttachment)) != 0) canUseRenderbuffer = false;
			// Cannot use renderbuffer for texture sub-views
			if (canUseRenderbuffer && (info.Usage & TextureUsage.SubView) != 0) canUseRenderbuffer = false;
			// Cannot use renderbuffer for non-2D textures
			if (canUseRenderbuffer && info.Type != TextureType.Texture2D) canUseRenderbuffer = false;
			// Cannot use renderbuffer for mipmapped textures
			if (canUseRenderbuffer && MipLevels != 1) canUseRenderbuffer = false;

			if (canUseRenderbuffer) {
				// Initialize the renderbuffer storage
				ID = Interface.CreateRenderbuffer();
				Interface.RenderbufferStorage(ID, (int)Samples, GLFormat.InternalFormat, (int)Size.X, (int)Size.Y);
			} else {
				// Initialize the texture storage
				GLTarget = GetGLTarget(GL, Type, Samples);
				ID = Interface.CreateTexture(GLTarget);
				Vector3i size = new() { X = (int)Size.X, Y = (int)Size.Y, Z = (int)Size.Z };
				switch(GLTarget) {
					case GLTextureTarget.Texture1DArray:
						size.Y = (int)ArrayLayers;
						break;
					case GLTextureTarget.Texture2DArray:
					case GLTextureTarget.Texture2DMultisampleArray:
					case GLTextureTarget.CubeMapArray:
						size.Z = (int)ArrayLayers;
						break;
					default:
						break;
				}
				Interface.TextureStorage(GLTarget, ID, size, (int)MipLevels, (int)Samples, GLFormat);
			}

			SubresourceRange = new TextureSubresourceRange() {
				Aspects = Format.Aspects,
				ArrayLayerCount = ArrayLayers,
				MipLevelCount = MipLevels
			};
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var gl33 = GL.GL33!;
			gl33.DeleteTextures(ID);
			foreach(BlitFramebufferState state in blitStates) {
				uint fbo = state.blitFramebuffer;
				if (fbo != 0) gl33.DeleteFramebuffers(fbo);
			}
		}

		/// <summary>
		/// Get the texture target for a specific array layer in this texture. This is necessary
		/// for implementations that don't support GL_ARB_texture_cube_map_array and need to use
		/// the old system of different cubemap face targets.
		/// </summary>
		/// <param name="arrayLayer">Texture array layer</param>
		/// <returns>Corresponding texture target</returns>
		public GLTextureTarget GetSubresourceTarget(int arrayLayer) {
			if (GLTarget == GLTextureTarget.CubeMapArray) {
				switch(arrayLayer) {
					case 0: return GLTextureTarget.CubeMapPositiveX;
					case 1: return GLTextureTarget.CubeMapNegativeX;
					case 2: return GLTextureTarget.CubeMapPositiveY;
					case 3: return GLTextureTarget.CubeMapNegativeY;
					case 4: return GLTextureTarget.CubeMapPositiveZ;
					case 5: return GLTextureTarget.CubeMapNegativeZ;
					default: break;
				}
			}
			return GLTarget;
		}

		// State object for a blit framebuffer
		private struct BlitFramebufferState {

			// The framebuffer used for texture blits
			public uint blitFramebuffer = 0;
			// The current mip level bound to the blit framebuffer
			public int currentBlitLevel = 0;
			// The current array layer bound to the blit framebuffer
			public int currentBlitLayer = 0;
			// The current buffer type bound to the blit framebuffer
			public GLBufferMask currentBlitMask = default;

			public BlitFramebufferState() { }

		}

		/// <summary>
		/// Enumeration of blittable framebuffers for a texture.
		/// </summary>
		public enum BlitFramebuffer {
			/// <summary>
			/// The "read" framebuffer.
			/// </summary>
			Read = 0,
			/// <summary>
			/// The "draw" framebuffer.
			/// </summary>
			Draw = 1
		}

		private readonly BlitFramebufferState[] blitStates = new BlitFramebufferState[2];

		/// <summary>
		/// Acquires a 2D layer of this texture as a blittable framebuffer.
		/// </summary>
		/// <param name="blit">Blit framebuffer to use</param>
		/// <param name="mipLevel">Mipmap level to select</param>
		/// <param name="arrayLayer">Array layer to select</param>
		/// <param name="mask">Aspect that should be acquired</param>
		/// <returns>Blittable framebuffer object</returns>
		public uint AcquireBlitFramebuffer(BlitFramebuffer blit, int mipLevel, int arrayLayer, GLBufferMask mask) {
			ref BlitFramebufferState state = ref blitStates[(int)blit];

			int? curLevel = null;
			int? curLayer = null;
			GLBufferMask? curBufMask = null;

			// Make sure blit framebuffer exists
			if (state.blitFramebuffer == 0) {
				state.blitFramebuffer = Interface.CreateFramebuffer();
			} else {
				curLevel = state.currentBlitLevel;
				curLayer = state.currentBlitLayer;
				curBufMask = state.currentBlitMask;
			}

			// If there is a difference in blit framebuffer state
			if (curLevel != mipLevel || curLayer != arrayLayer || curBufMask != mask) {
				GLFramebufferAttachment attach = default;
				if ((mask & GLBufferMask.Color) != 0) attach = GLFramebufferAttachment.Color0;
				else {
					if ((mask & GLBufferMask.Depth) != 0) attach = GLFramebufferAttachment.Depth;
					if ((mask & GLBufferMask.Stencil) != 0) attach = GLFramebufferAttachment.Stencil;
				}

				// Update the blit texture
				Interface.FramebufferTexture(state.blitFramebuffer, attach, this, mipLevel, arrayLayer);

				state.currentBlitLevel = mipLevel;
				state.currentBlitLayer = arrayLayer;
				state.currentBlitMask = mask;
			}

			return state.blitFramebuffer;
		}

	}

	/// <summary>
	/// OpenGL texture view implementation.
	/// </summary>
	public class GLTextureView : ITextureView, IGLTexture, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public GLPixelFormat GLFormat { get; }

		public ComponentMapping Mapping { get; }

		public TextureSubresourceRange SubresourceRange { get; }


		public GLTexture Texture { get; }

		public uint ID { get; }

		public GLTextureObjectType GLObjectType => Texture.GLObjectType;

		public GLTextureTarget GLTarget => Texture.GLTarget;

		public GLTextureView(GLGraphics graphics, TextureViewCreateInfo createInfo) {
			Graphics = graphics;
			Texture = (GLTexture)createInfo.Texture;
			Type = createInfo.Type;
			Format = createInfo.Format;
			Mapping = createInfo.Mapping;
			SubresourceRange = createInfo.SubresourceRange;

			GLFormat = GLEnums.StdToGLFormat(createInfo.Format) ?? throw new GLException("Unsupported pixel format");

			bool isIdentityMapping =
					Type == Texture.Type &&
					Format == Texture.Format &&
					Mapping.IsIdentity &&
					SubresourceRange.BaseMipLevel == 0 &&
					SubresourceRange.MipLevelCount == Texture.MipLevels &&
					SubresourceRange.BaseArrayLayer == 0 &&
					SubresourceRange.ArrayLayerCount == Texture.ArrayLayers;

			if (GLObjectType == GLTextureObjectType.Renderbuffer) {
				if (!isIdentityMapping) throw new GLException("Cannot create sub-view of renderbuffer-based texture");
				else ID = Texture.ID;
			} else {
				if (isIdentityMapping) {
					ID = Texture.ID;
				} else {
					GLTextureTarget target = GLTexture.GetGLTarget(GL, Type, Texture.Samples);
					var tv = GL.ARBTextureView;
					if (tv == null) throw new GLException("Cannot create texture sub-view without GL_ARB_texture_view");
					var dsa = GL.ARBDirectStateAccess;
					ID = dsa != null ? dsa.CreateTextures(target) : GL.GL33!.GenTextures();
					tv.TextureView(ID, target, Texture.ID, GLFormat.InternalFormat, SubresourceRange.BaseMipLevel, SubresourceRange.MipLevelCount, SubresourceRange.BaseArrayLayer, SubresourceRange.ArrayLayerCount);
					if (dsa != null) {
						dsa.TextureParameter(ID, GLTexParamter.SwizzleR, (int)GLEnums.Convert(Mapping.Red, GLTextureSwizzle.Red));
						dsa.TextureParameter(ID, GLTexParamter.SwizzleG, (int)GLEnums.Convert(Mapping.Green, GLTextureSwizzle.Green));
						dsa.TextureParameter(ID, GLTexParamter.SwizzleB, (int)GLEnums.Convert(Mapping.Blue, GLTextureSwizzle.Blue));
						dsa.TextureParameter(ID, GLTexParamter.SwizzleA, (int)GLEnums.Convert(Mapping.Alpha, GLTextureSwizzle.Alpha));
					} else {
						Graphics.State.BindTexture(target, ID);
						var gl33 = GL.GL33!;
						gl33.TexParameter(target, GLTexParamter.SwizzleR, (int)GLEnums.Convert(Mapping.Red, GLTextureSwizzle.Red));
						gl33.TexParameter(target, GLTexParamter.SwizzleG, (int)GLEnums.Convert(Mapping.Green, GLTextureSwizzle.Green));
						gl33.TexParameter(target, GLTexParamter.SwizzleB, (int)GLEnums.Convert(Mapping.Blue, GLTextureSwizzle.Blue));
						gl33.TexParameter(target, GLTexParamter.SwizzleA, (int)GLEnums.Convert(Mapping.Alpha, GLTextureSwizzle.Alpha));
					}
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (ID != Texture.ID) GL.GL33!.DeleteTextures(ID);
		}

	}

}
