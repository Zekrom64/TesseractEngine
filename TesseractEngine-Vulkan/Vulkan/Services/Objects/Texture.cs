using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;

namespace Tesseract.Vulkan.Services.Objects {

	/// <summary>
	/// Vulkan texture implementation.
	/// </summary>
	public class VulkanTexture : ITexture {

		private readonly VulkanGraphics graphics;

		/// <summary>
		/// The underlying Vulkan image.
		/// </summary>
		public VKImage Image { get; }

		public TextureType Type { get; init; }

		public required PixelFormat Format { get; init; }

		public Vector3ui Size { get; init; }

		public uint MipLevels { get; init; }

		public uint ArrayLayers { get; init; }

		public uint Samples { get; init; }

		public TextureUsage Usage { get; init; }

		public IVKMemoryBinding? MemoryBinding { get; init; }

		IMemoryBinding? ITexture.MemoryBinding => MemoryBinding;

		private ITextureView? identityView = null;

		public ITextureView IdentityView {
			get {
				identityView ??= graphics.CreateTextureView(new TextureViewCreateInfo() {
					Texture = this,
					Type = Type,
					Format = Format,
					Mapping = new ComponentMapping(),
					SubresourceRange = new TextureSubresourceRange() {
						Aspects = Format.Aspects,
						ArrayLayerCount = ArrayLayers,
						MipLevelCount = MipLevels
					}
				});
				return identityView;
			}
		}

		private readonly bool disposable;

		public VulkanTexture(VulkanGraphics graphics, VKImage image, bool disposable) {
			this.graphics = graphics;
			Image = image;
			this.disposable = disposable;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			identityView?.Dispose();
			if (disposable) {
				Image.Dispose();
				MemoryBinding?.Dispose();
			}
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
