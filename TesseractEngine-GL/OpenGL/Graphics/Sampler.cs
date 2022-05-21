using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL.Graphics {
	
	public class GLSampler : ISampler, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public uint ID { get; }

		public GLSampler(GLGraphics graphics, SamplerCreateInfo createInfo) {
			Graphics = graphics;
			ID = Graphics.Interface.CreateSampler();
			var gl33 = GL.GL33!;

			gl33.SamplerParameter(ID, GLSamplerParameter.MagFilter, (int)GLEnums.Convert(createInfo.MagnifyFilter));
			uint minfilter = createInfo.MinifyFilter switch {
				TextureFilter.Nearest => createInfo.MipmapMode switch {
					TextureFilter.Nearest => Native.GLEnums.GL_NEAREST_MIPMAP_NEAREST,
					TextureFilter.Linear => Native.GLEnums.GL_NEAREST_MIPMAP_LINEAR,
					_ => default
				},
				TextureFilter.Linear => createInfo.MipmapMode switch {
					TextureFilter.Nearest => Native.GLEnums.GL_LINEAR_MIPMAP_NEAREST,
					TextureFilter.Linear => Native.GLEnums.GL_LINEAR_MIPMAP_LINEAR,
					_ => default
				},
				_ => default
			};
			gl33.SamplerParameter(ID, GLSamplerParameter.WrapS, (int)GLEnums.Convert(createInfo.AddressMode.X));
			gl33.SamplerParameter(ID, GLSamplerParameter.WrapT, (int)GLEnums.Convert(createInfo.AddressMode.Y));
			gl33.SamplerParameter(ID, GLSamplerParameter.WrapR, (int)GLEnums.Convert(createInfo.AddressMode.Z));
			gl33.SamplerParameter(ID, GLSamplerParameter.LODBias, createInfo.MipLODBias);
			if (createInfo.AnisotropyEnable) gl33.SamplerParameter(ID, GLSamplerParameter.MaxAnisotropy, createInfo.MaxAnisotropy);
			if (createInfo.CompareEnable) {
				gl33.SamplerParameter(ID, GLSamplerParameter.CompareMode, (int)Native.GLEnums.GL_COMPARE_REF_TO_TEXTURE);
				gl33.SamplerParameter(ID, GLSamplerParameter.CompareFunc, (int)GLEnums.Convert(createInfo.CompareOp));
			}
			gl33.SamplerParameter(ID, GLSamplerParameter.MinLOD, createInfo.LODRange.Item1);
			gl33.SamplerParameter(ID, GLSamplerParameter.MaxLOD, createInfo.LODRange.Item2);
			switch(createInfo.BorderColor) {
				case SamplerBorderColor.CustomNorm:
				case SamplerBorderColor.CustomInt: {
					var color = createInfo.CustomBorderColor;
					if (color is Vector4 v4f) gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc float[] { v4f.X, v4f.Y, v4f.Z, v4f.W });
					else if (color is ITuple4<float> t4f) gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc float[] { t4f.X, t4f.Y, t4f.Z, t4f.W });
					else if (color is ITuple4<int> t4i) gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc int[] { t4i.X, t4i.Y, t4i.Z, t4i.W });
					else if (color is ITuple4<uint> t4u) gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc int[] { (int)t4u.X, (int)t4u.Y, (int)t4u.Z, (int)t4u.W });
				} break;
				case SamplerBorderColor.OpaqueBlackNorm:
				case SamplerBorderColor.OpaqueBlackInt:
					gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc float[] { 0.0f, 0.0f, 0.0f, 1.0f });
					break;
				case SamplerBorderColor.OpaqueWhiteNorm:
				case SamplerBorderColor.OpaqueWhiteInt:
					gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc float[] { 1.0f, 1.0f, 1.0f, 1.0f });
					break;
				case SamplerBorderColor.TransparentBlackNorm:
				case SamplerBorderColor.TransparentBlackInt:
					gl33.SamplerParameter(ID, GLSamplerParameter.BorderColor, stackalloc float[] { 0.0f, 0.0f, 0.0f, 0.0f });
					break;
				default:
					break;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteSamplers(ID);
		}

	}
}
