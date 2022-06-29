using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;

namespace Tesseract.OpenGL.Graphics {

	public class GLSwapchain : ISwapchain {

		public IWindow Window { get; }
		public IGLContext Context { get; }

		public Vector2i Size => Window.Size;

		public PixelFormat Format { get; }

		public SwapchainImageType ImageType => SwapchainImageType.Framebuffer;

		private GLFramebuffer image;
		public ISwapchainImage[] Images { get; private set; }

		public GLSwapchain(GLGraphicsProvider provider, GLGraphics graphics, SwapchainCreateInfo createInfo) {
			Window = provider.Window;
			Context = provider.Context;
			Context.MakeGLCurrent();

			switch(createInfo.PresentMode) {
				case SwapchainPresentMode.FIFO:
					Context.SetGLSwapInterval(1);
					break;
				case SwapchainPresentMode.Immediate:
					Context.SetGLSwapInterval(0);
					break;
				case SwapchainPresentMode.RelaxedFIFO:
					if (!provider.supportsRelaxedPresentMode) throw new ArgumentException("Relaxed FIFO present mode not supported", nameof(createInfo));
					Context.SetGLSwapInterval(-1);
					break;
				default:
					throw new ArgumentException($"Unsupported swapchain presentation mode {createInfo.PresentMode}", nameof(createInfo));
			}

			GL11 gl11 = provider.GL.GL11;
			int rbits = gl11.GetInteger(Native.GLEnums.GL_RED_BITS);
			int gbits = gl11.GetInteger(Native.GLEnums.GL_GREEN_BITS);
			int bbits = gl11.GetInteger(Native.GLEnums.GL_BLUE_BITS);
			int abits = gl11.GetInteger(Native.GLEnums.GL_ALPHA_BITS);
			if (rbits == 8 && gbits == 8 && bbits == 8 && abits == 8) Format = PixelFormat.R8G8B8A8UNorm;
			else {
				List<PixelChannel> channels = new() {
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = 0, Size = rbits, Type = ChannelType.Red },
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = rbits, Size = gbits, Type = ChannelType.Green },
					new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = rbits + gbits, Size = bbits, Type = ChannelType.Blue }
				};
				if (abits != 0) channels.Add(new PixelChannel() { NumberFormat = ChannelNumberFormat.UnsignedNorm, Offset = rbits + gbits + bbits, Size = abits, Type = ChannelType.Alpha });
				Format = PixelFormat.DefinePackedFormat(channels.ToArray());
			}

			image = new GLFramebuffer(graphics, Size);
			Images = new ISwapchainImage[] { image };
		}

		public event Action? OnRebuild;

		public int BeginFrame(ISync? signal) => 0; // We can cheat alot here

		public void EndFrame(ISync? signalFence, params ISync[] wait) {
			Context.SwapGLBuffers();
			if (signalFence is GLSync sync && sync.IsFence) sync.GenerateFence();
			// Recreate the "framebuffer" if the size changes
			if (image.Size != Size) {
				image = new GLFramebuffer(image.Graphics, Size);
				Images = new ISwapchainImage[] { image };
				OnRebuild?.Invoke();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

}
