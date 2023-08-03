using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	/// <summary>
	/// Lua constants.
	/// </summary>
	public static class Lua {

		public static readonly LibrarySpec Spec = new() { Name = "lua51" };
		public static readonly Library Library = LibraryManager.Load(Spec);
		public static readonly LuaFunctions Functions = new();

		static Lua() {
			Library.LoadFunctions(Functions);
		}

		/// <summary>
		/// Return count indicating the function may return a variable number of values.
		/// </summary>
		public const int MultRet = -1;

		/// <summary>
		/// Special stack index for the registry table.
		/// </summary>
		public const int RegistryIndex = -10000;

		/// <summary>
		/// Special stack index for the environment table.
		/// </summary>
		public const int EnvironIndex = -10001;

		/// <summary>
		/// Special stack index for the globals table.
		/// </summary>
		public const int GlobalsIndex = -10002;

		/// <summary>
		/// Gets a special stack index for the given upvalue (starting at index 1).
		/// </summary>
		/// <param name="i">Upvalue index</param>
		/// <returns>Special stack index for upvalue</returns>
		public static int UpValueIndex(int i) => GlobalsIndex - i;

		/// <summary>
		/// If the Lua runtime can load the debug package.
		/// </summary>
		public static bool HasDebug {
			get {
				unsafe {
					return Functions.luaopen_debug != default;
				}
			}
		}

		/// <summary>
		/// If the Lua runtime can load the bitwise-operator package.
		/// </summary>
		public static bool HasBit {
			get {
				unsafe {
					return Functions.luaopen_bit != default;
				}
			}
		}

	}
}
