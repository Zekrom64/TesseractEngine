using System;
using System.Collections.Generic;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// Enumeration of operations to perform on an attachment at the start of a subpass.
	/// </summary>
	public enum AttachmentLoadOp {
		/// <summary>
		/// The area bound to the attachment is preserved from previous operations.
		/// </summary>
		Load,
		/// <summary>
		/// The area bound to the attachment is cleared to a value specified at the start of a render pass.
		/// </summary>
		Clear,
		/// <summary>
		/// The area bound to the attachment may be discarded.
		/// </summary>
		DontCare
	}

	/// <summary>
	/// Enumeration of operations to perform on an attachment at the end of a subpass.
	/// </summary>
	public enum AttachmentStoreOp {
		/// <summary>
		/// The area bound to the attachment is preserved for future operations.
		/// </summary>
		Store,
		/// <summary>
		/// The area bound to the attachment may be discarded.
		/// </summary>
		DontCare
	}

	/// <summary>
	/// <para>A render pass manages framebuffer and attachment state during rendering.</para>
	/// </summary>
	public interface IRenderPass : IDisposable { }

	/// <summary>
	/// A render pass attachment describes layout and load/store for an image that will be attached to a framebuffer.
	/// </summary>
	public record struct RenderPassAttachment {

		/// <summary>
		/// The format of the texture view that will be used by this attachment.
		/// </summary>
		public PixelFormat Format { get; init; } = null!;

		/// <summary>
		/// The number of samples that will be used with this attachment
		/// </summary>
		public uint Samples { get; init; } = 1;

		/// <summary>
		/// The load operation that will be performed on the color/depth attachment at the start of the render pass.
		/// </summary>
		public AttachmentLoadOp LoadOp { get; init; } = AttachmentLoadOp.DontCare;

		/// <summary>
		/// The store operation that will be performed on the color/depth attachment at the end of the render pass.
		/// </summary>
		public AttachmentStoreOp StoreOp { get; init; } = AttachmentStoreOp.DontCare;

		/// <summary>
		/// The load operation that will be performed on the stencil attachment at the start of the render pass.
		/// </summary>
		public AttachmentLoadOp StencilLoadOp { get; init; } = AttachmentLoadOp.DontCare;

		/// <summary>
		/// The store operation that will be performed on the stencil attachment at the end of the render pass.
		/// </summary>
		public AttachmentStoreOp StencilStoreOp { get; init; } = AttachmentStoreOp.DontCare;

		/// <summary>
		/// The inital layout of the attachment's underlying texture at the start of the render pass.
		/// </summary>
		public TextureLayout InitialLayout { get; init; } = default;

		/// <summary>
		/// The final layout of the attachment's underlying texture at the end of the render pass.
		/// </summary>
		public TextureLayout FinalLayout { get; init; } = default;

		public RenderPassAttachment() { }

	}

	/// <summary>
	/// An attachment reference specifies the details of how an attachment is used in a subpass.
	/// </summary>
	public record struct RenderPassAttachmentReference {

		/// <summary>
		/// Attachment index for an unused attachment.
		/// </summary>
		public const uint Unused = 0xFFFFFFFFu;

		/// <summary>
		/// Index of the referenced attachment in the render pass.
		/// </summary>
		public uint Attachment { get; init; } = Unused;

		/// <summary>
		/// The layout of the attachment while it is in use by the current subpass.
		/// </summary>
		public TextureLayout Layout { get; init; } = default;

		/// <summary>
		/// If the referenced attachment is unused.
		/// </summary>
		public bool IsUnused => Attachment == Unused;

		public RenderPassAttachmentReference() { }

	}

	/// <summary>
	/// A subpass describes a collection of attachments to use when rendering.
	/// </summary>
	public record struct RenderPassSubpass {

		/// <summary>
		/// The type of pipeline that will be bound.
		/// </summary>
		public PipelineType PipelineBindType { get; init; } = PipelineType.Graphics;

		/// <summary>
		/// The list of input attachments for this subpass.
		/// </summary>
		public RenderPassAttachmentReference[]? InputAttachments { get; init; } = null;

		/// <summary>
		/// The list of color attachments for this subpass.
		/// </summary>
		public RenderPassAttachmentReference[]? ColorAttachments { get; init; } = null;

		/// <summary>
		/// The list of resolve attachments for this subpass.
		/// </summary>
		public RenderPassAttachmentReference[]? ResolveAttachments { get; init; } = null;

		/// <summary>
		/// The depth/stencil attachment for this subpass.
		/// </summary>
		public RenderPassAttachmentReference? DepthStencilAttachment { get; init; } = null;

		/// <summary>
		/// The list of preserve attachments for this subpass.
		/// </summary>
		public RenderPassAttachmentReference[]? PreserveAttachments { get; init; } = null;

		/// <summary>
		/// Creates a deep clone of this object.
		/// </summary>
		/// <returns>Deep cloned object</returns>
		public RenderPassSubpass DeepClone() => new() {
			PipelineBindType = PipelineBindType,
			InputAttachments = InputAttachments?.ShallowClone(),
			ColorAttachments = ColorAttachments?.ShallowClone(),
			ResolveAttachments = ResolveAttachments?.ShallowClone(),
			DepthStencilAttachment = DepthStencilAttachment,
			PreserveAttachments = PreserveAttachments?.ShallowClone()
		};

		public RenderPassSubpass() { }

	}

	/// <summary>
	/// A dependency describes a barrier to apply between subpasses that have inter-dependent attachment usages.
	/// </summary>
	public record struct RenderPassDependency {

		/// <summary>
		/// Index for specifying that a dependency is external to the current render pass.
		/// </summary>
		public const uint External = 0xFFFFFFFFu;

		/// <summary>
		/// The first subpass in the dependency.
		/// </summary>
		public uint SrcSubpass { get; init; } = External;

		/// <summary>
		/// The second subpass in the dependency.
		/// </summary>
		public uint DstSubpass { get; init; } = External;

		/// <summary>
		/// The pipeline stages initiating the dependency.
		/// </summary>
		public PipelineStage SrcStages { get; init; } = default;

		/// <summary>
		/// The pipeline stages awaiting on the dependency.
		/// </summary>
		public PipelineStage DstStages { get; init; } = default;

		/// <summary>
		/// The memory accesses initiating the dependency.
		/// </summary>
		public MemoryAccess SrcAccess { get; init; } = default;

		/// <summary>
		/// The memory accesses awaiting the dependency.
		/// </summary>
		public MemoryAccess DstAccess { get; init; } = default;

		public RenderPassDependency() { }

	}

	/// <summary>
	/// Render pass creation information.
	/// </summary>
	public record RenderPassCreateInfo {

		/// <summary>
		/// The list of attachments used by this render pass.
		/// </summary>
		public IReadOnlyList<RenderPassAttachment> Attachments { get; init; } = Array.Empty<RenderPassAttachment>();

		/// <summary>
		/// The list of subpasses contained in this render pass.
		/// </summary>
		public IReadOnlyList<RenderPassSubpass> Subpasses { get; init; } = Array.Empty<RenderPassSubpass>();

		/// <summary>
		/// The list of dependencies in this render pass.
		/// </summary>
		public IReadOnlyList<RenderPassDependency> Dependencies { get; init; } = Array.Empty<RenderPassDependency>();

	}

	/// <summary>
	/// Enumeration of types of subpass contents.
	/// </summary>
	public enum SubpassContents {
		/// <summary>
		/// The contents of the subpass are recorded inline in the command stream.
		/// </summary>
		Inline,
		/// <summary>
		/// The contents of the subpass are recorded in secondary command buffers that are executed from the command stream.
		/// </summary>
		SecondaryCommandBuffers
	}

}
