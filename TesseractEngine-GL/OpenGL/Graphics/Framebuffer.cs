using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Util;

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
		public GLRenderPass? RenderPass { get; }

		private GLTextureView[]? AttachmentViews { get; }

		// The list of OpenGL framebuffer objects corresponding to each subpass in the associated render pass.
		private uint[] IDs { get; }

		// A "transient" OpenGL framebuffer with attachments manually managed when used with a different render pass
		// Default null when uninitialized, set to 0 and never reassigned if this is the default framebuffer
		private uint? transientID = null;

		/// <summary>
		/// The Framebuffer Object ID of the currently used framebuffer. This will vary between render passes.
		/// </summary>
		public uint CurrentID {
			get {
				if (Graphics.State.CurrentFramebuffer != this) return 0;
				if (Graphics.State.CurrentRenderPass != RenderPass) return transientID!.Value;
				return IDs[Graphics.State.CurrentSubpass];
			}
		}

		public GLFramebuffer(GLGraphics graphics, Vector2i size) {
			Graphics = graphics;
			Size = size;
			Layers = 1;
			IDs = Array.Empty<uint>();
			transientID = 0;
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
						iface.SetFramebufferAttachment(fbo, GL30.GetColorAttachment(j), view);
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
					if (fmt.HasChannel(ChannelType.Depth)) iface.SetFramebufferAttachment(fbo, GLFramebufferAttachment.Depth, view);
					if (fmt.HasChannel(ChannelType.Stencil)) iface.SetFramebufferAttachment(fbo, GLFramebufferAttachment.Stencil, view);
				}
			}
		}

		public GLFramebuffer(GLGraphics graphics, FramebufferCreateInfo createInfo) {
			Graphics = graphics;
			Size = createInfo.Size;
			Layers = createInfo.Layers;
			RenderPass = (GLRenderPass)createInfo.RenderPass;
			AttachmentViews = createInfo.Attachments.ConvertAll(v1 => (GLTextureView)v1);
			IDs = new uint[RenderPass.Subpasses.Length];

			var iface = Graphics.Interface;

			// Create subpass framebuffers
			iface.CreateFramebuffers(IDs);

			// Set attachments for each subpass
			for(int i = 0; i < IDs.Length; i++)
				SetAttachmentsForSubpass(IDs[i], RenderPass.Subpasses[i]);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var gl33 = GL.GL33!;

			gl33.DeleteFramebuffers(IDs);
			if (transientID.HasValue && transientID.Value != 0) gl33.DeleteFramebuffers(transientID.Value);
		}

		/// <summary>
		/// Begins a render pass using this framebuffer.
		/// </summary>
		public void BeginRenderPass(ICommandSink.RenderPassBegin beginInfo) {
			var renderPass = Graphics.State.CurrentRenderPass;

			var iface = Graphics.Interface;
			// If default framebuffer, just bind immediately
			if (transientID == 0) {
				Graphics.State.BindFramebuffer(GLFramebufferTarget.Framebuffer, 0);
			} else {
				// Else check if a transient framebuffer is needed and begin the subpass
				if (renderPass != RenderPass) {
					if (!transientID.HasValue)
						transientID = iface.CreateFramebuffer();
				}
			}

			// Perform clear/invalidate operations on the current framebuffer
			if (transientID == 0) {
				var begins = renderPass.AttachmentBegins;
				switch(begins[0].LoadOp) {
					case AttachmentLoadOp.DontCare:
						iface.InvalidateSubFramebuffer(0, beginInfo.RenderArea, new GLFramebufferAttachment[] { GLFramebufferAttachment.Color0 });
						break;
					case AttachmentLoadOp.Clear:
						iface.ClearFramebufferf(0, GLClearBuffer.Color, 0, beginInfo.ClearValues[0].Color.Float32);
						break;
					case AttachmentLoadOp.Load:
					default:
						break;
				}
				switch (begins[1].LoadOp) {
					case AttachmentLoadOp.DontCare:
						iface.InvalidateSubFramebuffer(0, beginInfo.RenderArea, new GLFramebufferAttachment[] { GLFramebufferAttachment.Depth, GLFramebufferAttachment.Stencil });
						break;
					case AttachmentLoadOp.Clear: {
							var clearValue = beginInfo.ClearValues[1];
							iface.ClearFramebufferfi(0, GLClearBuffer.DepthStencil, 0, clearValue.Depth, clearValue.Stencil);
						} break;
					case AttachmentLoadOp.Load:
					default:
						break;
				}
			} else {

			}
		}

		/// <summary>
		/// Begins a subpass within the current.
		/// </summary>
		public void BeginSubpass() {
			var renderPass = Graphics.State.CurrentRenderPass;
			var subpass = Graphics.State.CurrentSubpass;

			// If default framebuffer, just skip
			if (transientID == 0) return;

			if (renderPass == RenderPass) {
				Graphics.State.BindFramebuffer(GLFramebufferTarget.Framebuffer, IDs[subpass]);
			} else {
				
			}
		}

		/// <summary>
		/// Ends a subpass within the current.
		/// </summary>
		/// <param name="renderPass"></param>
		/// <param name="subpass"></param>
		public void EndSubpass() {
			var renderPass = Graphics.State.CurrentRenderPass;

			// If default framebuffer, just skip
			if (transientID == 0) return;

			if (renderPass == RenderPass) {

			} else {

			}
		}

		/// <summary>
		/// Ends a render pass using this framebuffer.
		/// </summary>
		public void EndRenderPass() {
			var renderPass = Graphics.State.CurrentRenderPass;

			// If default framebuffer, just skip
			if (transientID == 0) return;

			if (renderPass == RenderPass) {
				
			} else {
				
			}
		}

	}

}
