using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {

	public enum GLTextureObjectType {
		Texture,
		Renderbuffer
	}

	public class GLTexture : ITexture, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public Vector3i Size { get; }

		public uint MipLevels { get; }

		public uint ArrayLayers { get; }

		public uint Samples { get; }

		public IMemoryBinding? MemoryBinding => null;

		public TextureUsage Usage { get; }


		public GLTextureObjectType GLObjectType { get; }

		public uint ID { get; }

		public GLPixelFormat GLFormat { get; }

		public GLTextureTarget GLTarget { get; }


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

			GLFormat = GLEnums.StdToGLFormat(info.Format);
			if (GLFormat == null) throw new GLException("Unsupported pixel format");

			// Check if we can get away with using a renderbuffer based on the texture usage
			bool canUseRenderbuffer = true;
			if ((info.Usage & (TextureUsage.ColorAttachment | TextureUsage.DepthStencilAttachment)) != 0) canUseRenderbuffer = false;
			if (canUseRenderbuffer && (info.Usage & TextureUsage.SubView) != 0) canUseRenderbuffer = false;
			if (canUseRenderbuffer && info.Type != TextureType.Texture2D) canUseRenderbuffer = false;

			GL33 gl33 = GL.GL33!;
			var dsa = GL.ARBDirectStateAccess;
			if (canUseRenderbuffer) {
				// Initialize the renderbuffer storage
				if (dsa != null) {
					ID = dsa.CreateRenderbuffers();
					if (Samples > 1) dsa.NamedRenderbufferStorageMultisample(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y);
					else dsa.NamedRenderbufferStorage(ID, GLFormat.InternalFormat, Size.X, Size.Y);
				} else {
					ID = gl33.GenRenderbuffers();
					Graphics.State.BindRenderbuffer(GLRenderbufferTarget.Renderbuffer, ID);
					if (Samples > 1) gl33.RenderbufferStorageMultisample(GLRenderbufferTarget.Renderbuffer, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y);
					else gl33.RenderbufferStorage(GLRenderbufferTarget.Renderbuffer, GLFormat.InternalFormat, Size.X, Size.Y);
				}
			} else {
				// Initialize the texture storage
				GLTarget = GetGLTarget(GL, Type, Samples);
				if (dsa != null) {
					ID = dsa.CreateTextures(GLTarget);
					switch (GLTarget) {
						case GLTextureTarget.Texture1D:
							dsa.TextureStorage1D(ID, (int)MipLevels, GLFormat.InternalFormat, Size.X);
							break;
						case GLTextureTarget.Texture1DArray:
							dsa.TextureStorage2D(ID, (int)MipLevels, GLFormat.InternalFormat, Size.X, (int)ArrayLayers);
							break;
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
						case GLTextureTarget.CubeMap:
							if (Samples > 1) dsa.TextureStorage2DMultisample(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, false);
							else dsa.TextureStorage2D(ID, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y);
							break;
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.CubeMapArray:
							if (Samples > 1) dsa.TextureStorage3DMultisample(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, (int)ArrayLayers, false);
							else dsa.TextureStorage3D(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, (int)ArrayLayers);
							break;
						case GLTextureTarget.Texture3D:
							if (Samples > 1) dsa.TextureStorage3DMultisample(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, Size.Z, false);
							else dsa.TextureStorage3D(ID, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, Size.Z);
							break;
					}
				} else {
					var ts = GL.ARBTextureStorage;
					var tsm = GL.ARBTextureStorageMultisample;
					ID = gl33.GenTextures();
					gl33.BindTexture(GLTarget, ID);
					if (ts != null) {
						switch (GLTarget) {
							case GLTextureTarget.Texture1D:
								ts.TexStorage1D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X);
								break;
							case GLTextureTarget.Texture1DArray:
								ts.TexStorage2D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, (int)ArrayLayers);
								break;
							case GLTextureTarget.Texture2D:
							case GLTextureTarget.Texture2DMultisample:
							case GLTextureTarget.CubeMap:
								if (tsm != null && Samples > 1) tsm.TexStorage2DMultisample(GLTarget, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, false);
								else ts.TexStorage2D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y);
								break;
							case GLTextureTarget.Texture2DArray:
							case GLTextureTarget.CubeMapArray:
							case GLTextureTarget.Texture2DMultisampleArray:
								if (tsm != null && Samples > 1) tsm.TexStorage3DMultisample(GLTarget, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, (int)ArrayLayers, false);
								ts.TexStorage3D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, (int)ArrayLayers);
								break;
							case GLTextureTarget.Texture3D:
								if (tsm != null && Samples > 1) tsm.TexStorage3DMultisample(GLTarget, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, Size.Z, false);
								else ts.TexStorage3D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, Size.Z);
								break;
						}
					} else {
						Graphics.State.BindBuffer(GLBufferTarget.PixelUnpack, 0);
						switch (GLTarget) {
							case GLTextureTarget.Texture1D:
								gl33.TexImage1D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture1DArray:
								gl33.TexImage2D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, (int)ArrayLayers, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture2D:
							case GLTextureTarget.Texture2DMultisample:
								if (Samples > 1) gl33.TexImage2DMultisample(GLTarget, (int)Samples, GLFormat.InternalFormat, Size.X, Size.Y, false);
								else gl33.TexImage2D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture2DArray:
							case GLTextureTarget.CubeMapArray:
								gl33.TexImage3D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, (int)ArrayLayers, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.CubeMap:
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveX, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeX, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveY, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeY, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapPositiveZ, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								gl33.TexImage2D(GLTextureTarget.CubeMapNegativeZ, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
							case GLTextureTarget.Texture3D:
								gl33.TexImage3D(GLTarget, (int)MipLevels, GLFormat.InternalFormat, Size.X, Size.Y, Size.Z, 0, GLFormat.Format, GLFormat.Type, IntPtr.Zero);
								break;
						}
					}
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var gl33 = GL.GL33!;
			gl33.DeleteTextures(ID);
			if (blitFramebuffer != 0) gl33.DeleteFramebuffers(blitFramebuffer);
		}


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

		private uint blitFramebuffer = 0;
		private int currentBlitLevel = 0;
		private int currentBlitLayer = 0;
		private GLBufferMask currentBlitMask = default;

		public uint AcquireBlitFramebuffer(int mipLevel, int arrayLayer, GLBufferMask mask) {
			var gl33 = GL.GL33!;
			var dsa = GL.ARBDirectStateAccess;

			int? curLevel = null;
			int? curLayer = null;
			GLBufferMask? curBufMask = null;

			// Make sure blit framebuffer exists
			if (blitFramebuffer == 0) {
				if (dsa != null) blitFramebuffer = dsa.CreateFramebuffers();
				else blitFramebuffer = gl33.GenFramebuffers();
			} else {
				curLevel = currentBlitLevel;
				curLayer = currentBlitLayer;
				curBufMask = currentBlitMask;
			}

			if (curLevel != mipLevel || curLayer != arrayLayer || curBufMask != mask) {
				GLFramebufferAttachment attach = default;
				if ((mask & GLBufferMask.Color) != 0) attach = GLFramebufferAttachment.Color0;
				else {
					if ((mask & GLBufferMask.Depth) != 0) attach = GLFramebufferAttachment.Depth;
					if ((mask & GLBufferMask.Stencil) != 0) attach = GLFramebufferAttachment.Stencil;
				}

				if (dsa != null && GLTarget != GLTextureTarget.CubeMap) {
					switch(GLTarget) {
						case GLTextureTarget.Texture1D:
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
							dsa.NamedFramebufferTexture(blitFramebuffer, attach, ID, mipLevel);
							break;
						case GLTextureTarget.Texture1DArray:
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.CubeMapArray:
							dsa.NamedFramebufferTextureLayer(blitFramebuffer, attach, ID, mipLevel, arrayLayer);
							break;
						default:
							break;
					}
				} else {
					Graphics.State.BindTexture(GLTarget, ID);
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Read, blitFramebuffer);
					switch(GLTarget) {
						case GLTextureTarget.Texture1D:
							gl33.FramebufferTexture1D(GLFramebufferTarget.Read, attach, GLTarget, ID, mipLevel);
							break;
						case GLTextureTarget.Texture2D:
						case GLTextureTarget.Texture2DMultisample:
							gl33.FramebufferTexture2D(GLFramebufferTarget.Read, attach, GLTarget, ID, mipLevel);
							break;
						case GLTextureTarget.Texture3D:
							gl33.FramebufferTexture3D(GLFramebufferTarget.Read, attach, GLTarget, ID, mipLevel, arrayLayer);
							break;
						case GLTextureTarget.Texture1DArray:
						case GLTextureTarget.Texture2DArray:
						case GLTextureTarget.Texture2DMultisampleArray:
						case GLTextureTarget.CubeMapArray:
							gl33.FramebufferTextureLayer(GLFramebufferTarget.Read, attach, ID, mipLevel, arrayLayer);
							break;
						case GLTextureTarget.CubeMap:
							gl33.FramebufferTexture2D(GLFramebufferTarget.Read, attach, GetSubresourceTarget(arrayLayer), ID, mipLevel);
							break;
						default:
							break;
					}
				}

				currentBlitLevel = mipLevel;
				currentBlitLayer = arrayLayer;
				currentBlitMask = mask;
			}

			return blitFramebuffer;
		}

	}

	public class GLTextureView : ITextureView, IGLObject {

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

		public GLTextureView(GLGraphics graphics, TextureViewCreateInfo createInfo) {
			Graphics = graphics;
			Texture = (GLTexture)createInfo.Texture;
			Type = createInfo.Type;
			Format = createInfo.Format;
			Mapping = createInfo.Mapping;
			SubresourceRange = createInfo.SubresourceRange;

			GLFormat = GLEnums.StdToGLFormat(createInfo.Format);
			if (GLFormat == null) throw new GLException("Unsupported pixel format");

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
