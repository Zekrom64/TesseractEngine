using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LuaJIT {

	/// <summary>
	/// A managed Lua runtime.
	/// </summary>
	public class LuaState : LuaBase, IDisposable {

		// The custom allocation function
		private readonly LuaAlloc? alloc = null;

		[UnmanagedCallersOnly]
		private static IntPtr AllocTrampoline(IntPtr ud, IntPtr ptr, nuint osize, nuint nsize) {
			// Use the userdata as a pointer to the state
			LuaState? self = (LuaState?)GCHandle.FromIntPtr(ud).Target;
			return self?.alloc?.Invoke(self, ptr, osize, nsize) ?? IntPtr.Zero;
		}

		/// <summary>
		/// Creates a new Lua runtime.
		/// </summary>
		/// <param name="alloc">The custom allocation function to use</param>
		public LuaState(LuaAlloc? alloc = null) {
			this.alloc = alloc;
			unsafe {
				var lua = Lua.Functions;
				// Determine the allocator function pointer
				delegate* unmanaged<IntPtr, IntPtr, nuint, nuint, IntPtr> palloc = alloc != null ? &AllocTrampoline : default;
				// Create the actual state
				L = lua.lua_newstate(palloc, (nint)Handle);
				// Create reference to this state within the Lua runtime
				SetSelf();
			}
		}

		~LuaState() {
			// Dispose on finalization if the user forgets
			Dispose();
		}

		public virtual void Dispose() {
			System.GC.SuppressFinalize(this);
			// If we haven't already been disposed
			if (L != IntPtr.Zero) {
				// Close the Lua state
				unsafe {
					Lua.Functions.lua_close(L);
				}
				// Free the GC handle so the state doesn't live forever
				Handle.Free();
				// Finally mark it as disposed by setting the state to null
				L = IntPtr.Zero;
			}
		}

		//=================//
		// Debug Functions //
		//=================//

		// Holds the wrapper delegates so the function pointers remain valid while in use
		private readonly object?[] hooks = new object[Enum.GetValues<LuaHookEvent>().Length];

		/// <summary>
		/// Sets a debugging hook for the Lua runtime. Setting a null hook will remove the current one.
		/// </summary>
		/// <param name="hook">The hook function to use, or null</param>
		/// <param name="evt">The event to set the hook for</param>
		/// <param name="count">The instruction count parameter for <see cref="LuaHookEvent.Count"/>, otherwise ignored</param>
		public void SetHook(LuaHook? hook, LuaHookEvent evt, int count = 0) {
			int mask = 1 << (int)evt;
			if (hook != null) {
				// Create a wrapper to manually marshal the values and save it in the array for its respective event
				var wrapper = (IntPtr pState, IntPtr pDebug) => {
					try {
						hook(Get(pState).RootState, MemoryUtil.ReadUnmanaged<LuaDebug>(pDebug));
					} catch (Exception ex) {
						CaptureManagedException(ex);
						Error("C# exception: " + ex.Message);
					}
				};
				hooks[(int)evt] = wrapper;
				// Get the corresponding function pointer and use that to set the Lua hook
				IntPtr pfn = Marshal.GetFunctionPointerForDelegate(wrapper);
				unsafe { Lua.Functions.lua_sethook(L, pfn, mask, count); }
			} else {
				// Clear references for the hook
				unsafe { Lua.Functions.lua_sethook(L, IntPtr.Zero, mask, 0); }
				hooks[(int)evt] = null;
			}
		}

	}

}
