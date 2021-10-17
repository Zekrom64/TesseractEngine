using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Util;

namespace Tesseract.Core.Native {

	/// <summary>
	/// A constant pointer stores a memory reference to a read-only object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IConstPointer<T> : IReadOnlyIndexer<int, T> {

		/// <summary>
		/// The underlying memory pointer.
		/// </summary>
		public IntPtr Ptr { get; }

		/// <summary>
		/// If the underlying memory pointer is null.
		/// </summary>
		public bool IsNull => Ptr == IntPtr.Zero;

		/// <summary>
		/// The value stored at the memory reference.
		/// </summary>
		public T Value { get; }
		
		/// <summary>
		/// The size of the array pointed to by this value. If only a single value this is 1. If the size
		/// is unknown this is -1.
		/// </summary>
		public int ArraySize { get; }
		
		/// <summary>
		/// Gets the pointer represented as a read-only span.
		/// </summary>
		public ReadOnlySpan<T> ReadOnlySpan { get; }

	}

	/// <summary>
	/// A pointer stores a memory reference to a modifiable object.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public interface IPointer<T> : IConstPointer<T>, IIndexer<int, T> {

		/// <summary>
		/// The value stored at the memory reference.
		/// </summary>
		public new T Value { get; set; }

		/// <summary>
		/// Gets the pointer represented as a span.
		/// </summary>
		public Span<T> Span { get; }

		ReadOnlySpan<T> IConstPointer<T>.ReadOnlySpan => Span;

	}
	
	/// <summary>
	/// A releaser handles releasing memory held by a managed pointer.
	/// </summary>
	/// <param name="ptr">Pointer to release</param>
	public delegate void Releaser(IntPtr ptr);

	/// <summary>
	/// A managed pointer references memory that is manually managed by the developer.
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public struct ManagedPointer<T> : IDisposable, IPointer<T> where T : struct {

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

		private readonly Releaser release;

		/// <summary>
		/// Creates a new managed pointer from an existing raw pointer and an optional releaser.
		/// </summary>
		/// <param name="ptr">Raw pointer</param>
		/// <param name="release">Releaser for pointer, or null</param>
		/// <param name="count">The number of array elements</param>
		/// <param name="readOnly">If the pointer should be read-only</param>
		public ManagedPointer(IntPtr ptr, Releaser release = null, int count = 1, bool readOnly = false) {
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
		public ManagedPointer(T value, bool readOnly) {
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
		public ManagedPointer(in ReadOnlySpan<T> values, bool readOnly) {
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
			foreach(T val in values) Marshal.StructureToPtr(val, Ptr + sz * i, false); 
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
		public ManagedPointer(Memory<T> memory, bool readOnly) {
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

	/// <summary>
	/// An unmanaged pointer references memory that is externally managed.
	/// </summary>
	/// <typeparam name="T">Type reference</typeparam>
	public struct UnmanagedPointer<T> : IPointer<T> where T : unmanaged {

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

	}

	/// <summary>
	/// <para>
	/// An object pointer stores a reference to a managed object.
	/// </para>
	/// <para>
	/// Creating a new object pointer from an object will pin the object in memory until explicitly
	/// disposed of. An object pointer can retrieve the pinned value from the raw pointer as well.
	/// Note that the pointer is merely a reference to the object, not a pointer to the object's raw memory.
	/// Additionaly, because this is a reference to an object the value referenced by the pointer cannot be
	/// directly changed (though the object may still be modifiable).
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type referenced</typeparam>
	public struct ObjectPointer<T> : IDisposable, IConstPointer<T> where T : class {

		public IntPtr Ptr { get; }

		public int ArraySize => 1;

		public T Value => (T)Handle.Target;

		public GCHandle Handle => GCHandle.FromIntPtr(Ptr);

		public ReadOnlySpan<T> ReadOnlySpan => throw new InvalidOperationException("Cannot get an object pointer as a span, underlying memory is opaque");

		public T this[int key] {
			get {
				if (key == 0) return Value;
				else throw new IndexOutOfRangeException();
			}
		}

		/// <summary>
		/// Creates an object pointer from a raw pointer.
		/// </summary>
		/// <param name="ptr">Raw pointer value</param>
		public ObjectPointer(IntPtr ptr) {
			Ptr = ptr;
		}

		/// <summary>
		/// Creates a new object pointer referencing the given value.
		/// </summary>
		/// <param name="value">Value to reference</param>
		public ObjectPointer(T value) {
			Ptr = GCHandle.ToIntPtr(GCHandle.Alloc(value));
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Handle.Free();
		}

		public static implicit operator IntPtr(ObjectPointer<T> optr) => optr.Ptr;

		public static implicit operator bool(ObjectPointer<T> optr) => optr.Ptr != IntPtr.Zero;

	}

}
