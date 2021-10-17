using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class VulkanTexture : ITexture {

		public VKImage Image { get; }

		public TextureType Type { get; init; }

		public PixelFormat Format { get; init; }

		public Vector3i Size { get; init; }

		public uint MipLevels { get; init; }

		public uint ArrayLayers { get; init; }

		public uint Samples { get; init; }

		public TextureUsage Usage { get; init; }

		public IMemoryBinding MemoryBinding { get; init; }

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

	public class VulkanTextureView : ITextureView {

		public VKImageView ImageView { get; }

		public TextureType Type { get; }

		public PixelFormat Format { get; }

		public ComponentMapping Mapping { get; }

		public TextureSubresourceRange SubresourceRange { get; }

		public VulkanTextureView(VKImageView imageView, TextureViewCreateInfo createInfo) {
			ImageView = imageView;
			Type = createInfo.Type;
			Format = createInfo.Format;
			Mapping = createInfo.Mapping;
			SubresourceRange = createInfo.SubresourceRange;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			ImageView.Dispose();
		}

	}

}
