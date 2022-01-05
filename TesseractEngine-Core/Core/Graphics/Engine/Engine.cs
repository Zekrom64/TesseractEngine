using Tesseract.Core.Graphics.Accelerated;

namespace Tesseract.Core.Graphics.Engine {

	/// <summary>
	/// Graphics engine creation information.
	/// </summary>
	public readonly struct GraphicsEngineCreateInfo {

		public IGraphics Graphics { get; init; }

		public IImageIO ImageIO { get; init; }

	}

	/// <summary>
	/// A graphics engine provides a higher-level abstracion for general
	/// purpose computer graphics.
	/// </summary>
	public class GraphicsEngine {

		public IGraphics Graphics { get; }

		public IImageIO ImageIO { get; }

		public GraphicsEngine(in GraphicsEngineCreateInfo createInfo) {
			Graphics = createInfo.Graphics;
			ImageIO = createInfo.ImageIO;
		}

	}

}
