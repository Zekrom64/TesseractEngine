using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Math;

namespace Tesseract.GLFW {

	public class GLFWCursor : IDisposable {

		public IntPtr Cursor { get; }

		public GLFWCursor(GLFWImage image, Vector2i hot) {
			Cursor = GLFW3.Functions.glfwCreateCursor(image, hot.X, hot.Y);
		}

		public GLFWCursor(GLFWCursorShape shape) {
			Cursor = GLFW3.Functions.glfwCreateStandardCursor(shape);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GLFW3.Functions.glfwDestroyCursor(Cursor);
		}

	}

}
