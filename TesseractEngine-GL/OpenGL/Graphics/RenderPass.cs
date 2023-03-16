using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL.Graphics {

	public class GLRenderPass : IRenderPass, IGLObject {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public IReadOnlyList<RenderPassAttachment> Attachments { get; }

		/// <summary>
		/// Metadata for a single subpass.
		/// </summary>
		internal class SubpassMetadata {

			/// <summary>
			/// The list of attachments to be invalidated at the beginning of this subpass.
			/// </summary>
			public List<GLFramebufferAttachment> PreInvalidateList { get; } = new List<GLFramebufferAttachment>();

			private GLFramebufferAttachment[]? preInvalidates = null;

			public GLFramebufferAttachment[] PreInvalidates => preInvalidates ??= PreInvalidateList.ToArray();

			/// <summary>
			/// The list of attachments to be invalidated at the end of this subpass.
			/// </summary>
			public List<GLFramebufferAttachment> PostInvalidateList { get; } = new List<GLFramebufferAttachment>();

			private GLFramebufferAttachment[]? postInvalidates = null;

			public GLFramebufferAttachment[] PostInvalidates => postInvalidates ??= PostInvalidateList.ToArray();

			/// <summary>
			/// <para>The list of resolve operations to perform from current framebuffer attachments to the given attachments by index at the end of the subpass</para>
			/// </summary>
			public List<(GLFramebufferAttachment, int)> Resolves { get; } = new List<(GLFramebufferAttachment, int)>();

		}
		
		/// <summary>
		/// The list of subpasses in the render pass.
		/// </summary>
		internal RenderPassSubpass[] Subpasses { get; }

		/// <summary>
		/// The list of metadata for each subpass.
		/// </summary>
		internal SubpassMetadata[] SubpassMetadatas { get; }

		/// <summary>
		/// The list of attachments within the framebuffer to clear at the start of the render pass.
		/// </summary>
		internal List<int> ClearAttachments { get; } = new List<int>();

		public GLRenderPass(GLGraphics graphics, RenderPassCreateInfo createInfo) {
			Graphics = graphics;
			Attachments = createInfo.Attachments;

			// Copy subpasses
			Subpasses = createInfo.Subpasses.ConvertAll(subpass => subpass.DeepClone()).ToArray();

			// Create array of subpass metadata
			SubpassMetadata[] metas = new SubpassMetadata[Subpasses.Length];
			for (int i = 0; i < metas.Length; i++) metas[i] = new SubpassMetadata();
			SubpassMetadatas = metas;

			// Track when attachments should be invalidated when marked with "don't care"
			Span<bool> loadInvalidates = stackalloc bool[createInfo.Attachments.Count];
			Span<bool> storeInvalidates = stackalloc bool[loadInvalidates.Length];
			Span<bool> stencilLoadInvalidates = stackalloc bool[loadInvalidates.Length];
			Span<bool> stencilStoreInvalidates = stackalloc bool[loadInvalidates.Length];

			// Iterate each attachment to check its load/store operations
			for (int i = 0; i < createInfo.Attachments.Count; i++) {
				var attachment = createInfo.Attachments[i];
				var format = attachment.Format;

				// If the attachment is marked with "don't care", mark it for invalidation, and track "clear" attachments
				bool shouldClear = false;
				if (format.HasChannel(ChannelType.Stencil)) {
					switch(attachment.StencilLoadOp) {
						case AttachmentLoadOp.Clear:
							shouldClear = true;
							break;
						case AttachmentLoadOp.DontCare:
							stencilLoadInvalidates[i] = true;
							break;
						default:
							break;
					}
					if (attachment.StencilStoreOp == AttachmentStoreOp.DontCare) stencilStoreInvalidates[i] = true;
				}
				switch(attachment.LoadOp) {
					case AttachmentLoadOp.Clear:
						shouldClear = true;
						break;
					case AttachmentLoadOp.DontCare:
						loadInvalidates[i] = true;
						break;
					default:
						break;
				}
				if (attachment.StoreOp == AttachmentStoreOp.DontCare) storeInvalidates[i] = true;

				if (shouldClear) ClearAttachments.Add(i);
			}

			static void TestInvalidation(RenderPassSubpass subpass, List<GLFramebufferAttachment> attachmentsToInvalidate, Span<bool> invalidates, Span<bool> stencilInvalidates) {
				// If the depth/stencil attachment is defined, test for invalidation
				if (subpass.DepthStencilAttachment != null) {
					int index = (int)subpass.DepthStencilAttachment.Value.Attachment;

					GLFramebufferAttachment? invalAttach = null;
					// Determine which attachment(s) to invalidate
					bool stencilInval = stencilInvalidates[index];
					if (invalidates[index]) {
						if (stencilInval) invalAttach = GLFramebufferAttachment.DepthStencil;
						else invalAttach = GLFramebufferAttachment.Depth;
					} else if (stencilInval) invalAttach = GLFramebufferAttachment.Stencil;

					// If an invalidation attachment is defined append to the list to invalidate
					if (invalAttach != null) attachmentsToInvalidate.Add(invalAttach.Value);

					// Clear invalidation flags
					invalidates[index] = false;
					stencilInvalidates[index] = false;
				}

				// If color attachment(s) are defined, test for load invalidation
				if (subpass.ColorAttachments != null) {
					for (int j = 0; j < subpass.ColorAttachments.Length; j++) {
						int index = (int)subpass.ColorAttachments[j].Attachment;
					}
				}
			}

			// Iterate each subpass forward
			for(int i = 0; i < Subpasses.Length; i++) {
				var subpass = Subpasses[i];

				// If a list of multisample color resolves is defined for the subpass, iterate each resolve
				if (subpass.ResolveAttachments != null) {
					for (int j = 0; j < subpass.ResolveAttachments.Length; j++) {
						// Add the resolve to the list for this subpass, mapping the color attachment from the subpass' framebuffer to the resolve attachment's index
						var resolve = subpass.ResolveAttachments[j];
						metas[i].Resolves.Add((GL30.GetColorAttachment(j), (int)resolve.Attachment));
					}
				}

				// Test load invalidates while going forward through the subpass list
				TestInvalidation(subpass, metas[i].PreInvalidateList, loadInvalidates, stencilLoadInvalidates);
			}

			// Iterate each subpass backward
			for(int i = Subpasses.Length - 1; i >= 0; i--) {
				var subpass = Subpasses[i];

				// Test store invalidates while going backward through the subpass list
				TestInvalidation(subpass, metas[i].PostInvalidateList, storeInvalidates, stencilStoreInvalidates);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}
}
