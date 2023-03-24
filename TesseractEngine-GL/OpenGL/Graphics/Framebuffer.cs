using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Engine;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL.Graphics {

	/// <summary>
	/// An OpenGL framebuffer object.
	/// </summary>
	public class GLFramebuffer : IFramebuffer, IGLObject {
	
		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		public Vector2i Size { get; }

		public uint Layers { get; }

		/// <summary>
		/// The render pass the framebuffer was created with.
		/// </summary>
		internal GLRenderPass? RenderPass { get; }

		/// <summary>
		/// The texture views for the attachments of the framebuffer, or null if none exist.
		/// </summary>
		internal GLTextureView[]? AttachmentViews { get; }

		// The list of OpenGL framebuffer objects corresponding to each subpass in the associated render pass.
		internal uint[] IDs { get; }

		// A "transient" OpenGL framebuffer with attachments manually managed when used with a different render pass
		// Default null when uninitialized, set to 0 and never reassigned if this is the default framebuffer
		private uint? transientID = null;

		/// <summary>
		/// If the framebuffer is the 'default' OpenGL framebuffer.
		/// </summary>
		internal bool IsDefault => transientID == 0;

		/// <summary>
		/// The Framebuffer Object ID of the currently used framebuffer. This will vary between render passes.
		/// If the framebuffer is not currently bound this will return 0.
		/// </summary>
		internal uint CurrentID {
			get {
				if (Graphics.State.CurrentFramebuffer != this) return 0;
				if (Graphics.State.CurrentRenderPass != RenderPass) return transientID!.Value;
				return IDs[Graphics.State.CurrentSubpass];
			}
		}

		// List of extra framebuffer IDs used during operations
		private List<uint> extraIDs = new();

		/// <summary>
		/// The list of framebuffer IDs and OpenGL attachments to be able to clear the attachments in this framebuffer.
		/// </summary>
		internal (uint ID, GLFramebufferAttachment Attachment)[] AttachmentClearFramebuffers { get; }

		public GLFramebuffer(GLGraphics graphics, Vector2i size) {
			Graphics = graphics;
			Size = size;
			Layers = 1;
			IDs = Array.Empty<uint>();
			transientID = 0;
			AttachmentClearFramebuffers = new (uint ID, GLFramebufferAttachment Attachment)[] {
				(0, GLFramebufferAttachment.Color0),
				(0, GLFramebufferAttachment.DepthStencil)
			};
		}

		public GLFramebuffer(GLGraphics graphics, FramebufferCreateInfo createInfo) {
			Graphics = graphics;
			Size = createInfo.Size;
			Layers = createInfo.Layers;
			RenderPass = (GLRenderPass)createInfo.RenderPass;
			AttachmentViews = createInfo.Attachments.ConvertAll(v1 => (GLTextureView)v1);
			IDs = new uint[RenderPass.Subpasses.Length];
			AttachmentClearFramebuffers = new (uint ID, GLFramebufferAttachment Attachment)[createInfo.Attachments.Length];

			var iface = Graphics.Interface;

			// Create subpass framebuffers
			iface.CreateFramebuffers(IDs);

			// Set attachments for each subpass
			for (int i = 0; i < IDs.Length; i++)
				SetAttachmentsForSubpass(IDs[i], RenderPass.Subpasses[i]);
		}

		private void SetAttachmentsForSubpass(uint fbo, RenderPassSubpass subpass) {
			var iface = Graphics.Interface;
			// If has color attachments
			if (subpass.ColorAttachments != null) {
				for (int j = 0; j < subpass.ColorAttachments.Length; j++) {
					// For each color attachment that is used
					var attachment = subpass.ColorAttachments[j];
					if (!attachment.IsUnused) {
						// Set the appropriate color attachment
						var view = AttachmentViews![attachment.Attachment];
						var attachPoint = GL30.GetColorAttachment(j);
						iface.FramebufferTexture(fbo, attachPoint, view, 0, 0);
						// Map the clear framebuffer
						var (clearID, _) = AttachmentClearFramebuffers[attachment.Attachment];
						if (clearID == 0) AttachmentClearFramebuffers[attachment.Attachment] = (fbo, attachPoint);
					}
				}
			}
			// If has depth/stencil attachment
			if (subpass.DepthStencilAttachment.HasValue) {
				var attachment = subpass.DepthStencilAttachment.Value;
				if (!attachment.IsUnused) {
					var view = AttachmentViews![attachment.Attachment];
					var fmt = view.Format;
					// Set depth/stencil attachment if format has respective component
					if (fmt.HasChannel(ChannelType.Depth)) iface.FramebufferTexture(fbo, GLFramebufferAttachment.Depth, view, 0, 0);
					if (fmt.HasChannel(ChannelType.Stencil)) iface.FramebufferTexture(fbo, GLFramebufferAttachment.Stencil, view, 0, 0);
					// Map the clear framebuffer
					var (clearID, _) = AttachmentClearFramebuffers[attachment.Attachment];
					if (clearID == 0) AttachmentClearFramebuffers[attachment.Attachment] = (fbo, GLFramebufferAttachment.DepthStencil);

				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var gl33 = GL.GL33!;

			gl33.DeleteFramebuffers(IDs);
			if (transientID.HasValue && transientID.Value != 0) gl33.DeleteFramebuffers(transientID.Value);
		}

		// TODO
		/// <summary>
		/// Begins a render pass using this framebuffer.
		/// </summary>
		internal void BeginRenderPass(ICommandSink.RenderPassBegin beginInfo) {
			var state = Graphics.State;
			var renderPass = state.CurrentRenderPass;
			if (renderPass == null) return;

			var iface = Graphics.Interface;
			// If default framebuffer, just bind immediately
			if (IsDefault) {
				state.BindFramebuffer(GLFramebufferTarget.Framebuffer, 0);
			} else {
				// Else check if a transient framebuffer is needed and begin the subpass
				if (renderPass != RenderPass) {
					transientID ??= iface.CreateFramebuffer();
					SetAttachmentsForSubpass(transientID.Value, renderPass.Subpasses[0]);
				}
			}

			if (renderPass.ClearAttachments.Count > 0) {
				state.SetTempScissor(beginInfo.RenderArea);
				// For each attachment to clear in the render pass
				foreach (int clearAttachment in renderPass.ClearAttachments) {
					// Get the clear mapping
					var (clearID, clearFBAttachment) = AttachmentClearFramebuffers[clearAttachment];
					// Determine the pixel format of the clear value
					PixelFormat format = PixelFormat.R32G32B32A32SFloat;
					if (AttachmentViews != null) format = AttachmentViews[clearAttachment].Format;
					else if (clearAttachment == 1) format = PixelFormat.D32SFloatS8UInt;
					// Clear the mapped framebuffer for the attachment
					iface.ClearFramebuffer(clearID, clearFBAttachment, beginInfo.ClearValues[clearAttachment], format);
				}
				state.UnsetTempScissor();
			}
		}

		/// <summary>
		/// Begins a subpass within the current.
		/// </summary>
		internal void BeginSubpass() {
			var state = Graphics.State;
			var iface = Graphics.Interface;
			var renderPass = state.CurrentRenderPass;
			if (renderPass == null) return;
			var subpass = state.CurrentSubpass;

			// If default framebuffer, skip binding
			if (!IsDefault) {
				// Bind the corresponding framebuffer as required, or set attachments for the transient framebuffer
				if (renderPass == RenderPass) {
					Graphics.State.BindFramebuffer(GLFramebufferTarget.Framebuffer, IDs[subpass]);
				} else {
					SetAttachmentsForSubpass(transientID!.Value, renderPass.Subpasses[subpass]);
				}
			}

			// Perform invalidation as required
			var invalidates = renderPass.SubpassMetadatas[subpass].PreInvalidates;
			if (invalidates.Length > 0) iface.InvalidateSubFramebuffer(state.DrawFramebuffer, invalidates, state.CurrentRenderArea);
		}

		/// <summary>
		/// Ends a subpass within the current.
		/// </summary>
		/// <param name="renderPass"></param>
		/// <param name="subpass"></param>
		internal void EndSubpass() {
			var state = Graphics.State;
			var iface = Graphics.Interface;
			var renderPass = Graphics.State.CurrentRenderPass;
			if (renderPass == null) return;
			var subpass = state.CurrentSubpass;

			// Perform invalidation as required
			var invalidates = renderPass.SubpassMetadatas[subpass].PostInvalidates;
			if (invalidates.Length > 0) iface.InvalidateSubFramebuffer(state.DrawFramebuffer, invalidates, state.CurrentRenderArea);
		}

		/// <summary>
		/// Ends a render pass using this framebuffer.
		/// </summary>
		internal void EndRenderPass() {
			// No-op for now
		}

		internal static GLFramebufferAttachment GetBaseAttachment(TextureAspect aspect) {
			// Always attach color to Color0
			if ((aspect & TextureAspect.Color) != 0) return GLFramebufferAttachment.Color0;
			// Determine depth/stencil attachment
			bool depth = (aspect & TextureAspect.Depth) != 0;
			bool stencil = (aspect & TextureAspect.Stencil) != 0;
			if (depth && stencil) return GLFramebufferAttachment.DepthStencil;
			else if (depth) return GLFramebufferAttachment.Depth;
			else if (stencil) return GLFramebufferAttachment.Stencil;
			// Else invalid combination of aspects
			else throw new ArgumentException("Invalid texture aspect combination", nameof(aspect));
		}

		internal static GLFramebufferAttachment GetBaseAttachment(GLBufferMask aspect) {
			// Always attach color to Color0
			if ((aspect & GLBufferMask.Color) != 0) return GLFramebufferAttachment.Color0;
			// Determine depth/stencil attachment
			bool depth = (aspect & GLBufferMask.Depth) != 0;
			bool stencil = (aspect & GLBufferMask.Stencil) != 0;
			if (depth && stencil) return GLFramebufferAttachment.DepthStencil;
			else if (depth) return GLFramebufferAttachment.Depth;
			else if (stencil) return GLFramebufferAttachment.Stencil;
			// Else invalid combination of aspects
			else throw new ArgumentException("Invalid texture aspect combination", nameof(aspect));
		}

		internal (uint ID, GLFramebufferAttachment Attachment) GetFBOForAttachment(int attachment, TextureAspect aspect, int arrayLayer = 0) {
			// If default FB, use the base attachment
			if (transientID == 0) return (0, GetBaseAttachment(aspect));

			// Use a transient framebuffer and bind the required attachment
			var iface = Graphics.Interface;
			transientID ??= iface.CreateFramebuffer();
			GLFramebufferAttachment glattach = GetBaseAttachment(aspect);
			Graphics.SetAttachmentsForAspect(transientID.Value, AttachmentViews![attachment], aspect, 0, arrayLayer);
			return (transientID.Value, glattach);
		}

	}

}
