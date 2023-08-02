using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LuaJIT {
	
	/// <summary>
	/// Enumeration of Lua status codes.
	/// </summary>
	public enum LuaStatus : int {
		/// <summary>
		/// Status is okay.
		/// </summary>
		Ok = 0,
		/// <summary>
		/// Coroutine thread requested to yield.
		/// </summary>
		Yield = 1,
		/// <summary>
		/// A Lua runtime error occurred.
		/// </summary>
		ErrRun = 2,
		/// <summary>
		/// An syntax error occurred loading the Lua chunk.
		/// </summary>
		ErrSyntax = 3,
		/// <summary>
		/// An error occurred calling the memory allocator.
		/// </summary>
		ErrMem = 4,
		/// <summary>
		/// An error occurred invoking the error handler in a protected call.
		/// </summary>
		ErrErr = 5
	}

	/// <summary>
	/// Enumeration of Lua types.
	/// </summary>
	public enum LuaType : int {
		/// <summary>
		/// No value is present.
		/// </summary>
		None = -1,
		/// <summary>
		/// Nil/null value.
		/// </summary>
		Nil = 0,
		/// <summary>
		/// Boolean value.
		/// </summary>
		Boolean = 1,
		/// <summary>
		/// Light userdata value (equivalent to an <see cref="IntPtr"/>).
		/// </summary>
		LightUserData = 2,
		/// <summary>
		/// Number value.
		/// </summary>
		Number = 3,
		/// <summary>
		/// String value.
		/// </summary>
		String = 4,
		/// <summary>
		/// Table value.
		/// </summary>
		Table = 5,
		/// <summary>
		/// Function value.
		/// </summary>
		Function = 6,
		/// <summary>
		/// Arbitrary userdata value.
		/// </summary>
		UserData = 7,
		/// <summary>
		/// Thread value.
		/// </summary>
		Thread = 8
	}

	/// <summary>
	/// Enumeration of Lua garbage collector operations.
	/// </summary>
	public enum LuaGC : int {
		/// <summary>
		/// Stops the garbage collector.
		/// </summary>
		Stop = 0,
		/// <summary>
		/// Restarts the garbage collector.
		/// </summary>
		Restart = 1,
		/// <summary>
		/// Performs a full garbage collection cycle.
		/// </summary>
		Collect = 2,
		/// <summary>
		/// Returns the amount of memory (in KiB) in use by Lua.
		/// </summary>
		Count = 3,
		/// <summary>
		/// Returns the remainder of the bytes (from <see cref="Count"/>) used by Lua.
		/// </summary>
		CountB = 4,
		/// <summary>
		/// Performs an incremental step of garbage collection, with the step "size"
		/// controlled by the data parameter. The return value will be 1 when a full
		/// garbage collection cycle is done.
		/// </summary>
		Step = 5,
		/// <summary>
		/// Sets the 'pause' value of the garbage collector to the data parameter.
		/// </summary>
		SetPause = 6,
		/// <summary>
		/// Sets the 'step multiplier' value of the garbage collector to the data parameter.
		/// </summary>
		SetStepMul = 7,
		/// <summary>
		/// Returns 1 if the garbage collector is running.
		/// </summary>
		IsRunning = 9
	}

	public enum LuaHook : int {
		Call = 0,
		Ret = 1,
		Line = 2,
		Count = 3,
		TailRet = 4
	}

}
