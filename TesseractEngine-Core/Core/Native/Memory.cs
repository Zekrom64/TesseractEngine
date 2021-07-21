using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	/// <summary>
	/// Provides utilities for interacting with native memory.
	/// </summary>
	public static class MemoryUtil {

		/// <summary>
		/// Copies raw bytes from one pointer to another.
		/// </summary>
		/// <typeparam name="T">Pointer element type</typeparam>
		/// <param name="dst">Pointer to copy to</param>
		/// <param name="src">Pointer to copy from</param>
		/// <param name="length">Length in bytes to copy</param>
		public static void Copy<T>(IPointer<T> dst, IConstPointer<T> src, long length) {
			unsafe {
				Buffer.MemoryCopy((void*)src.Ptr, (void*)dst.Ptr, length, length);
			}
		}

		/// <summary>
		/// Copies raw bytes from a span to a pointer.
		/// </summary>
		/// <typeparam name="T">Pointer element type</typeparam>
		/// <param name="dst">Pointer to copy to</param>
		/// <param name="src">Span to copy from</param>
		/// <param name="length">Length in bytes to copy</param>
		public static void Copy<T>(IPointer<T> dst, ReadOnlySpan<T> src, long length) where T : unmanaged {
			unsafe {
				fixed(T* pSrc = src) {
					Buffer.MemoryCopy(pSrc, (void*)dst.Ptr, length, length);
				}
			}
		}

		/// <summary>
		/// Copies raw bytes from a pointer to a span.
		/// </summary>
		/// <typeparam name="T">Pointer element type</typeparam>
		/// <param name="dst">Span to copy to</param>
		/// <param name="src">Pointer to copy from</param>
		/// <param name="length">Length in bytes to copy</param>
		public static void Copy<T>(Span<T> dst, IConstPointer<T> src, long length) where T : unmanaged {
			unsafe {
				fixed(T* pDst = dst) {
					Buffer.MemoryCopy((void*)src.Ptr, pDst, length, length);
				}
			}
		}

		/// <summary>
		/// Gets a null-terminated ASCII encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <returns>String at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringASCII(IntPtr ptr) => Marshal.PtrToStringAnsi(ptr);

		/// <summary>
		/// Gets a null-terminated ASCII encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <returns>String at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringASCII(IConstPointer<byte> ptr) => GetStringASCII(ptr.Ptr);

		/// <summary>
		/// Gets an ASCII encoded string from a span of bytes.
		/// </summary>
		/// <param name="memory">Span to get string from</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringASCII(ReadOnlySpan<byte> memory) => Encoding.ASCII.GetString(memory);

		/// <summary>
		/// Gets a null-terminated ASCII encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <returns>String at pointer</returns>
		public static string GetStringASCII(IntPtr ptr, int length) {
			unsafe {
				return GetStringASCII(new Span<byte>((void*)ptr, length));
			}
		}

		/// <summary>
		/// Writes an ASCII encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="length">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static void PutStringASCII(string str, IntPtr ptr, int length, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			unsafe {
				Encoding.ASCII.GetBytes(str, new Span<byte>((void*)ptr, length));
			}
		}

		/// <summary>
		/// Writes an ASCII encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="length">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringASCII(string str, IPointer<byte> ptr, int length, bool nullTerminate = true) => PutStringASCII(str, ptr.Ptr, length, nullTerminate);

		/// <summary>
		/// Writes an ASCII encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringASCII(string str, Span<byte> memory, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			Encoding.ASCII.GetBytes(str, memory);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF16(IntPtr ptr) => Marshal.PtrToStringUni(ptr);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF16(IConstPointer<byte> ptr) => GetStringUTF16(ptr.Ptr);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF16(ReadOnlySpan<byte> memory) => Encoding.Unicode.GetString(memory);

		public static string GetStringUTF16(IntPtr ptr, int length) {
			unsafe {
				return GetStringUTF16(new Span<byte>((void*)ptr, length));
			}
		}

		public static void PutStringUTF16(string str, IntPtr ptr, int length) {
			unsafe {
				Encoding.Unicode.GetBytes(str, new Span<byte>((void*)ptr, length));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringUTF16(string str, IPointer<byte> ptr, int length) => PutStringUTF16(str, ptr.Ptr, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringUTF16(string str, Span<byte> memory) => Encoding.Unicode.GetBytes(str, memory);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF8(IntPtr ptr) => Marshal.PtrToStringUTF8(ptr);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF8(IConstPointer<byte> ptr) => GetStringUTF8(ptr.Ptr);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetStringUTF8(ReadOnlySpan<byte> memory) => Encoding.UTF8.GetString(memory);

		public static string GetStringUTF8(IntPtr ptr, int length) {
			unsafe {
				return GetStringUTF8(new Span<byte>((void*)ptr, length));
			}
		}

		public static void PutStringUTF8(string str, IntPtr ptr, int length) {
			unsafe {
				Encoding.UTF8.GetBytes(str, new Span<byte>((void*)ptr, length));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringUTF8(string str, IPointer<byte> ptr, int length) => PutStringUTF8(str, ptr.Ptr, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutStringUTF8(string str, Span<byte> memory) => Encoding.UTF8.GetBytes(str, memory);

	}

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
		public static MemoryStack Current => localStack.Value;

		/// <summary>
		/// Gets the current memory stack and pushes a new stack frame.
		/// </summary>
		/// <returns>Current memory stack</returns>
		public static MemoryStack Push() => Current.PushFrame();

		// The underlying memory
		private readonly byte[] memory;
		// The GCHandle to the memory
		private readonly GCHandle memoryHandle;
		// The pointer to the base of stack memory
		private readonly IntPtr pBase;
		// The current offset into the stack
		private int offset;
		private readonly Stack<int> frameStack = new();

		public MemoryStack(int capacity) {
			memory = new byte[capacity];
			memoryHandle = GCHandle.Alloc(memoryHandle);
			pBase = memoryHandle.AddrOfPinnedObject();
			offset = memory.Length;
		}

		public MemoryStack() {
			memory = new byte[DefaultSize];
			memoryHandle = GCHandle.Alloc(memoryHandle);
			pBase = memoryHandle.AddrOfPinnedObject();
			offset = memory.Length;
		}

		~MemoryStack() {
			memoryHandle.Free();
		}

		/// <summary>
		/// Pointer to the base of stack memory.
		/// </summary>
		public IntPtr Base => pBase;

		/// <summary>
		/// 
		/// </summary>
		public int Pointer {
			set {
				if (value < 0 || value > memory.Length) throw new ArgumentOutOfRangeException(nameof(value));
				offset = value;
			}
			get => offset;
		}

		public MemoryStack PushFrame() {
			frameStack.Push(offset);
			return this;
		}

		public MemoryStack PopFrame() {
			offset = frameStack.Pop();
			return this;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Dispose pattern is used to simplify stack frame management")]
		public void Dispose() {
			PopFrame();
		}

		public UnmanagedPointer<T> Alloc<T>(int size) where T : unmanaged {
			unsafe {
				int bytesize = size * sizeof(T);
				int newoffset = offset - bytesize;
				if (newoffset < 0) throw new ArgumentOutOfRangeException(nameof(size), "Not enough memory to allocate structure");
				offset = newoffset;
				return new UnmanagedPointer<T>(pBase + offset);
			}
		}

		public UnmanagedPointer<T> Alloc<T>(params T[] values) where T : unmanaged {
			unsafe {
				UnmanagedPointer<T> ptr = Alloc<T>(values.Length);
				MemoryUtil.Copy(ptr, values, values.Length * sizeof(T));
				return ptr;
			}
		}

		public Span<T> AllocSpan<T>(int size) where T : unmanaged {
			int bytesize = size * Marshal.SizeOf<T>();
			int newoffset = offset - bytesize;
			if (newoffset < 0) throw new ArgumentOutOfRangeException(nameof(size), "Not enough memory to allocate structure");
			offset = newoffset;
			unsafe {
				return new Span<T>((void*)(pBase + offset), size);
			}
		}

		public ref T AllocRef<T>() where T : unmanaged => ref AllocSpan<T>(1)[0];

		public UnmanagedPointer<byte> ASCII(string text) => Alloc(Encoding.ASCII.GetBytes(text + '\0'));

		public UnmanagedPointer<IntPtr> ASCIIArray(params string[] text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = ASCII(text[i]);
			return array;
		}

		public UnmanagedPointer<IntPtr> ASCIIArray(in ReadOnlySpan<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = ASCII(text[i]);
			return array;
		}

		public UnmanagedPointer<byte> UTF8(string text) => Alloc(Encoding.UTF8.GetBytes(text + '\0'));

		public UnmanagedPointer<IntPtr> UTF8Array(params string[] text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = UTF8(text[i]);
			return array;
		}

		public UnmanagedPointer<IntPtr> UTF8Array(in ReadOnlySpan<string> text) {
			UnmanagedPointer<IntPtr> array = Alloc<IntPtr>(text.Length);
			for (int i = 0; i < text.Length; i++) array[i] = UTF8(text[i]);
			return array;
		}

	}
}
