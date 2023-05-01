using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Tesseract.Core.Native {

	/// <summary>
	/// <para>A memory stack manages a buffer of native memory that is still accessible from managed code. This is
	/// not "true" stack allocated memory like via <c>stackalloc</c>, </para>
	/// <para>
	/// Allocating from a memory stack is more efficient than allocating memory via the <see cref="Marshal"/> class,
	/// but the allocated memory follows the stack-allocation pattern (ie. it is "freed" when the stack frame is exited).
	/// To simplify stack frame management the memory stack implements the <see cref="IDisposable"/> pattern, so a frame
	/// can be created when the stack is acquired via <see cref="Push"/>, and with a <c>using</c> statement the frame
	/// is destroyed when the stack goes out of scope, eg:
	/// </para>
	/// <para><code>using MemoryStack sp = MemoryStack.Push()</code></para>
	/// <para>Note: The memory stack grows downwards in memory, starting at the top of stack memory.</para>
	/// </summary>
	public class MemoryStack : IDisposable {

		/// <summary>
		/// The default size of a memory stack.
		/// </summary>
		public const int DefaultSize = 65536;

		private static readonly ThreadLocal<MemoryStack> localStack = new(() => new MemoryStack());

		/// <summary>
		/// The thread-local ("current") memory stack.
		/// </summary>
		public static MemoryStack Current => localStack.Value!;

		/// <summary>
		/// Gets the current memory stack and pushes a new stack frame.
		/// </summary>
		/// <returns>Current memory stack</returns>
		public static MemoryStack Push() => Current.PushFrame();

		// The underlying memory
		private readonly Memory<byte> memory;
		// The handle to the memory
		private readonly MemoryHandle memoryHandle;
		// The pointer to the base of stack memory
		private readonly IntPtr pBase;
		// The current offset into the stack
		private int offset;
		private readonly Stack<int> frameStack = new();

		public MemoryStack(int capacity) {
			memory = new byte[capacity];
			memoryHandle = memory.Pin();
			unsafe { pBase = (IntPtr)memoryHandle.Pointer; }
			offset = memory.Length;
		}

		public MemoryStack() {
			memory = new byte[DefaultSize];
			memoryHandle = memory.Pin();
			unsafe { pBase = (IntPtr)memoryHandle.Pointer; }
			offset = memory.Length;
		}

		~MemoryStack() {
			memoryHandle.Dispose();
		}

		/// <summary>
		/// Pointer to the base of stack memory.
		/// </summary>
		public IntPtr Base => pBase;

		/// <summary>
		/// The 'stack pointer' indicating where the top of the stack is relative to the base.
		/// </summary>
		public int Pointer {
			set {
				if (value < 0 || value > memory.Length) throw new ArgumentOutOfRangeException(nameof(value));
				offset = value;
			}
			get => offset;
		}

		/// <summary>
		/// Pushes the current stack frame.
		/// </summary>
		/// <returns>This object</returns>
		public MemoryStack PushFrame() {
			frameStack.Push(offset);
			return this;
		}

		/// <summary>
		/// Pops the current stack frame.
		/// </summary>
		/// <returns>This object</returns>
		public MemoryStack PopFrame() {
			offset = frameStack.Pop();
			return this;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Dispose pattern is used to simplify stack frame management")]
		public void Dispose() {
			PopFrame();
		}

		/// <summary>
		/// Allocates one or more values of an unmanaged data type. The data is uninitialized and will contain
		/// whatever data was previously in stack memory at the same location.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="size">The number of values to allocate</param>
		/// <param name="alignment">The required alignment of allocated memory. If 0, aligned to the native integer size</param>
		/// <returns>Pointer to allocated memory</returns>
		public UnmanagedPointer<T> Alloc<T>(int size = 1, int alignment = 0) where T : unmanaged {
			if (size <= 0) return default;
			unsafe {
				if (alignment <= 0) alignment = sizeof(nint);
				int bytesize = size * sizeof(T);
				int newoffset = offset - bytesize;
				IntPtr pNew = pBase + newoffset;
				nint alignpad = (nint)pNew % alignment;
				newoffset -= (int)alignpad;
				if (newoffset < 0) throw new ArgumentOutOfRangeException(nameof(size), "Not enough memory to allocate structure");
				offset = newoffset;
				return new UnmanagedPointer<T>(pBase + offset, size);
			}
		}

		/// <summary>
		/// Allocates and initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="values">The list of values to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(params T[] values) where T : unmanaged {
			unsafe {
				UnmanagedPointer<T> ptr = Alloc<T>(values.Length);
				MemoryUtil.Copy(ptr, values, (ulong)(values.Length * sizeof(T)));
				return ptr;
			}
		}

		/// <summary>
		/// Allocates and initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="values">The list of values to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(in ReadOnlySpan<T> values) where T : unmanaged {
			unsafe {
				UnmanagedPointer<T> ptr = Alloc<T>(values.Length);
				MemoryUtil.Copy(ptr, values, (ulong)(values.Length * sizeof(T)));
				return ptr;
			}
		}

		/// <summary>
		/// Allocates and initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="values">The collection of values to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(IReadOnlyCollection<T> values) where T : unmanaged {
			unsafe {
				UnmanagedPointer<T> ptr = Alloc<T>(values.Count);
				int i = 0;
				foreach (T value in values) ptr[i++] = value;
				return ptr;
			}
		}

		/// <summary>
		/// Allocates and initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="values">The enumeration of values to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(IEnumerable<T> values) where T : unmanaged {
			unsafe {
				UnmanagedPointer<T> ptr = Alloc<T>(values.Count());
				int i = 0;
				foreach (T value in values) ptr[i++] = value;
				return ptr;
			}
		}

		/// <summary>
		/// Allocates an initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="handle">The handle to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(IPrimitiveHandle<T> handle) where T : unmanaged {
			UnmanagedPointer<T> ptr = Alloc<T>(1);
			ptr.Value = handle.PrimitiveHandle;
			return ptr;
		}

		/// <summary>
		/// Allocates an initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="handles">The handles to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(params IPrimitiveHandle<T>[] handles) where T : unmanaged {
			UnmanagedPointer<T> ptr = Alloc<T>(handles.Length);
			for (int i = 0; i < handles.Length; i++) ptr[i] = handles[i].PrimitiveHandle;
			return ptr;
		}

		/// <summary>
		/// Allocates an initializes values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="handles">The handles to initialize memory with</param>
		/// <returns>Pointer to values in memory</returns>
		public UnmanagedPointer<T> Values<T>(IReadOnlyCollection<IPrimitiveHandle<T>> handles) where T : unmanaged {
			UnmanagedPointer<T> ptr = Alloc<T>(handles.Count);
			int i = 0;
			foreach (IPrimitiveHandle<T> handle in handles) ptr[i++] = handle.PrimitiveHandle;
			return ptr;
		}

		/// <summary>
		/// Allocates a span of values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="size">The number of values to allocate</param>
		/// <returns>Span of values in stack memory</returns>
		public Span<T> AllocSpan<T>(int size) where T : unmanaged {
			if (size <= 0) return Span<T>.Empty;
			int bytesize = size * Marshal.SizeOf<T>();
			int newoffset = offset - bytesize;
			if (newoffset < 0) throw new ArgumentOutOfRangeException(nameof(size), "Not enough memory to allocate structure");
			offset = newoffset;
			unsafe {
				return new Span<T>((void*)(pBase + offset), size);
			}
		}

		//public ref T AllocRef<T>() where T : unmanaged => ref AllocSpan<T>(1)[0];

		/// <summary>
		/// Allocates a pointer to an ASCII string.
		/// </summary>
		/// <param name="text">String text</param>
		/// <returns>String pointer</returns>
		public UnmanagedPointer<byte> ASCII(string text) => text != null ? Values(Encoding.ASCII.GetBytes(text + '\0')) : default;

		/// <summary>
		/// Allocates an array of ASCII strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> ASCIIArray(params string[] text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = ASCII(text[i]);
			return array;
		}

		/// <summary>
		/// Allocates an array of ASCII strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> ASCIIArray(in ReadOnlySpan<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = ASCII(text[i]);
			return array;
		}

		/// <summary>
		/// Allocates an array of ASCII strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> ASCIIArray(IReadOnlyCollection<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Count);
			int i = 0;
			foreach (string txt in text) array[i++] = ASCII(txt);
			return array;
		}

		/// <summary>
		/// Allocates a pointer to a UTF-8 string.
		/// </summary>
		/// <param name="text">String text</param>
		/// <returns>String pointer</returns>
		public UnmanagedPointer<byte> UTF8(string text) => text != null ? Values(Encoding.UTF8.GetBytes(text + '\0')) : default;

		/// <summary>
		/// Allocates an array of UTF-8 strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> UTF8Array(params string[] text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = UTF8(text[i]);
			return array;
		}

		/// <summary>
		/// Allocates an array of UTF-8 strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> UTF8Array(in ReadOnlySpan<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = UTF8(text[i]);
			return array;
		}

		/// <summary>
		/// Allocates an array of UTF-8 strings.
		/// </summary>
		/// <param name="text">Strings to allocate</param>
		/// <returns>Pointer to array of string pointers</returns>
		public UnmanagedPointer<IntPtr> UTF8Array(IReadOnlyCollection<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Count);
			int i = 0;
			foreach (string txt in text) array[i++] = UTF8(txt);
			return array;
		}

	}

}
