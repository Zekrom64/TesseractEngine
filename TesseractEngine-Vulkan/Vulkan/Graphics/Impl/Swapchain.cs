using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Math;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class Swapchain : ISwapchain {

		public Vector2i Size { get; }

		public PixelFormat Format { get; }

		public SwapchainImageType ImageType => throw new NotImplementedException();

		public ISwapchainImage[] Images => throw new NotImplementedException();

		public event Action? OnRebuild;

		public int BeginFrame(ISync signal) {
			
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public void EndFrame(ISync signalFence, params ISync[] wait) {
			
		}

	}

}
