using Tesseract.Core.Services;
using Tesseract.OpenGL;
using Tesseract.GLFW.Services;
using Tesseract.Core.Graphics;
using Tesseract.GLFW;

namespace Tesseract.SDL {

	public class GLFWServiceWindowGLContextProvider : IGLContextProvider {

		public GLFWServiceWindow Window { get; }

		public GLFWServiceWindowGLContextProvider(GLFWServiceWindow window) {
			Window = window;
		}

		private GLFWGLContext? glcontext = null;

		public IGLContext CreateContext() {
			glcontext ??= new GLFWGLContext(Window.Window);
			return glcontext;
		}

	}

	/// <summary>
	/// Service manager for SDL2's OpenGL features.
	/// </summary>
	public static class GLFWGLServices {

		/// <summary>
		/// Registers all SDL2 OpenGL services.
		/// </summary>
		public static void Register() {
			// Inject context provider into the window service
			ServiceInjector.Inject(GLServices.GLContextProvider, (GLFWServiceWindow window) => new GLFWServiceWindowGLContextProvider(window));
			GLFWServiceWindow.OnParseAttributes += (WindowAttributeList attributes) => {
				if (attributes.TryGet(GLWindowAttributes.OpenGLWindow, out bool glwindow) && glwindow) {
					if (attributes.TryGet(GLWindowAttributes.RedBits, out int redBits)) GLFW3.WindowHint(GLFWWindowAttrib.RedBits, redBits);
					if (attributes.TryGet(GLWindowAttributes.GreenBits, out int greenBits)) GLFW3.WindowHint(GLFWWindowAttrib.GreenBits, greenBits);
					if (attributes.TryGet(GLWindowAttributes.BlueBits, out int blueBits)) GLFW3.WindowHint(GLFWWindowAttrib.BlueBits, blueBits);
					if (attributes.TryGet(GLWindowAttributes.AlphaBits, out int alphaBits)) GLFW3.WindowHint(GLFWWindowAttrib.AlphaBits, alphaBits);
					if (attributes.TryGet(GLWindowAttributes.DepthBits, out int depthBits)) GLFW3.WindowHint(GLFWWindowAttrib.DepthBits, depthBits);
					if (attributes.TryGet(GLWindowAttributes.StencilBits, out int stencilBits)) GLFW3.WindowHint(GLFWWindowAttrib.StencilBits, stencilBits);
					if (attributes.TryGet(GLWindowAttributes.AccumRedBits, out int accumRedBits)) GLFW3.WindowHint(GLFWWindowAttrib.AccumRedBits, accumRedBits);
					if (attributes.TryGet(GLWindowAttributes.AccumGreenBits, out int accumGreenBits)) GLFW3.WindowHint(GLFWWindowAttrib.AccumGreenBits, accumGreenBits);
					if (attributes.TryGet(GLWindowAttributes.AccumBlueBits, out int accumBlueBits)) GLFW3.WindowHint(GLFWWindowAttrib.AccumBlueBits, accumBlueBits);
					if (attributes.TryGet(GLWindowAttributes.AccumAlphaBits, out int accumAlphaBits)) GLFW3.WindowHint(GLFWWindowAttrib.AccumAlphaBits, accumAlphaBits);
					if (attributes.TryGet(GLWindowAttributes.ContextVersionMajor, out int majorVer)) GLFW3.WindowHint(GLFWWindowAttrib.ContextVersionMajor, majorVer);
					if (attributes.TryGet(GLWindowAttributes.ContextVersionMinor, out int minorVer)) GLFW3.WindowHint(GLFWWindowAttrib.ContextVersionMinor, minorVer);
					if (attributes.TryGet(GLWindowAttributes.Doublebuffer, out bool doublebuffer)) GLFW3.WindowHint(GLFWWindowAttrib.DoubleBuffer, doublebuffer ? 1 : 0);
					if (attributes.TryGet(GLWindowAttributes.DebugContext, out bool debugctx)) GLFW3.WindowHint(GLFWWindowAttrib.OpenGLDebugContext, debugctx ? 1 : 0);
					if (attributes.TryGet(GLWindowAttributes.ContextProfile, out GLProfile profile)) {
						switch (profile) {
							case GLProfile.Compatibility:
								GLFW3.WindowHint(GLFWWindowAttrib.OpenGLProfile, (int)GLFWOpenGLProfile.CompatProfile);
								break;
							case GLProfile.Core:
								GLFW3.WindowHint(GLFWWindowAttrib.OpenGLProfile, (int)GLFWOpenGLProfile.CoreProfile);
								break;
							default:
								GLFW3.WindowHint(GLFWWindowAttrib.OpenGLProfile, (int)GLFWOpenGLProfile.AnyProfile);
								break;
						}
					}
				}
			};
		}

	}
}
