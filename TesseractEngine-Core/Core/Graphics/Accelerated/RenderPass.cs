using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	public readonly struct RenderPassAttachment {

		/// <summary>
		/// The format of the texture view that will be used by this attachment.
		/// </summary>
		public PixelFormat Format { get; init; }

		/// <summary>
		/// The number of samples that will be used with this attachment
		/// </summary>
		public uint Samples { get; init; }

		public AttachmentLoadOp LoadOp { get; init; }

		public AttachmentStoreOp StoreOp { get; init; }

		public AttachmentLoadOp StencilLoadOp { get; init; }

		public AttachmentStoreOp StencilStoreOp { get; init; }

		public TextureLayout InitialLayout { get; init; }

		public TextureLayout FinalLayout { get; init; }

	}

	/// <summary>
	/// An attachment reference specifies the details of how an attachment is used in a subpass.
	/// </summary>
	public readonly struct RenderPassAttachmentReference {

		public uint Attachment { get; init; }

		public TextureLayout Layout { get; init; }

	}

	public readonly struct RenderPassSubpass {

		/// <summary>
		/// The type of pipeline that will be bound.
		/// </summary>
		public PipelineType PipelineBindType { get; init; }

		/// <summary>
		/// The list of
		/// </summary>
		public RenderPassAttachmentReference[] InputAttachments { get; init; }

		public RenderPassAttachmentReference[] ColorAttachments { get; init; }

		public RenderPassAttachmentReference[] ResolveAttachments { get; init; }

		public RenderPassAttachmentReference? DepthStencilAttachment { get; init; }

		public RenderPassAttachmentReference[] PreserveAttachments { get; init; }

	}

	public readonly struct RenderPassDependency {

		public uint SrcSubpass { get; init; }

		public uint DstSubpass { get; init; }

		public PipelineStage SrcStages { get; init; }

		public PipelineStage DstStages { get; init; }

		public MemoryAccess SrcAccess { get; init; }

		public MemoryAccess DstAccess { get; init; }

	}

	public readonly ref struct RenderPassCreateInfo {

		public ReadOnlySpan<RenderPassAttachment> Attachments { get; init; }

		public ReadOnlySpan<RenderPassSubpass> Subpasses { get; init; }

		public ReadOnlySpan<RenderPassDependency> Dependencies { get; init; }

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
