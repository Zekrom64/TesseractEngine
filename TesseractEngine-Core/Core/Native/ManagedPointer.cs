using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A managed pointer references memory whose lifetime is controlled by the managed environment.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public readonly struct ManagedPointer<T> : IDisposable, IPointer<T> where T : struct {

		public IntPtr Ptr { get; }

		public int ArraySize { get; }

		/// <summary>
		/// If the pointer is read-only. This is an extra layer of protection over
		/// just passing this as a <see cref="IConstPointer{T}"/>, where methods that
		/// attempt to write to memory will throw an exception.
		/// </summary>
		public bool IsReadOnly { get; }

		public T Value {
			get => Marshal.PtrToStructure<T>(Ptr);
			set {
				if (IsReadOnly) throw new InvalidOperationException("Cannot set value of read-only memory");
				Marshal.StructureToPtr(value, Ptr, true);
			}
		}

		public Span<T> Span {
			get {
				// Note: This does not respect the read-only flag but that is less bad for unmanaged types
				// No cheeky attempts to trash memory of non-unmanaged types
				if (MemoryUtil.IsUnmanaged<T>()) {
					unsafe {
						return new Span<T>((void*)Ptr, ArraySize);
					}
				} else throw new InvalidOperationException("Cannot wrap span to non-unmanaged type");
			}
		}

		private readonly Releaser? release;

		/// <summary>
		/// Creates a new managed pointer from an existing raw pointer and an optional releaser.
		/// </summary>
		/// <param name="ptr">Raw pointer</param>
		/// <param name="release">Releaser for pointer, or null</param>
		/// <param name="count">The number of array elements</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(IntPtr ptr, Releaser? release = null, int count = 1, bool readOnly = false) {
			Ptr = ptr;
			this.release = release;
			ArraySize = count;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Allocates a pointer to a value and marshals the given value to the new memory.
		/// </summary>
		/// <param name="value">Value to store in memory</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(T value, bool readOnly = false) {
			Ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
			Marshal.StructureToPtr(value, Ptr, false);
			release = ptr => {
				Marshal.DestroyStructure<T>(ptr);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = 1;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given default value to the new memory.
		/// </summary>
		/// <param name="count">Number of array elements</param>
		/// <param name="defVal">The default value in the array</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(int count, T defVal = default, bool readOnly = false) {
			int sz = Marshal.SizeOf<T>();
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(defVal, Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given array to the new memory.
		/// </summary>
		/// <param name="values">The array element values</param>
		public ManagedPointer(params T[] values) {
			int sz = Marshal.SizeOf<T>();
			int count = values.Length;
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(values[i], Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
			IsReadOnly = false;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given array to the new memory.
		/// </summary>
		/// <param name="readOnly">If the pointer should be read-only</param>
		/// <param name="values">The array element values</param>
		public ManagedPointer(bool readOnly, params T[] values) {
			int sz = Marshal.SizeOf<T>();
			int count = values.Length;
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(values[i], Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given array to the new memory.
		/// </summary>
		/// <param name="values">The array element values</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(in ReadOnlySpan<T> values, bool readOnly = false) {
			int sz = Marshal.SizeOf<T>();
			int count = values.Length;
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(values[i], Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given collection to the new memory.
		/// </summary>
		/// <param name="values">The array element values</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(IReadOnlyCollection<T> values, bool readOnly = false) {
			int sz = Marshal.SizeOf<T>();
			int count = values.Count;
			Ptr = Marshal.AllocHGlobal(sz * count);
			int i = 0;
			foreach (T val in values) Marshal.StructureToPtr(val, Ptr + sz * (i++), false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
			IsReadOnly = readOnly;
		}

		/// <summary>
		/// Pins the given memory and creates a new pointer to the pinned memory. Disposing of the
		/// managed pointer will unpin the memory.
		/// </summary>
		/// <param name="memory">Memory to pin</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(Memory<T> memory, bool readOnly = false) {
			MemoryUtil.AssertUnmanaged<T>(); // Don't allow cheeky things like converting memory w/ managed objects to pointers
			MemoryHandle handle = memory.Pin();
			unsafe { Ptr = (IntPtr)handle.Pointer; }
			release = ptr => { handle.Dispose(); };
			ArraySize = memory.Length;
			IsReadOnly = readOnly;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			release?.Invoke(Ptr);
		}

		public static implicit operator IntPtr(ManagedPointer<T> mptr) => mptr.Ptr;

		/// <summary>
		/// Checks if this pointer is valid (ie. not a null pointer).
		/// </summary>
		/// <param name="mptr">The pointer to check</param>
		public static implicit operator bool(ManagedPointer<T> mptr) => mptr.Ptr != IntPtr.Zero;

		public T this[int index] {
			get {
				if (index < 0 || index >= ArraySize) throw new IndexOutOfRangeException();
				return Marshal.PtrToStructure<T>(Ptr + Marshal.SizeOf<T>() * index);
			}
			set {
				if (IsReadOnly) throw new InvalidOperationException("Cannot set value of read-only memory");
				if (index < 0 || index >= ArraySize) throw new IndexOutOfRangeException();
				Marshal.StructureToPtr(value, Ptr + Marshal.SizeOf<T>() * index, true);
			}
		}

	}

}
