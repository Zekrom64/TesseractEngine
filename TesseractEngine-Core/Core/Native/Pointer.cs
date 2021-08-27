using System;
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

		public T Value {
			get => Marshal.PtrToStructure<T>(Ptr);
			set => Marshal.StructureToPtr(value, Ptr, true);
		}

		private readonly Releaser release;

		/// <summary>
		/// Creates a new managed pointer from an existing raw pointer and an optional releaser.
		/// </summary>
		/// <param name="ptr">Raw pointer</param>
		/// <param name="release">Releaser for pointer, or null</param>
		/// <param name="count">The number of array elements</param>
		public ManagedPointer(IntPtr ptr, Releaser release = null, int count = 1) {
			Ptr = ptr;
			this.release = release;
			ArraySize = count;
		}

		/// <summary>
		/// Allocates a pointer to a value and marshals the given value to the new memory.
		/// </summary>
		/// <param name="value">Value to store in memory</param>
		public ManagedPointer(T value) {
			Ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
			Marshal.StructureToPtr(value, Ptr, false);
			release = ptr => {
				Marshal.DestroyStructure<T>(ptr);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = 1;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given default value to the new memory.
		/// </summary>
		/// <param name="count">Number of array elements</param>
		/// <param name="defVal">The default value in the array</param>
		public ManagedPointer(int count, T defVal = default) {
			int sz = Marshal.SizeOf<T>();
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(defVal, Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
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
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given array to the new memory.
		/// </summary>
		/// <param name="values">The array element values</param>
		public ManagedPointer(in ReadOnlySpan<T> values) {
			int sz = Marshal.SizeOf<T>();
			int count = values.Length;
			Ptr = Marshal.AllocHGlobal(sz * count);
			for (int i = 0; i < count; i++) Marshal.StructureToPtr(values[i], Ptr + sz * i, false);
			release = ptr => {
				for (int i = 0; i < count; i++) Marshal.DestroyStructure<T>(ptr + sz * i);
				Marshal.FreeHGlobal(ptr);
			};
			ArraySize = count;
		}

		/// <summary>
		/// Allocates a pointer to an array of values and marshals the given collection to the new memory.
		/// </summary>
		/// <param name="values">The array element values</param>
		public ManagedPointer(in IReadOnlyCollection<T> values) {
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
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			release?.Invoke(Ptr);
		}

		public static implicit operator IntPtr(ManagedPointer<T> mptr) => mptr.Ptr;

		public T this[int index] {
			get {
				if (index < 0 || index >= ArraySize) throw new IndexOutOfRangeException();
				return Marshal.PtrToStructure<T>(Ptr + Marshal.SizeOf<T>() * index);
			}
			set {
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

		public UnmanagedPointer(IntPtr ptr, int count = -1) {
			Ptr = ptr;
			ArraySize = count;
		}

		public static implicit operator IntPtr(UnmanagedPointer<T> uptr) => uptr.Ptr;

		public static explicit operator UnmanagedPointer<T>(IntPtr ptr) => new(ptr);

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

	}

}
