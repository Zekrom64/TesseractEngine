using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.OpenGL.Graphics {

	public class GLGraphicsProvider : IGraphicsProvider {

		public static readonly Guid ID = new("f3712256-5018-4c98-8c8d-92b2e194a53a");

		public IGLContext Context { get; }
		public GL GL { get; }

		public IGraphicsProperites Properties { get; }

		public IGraphicsFeatures Features { get; }

		public IGraphicsLimits Limits { get; }

		public string Name => "OpenGL";

		public Guid UniqueID => ID;

		public bool MultiGraphics => false;

		public IWindow Window { get; }

		internal readonly bool supportsRelaxedPresentMode;
		private readonly List<SwapchainPresentMode> supportedPresentModes = new() { SwapchainPresentMode.FIFO, SwapchainPresentMode.Immediate };

		public GLGraphicsProvider(GLGraphicsEnumerator enumerator) {
			Context = enumerator.Context;
			Context.MakeGLCurrent();
			GL = new(Context);

			Properties = new GLGraphicsProperties(GL);
			Features = new GLGraphicsFeatures(GL, GLGraphicsFeatures.GatherHardwareFeatures(GL));
			Limits = new GLGraphicsLimits(GL);

			Window = enumerator.Window;

			supportsRelaxedPresentMode = Context.HasGLExtension("WGL_EXT_swap_control_tear") || Context.HasGLExtension("GLX_EXT_swap_control_tear");
			if (supportsRelaxedPresentMode) supportedPresentModes.Add(SwapchainPresentMode.RelaxedFIFO);
		}

		public IGraphics CreateGraphics(GraphicsCreateInfo createInfo) => new GLGraphics(this, createInfo);

		public ISwapchain CreateSwapchain(IGraphics graphics, SwapchainCreateInfo createInfo) {
			if (graphics is GLGraphics glgraphics) {
				if (glgraphics.Context != Context) throw new ArgumentException("Swapchain must be created with graphics ", nameof(graphics));
				return new GLSwapchain(this, glgraphics, createInfo);
			} else throw new ArgumentException("Swapchain must be created with OpenGL graphics", nameof(graphics));
		}

		public SwapchainSupportInfo? GetSwapchainSupport(IGraphics graphics, IWindow window) {
			if (window != Window) return null;
			return new SwapchainSupportInfo() {
				ImageType = SwapchainImageType.Framebuffer,
				SupportedPresentModes = supportedPresentModes
			};
		}

	}

	[GraphicsEnumerator]
	public class GLGraphicsEnumerator : IGraphicsEnumerator {

		public static IGraphicsEnumerator GetEnumerator(GraphicsEnumeratorCreateInfo createInfo) {
			IWindow? window = createInfo.Window;
			if (window != null && window.GetService(GLServices.GLContextProvider) == null) window = null;
			if (window == null) return EmptyGraphicsEnumerator.Instance;
			return new GLGraphicsEnumerator(window);
		}

		public IWindow Window { get; }
		public IGLContext Context { get; }

		private GLGraphicsEnumerator(IWindow window) {
			Window = window;
			Context = window.GetService(GLServices.GLContextProvider)!.CreateContext();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

		public IEnumerable<IGraphicsProvider> EnumerateProviders() {
			yield return new GLGraphicsProvider(this);
		}

		public bool TryGetProvider(Guid uniqueID, [NotNullWhen(true)] out IGraphicsProvider? provider) {
			provider = null;
			if (Window == null) return false;
			if (uniqueID == GLGraphicsProvider.ID) {
				provider = new GLGraphicsProvider(this);
				return true;
			}
			return false;
		}
	}

}
