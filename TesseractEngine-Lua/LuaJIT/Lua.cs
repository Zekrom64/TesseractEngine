using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	/// <summary>
	/// Lua constants.
	/// </summary>
	public static class Lua {

		internal const int IDSize = 60;

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

		/// <summary>
		/// Name of the metamethod invoked when a value is fetched from the corresponding object
		/// by its name or with the indexing operator. The first argument is the object reference
		/// and the second argument is the key.
		/// </summary>
		public const string MetamethodIndex = "__index";

		/// <summary>
		/// Name of the metamethod invoked when a value is written to the corresponding object
		/// by its name or with the indexing operator. The first argument is the object reference,
		/// the second is the key, and the third is the new value.
		/// </summary>
		public const string MetamethodNewIndex = "__newindex";

		/// <summary>
		/// Name of the metamethod invoked when an object is called like a function. The first
		/// argument is the object reference and any arguments given in the invocation are
		/// passed after the reference.
		/// </summary>
		public const string MetamethodCall = "__call";

		/// <summary>
		/// Name of the metatable value used to override the value returned from <c>getmetatable(t)</c>.
		/// </summary>
		public const string MetamethodMetatable = "__metatable";

		/// <summary>
		/// Name of the metamethod invoked when the object is passed to the <c>tostring(t)</c> function.
		/// The only argument is the object reference.
		/// </summary>
		public const string MetamethodToString = "__tostring";

		/// <summary>
		/// Name of the metamethod invoked when the length operator ('<c>#</c>') is applied to
		/// the object. The only argument is the object reference.
		/// </summary>
		public const string MetamethodLength = "__len";

		/// <summary>
		/// <para>
		/// Name of the metamethod invoked when the object is passed to the <c>pairs</c> function. The only
		/// argument is the object reference. The function should return an iterator function, the object
		/// reference, and the starting key in that order.
		/// </para>
		/// 
		/// <para>
		/// The iterator function must take two arguments, the object reference and the initial key, and
		/// return the next key and value. If the iterator function has reached the end it should
		/// return nothing.
		/// Sample implementation: <see href="http://lua-users.org/wiki/GeneralizedPairsAndIpairs"/>
		/// </para>
		/// </summary>
		public const string MetamethodPairs = "__pairs";

		/// <summary>
		/// <para>
		/// Name of the metamethod invoked when the object is passed to the <c>ipairs</c> function. The only
		/// argument is the object reference. The function should return an iterator function, the object
		/// reference, and the starting key in that order.
		/// </para>
		/// 
		/// <para>
		/// The iterator function must take two arguments, the object reference and the initial key, and
		/// return the next key and value. If the iterator function has reached the end it should
		/// return nothing.
		/// Sample implementation: <see href="http://lua-users.org/wiki/GeneralizedPairsAndIpairs"/>
		/// </para>
		/// </summary>
		public const string MetamethodIPairs = "__ipairs";

		/// <summary>
		/// Name of the metamethod invoked when userdata is garbage collected. The only
		/// argument is the userdata value.
		/// </summary>
		public const string MetamethodGC = "__gc";

		/// <summary>
		/// Name of the metamethod invoked when the object is negated. The only argument is the object reference.
		/// </summary>
		public const string MetamethodNegate = "__unm";

		/// <summary>
		/// Name of the metamethod invoked when the object is added with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodAdd = "__add";

		/// <summary>
		/// Name of the metamethod invoked when the object is subtract with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodSubtract = "__sub";

		/// <summary>
		/// Name of the metamethod invoked when the object is multiplied with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodMultiply = "__mul";

		/// <summary>
		/// Name of the metamethod invoked when the object is divided with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodDivide = "__div";

		/// <summary>
		/// Name of the metamethod invoked when the object is modulo with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodModulus = "__mod";

		/// <summary>
		/// Name of the metamethod invoked when the object is taken to the power of another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodPow = "__pow";

		/// <summary>
		/// Name of the metamethod invoked when the object is concatenated with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will check the
		/// left-hand value first for this metamethod before checking the right-hand value.
		/// </summary>
		public const string MetamethodConcatenate = "__concat";

		/// <summary>
		/// Name of the metamethod invoked when the object is equality-tested with another. The two values
		/// being compared are passed to the function (order is irrelevant as equality is symmetric).  The Lua
		/// runtime will only invoke this metamethod if both objects have the same metamethod reference. The 
		/// method will not be invoked if the two references refer to the same object.
		/// </summary>
		public const string MetamethodEqual = "__eq";

		/// <summary>
		/// Name of the metamethod invoked when the object is less-than compared with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will only invoke this
		/// metamethod if both objects have the same metamethod reference.
		/// </summary>
		public const string MetamethodLessThan = "__lt";

		/// <summary>
		/// Name of the metamethod invoked when the object is less-than-or-equal compared with another. The first argument
		/// is the left-hand value and the second is the right-hand. The Lua runtime will only invoke this
		/// metamethod if both objects have the same metamethod reference.
		/// </summary>
		public const string MetamethodLessThanOrEqual = "__le";



#if RELEASE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		internal static ReadOnlySpan<byte> CheckStr(ReadOnlySpan<byte> str, Span<byte> buf) {
#if DEBUG
			if (str.IsEmpty) {
				buf[0] = 0;
				return buf;
			}

			if (str[^1] == 0) return str;
			unsafe {
				fixed (byte* pStr = str) {
					if (pStr[str.Length] == 0) return str;
				}
			}

			if (str.Contains((byte)0)) return str;

			if (buf.Length < str.Length) buf = new byte[str.Length + 1];
			str.CopyTo(buf);
			buf[str.Length] = 0;
			return buf;
#else
			return str;
#endif
		}

	}
}
