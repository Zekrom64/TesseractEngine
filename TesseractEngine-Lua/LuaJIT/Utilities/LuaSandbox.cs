using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.LuaJIT.Utilities {

	/// <summary>
	/// <para>
	/// Manages a sandboxed Lua environment that imposes memory limits and allows
	/// asynchronous loading and execution of Lua code. The synchronous methods are
	/// still available and work as normal, but the sandbox additional has its own
	/// internal thread used for asynchronous execution. This does not make the class
	/// thread-safe, and calls to the sandbox still need to be externally synchronized.
	/// </para>
	/// 
	/// <para>
	/// Note that reentrant functions (managed functions added to the Lua runtime
	/// which themselves call Lua functions) should take special care handling the
	/// <see cref="LuaSandboxException"/> class; it should be rethrown instead of
	/// caught as it signals that the current function call should exit <i>immediately</i>,
	/// although finalization code may still be called to clean up the call frame.
	/// </para>
	/// 
	/// <para>
	/// Also note that asynchronous termination of calls into the Lua runtime depends
	/// on the <see cref="LuaHookEvent.Count"/> debug hook. If this hook is overwritten
	/// the user should call the <see cref="CheckTerminating"/> function to test
	/// if the curent call is returning and to throw <see cref="LuaSandboxException"/> if so.
	/// </para>
	/// </summary>
	public class LuaSandbox : LuaState, IDisposable {

		/// <summary>
		/// The maximum amount of memory the sandbox is allowed to use.
		/// </summary>
		public nuint MaxMemory { get; set; }

		/// <summary>
		/// The current amount of memory the sandbox is using.
		/// </summary>
		public nuint CurrentMemory { get; private set; }

		// Custom allocator which enforces memory limits

		private static IntPtr Alloc(LuaState lua, IntPtr ptr, nuint osize, nuint nsize) {
			LuaSandbox sandbox = (LuaSandbox)lua;

			nuint maxMemory = sandbox.MaxMemory;
			// Allow any allocations before the sandbox is fully initialized
			if (maxMemory == default) maxMemory = nuint.MaxValue;

			if (nsize == 0) {
				Marshal.FreeHGlobal(ptr);
				return IntPtr.Zero;
			} else {
				nuint avail = maxMemory - sandbox.CurrentMemory;
				if (osize == 0) {
					if (nsize > avail) return IntPtr.Zero;
					sandbox.CurrentMemory += nsize;
					return Marshal.AllocHGlobal((nint)nsize);
				} else {
					nint diff = (nint)(nsize - osize);
					if (diff > (nint)avail) return IntPtr.Zero;
					sandbox.CurrentMemory += nsize - osize;
					return Marshal.ReAllocHGlobal(ptr, (nint)nsize);
				}
			}
		}

		private const int StateWaiting = 0;

		private const int StateRunning = 1;

		private const int StateExiting = -1;

		private bool alive = true;
		// Loop variable to keep the sandbox thread alive
		private int asyncState = StateWaiting;
		// The sandbox thread for async execution
		private readonly Thread thread;

		private Func<LuaStatus>? asyncOperation;

		private TaskCompletionSource<LuaStatus>? asyncTaskCompletion;

		private CancellationToken asyncCancellation = CancellationToken.None;

		private readonly SemaphoreSlim signalSemaphore = new(1, 1);

		private LuaSandboxException? terminatingException;

		public LuaSandbox(uint maxMem = 1 << 20) : base(Alloc) {
			MaxMemory = maxMem;
			thread = new Thread(RunSandbox) { IsBackground = true };

			// Open some safe packages
			OpenBase();
			OpenMath();
			OpenString();
			OpenTable();

			// Clean up some built-in functions that are unsafe
			PushNil();
			SetGlobal("dofile"u8);
			PushNil();
			SetGlobal("loadfile"u8);

			// Hack (x)pcall to rethrow errors when terminating a call

			// Hook the runtime to call us every 1000 instructions
			// This allows us to interrupt execution if it takes too long
			SetHook(CountHook, LuaHookEvent.Count, 1000);
		}

		~LuaSandbox() {
			Dispose();
		}

		//===========================//
		// Overridden Base Functions //
		//===========================//

		public override void Dispose() {
			System.GC.SuppressFinalize(this);
			try {
				// Make us not alive and make sure the thread is exiting
				if (alive) {
					alive = false;
					asyncState = StateExiting;
					// Signal the sandbox thread to run and exit, waiting a bit for it to finish
					signalSemaphore.Release();
					thread.Join(100);
					// Dispose of the semaphore
					signalSemaphore.Dispose();
				}
			} finally {
				// Make sure the underlying state gets properly disposed
				base.Dispose();
			}
		}

		protected override void CaptureManagedException(Exception e) {
			// Only signal termination for a sandbox exception, and pass all others like normal
			if (e is LuaSandboxException sandboxException) {
				if (Interlocked.CompareExchange(ref asyncState, StateExiting, StateRunning) == StateRunning) {
					terminatingException = sandboxException;
				}
			}
		}

		public override LuaStatus ProtectedCall(int nargs = 0, int nresults = -1, int errfunc = 0) {
			LuaStatus status = base.ProtectedCall(nargs, nresults, errfunc);
			// Rethrow any terminating exception that was caught during a Lua call
			if (terminatingException != null) throw new LuaSandboxException("Terminating exception propagated through Lua call", terminatingException);
			return status;
		}

		//===================//
		// Sandbox Execution //
		//===================//

		private void RunSandbox() {
			while(alive) {
				// Wait for the next async task
				signalSemaphore.Wait();
				// If none is supplied we must exit
				if (asyncTaskCompletion == null) break;

				// Perform the specified protected call
				try {
					if (asyncOperation != null) {
						var status = asyncOperation.Invoke();
						// Cancel if exiting, otherwise pass the result status
						if (asyncState == StateExiting) asyncTaskCompletion.SetCanceled();
						else asyncTaskCompletion.SetResult(status);
					} else asyncTaskCompletion.SetCanceled();
				} catch (LuaSandboxException) {
					// Cancel if a sandbox exception propagates upwards
					asyncTaskCompletion.SetCanceled();
				} catch (Exception e) {
					// Pass any propagated exception
					asyncTaskCompletion.SetException(e);
				}

				// Cleanup after the async call
				asyncTaskCompletion = null;
				asyncState = StateWaiting;
				terminatingException = null;
			}
		}

		private void CountHook(LuaState state, LuaDebug info) => CheckTerminating();

		/// <summary>
		/// Checks if the sandbox is terminating the current call, generating an exception if so.
		/// </summary>
		/// <exception cref="LuaSandboxException">If the current call is terminating</exception>
		public void CheckTerminating() {
			// If the current task was requested to be cancelled, force us into the exiting state
			if (asyncCancellation.IsCancellationRequested) asyncState = StateExiting;
			// If in the exiting state, throw the requisite exception
			if (asyncState == StateExiting) throw new LuaSandboxException();
		}

		/// <summary>
		/// Terminates the current call into the Lua runtime. Note that unlike other functions
		/// which are not thread-safe, this function can be called from any thread.
		/// </summary>
		public void TerminateCall() {
			Interlocked.CompareExchange(ref asyncState, StateExiting, StateRunning);
		}

		//==================//
		// Async Operations //
		//==================//

		/// <summary>
		/// Invokes the given function in the sandbox's thread, generating a task that completes when
		/// the operation does or when it is cancelled.
		/// </summary>
		/// <param name="func">The function to invoke asynchronously</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the function</returns>
		/// <exception cref="InvalidOperationException">If there is already a Lua call being performed</exception>
		protected Task<LuaStatus> InvokeAsync(Func<LuaStatus> func, CancellationToken? ct) {
			// Transition to the running state if possible
			if (Interlocked.CompareExchange(ref asyncState, StateRunning, StateWaiting) != StateWaiting) throw new InvalidOperationException("Cannot perform async Lua call while one is currently in progress");
			// Load the async call operands
			asyncOperation = func;
			asyncTaskCompletion = new TaskCompletionSource<LuaStatus>();
			asyncCancellation = ct ?? CancellationToken.None;
			var task = asyncTaskCompletion.Task;
			// Signal the sandbox thread to start
			signalSemaphore.Release();
			return task;
		}

		/// <summary>
		/// Performs an asynchronous call similar to <see cref="LuaBase.Call(int, int)"/>.
		/// </summary>
		/// <param name="nargs">The number of arguments to the function</param>
		/// <param name="nresults">The number of result values</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua function call</returns>
		public Task<LuaStatus> CallAsync(int nargs = 0, int nresults = Lua.MultRet, CancellationToken? ct = null) => InvokeAsync(() => ProtectedCall(nargs, nresults), ct);

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.Load(LuaReader, ReadOnlySpan{byte})"/>.
		/// </summary>
		/// <param name="reader">The reader for Lua code</param>
		/// <param name="chunkName">The name of the chunk being loaded</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadAsync(LuaReader reader, ReadOnlySpan<byte> chunkName, CancellationToken? ct = null) {
			byte[] chunkName2 = chunkName.ToArray();
			return InvokeAsync(() => Load(reader, chunkName2), ct);
		}

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.Load(LuaReader, ReadOnlySpan{char})"/>.
		/// </summary>
		/// <param name="reader">The reader for Lua code</param>
		/// <param name="chunkName">The name of the chunk being loaded</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadAsync(LuaReader reader, ReadOnlySpan<char> chunkName, CancellationToken? ct = null) {
			char[] chunkName2 = chunkName.ToArray();
			return InvokeAsync(() => Load(reader, chunkName2), ct);
		}

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.Load(LuaReader, ReadOnlySpan{char})"/>.
		/// </summary>
		/// <param name="reader">The reader for Lua code</param>
		/// <param name="chunkName">The name of the chunk being loaded</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadAsync(LuaReader reader, string chunkName, CancellationToken? ct = null) => InvokeAsync(() => Load(reader, chunkName), ct);

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.LoadString(ReadOnlySpan{byte})"/>.
		/// </summary>
		/// <param name="str">The string of Lua code to load</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadStringAsync(ReadOnlySpan<byte> str, CancellationToken? ct = null) {
			byte[] str2 = str.ToArray();
			return InvokeAsync(() => LoadString(str2), ct);
		}

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.LoadString(ReadOnlySpan{byte})"/>.
		/// </summary>
		/// <param name="str">The string of Lua code to load</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadStringAsync(Memory<byte> str, CancellationToken? ct = null) => InvokeAsync(() => LoadString(str.Span), ct);

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.LoadString(ReadOnlySpan{char})"/>.
		/// </summary>
		/// <param name="str">The string of Lua code to load</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadStringAsync(ReadOnlySpan<char> str, CancellationToken? ct = null) {
			char[] str2 = str.ToArray();
			return InvokeAsync(() => LoadString(str2), ct);
		}

		/// <summary>
		/// Performs an asynchronous load similar to <see cref="LuaBase.LoadString(ReadOnlySpan{char})"/>.
		/// </summary>
		/// <param name="str">The string of Lua code to load</param>
		/// <param name="ct">An optional cancellation token for the operation</param>
		/// <returns>Task for the Lua load</returns>
		public Task<LuaStatus> LoadStringAsync(string str, CancellationToken? ct = null) => InvokeAsync(() => LoadString(str), ct);

	}

	/// <summary>
	/// Specialization of <see cref="LuaException"/> which, when thrown, will terminate the current
	/// call in a sandboxed Lua environment, bypassing any use of <c>pcall</c> or <c>xpcall</c>.
	/// Otherwise behaves the same as <see cref="LuaException"/>.
	/// </summary>
	public class LuaSandboxException : LuaException {

		/// <inheritdoc cref="LuaException(string?)"/>
		public LuaSandboxException(string? message = null) : base(message) { }

		/// <inheritdoc cref="LuaException(string?, Exception?)"/>
		public LuaSandboxException(string? message, Exception? innerException) : base(message, innerException) { }

	}

}
