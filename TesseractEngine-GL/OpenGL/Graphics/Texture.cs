using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {

	public class GLTexture : ITexture, IGLObject {

		public GL GL { get; }

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public Vector3i Size { get; }

		public uint MipLevels { get; }

		public uint ArrayLayers { get; }

		public uint Samples { get; }

		public IMemoryBinding MemoryBinding => null;

		public uint ID { get; }

		public GLPixelFormat GLFormat { get; }

		public static GLTextureTarget GetGLTarget(TextureType type, uint arrayLayers, uint samples) => type switch {
			TextureType.Texture1D => GLTextureTarget.Texture1D,
			TextureType.Texture1DArray => GLTextureTarget.Texture1DArray,
			TextureType.Texture2D => samples > 1 ? GLTextureTarget.Texture2DMultisample : GLTextureTarget.Texture2D,
			TextureType.Texture2DArray => samples > 1 ? GLTextureTarget.Texture2DMultisampleArray : GLTextureTarget.Texture2DArray,
			TextureType.Texture2DCube => GLTextureTarget.CubeMap,
			TextureType.Texture2DCubeArray => GLTextureTarget.CubeMapArray,
			_ => GLTextureTarget.Texture2D
		};

		public GLTexture(GL gl, TextureCreateInfo info) {
			GL = gl;
			Type = info.Type;
			Format = info.Format;
			Size = info.Size;
			MipLevels = info.MipLevels;
			ArrayLayers = info.ArrayLayers;
			Samples = info.Samples;

			GLFormat = GLEnums.StdToGLFormat(info.Format);
			if (GLFormat == null) throw new GLException("Unsupported pixel format");

			GL33 gl33 = gl.GL33;
			ID = gl33.GenTextures();
			GLTextureTarget target = GetGLTarget(Type, ArrayLayers, Samples);
			gl33.BindTexture(target, ID);
			if (gl.GL42 is GL42 gl42) {
				switch(Type) {
					case TextureType.Texture1D:
					case TextureType.Texture1DArray:
					case TextureType.Texture2D:
					case TextureType.Texture2DArray:
					case TextureType.Texture2DCube:
					case TextureType.Texture2DCubeArray:
					case TextureType.Texture3D:
						break;
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}



}
