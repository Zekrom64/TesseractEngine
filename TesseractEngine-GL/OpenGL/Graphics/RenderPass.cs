using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Util;

namespace Tesseract.OpenGL.Graphics {

	public class GLRenderPass : IRenderPass, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public struct AttachmentBegin {

			public AttachmentLoadOp LoadOp { get; init; }
			public AttachmentLoadOp StencilLoadOp { get; init; }

		}

		/// <summary>
		/// Operations to perform on an attachment at the end of a render pass.
		/// </summary>
		public struct AttachmentEnd {

			public AttachmentStoreOp StoreOp { get; init; }
			public AttachmentStoreOp StencilStoreOp { get; init; }

		}

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
			AttachmentBegins = new AttachmentBegin[createInfo.Attachments.Length];
			AttachmentEnds = new AttachmentEnd[AttachmentBegins.Length];
			for (int i = 0; i < createInfo.Attachments.Length; i++) {
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
