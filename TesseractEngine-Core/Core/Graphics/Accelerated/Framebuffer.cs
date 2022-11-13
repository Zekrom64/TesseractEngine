using System;
using Tesseract.Core.Numerics;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// <para>
	/// A framebuffer stores a collection of texture attachments which can be rendered to. All of these
	/// attachments must be of the same 2D size, and any array texture attachments must have the same
	/// number of array layers.
	/// </para>
	/// </summary>
	public interface IFramebuffer : IDisposable, ISwapchainImage {

		/// <summary>
		/// The 2D size of the framebuffer.
		/// </summary>
		public Vector2i Size { get; }

		/// <summary>
		/// The number of layers if layered attachments are used.
		/// </summary>
		public uint Layers { get; }

	}

	/// <summary>
	/// Framebuffer creation information.
	/// </summary>
	public record FramebufferCreateInfo {

		/// <summary>
		/// The render pass the framebuffer will be used with.
		/// </summary>
		public required IRenderPass RenderPass { get; init; }

		/// <summary>
		/// The list of attachments to use with the framebuffer.
		/// </summary>
		public ITextureView[] Attachments { get; init; } = Array.Empty<ITextureView>();

		/// <summary>
		/// The 2D size of the framebuffer.
		/// </summary>
		public required Vector2i Size { get; init; }

		/// <summary>
		/// The number of layers the framebuffer will have.
		/// </summary>
		public required uint Layers { get; init; }

	}

}
