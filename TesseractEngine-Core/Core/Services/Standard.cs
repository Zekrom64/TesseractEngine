using Tesseract.Core.Graphics;
using Tesseract.Core.Input;

namespace Tesseract.Core.Services {

	/// <summary>
	/// Standard graphics services.
	/// </summary>
	public static class GraphicsServices {

		/// <summary>
		/// The windowing system service.
		/// </summary>
		public static readonly IService<IWindowSystem> WindowSystem = new OpaqueService<IWindowSystem>();

		/// <summary>
		/// The gamma ramp object service.
		/// </summary>
		public static readonly IService<IGammaRampObject> GammaRampObject = new OpaqueService<IGammaRampObject>();

		/// <summary>
		/// The image IO service.
		/// </summary>
		public static readonly IService<IImageIO> ImageIO = new OpaqueService<IImageIO>();

		/// <summary>
		/// The processable image service.
		/// </summary>
		public static readonly IService<IProcessableImage> ProcessableImage = new OpaqueService<IProcessableImage>();

	}

	/// <summary>
	/// Standard input services.
	/// </summary>
	public static class InputServices {

		/// <summary>
		/// The input system service.
		/// </summary>
		public static readonly IService<IInputSystem> InputSystem = new OpaqueService<IInputSystem>();

		/// <summary>
		/// The haptic device service.
		/// </summary>
		public static readonly IService<IHapticDevice> HapticDevice = new OpaqueService<IHapticDevice>();

		/// <summary>
		/// The light system service.
		/// </summary>
		public static readonly IService<ILightSystem> LightSystem = new OpaqueService<ILightSystem>();

		/// <summary>
		/// The clipboard service.
		/// </summary>
		public static readonly IService<IClipboard> Clipboard = new OpaqueService<IClipboard>();

	}

}
