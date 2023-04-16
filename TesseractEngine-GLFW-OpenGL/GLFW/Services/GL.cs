
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.OpenGL;

namespace Tesseract.GLFW.Services {

	public class GLFWGLContext : IGLContext {

		public readonly GLFWWindow Window;

		public GLFWGLContext(GLFWWindow window) {
			Window = window;
		}

		private int? majorVersion;
		public int MajorVersion {
			get {
				majorVersion ??= Window.GetAttrib(GLFWWindowAttrib.ContextVersionMajor);
				return majorVersion.Value;
			}
		}

		private int? minorVersion;
		public int MinorVersion {
			get {
				minorVersion ??= Window.GetAttrib(GLFWWindowAttrib.ContextVersionMinor);
				return minorVersion.Value;
			}
		}

		public IntPtr GetGLProcAddress(string procName) => GLFW3.GetProcAddress(procName);

		public bool HasGLExtension(string extension) => GLFW3.ExtensionSupported(extension);

		public void MakeGLCurrent() => GLFW3.CurrentContext = Window;

		public void SetGLSwapInterval(int swapInterval) => GLFW3.SwapInterval = swapInterval;

		public void SwapGLBuffers() => Window.SwapBuffers();

		public void Dispose() {
			GC.SuppressFinalize(this);
			// No-op, context gets destroyed with window
		}

	}

}
