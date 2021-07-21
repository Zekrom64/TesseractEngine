using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {
	
	/// <summary>
	/// A texture layout determines how the content of a texture 
	/// </summary>
	public enum TextureLayout {
		Undefined,
		General,
		ColorAttachment,
		DepthStencilAttachment,
		DepthStencilSampled,
		ShaderSampled,
		TransferSrc,
		TransferDst
	}

	public enum TextureAspect {
		Color = 0x01,
		Depth = 0x02,
		Stencil = 0x04
	}

	public enum TextureFilter {
		Nearest,
		Linear
	}

	public enum TextureDimension {
		Dim1D,
		Dim2D,
		Dim3D
	}

	public enum TextureType {
		Texture1D,
		Texture1DArray,
		Texture2D,
		Texture2DCube,
		Texture2DArray,
		Texture2DCubeArray,
		Texture3D
	}

	public enum TextureUsage {
		TransferSrc = 0x0001,
		TransferDst = 0x0002,
		Sampled = 0x0004,
		Storage = 0x0008,
		ColorAttachment = 0x0010,
		DepthStencilAttachment = 0x0020,
		TransientAttachment = 0x0040,
		InputAttachment = 0x0080
	}

	public interface ITexture : IDisposable {

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public Vector3i Size { get; }

		public uint MipLevels { get; }

		public uint ArrayLayers { get; }

		public uint Samples { get; }

		public IMemoryBinding MemoryBinding { get; }

	}

	public record TextureCreateInfo {

		public TextureType Type { get; init; }

		public PixelFormat Format { get; init; }

		public Vector3i Size { get; init; }

		public uint MipLevels { get; init; }

		public uint ArrayLayers { get; init; }

		public uint Samples { get; init; }

		public TextureLayout InitialLayout { get; init; }

		/// <summary>
		/// Explicit memory binding information for the texture, or <c>null</c> to let the backend
		/// decide how memory should be bound for the texture.
		/// </summary>
		public IMemoryBinding MemoryBinding { get; init; }

	}

	public struct TextureSubresourceLayers {

		public TextureAspect Aspects;

		public uint MipLevel;

		public uint BaseArrayLayer;

		public uint LayerCount;

	}

	public struct TextureSubresourceRange {

		public TextureAspect Aspects;

		public uint BaseMipLevel;

		public uint LevelCount;

		public uint BaseArrayLayer;

		public uint ArrayLayerCount;

	}

	public interface ITextureView : IDisposable {

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public ComponentMapping Mapping { get; }

		public TextureSubresourceRange SubresourceRange { get; }

	}

	public record TextureViewCreateInfo {

		public ITexture Texture { get; init; }

		public TextureType Type { get; init; }

		public PixelFormat Format { get; init; }

		public ComponentMapping Mapping { get; init; }

		public TextureSubresourceRange SubresourceRange { get; init; }

	}

}
