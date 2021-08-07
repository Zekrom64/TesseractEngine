using System;
using Tesseract.Core.Native;
using Tesseract.GLFW.Native;

namespace Tesseract.GLFW {

	public static class GLFW3 {

		public const int DontCare = -1;

		public static readonly LibrarySpec LibrarySpec = new() { Name = "glfw3" };

		private static Library library;
		public static Library Library {
			get {
				if (library == null) library = LibraryManager.Load(LibrarySpec);
				return library;
			}
		}

		private static GLFW3Functions functions;
		public static GLFW3Functions Functions {
			get {
				if (functions == null) {
					functions = new();
					Library.LoadFunctions(functions);
				}
				return functions;
			}
		}

	}
}
