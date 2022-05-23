using System;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// A texture layout determines how the content of a texture
	/// are internally stored and how it may be used in this layout.
	/// </summary>
	public enum TextureLayout {
		/// <summary>
		/// An undefined layout that is not usable for anything other than initialization.
		/// </summary>
		Undefined,
		/// <summary>
		/// A layout that may be used for any purpose, but may be less efficient than
		/// a specially-optimized format.
		/// </summary>
		General,
		/// <summary>
		/// A layout optimized for use as a color attachment.
		/// </summary>
		ColorAttachment,
		/// <summary>
		/// A layout optimized for use as a depth/stencil attachment.
		/// </summary>
		DepthStencilAttachment,
		/// <summary>
		/// A layout optimized for use as a depth/stencil attachment which will also be sampled.
		/// </summary>
		DepthStencilSampled,
		/// <summary>
		/// A layout optimized for use as a sampled image.
		/// </summary>
		ShaderSampled,
		/// <summary>
		/// A layout optimized as a source for plain image copies.
		/// </summary>
		TransferSrc,
		/// <summary>
		/// A layout optimized as a destination for plain image copies.
		/// </summary>
		TransferDst,
		/// <summary>
		/// A layout optimized as a source for presentation.
		/// </summary>
		PresentSrc
	}

	/// <summary>
	/// A bitmask of different aspects of a texture. Texture formats may have different
	/// aspects with distinct purposes such as color, depth, and stencil values.
	/// </summary>
	[Flags]
	public enum TextureAspect {
		/// <summary>
		/// The color aspect of a texture, including red, green, blue, and alpha channels.
		/// </summary>
		Color = 0x01,
		/// <summary>
		/// The depth aspect of a texture.
		/// </summary>
		Depth = 0x02,
		/// <summary>
		/// The stencil aspect of a texture.
		/// </summary>
		Stencil = 0x04
	}

	/// <summary>
	/// Enumeration of texture filtering modes.
	/// </summary>
	public enum TextureFilter {
		/// <summary>
		/// The value of the nearest texel is selected.
		/// </summary>
		Nearest,
		/// <summary>
		/// The values of the nearest adjacent texels are selected and linearly blended based on the
		/// position of the sampling relative to the postion of the texels.
		/// </summary>
		Linear
	}

	/// <summary>
	/// Enumeration of texture dimensions.
	/// </summary>
	public enum TextureDimension {
		/// <summary>
		/// 1-dimensional texture.
		/// </summary>
		Dim1D,
		/// <summary>
		/// 2-dimensional texture.
		/// </summary>
		Dim2D,
		/// <summary>
		/// 3-dimensional texture.
		/// </summary>
		Dim3D
	}

	/// <summary>
	/// Enumeration of types of textures.
	/// </summary>
	public enum TextureType {
		/// <summary>
		/// 1-dimensional texture.
		/// </summary>
		Texture1D,
		/// <summary>
		/// 1-dimensional array texture.
		/// </summary>
		Texture1DArray,
		/// <summary>
		/// 2-dimensional texture.
		/// </summary>
		Texture2D,
		/// <summary>
		/// 2-dimensional cubemap texture.
		/// </summary>
		Texture2DCube,
		/// <summary>
		/// 2-dimensional array texture.
		/// </summary>
		Texture2DArray,
		/// <summary>
		/// 2-dimensional cubemap array texture.
		/// </summary>
		Texture2DCubeArray,
		/// <summary>
		/// 3-dimensional texture.
		/// </summary>
		Texture3D
	}

	/// <summary>
	/// Bitmask of usages of textures.
	/// </summary>
	[Flags]
	public enum TextureUsage {
		/// <summary>
		/// The texture can be the source for transfer operations.
		/// </summary>
		TransferSrc = 0x0001,
		/// <summary>
		/// The texture can be the destination for transfer operations.
		/// </summary>
		TransferDst = 0x0002,
		/// <summary>
		/// The texture can be sampled by a shader program.
		/// </summary>
		Sampled = 0x0004,
		/// <summary>
		/// The texture can be used as a storage image by a shader program.
		/// </summary>
		Storage = 0x0008,
		/// <summary>
		/// The texture can be used as a color attachment.
		/// </summary>
		ColorAttachment = 0x0010,
		/// <summary>
		/// The texture can be used as a depth and/or stencil attachment.
		/// </summary>
		DepthStencilAttachment = 0x0020,
		/// <summary>
		/// The texture may use lazily allocated memory when used as an attachment.
		/// </summary>
		TransientAttachment = 0x0040,
		/// <summary>
		/// The texture can be used as an input attachment.
		/// </summary>
		InputAttachment = 0x0080,

		/// <summary>
		/// The texture can have sub-views created from it.
		/// </summary>
		SubView = 0x1000
	}

	/// <summary>
	/// A texture is a multidimensional structure for storing
	/// </summary>
	public interface ITexture : IDisposable, ISwapchainImage {

		/// <summary>
		/// Gets the dimensions of the given texture type.
		/// </summary>
		/// <param name="type">Texture type</param>
		/// <returns>Dimensions of texture type</returns>
		public static TextureDimension GetTypeDimension(TextureType type) => type switch {
			TextureType.Texture1D => TextureDimension.Dim1D,
			TextureType.Texture1DArray => TextureDimension.Dim2D,
			TextureType.Texture2D => TextureDimension.Dim2D,
			TextureType.Texture2DCube => TextureDimension.Dim3D,
			TextureType.Texture2DArray => TextureDimension.Dim3D,
			TextureType.Texture2DCubeArray => TextureDimension.Dim3D,
			TextureType.Texture3D => TextureDimension.Dim3D,
			_ => throw new ArgumentException($"No such dimension for texture type \"{type}\"", nameof(type))
		};

		/// <summary>
		/// The type of this texture.
		/// </summary>
		public TextureType Type { get; }

		/// <summary>
		/// The pixel format of this texture.
		/// </summary>
		public PixelFormat Format { get; }

		/// <summary>
		/// The size of this texture.
		/// </summary>
		public Vector3ui Size { get; }

		/// <summary>
		/// The number of mipmap levels in this texture.
		/// </summary>
		public uint MipLevels { get; }

		/// <summary>
		/// The number of array layers in this texture.
		/// </summary>
		public uint ArrayLayers { get; }

		/// <summary>
		/// The number of multisample samples the texture uses.
		/// </summary>
		public uint Samples { get; }

		/// <summary>
		/// The bitmask of usages for this texture.
		/// </summary>
		public TextureUsage Usage { get; }

		/// <summary>
		/// The memory binding for this texture.
		/// </summary>
		public IMemoryBinding? MemoryBinding { get; }

	}

	/// <summary>
	/// Texture creation information.
	/// </summary>
	public record TextureCreateInfo {

		/// <summary>
		/// The type of texture to create.
		/// </summary>
		public TextureType Type { get; init; }

		/// <summary>
		/// The pixel format of the texture.
		/// </summary>
		public PixelFormat Format { get; init; } = null!;

		/// <summary>
		/// The size of the texture.
		/// </summary>
		public Vector3ui Size { get; init; }

		/// <summary>
		/// The number of mipmap levels in the texture.
		/// </summary>
		public uint MipLevels { get; init; }

		/// <summary>
		/// The number of array layers in the texture. If the texture is a type of
		/// cubemap this must be a multiple of 6.
		/// </summary>
		public uint ArrayLayers { get; init; }

		/// <summary>
		/// The number of multisampling samples the texture will use.
		/// </summary>
		public uint Samples { get; init; }

		/// <summary>
		/// The initial layout of the texture.
		/// </summary>
		public TextureLayout InitialLayout { get; init; }

		/// <summary>
		/// Bitmask of usages expected for this texture.
		/// </summary>
		public TextureUsage Usage { get; init; }

		/// <summary>
		/// Explicit memory binding information for the texture, or <c>null</c> to let the backend
		/// decide how memory should be bound for the texture.
		/// </summary>
		public IMemoryBinding? MemoryBinding { get; init; }

	}

	/// <summary>
	/// Structure defining a texture subresource targeting a single mipmap level and a range of layers.
	/// </summary>
	public struct TextureSubresourceLayers {

		/// <summary>
		/// Bitmask of the texture aspects contained in this subresource.
		/// </summary>
		public TextureAspect Aspects;

		/// <summary>
		/// The mip level contained in this subresource.
		/// </summary>
		public uint MipLevel;

		/// <summary>
		/// The first array layer in the subresource.
		/// </summary>
		public uint BaseArrayLayer;

		/// <summary>
		/// The number of array layers in the subresource.
		/// </summary>
		public uint LayerCount;

	}

	/// <summary>
	/// Structure defining a texture subresource targeting a range of mipmap levels and layers.
	/// </summary>
	public struct TextureSubresourceRange {

		/// <summary>
		/// Bitmask of the texture aspects contained in this subresource.
		/// </summary>
		public TextureAspect Aspects;

		/// <summary>
		/// The first mip level in the subresource.
		/// </summary>
		public uint BaseMipLevel;

		/// <summary>
		/// The number of mip levels in this subresource.
		/// </summary>
		public uint MipLevelCount;

		/// <summary>
		/// The first array layer in the subresource.
		/// </summary>
		public uint BaseArrayLayer;

		/// <summary>
		/// The number of array layers in the subresource.
		/// </summary>
		public uint ArrayLayerCount;

	}

	/// <summary>
	/// A texture view provides a view of a subset of a texture, and is used to link a texture to other resources
	/// such as framebuffers and bind sets.
	/// </summary>
	public interface ITextureView : IDisposable {

		/// <summary>
		/// The type of texture viewed by the texture view. This may differ from the texture type of
		/// the underlying texture referenced by this view.
		/// </summary>
		public TextureType Type { get; }

		/// <summary>
		/// The pixel format the texture view uses. The format of the texture view may differ from that of the original
		/// texture, so long as the formats are binary compatible; each texel must be of the same size in both formats
		/// such that the memory locations for texels are not different between formats. Only the interpretation of
		/// the bits of each texel may change between the texture and its view.
		/// </summary>
		public PixelFormat Format { get; }

		/// <summary>
		/// A mapping of components as they are read from the format of the texture view.
		/// </summary>
		public ComponentMapping Mapping { get; }

		/// <summary>
		/// The subresource range of the source texture that the view provides.
		/// </summary>
		public TextureSubresourceRange SubresourceRange { get; }

	}

	/// <summary>
	/// Texture view creation information.
	/// </summary>
	public record TextureViewCreateInfo {

		/// <summary>
		/// The source texture for the texture view.
		/// </summary>
		public ITexture Texture { get; init; } = null!;

		/// <summary>
		/// The type of texture view to create.
		/// </summary>
		public TextureType Type { get; init; }

		/// <summary>
		/// The pixel format of the texture view.
		/// </summary>
		public PixelFormat Format { get; init; } = null!;

		/// <summary>
		/// The component mapping of the texture view.
		/// </summary>
		public ComponentMapping Mapping { get; init; }

		/// <summary>
		/// The subresource range of the texture view within the source texture.
		/// </summary>
		public TextureSubresourceRange SubresourceRange { get; init; }

	}

}
