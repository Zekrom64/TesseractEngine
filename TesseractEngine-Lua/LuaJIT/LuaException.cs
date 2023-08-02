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

		public LuaException(string? message) : base(message) { }

	}

}
