﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Services;

namespace Tesseract.Core.Graphics.Engine {

	/// <summary>
	/// Graphics engine creation information.
	/// </summary>
	public readonly struct GraphicsEngineCreateInfo {

		public IGraphics Graphics { get; init; }

		public IImageIO ImageIO { get; init; }

		public IImageProcessing ImageProcessing { get; init; }

	}

	/// <summary>
	/// A graphics engine provides a higher-level abstracion for general
	/// purpose computer graphics.
	/// </summary>
	public class GraphicsEngine {

		public IGraphics Graphics { get; }

		public IImageIO ImageIO { get; }

		public IImageProcessing ImageProcessing { get; }

		public GraphicsEngine(in GraphicsEngineCreateInfo createInfo) {
			Graphics = createInfo.Graphics;
			ImageIO = createInfo.ImageIO;
			ImageProcessing = createInfo.ImageProcessing;
		}

	}

}
