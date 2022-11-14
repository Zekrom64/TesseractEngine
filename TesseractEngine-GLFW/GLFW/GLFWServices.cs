using Tesseract.Core.Services;
using Tesseract.GLFW.Services;

namespace Tesseract.GLFW {

	/// <summary>
	/// Service manager for GLFW.
	/// </summary>
	public static class GLFWServices {

		/// <summary>
		/// Registers all GLFW services.
		/// </summary>
		public static void Register() {
			GlobalServices.AddGlobalService(InputServices.InputSystem, new GLFWServiceInputSystem());
			GlobalServices.AddGlobalService(GraphicsServices.WindowSystem, new GLFWServiceWindowSystem());
		}

	}

}
