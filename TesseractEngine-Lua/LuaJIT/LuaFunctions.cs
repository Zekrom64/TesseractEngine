using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	using lua_Integer = nint;
	using lua_Number = Double;

	//using lua_CFunction = delegate* unmanaged<IntPtr, int>;
	//using lua_Alloc = delegate* unmanaged<IntPtr, IntPtr, nuint, nuint, IntPtr>;

	/// <summary>
	/// A managed Lua function. When invoked the stack will contain all of the
	/// arguments passed to the function, and it must return the number of
	/// values returned from the function (taken from the top of the stack).
	/// </summary>
	/// <param name="state">The Lua runtime</param>
	/// <returns>The number of return values</returns>
	public delegate int LuaFunction(LuaState state);

	/// <summary>
	/// A custom Lua memory allocation function. When invoked the pointer argument will
	/// contain any existing pointer, and the <c>osize</c> and <c>nsize</c> arguments
	/// will contain the old and new allocation sizes, respectively. This function is
	/// therefore invoked in three different ways:
	/// <list type="bullet">
	/// <item>
	/// <c>osize == 0</c>: The function should behave like <c>malloc</c> and allocate
	/// and return <c>nsize</c> bytes of memory.
	/// </item>
	/// <item>
	/// <c>nsize == 0</c>: The function should behave like <c>free</c> and deallocate
	/// the memory passed to it.
	/// </item>
	/// <item>
	/// Otherwise, the function should behave like <c>realloc</c>, and resize the
	/// memory passed to it from the old size to the new size.
	/// </item>
	/// </list>
	/// The memory allocator may return <see cref="IntPtr.Zero"/> when allocating or
	/// growing the size of memory to indicate an error, and this will be handled
	/// accordingly in the Lua runtime.
	/// </summary>
	/// <param name="ptr">Existing memory pointer</param>
	/// <param name="osize">Old memory size</param>
	/// <param name="nsize">New memory size</param>
	/// <returns>New memory pointer</returns>
	public delegate IntPtr LuaAlloc(IntPtr ptr, nuint osize, nuint nsize);

	public unsafe class LuaFunctions {

		// lua.h

		[NativeType("lua_State* lua_newstate(lua_Alloc f, void* ud)")]
		public delegate* unmanaged<delegate* unmanaged<IntPtr, IntPtr, nuint, nuint, IntPtr>, IntPtr, IntPtr> lua_newstate;
		[NativeType("void lua_close(lua_State* L)")]
		public delegate* unmanaged<IntPtr, void> lua_close;
		[NativeType("lua_State* lua_newthread(lua_State* L)")]
		public delegate* unmanaged<IntPtr, IntPtr> lua_newthread;

		[ExternFunction(Relaxed = true)]
		[NativeType("void lua_resetthread(lua_State* L, lua_State* th)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> lua_resetthread;

		[NativeType("lua_CFunction lua_atpanic(lua_State* L, lua_CFunction panicf)")]
		public delegate* unmanaged<IntPtr, delegate* unmanaged<IntPtr, int>, delegate* unmanaged<IntPtr, int>> lua_atpanic;

		[NativeType("int lua_gettop(lua_State* L)")]
		public delegate* unmanaged<IntPtr, int> lua_gettop;
		[NativeType("void lua_settop(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_settop;
		[NativeType("void lua_pushvalue(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_pushvalue;
		[NativeType("void lua_remove(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_remove;
		[NativeType("void lua_insert(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_insert;
		[NativeType("void lua_replace(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_replace;
		[NativeType("int lua_checkstack(lua_State* L, int sz)")]
		public delegate* unmanaged<IntPtr, int, int> lua_checkstack;

		[NativeType("void lua_xmove(lua_State* from, lua_State* to, int n)")]
		public delegate* unmanaged<IntPtr, IntPtr, int, void> lua_xmove;

		[NativeType("int lua_isnumber(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_isnumber;
		[NativeType("int lua_isstring(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_isstring;
		[NativeType("int lua_iscfunction(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_iscfunction;
		[NativeType("int lua_isuserdata(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_isuserdata;
		[NativeType("int lua_type(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_type;
		[NativeType("const char* lua_typename(lua_State* L, int tp)")]
		public delegate* unmanaged<IntPtr, int, IntPtr> lua_typename;

		[NativeType("int lua_equal(lua_State* L, int idx1, int idx2)")]
		public delegate* unmanaged<IntPtr, int, int, int> lua_equal;
		[NativeType("int lua_rawequal(lua_State* L, int idx1, int idx2)")]
		public delegate* unmanaged<IntPtr, int, int, int> lua_rawequal;
		[NativeType("int lua_lessthan(lua_State* L, int idx1, int idx2)")]
		public delegate* unmanaged<IntPtr, int, int, int> lua_lessthan;

		[NativeType("lua_Number lua_tonumber(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, lua_Number> lua_tonumber;
		[NativeType("lua_Integer lua_tointeger(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, lua_Integer> lua_tointeger;
		[NativeType("int lua_toboolean(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_toboolean;
		[NativeType("IntPtr lua_tolstring(lua_State* L, int idx, size_t* len)")]
		public delegate* unmanaged<IntPtr, int, out nuint, IntPtr> lua_tolstring;
		[NativeType("size_t lua_objlen(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, nuint> lua_objlen;
		[NativeType("lua_CFunction luia_tocfunction(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, delegate* unmanaged<IntPtr, int>> lua_tocfunction;
		[NativeType("void* lua_touserdata(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, IntPtr> lua_touserdata;
		[NativeType("lua_State* lua_tothread(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, IntPtr> lua_tothread;
		[NativeType("const void* lua_topointer(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, IntPtr> lua_topointer;

		[NativeType("void lua_pushnil(lua_State* L)")]
		public delegate* unmanaged<IntPtr, void> lua_pushnil;
		[NativeType("void lua_pushnumber(lua_State* L, lua_Number n)")]
		public delegate* unmanaged<IntPtr, lua_Number, void> lua_pushnumber;
		[NativeType("void lua_pushinteger(lua_State* L, lua_Integer n)")]
		public delegate* unmanaged<IntPtr, lua_Integer, void> lua_pushinteger;
		[NativeType("void lua_pushlstring(lua_State* L, const char* s, size_t l)")]
		public delegate* unmanaged<IntPtr, IntPtr, nuint, void> lua_pushlstring;
		[NativeType("void lua_pushstring(lua_State* L, const char* s)")]
		public delegate* unmanaged<IntPtr, IntPtr, void> lua_pushstring;
		[NativeType("void lua_pushcclosure(lua_State* L, lua_CFunction fn, int n)")]
		public delegate* unmanaged<IntPtr, delegate* unmanaged<IntPtr, int>, int, void> lua_pushcclosure;
		[NativeType("void lua_pushboolean(lua_State* L, int b)")]
		public delegate* unmanaged<IntPtr, int, void> lua_pushboolean;
		[NativeType("void lua_pushlightuserdata(lua_State* L, void* p)")]
		public delegate* unmanaged<IntPtr, nint, void> lua_pushlightuserdata;
		[NativeType("int lua_pushthread(lua_State* L)")]
		public delegate* unmanaged<IntPtr, int> lua_pushthread;

		[NativeType("void lua_gettable(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_gettable;
		[NativeType("void lua_getfield(lua_State* L, int index, const char* k)")]
		public delegate* unmanaged<IntPtr, int, IntPtr, void> lua_getfield;
		[NativeType("void lua_rawget(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_rawget;
		[NativeType("void lua_rawgeti(lua_State* L, int idx, int n)")]
		public delegate* unmanaged<IntPtr, int, int, void> lua_rawgeti;
		[NativeType("void lua_createtable(lua_State* L, int narr, int nrec)")]
		public delegate* unmanaged<IntPtr, int, int, void> lua_createtable;
		[NativeType("void* lua_newuserdata(lua_State* L, size_t sz)")]
		public delegate* unmanaged<IntPtr, nuint, IntPtr> lua_newuserdata;
		[NativeType("int lua_getmetatable(lua_State* L, int objindex)")]
		public delegate* unmanaged<IntPtr, int, int> lua_getmetatable;
		[NativeType("void lua_getfenv(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_getfenv;

		[NativeType("void lua_settable(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_settable;
		[NativeType("void lua_setfield(lua_State* L, int idx, const char* k)")]
		public delegate* unmanaged<IntPtr, int, IntPtr, void> lua_setfield;
		[NativeType("void lua_rawset(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, void> lua_rawset;
		[NativeType("void lua_rawseti(lua_State* L, int idx, int n)")]
		public delegate* unmanaged<IntPtr, int, int, void> lua_rawseti;
		[NativeType("void lua_setmetatable(lua_State* L, int objindex)")]
		public delegate* unmanaged<IntPtr, int, void> lua_setmetatable;
		[NativeType("int lua_setfenv(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_setfenv;

		[NativeType("void lua_call(lua_State* L, int nargs, int nresults)")]
		public delegate* unmanaged<IntPtr, int, int, void> lua_call;
		[NativeType("int lua_pcall(lua_State* L, int nargs, int nresults, int errfunc)")]
		public delegate* unmanaged<IntPtr, int, int, int, int> lua_pcall;

		[NativeType("int lua_gc(lua_State* L, int what, int data)")]
		public delegate* unmanaged<IntPtr, int, int, int> lua_gc;

		[NativeType("int lua_error(lua_State* L)")]
		public delegate* unmanaged<IntPtr, int> lua_error;
		[NativeType("int lua_next(lua_State* L, int idx)")]
		public delegate* unmanaged<IntPtr, int, int> lua_next;
		[NativeType("void lua_concat(lua_State* L, int n)")]
		public delegate* unmanaged<IntPtr, int, void> lua_concat;

		[NativeType("void lua_setexdata(lua_State* L, void* exdata)")]
		[ExternFunction(Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr, void> lua_setexdata;
		[NativeType("void* lua_getexdata(lua_State* L)")]
		[ExternFunction(Relaxed = true)]
		public delegate* unmanaged<IntPtr, IntPtr> lua_getexdata;

		// lauxlib.h

		[NativeType("int luaL_newmetatable(lua_State* L, const char* tname)")]
		public delegate* unmanaged<IntPtr, IntPtr, int> luaL_newmetatable;
		[NativeType("void* luaL_checkudata(lua_State* L, const char* tname)")]
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr> luaL_checkudata;

	}

}
