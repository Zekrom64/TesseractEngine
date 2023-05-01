using Tesseract.Core.Graphics;
using Tesseract.Core.Services;
using Tesseract.OpenGL;
using Tesseract.SDL.Services;

namespace Tesseract.SDL {

	public class SDLServiceWindowGLContextProvider : IGLContextProvider {

		public SDLServiceWindow Window { get; }

		public SDLServiceWindowGLContextProvider(SDLServiceWindow window) {
			Window = window;
		}

		private SDLGLContext? glcontext = null;

		public IGLContext CreateContext() {
			glcontext ??= new SDLGLContext(Window.Window);
			return glcontext;
		}

	}

	/// <summary>
	/// Service manager for SDL2's OpenGL features.
	/// </summary>
	public static class SDLGLServices {

		/// <summary>
		/// Registers all SDL2 OpenGL services.
		/// </summary>
		public static void Register() {
			// Inject context provider into the window service
			ServiceInjector.Inject(GLServices.GLContextProvider, (SDLServiceWindow window) => new SDLServiceWindowGLContextProvider(window));
			// Inject window setup code
			SDLServiceWindow.OnParseAttributes += (WindowAttributeList attributes, ref SDLWindowFlags flags) => {
				if (attributes.TryGet(GLWindowAttributes.OpenGLWindow, out bool glwindow) && glwindow) flags |= SDLWindowFlags.OpenGL;
			};
			SDLServiceWindow.OnWindowSetup += (WindowAttributeList attributes) => {
				if (attributes.TryGet(GLWindowAttributes.OpenGLWindow, out bool glwindow) && glwindow) {
					unsafe {
						if (attributes.TryGet(GLWindowAttributes.RedBits, out int redbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.RedSize, redbits);
						if (attributes.TryGet(GLWindowAttributes.GreenBits, out int greenbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.GreenSize, greenbits);
						if (attributes.TryGet(GLWindowAttributes.BlueBits, out int bluebits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.BlueSize, bluebits);
						if (attributes.TryGet(GLWindowAttributes.AlphaBits, out int alphabits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.AlphaSize, alphabits);
						if (attributes.TryGet(GLWindowAttributes.DepthBits, out int depthbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.DepthSize, depthbits);
						if (attributes.TryGet(GLWindowAttributes.StencilBits, out int stencilbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.StencilSize, stencilbits);
						if (attributes.TryGet(GLWindowAttributes.AccumRedBits, out int accredbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.AccumRedSize, accredbits);
						if (attributes.TryGet(GLWindowAttributes.AccumGreenBits, out int accgreenbits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.AccumGreenSize, accgreenbits);
						if (attributes.TryGet(GLWindowAttributes.AccumBlueBits, out int accbluebits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.AccumBlueSize, accbluebits);
						if (attributes.TryGet(GLWindowAttributes.AccumAlphaBits, out int accalphabits)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.AccumAlphaSize, accalphabits);
						if (attributes.TryGet(GLWindowAttributes.Doublebuffer, out bool doublebuffer)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.DoubleBuffer, doublebuffer ? 1 : 0);
						if (attributes.TryGet(GLWindowAttributes.ContextVersionMajor, out int majorver)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.ContextMajorVersion, majorver);
						if (attributes.TryGet(GLWindowAttributes.ContextVersionMinor, out int minorver)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.ContextMinorVersion, minorver);
						if (attributes.TryGet(GLWindowAttributes.ContextProfile, out GLProfile profile)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.ContextProfileMask, (int)(profile switch {
							GLProfile.Compatibility => SDLGLProfile.Compatibility,
							GLProfile.Core => SDLGLProfile.Core,
							_ => default
						}));
						if (attributes.TryGet(GLWindowAttributes.DebugContext, out bool debug) && debug) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.ContextFlags, (int)SDLGLContextFlag.DebugFlag);
						if (attributes.TryGet(GLWindowAttributes.NoError, out bool noerror)) SDL2.Functions.SDL_GL_SetAttribute(SDLGLAttr.ContextNoError, noerror ? 1 : 0);
					}
				}
			};
		}

	}
}
