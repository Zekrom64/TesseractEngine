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
	public struct RenderPassAttachment {

		/// <summary>
		/// The format of the texture view that will be used by this attachment.
		/// </summary>
		public PixelFormat Format { get; set; }

		/// <summary>
		/// The number of samples that will be used with this attachment
		/// </summary>
		public int Samples { get; set; }

		public AttachmentLoadOp LoadOp { get; set; }

		public AttachmentStoreOp StoreOp { get; set; }

		public AttachmentLoadOp StencilLoadOp { get; set; }

		public AttachmentStoreOp StencilStoreOp { get; set; }

		public TextureLayout InitialLayout { get; set; }

		public TextureLayout FinalLayout { get; set; }

	}

	/// <summary>
	/// An attachment reference specifies the details of how an attachment is used in a subpass.
	/// </summary>
	public struct RenderPassAttachmentReference {

		public uint Attachment { get; set; }

		public TextureLayout Layout { get; set; }

	}

	public struct RenderPassSubpass {

		/// <summary>
		/// The type of pipeline that will be bound.
		/// </summary>
		public PipelineType PipelineBindType { get; }

		/// <summary>
		/// The list of
		/// </summary>
		public RenderPassAttachmentReference[] InputAttachments { get; set; }

		public RenderPassAttachmentReference[] ColorAttachments { get; set; }

		public RenderPassAttachmentReference[] ResolveAttachments { get; set; }

		public RenderPassAttachmentReference? DepthStencilAttachment { get; set; }

		public RenderPassAttachmentReference[] PreserveAttachments { get; set; }

	}

	public struct RenderPassDependency {

		public uint SrcSubpass { get; set; }

		public uint DstSubpass { get; set; }

		public PipelineStage SrcStages { get; set; }

		public PipelineStage DstStages { get; set; }

	}

	public struct RenderPassCreateInfo {

		public RenderPassAttachment[] Attachments { get; set; }

		public RenderPassSubpass[] Subpasses { get; set; }

		public RenderPassDependency[] Dependencies { get; set; }

	}

}
