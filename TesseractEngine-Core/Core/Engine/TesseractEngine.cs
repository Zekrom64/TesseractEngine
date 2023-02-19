using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Input;
using Tesseract.Core.Resource;
using Tesseract.Core.Utilities;

namespace Tesseract.Core.Engine {
	
	/// <summary>
	/// Engine creation information.
	/// </summary>
	public record class EngineCreateInfo {
	
		/// <summary>
		/// The graphcis to use.
		/// </summary>
		public required IGraphics Graphics { get; init; }

		/// <summary>
		/// The window to use.
		/// </summary>
		public required IWindow Window { get; init; }

		/// <summary>
		/// The input system to use.
		/// </summary>
		public required IInputSystem InputSystem { get; init; }

		/// <summary>
		/// The image I/O to use.
		/// </summary>
		public IImageIO ImageIO { get; init; } = new ImageSharpService();
	
	}

	public class TesseractEngine {

		/// <summary>
		/// The creation information for this instance of the engine.
		/// </summary>
		public EngineCreateInfo CreateInfo { get; }

		/// <summary>
		/// The graphics backend this engine uses.
		/// </summary>
		public IGraphics Graphics => CreateInfo.Graphics;

		/// <summary>
		/// The properties of the graphics backend this engine uses.
		/// </summary>
		public IGraphicsProperites GraphicsProperites { get; }

		/// <summary>
		/// The thread safety level of the graphics backend.
		/// </summary>
		public ThreadSafetyLevel ThreadSafety => GraphicsProperites.APIThreadSafety;

		/// <summary>
		/// The image I/O used by this engine.
		/// </summary>
		public IImageIO ImageIO => CreateInfo.ImageIO;

		public TesseractEngine(EngineCreateInfo createInfo) {
			CreateInfo = createInfo;
			GraphicsProperites = Graphics.Properties;
		}



		internal void InvokeInRenderThread(Action action) {

		}

	}

	/// <summary>
	/// Interface for objects that are instantiated as part of an engine.
	/// </summary>
	public interface IEngineObject {

		/// <summary>
		/// The engine instance this object belongs to.
		/// </summary>
		public TesseractEngine Engine { get; }

	}

}
