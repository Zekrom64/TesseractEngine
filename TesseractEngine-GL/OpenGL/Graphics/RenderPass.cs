using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL.Graphics {

	public class GLRenderPass : IRenderPass, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;


		/// <summary>
		/// Operations to perform on an attachment at the beginning of a render pass.
		/// </summary>
		public struct AttachmentBegin {

			/// <summary>
			/// The load operation to perform for the color/depth component.
			/// </summary>
			public AttachmentLoadOp LoadOp { get; init; }

			/// <summary>
			/// The load operation to perform for the stencil component.
			/// </summary>
			public AttachmentLoadOp StencilLoadOp { get; init; }

		}

		/// <summary>
		/// Operations to perform on an attachment at the end of a render pass.
		/// </summary>
		public struct AttachmentEnd {

			/// <summary>
			/// The store operation to perform for the color/depth component.
			/// </summary>
			public AttachmentStoreOp StoreOp { get; init; }

			/// <summary>
			/// The store operation to perform for the stencil component.
			/// </summary>
			public AttachmentStoreOp StencilStoreOp { get; init; }

		}

		/// <summary>
		/// Metadata for a single subpass.
		/// </summary>
		public struct SubpassMetadata {

			/// <summary>
			/// The list of attachments to be invalidated at some point in this subpass.
			/// </summary>
			public uint[] AttachmentInvalidates { get; init; }

			/// <summary>
			/// <para>The list of resolve operations to perform between attachments</para>
			/// <para>
			/// When resolving 
			/// </para>
			/// </summary>
			public (uint, uint) AttachmentResolves { get; init; }

			/// <summary>
			/// If the subpass requires a glTextureBarrier call at the beginning (ie. it 
			/// </summary>
			public bool RequiresTextureBarrier { get; init; }

		}

		/// <summary>
		/// The list of operations to perform on each attachment at the beginning of the render pass.
		/// </summary>
		public AttachmentBegin[] AttachmentBegins { get; }
		/// <summary>
		/// The list of operations to perform on each attachment at the end of the render pass.
		/// </summary>
		public AttachmentEnd[] AttachmentEnds { get; }
		/// <summary>
		/// The list of subpasses in the render pass.
		/// </summary>
		public RenderPassSubpass[] Subpasses { get; }
		/// <summary>
		/// The list of metadata for each subpass.
		/// </summary>
		public SubpassMetadata[] SubpassMetadatas { get; }

		public GLRenderPass(GLGraphics graphics, RenderPassCreateInfo createInfo) {
			Graphics = graphics;
			AttachmentBegins = new AttachmentBegin[createInfo.Attachments.Count];
			AttachmentEnds = new AttachmentEnd[AttachmentBegins.Length];
			for (int i = 0; i < createInfo.Attachments.Count; i++) {
				var attachment = createInfo.Attachments[i];
				AttachmentBegins[i] = new AttachmentBegin {
					LoadOp = attachment.LoadOp,
					StencilLoadOp = attachment.StencilLoadOp
				};
				AttachmentEnds[i] = new AttachmentEnd {
					StoreOp = attachment.StoreOp,
					StencilStoreOp = attachment.StencilStoreOp
				};
			}
			Subpasses = createInfo.Subpasses.ConvertAll(subpass => subpass.DeepClone()).ToArray();
			SubpassMetadatas = Subpasses.ConvertAll(subpass => {
				return new SubpassMetadata() {

				};
			});
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}
}
