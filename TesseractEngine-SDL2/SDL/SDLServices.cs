using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Services;
using Tesseract.SDL.Services;

namespace Tesseract.SDL {

	/// <summary>
	/// Service manager for SDL2.
	/// </summary>
	public static class SDLServices {

		/// <summary>
		/// Registers all SDL2 services.
		/// </summary>
		public static void Register() {
			GlobalServices.AddGlobalService(InputServices.InputSystem, new SDLServiceInputSystem());
			GlobalServices.AddGlobalService(GraphicsServices.WindowSystem, new SDLServiceWindowSystem());
		}

	}
}
