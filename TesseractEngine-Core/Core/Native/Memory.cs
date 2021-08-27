using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A primitive handle is an object whose native representation is that
	/// of a handle of a simple primitive type. This interface is the non-generic form
	/// which only provides a method to write the handle to memory.
	/// <seealso cref="IPrimitiveHandle{T}"/>
	/// </summary>
	public interface IPrimitiveHandle {

		public void WritePrimitiveHandle(IntPtr ptr) { }
	
	}

	/// <summary>
	/// A primitive handle is an object whose native representation is that
	/// of a handle of a simple primitive type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IPrimitiveHandle<T> : IPrimitiveHandle where T : unmanaged {

		/// <summary>
		/// The primitive handle value.
		/// </summary>
		public T PrimitiveHandle { get; }

		void IPrimitiveHandle.WritePrimitiveHandle(IntPtr ptr) => MemoryUtil.WriteUnmanaged(ptr, PrimitiveHandle);

	}

	/*
	public interface IBlittable {

		public void WriteToMemory(IntPtr ptr);

		public void ReadFromMemory(IntPtr ptr);

	}
	*/

	/// <summary>
	/// Provides utilities for interacting with native memory.
	/// </summary>
	public static class MemoryUtil {

		// Unmanaged read/write

		/// <summary>
		/// Reads an unmanaged value from a pointer. This avoids the contrivances of the
		/// <see cref="Marshal"/> class but will only work for unmanaged types. This is also
		/// unsafe as it does raw pointer operations.
		/// </summary>
		/// <typeparam name="T">Unmanaged type to read</typeparam>
		/// <param name="ptr">Pointer to unmanaged type</param>
		/// <returns>Value from pointer</returns>
		public static unsafe T ReadUnmanaged<T>(IntPtr ptr) where T : unmanaged { unsafe { return *(T*)ptr; } }

		/// <summary>
		/// Writes an unmanaged value to a pointer. This avoids the contrivances of the
		/// <see cref="Marshal"/> class but will only work for unmanaged types. This is also
		/// unsafe as it does raw pointer operations.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ptr"></param>
		/// <param name="value"></param>
		public static unsafe void WriteUnmanaged<T>(IntPtr ptr, T value) where T : unmanaged { unsafe { *(T*)ptr = value; } }

		/* TODO: Wait for more consideration on the need and practicality of a reader/writer system for complex types
		public interface IReaderWriter {

			public object ReadRaw(IntPtr ptr);

			public void WriteRaw(object value, IntPtr ptr);

		}

		public struct ReaderWriter<T> : IReaderWriter where T : struct {

			public Func<IntPtr, T> Reader { get; init; }

			public Action<T, IntPtr> Writer { get; init; }

			public object ReadRaw(IntPtr ptr) => Reader(ptr);

			public void WriteRaw(object value, IntPtr ptr) => Writer((T)value, ptr);
		}

		// Cache of reader/writer objects for different types
		private static readonly Dictionary<Type, object> rwcache = new();

		/// <summary>
		/// Gets a <see cref="ReaderWriter{T}"/> instance for the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static ReaderWriter<T> GetReaderWriter<T>() where T : struct {
			Type type = typeof(T);
			lock (rwcache) {
				if (rwcache.TryGetValue(type, out object rw)) return (ReaderWriter<T>)rw;
				else {
					ReaderWriter<T> readwrite;
					if (type.IsEnum) { // For enum types
						// Get the underlying type
						Type enumType = type.GetEnumUnderlyingType();
						// Get the raw reader/writer for the type
						IReaderWriter etrw = (IReaderWriter)rwcache[enumType];
						// Initialize the new reader/writer using the raw version and casting
						readwrite = new() {
							Reader = ptr => (T)etrw.ReadRaw(ptr),
							Writer = (val, ptr) => etrw.WriteRaw(val, ptr)
						};
					} else if (type.IsSubclassOf(typeof(IPrimitiveHandle))) { // For IPrimitiveHandle objects
						// Cannot read, but can use the writer method of the class
						readwrite = new() {
							Reader = ptr => throw new InvalidOperationException("Cannot read primitive handle value"),
							Writer = (val, ptr) => ((IPrimitiveHandle)val).WritePrimitiveHandle(ptr)
						};
					} else if (type.IsSubclassOf(typeof(IBlittable))) { // For IBlittable objects
						// The reader/writer will call the methods on the object
						readwrite = new() {
							Reader = ptr => {
								T t = new();
								((IBlittable)t).ReadFromMemory(ptr);
								return t;
							},
							Writer = (val, ptr) => ((IBlittable)val).WriteToMemory(ptr)
						};
					} else {
						readwrite = new() {
							Reader = ptr => Marshal.PtrToStructure<T>(ptr),
							Writer = (val, ptr) => Marshal.StructureToPtr(val, ptr, false)
						};
					}
					rwcache[type] = readwrite;
					return readwrite;
				}
			}
		}
		*/

		// Span <-> Pointer Copying

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

		// String allocation

		/// <summary>
		/// Creates a new managed pointer to an array of encoded string bytes.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="encoding">The string encoding to use</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Managed pointer to string bytes</returns>
		public static ManagedPointer<byte> AllocStringBytes(string str, Encoding encoding, bool nullTerminate = true) {
			if (nullTerminate && str.Last() != '\0') str += "\0";
			ManagedPointer<byte> ptr = new(encoding.GetByteCount(str));
			unsafe {
				encoding.GetBytes(str, new Span<byte>((void*)ptr.Ptr, ptr.ArraySize));
			}
			return ptr;
		}

		/// <summary>
		/// Creates a new managed pointer to an array of ASCII encoded string bytes.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Managed pointer to string bytes</returns>
		public static ManagedPointer<byte> AllocASCII(string str, bool nullTerminate = true) => AllocStringBytes(str, Encoding.ASCII, nullTerminate);

		/// <summary>
		/// Creates a new managed pointer to an array of UTF-8 encoded string bytes.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Managed pointer to string bytes</returns>
		public static ManagedPointer<byte> AllocUTF8(string str, bool nullTerminate = true) => AllocStringBytes(str, Encoding.UTF8, nullTerminate);

		// Searches

		/// <summary>
		/// Finds the first value in memory that matches the specified value.
		/// </summary>
		/// <typeparam name="T">The type to search for</typeparam>
		/// <param name="span">Span to search within</param>
		/// <param name="value">The value to search for</param>
		/// <returns>Index of the first specified value, or -1 if it was not found</returns>
		public static int FindFirst<T>(in ReadOnlySpan<T> span, T value) where T : IEquatable<T> {
			for (int i = 0; i < span.Length; i++) if (span[i].Equals(value)) return i;
			return -1;
		}

		/// <summary>
		/// Finds the first value in native memory that matches the specified value. Note that unbounded
		/// searches are unsafe as they can escape into unmanaged memory.
		/// </summary>
		/// <typeparam name="T">The type to search for</typeparam>
		/// <param name="ptr">Pointer to memory to search</param>
		/// <param name="value">The value to search for</param>
		/// <param name="maxLength">The maximum number of elements to search, or search forever if -1</param>
		/// <returns>Index of the first specified value, or -1 if it was not found</returns>
		public static unsafe int FindFirst<T>(IntPtr ptr, T value, int maxLength = -1) where T : unmanaged, IEquatable<T> {
			unsafe {
				T* tptr = (T*)ptr;
				if (maxLength < 0) {
					int i = 0;
					while (!tptr[i].Equals(value)) i++;
					return i;
				} else {
					for(int i = 0; i < maxLength; i++) if (tptr[i].Equals(value)) return i;
					return -1;
				}
			}
		}

		// ASCII Encoding

		/// <summary>
		/// Gets an ASCII encoded string from a pointer. If a null pointer is supplied null is returned.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>The ASCII string at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetASCII(IntPtr ptr, int length = -1, bool nullTerminated = true) {
			if (ptr == IntPtr.Zero) return null;
			if (length < 0) length = FindFirst<byte>(ptr, 0);
			else if (nullTerminated) length = FindFirst<byte>(ptr, 0, length);
			unsafe {
				return Encoding.ASCII.GetString(new ReadOnlySpan<byte>((void*)ptr, length));
			}
		}

		/// <summary>
		/// Gets a null-terminated ASCII encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>String at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetASCII(IConstPointer<byte> ptr, int length = -1, bool nullTerminated = true) => GetASCII(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

		/// <summary>
		/// Gets an ASCII encoded string from a span of bytes.
		/// </summary>
		/// <param name="span">Span to get string from</param>
		/// <param name="nullTerminated">If the string is null terminated.</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetASCII(ReadOnlySpan<byte> span, bool nullTerminated = true) {
			int length = span.Length;
			if (nullTerminated) length = FindFirst<byte>(span, 0);
			return Encoding.ASCII.GetString(span[..length]);
		}

		/// <summary>
		/// Writes an ASCII encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static void PutASCII(string str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			unsafe {
				Encoding.ASCII.GetBytes(str, new Span<byte>((void*)ptr, maxLength));
			}
		}

		/// <summary>
		/// Writes an ASCII encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutASCII(string str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutASCII(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes an ASCII encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutASCII(string str, Span<byte> memory, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			Encoding.ASCII.GetBytes(str, memory);
		}

		// UTF-16 Encoding

		/// <summary>
		/// Gets a UTF-16 encoded string from a pointer. If a null pointer is supplied null is returned.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>The UTF-16 string at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF16(IntPtr ptr, int length = -1, bool nullTerminated = true) {
			if (ptr == IntPtr.Zero) return null;
			if (length < 0) length = FindFirst<byte>(ptr, 0);
			else if (nullTerminated) length = FindFirst<ushort>(ptr, 0, length);
			unsafe {
				return Encoding.Unicode.GetString(new ReadOnlySpan<byte>((void*)ptr, length * sizeof(ushort)));
			}
		}

		/// <summary>
		/// Gets a null-terminated UTF-16 encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>String at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF16(IConstPointer<byte> ptr, int length = -1, bool nullTerminated = true) => GetUTF16(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

		/// <summary>
		/// Gets a UTF-16 encoded string from a span of bytes.
		/// </summary>
		/// <param name="span">Span to get string from</param>
		/// <param name="nullTerminated">If the string is null terminated.</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF16(ReadOnlySpan<byte> span, bool nullTerminated = true) {
			int length = span.Length;
			if (nullTerminated) {
				unsafe {
					fixed(byte* pSpan = span) {
						length = FindFirst<ushort>((IntPtr)pSpan, 0) * sizeof(ushort);
					}
				}
			}
			return Encoding.Unicode.GetString(span[..length]);
		}

		/// <summary>
		/// Writes a UTF-16 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static void PutUTF16(string str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			unsafe {
				Encoding.Unicode.GetBytes(str, new Span<byte>((void*)ptr, maxLength));
			}
		}

		/// <summary>
		/// Writes a UTF-16 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutUTF16(string str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutUTF16(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes a UTF-16 encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutUTF16(string str, Span<byte> memory, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			Encoding.Unicode.GetBytes(str, memory);
		}

		// UTF-8 Encoding

		/// <summary>
		/// Gets a UTF-8 encoded string from a pointer. If a null pointer is supplied null is returned.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>The UTF-8 string at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF8(IntPtr ptr, int length = -1, bool nullTerminated = true) {
			if (ptr == IntPtr.Zero) return null;
			if (length < 0) length = FindFirst<byte>(ptr, 0);
			else if (nullTerminated) length = FindFirst<byte>(ptr, 0, length);
			unsafe {
				return Encoding.UTF8.GetString(new ReadOnlySpan<byte>((void*)ptr, length));
			}
		}

		/// <summary>
		/// Gets a null-terminated UTF-8 encoded string from a pointer.
		/// </summary>
		/// <param name="ptr">Pointer to get string from</param>
		/// <param name="length">The length of the string. If -1 the null terminated length is always used.</param>
		/// <param name="nullTerminated">If the string is null terminated. If true the supplied length is the maximum length, else it is a fixed string length.</param>
		/// <returns>String at pointer</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF8(IConstPointer<byte> ptr, int length = -1, bool nullTerminated = true) => GetUTF8(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

		/// <summary>
		/// Gets an UTF-8 encoded string from a span of bytes.
		/// </summary>
		/// <param name="span">Span to get string from</param>
		/// <param name="nullTerminated">If the string is null terminated.</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF8(ReadOnlySpan<byte> span, bool nullTerminated = true) {
			int length = span.Length;
			if (nullTerminated) length = FindFirst<byte>(span, 0);
			return Encoding.UTF8.GetString(span[..length]);
		}

		/// <summary>
		/// Writes a UTF-8 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static void PutUTF8(string str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			unsafe {
				Encoding.UTF8.GetBytes(str, new Span<byte>((void*)ptr, maxLength));
			}
		}

		/// <summary>
		/// Writes a UTF-8 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutUTF8(string str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutUTF8(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes a UTF-8 encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PutUTF8(string str, Span<byte> memory, bool nullTerminate = true) {
			if (nullTerminate) str += '\0';
			Encoding.UTF8.GetBytes(str, memory);
		}

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
		/// <returns>Pointer to allocated memory</returns>
		public UnmanagedPointer<T> Alloc<T>(int size = 1) where T : unmanaged {
			unsafe {
				int bytesize = size * sizeof(T);
				int newoffset = offset - bytesize;
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
				MemoryUtil.Copy(ptr, values, values.Length * sizeof(T));
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
			foreach(IPrimitiveHandle<T> handle in handles) ptr[i++] = handle.PrimitiveHandle;
			return ptr;
		}

		/// <summary>
		/// Allocates a span of values in stack memory.
		/// </summary>
		/// <typeparam name="T">The data type to allocate</typeparam>
		/// <param name="size">The number of values to allocate</param>
		/// <returns>Span of values in stack memory</returns>
		public Span<T> AllocSpan<T>(int size) where T : unmanaged {
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
		public UnmanagedPointer<byte> ASCII(string text) => Values(Encoding.ASCII.GetBytes(text + '\0'));

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
		public UnmanagedPointer<byte> UTF8(string text) => Values(Encoding.UTF8.GetBytes(text + '\0'));

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
			foreach(string txt in text) array[i++] = UTF8(txt);
			return array;
		}

	}
}
