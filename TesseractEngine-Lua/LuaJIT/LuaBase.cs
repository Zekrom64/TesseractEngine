using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	using LuaInteger = IntPtr;
	using LuaNumber = Double;

	/// <summary>
	/// Base class for objects which interface with a Lua runtime. Note that because of how
	/// Lua handles threads (see <see cref="NewThread"/>) instances of this class should
	/// <b>never</b> be cached, only the <see cref="LuaState"/> for the main thread
	/// can be persistently stored safely.
	/// </summary>
	public class LuaBase {

		/// <summary>
		/// The handle holding a reference to this object. This will only
		/// be initialized by a call to <see cref="SetSelf"/>.
		/// </summary>
		protected GCHandle Handle { get; private set; }

		// Reads the self reference pointer for the root state from the given Lua state.
		private static IntPtr GetSelfPtr(IntPtr l) {
			unsafe {
				var lua = Lua.Functions;
				// Use lua_getexdata if available
				if (lua.lua_getexdata != default) {
					return lua.lua_getexdata(l);
				} else {
					// Else fall back to the registry value
					fixed (byte* pName = "__self"u8) {
						lua.lua_getfield(l, Lua.RegistryIndex, (IntPtr)pName);
						IntPtr pSelf = lua.lua_touserdata(l, -1);
						lua.lua_settop(l, -2);
						return pSelf;
					}
				}
			}
		}

		/// <summary>
		/// Gets the root state behind this Lua interface. This is the "main" thread,
		/// where the <see cref="LuaBase"/> class may represent any thread.
		/// </summary>
		public LuaState RootState {
			get {
				// If this is already a root return itself
				if (this is LuaState state) return state;
				// Else access the root through the self pointer
				return (LuaState)GCHandle.FromIntPtr(GetSelfPtr(L)).Target!;
			}
		}

		/// <summary>
		/// Gets a managed Lua interface from the given <c>lua_State*</c> value.
		/// </summary>
		/// <param name="l">Unmanaged Lua state</param>
		/// <returns>Managed Lua state</returns>
		public static LuaBase Get(IntPtr l) {
			IntPtr pSelf = GetSelfPtr(l);
			LuaBase? self = null;
			// Try to get the interface from the self reference
			if (pSelf != IntPtr.Zero) {
				self = (LuaBase)GCHandle.FromIntPtr(pSelf).Target!;
				// If the lua_State* value differs (ie. for threads), we can't use the self reference
				if (self.L != l) self = null;
			}
			// If we couldn't get an existing self reference, create a new temporary one
			self ??= new LuaBase(l);
			return self;
		}

		/// <summary>
		/// Sets the self reference, <see cref="Handle"/>, and stores the value in the associated
		/// Lua interface, allowing back-reference to this C# object from a raw <c>lua_State*</c>
		/// value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void SetSelf() {
			Handle = GCHandle.Alloc(this);
			unsafe {
				var lua = Lua.Functions;
				if (lua.lua_setexdata != default) {
					lua.lua_setexdata(L, (nint)Handle);
				} else {
					PushLightUserdata((nint)Handle);
					SetField(Lua.RegistryIndex, "__self"u8);
				}
			}
		}

		/// <summary>
		/// The underlying <c>lua_State*</c> for this Lua interface.
		/// </summary>
		public IntPtr L { get; protected set; }

		protected LuaBase(IntPtr l = default) {
			L = l;
		}

		/// <summary>
		/// This method is invoked when a managed exception is captured before converting it into a Lua error.
		/// </summary>
		/// <param name="e">The exception that was captured</param>
		protected virtual void CaptureManagedException(Exception e) { }


		/// <summary>
		/// <para>
		/// Creates a new Lua thread, pushing it onto the stack and returning a Lua interface
		/// for the thread. Each thread shares its global state with all other threads, but has
		/// its own indpendent stack.
		/// </para>
		/// 
		/// <para>
		/// Threads are managed by the Lua runtime and will be automatically garbage collected by
		/// it when they are no longer referenced on the Lua side. Any instances of <see cref="LuaBase"/>
		/// referencing the thread after it is disposed become invalid and will have undefined behavior
		/// if used.
		/// </para>
		/// </summary>
		/// <returns>Interface for new thread</returns>
		public LuaBase NewThread() {
			unsafe {
				return Get(Lua.Functions.lua_newthread(L));
			}
		}

		/// <summary>
		/// Index of the top of the stack.
		/// </summary>
		public int Top {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					return Lua.Functions.lua_gettop(L);
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set {
				unsafe {
					Lua.Functions.lua_settop(L, value);
				}
			}
		}

		/// <summary>
		/// Pushes the value at the given index onto the top of the stack. Note that for non-primitive
		/// types (ie. not a nil, boolean, or number) this is a <i>reference</i> to the value, so both
		/// the original and new values on the stack point to the same object.
		/// </summary>
		/// <param name="index"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushValue(int index) {
			unsafe {
				Lua.Functions.lua_pushvalue(L, index);
			}
		}

		/// <summary>
		/// Removes the value on the stack at the given index, shifting any values above it downwards.
		/// </summary>
		/// <param name="index">The stack index of the value to remove</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(int index) {
			unsafe {
				Lua.Functions.lua_remove(L, index);
			}
		}

		/// <summary>
		/// Inserts the topmost value on the stack at the given index, removing it from the top
		/// before returnin.
		/// </summary>
		/// <param name="index">The stack index of the place to insert at</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Insert(int index) {
			unsafe {
				Lua.Functions.lua_insert(L, index);
			}
		}

		/// <summary>
		/// Replaces the value at the given index in the stack with the topmost value, removing
		/// the top value before returning.
		/// </summary>
		/// <param name="index">The stack index of the value to replace</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Replace(int index) {
			unsafe {
				Lua.Functions.lua_replace(L, index);
			}
		}

		/// <summary>
		/// Ensures that there are the given number of free slots in the stack, returning
		/// if the requested capacity was allocated.
		/// </summary>
		/// <param name="sz">The requested capacity of free slots in the stack</param>
		/// <returns>If the stack was able to grow to fit the requested capacity</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CheckStack(int sz) {
			unsafe {
				return Lua.Functions.lua_checkstack(L, sz) != 0;
			}
		}

		//===============================//
		// Access functions (stack -> C) //
		//===============================//

		/// <summary>
		/// Tests if the value at the given stack index is a number.
		/// </summary>
		/// <param name="index">Stack index of the value to test</param>
		/// <returns>If the value is a number</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNumber(int index) {
			unsafe {
				return Lua.Functions.lua_isnumber(L, index) != 0;
			}
		}

		/// <summary>
		/// Tests if the value at the given stack index is a string.
		/// </summary>
		/// <param name="index">Stack index of the value to test</param>
		/// <returns>If the value is a string</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsString(int index) {
			unsafe {
				return Lua.Functions.lua_isstring(L, index) != 0;
			}
		}

		/// <summary>
		/// Tests if the value at the given stack index is userdata.
		/// </summary>
		/// <param name="index">Stack index of the value to test</param>
		/// <returns>If the value is userdata</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsUserdata(int index) {
			unsafe {
				return Lua.Functions.lua_isuserdata(L, index) != 0;
			}
		}

		/// <summary>
		/// Gets the type of the value at the given stack index.
		/// </summary>
		/// <param name="index">Stack index of the value</param>
		/// <returns>Value type</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaType Type(int index) {
			unsafe {
				return (LuaType)Lua.Functions.lua_type(L, index);
			}
		}

		/// <summary>
		/// Gets the type of the value at the given stack index as a string.
		/// </summary>
		/// <param name="index">Stack index of the value</param>
		/// <returns>Value type string</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string TypeName(int index) {
			unsafe {
				return MemoryUtil.GetUTF8(Lua.Functions.lua_typename(L, index))!;
			}
		}

		/// <summary>
		/// Tests if the two values on the stack are equal, potentially invoking the <c>__eq</c> metamethod.
		/// </summary>
		/// <param name="index1">First value index</param>
		/// <param name="index2">Second value index</param>
		/// <returns>If the two values are equal</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equal(int index1, int index2) {
			unsafe {
				return Lua.Functions.lua_equal(L, index1, index2) != 0;
			}
		}

		/// <summary>
		/// Tests if the two values on the stack are equal, ignoring any metamethods.
		/// </summary>
		/// <param name="index1">First value index</param>
		/// <param name="index2">Second value index</param>
		/// <returns>If the two values are equal</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool RawEqual(int index1, int index2) {
			unsafe {
				return Lua.Functions.lua_rawequal(L, index1, index2) != 0;
			}
		}

		/// <summary>
		/// Tests if the first stack value is less than the second, potentially invoking the <c>__lt</c> metamethod.
		/// </summary>
		/// <param name="index1">First value index</param>
		/// <param name="index2">Second value index</param>
		/// <returns>If the first value is less than the second</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool LessThan(int index1, int index2) {
			unsafe {
				return Lua.Functions.lua_lessthan(L, index1, index2) != 0;
			}
		}

		/// <summary>
		/// Converts the given stack value to a number, returning 0 if the conversion fails.
		/// </summary>
		/// <param name="index">Value index</param>
		/// <returns>Number value, or 0</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaNumber ToNumber(int index) {
			unsafe {
				return Lua.Functions.lua_tonumber(L, index);
			}
		}

		/// <summary>
		/// Converts the given stack value to an integer, returning 0 if the conversion fails.
		/// </summary>
		/// <param name="index">Value index</param>
		/// <returns>Integer value, or 0</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaInteger ToInteger(int index) {
			unsafe {
				return Lua.Functions.lua_tointeger(L, index);
			}
		}

		/// <summary>
		/// Converts the given stack value to a boolean, returning <c>false</c> if the conversion fails.
		/// </summary>
		/// <param name="index">Value index</param>
		/// <returns>Boolean value, or <c>false</c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool ToBoolean(int index) {
			unsafe {
				return Lua.Functions.lua_toboolean(L, index) != 0;
			}
		}

		/// <summary>
		/// Converts the given stack value to a string, returning <c>null</c> if the conversion fails.
		/// </summary>
		/// <param name="index">Value index</param>
		/// <returns>String value, or <c>null</c></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? ToString(int index) {
			unsafe {
				IntPtr pstr = Lua.Functions.lua_tolstring(L, index, out nuint length);
#if DEBUG
				Debug.Assert(length < int.MaxValue);
#endif
				return MemoryUtil.GetUTF8(pstr, (int)length);
			}
		}

		/// <summary>
		/// Converts the given stack value to the raw bytes of a string, returning an empty span
		/// if the conversion fails.
		/// </summary>
		/// <param name="index">Value index</param>
		/// <returns>String value bytes, or an empty span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<byte> ToStringBytes(int index) {
			unsafe {
				IntPtr pstr = Lua.Functions.lua_tolstring(L, index, out nuint length);
#if DEBUG
				Debug.Assert(length < int.MaxValue);
#endif
				return new Span<byte>((void*)pstr, (int)length);
			}
		}

		/// <summary>
		/// Gets the "length" of the given value for the following rules:
		/// <list type="bullet">
		/// <item>For strings, the length in bytes</item>
		/// <item>For tables, the result of the length ('<c>#</c>') operator</item>
		/// <item>For userdata, the byte size of its memory</item>
		/// <item>For all other types, 0</item>
		/// </list>
		/// </summary>
		/// <param name="index">The value to get the length of</param>
		/// <returns>The length of the object</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public nuint ObjLen(int index) {
			unsafe {
				return Lua.Functions.lua_objlen(L, index);
			}
		}

		/// <summary>
		/// Gets the given userdata value on the stack, either the raw value itself for light
		/// userdata or the pointer to the userdata memory for normal userdata. Returns zero otherwise.
		/// </summary>
		/// <param name="index">The value to get the userdata for</param>
		/// <returns>Userdata value, or zero</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public nint ToUserdata(int index) {
			unsafe {
				return Lua.Functions.lua_touserdata(L, index);
			}
		}

		/// <summary>
		/// Gets the given userdata value as a reference to the allocated memory. The reference will remain
		/// valid until the userdata is garbage collected by the Lua runtime.
		/// </summary>
		/// <typeparam name="T">The userdata type</typeparam>
		/// <param name="index">The value to get the userdata for</param>
		/// <returns>Userdata reference</returns>
