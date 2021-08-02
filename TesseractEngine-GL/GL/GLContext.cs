using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.GL {
	
	/// <summary>
	/// An OpenGL context manages state for an instance of the OpenGL API. To be used with other OpenGL commands
	/// a context must be bound to the current thread. An OpenGL context may only be bound to one thread at a time.
	/// </summary>
	public interface IGLContext {

		/// <summary>
		/// The major version number of the supported OpenGL API.
		/// </summary>
		public int MajorVersion { get; }

		/// <summary>
		/// The minor version number of the supported OpenGL API.
		/// </summary>
		public int MinorVersion { get; }

		/// <summary>
		/// Tests if this OpenGL context supports the given extension.
		/// </summary>
		/// <param name="extension">Extension name</param>
		/// <returns>If this context supports the extensions</returns>
		public bool HasGLExtension(string extension);

		/// <summary>
		/// Gets the function pointer for an OpenGL function.
		/// </summary>
		/// <param name="procName">Function name</param>
		/// <returns>Function pointer or NULL</returns>
		public IntPtr GetGLProcAddress(string procName);

		/// <summary>
		/// Binds this OpenGL context to the current thread.
		/// </summary>
		public void MakeGLCurrent();

		/// <summary>
		/// Swaps back and front buffers for this OpenGL context.
		/// </summary>
		public void SwapGLBuffers();

		/// <summary>
		/// Sets the swap interval for the current context.
		/// </summary>
		/// <param name="swapInterval"></param>
		public void SetGLSwapInterval(int swapInterval);

	}

	/// <summary>
	/// A GL context object is an object that stores a reference to an OpenGL context.
	/// </summary>
	public interface IGLContextObject {

		/// <summary>
		/// The OpenGL context this object references.
		/// </summary>
		public IGLContext Context { get; }

	}

}
