using System;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {

	public interface IFramebuffer : IDisposable, ISwapchainImage {

		public Vector2i Size { get; }

		public uint Layers { get; }

	}

	public record FramebufferCreateInfo {

		public IRenderPass RenderPass { get; init; } = null!;

		public ITextureView[] Attachments { get; init; } = Array.Empty<ITextureView>();

		public Vector2i Size { get; init; }

		public uint Layers { get; init; }

	}

}
