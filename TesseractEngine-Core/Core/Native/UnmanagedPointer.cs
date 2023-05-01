using System;
using System.Buffers;

namespace Tesseract.Core.Native {

	/// <summary>
	/// An unmanaged pointer references memory that is externally managed.
	/// </summary>
	/// <typeparam name="T">Type reference</typeparam>
	public readonly struct UnmanagedPointer<T> : IPointer<T> where T : unmanaged {

		public IntPtr Ptr { get; }

		public int ArraySize { get; }

		public T Value {
			get => MemoryUtil.ReadUnmanaged<T>(Ptr);
			set => MemoryUtil.WriteUnmanaged(Ptr, value);
		}

		public Span<T> Span {
			get {
				int count = ArraySize;
				if (count < 0) count = 1;
				unsafe {
					return new Span<T>((void*)Ptr, count);
				}
			}
		}

		/// <summary>
		/// Creates a new unmanaged pointer.
		/// </summary>
		/// <param name="ptr">The pointer value</param>
		/// <param name="count">The number of elements pointed to, or -1 if this is not known</param>
		public UnmanagedPointer(IntPtr ptr, int count = -1) {
			Ptr = ptr;
			ArraySize = count;
		}

		/// <summary>
		/// Creates a new unmanaged pointer to memory represented by a memory handle.
		/// </summary>
		/// <param name="memory">The memory handle</param>
		/// <param name="count">The number of elements in the memory given by the memory handle</param>
		public UnmanagedPointer(MemoryHandle memory, int count) {
			unsafe { Ptr = (IntPtr)memory.Pointer; }
			ArraySize = count;
		}

		public static implicit operator IntPtr(UnmanagedPointer<T> uptr) => uptr.Ptr;

		public static explicit operator UnmanagedPointer<T>(IntPtr ptr) => new(ptr);

		/// <summary>
		/// Checks if this pointer is valid (ie. not a null pointer).
		/// </summary>
		/// <param name="uptr">The pointer to check</param>
		public static implicit operator bool(UnmanagedPointer<T> uptr) => uptr.Ptr != IntPtr.Zero;

		public T this[int index] {
			get {
				if (ArraySize != -1 && (index < 0 || index >= ArraySize)) throw new IndexOutOfRangeException();
				unsafe { return MemoryUtil.ReadUnmanaged<T>(Ptr + sizeof(T) * index); }
			}
			set {
				if (ArraySize != -1 && (index < 0 || index >= ArraySize)) throw new IndexOutOfRangeException();
				unsafe { MemoryUtil.WriteUnmanaged(Ptr + sizeof(T) * index, value); }
			}
		}

		/// <summary>
		/// <para>Offsets this pointer by the given number of elements.</para>
		/// <para>
		/// The array size of the resulting pointer is modified based on the offset and current
		/// array size; If the offset is outside the bounds of the array (&lt;0 or &gt;=Length)
		/// the new array size will be -1 (undefined). Otherwise the size will shrink to fit
		/// the remaining known elements of the array.
		/// </para>
		/// </summary>
		/// <param name="ptr">The pointer to offset</param>
		/// <param name="offset">The offset to apply in elements</param>
		/// <returns>The offset pointer.</returns>
		public static UnmanagedPointer<T> operator +(UnmanagedPointer<T> ptr, int offset) {
			unsafe {
				int sz = ptr.ArraySize;
				if (sz != -1) {
					if (offset < 0) sz = -1;
					else sz -= offset;
					if (sz < 1) sz = -1;
				}

				IntPtr iptr = ptr.Ptr + offset * sizeof(T);

				return new UnmanagedPointer<T>(iptr, sz);
			}
		}
	}

}
