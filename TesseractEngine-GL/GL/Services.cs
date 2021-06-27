using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Services;
using Tesseract.Core.Util;

namespace Tesseract.GL {

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
	/// Enumeration of OpenGL window hints.
	/// </summary>
	public enum GLWindowHint {
		/// <summary>
		/// The minimum number of bits for the red channel of the default framebuffer.
		/// </summary>
		RedBits,
		/// <summary>
		/// The minimum number of bits for the green channel of the default framebuffer.
		/// </summary>
		GreenBits,
		/// <summary>
		/// The minimum number of bits for the blue channel of the default framebuffer.
		/// </summary>
		BlueBits,
		/// <summary>
		/// The minimum number of bits for the alpha channel of the default framebuffer.
		/// </summary>
		AlphaBits,
		/// <summary>
		/// The minimum number of bits for the depth buffer of the default framebuffer.
		/// </summary>
		DepthBits,
		/// <summary>
		/// The minimum number of bits for the stencil buffer of the default framebuffer.
		/// </summary>
		StencilBits,
		/// <summary>
		/// The minimum number of bits for the red channel of the default accumulation buffer.
		/// </summary>
		AccumRedBits,
		/// <summary>
		/// The minimum number of bits for the green channel of the default accumulation buffer.
		/// </summary>
		AccumGreenBits,
		/// <summary>
		/// The minimum number of bits for the blue channel of the default accumulation buffer.
		/// </summary>
		AccumBlueBits,
		/// <summary>
		/// The minimum number of bits for the alpha channel of the default accumulation buffer.
		/// </summary>
		AccumAlphaBits,
		/// <summary>
		/// If the context should use doublebuffering.
		/// </summary>
		Doublebuffer,
		/// <summary>
		/// The minimum major version of the OpenGL API to support.
		/// </summary>
		ContextVersionMajor,
		/// <summary>
		/// The minimum minor version of the OpenGL API to support.
		/// </summary>
		ContextVersionMinor,
		/// <summary>
		/// The OpenGL context profile to use.
		/// </summary>
		ContextProfile,
		/// <summary>
		/// If the context should be created supporting debug extensions.
		/// </summary>
		DebugContext
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

	/// <summary>
	/// An OpenGL windowing system allows for setting OpenGL-related properties of the windowing system.
	/// </summary>
	public interface IGLWindowSystem {

		/// <summary>
		/// Sets an OpenGL hint for created windows.
		/// </summary>
		/// <param name="hint">OpenGL window hint</param>
		/// <param name="value">Window hint value</param>
		[ThreadSafety(ThreadSafetyLevel.MainThread)]
		public void SetGLHint(GLWindowHint hint, int value);

	}

	/// <summary>
	/// OpenGL services.
	/// </summary>
	public static class GLServices {

		/// <summary>
		/// Service for OpenGL context providers.
		/// </summary>
		public static readonly IService<IGLContextProvider> GLContextProvider = new OpaqueService<IGLContextProvider>();

		/// <summary>
		/// Service for OpenGL windowing systems.
		/// </summary>
		public static readonly IService<IGLWindowSystem> GLWindowSystem = new OpaqueService<IGLWindowSystem>();

	}

}
