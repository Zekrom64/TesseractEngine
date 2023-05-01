using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;
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
					unsafe {
						SDL2.CheckError(SDL2.Functions.SDL_GL_GetAttribute(SDLGLAttr.ContextMajorVersion, out int value));
						majorVersion = value;
					}
				}
				return majorVersion.Value;
			}
		}

		public int MinorVersion {
			get {
				if (!minorVersion.HasValue) {
					MakeGLCurrent();
					unsafe {
						SDL2.CheckError(SDL2.Functions.SDL_GL_GetAttribute(SDLGLAttr.ContextMinorVersion, out int value));
						minorVersion = value;
					}
				}
				return minorVersion.Value;
			}
		}

		public SDLGLContext(IntPtr window, IntPtr context) {
			Window = window;
			Context = context;
		}

		public SDLGLContext(SDLWindow window) {
			Window = window.Window;
			unsafe {
				IntPtr pContext = SDL2.Functions.SDL_GL_CreateContext(Window);
				if (pContext == IntPtr.Zero) throw new SDLException(SDL2.GetError());
				Context = pContext;
			}
		}

		public IntPtr GetGLProcAddress(string procName) {
			MakeGLCurrent();
			unsafe {
				fixed (byte* pProcName = MemoryUtil.StackallocUTF8(procName, stackalloc byte[256])) {
					return SDL2.Functions.SDL_GL_GetProcAddress(pProcName);
				}
			}
		}

		public bool HasGLExtension(string extension) {
			MakeGLCurrent();
			unsafe {
				fixed(byte* pExtension = MemoryUtil.StackallocUTF8(extension, stackalloc byte[256])) {
					return SDL2.Functions.SDL_GL_ExtensionSupported(pExtension);
				}
			}
		}

		public void MakeGLCurrent() {
			unsafe {
				if (SDL2.Functions.SDL_GL_GetCurrentContext() != Context)
					SDL2.CheckError(SDL2.Functions.SDL_GL_MakeCurrent(Window, Context));
			}
		}

		public void SetGLSwapInterval(int swapInterval) {
			MakeGLCurrent();
			unsafe {
				SDL2.CheckError(SDL2.Functions.SDL_GL_SetSwapInterval(swapInterval));
			}
		}

		public void SwapGLBuffers() {
			MakeGLCurrent();
			unsafe {
				SDL2.Functions.SDL_GL_SwapWindow(Window);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			unsafe {
				SDL2.Functions.SDL_GL_DeleteContext(Context);
			}
		}

	}

}
