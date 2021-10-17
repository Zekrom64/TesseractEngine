using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.Core.Graphics.Accelerated {

	public interface IFramebuffer : IDisposable, ISwapchainImage {

		public Vector2i Size { get; }

		public uint Layers { get; }

	}

	public record FramebufferCreateInfo {

		public IRenderPass RenderPass { get; init; }

		public ITextureView[] Attachments { get; init; }

		public Vector2i Size { get; init; }

		public uint Layers { get; init; }

	}

}