#if RELEASE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
		public ref T ToUserdata<T>(int index) where T : unmanaged {
			unsafe {
#if DEBUG
				// In debug mode, make sure the value is genuine userdata and of the correct size
				Debug.Assert(Type(index) == LuaType.UserData);
				Debug.Assert(ObjLen(index) == (nuint)sizeof(T));
#endif
				IntPtr pUD = Lua.Functions.lua_touserdata(L, index);
				if (pUD == IntPtr.Zero) throw new NullReferenceException("Value is not valid userdata");
				return ref *(T*)pUD;
			}
		}

		// TODO: lua_tothread

		/// <summary>
		/// Converts the given value to an opaque pointer.
		/// </summary>
		/// <param name="index">Value to convert to pointer</param>
		/// <returns>Value pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr ToPointer(int index) {
			unsafe {
				return Lua.Functions.lua_topointer(L, index);
			}
		}

		//=============================//
		// Push Functions (C -> stack) //
		//=============================//

		/// <summary>
		/// Pushes a nil value on top of the stack.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushNil() {
			unsafe {
				Lua.Functions.lua_pushnil(L);
			}
		}

		/// <summary>
		/// Pushes a number on top of the stack.
		/// </summary>
		/// <param name="number">The number to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushNumber(LuaNumber number) {
			unsafe {
				Lua.Functions.lua_pushnumber(L, number);
			}
		}

		/// <summary>
		/// Pushes a number on top of the stack.
		/// </summary>
		/// <param name="number">The number to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushInteger(LuaInteger integer) {
			unsafe {
				Lua.Functions.lua_pushinteger(L, integer);
			}
		}

		/// <summary>
		/// Pushes a string on top of the stack.
		/// </summary>
		/// <param name="str">The string to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushString(ReadOnlySpan<char> str) => PushString(MemoryUtil.StackallocUTF8(str, stackalloc byte[1024]));

		/// <summary>
		/// Pushes a string of bytes as a string value.
		/// </summary>
		/// <param name="bytes">String bytes to push as a string</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushString(ReadOnlySpan<byte> bytes) {
			unsafe {
				fixed (byte* pBytes = bytes) {
					Lua.Functions.lua_pushlstring(L, (IntPtr)pBytes, (nuint)bytes.Length);
				}
			}
		}

		/// <summary>
		/// Pushes an unmanaged function on the stack.
		/// </summary>
		/// <param name="func">Unmanaged function to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void PushFunction(delegate* unmanaged<IntPtr, int> func) => PushClosure(func, 0);

		/// <summary>
		/// Pushes an unmanaged closure on the stack, poping the
		/// given number of upvalues from the stack.
		/// </summary>
		/// <param name="func">Unmanged function to push</param>
		/// <param name="n">Number of upvalues to bundle with the function</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void PushClosure(delegate* unmanaged<IntPtr, int> func, int n) => Lua.Functions.lua_pushcclosure(L, func, n);

		// Trampoline function for "raw" delegate closures (with no upvalues)
		[UnmanagedCallersOnly]
		private static int ClosureRawTrampoline(IntPtr pState) {
			// Get the corresponding state object
			LuaBase lua = Get(pState);
			// Get the delegate handle from the first upvalue
			GCHandle hFunc = GCHandle.FromIntPtr(lua.ToUserdata<IntPtr>(Lua.UpValueIndex(1)));
			LuaFunction func = (LuaFunction)hFunc.Target!;
			// Invoke the delegate, passing its return value (converting C# exceptions)
			Exception ex;
			try {
				return func(lua);
			} catch (Exception e) {
				lua.CaptureManagedException(e);
				ex = e;
			}
			lua.Error("C# error: " + ex.Message);
			return 0;
		}

		// Inner trampoline function for delegate closures with upvalues
		[UnmanagedCallersOnly]
		private static int ClosureInnerTrampoline(IntPtr pState) {
			// Get the corresponding state object
			LuaBase lua = Get(pState);
			// Get the delegate handle from the first argument
			GCHandle hFunc = GCHandle.FromIntPtr(lua.ToUserdata<IntPtr>(1));
			LuaFunction func = (LuaFunction)hFunc.Target!;
			// Remove the first argument to match the calling environment
			lua.Remove(1);
			// Invoke the delegate, passing its return value (converting c# exceptions)
			Exception ex;
			try {
				return func(lua);
			} catch (Exception e) {
				lua.CaptureManagedException(e);
				ex = e;
			}
			lua.Error("C# error: " + ex.Message);
			return 0;
		}

		// Outer trampoline function for delegate closures with upvalues
		[UnmanagedCallersOnly]
		private static int ClosureOuterTrampoline(IntPtr pState) {
			// Get the corresponding state object
			LuaBase lua = Get(pState);
			// Count the number of arguments passed to this function
			int argc = lua.Top;
			// Insert the inner function and delegate handle before the arguments
			lua.PushValue(Lua.UpValueIndex(1));
			lua.Insert(1);
			lua.PushValue(Lua.UpValueIndex(2));
			lua.Insert(2);
			// Perform the function call and return based on the remaining values on the stack
			lua.Call(argc + 1, Lua.MultRet);
			return lua.Top;
		}

		[UnmanagedCallersOnly]
		private static unsafe int ClosureGCCallback(IntPtr pState) {
			// Get the delegate handle from the upvalue
			IntPtr pFunc = Lua.Functions.lua_touserdata(pState, 1);
			GCHandle hFunc = GCHandle.FromIntPtr(Marshal.ReadIntPtr(pFunc));
			// Free the delegate handle
			hFunc.Free();
			return 0;
		}

		/// <summary>
		/// Pushes a managed function on the stack.
		/// </summary>
		/// <param name="func">Managed function to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushFunction(LuaFunction func) => PushClosure(func, 0);

		/// <summary>
		/// Pushes a managed closure on the stack, poping the given number of
		/// upvalues from the stack.
		/// </summary>
		/// <param name="func">Managed function to push</param>
		/// <param name="n">Number of upvalues to bundle with the function</param>
		public void PushClosure(LuaFunction func, int n) {
			unsafe {
				var lua = Lua.Functions;
				GCHandle hFunc = GCHandle.Alloc(func);

				void PushFuncObj() {
					// Create userdata object to hold the GCHandle value
					NewUserdata<IntPtr>() = (IntPtr)hFunc;
					var lua = Lua.Functions;
					// Set its metatable to free the GCHandle on Lua GC
					if (NewMetatable("__gcdelegate"u8)) {
						lua.lua_pushcclosure(L, &ClosureGCCallback, 0);
						SetField(-2, "__gc"u8);
					}
					lua.lua_setmetatable(L, -2);
				}

				if (n == 0) {
					// No upvalues to worry about, we can do this with a single trampoline
					// The only upvalue is the delegate handle
					PushFuncObj();
					lua.lua_pushcclosure(L, &ClosureRawTrampoline, 1);
				} else {
					// To preserve upvalues properly we need nested trampolines
					// The inner holds the upvalues the function expects
					lua.lua_pushcclosure(L, &ClosureInnerTrampoline, n);
					// The outer holds the inner wrapper as its first upvalue and the delegate handle as its second
					// It will pass the delegate handle as a hidden first argument
					PushFuncObj();
					lua.lua_pushcclosure(L, &ClosureOuterTrampoline, 2);
				}

			}
		}

		/// <summary>
		/// Pushes a boolean on top of the stack.
		/// </summary>
		/// <param name="number">The boolean value to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushBoolean(bool b) {
			unsafe {
				Lua.Functions.lua_pushboolean(L, b ? 1 : 0);
			}
		}

		/// <summary>
		/// Pushes a light-userdata value on top of the stack.
		/// </summary>
		/// <param name="number">The userdata to push</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushLightUserdata(nint ud) {
			unsafe {
				Lua.Functions.lua_pushlightuserdata(L, ud);
			}
		}

		/// <summary>
		/// Pushes the current thread onto its own stack, returning if it is the "main" thread.
		/// </summary>
		/// <returns>If the pushed thread is the main thread</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool PushThread() {
			unsafe {
				return Lua.Functions.lua_pushthread(L) != 0;
			}
		}

		//==============================//
		// Get Functions (Lua -> stack) //
		//==============================//

		/// <summary>
		/// Reads from the table at the given index, using the value on top
		/// of the stack as the key and replacing it with the retrieved value.
		/// </summary>
		/// <param name="index">Table stack index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTable(int index) {
			unsafe {
				Lua.Functions.lua_gettable(L, index);
			}
		}

		/// <summary>
		/// Reads from the table at the given index, using the given string
		/// key and pushing the retrieved value.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="key">Key string</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetField(int index, ReadOnlySpan<char> key) => GetField(index, MemoryUtil.StackallocUTF8(key, stackalloc byte[1024]));

		/// <summary>
		/// Reads from the table at the given index, using the given string
		/// key and pushing the retrieved value.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="key">Key string</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetField(int index, ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = Lua.CheckStr(key, stackalloc byte[1024])) {
					Lua.Functions.lua_getfield(L, index, (IntPtr)pKey);
				}
			}
		}

		/// <summary>
		/// Reads from the table at the given index, bypassing any metamethods
		/// set on the table, using the value on top of the stack as the key
		/// and replacing it with the retrieved value.
		/// </summary>
		/// <param name="index">Table stack index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RawGet(int index) {
			unsafe {
				Lua.Functions.lua_rawget(L, index);
			}
		}

		/// <summary>
		/// Reads from the table at the given index, bypassing any metamethods
		/// set on the table, using the given number as an index into the table
		/// and pushing the retreived value.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="n">Index value into the table</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RawGet(int index, int n) {
			unsafe {
				Lua.Functions.lua_rawgeti(L, index, n);
			}
		}

		/// <summary>
		/// Creates a new table, pushing it onto the stack.
		/// </summary>
		/// <param name="narr">Number of array entries to reserve in the table</param>
		/// <param name="nrec">Number of hash entries to reserve in the table</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CreateTable(int narr = 0, int nrec = 0) {
			unsafe {
				Lua.Functions.lua_createtable(L, narr, nrec);
			}
		}

		/// <summary>
		/// Pushes a new userdata value on top of the stack, returning a reference to the allocated memory for it.
		/// The reference will remain valid until the userdata is garbage collected by the Lua runtime.
		/// </summary>
		/// <typeparam name="T">The userdata type</typeparam>
		/// <returns>Reference to the created userdata</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T NewUserdata<T>() where T : unmanaged {
			unsafe {
				return ref *(T*)Lua.Functions.lua_newuserdata(L, (nuint)sizeof(T));
			}
		}

		/// <summary>
		/// Attempts to get the metatable of the given value, pushing it onto the stack
		/// if successful, otherwise nothing is pushed. Returns if a metatable exists.
		/// </summary>
		/// <param name="index">Value stack index</param>
		/// <returns>If the metatable was retrieved and pushed</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetMetatable(int index) {
			unsafe {
				return Lua.Functions.lua_getmetatable(L, index) != 0;
			}
		}

		/// <summary>
		/// Pushes the environment table of the given value onto the stack.
		/// </summary>
		/// <param name="index">Value stack index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetFEnv(int index) {
			unsafe {
				Lua.Functions.lua_getfenv(L, index);
			}
		}

		//==============================//
		// Set Functions (stack -> Lua) //
		//==============================//

		/// <summary>
		/// Pops a value and key off the stack (in that order) and stores them
		/// in the table at the given index.
		/// </summary>
		/// <param name="index">Table stack index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetTable(int index) {
			unsafe {
				Lua.Functions.lua_settable(L, index);
			}
		}

		/// <summary>
		/// Pops a value off the stack and stores it in the table at the given
		/// index under the given name.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="key">Key string</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetField(int index, ReadOnlySpan<char> key) => SetField(index, MemoryUtil.StackallocUTF8(key, stackalloc byte[1024]));

		/// <summary>
		/// Pops a value off the stack and stores it in the table at the given
		/// index under the given name.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="key">Key string</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetField(int index, ReadOnlySpan<byte> key) {
			unsafe {
				fixed (byte* pKey = Lua.CheckStr(key, stackalloc byte[1024])) {
					Lua.Functions.lua_setfield(L, index, (IntPtr)pKey);
				}
			}
		}

		/// <summary>
		/// Identical to <see cref="SetTable(int)"/> but bypasses any metamethods.
		/// </summary>
		/// <param name="index">Table stack index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RawSet(int index) {
			unsafe {
				Lua.Functions.lua_rawset(L, index);
			}
		}

		/// <summary>
		/// Pops a value off the stack and stores it in the table at the given
		/// stack index with the number specifying the index in the table to store at,
		/// and bypassing any metamethods.
		/// </summary>
		/// <param name="index">Table stack index</param>
		/// <param name="n">Index in table to store at</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RawSet(int index, int n) {
			unsafe {
				Lua.Functions.lua_rawseti(L, index, n);
			}
		}

		/// <summary>
		/// Sets the metatable of the given value to the table on top
		/// of the stack, poping the top value.
		/// </summary>
		/// <param name="index">Stack value index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetMetatable(int index) {
			unsafe {
				Lua.Functions.lua_setmetatable(L, index);
			}
		}

		/// <summary>
		/// Sets the environment table of the given value to the table on top
		/// of the stack, poping the top value.
		/// </summary>
		/// <param name="index">Stack value index</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetFEnv(int index) {
			unsafe {
				Lua.Functions.lua_setfenv(L, index);
			}
		}

		//=========================//
		// Load and Call Functions //
		//=========================//

		/// <summary>
		/// Performs a Lua function call with the given number of arguments to the function just
		/// below them on the stack.
		/// </summary>
		/// <param name="nargs">The number of arguments to pass to the function</param>
		/// <param name="nresults">The number of results to push</param>
		/// <exception cref="LuaException">If there is an error calling the function</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Call(int nargs = 0, int nresults = Lua.MultRet) {
			// Do a protected call anyway to make sure things get unwound correctly
			var status = ProtectedCall(nargs, nresults);
			string message;
			switch (status) {
				case LuaStatus.Ok:
					return;
				// Throw exceptions if there was an error
				case LuaStatus.ErrMem:
					throw new LuaException("Out of memory");
				case LuaStatus.ErrRun:
					message = ToString(-1) ?? "<Unknown Error>";
					Pop(1);
					throw new LuaException(message);
				default:
					throw new LuaException("Unexpected Lua error: " + status);
			}
		}

		/// <summary>
		/// Performs a protected function call similar to <see cref="Call(int, int)"/>, but
		/// not unwinding any further on error. An optional error handler function may be
		/// passed on the stack which is called with the error value if one is generated.
		/// If there is an error, the apropriate return value is returned and if it is a
		/// runtime error, the error value is pushed onto the stack instead of the results
		/// of the function call. If an error handler function is provided it is given this
		/// value first and may mutate it before returning the error value pushed on the stack.
		/// </summary>
		/// <param name="nargs">The number of arguments to pass to the function</param>
		/// <param name="nresults">The number of results to push</param>
		/// <param name="errfunc">The stack index of the error handler function, or zero to ignore</param>
		/// <returns>The status of the protected call</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual LuaStatus ProtectedCall(int nargs = 0, int nresults = Lua.MultRet, int errfunc = 0) {
			unsafe {
				return (LuaStatus)Lua.Functions.lua_pcall(L, nargs, nresults, ToAbsoluteIndex(errfunc));
			}
		}

		[UnmanagedCallersOnly]
		private static int ProtectedCallTrampoline(IntPtr pState) {
			LuaBase lua = LuaState.Get(pState);
			GCHandle hFunc = GCHandle.FromIntPtr(lua.ToUserdata(1));
			LuaFunction func = (LuaFunction)hFunc.Target!;
			Exception ex;
			try {
				return func(lua);
			} catch (Exception e) {
				lua.CaptureManagedException(e);
				ex = e;
			}
			lua.Error("C# exception: " + ex.Message);
			return 0;
		}

		/// <summary>
		/// Performs a protected call to the given function, passing no arguments and
		/// discarding all return values. If the function generates a runtime error
		/// it pushes it on the stack before returning.
		/// </summary>
		/// <param name="func">The function to call</param>
		/// <returns>The status of the protected call</returns>
		public LuaStatus ProtectedCall(LuaFunction func) {
			unsafe {
				GCHandle hFunc = GCHandle.Alloc(func);
				LuaStatus status = (LuaStatus)Lua.Functions.lua_cpcall(L, &ProtectedCallTrampoline, (nint)hFunc);
				hFunc.Free();
				return status;
			}
		}

		// Handle holding the memory currently being loaded
		private GCHandle lastReaderMemory;

		[UnmanagedCallersOnly]
		private static unsafe IntPtr LoadTrampoline(IntPtr pState, IntPtr ud, nuint* size) {
			LuaBase lua = Get(pState);
			GCHandle hReader = GCHandle.FromIntPtr(ud);
			LuaReader reader = (LuaReader)hReader.Target!;

			try {
				ReadOnlyMemory<byte> data = reader(lua);
				if (lua.lastReaderMemory.IsAllocated) lua.lastReaderMemory.Free();
				if (data.IsEmpty) {
					*size = 0;
					return IntPtr.Zero;
				} else {
					lua.lastReaderMemory = GCHandle.Alloc(data, GCHandleType.Pinned);
					*size = (nuint)data.Length;
					return lua.lastReaderMemory.AddrOfPinnedObject();
				}
			} catch (Exception) {
				*size = 0;
				return IntPtr.Zero;
			}
		}

		/// <summary>
		/// Attempts to load a chunk of Lua code using the given reader.
		/// </summary>
		/// <param name="reader">The reader to read code from</param>
		/// <param name="chunkName">The name of the chunk being loaded</param>
		/// <returns>The status of the load operation</returns>
		public LuaStatus Load(LuaReader reader, ReadOnlySpan<char> chunkName) {
			unsafe {
				GCHandle hReader = GCHandle.Alloc(reader);
				LuaStatus status;
				try {
					fixed (byte* pChunkName = MemoryUtil.StackallocUTF8(chunkName, stackalloc byte[1024])) {
						status = (LuaStatus)Lua.Functions.lua_load(L, &LoadTrampoline, (nint)hReader, (nint)pChunkName);
					}
				} finally {
					if (lastReaderMemory.IsAllocated) lastReaderMemory.Free();
					hReader.Free();
				}
				return status;
			}
		}

		/// <summary>
		/// Attempts to load a chunk of Lua code using the given reader.
		/// </summary>
		/// <param name="reader">The reader to read code from</param>
		/// <param name="chunkName">The name of the chunk being loaded</param>
		/// <returns>The status of the load operation</returns>
		public LuaStatus Load(LuaReader reader, ReadOnlySpan<byte> chunkName) {
			unsafe {
				GCHandle hReader = GCHandle.Alloc(reader);
				LuaStatus status;
				try {
					fixed (byte* pChunkName = Lua.CheckStr(chunkName, stackalloc byte[1024])) {
						status = (LuaStatus)Lua.Functions.lua_load(L, &LoadTrampoline, (nint)hReader, (nint)pChunkName);
					}
				} finally {
					if (lastReaderMemory.IsAllocated) lastReaderMemory.Free();
					hReader.Free();
				}
				return status;
			}
		}

		//=====================//
		// Coroutine Functions //
		//=====================//

		/// <summary>
		/// Yields a coroutine. This should be used as the return expression of a function. When
		/// called in this way, the running coroutine is suspended and execution returns to the
		/// <see cref="Resume(int)"/> that started this coroutine.
		/// </summary>
		/// <param name="nresults">The number of results to pop and return</param>
		/// <returns>The return value to pass</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Yield(int nresults) {
			unsafe {
				return Lua.Functions.lua_yield(L, nresults);
			}
		}

		/// <summary>
		/// Starts a new coroutine on the current thread, returning when it yields or finishes
		/// execution. The given number of arguments are used like a normal invocation of
		/// <see cref="Call(int, int)"/> and any values returned from <see cref="Yield(int)"/>
		/// are pushed onto the stack when this function returns. If an error occurs, the stack
		/// is not unwound and can be debugged.
		/// </summary>
		/// <param name="narg">The number of arguments to pass to the coroutine</param>
		/// <returns>The result status of the coroutine</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaStatus Resume(int narg) {
			unsafe {
				return (LuaStatus)Lua.Functions.lua_resume(L, narg);
			}
		}

		/// <summary>
		/// The status of the current thread.
		/// </summary>
		public LuaStatus Status {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get {
				unsafe {
					return (LuaStatus)Lua.Functions.lua_status(L);
				}
			}
		}

		//=========================//
		// Miscellaneous Functions //
		//=========================//

		/// <summary>
		/// Controls the Lua runtime's garbage collector based on the <see cref="LuaGC"/>
		/// parameter passed to it.
		/// </summary>
		/// <param name="what">The operation to perform</param>
		/// <param name="data">Any extra data to pass to the operation</param>
		/// <returns>Any result value from the operation</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GC(LuaGC what, int data = 0) {
			unsafe {
				return Lua.Functions.lua_gc(L, (int)what, data);
			}
		}

		/// <summary>
		/// Pops the topmost value from the stack and generates a Lua error. This will perform
		/// a <c>long_jmp</c> in native code (although a library like LuaJIT <i>may</i> unwind
		/// the stack properly). However, this should not be relied on and user-code should
		/// throw a managed exception instead (as managed functions registered in Lua will
		/// catch any exceptions and convert them to errors before returning)
		/// </summary>
		/// <exception cref="Exception">If the Lua error function does not throw properly</exception>
		[DoesNotReturn]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void Error() {
			unsafe {
				Lua.Functions.lua_error(L);
			}
			throw new Exception("lua_error returned!");
		}

		/// <summary>
		/// Generates a Lua error by pushing the message string and invoking <see cref="Error()"/>.
		/// </summary>
		/// <param name="message">Error message</param>
		[DoesNotReturn]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void Error(ReadOnlySpan<char> message) {
			PushString(message);
			Error();
		}

		/// <summary>
		/// Pops a key from the stack and pushes the key and value from the table at the given index.
		/// If there are no more elements in the table, nothing is pushed and false is returned. If
		/// the key is nil this will go to the first key-value pair in the table.
		/// </summary>
		/// <param name="index">Index of the table to traverse</param>
		/// <returns>If another key-value pair was pushed</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Next(int index) {
			unsafe {
				return Lua.Functions.lua_next(L, index) != 0;
			}
		}

		/// <summary>
		/// Concatenates the topmost values on the stack (potentially invoking metamethods), leaving
		/// the concatenated value on top of the stack.
		/// </summary>
		/// <param name="n">The number of values to concatenate</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Concat(int n) {
			unsafe {
				Lua.Functions.lua_concat(L, n);
			}
		}

		//=================//
		// Macro Functions //
		//=================//

		/// <summary>
		/// Pops one or more values from the stack.
		/// </summary>
		/// <param name="n">The number of values to pop</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Pop(int n = 1) => Top = -n - 1;

		/// <summary>
		/// Tests if the value at the given stack index is nil.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>If the value is nil</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNil(int index) => Type(index) == LuaType.Nil;

		/// <summary>
		/// Tests if the value at the given stack index is a boolean.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>If the value is a boolean</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsBoolean(int index) => Type(index) == LuaType.Boolean;

		/// <summary>
		/// Tests if the value at the given stack index is a function.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>If the value is a function</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsFunction(int index) => Type(index) == LuaType.Function;

		/// <summary>
		/// Tests if the value at the given stack index is a table.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>If the value is a table</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsTable(int index) => Type(index) == LuaType.Table;

		/// <summary>
		/// Tests if the value at the given stack index is light userdata.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>If the value is light userdata</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsLightUserdata(int index) => Type(index) == LuaType.LightUserData;

		/// <summary>
		/// Registers the given function globally under the given name.
		/// </summary>
		/// <param name="name">The name to register the function under</param>
		/// <param name="func">The function to register</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Register(ReadOnlySpan<byte> name, delegate* unmanaged<IntPtr, int> func) {
			PushFunction(func);
			SetGlobal(name);
		}

		/// <summary>
		/// Registers the given function globally under the given name.
		/// </summary>
		/// <param name="name">The name to register the function under</param>
		/// <param name="func">The function to register</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Register(ReadOnlySpan<char> name, delegate* unmanaged<IntPtr, int> func) {
			PushFunction(func);
			SetGlobal(name);
		}

		/// <summary>
		/// Registers the given function globally under the given name.
		/// </summary>
		/// <param name="name">The name to register the function under</param>
		/// <param name="func">The function to register</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Register(ReadOnlySpan<byte> name, LuaFunction func) {
			PushFunction(func);
			SetGlobal(name);
		}

		/// <summary>
		/// Registers the given function globally under the given name.
		/// </summary>
		/// <param name="name">The name to register the function under</param>
		/// <param name="func">The function to register</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Register(ReadOnlySpan<char> name, LuaFunction func) {
			PushFunction(func);
			SetGlobal(name);
		}

		/// <summary>
		/// Pops a value and stores it as a global variable under the given name.
		/// </summary>
		/// <param name="name">Global variable name</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetGlobal(ReadOnlySpan<byte> name) => SetField(Lua.GlobalsIndex, name);

		/// <summary>
		/// Pops a value and stores it as a global variable under the given name.
		/// </summary>
		/// <param name="name">Global variable name</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetGlobal(ReadOnlySpan<char> name) => SetField(Lua.GlobalsIndex, name);

		/// <summary>
		/// Gets a global variable by name and pushes it on the stack.
		/// </summary>
		/// <param name="name">Global variable name</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetGlobal(ReadOnlySpan<byte> name) => GetField(Lua.GlobalsIndex, name);

		/// <summary>
		/// Gets a global variable by name and pushes it on the stack.
		/// </summary>
		/// <param name="name">Global variable name</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetGlobal(ReadOnlySpan<char> name) => GetField(Lua.GlobalsIndex, name);

		/// <summary>
		/// Converts a potentially top-relative stack index to an absolute index.
		/// </summary>
		/// <param name="index">Index to convert</param>
		/// <returns>Absolute index</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ToAbsoluteIndex(int index) => index < 0 && index > Lua.RegistryIndex ? Top + index + 1 : index;

		//==================//
		// Standard Library //
		//==================//

		/// <summary>
		/// Opens the base package.
		/// Note: There are some functions in the base package that may be undesirable
		/// for sandboxed code (eg. <c>dofile</c>, <c>loadfile</c>, etc.)
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenBase() {
			unsafe {
				Lua.Functions.luaopen_base(L);
			}
		}

		/// <summary>
		/// Opens the math package.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenMath() {
			unsafe {
				Lua.Functions.luaopen_math(L);
			}
		}

		/// <summary>
		/// Opens the string package.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenString() {
			unsafe {
				Lua.Functions.luaopen_string(L);
			}
		}

		/// <summary>
		/// Opens the table package.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenTable() {
			unsafe {
				Lua.Functions.luaopen_table(L);
			}
		}

		/// <summary>
		/// Opens the input/output package.
		/// Note: This may be undesriable for sandboxed environments.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenIO() {
			unsafe {
				Lua.Functions.luaopen_io(L);
			}
		}

		/// <summary>
		/// Opens the operating system interface package.
		/// Note: This may be undesriable for sandboxed environments.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenOS() {
			unsafe {
				Lua.Functions.luaopen_os(L);
			}
		}

		/// <summary>
		/// Opens the package manager package.
		/// Note: This may be undesriable for sandboxed environments.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenPackage() {
			unsafe {
				Lua.Functions.luaopen_package(L);
			}
		}

		/// <summary>
		/// Opens the debugging package, throwing an error if it is not available.
		/// Note: This may be undesriable for sandboxed environments.
		/// </summary>
		/// <exception cref="LuaException">If the debug package is not available</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenDebug() {
			unsafe {
				if (Lua.Functions.luaopen_debug != default) Lua.Functions.luaopen_debug(L);
				else throw new LuaException("Lua library does not support debug package");
			}
		}

		/// <summary>
		/// Opens the bitwise-operator package, throwing an error if it is not available.
		/// </summary>
		/// <exception cref="LuaException">If the bitwise-operator package is not available</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OpenBit() {
			unsafe {
				if (Lua.Functions.luaopen_bit != default) Lua.Functions.luaopen_bit(L);
				else throw new LuaException("Lua library does not support bitwise-operator package");
			}
		}

		/// <summary>
		/// Opens all available packages in the Lua environment.
		/// Note: This may be undesriable for sandboxed environments.
		/// </summary>
		public void OpenAll() {
			unsafe {
				var lua = Lua.Functions;
				// Try luaL_openlibs first
				if (lua.luaL_openlibs != default) lua.luaL_openlibs(L);
				// Else if the library doesn't support that do it manually
				else {
					lua.luaopen_base(L);
					lua.luaopen_math(L);
					lua.luaopen_string(L);
					lua.luaopen_table(L);
					lua.luaopen_io(L);
					lua.luaopen_os(L);
					lua.luaopen_package(L);
					if (lua.luaopen_debug != default) lua.luaopen_debug(L);
					if (lua.luaopen_bit != default) lua.luaopen_bit(L);
				}
			}
		}

		//===================//
		// Auxiliary Library //
		//===================//

		/// <summary>
		/// Generates an error indicating the value at the given index is not of an expected type.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <param name="tname">Expected type name</param>
		/// <exception cref="LuaException">The Lua error</exception>
		[DoesNotReturn]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Maintain instance-based invocation as with luaL_typeerror")]
		public void TypeError(int index, ReadOnlySpan<byte> tname) =>
			throw new LuaException($"Value #{index} is not of correct type, expected {Encoding.UTF8.GetString(tname)}");

		/// <summary>
		/// Generates an error indicating the value at the given index is not of an expected type.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <param name="tname">Expected type name</param>
		/// <exception cref="LuaException">The Lua error</exception>
		[DoesNotReturn]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Maintain instance-based invocation as with luaL_typeerror")]
		public void TypeError(int index, ReadOnlySpan<char> tname) =>
			throw new LuaException($"Value #{index} is not of correct type, expected {new string(tname)}");

		/// <summary>
		/// Checks if the value at the given index is a string, generating an error if not.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The string value if present</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string CheckString(int index) {
			if (!IsString(index)) TypeError(index, "string");
			return ToString(index)!;
		}

		/// <summary>
		/// Checks if the value at the given index is a string, generating an error if not.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The string value if present</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<byte> CheckStringBytes(int index) {
			if (!IsString(index)) TypeError(index, "string");
			return ToStringBytes(index);
		}

		/// <summary>
		/// Checks if the value at the given index is a string, returning it if so and null otherwise.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The string value or null</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? OptString(int index) {
			if (!IsString(index)) return null;
			return ToString(index);
		}

		/// <summary>
		/// Checks if the value at the given index is a number, generating an error if not.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The number value if present</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaNumber CheckNumber(int index) {
			if (!IsNumber(index)) TypeError(index, "number");
			return ToNumber(index);
		}

		/// <summary>
		/// Checks if the value at the given index is a number, returning it if so and null otherwise.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The number value or null</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaNumber? OptNumber(int index) => IsNumber(index) ? ToNumber(index) : null;

		/// <summary>
		/// Checks if the value at the given index is a number, generating an error if not.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The number value (as an integer) if present</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaInteger CheckInteger(int index) {
			if (!IsNumber(index)) TypeError(index, "number");
			return ToInteger(index);
		}

		/// <summary>
		/// Checks if the value at the given index is a number, returning it if so and null otherwise.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The number value (as an integer) or null</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LuaInteger? OptInteger(int index) => IsNumber(index) ? ToInteger(index) : null;

		/// <summary>
		/// Registers a metatable under the given type name, pushing it onto the stack and
		/// returning if this is the first use of the table.
		/// </summary>
		/// <param name="tname">Metatable type name</param>
		/// <returns>If the metatable was just created</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool NewMetatable(ReadOnlySpan<byte> tname) {
			unsafe {
				var lua = Lua.Functions;
				// Use luaL_newmetatable if available
				if (lua.luaL_newmetatable != default) {
					fixed (byte* pName = Lua.CheckStr(tname, stackalloc byte[1024])) {
						return lua.luaL_newmetatable(L, (IntPtr)pName) != 0;
					}
				} else {
					// Else replicate its functionality by instantiating the named table in the registry
					fixed (byte* pName = Lua.CheckStr(tname, stackalloc byte[1024])) {
						lua.lua_getfield(L, Lua.RegistryIndex, (IntPtr)pName);
						if (lua.lua_type(L, -1) != (int)LuaType.Table) {
							lua.lua_settop(L, -2);
							lua.lua_createtable(L, 0, 0);
							lua.lua_pushvalue(L, -1);
							lua.lua_setfield(L, Lua.RegistryIndex, (IntPtr)pName);
							return true;
						} else return false;
					}
				}
			}
		}

		/// <summary>
		/// Registers a metatable under the given type name, pushing it onto the stack and
		/// returning if this is the first use of the table.
		/// </summary>
		/// <param name="tname">Metatable type name</param>
		/// <returns>If the metatable was just created</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool NewMetatable(ReadOnlySpan<char> tname) => NewMetatable(MemoryUtil.StackallocUTF8(tname, stackalloc byte[1024]));

		/// <summary>
		/// Tests if the value at the given index is userdata of the apropriate type,
		/// returning a pointer to it if it is correct, else returning null.
		/// </summary>
		/// <typeparam name="T">Userdata type</typeparam>
		/// <param name="index">Userdata value index</param>
		/// <param name="tname">Userdata type name</param>
		/// <returns>Userdata pointer, or null</returns>
		public UnmanagedPointer<T> TestUserdata<T>(int index, ReadOnlySpan<byte> tname) where T : unmanaged {
			unsafe {
				var lua = Lua.Functions;
				// Use luaL_testudata if available
				if (lua.luaL_testudata != default) {
					fixed (byte* pName = Lua.CheckStr(tname, stackalloc byte[1024])) {
						return (UnmanagedPointer<T>)lua.luaL_testudata(L, (IntPtr)pName);
					}
				} else {
					// Else emulate the functionality, checking for raw equality of respective metatables
					if (lua.lua_type(L, index) != (int)LuaType.UserData) return default;
					lua.lua_getmetatable(L, index);
					fixed (byte* pName = Lua.CheckStr(tname, stackalloc byte[1024])) {
						lua.lua_getfield(L, Lua.RegistryIndex, (IntPtr)pName);
					}
					int eq = lua.lua_rawequal(L, -1, -2);
					lua.lua_settop(L, -3);
					return (UnmanagedPointer<T>)(eq == 0 ? IntPtr.Zero : lua.lua_touserdata(L, index));
				}
			}
		}

		/// <summary>
		/// Tests if the value at the given index is userdata of the apropriate type,
		/// returning a pointer to it if it is correct, else returning null.
		/// </summary>
		/// <typeparam name="T">Userdata type</typeparam>
		/// <param name="index">Userdata value index</param>
		/// <param name="tname">Userdata type name</param>
		/// <returns>Userdata pointer, or null</returns>
		public UnmanagedPointer<T> TestUserdata<T>(int index, ReadOnlySpan<char> tname) where T : unmanaged =>
			TestUserdata<T>(index, MemoryUtil.StackallocUTF8(tname, stackalloc byte[1024]));

		/// <summary>
		/// Checks if the given value on the stack is userdata of the apropriate type,
		/// generating a Lua error if not. If it is correct, it returns the userdata
		/// value by reference.
		/// </summary>
		/// <typeparam name="T">Userdata type</typeparam>
		/// <param name="index">Userdata value index</param>
		/// <param name="tname">Userdata type name</param>
		/// <returns>Userdata value reference</returns>
		/// <exception cref="LuaException">If the value is not the correct type</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T CheckUserdata<T>(int index, ReadOnlySpan<byte> tname) where T : unmanaged {
			UnmanagedPointer<T> pData = TestUserdata<T>(index, tname);
			if (!pData) TypeError(index, tname);
			unsafe {
				return ref *(T*)pData.Ptr;
			}
		}

		/// <summary>
		/// Checks if the given value on the stack is userdata of the apropriate type,
		/// generating a Lua error if not. If it is correct, it returns the userdata
		/// value by reference.
		/// </summary>
		/// <typeparam name="T">Userdata type</typeparam>
		/// <param name="index">Userdata value index</param>
		/// <param name="tname">Userdata type name</param>
		/// <returns>Userdata value reference</returns>
		/// <exception cref="LuaException">If the value is not the correct type</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T CheckUserdata<T>(int index, ReadOnlySpan<char> tname) where T : unmanaged {
			UnmanagedPointer<T> pData = TestUserdata<T>(index, tname);
			if (!pData) throw new LuaException($"Value #{index} is not of correct userdata type");
			unsafe {
				return ref *(T*)pData.Ptr;
			}
		}

		/// <summary>
		/// Loads a string of text as Lua code.
		/// </summary>
		/// <param name="str">The string of text to load</param>
		/// <returns>The status of the load operation</returns>
		public LuaStatus LoadString(ReadOnlySpan<byte> str) {
			unsafe {
				// Use luaL_loadstring if available, else use a custom function with lua_load
				if (Lua.Functions.luaL_loadstring != default) {
					fixed (byte* pStr = Lua.CheckStr(str, stackalloc byte[4096])) {
						return (LuaStatus)Lua.Functions.luaL_loadstring(L, (IntPtr)pStr);
					}
				} else {
					ReadOnlyMemory<byte> text = new byte[str.Length];
					return Load(state => text, str);
				}
			}
		}

		/// <summary>
		/// Loads a string of text as Lua code.
		/// </summary>
		/// <param name="str">The string of text to load</param>
		/// <returns>The status of the load operation</returns>
		public LuaStatus LoadString(ReadOnlySpan<char> str) => LoadString(MemoryUtil.StackallocUTF8(str, stackalloc byte[4096]));

		// Things that should be in the aux library but aren't

		/// <summary>
		/// Checks if the value at the given index is a boolean, returning it if so and null otherwise.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The boolean value or null</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool? OptBoolean(int index) {
			if (!IsBoolean(index)) return null;
			return ToBoolean(index);
		}

		/// <summary>
		/// Checks if the value at the given index is a boolean, generating an error if not.
		/// </summary>
		/// <param name="index">Stack value index</param>
		/// <returns>The boolean value if present</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CheckBoolean(int index) {
			if (!IsBoolean(index)) TypeError(index, "boolean");
			return ToBoolean(index);
		}


		//=================//
		// Debug Functions //
		//=================//

		/// <summary>
		/// <para>Gets information about a specific function/invocation.</para>
		/// <para>
		/// If the string defining the information to retrieve starts with a '>', it will pop
		/// a function from the stack and gather information for that. Otherwise it will use
		/// the existing information from a previous call to <see cref="GetInfo"/> or from a
		/// <see cref="LuaHook"/>. Each character in the string determines the fields to update
		/// in the structure or push on the stack:
		/// <list type="table">
		/// <item>
		/// <term>n</term>
		/// <description>Sets the <see cref="LuaDebug.Name"/> and <see cref="LuaDebug.NameWhat"/> values</description>
		/// </item>
		/// <item>
		/// <term>S</term>
		/// <description>Sets the <see cref="LuaDebug.Source"/>, <see cref="LuaDebug.ShortSrc"/>, <see cref="LuaDebug.LineDefined"/>,
		/// <see cref="LuaDebug.LastLineDefined"/>, and <see cref="LuaDebug.What"/> values</description>
		/// </item>
		/// <item>
		/// <term>l</term>
		/// <description>Sets the <see cref="LuaDebug.CurrentLine"/> value</description>
		/// </item>
		/// <item>
		/// <term>u</term>
		/// <description>Sets the <see cref="LuaDebug.NUps"/> value</description>
		/// </item>
		/// <item>
		/// <term>f</term>
		/// <description>Pushes the function referred to by the debug information</description>
		/// </item>
		/// <item>
		/// <term>L</term>
		/// <description>Pushes a table containing each line of executable code in the function indexed by their line number.</description>
		/// </item>
		/// </list>
		/// </para>
		/// </summary>
		/// <param name="what">String defining the information to get</param>
		/// <param name="info">The debug information structure</param>
		/// <returns>If the get operation was successful</returns>
		public bool GetInfo(ReadOnlySpan<char> what, ref LuaDebug info) {
			LuaDebug debug = new();
			int status;
			unsafe {
				fixed (byte* pWhat = MemoryUtil.StackallocUTF8(what, stackalloc byte[16])) {
					status = Lua.Functions.lua_getinfo(L, (IntPtr)pWhat, (IntPtr)(&debug));
				}
			}
			return status != 0;
		}

		/// <summary>
		/// Gets information about a variable local to the given function debugging info. Note that 
		/// these indices start at 1. This returns the name of the local value (if it exists) and
		/// pushes it on the stack.
		/// </summary>
		/// <param name="info">Debug info</param>
		/// <param name="n">Index of the local variable</param>
		/// <returns>String identifying the local variable, or null</returns>
		public string? GetLocal(in LuaDebug info, int n) {
			unsafe {
				fixed (LuaDebug* pInfo = &info) {
					return MemoryUtil.GetUTF8(Lua.Functions.lua_getlocal(L, (IntPtr)pInfo, n));
				}
			}
		}

		/// <summary>
		/// Gets information about the runtime stack, where <i>level</i> is the offset into
		/// the current call stack (ie. level 0 is the current function, level 1 is the
		/// caller, etc.) Returns whether the information was successfully retrieved.
		/// </summary>
		/// <param name="level">Function level</param>
		/// <param name="info">Function stack information</param>
		/// <returns>If the stack information was retrieved</returns>
		public bool GetStack(int level, out LuaDebug info) {
			unsafe {
				fixed (LuaDebug* pInfo = &info) {
					return Lua.Functions.lua_getstack(L, level, (IntPtr)pInfo) == 0;
				}
			}
		}

		/// <summary>
		/// Gets information about a closure's upvalue. This returns the name of the upvalue (or
		/// an empty string if it is for a C function) and pushes the value onto the stack. If the
		/// upvalue does not exist it returns null and does not push anything.
		/// </summary>
		/// <param name="funcIndex">The index of the closure function on the stack</param>
		/// <param name="n">The index of the upvalue</param>
		/// <returns>String identifying the upvalue, or null</returns>
		public string? GetUpValue(int funcIndex, int n) {
			unsafe {
				return MemoryUtil.GetUTF8(Lua.Functions.lua_getupvalue(L, funcIndex, n));
			}
		}

	}

}
