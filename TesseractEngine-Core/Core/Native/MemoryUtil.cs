using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Text;

namespace Tesseract.Core.Native {

	/// <summary>
	/// Provides utilities for interacting with native memory.
	/// </summary>
	public static class MemoryUtil {

		// Pointer/value casting

		/// <summary>
		/// Recasts a pointer to one type as a pointer to another type. Both of the types
		/// must be unmanaged to prevent recasting of types that require marshalling.
		/// </summary>
		/// <typeparam name="T1">First pointer type</typeparam>
		/// <typeparam name="T2">Second pointer type</typeparam>
		/// <param name="ptr">Pointer to case</param>
		/// <returns>Recast version of pointer</returns>
		public static IConstPointer<T2> RecastAs<T1, T2>(IConstPointer<T1> ptr) where T1 : unmanaged where T2 : unmanaged {
			int count = ptr.ArraySize;
			if (count > 0) {
				unsafe {
					count = (count * sizeof(T1)) / sizeof(T2);
				}
			}
			return new UnmanagedPointer<T2>(ptr.Ptr, count);
		}

		/// <summary>
		/// Recasts a pointer to one type as a pointer to another type. Both of the types
		/// must be unmanaged to prevent recasting of types that require marshalling.
		/// </summary>
		/// <typeparam name="T1">First pointer type</typeparam>
		/// <typeparam name="T2">Second pointer type</typeparam>
		/// <param name="ptr">Pointer to case</param>
		/// <returns>Recast version of pointer</returns>
		public static IPointer<T2> RecastAs<T1, T2>(IPointer<T1> ptr) where T1 : unmanaged where T2 : unmanaged {
			int count = ptr.ArraySize;
			if (count > 0) {
				unsafe {
					count = (count * sizeof(T1)) / sizeof(T2);
				}
			}
			return new UnmanagedPointer<T2>(ptr.Ptr, count);
		}

		/// <summary>
		/// Performs a 'bitwise' cast of one type to another, reinterpreting the underlying bytes.
		/// </summary>
		/// <typeparam name="T1">Type to cast from</typeparam>
		/// <typeparam name="T2">Type to cast to</typeparam>
		/// <param name="x">The value to cast</param>
		/// <returns>The reinterpreted value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T2 BitwiseCast<T1, T2>(T1 x) where T1 : unmanaged where T2 : unmanaged => MemoryMarshal.Cast<T1, T2>(stackalloc T1[] { x })[0];

		// Unmanaged type check

		/// <summary>
		/// Checks if the provided type is unmanaged.
		/// </summary>
		/// <typeparam name="T">Type to check</typeparam>
		/// <returns>If the type is unmanaged</returns>
		public static bool IsUnmanaged<T>() {
			Type t = typeof(T);
			if (t.IsGenericType) return false; // Generic types are managed
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return false; // Anything with references is managed
			if (!t.IsValueType) return false; // Reference types are managed
			return true;
		}

		/// <summary>
		/// Asserts that the provided type is unmanaged.
		/// </summary>
		/// <typeparam name="T">Type to check</typeparam>
		public static void AssertUnmanaged<T>() {
			if (!IsUnmanaged<T>()) throw new ArgumentException("The specified type must be unmanaged", nameof(T));
		}

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

		// Memory fill

		/// <summary>
		/// Fills the memory at the given address with binary zeroes ("zero'ing" it). The number of elements
		/// to zero must either implicitly be supplied with the pointer or explicitly given.
		/// </summary>
		/// <typeparam name="T">The pointer element type</typeparam>
		/// <param name="ptr">The pointer to elements to zero</param>
		/// <param name="length">The number of elements to zero, or -1 to use the pointer array size</param>
		public static void ZeroMemory<T>(IPointer<T> ptr, int length = -1) where T : unmanaged {
			if (length == -1) length = ptr.ArraySize;
			if (length == -1) throw new ArgumentException("Cannot zero pointer with no explicit or implicit length", nameof(ptr));
			unsafe {
				T* pElem = (T*)ptr.Ptr;
				for (int i = 0; i < length; i++) pElem[i] = default;
			}
		}

