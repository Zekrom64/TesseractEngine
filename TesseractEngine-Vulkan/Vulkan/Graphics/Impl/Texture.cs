using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;

namespace Tesseract.Vulkan.Graphics.Impl {

	/// <summary>
	/// Vulkan texture implementation.
	/// </summary>
	public class VulkanTexture : ITexture {

		/// <summary>
		/// The underlying Vulkan image.
		/// </summary>
		public VKImage Image { get; }

		public TextureType Type { get; init; }

		public PixelFormat Format { get; init; } = null!;

		public Vector3ui Size { get; init; }

		public uint MipLevels { get; init; }

		public uint ArrayLayers { get; init; }

		public uint Samples { get; init; }

		public TextureUsage Usage { get; init; }

		public IMemoryBinding MemoryBinding { get; init; } = null!;

		private readonly bool disposable;

		public VulkanTexture(VKImage image, bool disposable) {
			Image = image;
			this.disposable = disposable;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			if (disposable) Image.Dispose();
		}

	}

	/// <summary>
	/// Vulkan texture view implementation.
	/// </summary>
	public class VulkanTextureView : ITextureView {

		/// <summary>
		/// The underlying Vulkan texture view.
		/// </summary>
		public VKImageView ImageView { get; }

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public ComponentMapping Mapping { get; }

		public TextureSubresourceRange SubresourceRange { get; }

		internal readonly VulkanTexture Texture;

		public VulkanTextureView(VKImageView imageView, TextureViewCreateInfo createInfo) {
			ImageView = imageView;
			Type = createInfo.Type;
			Format = createInfo.Format;
			Mapping = createInfo.Mapping;
			SubresourceRange = createInfo.SubresourceRange;

			Texture = (VulkanTexture)createInfo.Texture;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ImageView.Dispose();
		}

	}

}
