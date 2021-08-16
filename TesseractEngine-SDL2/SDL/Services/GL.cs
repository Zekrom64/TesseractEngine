using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.OpenGL;

namespace Tesseract.SDL.Services {

	public class SDLGLContext : IGLContext {

		public IntPtr Window { get; }
		public IntPtr Context { get; }

		private int? majorVersion, minorVersion;

		public int MajorVersion {
			get {
				if (!majorVersion.HasValue) {
					MakeGLCurrent();
					SDL2.CheckError(SDL2.Functions.SDL_GL_GetAttribute(SDLGLAttr.ContextMajorVersion, out int value));
					majorVersion = value;
				}
				return majorVersion.Value;
			}
		}

		public int MinorVersion {
			get {
				if (!minorVersion.HasValue) {
					MakeGLCurrent();
					SDL2.CheckError(SDL2.Functions.SDL_GL_GetAttribute(SDLGLAttr.ContextMinorVersion, out int value));
					minorVersion = value;
				}
				return minorVersion.Value;
			}
		}

		public SDLGLContext(IntPtr window, IntPtr context) {
			Window = window;
			Context = context;
		}

		public IntPtr GetGLProcAddress(string procName) {
			MakeGLCurrent();
			return SDL2.Functions.SDL_GL_GetProcAddress(procName);
		}

		public bool HasGLExtension(string extension) {
			MakeGLCurrent();
			return SDL2.Functions.SDL_GL_ExtensionSupported(extension);
		}

		public void MakeGLCurrent() {
			if (SDL2.Functions.SDL_GL_GetCurrentContext() != Context)
				SDL2.CheckError(SDL2.Functions.SDL_GL_MakeCurrent(Window, Context));
		}

		public void SetGLSwapInterval(int swapInterval) {
			MakeGLCurrent();
			SDL2.CheckError(SDL2.Functions.SDL_GL_SetSwapInterval(swapInterval));
		}

		public void SwapGLBuffers() {
			MakeGLCurrent();
			SDL2.Functions.SDL_GL_SwapWindow(Window);
		}

	}

}