		// Span <-> Pointer Copying

		/// <summary>
		/// Copies raw bytes from one pointer to another.
		/// </summary>
		/// <typeparam name="T">Pointer element type</typeparam>
		/// <param name="dst">Pointer to copy to</param>
		/// <param name="src">Pointer to copy from</param>
		/// <param name="length">Length in bytes to copy</param>
		public static void Copy<T>(IPointer<T> dst, IConstPointer<T> src, ulong length) {
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
		public static void Copy<T>(IPointer<T> dst, ReadOnlySpan<T> src, ulong length) where T : unmanaged {
			unsafe {
				fixed (T* pSrc = src) {
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
		public static void Copy<T>(Span<T> dst, IConstPointer<T> src, ulong length) where T : unmanaged {
			unsafe {
				fixed (T* pDst = dst) {
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
		public static ManagedPointer<byte> AllocStringBytes(ReadOnlySpan<char> str, Encoding encoding, bool nullTerminate = true) {
			int nBytes = encoding.GetByteCount(str);
			bool appendNull = nullTerminate && str[^1] != '\0';
			if (appendNull) nBytes++;
			ManagedPointer<byte> ptr = new(nBytes);
			encoding.GetBytes(str, ptr.Span);
			if (appendNull) ptr[nBytes - 1] = 0;
			return ptr;
		}

		/// <summary>
		/// Creates a new managed pointer to an array of ASCII encoded string bytes.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Managed pointer to string bytes</returns>
		public static ManagedPointer<byte> AllocASCII(ReadOnlySpan<char> str, bool nullTerminate = true) => AllocStringBytes(str, Encoding.ASCII, nullTerminate);

		/// <summary>
		/// Creates a new managed pointer to an array of UTF-8 encoded string bytes.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Managed pointer to string bytes</returns>
		public static ManagedPointer<byte> AllocUTF8(ReadOnlySpan<char> str, bool nullTerminate = true) => AllocStringBytes(str, Encoding.UTF8, nullTerminate);

		/// <summary>
		/// Creates a span of encoded string bytes, attempting to use a stack-allocated buffer.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="stackbuf">The stack-allocated buffer</param>
		/// <param name="encoding">The encoding the string should use</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Span of string bytes</returns>
		public static Span<byte> StackallocStringBytes(ReadOnlySpan<char> str, Span<byte> stackbuf, Encoding encoding, bool nullTerminate = true) {
			Span<byte> resultbuf = stackbuf;
			// Try to convert to bytes using the stack buffer
			int nBytes = encoding.GetBytes(str, resultbuf);
			// If the buffer is almost entirely filled, assume we have missed at least one character
			if (nBytes > stackbuf.Length-4) {
				// Get required byte count and transcribe encoding bytes
				bool appendNull = nullTerminate && str[^1] != '\0';
				nBytes = encoding.GetByteCount(str);
				if (appendNull) nBytes++;
				resultbuf = new byte[nBytes];
				if (appendNull) resultbuf[^1] = 0;
				encoding.GetBytes(str, resultbuf);
			} else {
				if (nullTerminate) resultbuf[nBytes++] = 0;
			}
			return resultbuf;
		}

		/// <summary>
		/// Creates a span of ASCII encoded string bytes, attempting to use a stack-allocated buffer.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="stackbuf">The stack-allocated buffer</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Span of string bytes</returns>
		public static Span<byte> StackallocASCII(ReadOnlySpan<char> str, Span<byte> stackbuf, bool nullTerminate = true) => StackallocStringBytes(str, stackbuf, Encoding.ASCII, nullTerminate);

		/// <summary>
		/// Creates a span of UTF-8 encoded string bytes, attempting to use a stack-allocated buffer.
		/// </summary>
		/// <param name="str">The string to encode</param>
		/// <param name="stackbuf">The stack-allocated buffer</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		/// <returns>Span of string bytes</returns>
		public static Span<byte> StackallocUTF8(ReadOnlySpan<char> str, Span<byte> stackbuf, bool nullTerminate = true) => StackallocStringBytes(str, stackbuf, Encoding.UTF8, nullTerminate);

		/// <summary>
		/// Creates a span of pointers to encoded string bytes, using a stack allocated buffer.
		/// </summary>
		/// <param name="strs">The collection of strings to encode</param>
		/// <param name="ptrs">The span to store the string pointers into</param>
		/// <param name="stack">The stack to allocate the string bytes from</param>
		/// <param name="encoding">The string encoding to use</param>
		/// <returns>The span of string pointers</returns>
		public static Span<IntPtr> StackallocStringPtrArray(IReadOnlyCollection<string> strs, Span<IntPtr> ptrs, MemoryStack stack, Encoding encoding) {
			Span<byte> buf = stackalloc byte[4096];

			int i = 0;
			foreach(string str in strs) {
				int nBytes = encoding.GetBytes(str, buf);
				if (nBytes > buf.Length - 4) {
					nBytes = encoding.GetByteCount(str) + 1;
					buf = new byte[nBytes];
					buf[^1] = 0;
					encoding.GetBytes(str, buf);
				} else {
					buf[nBytes++] = 0;
				}
				ptrs[i++] = stack.Values((ReadOnlySpan<byte>)buf[..nBytes]);
			}

			return ptrs;
		}

		/// <summary>
		/// Creates a span of pointers to ASCII encoded string bytes, using a stack allocated buffer.
		/// </summary>
		/// <param name="strs">The collection of strings to encode</param>
		/// <param name="ptrs">The span to store the string pointers into</param>
		/// <param name="stack">The stack to allocate the string bytes from</param>
		/// <returns>The span of string pointers</returns>
		public static Span<IntPtr> StackallocASCIIArray(IReadOnlyCollection<string> strs, Span<IntPtr> ptrs, MemoryStack stack) => StackallocStringPtrArray(strs, ptrs, stack, Encoding.ASCII);

		/// <summary>
		/// Creates a span of pointers to UTF-8 encoded string bytes, using a stack allocated buffer.
		/// </summary>
		/// <param name="strs">The collection of strings to encode</param>
		/// <param name="ptrs">The span to store the string pointers into</param>
		/// <param name="stack">The stack to allocate the string bytes from</param>
		/// <returns>The span of string pointers</returns>
		public static Span<IntPtr> StackallocUTF8Array(IReadOnlyCollection<string> strs, Span<IntPtr> ptrs, MemoryStack stack) => StackallocStringPtrArray(strs, ptrs, stack, Encoding.UTF8);

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
					for (int i = 0; i < maxLength; i++) if (tptr[i].Equals(value)) return i;
					return -1;
				}
			}
		}

		// Memory Operations

		/// <summary>
		/// Compares the contents of memory at two locations, checking if the underlying bytes are equal.
		/// </summary>
		/// <typeparam name="T">Type of values in memory</typeparam>
		/// <param name="left">First region of memory</param>
		/// <param name="right">Second region of memory</param>
		/// <returns>If the underlying memory of two locations is equal</returns>
		public static bool MemoryEqual<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right) where T : unmanaged, IEquatable<T> {
			int length = Math.Min(left.Length, right.Length);
			unsafe {
				fixed (T* pLeft = left) {
					fixed (T* pRight = right) {
						return MemoryEqual<T>(
							new UnmanagedPointer<T>((nint)pLeft, left.Length),
							new UnmanagedPointer<T>((nint)pRight, right.Length),
							length
						);
					}
				}
			}
		}

		/// <summary>
		/// Compares the contents of memory at two locations, checking if the underlying bytes are equal.
		/// </summary>
		/// <typeparam name="T">Type of values in memory</typeparam>
		/// <param name="left">First region of memory</param>
		/// <param name="right">Second region of memory</param>
		/// <param name="length">The number of items to compare</param>
		/// <returns>If the underlying memory of two locations is equal</returns>
		/// <exception cref="ArgumentException">If an illegal length is provided</exception>
		public static bool MemoryEqual<T>(IConstPointer<T> left, IConstPointer<T> right, int length) where T : unmanaged, IEquatable<T> {
			if (left.ArraySize != -1 && left.ArraySize < length) throw new ArgumentException("Requested length exceeds the known array size of a pointer", nameof(length));
			if (right.ArraySize != -1 && right.ArraySize < length) throw new ArgumentException("Requested length exceeds the known array size of a pointer", nameof(length));

			int lengthBytes = length * Marshal.SizeOf<T>();
			// Only bother advanced operations for >128 byte memory
			if (lengthBytes > 128) {
				// Verify 16-byte alignment
				IntPtr pLeft = left.Ptr, pRight = right.Ptr;
				if ((pLeft & 0xF) == 0 && (pRight & 0xF) == 0) {
					if (Sse2.IsSupported) {
						// Optimized SSE comparison
						unsafe {
							byte* pbLeft = (byte*)pLeft, pbRight = (byte*)pRight;
							Vector128<byte> a, b;
							while (lengthBytes >= 16) {
								a = Sse2.LoadAlignedVector128(pbLeft);
								b = Sse2.LoadAlignedVector128(pbRight);
								if (Sse2.MoveMask(Sse2.CompareEqual(a, b)) != 0xFF) return false;
								pbLeft += 16;
								pbRight += 16;
								lengthBytes -= 16;
							}

							while (lengthBytes-- > 0) {
								if (*pbLeft++ != *pbRight++) return false;
							}
						}
						return true;
					} else if (AdvSimd.IsSupported) {
						// Optimized AdvSIMD comparison
						unsafe {
							byte* pbLeft = (byte*)pLeft, pbRight = (byte*)pRight;
							Vector128<byte> a, b, eq;
							while (lengthBytes >= 16) {
								a = AdvSimd.LoadVector128(pbLeft);
								b = AdvSimd.LoadVector128(pbRight);
								eq = AdvSimd.CompareEqual(a, b);
								// We can only use *Across on AArch64, otherwise a more complex sequence is required
								if (AdvSimd.Arm64.IsSupported && AdvSimd.Arm64.MinAcross(eq.AsUInt32())[0] != 0xFFFFFFFFU) return false;
								else {
									// Bitwise and lower and upper 64 bits
									Vector64<uint> eq2 = AdvSimd.And(eq.GetUpper(), eq.GetLower()).AsUInt32();
									// Bitwise and 32-bit upper and lower halves of the result
									// Must have all bits set
									if ((eq2[1] & eq2[0]) != 0xFFFFFFFFU) return false;
								}
								pbLeft += 16;
								pbRight += 16;
								lengthBytes -= 16;
							}

							while (lengthBytes-- > 0) {
								if (*pbLeft++ != *pbRight++) return false;
							}
						}
						return true;
					}
				}
			}

			// Fallback comparison loop
			for (int i = 0; i < length; i++) {
				if (left[i].Equals(right[i])) return false;
			}

			return true;
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
		public static string? GetASCII(IntPtr ptr, int length = -1, bool nullTerminated = true) {
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
		public static string? GetASCII(IConstPointer<byte> ptr, int length = -1, bool nullTerminated = true) => GetASCII(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

		/// <summary>
		/// Gets an ASCII encoded string from a span of bytes.
		/// </summary>
		/// <param name="span">Span to get string from</param>
		/// <param name="nullTerminated">If the string is null terminated.</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetASCII(ReadOnlySpan<byte> span, bool nullTerminated = true) {
			int length = -1;
			if (nullTerminated) length = FindFirst<byte>(span, 0);
			if (length == -1) length = span.Length;
			return Encoding.ASCII.GetString(span[..length]);
		}

		/// <summary>
		/// Writes an ASCII encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static int PutASCII(ReadOnlySpan<char> str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			unsafe {
				return PutASCII(str, new Span<byte>((void*)ptr, maxLength), nullTerminate);
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
		public static int PutASCII(ReadOnlySpan<char> str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutASCII(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes an ASCII encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PutASCII(ReadOnlySpan<char> str, Span<byte> memory, bool nullTerminate = true) {
			int bytes = Encoding.ASCII.GetBytes(str, memory);
			if (nullTerminate && str[^1] != '\0') {
				if (bytes >= memory.Length) throw new ArgumentException("Not enough space to append null terminator", nameof(memory));
				memory[bytes++] = 0;
			}
			return bytes;
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
		public static string? GetUTF16(IntPtr ptr, int length = -1, bool nullTerminated = true) {
			if (ptr == IntPtr.Zero) return null;
			if (length < 0) length = FindFirst<ushort>(ptr, 0);
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
		public static string? GetUTF16(IConstPointer<char> ptr, int length = -1, bool nullTerminated = true) => GetUTF16(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

		/// <summary>
		/// Gets a UTF-16 encoded string from a span of characters.
		/// </summary>
		/// <param name="span">Span to get string from</param>
		/// <param name="nullTerminated">If the string is null terminated.</param>
		/// <returns>String encoded in span</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetUTF16(ReadOnlySpan<char> span, bool nullTerminated = true) {
			int length = -1;
			if (nullTerminated) length = FindFirst(span, '\0');
			if (length < 0) length = span.Length;
			return new string(span[..length]);
		}

		/// <summary>
		/// Writes a UTF-16 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static int PutUTF16(ReadOnlySpan<char> str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			unsafe {
				return PutUTF16(str, new Span<byte>((void*)ptr, maxLength), nullTerminate);
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
		public static int PutUTF16(ReadOnlySpan<char> str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutUTF16(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes a UTF-16 encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PutUTF16(ReadOnlySpan<char> str, Span<byte> memory, bool nullTerminate = true) {
			int bytes = Encoding.Unicode.GetBytes(str, memory);
			if (nullTerminate && str[^1] != '\0') {
				if (bytes >= memory.Length - 1) throw new ArgumentException("Not enough space to append null terminator", nameof(memory));
				memory[bytes++] = 0;
				memory[bytes] = 0;
			}
			return bytes;
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
		public static string? GetUTF8(IntPtr ptr, int length = -1, bool nullTerminated = true) {
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
		public static string? GetUTF8(IConstPointer<byte> ptr, int length = -1, bool nullTerminated = true) => GetUTF8(ptr != null ? ptr.Ptr : IntPtr.Zero, length, nullTerminated);

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
			if (length == -1) length = span.Length;
			return Encoding.UTF8.GetString(span[..length]);
		}

		/// <summary>
		/// Writes a UTF-8 encoded string to a pointer.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="ptr">Pointer to write string at</param>
		/// <param name="maxLength">Maximum number of bytes to write</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		public static int PutUTF8(ReadOnlySpan<char> str, IntPtr ptr, int maxLength, bool nullTerminate = true) {
			unsafe {
				return PutUTF8(str, new Span<byte>((void*)ptr, maxLength), nullTerminate);
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
		public static int PutUTF8(ReadOnlySpan<char> str, IPointer<byte> ptr, int maxLength, bool nullTerminate = true) => PutUTF8(str, ptr.Ptr, maxLength, nullTerminate);

		/// <summary>
		/// Writes a UTF-8 encoded string to a span of bytes.
		/// </summary>
		/// <param name="str">String to write</param>
		/// <param name="memory">Span to write string to</param>
		/// <param name="nullTerminate">If the string should be null-terminated</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PutUTF8(ReadOnlySpan<char> str, Span<byte> memory, bool nullTerminate = true) {
			int bytes = Encoding.UTF8.GetBytes(str, memory);
			if (nullTerminate && str[^1] != '\0') {
				if (bytes >= memory.Length) throw new ArgumentException("Not enough space to append null terminator", nameof(memory));
				memory[bytes++] = 0;
			}
			return bytes;
		}

	}

}
