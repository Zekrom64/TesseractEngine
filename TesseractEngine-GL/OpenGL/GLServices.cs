using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics;
using Tesseract.Core.Services;
using Tesseract.Core.Utilities;

namespace Tesseract.OpenGL {

	/// <summary>
	/// A GL context provider provides a mechanism for creating an OpenGL context bound to the target.
	/// </summary>
	public interface IGLContextProvider {

		/// <summary>
		/// Creates an OpenGL context bound to the current target.
		/// </summary>
		/// <returns>OpenGL context</returns>
		public IGLContext CreateContext();

	}

	/// <summary>
	/// Enumeration of OpenGL context profiles.
	/// </summary>
	public enum GLProfile {
		/// <summary>
		/// Compatibility profile supporting pre-OpenGL 3.0 features.
		/// </summary>
		Compatibility,
		/// <summary>
		/// Core profile supporting only OpenGL 3.0 and newer features.
		/// </summary>
		Core
	}

	public static class GLWindowAttributes {

		/// <summary>
		/// Indicates that the window will be rendered to using the OpenGL API.
		/// </summary>
		public static readonly IWindowAttribute<bool> OpenGLWindow = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// The minimum number of bits for the red channel of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> RedBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the green channel of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> GreenBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the blue channel of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> BlueBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the alpha channel of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> AlphaBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the depth buffer of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> DepthBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the stencil buffer of the default framebuffer.
		/// </summary>
		public static readonly IWindowAttribute<int> StencilBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the red channel of the default accumulation buffer.
		/// </summary>
		public static readonly IWindowAttribute<int> AccumRedBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the green channel of the default accumulation buffer.
		/// </summary>
		public static readonly IWindowAttribute<int> AccumGreenBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the blue channel of the default accumulation buffer.
		/// </summary>
		public static readonly IWindowAttribute<int> AccumBlueBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum number of bits for the alpha channel of the default accumulation buffer.
		/// </summary>
		public static readonly IWindowAttribute<int> AccumAlphaBits = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// If the context should use doublebuffering.
		/// </summary>
		public static readonly IWindowAttribute<bool> Doublebuffer = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// The minimum major version of the OpenGL API to support.
		/// </summary>
		public static readonly IWindowAttribute<int> ContextVersionMajor = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The minimum minor version of the OpenGL API to support.
		/// </summary>
		public static readonly IWindowAttribute<int> ContextVersionMinor = new OpaqueWindowAttribute<int>();

		/// <summary>
		/// The OpenGL context profile to use.
		/// </summary>
		public static readonly IWindowAttribute<GLProfile> ContextProfile = new OpaqueWindowAttribute<GLProfile>();

		/// <summary>
		/// If the context should be created supporting debug extensions.
		/// </summary>
		public static readonly IWindowAttribute<bool> DebugContext = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// If the context should suppress error checking.
		/// </summary>
		public static readonly IWindowAttribute<bool> NoError = new OpaqueWindowAttribute<bool>();

	}

	/// <summary>
	/// OpenGL services.
	/// </summary>
	public static class GLServices {

		/// <summary>
		/// Service for OpenGL context providers.
		/// </summary>
		public static readonly IService<IGLContextProvider> GLContextProvider = new OpaqueService<IGLContextProvider>();

	}

}
