using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LuaJIT {

	/// <summary>
	/// Class for Lua-generated exceptions.
	/// </summary>
	public class LuaException : Exception {

		/// <inheritdoc cref="Exception(string?)"/>
		public LuaException(string? message) : base(message) { }

		/// <inheritdoc cref="Exception(string?, Exception?)"/>
		public LuaException(string? message, Exception? innerException) : base(message, innerException) { }

	}

}
